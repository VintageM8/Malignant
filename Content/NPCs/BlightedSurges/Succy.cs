using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.Audio;

namespace Malignant.Content.NPCs.BlightedSurges
{
    class Succy : ModNPC
    {

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 90;
            NPC.damage = 8;
            NPC.defense = 3;
            NPC.lifeMax = 30;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;
            NPC.noGravity = false;
            NPC.aiStyle = -1;
        }

        public int counting = 0;
        public int countingAttach = 0;

        public bool fromRight = false;
        public bool fromLeft = false;
        public bool rolling = true;
        public bool jumping = false;
        public bool attach = false;
        public bool canJump = true;

        public override void AI()
        {
            Player player = Main.player[NPC.target];

            NPC.TargetClosest(true);

            int distance = (int)Vector2.Distance(NPC.Center, player.Center);

            if (player.Center.X > NPC.Center.X)
            {
                NPC.spriteDirection = -1;
            }
            else
            {
                NPC.spriteDirection = 1;
            }

            if (canJump == true)
            {
                if (distance > 200)
                {
                    rolling = true;
                }
                else
                {
                    jumping = true;
                    rolling = false;
                }
            }

            if (attach == false)
            {
                if (NPC.collideX)
                {
                    NPC.velocity.Y -= 3;
                }

                if (rolling == true)
                {
                    if (player.Center.X > NPC.Center.X)
                    {
                        NPC.rotation += 0.25f;
                    }
                    else
                    {
                        NPC.rotation -= 0.25f;
                    }

                    if (player.Center.X > NPC.Center.X)
                    {
                        NPC.velocity.X += 0.25f;
                    }
                    if (player.Center.X < NPC.Center.X)
                    {
                        NPC.velocity.X -= 0.25f;
                    }

                    if (NPC.velocity.X >= 6f)
                    {
                        NPC.velocity.X = 6f;
                    }
                    else if (NPC.spriteDirection == 1 && NPC.velocity.Y == 0)
                    {
                        int a = Dust.NewDust(NPC.BottomRight, 5, 1, DustID.Smoke);
                        Main.dust[a].noGravity = true;
                    }
                    if (NPC.velocity.X <= -6f)
                    {
                        NPC.velocity.X = -6f;
                    }
                    else if (NPC.spriteDirection == -1 && NPC.velocity.Y == 0)
                    {
                        int b = Dust.NewDust(NPC.BottomLeft, 5, 1, DustID.Smoke);
                        Main.dust[b].noGravity = true;
                    }
                }
            }

            if (canJump == false)
            {
                counting += 1;

                NPC.velocity.X = 0;

                if (counting >= 60)
                {
                    canJump = true;
                    counting = 0;
                }
            }

            if (jumping == true && canJump == true)
            {
                NPC.rotation = default;

                counting += 1;

                if (counting == 1)
                {
                    SoundEngine.PlaySound(SoundID.DD2_GoblinScream, NPC.position);

                    if (player.Center.X > NPC.Center.X)
                    {
                        NPC.velocity.X = 7f;
                        NPC.velocity.Y = -5.5f;
                    }
                    if (player.Center.X < NPC.Center.X)
                    {
                        NPC.velocity.X = -7f;
                        NPC.velocity.Y = -5.5f;
                    }
                }

                if (counting >= 30)
                {
                    if (NPC.collideY || counting >= 120)
                    {
                        if (player.Center.X > NPC.Center.X)
                        {
                            NPC.velocity.X = 5f;
                        }
                        if (player.Center.X < NPC.Center.X)
                        {
                            NPC.velocity.X = -5f;
                        }
                        jumping = false;
                        canJump = false;
                        rolling = true;
                        counting = 0;
                    }
                }
            }

            if (attach == true)
            {
                if (fromRight == true)
                {
                    NPC.spriteDirection = -1;
                    NPC.position.X = player.position.X - 20;
                    NPC.position.Y = player.position.Y;
                }
                else if (fromLeft == true)
                {
                    NPC.spriteDirection = 1;
                    NPC.position.X = player.position.X + 15;
                    NPC.position.Y = player.position.Y;
                }

                NPC.velocity = new Vector2(0, 0);

                if (player.statLife <= 0)
                {
                    attach = false;
                    jumping = false;
                    canJump = true;
                    rolling = true;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (attach == false && jumping == true)
            {
                if (NPC.velocity.X > 0)
                {
                    fromRight = true;
                }
                else
                {
                    fromLeft = true;
                }

                attach = true;
            }

            SoundEngine.PlaySound(SoundID.DD2_JavelinThrowersAttack, NPC.position);
        }
    }
}
