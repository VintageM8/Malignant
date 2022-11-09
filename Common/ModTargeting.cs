using Microsoft.Xna.Framework;
using Terraria;

namespace Malignant.Common; 

public class ModTargeting
{
    public static Vector2 LinearAdvancedTargeting(Vector2 sourcepos, Vector2 targetpos, Vector2 targetvel, float shotspeed, ref float t)
    {
        Vector2 pos = targetpos - sourcepos;
        Vector2 v = targetvel;
        Vector2 w;                              //the output aka the velocity of the proj

        if (shotspeed < targetvel.Length())
        {
            v = Vector2.Zero;
        }
            
        t = (float)System.Math.Sqrt(System.Math.Pow(shotspeed, 2) * (System.Math.Pow(pos.Length(), 2)) - System.Math.Pow(v.Y * pos.X - v.X * pos.Y, 2)) + v.Y * pos.Y + v.X * pos.X;
        t =  t / (float)(System.Math.Pow(shotspeed, 2) - System.Math.Pow(v.Length(), 2));
        w = pos / t + v;
        return w;
    }
    public static Vector2 LinearAdvancedTargeting(Vector2 sourcepos, Vector2 targetpos, Vector2 targetvel, float shotspeed)
    {
        Vector2 pos = targetpos - sourcepos;
        Vector2 v = targetvel;
        Vector2 w;                              //the output aka the velocity of the proj
        float t;

        if (shotspeed < targetvel.Length())
        {
            v = Vector2.Zero;
        }

        t = (float)System.Math.Sqrt(System.Math.Pow(shotspeed, 2) * (System.Math.Pow(pos.Length(), 2)) - System.Math.Pow(v.Y * pos.X - v.X * pos.Y, 2)) + v.Y * pos.Y + v.X * pos.X;
        t = t / (float)(System.Math.Pow(shotspeed, 2) - System.Math.Pow(v.Length(), 2));
        w = pos / t + v;
        return w;
    }
        
    public static Vector2 TargetPosition(Vector2 targetPos, Vector2 npcPos, float projVel)
    {
        Vector2 temp = targetPos - npcPos;
        temp.Normalize();
        temp *= projVel;
        return temp;
    }
        
    public static void FallingTargeting(NPC npc, Player target, Vector2 Yoffset, int ShotSpeed , ref float delay, ref Vector2 projectileVel)
    {
        int temp = 1;
        if (target.velocity.Y > 0) //only check when falling
        {
            while (temp <= delay)   //simulate movement and get the intersectionpoint of the player block collision
            {
                Vector2 predPos = target.Center + target.velocity * temp; //movement equation constant velocity
                Point tileLoc = predPos.ToTileCoordinates();    
                Tile tile = Framing.GetTileSafely(tileLoc.X, tileLoc.Y);
                if (Main.tileSolidTop[tile.TileType] || (tile.HasTile && Main.tileSolid[tile.TileType]))
                {
                    break;
                }
                temp++;
            }
            if (temp < delay)   //if collision happened set new speed
            {
                Vector2 newPos = target.Center + Yoffset + target.velocity * temp;
                delay = (newPos - npc.position).Length() / ShotSpeed;
                projectileVel = ModTargeting.TargetPosition(newPos, npc.Center, ShotSpeed);
            }
        }
    }
}