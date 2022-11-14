using Malignant.Core;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common;

namespace Malignant.Content.Items.Dedicated.Addi
{
    public class WackAssProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
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
                if (Projectile.ai[0] < 30)
                {
                    Projectile.timeLeft = 600;
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
                SoundEngine.PlaySound(SoundID.DD2_SkyDragonsFuryShot, Projectile.position);
                for (int i = 0; i < 4; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Utility.PolarVector(2, MathHelper.PiOver2 * i),
                        ModContent.ProjectileType<NeonThing>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
            }
            if (Projectile.ai[0] >= 240)
            {
                Projectile.Move(player.Center, Projectile.ai[1], 1);
                Projectile.ai[1] *= 1.01f;
                if (Projectile.DistanceSQ(player.Center) < 20 * 20)
                    Projectile.Kill();
            }
        }
    }
}
