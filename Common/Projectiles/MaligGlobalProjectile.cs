using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Malignant.Content.Items.Prayer.ChivalrousMirror;

namespace Malignant.Common.Projectiles
{
    class MaligGlobalProjectile : GlobalProjectile
    {
        //Old code from Trinitarian, still works
        public bool Cloned = false;
        public override bool InstancePerEntity => true;


        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            MaligGlobalProjectile globalprojectile = projectile.GetGlobalProjectile<MaligGlobalProjectile>();
            Projectile Mirror = Main.projectile[0];
            bool isClose = false;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<ChivalrousMirror>())
                {
                    Mirror = Main.projectile[i];
                }
            }
            if (Mirror.type == ModContent.ProjectileType<ChivalrousMirror>())
            {
                for (int i = -5; i < 6; i++)
                {
                    int timer = (int)Mirror.ai[0];
                    Vector2 target = projectile.Center + new Vector2(-10 * i * (float)Math.Sin((2 * Math.PI) / 360 * timer), 10 * i * (float)Math.Cos((2 * Math.PI) / 360 * timer));
                    if ((target - Mirror.Center).LengthSquared() < 20 * 20)
                    {
                        isClose = true;
                    }
                }
                if (isClose && globalprojectile.Cloned == false)
                {
                    Projectile temp = Projectile.NewProjectileDirect(projectile.GetSource_FromAI("global"), projectile.position, projectile.velocity.RotatedBy(Math.PI / 12), projectile.type, 1, 1, player.whoAmI, 0, 0);
                    projectile.velocity.RotatedBy(-Math.PI / 12);
                    MaligGlobalProjectile globalprojectileClone = temp.GetGlobalProjectile<MaligGlobalProjectile>();
                    globalprojectileClone.Cloned = true;
                    globalprojectile.Cloned = true;
                }
            }
        }

    }
}
