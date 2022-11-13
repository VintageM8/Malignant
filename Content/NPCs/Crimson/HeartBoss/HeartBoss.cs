using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.NPCs.Crimson.HeartBoss
{
    public class HeartBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("h");

            Main.npcFrameCount[Type] = 6;
		}

        public override void SetDefaults()
        {
			NPC.width = 45;
			NPC.height = 45;
			NPC.damage = 15;
			NPC.defense = 12;
			NPC.lifeMax = 4300;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;    
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.aiStyle = -1;
			NPC.boss = true;
			NPC.SpawnWithHigherTime(30);
		}

		Player Target => Main.player[NPC.target];

		enum AIPhase
        {
			Spawn,
			Fly,
			SpewBlood,
			Kamikaze
        }

        AIPhase aiPhase = HeartBoss.AIPhase.Spawn;

        int despawnTimer;
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();

                if (despawnTimer++ > 300)
                {
                    
                }
            }
            else
            {
                despawnTimer = 0;

                switch (aiPhase)
                {
                    case AIPhase.Spawn:
                        Spawn();
                        break;
                    case AIPhase.Fly:
                        Fly();
                        break;
                    case AIPhase.SpewBlood:
                        SpewBlood();
                        break;
                    case AIPhase.Kamikaze:
                        Kamikaze();
                        break;
                }
            }
        }

        void Spawn()
        {
            aiPhase = AIPhase.Fly;
        }

        void Fly()
        {

        }

        void SpewBlood()
        {

        }

        void Kamikaze()
        {

        }

        public override void FindFrame(int frameHeight)
        {
            switch (aiPhase)
            {
                default:
                    int speed = 10;

                    NPC.frameCounter++;
                    if (NPC.frameCounter >= Main.npcFrameCount[Type] * speed)
                    {
                        NPC.frameCounter = 0;
                    }
                    NPC.frame.Y = (int)NPC.frameCounter / speed * frameHeight;

                    break;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Vector2 origin = new Vector2(34, 28);

            Main.EntitySpriteDraw(
                tex,
                NPC.Center,
                NPC.frame,
                drawColor,
                NPC.rotation,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0
                );

            return false;
        }
    }
}
