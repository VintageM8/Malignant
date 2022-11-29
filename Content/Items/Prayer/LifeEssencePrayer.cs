using Malignant.Common;
using Malignant.Content.Dusts;
using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer
{
    public class LifeEssencePrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(LifeEssencePrayer), "PrayerTest");
        public override string AbilityType => PrayerContent.AbilityType<LifeEssenceAbility>();
    }

    public class LifeEssenceAbility : PrayerAbility
    {
        public override string TexturePath => base.TexturePath.Replace(nameof(LifeEssenceAbility), "PrayerTest");
        public override string DisplayName => "Life Essence";
        public override int Cooldown => 180;
        public override IEnumerator OnUseAbilityRoutine(Player player, EntitySource_PrayerAbility source)
        {
            bool quickCast = QuickCastAbility.Active;

            float maxAddLife = player.statLife + (player.statLifeMax - player.statLife) * 0.5f;
            for (int i = 0; i < 999 && player.statLife < maxAddLife && player.statLife > 0; i++)
            {
                player.statLife++;
                Dust.NewDust(player.position - Microsoft.Xna.Framework.Vector2.UnitX * player.width * 0.25f, (int)(player.width * 1.5f), player.height, ModContent.DustType<HealingDust>(), Scale: Main.rand.NextFloat(0.8f, 1.2f));
                yield return WaitFor.Frames(quickCast ? 2 : 4);
            }
        }
    }
}
