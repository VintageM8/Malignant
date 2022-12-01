using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;

namespace Malignant.Common.Systems
{
    public class CameraSystem : ModSystem
    {
        public static int ShakeTimer = 0;
        public static float ScreenShakeAmount = 0;

        public override void ModifyScreenPosition()
        {
            Player player = Main.LocalPlayer;           
            if (!Main.gameMenu)
            {
                ShakeTimer++;
                if (ScreenShakeAmount >= 0 && ShakeTimer >= 5)
                {
                    ScreenShakeAmount -= 0.1f;
                }
                if (ScreenShakeAmount < 0)
                {
                    ScreenShakeAmount = 0;
                }
                Main.screenPosition += new Vector2(ScreenShakeAmount * Main.rand.NextFloat(), ScreenShakeAmount * Main.rand.NextFloat());
            }
            else
            {
                ScreenShakeAmount = 0;
                ShakeTimer = 0;
            }
        }
    }
}