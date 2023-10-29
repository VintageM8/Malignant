using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc
{
    public class BlessedMetal : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 25;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
        }

        public override void AddRecipes() //Temporary
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.IronBar, 2)
                .AddTile(TileID.Furnaces)
                .Register();
            CreateRecipe(1)
                .AddTile(TileID.Furnaces)
                .AddIngredient(ItemID.LeadBar, 2)
                .Register();
        }
    }
}
