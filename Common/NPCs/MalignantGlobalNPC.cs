using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Common.NPCs;

public class MalignantGlobalNPC : GlobalNPC
{
    // TODO: REWORK
    public int[] Add = new int[50];
    public Vector2[] AddPositions = new Vector2[50];

    // TODO: better name
    public bool gettingSucked;
    public override bool InstancePerEntity => true;

}
