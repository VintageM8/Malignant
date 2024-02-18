using Malignant.Common;
using Malignant.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Malignant
{
    public class Malignant : Mod
    {
        public static Malignant Mod { get; set; }
        public static Malignant Instance { get; set; }

        public static int PrayerToken;

        public static string MeterStyle = "Default";
        public static bool MeterText = true;


        public static float PurityMeterX = 0.5f;
        public static float PurityMeterY = 0.06f;
        public static Texture2D SoHShrineText;

        public Malignant()
        {
            Instance = this;
            Mod = this;
        }

        public override void Load()
        {
            PrayerToken = CustomCurrencyManager.RegisterCurrency(new Content.Currencies.PrayerTokenCurrency(ModContent.ItemType<Content.Items.Misc.PrayerToken>(), 999L, "Prayer Token"));

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
