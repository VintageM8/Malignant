﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;

namespace Malignant.Content.NPCs.Corruption.Shreiker
{
    public class Shreiker : ModNPC
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shreiker");
            Main.npcFrameCount[NPC.type] = 5;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 94;
            NPC.height = 408 / 6;
            NPC.damage = 52;
            NPC.defense = 15;
            NPC.lifeMax = 1800;
            NPC.HitSound = SoundID.NPCHit21;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.value = Item.buyPrice(0, 5, 0, 0);
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement("Once a fallower of Our Lord, now a lifeliss husk that lurks around Terraria, mf cant even get a job."),
            });
        }

        int frame = 0;
        bool dashing = false;

        public override void FindFrame(int frameHeight)
        {
            if (!dashing)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter > 5)
                {
                    NPC.frameCounter = 0;
                    frame++;
                }
                if (frame >= 5)
                {
                    frame = 0;
                }
            }
        }

        int AIPhase = 0;
        int attackCounter = 0;
        int aliveCounter = 0;
        bool start = true;
        Player player;
        public override void AI()
        {
            if (start)
            {
                NPC.TargetClosest();
                player = Main.player[NPC.target];
                start = false;
            }

            aliveCounter++;
            if (aliveCounter % 600 == 120)
            {
                WeightedRandom<string> messages = new WeightedRandom<string>();
                messages.Add("Hisssss...");
                messages.Add("*cursing*");
                messages.Add("Murrderrrr...");
                CombatText.NewText(NPC.getRect(), Color.Red, messages, true);
            }

            if (AIPhase == 0)
            {
                attackCounter++;
                NPC.rotation = MathHelper.ToRadians(NPC.velocity.X);
                NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
                NPC.velocity += NPC.DirectionTo(player.Center) * 0.5f;
                NPC.velocity *= 0.96f;
                if (attackCounter > 180)
                {
                    attackCounter = 0;
                    AIPhase = 1;
                    NPC.velocity = NPC.DirectionTo(player.Center) * 20f;
                    NPC.rotation = NPC.DirectionTo(player.Center).ToRotation();
                    if (NPC.spriteDirection == -1) NPC.rotation += MathHelper.ToRadians(180);
                    dashing = true;
                }
            }
            else if (AIPhase == 1)
            {
                attackCounter++;
                NPC.velocity *= 0.98f;
                NPC.velocity.Y += 0.1f;
                if (attackCounter > 60)
                {
                    AIPhase = 2;
                    attackCounter = 0;
                    dashing = false;
                }
            }
            else if (AIPhase == 2)
            {
                attackCounter++;
                NPC.rotation = MathHelper.ToRadians(NPC.velocity.X);
                NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
                if (attackCounter > 60)
                {
                    attackCounter = 0;
                    AIPhase = 0;
                }
                NPC.velocity *= 0.93f;
            }
        }
    }
}
