using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Audio;
using Malignant.Content.Buffs;

namespace Malignant.Content.Items.Weapon.Crimson.FleshBlazer
{
    public class BlazerBomb : ModProjectile
    {
        private List<Vector2> cache;

        private bool shot = false;

        private Player owner => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Scourcher Bomb");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Shuriken);
            Projectile.width = 18;
            Projectile.damage = 0;
            Projectile.height = 18;
            Projectile.timeLeft = 150;
            Projectile.aiStyle = 14;
            Projectile.friendly = false;
        }

        public override void AI()
        {
            float progress = 1 - (Projectile.timeLeft / 150f);
            for (int i = 0; i < 3; i++)
            {
                Dust sparks = Dust.NewDustPerfect(Projectile.Center + (Projectile.rotation.ToRotationVector2()) * 17, DustID.Torch, (Projectile.rotation + Main.rand.NextFloat(-0.6f, 0.6f)).ToRotationVector2() * Main.rand.NextFloat(0.4f, 1.2f));
                sparks.fadeIn = progress * 45;

            }
        }


        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);
            //Maybe later
            /*Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BombExplosion>(), Projectile.ai[0] == 0 ? 120 : 20, 2, Projectile.owner); 
            for (int i = 0; i < 3; i++)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(7, 7), ModContent.ProjectileType<HayBundle>(), Projectile.damage / 2, Projectile.knockBack, owner.whoAmI);
                proj.friendly = true;
                proj.hostile = false;
                proj.scale = 0.75f;
            }*/

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustID.Torch);
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = 70 + Main.rand.Next(60);
                dust.rotation = Main.rand.NextFloat(6.28f);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(16, 16), 0, 0, DustID.Torch);
                dust.velocity = Main.rand.NextVector2Circular(10, 10);
                dust.scale = Main.rand.NextFloat(1.5f, 1.9f);
                dust.alpha = Main.rand.Next(80) + 40;
                dust.rotation = Main.rand.NextFloat(6.28f);

                Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(25, 25), DustID.Torch).scale = 0.9f;
            }
        }

        private void ManageCaches()
        {
            if (cache == null)
            {
                cache = new List<Vector2>();
                for (int i = 0; i < 10; i++)
                {
                    cache.Add(Projectile.Center);
                }
            }

            cache.Add(Projectile.Center);

            while (cache.Count > 10)
            {
                cache.RemoveAt(0);
            }

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            target.AddBuff(BuffID.OnFire, 120);
            target.AddBuff(ModContent.BuffType<SmokeDebuff>(), 120);

        }
    }
}