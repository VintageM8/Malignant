using Malignant.Assets.Textures;
using Malignant.Common.Helper;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Accessories
{
    internal class ShieldOfRighteousness : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 3;
            player.noKnockback = true;
            base.UpdateAccessory(player, hideVisual);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CobaltShield)
                .AddIngredient(ItemID.GoldBar, 18)
                .AddIngredient(ItemID.LightShard, 2)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    class ShieldOfRighteousnessPlayer : ModPlayer
    {
        public override void OnHurt(Player.HurtInfo info)
        {
            base.OnHurt(info);
            if(Player.statLife < (int)(Player.statLifeMax * .5f))
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Main.rand.NextVector2Circular(10f, 10f), ModContent.ProjectileType<HolyCross>(), 83, 1f, Player.whoAmI);
            }
        }
    }
    class HolyCross : ModProjectile
    {
        public override string Texture => MalignantTexture.MISSINGTEXTURE;
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }
        public override void AI()
        {
            if(Projectile.velocity != Vector2.Zero)
            {
                Projectile.velocity -= Projectile.velocity * .05f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.EvenEasierDrawTrail(lightColor, .1f);
            return base.PreDraw(ref lightColor);
        }
    }
}
