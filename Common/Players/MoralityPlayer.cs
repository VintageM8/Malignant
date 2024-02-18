using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Effects;
using Terraria.Audio;
using Microsoft.Xna.Framework.Audio;

namespace Malignant.Common.Players
{
    class MoralityPlayer : ModPlayer
    {
        public float PurityPercent = 0;
        public Color PurityMeterColor = Color.White;

        public static MoralityPlayer modPlayer(Player Player)
        {
            return Player.GetModPlayer<MoralityPlayer>();
        }

        public override void ResetEffects()
        {
            PurityMeterColor = Color.White;
        }
    }
}
