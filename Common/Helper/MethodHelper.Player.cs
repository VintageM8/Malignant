using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Common.Helper
{
    public partial class MethodHelper
    {
        public static Vector2 ShoulderPosition(this Player player)
        {
            return player.RotatedRelativePoint(player.MountedCenter) + new Vector2(-4 * player.direction, -2);
        }
    }
}
