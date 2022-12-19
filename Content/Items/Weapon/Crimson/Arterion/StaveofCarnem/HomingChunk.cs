using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Malignant.Effects;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Weapon.Crimson.Arterion.StaveofCarnem
{
    public class HomingChunk : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.light = 0.5f;
            Projectile.scale = 0.75f;
            Projectile.timeLeft = 240;
            DrawOriginOffsetY = -1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Bleeding, 60);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Vector2 usePos = Projectile.position;

            Vector2 rotVector = (Projectile.rotation - MathHelper.ToRadians(90f)).ToRotationVector2();
            usePos += rotVector * 16f;

            for (int i = 0; i < 10; i++)
            {
                Dust dust;
                dust = Main.dust[Terraria.Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 0)];
                dust.noGravity = true;
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.Center, 0.8f, 0.07f, 0f);

            float num132 = (float)Math.Sqrt((double)(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y));
            float num133 = Projectile.localAI[0];
            if (num133 == 0f)
            {
                Projectile.localAI[0] = num132;
                num133 = num132;
            }
            float num134 = Projectile.position.X;
            float num135 = Projectile.position.Y;
            float num136 = 800f;
            bool flag3 = false;
            int num137 = 0;
            if (Projectile.ai[1] == 0f)
            {
                for (int num138 = 0; num138 < 200; num138++)
                {
                    if (Main.npc[num138].CanBeChasedBy(this, false) && (Projectile.ai[1] == 0f || Projectile.ai[1] == num138 + 1))
                    {
                        float num139 = Main.npc[num138].position.X + Main.npc[num138].width / 2;
                        float num140 = Main.npc[num138].position.Y + Main.npc[num138].height / 2;
                        float num141 = Math.Abs(Projectile.position.X + Projectile.width / 2 - num139) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num140);
                        if (num141 < num136 && Collision.CanHit(new Vector2(Projectile.position.X + Projectile.width / 2, Projectile.position.Y + Projectile.height / 2), 1, 1, Main.npc[num138].position, Main.npc[num138].width, Main.npc[num138].height))
                        {
                            num136 = num141;
                            num134 = num139;
                            num135 = num140;
                            flag3 = true;
                            num137 = num138;
                        }
                    }
                }
                if (flag3)
                {
                    Projectile.ai[1] = num137 + 1;
                }
                flag3 = false;
            }
            if (Projectile.ai[1] > 0f)
            {
                int num142 = (int)(Projectile.ai[1] - 1f);
                if (Main.npc[num142].active && Main.npc[num142].CanBeChasedBy(this, true) && !Main.npc[num142].dontTakeDamage)
                {
                    float num143 = Main.npc[num142].position.X + Main.npc[num142].width / 2;
                    float num144 = Main.npc[num142].position.Y + Main.npc[num142].height / 2;
                    if (Math.Abs(Projectile.position.X + Projectile.width / 2 - num143) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num144) < 1000f)
                    {
                        flag3 = true;
                        num134 = Main.npc[num142].position.X + Main.npc[num142].width / 2;
                        num135 = Main.npc[num142].position.Y + Main.npc[num142].height / 2;
                    }
                }
                else
                {
                    Projectile.ai[1] = 0f;
                }
            }
            if (!Projectile.friendly)
            {
                flag3 = false;
            }
            if (flag3)
            {
                float num145 = num133;
                Vector2 vector10 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
                float num146 = num134 - vector10.X;
                float num147 = num135 - vector10.Y;
                float num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
                num148 = num145 / num148;
                num146 *= num148;
                num147 *= num148;
                int num149 = 8;
                Projectile.velocity.X = (Projectile.velocity.X * (num149 - 1) + num146) / num149;
                Projectile.velocity.Y = (Projectile.velocity.Y * (num149 - 1) + num147) / num149;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            default(RedTrail).Draw(base.Projectile);
            Texture2D value = ModContent.Request<Texture2D>("Malignant/Content/Items/Weapon/Crimson/Arterion/StaveofCarnem/CarnemProj", (ReLogic.Content.AssetRequestMode)2).Value;
            return true;
        }
    }
}
