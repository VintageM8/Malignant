using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;
using Malignant.Common.Systems;
using Microsoft.Xna.Framework.Graphics;
using Malignant.Content.NPCs.Crimson.HeartBoss;
using Malignant.Content.Projectiles;

namespace Malignant.Content.Items.Weapon.Spider.AraneaeFangs
{
    public class Fangs : ModProjectile
    {
        private List<Vector2> cache;


        private Player Owner => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fangs");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Shuriken);
            Projectile.width = 18;
            Projectile.damage = 0;
            Projectile.height = 18;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 150;
            Projectile.aiStyle = 14;
            Projectile.friendly = false;
        }

        public override void Kill(int timeLeft)
        {
            //for (int i = 1; i < 4; i++)
            //Dust.NewDust(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(5, 5), Mod.Find<ModGore>("ImpactSMG_Gore" + i.ToString()).Type, 1f);
            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Explosion>(), 0, 0, Owner.whoAmI);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projHitbox.TopLeft() - new Vector2(8, 8), projHitbox.Size() + new Vector2(16, 16)))
                return true;

            return false;
        }

        public override void AI()
        {
            ManageCaches();

        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            CameraSystem.ScreenShakeAmount += 3;

            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.4f, ModContent.ProjectileType<BloodBubble>(), 0, 0, Owner.whoAmI, Main.rand.Next(15, 25), Projectile.velocity.ToRotation());
            proj.extraUpdates = 0;

            for (int i = 0; i < 7; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, 6, -Projectile.velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(), 0, default, 1.25f).noGravity = true;
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
