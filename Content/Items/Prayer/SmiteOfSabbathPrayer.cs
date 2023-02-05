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

namespace Malignant.Content.Items.Prayer
{
    public class SmiteOfSabbathPrayer : PrayerItem
    {
        public override string Texture => base.Texture.Replace(nameof(SmiteOfSabbathPrayer), "PrayerTest");
        public override string AbilityType => PrayerContent.AbilityType<SmiteOfSabbathAbility>();
    }

    public class SmiteOfSabbathAbility : PrayerAbility
    {
        public override string TexturePath => base.TexturePath.Replace(nameof(SmiteOfSabbathAbility), "PrayerTest");
        public override string DisplayName => "Smite of the Sabbath";
        public override int Cooldown => 120;
        public override SoundStyle SwapSound => SoundManager.Sounds["Biter"];

        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {
            int i = 0;
            float spread = 10f * 0.0174f;
            double startAngle = Math.Atan2(6, 6) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<WindsofGod>(), 10, 0f, player.whoAmI);
        }
        
        
        public override IEnumerator OnUseAbilityRoutine(Player player, EntitySource_PrayerAbility source)
        {
            
            for (int i = 0; i < 120; i++)
            {
                Main.NewText("Prayer Cooldown Over");
                yield return null;
            }
        }
    }
}
