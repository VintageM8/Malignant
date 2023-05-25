using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Content.Items.Misc;

namespace Malignant.Content.Items.Hell
{
    public class DemonShot : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Demon Shot");
            //Tooltip.SetDefault("Shatters on cursor point and releases a forbidden power");
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 18;
            Item.crit = 0;
            Item.ammo = AmmoID.Bullet;
            Item.width = 12;
            Item.height = 16;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.maxStack = 999;
            Item.consumable = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<DemonShotProj>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient(ItemID.MusketBall, 50)
                .AddIngredient(ModContent.ItemType<BrokenDemonHorn>(), 3)
                .AddIngredient(ItemID.HellstoneBar, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
