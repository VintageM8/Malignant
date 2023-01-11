using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Malignant.Content.NPCs.Crimson.Heart
{
    public class BloodySpit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloody Spit");
        }
        public override void SetDefaults()
        {
            Projectile.width = 400;
            Projectile.height = 400;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.timeLeft = 200;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 1f)
            {
                Projectile.alpha += 6;
                if (Projectile.alpha >= 255)
                    Projectile.Kill();
            }
            else
            {
                Projectile.alpha -= 20;
                if (Projectile.alpha <= 0)
                    Projectile.localAI[0] = 1f;
            }
           
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Vector2 origin = new(texture.Width / 2f, texture.Height / 2f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Main.EntitySpriteDraw(texture, position, null, Color.Crimson * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale, 0, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}
