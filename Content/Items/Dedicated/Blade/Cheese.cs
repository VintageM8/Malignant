using Terraria;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Dedicated.Blade
{
    public class Cheese : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cheese");
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
