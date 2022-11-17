using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.NPCs.Crimson.Heart
{
    public class HeartMan : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heart Man");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 90;
            NPC.lifeMax = 155;
            NPC.defense = 14;
            NPC.damage = 25;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.Venom] = true;
            NPC.buffImmune[BuffID.Confused] = true;
            NPC.value = 1200f;
            NPC.knockBackResist = 0.75f;
            NPC.noGravity = true;
            NPC.netAlways = true;
            NPC.chaseable = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;

        }

        bool aggroed = false;
        int moveSpeed = 0;
        int moveSpeedY = 0;
        float HomeY = 100f;

         public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCrimson && spawnInfo.Player.ZoneOverworldHeight ? .075f : 0f;

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;
            Player target = Main.player[NPC.target];
            float distance = NPC.DistanceSQ(target.Center);
            bool expertMode = Main.expertMode;
            if (distance < 200 * 200)
            {
                if (!aggroed)
                    SoundEngine.PlaySound(SoundID.Zombie53, NPC.Center);
                aggroed = true;
            }

            if (Main.netMode != NetmodeID.Server && Main.rand.Next(60) == 0)
                SoundEngine.PlaySound(new SoundStyle("Malignant/Assets/SFX/HeartbeatFx"), NPC.position);

            if (!aggroed)
            {
                if (NPC.localAI[0] == 0f)
                {
                    NPC.localAI[0] = NPC.Center.Y;
                    NPC.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
                }
                if (NPC.Center.Y >= NPC.localAI[0])
                {
                    NPC.localAI[1] = -1f;
                    NPC.netUpdate = true;
                }
                if (NPC.Center.Y <= NPC.localAI[0] - 2f)
                {
                    NPC.localAI[1] = 1f;
                    NPC.netUpdate = true;
                }
                NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + 0.009f * NPC.localAI[1], -.25f, .25f);

            }
            else
            {
                Player player = Main.player[NPC.target];

                if (NPC.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
                    moveSpeed--;

                if (NPC.Center.X <= player.Center.X && moveSpeed <= 30)
                    moveSpeed++;

                NPC.velocity.X = moveSpeed * 0.08f;

                if (NPC.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -20) //Flies to players Y position
                {
                    moveSpeedY--;
                    HomeY = 165f;
                }

                if (NPC.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 20)
                    moveSpeedY++;

                NPC.velocity.Y = moveSpeedY * 0.1f;
                if (Main.rand.NextBool(250))
                    HomeY = -25f;

                ++NPC.ai[0];

                if (NPC.ai[0] >= 720)
                    NPC.ai[0] = 0;

                Vector2 velocity = Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2) * new Vector2(5f, 3f);
                int damage = Main.expertMode ? 12 : 18;
                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 10, NPC.Center.Y + 10, velocity.X, velocity.Y, ModContent.ProjectileType<BloodSpurt>(), damage, 0.0f, Main.myPlayer, 0.0f, NPC.whoAmI);
                Main.projectile[p].hostile = true;

                NPC.ai[0] = 0;
                NPC.netUpdate = true;
            }

            Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.122f, .5f, .48f);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            if (NPC.frameCounter % 6f == 5f)
            {
                NPC.frame.Y += frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 6)
            {
                NPC.frame.Y = 0;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Obsidian, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DungeonSpirit, 2.5f * hitDirection, -2.5f, 0, default, .34f);
            }
        }
    }
}
