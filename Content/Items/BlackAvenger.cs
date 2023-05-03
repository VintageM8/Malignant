using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items
{
    internal class BlackAvenger : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 36;

            Item.damage = 50;
            Item.knockBack = 3f;
            Item.crit = 12;

            Item.useTime = 35;
            Item.useAnimation = 35;

            Item.rare = 3;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(gold: 1);
            Item.DamageType = DamageClass.Ranged;

            Item.noMelee = true;
            Item.noUseGraphic = false;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                type = ProjectileID.StickyGrenade;
            }
        }
    }
    class BlackAvengerPlayer : ModPlayer
    {
        int ReloadCount = 0;
        public override void PostUpdate()
        {
            if(Player.ItemAnimationJustStarted)
            {
                ++ReloadCount;
            }
        }
    }
    class StickyExplosionProjectile : ModProjectile
    {
        public override string Texture => "Malignant/Common/GhostHitBox";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 0;
            Projectile.hide = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
    }
}
