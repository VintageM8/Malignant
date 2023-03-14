using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Malignant.Common.Projectiles;
using Terraria.Audio;

namespace Malignant.Content.Items.Spider.StaffSpiderEye
{ 
    public class SpiderEyeProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.ignoreWater = false;
            Projectile.width = 30;
            Projectile.penetrate = 1;
            Projectile.height = 30;
            Projectile.friendly = false;
            Projectile.light = 1f;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 440;
        }

        int timer = 16;
        public override void AI()
        {
            Projectile projectile = Main.projectile[Main.myPlayer];
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center + new Vector2(300 * (float)Math.Cos((2 * Math.PI) / 360 * Projectile.ai[0]), 300 * (float)Math.Sin((2 * Math.PI) / 360 * Projectile.ai[0]));
            Projectile.ai[0]++;

            timer--;
            if (timer == 0)
            {
                SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X + Main.rand.Next(-3, 5), Projectile.velocity.Y + Main.rand.Next(-3, 5), ModContent.ProjectileType<SpiderEyeProj_2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                timer = 21;
            }

            /*Projectile.ai[1] += 1f;
            if (Projectile.ai[1] >= 7200f)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }

            }*/
        }
    }
}
