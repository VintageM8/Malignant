using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Helper;
using Terraria.Utilities;

namespace Malignant.Common.Projectiles
{
    public abstract class ChargedBowProjectile : ModProjectile
    {
        public abstract (Vector2, Vector2) StringTexturePositions { get; }

        public virtual SoundStyle ShootSound => SoundID.Item102;
        public virtual void SetNewDefaults()
        {

        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 9999;
            Projectile.netImportant = true;
            Projectile.extraUpdates = 1;

            SetNewDefaults();
        }

        public virtual int ShootProjectileType => ProjectileID.None;
        public override void OnSpawn(IEntitySource source)
        {
            if (Player.whoAmI == Main.myPlayer)
            {
                int ammoItemType = (source as EntitySource_ItemUse_WithAmmo)?.AmmoItemIdUsed ?? ItemID.None;
                Item ammoItem = ammoItemType != ItemID.None ? ContentSamples.ItemsByType[ammoItemType] : null;

                int projType = ShootProjectileType;

                EntitySource_ItemUse_WithAmmo itemSource = source as EntitySource_ItemUse_WithAmmo;

                if (projType == ProjectileID.None)
                {
                    projType = ammoItem?.shoot ?? ProjectileID.None;
                }

                ShootVelocityMultiplier = 1f;


                arrow = Projectile.NewProjectileDirect(source, Projectile.Center, Vector2.Zero, projType, (itemSource?.Item.damage + (ammoItem?.damage ?? 0)) ?? 0, itemSource?.Item.knockBack ?? 0, Player.whoAmI);
                arrow.netUpdate = true;
            }
        }
        public override bool ShouldUpdatePosition() => false;

        public virtual int ChargeFramesMax => 100;
        public virtual int ShootFramesMax => 6;
        public virtual int PostShootFramesMax => 20;

        public Player Player => Main.player[Projectile.owner];
        public Vector2 directionToMouse;
        public Projectile arrow;
        ref float FrameTimer => ref Projectile.ai[0];
        public sealed override void AI()
        {
            int maxFrames = ChargeFramesMax + ShootFramesMax + PostShootFramesMax;
            if (FrameTimer >= maxFrames)
            {
                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;

            if (Main.myPlayer == Player.whoAmI)
            {
                directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);
                Projectile.netUpdate = true;
            }
                

            float mouseRot = directionToMouse.ToRotation();

            Projectile.spriteDirection = mouseRot > MathHelper.PiOver2 || mouseRot < -MathHelper.PiOver2 ? -1 : 1;
            Projectile.rotation = mouseRot;
            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) + new Vector2(Player.direction * -3, -3);

            Player.direction = Projectile.spriteDirection;

            Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            Vector2 stringCenterPos = StringPoint(0.5f);
            Player.CompositeArmStretchAmount frontAront = stringCenterPos.DistanceSQ(Projectile.Center) > 20 ? Player.CompositeArmStretchAmount.Full : Player.CompositeArmStretchAmount.Quarter;
            Player.SetCompositeArmFront(true, frontAront, Projectile.Center.DirectionTo(stringCenterPos).ToRotation() - MathHelper.PiOver2);

            if (!Player.controlUseItem && FrameTimer < ChargeFramesMax)
            {
                ShootVelocityMultiplier = FrameTimer / maxFrames;
                FrameTimer = ChargeFramesMax;
            }

            FrameTimer++;
        }

        public override void PostAI()
        {
            if (Player.whoAmI == Main.myPlayer)
                if (FrameTimer > ChargeFramesMax)
                {
                    PostCharge();
                }
                else
                {
                    Charge();
                    arrow.netUpdate = true;
                }
        }

        public virtual void Charge()
        {
            arrow.Center = StringPoint(0.5f) + Projectile.rotation.ToRotationVector2() * arrow.width * 0.45f;
            arrow.rotation = Projectile.rotation;
            arrow.velocity = Vector2.Zero;
        }

        public bool shotProjectiles;
        void PostCharge()
        {
            if (!shotProjectiles)
            {
                shotProjectiles = true;

                SoundEngine.PlaySound(ShootSound with { Pitch = ShootVelocityMultiplier - 0.5f }, Projectile.Center);
                Shoot();
            }
        }

        ref float ShootVelocityMultiplier => ref Projectile.ai[1];
        public virtual void Shoot()
        {
            arrow.velocity = Projectile.rotation.ToRotationVector2() * Projectile.velocity.Length() * ShootVelocityMultiplier;

            arrow.netUpdate = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(directionToMouse);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            directionToMouse = reader.ReadVector2();
        }

        public virtual Rectangle? DrawFrame => null;
        public virtual Vector2 DrawOrigin => new Vector2(-5, (DrawFrame?.Height ?? Tex.Height) * 0.5f);
        public virtual Vector2 DrawPosition => Projectile.Center;
        public Texture2D Tex => TextureAssets.Projectile[Type].Value;
        
        float StringCurve(float linePosition)
        {
            if (FrameTimer >= ChargeFramesMax + ShootFramesMax)
                return 0f;

            float multiplier = 18f;

            float denominator;
            if (FrameTimer > ChargeFramesMax)
            {
                denominator = (1f - (FrameTimer - ChargeFramesMax) / ShootFramesMax) * ShootVelocityMultiplier;
            }
            else
            {
                denominator = FrameTimer / ChargeFramesMax;
            }
            return MathF.Cos(linePosition * MathHelper.Pi - MathHelper.PiOver2) * denominator * multiplier;
        }

        public float MaxStringLenght => Math.Abs(StringTexturePositions.Item1.Y - StringTexturePositions.Item2.Y);
        public Vector2 StringPoint(float linePosition) => DrawPosition + (StringTexturePositions.Item1 + Vector2.UnitY * (MaxStringLenght * linePosition) - DrawOrigin - Vector2.UnitX * StringCurve(linePosition)).RotatedBy(Projectile.rotation);
        public virtual int StringThickness => 1;
        public virtual Color StringColor => Color.LightGray;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Tex is not null)
            {
                Main.spriteBatch.Draw(
                    Tex,
                    DrawPosition - Main.screenPosition,
                    DrawFrame,
                    lightColor,
                    Projectile.rotation,
                    DrawOrigin,
                    Projectile.scale,
                    Projectile.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None,
                    0
                    );

                for (int i = 0; i < MaxStringLenght; i++)
                {
                    Vector2 point = StringPoint(i / MaxStringLenght) - Main.screenPosition;

                    MethodHelper.DrawRectangle(new Rectangle((int)point.X, (int)point.Y, StringThickness, StringThickness), lightColor.MultiplyRGBA(StringColor));
                }

                return false;
            }

            return true;
        }
    }

    public class GlobalChargingProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool IsBeingCharged { get; set; }
    }

    public class GlobalChargingItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ContentSamples.ProjectilesByType[item.shoot].ModProjectile is ChargedBowProjectile cbp)
            {
                TooltipLine speedLine = tooltips.FirstOrDefault(t => t.Name == "Speed");
                if (speedLine is not null)
                {
                    speedLine.Text = $"[c/f57842:{((cbp.ChargeFramesMax + cbp.ShootFramesMax + cbp.PostShootFramesMax) / cbp.Projectile.extraUpdates / 60f).ToString("F2")}s] [c/996a5f:use time]\n" +
                        $"[c/f57842:{(cbp.ChargeFramesMax / cbp.Projectile.extraUpdates / 60f).ToString("F2")}s] [c/996a5f:max charge time]";
                }
            }
        }
    }
}
