using Terraria;
using Terraria.Audio;
using Malignant.Common;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Content.Items.Misc;
using Malignant.Content.NPCs.Norse.Njor;

namespace Malignant.Content.Items.Consumeable.Summons
{
    public class FrostedBeacon : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 44;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ZoneSnow == true)
            {
                return !NPC.AnyNPCs(ModContent.NPCType<Njor>());
            }
            else
            {
                return false;
            }
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = ModContent.NPCType<Njor>();

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
               .AddTile(TileID.DemonAltar)
               .AddIngredient(ItemID.IceBlock, 35)
               .Register();
        }
    }
}