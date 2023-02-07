using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Malignant.Common.CustomSwingStyle;

namespace Malignant.Content.Items.Misc.HolyGreatsword
{
    public class HolyGreatsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Holy Greatsword");
            Tooltip.SetDefault("This sword swords"); //10/10 tooltip
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Melee;
            Item.width = Item.height = 90;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = CustomUsestyleID.SwingVersionTwo;
            Item.knockBack = 4;
            Item.value = 10000;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            //Item.noMelee = true;
            //Item.shoot = ModContent.ProjectileType<HolyGSProj>();
            //Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.LightPurple;
        }
    }
}
