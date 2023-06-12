using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Helper;
using Malignant.Content.Items.Crimson.Arterion.BurstingArtery;
using Malignant.Core;

namespace Malignant.Content.Items.Crimson.StaffofCarnem
{
    public class CarnemProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 11;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 5;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 2;
        }

        public float timer;

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Lighting.AddLight(Projectile.position, TorchID.Red);

            if (Projectile.localAI[0] < 48f)
            {
                Projectile.localAI[0] += 1f;
            }
            if (Projectile.ai[1] > 0f)
            {
                MethodHelper.SineWave(this.Projectile, 5f, 1f, 5, true, null, false);
            }
            else
            {
                MethodHelper.SineWave(this.Projectile, -5f, 1f, 5, true, null, false);
            }            Projectile.ai[0] += 1f;
            DelegateMethods.v3_1 = new Vector3(0.5f, 0.1f, 0.1f);

            timer++;

            if (timer >= 100)
            {
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
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 4; i++)
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Utility.PolarVector(1, MathHelper.PiOver2 * i),
                    ModContent.ProjectileType<BurstingArtyProj_Two>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Pink) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
