using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Content.NPCs.Crimson.Heart;
using Malignant.Common.Systems;
using Malignant.Content.Items.Hell.MarsHell;

namespace Malignant.Content.NPCs.Crimson.FlyingHeart
{
    public class FlyingHeart : ModNPC
    {
        enum StateID
        {
            Spawn,
            Floating,
            GroundPound,
            HitGround,
        };

        StateID state = StateID.Spawn;

        float counter;

        float timer;

        float heightMod = 1f;

        float widthMod = 1f;

        float maxVelocity = 5f;

        public override void SetStaticDefaults()
        {
            //DisplayName.AutoStaticDefaults("Flying Heart");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 44;
            NPC.damage = 25;
            NPC.defense = 5;
            NPC.knockBackResist = 0.2f;
            NPC.value = 90;
            NPC.lifeMax = 45;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath21;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = -NPC.direction;
            NPC.frameCounter++;
            if ((NPC.velocity.X != 0f && NPC.velocity.Y == 0f))
            {
                if (NPC.frameCounter >= 6)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y = ((NPC.frame.Y + 1) % 5) * frameHeight;
                }
            }
            else
                NPC.frame.Y = 2 * frameHeight;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement("Hearts of the damned sinners, now reserected and out for vengance against the Lord who trapped them in eternal suffering."),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCrimson && spawnInfo.Player.ZoneOverworldHeight ? .075f : 0f;

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
                    NPC.velocity.Y -= 5f;
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

                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<MarsHellBoom>(), 0, 1f, Main.myPlayer);
                widthMod = 2f;
                heightMod = 0.5f;

                state = StateID.Floating;

            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 3));           
        }
    }
}
