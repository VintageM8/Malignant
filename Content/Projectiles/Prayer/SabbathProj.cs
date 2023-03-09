using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Malignant.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using Malignant.Common.Helper;
using Terraria.Audio;

namespace Malignant.Content.Projectiles.Prayer
{
    public class SabbathProj : ModProjectile 
    {

        public override void SetDefaults()
        {
            Projectile.arrow = true;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 29;
            Projectile.friendly = true;

        }

        int Timer;
        Vector2 SpawnVel;
        public override void AI()
        {
            for (int i = 0; i < 360; i += 90)
            {
                Vector2 pos = Projectile.Center + new Vector2(50).RotatedBy(MathHelper.ToRadians(i + Timer * 4));
                int D = Dust.NewDust(pos, 1, 1, DustID.Clentaminator_Red);
                Main.dust[D].noGravity = true;
                // Main.dust[D].
            }
            if (Timer == 0)
            {
                SpawnVel = Projectile.velocity;
                Projectile.velocity = new Vector2(0);
            }
            Timer++;
            if (Timer > 30)
            {
                Projectile.velocity = SpawnVel * (Timer - 80) / 20;
            }
        }

        public override void Kill(int timeLeft)
        {
            Vector2 origin = Projectile.Center;
            float radius = 12;
            int numLocations = 12;
            for (int i = 0; i < 12; i++)
            {
                Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                Vector2 dustvelocity = new Vector2(0f, 0.5f).RotatedBy(MathHelper.ToRadians(360f / numLocations * i));
                int dust = Dust.NewDust(position, 2, 2, DustID.InfernoFork, dustvelocity.X, dustvelocity.Y, 0, default, 1);
                Main.dust[dust].noGravity = false;
            }
        }
        /*public override void OnTileCollide
        {

        }*/
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 14f, 0f, ModContent.ProjectileType<Fireball>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, -14f, 0f, ModContent.ProjectileType<Fireball>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0f, 14f, ModContent.ProjectileType<Fireball>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0f, -14f, ModContent.ProjectileType<Fireball>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            MethodHelper.NewDustCircular(Projectile.Center, Projectile.width * 0.1f, i => Main.rand.NextFromList(DustID.Torch, DustID.InfernoFork), 85, minMaxSpeedFromCenter: (10, 12), dustAction: d => d.noGravity = true);

        }

    }
}
