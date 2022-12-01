using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Malignant.Core;

namespace Malignant.Common;

public class MalignantSystem : ModSystem
{
    //bosses
    public static bool downedViking;
    public static bool downedIceBoss;

    public void CreateIceCastle(GenerationProgress progress, GameConfiguration g)
    {
        progress.Message = "Creating Ice Structure (Malignant)";
        Point Location = FindIceLocation();
        int[] Chests = StructureLoader.ReadStruct(Location, "Assets/Structures/IceCastle");
        foreach (int i in Chests)
        {
            //add with this
            //Main.chest[i].AddItem(ModContent.ItemType<CursedFireballStaff>(), 1);
            //this will only run once since theirs 1 chest
        }
    }
    int attempts = 0;
    const int MaxAttempts = 500000;
    private Point FindIceLocation()
    {
        attempts++;
        Point p = new Point();
        p.X = Main.rand.Next(0, Main.maxTilesX);
        p.Y = Main.rand.Next((int)WorldGen.rockLayerLow, (int)WorldGen.rockLayerHigh);
        bool VaildLoc = false;
        Tile t = Framing.GetTileSafely(p);
        if (t.TileType == TileID.IceBlock || t.TileType == TileID.SnowBlock)
        {
            VaildLoc = true;
        }
        if (VaildLoc)
        {
            return p;
        }
        else if (attempts > MaxAttempts)
        {
            Mod.Logger.Warn("cannot find a snow index");
            throw new Exception($"cannot find snow with {MaxAttempts} attempts");
        }
        else
        {
            return FindIceLocation();
        }
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
    {
        int CleanupIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
        tasks.Insert(CleanupIndex, new PassLegacy("IceCastle", CreateIceCastle));
    }

    internal static bool ActiveAndSolid(int x, int y)
    {
        return Framing.GetTileSafely(x, y).HasTile && Main.tileSolid[Main.tile[x, y].TileType] && !Main.tileCut[Main.tile[x, y].TileType];
    }

    internal static bool check2x2(int x, int y)
    {
        return
            ActiveAndSolid(x, y + 1) &&
            ActiveAndSolid(x + 1, y + 1);
    }

    internal static bool check2x2_Liquid(int x, int y)
    {
        return
            ActiveAndSolid(x, y + 1) &&
            ActiveAndSolid(x + 1, y + 1);
    }
}
