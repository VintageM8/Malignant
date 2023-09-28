using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Malignant.Content.Projectiles.Enemy.Njor;
using Terraria.GameContent.ItemDropRules;
using static Terraria.ModLoader.ModContent;
using Malignant.Content.Items.Snow.Cocytus.ForgottenFrost;
using Malignant.Content.Items.Snow.Cocytus.NjorStaff;
using Malignant.Content.Items.Snow.Cocytus.NjorSword;

namespace Malignant.Content.NPCs.Norse.Njor
{
	public class Njor : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 400;
			NPC.height = 400;
			NPC.damage = Main.rand.Next(15,25);
			NPC.defense = 12;
			NPC.lifeMax = 4300;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.noGravity = true;
			NPC.aiStyle = -1;
			NPC.alpha = 255;
			NPC.scale = 0.25f;
			NPC.boss = true;
			NPC.SpawnWithHigherTime(30);
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Njor");
            }

        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
			NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * balance);
			NPC.damage = (int)(NPC.damage * 1.6f);
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

		 	npcLoot.Add(ItemDropRule.OneFromOptions(1, new int[]
			{				
				ItemType<NjorsStaff>(),
				ItemType<IceSword>(),
				ItemType<IcyTundra>(),
			}
		));

		}
		#endregion

		#region Attack Stuff
		public int prepareAttack = 0;
		public int attackChoice = -1;
		public int chargeSpike = 0;
		public int SideSwingTime = 0;
		public int bombTime = 0;

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
						Dust d = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y - dustHeight), DustID.BlueCrystalShard, speed * 10);
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
							Dust d = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y - 16), DustID.BlueCrystalShard, speed2 * 20, Scale: 3f);
							d.noGravity = true;
							Dust e = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y - 16), DustID.BlueCrystalShard, speed2 * 10, Scale: 3f);
							e.noGravity = true;
							Dust f = Dust.NewDustPerfect(new Vector2(NPC.Center.X, NPC.Center.Y - 16), DustID.BlueCrystalShard, speed2 * 5, Scale: 3f);
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
					if (targetPosition.X < NPC.position.X && NPC.velocity.X > -4)
					{
						NPC.netUpdate = true;
						NPC.velocity.X -= 0.05f;
					}
					if (targetPosition.X > NPC.position.X && NPC.velocity.X < 4)
					{
						NPC.netUpdate = true;
						NPC.velocity.X += 0.05f;
					}
					if (Main.player[NPC.target].position.Y < NPC.position.Y)
					{
						NPC.netUpdate = true;
						NPC.velocity.Y -= 0.05f;
					}
					if (Main.player[NPC.target].position.Y > NPC.position.Y)
					{
						NPC.netUpdate = true;
						NPC.velocity.Y += 0.05f;
					}
					NPC.position += NPC.velocity;
				}

				if(enragedMode == true)
				{
					NPC.TargetClosest(true);
					Vector2 targetPosition = Main.player[NPC.target].position;
					if (targetPosition.X < NPC.position.X && NPC.velocity.X > -4)
					{
						NPC.netUpdate = true;
						NPC.velocity.X -= 0.11f;
					}
					if (targetPosition.X > NPC.position.X && NPC.velocity.X < 4)
					{
						NPC.netUpdate = true;
						NPC.velocity.X += 0.11f;
					}
					if (Main.player[NPC.target].position.Y < NPC.position.Y)
					{
						NPC.netUpdate = true;
						NPC.velocity.Y -= 0.11f;
					}
					if (Main.player[NPC.target].position.Y > NPC.position.Y)
					{
						NPC.netUpdate = true;
						NPC.velocity.Y += 0.11f;
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
						dust = Main.dust[Terraria.Dust.NewDust(new Vector2(NPC.Center.X, NPC.Center.Y - 50), 5, 5, DustID.BlueCrystalShard, 0f, 0f, 0, new Color(255, 0, 201), 1f)];
						dust.noGravity = true;
					}

					SoundEngine.PlaySound(SoundID.Item28, NPC.position);

					if (NPC.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
					{
						var source = NPC.GetSource_FromAI();
						Vector2 position = new Vector2(NPC.Center.X, NPC.Center.Y - 50);
						Vector2 targetPosition = Main.player[NPC.target].Center;
						Vector2 direction = targetPosition - position;
						direction.Normalize();
						float speed = 10f;
						int type = ModContent.ProjectileType<SimpleShot>();
						int damage = 20;
						Projectile.NewProjectile(source, position, direction * speed, type, damage, 0f, Main.myPlayer);
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

                #region Ice Bomb
                if (attackChoice == 0)
                {
					if (enragedMode == true)
					{
						bombTime += 1;

						if (bombTime == 1)
						{
							Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<IceBomb>(), 0, 5);
						}

						if (bombTime == 60)
						{
							Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<IceBomb>(), 0, 5);
						}

						if (bombTime == 120)
						{
							Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(0, 0), ModContent.ProjectileType<IceBomb>(), 0, 5);
							prepareAttack = 0;
							attackChoice = -1;
							bombTime = 0;
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
						Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(-2, 5), ModContent.ProjectileType<HomeSpike>(), Main.rand.Next(10, 20), 5);
						Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(0, 5), ModContent.ProjectileType<HomeSpike>(), Main.rand.Next(10, 20), 5);
						Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y + 10), new Vector2(2, 5), ModContent.ProjectileType<HomeSpike>(), Main.rand.Next(10, 20), 5);
						attackChoice = -1;
						prepareAttack = 0;
						chargeSpike = 0;
					}
				}
				#endregion

				#region Swinging Projectile Attack
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
							Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(8, 1.5f), ModContent.ProjectileType<SideSwingRight>(), Main.rand.Next(10, 20), 5);
							Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(-8, 1.5f), ModContent.ProjectileType<SideSwingLeft>(), Main.rand.Next(10, 20), 5);

							firedSide1 = true;
						}

						if (SideSwingTime >= 150)
						{
							Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(8, -2f), ModContent.ProjectileType<SideSwingRight>(), Main.rand.Next(10, 20), 5);
							Projectile.NewProjectile(null, new Vector2(NPC.Center.X, NPC.Center.Y), new Vector2(-8, -2f), ModContent.ProjectileType<SideSwingLeft>(), Main.rand.Next(10, 20), 5);

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


	    public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter % 6f == 5f)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 4) 
            {
                NPC.frame.Y = 0; // Reset back to default
            }
        }

	}
}
