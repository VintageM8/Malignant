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
        public static void SlowRotation(this ref float currentRotation, float targetAngle, float speed)
        {
            float actDirection = new Vector2((float)Math.Cos(currentRotation), (float)Math.Sin(currentRotation))
                .ToRotation();
            targetAngle = new Vector2((float)Math.Cos(targetAngle), (float)Math.Sin(targetAngle)).ToRotation();

            int f;
            //this makes f 1 or -1 to rotate the shorter distance
            if (Math.Abs(actDirection - targetAngle) > Math.PI)
            {
                f = -1;
            }
            else
            {
                f = 1;
            }

            if (actDirection <= targetAngle + speed * 2 && actDirection >= targetAngle - speed * 2)
            {
                actDirection = targetAngle;
            }
            else if (actDirection <= targetAngle)
            {
                actDirection += speed * f;
            }
            else if (actDirection >= targetAngle)
            {
                actDirection -= speed * f;
            }

            actDirection = new Vector2((float)Math.Cos(actDirection), (float)Math.Sin(actDirection)).ToRotation();
            currentRotation = actDirection;
        }

        public static void Move(this Projectile projectile, Vector2 vector, float speed, float turnResistance = 10f,
            bool toPlayer = false)
        {
            Player player = Main.player[projectile.owner];
            Vector2 moveTo = toPlayer ? player.Center + vector : vector;
            Vector2 move = moveTo - projectile.Center;
            float magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }

            move = (projectile.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = Magnitude(move);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }

            projectile.velocity = move;
        }

        public static float Magnitude(Vector2 mag) // For the Move code above
        {
            return (float)Math.Sqrt(mag.X * mag.X + mag.Y * mag.Y);
        }

        public static void LookByVelocity(this Projectile projectile)
        {
            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = 1;
                projectile.direction = 1;
            }
            else if (projectile.velocity.X < 0)
            {
                projectile.spriteDirection = -1;
                projectile.direction = -1;
            }
        }

        public static Vector2[] GenerateCircularPositions(this Vector2 center, float radius, int amount = 8, float rotation = 0)
        {
            if (amount <= 0)
                return Array.Empty<Vector2>();

            Vector2[] postitions = new Vector2[amount];

            float angle = MathHelper.Pi * 2f / amount;
            angle += rotation;

            for (int i = 0; i < amount; i++)
            {
                Vector2 position = (angle * i).ToRotationVector2();
                position *= radius;
                position += center;

                postitions[i] = position;
            }

            return postitions;
        }

        public static List<Projectile> OwnedProjectiles(this Player player, int type = -1)
        {
            List<Projectile> projectiles = new List<Projectile>();
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile is not null && projectile.owner == player.whoAmI && (projectile.type < 0 || projectile.type == type))
                {
                    projectiles.Add(projectile);
                }
            }

            return projectiles;
        }

        public static T[] ForEach<T>(this T[] arr, Func<T, T> predicate)
        {
            T[] values = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                values[i] = predicate(arr[i]);
            }

            return values;
        }
    }
}
