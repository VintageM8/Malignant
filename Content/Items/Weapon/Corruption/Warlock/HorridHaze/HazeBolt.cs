using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;

namespace Malignant.Content.Items.Weapon.Warlock.HorridHaze
{
    public class HazeBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Haze Bolt");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 12;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.timeLeft = 200;

            Projectile.penetrate = -1;

            Projectile.aiStyle = 1;
        }

        public override bool PreAI()
        {
            if (Projectile.ai[0] == 0)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            else
            {
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                int num996 = 15;
                bool flag52 = false;
                bool flag53 = false;
                Projectile.localAI[0] += 1f;
                if (Projectile.localAI[0] % 30f == 0f)
                    flag53 = true;

                int num997 = (int)Projectile.ai[1];
                if (Projectile.localAI[0] >= (float)(60 * num996))
                    flag52 = true;
                else if (num997 < 0 || num997 >= 200)
                    flag52 = true;
                else if (Main.npc[num997].active && !Main.npc[num997].dontTakeDamage)
                {
                    Projectile.Center = Main.npc[num997].Center - Projectile.velocity * 2f;
                    Projectile.gfxOffY = Main.npc[num997].gfxOffY;
                    if (flag53)
                    {
                        Main.npc[num997].HitEffect(0, 1.0);
                    }
                }
                else
                    flag52 = true;

                if (flag52)
                    Projectile.Kill();
            }
            return false;
        }


        

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 3);
                Main.dust[d].scale *= 0.8f;
            }
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);

            if (base.Projectile.owner == Main.myPlayer)
            {
                base.Projectile.localAI[1] = -1f;
                base.Projectile.maxPenetrate = 0;
                base.Projectile.Damage();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            {
                Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.3f, 0.8f, 1.1f);
                Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Width() * 0.5f, Projectile.height * 0.5f);
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                    Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    Main.EntitySpriteDraw((Texture2D)TextureAssets.Projectile[Projectile.type], drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                }
                return true;
            }
        }
    }
}
