using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Content.Dusts;
using Malignant.Content.NPCs.Corruption.CursedOccultist;

namespace Malignant.Content.NPCs.Crimson.MutilatedFlesh
{
    public class MutilatedFlesh : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 24;
            NPC.damage = 18;
            NPC.defense = 4;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath12;
            NPC.value = 650f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 67;
            AIType = 360;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCrimson ? 0.50f : 0f;
        public override void HitEffect(NPC.HitInfo hit)
        {
            int num = NPC.life > 0 ? 1 : 5;
            for (int k = 0; k < num; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Blood>());
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            if (NPC.frameCounter > 8.0)
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y > frameHeight * 2)
                {
                    NPC.frame.Y = 0;
                }
            }
        }
    }
}