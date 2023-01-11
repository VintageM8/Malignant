using Malignant.Common;
using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer
{
    public class QuickCastPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(QuickCastPrayer), "PrayerTest");
        public override string AbilityType => PrayerContent.AbilityType<QuickCastAbility>();
    }

    public class QuickCastAbility : PrayerAbility
    {
        public override string TexturePath => base.TexturePath.Replace(nameof(QuickCastAbility), "PrayerTest");
        public override string DisplayName => "Quick Cast";
        public override void OnUnload()
        {
            _active = false;
        }

        static bool _active;
        public static bool Active 
        {
            get
            {
                if (_active)
                {
                    _active = false;
                    return true;
                }
                return false;
            }
        }
        public override IEnumerator OnUseAbilityRoutine(Player player, EntitySource_PrayerAbility source)
        {
            _active = true;
            for (int i = 0; i < 600; i++)
            {
                yield return null;
            }
            _active = false;
        }
    }
}
