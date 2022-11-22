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
            Item.DefaultToWhip(ModContent.ProjectileType<CorruptedCordProj>(), 20, 2, 4);

            Item.shootSpeed = 3;
            Item.rare = ItemRarityID.Green;

            Item.channel = false;
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}
