using Malignant.Common.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Crimson.Abowmanation
{
    public class AbowmanationHeldProjectile : ChargedBowProjectile
    {
        public override (Vector2, Vector2) StringTexturePositions => (new Vector2(11, 14), new Vector2(11, 69));
        public override int ShootProjectileType => ModContent.ProjectileType<AbowmanationArrow>();
        public override int StringThickness => 2;
        public override Color StringColor => Color.OrangeRed * 0.8f;
        public override int ChargeFramesMax => 90;
        public override void Charge()
        {
            Vector2 rotVector = Projectile.rotation.ToRotationVector2();
            arrow.Center = StringPoint(0.5f) + rotVector * 34;
            arrow.velocity = rotVector;

            arrow.netUpdate = true;
        }
    }
}
