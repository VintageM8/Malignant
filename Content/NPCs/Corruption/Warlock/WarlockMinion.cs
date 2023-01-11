using Malignant.Content.Projectiles.Enemy.Warlock;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Malignant.Content.NPCs.Corruption.Warlock
{
    public class WarlockMinion : ModNPC
    {
        private int moveSpeed;
        public bool kill = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lost Souls");
            //Main.npcFrameCount[NPC.type] = 0;
        }

        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 38;
            NPC.damage = 25;
            NPC.defense = 15;
            NPC.lifeMax = 60;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.knockBackResist = 0.4f;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];

            if (NPC.lifeMax > 60 || NPC.life > 60)
            {
                NPC.lifeMax = 60;
                NPC.life = 60;
            }

            switch (NPC.ai[0])
            {
                case 0:
                    {
                        if (!PlayerAlive(player)) { break; }

                        if (NPC.ai[3] == 0)
                        {
                            moveSpeed = Main.rand.Next(5, 10);
                            NPC.ai[3]++;
                        }

                        Vector2 moveTo = player.Center;
                        var move = moveTo - NPC.Center;

                        float length = move.Length();
                        if (length > moveSpeed)
                        {
                            move *= moveSpeed / length;
                        }
                        var turnResistance = 45;
                        move = (NPC.velocity * turnResistance + move) / (turnResistance + 1f);
                        length = move.Length();
                        if (length > 10)
                        {
                            move *= moveSpeed / length;
                        }
                        NPC.velocity.X = move.X;
                        NPC.velocity.Y = move.Y * .98f;

                        if (++NPC.ai[1] > 600 + Main.rand.Next(100) && NPC.ai[2] == 1)
                        {
                            NPC.ai[1] = 0;
                            NPC.ai[0] = 1;
                        }
                    }
                    break;
                case 1:
                    {
                        if (!PlayerAlive(player)) { break; }

                        NPC.velocity = Vector2.Zero;

                        if (++NPC.ai[1] % 60 == 0)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(player.Center) * 7.5f, ProjectileType<CursedWave>(), NPC.damage, 10f, Main.myPlayer);
                            }
                            NPC.ai[2]++;
                        }

                        if (NPC.ai[2] == 1)
                        {
                            NPC.ai[2] = 0;
                            NPC.ai[1] = 0;
                            NPC.ai[0] = 0;
                        }
                    }
                    break;

            }
            if (kill == true)
            {
                NPC.active = false;
                NPC.life = 0;
            }
        }

        bool PlayerAlive(Player player)
        {
            if (!player.active || player.dead)
            {
                player = Main.player[NPC.target];
                NPC.TargetClosest();
                if (!player.active || player.dead)
                {
                    if (NPC.timeLeft > 25)
                    {
                        NPC.timeLeft = 25;
                        NPC.velocity = Vector2.UnitY * -7;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
