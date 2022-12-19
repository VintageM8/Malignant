using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Weapon.Crimson.Arterion.StaveofCarnem
{
    public class CarnemProj : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.light = 0.5f;
            Projectile.scale = 0.75f;
            Projectile.timeLeft = 360;
            DrawOffsetX = -6;
            DrawOriginOffsetY = -6;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Bleeding, 60);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Vector2 usePos = Projectile.position;

            Vector2 rotVector = (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            usePos += rotVector * 16f;

            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                dust = Main.dust[Terraria.Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 0)];
                dust.noGravity = true;
            }
        }

        public int untilCharge = 60;
        public bool initialFly = true;
        public int timeToCharge = 60;
        public int burstAmount = 0;

        private static readonly int[] unwantedPrefixes = new int[] { PrefixID.Inept, PrefixID.Ignorant, PrefixID.Deranged };

        public override void AI()
        {
            untilCharge -= 1;

            if (initialFly == true)
            {
                Projectile.rotation += 0.30f;
            }

            if (initialFly == false)
            {
                timeToCharge -= 1;
            }

            if (untilCharge <= 0 & timeToCharge > 0)
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;

                Projectile.rotation += 0.5f;
                initialFly = false;
            }

            if (timeToCharge <= 0)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                if (burstAmount == 0)
                {
                    for (int i = 0; i < 45; i++)
                    {
                        Vector2 speed2 = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y - 16), DustID.Blood, speed2 * 5);
                        d.noGravity = true;
                    }
                    burstAmount += 1;
                }

                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];
                    float shootToX = target.position.X + target.width * 0.5f - Projectile.Center.X;
                    float shootToY = target.position.Y - Projectile.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                    if (distance < 480f && !target.friendly && target.active)
                    {
                        distance = 3f / distance;
                        shootToX *= distance * 4;
                        shootToY *= distance * 4;
                        int proj = Projectile.NewProjectile(null, Projectile.Center.X, Projectile.Center.Y, shootToX, shootToY, ModContent.ProjectileType<HomingChunk>(), Main.rand.Next(10, 25), Projectile.knockBack, Main.myPlayer, 0f, 0f);
                        Main.projectile[proj].timeLeft = 300;
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                        Projectile.ai[0] = -50f;
                    }
                }
                Projectile.Kill();
            }
        }
    }
}
