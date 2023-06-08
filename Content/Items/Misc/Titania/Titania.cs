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
                Projectile.NewProjectile(Item.GetSource_OnHit(target), player.Center, Vector2.Zero, ModContent.ProjectileType<HolyRune>(), Item.damage - 13, Item.knockBack / 2, player.whoAmI);
            }
        }
    }

    public class HolyRune : ModProjectile
    {
        bool initilize = true;

        Vector2 spawnPosition;

        public override string Texture => "Malignant/Assets/Textures/Rune"; //Shmircle

        public override bool ShouldUpdatePosition() => false;

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.Size = new Vector2(10f);
            Projectile.scale = 0.01f;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = -1;
            Projectile.timeLeft = 100;
        }

        public override void AI()
        {
            if (initilize)
            {
                spawnPosition = Projectile.Center;

                initilize = false;
            }

            Projectile.Center = spawnPosition;

            Projectile.scale += 0.02f;
            Projectile.Size += new Vector2(10f);
            Projectile.alpha += 10;

            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (MalignantLists.unholyEnemies.Contains(target.type))
            {
                target.AddBuff(BuffID.OnFire, 120);
                target.AddBuff(ModContent.BuffType<SmokeDebuff>(), 120);

            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 position = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(new Color(255, 215, 0, 0));

            Main.EntitySpriteDraw(texture, position, sourceRectangle, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
