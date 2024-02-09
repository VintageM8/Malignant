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

namespace Malignant.Content.Items.Prayer.SmiteofSabbath
{
    public class SmiteOfSabbathPrayer : PrayerItem
    {
        public override string Texture => "Malignant/Content/Items/Prayer/FireballAbility";
        public override string AbilityType => PrayerContent.AbilityType<SmiteOfSabbathAbility>(); public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.consumable = true;
        }
    }

    public class SmiteOfSabbathAbility : PrayerAbility
    {
        public override string TexturePath => "Malignant/Content/Items/Prayer/FireballAbility";
        public override string DisplayName => "Smite of the Sabbath";
        public override int Cooldown => 120;
        public override SoundStyle SwapSound => SoundManager.Sounds["prayer"];
        public override Alignment Alignment => Alignment.Holy;

        protected override void OnUseAbility(Player player, EntitySource_PrayerAbility source)
        {
            Projectile.NewProjectile(source, Main.MouseWorld, Main.MouseWorld.DirectionTo(player.Center) * 10, ModContent.ProjectileType<SabbathProj>(), 45, 0f, player.whoAmI);

            for (int i = 0; i < 360; i += 90)
            {
                Vector2 SpawnPos = Main.MouseWorld + new Vector2(90).RotatedBy(MathHelper.ToRadians(i));
                Vector2 Velociry = Main.MouseWorld - SpawnPos;
                Velociry.Normalize();
                Projectile.NewProjectile(source, SpawnPos, Velociry * 5, 10, 0, player.whoAmI);
            }
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
