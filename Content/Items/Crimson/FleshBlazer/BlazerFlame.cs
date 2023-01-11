using Malignant.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Crimson.FleshBlazer
{
    public class BlazerFlame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 90;
            Projectile.extraUpdates = 2;
            Projectile.scale = Main.rand.NextFloat(0.3f, 0.7f);
            Projectile.alpha = 255;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void AI()
        {
            float progress = 1 - (Projectile.timeLeft / 20f);
            for (int i = 0; i < 3; i++)
            {
                Dust sparks = Dust.NewDustPerfect(Projectile.Center + (Projectile.rotation.ToRotationVector2()) * 17, DustID.Torch, (Projectile.rotation + Main.rand.NextFloat(-0.6f, 0.6f)).ToRotationVector2() * Main.rand.NextFloat(0.4f, 1.2f));
                sparks.fadeIn = progress * 45;

            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.755f, 0.140f, 0f));
            if (Projectile.timeLeft <= 20)
            {
                Projectile.scale -= 0.02f;
            }
            if (Projectile.scale <= 0)
            {
                Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            target.AddBuff(BuffID.OnFire, 120);
            target.AddBuff(ModContent.BuffType<SmokeDebuff>(), 120);

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                var offset = new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + offset;
                Color color = new Color(252, 152, 3, Projectile.oldPos.Length * 6) * (1f - Projectile.alpha) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.oldRot[k], texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);


            return false;
        }
    }
}
