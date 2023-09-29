using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Malignant.Assets.Textures;
using Terraria.DataStructures;

namespace Malignant.Content.Items.Snow.Cocytus.ForgottenFrost
{
    internal class ForgottenFrost : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 24;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<ForgottenFrostProjectile>();
            Item.shootSpeed = 12;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item11;
            Item.useAmmo = AmmoID.Snowball;
        }
        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ForgottenFrostProjectile>()] < 8)
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ForgottenFrostProjectile>(), damage, knockback, player.whoAmI);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
    class ForgottenFrostProjectile : ModProjectile
    {
        public override string Texture => MalignantTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 999;
            Projectile.tileCollide = true;
        }
        Vector2 FakeVelocity = Vector2.Zero;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 15f;
                if (Vector2.DistanceSquared(Main.MouseWorld, Projectile.Center) <= 100 * 100)
                    Projectile.ai[0]++;
            }
            if (Projectile.ai[0] == 1)
            {
                FakeVelocity = Projectile.Center - Main.MouseWorld;
                Projectile.ai[0]++;
            }
            if (Projectile.ai[0] == 2)
            {
                Projectile.Center = Main.MouseWorld + FakeVelocity.RotatedBy(MathHelper.ToRadians(++Projectile.ai[1] * 5));
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
                target.AddBuff(BuffID.Frostburn, 300);
        }
    }
}