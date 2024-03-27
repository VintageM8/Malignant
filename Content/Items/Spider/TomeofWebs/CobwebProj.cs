using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Malignant.Content.Buffs;

namespace Malignant.Content.Items.Spider.TomeofWebs
{
    public class CobwebProj : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 999;
            Projectile.tileCollide = false;
        }
        Vector2 FakeVelocity = Vector2.Zero;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 15f;
                if (Vector2.DistanceSquared(Main.MouseWorld, Projectile.Center) <= 50 * 50)
                    Projectile.ai[0]++;
            }
            if (Projectile.ai[0] == 1)
            {
                FakeVelocity = Projectile.Center - Main.MouseWorld;
                Projectile.ai[0]++;
            }
            if (Projectile.ai[0] == 2)
            {
                Projectile.Center = Main.MouseWorld + FakeVelocity.RotatedBy(MathHelper.ToRadians(++Projectile.ai[1] * 5));
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Webbed>(), 120);

        }

    }
}