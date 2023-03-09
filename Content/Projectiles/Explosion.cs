using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace Malignant.Content.Projectiles
{
    internal class Explosion : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Explosion");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.Size = new Vector2(32, 32);
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Projectile.frameCounter++;

            if (Projectile.frameCounter % 3 == 0)
                Projectile.frame++;

            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.active = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            int frameHeight = tex.Height / Main.projFrames[Projectile.type];
            var frame = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, new Vector2(tex.Width * 0.5f, frameHeight * 0.75f), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}