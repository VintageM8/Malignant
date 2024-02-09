using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Malignant.Common.Helper;
using Mono.Cecil;
using System;
using Malignant.Common.Projectiles;

namespace Malignant.Content.Items.Prayer.ChivalrousMirror
{
    public class ChivalrousMirror : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 28;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 300;
            Projectile.scale = 1f;
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
                if (Projectile.ai[0] < 300) //Moves projectile to the players cursor.
                {
                    Projectile.timeLeft = 300;
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
                //SoundEngine.PlaySound(SoundID. Projectile.position)
            }
        }
    }
    public class PrayerShit //Cloneing shit
    {
        public static void ElfAbility(Player player)
        {
            Projectile temp = Projectile.NewProjectileDirect(player.GetSource_Misc("MirrorPrayer"), player.Center, Vector2.Zero, ModContent.ProjectileType<ChivalrousMirror>(), 1, 1, player.whoAmI);
            MaligGlobalProjectile globalprojectileClone = temp.GetGlobalProjectile<MaligGlobalProjectile>();
            globalprojectileClone.Cloned = true;
        }
    }

}
