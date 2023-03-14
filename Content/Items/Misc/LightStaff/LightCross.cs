using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Malignant.Core;
using Terraria.Audio;
using Malignant.Common.Helper;
using IL.Terraria.GameContent;
using On.Terraria.GameContent;
using Malignant.Common;
using Malignant.Content.Items.Crimson.Arterion.BurstingArtery;
using Malignant.Content.Projectiles;

namespace Malignant.Content.Items.Misc.LightStaff
{
    public class LightCross : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 28;
            Projectile.timeLeft = 520;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.timeLeft <= 420;
        }
        public override void AI()
        {
            if (Projectile.ai[0]++ >= 30 && Projectile.ai[0] <= 240)
            {
                Projectile.velocity *= 0.9f;
                Projectile.rotation.SlowRotation(0, (float)Math.PI / 20);
            }
            if (Projectile.timeLeft == 420)
            {
                SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
                for (int i = 0; i < 360; i += 5)
                {
                    Vector2 circular = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i));
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5) + circular, 0, 0, DustID.GoldFlame, 0, 0, Projectile.alpha);
                    dust.velocity *= 0.15f;
                    dust.velocity += -Projectile.velocity;
                    dust.scale = 2.75f;
                    dust.noGravity = true;
                }
            }
            if (Projectile.timeLeft > 420)
            {
                Player player = Main.player[(int)Projectile.ai[0]];
                if (player.active)
                {
                    Vector2 toPlayer = Projectile.Center - Main.MouseWorld;
                    toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * 12;
                    Projectile.velocity = -toPlayer;
                }
            }
            else
            {
                Projectile.hostile = false;
                int dust = Dust.NewDust(Projectile.Center + new Vector2(-4, -4), 0, 0, 164, 0, 0, Projectile.alpha, default, 1.25f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.1f;
                Main.dust[dust].scale *= 0.75f;
            }

            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.8f / 255f, (255 - Projectile.alpha) * 0.0f / 255f, (255 - Projectile.alpha) * 0.0f / 255f);
            if (Projectile.timeLeft <= 25)
                Projectile.alpha += 10;

            if (Projectile.ai[0] == 60 && Main.myPlayer == Projectile.owner)
            {
                Projectile.ai[1] = 10;
                SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
                Vector2 velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-10f, 10f))) * Main.rand.NextFloat(0.8f, 1.1f);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, velocity, ModContent.ProjectileType<HomingFireball>(), (int)(Projectile.damage * 0.66f), 1f, Projectile.owner);

            }
        }
    }   
}