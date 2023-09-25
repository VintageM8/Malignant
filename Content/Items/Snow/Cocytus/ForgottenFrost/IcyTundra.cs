using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Malignant.Content.Items.Snow.Cocytus.ForgottenFrost
{
    public class IcyTundra : ModItem
    {
		private int charges;
		public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Winds of Hell");
			//Tooltip.SetDefault("Hold down mouse to build up your attack");
        }

        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 24;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.crit = 4;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
			Item.channel = true;
            Item.shoot = ProjectileID.SnowBallFriendly;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Snowball;
        }

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0);
		}
	}
}