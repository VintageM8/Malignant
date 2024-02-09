using Malignant.Common;
using Malignant.Common.Systems;
using Terraria.ModLoader;

namespace Malignant
{
    public class Malignant : Mod
    {
        public static Malignant Mod { get; set; }
        public static Malignant Instance { get; set; }

        public Malignant()
        {
            Instance = this;
            Mod = this;
        }

        public override void Load()
        {
            Instance = this;
            MalignantLists.LoadLists();
            PrayerContent.Load(Mod);
            SoundManager.Load(Mod);
        }

        public override void Unload()
        {
            MalignantLists.UnloadLists();
            Mod = null;
        }
    }
}
