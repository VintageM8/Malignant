using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Malignant.Common;
using Malignant.Content.Projectiles;
using System;

namespace Malignant.Content.Items.Misc.Titania
{
    public class Titania : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 4;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (MalignantLists.unholyEnemies.Contains(target.type))
            {
                //SoundEngine.PlaySound(SoundID.Item, (int)target.position.X, (int)target.position.Y, 14);
                Projectile.NewProjectile(Item.GetSource_OnHit(target), target.Center, Main.rand.NextVector2Circular(7, 7), ModContent.ProjectileType<Explosion>(), Item.damage, Item.knockBack, player.whoAmI);
            }
        }
    }
}
