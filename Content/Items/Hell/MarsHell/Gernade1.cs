using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common;
using Malignant.Common.Systems;

namespace Malignant.Content.Items.Hell.MarsHell
{
    public class Gernade1 : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.aiStyle = 2;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }


        public override void AI()
        {
            float progress = 1 - (Projectile.timeLeft / 150f);
            for (int i = 0; i < 3; i++)
            {
                Dust sparks = Dust.NewDustPerfect(Projectile.Center + (Projectile.rotation.ToRotationVector2()) * 17, DustID.InfernoFork, (Projectile.rotation + Main.rand.NextFloat(-0.6f, 0.6f)).ToRotationVector2() * Main.rand.NextFloat(0.4f, 1.2f));
                sparks.fadeIn = progress * 45;

            }
        }

        public override void Kill(int timeLeft)
        {
            CameraSystem.ScreenShakeAmount = 2.5f;

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustID.InfernoFork);
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = 70 + Main.rand.Next(60);
                dust.rotation = Main.rand.NextFloat(6.28f);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustID.InfernoFork);
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = Main.rand.Next(80) + 40;
                dust.rotation = Main.rand.NextFloat(6.28f);

                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(25, 25), DustID.InfernoFork).scale = 0.9f;
            }

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            player.GetModPlayer<MalignantPlayer>().itemCombo++;
            player.GetModPlayer<MalignantPlayer>().itemComboReset = 480;
        }
    }
}
