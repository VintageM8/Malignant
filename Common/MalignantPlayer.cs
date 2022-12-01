using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.ID;

namespace Malignant.Common
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

        public override void ResetEffects()
        {

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

        public override void OnEnterWorld(Player player)
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
                OrbitingProjectilePositions[0, i] = Player.Center + new Vector2(200 * (float)Math.Cos(period * (RotationTimer + (300 / OrbitingProjectileCount[0] * i))), 200 * (float)Math.Sin(period * (RotationTimer + (300 / OrbitingProjectileCount[0] * i))));
            }
        }

        //This is where we make our central timer that the orbiting projectile uses.
        public override void PostUpdate()
        {
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
    }
}
