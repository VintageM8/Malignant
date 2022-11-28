using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ID;
using Malignant.Content.Buffs;
using Malignant.Content.Projectiles.Prayer;
using Malignant.Content.Projectiles.Enemy.Warlock;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using ReLogic.Content;

namespace Malignant.Common
{
    public class PrayerSystem : ModPlayer
    {
        public List<PrayerAbility> Abilities { get; private set; } = new List<PrayerAbility>();
        public int[] SharedCooldowns { get; private set; } = new int[99];
        public int SelectedAbilityIndex { get; private set; }
        public PrayerAbility SelectedAbility => SelectedAbilityIndex < Abilities.Count ? Abilities[SelectedAbilityIndex] : null;

        const string AbilitiesKey = "Abilities";
        public override void SaveData(TagCompound tag)
        {
            List<int> types = new List<int>();
            foreach (PrayerAbility ability in Abilities)
                types.Add(ability.Type);

            tag[AbilitiesKey] = types;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey(AbilitiesKey))
            {
                List<int> types = tag.Get<List<int>>(AbilitiesKey);
                foreach (int type in types)
                    Abilities.Add(PrayerContent.PrayerAbilities[type]);
            }
        }

        public override void PostUpdate()
        {
            for (int i = 0; i < SharedCooldowns.Length; i++)
                if (SharedCooldowns[i] > 0)
                    SharedCooldowns[i]--;
                
            foreach (PrayerAbility ability in Abilities)
                if (ability.CooldownIndex >= 0)
                    ability.Cooldown = SharedCooldowns[Math.Clamp(SelectedAbility.CooldownIndex, 0, SharedCooldowns.Length - 1)];
                else
                    ability.Cooldown--;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Player player = Main.player[Main.myPlayer];
            if (Malignant.UseAbilty.JustPressed && SelectedAbility is not null)
            {
                int cooldown = SelectedAbility.TryUseAbility(player);
                if (cooldown > 0)
                {
                    if (SelectedAbility.CooldownIndex < 0)
                        SelectedAbility.Cooldown = cooldown;
                    else
                        SharedCooldowns[Math.Clamp(SelectedAbility.CooldownIndex, 0, SharedCooldowns.Length - 1)] = cooldown;
                }
            }
        }
    }

    public class PrayerContent
    {
        public static List<PrayerAbility> PrayerAbilities { get; private set; }
        public static void Load(Mod mod)
        {
            PrayerAbilities = new List<PrayerAbility>();
            foreach (Type type in mod.Code.GetTypes())
            {
                if (type.GetTypeInfo().IsSubclassOf(typeof(PrayerAbility)))
                {
                    PrayerAbility prayerAbility = Activator.CreateInstance(type) as PrayerAbility;
                    prayerAbility.Load();

                    PrayerAbilities.Add(prayerAbility);
                }
            }
        }

        public static int AbilityType<T>() where T : PrayerAbility
        {
            return PrayerAbilities.IndexOf(PrayerAbilities.First(ability => ability is T));
        }
    }

    public abstract class PrayerAbility
    {
        public int Type => PrayerContent.PrayerAbilities.IndexOf(this);

        public virtual string Texture => (GetType().Namespace + "." + GetType().Name).Replace('.', '/');
        public Texture2D AbilityTexture { get; private set; }

        public void Load()
        {
            AbilityTexture = ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad).Value;
        }

        public int TryUseAbility(Player player)
        {
            if (CanUseAbility())
            {
                return OnUseAbility(player);
            }
            return 0;
        }

        /// <summary>
        /// Setting this to anything less than 0 allows for the ability to have individual cooldown.
        /// If it's set to something higher than 0 it will have a shared cooldown with any other abilities with the same index.
        /// </summary>
        public virtual int CooldownIndex => 0;

        int _cooldown;
        public int Cooldown { get => _cooldown; set => _cooldown = value; }
        public virtual bool CanUseAbility() => Cooldown <= 0;

        /// <summary>
        /// This method runs when player uses the ability.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>This ability's cooldown.</returns>
        protected virtual int OnUseAbility(Player player)
        {
            return 60;
        }
    }

    public interface IPrayerItem
    {
        int AbilityType { get; }
    }

    public abstract class PrayerItem : ModItem, IPrayerItem
    {
        public abstract int AbilityType { get; }

        public sealed override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.maxStack = 1;
            Item.noMelee = true;
            Item.consumable = true;
            Item.autoReuse = false;

            ChangeDefaults();
        }

        public virtual void ChangeDefaults() { }

        public sealed override bool? UseItem(Player player)
        {
            PrayerSystem prayerSystem = player.GetModPlayer<PrayerSystem>();

            bool shouldAddAbility = !prayerSystem.Abilities.Any(ability => ability.Type == AbilityType);
            if (shouldAddAbility)
                prayerSystem.Abilities.Add(PrayerContent.PrayerAbilities[AbilityType]);

            return shouldAddAbility;
        }
    }
}
