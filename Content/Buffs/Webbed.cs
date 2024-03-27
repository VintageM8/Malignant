using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Malignant.Content.Dusts;
using ParticleLibrary;
using Terraria.ID;

namespace Malignant.Content.Buffs
{
    public class Webbed : ModBuff
    {
        public override void Update(NPC NPC, ref int buffIndex)
        {
            if (NPC.velocity.Y < 0)
                NPC.velocity.Y *= 0.6f;
            if (NPC.velocity.Y < -2)
            {
                NPC.velocity.Y = 0;
            }

            NPC.velocity.X *= 0.8f;

            Dust.NewDustPerfect(new Vector2(NPC.position.X + Main.rand.Next(NPC.width), NPC.position.Y + NPC.height - Main.rand.Next(7)), DustID.SilverCoin, Vector2.Zero);
            for (int m = 0; m < 20; m++)
            {
                Vector2 position = new Vector2(NPC.position.X + Main.rand.Next(NPC.width), NPC.position.Y + NPC.height - Main.rand.Next(7));
                Particle p = ParticleManager.NewParticle(position, new Vector2(0, -2f), ParticleManager.NewInstance<StarParticle>(),
                    default(Color), Main.rand.NextFloat(0.2f, 1));
                p.layer = Particle.Layer.BeforePlayersBehindNPCs;
            }

        }
    }
}
