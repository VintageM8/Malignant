using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Malignant.Content.Items.Weapon.Corruption.Warlock.StaffofFlame
{
    public class CursedFireballStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stave of Cursed Flame");
            Tooltip.SetDefault("HEHEHA\nRight Click to do more HEHEHA");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Blue;
            Item.mana = 16;
            Item.UseSound = SoundID.Item21;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 45;
            Item.channel = true;
            Item.autoReuse = true;
            Item.useAnimation = 20;
            Item.useTime = 12;
            Item.width = 50;
            Item.height = 56;
            Item.shoot = ModContent.ProjectileType<CFStaffProj>();
            Item.shootSpeed = 10f;
            Item.knockBack = 6f;
            Item.DamageType = DamageClass.Magic;
            Item.value = Item.sellPrice(gold: 1, silver: 75);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            for (int i = 0; i < 3; i++)
            {
                // The target for the projectile to move towards
                Vector2 target = Main.MouseWorld;
                position += Vector2.Normalize(velocity);
                float speed = (float)(3.0 + (double)Main.rand.NextFloat() * 6.0);
                Vector2 start = Vector2.UnitY.RotatedByRandom(6.32);
                Projectile.NewProjectile(source, position.X, position.Y, start.X * speed, start.Y * speed, type, damage, knockBack, player.whoAmI, target.X, target.Y);
            }

            return false;
        }

        public override bool CanUseItem(Player Player)
        {
            if (Player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Shoot;               
                Item.mana = 15;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.mana = 5;
            }
            return base.CanUseItem(Player);
        }

        public override bool AltFunctionUse(Player Player)
        {
            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Vector2 dir = Vector2.Normalize(velocity) * 9;
                velocity = dir;
                type = ModContent.ProjectileType<CursedFB>();
            }
        }
    }
}
