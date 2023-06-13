using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Malignant.Content.Items.Misc
{
    internal class FruitOfTheGarden : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 29;
            Item.height = 27;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 4, 12, 10);
            Item.consumable = true;
            Item.useTurn = true;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.buffTime = 1;
            Item.buffType = BuffID.WellFed3;
        }

        public const int MAX_LIFE_PER_BOSS = 50;

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<FruitOfTheGardenModPlayer>().TryConsumeFruit();
        }
    }

    internal class FruitOfTheGardenModPlayer : ModPlayer
    {
        public HashSet<int> KilledBosses { get; private set; } = new HashSet<int>();
        private int FruitOfGardenAddedLife { get; set; } = 0;
        public bool ConsumedFruit => FruitOfGardenAddedLife > 0;

        public bool TryConsumeFruit()
        {
            if (KilledBosses.Count == 0)
            {
                return false;
            }

            FruitOfGardenAddedLife += KilledBosses.Count * FruitOfTheGarden.MAX_LIFE_PER_BOSS;
            KilledBosses.Clear();

            return true;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["KilledBosses"] = KilledBosses.ToArray();
            tag["FruitOfGardenAddedLife"] = FruitOfGardenAddedLife;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("KilledBosses"))
            {
                KilledBosses.UnionWith((int[])tag["KilledBosses"]);
            }

            if (tag.ContainsKey("FruitOfGardenAddedLife"))
            {
                FruitOfGardenAddedLife = (int)tag["FruitOfGardenAddedLife"];
            }
        }

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            health = StatModifier.Default with { Base = FruitOfGardenAddedLife };
            mana = StatModifier.Default;
        }
    }

    internal class FruitOfTheGardenGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (npc.boss)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.LocalPlayer.GetModPlayer<FruitOfTheGardenModPlayer>().KilledBosses.Add(npc.type);
                    return;
                }

                // SERVER
                foreach (Player player in Main.player)
                {
                    if (player is null || !player.TryGetModPlayer(out FruitOfTheGardenModPlayer FOTGPlayer))
                    {
                        continue;
                    }

                    FOTGPlayer.KilledBosses.Add(npc.type);
                }
            }
        }
    }
}
