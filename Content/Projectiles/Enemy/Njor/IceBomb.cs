using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Malignant.Content.Projectiles.Enemy.Njor
{
	public class IceBomb : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 181;
			Projectile.hostile = true;
			DrawOffsetX = -30;
			DrawOriginOffsetY = -20;
		}

        public override void Kill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

			for (int i = 0; i < 20; i++)
			{
				Dust dust;
				dust = Main.dust[Terraria.Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 10, 10, DustID.BlueCrystalShard, 0f, 0f, 0, new Color(255, 0, 201), 1f)];
				dust.noGravity = true;
			}
		}

        public override void AI()
		{
			Projectile.ai[1] += 1;

			if (Projectile.ai[1] > 0 && Projectile.ai[1] < 30)
			{
				Projectile.scale += 0.01f;
			}
			if (Projectile.ai[1] > 30 && Projectile.ai[1] < 60)
			{
				Projectile.scale -= 0.01f;
			}
			if (Projectile.ai[1] > 60 && Projectile.ai[1] < 90)
			{
				Projectile.scale += 0.01f;
			}
			if (Projectile.ai[1] > 90 && Projectile.ai[1] < 120)
			{
				Projectile.scale -= 0.01f;
			}
			if (Projectile.ai[1] > 120 && Projectile.ai[1] < 150)
			{
				Projectile.scale += 0.01f;
			}
			if (Projectile.ai[1] > 150 && Projectile.ai[1] < 180)
			{
				Projectile.scale -= 0.01f;
			}

			if (Projectile.ai[1] == 180)
			{
				Projectile.NewProjectile(null, Projectile.position, new Vector2(-8, 0), ModContent.ProjectileType<GenericSpike>(), 20, 2);
				Projectile.NewProjectile(null, Projectile.position, new Vector2(8, 0), ModContent.ProjectileType<GenericSpike>(), 20, 2);
				Projectile.NewProjectile(null, Projectile.position, new Vector2(-5, 5), ModContent.ProjectileType<GenericSpike>(), 20, 2);
				Projectile.NewProjectile(null, Projectile.position, new Vector2(5, 5), ModContent.ProjectileType<GenericSpike>(), 20, 2);
				Projectile.NewProjectile(null, Projectile.position, new Vector2(-5, -5), ModContent.ProjectileType<GenericSpike>(), 20, 2);
				Projectile.NewProjectile(null, Projectile.position, new Vector2(5, -5), ModContent.ProjectileType<GenericSpike>(), 20, 2);
				Projectile.NewProjectile(null, Projectile.position, new Vector2(0, 8), ModContent.ProjectileType<GenericSpike>(), 20, 2);
				Projectile.NewProjectile(null, Projectile.position, new Vector2(0, -8), ModContent.ProjectileType<GenericSpike>(), 20, 2);
				Projectile.Kill();
			}
		}
	}
}
