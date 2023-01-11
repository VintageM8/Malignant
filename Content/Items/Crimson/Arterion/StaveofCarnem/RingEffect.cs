using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Weapon.Crimson.Arterion.StaveofCarnem
{
    public class RingEffect : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 999;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }

        public int count = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.channel)
            {
                if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f)
                {
                    Projectile.position = new Vector2(Main.MouseWorld.X - 256, Main.MouseWorld.Y - 256);
                }
            }
            else
            {
                Projectile.Kill();
            }

            if (player.ownedProjectileCounts[ModContent.ProjectileType<innerRing>()] < 1)
            {
                Projectile.NewProjectile(null, Projectile.position, Projectile.velocity, ModContent.ProjectileType<innerRing>(), 0, 0, player.whoAmI);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<outerRing>()] < 1)
            {
                Projectile.NewProjectile(null, Projectile.position, Projectile.velocity, ModContent.ProjectileType<outerRing>(), 0, 0, player.whoAmI);
            }

            count += 1;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];

                int distance = (int)Vector2.Distance(new Vector2(Projectile.Center.X + 256, Projectile.Center.Y + 256), target.Center);
            }
        }
    }

    public class innerRing : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 512;
            Projectile.height = 512;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 999;
            Projectile.penetrate = -1;
            Projectile.scale = 0.5f;
            DrawOffsetX = -128;
            DrawOriginOffsetY = -128;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.channel)
            {
                if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f)
                {
                    Projectile.position = new Vector2(Main.MouseWorld.X - 128, Main.MouseWorld.Y - 128);
                }
            }
            else
            {
                Projectile.Kill();
            }

            Projectile.rotation -= 0.05f;

            Lighting.AddLight(Projectile.Center, TorchID.Ichor);
        }
    }

    public class outerRing : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 512;
            Projectile.height = 512;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 999;
            Projectile.penetrate = -1;
            Projectile.scale = 0.5f;
            DrawOffsetX = -128;
            DrawOriginOffsetY = -128;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player.channel)
            {
                if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f)
                {
                    Projectile.position = new Vector2(Main.MouseWorld.X - 128, Main.MouseWorld.Y - 128);
                }
            }
            else
            {
                Projectile.Kill();
            }

            Projectile.rotation += 0.05f;

            Lighting.AddLight(Projectile.Center, TorchID.Ichor);
        }
    }
}