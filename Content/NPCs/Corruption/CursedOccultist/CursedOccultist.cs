using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.NPCs.Corruption.CursedOccultist
{
    public class CursedOccultist : ModNPC
    {
        public bool spawningRuneCircle = false;
        public int spawningRuneCircleTimer = 0;
        public int runecircleTimer = 0;
        public int projectileTimer = 1;
        public int resetTimer = 0;
        public int spawnedProjectiles = 0;
        public int rune1 = 0;
        public int rune2 = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName("Cursed Occultist");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 44;
            NPC.lifeMax = 150;
            NPC.defense = 7;
            NPC.value = 100f;
            AIType = 3;
            NPC.knockBackResist = 0.2f;
            NPC.width = 24;
            NPC.height = 36;
            NPC.damage = 28;
            NPC.lavaImmune = false;
            NPC.HitSound = SoundID.NPCHit1;
        }


        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(spawningRuneCircle);
            writer.Write(spawningRuneCircleTimer);
            writer.Write(runecircleTimer);
            writer.Write(projectileTimer);
            writer.Write(resetTimer);
            writer.Write(spawnedProjectiles);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            spawningRuneCircle = reader.ReadBoolean();
            spawningRuneCircleTimer = reader.ReadInt32();
            runecircleTimer = reader.ReadInt32();
            projectileTimer = reader.ReadInt32();
            resetTimer = reader.ReadInt32();
            spawnedProjectiles = reader.ReadInt32();
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;
            if ((NPC.velocity.X != 0f && NPC.velocity.Y == 0f))
            {
                if (NPC.frameCounter >= 7)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y = ((NPC.frame.Y + 1) % 5) * frameHeight;
                }
            }
            else
                NPC.frame.Y = 2 * frameHeight;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            NPC.spriteDirection = NPC.direction;

            spawningRuneCircleTimer++;
            if (spawningRuneCircleTimer >= 240)
                spawningRuneCircle = true;
            else
                spawningRuneCircle = false;

            if (spawningRuneCircle)
            {

                NPC.aiStyle = -1;
                NPC.velocity.X = 0f;
                NPC.velocity.Y = 4f;
                if (NPC.collideY)
                {
                    runecircleTimer++;
                    if (runecircleTimer > 45)
                        projectileTimer++;
                    if (runecircleTimer == 45 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
                        rune1 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y - 90, 0f, 0f, ModContent.ProjectileType<OccultRune1>(), 10, 3f, 0);
                        rune2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y - 90, 0f, 0f, ModContent.ProjectileType<OccultRune2>(), 10, 3f, 0);
                        Main.projectile[rune1].Center += new Vector2(0, 35).RotatedBy(MathHelper.ToRadians(120));
                        Main.projectile[rune2].Center += new Vector2(0, 35).RotatedBy(MathHelper.ToRadians(240));
                        Main.projectile[rune1].ai[1] = NPC.whoAmI;
                        Main.projectile[rune2].ai[1] = NPC.whoAmI;
                    }

                    if (projectileTimer % 60 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int chosenProjectile = 0;
                        switch (Main.rand.Next(4))
                        {
                            case 0:
                                chosenProjectile = ModContent.ProjectileType<Rune_1>();
                                break;
                            case 1:
                                chosenProjectile = ModContent.ProjectileType<Rune_2>();
                                break;
                            case 2:
                                chosenProjectile = ProjectileID.GreekFire1;
                                break;
                            case 3:
                                chosenProjectile = ProjectileID.CursedFlameHostile; //I forgor name lolz
                                break;
                            default:
                                break;
                        }
                        spawnedProjectiles++;
                        SoundEngine.PlaySound(SoundID.Item28, NPC.Center);
                        int chosenDust = Main.rand.NextBool(2) ? 173 : 157;
                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 2, NPC.Center.Y - 88, 0f, 0f, chosenProjectile, 20, 3f, 0);
                        Main.projectile[p].ai[1] = NPC.whoAmI;
                        int num = 10;
                        for (int index1 = 0; index1 < num; ++index1)
                        {
                            int index2 = Dust.NewDust(new Vector2(NPC.Center.X + 2, NPC.Center.Y - 88), 0, 0, chosenDust, 0.0f, 0.0f, 0, new Color(), 0.75f);
                            Main.dust[index2].velocity *= 1.2f;
                            --Main.dust[index2].velocity.Y;
                            Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, new Vector2(NPC.Center.X + 2, NPC.Center.Y - 88), 0.75f);
                        }
                    }
                    if (spawnedProjectiles >= 2)
                        resetTimer++;
                    if (resetTimer > 200)
                    {
                        Main.projectile[rune1].Kill();
                        Main.projectile[rune2].Kill();
                        runecircleTimer = 0;
                        spawningRuneCircleTimer = 0;
                        spawnedProjectiles = 0;
                        projectileTimer = 1;
                        resetTimer = 0;
                        NPC.netUpdate = true;
                    }
                    if (Main.projectile[rune1].active && Main.projectile[rune2].active && runecircleTimer > 59)
                    {
                        DrawDustBeetweenThisAndThat(Main.projectile[rune1].Center, Main.projectile[rune1].Center);
                        DrawDustBeetweenThisAndThat(Main.projectile[rune2].Center, Main.projectile[rune1].Center);
                        if (Main.rand.NextBool(2))
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                int chosenDust = Main.rand.NextBool(2) ? 173 : 157;
                                Vector2 offset = new Vector2();
                                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                                offset.X += (float)(Math.Sin(angle) * 17f);
                                offset.Y += (float)(Math.Cos(angle) * 17f);
                                Dust dust = Main.dust[Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 88) + offset - new Vector2(2, 4), 0, 0, chosenDust, 0, 0, 0, new Color(), 0.95f)];
                                dust.velocity = Vector2.Zero;
                                dust.fadeIn = 0.2f;
                                dust.noGravity = true;
                                dust.noLight = false;
                            }
                        }
                    }
                }
            }
            else
                NPC.aiStyle = 3;
        }
        #region dustywusty
        public static void DrawDustBeetweenThisAndThat(Vector2 vector3, Vector2 vector1)
        {
            Vector2 range = vector3 - vector1;
            if (Main.rand.NextBool(12))
            {
                for (int i = 0; i < 8; i++)
                {
                    int chosenDust = Main.rand.NextBool(2) ? 173 : 157;
                    Dust dust = Main.dust[Dust.NewDust(vector1 + range * Main.rand.NextFloat() + Vector2.Zero, 0, 0, chosenDust)];
                    dust.noGravity = true;
                    dust.noLight = false;
                    dust.velocity = range * 0.001f;
                    dust.scale = 1.24f;
                }
            }
        }
        #endregion

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 7; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f, -2.5f, 0, default, 1.2f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f, -2.5f, 0, default, 0.5f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f, -2.5f, 0, default, 0.7f);
            }
        }
    }
}
