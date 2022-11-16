using Malignant.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Weapon.Corruption.Warlock
{
    public class BoeyrProjectile : ChargedBowProjectile
    {
        public override (Vector2, Vector2) StringTexturePositions => (new Vector2(3, 9), new Vector2(3, 50));
        public override int ShootProjectileType => ModContent.ProjectileType<BoeyrSkullProjectile>();
        public override int StringThickness => 2;
        public override void Charge(Projectile[] projectiles)
        {
            Vector2 rotVector = Projectile.rotation.ToRotationVector2();
            projectiles[0].Center = StringPoint(0.5f) + rotVector * 34;
            projectiles[0].velocity = rotVector;
        }
    }
}
