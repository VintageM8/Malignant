using Malignant.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc.AngelofLight
{
    public class AngelofLight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angel of Light");
        }

        public override void SetDefaults()
        {
            Item.crit = 4;
            Item.damage = 68;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 58;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LightBowHold>();
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Arrow;
            Item.noUseGraphic = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = Item.shoot;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<LightBowHold>()] <= 0;
        }
    }
}
