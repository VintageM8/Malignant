using System;
using System.Collections.Generic;
using Malignant.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Malignant.Content.Items.Dedicated.P3XY7
{
    public class P3Guitar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("P3's Guitar");
        }
        public override void SetDefaults()
        {

            Item.crit = 12;
            Item.damage = 45;
            Item.DamageType = DamageClass.Magic;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Guitar;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(0, 25, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<MusicNote1>();
            Item.shootSpeed = 20f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            List<int> notes = new List<int>();
            notes.Add(ModContent.ProjectileType<MusicNote1>());
            notes.Add(ModContent.ProjectileType<MusicNote2>());
            notes.Add(ModContent.ProjectileType<MusicNote3>());
            //float x = (float)Math.Cos(new Random().NextDouble() * 6.283185307179587f) * (float)new Random().NextDouble() * 8;
            //float y = (float)Math.Sin(new Random().NextDouble() * 6.283185307179587f) * (float)new Random().NextDouble() * 8;
            //int val = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, x, y, notes[new Random().Next(3)], Item.damage, 0f, Main.myPlayer, 0f, 0f);
            float numberProjectiles = 6; // 3 shots
            float rotation = MathHelper.ToRadians(180);//Shoots them in a 45 degree radius. (This is technically 90 degrees because it's 45 degrees up from your cursor and 45 degrees down)
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Vector for spread. Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(source, position, perturbedSpeed, notes[new Random().Next(3)], damage, knockback, player.whoAmI); //Creates a new projectile with our new vector for spread.
            }
            SoundEngine.PlaySound(SoundID.Item47 with { Volume = 1f, PitchVariance = 1 });

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(position, DustID.GemRuby, Main.rand.NextVector2Circular(1f, 1f) * 20, 0, default, 1f);
                dust.noGravity = true;
            }
            return false; 
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