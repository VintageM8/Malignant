using Malignant.Common.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc.AngelofLight
{
    public class LightBowHold : ChargedBowProjectile
    {
        public override (Vector2, Vector2) StringTexturePositions => (new Vector2(3, 9), new Vector2(3, 50));
        public override int ShootProjectileType => ModContent.ProjectileType<ArrowofLight>();
        public override int StringThickness => 1;

        public override int ChargeFramesMax => 70;
        public override void Charge()
        {
            Vector2 rotVector = Projectile.rotation.ToRotationVector2();
            arrow.Center = StringPoint(0.5f) + rotVector * 34;
            arrow.velocity = rotVector;

            arrow.netUpdate = true;
        }
    }
}
