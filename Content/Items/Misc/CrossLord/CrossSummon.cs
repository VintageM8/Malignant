using Malignant.Common.Helper;
using Malignant.Content.Items.Hell.MarsHell;
using Malignant.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Malignant.Content.Buffs.Summon;

namespace Malignant.Content.Items.Misc.CrossLord
{
    public class CrossSummon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            DrawOffsetX = -4;
            DrawOriginOffsetY = 0;
            DrawOriginOffsetX = 0;

            Projectile.penetrate = -1;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active)
            {
                Projectile.active = false;
                return;
            }
            if (player.dead)
            {
                player.ClearBuff(BuffType<CrossSummonBuff>());
            }
            if (player.HasBuff(BuffType<CrossSummonBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            //ai[0] corresponds to the timer
            //ai[1] corresponds to the target
            //localAI[0] corresponds to which 'side' of the enemy we're on

            //set target
            int targetID = -1;
            Projectile.Minion_FindTargetInRange(750, ref targetID, false);
            Projectile.ai[1] = targetID;

            int index = 0;
            int ownedProjectiles = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == Projectile.type && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].ai[1] == Projectile.ai[1])
                {
                    ownedProjectiles++;
                    if (i < Projectile.whoAmI)
                    {
                        index++;
                    }
                }
            }

            float advancePerAttack = 4;

            Vector2 goalPosition = player.Center;
            Vector2 goalVelocity = player.velocity;
            bool doAttacks = false;
            float goalAngle = MethodHelper.timer * (MathHelper.TwoPi / 20f / advancePerAttack) + index * MathHelper.TwoPi / ownedProjectiles;

            if (Projectile.ai[1] != -1)
            {
                goalPosition = Main.npc[(int)Projectile.ai[1]].Center;
                goalVelocity = Main.npc[(int)Projectile.ai[1]].velocity;
                doAttacks = true;
                goalAngle = MethodHelper.timer * (MathHelper.TwoPi / 20f / advancePerAttack) + index * MathHelper.TwoPi / advancePerAttack / ownedProjectiles + MathHelper.TwoPi / advancePerAttack * Projectile.localAI[0];
            }

            //motion code
            if (doAttacks)
            {
                if (Projectile.ai[0] < 0)
                    Projectile.ai[0]++;

                if (Projectile.ai[0] == 0 && Main.rand.NextBool(60))
                {
                    //do attack
                    Projectile.localAI[0] = (Projectile.localAI[0] + 1) % advancePerAttack;

                    Projectile.ai[0] = -20;
                    Projectile.velocity = goalVelocity + (goalPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * 16;
                }
            }
            else
            {
                Projectile.ai[0] = 0;
                Projectile.localAI[0] = Main.rand.Next((int)advancePerAttack);
            }

            if (Projectile.ai[0] >= 0)
            {
                goalPosition += new Vector2(160, 0).RotatedBy(goalAngle);

                goalVelocity += (goalPosition - Projectile.Center) / 8;
                Projectile.velocity += (goalVelocity - Projectile.velocity) / 8;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if ((Projectile.Center - player.Center).Length() > 2000)
            {
                Projectile.position = player.Center + (Projectile.position - Projectile.Center);
            }

            Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3());
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return Projectile.ai[0] < 0;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);

            Projectile.ai[1] = 0;

            target.AddBuff(BuffID.OnFire, 240);

            target.immune[Projectile.owner] = 0;

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ProjectileType<MarsHellBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.Pink) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
