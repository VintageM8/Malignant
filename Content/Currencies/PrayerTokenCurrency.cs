using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;

namespace Malignant.Content.Currencies
{
    public class PrayerTokenCurrency : CustomCurrencySingleCoin
    {
        public PrayerTokenCurrency(int coinItemID, long currencyCap, string CurrencyTextKey) : base(coinItemID, currencyCap)
        {
            this.CurrencyTextKey = CurrencyTextKey;
            CurrencyTextColor = Color.BlueViolet;
        }
    }
}
