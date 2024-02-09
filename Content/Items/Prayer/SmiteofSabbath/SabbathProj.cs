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
using Malignant.Common.Systems;

namespace Malignant.Content.Items.Prayer.SmiteofSabbath
{
    public class SabbathProj : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 28;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;
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
                if (Projectile.ai[0] < 60) //Moves projectile to the players cursor.
                {
                    Projectile.timeLeft = 60;
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
    


        public override void Kill(int timeLeft)
        {
            CameraSystem.ScreenShakeAmount = 3f;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);

            for (int i = 0; i < 25; i++)
            {
                Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(), Main.rand.Next(new int[] { GoreID.Smoke1, GoreID.Smoke2, GoreID.Smoke3 }), Main.rand.NextFloat(0.25f, 1f));
            }

            for (int k = 0; k < 20; k++)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.GemRuby, Main.rand.NextVector2Circular(1f, 1f) * 10, 0, default, 2f).noGravity = true;
            }

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SabbathSmite>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            //DrawTrail(spriteBatch);

            float progress = 1 - Projectile.timeLeft / 150f;
            Color overlayColor;

            progress *= progress;
            Color glowColor;

            if (progress < 0.5f)
                overlayColor = Color.Lerp(new Color(0, 0, 0, 0), Color.Orange * 0.5f, progress * 2);
            else
                overlayColor = Color.Lerp(Color.Orange * 0.5f, Color.White, (progress - 0.5f) * 2);

            Texture2D mainTex = TextureAssets.Projectile[Projectile.type].Value;
            spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, mainTex.Size() / 3, Projectile.scale, SpriteEffects.None, 0f);

            Texture2D overlayTex = ModContent.Request<Texture2D>(Texture + "_White").Value;
            spriteBatch.Draw(overlayTex, Projectile.Center - Main.screenPosition, null, overlayColor, Projectile.rotation, mainTex.Size() / 3, Projectile.scale, SpriteEffects.None, 0f);


            if (progress < 0.5f)
                glowColor = Color.Lerp(new Color(0, 0, 0, 0), Color.Orange, progress * 2);
            else
                glowColor = Color.Lerp(Color.Orange, Color.White, (progress - 0.5f) * 2);

            Texture2D glowTex = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            //olor glowColor = new Color(169, 92, 255);
            glowColor.A = 0;
            Main.spriteBatch.Draw(glowTex, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.rotation, new Vector2(8, glowTex.Height / 3), Projectile.scale, SpriteEffects.None, 0f);
            return false;

        }

    }
}
