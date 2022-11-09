using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using Malignant.Core;

namespace Malignant.Content.Items.Dedicated.Blade
{
    public class BorgorGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Burger Gun");
            Tooltip.SetDefault("[c/eeff00f:Dedicated Item:] Dedicated to [c/ffa242:BladeBurger]\n'Exo mechs was worth it tho lmao that boss is actually fun unlike DoG'.");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item11;
            Item.width = 24;
            Item.height = 28;
            Item.damage = 86;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ModContent.ProjectileType<Borgor>();
            Item.shootSpeed = 11;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Vector2 pos = Utility.GetInventoryPosition(position, frame, origin, scale);
            Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
            Texture2D flash = ModContent.Request<Texture2D>("Malignant/Assets/Textures/Flash2").Value;
            UnifiedRandom rand = new UnifiedRandom(Item.whoAmI);
            for (int i = 0; i < 5; i++)
            {
                float alpha = 0.3f + (0.15f * i);
                float r = rand.NextFloat();
                float s = 0.6f - (0.06f * i);
                spriteBatch.Draw(
                    flash,
                    pos,
                    null,
                    Color.LightYellow * alpha,
                    Main.GlobalTimeWrappedHourly * r,
                    new Vector2(flash.Width / 2, flash.Height),
                    new Vector2(0.2f, s),
                    SpriteEffects.None,
                    0f);
            }
            spriteBatch.Draw(texture, pos, null, Color.White, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
