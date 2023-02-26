using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Malignant.Core;

namespace Malignant.Content.Items.Crimson.Arterion.ArterionSoul
{
    public class ArtSoulProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.height = 3;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 3;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }
        float time, frequencyMultiplier, amplitude;
        bool runOnce;
        Vector2 initialCenter, initialVel;
        public override void AI()
        {
            if (!runOnce)
            {
                initialCenter = Projectile.Center;
                initialVel = Projectile.velocity;
                runOnce = true;
            }
            Utility.SineMovement(Projectile, initialCenter, initialVel, 0.15f, 60);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Utility.Reload(Main.spriteBatch, BlendState.Additive);
            Texture2D glow = ModContent.Request<Texture2D>("Malignant/Assets/Textures/glow2").Value;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
            {
                Main.spriteBatch.Draw(glow, Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2f, Projectile.height / 2f), new Rectangle(0, 0, glow.Width, glow.Height), Color.Crimson * (1f - fadeMult * i), Projectile.oldRot[i], glow.Size() / 2, Projectile.scale * .25f * (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type], Projectile.ai[0] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            Utility.Reload(Main.spriteBatch, BlendState.AlphaBlend);
            return false;
        }
    }
}
