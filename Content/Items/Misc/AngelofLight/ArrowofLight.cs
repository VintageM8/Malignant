using Malignant.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Content.Dusts;
using System;
using Malignant.Content.Items.Crimson.FleshBlazer;

namespace Malignant.Content.Items.Misc.AngelofLight
{
    public class ArrowofLight : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PrayerUse>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            Main.dust[d].noGravity = true;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity.Y += 0.1f;
            Projectile.alpha = 0;
        }

        private Player owner => Main.player[Projectile.owner];
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (MalignantLists.unholyEnemies.Contains(target.type))
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(7, 7), ModContent.ProjectileType<ScourcherBible>(), Projectile.damage / 2, Projectile.knockBack, owner.whoAmI);
                    proj.friendly = true;
                    proj.hostile = false;
                }

            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            var effects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);
            Color color = new(255, 255, 255, 0);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color oldColor = color * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(oldColor), Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
            return false;
        }

        /*public override void Kill(int timeLeft)
        {
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<HolyDust>(), Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, Color.White, Main.rand.NextFloat(1, 1.75f)).noGravity = true;
            }
        }*/
    }
}
