using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Malignant.Content.Items.Misc;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

using static Terraria.ModLoader.ModContent;
using Malignant.Common.Helper;

namespace Malignant.Common.Projectiles
{
    public abstract class HeldGunModItem : ModItem
    {
        public abstract (float centerYOffset, float muzzleOffset, Vector2 drawOrigin, Vector2 recoil) HeldProjectileData { get; }

        public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileType<HeldProjectileGun>(), damage, knockback, player.whoAmI, type);
            return false;
        }
        /// <summary>
        /// By default shoots one projectile of default type. Runs client side.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="source"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        /// <param name="knockback"></param>
        public virtual void ShootGun(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        }
    }

    public sealed class HeldProjectileGun : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Acorn;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 999;
            Projectile.extraUpdates = 1;
        }

        private EntitySource_ItemUse_WithAmmo itemSource;
        private HeldGunModItem heldGunItem;
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.ModItem is HeldGunModItem heldGunItem)
            {
                this.itemSource = itemSource;
                this.heldGunItem = heldGunItem;
                return;
            }

            Projectile.active = false;
        }

        public override bool ShouldUpdatePosition() => false;

        private Player Player => Main.player[Projectile.owner];
        private Vector2 directionToMouse;
        private Vector2 recoil;
        private bool shotProjectile;
        public override void AI()
        {
            if (Player.ItemAnimationEndingOrEnded || Player.HeldItem.type != heldGunItem.Type)
            {
                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;

            if (Main.myPlayer == Player.whoAmI)
            {
                Projectile.SetHeldProjectileInHand(Player, heldGunItem.HeldProjectileData.centerYOffset);
                directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);

                Projectile.netUpdate = true;
            }

            if (!shotProjectile)
            {
                heldGunItem.ShootGun(
                    Player,
                    itemSource,
                    Projectile.Center + directionToMouse * heldGunItem.HeldProjectileData.muzzleOffset,
                    directionToMouse * Projectile.velocity.Length(),
                    (int)Projectile.ai[0],
                    Projectile.damage,
                    Projectile.knockBack
                );

                shotProjectile = true;
                if (Main.myPlayer == Player.whoAmI)
                {
                    recoil += heldGunItem.HeldProjectileData.recoil;
                }
            }

            Projectile.rotation = directionToMouse.ToRotation() + -recoil.Y * Player.direction;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            recoil *= 0.92f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(directionToMouse);
            writer.WriteVector2(recoil);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            directionToMouse = reader.ReadVector2();
            recoil = reader.ReadVector2();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Item[heldGunItem.Type].Value;
            Vector2 normOrigin = heldGunItem.HeldProjectileData.drawOrigin + Vector2.UnitX * recoil.X;

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
                Player.direction == -1 ? new Vector2(texture.Width - normOrigin.X, normOrigin.Y) : normOrigin,
                Projectile.scale,
                Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );

            return false;
        }
    }
}
