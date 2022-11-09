using Microsoft.Xna.Framework;
using Malignant.Common;
using System;
using Terraria;
using Terraria.ID;
using Malignant.Common.NPCs;
using Terraria.ModLoader;

namespace Malignant.Content.NPCs.Norse.Zolzar
{
    class VikingBossAdd : ModNPC
    {

		private const int State_Moving = 1;
		private const int State_Attacking = 2;
		private const int State_Circling = 3;

		public float AI_State
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		public float AI_Owner
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		public float AI_Timer
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		public float AddID
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public float Attack_State
		{
			get => NPC.localAI[0];
			set => NPC.localAI[0] = value;
		}
		private bool npcDashing = false;
		private float DashTime;
		private Vector2 tempPos;
		private Vector2 IntSpeed;
		float TimerStart = 0;
		double angle = 0;

		private const int npcVelocity = 11;
		private const int DashSpeed = 20;
		private const int LightningStrikeSpeed = 16;
		private const int FollowTime = 45;
		private const int ShootingDelay = 45;
		private const int ExplosionTime = 390;
		private const int LightningDMG = 60;
		private const float LightningKB = 1;
		private const float ExplosionDelay = 30;
		private const float OverDashFactor = 1.5f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viking Adds");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.width = 50;
			NPC.height = 50;
			NPC.damage = 35;
			NPC.defense = 8;
			NPC.lifeMax = 8900;
			NPC.value = 60f;
			NPC.knockBackResist = 0;
			NPC.scale = 1f;
			NPC.stepSpeed = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.Item25;
		}
		public override bool PreAI()
		{
			Player target = Main.player[NPC.target];
			MalignantPlayer globaltarget = target.GetModPlayer<MalignantPlayer>();
			int Frames = 20;
			globaltarget.PreviousVelocity[0] = target.velocity;
			for (int i = 0; i < Frames; i++)
			{
				IntSpeed = Vector2.Lerp(IntSpeed, globaltarget.PreviousVelocity[Frames - 1 - i], 0.14f);
			}

			return true;
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			if (AI_State == State_Moving || AI_State == State_Circling)
			{
				Moving();
			}
			if (AI_State == State_Attacking)
			{
				if (AI_Timer == 0)
				{
					NPC.netUpdate = true;
				}
				switch (Attack_State)
				{
					case 0:
						LightningStrike();
						break;
					case 1:
						Dash();
						break;
					case 2:
						LightningCircle();
						break;
				}
			}
			AI_Timer++;	
		}
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
			NPC Owner = Main.npc[(int)AI_Owner];
			Owner.life -= (int)(damage * 0.3);
			if (Owner.life <= 0)
            {
				Owner.checkDead();
            }
		}
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
			NPC Owner = Main.npc[(int)AI_Owner];
			Owner.life -= (int)(damage * 0.5);
			if (Owner.life <= 0)
			{
				Owner.checkDead();
			}
		}

        private void Moving()
		{
			NPC Owner = Main.npc[(int)AI_Owner];
			MalignantGlobalNPC globalOwner = Owner.GetGlobalNPC<MalignantGlobalNPC>();
			int AddNumber = (int)Owner.ai[3]; //this is the number of Adds currently
			Vector2 WantedPosition = globalOwner.AddPositions[AddNumber - (int)AddID - 1];
			NPC.velocity = Owner.velocity;
			if (NPC.DistanceSQ(WantedPosition) < 13 * 13)
			{
				AI_State = State_Circling;
				NPC.Center = WantedPosition;
				TimerStart = 0;
				//npc.velocity = Vector2.Zero;
			}
			else if (AI_State == State_Moving)
			{
				Vector2 npcVel = WantedPosition - NPC.Center;
				if (npcVel != Vector2.Zero)
				{
					npcVel.Normalize();
				}
				npcVel *= npcVelocity;
				NPC.velocity += npcVel;
			}
			else
            {
				if (TimerStart == 0)
				{
					angle = Math.Atan2(NPC.Center.Y - Owner.Center.Y, NPC.Center.X - Owner.Center.X);
					TimerStart = Owner.localAI[0];
				}

				double period = 2 * Math.PI / 150f;
				NPC.Center = Owner.Center + new Vector2(300 * (float)Math.Cos(period * (Owner.localAI[0] - TimerStart) + angle), 300 * (float)Math.Sin(period * (Owner.localAI[0] - TimerStart) + angle));
			}
		}
		private void Dash()
        {
			Player target = Main.player[NPC.target];
			if (AI_Timer == 0) {
				float time = 0;
				Vector2 npcVel = ModTargeting.LinearAdvancedTargeting(NPC.Center, target.Center, IntSpeed, DashSpeed, ref time);
				ModTargeting.FallingTargeting(NPC, target, new Vector2(0, -28), (int)DashSpeed, ref time, ref npcVel);
				if (time > 15) DashTime = time * OverDashFactor;
				else DashTime = 15 * OverDashFactor;
				//if (npcVel != Vector2.Zero)
				//{
				//    npcVel.Normalize();
				//}
				//npcVel *= DashSpeed;
				NPC.velocity = npcVel;
				npcDashing = true;
			}

			if (AI_Timer >= DashTime || AI_Timer >= 120)
            {
				AI_State = State_Moving;
				AI_Timer = -1;
				npcDashing = false;
			}
        }
		private void LightningStrike()
		{
			Player target = Main.player[NPC.target];
			if (MoveTo(target.Center + new Vector2(0, -300), LightningStrikeSpeed, false, true))
			{
				if (AI_Timer % 20 == 0)
				{
					Vector2 projVel = target.Center - NPC.Center;
					if (projVel != Vector2.Zero)
					{
						projVel.Normalize();
					}
					projVel *= 7;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, projVel, ProjectileID.CultistBossLightningOrbArc, LightningDMG, LightningKB, Main.myPlayer, projVel.ToRotation(), AI_Timer);
				}
			}
			else
			{
				AI_Timer--;
			}
			if (AI_Timer >= 40)
            {
				AI_State = State_Moving;
				AI_Timer = -1;
            }
		}
		private void LightningCircle()
		{
			NPC Owner = Main.npc[(int)AI_Owner];
			Player target = Main.player[NPC.target];
			MalignantGlobalNPC globalOwner = Owner.GetGlobalNPC<MalignantGlobalNPC>();
			NPC.damage = 0;
			int AddNumber = (int)Owner.ai[3]; //this is the number of Adds currently
											  			
			if (AI_Timer <= AddNumber * FollowTime + 240)
            {
				Vector2 WantedPosition = target.Center + new Vector2(450 * (float)Math.Cos(Math.PI * 2 * AddID / AddNumber), 450 * (float)Math.Sin(Math.PI * 2 * AddID / AddNumber));
				MoveTo(WantedPosition, 14, true, true);
				if ((AI_Timer - 120) == AddID * FollowTime && (AI_Timer - 120) > 0)
				{
					float time = 0;
					Vector2 projVel = ModTargeting.LinearAdvancedTargeting(NPC.Center, target.Center, IntSpeed, 7 * 4, ref time);
					ModTargeting.FallingTargeting(NPC, target, new Vector2(0, -28), 7 * 4, ref time, ref projVel);
					Projectile.NewProjectile(NPC.GetSource_FromAI(),NPC.Center, projVel / 4, ProjectileID.CultistBossLightningOrbArc, LightningDMG, LightningKB, Main.myPlayer, projVel.ToRotation(), AI_Timer);
				}

			}
			if (AI_Timer == AddNumber * FollowTime + 240 + 1)
            {
				MoveTo(target.Center, 6, false, false, AddNumber * FollowTime + 240 + 1);
            }
				//if (AI_Timer == FollowTime + ShootingDelay)
				//{
				//	Vector2 projVel = target.Center - npc.Center;
				//	if (projVel != Vector2.Zero)
				//	{
				//		projVel.Normalize();
				//	}
				//	projVel *= 7;
				//	Projectile.NewProjectile(npc.Center, projVel, ProjectileID.CultistBossLightningOrbArc, LightningDMG, LightningKB, Main.myPlayer, projVel.ToRotation(), AI_Timer); //TODO randomise the lightning seed
				//}
			if (AI_Timer == AddNumber * FollowTime + 240 + 1 + 75 && AddID == 0)
			{
				for (int i = 0; i < 4; i++)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 8 * (float)Math.Cos(2 * Math.PI * i / 4), 8 * (float)Math.Sin(2 * Math.PI * i / 4), ProjectileID.CultistBossIceMist, 40, 1, target.whoAmI);
				}
			}
			if (AI_Timer >= AddNumber * FollowTime + 240 + 1 + 75 + ExplosionDelay)
			{
				NPC.damage = 90;
				AI_State = State_Moving;
				AI_Timer = -1;
			}

		}
		private bool MoveTo(Vector2 WantedPosition, float TravelSpeed, bool follow, bool relative = false, int delay = 0)
		{
			Player player = Main.player[NPC.target];
			if (AI_Timer == delay || follow)
			{
				if (relative) NPC.velocity = player.velocity;
				tempPos = WantedPosition;
				Vector2 npcVel = tempPos - NPC.Center;
				if (npcVel != Vector2.Zero)
				{
					npcVel.Normalize();
				}
				npcVel *= TravelSpeed;
				NPC.velocity += npcVel;
			}
			if (NPC.DistanceSQ(tempPos) <= 14 * 14)
			{
				NPC.Center = tempPos;
				NPC.velocity = Vector2.Zero;
				return true;
			}
			return false;
		}
		private void GenerateAddPositions()
		{
			NPC Owner = Main.npc[(int)AI_Owner];			
			MalignantGlobalNPC globalOwner = Owner.GetGlobalNPC<MalignantGlobalNPC>();
			int AddNumber = (int)Owner.ai[3]; //this is the number of Adds currently
			double period = 2f * Math.PI / 300f;
			for (int i = 0; i < AddNumber; i++)
			{
				globalOwner.AddPositions[i] = NPC.Center + new Vector2(300 * (float)Math.Cos(period * (Owner.localAI[0] + (300 / AddNumber * i))), 300 * (float)Math.Sin(period * (Owner.localAI[0] + (300 / AddNumber * i))));
			}
		}
		public override void FindFrame(int frameHeight)
		{
			int factor = (int)((Main.player[NPC.target].Center.X - NPC.Center.X) / Math.Abs(Main.player[NPC.target].Center.X - NPC.Center.X));
			NPC.frameCounter++;

			if (NPC.frameCounter % 6f == 5f)
			{
				NPC.frame.Y += frameHeight;
			}
			if (NPC.frame.Y >= frameHeight * 4) // 10 is max # of frames
			{
				NPC.frame.Y = 0; // Reset back to default
			}

			NPC.spriteDirection = factor;
		}
		//this is probably really scuffed. Id handles the the reordering of the adds once one of them dies. I wrote the algorythm for reordering them but forgot that the way the spots are assigned is such that AddID 0 goes to the last spot on the circle.
		//that lead to them rotating against the spinning direction which looked ugly. So i just fixed it by shifting the array to the left by one and then just overriding the AddIDs. The shifting makes it so the try to align with the previous point instead of the next one wich fixes it. 
		//This logic can probably be reworked but it's important that the direction the adds move in after they get reassigned is in the direction of the spin. Because it looks really bad if thats not the case. So carefull !!!!
		public override bool CheckDead()
		{
			NPC Owner = Main.npc[(int)AI_Owner];
			MalignantGlobalNPC globalOwner = Owner.GetGlobalNPC<MalignantGlobalNPC>();
			Owner.ai[3]--; //reduce the Addnumber by 1
			int AddNumber = (int)Owner.ai[3];
			NPC.boss = false;
            if (AddNumber > 0 && AddID != 0)
            {
                for (int i = (int)AddID; i < AddNumber; i++)
                {
                    globalOwner.Add[i] = globalOwner.Add[i + 1];
                }               
				globalOwner.Add[AddNumber] = globalOwner.Add[0];
			}
			if (AddID == 0 || true)
			{
				for (int j = 0; j < AddNumber; j++)
				{
					globalOwner.Add[j] = globalOwner.Add[j + 1];
				}
			}
            for (int k = 0; k < AddNumber; k++)
			{
				Main.npc[globalOwner.Add[k]].ai[3] = k;
			}
			globalOwner.Add[AddNumber] = 0;
			globalOwner.Add[AddNumber] = 0;
			GenerateAddPositions();
			return true;
		}
	}
}
