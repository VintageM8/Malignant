﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Malignant.Common.Players;

namespace Malignant.Content.Items.Snow.Cocytus.NjorSword
{
    public class NjorSwordProj : ModProjectile
    {
        public int combowombo;
        public static bool swung = false;
        public int SwingTime = 50;
        public float holdOffset = 50f;
        public override string Texture => "Malignant/Content/Items/Snow/Cocytus/NjorSword/IceSword";

        public override void SetDefaults()
        {
            Projectile.damage = 100;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 74;
            Projectile.width = 74;
            Projectile.friendly = true;
            Projectile.scale = 1f;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public virtual float Lerp(float val)
        {
            return val == 1f ? 1f : (val == 1f ? 1f : (float)Math.Pow(2, val * 10f - 10f) / 2f);
        }

        public override void AI()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10000;
            AttachToPlayer();
        }

        public override bool ShouldUpdatePosition() => false;

        public void AttachToPlayer()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || player.CCed || player.noItems)
            {
                return;
            }

            int dir = (int)Projectile.ai[1];
            float swingProgress = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            // the actual rotation it should have
            float defRot = Projectile.velocity.ToRotation();
            // starting rotation
            float endSet = ((MathHelper.PiOver2) / 0.2f);
            float start = defRot - endSet;

            // ending rotation
            float end = defRot + endSet;
            // current rotation obv
            float rotation = dir == 1 ? start.AngleLerp(end, swingProgress) : start.AngleLerp(end, 1f - swingProgress);
            // offsetted cuz sword sprite
            Vector2 position = player.RotatedRelativePoint(player.MountedCenter);
            position += rotation.ToRotationVector2() * holdOffset;
            Projectile.Center = position;
            Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver4;

            player.heldProj = Projectile.whoAmI;
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemRotation = rotation * player.direction;
            player.itemTime = 2;
            player.itemAnimation = 2;
            Projectile.netUpdate = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];

            player.GetModPlayer<MalignantPlayer>().itemCombo++;
            player.GetModPlayer<MalignantPlayer>().itemComboReset = 480;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Color drawColor = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}