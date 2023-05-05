using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Spider.TomeofWebs
{
    public class WebTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Webs");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            Item.damage = 89;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 22;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 8, 15, 99);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CobwebProj>();
            Item.shootSpeed = 22f;
            Item.channel = true;
        }
    }
}