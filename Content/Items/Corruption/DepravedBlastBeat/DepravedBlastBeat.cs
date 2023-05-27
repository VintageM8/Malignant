using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Malignant.Common.Projectiles;
using Malignant.Common.Players;

namespace Malignant.Content.Items.Corruption.DepravedBlastBeat
{
    public class DepravedBlastBeat : HeldGunModItem
    {
        public override (float centerYOffset, float muzzleOffset, Vector2 drawOrigin, Vector2 recoil) HeldProjectileData => (6, 30, new Vector2(10, 12), new Vector2(5, 0.4f));
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Depraved Blast Beater");
            //Tooltip.SetDefault("Every 3rd shot a cross orbits you\nOnce served the wrectched...now it slays them.");
        }
        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 20;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DepravedBlast_Proj>();
            Item.shootSpeed = 20f;
            //Item.useAmmo = AmmoID.Bullet;
            Item.noUseGraphic = true;
            Item.channel = true;
            
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.GetModPlayer<MalignantPlayer>().itemCombo >= 0)
            {
                type = ModContent.ProjectileType<DepravedBlast_Proj>();
                damage = 16;
            }
            if (player.GetModPlayer<MalignantPlayer>().itemCombo >= 3)
            {
                type = ModContent.ProjectileType<DepravedBlast_Proj2>();
                damage = 24;
                Item.shootSpeed = 25f;
                Item.crit = 4;
            }

        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddTile(TileID.MythrilAnvil)
                .AddIngredient(ItemID.HallowedBar, 8)
                .AddIngredient(ItemID.DemoniteBar, 25)
                .AddIngredient(ItemID.RottenChunk, 5)
                .Register();
        }

    }
}