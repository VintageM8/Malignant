using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Malignant.Content.Dusts;

namespace Malignant.Content.Buffs
{
    public class SmokeDebuff : ModBuff
    {
        public override void Update(NPC NPC, ref int buffIndex)
        {
            Vector2 vel = new Vector2(0, -1).RotatedByRandom(0.5f) * 0.4f;
            if (Main.rand.NextBool(4))
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Smoke>(), vel.X, vel.Y, 0, new Color(60, 55, 50) * 0.5f, Main.rand.NextFloat(0.5f, 1));
        }
    }
}