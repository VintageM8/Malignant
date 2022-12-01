using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace Malignant.Common.Helper
{
    public static partial class MethodHelper
    {
        public static void EasyDraw(this Projectile projectile, Color color, Vector2? position = null, Vector2? origin = null, SpriteEffects? spriteEffects = null)
        {
            Texture2D tex = TextureAssets.Projectile[projectile.type].Value;

            int frameHeight = tex.Height / Main.projFrames[projectile.type];
            Rectangle rect = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);

            Main.spriteBatch.Draw(
                tex,
                (position ?? projectile.Center) - Main.screenPosition,
                rect,
                color,
                projectile.rotation,
                origin ?? (rect.Size() * 0.5f),
                projectile.scale,
                spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
                0
                );
        }

        public static void EasyDrawAfterImage(this Projectile projectile, Color? color = null, Vector2[] oldPos = null, Vector2? origin = null, SpriteEffects? spriteEffects = null)
        {
            Texture2D tex = TextureAssets.Projectile[projectile.type].Value;

            int frameHeight = tex.Height / Main.projFrames[projectile.type];
            Rectangle rect = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);

            Vector2[] positions = oldPos ?? projectile.oldPos;
            for (int i = 0; i < positions.Length; i++)
            {
                Vector2 position = positions[i];

                Main.spriteBatch.Draw(
                    tex,
                    position - Main.screenPosition,
                    rect,
                    (color ?? Color.White) * ((float)(positions.Length - (i + 1)) / positions.Length),
                    projectile.rotation,
                    origin ?? rect.Size() * 0.5f,
                    projectile.scale,
                    spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
                    0
                );
            }
        }

        public static int WhoAmIType(this Projectile projectile)
        {
            int whoAmI = 0;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.whoAmI == projectile.whoAmI)
                    break;
                if (proj.type == projectile.type)
                    whoAmI++;
            }

            return whoAmI;
        }
    }
}
