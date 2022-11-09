using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Projectiles.Enemy.Warlock
{
    public class CursedWave : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 160;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            if (++Projectile.ai[1] % 2 == 0)
            {
                float num116 = 16f;
                for (int num117 = 0; (float)num117 < 16; num117++)
                {
                    Vector2 spinningpoint7 = new Vector2(-20, -2);
                    spinningpoint7 += -Vector2.UnitY.RotatedBy((float)num117 * ((float)Math.PI * 2f / num116)) * new Vector2(1f, 4f);
                    spinningpoint7 = spinningpoint7.RotatedBy(Projectile.velocity.ToRotation());
                    Vector2 position = Projectile.Center;
                    Dust dust = Dust.NewDustPerfect(position, 63, new Vector2(0f, 0f), 0, new Color(0, 242, 255), 1f);
                    dust.noLight = true;
                    dust.noGravity = true;
                    dust.position = Projectile.Center + spinningpoint7;
                }
            }
            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            NPC host = Main.npc[(int)Projectile.ai[0]];
            if (host.life < host.lifeMax - 20)
            {
                int steps = (int)host.Distance(target.Center) / 8;
                for (int i = 0; i < steps; i++)
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(Vector2.Lerp(host.Center, target.Center, (float)i / steps), 2, 2, DustID.LifeDrain);
                        dust.velocity = target.DirectionTo(dust.position) * 2;
                        dust.noGravity = true;
                    }
                }
                host.life += 20;
                host.HealEffect(20);
            }
            Projectile.Kill();
        }



    }
}