using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Weapon.Corruption.Warlock.StaffofFlame
{
    public class CursedFB : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Blast");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 30;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(10))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
        }

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 14f, 0f, ModContent.ProjectileType<CFStaffProj>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -14f, 0f, ModContent.ProjectileType<CFStaffProj>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, 14f, ModContent.ProjectileType<CFStaffProj>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, -14f, ModContent.ProjectileType<CFStaffProj>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);

            for (int num621 = 0; num621 < 20; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 35; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.CursedTorch, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
        }
    }
}