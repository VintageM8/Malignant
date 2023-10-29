using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Audio;
using Malignant.Common.Projectiles;

namespace Malignant.Content.Items.Holy.WarbladeofLight
{
    public class WarbladeSwing : HeldSword
    {
        public override string Texture => "Malignant/Content/Items/Holy/WarbladeofLight/WarbladeofLight";

        public override void SetDefaults()
        {
            SwingTime = 30;
            holdOffset = 50f;
            base.SetDefaults();
            Projectile.width = Projectile.height = 75;
            Projectile.friendly = true;
            Projectile.localNPCHitCooldown = SwingTime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override float Lerp(float val)
        {
            return val == 1f ? 1f : (val == 0f
                ? 0f
                : (float)Math.Pow(2, val * 10f - 10f) / 2f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // draws the slash
            Player player = Main.player[Projectile.owner];
            Texture2D slash = ModContent.Request<Texture2D>("Malignant/Assets/Textures/slash_02").Value;
            float mult = Lerp(Utils.GetLerpValue(0f, SwingTime, Projectile.timeLeft));
            float alpha = (float)Math.Sin(mult * Math.PI);
            Vector2 pos = player.Center + Projectile.velocity * (40f - mult * 30f);
            Main.EntitySpriteDraw(slash, pos - Main.screenPosition, null, Color.White * alpha, Projectile.velocity.ToRotation() - MathHelper.PiOver2, slash.Size() / 2, Projectile.scale / 2, SpriteEffects.None, 0);
            // draws the main blade
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 orig = texture.Size() / 2;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, orig, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
