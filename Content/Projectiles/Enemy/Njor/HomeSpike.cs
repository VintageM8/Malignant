using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Projectiles.Enemy.Njor
{
	public class HomeSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Homing Spike");
		}

		public override void SetDefaults()
		{
			Projectile.width = 15;
			Projectile.height = 15;
			Projectile.aiStyle = 0;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
			Projectile.scale = 0.25f;
			Projectile.hostile = true;
			DrawOffsetX = -28;
			DrawOriginOffsetY = 0;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if (Projectile.timeLeft > 140)
			{
				for (int i = 0; i < 3; i++)
				{
					float num4 = Projectile.velocity.X / 3f * (float)i;
					float num5 = Projectile.velocity.Y / 3f * (float)i;
					int num6 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 185, 0f, 0f, 200, default(Color), 1f);
					Main.dust[num6].position.X = Projectile.Center.X - num4;
					Main.dust[num6].position.Y = Projectile.Center.Y - num5;
					Main.dust[num6].noGravity = true;
					Main.dust[num6].velocity *= 0f;
				}
			}

			if (Projectile.timeLeft == 140)
			{
				float num = 30f;
				int num2 = 0;
				while ((float)num2 < num)
				{
					Vector2 vector = Vector2.UnitX * 0f;
					vector += -Utils.RotatedBy(Vector2.UnitY, (double)((float)num2 * (6.28318548f / num)), default(Vector2)) * new Vector2(6f, 16f);
					vector = Utils.RotatedBy(vector, (double)Utils.ToRotation(Projectile.velocity), default(Vector2));
					int num3 = Dust.NewDust(Projectile.Center, 0, 0, 185, 0f, 0f, 0, default(Color), 1.25f);
					Main.dust[num3].noGravity = true;
					Main.dust[num3].position = Projectile.Center + vector;
					Main.dust[num3].velocity = Projectile.velocity * 0f + Utils.SafeNormalize(vector, Vector2.UnitY) * 1f;
					num2++;
				}
			}
			if (Projectile.timeLeft < 140)
			{
				Projectile.alpha = 50;

				for (int i = 0; i < 3; i++)
				{
					float num4 = Projectile.velocity.X / 3f * (float)i;
					float num5 = Projectile.velocity.Y / 3f * (float)i;
					int num6 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 185, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num6].position.X = Projectile.Center.X - num4;
					Main.dust[num6].position.Y = Projectile.Center.Y - num5;
					Main.dust[num6].noGravity = true;
					Main.dust[num6].velocity *= 0f;
				}

				float num7 = Projectile.Center.X;
				float num8 = Projectile.Center.Y;
				float range = 1000f;
				bool flag = false;
				int num13;
				for (int j = 0; j < 200; j = num13 + 1)
				{
					if (Projectile.Distance(Main.player[j].Center) < range && Collision.CanHit(Projectile.Center, 1, 1, Main.player[j].Center, 1, 1))
					{
						float num10 = Main.player[j].position.X + (float)(Main.player[j].width / 2);
						float num11 = Main.player[j].position.Y + (float)(Main.player[j].height / 2);
						float num12 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num10) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num11);
						if (num12 < range)
						{
							range = num12;
							num7 = num10;
							num8 = num11;
							flag = true;
						}
					}
					num13 = j;
				}
				if (flag)
				{
					float speed = 9f;
					Vector2 vector2 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float num15 = num7 - vector2.X;
					float num16 = num8 - vector2.Y;
					float num17 = (float)Math.Sqrt((double)(num15 * num15 + num16 * num16));
					num17 = speed / num17;
					num15 *= num17;
					num16 *= num17;
					Projectile.velocity.X = (Projectile.velocity.X * 20f + num15) / 21f;
					Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num16) / 21f;
				}
			}
		}
	}
}
