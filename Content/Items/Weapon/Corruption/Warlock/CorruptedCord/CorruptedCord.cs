using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Weapon.Corruption.Warlock.CorruptedCord
{
    public class CorruptedCord : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {

            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 32;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Green;

            Item.shoot = ModContent.ProjectileType<CorruptedCordProj>();
            Item.shootSpeed = 4;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item152;
            //Item.channel = true; 
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
