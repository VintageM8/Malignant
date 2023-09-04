using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Malignant.Content.Dusts;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;
using Malignant.Content.Items.Hell.MarsHell;
using Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles;

namespace Malignant.Content.NPCs.Crimson.Heart.Projectiles
{
    public class BloodBomb : ModProjectile
    {
        public override string Texture => "Malignant/Content/NPCs/Crimson/Heart/Projectiles/BloodBlister";
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = 0;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 181;
            Projectile.hostile = true;
            DrawOffsetX = -30;
            DrawOriginOffsetY = -20;
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
                Projectile.NewProjectile(null, Projectile.position, new Vector2(-8, 0), ProjectileType<SpikeSpawner>(), 20, 2);
                Projectile.NewProjectile(null, Projectile.position, new Vector2(8, 0), ProjectileType<SpikeSpawner>(), 20, 2);
                Projectile.NewProjectile(null, Projectile.position, new Vector2(-5, 5), ProjectileID.BloodNautilusShot, 20, 2);
                Projectile.NewProjectile(null, Projectile.position, new Vector2(5, 5), ProjectileID.BloodNautilusShot, 20, 2);
                Projectile.NewProjectile(null, Projectile.position, new Vector2(-5, -5), ProjectileID.BloodNautilusShot, 20, 2);
                Projectile.NewProjectile(null, Projectile.position, new Vector2(5, -5), ProjectileID.BloodNautilusShot, 20, 2);
                Projectile.NewProjectile(null, Projectile.position, new Vector2(0, 8), ProjectileType<SpikeSpawner>(), 20, 2);
                Projectile.NewProjectile(null, Projectile.position, new Vector2(0, -8), ProjectileType<SpikeSpawner>(), 20, 2);
                Projectile.Kill();
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_KoboldFlyerDeath with { Volume = .3f }, Projectile.position);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CrimsonStuff>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustType<Blood>());
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = 70 + Main.rand.Next(60);
                dust.rotation = Main.rand.NextFloat(6.28f);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustType<Blood>());
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = Main.rand.Next(80) + 40;
                dust.rotation = Main.rand.NextFloat(6.28f);

                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(25, 25), DustType<Blood>()).scale = 0.9f;
            }
        }

    }
}
