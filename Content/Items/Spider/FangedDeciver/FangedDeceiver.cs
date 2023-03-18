using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Malignant.Content.Items.Misc;
using Malignant.Content.Items.Crimson.FleshBlazer;
using Malignant.Content.Projectiles.Prayer;
using Malignant.Content.Items.Corruption.DepravedBlastBeat;
using Malignant.Common.Helper;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System.IO;

namespace Malignant.Content.Items.Spider.FangedDeciver
{
    public class FangedDeceiver : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fanged Deciver");
            Tooltip.SetDefault("Shoots out a venomus bullet\nEvery 5th shot fangs shoot out around you\nGains power as you progress");
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 38;
            Item.crit = 0;
            Item.damage = 34;
            Item.useAnimation = Item.useTime = 50;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item36;

            Item.rare = ItemRarityID.LightRed;

            Item.shootSpeed = 10f;
            Item.shoot = ProjectileType<FangedDeceiverHeldProjectile>();
            Item.useAmmo = AmmoID.Bullet;

            Item.noUseGraphic = true;
        }

        private int shotCount;
        private int fangCount = 4;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (++shotCount > 4)
            {
                foreach (Vector2 fangDirection in Vector2.Zero.GenerateCircularPositions(1f, fangCount))
                {
                    Projectile.NewProjectileDirect(
                        source,
                        player.Center,
                        fangDirection * Item.shootSpeed,
                        ProjectileType<DepravedBlast_Proj>(),
                        10,
                        0f,
                        player.whoAmI
                    ).netUpdate = true;
                }
                
                shotCount = 0;
            }
            Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, type);
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            if (NPC.downedMechBossAny)
            {
                Item.damage = 49;
                fangCount = 5;
            }

            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                Item.damage = 52;
                fangCount = 6;
            }

            if (NPC.downedPlantBoss)
            {
                Item.damage = 58;
                fangCount = 7;
            }

            if (NPC.downedGolemBoss)
            {
                Item.damage = 62;
                fangCount = 8;
            }

            if (NPC.downedFishron)
            {
                Item.damage = 67;
                fangCount = 10;
            }

            if (NPC.downedAncientCultist)
            {
                Item.damage = 75;
                fangCount = 12;
            }

            if (NPC.downedMoonlord)
            {
                Item.damage = 82;
                fangCount = 16;
            }
          
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddTile(TileID.Anvils)
                .AddIngredient(ItemID.SpiderFang, 18)
                .AddIngredient(ItemID.QuadBarrelShotgun, 1)
                .Register();

        }
    }

    public class FangedDeceiverHeldProjectile : ModProjectile
    {
        public override string Texture => base.Texture.Replace("HeldProjectile", string.Empty);

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

        public override bool ShouldUpdatePosition() => false;

        private Player Player => Main.player[Projectile.owner];
        private Vector2 directionToMouse;
        private Vector2 recoil;
        bool shotProjectile;
        public override void AI()
        {
            if (Player.ItemAnimationEndingOrEnded || Player.HeldItem.type != ItemType<FangedDeceiver>())
            {
                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;

            if (Main.myPlayer == Player.whoAmI)
            {
                Projectile.SetHeldProjectileInHand(Player, 9f);
                directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);

                if (!shotProjectile)
                {
                    ShootProjectiles();
                    shotProjectile = true;
                }

                Projectile.netUpdate = true;
            }

            Projectile.rotation = directionToMouse.ToRotation() + -recoil.Y * Player.direction;
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            recoil *= 0.92f;
        }

        private void ShootProjectiles()
        {
            Vector2 muzzlePosition = Projectile.Center + directionToMouse * 45f;
            float shootSpeed = Projectile.velocity.Length();
            for (int i = 0; i < 6; i++)
            {
                Vector2 newVelocity = directionToMouse.RotatedByRandom(MathHelper.ToRadians(15f)) * shootSpeed;
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);

                Projectile.NewProjectileDirect(
                    Projectile.GetSource_FromAI(), 
                    muzzlePosition, 
                    newVelocity, 
                    (int)Projectile.ai[0], 
                    Projectile.damage, 
                    Projectile.knockBack, 
                    Player.whoAmI
                ).netUpdate = true;
            }

            recoil += new Vector2(8f, 0.5f);
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
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 normOrigin = new Vector2(9, 2) + Vector2.UnitX * recoil.X;

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
