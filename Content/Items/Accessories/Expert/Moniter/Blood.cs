using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Malignant.Content.Items.Accessories.Expert.Moniter
{
    public class Blood : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Blood");
            // Main.projFrames[base.Projectile.type] = 1; Not needed
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

       bool spawnStuff = true;
        Vector2 initialCenter;
        public override void AI()
        {
            if (spawnStuff)
            {
                initialCenter = Projectile.Center;
                spawnStuff = false;
            }

            if (Main.rand.NextBool(10))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CrimsonTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

            int dustCount = 80;

            Vector2 direction = initialCenter.DirectionTo(Projectile.Center);
            float interval = initialCenter.Distance(Projectile.Center) / dustCount;

            for (int i = 0; i < dustCount; i++)
            {
                float prog = (float)i / dustCount;
                float curve = MathF.Sin(i * 0.27f + Main.GameUpdateCount * 0.5f) * MathF.Sin(prog * MathHelper.Pi);
                Dust.NewDustDirect(initialCenter + direction * interval * i + direction.RotatedBy(MathHelper.PiOver2) * curve * 25, 0, 0, DustID.Blood, Scale: 1.3f * prog).noGravity = true;
            }
        }
    }
}
