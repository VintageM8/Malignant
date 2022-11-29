using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Dusts
{
    public class HealingDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity = dust.position - Vector2.UnitY * 30;
            dust.frame = new Rectangle(0, 0, 20, 20);
        }

        public override bool Update(Dust dust)
        {
            dust.scale -= 0.01f;
            dust.position = Vector2.Lerp(dust.position, dust.velocity, 0.03f) + Vector2.UnitX * MathF.Sin(dust.scale * MathHelper.TwoPi) * 1.5f;
            dust.alpha = (int)(MathF.Sin(dust.scale * MathHelper.Pi) * 255f);

            
            if (dust.scale < 0.02f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
