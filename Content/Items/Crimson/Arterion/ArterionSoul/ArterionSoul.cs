using Malignant.Content.Items.Corruption.DepravedBlastBeat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Crimson.Arterion.ArterionSoul
{
    public class ArterionSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arterion's Soul");
        }

        public override void SetDefaults()
        {
            Item.damage = 78;
            Item.width = 62;
            Item.height = 62;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.RaiseLamp;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 8, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item24;
            Item.shoot = ModContent.ProjectileType<ArtSoulProj>();
            Item.shootSpeed = 10f;
            Item.mana = 28;
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
