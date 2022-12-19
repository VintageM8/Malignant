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
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(position, 1, 1, DustID.Blood, velocity.X / 2, velocity.Y / 2, 0);
            }

            float numberProjectiles = Main.rand.Next(2, 5);
            float rotation = MathHelper.ToRadians(15);
            position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                Projectile.NewProjectile(null, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        /*public override bool CanUseItem(Player Player)
        {
            if (Player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.mana = 16;
            }
            else
            {
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.mana = 16;
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
                type = ModContent.ProjectileType<CarnemHex>();
            }
        }*/
    }
}
