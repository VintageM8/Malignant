using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Malignant.Content.Items.Dedicated.P3XY7
{
    public class MusicNote1 : ModProjectile
    {
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
        }
        public override void AI()
        {
            Projectile.scale *= (1 + 0.00098095238095238095238095238095f);
            if (Projectile.timeLeft % 30 == 0)
            {
                SoundStyle Riff1 = new SoundStyle("Malignant/Assets/SFX/Riff1");
                SoundEngine.PlaySound(Riff1, Projectile.Center);
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
    }
    public class MusicNote2 : ModProjectile
    {
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
        }
        public override void AI()
        {
            Projectile.scale *= (1 + 0.00098095238095238095238095238095f);
            if (Projectile.timeLeft % 30 == 0)
            {
                SoundStyle Riff1 = new SoundStyle("Malignant/Assets/SFX/Riff1");
                SoundEngine.PlaySound(Riff1, Projectile.Center);
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
    }
    public class MusicNote3 : ModProjectile
    {
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
        }
        public override void AI()
        {
            Projectile.scale *= (1 + 0.00098095238095238095238095238095f);
            if (Projectile.timeLeft % 30 == 0)
            {
                SoundStyle Riff1 = new SoundStyle("Malignant/Assets/SFX/Riff1");
                SoundEngine.PlaySound(Riff1, Projectile.Center);
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
    }
}
