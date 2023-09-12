using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.ComponentModel.Design.Serialization;
using Malignant.Common;
using Microsoft.Xna.Framework;

namespace Malignant.Content.NPCs.Crimson.Arterion
{
    [AutoloadBossHead]
    internal partial class Arterion : ComplexAINPC<Arterion.AIState>
    {
        internal enum AIState
        {
            SpawnAnimation,
            Phase1,
            Phase2,
            Fleeing
        }

        public override void SetDefaults2()
        {
            NPC.width = 100;
            NPC.height = 135;
            NPC.damage = Main.rand.Next(15, 25);
            NPC.defense = 12;
            NPC.lifeMax = 4300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noTileCollide = true;
            NPC.value = 60f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.scale = 1f;
            NPC.boss = true;
            NPC.knockBackResist = 0f;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Boss/Arterion");
            }
        }

        [StateMethod(AIState.SpawnAnimation)]
        private void DoSpawnAnimation()
        {
            if (StateTime.Seconds > 3f)
            {
                SetState(AIState.Phase1);
            }
        }

        [StateMethod(AIState.Fleeing)]
        private void DoFleeing()
        {
            if (StateTime.Seconds > 2f)
            {
                NPC.active = false;
            }
            else if (StateTime.Seconds > 0.35f)
            {
                NPC.velocity += Vector2.UnitY * 0.5f;
            }
            else
            {
                NPC.velocity -= Vector2.UnitY * 0.1f;
            }
        }
    }
}
