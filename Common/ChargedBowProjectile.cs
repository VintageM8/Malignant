using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Common
{
    public interface IChargedBow
    {
        (Vector2, Vector2) StringTexturePositions { get; }
    }

    public abstract class ChargedBowProjectile : ModProjectile, IChargedBow
    {
        public override string Texture => base.Texture.Replace("Projectile", string.Empty);

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

            SetNewDefaults();
        }

        public virtual int ShootProjectileType => ProjectileID.None;
        public virtual int ShootProjectileCount => 1;
        public override void OnSpawn(IEntitySource source)
        {
            int projType = ShootProjectileType;

            EntitySource_ItemUse_WithAmmo itemSource = source as EntitySource_ItemUse_WithAmmo;

            if (projType == ProjectileID.None)
            {
                if (itemSource is not null)
                {
                    projType = ContentSamples.ItemsByType[itemSource.AmmoItemIdUsed].shoot;
                }
                else
                {
                    // Throw error or something
                }
            }

            projectiles = new Projectile[ShootProjectileCount];
            for (int i = 0; i < ShootProjectileCount; i++)
            {
                Projectile proj = Projectile.NewProjectileDirect(source, Projectile.Center, Vector2.Zero, projType, itemSource?.Item.damage ?? 0, itemSource?.Item.knockBack ?? 0, Player.whoAmI);
                proj.GetGlobalProjectile<GlobalChargingProjectile>().IsBeingCharged = true;
                projectiles[i] = proj;
            }
        }

        public override bool ShouldUpdatePosition() => false;

        public virtual int ChargeFramesMax => 50;
        public virtual int ShootFramesMax => 4;
        public virtual int PostShootFramesMax => 5;

        public Player Player => Main.player[Projectile.owner];
        public Vector2 directionToMouse;
        Projectile[] projectiles;
        ref float frameTimer => ref Projectile.ai[0];
        public sealed override void AI()
        {
            int maxFrames = ChargeFramesMax + ShootFramesMax + PostShootFramesMax;
            if (frameTimer >= maxFrames)
            {
                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;

            if (Main.myPlayer == Player.whoAmI)
                directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);

            float mouseRot = directionToMouse.ToRotation();

            Projectile.spriteDirection = mouseRot > MathHelper.PiOver2 || mouseRot < -MathHelper.PiOver2 ? -1 : 1;
            Projectile.rotation = mouseRot;
            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) + new Vector2(Player.direction * -3, -3);

            Player.direction = Projectile.spriteDirection;

            Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            Vector2 stringCenterPos = StringPoint(0.5f);
            Player.CompositeArmStretchAmount frontAront = stringCenterPos.DistanceSQ(Projectile.Center) > 20 ? Player.CompositeArmStretchAmount.Full : Player.CompositeArmStretchAmount.Quarter;
            Player.SetCompositeArmFront(true, frontAront, Projectile.Center.DirectionTo(stringCenterPos).ToRotation() - MathHelper.PiOver2);

            if (!Player.controlUseItem && frameTimer < ChargeFramesMax)
            {
                shootStrenght = frameTimer / maxFrames;
                frameTimer = ChargeFramesMax;
            }

            frameTimer++;
        }

        public override void PostAI()
        {
            if (frameTimer > ChargeFramesMax)
            {
                PostCharge();
            }
            else
            {
                Charge(projectiles);
            }
        }

        public virtual void Charge(Projectile[] projectiles)
        {
            projectiles[0].Center = StringPoint(0.5f) + Projectile.rotation.ToRotationVector2() * projectiles[0].width * 0.45f;
            projectiles[0].rotation = Projectile.rotation;
            projectiles[0].velocity = Vector2.Zero;
        }

        public bool shotProjectiles;
        void PostCharge()
        {
            if (!shotProjectiles)
            {
                shotProjectiles = true;
                Array.ForEach(projectiles, p => p.GetGlobalProjectile<GlobalChargingProjectile>().IsBeingCharged = false);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Shoot(projectiles);
            }
        }

        float shootStrenght = 1f;
        /// <summary>
        /// Override the shooting of projectiles. Runs on singleplayer and server-side.
        /// </summary>
        /// <param name="projectiles"></param>
        public virtual void Shoot(Projectile[] projectiles)
        {
            projectiles[0].velocity = Projectile.rotation.ToRotationVector2() * Projectile.velocity.Length() * shootStrenght;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(directionToMouse.X);
            writer.Write(directionToMouse.Y);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            directionToMouse.X = reader.ReadSingle();
            directionToMouse.Y = reader.ReadSingle();
        }

        public virtual Rectangle? DrawFrame => null;
        public virtual Vector2 DrawOrigin => new Vector2(-5, (DrawFrame?.Height ?? Tex.Height) * 0.5f);
        public virtual Vector2 DrawPosition => Projectile.Center;
        public Texture2D Tex => TextureAssets.Projectile[Type].Value;
        
        float StringCurve(float linePosition)
        {
            if (frameTimer >= ChargeFramesMax + ShootFramesMax)
                return 0f;

            float multiplier = 18f;

            float denominator;
            if (frameTimer > ChargeFramesMax)
            {
                denominator = (1f - (frameTimer - ChargeFramesMax) / ShootFramesMax) * shootStrenght;
            }
            else
            {
                denominator = frameTimer / ChargeFramesMax;
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
}
