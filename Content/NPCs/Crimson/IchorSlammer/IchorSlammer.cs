using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Malignant.Common.Systems;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;
using System;
using Terraria.GameContent;
using System.Drawing;

namespace Malignant.Content.NPCs.Crimson.IchorSlammer
{
    public class IchorSlammer : ModNPC
    {
        enum StateID
        {
            Spawn,
            Floating,
            GroundPound,
            HitGround,
            Stunned
        };

        StateID state = StateID.Spawn;

        float counter;

        float timer;

        float heightMod = 1f;

        float widthMod = 1f;

        float maxVelocity = 5f;

        public override void SetStaticDefaults()
        {

            //NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0) { Velocity = 1f };
            //NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 100;
            NPC.damage = 40;
            NPC.defense = 15;
            NPC.knockBackResist = 0f;

            NPC.width = 80;
            NPC.height = 80;
            NPC.scale = 1f;

            NPC.HitSound = SoundID.DD2_LightningBugHurt;
            NPC.DeathSound = SoundID.DD2_LightningBugDeath;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.sellPrice(0, 0, 0, 90);

            NPC.aiStyle = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Visuals.Moon,
                new FlavorTextBestiaryInfoElement("God I wish I was you, this mf has a FAT ass."),
            });
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];

            if (state == StateID.Spawn)
            {
                state = StateID.Floating;
            }

            NPC.TargetClosest(true);
            //Lighting.AddLight(NPC.Center, new Color(241, 212, 62).ToVector3() * 0.5f);

            if (state == StateID.GroundPound)
            {
                NPC.noTileCollide = false;
                NPC.damage = 100;

                NPC.velocity.X *= 0.7f;
                NPC.velocity.Y += 0.5f;


                if (NPC.velocity.Y >= 0f)
                {
                    heightMod = 1f + (NPC.velocity.Length() * 0.1f);
                    widthMod = 1f - (NPC.velocity.Length() * 0.1f);
                }

                if (heightMod >= 2f)
                {
                    heightMod = 2f;
                }

                if (widthMod <= 0.5f)
                {
                    widthMod = 0.5f;
                }

                if (NPC.collideY)
                {
                    state = StateID.HitGround;
                    SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, NPC.Center);
                }
            }

            if (state == StateID.Floating)
            {
                NPC.noTileCollide = true;
                NPC.damage = 0;

                NPC.spriteDirection = NPC.direction;

                NPC.velocity += ((player.Center + new Vector2(0, -200)) - NPC.Center) * 0.02f;

                if (Math.Abs(NPC.velocity.X) >= maxVelocity)
                {
                    NPC.velocity.X = maxVelocity * Math.Sign(NPC.velocity.X);
                }

                if (Math.Abs(NPC.velocity.Y) >= maxVelocity)
                {
                    NPC.velocity.Y = maxVelocity * Math.Sign(NPC.velocity.Y);
                }

                NPC.ai[0]++;

                if (NPC.ai[0] >= 60f)
                {
                    NPC.velocity.Y -= 10f;
                    NPC.ai[0] = 0f;


                    SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);
                    state = StateID.GroundPound;
                   
                }

                if (NPC.Distance(player.Center) >= 300f)
                {
                    NPC.ai[0] = 0f;
                }
            }

            if (state == StateID.HitGround)
            {
                CameraSystem.ScreenShakeAmount = 3f;

                widthMod = 2f;
                heightMod = 0.5f;

                state = StateID.Floating;

            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && !Main.dayTime)
                return 0.15f;
            return 0;
        }
    }
}
