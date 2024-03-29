﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Players;

namespace Malignant.Content.Items.Snow.Cocytus.NjorStaff
{
    public class NjorEnergy : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ProjectileID.LostSoulFriendly;
        public override bool? CanDamage() => false;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 900;
        }

        public override void AI()
        {
            Player parent = Main.player[(int)Projectile.ai[0]];

            Projectile.position = Vector2.Lerp(Projectile.position, parent.Center, Projectile.ai[1]++ / 60f);

            for (int num1103 = 0; num1103 < 2; num1103++)
            {
                int num1106 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Snow, Projectile.velocity.X, Projectile.velocity.Y, 50, default(Color), 0.4f);
                switch (num1103)
                {
                    case 0:
                        Main.dust[num1106].position = (Main.dust[num1106].position + Projectile.Center * 5f) / 6f;
                        break;
                    case 1:
                        Main.dust[num1106].position = (Main.dust[num1106].position + (Projectile.Center + Projectile.velocity / 2f) * 5f) / 6f;
                        break;
                }
                Dust dust81 = Main.dust[num1106];
                dust81.velocity *= 0.1f;
                Main.dust[num1106].noGravity = true;
                Main.dust[num1106].fadeIn = 1f;
            }

            if (Projectile.getRect().Intersects(parent.getRect()))
            {
                SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact);
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Snow, Projectile.velocity.X, Projectile.velocity.Y, 50, default(Color), 0.4f);

                parent.GetModPlayer<MalignantPlayer>().BuildCount++;

                Projectile.Kill();
            }
        }
    }
}
