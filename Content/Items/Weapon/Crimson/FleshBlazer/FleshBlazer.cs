using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Malignant.Content.Items.Weapon.Crimson.FleshBlazer
{
    public class FleshBlazer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell's Scourcher");
            Tooltip.SetDefault("Scorch your way through The Wretched\nUses gel as ammo\nAfter 8 shots you shoot out a bible to damn The Wretched");
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 44;
            Item.height = 16;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.crit = 0;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BlazerFlame>();
            Item.shootSpeed = 5f;
            Item.channel = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }

        private int FlameCount;
        public override void HoldItem(Player player)
        {
            if (!player.channel)
                FlameCount = 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 2;
            float rotation = MathHelper.ToRadians(5);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
            }
            float rot = velocity.ToRotation();
            float spread = 0.4f;

            Vector2 offset = new Vector2(1, -0.05f * player.direction).RotatedBy(rot);


            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item34 with { Volume = 1f, Pitch = Main.rand.NextFloat(0.5f, 2f), MaxInstances = 400 });

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(player.Center + offset * 70, DustID.Torch, Vector2.UnitY * -2 + offset.RotatedByRandom(spread) * 5, 0, new Color(60, 55, 50) * 0.5f, Main.rand.NextFloat(0.5f, 1));
            }
            FlameCount++;
            if (FlameCount >= 4)
            {
                for (int i = -2; i < 3; i++)
                    Projectile.NewProjectile(source, new Vector2(Main.MouseWorld.X + i * 25, Main.screenPosition.Y - 100), Vector2.UnitY * 1.5f, ModContent.ProjectileType<ScourcherBible>(), damage, knockback, player.whoAmI);
                FlameCount = 0;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.CrimtaneBar, 10)
                .AddIngredient(ItemID.TissueSample, 12)
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }
    }
}