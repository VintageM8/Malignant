﻿using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Malignant.Content.Dusts;
using static Terraria.ModLoader.ModCactus;
using static Terraria.ModLoader.ModContent;

namespace Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles
{
    public class BloodSaw : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = 0;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 500;
            Projectile.hostile = true;
            DrawOffsetX = -30;
            DrawOriginOffsetY = -20;
        }

        public override void AI()
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                dust = Main.dust[Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType<Blood>(), 0f, 0f, 0, new Color(255, 0, 201), 1f)] ;
                dust.noGravity = true;
            }

            Projectile.rotation += 0.25f;

            Projectile.ai[1] += 1;

            if (Projectile.ai[1] > 60 && Projectile.ai[1] < 180)
            {
                Projectile.velocity.X += 0.2f;
            }
            if (Projectile.ai[1] > 180 && Projectile.ai[1] < 300)
            {
                Projectile.velocity.X -= 0.2f;
            }

            if (Projectile.ai[1] > 300)
            {
                Projectile.velocity.Y += 0.18f;
            }
        }
    }
}
