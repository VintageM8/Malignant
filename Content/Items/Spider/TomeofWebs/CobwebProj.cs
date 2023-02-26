using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Spider.TomeofWebs
{
    public class CobwebProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CobwebProj");
            //Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 16;
            Projectile.aiStyle = 43;
            AIType = 227;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 54;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.025f) / 255f, ((255 - Projectile.alpha) * 0.25f) / 255f, ((255 - Projectile.alpha) * 0.05f) / 255f);
            Projectile.velocity.Y += Projectile.ai[0];
            var vector = Projectile.velocity * 1.08f;
            Projectile.velocity = vector;
        }
    }
}