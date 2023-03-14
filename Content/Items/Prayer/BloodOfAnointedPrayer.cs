using IL.Terraria.GameContent;
using Malignant.Common;
using Malignant.Common.Systems;
using Malignant.Content.Dusts;
using System;
using System.Collections;
using System.Collections.Generic;
using Terraria.Audio;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer
{
    public class BloodOfAnointedPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(BloodOfAnointedPrayer), "PrayerTest");
        public override string AbilityType => PrayerContent.AbilityType<BloodOfAnointedAbility>();

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Tempararly increases life regen and max life by 50");
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.consumable = true;
        }
    }

    public class BloodOfAnointedAbility : PrayerAbility
    {
        public override string DisplayName => "Blood of Anointed";
        public override int Cooldown => 2400;
        public override SoundStyle SwapSound => SoundManager.Sounds["prayer"];
        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {

            // player.statLifeMax2 += 50;
            player.lifeRegen += 2;
            Dust.NewDust(player.position - Microsoft.Xna.Framework.Vector2.UnitX * player.width * 0.25f, (int)(player.width * 1.5f), player.height, ModContent.DustType<HealingDust>(), Scale: Main.rand.NextFloat(0.8f, 1.2f));

            float maxAddLife = player.statLife + (player.statLifeMax - player.statLife) * 0.5f;
            for (int i = 0; i < 999 && player.statLife < maxAddLife && player.statLife > 0; i++)
            {
                player.statLife++;
                Dust.NewDust(player.position - Microsoft.Xna.Framework.Vector2.UnitX * player.width * 0.25f, (int)(player.width * 1.5f), player.height, ModContent.DustType<HealingDust>(), Scale: Main.rand.NextFloat(0.8f, 1.2f));

            }

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
