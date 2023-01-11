using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Corruption.Warlock.StaffofFlame
{
    public class CFStaffProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.CrystalLeafShot;

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.width = Projectile.height = 120;
            Projectile.scale = 1f;
            Projectile.alpha = 255;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            Projectile.aiStyle = 0;
            Projectile.timeLeft = 5;
        }
    }
}