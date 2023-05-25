using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Malignant.Content.Buffs;
using Microsoft.Xna.Framework;

namespace Malignant.Content.Items.Misc
{
    public class Head : ModItem
    {

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Head");
            //Tooltip.SetDefault("Eat and gain a small power boost. \nIf you eat it you have problems");
        }

        public override void SetDefaults()
        {
            Item.DefaultToFood(22, 22, BuffID.WellFed3, 57600); 
            Item.value = Item.buyPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.White;
        }

        public override void OnConsumeItem(Player player)
        {
            int healAmount = (int)MathHelper.Min(player.statLifeMax2 - player.statLife, 1);
            player.HealEffect(1);
            player.statLife += healAmount;
            

            player.AddBuff(BuffID.WellFed, 18000);
            player.AddBuff(ModContent.BuffType<SicklyPower>(), 600);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, player.position);
           
        }
    }
}