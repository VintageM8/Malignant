using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common;

namespace Malignant.Content.Items.Weapon.Njor.NjorSword
{
    public class IceSword : ModItem
    {
        public int AttackCounter = 1;
        public int combowombo = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cerberus"); 
            Tooltip.SetDefault("'Hitting enemies channels ancient nordic energy, increasing damage and attack speed'");
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Melee;
            Item.width = 0;
            Item.height = 0;
            Item.useTime = 100;
            Item.useAnimation = 100;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4;
            Item.value = 10000;
            Item.noMelee = true;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<NjorSwordProj>();
            Item.shootSpeed = 20f;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.GetModPlayer<MalignantPlayer>().itemCombo >= 0)
            {
                type = ModContent.ProjectileType<NjorSwordProj>();
                damage = 16;
            }
            if (player.GetModPlayer<MalignantPlayer>().itemCombo >= 4)
            {
                type = ModContent.ProjectileType<NjorSwordProj2>();
                damage = 24;
            }
            if (player.GetModPlayer<MalignantPlayer>().itemCombo >= 10)
            {
                type = ModContent.ProjectileType<NjorSwordProj3>();
                damage = 28;
            }

            if (player.GetModPlayer<MalignantPlayer>().itemCombo >= 16)
            {
                type = ModContent.ProjectileType<NjorSwordProj4>();
                damage = 32;
                SoundEngine.PlaySound(SoundID.Item34, player.position);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int dir = AttackCounter;
            /*if (player.direction == 1)
            {
                player.GetModPlayer<CorrectSwing>().SwingChange = (int)AttackCounter;
            }
            else
            {
                player.GetModPlayer<CorrectSwing>().SwingChange = (int)AttackCounter * -1;

            }*/
            AttackCounter = -AttackCounter;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1, dir);

            return false;
        }
    }
}