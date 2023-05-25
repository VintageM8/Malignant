/*﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items
{
    internal class BlackAvenger : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 36;

            Item.damage = 50;
            Item.knockBack = 3f;
            Item.crit = 12;

            Item.useTime = 35;
            Item.useAnimation = 35;

            Item.rare = 3;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(gold: 1);
            Item.DamageType = DamageClass.Ranged;

            Item.noMelee = true;
            Item.noUseGraphic = false;

            Item.scale = .5f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            player.GetModPlayer<BlackAvengerPlayer>().ReloadCount++;
            if (player.altFunctionUse == 2)
            {
                type = ProjectileID.StickyGrenade;
            }
        }
    }
    class BlackAvengerPlayer : ModPlayer
    {
        public int ReloadCount = 0;
        bool IsInReloadState = false;
        int ReloadCoolDown = 0;
        public override void PreUpdate()
        {
            if (ReloadCount >= 6)
            {
                IsInReloadState = true;
                ReloadCount = 0;
                ReloadCoolDown = 180;
            }
        }
        public override void PostUpdate()
        {
            Item item = Player.HeldItem;
            if (item.type != ModContent.ItemType<BlackAvenger>())
            {
                if (ReloadCoolDown > 0)
                {
                    ReloadCoolDown = 180;
                }
                return;
            }
            ReloadCoolDown -= ReloadCoolDown > 0 ? 1 : 0;
            if (ReloadCoolDown <= 0)
            {
                IsInReloadState = false;
            }
        }
        public override bool CanUseItem(Item item)
        {
            if (item.type != ModContent.ItemType<BlackAvenger>())
            {
                return base.CanUseItem(item);
            }
            return !IsInReloadState;
        }
    }
    class BlackAvengerBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.width = 28;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return base.OnTileCollide(oldVelocity);
        }
        public override void AI()
        {
            if(Projectile.velocity.Y < 20)
            {
                Projectile.velocity.Y += .5f;
            }
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<StickyExplosionProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
    class StickyExplosionProjectile : ModProjectile
    {
        public override string Texture => "Malignant/Common/GhostHitBox";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 0;
            Projectile.hide = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }
    }
}*/
