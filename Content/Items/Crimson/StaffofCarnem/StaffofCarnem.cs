using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Malignant.Content.Items.Crimson.StaffofCarnem
{
    public class StaffofCarnem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Staff of Carnem");
            Tooltip.SetDefault("Pending Re-work");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.width = 62;
            Item.height = 62;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5f;
            Item.value = Item.buyPrice(0, 8, 50, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.channel = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item24;
            Item.shoot = ModContent.ProjectileType<CarnemProj>();
            Item.shootSpeed = 10f;
            Item.mana = 28;
        }

     }
}
