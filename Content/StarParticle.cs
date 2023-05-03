using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleLibrary;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Malignant.Core;

namespace Malignant.Content
{
    public class StarParticle : Particle //Finna rename this bitch
    {

        private int frameCount;
        private int frameTick;
        public override string Texture => "Malignant/Assets/Textures/Pixel";
        public override void SetDefaults()
        {
            width = 34;
            height = 34;
            Scale = 1f;
            timeLeft = 40;
        }


        public override void AI()
        {
            rotation = velocity.ToRotation();

            velocity *= 0.98f;
            Scale *= 1.05f;
            if (Scale <= 0f)
                active = false;
            opacity = 125f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Texture2D meow = Request<Texture2D>("Malignant/Assets/Textures/ParticleTextures/Particle1").Value;
            Texture2D weed = Request<Texture2D>("Malignant/Assets/Textures/ParticleTextures/Particle1").Value;
            Texture2D Fmrnch = Request<Texture2D>("Malignant/Assets/Textures/ParticleTextures/Particle1").Value; //I hate the French

            float alpha = timeLeft <= 20 ? 1f - 1f / 20f * (20 - timeLeft) : 1f;
            if (alpha < 0f) alpha = 0f;
            Color color = Color.Multiply(new(2.58f, 1.39f, 0.95f, 0), alpha / 2);
            Color color2 = Color.Multiply(new(2.58f, 1.39f, 0.95f, 0), alpha / 5);

            spriteBatch.Draw(meow, position - Main.screenPosition, meow.AnimationFrame(ref frameCount, ref frameTick, 7, 7, true), color2, 0f, new Vector2(meow.Width / 2f, meow.Height / 2f / 7f), Scale / 3.2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(Fmrnch, position - Main.screenPosition, new Rectangle(0, 0, Fmrnch.Width, Fmrnch.Height), color, rotation, new Vector2(Fmrnch.Width / 2f, Fmrnch.Height / 2f), 0.17f * Scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
