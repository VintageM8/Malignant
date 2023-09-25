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
using Mono.Cecil;
using System.Linq;
using Malignant.Content.Items.Misc.Titania;
using static Terraria.ModLoader.ModContent;
using Malignant.Content.Items.Misc;
using Malignant.Content.Items.Consumeable.Summons;

namespace Malignant.Common.Systems;

public class MalignantSystem : ModSystem
{
    //bosses
    public static bool downedViking;
    public static bool downedIceBoss;
    static int[] replaceTiles =
    {
        TileID.Trees, TileID.Grass
    };
    
    public static void CreateChurch(GenerationProgress progress, GameConfiguration g)
    {
        if(progress != null)
        {
            progress.Message = "Creating Church(Maligant)";
        }
        Point Location = FindChurchLoc(progress, out int groundType); 
        //no chests
        Chest c = Main.chest[StructureLoader.ReadStruct(Location, "Assets/Structures/Church" , progress)[0]];
        c.AddItem(ItemType<Titania>(), 1);
        c.AddItem(ItemType<FruitOfTheGarden>(), 1);
        for (int i = -5; i < CHURCH_X_LENGTH + 5; i++) {
            for(int j = 0; j < MAX_H_DIF; j++)
            {
                Tile t =Framing.GetTileSafely(Location.X + i, Location.Y + j);
                if(!t.HasTile || replaceTiles.Contains(t.TileType) || !Main.tileSolid[t.TileType])
                {
                    t.ClearTile();
                    WorldGen.PlaceTile(Location.X + i, Location.Y + j, groundType, true, true);
                }
                t.Slope = SlopeType.Solid;
            }
        }
        /*for (int i = -3; i < 135; i++)    
        {
            for (int j = 0; j < Math.Min(Move, 20); j++)
            {
                WorldGen.PlaceTile(Location.X + i, Location.Y + j, TileID.Dirt, true, true);
                Framing.GetTileSafely(Location.X + i, Location.Y + j).Slope = 0;
            }
        }*/
    }
    const int MAX_H_DIF = 15;
    const int CHURCH_X_LENGTH = 65;//there should be a better way of doing this, but it would need changing how the loader works
    
    public static bool VaildChurchLoc(GenerationProgress progress, Point p, out int gtype)
    {
        progress.Value = 0;

        Dictionary<int, int> mostCommon = new Dictionary<int, int>();
        
        gtype = 0;
        for(int i = -5; i < CHURCH_X_LENGTH + 5; i++)
        {
            progress.Value = ((float)i / (CHURCH_X_LENGTH + 5));
            var surfacearea = goToSurface(p.X + i);
            if (surfacearea == null)
            {
                return false;
            }
            var surfacepoint = surfacearea.Value;
            if(Math.Abs(surfacepoint.Y - p.Y) > MAX_H_DIF)
            {
                return false;
            }
            //up to 10 down to check for evil blocks
            for(int j = 0; j < 10; j++)
            {
                Tile check = Framing.GetTileSafely(surfacepoint.X, surfacepoint.Y + j);
                if (check.HasTile)
                {
                    if (mostCommon.ContainsKey(check.TileType))
                    {
                        mostCommon[check.TileType] += 1;
                    }
                    else
                    {
                        mostCommon.Add(check.TileType, 1);
                    }
                    if (EvilBlock(check))
                    {
                        return false;
                    }
                }
            }
        }
        int max = 0;
        foreach(KeyValuePair<int, int> pair in mostCommon)
        {
            if(pair.Value > max)
            {
                max = pair.Value;
                gtype = pair.Key;
            }
        }
        return true;
       /* for (int i = 0; i < 100; i++)
        {
            for (int j = -5; j < 10; j++)
            {
                Point n = new Point(i, -j) + p;
                if (j < 0)
                {
                    Tile t = Framing.GetTileSafely(n);
                    if (!(t.WallType == WallID.DirtUnsafe || t.TileType == TileID.Dirt || t.TileType == TileID.Grass))
                    {
                        return false;
                    }
                }
                else if (j < 5)
                {
                    Tile t = Framing.GetTileSafely(n);
                    if (!(t.WallType == WallID.DirtUnsafe || t.TileType == TileID.Dirt || t.TileType == TileID.Dirt || t.TileType == TileID.Stone) || !t.HasTile)
                    {
                        return false;
                    }
                }
                else
                {
                    if ((!Framing.GetTileSafely(n).HasTile || Framing.GetTileSafely(n).TileType == TileID.Trees) && Framing.GetTileSafely(n).WallType != WallID.DirtUnsafe)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
        //idk what I was doing here but it should work
        //it does not*/
    }
    static int[] evilBlocks =
    {
        TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.Crimsand, TileID.CrimsonGrass, TileID.CrimsonJungleGrass, TileID.Crimstone, TileID.Ebonsand, TileID.Ebonstone, TileID.CorruptGrass, TileID.CorruptIce, TileID.FleshIce
    };
    private static bool EvilBlock(Tile check)
    {
        foreach(int id in evilBlocks)
        {
            if(check.TileType  == id)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// returns if a tile is on the top surface of the wo0rld
    /// Current only works for forest, for use in goToSurface
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static bool onSurface(Tile t, Tile below)
    {
        return ((t.WallType == WallID.None && !t.HasTile) || (t.TileType == TileID.Trees )) && (below.WallType == WallID.DirtUnsafe || below.HasTile);
    }
    /// <summary>
    /// Returns a point on the surface with the given x value
    /// A point on the surface is one above the top dirt layer connected to where the player spawns
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Point? goToSurface(int x)
    {
        var y = Main.worldSurface;
        Tile t = Framing.GetTileSafely(x, (int)y);
        Tile below = Framing.GetTileSafely(x, (int)y + 1);
        while (!onSurface(t, below))
        {
            below = Framing.GetTileSafely(x, (int)y);
            y--;
            t = Framing.GetTileSafely(x, (int)y);
            if(y == 0)
            {
                return null;
            }
        }
        return new Point(x, (int)y);
    }

    public static Point FindChurchLoc(GenerationProgress progress, out int groundType)
    {
        int wsize = Main.maxTilesX / 2;//dist to edge from center
        //we want to be the 1/3 between either edge
        int minscale, maxscale;
        minscale = wsize / 3;
        maxscale = wsize * 2 / 3;
        int attemptNum = 0;
        while (attemptNum < MaxAttempts)
        {
            attemptNum++;
            int side = Main.rand.Next(0, 2) == 0 ? 0:wsize ;
            int x = Main.rand.Next(minscale, maxscale) + side;
            Point? pn = goToSurface(x);
            if (pn == null)
            {
                continue;
            }
            Point p = pn.Value;
            if (VaildChurchLoc(progress, p,  out int gtype))
            {
                groundType = gtype;
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
            Main.chest[i].AddItem(ModContent.ItemType<FrostedBeacon>(), 1);
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
        p.Y = Main.rand.Next((int)GenVars.rockLayerLow, (int)GenVars.rockLayerHigh);
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

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
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
