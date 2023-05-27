using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Malignant.Content.Items.Crimson.Arterion.HerzanfallDagger
{
    class HerzanfallDagger : ModItem
    {
        int charge = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Heart's Skean");
        }

        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 32;
            Item.damage = 62;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.crit = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Lime;
            Item.channel = true;
        }

        public override void HoldItem(Player player)
        {
            if (player.channel)
            {
                //player.itemAnimation = player.itemAnimationMax - 1;

                if (charge % 30 == 0 && charge < 90)
                {
                    int index = charge / 30;
                    float rot = MathHelper.Pi / 3f * index - MathHelper.Pi / 3f;
                    var pos = player.Center + Vector2.UnitY.RotatedBy(rot) * -45;
                    int i = Projectile.NewProjectile(player.GetSource_ItemUse(Item), pos, Vector2.Zero, ProjectileType<KnifeProjectile>(), Item.damage, Item.knockBack, player.whoAmI, 0, charge);
                    Main.projectile[i].frame = index;

                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, player.Center);
                }
                charge++;
            }

            else charge = 0;
        }
    }
}
