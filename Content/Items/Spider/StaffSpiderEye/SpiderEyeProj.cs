
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Malignant.Common;
using MonoMod.Core.Utils;
using Terraria.Audio;
using Malignant.Content.Items.Crimson.Arterion.BurstingArtery;
using Malignant.Content.Items.Hell.MarsHell;
using Malignant.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Malignant.Common.Helper;

namespace Malignant.Content.Items.Spider.StaffSpiderEye
{
    public class SpiderEyeProj : ModProjectile
    {
        public override string Texture => "Malignant/Content/Items/Spider/SpiderNeckless/SpiderFangProjectile";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
        }

        public Trail trail;
        public Trail trail2;

        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D trailTexture = ModContent.Request<Texture2D>("Malignant/Assets/Textures/Trails/Stretched").Value;

            if (trail == null)
            {
                trail = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(25f), (p) => Projectile.GetAlpha(new Color(148, 0, 211, 100)));
                trail.drawOffset = Projectile.Size * 0f;
            }
            if (trail2 == null)
            {
                trail2 = new Trail(trailTexture, Trail.DefaultPass, (p) => new Vector2(10f), (p) => Projectile.GetAlpha(new Color(255, 255, 255, 100)));
                trail2.drawOffset = Projectile.Size * 0f;
            }

            trail.Draw(Projectile.oldPos);
            trail2.Draw(Projectile.oldPos);

            return false;
        }
    }

}