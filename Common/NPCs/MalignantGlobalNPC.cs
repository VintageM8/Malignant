using Malignant.Content.Items.Misc;
using Malignant.Content.Items.Spider.StaffSpiderEye;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Common.NPCs;

public class MalignantGlobalNPC : GlobalNPC
{
    // TODO: REWORK
    public int[] Add = new int[50];
    public Vector2[] AddPositions = new Vector2[50];

    // TODO: better name
    public bool gettingSucked; //Is this being used at all lol?
    public override bool InstancePerEntity => true;

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.WallCreeper || npc.type == NPCID.WallCreeperWall)
        {
            if (Main.rand.NextFloat() < .13f)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StaffofSpiderEye>(), 1));
            }
        }

        if (npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon)
        {
            if (Main.rand.NextFloat() < .45f)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrokenDemonHorn>(), 2));
            }
        }

    }
}
