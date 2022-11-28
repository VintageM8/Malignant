using Malignant.Common;
using Malignant.Content.Projectiles.Prayer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer
{
    public class PaladinPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(PaladinPrayer), "PrayerTest");
        public override int AbilityType => PrayerContent.AbilityType<PaladinAbility>();
    }

    public class PaladinAbility : PrayerAbility
    {
        public override string Texture => base.Texture.Replace(nameof(PaladinAbility), "PrayerTest");

        protected override int OnUseAbility(Player player)
        {
            Projectile.NewProjectile(player.GetSource_Misc("Paladin"), Main.MouseWorld + new Vector2(0, -50), new Vector2(0, 0), ModContent.ProjectileType<WindsofGod>(), 10, 0f, player.whoAmI);
            return 10;
        }
    }
}
