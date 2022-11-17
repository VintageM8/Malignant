using Malignant.Common;
using Malignant.Content.Projectiles.Prayer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.PrayerAbilities
{
    public class PaladinAbility : IPrayerAbility
    {
        public int CooldownTime => 10;

        public void Use(Player player)
        {
            Projectile.NewProjectile(player.GetSource_Misc("Paladin"), Main.MouseWorld + new Vector2(0, -50), new Vector2(0, 0), ModContent.ProjectileType<WindsofGod>(), 10, 0f, player.whoAmI);
        }
    }
}
