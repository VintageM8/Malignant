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
            NPC.width = 44;
            NPC.height = 60;
            NPC.damage = 50;
            NPC.defense = 31;
            NPC.lifeMax = 3200;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.Venom] = true;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.CursedInferno] = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 60f;
            NPC.knockBackResist = 0.03f;
            NPC.aiStyle = 44;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.stepSpeed = 2f;
            NPC.rarity = 3;

            AIType = NPCID.FlyingFish;
        }       

        public override void AI()
        {
            Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.091f, 0.24f, .24f);

            NPC.rotation = NPC.velocity.X * .009f;
            NPC.ai[0]++;
            NPC.ai[1] += 0.04f;

            if (NPC.ai[0] == 100 || NPC.ai[0] == 240 || NPC.ai[0] == 360 || NPC.ai[0] == 620)
            {
                SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);
                Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * new Vector2(Main.rand.Next(8, 10), Main.rand.Next(8, 10));
                NPC.velocity = direction * 0.96f;
            }

            if (NPC.ai[0] >= 680)
            {
                SoundEngine.PlaySound(SoundID.Item109, NPC.Center);
                DustHelper.DrawStar(NPC.Center, DustID.GoldCoin, pointAmount: 5, mainSize: 2.25f * 2.33f, dustDensity: 2, pointDepthMult: 0.3f, noGravity: true);

                for (int i = 0; i < 5; i++)
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, Main.rand.Next(-8, 8), Main.rand.Next(-8, 8), ModContent.ProjectileType<BloodBubble>(), 30, 1, Main.myPlayer, 0, 0);

                NPC.ai[0] = 0;
            }
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
