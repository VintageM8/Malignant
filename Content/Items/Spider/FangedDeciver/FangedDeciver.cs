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

namespace Malignant.Content.Items.Spider.FangedDeciver
{
    public class FangedDeciver : ModItem
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
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.useAmmo = AmmoID.Bullet;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item36;
            Item.rare = ItemRarityID.LightRed;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileID.Bullet;
            Item.channel = true;
        }

        private int ShotCount;
        public override void HoldItem(Player player)
        {
            if (!player.channel)
                ShotCount = 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int NumProjectiles = 6;

            for (int i = 0; i < NumProjectiles; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);

                Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }
            ShotCount++;
            if (ShotCount >= 4)
            {
                int i = 0;
                float spread = 10f * 0.0174f;
                double startAngle = Math.Atan2(6, 6) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

                Projectile.NewProjectile(source, player.Center.X, player.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<DepravedBlast_Proj>(), 10, 0f, player.whoAmI);
                ShotCount = 0;
            }
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            if (NPC.downedMechBossAny)
                Item.damage = 49;

            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                Item.damage = 52;
            }

            if (NPC.downedPlantBoss)
                Item.damage = 58;

            if (NPC.downedGolemBoss)
            {
                Item.damage = 62;
            }
            if (NPC.downedFishron)
                Item.damage = 67;

            if (NPC.downedAncientCultist)
                Item.damage = 75;

            if (NPC.downedMoonlord)
                Item.damage = 82;
          
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
