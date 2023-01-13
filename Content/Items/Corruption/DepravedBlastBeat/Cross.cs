using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Malignant.Common.Projectiles.Orbiting;

namespace Malignant.Content.Items.Corruption.DepravedBlastBeat
{ 
    public class Cross : OrbitingProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = false;
            Projectile.width = 30;
            Projectile.penetrate = 1;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.light = 0f;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 60 * 60 * 3;
            ProjectileSlot = 1;
            Period = 300;
            PeriodFast = 100;
            ProjectileSpeed = 20;
            OrbitingRadius = 220;
            CurrentOrbitingRadius = 100;
        }
        public override void AI()
        {
            player = Main.player[Projectile.owner];
            RelativeVelocity = player.velocity;
            OrbitCenter = player.Center;
            base.AI();
        }
        public override void Kill(int timeLeft)
        {
            if (Proj_State == 1 || Proj_State == 2)
            {
                GeneratePositionsAfterKill();
            }

            for (int i = 0; i < 3; i++)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(7, 7), ModContent.ProjectileType<DepravedBlast_Proj2>(), Projectile.damage / 2, Projectile.knockBack);
                proj.friendly = true;
                proj.hostile = false;
                proj.scale = 0.75f;
            }
        }

        public override void Attack()
        {
            Vector2 ProjectileVelocity = Main.MouseWorld - Projectile.Center;
            if (ProjectileVelocity != Vector2.Zero)
            {
                ProjectileVelocity.Normalize();
            }
            ProjectileVelocity *= 16;
            Projectile.velocity = ProjectileVelocity;
            Proj_State = 5;
            //This method is responsible for correctly reordering the projetiles when one of them dies. We call this here to make sure the already fired projectiles do not count towards the current ones.
            GeneratePositionsAfterKill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
