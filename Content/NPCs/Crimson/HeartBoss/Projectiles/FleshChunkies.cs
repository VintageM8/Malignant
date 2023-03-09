using Malignant.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Content.Projectiles;
using System;

namespace Malignant.Content.NPCs.Crimson.HeartBoss.Projectiles
{
    public class FleshChunkies : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flesh Chunks");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(3);
            AIType = 3;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            //Projectile.rotation += 1f;
            Lighting.AddLight(Projectile.Center, Projectile.Opacity * 0.8f, Projectile.Opacity * 0.2f, Projectile.Opacity * 0.2f);
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            Projectile.velocity *= 1.02f;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle((int)(Projectile.position.X - Projectile.width / 2), (int)(Projectile.position.Y - Projectile.height / 2), Projectile.width * 2, Projectile.height * 2);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 8)
            {
                Vector2 circularLocation = new Vector2(-20, 0).RotatedBy(MathHelper.ToRadians(i));
                int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.Blood);
                Main.dust[num1].noGravity = true;
                Main.dust[num1].scale = 2.25f;
                Main.dust[num1].velocity = circularLocation * 0.35f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.myPlayer == player.whoAmI)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), new Vector2(Projectile.Center.X, Projectile.Center.Y - 36), Vector2.Zero, ModContent.ProjectileType<SmallTendrails>(), Projectile.damage, 3, Projectile.owner);
            }
            return true;
        }
    }
}