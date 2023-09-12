using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;

namespace Malignant.Content.NPCs.Crimson.Arterion
{
    internal partial class Arterion
    {
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D mainTexture = TextureAssets.Npc[Type].Value;
            Main.spriteBatch.Draw(
                mainTexture,
                NPC.Center - screenPos + Vector2.UnitY * 10f,
                null,
                drawColor,
                NPC.rotation,
                mainTexture.Size() * 0.5f,
                NPC.scale,
                NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f
            );

            return false;
        }
    }
}
