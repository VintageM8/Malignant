using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Malignant.Content.Items.Crimson.Arterion.BurstingArtery
{
    public class BurstingArtery : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bursting Artery");
            Tooltip.SetDefault("Shoots a projectile that shatters on cursor");
        }

        public override void SetDefaults()
        {
            Item.damage = 89;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item5;
            Item.width = 32;
            Item.height = 74;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BurstingArt_Held>();
            Item.shootSpeed = 10f;
            Item.noUseGraphic = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = Item.shoot;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<BurstingArt_Held>()] <= 0;
        }
    }
}
