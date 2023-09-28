using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Players;
using Malignant.Content.Items.Misc;

namespace Malignant.Content.Items.Holy.HolyGauntlet
{
    public class HolyGauntlet : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holy Gauntlet");
            //Tooltip.SetDefault("Killing unholy enemies replenishes your lifeforce");
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
            player.GetModPlayer<MalignantPlayer>().HolyGauntlet = true;
        }

        public override void AddRecipes() //Will become a Chefron drop when boss is coded in
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.GoldBar, 8)
                .AddIngredient(ModContent.ItemType<BlessedMetal>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(1)
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.PlatinumBar, 6)
                .AddIngredient(ModContent.ItemType<BlessedMetal>(), 6)
                .Register();
        }
    }
}
