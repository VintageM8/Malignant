﻿using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;

namespace Malignant.Content.Items.Crimson.Arterion.MoniterAccessory
{
    public class BloodRune : ModProjectile
    {
        Vector2 initPos = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blood Rune");
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 82;
            Projectile.height = 82;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 240;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;

        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            if (Projectile.ai[1] == 0)
            {
                if (Projectile.ai[0] == 0)
                {
                    initPos = Projectile.position;
                }

                Projectile.position = Vector2.SmoothStep(initPos, initPos - new Vector2(0, 36), Projectile.ai[0] / 45f);
                Projectile.alpha -= 7;

                if (Projectile.ai[0] == 0)
                {
                    Projectile.scale = 0.01f;
                }

                if (Projectile.ai[0] > 2 && Projectile.ai[0] < 45)
                {
                    Projectile.scale = MathHelper.Lerp(Projectile.scale, 1, 0.05f);
                    Projectile.localAI[0] = MathHelper.Lerp(0.001f, 5f, 0.05f);
                    Projectile.rotation += Projectile.localAI[0];
                }

                if (Projectile.ai[0] == 45)
                {
                    Projectile.ai[1] = 1;
                }
            }
            else
            {
                Projectile.localAI[0] = MathHelper.Lerp(0.001f, 5f, 0.05f);

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    float distance = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
                    if (distance <= 400 && !Main.npc[i].friendly && Projectile.ai[0] >= 90)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.One.RotatedByRandom(Math.PI) * 4, ProjectileType<Blood>(), Projectile.damage, 3f, Main.myPlayer);

                            Projectile.Kill();
                        }
                    }
                }
            }

            Projectile.ai[0]++;
            Projectile.rotation += Projectile.localAI[0];
        }

        public override void Kill(int TimeLeft)
        {
            for (int i = 0; i < 30; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int i = 0; i < Main.rand.Next(8, 10); i++)
            {
                Vector2 perturbedSpeed = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(360));
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<Blood>(), 40, 5f, Projectile.owner);
            }

        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}