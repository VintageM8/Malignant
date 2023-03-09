using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common;

namespace Malignant.Content.NPCs.Crimson.HeartBoss
{
    public class MiniHeart : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mini Heart");
        }

        public override void SetDefaults()
        {
            NPC.width = 88;
            NPC.height = 60;
            NPC.damage = 20;
            NPC.defense = 18;
            NPC.lifeMax = 550;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 360f;
            NPC.rarity = 2;
            NPC.knockBackResist = .45f;
            NPC.aiStyle = 14;
        }
        int aiTimer;
        public override void AI()
        {
            aiTimer++;
            if (aiTimer == 100 || aiTimer == 480)
            {
                SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);

                var direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * Main.rand.Next(6, 9);
                NPC.velocity = direction * 0.98f;
            }

            if (aiTimer >= 120 && aiTimer <= 300)
            {
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.PortalBolt);
                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].noGravity = true;

                Vector2 dustSpeed = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101)));
                dustSpeed *= (Main.rand.Next(50, 100) * 0.04f);
                Main.dust[dust].velocity = dustSpeed;
                Main.dust[dust].position = NPC.Center - Vector2.Normalize(dustSpeed) * 34f;
            }

            if (aiTimer == 300)
            {
                SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 9f;
                    int damage = Main.expertMode ? 9 : 15;

                    int amountOfProjectiles = Main.rand.Next(2, 4);
                    for (int i = 0; i < amountOfProjectiles; ++i)
                    {
                        float A = Main.rand.Next(-150, 150) * 0.01f;
                        float B = Main.rand.Next(-150, 150) * 0.01f;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<BloodBubble_Two>(), damage, 1, Main.myPlayer, 0, 0);
                    }
                }
            }

            if (aiTimer >= 500)
                aiTimer = 0;
        }

        public override void HitEffect(int hitDirection, double damage)
        {

            for (int k = 0; k < 2; k++)
            {
                Vector2 vel = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101))) * (Main.rand.Next(50, 100) * 0.04f);
                int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Blood);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = vel;
                Main.dust[dust].position = NPC.Center - (Vector2.Normalize(vel) * 34f);
            }
        }
    }
}
