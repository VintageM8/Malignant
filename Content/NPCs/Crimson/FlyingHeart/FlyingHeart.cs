using Microsoft.Xna.Framework;
using System;
using Terraria;
using Malignant.Core;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.NPCs.Crimson.FlyingHeart
{
    public class FlyingHeart : ModNPC
    {
        public enum Attacks { Swing = 1, Throw = 2 }

        protected int MoveDirection;
        protected Vector2 InitialPosition;
        protected bool SlashPlayer;

        public bool ThrewSword = true;

        public override bool CheckActive() => false;

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 90;
            NPC.damage = 26;
            NPC.defense = 9;
            NPC.lifeMax = 200;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath53;
            NPC.value = 60f;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(MoveDirection);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                MoveDirection = reader.ReadInt32();
            }
        }

        public enum AIStates
        {
            Float = 0,
            Swing = 1,
            Throw = 2
        }

        public ref float AICase => ref NPC.ai[0];
        public ref float AICounter => ref NPC.ai[1];
        public ref float MiscCounter => ref NPC.ai[2];

        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            switch (AICase)
            {
                case (int)AIStates.Float:

                    if (AICounter++ == 180)
                    {

                        InitialPosition = NPC.Center;
                        NPC.velocity = Vector2.Zero;

                        MoveDirection = Main.rand.NextBool() ? -1 : 1;

                        Attacks[] RandomAttack = (Attacks[])Enum.GetValues(typeof(Attacks));
                        RandomAttack.Shuffle();

                        AICase = (int)RandomAttack[0];
                        AICounter = 0;
                        MiscCounter = 0;

                        NPC.netUpdate = true;
                    }
                    break;
                case (int)AIStates.Swing:
                    // Move backwards while disappearing
                    if (AICounter++ <= 60)
                    {
                        Vector2 PositionOffset = Vector2.UnitX * 50 * MoveDirection;
                        NPC.Center = Vector2.Lerp(InitialPosition, InitialPosition + PositionOffset, AICounter / 60f);
                        NPC.alpha = (int)MathHelper.Lerp(0, 255, Utils.Clamp(AICounter, 0, 45) / 45f);
                    }

                    // Lunge forward with a swing
                    if (AICounter > 60 && AICounter < 180)
                    {
                        NPC.direction = MoveDirection;

                        // Determine which side of the player to 
                        if (AICounter < 150)
                        {
                            NPC.Center = player.Center + Vector2.UnitX * 75 * -MoveDirection;
                            NPC.alpha = (int)MathHelper.Lerp(255, 0, (AICounter - 60f) / 90f);
                        }
                        else
                        {
                            if (AICounter == 170)
                            {
                                NPC.velocity = Vector2.UnitX * 10 * NPC.direction;
                            }
                        }
                    }

                    if (AICounter == 180)
                    {
                        NPC.velocity = Vector2.Zero;

                        AICase = (int)AIStates.Float;
                        AICounter = 0;
                    }
                    break;
                case (int)AIStates.Throw:
                    //Main.NewText("throw");

                    if (AICounter++ == 120)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 2, ProjectileID.BloodNautilusShot, 30, 3f, Main.myPlayer, 0, NPC.whoAmI);
                    }

                    if (AICounter == 180)
                    {
                        AICase = (int)AIStates.Float;
                        AICounter = 0;
                    }
                    break;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement("Hearts of the damned sinners, now reserected and out for vengance against the Lord who trapped them in eternal suffering."),
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCrimson && spawnInfo.Player.ZoneOverworldHeight ? .075f : 0f;

       

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 3));           
        }
    }
}
