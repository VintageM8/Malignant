using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Tiles;

namespace Malignant.Content.Items
{
    public class AlterTest : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Alter Test");
        }


        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 14;

            Item.maxStack = 999;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;

            Item.createTile = ModContent.TileType<CocytusAlter>();
        }
    }
}
