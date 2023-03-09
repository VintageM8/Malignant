using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using System.Collections.Generic;
using Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles;

namespace Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles
{
    public class HealSpikes : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.alpha = 255;

            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
        }

        public bool collidedPlatform = false;

        public int collideTime = 0;
        public int choice = 0;

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(null, new Vector2(Projectile.Center.X, Projectile.Center.Y + 8), new Vector2(0f, 0f), ModContent.ProjectileType<HealSpike_2>(), 25, 10);
        }

        public override void AI()
        {
            choice = Main.rand.Next(3);

            Projectile.velocity.Y += 0.05f;

            int a = Dust.NewDust(Projectile.Center, 2, 2, DustID.BlueFairy);
            Main.dust[a].noGravity = true;
            Main.dust[a].noLight = true;

            int b = Dust.NewDust(Projectile.Center, 2, 2, DustID.BlueCrystalShard);
            Main.dust[b].noGravity = true;
            Main.dust[b].noLight = true;

            if (collidedPlatform == true)
            {
                collideTime += 1;

                if (collideTime >= 30)
                {
                    collidedPlatform = false;
                    collideTime = 0;
                }
            }

            if (collidedPlatform == false)
            {
                int x = (int)Projectile.position.X / 16;
                int y = (int)Projectile.position.Y / 16;

                if (Main.tile[x, y].TileType == TileID.Platforms)
                {
                    Projectile.NewProjectile(null, new Vector2(Projectile.Center.X, Projectile.Center.Y + 2), new Vector2(0f, 0f), ModContent.ProjectileType<HealSpike_2>(), 25, 10);

                    collidedPlatform = true;
                }
            }

            Lighting.AddLight(Projectile.position, TorchID.Ice);
        }
    }
    public class HealSpike_2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.alpha = 255;

            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.tileCollide = true;
        }

        public int choice = 0;

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item101, Projectile.position);

            if (choice == 0)
            {
                Projectile.NewProjectile(null, new Vector2(Projectile.Center.X, Projectile.Center.Y - 97), new Vector2(0f, 0f), ModContent.ProjectileType<Spike>(), 25, 10);
            }
            if (choice == 1)
            {
                Projectile.NewProjectile(null, new Vector2(Projectile.Center.X, Projectile.Center.Y - 97), new Vector2(0f, 0f), ModContent.ProjectileType<Spike>(), 25, 10);
            }
            if (choice == 2)
            {
                Projectile.NewProjectile(null, new Vector2(Projectile.Center.X, Projectile.Center.Y - 79), new Vector2(0f, 0f), ModContent.ProjectileType<Spike>(), 25, 10);
            }
            SoundEngine.PlaySound(SoundID.Item101, Projectile.position);

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(new Vector2(Projectile.BottomLeft.X, Projectile.position.Y), 5, 1, 185, Main.rand.NextFloat(-1, 0), -2);
                Dust.NewDust(new Vector2(Projectile.BottomLeft.X, Projectile.position.Y), 5, 1, 185, Main.rand.NextFloat(0, 1), -2);
                Dust.NewDust(new Vector2(Projectile.BottomLeft.X, Projectile.position.Y), 5, 1, DustID.BlueFairy, Main.rand.NextFloat(-1, 0), -2);
                Dust.NewDust(new Vector2(Projectile.BottomLeft.X, Projectile.position.Y), 5, 1, DustID.BlueFairy, Main.rand.NextFloat(0, 1), -2);
                Dust.NewDust(new Vector2(Projectile.BottomLeft.X, Projectile.position.Y), 5, 1, DustID.IceTorch, Main.rand.NextFloat(-1, 0), -2);
                Dust.NewDust(new Vector2(Projectile.BottomLeft.X, Projectile.position.Y), 5, 1, DustID.IceTorch, Main.rand.NextFloat(0, 1), -2);
            }
        }

        public override void AI()
        {
            choice = Main.rand.Next(3);

            Projectile.ai[1] += 1;

            if (Projectile.ai[1] >= 60)
            {
                Projectile.Kill();
            }

            Lighting.AddLight(Projectile.position, TorchID.Ice);
        }
    }
    public class HealingSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 214;

            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.timeLeft = 180;
        }

        public bool flip = false;
        public bool tryFlip = false;
        public bool fading = false;
        public bool animate = true;


        public override bool CanHitPlayer(Player target)
        {
            if (fading == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override void AI()
        {
            if (animate == true)
            {
                int frameSpeed = 2;
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= frameSpeed)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                    if (Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        Projectile.frame = 0;
                    }
                }
            }

            if (Projectile.frame == 4)
            {
                animate = false;
            }

            if (animate == false)
            {
                Projectile.frame = 4;
            }

            if (Main.rand.NextFloat() < .5f && tryFlip == false)
            {
                flip = true;
                tryFlip = true;
            }
            else
            {
                tryFlip = true;
            }

            if (flip == true)
            {
                Projectile.spriteDirection = -1;
            }

            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;


            Lighting.AddLight(Projectile.position, TorchID.Ice);

            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha += 5;
                fading = true;
            }
        }

    }
}
