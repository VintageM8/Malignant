using Terraria;
using Terraria.Audio;
using Malignant.Common;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Content.Items.Misc;

namespace Malignant.Content.Items.Consumeable.Summons
{
    public class ArterionSpawn : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pierced Heart");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 42;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = false;
        }
        /*public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Arterion>()) && Main.dayTime;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.ScaryScream, player.position);

                int type = ModContent.NPCType<Arterion>();

                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                else
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
            }
            return true;
        }*/

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddTile(TileID.DemonAltar)
                .AddIngredient(ItemID.Vertebrae, 15)
                .AddIngredient(ItemID.TissueSample, 8)
                .AddIngredient(ItemID.Ectoplasm, 10)
                .Register();
        }

    }
}
