using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Malignant.Content.Items.Weapon.Crimson.Arterion.StaveofCarnem
{
    public class StaffofCarnem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stave of Carnem");
            Tooltip.SetDefault("Shoots a spinning hunk of flesh that homes in on The Wretched\nRight Click to summon a holy crimatic hex around those who defy Our Lord");
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
            Item.shoot = ModContent.ProjectileType<CarnemProj>();
            Item.shootSpeed = 10f;
            Item.knockBack = 6f;
            Item.DamageType = DamageClass.Magic;
            Item.value = Item.sellPrice(gold: 1, silver: 75);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -1; i < 2; i++)
            {
                if (i == 0)
                    continue;
                Projectile.NewProjectile(source, position, /*Utils.RotatedBy(velocity, (double)(MathHelper.ToRadians(16f) * (float)i))*/velocity, type, damage, knockback, player.whoAmI, i);
            }
            return false;
        }

    }
}
