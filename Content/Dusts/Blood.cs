using Terraria;
using Terraria.ModLoader;

namespace Malignant.Content.Dusts
{
    public class Blood : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.4f;
            // dust.noGravity = false;
            dust.noLight = true;
            dust.scale *= 0.5f;
        }
    }
}
