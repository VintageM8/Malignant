using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Common
{
    public partial class MethodHelper
    {
        static Texture2D LineTexture;
        public static void DrawRectangle(this Rectangle rectangle, Color? color = null)
        {
            if (LineTexture is null)
            {
                LineTexture = new Texture2D(Main.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                LineTexture.SetData(new Color[] { Color.White });
            }

            Main.spriteBatch.Draw(LineTexture, rectangle, color ?? Color.White);
        }
    }
}
