using Malignant.Common;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Projectiles.Prayer
{
    public class WindsofGod : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Winds of God");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 45;
            Projectile.height = 45;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
        }


        bool spawnStuff = true;
        Vector2 initialCenter;
        public override void AI()
        {
            if (spawnStuff)
            {
                initialCenter = Projectile.Center;
                spawnStuff = false;
            }

            if (Main.rand.NextBool(10))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

            int dustCount = 80;

            Vector2 direction = initialCenter.DirectionTo(Projectile.Center);
            float interval = initialCenter.Distance(Projectile.Center) / dustCount;

            for (int i = 0; i < dustCount; i++)
            {
                float prog = (float)i / dustCount;
                float curve = MathF.Sin(i * 0.27f + Main.GameUpdateCount * 0.5f) * MathF.Sin(prog * MathHelper.Pi );
                Dust.NewDustDirect(initialCenter + direction * interval * i + direction.RotatedBy(MathHelper.PiOver2) * curve * 25, 0, 0, DustID.InfernoFork, Scale: 1.3f * prog).noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void Kill(int timeLeft)
        {
            /*
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 14f, 0f, ModContent.ProjectileType<HolyWind>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, -14f, 0f, ModContent.ProjectileType<HolyWind>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0f, 14f, ModContent.ProjectileType<HolyWind>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0f, -14f, ModContent.ProjectileType<HolyWind>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

            for (int num621 = 0; num621 < 20; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 35; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
            */

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            MethodHelper.NewDustCircular(Projectile.Center, Projectile.width * 0.1f, i => Main.rand.NextFromList(DustID.CursedTorch, DustID.InfernoFork), 85, minMaxSpeedFromCenter: (8, 10), dustAction: d => d.noGravity = true);
            MethodHelper.ForeachNPCInRange(Projectile.Center, Projectile.width * 3, npc =>
            {
                if (!npc.friendly && npc.immune[Projectile.owner] <= 0)
                    npc.StrikeNPC((int)(Projectile.damage * 0.5f), 0, 0);
            });
        }

    }
}
