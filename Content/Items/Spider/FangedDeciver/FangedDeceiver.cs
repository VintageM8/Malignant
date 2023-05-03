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
using Malignant.Common.Projectiles;

namespace Malignant.Content.Items.Spider.FangedDeciver
{
    public class FangedDeceiver : HeldGunModItem
    {
        public override (float centerYOffset, float muzzleOffset, Vector2 drawOrigin, Vector2 recoil) HeldProjectileData => (9f, 45f, new Vector2(9, 2), new Vector2(8f, 0.5f));

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
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;

            Item.noUseGraphic = true;
        }

        private int shotCount;
        private int fangCount = 4;
        public override void ShootGun(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15f));
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);

                Projectile.NewProjectileDirect(
                    source,
                    position,
                    newVelocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI
                ).netUpdate = true;
            }

            if (++shotCount > 4)
            {
                foreach (Vector2 fangDirection in Vector2.Zero.GenerateCircularPositions(1f, fangCount))
                {
                    Projectile.NewProjectileDirect(
                        source,
                        player.Center,
                        fangDirection * Item.shootSpeed,
                        ProjectileType<SpiderFangProjectile>(),
                        damage * 2,
                        0f,
                        player.whoAmI
                    ).netUpdate = true;
                }

                shotCount = 0;
            }
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
}
