using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Malignant.Content.Projectiles.Enemy.Njor
{
	public class SimpleShot : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 3;
			Projectile.height = 3;
            Projectile.alpha = 255;

			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.scale = 1f;
		}

        public override void AI()
        {
			for (int i = 0; i < 10; i++)
			{
				Dust dust;
				dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, 2, 2, DustID.BlueCrystalShard, 0f, 0f, 0, new Color(255, 0, 201), 1f)];
				dust.noGravity = true;
			}

			//ParticleManager.NewParticle<NjorEyeShot>(Projectile.Center, Projectile.velocity, new Color(255, 255, 255), 0.75f, 0, 0);

            Lighting.AddLight(Projectile.position, TorchID.Ice);
        }
    }
}