using Malignant.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Helper;
using Malignant.Content.Projectiles;

namespace Malignant.Content.Items.Prayer.FangedVengance
{
    public class HomingFang : ModProjectile
    {
        public override string Texture => "Malignant/Content/Items/Spider/SpiderNeckless/SpiderFangProjectile";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 100;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.ai[0]++ >= 30 && Projectile.ai[0] <= 240)
            {
                Projectile.velocity *= 0.9f;
                Projectile.rotation.SlowRotation(0, (float)Math.PI / 20);
            }
            else if (Projectile.owner == player.whoAmI)
            {
                if (Projectile.ai[0] < 30) //Moves projectile to the players cursor.
                {
                    Projectile.timeLeft = 100;
                    Projectile.ai[0] = 0;
                    Projectile.Move(Main.MouseWorld, 10, 10);
                    if (Projectile.DistanceSQ(Main.MouseWorld) < 60 * 60)
                        Projectile.ai[0] = 30;
                }
                Projectile.LookByVelocity();
                Projectile.rotation += Projectile.velocity.Length() / 50 * Projectile.spriteDirection;
            }
            if (Projectile.ai[0] == 60 && Main.myPlayer == Projectile.owner)
            {
                Projectile.ai[1] = 10;
                //SoundEngine.PlaySound(SoundID. Projectile.position)
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i=0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustID.Shadowflame);
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = 70 + Main.rand.Next(60);
                dust.rotation = Main.rand.NextFloat(6.28f);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustID.Shadowflame);
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = Main.rand.Next(80) + 40;
                dust.rotation = Main.rand.NextFloat(6.28f);

                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(25, 25), DustID.Shadowflame).scale = 0.9f;
            }
        }
    }
}
