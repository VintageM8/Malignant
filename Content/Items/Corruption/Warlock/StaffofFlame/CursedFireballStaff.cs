using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace Malignant.Content.Items.Corruption.Warlock.StaffofFlame
{
    public class CursedFireballStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stave of Cursed Flame");
            //Tooltip.SetDefault("Shoots out explodiong cursed fire balls\nRight click to denotate them early");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightRed;
            Item.mana = 12;
            Item.UseSound = SoundID.Item21;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 59;
            Item.channel = true;
            Item.autoReuse = true;
            Item.useAnimation = 20;
            Item.useTime = 12;
            Item.width = 50;
            Item.height = 56;
            Item.shoot = ModContent.ProjectileType<CursedFB>();
            Item.shootSpeed = 10f;
            Item.knockBack = 6f;
            Item.DamageType = DamageClass.Magic;
            Item.value = Item.sellPrice(gold: 1, silver: 75);
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == Item.shoot && player.ownedProjectileCounts[Item.shoot] > 0)
                    {
                        Main.projectile[i].Kill();
                    }
                }

                Item.shoot = ProjectileID.None;
                Item.useTime = Item.useAnimation = 40;
                Item.UseSound = SoundID.DD2_KoboldIgnite;
            }
            else
            {
                Item.shoot = ModContent.ProjectileType<CursedFB>();
                Item.useTime = Item.useAnimation = 10;
                Item.UseSound = SoundID.DD2_LightningAuraZap;
            }

            return player.ownedProjectileCounts[Item.shoot] < 10;
        }
        public override bool AltFunctionUse(Player Player)
        {
            return true;
        }
    }
}