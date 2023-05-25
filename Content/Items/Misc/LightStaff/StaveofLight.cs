using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc.LightStaff
{
    public class StaveofLight : ModItem
    {
        float Counter = 0;
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stave of Light");
            //Tooltip.SetDefault("Summons crosses that shoots a homing fireball");
        }
        public override void SetDefaults()
        {
            Item.damage = 62;
            Item.crit = 8;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.width = 36;
            Item.DamageType = DamageClass.Magic;
            Item.height = 38;
            Item.useTime = 17;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = 10000;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LightCross>();
            Item.shootSpeed = 0f;
            Item.staff[Item.type] = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3 + (Main.expertMode ? 1 : 0); i++)
            {
                Vector2 toLocation = player.Center + new Vector2(Main.rand.NextFloat(100, 240), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    damage = Item.damage;
                    Projectile.NewProjectile(source, toLocation, Vector2.Zero, ModContent.ProjectileType<LightCross>(), damage, 0, Main.myPlayer, player.whoAmI);
                }
                Vector2 toLocationVelo = toLocation - player.Center;
                Vector2 from = player.Center;
                for (int j = 0; j < 300; j++)
                {
                    Vector2 velo = toLocationVelo.SafeNormalize(Vector2.Zero);
                    from += velo * 12;
                    Vector2 circularLocation = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(j * 12 + Counter));

                    int dust = Dust.NewDust(from + new Vector2(-4, -4) + circularLocation, 0, 0, DustID.Gold, 0, 0, 0, default, 1.25f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.1f;
                    Main.dust[dust].scale = 1.8f;

                    if ((from - toLocation).Length() < 24)
                    {
                        break;
                    }
                }
            }
            return false;
        }

    }
}