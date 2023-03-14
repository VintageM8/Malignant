using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Malignant.Common;
using Malignant.Content.Projectiles;

namespace Malignant.Content.Items.Hell
{
    public class DemonShotProj : ModProjectile
    {
        private bool HasTouchedMouse;

        private bool initialized = false;

        private float distanceToExplode = 130;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonShotProj");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;
            distanceToExplode = Main.rand.Next(145, 175);

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            if (!initialized)
            {
                initialized = true;
                if (Projectile.Distance(Main.MouseWorld) > distanceToExplode)
                    distanceToExplode = Projectile.Distance(Main.MouseWorld) * Main.rand.NextFloat(0.9f, 1.1f);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.velocity *= 1.025f;

            if (distanceToExplode < 0)
                HasTouchedMouse = true;

            if (Main.myPlayer == Projectile.owner && Projectile.timeLeft < 230 && HasTouchedMouse) //only explodes into scrap after a certain amount of time to prevent "shotgunning"
                Explode();

            if (HasTouchedMouse)
            {
                DustHelper.DrawCircle(Projectile.Center, DustID.Torch, 2, 3, 3, 1, 2, nogravity: true);
            }


            distanceToExplode -= Projectile.velocity.Length();
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustPerfect(Projectile.Center + new Vector2(0f, 28f), DustID.CrimsonTorch, (Projectile.velocity * 0.75f).RotatedByRandom(MathHelper.ToRadians(10f)), 0, new Color(255, 255, 60) * 0.8f, 1.15f);

                Dust.NewDustPerfect(Projectile.Center, DustID.CrimsonTorch, (Projectile.velocity * Main.rand.NextFloat(0.5f, 0.6f)).RotatedByRandom(MathHelper.ToRadians(15f)), 0, new Color(150, 80, 40), Main.rand.NextFloat(0.25f, 0.5f));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            Vector2 origin = sourceRectangle.Size() / 2f;

            float offsetX = -10f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

            Color drawColor = Projectile.GetAlpha(lightColor);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture,
                Projectile.position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            return false;
        }

        public void Explode()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Main.myPlayer == Projectile.owner)
                {

                    Vector2 velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-10f, 10f))) * Main.rand.NextFloat(0.8f, 1.1f);
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<Explosion>(), (int)(Projectile.damage * 0.66f), 1f, Projectile.owner);
                }
            }
            Projectile.Kill();
        }
    }
}
