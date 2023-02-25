using Malignant.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using Malignant.Common.Helper;
using Malignant.Content.Dusts;
using Malignant.Common.Systems;

namespace Malignant.Content.Items.Prayer
{
    public class HolySpeedPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(HolySpeedPrayer), "PrayerTest");
        public override string AbilityType => PrayerContent.AbilityType<HolySpeedAbility>();

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.consumable = true;
        }
    }

    public class HolySpeedAbility : PrayerAbility
    {
        public override string DisplayName => "Speed of the Holy";
        public override int Cooldown => 660; // also temporary, TODO balance
        public override SoundStyle SwapSound => SoundManager.Sounds["prayer"];

        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {
            MethodHelper.DrawCircle(player.Center, ModContent.DustType<PrayerUse>(), 3, 4, 4, 2, 3, nogravity: true);       
        }

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
            for (int i = 0; i < 120; i++)
            {
                Main.NewText("Prayer Cooldown Over");
                yield return null;
            }

        }
    
    }
}
