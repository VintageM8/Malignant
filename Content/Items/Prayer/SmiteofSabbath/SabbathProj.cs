using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Malignant.Common.Helper;
using Mono.Cecil;
using System;

namespace Malignant.Content.Items.Prayer.SmiteofSabbath
{
    public class SabbathProj : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 28;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.timeLeft = 60;
            Projectile.scale = 1f;
        }
        public override void AI()
        {
            Dust dust;
            Vector2 position = Projectile.Center;
            dust = Main.dust[Terraria.Dust.NewDust(position, 20, 20, DustID.Web, 0, 0, 0, new Color(255, 255, 255), 0.5f)];
            Projectile.rotation += 0.5f;
            Projectile.velocity *= 0.95f;
        }


        public override void Kill(int timeLeft)
        {

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SabbathSmite>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            //DrawTrail(spriteBatch);

            float progress = 1 - Projectile.timeLeft / 150f;
            Color overlayColor;

            progress *= progress;
            Color glowColor;

            if (progress < 0.5f)
                overlayColor = Color.Lerp(new Color(0, 0, 0, 0), Color.Orange * 0.5f, progress * 2);
            else
                overlayColor = Color.Lerp(Color.Orange * 0.5f, Color.White, (progress - 0.5f) * 2);

            Texture2D mainTex = TextureAssets.Projectile[Projectile.type].Value;
            spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, mainTex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);

            Texture2D overlayTex = ModContent.Request<Texture2D>(Texture + "_White").Value;
            spriteBatch.Draw(overlayTex, Projectile.Center - Main.screenPosition, null, overlayColor, Projectile.rotation, mainTex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);


            if (progress < 0.5f)
                glowColor = Color.Lerp(new Color(0, 0, 0, 0), Color.Orange, progress * 2);
            else
                glowColor = Color.Lerp(Color.Orange, Color.White, (progress - 0.5f) * 2);

            Texture2D glowTex = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            //olor glowColor = new Color(169, 92, 255);
            glowColor.A = 0;
            Main.spriteBatch.Draw(glowTex, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.rotation, new Vector2(8, glowTex.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
            return false;

        }

    }
}
