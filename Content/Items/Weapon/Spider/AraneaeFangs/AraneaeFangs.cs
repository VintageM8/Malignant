using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Weapon.Spider.AraneaeFangs
{
    public class AraneaeFangs : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Araneae's Fangs");
            Tooltip.SetDefault("Hold click to charge your attack\nOnce your attack is charged 3 fangs shoot out and stick in The Wretched");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = null;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.reuseDelay = 5;
            Item.width = 28;
            Item.height = 72;
            Item.shoot = ModContent.ProjectileType<AraneaeHoldout>();
            Item.shootSpeed = 10f;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.Cobweb, 18)
                .AddIngredient(ItemID.FlintlockPistol, 1)
                .AddIngredient(ItemID.TissueSample, 8)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(1)
               .AddIngredient(ItemID.Cobweb, 18)
               .AddIngredient(ItemID.FlintlockPistol, 1)
               .AddIngredient(ItemID.ShadowScale, 8)
               .AddTile(TileID.Anvils)
               .Register();
        }
    }
}
