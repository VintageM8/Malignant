using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Players;
using Malignant.Common.Systems;
using Malignant.Content.Items.Crimson.FleshBlazer;
using ParticleLibrary;
using Malignant.Content;
using Microsoft.Xna.Framework.Graphics;
using Malignant.Content.Buffs;
using Terraria.Audio;
using Malignant.Core;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Malignant.Content.Items.Hell.MarsHell
{
    public class Gernade1 : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Shuriken);
            Projectile.width = 30;
            Projectile.damage = 0;
            Projectile.height = 30;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 150;
            Projectile.aiStyle = 14;
            Projectile.friendly = false;
        }


        public override void AI()
        {
            Vector2 dir = Main.rand.NextVector2Unit() * 0.1f;
            float progress = 1 - (Projectile.timeLeft / 150f);
            for (int i = 0; i < 3; i++)
            {
                ParticleManager.NewParticle(Projectile.Center, dir * Main.rand.NextFloat(10, 25), ParticleManager.NewInstance<FireParticle>(), new Color(255f, 69f, 0f, 0), 0.3f, Projectile.whoAmI);

            }
        }

        public override void Kill(int timeLeft)
        {
            CameraSystem.ScreenShakeAmount = 2.5f;

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<MarsHellBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustID.InfernoFork);
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = 70 + Main.rand.Next(60);
                dust.rotation = Main.rand.NextFloat(6.28f);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustID.InfernoFork);
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = Main.rand.Next(80) + 40;
                dust.rotation = Main.rand.NextFloat(6.28f);

                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(25, 25), DustID.InfernoFork).scale = 0.9f;
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];

            player.GetModPlayer<MalignantPlayer>().itemCombo++;
            player.GetModPlayer<MalignantPlayer>().itemComboReset = 480;
        }
    }
    public class MarsHellBoom : ModProjectile
    {
        bool initilize = true;

        Vector2 spawnPosition;

        public override string Texture => "Malignant/Assets/Textures/Cirmcle"; //Shmircle

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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);

            target.AddBuff(BuffID.OnFire, 120);
            target.AddBuff(ModContent.BuffType<SmokeDebuff>(), 120);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(new Color(255, 69, 0, 0));

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
