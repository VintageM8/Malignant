using Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles;
using Malignant.Content.Projectiles.Enemy.Warlock;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.NPCs.Crimson.HeartBoss
{
    public class BloodBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Bubble");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ToxicBubble);
            AIType = ProjectileID.ToxicBubble;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 240;
        }
        private float teleAlpha;
        public override void PostAI()
        {
            if (Projectile.timeLeft < 60)
                teleAlpha += 0.016f;
        }
        public override void Kill(int timeLeft)
        {
            Projectile.hostile = true;
            SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
                if (!target.active || target.dead)
                    continue;

                if (Projectile.DistanceSQ(target.Center) > 120 * 120)
                    continue;

                int hitDirection = Projectile.Center.X > target.Center.X ? -1 : 1;
            }
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Scale: 2);
                Main.dust[dustIndex].velocity *= 8f;
            }
        }
    }
}
