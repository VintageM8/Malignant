using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Spider.TomeofWebs
{
    public class WebTomeCenter : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("WebTomeCenter");
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
            var player = Main.player[Projectile.owner];
            if (Main.myPlayer != Projectile.owner)
                return;
            Vector2 vector;
            vector.X = Main.MouseWorld.X - 30f;
            vector.Y = Main.MouseWorld.Y - 30f;
            Projectile.netUpdate = true;
            Projectile.position = vector;
        }

        public override void Kill(int timeLeft)
        {
            var player = Main.player[Projectile.owner];
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y - 75f, 0.0f, 0.2f, ModContent.ProjectileType<CobwebProj>(), (int)(15 * player.GetDamage(DamageClass.Magic).Multiplicative), 3.0f, Projectile.owner, 0.0f, 0.0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X - 75f, Projectile.Center.Y, 0.2f, 0.0f, ModContent.ProjectileType<CobwebProj>(), (int)(15 * player.GetDamage(DamageClass.Magic).Multiplicative), 3.0f, Projectile.owner, 0.0f, 0.0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y + 75f, 0.0f, -0.2f, ModContent.ProjectileType<CobwebProj>(), (int)(15 * player.GetDamage(DamageClass.Magic).Multiplicative), 3.0f, Projectile.owner, 0.0f, 0.0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X + 75f, Projectile.Center.Y, -0.2f, 0.0f, ModContent.ProjectileType<CobwebProj>(), (int)(15 * player.GetDamage(DamageClass.Magic).Multiplicative), 3.0f, Projectile.owner, 0.0f, 0.0f);
        }
    }
}
