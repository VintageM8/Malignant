using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Malignant.Content.Buffs.Summon;

namespace Malignant.Content.Items.Holy.CrossLord
{
    public class CrossOfOurLord : ModItem
    {

        public override void SetDefaults()
        {
            Item.SetWeaponValues(80, 3, 0);
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;

            Item.width = 58;
            Item.height = 58;

            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 1;
            Item.noMelee = true;

            Item.UseSound = SoundID.Item44;
            Item.autoReuse = true;
            Item.buffType = BuffType<CrossSummonBuff>();
            Item.shoot = ProjectileType<CrossSummon>();

            Item.value = 100000;
            Item.rare = ItemRarityID.Yellow;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ImpStaff)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}