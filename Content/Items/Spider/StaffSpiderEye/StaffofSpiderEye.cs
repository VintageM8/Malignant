using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Malignant.Content.Items.Spider.StaffSpiderEye
{
    public class StaffofSpiderEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Staff of the Spider Eye");
            //Tooltip.SetDefault("Shoots out fangs");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 132;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0;
            Item.value = Item.sellPrice(silver: 460);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SpiderEyeProj>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cobweb, 18)
                .AddIngredient(ItemID.SpiderFang, 22)
                .AddIngredient(ItemID.Ectoplasm, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}