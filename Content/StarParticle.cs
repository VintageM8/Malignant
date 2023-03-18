using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.ModLoader;

namespace Malignant.Content
{
    public class StarParticle : Particle
    {
        public override string Texture => "Divergency/Assets/Textures/ParticleTextures/SmallStar";

        public override void SetDefaults()
        {
            width = 1;
            height = 1;
            timeLeft = 30;

        }

        public override void AI()
        {
            velocity *= 0.96f;

            Scale /= 1.1f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 drawPos, Color lightColor)
        {

            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            float alpha = timeLeft > 40f ? (20f - (timeLeft - 40f)) / 20f : timeLeft <= 20f ? timeLeft / 20f : 1f;
            Color color2 = Color.Multiply(new(2.55f, 2.55f, 2.55f, 0), alpha);

            spriteBatch.Draw(texture, Center - Main.screenPosition, texture.Bounds, color * alpha, rotation, texture.Size() * 0.5f, Scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, Center - Main.screenPosition, texture.Bounds, color2, rotation, texture.Size() * 0.5f, Scale / 2f, SpriteEffects.None, 0f);


            return false;
        }
    }
}
