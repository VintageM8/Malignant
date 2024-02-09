using Malignant.Common;
using Malignant.Content.Dusts;
using Malignant.Common.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Terraria.Audio;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Prayer.FangedVengance
{
    public class FangedVengance : PrayerItem
    {
        public override string Texture => "Malignant/Content/Items/Prayer/FireballAbility";
        public override string AbilityType => PrayerContent.AbilityType<FangedVenganceAbility>();

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.consumable = true;
        }
    }

    public class FangedVenganceAbility : PrayerAbility
    {
        public override string TexturePath => "Malignant/Content/Items/Prayer/FireballAbility";
        public override string DisplayName => "Fanged Vengance";
        public override int Cooldown => 1;

        public override SoundStyle SwapSound => SoundManager.Sounds["prayer"];

        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(source, player.Center, player.Center.DirectionTo(Main.MouseWorld) * 21, ModContent.ProjectileType<HomingFang>(), 45, 0f, player.whoAmI);
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