using Malignant.Content.Projectiles.Enemy.Warlock;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.NPCs.Corruption.SacraficedSoul
{
    public class SacraficedSoul : ModNPC
    {
        float ai1 = 0;
        private int hitDirection;
        private int damage;

        public override void SetDefaults()
        {
            NPC.lifeMax = 150;
            NPC.damage = 70;
            NPC.defense = 16;
            NPC.knockBackResist = 0f;
            NPC.width = 48;
            NPC.height = 56;
            Main.npcFrameCount[NPC.type] = 4;
            NPC.value = 4450;
            NPC.npcSlots = 0.6f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            bool lineOfSight = Collision.CanHitLine(player.position, player.width, player.height, NPC.position, NPC.width, NPC.height);
            float speed = 0.65f + 0.35f * (float)Math.Sin(MathHelper.ToRadians(ai1 * 3));
            Vector2 toPlayer = player.Center - NPC.Center;
            float length = toPlayer.Length();
            if (lineOfSight || length <= 640)
            {
                if (length > 320 && !lineOfSight)
                    speed *= 0.5f;
                toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
                NPC.velocity.Y *= 0.98f;
                NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 0.15f * speed;
            }
            NPC.velocity.Y += 0.02f * (float)Math.Sin(MathHelper.ToRadians(ai1 * 6));
            for (int i = 0; i < 2; i++)
            {
                int num1 = Dust.NewDust(NPC.position, NPC.width - 4, 24, DustID.CursedTorch);
                Main.dust[num1].noGravity = true;
                Main.dust[num1].velocity.X = NPC.velocity.X;
                Main.dust[num1].velocity.Y = -3 + i * 1.5f;
                Main.dust[num1].scale *= 1.25f + i * 0.15f;
            }
            ai1++;
            if (ai1 >= 600)
            {
                ai1 = 0;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient && Main.rand.NextBool(75))
            {
                Vector2 spawn = (NPC.position + new Vector2(8, 8) + new Vector2(Main.rand.Next(NPC.width - 16), Main.rand.Next(NPC.height - 16)));
                Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, NPC.velocity * Main.rand.NextFloat(-0.1f, 0.1f), ModContent.ProjectileType<WarlockRune>(), Main.myPlayer, -2);
            }
            NPC.velocity = Collision.TileCollision(NPC.position + new Vector2(8, 8), NPC.velocity, NPC.width - 16, NPC.height - 16, true);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= 5f)
            {
                NPC.frameCounter -= 5f;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= 4 * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            if (NPC.life > 0)
            {
                int num = 0;
                while (num < damage / NPC.lifeMax * 50.0)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CursedTorch, (float)(2 * hitDirection) - 2f);
                    num++;
                }
            }
            else
            {
                for (int k = 0; k < 30; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CursedTorch, (float)(2 * hitDirection), -2f);
                }
            }
        }
    }
}
