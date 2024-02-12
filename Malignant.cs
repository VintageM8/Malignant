using Malignant.Common;
using Malignant.Common.Systems;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Malignant
{
    public class Malignant : Mod
    {
        public static Malignant Mod { get; set; }
        public static Malignant Instance { get; set; }

        public static int PrayerToken;

        public Malignant()
        {
            Instance = this;
            Mod = this;
        }

        public override void Load()
        {
            PrayerToken = CustomCurrencyManager.RegisterCurrency(new Content.Currencies.PrayerTokenCurrency(ModContent.ItemType<Content.Items.Misc.PrayerToken>(), 999L, "Mods.Malignant.Currencies.PrayerTokenCurrency"));

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
