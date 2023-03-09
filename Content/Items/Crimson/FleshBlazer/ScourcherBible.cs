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

namespace Malignant.Content.Items.Crimson.FleshBlazer
{
    public class ScourcherBible : ModProjectile
    { 
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Scourcher Bible");
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];           

            if (Projectile.ai[0]++ >= 30 && Projectile.ai[0] <= 240)
            {
                Projectile.velocity *= 0.9f;
                Projectile.rotation.SlowRotation(0, (float)Math.PI / 20);
            }
            else if (Projectile.owner == player.whoAmI)
            {
                if (Projectile.ai[0] < 30)
                {
                    Projectile.timeLeft = 600;
                    Projectile.ai[0] = 0;
                    Projectile.Move(Main.MouseWorld, 10, 10);
                    if (Projectile.DistanceSQ(Main.MouseWorld) < 60 * 60)
                        Projectile.ai[0] = 30;
                }
                Projectile.LookByVelocity();
                Projectile.rotation += Projectile.velocity.Length() / 50 * Projectile.spriteDirection;
            }
            if (Projectile.ai[0] == 60 && Main.myPlayer == Projectile.owner)
            {
                Projectile.ai[1] = 10;
                SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
                for (int i = 0; i < 4; i++)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Utility.PolarVector(1, MathHelper.PiOver2 * i),
                        ModContent.ProjectileType<BurstingArtyProj_Two>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
                Projectile.Kill();
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                //for some reason the BuzzSpark dust spawns super offset 
                MethodHelper.DrawCircle(Projectile.Center, DustID.GoldCoin, 2, 4, 4, 1, 2, nogravity: true);
            }
        }
    }
}
