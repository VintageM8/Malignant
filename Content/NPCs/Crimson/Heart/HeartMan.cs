using Malignant.Content.NPCs.Crimson.Heart.Projectiles;
using Malignant.Common.Systems;
using Malignant.Content.Items.Crimson.Arterion.MoniterAccessory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles;
using Malignant.Content.Projectiles.Enemy;
using Malignant.Content.Items.Hell.MarsHell;
using System.IO;
using Terraria.Audio;
using Terraria.Utilities;
using Malignant.Core;

namespace Malignant.Content.NPCs.Crimson.Heart
{
    public class HeartMan : ModNPC //its not even a man lol
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Viscera");
            //Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 102;
            NPC.lifeMax = 9800;
            NPC.defense = 20;
            NPC.damage = 45;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.value = 1200f;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            //NPC.netAlways = true;
            //NPC.chaseable = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Boss/Viscera");

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartMoniter>(), 1));

        }

        float deathAlpha;
        bool ded;
        Vector2 pointOfInterest;
        public float dustNum = 0;
        public float dustHeight = -40;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(pointOfInterest);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            pointOfInterest = reader.ReadVector2();
        }
        public override bool CheckDead()
        {
            if (NPC.life <= 0 && !ded)
            {
                NPC.life = 1;
                AIState = Death;
                NPC.frameCounter = 0;
                NPC.immortal = true;
                NPC.dontTakeDamage = true;
                //CameraSystem.ChangeCameraPos(NPC.Center, 300);
                CameraSystem.ScreenShakeAmount = 20;
                ded = true;
                AITimer = AITimer2 = 0;
                NPC.velocity = Vector2.Zero;
                NPC.life = 1;
                return false;
            }
            return true;
        }
        public float AIState
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        public float AITimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        public float AITimer2
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public float AITimer3
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        const int Death = -1, Spawn = 0, Idle = 1, FlameThrower = 2, RocksAtPlayer = 3, RockFall = 4, RandomLasers = 5, SpinDash = 6, RepeatedBanging = 7;
        Vector2 lastPos;
        //SoundStyle summon = new("EbonianMod/Sounds/ExolSummon");
        /*SoundStyle roar = new("EbonianMod/Sounds/ExolRoar")
        {
            PitchVariance = 0.25f,
        };*/
        public override void AI()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(false);
            if (player.dead || !player.active || !player.ZoneCrimson)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (NPC.HasValidTarget)
                {
                    AIState = Spawn;
                    AITimer = 0;
                }
                if (player.dead || !player.active || !player.ZoneCrimson)
                {
                    NPC.velocity.Y = 30;
                    NPC.timeLeft = 10;
                    NPC.active = false;
                }
                return;
            }
            if (Main.rand.NextBool(5))
                /*if (NPC.life < NPC.lifeMax / 4) To do later on
                {
                    Helper.DustExplosion(NPC.Center, Vector2.One, 2, Color.Gray * 0.45f, false, false, 0.6f, 0.5f, new(Main.rand.NextFloat(-4, 4), -10));
                }
                else if (NPC.life < NPC.lifeMax / 3)
                {
                    Helper.DustExplosion(NPC.Center, Vector2.One, 2, Color.Gray * 0.35f, false, false, 0.4f, 0.5f, -Vector2.UnitY * Main.rand.NextFloat(6, 8));
                }
                else if (NPC.life < NPC.lifeMax / 2)
                {
                    Helper.DustExplosion(NPC.Center, Vector2.One, 2, Color.Gray * 0.25f, false, false, 0.2f, 0.5f, -Vector2.UnitY * Main.rand.NextFloat(4, 8));
                }*/

            if (AIState == Death)
            {
                
            }
            else if (AIState == Spawn) //This doesnt work for some reason
            {
                AITimer++;
                if (AITimer == 1)
                {
                    CameraSystem.SetBossTitle(180, "Viscera, Primal Hunter", Color.Crimson);
                        CameraSystem.ChangeCameraPos(NPC.Center, 180);
                        dustHeight += 0.2f;
                        dustNum += 0.020f;
                        for (int i = 0; i < dustNum; i++)
                        {
                            Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                            Dust d = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y - dustHeight), DustID.BlueCrystalShard, speed * 10);
                            d.noGravity = true;
                        }
                        //SoundEngine.PlaySound(summon);
                        CameraSystem.ScreenShakeAmount = 15f;
                }
                if (AITimer >= 30)
                {
                    AITimer = 0;
                    AIState = FlameThrower;
                }
            }
            else if (AIState == FlameThrower)
            {
                NPC.damage = 0;
                AITimer++;
                if (AITimer == 1)
                {
                    CameraSystem.ScreenShakeAmount = 5f;
                    //SoundEngine.PlaySound(roar);
                }
                if (AITimer < 30)
                {
                    Vector2 pos = new Vector2(player.position.X, player.position.Y - 100);
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.18f;
                }
                if (AITimer == 30)
                {
                    lastPos = player.Center;
                    NPC.velocity = Vector2.Zero;
                    Vector2 vector16 = NPC.DirectionTo(player.Center) * 7f;
                }
                if (AITimer == 60)
                {
                    NPC.velocity = Vector2.Zero;
                        float rotation = MathHelper.ToRadians(45);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9);
                        Vector2 pos = NPC.Center - Vector2.UnitY * 23;
                        for (int i = 0; i < 2; i++)
                        {
                            Vector2 perturbedSpeed = (Utility.FromAToB(NPC.Center, player.Center) * 9.5f).RotatedBy(Main.rand.NextFloat(-rotation, rotation));
                            Vector2 perturbedSpeed1 = (Utility.FromAToB(NPC.Center, player.Center) * 9.5f).RotatedBy(Main.rand.NextFloat(-rotation, rotation));
                            perturbedSpeed1.Normalize();

                            Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, perturbedSpeed, ModContent.ProjectileType<BloodBlister>(), 15, 1.5f, player.whoAmI);
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, Utility.FromAToB(NPC.Center, player.Center) * 9.5f, ModContent.ProjectileType<BloodBlister>(), 15, 1.5f, player.whoAmI);



                    }

                    if (AITimer >= 100)
                {
                    NPC.rotation = 0;
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    AIState = RocksAtPlayer;
                }
            }
            else if (AIState == RocksAtPlayer)
            {
                AITimer++;
                if (AITimer == 1)
                    SoundEngine.PlaySound(SoundID.Roar);
                Vector2 pos = new Vector2(player.position.X, player.position.Y - 335);
                Vector2 target = pos;
                Vector2 moveTo = target - NPC.Center;
                NPC.velocity = (moveTo) * 0.18f;

                if (AITimer < 100 && AITimer % 20 == 0)
                {
                        for (int i = 0; i < 3; i++) //Tendrails 
                        {
                            // Get the ground beneath the player
                            Vector2 playerPos = new Vector2((player.position.X - 30 * i) / 16, (player.position.Y) / 16);
                            Vector2 playerPos2 = new Vector2((player.position.X + 30 * i) / 16, (player.position.Y) / 16);
                            Tile tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                            while (!tile.HasTile || tile.TileType == TileID.Trees)
                            {
                                playerPos.Y += 1;
                                tile = Framing.GetTileSafely((int)playerPos.X, (int)playerPos.Y);
                            }

                            Tile tile2 = Framing.GetTileSafely((int)playerPos2.X, (int)playerPos2.Y);
                            while (!tile2.HasTile || tile2.TileType == TileID.Trees)
                            {
                                playerPos2.Y += 1;
                                tile2 = Framing.GetTileSafely((int)playerPos2.X, (int)playerPos2.Y);
                            }
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                if (i == 0)
                                {
                                    //Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<MarsHellBoom>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<Spike>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                }
                                else
                                {
                                    //Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<MarsHellBoom>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<Spike>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                    //Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<MarsHellBoom>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                    Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ModContent.ProjectileType<Spike>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                }
                            }
                        }
                    }
                if (AITimer >= 100)
                {
                    NPC.rotation = 0;
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    AIState = RockFall;
                }
            }
            else if (AIState == RockFall)
            {
                AITimer++;
                if (AITimer == 1)
                        SoundEngine.PlaySound(SoundID.Roar);
                    Vector2 pos = new Vector2(player.position.X, player.position.Y - 180);
                    Vector2 target = pos;
                    Vector2 moveTo = target - NPC.Center;
                    NPC.velocity = (moveTo) * 0.18f;
                if (AITimer % 10 == 0 && AITimer > 60)
                {

                   Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Bottom, Vector2.Zero, ModContent.ProjectileType<BloodBombTwo>(), 1, Main.myPlayer, NPC.whoAmI);

                }
                if (AITimer >= 100)
                {
                    NPC.noTileCollide = true;
                    NPC.rotation = 0;
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                    AITimer = 0;
                    AITimer2 = 0;
                    AITimer3 = 0;
                    AIState = FlameThrower;
                }
            }
        }
    }
}
