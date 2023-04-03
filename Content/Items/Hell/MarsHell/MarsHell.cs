using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Malignant.Common.Players;
using Malignant.Common.Projectiles;

namespace Malignant.Content.Items.Hell.MarsHell
{
    public class MarsHell : HeldGunModItem
    {
        public override (float centerYOffset, float muzzleOffset, Vector2 drawOrigin, Vector2 recoil) HeldProjectileData => (5, 35, new Vector2(11, 11), new Vector2(8, 0.7f));

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Mars Hell");
            Tooltip.SetDefault("Shoots out a gernade\n" +
                "Gernade gets stronger overtime");
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.crit = 0;
            Item.damage = 23;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.noMelee = true;
            Item.autoReuse = true;
            //Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item36;
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 10f;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileID.Bullet;
            Item.channel = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.GetModPlayer<MalignantPlayer>().itemCombo >= 0)
            {
                type = ModContent.ProjectileType<Gernade1>();
                damage = 23;
                Item.useTime = 50;
            }
            if (player.GetModPlayer<MalignantPlayer>().itemCombo >= 10)
            {
                type = ModContent.ProjectileType<Gernade2>();
                damage = 36;
                Item.useTime = 38;
            }
            if (player.GetModPlayer<MalignantPlayer>().itemCombo >= 10)
            {
                type = ModContent.ProjectileType<Gernade3>();
                damage = 48;
                Item.crit = 4;
            }

        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }
    }
}