using Malignant.Common;
using Malignant.Common.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer
{
    public class HolySpeedPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(HolySpeedPrayer), "PrayerTest");
        public override string AbilityType => PrayerContent.AbilityType<HolySpeedAbility>();
    }

    public class HolySpeedAbility : PrayerAbility
    {
        public override string DisplayName => "Speed of the Holy";
        public override int Cooldown => 660; // also temporary, TODO balance
        
        public override IEnumerator OnUseAbilityRoutine(Player player, EntitySource_PrayerAbility source)
        {
            // temporary sounds
            SoundEngine.PlaySound(SoundManager.Sounds["UP_1"] with { Volume = 3.5f }, player.Center);

            for (int i = 0; i < 600; i++)
            {
                player.GetAttackSpeed(DamageClass.Generic) += 1f;
                yield return null;
            }

            SoundEngine.PlaySound(SoundManager.Sounds["END_1"] with { Volume = 3 }, player.Center);
        }
    }
}
