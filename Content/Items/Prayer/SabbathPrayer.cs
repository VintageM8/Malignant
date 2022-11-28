using Malignant.Common;
using Malignant.Content.Projectiles.Prayer;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer
{
    public class SabbathPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(SabbathPrayer), "PrayerTest");
        public override int AbilityType => PrayerContent.AbilityType<SabbathAbility>();
    }

    public class SabbathAbility : PrayerAbility
    {
        public override string Texture => base.Texture.Replace(nameof(SabbathAbility), "PrayerTest");
        protected override int OnUseAbility(Player player)
        {
            int i = 0;
            float spread = 10f * 0.0174f;
            double startAngle = Math.Atan2(6, 6) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

            Projectile.NewProjectile(player.GetSource_Misc("Smite of the Sabbath"), player.Center.X, player.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<HolyWind>(), 10, 0f, player.whoAmI);

            return 60;
        }
    }
}
