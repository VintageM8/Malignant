using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Content.Dusts;

namespace Malignant.Common.Helper
{
    public static partial class MethodHelper
    {
        public static void DustExplosion(Vector2 pos, Vector2 size = default, bool colored = false, Color color = default, bool sound = true, bool smoke = true)
        {
            int dustType = colored ? ModContent.DustType<FireD_2>() : ModContent.DustType<FireDust>();
            if (sound)
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, pos);
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(pos, dustType, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, color, Main.rand.NextFloat(1, 1.75f)).noGravity = true;
            }
            if (smoke)
                for (int num905 = 0; num905 < 10; num905++)
                {
                    int num906 = Dust.NewDust(new Vector2(pos.X, pos.Y), (int)size.X, (int)size.Y, 31, 0f, 0f, 0, default(Color), 2.5f);
                    Main.dust[num906].position = pos + Vector2.UnitX.RotatedByRandom(3.1415927410125732) * size.X / 2f;
                    Main.dust[num906].noGravity = true;
                    Dust dust2 = Main.dust[num906];
                    dust2.velocity *= 3f;
                }
            if (smoke)
                for (int num899 = 0; num899 < 4; num899++)
                {
                    int num900 = Dust.NewDust(new Vector2(pos.X, pos.Y), (int)size.X, (int)size.Y, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[num900].position = pos + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * size.X / 2f;
                }
        }

        public static void DrawStar(Vector2 position, int dustType, float pointAmount = 5, float mainSize = 1, float dustDensity = 1, float dustSize = 1f, float pointDepthMult = 1f, float pointDepthMultOffset = 0.5f, bool noGravity = false, float randomAmount = 0, float rotationAmount = -1)
        {
            float rot;
            if (rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }

            float density = 1 / dustDensity * 0.1f;

            for (float k = 0; k < 6.28f; k += density)
            {
                float rand = 0;
                if (randomAmount > 0) { rand = Main.rand.NextFloat(-0.01f, 0.01f) * randomAmount; }

                float x = (float)Math.Cos(k + rand);
                float y = (float)Math.Sin(k + rand);
                float mult = ((Math.Abs(((k * (pointAmount / 2)) % (float)Math.PI) - (float)Math.PI / 2)) * pointDepthMult) + pointDepthMultOffset;//triangle wave function
                Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mult * mainSize, 0, default, dustSize).noGravity = noGravity;
            }
        }

        public static void DrawCircle(Vector2 position, int dustType, float mainSize = 1, float RatioX = 1, float RatioY = 1, float dustDensity = 1, float dustSize = 1f, float randomAmount = 0, float rotationAmount = 0, bool nogravity = false)
        {
            float rot;
            if (rotationAmount < 0) { rot = Main.rand.NextFloat(0, (float)Math.PI * 2); } else { rot = rotationAmount; }

            float density = 1 / dustDensity * 0.1f;

            for (float k = 0; k < 6.28f; k += density)
            {
                float rand = 0;
                if (randomAmount > 0) { rand = Main.rand.NextFloat(-0.01f, 0.01f) * randomAmount; }

                float x = (float)Math.Cos(k + rand) * RatioX;
                float y = (float)Math.Sin(k + rand) * RatioY;
                if (dustType == 222 || dustType == 130 || nogravity)
                {
                    Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mainSize, 0, default, dustSize).noGravity = true;
                }
                else
                {
                    Dust.NewDustPerfect(position, dustType, new Vector2(x, y).RotatedBy(rot) * mainSize, 0, default, dustSize);
                }
            }
        }

        // I know there is a method for this above but I wanted to add stuff but idk how it does stuff so i just rewrote it kinda (actually stole it from DF lol).
        public static void NewDustCircular(Vector2 center, float radius, Func<int, int> dustTypeFunc, int amount = 8, float rotation = 0, (float, float)? minMaxSpeedFromCenter = null, Action<Dust> dustAction = null)
        {
            Vector2[] positions = center.GenerateCircularPositions(radius, amount, rotation);
            for (int i = 0; i < positions.Length; i++)
            {
                Vector2 pos = positions[i];
                Vector2 velocity = minMaxSpeedFromCenter is not null ? (center.DirectionTo(pos) * Main.rand.NextFloat(minMaxSpeedFromCenter.Value.Item1, minMaxSpeedFromCenter.Value.Item2)) : Vector2.Zero;

                Dust dust = Dust.NewDustDirect(pos, 0, 0, dustTypeFunc.Invoke(i), velocity.X, velocity.Y);

                dustAction?.Invoke(dust);
            }
        }

        public static void NewDustCircular(Vector2 center, float radius, int dustType, int amount = 8, float rotation = 0, (float, float)? minMaxSpeedFromCenter = null, Action<Dust> dustAction = null)
        {
            NewDustCircular(center, radius, i => dustType, amount, rotation, minMaxSpeedFromCenter, dustAction);
        }
    }
}

