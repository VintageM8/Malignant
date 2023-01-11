using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Malignant.Core;
using System.Collections.Generic;
using Malignant.Content.Projectiles.Enemy.Warlock;

namespace Malignant.Content.Items.Weapon.Crimson.Arterion.StaveofCarnem
{
    public class CarnemProj : ModProjectile
    {
        private List<Vector2> cache;
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.light = 0.5f;
            Projectile.scale = 0.75f;
            Projectile.timeLeft = 150;
            DrawOffsetX = -6;
            DrawOriginOffsetY = -6;
        }        

        bool runUno;
        Vector2 initialCenter, initialVel;
        public override void AI()
        {
            if (!runUno)
            {
                initialCenter = Projectile.Center;
                initialVel = Projectile.velocity;
                runUno = true;
            }
            Utility.SineMovement(Projectile, initialCenter, initialVel, 0.15f, 45);
            Projectile.rotation += MathHelper.ToRadians(5f);

            ManageCaches();
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 14f, 0f, ModContent.ProjectileType<WarlockExplosion>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -14f, 0f, ModContent.ProjectileType<WarlockExplosion>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, 14f, ModContent.ProjectileType<WarlockExplosion>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, -14f, ModContent.ProjectileType<WarlockExplosion>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.ai[0] = 1f;
            Projectile.ai[1] = (float)target.whoAmI;
            Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
            Projectile.netUpdate = true;
            Projectile.damage = 0;

            int num31 = 3;
            Point[] array2 = new Point[num31];
            int num32 = 0;

            for (int n = 0; n < 1000; n++)
            {
                if (n != Projectile.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == Main.myPlayer && Main.projectile[n].type == Projectile.type && Main.projectile[n].ai[0] == 1f && Main.projectile[n].ai[1] == target.whoAmI)
                {
                    array2[num32++] = new Point(n, Main.projectile[n].timeLeft);
                    if (num32 >= array2.Length)
                        break;
                }
            }

            if (num32 >= array2.Length)
            {
                int num33 = 0;
                for (int num34 = 1; num34 < array2.Length; num34++)
                {
                    if (array2[num34].Y < array2[num33].Y)
                        num33 = num34;
                }
                Main.projectile[array2[num33].X].Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;


            float progress = 1 - Projectile.timeLeft / 150f;
            Color overlayColor;

            if (progress < 0.5f)
                overlayColor = Color.Lerp(new Color(0, 0, 0, 0), Color.Orange * 0.5f, progress * 2);
            else
                overlayColor = Color.Lerp(Color.Orange * 0.5f, Color.White, (progress - 0.5f) * 2);

            Texture2D mainTex = TextureAssets.Projectile[Projectile.type].Value;
            spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, mainTex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);

            Texture2D overlayTex = ModContent.Request<Texture2D>(Texture + "_White").Value;
            spriteBatch.Draw(overlayTex, Projectile.Center - Main.screenPosition, null, overlayColor, Projectile.rotation, mainTex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);

            progress *= progress;
            Color glowColor;

            if (progress < 0.5f)
                glowColor = Color.Lerp(new Color(0, 0, 0, 0), Color.Orange, progress * 2);
            else
                glowColor = Color.Lerp(Color.Orange, Color.White, (progress - 0.5f) * 2);

            Texture2D glowTex = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            spriteBatch.Draw(glowTex, Projectile.Center - Main.screenPosition, null, new Color(glowColor.R, glowColor.G, glowColor.B, 0) * 0.5f, Projectile.rotation, glowTex.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            return false;
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
    }
}

