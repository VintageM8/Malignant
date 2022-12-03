using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using static Terraria.ModLoader.ModContent;
using Malignant.Core;
using Malignant.Common.Systems;
using Malignant.Content.NPCs.Crimson.Heart;

namespace Malignant.Content.NPCs.Crimson.HeartBoss
{
    public class Arterion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arterion");
            //Main.npcFrameCount[Type] = 11;
            NPCID.Sets.TrailCacheLength[Type] = 5;
            NPCID.Sets.ShouldBeCountedAsBoss[Type] = true;
            NPCID.Sets.TrailingMode[Type] = 0;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * (0.5f + bossLifeScale * 0.3f));
        }
        public override void SetDefaults()
        {
            NPC.width = 136;
            NPC.height = 114;
            NPC.lifeMax = 50000;
            NPC.defense = 38;
            NPC.aiStyle = 0;
            NPC.damage = 69;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.noGravity = true;
            NPC.knockBackResist = 0f;
            NPC.noTileCollide = true;
            NPC.boss = true;
        }

        const int Idle = 1;
        const int Intro = 0;
        const int Helix = 1;
        const int Bob = 2;
        public float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        Vector2 center;
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (Main.dayTime)
            {
                AIState = -69420;
                NPC.velocity = new Vector2(0, -20f);
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
            }
            if (AIState == Intro)
            {
                AITimer++;
                /*if (AITimer == 1)
                {
                    CameraSystem.SetBossTitle(160, "Aterion", Color.Gold, "Heart of the Crimson", CameraSystem.BossTitleStyleID.Arterion);
                }*/
                if (AITimer >= 180)
                {
                    AIState = Helix;
                    AITimer = 0;
                }
            }
            //do idle shit later
            else if (AIState == Helix)
            {
                AITimer++;
                switch (AITimer)
                {
                    case 30:
                        
                        AITimer2 = -10;
                        center = player.Center;
                        break;
                    case 100:
                        
                        AITimer2 = -10;
                        center = player.Center;
                        break;
                    case 170:

                        AITimer2 = -10;
                        center = player.Center;
                        break;
                    case 250:

                        AITimer2 = -10;
                        center = player.Center;
                        break;
                }
                if (++AITimer2 >= 5 && AITimer >= 30)
                {
                    AITimer2 = 0;
                    for (int i = -1; i < 2; i++)
                    {
                        if (i == 0)
                            continue;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Utility.FromAToB(NPC.Center, center) * 19f, ProjectileType<BloodBubble>(), 15, 0, player.whoAmI, i);

                    }
                }
                if (AITimer >= 330)
                {
                    AIState = Bob;
                    AITimer = 0;
                }
            }

            else if (AIState == Bob) //seeing if shit works :sparkle: it doesnt :sparkle:
            {
                AITimer++;
                if (AITimer == 1)
                {
                    NPC.ai[3] = 1;
                }
                else if (AITimer == 101)
                {
                    NPC.ai[3] = -1;
                }
                if (AITimer == 100)
                {
                    AITimer2 = 2;
                }
                if (AITimer2 == 0)
                {
                    NPC.velocity = Vector2.Zero;
                    NPC.Center = Vector2.Lerp(NPC.Center, player.Center + Vector2.UnitX * 340 * NPC.ai[3], 0.035f);
                }
                if (AITimer2 == 3)
                {
                    AITimer++;
                    Vector2 rainPos = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y);
                    if (AITimer % 5 == 0)
                    {
                        Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), rainPos, Vector2.UnitY * 10, ModContent.ProjectileType<BloodBlister>(), 25, 1);

                    }
                }
                if (AITimer2 == 1 || AITimer2 == 3)
                    NPC.direction = NPC.velocity.X > 1 ? 1 : -1;
                if (NPC.collideX && AITimer2 == 1)
                {
                    CameraSystem.ScreenShakeAmount = 8;
                    NPC.velocity = Vector2.Zero;
                    AITimer2 = 3;
                }
                if (AITimer2 == 2)
                {
                    NPC.noTileCollide = false;
                    AITimer2 = 1;
                    NPC.damage = 15;
                    Vector2 vector9 = new Vector2(NPC.position.X + (NPC.width * 0.5f), NPC.position.Y + (NPC.height * 0.5f));
                    float rotation2 = (float)Math.Atan2((vector9.Y) - (player.Center.Y), (vector9.X) - (player.Center.X));
                    NPC.velocity.X = (float)(Math.Cos(rotation2) * 65) * -1;
                }

                if (AITimer >= 330)
                {
                    AIState = Intro;
                    AITimer = 0;
                }
            }
        }
    }
}
