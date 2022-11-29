using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Common
{
    public class SoundManager
    {
        public static Dictionary<string, SoundStyle> Sounds { get; private set; }

        static readonly string[] loadFolders = new string[]
        {
            "Music",
            "SFX"
        };

        public static void Load(Mod mod)
        {
            Sounds = new Dictionary<string, SoundStyle>();
            foreach (string file in mod.GetFileNames())
            {
                foreach (string folder in loadFolders)
                {
                    string startsWith = $"Assets/{folder}/";

                    if (file.StartsWith(startsWith) && (file.EndsWith(".wav") || file.EndsWith(".ogg") || file.EndsWith(".mp3")))
                    {
                        string path = file.Replace(".xnb", string.Empty);
                        string name = path.Replace(startsWith, string.Empty);

                        Sounds[name] = new SoundStyle(path);
                    }
                }
            }
        }
    }
}
