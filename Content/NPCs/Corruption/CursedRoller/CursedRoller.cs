using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.NPCs.Corruption.CursedRoller
{
    public class CursedRoller : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new(0)
            {
                Position = new(0, -5f),
                PortraitPositionYOverride = -20f
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 60;
            NPC.defense = 24;
            NPC.lifeMax = 140;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            // npc.noGravity = false;
            // npc.noTileCollide = false;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 26;
            AIType = NPCID.Unicorn;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCorrupt && spawnInfo.Player.ZoneOverworldHeight ? .075f : 0f;

        public override void AI()
        {
            NPC.rotation += NPC.velocity.X * 0.05f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            //Add Stuff here
        }
    }
}
