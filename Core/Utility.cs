using Microsoft.Xna.Framework;
using System;
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

    }
}