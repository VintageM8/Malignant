using Malignant.Content.Items.Snow.Cocytus;
using Malignant.Content.Items.Snow.Cocytus.NjorStaff;
using Malignant.Content.Items.Snow.Cocytus.NjorSword;
using Malignant.Content.NPCs.Norse.Njor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Malignant.Content.Items.Misc.Bags
{
    public class IceBossBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag (Cocytus)");
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

        public override void OpenBossBag(Player Player)
        {
            int weapon = Main.rand.Next(3);

            for (int k = 0; k < (Main.masterMode ? 3 : 2); k++)
            {
                switch (weapon % 3)
                {
                    case 0: Player.QuickSpawnItem(Player.GetSource_OpenItem(Item.type), ItemType<IcyTundra>()); break;
                    case 1: Player.QuickSpawnItem(Player.GetSource_OpenItem(Item.type), ItemType<NjorsStaff>()); break;
                    case 2: Player.QuickSpawnItem(Player.GetSource_OpenItem(Item.type), ItemType<IceSword>()); break;
                }

                weapon++;
            }
        }

        public override int BossBagNPC => ModContent.NPCType<Njor>();
    }
}
