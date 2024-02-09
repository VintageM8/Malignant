using Malignant.Common;
using Malignant.Content.Projectiles.Prayer;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using Malignant.Content.Buffs;
using Malignant.Common.Systems;

namespace Malignant.Content.Items.Prayer.ChivalrousMirror
{
    public class ChivalrousMirrorPrayer : PrayerItem
    {
        public override string Texture => "Malignant/Content/Items/Prayer/FireballAbility";
        public override string AbilityType => PrayerContent.AbilityType<ChivalrousMirrorAbility>(); public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.consumable = true;
        }
    }

    public class ChivalrousMirrorAbility : PrayerAbility
    {
        public override string TexturePath => "Malignant/Content/Items/Prayer/FireballAbility";
        public override string DisplayName => "Chivalrous Mirror";
        public override int Cooldown => 120;
        public override SoundStyle SwapSound => SoundManager.Sounds["prayer"];
        public override Alignment Alignment => Alignment.Holy;

        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Main.MouseWorld.DirectionTo(player.Center) * 10, ModContent.ProjectileType<ChivalrousMirror>(), 45, 0f, player.whoAmI);
        }



        public override IEnumerator OnUseAbilityRoutine(Player player, EntitySource_PrayerAbility source)
        {
            SoundEngine.PlaySound(SoundManager.Sounds["UP_1"] with { Volume = 3.5f }, player.Center);

            for (int i = 0; i < 120; i++)
            {
                Main.NewText("Prayer Cooldown Over");
                yield return null;
            }
        }
    }
}
