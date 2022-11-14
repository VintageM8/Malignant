using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Malignant.Content.Projectiles.Enemy.Njor
{
	public class GenericSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spike");
		}

		public override void SetDefaults()
		{
			Projectile.width = 15;
			Projectile.height = 15;
			Projectile.aiStyle = 0;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
			Projectile.scale = 0.75f;
			Projectile.hostile = true;
			DrawOffsetX = -28;
			DrawOriginOffsetY = 0;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

			for (int i = 0; i < 20; i++)
			{
				Dust dust;
				dust = Main.dust[Terraria.Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 5, 5, DustID.BlueCrystalShard, 0f, 0f, 0, new Color(255, 0, 201), 1f)];
				dust.noGravity = true;
			}
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
	}
}
