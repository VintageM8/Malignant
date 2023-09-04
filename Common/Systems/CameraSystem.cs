using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;
using Malignant.Common.Players;

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
        float zoomBefore;
        public static float zoomAmount;
        public static Vector2 cameraChangeStartPoint;
        public static Vector2 CameraChangePos;
        public static float CameraChangeTransition;
        public static int CameraChangeLength;
        public static bool isChangingCameraPos;
        public static void ChangeCameraPos(Vector2 pos, int length, float zoom = 1.65f)
        {
            cameraChangeStartPoint = Main.screenPosition;
            CameraChangeLength = length;
            CameraChangePos = pos - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            isChangingCameraPos = true;
            CameraChangeTransition = 0;
            if (Main.GameZoomTarget < zoom)
                zoomAmount = zoom;
        }

        public class BossTitleStyleID
        {
            public static readonly int Generic = -1;
            public static readonly int Arterion = 0;
        }
        public static void SetBossTitle(int progress, string name, Color color, string title = null, int style = -1)
        {
            MalignantPlayer player = Main.LocalPlayer.GetModPlayer<MalignantPlayer>();
            player.bossTextProgress = progress;
            player.bossMaxProgress = progress;
            player.bossName = name;
            player.bossTitle = title;
            player.bossColor = color;
            player.bossStyle = style;
        }
    }
}
