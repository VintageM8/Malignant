using Malignant.Content.Items.Weapon.Njor;
using Malignant.Content.Items.Weapon.Njor.NjorStaff;
using Malignant.Content.Items.Weapon.Njor.NjorSword;
using Malignant.Content.NPCs.Norse.Njor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc.Bags
{
    public class IceBossBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {

            int choice = Main.rand.Next(3);
            // Always drops one of:
            if (choice == 0) // 
            {
                player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<IceSword>(), 1);
            }
            else if (choice == 1)
            {
                player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<NjorsStaff>(), 1);
            }
            else if (choice == 2)
            {
                player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<IcyTundra>(), 1);
            }
        }

        public override int BossBagNPC => ModContent.NPCType<Njor>();
    }
}
