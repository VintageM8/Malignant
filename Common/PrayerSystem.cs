using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ID;
using Malignant.Content.Buffs;
using Malignant.Content.Projectiles.Prayer;
using Malignant.Content.Projectiles.Enemy.Warlock;
using System;

namespace Malignant.Common
{
    public class PrayerSystem : ModPlayer
    {
        public AbiltyID CurrentA;

        public enum AbiltyID : int
        {
            None,//0
            Paladin,//1
            Sabbath,
        }
        /*public override void TagCompound SaveData()
        {
            return new TagCompound 
            {
				{"CurrentA", (int)CurrentA},
            };
        }
        public override void Load(TagCompound tag)
        {
            CurrentA = (AbiltyID)tag.GetInt("CurrentA");        
        }*/

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Player player = Main.player[Main.myPlayer];
            Player p = Main.player[Main.myPlayer];
            if (Malignant.UseAbilty.JustPressed && !p.HasBuff(ModContent.BuffType<Cooldown>()))
            {
                switch (CurrentA)
                {//Add stuff for the abiltys here, if you want to make more, add more IDs
                    case (int)AbiltyID.None:
                        // Main.NewText("No Abilty");
                        // p.AddBuff(ModContent.BuffType<Cooldown>(), 120);
                        break;
                    case AbiltyID.Paladin:
                        //Main.NewText("Paladin");
                        p.AddBuff(ModContent.BuffType<Cooldown>(), 10);
                        Projectile.NewProjectile(p.GetSource_Misc("Paladin"), Main.MouseWorld + new Vector2(0, -50), new Vector2(0, 0), ModContent.ProjectileType<WindsofGod>(), 10, 0f, p.whoAmI);
                        break;
                    case AbiltyID.Sabbath:
                        //Main.NewText("Paladin");
                        p.AddBuff(ModContent.BuffType<Cooldown>(), 10);

                        int i = 0; 
                        float spread = 10f * 0.0174f;
                        double startAngle = Math.Atan2(6, 6) - spread / 2;
                        double deltaAngle = spread / 8f;
                        double offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;

                        Projectile.NewProjectile(p.GetSource_Misc("Smite of the Sabbath"), player.Center.X, player.Center.Y, (float)(Math.Sin(offsetAngle) * 3f), (float)(Math.Cos(offsetAngle) * 3f), ModContent.ProjectileType<HolyWind>(), 10, 0f, player.whoAmI);
                        

                        break;
                    default:
                        Mod.Logger.InfoFormat("Unknown Ability ID: {0}", CurrentA);
                        break;
                }
            }
        }
    }
}
