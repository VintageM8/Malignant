using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace Malignant.Common
{
    public static partial class MethodHelper
    {
        public static void EasyDraw(this NPC npc, Color color, Vector2? position = null, Vector2? origin = null, SpriteEffects? spriteEffects = null)
        {
            Texture2D tex = TextureAssets.Npc[npc.type].Value;

            Vector2 drawOrigin = origin ?? (npc.frame.Size() * 0.5f);

            Main.spriteBatch.Draw(
                tex,
                (position ?? npc.Center) - Main.screenPosition,
                npc.frame,
                color,
                npc.rotation,
                drawOrigin,
                npc.scale,
                spriteEffects ?? (npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
                0
                );
        }

        public static void EasyDrawAfterImage(this NPC npc, Color? color = null, Vector2[] oldPos = null, Vector2? origin = null, SpriteEffects? spriteEffects = null)
        {
            Texture2D tex = TextureAssets.Npc[npc.type].Value;

            Vector2[] positions = oldPos ?? npc.oldPos;
            for (int i = 0; i < positions.Length; i++)
            {
                Vector2 position = positions[i];

                Main.spriteBatch.Draw(
                    tex,
                    position - Main.screenPosition,
                    npc.frame,
                    (color ?? Color.White) * ((float)(positions.Length - (i + 1)) / positions.Length),
                    npc.rotation,
                    origin ?? npc.frame.Size() * 0.5f,
                    npc.scale,
                    spriteEffects ?? (npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
                    0
                );
            }
        }
    }
}
