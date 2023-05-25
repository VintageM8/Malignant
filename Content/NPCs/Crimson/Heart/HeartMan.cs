using Malignant.Common;
using Malignant.Common.Systems;
using Malignant.Content.Items.Crimson.Arterion.MoniterAccessory;
using Malignant.Content.Projectiles.Enemy.Warlock;
using Malignant.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;


namespace Malignant.Content.NPCs.Crimson.Heart
{
    public class HeartMan : ModNPC //its not even a man lol
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Viscera");
            //Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 102;
            NPC.lifeMax = 9800;
            NPC.defense = 20;
            NPC.damage = 45;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.value = 1200f;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            //NPC.netAlways = true;
            //NPC.chaseable = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Viscera");

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartMoniter>(), 1));

        }

        private const int Intro = 0;
        private const int Dash = 0;
        private const int BloodBurst = 1;
        private const int BloodRain = 2;
        private const int Heal = 3;

        int difficulty;
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

        float alpha = 1;
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (!player.active || player.dead)
                NPC.timeLeft = 2;
            if (NPC.life < 100)
            {
                alpha = Utils.GetLerpValue(0, 100, NPC.life);
            }
            NPC.spriteDirection = NPC.direction;
            bool expertMode = Main.expertMode;

            Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.122f, .5f, .48f);

            if (AIState == Dash)
            {
                //Main.NewText("AI 1");

                NPC.damage = 0;
                for (int i = 0; i < (difficulty > 4 ? 10 : 7); i++)
                {
                    int damage = expertMode ? 0 : 0;
                    int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + Main.rand.Next(-60, 60), NPC.Center.Y + Main.rand.Next(-60, 60), Main.rand.NextFloat(-5.3f, 5.3f), Main.rand.NextFloat(-5.3f, 5.3f), ModContent.ProjectileType<BloodSpurt>(), damage, 1, Main.myPlayer, 0, 0);;
                    Main.projectile[p].scale = Main.rand.NextFloat(.6f, .8f);
                    DustHelper.DrawStar(Main.projectile[p].Center, 272, pointAmount: 6, mainSize: .9425f, dustDensity: 2, dustSize: .5f, pointDepthMult: 0.3f, noGravity: true);

                    if (Main.projectile[p].velocity == Vector2.Zero)
                        Main.projectile[p].velocity = new Vector2(2.25f, 2.25f);

                    if (Main.projectile[p].velocity.X < 2.25f && Math.Sign(Main.projectile[p].velocity.X) == Math.Sign(1) || Main.projectile[p].velocity.X > -2.25f && Math.Sign(Main.projectile[p].velocity.X) == Math.Sign(-1))
                        Main.projectile[p].velocity.X *= 2.15f;

                    Main.projectile[p].netUpdate = true;
                }
                if (++AITimer2 == 30)
                {
                    NPC.Center = player.Center - Vector2.UnitX * player.direction * 145;
                }
                if (AITimer2 == 90)
                {
                    if (player.direction != NPC.direction)
                    {
                        AITimer++;
                        AITimer2 = 0;
                        NPC.ai[3] = 0;
                    }
                    else
                    {
                        Terraria.Audio.SoundStyle ae = new Terraria.Audio.SoundStyle("Malignant/Assets/SFX/HeartbeatFx")
                        {
                            Pitch = -0.5f
                        };
                        Terraria.Audio.SoundEngine.PlaySound(ae);
                    }
                }
                if (AITimer2 >= 205)
                {
                    NPC.damage = 0;
                    AITimer++;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;

                }
                if (AITimer2 > 90)
                {
                    NPC.ai[3] = (float)Math.Sin((double)((AITimer2 - 90) / 115)) * 2;
                }
                if (AITimer == 2)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;
                    AIState = BloodBurst;
                }
            }
            else if (AIState == BloodBurst)
            {
                //Main.NewText("AI 2");
                AITimer++;
                if (player.Center.Distance(NPC.Center) > (16 * 2) && AITimer >= 30)
                    NPC.velocity = Utility.FromAToB(NPC.Center, player.Center, true) * 1.75f;
                else
                    NPC.velocity = Vector2.Zero;

                
                    float rotation = MathHelper.ToRadians(45);
                    Vector2 pos = NPC.Center - Vector2.UnitY * 23;
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 perturbedSpeed = (Utility.FromAToB(NPC.Center, player.Center) * 9.5f).RotatedBy(Main.rand.NextFloat(-rotation, rotation));
                        Vector2 perturbedSpeed1 = (Utility.FromAToB(NPC.Center, player.Center) * 9.5f).RotatedBy(Main.rand.NextFloat(-rotation, rotation));
                        perturbedSpeed1.Normalize();
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, perturbedSpeed, ModContent.ProjectileType<BloodSpurt>(), 15, 1.5f, player.whoAmI);
                    }
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Utility.FromAToB(NPC.Center, player.Center) * 9.5f, ModContent.ProjectileType<BloodySpit>(), 15, 1.5f, player.whoAmI);
                    AITimer2 = 0;
                
                if (AITimer >= 180)
                {
                    AITimer = 0;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;
                    AIState = BloodRain;
                    NPC.velocity = Vector2.Zero;
                }
            }
            else if (AIState == BloodRain)
            {
                //Main.NewText("AI 3");
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
                if (AITimer >= 200)
                {
                    NPC.noTileCollide = true;
                    AIState = Heal;
                    NPC.aiStyle = 0;
                    AITimer = 0;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;
                }
            }
            else if (AIState == Heal)
            {
                //Main.NewText("AI 4");
                AITimer++;
                if (AITimer == 1 || AITimer == 100)
                    
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HealingChunk>(), 0, 0, player.whoAmI, 0, 0);
                    }
                NPC.damage = 0;
                if (AITimer >= 200)
                {
                    AIState = Dash;
                    AITimer = 0;
                    AITimer2 = 0;
                    NPC.ai[3] = 0;
                }
            }

        }

        /*public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter % 6f == 5f)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 6)
            {
                NPC.frame.Y = 0;
            }
        }*/
        private float hitDirection;

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CrimsonTorch, 2.5f * hitDirection, -2.5f, 0, default, .34f);
            }
        }
    }
}
