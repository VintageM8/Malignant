using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Malignant.Common.Helper;
using static Malignant.Common.CustomSwingStyle;

namespace Malignant.Common
{
    internal class CustomSwingStyle : GlobalItem
    {
        public class CustomUsestyleID
        {
            public const int SwingVersionTwo = 16;
        }
        const float PLAYERARMLENGTH = 11f;
        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            //this remain untouch cause idk what in the hell should i change here
            if (item.useStyle == CustomUsestyleID.SwingVersionTwo)
            {
                //Get the direction of the weapon, and the distance from the player to the hilt
                Vector2 handPos = Vector2.UnitY.RotatedBy(player.compositeFrontArm.rotation);

                //Use afforementioned direction, and get the distance from the player to the tip of the weapon
                float length = new Vector2(item.width, item.height).Length() * player.GetAdjustedItemScale(item);
                Vector2 endPos = handPos;

                //Use values obtained above to construct an approximation of the two most important points
                handPos *= PLAYERARMLENGTH;
                endPos *= length;
                handPos += player.MountedCenter;
                endPos += player.MountedCenter;

                //Use helper method to get coordinates and size for the rectangle
                (int X1, int X2) XVals = Order(handPos.X, endPos.X);
                (int Y1, int Y2) YVals = Order(handPos.Y, endPos.Y);

                //Create the new bounds of the hitbox
                hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
                player.GetModPlayer<MeleeOverhaulPlayer>().SwordHitBox = hitbox;
                int proj = Projectile.NewProjectile(item.GetSource_ItemUse(item), player.itemLocation, player.GetModPlayer<MeleeOverhaulPlayer>().data, ModContent.ProjectileType<GhostHitBox>(), player.GetWeaponDamage(item), player.HeldItem.knockBack, player.whoAmI);
                Main.projectile[proj].Hitbox = hitbox;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.useStyle != CustomUsestyleID.SwingVersionTwo)
            {
                return base.CanUseItem(item, player);
            }
            return player.GetModPlayer<MeleeOverhaulPlayer>().delaytimer <= 0;
        }
        public override float UseSpeedMultiplier(Item item, Player player)
        {
            return 1;
        }
        public override void UseStyle(Item Item, Player player, Rectangle heldItemFrame)
        {
            if (Item.useStyle == CustomUsestyleID.SwingVersionTwo)
            {
                MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                Item.noUseGraphic = false;
                //if (modPlayer.count == 2)
                //{
                //    CircleSwingAttack(player, modPlayer);
                //    return;
                //}
                SwingVersionTwoAttack(player, Item, modPlayer);
            }
        }
        private static (int, int) Order(float v1, float v2) => v1 < v2 ? ((int)v1, (int)v2) : ((int)v2, (int)v1);
        public override void ModifyHitNPC(Item Item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (Item.useStyle == CustomUsestyleID.SwingVersionTwo)
            {
                MeleeOverhaulPlayer modPlayer = player.GetModPlayer<MeleeOverhaulPlayer>();
                float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
                float mult = MathHelper.Lerp(.85f, 1.2f, percentDone);
                damage = (int)(damage * mult);
                knockBack *= mult;
                int proj = Projectile.NewProjectile(Item.GetSource_ItemUse(Item), player.itemLocation, Vector2.Zero, ModContent.ProjectileType<GhostHitBox>(), damage, knockBack, player.whoAmI);
                Main.projectile[proj].Hitbox = modPlayer.SwordHitBox;
            }
            base.ModifyHitNPC(Item, player, target, ref damage, ref knockBack, ref crit);
        }
        private void SwingVersionTwoAttack(Player player,Item item, MeleeOverhaulPlayer modPlayer)
        {
            int VerticleDirectionSwingVersionTwo = modPlayer.count == 0 ? -1 : 1;
            float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
            float baseAngle = modPlayer.data.ToRotation();
            float angle = MathHelper.ToRadians(baseAngle + 90) * player.direction;
            float start = baseAngle + angle * VerticleDirectionSwingVersionTwo;
            float end = baseAngle - angle * VerticleDirectionSwingVersionTwo;
            float currentAngle = MathHelper.SmoothStep(start, end, MethodHelper.InExpo(percentDone));
            player.itemRotation = currentAngle;
            player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
            player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
            player.itemLocation = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * PLAYERARMLENGTH;
        }
    }
    public class MeleeOverhaulPlayer : ModPlayer
    {
        public Vector2 data;
        public int count = -1;
        public Rectangle SwordHitBox;
        int iframeCounter = 0;
        public int delaytimer = 10;
        public int oldHeldItem;
        public override void PreUpdate()
        {
            if (Player.HeldItem.useStyle != CustomUsestyleID.SwingVersionTwo)
            {
                return;
            }
            Player.HeldItem.useTurn = false;
        }
        private void ComboHandleSystem()
        {
            if (Player.HeldItem.type != oldHeldItem)
            {
                count = -1;
                return;
            }
            count++;
            if (count >= 2)
            {
                count = 0;
            }
        }
        //private void SpinAttackExtraHit()
        //{
        //    if (count != 2)
        //    {
        //        return;
        //    }
        //    Item item = Player.HeldItem;
        //    for (int i = 0; i < Main.maxNPCs; i++)
        //    {
        //        NPC npc = Main.npc[i];
        //        if (npc.Hitbox.Intersects(SwordHitBox) && CanAttack(npc) && npc.immune[Player.whoAmI] > 0 && iframeCounter == 0)
        //        {
        //            npc.StrikeNPC((int)(item.damage * 1.5f), item.knockBack, Player.direction, critReference);
        //            iframeCounter = (int)(Player.itemAnimationMax * .333f);
        //            Player.dpsDamage += (int)(item.damage * 1.5f);
        //        }
        //    }
        //}
        public override void PostUpdate()
        {
            delaytimer -= delaytimer > 0 ? 1 : 0;
            iframeCounter -= iframeCounter > 0 ? 1 : 0;
            if (Player.HeldItem.useStyle != CustomUsestyleID.SwingVersionTwo)
            {
                return;
            }
            Player.HeldItem.noUseGraphic = true;
            if (Player.ItemAnimationJustStarted && Player.ItemAnimationActive && delaytimer == 0)
            {
                delaytimer = Player.itemAnimationMax + (int)(Player.itemAnimationMax * .34f);
                data = (Main.MouseWorld - Player.MountedCenter).SafeNormalize(Vector2.Zero);
                ComboHandleSystem();
            }
            if (Player.ItemAnimationActive)
            {
                Player.direction = data.X > 0 ? 1 : -1;
                //SpinAttackExtraHit();
            }
            oldHeldItem = Player.HeldItem.type;
        }
    }
    public class GhostHitBox : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1;
            Projectile.hide = true;
        }
    }
}