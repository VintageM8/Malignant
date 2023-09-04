using Malignant.Common.Helper;
using Malignant.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Players;
using Malignant.Common.Systems;
using Malignant.Content.Items.Crimson.FleshBlazer;
using Terraria.Audio;

namespace Malignant.Content.Items.Corruption.DepravedBlastBeat
{
    public class DepravedBlast_Proj2 : ModProjectile
    {
        public override string Texture => "Malignant/Content/Items/Corruption/DepravedBlastBeat/DepravedBlast_Proj";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
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

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1f;
            AIType = ProjectileID.Bullet;

            //drawOriginOffsetX = -5;
            //drawOriginOffsetY = -20;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.position);
            CameraSystem.ScreenShakeAmount = 5f;
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CorruptVortex>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            for (int k = 0; k < 20; k++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.GemAmethyst, Main.rand.NextVector2Circular(1f, 1f) * 10, 0, default, 2f).noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            Player player = Main.player[Projectile.owner];

            player.GetModPlayer<MalignantPlayer>().itemCombo++;
            player.GetModPlayer<MalignantPlayer>().itemComboReset = 480;


        }

        public Trail trail;
        public Trail trail2;

        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D trailTexture = ModContent.Request<Texture2D>("Malignant/Assets/Textures/Trails/Squigg").Value;

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
    public class CorruptVortex : ModProjectile
    {
        bool initilize = true;

        Vector2 spawnPosition;

        public override string Texture => "Malignant/Assets/Textures/vortex2"; //Shmircle

        public override bool ShouldUpdatePosition() => false;

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.Size = new Vector2(10f);
            Projectile.scale = 0.01f;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 100;
        }

        public override void AI()
        {
            if (initilize)
            {
                spawnPosition = Projectile.Center;

                initilize = false;
            }

            Projectile.Center = spawnPosition;

            Projectile.scale += 0.02f;
            Projectile.Size += new Vector2(10f);
            Projectile.alpha += 10;
            Projectile.localAI[0] = MathHelper.Lerp(0.001f, 5f, 0.05f);
            Projectile.rotation += Projectile.localAI[0];

            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(new Color(148, 0, 211, 0));

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }

}
