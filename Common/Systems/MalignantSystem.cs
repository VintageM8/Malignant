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
    public void CreateChurch(GenerationProgress progress, GameConfiguration g)
    {
        progress.Message = "Creating Church(Maligant)";
         Point Location = FindChurchLoc();
        //WorldGen.PlaceTile(Location.X, Location.Y, TileID.AmberGemspark);
        //no chests
        StructureLoader.ReadStruct(Location, "Assets/Structures/Church");
        for(int i = 0; i < 121; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                WorldGen.PlaceTile(Location.X + i, Location.Y + j, TileID.Dirt, true, true);
            }
        }
    }
   
    private bool VaildChurchLoc(Point p)
    { 
        for (int i = 0; i < 100; i++)
        {
            for (int j = -5; j < 10; j++)
            {   
                Point n = new Point(i, -j) + p;
                if (j < 0)
                {
                    Tile t = Framing.GetTileSafely(n);
                    if (!(t.WallType == WallID.DirtUnsafe || t.TileType == TileID.Dirt || t.TileType == TileID.Grass)) {
                        return false;
                    }
                }
                else if (j < 5)
                {
                    Tile t = Framing.GetTileSafely(n);
                    if (!(t.WallType == WallID.DirtUnsafe || t.TileType == TileID.Dirt || t.TileType == TileID.Dirt || t.TileType == TileID.Stone) || !(t.HasTile))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!Framing.GetTileSafely(n).HasTile || Framing.GetTileSafely(n).TileType == TileID.Trees)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
        //idk what I was doing here but it should work
    }
    public Point FindChurchLoc()
    {
        int attemptNum = 0;
        while (attemptNum < MaxAttempts)
        {
            attemptNum++;
            int side = 1;//Main.rand.Next(0, 2) == 0 ? 1 : -1;
            Point p = new Point(Main.spawnTileX, (int)Main.worldSurface) + new Point(Main.rand.Next(50, 600) * side, Main.rand.Next(-60, -25));
            if (VaildChurchLoc(p))
            {
                while (Framing.GetTileSafely(p).HasTile)
                {
                    p.Y -= 1;
                }
                return p;
            }
            
         }
        throw new Exception($"Failed to find a Church Location in {MaxAttempts} tries");
    }

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
    int IceAttempts = 0;
    const int MaxAttempts = 5000000;
    private Point FindIceLocation()
    {
        IceAttempts++;
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
        else if (IceAttempts > MaxAttempts)
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
        tasks.Insert(CleanupIndex, new PassLegacy("Church", CreateChurch));
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
