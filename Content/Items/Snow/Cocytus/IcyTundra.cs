using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Malignant.Content.Items.Snow.Cocytus
{
    public class IcyTundra : ModItem
    {
		private int charges;
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Winds of Hell");
			Tooltip.SetDefault("Hold down mouse to build up your attack");
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

		public override void HoldItem(Player player)
		{
			if (!player.channel && charges > 0)
			{
				for (int i = 0; i < charges; i++)
				{
					Projectile shot = Main.projectile[Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.MountedCenter, new Vector2(Item.shootSpeed * Main.rand.NextFloat(0.75f, 1.333f), 0).RotatedBy((Main.MouseWorld - player.MountedCenter).ToRotation()).RotatedByRandom(0.5f), Item.shoot, Item.damage, Item.knockBack, player.whoAmI)];
					shot.maxPenetrate = 1;
					shot.penetrate = 1;
					shot.timeLeft = 600;
				}
				charges = 0;

				SoundEngine.PlaySound(SoundID.Item9, player.Center);
			}
			else if (player.channel && !player.HasAmmo(Item))
			{
				player.itemTime = 10;
				player.itemAnimation = 10;
			}

			if (player.channel)
			{
				player.itemRotation = (Main.MouseWorld - player.MountedCenter).ToRotation();
				if (player.direction == -1) { player.itemRotation += (float)Math.PI; }
			}
		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			return charges < 10;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (charges < 10)
			{
				charges++;
				SoundEngine.PlaySound(SoundID.Item30, player.MountedCenter);

				for (int i = 0; i < 5; i++)
				{
					Vector2 position30 = player.Center;
					int width27 = 0;
					int height27 = 0;
					float speedX13 = player.velocity.X * 0.5f;
					float speedY13 = player.velocity.Y * 0.5f;
					Color newColor = default(Color);
					Dust.NewDust(position30, width27, height27, DustID.SnowBlock, speedX13, speedY13, 51, newColor, 1.2f);
				}
				/*for (int i = 0; i < 3; i++)
				{
					Gore.NewGore(player.GetSource_ItemUse(Item), player.MountedCenter - new Vector2(8, 8), new Vector2(player.velocity.X * 0.2f, player.velocity.Y * 0.2f), Main.rand.Next(16, 18));
				}*/
			}

			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0);
		}
	}
}