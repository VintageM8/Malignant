using Malignant.Common;
using Terraria.ModLoader;

namespace Malignant
{
	public class Malignant : Mod
	{
        public static Malignant Mod { get; set; }
        

        public Malignant()
        {
            Mod = this;
        }

        public override void Unload()
        {
            Mod = null;
        }

        public override void Load()
        {
            PrayerContent.Load(Mod);
        }
    }
}