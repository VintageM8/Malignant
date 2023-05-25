using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Malignant.Common.Helper;
using Malignant.Common.Projectiles.Orbiting;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.DataStructures;
using System.Linq;

namespace Malignant.Content.Items.Corruption.Warlock.NightsisterBlade
{
    public class NightsisterBladeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Blade");
        }

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.netImportant = true;

            Projectile.localNPCHitCooldown = TIME_LEFT_ONHIT + 10;
            Projectile.usesLocalNPCImmunity = true;
        }

        Player Player => Main.player[Projectile.owner];

        public bool circling = true;
        public override void AI()
        {
            if (circling)
            {
                int circlingCount = CirclingCount(Player, Projectile, out int id);

                if (circlingCount == 0) return;

                Vector2[] positions = MethodHelper.GenerateCircularPositions(Player.Center, 90, circlingCount);

                Projectile.Center = Vector2.Lerp(Projectile.Center, positions[id], 0.4f);

                if (Main.myPlayer == Player.whoAmI)
                {
                    Projectile.ai[0] = Projectile.Center.DirectionTo(Main.MouseWorld).ToRotation();
                    Projectile.netUpdate = true;
                }

                Projectile.rotation = Projectile.ai[0];

                if (!Player.controlUseItem)
                {
                    Projectile.velocity = Vector2.Zero;
                    Projectile.friendly = true;

                    int additionalDamage = (int)(Projectile.damage * 0.1f * circlingCount);
                    Projectile.damage += additionalDamage;

                    circling = false;
                }

                Projectile.timeLeft = 300;
            }
            else
            {
                if (Projectile.timeLeft > TIME_LEFT_ONHIT && Projectile.velocity.LengthSquared() < 30 * 30)
                {
                    Projectile.velocity += Projectile.rotation.ToRotationVector2() * 6;
                }
                else
                {
                    alpha -= 1f / TIME_LEFT_ONHIT;
                    Projectile.velocity *= 0.55f;
                }
            }
            
            FX();
        }
        
        public static int CirclingCount(Player player)
        {
            return player.OwnedProjectiles(ModContent.ProjectileType<NightsisterBladeProjectile>()).Count(proj => !(proj.ModProjectile as NightsisterBladeProjectile).circling);
        }

        public static int CirclingCount(Player player, Projectile whoAmIProj, out int whoAmI)
        {
            whoAmI = 0;
            bool updateWhoAmI = true;

            int count = 0;
            foreach (Projectile proj in player.OwnedProjectiles(whoAmIProj.type))
            {
                if ((proj.ModProjectile as NightsisterBladeProjectile).circling)
                {
                    if (proj.whoAmI == whoAmIProj.whoAmI)
                        updateWhoAmI = false;

                    if (updateWhoAmI)
                        whoAmI++;

                    count++;
                }
            }

            return count;
        }

        const int TIME_LEFT_ONHIT = 30;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life - damage > 0 && Projectile.timeLeft > TIME_LEFT_ONHIT) Projectile.timeLeft = TIME_LEFT_ONHIT;
        }

        public override bool ShouldUpdatePosition() => !circling;

        void FX()
        {
            if (Main.rand.NextBool())
            {
                int dustType = Main.rand.NextFromList(new int[]
                {
                    DustID.ShadowbeamStaff,
                    DustID.ShadowbeamStaff,
                    DustID.AmberBolt
                });

                Vector2 rotVector = Projectile.rotation.ToRotationVector2();
                Dust.NewDustDirect(
                    Projectile.Center + rotVector * (Main.rand.Next(swordLength) - swordLength * 0.5f) - rotVector.RotatedBy(MathHelper.PiOver2) * 7,
                    15,
                    15,
                    dustType
                    ).noGravity = true;
            }
        }

        const int swordLength = 90;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 rotVector = Projectile.rotation.ToRotationVector2();
            Vector2 startCollision = Projectile.Center - rotVector * swordLength * 0.5f;
            Vector2 endCollision = startCollision + rotVector * swordLength;

            float bullshit = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), startCollision, endCollision, 15, ref bullshit);
        }

        float alpha = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            Vector2 origin = tex.Size() * 0.5f;

            if (!circling)
            {
                Vector2 afterImagePos = Projectile.Center;
                Vector2 rotVector = Projectile.rotation.ToRotationVector2();
                for (int i = 0; i < 3; i++)
                {
                    afterImagePos += rotVector * Main.rand.Next(-30, 0) + Main.rand.NextVector2Unit() * 3;

                    Main.spriteBatch.Draw(
                    tex,
                    afterImagePos - Main.screenPosition,
                    null,
                    lightColor * 0.25f * alpha,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                    );
                }
            }

            Main.spriteBatch.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor * alpha,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
                );

            return false;
        }
    }
}