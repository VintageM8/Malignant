using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Malignant.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace Malignant.Content.Items.Crimson.Arterion.BurstingArtery
{
    public class BurstingArtyProj_Two : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BurstingArtyProj_Two");
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;

            Projectile.frame = Main.rand.Next(3);
        }

        public override void AI()
        {
            Projectile.rotation += 0.25f * Projectile.direction;
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 20)
                Projectile.velocity.Y += 0.94f;

            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector2 vel = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(Main.rand.NextFloat(-15f, 15f))) * Main.rand.NextFloat(-0.25f, -0.35f);

                Dust.NewDustPerfect(Projectile.Center + new Vector2(0f, 28f), DustID.CrimsonTorch, vel, 0, new Color(255, 255, 60) * 0.8f, 0.95f);

                Dust.NewDustPerfect(Projectile.Center, DustID.CrimsonTorch, vel * 1.2f, 0, new Color(150, 80, 40), Main.rand.NextFloat(0.2f, 0.4f));
            }
            SoundEngine.PlaySound(SoundID.NPCHit4.WithPitchOffset(Main.rand.NextFloat(-0.1f, 0.1f)).WithVolumeScale(0.5f), Projectile.position);
        }
    }
}
