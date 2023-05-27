using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Malignant.Common.Players;

namespace Malignant.Content.Items.Corruption.DepravedBlastBeat
{
    public class DepravedBlast_Proj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 18;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1f;
            AIType = ProjectileID.Bullet;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            Player player = Main.player[Projectile.owner];

            player.GetModPlayer<MalignantPlayer>().itemCombo++;
            player.GetModPlayer<MalignantPlayer>().itemComboReset = 480;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Blue.ToVector3() * 0.7f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.Kill();
            }
            return false;
        }
    }
}
