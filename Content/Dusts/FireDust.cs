using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Dusts
{
    public class FireDust : ModDust
    {
        public override string Texture => "Malignant/Assets/Textures/Pixel";
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
            dust.scale = 0.35f;
            dust.customData = Main.rand.Next(1, 3);
            base.OnSpawn(dust);

        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.03f;
            dust.velocity *= 0.95f;
            if (dust.scale <= 0)
                dust.active = false;

            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<FireDust>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("Malignant/Assets/Textures/ParticleTextures/Flame" + d.customData).Value;
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, d.scale * 0.85f, SpriteEffects.None, 0);
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.OrangeRed, 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0); ;
                }
            }
        }
    }
    public class FireD_2 : ModDust
    {
        public override string Texture => "Malignant/Assets/Textures/Pixel";
        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 255;
            dust.noLight = true;
            dust.noGravity = true;
            dust.scale = 0.35f;
            dust.customData = Main.rand.Next(1, 3);
            base.OnSpawn(dust);

        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.scale -= 0.03f;
            dust.velocity *= 0.95f;
            if (dust.scale <= 0)
                dust.active = false;

            return false;
        }
        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<FireD_2>() && d.active)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("Malignant/Assets/Textures/ParticleTextures/Flame" + d.customData).Value;
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, d.scale * 0.85f, SpriteEffects.None, 0);
                    sb.Draw(tex, d.position - Main.screenPosition, null, d.color, 0, tex.Size() / 2, d.scale, SpriteEffects.None, 0); ;
                }
            }
        }
    }
}
