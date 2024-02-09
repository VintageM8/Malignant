using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Malignant.Content.BlightedSurges.Waves;
using System;

namespace  Malignant.Content.BlightedSurges
{
    class BlightedSurge : ModSystem
    {
        public static bool SurgeActive = false;
        public static Color DreadwindTargetColor;
        public static Color CurrentDreadwindColor;

        public override void OnWorldLoad()
        {
            SurgeActive = false;
        }

        public static List<SurgeWave> upcomingWaves = new();
        public static List<int> extraEnemies = new();
        public static SurgeWave currentWave = null;
        public static int waveNumber = 0;
        public static void StartDreadwind()
        {
            waveNumber = 0;
            upcomingWaves.Clear();
            extraEnemies.Clear();
            upcomingWaves.Add(new WaveOfLust());
            //upcomingWaves.Add(new WaveOfNight());
            //upcomingWaves.Add(new WaveOfFlight());
            //upcomingWaves.Add(new WaveOfMight());
            //upcomingWaves.Add(new WaveOfSight());
            //upcomingWaves.Add(new WaveOfFright());
            //upcomingWaves.Add(new WaveOfPlight());
            SurgeActive = true;
            StartNextWave();
            //CurrentDreadwindColor = Color.Black;

        }

        public const int DreadwindLargeDamage = 200;
        public const int DreadwindMidDamage = 150;
        public const int DreadwindLowDamage = 100;

        public static void StartNextWave()
        {
            if (upcomingWaves.Count == 0)
            {
                FinishSurge(true);
                return;
            }
            waveNumber++;
            currentWave = upcomingWaves[0];
            upcomingWaves.RemoveAt(0);
            Main.NewText("Wave " + waveNumber + ": " + currentWave, currentWave.WaveColor);
            DreadwindTargetColor = currentWave.WaveColor;
            currentWave.InitializeWave(Main.LocalPlayer);
        }


        public static void FinishSurge(bool victory)
        {
            SurgeActive = false;
            Main.NewText("The Temptations diminish...", Color.LightGreen);
        }

        int endTimer = 0;
        int nextWaveTimer = 0;
        public static float HesperusTelegraphRotation = 0f;
        public override void PostUpdateEverything()
        {
            if (!SurgeActive)
            {
                return;
            }

            Player player = Main.LocalPlayer;

            HesperusTelegraphRotation += MathHelper.ToRadians(1f);
            HesperusTelegraphRotation = MathHelper.WrapAngle(HesperusTelegraphRotation);

            CurrentDreadwindColor = Color.Lerp(CurrentDreadwindColor, DreadwindTargetColor, 0.1f);

            if (!player.ZoneUnderworldHeight || player.dead)
            {
                endTimer++;
                if (endTimer > 60 * 2)
                {
                    FinishSurge(false);
                }
            }
            else
            {
                endTimer = 0;
            }

            for (int i = 0; i < currentWave.requiredKills.Count; i++)
            {
                if (i >= currentWave.requiredKills.Count)
                {
                    break;
                }
                if (!NPC.AnyNPCs(currentWave.requiredKills[i]))
                {
                    currentWave.requiredKills.RemoveAt(i);
                }
            }

            if (currentWave.requiredKills.Count <= 0)
            {
                nextWaveTimer++;
                if (nextWaveTimer >= 60 && !player.dead)
                {
                    StartNextWave();
                    nextWaveTimer = 0;
                }
            }
        }
    }

    class SurgeWave
    {
        public virtual Color WaveColor => Color.White;

        public virtual string WaveName => "Default Name";

        public override string ToString()
        {
            return WaveName;
        }

        public virtual void InitializeWave(Player player)
        {

        }

        public List<int> requiredKills = new List<int>();
        public int SpawnEnemy(int type, Vector2 position)
        {
            if (!requiredKills.Contains(type))
            {
                requiredKills.Add(type);
            }
            return NPC.NewNPC(new EntitySource_Misc("BlightedSurge"), (int)position.X, (int)position.Y, type);
        }
    }
}