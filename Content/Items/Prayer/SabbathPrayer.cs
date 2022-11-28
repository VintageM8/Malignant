﻿using Malignant.Common;
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
        public override int Cooldown => 80;

        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {
            int i = 0;
            float spread = 10f * 0.0174f;
            double startAngle = Math.Atan2(6, 6) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<HolyWind>(), 10, 0f, player.whoAmI);
        }

        public override IEnumerator OnUseAbilityRoutine(Player player, EntitySource_PrayerAbility source)
        {
            // Write "fart" every frame for 60 frames
            for (int i = 0; i < 60; i++)
            {
                Main.NewText("fart");
                yield return null;
            }
        }
    }
}
