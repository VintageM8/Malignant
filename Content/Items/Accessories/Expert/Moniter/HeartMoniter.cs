using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common;

namespace Malignant.Content.Items.Accessories.Moniter
{
    public class HeartMoniter : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart Moniter");
            Tooltip.SetDefault("Increases max health by 75\nReleases a rune that shoots homing blood clots");
        }

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
            player.GetModPlayer<MalignantPlayer>().Moniter = true;
            player.statLifeMax2 += 75;
        }
    }
}
