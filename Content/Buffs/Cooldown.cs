using Terraria;
using Terraria.ModLoader;

namespace Malignant.Content.Buffs
{
    public class Cooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cooldown");
            Description.SetDefault("wait to use your abilty!");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
}
