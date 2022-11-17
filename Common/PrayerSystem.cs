using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ID;
using Malignant.Content.Buffs;
using Malignant.Content.Projectiles.Prayer;
using Malignant.Content.Projectiles.Enemy.Warlock;
using System;

namespace Malignant.Common
{
    public class PrayerSystem : ModPlayer
    {
        public IPrayerAbility currentAbility;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Player player = Main.player[Main.myPlayer];
            if (Malignant.UseAbilty.JustPressed && !player.HasBuff(ModContent.BuffType<Cooldown>()) && currentAbility is not null)
            {
                currentAbility.Use(player);
                player.AddBuff(ModContent.BuffType<Cooldown>(), currentAbility.CooldownTime);
            }
        }
    }
}
