using Malignant.Common.Helper;
using Malignant.Content.Items.Hell.MarsHell;
using Microsoft.Xna.Framework;
using ParticleLibrary;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Projectiles.Prayer
{
    public class WindsofGod : ModProjectile
    {
        public override string Texture => "Malignant/Content/Projectiles/Fireball";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 45;
            Projectile.height = 45;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
        }


        bool spawnStuff = true;
        Vector2 initialCenter;
        
        public override void AI()
        {

            for (int i = 0; i < 3; i++)
            {
                Vector2 dir = Main.rand.NextVector2Unit() * 0.1f;
                ParticleManager.NewParticle(Projectile.Center, dir * Main.rand.NextFloat(10, 25), ParticleManager.NewInstance<FireParticle>(), new Color(255f, 69f, 0f, 0), 0.3f, Projectile.whoAmI);
            }

            if (spawnStuff)
            {
                initialCenter = Projectile.Center;
                spawnStuff = false;
            }

            if (Main.rand.NextBool(10))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

            int dustCount = 80;

            Vector2 direction = initialCenter.DirectionTo(Projectile.Center);
            float interval = initialCenter.Distance(Projectile.Center) / dustCount;

            for (int i = 0; i < dustCount; i++)
            {
                float prog = (float)i / dustCount;
                float curve = MathF.Sin(i * 0.27f + Main.GameUpdateCount * 0.5f) * MathF.Sin(prog * MathHelper.Pi );
                Dust.NewDustDirect(initialCenter + direction * interval * i + direction.RotatedBy(MathHelper.PiOver2) * curve * 25, 0, 0, DustID.InfernoFork, Scale: 1.3f * prog).noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MarsHellBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            MethodHelper.NewDustCircular(Projectile.Center, Projectile.width * 0.1f, i => Main.rand.NextFromList(DustID.InfernoFork, DustID.InfernoFork), 85, minMaxSpeedFromCenter: (8, 10), dustAction: d => d.noGravity = true);
        }

    }
}
