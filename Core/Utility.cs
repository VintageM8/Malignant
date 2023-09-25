using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace Malignant.Core
{
    public static class Utility
    {
        public static Color ConvectiveFlameColor(float progress)
        {
            float clampedProgress = Math.Clamp(progress, 0, 1);
            float r = 1.25f - clampedProgress / 2;
            float g = clampedProgress < 0.5f ? 4 * clampedProgress * (1 - clampedProgress) : 13 / 12f - clampedProgress / 6f;
            float b = 2 * clampedProgress;
            return new Color(r, g, b);
        }

        public static int AddItem(this Chest c, int itemID, int amount = 1)
        {
            Item i = new Item();
            i.SetDefaults(itemID);
            i.stack = amount;
            return c.AddItemToShop(i);
        }

        public static Rectangle AnimationFrame(this Texture2D texture, ref int frame, ref int frameTick, int frameTime, int frameCount, bool frameTickIncrease, int overrideHeight = 0)
        {
            if (frameTick >= frameTime)
            {
                frameTick = -1;
                frame = frame == frameCount - 1 ? 0 : frame + 1;
            }
            if (frameTickIncrease)
                frameTick++;
            return new Rectangle(0, overrideHeight != 0 ? overrideHeight * frame : (texture.Height / frameCount) * frame, texture.Width, texture.Height / frameCount);
        }

        public static Rectangle Animate(this Texture2D texture, ref Rectangle frameData, ref int frameTick, int frameTime, int horizontalFrames = 1, int verticalFrames = 1, int sizeOffsetX = 0, int sizeOffsetY = 0)
        {
            frameData.Width = horizontalFrames;
            frameData.Height = verticalFrames;

            if (frameTick >= frameTime)
            {
                frameTick = 0;
                frameData.X = frameData.X == frameData.Width - 1 ? 0 : frameData.X + 1;
                frameData.Y = frameData.Y == frameData.Height - 1 ? 0 : frameData.Y + 1;
            }
            frameTick++;

            return new Rectangle(sizeOffsetX != 0 ? sizeOffsetX * frameData.X : (texture.Width / frameData.Width) * frameData.X,   // Horizontal frame position
                                 sizeOffsetY != 0 ? sizeOffsetY * frameData.Y : (texture.Height / frameData.Height) * frameData.Y, // Vertical frame position
                                 texture.Width / frameData.Width,                   // Horizontal frame width
                                 texture.Height / frameData.Height);                // Vertical frame width
        }
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

        public static List<T> Shuffle<T>(this List<T> list)
        {
            int c = list.Count;
            List<T> current = new List<T>();
            for (int i = 0; i < c; i++)
            {
                int index = Main.rand.Next(list.Count);
                current.Add(list[index]);
                list.RemoveAt(index);
            }

            return current;
        }

        public static T[] Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = Main.rand.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            return array;
            //return Shuffle<T>(new List<T>(array)).ToArray();
        }

    }
}

