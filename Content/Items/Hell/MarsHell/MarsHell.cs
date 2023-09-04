using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static Terraria.ModLoader.ModContent;
using Malignant.Common.Players;
using Malignant.Common.Projectiles;
using Malignant.Content.Items.Crimson.FleshBlazer;

namespace Malignant.Content.Items.Hell.MarsHell
{
    public class MarsHell : HeldGunModItem
    {
        private int cooldown = 0;
        public override (float centerYOffset, float muzzleOffset, Vector2 drawOrigin, Vector2 recoil) HeldProjectileData => (5, 35, new Vector2(11, 11), new Vector2(8, 0.7f));

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            //DisplayName.SetDefault("Mars Hell");
            //Tooltip.SetDefault("Shoots out a bomb every 4 shots\n" +
               // "\n<right> to launch a volly of smitful crosses, 1 minute cooldown");

        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.crit = 0;
            Item.damage = 20;
            Item.useAnimation = 28;
            Item.useTime = 28;
            Item.noMelee = true;
            Item.autoReuse = false;
            //Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item36;
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 15f;
            Item.noUseGraphic = true;
            Item.shoot = ProjectileID.Bullet;
            Item.channel = true;
        }

        public override void HoldItem(Player Player)
        {
            cooldown--;
        }

        private int shotCount;
        public override void ShootGun(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                cooldown = 130;
            }

            for (int i = 0; i < 6; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15f));
                newVelocity *= 1f + Main.rand.NextFloat(0.3f);

                Projectile.NewProjectileDirect(
                    source,
                    position,
                    newVelocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI
                ).netUpdate = true;
            }
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {

            if (player.altFunctionUse == 2)
            {
                Vector2 dir = Vector2.Normalize(velocity) * 9;
                velocity = dir;
                for (int i = 0; i < 5; i++)
                {
                    type = ModContent.ProjectileType<ScourcherBible>();
                }
            }

        }

        public override bool CanUseItem(Player Player)
        {
            if (Player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.useTime = 45;
                Item.useAnimation = 45;
                Item.shootSpeed = 12f;

                if (cooldown > 0)
                    return false;
            }
            else
            {
                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.shootSpeed = 5f;
            }

            return base.CanUseItem(Player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        public override bool AltFunctionUse(Player Player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.HellstoneBar, 22)
                .AddIngredient(ItemID.GoldBar, 15)
                .Register();

            CreateRecipe(1)
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.HellstoneBar, 22)
                .AddIngredient(ItemID.PlatinumBar, 15)
                .Register();
        }
    }
}