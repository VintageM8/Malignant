using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Malignant.Common;
using Malignant.Content.PrayerAbilities;

namespace Malignant.Content.Items
{ 
    public class PrayerTest : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.maxStack = 1;
            Item.noMelee = true;
            Item.consumable = true;
            Item.autoReuse = false;
        }
        public override bool? UseItem(Player player)
        {
            PrayerSystem p = player.GetModPlayer<PrayerSystem>();
            if (p.currentAbility is SabbathAbility)
                return false;

            CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 50, 50), new Color(0, 200, 0), "E A SPORTS");
            p.currentAbility = new SabbathAbility();

            return true;
        }
    }
}
