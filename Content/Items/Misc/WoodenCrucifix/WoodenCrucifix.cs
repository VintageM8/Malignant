using Malignant.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc.WoodenCrucifix
{
    public class WoodenCrucifix : ModItem
    {

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 15, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Melee) += 0.02f;
            player.GetDamage(DamageClass.Ranged) += 0.02f;
            player.GetDamage(DamageClass.Magic) += 0.02f;
            player.GetDamage(DamageClass.Summon) += 0.02f;

            player.GetModPlayer<MalignantPlayer>().WoodenCross = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.Wood, 28)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
