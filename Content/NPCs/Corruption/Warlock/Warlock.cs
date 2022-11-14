using Malignant.Content.Projectiles.Enemy.Warlock;
using Malignant.Core;
using Microsoft.Xna.Framework;
using Malignant.Common;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace Malignant.Content.NPCs.Corruption.Warlock
{
    //AutoloadBossHead]
    public class Warlock : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Warlock");
            Main.npcFrameCount[NPC.type] = 7;
        }
        public override void SetDefaults()
        {
            NPC.width = 65;
            NPC.height = 65;
            NPC.damage = 60;
            NPC.defense = 24;
            NPC.lifeMax = 40500;
            NPC.HitSound = SoundID.NPCHit57;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 3f;
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.aiStyle = -1;
            NPC.netAlways = true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            /*
            for (int num331 = 0; num331 < 20; num331++)s
            {
                DustHelper.DrawCircle(NPC.Center, DustID.ChlorophyteWeapon, 2, 4, 4, 1, 2, nogravity: true);
            }
            */
            DustHelper.NewDustCircular(
                    NPC.Center,
                    18,
                    i => Main.rand.NextFromList(DustID.Blood, DustID.t_Flesh, DustID.Bone),
                    Main.rand.Next(7, 11),
                    minMaxSpeedFromCenter: (2, 7),
                    dustAction: dust => dust.scale = Main.rand.NextFloat(0.85f, 1.7f)
                    );
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.damage = (int)(NPC.damage * 0.5f);
            NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * bossLifeScale);
            NPC.defense += 3 * numPlayers;
        }
        int amountoftimes = 0;
        bool DrawLinearDash = false;
        Vector2 playeroldcenter;
        Vector2 npcoldcenter;
        Vector2 maxvelocity;
        int progress = 0;
        int[] AttackArray;
        float[] phasepercentages;
        int[] DashArray;
        Vector2[] PositionsList;
        bool Dash = true;
        int difficulty;
        float dashproj = 0f;
        public override void AI()
        {
            Player player = Main.player[NPC.target];

            //Despawning stuff
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
           

                if (NPC.active)

                if (NPC.ai[0] != 0)
                {
                    difficulty = CalculateDifficulty(phasepercentages, 5);
                }
            switch (NPC.ai[0])
            {
                //first tick
                case 0:
                    {
                        SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        AttackArray = InitialAttackArray();
                        phasepercentages = InitalPercentageArray();
                        DashArray = InitialDashArray();
                        if (++NPC.ai[1] > 60)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[0] = GetAttack();
                        }
                    }
                    break;
                // randomly offset cursed waves
                case 1:
                    {
                        bool expertMode = Main.expertMode;
                        if (NPC.ai[1] == 0)
                            NPC.velocity = Vector2.Zero;

                        if (++NPC.ai[1] == 10)
                        {
                            SoundEngine.PlaySound(SoundID.Item5);
                            for (int i = 0; i < (difficulty > 4 ? 10 : 7); i++)
                            {
                                int damage = expertMode ? 32 : 48;
                                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + Main.rand.Next(-60, 60), NPC.Center.Y + Main.rand.Next(-60, 60), Main.rand.NextFloat(-5.3f, 5.3f), Main.rand.NextFloat(-5.3f, 5.3f), ModContent.ProjectileType<LeechingBlast>(), damage, 1, Main.myPlayer, 0, 0);
                                Main.projectile[p].scale = Main.rand.NextFloat(.6f, .8f);
                                DustHelper.DrawStar(Main.projectile[p].Center, 272, pointAmount: 6, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);

                                if (Main.projectile[p].velocity == Vector2.Zero)
                                    Main.projectile[p].velocity = new Vector2(2.25f, 2.25f);

                                if (Main.projectile[p].velocity.X < 2.25f && Math.Sign(Main.projectile[p].velocity.X) == Math.Sign(1) || Main.projectile[p].velocity.X > -2.25f && Math.Sign(Main.projectile[p].velocity.X) == Math.Sign(-1))
                                    Main.projectile[p].velocity.X *= 2.15f;

                                Main.projectile[p].netUpdate = true;
                            }
                        }
                        if (NPC.ai[1] > 60)
                        {
                            NPC.ai[1] = 0;
                            if (++amountoftimes >= 1)
                            {
                                amountoftimes = 0;
                                NPC.ai[0] = GetAttack();
                            }
                        }
                    }
                    break;
                //move in circle around player and fire cursed waves
                case 2:
                    {
                        bool expertMode = Main.expertMode;
                        int pointmax = 36;
                        PositionsList = CalculatePointsInAcircle(player.Center, 300, pointmax);
                        if (NPC.ai[1] == 0)
                            progress = FindClosesPoint(PositionsList);
                        maxvelocity = NPC.velocity = FollowPoints(PositionsList, out var newprogress, progress, NPC.Center, 10);
                        progress = newprogress;
                        if (++NPC.ai[1] > 8)
                        {
                            NPC.ai[1] = 8;
                        }
                        if (++NPC.ai[1] == 8)
                        {
                            int dmg = expertMode ? 32 : 48;
                            SoundEngine.PlaySound(SoundID.Item5);
                            for (int i = 0; i < (difficulty > 4 ? 1 : 7); i++)
                            {
                                //int damage = expertMode ? 32 : 48;
                                DustHelper.DrawStar(NPC.Center, 163, pointAmount: 163, mainSize: 2.7425f, dustDensity: 4, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
                                SoundEngine.PlaySound(SoundID.NPCDeath55 with { PitchVariance = 0.2f }, NPC.Center);
                                float spread = 10f * 0.0174f;
                                double startAngle = Math.Atan2(6, 6) - spread / 2;
                                double deltaAngle = spread / 8f;



                                double offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
                                int damage = expertMode ? 32 : 48;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<CursedWave>(), damage, 0, player.whoAmI);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * 3f), (float)(-Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<CursedWave>(), damage, 0, player.whoAmI);

                            }
                            NPC.ai[2] = 0;
                        }
                        if (progress >= pointmax)
                        {
                            progress = 0;
                            if (amountoftimes++ >= 1)
                            {
                                NPC.ai[0]++;
                            }
                        }

                    }
                    break;
                //decelarate after spin
                case 3:
                    {
                        NPC.velocity = Decelerate(NPC.ai[2], maxvelocity);
                        NPC.ai[2] += 0.06f;
                        if (NPC.ai[2] >= 1)
                        {
                            PositionsList = null;
                            maxvelocity = Vector2.Zero;
                            NPC.velocity = Vector2.Zero;
                            amountoftimes = 0;
                            ResetAllAis();
                            NPC.ai[0] = GetAttack();
                        }
                    }
                    break;
                //dash 3 times
                case 4:
                    {
                        bool expertMode = Main.expertMode;
                        if (NPC.ai[1] == 0)
                        {
                            DrawLinearDash = true;
                            playeroldcenter = player.Center;
                            npcoldcenter = NPC.Center;
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        }
                        NPC.ai[2] += 0.03f;
                        dashproj += 0.01f;
                        if (difficulty > 2 && dashproj > 0.06f)
                        {
                            dashproj = 0;
                            int damage = expertMode ? 32 : 48;
                            //Projectile.NewProjectile(NPC.GetBossSpawnSource(player.whoAmI), NPC.Center, -Utility.Normalized(NPC.velocity).RotatedBy(-0.15) * 5, ModContent.ProjectileType<LifeDrainer>(), NPC.damage, 0.5f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetBossSpawnSource(player.whoAmI), NPC.Center, -Utility.Normalized(NPC.velocity).RotatedBy(0.15) * 3, ModContent.ProjectileType<WarlockRune>(), NPC.damage, 0.5f, Main.myPlayer);
                        }
                        if (++NPC.ai[1] == 30)
                        {
                            maxvelocity = NPC.velocity = LinearDashVelocity(40f, NPC.Center, playeroldcenter);
                        }
                        if (NPC.ai[1] > 30f)
                        {
                            NPC.ai[3] += 0.4f / MathHelper.Clamp(NPC.Distance(playeroldcenter), 0, 10);
                            NPC.velocity = Decelerate(NPC.ai[3], maxvelocity);
                        }
                        if (NPC.ai[3] >= 1f)
                        {
                            ResetAllAis();
                            playeroldcenter = Vector2.Zero;
                            npcoldcenter = Vector2.Zero;
                            maxvelocity = Vector2.Zero;
                            NPC.velocity = Vector2.Zero;
                            DrawLinearDash = false;
                            progress = 3;
                            if (++amountoftimes > 3)
                            {
                                amountoftimes = 0;
                                NPC.ai[0] = GetAttack();
                                NPC.ai[1] = 0;
                            }
                        }
                    }
                    break;
                #region unused attacks 
                    //life drain 
                case 5:
                    {
                        if (NPC.ai[2] > 0)
                        {
                            if (++NPC.ai[1] == 15)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    Vector2 vector;
                                    double angle = Main.rand.NextDouble() * 2d * Math.PI;
                                    vector.X = (float)(Math.Sin(angle) * 100);
                                    vector.Y = (float)(Math.Cos(angle) * 100);
                                    Dust dust2 = Main.dust[Dust.NewDust(vector, 2, 2, DustID.LifeDrain, 0f, 0f, 100, default, 1f)];
                                    dust2.noGravity = true;
                                }

                                Vector2 place = NPC.Center + Utility.GetRandomVector(250, 250, 350, 350, -350, -350);
                                SoundEngine.PlaySound(SoundID.Item5);
                                if (Main.rand.NextBool(2))
                                    Projectile.NewProjectile(NPC.GetBossSpawnSource(player.whoAmI), place, Utility.DirectionTo(player.Center, place).RotatedByRandom(0.3f) * 10, ModContent.ProjectileType<LeechingBlast>(), NPC.damage, 0.5f, Main.myPlayer);
                            }
                            if (NPC.ai[1] > 15)
                            {
                                NPC.ai[1] = 0;
                                if (++amountoftimes >= 3)
                                {
                                    ResetAllAis();
                                    amountoftimes = 0;
                                    NPC.ai[0] = GetAttack();
                                }
                            }
                        }
                        else
                        {
                            NPC.velocity = NPC.DirectionTo(player.Center) * 10;
                            if (NPC.Distance(player.Center) < 100)
                            {
                                NPC.velocity = Vector2.Zero;
                                NPC.ai[2]++;
                            }
                        }
                    }
                    break;
                // cone dash w/ healing minions?
                case 6:
                    {
                        bool expertMode = Main.expertMode;
                        if (NPC.ai[1] == 0)
                        {
                            int dmg = expertMode ? 32 : 48;
                            if (++NPC.ai[1] % 120 == 0 && NPC.ai[1] < 360)
                            {
                                for (int i = -1; i < 1; i++)
                                {
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)((player.Center.X + 300 * 51) + (/*150*/ 75 + (/*300*/ 150 * i))), (int)NPC.Center.Y, ModContent.NPCType<WarlockMinion>(), 0, 0, 0);
                                    }
                                }
                                int count = 0;
                                for (int k = 0; k < 200; k++)
                                {
                                    if (Main.npc[k].active && Main.npc[k].type == Mod.Find<ModNPC>("WarlockMinion").Type)
                                    {
                                        if (count < 4)
                                        {
                                            count++;
                                        }
                                        else
                                        {
                                            ((WarlockMinion)Main.npc[k].ModNPC).kill = true;
                                        }
                                    }
                                }
                            }
                            DrawLinearDash = true;
                            playeroldcenter = player.Center;
                            npcoldcenter = NPC.Center;
                            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                        }
                        NPC.ai[2] += 0.01f;
                        if (++NPC.ai[1] == 17)
                        {
                            maxvelocity = NPC.velocity = LinearDashVelocity(60f, NPC.Center, playeroldcenter);
                        }
                        if (NPC.ai[1] > 17f)
                        {
                            NPC.ai[3] += 0.4f / MathHelper.Clamp(NPC.Distance(playeroldcenter), 0, 10);
                            NPC.velocity = Decelerate(NPC.ai[3], maxvelocity);
                        }
                        if (NPC.ai[3] >= 1f)
                        {
                            ResetAllAis();
                            playeroldcenter = Vector2.Zero;
                            npcoldcenter = Vector2.Zero;
                            maxvelocity = Vector2.Zero;
                            NPC.velocity = Vector2.Zero;
                            DrawLinearDash = false;
                            NPC.ai[0] = GetAttack();
                        }
                    }
                    break;
                    #endregion
            }
        }
        #region meth
        /*private bool AnyProjectiles(int type)
        {
            bool any = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
                if (Main.projectile[i].type == type)
                    any = true;
            return any;
        }*/
        private int GetAttack()
        {
            return AttackArray[Main.rand.Next(0, AttackArray.Length - 1)];
        }
        private void ChangeAttacks()
        {
            int Difficulty = CalculateDifficulty(phasepercentages, 5);
            if (Difficulty < 2 && ContainsNumber(6))
                RemoveNumberFromArray(6, 2);
            if (Difficulty >= 2 && !ContainsNumber(6))
                AddAtRandom(6, 3);
            if (Difficulty < 3 && ContainsNumber(5))
                RemoveNumberFromArray(5, GetRandom1());
            if (Difficulty >= 3 && !ContainsNumber(5))
                AddAtRandom(5, 2);

        }

        private int[] InitialAttackArray()
        {
            int[] array = new int[10];
            array[0] = 1;
            array[1] = 1;
            array[2] = 2;
            array[3] = 1;
            array[4] = 2;
            array[5] = 2;
            array[6] = 4;
            array[7] = 4;
            array[8] = 4;
            array[9] = 2;
            return array;
        }
        private int[] InitialDashArray()
        {
            int[] array = new int[6];
            array[0] = 4;
            array[1] = 4;
            array[2] = 4;
            array[3] = 4;
            array[4] = 4;
            array[5] = 4;
            return array;
        }
        private float[] InitalPercentageArray()
        {
            float[] array = new float[4];
            array[0] = 15f;
            array[1] = 45f;
            array[2] = 75f;
            array[3] = 90f;
            return array;
        }
        private int GetRandom1()
        {
            int num = -1;
            switch (Main.rand.Next(1, 2))
            {
                case 1:
                    num = 1;
                    break;
                case 2:
                    num = 2;
                    break;
            }
            return num;
        }
        private void AddAtRandom(int num, int amount)
        {
            for (int i = 0; i < amount; i++)
                AttackArray[Main.rand.Next(0, AttackArray.Length - 1)] = num;
        }

        private void RemoveNumberFromArray(int remove, int replace)
        {
            for (int i = 0; i < AttackArray.Length - 1; i++)
                if (AttackArray[i] == remove)
                    AttackArray[i] = replace;
        }
        /*private void RemoveNumberFromDashArray(int remove, int replace)
        {
            for (int i = 0; i < DashArray.Length - 1; i++)
                if (DashArray[i] == remove)
                    DashArray[i] = replace;
        }*/
        private bool ContainsNumber(int number)
        {
            bool contains = false;
            for (int i = 0; i < AttackArray.Length - 1; i++)
                if (AttackArray[i] == number)
                    contains = true;
            return contains;
        }
        /*private bool ContainsNumberDash(int number)
        {
            bool contains = false;
            for (int i = 0; i < DashArray.Length - 1; i++)
                if (DashArray[i] == number)
                    contains = true;
            return contains;
        }*/
        private int CalculateDifficulty(float[] percentages, int MaxDifficulty)
        {
            int difficulty = 1;
            if (NPC.life < NPC.lifeMax * 0.75f)
                difficulty += 1;
            if (NPC.life < NPC.lifeMax * 0.5f)
                difficulty += 1;
            if (NPC.life < NPC.lifeMax * 0.25f)
                difficulty += 1;
            return (int)MathHelper.Clamp(difficulty, 1, MaxDifficulty);
        }

        private Vector2[] CalculatePointsInAcircle(Vector2 origin, float radius, int numLocations)
        {
            Vector2[] list = new Vector2[numLocations + 1];
            for (int i = 0; i < numLocations; i++)
            {
                list[i] = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
            }
            return list;
        }
        private Vector2 FollowPoints(Vector2[] points, out int newprogress, int progress, Vector2 start, float speed = 1)
        {
            newprogress = progress;
            Vector2 velocity = Utility.DirectionTo(points[progress], start) * speed;
            if (Vector2.Distance(points[progress], start + velocity) < 200)
            {

                newprogress++;
            }
            return velocity;
        }
        private int FindClosesPoint(Vector2[] points)
        {
            int closest = 0;
            for (int i = 0; i < points.Length - 1; i++)
                if (NPC.Distance(points[i]) < NPC.Distance(points[closest]))
                    closest = i;
            return closest;
        }
        private void ResetAllAis(bool excludezero = true)
        {
            for (int i = excludezero ? 1 : 0; i < NPC.ai.Length; i++)
                NPC.ai[i] = 0;
        }
        private bool IsEven(int num)
        {
            return num % 2 == 0;
        }
        private Vector2 LinearDashVelocity(float speed, Vector2 start, Vector2 end)
        {
            Vector2 velocity = Utility.DirectionTo(end, start);
            velocity *= speed;
            return velocity;
        }
        private Vector2 Decelerate(float progress, Vector2 maxvelocity)
        {
            Vector2 velocity = Vector2.Lerp(maxvelocity, Vector2.Zero, progress);
            return velocity;
        }
        #endregion

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter % 6f == 5f)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 7) 
            {
                NPC.frame.Y = 0; 
            }
        }

        Vector2 lastPosNoDash = Vector2.Zero;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float velLengthSQ = NPC.velocity.LengthSquared();
            if (NPC.ai[0] == 4 && velLengthSQ > 0f)
            {
                Vector2[] positions = new Vector2[15];
                float diff = (NPC.Center - lastPosNoDash).Length() / positions.Length;
                Vector2 dir = NPC.Center.DirectionTo(lastPosNoDash);
                for (int i = 1; i < positions.Length; i++)
                {
                    positions[i] = NPC.Center + dir * diff * i;
                }

                NPC.EasyDrawAfterImage(drawColor * velLengthSQ * 0.5f, positions);
            }
            else
            {
                lastPosNoDash = NPC.Center;
            }

            NPC.EasyDrawNPC(drawColor, origin: new Vector2(80, 70));
            return false;
        }
    }
}