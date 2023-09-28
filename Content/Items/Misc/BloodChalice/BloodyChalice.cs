using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Malignant.Content.Items.Holy.Titania;
using Malignant.Content.Projectiles.Prayer;
using Mono.Cecil;
using Terraria.DataStructures;
using Malignant.Content.Items.Crimson.FleshBlazer;
using System;
using Terraria.GameContent;

namespace Malignant.Content.Items.Misc.BloodChalice
{
    public class BloodyChalice : ModItem
    {
        public override void SetStaticDefaults()
        {

            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.width = 40;
            Item.height = 40;
            Item.mana = 3;
            Item.useTime = 2;
            Item.DamageType = DamageClass.Generic;
            Item.useAnimation = 2;
            Item.useStyle = 5;
            Item.knockBack = 10;
            Item.value = 1000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item8;
            Item.channel = true;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.shoot = ProjectileID.BloodArrow;
            Item.shootSpeed = 14;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            {
                Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<HolyRune>(), Item.damage - 13, Item.knockBack / 2, player.whoAmI);

                Projectile.NewProjectile(source, player.Center, player.Center.DirectionTo(Main.MouseWorld) * 21, ModContent.ProjectileType<HeartThing>(), Item.damage - 13, 45, player.whoAmI);
                return false;
            }
        }

    }
}
