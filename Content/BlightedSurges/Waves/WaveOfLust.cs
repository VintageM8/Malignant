using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Malignant.Content.BlightedSurges;
using Malignant.Content.NPCs.BlightedSurges;
using MonoMod.Utils;

namespace Malignant.Content.BlightedSurges.Waves
{
    internal class WaveOfLust : SurgeWave
    {
        //public override Color WaveColor => Color.Cyan;

        public override string WaveName => "Lust";

        public override void InitializeWave(Player player)
        {
            SpawnEnemy(ModContent.NPCType<Succy>(), player.Center + new Vector2(1500, 0));
            SpawnEnemy(ModContent.NPCType<Succy>(), player.Center + new Vector2(-1500, 0));
            SpawnEnemy(ModContent.NPCType<Succy>(), player.Center + new Vector2(0, -1500));

            //requiredKills.Add(ModContent.NPCType<Samyaza>());
        }
    }
}
