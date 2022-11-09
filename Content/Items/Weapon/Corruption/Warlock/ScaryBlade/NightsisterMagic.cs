using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Malignant.Common;
using Malignant.Common.Projectiles.Orbiting;

namespace Malignant.Content.Items.Weapon.Corruption.Warlock.ScaryBlade
{

    public class NightsisterMagicMain : ModProjectile
    {
        //private float CircleArr = 1;
        //private int PosCheck = 0;
        //private int PosPlay = 0;

        //private int OrignalDamage = 0;
        //private int NumProj = 0;

        //private bool charge = false;
        public float timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Blade");
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
        public override string Texture => "Malignant/Content/Items/Weapon/Corruption/Warlock/ScaryBlade/NightsisterBlade";
        public override void AI()
        {
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
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<NightsisterMagic>(), 30, 1, player.whoAmI, 0, 0);
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

    public class NightsisterMagic : OrbitingProjectile
    {
        //private float CircleArr = 1;
        //private int PosCheck = 0;
        //private int PosPlay = 0;

        //private int OrignalDamage = 0;
        //private int NumProj = 0;

        //private bool charge = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Blade");
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
            OrbitingRadius = 150;
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
        /*public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Trinitarian/Content/Items/Weapons/Hardmode/Melee/NightsisterBlade");
            if (Proj_State == State_Moving || Proj_State == State_Spawning)
            {
                spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 91, 18), lightColor, Projectile.rotation, new Vector2(91 * 0.5f, 9), 1, 0, 0);
            } 
            else
            {
                spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 91, 18), lightColor, Projectile.rotation, new Vector2((91 - Projectile.width * 0.5f), 9), 1, 0, 0);
            }
            return false;
        }*/
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            MalignantPlayer modplayer = player.GetModPlayer<MalignantPlayer>();
        }
        public override void AI()
        {
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

        }
    }
}