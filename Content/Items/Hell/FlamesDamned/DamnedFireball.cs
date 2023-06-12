using Malignant.Common.Projectiles.Orbiting;
using Terraria.GameContent;
using Malignant.Content.Items.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common.Systems;
using Malignant.Common.Players;
using Malignant.Content.Dusts;
using Malignant.Common.Helper;

namespace Malignant.Content.Items.Hell.FlamesDamned
{
    public class DamnedFireball_2 : ModProjectile
    {
        bool spawnStuff = true;
        Vector2 initialCenter;
        public float timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override string Texture => "Malignant/Content/Items/Hell/FlamesDamned/Fireball";
        public override void AI()
        {
            if (spawnStuff)
            {
                initialCenter = Projectile.Center;
                spawnStuff = false;
            }

            if (Main.rand.NextBool(10))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Smoke>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

            Player player = Main.player[Projectile.owner];
            Vector2 distanceVector = Main.MouseWorld - player.Center;
            if (distanceVector != Vector2.Zero)
            {
                distanceVector.Normalize();
            }
            Projectile.Center = player.Center + distanceVector * 60;
            float dir = distanceVector.X / Math.Abs(distanceVector.X);
            player.ChangeDir((int)dir); // Set player direction to where we are shooting
            player.heldProj = Projectile.whoAmI; // Update player's held projectile
            player.itemTime = 2; // Set item time to 2 frames while we are used
            player.itemAnimation = 2; // Set item animation time to 2 frames while we are used
            player.itemRotation = (float)Math.Atan2(distanceVector.Y * dir, distanceVector.X * dir); // Set the item rotation to where we are shooting
            if (player.channel)
            {
                Projectile.timeLeft++;
                MalignantPlayer modplayer = player.GetModPlayer<MalignantPlayer>();
                if (timer % 30 == 10 && modplayer.OrbitingProjectileCount[2] <= 5)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<DamnedFireball>(), 30, 1, player.whoAmI, 0, 0);
                }
                timer++;
            }
            else
            {
                Projectile.Kill();
            }

        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            MalignantPlayer modplayer = player.GetModPlayer<MalignantPlayer>();

            timer = 0;
            for (int i = 0; i < modplayer.OrbitingProjectileCount[2]; i++)
            {
                modplayer.OrbitingProjectile[2, i].localAI[1] = 4;
            }
        }
    }

    public class DamnedFireball : OrbitingProjectile
    {
        //private float CircleArr = 1;
        //private int PosCheck = 0;
        //private int PosPlay = 0;

        //private int OrignalDamage = 0;
        //private int NumProj = 0;

        //private bool charge = false;
        public override string Texture => "Malignant/Content/Items/Hell/FlamesDamned/Fireball";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            ProjectileSlot = 2;
            OrbitingRadius = 80;
            Period = 180;
            PeriodFast = 35;
            ProjectileSpeed = 14;
            Snappingdistance = 18;
            SpawnStyle = 1;
        }
        public override void Attack()
        {
            MalignantPlayer modplayer = player.GetModPlayer<MalignantPlayer>();
            Vector2 ProjectileVelocity = (Projectile.Center - player.Center) / 3 + Main.MouseWorld - Projectile.Center;
            Projectile.penetrate = 3;
            if (ProjectileVelocity != Vector2.Zero)
            {
                ProjectileVelocity.Normalize();
            }
            ProjectileVelocity *= 22;
            Projectile.velocity = ProjectileVelocity;
            Proj_State = 5;
            GeneratePositionsAfterKill();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            MalignantPlayer modplayer = player.GetModPlayer<MalignantPlayer>();

            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.t_Flesh,
                              -Projectile.velocity.X * 0.3f, -Projectile.velocity.Y * 0.3f, Scale: 2);

            }

        }
        bool spawnStuff;
        public override void AI()
        {
            if (spawnStuff)
            {
                spawnStuff = false;
            }

            if (Main.rand.NextBool(10))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<Smoke>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);


            player = Main.player[Projectile.owner];
            OrbitCenter = player.Center;
            RelativeVelocity = player.velocity;
            MalignantPlayer modplayer = player.GetModPlayer<MalignantPlayer>();
            Vector2 pointingDirection = (Projectile.Center - player.Center) / 3 + Main.MouseWorld - Projectile.Center;

            if (Proj_State == State_Moving || Proj_State == State_Spawning || Proj_State == State_Initializing)
            {
                Projectile.tileCollide = false;
                Projectile.timeLeft += 1;
                Projectile.rotation = pointingDirection.ToRotation();
                Projectile.damage = 30 * modplayer.OrbitingProjectileCount[ProjectileSlot];
            }
            base.AI();

        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                float distance = Vector2.Distance(Projectile.Center, Main.player[i].Center);
                if (distance <= 1050)
                {
                    CameraSystem.ScreenShakeAmount = 3;
                }
            }
        }

        public Trail trail;
        public Trail trail2;
        private bool initialized;

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
