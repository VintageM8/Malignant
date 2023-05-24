using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Crimson.Arterion.BloodyVein
{
    public class BloodyVein : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<BloodyVeinProj>(), 20, 2, 4);

            Item.shootSpeed = 3;
            Item.rare = ItemRarityID.Red;

            Item.channel = false;
        }

        public override bool MeleePrefix()
        {
            return true;
        }
    }
}