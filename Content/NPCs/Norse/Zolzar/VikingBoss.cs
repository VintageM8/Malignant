using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Malignant.Common.NPCs;
using Malignant.Common;

namespace Malignant.Content.NPCs.Norse.Zolzar
{
    [AutoloadBossHead]
    public class VikingBoss : ModNPC
    {
        private int bufferCount = 0;

        bool npcDashing = false;
        int spriteDirectionStore = 0;

        private const int State_Spawning = 0;
        private const int State_Moving = 1;
        private const int State_Attacking = 2;
        private const int State_Circling = 3; //This is used for the Adds the boss never enters this state.

        private const float AttackTargetingDistanceSQ = 1000 * 1000;
        private const int MovementTime = 360;
        private const int MaxDashes = 2;
        private const float Max_Speed = 7;
        private const int DashDelay = 20;

        private const float SoftMovementDistanceSQ = 700 * 700;

        private const float DashSpeed = 24;

        private const int SpawnDelay = 60;

        private float DashTime;
        private int TimesDashed;
        private Vector2 tempPos;
        private Vector2 IntSpeed;


        public float AI_State
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float Attack_State
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float AI_Timer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float AddNumber
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        public float RotationTimer
        {
            get => NPC.localAI[0];
            set => NPC.localAI[0] = value;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zolzar, Berserker Viking");
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 102;
            NPC.height = 165;
            NPC.damage = 45;
            NPC.defense = 18;
            NPC.lifeMax = 35000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.Item25;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.value = Item.buyPrice(gold: 3);
            NPC.npcSlots = 10f;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/ZozarP2");
            }
            
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale);
            NPC.defense = 28;
        }

        private void BossText(string text) // boss messages
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, Color.Green);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Green);
            }
        }
        public override bool PreAI()
        {
            Player target = Main.player[NPC.target];
            MalignantPlayer globaltarget = target.GetModPlayer<MalignantPlayer>();
            int Frames = 20;
            for (int i = 1; i < Frames; i++)
            {
                globaltarget.PreviousVelocity[i] = globaltarget.PreviousVelocity[i - 1];
            }
            globaltarget.PreviousVelocity[0] = target.velocity;
            for (int i = 0; i < Frames; i++)
            {
                IntSpeed = Vector2.Lerp(IntSpeed, globaltarget.PreviousVelocity[Frames - 1 - i], 0.14f);
            }

            return true;
        }
        public override void AI()
        {
            //TODO better targeting selection
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            //This handles the timing for calculating the spinning adds. The period for 1 rotation is exactly 300 ticks. Changing this makes the circle spin slower/faster but changing it is not recommended.
            //There are a a coupple things that rely on the rotation speed that would have to be changed which involves some math. So unless you know what you are doing don't touch this number.
            //Things that would require recalculation: Add movementspeed, Add locking distance, GenerateAddPositions.
            GenerateAddPositions();
            RotationTimer++;

            //Initial spawning. Boss starts with 5 Adds
            if (!MalignantSystem.downedViking && AI_State == State_Spawning)
            {
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
               
                if (AI_Timer <= 200)
                {
                    if (AI_Timer % 50 == 0)
                    {
                        SpawnAdd(NPC.Center);
                    }
                }
                else
                {
                    NPC.dontTakeDamage = false;
                    AI_State = State_Moving;
                    AI_Timer = 0;
                    NPC.netUpdate = true;
                }
                AI_Timer++;
                return;
            }

            // Handles Despawning
            if (NPC.target < 0 || NPC.target == 255 || player.dead || !player.active)
            {
                NPC.TargetClosest(false);
                NPC.velocity.Y = NPC.velocity.Y - 0.1f;
                Main.NewText("Bye");
                if (NPC.timeLeft > 20)
                {
                    NPC.timeLeft = 20;
                    return;
                }
            }
            
            //Basic non attacking cooldown phase.
            if (AI_State == State_Moving)
            {
                Moving();
                float distPlayerSQ = (NPC.Center - player.Center).LengthSquared();
                //Condition for when the boss can attack.
                if (AI_Timer >= MovementTime && NPC.HasValidTarget && distPlayerSQ < AttackTargetingDistanceSQ)
                {
                    AI_State = State_Attacking;
                    AI_Timer = 0;
                }
            }

            //Attack logic.
            if (AI_State == State_Attacking)
            {
                //Initialization for the Attacks.
                if (AI_Timer == 0)
                {
                    //prevents repeat BigDAshAttack
                    int nextAttackstate = Main.rand.Next(0, 9);
                    while (nextAttackstate == Attack_State)
                    {
                        nextAttackstate = Main.rand.Next(0, 9);
                    }
                    Attack_State = nextAttackstate;
                    if (AddNumber == 0) Attack_State = 0;
                    //Attack_State = 1;
                    NPC.netUpdate = true;
                }
                switch (Attack_State)
                {
                    case 0:
                        if (AI_Timer == 0)
                        {
                            if (AddNumber == 0) SpawnAdd(NPC.Center, 5);
                            else SpawnAdd(NPC.Center, 2);
                        }
                        BigDashAttack();
                        break;
                    case 1:
                    case 2:
                    case 3:
                        if (AI_Timer == 0) SpawnAdd(NPC.Center);
                        for (int i = 0; i < (int)AddNumber / 4f; i++)
                        {
                            SendAdd(2, 60 * i);
                        }
                        if (AI_Timer >= 60 * AddNumber)
                        {
                            AI_State = State_Moving;
                            AI_Timer = -1;
                        }
                        
                        //npc.velocity = Vector2.Zero;
                        //TrinitarianGlobalNPC globalnpc = npc.GetGlobalNPC<TrinitarianGlobalNPC>();
                        //if (AI_Timer == 0)
                        //{
                        //    SpawnAdd(npc.Center, 2);
                        //}
                        //if (AI_Timer % 180 == 0)
                        //{
                        //    Main.npc[globalnpc.Add[2]].life = 0;
                        //    Main.npc[globalnpc.Add[2]].checkDead();
                        //}

                        break;
                    case 4:
                    case 5:
                    case 6:
                        if (AI_Timer == 0) SpawnAdd(NPC.Center);
                        LightningStrike();
                        if (AI_Timer >= ((int)(AddNumber/2f) - 1) * SpawnDelay)
                        {
                            AI_State = State_Moving;
                            AI_Timer = -1;
                        }
                        break;
                    case 7:
                    case 8:
                        if (AI_Timer == 0) SpawnAdd(NPC.Center);
                        int dashamount = (int)(AddNumber / 4f) == 0 ? (int)(AddNumber / 4f) : 2;
                        SendAdd(dashamount, 0);
                        LightningStrike(20);
                        SendAdd(dashamount, 80);
                        if (AI_Timer >= 200)
                        {
                            AI_State = State_Moving;
                            AI_Timer = -1;
                        }
                        break;
                }
            }
            AI_Timer++;
            //npc.velocity = Vector2.Zero;
        }
        private void Moving()
        {
            Player player = Main.player[NPC.target];
            float distPlayerSQ = (NPC.Center - player.Center).LengthSquared();
            Vector2 npcAcc = player.Center - NPC.Center;
            float maxspeed = Max_Speed;
            //Assign weight of the NPC
            if (npcAcc != Vector2.Zero)
            {
                npcAcc.Normalize();
                npcAcc *= 1 / 5f;
            }
            //scale speed with distance. Closer = Slower.
            if (distPlayerSQ < SoftMovementDistanceSQ)
            {
                //Simple scaling function !!NOT linear since the quantitys are squared!!.
                maxspeed *= distPlayerSQ / SoftMovementDistanceSQ;
            }
            //apply acceleration
            if ((NPC.velocity + npcAcc).LengthSquared() <= maxspeed * maxspeed)
            {
                NPC.velocity += npcAcc;
            }
            //Enforce the maxspeed by setting the magnitude of the vector.
            else
            {
                NPC.velocity += npcAcc;
                if (NPC.velocity != Vector2.Zero)
                {
                    NPC.velocity.Normalize();
                }
                NPC.velocity *= maxspeed;
            }
        }
        //This spawns the Adds.
        private void SpawnAdd(Vector2 pos, int count = 1)
        {
            MalignantGlobalNPC globalnpc = NPC.GetGlobalNPC<MalignantGlobalNPC>();
            for (int i = 0; i < count; i++)
            {   
                globalnpc.Add[(int)AddNumber] = NPC.NewNPC(NPC.GetSource_FromAI(), (int)pos.X, (int)pos.Y, ModContent.NPCType<VikingBossAdd>(), 0, 1, NPC.whoAmI, 0, AddNumber, NPC.target);
                AddNumber++;
            }           
            GenerateAddPositions();
        }
        //This is here to use in the BigDash.
        private void SendAdd(int SendCount, int delay = 0)
        {
           MalignantGlobalNPC globalnpc = NPC.GetGlobalNPC<MalignantGlobalNPC>();
            Moving();
            if (AI_Timer == delay)
            {
                int j = 0;
                //this selects the adds that are currently available.
                for (int i = 0; i < SendCount; i++)
                {                  
                    while (Main.npc[globalnpc.Add[j]].ai[0] != State_Circling && j < 40)
                    {
                        j++;
                    }
                    Main.npc[globalnpc.Add[j]].ai[0] = State_Attacking;   //Setting the State of the Add
                    Main.npc[globalnpc.Add[j]].ai[2] = 0;   //carefull here we need to reset to 0 and not -1 !!
                    Main.npc[globalnpc.Add[j]].localAI[0] = 1;  //Assigning the Attack state of the Add
                    j++;
                }
            }
        }

        private void LightningStrike(int delay = 0)
        {
            MalignantGlobalNPC globalnpc = NPC.GetGlobalNPC<MalignantGlobalNPC>();
            Moving();
            //this selects the adds that are currently available.
            if (AI_Timer >= delay)
            {
                int j = 0;
                if ((AI_Timer - delay) % SpawnDelay == 0)
                {
                    while (Main.npc[globalnpc.Add[j]].ai[0] != State_Circling && j < 40)
                    {
                        j++;
                    }
                    Main.npc[globalnpc.Add[j]].ai[0] = State_Attacking;   //Setting the State of the Add
                    Main.npc[globalnpc.Add[j]].ai[2] = 0;   //carefull here we need to reset to 0 and not -1 !!
                    Main.npc[globalnpc.Add[j]].localAI[0] = 0;  //Assigning the Attack state of the Add
                }
            }
        }
        private void Dash(int delay = 0)
        {
            Player target = Main.player[NPC.target];
            if (AI_Timer == delay)
            {
                float time = 0;
                Vector2 npcVel = MethodHelper.LinearAdvancedTargeting(NPC.Center, target.Center, IntSpeed, DashSpeed, ref time);
                MethodHelper.FallingTargeting(NPC, target, new Vector2(0, -28), (int)DashSpeed, ref time, ref npcVel);
                if (time > 15) DashTime = time * (1.4f + 0.3f*TimesDashed);
                else DashTime = 15 * (1.4f + 0.3f * TimesDashed);
                //if (npcVel != Vector2.Zero)
                //{
                //    npcVel.Normalize();
                //}
                //npcVel *= DashSpeed;
                NPC.velocity = npcVel;                
            }
        }
        private void BigDashAttack()
        {
            Player target = Main.player[NPC.target];
            MalignantGlobalNPC globalnpc = NPC.GetGlobalNPC<MalignantGlobalNPC>();
            int DashStartTime = (int)AddNumber * 45 + 120;
            if (AI_Timer == 0)
            {
                for (int i = 0; i < AddNumber; i++)
                {
                    Main.npc[globalnpc.Add[i]].ai[0] = State_Attacking;   //Setting the State of the Add
                    Main.npc[globalnpc.Add[i]].ai[2] = 0;   //carefull here we need to reset to 0 and not -1 !!
                    Main.npc[globalnpc.Add[i]].localAI[0] = 2;  //Assigning the Attack state of the Add
                }
            }
            if (AI_Timer < DashStartTime && TimesDashed == 0)
            {
                float factor = target.Center.X - NPC.Center.X;
                factor /= Math.Abs(factor); 
                MoveTo(target.Center - new Vector2 (factor * Main.screenWidth/3f, Main.screenHeight / 3f), 12, true);
            }  
            
            Dash(DashStartTime);
            if (AI_Timer == DashStartTime)
            {
                npcDashing = true;
            }      
            if (AI_Timer >= DashTime + DashStartTime && TimesDashed < MaxDashes)
            {
                AI_Timer = DashStartTime - DashDelay;
                NPC.velocity *= 0.04f;
                npcDashing = false;
                TimesDashed++;
            }
            if (TimesDashed == MaxDashes)
            {
                npcDashing = false;
                TimesDashed = 0;
                AI_State = State_Moving;
                AI_Timer = -1;
            }           
        }
        private void GenerateAddPositions()
        {
            MalignantGlobalNPC globalnpc = NPC.GetGlobalNPC<MalignantGlobalNPC>();
            double period = 2f * Math.PI / 300f;
            for (int i = 0; i < AddNumber; i++)
            {
                globalnpc.AddPositions[i] = NPC.Center + new Vector2(300 * (float)Math.Cos(period * (RotationTimer + (300 / AddNumber * i))), 300 * (float)Math.Sin(period * (RotationTimer + (300 / AddNumber * i))));
            }
        }
        private bool MoveTo(Vector2 WantedPosition, float TravelSpeed, bool follow)
        {
            Player player = Main.player[NPC.target];
            if (AI_Timer == 0 || follow)
            {
                NPC.velocity = player.velocity;
                tempPos = WantedPosition;
                Vector2 npcVel = tempPos - NPC.Center;
                if (npcVel != Vector2.Zero)
                {
                    npcVel.Normalize();
                }
                npcVel *= TravelSpeed;
                NPC.velocity = npcVel;
            }
            if (NPC.DistanceSQ(tempPos) <= 14 * 14)
            {
                NPC.Center = tempPos;
                NPC.velocity = Vector2.Zero;
                return true;
            }
            return false;
        }
        /*private void SpawnThorns()
        {
            Player player = Main.player[NPC.target];
            if (AI_Timer % 60 == 0 && AI_Timer < 240)
            {
                // Get the ground beneath the player
                Vector2 playerPos = new Vector2(player.position.X / 16, player.position.Y / 16);
                Tile tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                while (!tile.HasTile || tile.TileType == TileID.Trees)
                {
                    playerPos.Y += 1;
                    tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<LightningScythe>(), 26, 2.5f, Main.myPlayer, 0f, 0f);
                }
            }

            if (AI_Timer == 240)
            {
                for (int i = 0; i < 3; i++)
                {
                    // Get the ground beneath the player
                    Vector2 playerPos = new Vector2((player.position.X - 30 * i) / 16, (player.position.Y) / 16);
                    Vector2 playerPos2 = new Vector2((player.position.X + 30 * i) / 16, (player.position.Y) / 16);
                    Tile tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                    while (!tile.HasTile || tile.TileType == TileID.Trees)
                    {
                        playerPos.Y += 1;
                        tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                    }

                    Tile tile2 = Framing.GetTileSafely((int)playerPos2.X, (int)playerPos2.Y);
                    while (!tile2.HasTile || tile2.TileType == TileID.Trees)
                    {
                        playerPos2.Y += 1;
                        tile2 = Framing.GetTileSafely((int)playerPos2.X, (int)playerPos2.Y);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (i == 0)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 28, 2.5f, Main.myPlayer, 0f, 0f);
                        }
                        else
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 28, 2.5f, Main.myPlayer, 0f, 0f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos2 * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 28, 2.5f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
            }

            if (AI_Timer == 300)
            {
                AI_State = State_Moving;
                AI_Timer = -1;
            }
        }
        private void ShootSeeds()
        {
            if (AI_Timer % 75 == 0)
            {
                int numSeeds = NPC.life <= NPC.lifeMax * 0.25f ? 16 : 13;
                float numberProjectiles = Main.rand.Next(7, numSeeds);
                Vector2 position = NPC.Center;
                int speedX = 1;
                int speedY = Main.rand.Next(-25, -15);
                float rotation = MathHelper.ToRadians(45);
                position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f; //this defines the distance of the projectiles form the player when the projectile spawns
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .4f; // This defines the projectile roatation and speed. .4f == projectile speed
                        Projectile.NewProjectile(NPC.GetSource_FromAI(),  NPC.Center.X, NPC.Center.Y - 85, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.PhantasmalBolt, 17, 1f, Main.myPlayer);
                    }
                }
            }

            //if (AI_Timer == 300)
            //{
            //    if (changedPhase2)
            //    {
            //        AI_State = State_Moving;
            //        AI_Timer = -1;
            //    }
            //    else
            //    {
            //        AI_State = State_Moving;
            //        AI_Timer = -1;
            //    }
            //}
        }
        private void MultipleThorns()
        {
            Player player = Main.player[NPC.target];
            if (AI_Timer % 120 == 0)
            {
                int randChoice = Main.rand.Next(2);
                NPC.netUpdate = true;
                if (randChoice == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        // Get the ground beneath the player
                        Vector2 playerPos = new Vector2((player.position.X - 30 * i) / 16, (player.position.Y) / 16);
                        Vector2 playerPos2 = new Vector2((player.position.X + 30 * i) / 16, (player.position.Y) / 16);
                        Tile tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                        while (!tile.HasTile || tile.TileType == TileID.Trees)
                        {
                            playerPos.Y += 1;
                            tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                        }

                        Tile tile2 = Framing.GetTileSafely((int)playerPos2.X, (int)playerPos2.Y);
                        while (!tile2.HasTile || tile2.TileType == TileID.Trees)
                        {
                            playerPos2.Y += 1;
                            tile2 = Framing.GetTileSafely((int)playerPos2.X, (int)playerPos2.Y);
                        }
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (i == 0)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 28, 2.5f, Main.myPlayer, 0f, 0f);
                            }
                            else
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 28, 2.5f, Main.myPlayer, 0f, 0f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos2 * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 28, 2.5f, Main.myPlayer, 0f, 0f);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        // Get the ground beneath the player
                        Vector2 playerPos = new Vector2((player.position.X - 90 * i) / 16, (player.position.Y) / 16);
                        Vector2 playerPos2 = new Vector2((player.position.X + 90 * i) / 16, (player.position.Y) / 16);
                        Tile tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                        while (!tile.HasTile || tile.TileType == TileID.Trees)
                        {
                            playerPos.Y += 1;
                            tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                        }

                        Tile tile2 = Framing.GetTileSafely((int)playerPos2.X, (int)playerPos2.Y);
                        while (!tile2.HasTile || tile2.TileType == TileID.Trees)
                        {
                            playerPos2.Y += 1;
                            tile2 = Framing.GetTileSafely((int)playerPos2.X, (int)playerPos2.Y);
                        }
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            if (i == 0)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(),  playerPos * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 28, 2.5f, Main.myPlayer, 0f, 0f);
                            }
                            else
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(),playerPos * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 28, 2.5f, Main.myPlayer, 0f, 0f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos2 * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 28, 2.5f, Main.myPlayer, 0f, 0f);
                            }
                        }
                    }
                }
            }

            if (AI_Timer == 320)
            {
                AI_State = State_Moving;
                AI_Timer = -1;
            }
        }
        private void ThornWave()
        {
            if (AI_Timer % 15 == 0)
            {
                // Get the ground beneath the player
                Vector2 npcPos = new Vector2((NPC.position.X - 60 * bufferCount) / 16, NPC.position.Y / 16);
                Tile tile = Framing.GetTileSafely((int)npcPos.X, (int)npcPos.Y);
                while (!tile.HasTile || tile.TileType == TileID.Trees)
                {
                    npcPos.Y += 1;
                    tile = Framing.GetTileSafely((int)npcPos.X, (int)npcPos.Y);
                }

                // Same thing going right, I'm lazy
                Vector2 npcPos2 = new Vector2((NPC.position.X + NPC.width + (60 * bufferCount)) / 16, NPC.position.Y / 16);
                Tile tile2 = Framing.GetTileSafely((int)npcPos2.X, (int)npcPos2.Y);
                while (!tile2.HasTile || tile2.TileType == TileID.Trees)
                {
                    npcPos2.Y += 1;
                    tile2 = Framing.GetTileSafely((int)npcPos2.X, (int)npcPos2.Y);
                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(),npcPos2 * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 31, 2.5f, Main.myPlayer, 0f, 0f);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(),  npcPos * 16, new Vector2(0, -10), ProjectileID.CultistBossLightningOrb, 31, 2.5f, Main.myPlayer, 0f, 0f);
                    bufferCount++;
                }
            }

            if (AI_Timer == 180)
            {
                AI_State = State_Moving;
                AI_Timer = -1;
                bufferCount = 0;
            }
        }*/

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.EasyDraw(drawColor, origin: NPC.spriteDirection == 1 ? new Vector2(110, 110) : new Vector2(70, 110));
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            MalignantGlobalNPC globalnpc = NPC.GetGlobalNPC<MalignantGlobalNPC>();
            for (int i = 0; i < AddNumber; i++)
            {
                spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Content/NPCs/Bosses/Zolzar/DebugSqure").Value, globalnpc.AddPositions[i] - Main.screenPosition, new Color(0, 0, 254));
            }          
        }

        public override void FindFrame(int frameHeight)
        {
            int factor = (int)((Main.player[NPC.target].Center.X - NPC.Center.X)/ Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X));
            NPC.frameCounter++;

            if (NPC.frameCounter % 6f == 5f)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 8) // 8 is max # of frames
            {
                NPC.frame.Y = 0; // Reset back to default
            }
            if (!npcDashing)
            {
                NPC.spriteDirection = -factor;
            }
            while (npcDashing == true && NPC.spriteDirection != spriteDirectionStore)
            {
                NPC.spriteDirection = spriteDirectionStore;
            }
        }

        public override bool CheckDead()
        {
            if (NPC.ai[3] == 0f)
            {
                NPC.ai[2] = 0;
                NPC.ai[3] = 1f;
                NPC.damage = 0;
                NPC.life = NPC.lifeMax;
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
                return false;
            }
            return true;
        }
        /*public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<UlvkilSoul>(), 2));
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<VikingBossBag>()));
        }*/
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }
    }
}
