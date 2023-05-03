using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Crimson.StaffofCarnem
{
    public class StaffofCarnem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Staff of Carnem");
            Tooltip.SetDefault("Pending Re-work");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.width = 62;
            Item.height = 62;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 8, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.channel = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item24;
            Item.shoot = ModContent.ProjectileType<CarnemProj>();
            Item.shootSpeed = 10f;
            Item.mana = 28;
        }
        //This is a small charging system that i made
        int count = 0;
        public override void HoldItem(Player player)
        {
            if (Main.mouseRight)
            {
                ++count;
                Vector2 Position = Main.rand.NextVector2CircularEdge(10, 10) + player.Center;
                Vector2 velocityUnNormalize = player.Center - Position;
                int dust = Dust.NewDust(Position, 0, 0, DustID.AmberBolt, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.2f));
                Main.dust[dust].fadeIn = 1f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = velocityUnNormalize.SafeNormalize(Vector2.Zero) * velocityUnNormalize.Length();
                if (count >= 4 && player.GetModPlayer<PlayerCharge>().ChargePower <= 20)
                {
                    player.GetModPlayer<PlayerCharge>().ChargePower++;
                    count = 0;
                }
            }
            if (Main.mouseRightRelease)
            {
                player.GetModPlayer<PlayerCharge>().ChargePower = 0;
                count = 0;
            }
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            damage += (int)player.GetModPlayer<PlayerCharge>().ChargePower;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<PlayerCharge>().ChargePower <= 20)
            {
                return base.Shoot(player, source, position, velocity, type, damage, knockback);
            }
            for (int i = 0; i < 30; i++)//PlaceHolder dust
            {
                int dust = Dust.NewDust(player.Center, 0, 0, DustID.AmberBolt, 0, 0, 0, default, Main.rand.NextFloat(.9f, 1.2f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(10, 10);
            }
            player.GetModPlayer<PlayerCharge>().ChargePower = 0;
            float rotation = MathHelper.ToRadians(30);
            for (int i = 0; i < 5; i++)
            {
                Vector2 Rotate = velocity.RotatedBy(MathHelper.Lerp(rotation, -rotation, i * .2f));
                Projectile.NewProjectile(source, position, Rotate, type, damage, knockback, player.whoAmI);
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
    class PlayerCharge : ModPlayer
    {
        public float ChargePower = 0;
    }
}
