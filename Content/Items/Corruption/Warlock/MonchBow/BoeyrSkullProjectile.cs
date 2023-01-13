using Malignant.Common.Helper;
using Malignant.Common.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Corruption.Warlock.MonchBow
{
    public class BoeyrSkullProjectile : ModProjectile
    {
        SoundStyle biteSound;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;

            ProjectileID.Sets.TrailCacheLength[Type] = 3;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        const int size = 14;
        public override void SetDefaults()
        {
            Projectile.width = size;
            Projectile.height = size;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;

            Projectile.aiStyle = -1;

            Projectile.localNPCHitCooldown = 15;
            Projectile.usesLocalNPCImmunity = true;

            biteSound = new SoundStyle("Malignant/Assets/SFX/Biter");
        }

        int biteTimer;
        public override void AI()
        {
            if (shouldStickToTarget)
            {
                Projectile.Center = target.Center + offsetFromCenterTarget;
                if (Projectile.timeLeft < TIME_LEFT_ONHIT)
                {
                    Projectile.velocity = Vector2.Zero;

                    if (Projectile.timeLeft > TIME_LEFT_ONHIT * 0.9f)
                    {
                        Projectile.scale += 0.13f;
                    }
                    else
                    {
                        Projectile.scale *= 0.99f;
                    }

                    Projectile.position = Projectile.Center;
                    Projectile.width = (int)(size * Projectile.scale);
                    Projectile.height = (int)(size * Projectile.scale);
                    Projectile.Center = Projectile.position;

                    if (biteTimer++ > Projectile.localNPCHitCooldown * 0.5f)
                    {
                        if (Projectile.frame == 1)
                        {
                            float randRot = Main.rand.NextFloatDirection() * MathHelper.PiOver4 * 0.2f;
                            Projectile.rotation += randRot;
                            offsetFromCenterTarget -= Projectile.Center.DirectionTo(target.Center).RotatedBy(randRot) * 0.2f;
                            Projectile.frame = 0;

                            Vector2 rotDir = Projectile.rotation.ToRotationVector2() * Projectile.spriteDirection;

                            MethodHelper.NewDustCircular(
                                Projectile.Center + rotDir * 8,
                                2,
                                i => Main.rand.NextFromList(DustID.Blood, DustID.Blood, DustID.Bone),
                                8,
                                Main.rand.NextFloat(),
                                (2.5f, 5),
                                d =>
                                {
                                    d.scale = Main.rand.NextFloat(0.4f, 1.8f);
                                    d.velocity += rotDir * 4;
                                }
                                );

                            Projectile.scale += Main.rand.NextFloat(0.3f);

                            SoundEngine.PlaySound(biteSound, Projectile.Center);
                        }
                        else
                        {
                            Projectile.frame = 1;
                            Projectile.scale -= Main.rand.NextFloat(0.2f);
                        }
                        biteTimer = 0;
                    }
                }
            }
            else
            {
                Projectile.frame = 1;
                Projectile.scale = 1;

                if (!Projectile.GetGlobalProjectile<GlobalChargingProjectile>().IsBeingCharged)
                {
                    Projectile.velocity.Y += 0.05f;
                }

                Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.direction == -1 ? MathHelper.Pi : 0);
                Projectile.spriteDirection = Projectile.direction;
            }

            Projectile.alpha = (int)Math.Clamp((TIME_LEFT_ONHIT * 0.3f -  Projectile.timeLeft) / (TIME_LEFT_ONHIT * 0.3f) * 255, 0, 255);
        }

        public override bool ShouldUpdatePosition() => !Projectile.GetGlobalProjectile<GlobalChargingProjectile>().IsBeingCharged;

        const int TIME_LEFT_ONHIT = 100;

        Vector2 offsetFromCenterTarget;
        NPC target;
        bool shouldStickToTarget => target is not null && target.life > 0 && target.active;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.frame = 0;
            SoundEngine.PlaySound(biteSound, Projectile.Center);

            if (!shouldStickToTarget || Projectile.timeLeft > TIME_LEFT_ONHIT)
            {
                this.target = target;
                offsetFromCenterTarget = Projectile.Center - target.Center;

                Projectile.timeLeft = TIME_LEFT_ONHIT;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!shouldStickToTarget)
            {
                MethodHelper.NewDustCircular(
                        Projectile.Center,
                        5,
                        i => Main.rand.NextFromList(DustID.Bone, DustID.Blood),
                        26,
                        Main.rand.NextFloat(),
                        (2, 4),
                        d =>
                        {
                            d.scale = Main.rand.NextFloat(0.8f, 1.7f);
                            d.velocity += Projectile.velocity * -0.2f;
                        }
                        );
                return true;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 texDrawOrigin = Projectile.spriteDirection == -1 ? new Vector2(13, 14) : new Vector2(25, 14);

            Projectile.EasyDrawAfterImage(lightColor * 0.8f, Projectile.oldPos.ForEach(pos => pos + Projectile.Hitbox.Size() * 0.5f + Main.rand.NextVector2Unit() * 4), origin: texDrawOrigin);
            Projectile.EasyDraw(lightColor * ((255 - Projectile.alpha) / 255f), origin: texDrawOrigin);
            return false;
        }
    }
}

