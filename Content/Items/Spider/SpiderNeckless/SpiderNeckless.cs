using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Spider.SpiderNeckless
{
    [AutoloadEquip(EquipType.Front)]
    internal class SpiderNeckless : ModItem
    {
        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("Inflict Venom");
        }
        public override void SetDefaults()
        {
            Item.DefaultToAccessory(30, 32);
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<SpiderFangNecklessPlayer>().SpiderFangNeckless = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SharkToothNecklace)
                .AddIngredient(ItemID.SpiderFang, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    class SpiderFangProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 100;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Generic;
        }
        int count = 0;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (count >= 10)
            {
                Projectile.velocity.Y += Projectile.velocity.Y < 20 ? .5f : 0;
            }
            count++;
            Projectile.alpha += 2;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Venom, 60);
        }
    }
    class SpiderFangNecklessPlayer : ModPlayer
    {
        public bool SpiderFangNeckless = false;
        public override void ResetEffects()
        {
            SpiderFangNeckless = false;
            base.ResetEffects();
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (SpiderFangNeckless)
            {
                target.AddBuff(BuffID.Venom, 60);
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (SpiderFangNeckless)
            {
                for (int i = 0; i < 5; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center,
                        Main.rand.NextVector2Unit(-MathHelper.PiOver2 - MathHelper.PiOver4, MathHelper.PiOver4) * Main.rand.NextFloat(7f, 10f)
                        , ModContent.ProjectileType<SpiderFangProjectile>(), 30, 1f, Player.whoAmI);
                }
            }
        }
    }
}
