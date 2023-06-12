using Terraria;
using Terraria.ModLoader;

namespace Malignant.Content.Buffs
{
    public class SicklyPower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sickly Power");
            // Description.SetDefault("God does not approve of how you gained this");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {       
            player.moveSpeed += 0.05f;
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetCritChance(DamageClass.Generic) += 0.05f;
        }
    }
}
