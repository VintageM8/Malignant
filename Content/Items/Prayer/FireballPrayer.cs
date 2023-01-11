using Malignant.Common;
using Malignant.Content.Projectiles.Prayer;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer
{
    public class FireballPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(FireballPrayer), "PrayerTest");
        public override string AbilityType => PrayerContent.AbilityType<FireballAbility>();
    }

    public class FireballAbility : PrayerAbility
    {
        public override string DisplayName => "Flames of God";
        public override int Cooldown => 30;

        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {
            Projectile.NewProjectile(source, player.Center, player.Center.DirectionTo(Main.MouseWorld) * 21, ModContent.ProjectileType<WindsofGod>(), 45, 0f, player.whoAmI);
        }
    }
}
