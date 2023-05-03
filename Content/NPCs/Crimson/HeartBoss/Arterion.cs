using Terraria.Audio;
using Microsoft.Xna.Framework;
using Malignant.Content.Projectiles.Enemy.Njor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles;
using System;
using Malignant.Content.NPCs.Crimson.Heart;
using Malignant.Content.Dusts;

namespace Malignant.Content.NPCs.Crimson.HeartBoss
{
    public class Arterion : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arterion, Touched of Infection");
            //Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 800;
            NPC.height = 1000;
            NPC.damage = Main.rand.Next(15, 25);
            NPC.defense = 12;
            NPC.lifeMax = 4300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.alpha = 255;
            NPC.scale = 0.25f;
            NPC.boss = true;
            NPC.knockBackResist = 0f;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Arterion");
            }
        }

        public bool initialSpawn = true;
        public bool spawning = true;
        public float dustNum = 0;
        public int burstTime = 0;
        public bool burstEff = true;
        public bool moving = false;
        public float dustHeight = -40;
        public bool enragedMode = false;
        public bool screamed = false;

        #region Drops
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            for (int d = 0; d < 20; d++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 101, 0f, 0f, 150);
            }
        }
        #endregion

        #region Attack Stuff
        public int prepareAttack = 0;
        public int attackChoice = -1;
        public int chargeSpike = 0;
        public int SideSwingTime = 0;
        public int attackTime = 0;

        public bool isChargingSpike = false;
        public bool fireSpike = false;
        public bool firedSide1 = false;
        #endregion

        public override void AI()
        {
            if (NPC.life <= NPC.lifeMax / 2)
            {
                enragedMode = true;
            }

            if (enragedMode == true && screamed == false)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                screamed = true;
            }

            Player player = Main.player[NPC.target];

            NPC.TargetClosest(true);

            #region Spawn Animation
            if (initialSpawn == true)
            {
                if (spawning == true)
                {
                    dustHeight += 0.2f;
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                    NPC.alpha--;
                    NPC.scale += 0.0025f;
                    dustNum += 0.020f;

                    for (int i = 0; i < dustNum; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y - dustHeight), DustType<Blood>(), speed * 10);
                        d.noGravity = true;
                    }
                }

                if (NPC.scale >= 0.8 && burstEff == true)
                {
                    spawning = false;
                    NPC.velocity.Y = 0;
                    NPC.velocity.X = 0;
                    burstTime += 1;

                    if (burstTime >= 60)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        for (int i = 0; i < 45; i++)
                        {
                            Vector2 speed2 = Main.rand.NextVector2CircularEdge(1f, 1f);
                            Dust d = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y - 16), DustType<Blood>(), speed2 * 20, Scale: 3f);
                            d.noGravity = true;
                            Dust e = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y - 16), DustType<Blood>(), speed2 * 10, Scale: 3f);
                            e.noGravity = true;
                            Dust f = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y - 16), DustType<Blood>(), speed2 * 5, Scale: 3f);
                            f.noGravity = true;
                        }
                        NPC.scale = 1;
                        NPC.alpha = 0;
                        burstTime = 0;
                        burstEff = false;
                        moving = true;
                        initialSpawn = false;
                    }
                }
            }
            #endregion

            #region Basic Movement
            if (moving == true)
            {
                if (enragedMode == false)
                {
                    NPC.TargetClosest(true);
                    Vector2 targetPosition = Main.player[NPC.target].position;
                    if (targetPosition.X < NPC.position.X && NPC.velocity.X > -2)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.X -= 0.02f;
                    }
                    if (targetPosition.X > NPC.position.X && NPC.velocity.X < 2)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.X += 0.02f;
                    }
                    if (Main.player[NPC.target].position.Y < NPC.position.Y)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.Y -= 0.02f;
                    }
                    if (Main.player[NPC.target].position.Y > NPC.position.Y)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.Y += 0.02f;
                    }
                    NPC.position += NPC.velocity;
                }

                if (enragedMode == true)
                {
                    NPC.TargetClosest(true);
                    Vector2 targetPosition = Main.player[NPC.target].position;
                    if (targetPosition.X < NPC.position.X && NPC.velocity.X > -4)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.X -= 0.08f;
                    }
                    if (targetPosition.X > NPC.position.X && NPC.velocity.X < 4)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.X += 0.08f;
                    }
                    if (Main.player[NPC.target].position.Y < NPC.position.Y)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.Y -= 0.08f;
                    }
                    if (Main.player[NPC.target].position.Y > NPC.position.Y)
                    {
                        NPC.netUpdate = true;
                        NPC.velocity.Y += 0.08f;
                    }
                    NPC.position += NPC.velocity;
                }
            }
            #endregion

            if (initialSpawn == false)
            {
                #region Constant Shooting
                if (enragedMode == false)
                {
                    NPC.ai[1] += 1;
                }
                else if (enragedMode == true)
                {
                    NPC.ai[1] += 2;
                }

                if (NPC.ai[1] >= 180 && Main.rand.NextFloat() < .25f)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Dust dust;
                        dust = Main.dust[Terraria.Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 50), 5, 5, DustType<Blood>(), 0f, 0f, 0, new Color(255, 0, 201), 1f)];
                        dust.noGravity = true;
                    }

                    SoundEngine.PlaySound(SoundID.Item28, NPC.position);

                    if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        var source = NPC.GetSource_FromAI();
                        NPC.velocity.X = 0;
                        NPC.velocity.Y -= 1;
                        Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(-2, 5), ModContent.ProjectileType<SpikeSpawner>(), Main.rand.Next(10, 20), 5);
                        Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(0, 5), ModContent.ProjectileType<SpikeSpawner>(), Main.rand.Next(10, 20), 5);
                        Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(2, 5), ModContent.ProjectileType<SpikeSpawner>(), Main.rand.Next(10, 20), 5);
                    }
                    NPC.ai[1] = 0;
                }

                if (NPC.ai[1] >= 190)
                {
                    NPC.ai[1] = 0;
                }
                #endregion

                #region Choose Attack
                if (enragedMode == false)
                {
                    prepareAttack += 1;
                }
                else if (enragedMode == true)
                {
                    prepareAttack += 2;
                }

                if (prepareAttack >= 240 && Main.rand.NextFloat() < .1f)
                {
                    attackChoice = Main.rand.Next(3);
                    prepareAttack = -1000;
                }
                #endregion

                #region Trio Attack
                if (attackChoice == 0)
                {
                    if (enragedMode == true)
                    {
                        attackTime += 1;

                        if (attackTime == 1) //Small Tendrils + Heal 
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<HealingChunk>(), 0, 0, player.whoAmI, 0, 0); //Heal

                            for (int i = 0; i < 3; i++) //Small Tendrails 
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
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ProjectileType<Spike>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                    }
                                    else
                                    {
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ProjectileType<Spike>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                        Projectile.NewProjectile(NPC.GetSource_FromAI(), playerPos * 16, new Vector2(0, -10), ProjectileType<Spike>(), 28, 2.5f, Main.myPlayer, (int)0f);
                                    }
                                }
                            }
                        }

                        if (attackTime == 60) //Bombs
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                float spread = 10f * 0.0174f;
                                double startAngle = Math.Atan2(6, 6) - spread / 2;
                                double deltaAngle = spread / 8f;
                                bool expertMode = Main.expertMode;
                                double offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
                                int damage = expertMode ? 32 : 48;
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<HeartBomb>(), damage, 0, player.whoAmI);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * 3f), (float)(-Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<HeartBomb>(), damage, 0, player.whoAmI);
                            }
                        }

                        if (attackTime == 120) //Raining Blood (SLAYYYYERRRRR)
                        {
                            Vector2 rainPos = new Vector2(Main.screenPosition.X + Main.screenWidth * Main.rand.NextFloat(), Main.screenPosition.Y);
                            Projectile p = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), rainPos, Vector2.UnitY * 10, ModContent.ProjectileType<BloodBlister>(), 25, 1);
                            prepareAttack = 0;
                            attackChoice = -1;
                            attackTime = 0;
                        }
                    }
                    else
                    {
                        prepareAttack = 0;
                        attackChoice = -1;
                    }
                }
                #endregion

                #region Homing Spike Attack
                if (attackChoice == 1)
                {
                    chargeSpike += 1;
                }
                if (attackChoice == 1 && chargeSpike <= 120)
                {
                    NPC.velocity.X = 0;
                    NPC.velocity.Y = 0;
                    Vector2 chargeSpeed = Main.rand.NextVector2Unit((float)MathHelper.Pi / 4, (float)MathHelper.Pi / 2) * Main.rand.NextFloat();
                    Dust d = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y + 10), DustID.BlueCrystalShard, chargeSpeed * 10);
                    d.noGravity = true;

                    if (chargeSpike >= 120)
                    {
                        NPC.velocity.X = 0;
                        NPC.velocity.Y -= 1;
                        Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(-2, 5), ModContent.ProjectileType<SimpleShot>(), Main.rand.Next(10, 20), 5);
                        Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(0, 5), ModContent.ProjectileType<SimpleShot>(), Main.rand.Next(10, 20), 5);
                        Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(2, 5), ModContent.ProjectileType<SimpleShot>(), Main.rand.Next(10, 20), 5);
                        attackChoice = -1;
                        prepareAttack = 0;
                        chargeSpike = 0;
                    }
                }
                #endregion

                #region Blood Saw Attack
                if (attackChoice == 2)
                {
                    SideSwingTime += 1;

                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2Circular(10f, 2f);
                        Dust d = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y + 10), DustID.BlueCrystalShard, speed);
                        d.noGravity = true;
                    }

                    if (SideSwingTime < 150)
                    {
                        NPC.velocity.X = 0;
                        NPC.velocity.Y = 0;
                    }

                    if (SideSwingTime > 30)
                    {
                        if (firedSide1 == false)
                        {
                            Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(8, 1.5f), ModContent.ProjectileType<BloodSaw>(), Main.rand.Next(10, 20), 5);
                            Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(-8, 1.5f), ModContent.ProjectileType<BloodSaw>(), Main.rand.Next(10, 20), 5);
                            Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(-8, 1.5f), ModContent.ProjectileType<BloodSaw>(), Main.rand.Next(10, 20), 5);

                            firedSide1 = true;
                        }

                        if (SideSwingTime >= 150)
                        {
                            Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(8, -2f), ModContent.ProjectileType<BloodSaw>(), Main.rand.Next(10, 20), 5);
                            Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(-8, -2f), ModContent.ProjectileType<BloodSaw>(), Main.rand.Next(10, 20), 5);
                            Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(8, 1.5f), ModContent.ProjectileType<BloodSaw>(), Main.rand.Next(10, 20), 5);

                            attackChoice = -1;
                            prepareAttack = 0;
                            SideSwingTime = 0;
                            firedSide1 = false;
                        }
                    }
                }
                #endregion
            }
        }
    }
}
