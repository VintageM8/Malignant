
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

namespace Malignant.Content.Items.Spider.StaffSpiderEye
{
    public class SpiderEyeProj : ModProjectile
    {
        public override string Texture => "Malignant/Content/Items/Spider/SpiderNeckless/SpiderFangProjectile";

        private bool HasTouchedMouse;

        private bool initialized = false;

        private float distanceToExplode = 130;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;

            Projectile.timeLeft = 240;
            Projectile.penetrate = 1;
            distanceToExplode = Main.rand.Next(145, 175);

            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            if (!initialized)
            {
                initialized = true;
                if (Projectile.Distance(Main.MouseWorld) > distanceToExplode)
                    distanceToExplode = Projectile.Distance(Main.MouseWorld) * Main.rand.NextFloat(0.9f, 1.1f);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.velocity *= 1.025f;

            if (distanceToExplode < 0)
                HasTouchedMouse = true;

            if (Main.myPlayer == Projectile.owner && Projectile.timeLeft < 230 && HasTouchedMouse)
                SplitIntoMore();

            if (HasTouchedMouse)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SpiderViz>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }


            distanceToExplode -= Projectile.velocity.Length();
        }

        public void SplitIntoMore()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-10f, 10f))) * Main.rand.NextFloat(0.8f, 1.1f);
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<BurstingArtyProj_Two>(), (int)(Projectile.damage * 0.66f), 1f, Projectile.owner);
                }
            }
            Projectile.Kill();
        }
    }
    public class SpiderViz : ModProjectile
    {
        bool initilize = true;

        Vector2 spawnPosition;

        public override string Texture => "Malignant/Assets/Textures/Rune1"; //Shmircle

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
            Color color = Projectile.GetAlpha(new Color(128, 0, 128, 0));

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }

}