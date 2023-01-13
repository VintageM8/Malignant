using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc
{
    public class BrokenDemonHorn : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Demon Horn");
            Tooltip.SetDefault("Be wary of the weapons crafted from this, for they can corrupt any user");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 0, 10);
            Item.maxStack = 999;
        }
    }
}
