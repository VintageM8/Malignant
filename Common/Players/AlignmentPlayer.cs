using Malignant.Content.Items.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Malignant.Common.Players
{
    // Simple implementation subject to change just needed it for the prayers.
    internal class AlignmentPlayer : ModPlayer
    {
        public int KarmaPoints { get; private set; } = 0;

        public Alignment Alignment
        {
            // Also temporary
            get
            {
                if (Player.GetModPlayer<FruitOfTheGardenModPlayer>().ConsumedFruit || KarmaPoints > 0)
                {
                    return Alignment.Holy;
                }

                if (KarmaPoints == 0)
                {
                    return Alignment.Neutral;
                }

                return Alignment.Unholy;
            }
        }
    }
}
