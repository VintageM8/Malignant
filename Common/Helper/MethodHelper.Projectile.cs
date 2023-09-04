using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Common.Helper
{
    public static partial class MethodHelper
    {
        public static int timer;
        public static bool IsHammer(this Projectile proj) => proj.CountsAsClass(DamageClass.Melee);
        public static float CircleDividedEqually(float i, float max)
        {
            return 2f * (float)Math.PI / max * i;
        }
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

        public static void EvenEasierDrawTrail(this Projectile projectile, Color lightColor, float ManualScaleAccordinglyToLength = 0)
        {
            Main.instance.LoadProjectile(projectile.type);
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
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
        public static void SineWave(this Projectile projectile, float timer, float amplitude, float waveStep, bool firstTick, Action<Projectile> changeDirection = null, bool reverseWave = false)
        {
            float num = timer * waveStep;
            float num2 = (float)Math.Sin((double)num) * amplitude;
            float num3;
            float num4;
            if (firstTick)
            {
                num3 = projectile.velocity.Length();
                num4 = Terraria.Utils.ToRotation(projectile.velocity);
            }
            else
            {
                float num5 = num2 - (float)Math.Sin((double)(num - waveStep)) * amplitude;
                num3 = (float)Math.Sqrt((double)(projectile.velocity.LengthSquared() - num5 * num5));
                num4 = Terraria.Utils.ToRotation(Terraria.Utils.RotatedBy(projectile.velocity, (double)(-(double)Terraria.Utils.ToRotation(new Vector2(num3, num5))), default(Vector2)));
            }
            if (changeDirection != null)
            {
                projectile.velocity = Terraria.Utils.ToRotationVector2(num4) * num3;
                changeDirection(projectile);
                num4 = Terraria.Utils.ToRotation(projectile.velocity);
                num3 = projectile.velocity.Length();
            }
            if (reverseWave)
            {
                amplitude *= -1f;
                num2 *= -1f;
            }
            projectile.velocity = Terraria.Utils.RotatedBy(new Vector2(num3, (float)Math.Sin((double)(num + waveStep)) * amplitude - num2), (double)num4, default(Vector2));
            projectile.rotation = Terraria.Utils.ToRotation(projectile.velocity) + 1.5707964f;
        }

        public static void SetHeldProjectileInHand(this Projectile projectile, Player player, float? verticalOffset = null)
        {
            projectile.Center = player.ShoulderPosition();
            Vector2 direction = projectile.Center.DirectionTo(Main.MouseWorld);

            if (verticalOffset.HasValue)
            {
                projectile.Center += direction.RotatedBy(-MathHelper.PiOver2 * player.direction) * verticalOffset.Value;
            }
        }
    }
}
