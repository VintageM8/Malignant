using Microsoft.Xna.Framework;
using Malignant.Content.NPCs.Norse.Njor;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Malignant.Common.Systems;

namespace Malignant.Tiles
{
    public class CocytusAlter : ModTile
    {
        public override string Texture => "Terraria/Images/Item_0";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileLighted[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.AnchorBottom = new AnchorData((AnchorType)0b_11111111, 3, 0); //Any anchor is valid
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.addTile(Type);

            TileID.Sets.DisableSmartCursor[Type] = true;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged) => MalignantSystem.downedIceBoss;
        public override bool CanExplode(int i, int j) => false;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            if (tile.TileFrameY <= 18 && (tile.TileFrameX <= 36 || tile.TileFrameX >= 72))
            {
                r = 0.301f * 1.5f;
                g = 0.110f * 1.5f;
                b = 0.126f * 1.5f;
            }
        }
        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.cursorItemIconEnabled = true; //Show text when hovering over this tile
            Main.LocalPlayer.cursorItemIconID = -1;// mod.ItemType("VinewrathBox");

            if (NPC.AnyNPCs(ModContent.NPCType<Njor>()))
                Main.LocalPlayer.cursorItemIconText = "";
            else
                Main.LocalPlayer.cursorItemIconText = "The tundra rumbles...";
        }

        public override bool RightClick(int i, int j)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Njor>())) //Do nothing if the boss is alive
                return false;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Main.NewText("Cocytus has awoken!", 175, 75, 255);
                int npcID = NPC.NewNPC(new EntitySource_TileBreak(i, j), i * 16, j * 16 - 600, ModContent.NPCType<Njor>());
                Main.npc[npcID].netUpdate2 = true;
            }
            else
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                    return false;

                //MaligMulti.SpawnBossFromClient((byte)Main.LocalPlayer.whoAmI, ModContent.NPCType<Njor>(), i * 16, (j * 16) - 600); WIP Multiplayer stuff
            }
            return true;
        }
    }
}
