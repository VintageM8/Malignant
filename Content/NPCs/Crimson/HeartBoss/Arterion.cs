using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using static Malignant.Common.Systems.CameraSystem;
using Malignant.Common.Systems;
using Malignant.Core;
using Malignant.Content.NPCs.Corruption.Warlock;
using Malignant.Content.Projectiles.Enemy.Warlock;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.ItemDropRules;
using Malignant.Content.Items.Crimson.Arterion.BurstingArtery;
using Malignant.Content.Items.Crimson.Arterion.HerzanfallDagger;
using Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles;

namespace Malignant.Content.NPCs.Crimson.HeartBoss
{
    public class Arterion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arterion");
            /*NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
            NPCID.Sets.TrailingMode[Type] = 0;*/
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void SetDefaults()
        {
            NPC.width = 116;
            NPC.height = 114;
            NPC.lifeMax = 82000;
            NPC.defense = 5;
            NPC.aiStyle = 0;
            NPC.damage = 10;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.boss = false;
            if (Main.getGoodWorld)
                NPC.scale = 0.5f;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Arterion");
            }
        }

        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !ded)
            {
                NPC.life = 1;
                AIState = Death;
                CameraSystem.ScreenShakeAmount = 15f;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                NPC.dontTakeDamage = true;
                ded = true;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
                NPC.life = 1;
                return false;
            }
            return true;
        }

        #region Drops
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptions(1, new int[]
            {
                ItemType<BurstingArtery>(),
                ItemType<HerzanfallDagger>(),
            }
            ));

        }
        #endregion

        /*public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (damage >= NPC.life && !ded)
            {
                damage = 0;
                AIState = Death;
                NPC.life = 1;
                NPC.dontTakeDamage = true;
                ded = true;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (damage >= NPC.life && !ded)
            {
                damage = 0;
                ded = true;
                AIState = Death;
                NPC.life = 1;
                NPC.dontTakeDamage = true;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
            }
        }*/

        bool DrawLinearDash = false;
        Vector2 playeroldcenter;
        Vector2 npcoldcenter;

        bool ded;
        private const int AISlot = 0;
        private const int TimerSlot = 1;
        private const int Angry = -3, Death = -4;
        private const int Idle = -1;
        private const int Intro = -2;
        private const int PreProvokation = 0;
        private const int BloodBubble = 1;
        private const int BloodSplat = 2;
        private const int Floaties = 3;
        private const int BloodBubble2 = 4;
        private const int BloodSplat2 = 5;
        public float AIState
        {
            get => NPC.ai[AISlot];
            set => NPC.ai[AISlot] = value;
        }

        public float AITimer
        {
            get => NPC.ai[TimerSlot];
            set => NPC.ai[TimerSlot] = value;
        }
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        bool stunned;
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (AIState == Floaties && AITimer > 100 && !stunned)
            {
                NPC.velocity = Vector2.Zero;
                NPC.damage = 0;
                stunned = true;
                NPC.noTileCollide = false;
                NPC.noGravity = false;
                NPC.ai[3] = 200;
                NPC.frameCounter = 0;
            }
        }
        int firstDir;

        bool yes;
        int nextAttack = Floaties;
        Vector2[] random = new Vector2[8]; 
        Vector2 arena;
        int aa;
        bool angery;
        float alpha;
        public override void AI()
        {
            if (NPC.life < NPC.lifeMax / 2 && !angery && Main.expertMode)
            {
                AITimer = 0;
                AITimer2 = 0;
                AIState = Angry;
                angery = true;
            }
            if (AIState != Floaties)
                NPC.damage = 0;
            Player player = Main.player[NPC.target];
            /*if (Main.dayTime)
            {
                AIState = -1;
                stunned = false;
                NPC.velocity = new Vector2(0, -20f);
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
            }*/
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead)
                {
                    NPC.velocity = new Vector2(0, 10f);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    return;
                }
            }
            if (stunned)
            {
                NPC.ai[3]--;
                if (NPC.ai[3] <= 0)
                {
                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    stunned = false;
                    NPC.frameCounter = 0;
                    NPC.velocity = Vector2.Zero;
                }
            }
            if (AIState == PreProvokation)
            {
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.0005f);
                if (NPC.life < NPC.lifeMax)
                {
                    AIState = Intro;
                    NPC.boss = true;
                    //Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/voltageVagrant");
                }
            }
            if (AIState == Intro)
            {
                AITimer++;
                if (AITimer == 1)
                {
                    NPC.boss = true;
                    //Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/voltageVagrant");
                    yes = true;
                    CameraSystem.ScreenShakeAmount = 20f;
                    //RegreUtils.SetBossTitle(160, "Voltage Vagrant", Color.White, "Bringer of Storms", BossTitleStyleID.Vagrant);
                    //RegreSystem.ChangeCameraPos(NPC.Center, 160);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BetsySummon);
                }
                if (AITimer >= 160)
                {
                    AITimer = 0;
                    AIState = Idle;
                }
            }
            else if (AIState == Death)
            {
                AITimer2++;
                AITimer++;
                SoundStyle a = SoundID.NPCHit1;
                a.Volume = 0;
                NPC.HitSound = a; Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = NPC.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, NPC.width / 2, NPC.height / 2, DustID.OasisCactus, 0f, 1f, 0, new Color(255, 255, 255), 1.2790698f)];

                if (AITimer < 120)
                {
                    if (AITimer2 == 5)
                    {
                        Vector2 rand = new Vector2(NPC.position.X + NPC.width * Main.rand.NextFloat(), NPC.position.Y + NPC.height * Main.rand.NextFloat());
                        Projectile.NewProjectile(NPC.GetSource_Death(), rand, Vector2.Zero, ModContent.ProjectileType<DeathOrb>(), 0, 1);
                    }
                    if (AITimer2 >= 10)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_LightningBugHurt);
                        Vector2 rand = new Vector2(NPC.position.X + NPC.width * Main.rand.NextFloat(), NPC.position.Y + NPC.height * Main.rand.NextFloat());
                        Projectile.NewProjectile(NPC.GetSource_Death(), rand, Vector2.Zero, ModContent.ProjectileType<DeathOrb>(), 0, 1);
                        AITimer2 = 0;
                    }
                }
                if (AITimer == 110)
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.damage = 0;
                    stunned = true;
                    NPC.noTileCollide = false;
                    NPC.noGravity = false;
                    NPC.ai[3] = 140;
                    NPC.frameCounter = 0;
                }
                if (AITimer == 250)
                    aa = NPC.direction;
                if (AITimer > 250)
                {
                    NPC.aiStyle = -1;
                    NPC.direction = aa;
                    NPC.velocity = new Vector2(4 * aa, -10);
                }
                if (AITimer >= 400)
                {
                    NPC.immortal = false;
                    NPC.StrikeNPC(NPC.lifeMax, 0, 0);
                }
            }
            else if (AIState == Angry)
            {
                AITimer++;
                if (AITimer == 1)
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ScaryScream);
                if (AITimer > 50)
                {
                    AITimer2++;
                    if (AITimer2 == 5)
                    {
                        random[3] = player.Center;
                        /*for (int i = 0; i < 3; i++)
                        {
                            random[i] = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), player.Center.Y);
                            RegreUtils.SpawnTelegraphLine(random[i], NPC.GetSource_FromAI());
                        }*/
                        //Utility.SpawnTelegraphLine(random[3], NPC.GetSource_FromAI());
                    }
                    if (AITimer2 == 10)
                    {
                        ScreenShakeAmount = 5f;
                        AITimer2 = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            Projectile.NewProjectile(null, NPC.Center, new Vector2(-5, 1), ModContent.ProjectileType<SpikeSpawner>(), 0, 0);
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), random[3], Vector2.Zero, ModContent.ProjectileType<PlayerTele>(), 5, 0);
                    }
                }
                if (AITimer >= 230)
                {
                    AIState = Idle;
                    nextAttack = Floaties;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            if (AIState == Idle)
            {
                AITimer++;
                NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.015f);
                if (AITimer >= 180)
                {
                    AIState = nextAttack;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == BloodBubble)
            {
                if (alpha < 1 && AITimer < 10)
                    alpha += 0.05f;
                if (!NPC.AnyNPCs(ModContent.NPCType<MiniHeart>()))
                    AITimer++;

                NPC.Center = Vector2.Lerp(NPC.Center, player.Center - Vector2.UnitY * 200, 0.035f);
                NPC.dontTakeDamage = NPC.AnyNPCs(ModContent.NPCType<MiniHeart>());
                if (AITimer == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float angle = 2f * (float)Math.PI / 4f * i;
                        Point pos = new Vector2(NPC.Center.X + (float)Math.Cos(angle) * 100, NPC.Center.Y + (float)Math.Sin(angle) * 100).ToPoint();
                        NPC.NewNPC(NPC.GetSource_FromAI(), pos.X, pos.Y, ModContent.NPCType<MiniHeart>(), 0, NPC.whoAmI, angle, i);

                        Projectile.NewProjectile(null, NPC.Center, new Vector2(-5, 1), ModContent.ProjectileType<SpikeSpawner>(), 0, 0);
                        Projectile.NewProjectile(null, NPC.Center, new Vector2(-5, 1), ModContent.ProjectileType<SpikeSpawner>(), 0, 0);
                        Projectile.NewProjectile(null, NPC.Center, new Vector2(-2.5f, 1), ModContent.ProjectileType<SpikeSpawner>(), 0, 0);
                        Projectile.NewProjectile(null, NPC.Center, new Vector2(-2.5f, 1), ModContent.ProjectileType<SpikeSpawner>(), 0, 0);
                        Projectile.NewProjectile(null, NPC.Center, new Vector2(5, 1), ModContent.ProjectileType<SpikeSpawner>(), 0, 0);
                        Projectile.NewProjectile(null, NPC.Center, new Vector2(5, 1), ModContent.ProjectileType<SpikeSpawner>(), 0, 0);
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_LightningBugDeath);
                    AITimer++;
                }
                AITimer2++;
                if (angery)
                    AITimer2++;
                if (AITimer2 >= 40)
                    AITimer2 = -40;
                if (AITimer2 == 20)
                {

                }
                if (AITimer > 10 && alpha > 0)
                    alpha -= 0.05f;
                if (AITimer >= 60)
                {
                    AIState = Idle;
                    nextAttack = BloodSplat;
                    NPC.dontTakeDamage = false;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == Floaties)
            {
                if (!stunned)
                {
                    AITimer++;
                    if (angery)
                        AITimer++;
                }
                else
                    AITimer = 239;
                if (AITimer > 1)
                {
                    if (AITimer % 5 == 0)
                    {
                        Vector2 place = NPC.Center + Utility.GetRandomVector(250, 250, 350, 350, -350, -350);
                        DrawLinearDash = true;
                    }
                }
                if (AITimer == 1 + (angery ? 1 : 0))
                {
                    arena = new Vector2(Main.screenPosition.X, player.Center.Y);
                    NPC.ai[3] = 1;
                }
                else if (AITimer == 101 + (angery ? 1 : 0))
                {
                    NPC.ai[3] = -1;
                }
                if (AITimer < 100 || (AITimer > 150 && AITimer < 200))
                {
                    NPC.damage = 0;
                    NPC.velocity = Vector2.Zero;
                    NPC.Center = Vector2.Lerp(NPC.Center, player.Center + Vector2.UnitX * 550 * NPC.ai[3], 0.035f);
                }
                //NPC.Center = new Vector2(NPC.Center.X, Vector2.Lerp(NPC.Center, player.Center, 0.035f).Y);
                if (AITimer >= 35)
                {
                    if (++AITimer2 >= 8)
                    {
                        AITimer2 = 0;
                        int rain = 0;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                rain = ModContent.ProjectileType<HomingChunk>();
                                break;
                            case 1:
                                rain = ModContent.ProjectileType<HomingChunk>();
                                break;
                            case 2:
                                rain = ModContent.ProjectileType<HomingChunk>();
                                break;
                        }
                        Vector2 random = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), random, new Vector2(Main.windSpeedCurrent * 2, 5), rain, 10, 0, Main.myPlayer);
                    }
                }

                if ((AITimer == 100 || AITimer == 200) && !stunned)
                {
                    NPC.rotation = 0;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.ScaryScream);
                    NPC.damage = 15;
                    NPC.velocity.X *= 0.98f;
                    Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));

                    float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                    NPC.velocity.X = (float)(Math.Cos(rotation2) * 65) * -1;
                    //NPC.velocity += new Vector2(40 * (NPC.direction), 0);
                }
                if (AITimer >= 240)
                {
                    stunned = false;
                    NPC.damage = 10;
                    AIState = Idle;
                    nextAttack = BloodBubble;
                    NPC.rotation = 0;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == BloodSplat)
            {
                NPC.frameCounter++;
                AITimer++;
                if (AITimer < 30)
                    NPC.Center = Vector2.Lerp(NPC.Center, player.Center, 0.025f);
                if (AITimer == 40)
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BetsysWrathImpact);
                if (AITimer > 50)
                {
                    AITimer2++;
                    if (AITimer2 == 10)
                    {
                        random[3] = player.Center;
                        for (int i = 0; i < 3; i++)
                        {
                            random[i] = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), player.Center.Y);
                            //RegreUtils.SpawnTelegraphLine(random[i], NPC.GetSource_FromAI());
                        }
                        //RegreUtils.SpawnTelegraphLine(random[3], NPC.GetSource_FromAI());
                    }
                    if (AITimer2 == (angery ? 45 : 60))
                    {
                        CameraSystem.ScreenShakeAmount = 5f;
                        AITimer2 = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            float spread = 10f * 0.0174f;
                            double startAngle = Math.Atan2(6, 6) - spread / 2;
                            double deltaAngle = spread / 8f;


                            bool expertMode = Main.expertMode;
                            double offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
                            int damage = expertMode ? 32 : 48;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<HeartBomb>(), damage, 0, player.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * 3f), (float)(-Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<HeartBomb>(), damage, 0, player.whoAmI);
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), random[3], Vector2.Zero, ModContent.ProjectileType<BloodBubble>(), 15, 0);
                    }
                }
                if (AITimer >= 290)
                {
                    AIState = Idle;
                    nextAttack = BloodBubble2;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == BloodBubble2) //Heal Spikes
            {
                AITimer++;
                if (++AITimer2 >= 100)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        float angle = 2f * (float)Math.PI / 7f * i;
                        Vector2 pos = new Vector2(NPC.Center.X + (float)Math.Cos(angle) * 100, NPC.Center.Y + (float)Math.Sin(angle) * 100);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Vector2.Zero, ModContent.ProjectileType<HealSpikes>(), 15, 0, player.whoAmI, 0, angle);
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BetsysWrathImpact);
                    AITimer2 = 0;
                }


                if (AITimer >= 200)
                {
                    AIState = Idle;
                    if (angery)
                        nextAttack = BloodSplat2;
                    else
                        nextAttack = Floaties;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
            else if (AIState == BloodSplat2)
            {
                NPC.frameCounter++;
                AITimer++;
                if (AITimer == 1)
                {
                    AITimer2 = 1;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BetsysWrathImpact);
                }
                if (AITimer >= 50)
                {
                    if (++AITimer2 >= 5)
                    {
                        AITimer2 = 0;
                        int rain = 0;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                rain = ModContent.ProjectileType<BloodBubble>();
                                break;
                            case 1:
                                rain = ModContent.ProjectileType<BloodBubble>();
                                break;
                            case 2:
                                rain = ModContent.ProjectileType<BloodBubble>();
                                break;
                        }
                        Vector2 random = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), random, new Vector2(0, 7), rain, 10, 0, Main.myPlayer);
                    }
                    if (AITimer == 50 || AITimer == 100 || AITimer == 150 || AITimer == 200 || AITimer == 250)
                    {
                        random[7] = player.Center;
                        //RegreUtils.SpawnTelegraphLine(random[7], NPC.GetSource_FromAI());
                    }
                    if (AITimer == 75 || AITimer == 125 || AITimer == 175 || AITimer == 225 || AITimer == 275)
                    {
                        CameraSystem.ScreenShakeAmount = 10f;
                        for (int i = 0; i < 2; i++)
                        {
                            bool expertMode = Main.expertMode;
                            float spread = 10f * 0.0174f;
                            double startAngle = Math.Atan2(6, 6) - spread / 2;
                            double deltaAngle = spread / 8f;



                            double offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
                            int damage = expertMode ? 32 : 48;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<HeartBomb>(), damage, 0, player.whoAmI);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * 3f), (float)(-Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<HeartBomb>(), damage, 0, player.whoAmI);
                        }
                    }
                }

                if (AITimer >= 275)
                {
                    AIState = Idle;
                    nextAttack = Floaties;
                    AITimer = 0;
                    AITimer2 = 0;
                }
            }
        }

        #region DrawCode

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (DrawLinearDash)
                DrawDashLinear(spriteBatch, npcoldcenter, playeroldcenter, Color.Lerp(Color.White, Color.Red, NPC.ai[2]));
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
        private void DrawDashLinear(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            Vector2 unit = end - start;
            unit.Normalize();
            DrawLine(start, end + unit * 4000, color, spriteBatch);
        }
        private void DrawLine(Vector2 start, Vector2 end, Color color, SpriteBatch spriteBatch, float scale = 1)
        {
            Vector2 unit = end - start;
            float length = unit.Length();
            unit.Normalize();
            for (int i = 0; i < length; i++)
            {
                Vector2 drawpos = start + unit * i - Main.screenPosition;
                spriteBatch.Draw(ModContent.Request<Texture2D>("Malignant/Assets/Textures/Pixel").Value, drawpos, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }
        #endregion
    }
}
