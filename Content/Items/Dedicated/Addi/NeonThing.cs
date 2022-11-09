using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace Malignant.Content.Items.Dedicated.Addi
{
    public class NeonThing : ModProjectile
    {
        private NPC target;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 400;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Confetti_Green, newColor: Color.LightGreen, Scale: 0.5f)].noGravity = true;
            }

            if (target == null || !target.active || !target.chaseable || target.dontTakeDamage || (target.Center - Projectile.Center).Length() > 2000)
            {
                float distance = 2000f;
                bool isTarget = false;
                int targetID = -1;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && Main.npc[k].chaseable)
                    {
                        Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            targetID = k;
                            distance = distanceTo;
                            isTarget = true;
                        }
                    }
                }

                if (isTarget)
                {
                    target = Main.npc[targetID];
                }
                else
                {
                    target = null;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 4;

            if (target != null)
            {
                Vector2 a = target.Center - Projectile.Center + target.velocity / 0.2f;
                if (a.Length() > 1) { a.Normalize(); }
                a *= 0.2f;
                Projectile.velocity += a;
            }

            Projectile.velocity *= 1.01f;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Confetti_Green, newColor: Color.LightGreen, Scale: 1f)].noGravity = true;
            }
        }
    }
}
