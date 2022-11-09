using Terraria;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Dedicated.Blade
{
    public class Patty : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Patty");
        }

        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 29;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
        }
    }
}
