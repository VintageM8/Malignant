using Malignant.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Content.Projectiles;
using System;
using Malignant.Content.Buffs;

namespace Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles
{
    public class PlayerTele : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(3);
            AIType = 3;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            //Projectile.rotation += 1f;
            Lighting.AddLight(Projectile.Center, Projectile.Opacity * 0.8f, Projectile.Opacity * 0.2f, Projectile.Opacity * 0.2f);

            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
            if (Projectile.localAI[0] == 0)
            {
                AdjustMagnitude(ref Projectile.velocity);
            }
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] < 60)
            {
                Vector2 move = Vector2.Zero;
                float distance = 600;
                bool targetted = false;
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player target = Main.player[p];
                    if (!target.active || target.dead || target.invis || !Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                        continue;

                    Vector2 newMove = target.Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        targetted = true;
                    }
                }

                if (targetted)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
            }

            for (int p = 0; p < Main.maxPlayers; p++)
            {
                Player player = Main.player[p];

                player.AddBuff(ModContent.BuffType<NoMove>(), 10);
            }
        }

        private static void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f)
            {
                vector *= 12f / magnitude;
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width / 2), (int)(Projectile.position.Y - Projectile.height / 2), Projectile.width * 2, Projectile.height * 2);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 8)
            {
                Vector2 circularLocation = new Vector2(-20, 0).RotatedBy(MathHelper.ToRadians(i));
                int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.Blood);
                Main.dust[num1].noGravity = true;
                Main.dust[num1].scale = 2.25f;
                Main.dust[num1].velocity = circularLocation * 0.35f;
            }
        }
    }
}