using Terraria;
using Terraria.ModLoader;

namespace Malignant.Content.Buffs
{
    public class NoMove : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.velocity.Y < 0)
                player.velocity.Y *= 0.6f;
            if (player.velocity.Y < -2)
            {
                player.velocity.Y = 0;
                player.jump = 0;
            }

            player.velocity.X *= 0.8f;
        }
    }
}
