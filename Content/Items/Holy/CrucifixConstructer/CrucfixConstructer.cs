using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Malignant.Common.Players;
using Malignant.Content.Buffs;
using Malignant.Content.Items.Crimson.FleshBlazer;
using Malignant.Content.Items.Misc;

namespace Malignant.Content.Items.Holy.CrucifixConstructer
{
    public class CrucfixConstructer : ModItem
    {

        public int AttackCounter = 1;
        public int combowombo = 0;
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.width = 0;
            Item.height = 0;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.knockBack = 4;
            Item.value = 10000;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Yellow;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<HammerThrow>();
            Item.shootSpeed = 10f;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {

            if (player.altFunctionUse == 2)
            {
                Vector2 dir = Vector2.Normalize(velocity) * 9;
                velocity = dir;
                for (int i = 0; i < 5; i++)
                {
                    type = ModContent.ProjectileType<Crucifix>();
                }
            }

        }

        public override bool CanUseItem(Player Player)
        {
            if (Player.altFunctionUse == 2)
            {
                Item.useStyle = ItemUseStyleID.Swing;
                Item.useTime = 15;
                Item.useAnimation = 15;
                Item.shootSpeed = 10f;
            }
            else
            {
                Item.useTime = 30;
                Item.useAnimation = 30;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.shootSpeed = 10f;
            }

            return base.CanUseItem(Player);
        }

        public override bool AltFunctionUse(Player Player)
        {
            return true;
        }

        public override void AddRecipes() //Temporary
        {
            CreateRecipe(1)
                .AddIngredient(ModContent.ItemType<BlessedMetal>(), 10)
                .AddIngredient(ModContent.ItemType<BrokenDemonHorn>(), 4)
                .AddIngredient(ItemID.GoldDust, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
