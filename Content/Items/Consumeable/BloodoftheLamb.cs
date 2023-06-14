using Malignant.Content.Items.Spider.SpiderNeckless;
using Mono.Cecil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Consumeable
{
    public class BloodoftheLamb : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.rare = ItemRarityID.LightRed;
            Item.healLife = 150; // While we change the actual healing value in GetHealLife, item.healLife still needs to be higher than 0 for the item to be considered a healing item
            Item.potion = true; // Makes it so this item applies potion sickness on use and allows it to be used with quick heal
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }
        
    }
}
