using Malignant.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Corruption.Warlock.NightsisterBlade
{
    public class NightsisterBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nightsister's Blade");
            Tooltip.SetDefault("Summons blades that orbit around you and inflict venom\nForged from the fury of the night.");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.crit = 8;
            Item.width = 66;
            Item.height = 66;
            Item.useTime = 20;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 20; 
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item117;
            Item.shoot = ModContent.ProjectileType<NightsisterBladeProjectile>();
            Item.shootSpeed = 1f;
            Item.channel = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddIngredient(ItemID.DarkShard, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool CanUseItem(Player player)
        {
            

            return true;
        }
    }
}