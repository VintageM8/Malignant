using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Snow.Cocytus.NjorStaff
{
    public class NjorsStaffproj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Njors Staff");
        }

        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 29;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            //AIType = ProjectileID.IceBolt;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 origin = Projectile.Center;
            float radius = 10;
            int numLocations = 12;
            for (int i = 0; i < 12; i++)
            {
                Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                Vector2 dustvelocity = new Vector2(0f, 0.5f).RotatedBy(MathHelper.ToRadians(360f / numLocations * i));
                int dust = Dust.NewDust(position, 2, 2, DustID.Snow, dustvelocity.X, dustvelocity.Y, 0, default, 1);
                Main.dust[dust].noGravity = false;
            }
        }

        public Trail trail;
        public Trail trail2;
        private bool initialized;

        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D trailTexture = ModContent.Request<Texture2D>("Malignant/Assets/Textures/Trails/Stretched").Value;

            if (trail == null)
            {
                trail = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(40f), (p) => Projectile.GetAlpha(new Color(115, 115, 208, 100)));
                trail.drawOffset = Projectile.Size / 2f;
            }
            if (trail2 == null)
            {
                trail2 = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(15f), (p) => Projectile.GetAlpha(new Color(0, 0, 128, 100)));
                trail2.drawOffset = Projectile.Size / 2f;
            }

            trail.Draw(Projectile.oldPos);
            trail2.Draw(Projectile.oldPos);
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor);

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(6) == 0)
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
        }
    }
}
