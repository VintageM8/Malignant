﻿using Malignant.Content.Items.Crimson.FleshBlazer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Corruption.Warlock.StaffofFlame
{
    public class CursedFB : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override Color? GetAlpha(Color lightColor) => new(255, 255, 255, 100);

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.width = Projectile.height = 16;
            Projectile.scale = 1f;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;

            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
        }

        private bool initilize = true;

        public override void AI()
        {
            Vector3 RGB = new Vector3(1.45f, 2.55f, 0.94f);
            float multiplier = 0.2f;
            float max = 1f;
            float min = 0.5f;
            RGB *= multiplier;
            if (RGB.X > max)
            {
                multiplier = 0.3f;
            }
            if (RGB.X < min)
            {
                multiplier = 0.6f;
            }
            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            Projectile.velocity *= 0.98f;

            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Water, new Vector2(Main.rand.NextFloat(-0.2f, 0.2f)), 0, default, 1f);
            dust.noGravity = true;

            if (initilize)
            {
                int numberDust = 10;

                SoundEngine.PlaySound(SoundID.Waterfall, Projectile.Center);

                for (int i = 0; i < numberDust; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);

                    dust = Dust.NewDustPerfect(Projectile.Center, DustID.CursedTorch, speed * 5, 0, default, 1f);
                    dust.noGravity = true;
                }

                initilize = false;
            }

            Vector2 move = Vector2.Zero;
            float distance = 200f;
            bool target = false;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        distance = distanceTo;
                        target = true;
                    }
                }
            }
            if (target)
            {
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                AdjustMagnitude(ref Projectile.velocity);
            }

        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
            }
        }

        public override void Kill(int timeLeft)
        {
            Vector2 dir = Main.rand.NextVector2Unit() * 0.1f;
            Player player = Main.player[Projectile.owner];

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<CFStaffProj>(), Projectile.damage + player.ownedProjectileCounts[Projectile.type] * 2,
               Projectile.knockBack * player.ownedProjectileCounts[Projectile.type], Projectile.owner);

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CursedFBExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            ParticleManager.NewParticle(Projectile.Center, dir * Main.rand.NextFloat(10, 25), ParticleManager.NewInstance<StarParticle>(), new Color(0.50f, 2f, 0.5f, 0), 0.3f, Projectile.whoAmI, Layer: Particle.Layer.BeforeNPCs);


            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileID.CursedFlameFriendly, Projectile.damage + player.ownedProjectileCounts[Projectile.type] * 2,
               Projectile.knockBack * player.ownedProjectileCounts[Projectile.type], Projectile.owner);
            }
        }

        public class CursedFBExplosion : ModProjectile
        {
            bool initilize = true;

            Vector2 spawnPosition;

            public override string Texture => "Malignant/Assets/Textures/Cshmircle_2"; //Shmircle

            public override bool ShouldUpdatePosition() => false;

            public override void SetDefaults()
            {
                Projectile.penetrate = -1;
                Projectile.DamageType = DamageClass.Ranged;
                Projectile.friendly = true;
                Projectile.hostile = false;

                Projectile.Size = new Vector2(10f);
                Projectile.scale = 0.01f;

                Projectile.tileCollide = false;
                Projectile.ignoreWater = true;

                Projectile.aiStyle = -1;
                Projectile.timeLeft = 100;
            }

            public override void AI()
            {
                if (initilize)
                {
                    spawnPosition = Projectile.Center;

                    initilize = false;
                }

                Projectile.Center = spawnPosition;

                Projectile.scale += 0.02f;
                Projectile.Size += new Vector2(10f);
                Projectile.alpha += 10;

                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                }
            }

            public override bool PreDraw(ref Color lightColor)
            {
                Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

                int frameHeight = texture.Height / Main.projFrames[Projectile.type];
                int frameY = frameHeight * Projectile.frame;

                Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
                Vector2 origin = sourceRectangle.Size() / 2f;
                Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(new Color(50, 205, 50, 0));

                Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

                return false;
            }          
        }
    }
}