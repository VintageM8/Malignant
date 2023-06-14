using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Players;

namespace Malignant.Content.Items.Misc.EvilEye
{
    public class EvilEye : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Expert;
            Item.accessory = true;
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MalignantPlayer>().EvilEye = true;
        }
    }
}
