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
        public override Color? GetAlpha(Color lightColor) => new(251, 139, 95, 100);

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.width = Projectile.height = 60;
            Projectile.scale = 1f;
            Projectile.penetrate = 3;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.alpha = 255;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 30;
            Projectile.hide = true;
            Projectile.CritChance = 0;

        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.active && player.Hitbox.Intersects(Projectile.Hitbox))
            {
                player.AddBuff(BuffID.DryadsWard, 180, true, false);

            }
            Vector3 RGB = new Vector3(2.51f, 1.39f, 0.95f);
            float multiplier = 1f;
            float max = 2f;
            float min = 1f;
            RGB *= multiplier;
            if (RGB.X > max)
            {
                multiplier = 0.5f;
            }
            if (RGB.X < min)
            {
                multiplier = 0.6f;
            }
            Lighting.AddLight(Projectile.position, RGB.X, RGB.Y, RGB.Z);

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
