using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Malignant.Content.Items.Accessories.Expert.Moniter
{
    public class Blood : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Blood");
            // Main.projFrames[base.Projectile.type] = 1; Not needed
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            base.Projectile.localAI[1] += 1f;
            if (base.Projectile.localAI[1] > 10f && Main.rand.NextBool(3))
            {
                int num = 6;
                for (int i = 0; i < num; i++)
                {
                    Vector2 value = (Vector2.Normalize(base.Projectile.velocity) * new Vector2(base.Projectile.width, base.Projectile.height) / 2f).RotatedBy((double)(i - (num / 2 - 1)) * Math.PI / (double)num) + base.Projectile.Center;
                    Vector2 value2 = (Main.rand.NextFloat() * (float)Math.PI - (float)Math.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
                    int num2 = Dust.NewDust(value + value2, 0, 0, 217, value2.X * 2f, value2.Y * 2f, 100, default(Color), 1.4f);
                    Dust obj = Main.dust[num2];
                    obj.noGravity = true;
                    obj.noLight = true;
                    obj.velocity /= 4f;
                    obj.velocity -= base.Projectile.velocity;
                }
                base.Projectile.alpha -= 5;
                if (base.Projectile.alpha < 50)
                {
                    base.Projectile.alpha = 50;
                }
                base.Projectile.rotation += base.Projectile.velocity.X * 0.1f;
                base.Projectile.frame = (int)(base.Projectile.localAI[1] / 3f) % 3;
                Lighting.AddLight((int)base.Projectile.Center.X / 16, (int)base.Projectile.Center.Y / 16, 0.1f, 0.4f, 0.6f);
            }
            int num3 = -1;
            Vector2 vector = base.Projectile.Center;
            float num4 = 500f;
            Vector2 value3 = new Vector2(0.5f);
            if (base.Projectile.localAI[0] == 0f && base.Projectile.ai[0] == 0f)
            {
                base.Projectile.localAI[0] = 30f;
            }
            bool flag = false;
            if (base.Projectile.ai[0] != 0f)
            {
                int num6 = (int)(base.Projectile.ai[0] - 1f);
                if (Main.npc[num6].active && !Main.npc[num6].dontTakeDamage && Main.npc[num6].immune[base.Projectile.owner] == 0)
                {
                    if (Math.Abs(base.Projectile.Center.X - Main.npc[num6].Center.X) + Math.Abs(base.Projectile.Center.Y - Main.npc[num6].Center.Y) < 1000f)
                    {
                        flag = true;
                        vector = Main.npc[num6].Center;
                    }
                }
                else
                {
                    base.Projectile.ai[0] = 0f;
                    flag = false;
                    base.Projectile.netUpdate = true;
                }
            }
            if (flag)
            {
                double num7 = (double)(vector - base.Projectile.Center).ToRotation() - (double)base.Projectile.velocity.ToRotation();
                if (num7 > Math.PI)
                {
                    num7 -= Math.PI * 2.0;
                }
                if (num7 < -Math.PI)
                {
                    num7 += Math.PI * 2.0;
                }
                base.Projectile.velocity = base.Projectile.velocity.RotatedBy(num7 * 0.1);
            }
            float num8 = base.Projectile.velocity.Length();
            base.Projectile.velocity.Normalize();
            base.Projectile.velocity = base.Projectile.velocity * (num8 + 0.0025f);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, base.Projectile.position);
            for (int i = 0; i < 5; i++)
            {
                int num = Dust.NewDust(base.Projectile.position + base.Projectile.velocity, base.Projectile.width, base.Projectile.height, 34);
                Main.dust[num].velocity *= 0f;
                Main.dust[num].noGravity = true;
            }
        }
    }
}
