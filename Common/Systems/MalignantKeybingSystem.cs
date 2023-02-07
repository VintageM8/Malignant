using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Malignant.Common.Systems
{
    public class MalignantKeybingSystem : ModSystem
    {
        public static ModKeybind UsePrayerAbility { get; private set; }
        public static ModKeybind ChangePrayerAbility { get; private set; }

        public override void Load()
        {
            UsePrayerAbility = KeybindLoader.RegisterKeybind(Mod, "Use Prayer Abilty", "R");
            ChangePrayerAbility = KeybindLoader.RegisterKeybind(Mod, "Change Prayer Ability", "T");
        }

        public override void Unload()
        {
            UsePrayerAbility = null;
            ChangePrayerAbility = null;
        }
    }
}
