using Malignant.Common;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer
{
    public class CommunionPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(CommunionPrayer), "PrayerTest");
        public override string AbilityType => PrayerContent.AbilityType<CommunionAbility>();
    }

    public class CommunionAbility : PrayerAbility
    {
        public override string TexturePath => base.TexturePath.Replace(nameof(CommunionAbility), "PrayerTest");
        public override string DisplayName => "Communion";
        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {
            // TODO: Healing Projectile
        }
    }

    /*
    public class CommunionProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 35;
            Projectile.height = 35;
            Projectile.penetrate = 2;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 60;
        }
    }
    */
}
