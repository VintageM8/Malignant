using Microsoft.Xna.Framework;
using Terraria;
using Malignant.Content.Buffs.Summon;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc.LoyalLens
{
    public class LoyalLens : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.knockBack = 3f;
            Item.mana = 10;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = 1;
            Item.value = Item.buyPrice(0, 30);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;

            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<LoyalLensBuff>();
            Item.shoot = ModContent.ProjectileType<LoyalLensSummon>();
        }
    }
}