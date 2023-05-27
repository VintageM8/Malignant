using Malignant.Content.NPCs.Corruption.Warlock;
using Terraria;
using Terraria.Audio;
using Malignant.Common;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Consumeable.Summons
{
    public class CursedTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cursed Skull");
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
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Warlock>()) && Main.dayTime;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                DustHelper.DrawCircle(player.Center, DustID.ChlorophyteWeapon, 2, 4, 4, 1, 2, nogravity: true);

                int type = ModContent.NPCType<Warlock>();

                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddTile(TileID.DemonAltar)
                .AddIngredient(ItemID.RottenChunk, 15)
                .AddIngredient(ItemID.CursedFlame, 18)
                .Register();
        }

    }
}
