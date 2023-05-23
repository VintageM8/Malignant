using Malignant.Common.Helper;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Projectiles
{
    public class HomingFireball : ModProjectile
    {
        public override string Texture => "Malignant/Content/Projectiles/Fireball";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 45;
            Projectile.height = 45;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.penetrate = 3;
        }


        bool spawnStuff = true;
        Vector2 initialCenter;
        public override void AI()
        {
            if (spawnStuff)
            {
                initialCenter = Projectile.Center;
                spawnStuff = false;
            }

            if (Main.rand.NextBool(10))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.InfernoFork, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

            int dustCount = 80;

            Vector2 direction = initialCenter.DirectionTo(Projectile.Center);
            float interval = initialCenter.Distance(Projectile.Center) / dustCount;

            for (int i = 0; i < dustCount; i++)
            {
                float prog = (float)i / dustCount;
                float curve = MathF.Sin(i * 0.27f + Main.GameUpdateCount * 0.5f) * MathF.Sin(prog * MathHelper.Pi);
                Dust.NewDustDirect(initialCenter + direction * interval * i + direction.RotatedBy(MathHelper.PiOver2) * curve * 25, 0, 0, DustID.InfernoFork, Scale: 1.3f * prog).noGravity = true;
            }

            Vector2 move = Vector2.Zero;
            float distance = 200f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                AdjustMagnitude(ref Projectile.velocity);
            }

        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
            }
        }
        

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void Kill(int timeLeft)
        {

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            MethodHelper.NewDustCircular(Projectile.Center, Projectile.width * 0.1f, i => Main.rand.NextFromList(DustID.Torch, DustID.InfernoFork), 85, minMaxSpeedFromCenter: (8, 10), dustAction: d => d.noGravity = true);
        }

    }
}
