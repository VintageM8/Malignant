using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Spider.SpiderFangNecklace
{
    internal class SpiderFangNecklace : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToAccessory(30, 32);
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<SpiderNecklessPlayer>().SpiderNeckless = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SharkToothNecklace)
                .AddIngredient(ItemID.SpiderFang, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    class SpiderFangProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.timeLeft = 100;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Generic;
        }
        int count = 0;
        public override void AI()
        {
            if (count >= 10)
            {
                Projectile.velocity.Y += Projectile.velocity.Y < 20 ? .5f : 0;
            }
            count++;
        }
    }
    class SpiderNecklessPlayer : ModPlayer
    {
        public bool SpiderNeckless = false;
        public override void ResetEffects()
        {
            SpiderNeckless = false;
            base.ResetEffects();
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (SpiderNeckless)
            {
                target.AddBuff(BuffID.Venom, 60);
            }
        }
        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            if (SpiderNeckless)
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Main.rand.NextVector2Unit(-MathHelper.PiOver4, MathHelper.PiOver4), ModContent.ProjectileType<SpiderFangProjectile>(), 30, 1f, Player.whoAmI);
                }
            }
        }
    }
}
