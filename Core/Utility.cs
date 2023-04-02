using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;

namespace Malignant.Core
{
    public static class Utility
    {
        public static Vector2 FromAToB(Vector2 a, Vector2 b, bool normalize = true, bool reverse = false)
        {
            Vector2 baseVel = b - a;
            if (normalize)
                baseVel.Normalize();
            if (reverse)
            {
                Vector2 baseVelReverse = a - b;
                if (normalize)
                    baseVelReverse.Normalize();
                return baseVelReverse;
            }
            return baseVel;
        }

        public static Vector2 GetInventoryPosition(Vector2 position, Rectangle frame, Vector2 origin, float scale)
        {
            return position + (((frame.Size() / 2f) - origin) * scale * Main.inventoryScale) + new Vector2(1.5f, 1.5f);
        }

        public static Color MultiLerpColor(float percent, params Color[] colors)
        {
            float per = 1f / ((float)colors.Length - 1);
            float total = per;
            int currentID = 0;
            while (percent / total > 1f && currentID < colors.Length - 2) { total += per; currentID++; }
            return Color.Lerp(colors[currentID], colors[currentID + 1], (percent - per * currentID) / per);
        }

        public static Vector2 DirectionTo(Vector2 Destination, Vector2 start)
        {
            return Vector2.Normalize(Destination - start);
        }

        public static Vector2 GetRandomVector(int MaxX, int MaxY, int MinX = 0, int MinY = 0)
        {
            return new Vector2(Main.rand.Next(MinX, MaxX), Main.rand.Next(MinY, MaxY));
        }
        public static Vector2 GetRandomVector(float mindistx, float mindisty, int MaxX, int MaxY, int MinX = 0, int MinY = 0)
        {
            float x = Main.rand.Next(MinX, MaxX);
            float y = Main.rand.Next(MinY, MaxY);
            while (Math.Abs(x) < mindistx)
                x = Main.rand.Next(MinX, MaxX);
            while (Math.Abs(y) < mindisty)
                y = Main.rand.Next(MinY, MaxY);
            return new Vector2(x, y);
        }

        public static Vector2 Normalized(Vector2 input)
        {
            input.Normalize();
            return input;
        }

        public static Vector2 PolarVector(float radius, float theta) =>
            new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * radius;
            
        public static void SineMovement(this Projectile projectile, Vector2 initialCenter, Vector2 initialVel, float frequencyMultiplier, float amplitude)
        {
            projectile.ai[1]++;
            float wave = (float)Math.Sin(projectile.ai[1] * frequencyMultiplier);
            Vector2 vector = new Vector2(initialVel.X, initialVel.Y).RotatedBy(MathHelper.ToRadians(90));
            vector.Normalize();
            wave *= projectile.ai[0];
            wave *= amplitude;
            Vector2 offset = vector * wave;
            projectile.Center = initialCenter + (projectile.velocity * projectile.ai[1]);
            projectile.Center = projectile.Center + offset;
        }
        public static void Reload(this SpriteBatch spriteBatch, BlendState blendState = default)
        {
            if ((bool)spriteBatch.GetType().GetField("beginCalled", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch))
            {
                spriteBatch.End();
            }
            SpriteSortMode sortMode = SpriteSortMode.Deferred;
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix matrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }

    }
}
