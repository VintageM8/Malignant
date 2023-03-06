using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Malignant.Common.Projectiles;

namespace Malignant.Content.Items.Misc.WarbladeofLight
{
    public class WarbladeofLight : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tranquility, Warblade of Light");
            Tooltip.SetDefault("Gains power as you progress\n[c/eeff00f:Chosen Item]");
        }

        public int AttackCounter = 1;
        public int combowombo = 0;
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.width = 0;
            Item.height = 0;
            Item.useAnimation = 7;
            Item.useTime = 7;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 4;
            Item.value = 10000;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Yellow;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WarbladeSwing>();
            Item.shootSpeed = 12f;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            int dir = AttackCounter;
            AttackCounter = -AttackCounter;

            player.GetModPlayer<MalignantPlayer>().itemCombo++;
            player.GetModPlayer<MalignantPlayer>().itemComboReset = 600;
            if (player.GetModPlayer<MalignantPlayer>().itemCombo <= 2 || player.GetModPlayer<MalignantPlayer>().itemCombo == 9)
            {
                Item.UseSound = SoundID.Item1;
                Projectile.NewProjectile(null, position, velocity * 10, ModContent.ProjectileType<WarbladeSwing>(), damage, knockback, player.whoAmI, 1, dir);


            }
            if (player.GetModPlayer<MalignantPlayer>().itemCombo == 4)
            {
                Projectile.NewProjectile(null, position, velocity / 5, ModContent.ProjectileType<WarbladeThrow>(), damage, knockback, player.whoAmI);
                Item.UseSound = SoundID.Item1;
                player.GetModPlayer<MalignantPlayer>().itemCombo = 0;

            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
}
