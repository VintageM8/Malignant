using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Malignant.Content.Items.Corruption.DepravedBlastBeat
{
    public class DepravedBlastBeat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Depraved Blast Beater");
            Tooltip.SetDefault("Every 3rd shot a cross orbits you\nOnce served the wrectched...now it slays them.");
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
            Item.shootSpeed = 12f;
            //Item.useAmmo = AmmoID.Bullet;
            Item.channel = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        private int CastCount;
        public override void HoldItem(Player player)
        {
            if (!player.channel)
                CastCount = 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            float numberProjectiles = 3;
            float rotation = MathHelper.ToRadians(20);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }

            CastCount++;
            if (CastCount >= 4)
            {
                SoundEngine.PlaySound(SoundID.Splash, player.position);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Cross>(), damage, knockback, player.whoAmI);
                CastCount = 0;
            }
            return false;
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