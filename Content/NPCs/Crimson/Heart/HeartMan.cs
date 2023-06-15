using Malignant.Content.NPCs.Crimson.Heart.Projectiles;
using Malignant.Common.Systems;
using Malignant.Content.Items.Crimson.Arterion.MoniterAccessory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles;
using Malignant.Content.Projectiles.Enemy;
using Malignant.Content.Items.Hell.MarsHell;

namespace Malignant.Content.NPCs.Crimson.Heart
{
    public class HeartMan : ModNPC //its not even a man lol
    {
        Vector2 meowBitch;
        bool DrawLinearDash = false;
        Vector2 playeroldcenter;
        Vector2 npcoldcenter;
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
                if (++AITimer2 == 30)
                {
                    Vector2 pos = new Vector2(player.position.X, player.position.Y - 335);
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.18f;

                }
                if (AITimer2 == 30)
                {
                    meowBitch = player.Center;
                    NPC.velocity = Vector2.Zero;
                    Vector2 vector16 = NPC.DirectionTo(player.Center) * 7f;
                    DrawLinearDash = true;
                }
                if (AITimer2 == 90)
                {
                    if (player.direction != NPC.direction)
                    {
                        NPC.velocity = Vector2.Zero;
                        Vector2 vector16 = NPC.DirectionTo(player.Center) * 7f;
                        for (float i = (-9); i <= 9; i++)
                        {

                            Projectile projectile = Main.projectile[Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, 9.5f * Utils.RotatedBy(NPC.DirectionTo(meowBitch), (double)(MathHelper.ToRadians(32.5f) * (float)i), default(Vector2)), ModContent.ProjectileType<BloodBlister>(), 20, 1f, Main.myPlayer)];
                            projectile.tileCollide = false;
                            projectile.friendly = false;
                            projectile.hostile = true;
                            projectile.ai[1] = 0.7f;
                            projectile.timeLeft = 230;
                        }

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
                Vector2 pos = new Vector2(player.position.X, player.position.Y - 100);
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * 0.055f;
                if (AITimer >= 20)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MarsHellBoom>(), 0, 1f, Main.myPlayer);
                }
                if (AITimer >= 30)
                {
                    Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(-2, 5), ModContent.ProjectileType<BloodSpurt>(), Main.rand.Next(10, 20), 5);
                    Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(0, 5), ModContent.ProjectileType<BloodSpurt>(), Main.rand.Next(10, 20), 5);
                    Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(2, 5), ModContent.ProjectileType<BloodSpurt>(), Main.rand.Next(10, 20), 5);
                }
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
                    NPC.damage = 0;
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
                AITimer++;
                Vector2 pos = new Vector2(player.position.X, player.position.Y - 100);
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * 0.085f;
                if (AITimer >= 20)
                {
                    CameraSystem.ScreenShakeAmount = 2.5f;

                    for (int i = 0; i < 3; i++) //Spike Alert
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
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<MarsHellBoom>(), 28, 2.5f, Main.myPlayer, (int)0f);
                            }
                            else
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<MarsHellBoom>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<MarsHellBoom>(), 28, 2.5f, Main.myPlayer, (int)0f);
                            }
                        }
                    }

                }
                if (AITimer >= 120)
                {
                    for (int i = 0; i < 3; i++) //Spikes
                    {
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
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<Spike>(), 28, 2.5f, Main.myPlayer, (int)0f);
                            }
                            else
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<Spike>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<Spike>(), 28, 2.5f, Main.myPlayer, (int)0f);
                            }
                        }
                    }
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
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (DrawLinearDash)
                DrawDashLinear(spriteBatch, npcoldcenter, playeroldcenter, Color.Lerp(Color.White, Color.Red, NPC.ai[2]));
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        #region stuff
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
