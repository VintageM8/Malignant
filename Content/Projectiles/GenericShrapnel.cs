using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Malignant.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Malignant.Content.Projectiles
{
    public class GenericShrapnel : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;

            Projectile.frame = Main.rand.Next(3);
        }

        public override void AI()
        {
            Projectile.rotation += 0.25f * Projectile.direction;
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 20)
                Projectile.velocity.Y += 0.94f;

            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;
        }

        public override void Kill(int timeLeft)
        {

        }
    }
}