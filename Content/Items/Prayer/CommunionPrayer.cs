using Malignant.Common;
using Malignant.Content.Dusts;
using Malignant.Common.Helper;
using Malignant.Common.Systems;
using Terraria.Audio;
using System.Collections;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer
{
    public class CommunionPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(CommunionPrayer), "PrayerTest");
        public override string AbilityType => PrayerContent.AbilityType<CommunionAbility>();

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Gives Wellfed, Tipsy, and Dryad buff\nClears potion cooldown");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.consumable = true;
        }
    }

    public class CommunionAbility : PrayerAbility
    {
        public override string TexturePath => base.TexturePath.Replace(nameof(CommunionAbility), "PrayerTest");
        public override string DisplayName => "Communion";
        public override int Cooldown => 1320;
        public override Alignment Alignment => Alignment.Holy;

        public override SoundStyle SwapSound => SoundManager.Sounds["prayer"];
        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {

            MethodHelper.DrawCircle(player.Center, ModContent.DustType<PrayerUse>(), 3, 4, 4, 2, 3, nogravity: true);
            player.AddBuff(BuffID.WellFed2, 660);
            player.AddBuff(BuffID.Tipsy, 660);
            player.AddBuff(BuffID.DryadsWard, 660);
        }

        public override IEnumerator OnUseAbilityRoutine(Player player, EntitySource_PrayerAbility source)
        {
            for (int i = 0; i < 1; i++)
            {
                Main.NewText("Prayer Cooldown Over");
                yield return null;
            }

        }
    }
}
