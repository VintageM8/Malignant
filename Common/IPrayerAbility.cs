using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Common
{
    public interface IPrayerAbility
    {
        public void Use(Player player);
        public int CooldownTime { get; }
    }
}
