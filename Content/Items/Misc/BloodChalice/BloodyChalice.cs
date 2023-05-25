using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Misc.BloodChalice
{
    public class BloodyChalice : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloody Chalice");
            T//ooltip.SetDefault("Heals for 200 life but applies a random debuff each use\n Has unlimited uses\n[c/660000:Cursed Items:]");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.rare = ItemRarityID.Expert;
            Item.healLife = 220;
            Item.potion = true;
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }

        public override void OnConsumeItem(Player player)
        {

            if (Main.rand.NextBool(5))
            {
                int buffType = 0;
                switch (Main.rand.Next(4))
                {
                    case 0:
                        buffType = BuffID.OnFire;
                        break;
                    case 1:
                        buffType = BuffID.Poisoned;
                        break;
                    case 2:
                        buffType = BuffID.Confused;
                        break;
                    case 4:
                        buffType = BuffID.Frostburn;
                        break;
                    default:
                        break;
                }
                player.AddBuff(buffType, (int)(60 * Main.rand.NextFloat(2f, 5f)));
            }
        }
    }
}