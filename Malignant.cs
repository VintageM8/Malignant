using Terraria.ModLoader;

namespace Malignant
{
	public class Malignant : Mod
	{
        public static Malignant Mod { get; set; }
        public static ModKeybind UseAbilty;

        public Malignant()
        {
            Mod = this;
        }

        public override void Unload()
        {
            Mod = null;
            UseAbilty = null;
        }

        public override void Load()
        {
            UseAbilty = KeybindLoader.RegisterKeybind(Mod, "Use Abilty", "R");

        }
    }
}