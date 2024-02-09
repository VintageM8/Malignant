using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;
using Malignant.Content.Items.Crimson.Arterion.MoniterAccessory;

//DONT CHANGE THIS NAMESPACE
namespace Malignant.Common.Players
{
    public class MalignantPlayer : ModPlayer
    {

        public Vector2[] PreviousVelocity = new Vector2[30];

        //Orbiting
        public int RotationTimer = 0;
        public int[] OrbitingProjectileCount = new int[5];                               //Current upadted count of how many projectiles are active.
        public Vector2[,] OrbitingProjectilePositions = new Vector2[5, 50];             //Used to store the desired positions for the projectiles.
        public Projectile[,] OrbitingProjectile = new Projectile[5, 50];

        //Weapons
        public int itemCombo;
        public int itemComboReset;
        public int lastSelectedItem;
        public int BuildCount = 0;

        //Accessories
        public bool Moniter;
        public bool Lich;
        public bool HolyGauntlet;
        public bool EvilEye;
        public bool WoodenCross;

        //Boss Stuff
        public int bossTextProgress, bossMaxProgress;
        public string bossName, biomeName;
        public string bossTitle, biomeTitle;
        public int bossStyle;
        public Color bossColor;

        public override void ResetEffects()
        {
            Moniter = false;
            Lich = false;
            HolyGauntlet = false;

            if (itemComboReset <= 0)
            {
                itemCombo = 0;
                itemComboReset = 0;
            }
            else
            {
                itemComboReset--;
            }
        }


        public override bool PreItemCheck()
        {
            if (Player.selectedItem != lastSelectedItem)
            {
                itemComboReset = 0;
                itemCombo = 0;
                lastSelectedItem = Player.selectedItem;
            }
            if (itemComboReset > 0)
            {
                itemComboReset--;
                if (itemComboReset == 0)
                {
                    itemCombo = 0;
                }
            }

            return true;
        }

        public override void OnEnterWorld()
        {
            //Important for Orbiting projectiles.
            for (int i = 0; i < OrbitingProjectileCount.Length; i++)
            {
                OrbitingProjectileCount[i] = 0;
            }
        }

        public override void UpdateDead()
        {
            //Important for Orbiting projectiles.
            for (int i = 0; i < OrbitingProjectileCount.Length; i++)
            {
                OrbitingProjectileCount[i] = 0;
            }
        }

        public void GenerateProjectilePositions()
        {
            double period = 2f * Math.PI / 300f;
            for (int i = 0; i < OrbitingProjectileCount[0]; i++)
            {
                //Radius 200.
                OrbitingProjectilePositions[0, i] = Player.Center + new Vector2(200 * (float)Math.Cos(period * (RotationTimer + 300 / OrbitingProjectileCount[0] * i)), 200 * (float)Math.Sin(period * (RotationTimer + 300 / OrbitingProjectileCount[0] * i)));
            }
        }

        //This is where we make our central timer that the orbiting projectile uses.
        public override void PostUpdate()
        {
            if (bossTextProgress > 0)
                bossTextProgress--;
            if (bossTextProgress == 0)
            {
                bossName = null;
                bossTitle = null;
                bossMaxProgress = 0;
                bossStyle = -1;
                bossColor = Color.White;
            }

            bool temp = false;
            for (int i = 0; i < 5; i++)
            {
                if (OrbitingProjectileCount[i] > 0) temp = true;
            }
            if (temp)
            {
                GenerateProjectilePositions();
                RotationTimer++;
            }
            else RotationTimer = 0;
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (Moniter)
            {
                int projectiles = 3;
                if (Main.netMode != NetmodeID.MultiplayerClient && Main.myPlayer == Player.whoAmI)
                {
                    for (int i = 0; i < projectiles; i++)
                    {
                        Projectile.NewProjectile(Player.GetSource_OnHurt(null), Player.Center, new Vector2(7).RotatedBy(MathHelper.ToRadians(360 / projectiles * i + i)), ModContent.ProjectileType<BloodRune>(), 19, 2, Player.whoAmI);
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Lich)
            {
                if (target.CountsAsACritter)
                {
                    int healAmount = (int)MathHelper.Min(Player.statLifeMax2 - Player.statLife, 10);
                    Player.HealEffect(10);
                    Player.statLife += healAmount;
                }
            }
            if (HolyGauntlet)
            {
                if (MalignantLists.unholyEnemies.Contains(target.type) && target.life <= 0)
                {
                    int healAmount = (int)MathHelper.Min(Player.statLifeMax2 - Player.statLife, 10);
                    Player.HealEffect(10);
                    Player.statLife += healAmount;

                }
            }
            if (EvilEye)
            {
                if (MalignantLists.ghostEnemies.Contains(target.type))
                {
                    damageDone = (int)(damageDone * 1.8f);
                }

            }
            if (WoodenCross)
            {
                if (MalignantLists.unholyEnemies.Contains(target.type))
                {
                    target.AddBuff(BuffID.OnFire, 380);
                }
            }

        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Lich)
            {
                if (target.CountsAsACritter)
                {
                    int healAmount = (int)MathHelper.Min(Player.statLifeMax2 - Player.statLife, 10);
                    Player.HealEffect(10);
                    Player.statLife += healAmount;
                }
            }
            if (HolyGauntlet)
            {
                if (MalignantLists.unholyEnemies.Contains(target.type) && target.life <= 0)
                {
                    int healAmount = (int)MathHelper.Min(Player.statLifeMax2 - Player.statLife, 10);
                    Player.HealEffect(10);
                    Player.statLife += healAmount;

                }
            }
            if (EvilEye)
            {
                if (MalignantLists.ghostEnemies.Contains(target.type))
                {
                    damageDone = (int)(damageDone * 1.8f);
                }

            }
            if (WoodenCross)
            {
                if (MalignantLists.unholyEnemies.Contains(target.type))
                {
                    target.AddBuff(BuffID.OnFire, 380);
                }
            }
        }
    }
}
