using Malignant.Common.Systems;
using Malignant.Content.Dusts;
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
            Dust.NewDust(player.position - Microsoft.Xna.Framework.Vector2.UnitX * player.width * 0.25f, (int)(player.width * 1.5f), player.height, DustID.GoldCoin, Scale: Main.rand.NextFloat(0.8f, 1.2f));
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
