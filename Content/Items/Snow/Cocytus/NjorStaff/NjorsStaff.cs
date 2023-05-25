using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Snow.Cocytus.NjorStaff
{
    public class NjorsStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Scepter of Cocytus");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = null;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.reuseDelay = 5;
            Item.width = 28;
            Item.height = 72;
            Item.shoot = ModContent.ProjectileType<NjorHoldout>();
            Item.shootSpeed = 10f;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(gold: 1);
        }
    }
}