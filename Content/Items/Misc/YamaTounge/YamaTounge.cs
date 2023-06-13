using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Players;

namespace Malignant.Content.Items.Misc.YamaTounge
{
    public class YamaTounge : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.runAcceleration *= 0.95f;
            player.runSlowdown *= 0.85f;

            player.accRunSpeed += 0.1f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.2f;

        }
    }
}
