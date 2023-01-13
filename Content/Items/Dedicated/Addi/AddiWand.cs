using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using Malignant.Core;

namespace Malignant.Content.Items.Dedicated.Addi
{
    public class AddiWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Addri's Wand");
            Tooltip.SetDefault("[c/39ff14:Dedicated Item:] Dedicated to [c/800080:Addri]\n 'THE GAY TWIZZLERS'!");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 26;
            Item.height = 28;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 99999999999999;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item43; //Filler, till I get some funny audio
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WackAssProjectile>();
            Item.shootSpeed = 5f;
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
