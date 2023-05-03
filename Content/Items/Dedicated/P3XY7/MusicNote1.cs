using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Malignant.Common.Helper;
using Malignant.Core;
using System.Threading;

namespace Malignant.Content.Items.Dedicated.P3XY7
{
    public class MusicNote1 : ModProjectile
    {
        public int timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 520;
            Projectile.scale = 1.3f;
        }
        public override void AI()
        {
            if (initialized)
            {
                
                Projectile.scale *= (1 + 0.00098095238095238095238095238095f);
                if (Projectile.timeLeft % 30 == 0)
                {
                    //  SoundStyle Riff1 = new SoundStyle("Malignant/Assets/SFX/Riff1");
                    // SoundEngine.PlaySound(Riff1, Projectile.Center);
                    Projectile.damage += 4;
                }
                float centerX = Projectile.Center.X;
                float centerY = Projectile.Center.Y;
                float minDist = 720f;
                bool chasing = false;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                    {
                        float centerX2 = Main.npc[i].position.X + (float)(Main.npc[i].width / 2);
                        float centerY2 = Main.npc[i].position.Y + (float)(Main.npc[i].height / 2);
                        float dist = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - centerX2) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - centerY2);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            centerX = centerX2;
                            centerY = centerY2;
                            chasing = true;
                        }
                    }
                }
                if (chasing)
                {
                    float idealVelocity = 45f;
                    Vector2 vector = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                    float xDist = centerX - vector.X;
                    float yDist = centerY - vector.Y;
                    float distNorm = (float)Math.Sqrt((double)(xDist * xDist + yDist * yDist));
                    distNorm = idealVelocity / distNorm;
                    xDist *= distNorm;
                    yDist *= distNorm;
                    Projectile.velocity.X = (Projectile.velocity.X * 20f + xDist) / 21f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 20f + yDist) / 21f;
                }
            }
            else
            {
                timer++;
                Projectile.SineWave(timer, 19f, 3f, true);
                Projectile.velocity *= 0.98f;
                if (timer == 60)
                {
                    initialized = true;
                }

            }
        }
        public Trail trail;
        public Trail trail2;
        private bool initialized;

        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D trailTexture = ModContent.Request<Texture2D>("Malignant/Assets/Textures/Trails/Stretched").Value;

            if (trail == null)
            {
                trail = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(40f), (p) => Projectile.GetAlpha(new Color(215, 0, 64, 100)));
                trail.drawOffset = Projectile.Size / 2f;
            }
            if (trail2 == null)
            {
                trail2 = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(15f), (p) => Projectile.GetAlpha(new Color(255, 255, 255, 100)));
                trail2.drawOffset = Projectile.Size / 2f;
            }

            trail.Draw(Projectile.oldPos);
            trail2.Draw(Projectile.oldPos);
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;

        }
    }
    public class MusicNote2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override string Texture => "Malignant/Content/Items/Dedicated/P3XY7/MusicNote1";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 520;
            Projectile.scale = 1.3f;
        }
        public override void AI()
        {
            Projectile.scale *= (1 + 0.00098095238095238095238095238095f);
            if (Projectile.timeLeft % 30 == 0)
            {
               // SoundStyle Riff1 = new SoundStyle("Malignant/Assets/SFX/Riff1");
               // SoundEngine.PlaySound(Riff1, Projectile.Center);
                Projectile.damage += 4;
            }
            float centerX = Projectile.Center.X;
            float centerY = Projectile.Center.Y;
            float minDist = 720f;
            bool chasing = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                {
                    float centerX2 = Main.npc[i].position.X + (float)(Main.npc[i].width / 2);
                    float centerY2 = Main.npc[i].position.Y + (float)(Main.npc[i].height / 2);
                    float dist = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - centerX2) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - centerY2);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        centerX = centerX2;
                        centerY = centerY2;
                        chasing = true;
                    }
                }
            }
            if (chasing)
            {
                float idealVelocity = 45f;
                Vector2 vector = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float xDist = centerX - vector.X;
                float yDist = centerY - vector.Y;
                float distNorm = (float)Math.Sqrt((double)(xDist * xDist + yDist * yDist));
                distNorm = idealVelocity / distNorm;
                xDist *= distNorm;
                yDist *= distNorm;
                Projectile.velocity.X = (Projectile.velocity.X * 20f + xDist) / 21f;
                Projectile.velocity.Y = (Projectile.velocity.Y * 20f + yDist) / 21f;
            }

        }
        public Trail trail;
        public Trail trail2;
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D trailTexture = ModContent.Request<Texture2D>("Malignant/Assets/Textures/Trails/Stretched").Value;

            if (trail == null)
            {
                trail = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(40f), (p) => Projectile.GetAlpha(new Color(215, 0, 64, 100)));
                trail.drawOffset = Projectile.Size / 2f;
            }
            if (trail2 == null)
            {
                trail2 = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(15f), (p) => Projectile.GetAlpha(new Color(255, 255, 255, 100)));
                trail2.drawOffset = Projectile.Size / 2f;
            }

            trail.Draw(Projectile.oldPos);
            trail2.Draw(Projectile.oldPos);
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;

        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GemRuby, Main.rand.NextVector2Circular(1f, 1f) * 10, 0, default, 1f);
                dust.noGravity = true;
            }
        }
    }
    public class MusicNote3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 520;
            Projectile.scale = 1.3f;
        }
        public override void AI()
        {
            Projectile.scale *= (1 + 0.00098095238095238095238095238095f);
            if (Projectile.timeLeft % 30 == 0)
            {
                //SoundStyle Riff1 = new SoundStyle("Malignant/Assets/SFX/Riff1");
               // SoundEngine.PlaySound(Riff1, Projectile.Center);
                Projectile.damage += 4;
            }
            float centerX = Projectile.Center.X;
            float centerY = Projectile.Center.Y;
            float minDist = 720f;
            bool chasing = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                {
                    float centerX2 = Main.npc[i].position.X + (float)(Main.npc[i].width / 2);
                    float centerY2 = Main.npc[i].position.Y + (float)(Main.npc[i].height / 2);
                    float dist = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - centerX2) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - centerY2);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        centerX = centerX2;
                        centerY = centerY2;
                        chasing = true;
                    }
                }
            }
            if (chasing)
            {
                float idealVelocity = 45f;
                Vector2 vector = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float xDist = centerX - vector.X;
                float yDist = centerY - vector.Y;
                float distNorm = (float)Math.Sqrt((double)(xDist * xDist + yDist * yDist));
                distNorm = idealVelocity / distNorm;
                xDist *= distNorm;
                yDist *= distNorm;
                Projectile.velocity.X = (Projectile.velocity.X * 20f + xDist) / 21f;
                Projectile.velocity.Y = (Projectile.velocity.Y * 20f + yDist) / 21f;
            }
        }
        public Trail trail;
        public Trail trail2;
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D trailTexture = ModContent.Request<Texture2D>("Malignant/Assets/Textures/Trails/Stretched").Value;

            if (trail == null)
            {
                trail = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(40f), (p) => Projectile.GetAlpha(new Color(215, 0, 64, 100)));
                trail.drawOffset = Projectile.Size / 2f;
            }
            if (trail2 == null)
            {
                trail2 = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(15f), (p) => Projectile.GetAlpha(new Color(255, 255, 255, 100)));
                trail2.drawOffset = Projectile.Size / 2f;
            }

            trail.Draw(Projectile.oldPos);
            trail2.Draw(Projectile.oldPos);
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;

        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GemRuby, Main.rand.NextVector2Circular(1f, 1f) * 10, 0, default, 1f);
                dust.noGravity = true;
            }
        }
    }
}
