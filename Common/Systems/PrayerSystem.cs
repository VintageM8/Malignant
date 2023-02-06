using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;

namespace Malignant.Common
{
    public class PrayerSystem : ModPlayer
    {
        public List<PrayerAbility> Abilities { get; private set; } = new List<PrayerAbility>();
        public int[] SharedCooldowns { get; private set; } = new int[99];
        public int SelectedAbilityIndex { get; private set; }
        public PrayerAbility SelectedAbility => SelectedAbilityIndex < Abilities.Count ? Abilities[SelectedAbilityIndex] : null;

        const string AbilitiesKey = "Abilities";
        const string SelectedAbilityIndexKey = "SelectedAbilityIndex";
        public override void SaveData(TagCompound tag)
        {
            List<string> types = new List<string>();
            foreach (PrayerAbility ability in Abilities)
                types.Add(ability.Type);

            tag[AbilitiesKey] = types;
            tag[SelectedAbilityIndexKey] = SelectedAbilityIndex;

            AbilityUseCoroutines.Clear();
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey(AbilitiesKey))
            {
                List<string> types = tag.Get<List<string>>(AbilitiesKey);
                foreach (string type in types)
                {
                    PrayerAbility ability = PrayerContent.GetAbility(type);
                    if (ability is not null)
                        Abilities.Add(ability);
                }
            }

            if (tag.ContainsKey(SelectedAbilityIndexKey))
            {
                SelectedAbilityIndex = tag.GetInt(SelectedAbilityIndexKey);
            }
        }

        public override void Unload()
        {
            AbilityUseCoroutines.Clear();
            Abilities.ForEach(ability => ability.Unload());
        }

        public override void PostUpdateEquips()
        {
            ManageCoroutines();
        }
        
        public override void PostUpdate()
        {
            ManageCooldowns();
        }

        void ManageCooldowns()
        {
            for (int i = 0; i < SharedCooldowns.Length; i++)
                if (SharedCooldowns[i] > 0)
                    SharedCooldowns[i]--;

            foreach (PrayerAbility ability in Abilities)
            {
                if (ability is not null)
                {
                    if (ability.CooldownIndex >= 0)
                        ability.CooldownTimer = SharedCooldowns[Math.Clamp(SelectedAbility.CooldownIndex, 0, SharedCooldowns.Length - 1)];
                    else
                        ability.CooldownTimer--;
                }
            }
        }

        public static List<Coroutine> AbilityUseCoroutines { get; private set; } = new List<Coroutine>();
        static void ManageCoroutines()
        {
            for (int i = 0; i < AbilityUseCoroutines.Count; i++)
            {
                Coroutine routine = AbilityUseCoroutines[i];

                routine.Update();

                if (!routine.Active)
                {
                    AbilityUseCoroutines.Remove(routine);
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            Player player = Main.LocalPlayer;
            if (MalignantKeybingSystem.UsePrayerAbility.JustPressed && SelectedAbility is not null)
            {

                if (SelectedAbility.TryUseAbility(player, new EntitySource_PrayerAbility(SelectedAbility)))
                {
                    onUseAlpha = 2;

                    if (SelectedAbility.CooldownIndex < 0)
                        SelectedAbility.CooldownTimer = SelectedAbility.Cooldown;
                    else
                        SharedCooldowns[Math.Clamp(SelectedAbility.CooldownIndex, 0, SharedCooldowns.Length - 1)] = SelectedAbility.Cooldown;
                }
            }
            else if (MalignantKeybingSystem.ChangePrayerAbility.JustPressed)
            {
                SelectedAbilityIndex++;
                if (SelectedAbilityIndex >= Abilities.Count)
                    SelectedAbilityIndex = 0;

                if (SelectedAbility is not null)
                {
                    drawProgress = 1;
                    onUseAlpha = 0;

                    SoundEngine.PlaySound(SelectedAbility.SelectSound, Player.Center);
                }
            }
        }

        float drawProgress = 0;
        float onUseAlpha = 0;
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (SelectedAbility is not null && (drawProgress > 0.001f || onUseAlpha > 0.001f))
            {
                Vector2 drawPos = Player.Center - Vector2.UnitY * 75 * Math.Clamp((1 - drawProgress) * 2.2f, 0, 1) - Main.screenPosition;
                Vector2 scale = new Vector2(Math.Clamp((1 - drawProgress) * 2, 0, 1), Math.Clamp(drawProgress * 2.5f, 1, 3)) * (1 + onUseAlpha * 0.25f);
                float alpha = -MathF.Pow(2 * drawProgress - 1, 10) + 1 + onUseAlpha;

                Main.spriteBatch.Draw(
                    SelectedAbility.Texture,
                    drawPos,
                    null,
                    Color.Lerp(Color.White, Color.LightGoldenrodYellow, onUseAlpha) * alpha,
                    0,
                    SelectedAbility.Texture.Size() * 0.5f,
                    scale,
                    SpriteEffects.None,
                    0
                    );

                DynamicSpriteFont font = FontAssets.ItemStack.Value;
                Vector2 textScale = scale * 0.8f;
                ChatManager.DrawColorCodedStringWithShadow(
                    Main.spriteBatch,
                    font,
                    SelectedAbility.DisplayName,
                    drawPos + Vector2.UnitY * (SelectedAbility.Texture.Height * 0.5f + 10 * (1 + onUseAlpha * 0.5f)),
                    Color.LightGoldenrodYellow * alpha,
                    0,
                    font.MeasureString(SelectedAbility.DisplayName) * 0.5f,
                    textScale
                    );

                drawProgress = MathHelper.Lerp(drawProgress, 0, 0.02f);
                onUseAlpha *= 0.98f;
            }
        }
    }

    public class PrayerDrawLayer /* heh （￣︶￣)　*/ : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);
        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            /*
            for (int i = 0; i < PrayerSystem.DrawCoroutines.Count; i++)
            {
                Coroutine drawCR = PrayerSystem.DrawCoroutines[i];

                if (drawCR.Enumerator.Current is DrawData data)
                {
                    drawInfo.DrawDataCache.Add(data);
                    Main.NewText("lol");
                }

                drawCR.Update();

                if (!drawCR.Active)
                {
                    PrayerSystem.DrawCoroutines.Remove(drawCR);
                }
            }
            */
        }
    }

    public class PrayerContent
    {
        static Dictionary<string, PrayerAbility> prayerAbilities;
        public static void Load(Mod mod)
        {
            prayerAbilities = new Dictionary<string, PrayerAbility>();
            foreach (Type type in mod.Code.GetTypes())
            {
                if (type.GetTypeInfo().IsSubclassOf(typeof(PrayerAbility)))
                {
                    PrayerAbility prayerAbility = Activator.CreateInstance(type) as PrayerAbility;
                    prayerAbility.Load();

                    prayerAbilities[prayerAbility.Type] = prayerAbility;
                }
            }
        }

        public static PrayerAbility GetAbility(string internalName)
        {
            return prayerAbilities.ContainsKey(internalName) ? prayerAbilities[internalName] : null;
        }

        public static string AbilityType<T>() where T : PrayerAbility
        {
            return typeof(T).Name;
        }
    }

    public abstract class PrayerAbility
    {
        public string Type => GetType().Name;
        public virtual string DisplayName => Type;
        public virtual string Description => string.Empty;
        public virtual string TexturePath => (GetType().Namespace + "." + GetType().Name).Replace('.', '/');
        public Texture2D Texture { get; private set; }

        public virtual SoundStyle SelectSound => SoundID.Tink;
        public virtual SoundStyle UseSound => SoundID.Unlock;
        public virtual SoundStyle SwapSound => SoundID.Unlock;
        public void Load()
        {
            Texture = ModContent.Request<Texture2D>(TexturePath, AssetRequestMode.ImmediateLoad).Value;
            OnLoad();
        }

        public void Unload()
        {
            OnUnload();
        }

        public virtual void OnLoad()
        {
            
        }

        public virtual void OnUnload()
        {

        }

        public bool TryUseAbility(Player player, EntitySource_PrayerAbility source)
        {
            if (CanUseAbility(player))
            {
                BeginFXCouroutine(OnUseAbilityRoutine(player, source));
                OnUseAbility(player, source);

                SoundEngine.PlaySound(UseSound, player.Center);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Setting this to anything less than 0 allows for the ability to have individual cooldown.
        /// If it's set to something higher or equal to 0 it will have a shared cooldown with any other abilities with the same index.
        /// </summary>
        public virtual int CooldownIndex => -1;
        public virtual int Cooldown => 60;

        int _cooldownTimer;
        public int CooldownTimer { get => _cooldownTimer; set => _cooldownTimer = value; }
        /// <summary>
        /// By default this checks if CooldownTimer <= 0 so if you plan on overriding this make sure to check for that too if you want the cooldown to work.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool CanUseAbility(Player player) => CooldownTimer <= 0;

        /// <summary>
        /// This method runs when player uses the ability.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="source"></param>
        protected virtual void OnUseAbility(Player player, EntitySource_PrayerAbility source) { }

        static void BeginFXCouroutine(IEnumerator enumerator)
        {
            Coroutine coroutine = new Coroutine(enumerator);
            PrayerSystem.AbilityUseCoroutines.Add(coroutine);
        }

        /// <summary>
        /// Allows async doing stuff.
        /// </summary>
        /// <returns>
        /// <para>yield return null: Wait one frame.</para>
        /// yield return WaitFor.Frames(n): Wait n frames.
        /// </returns>
        public virtual IEnumerator OnUseAbilityRoutine(Player player, EntitySource_PrayerAbility source)
        {
            yield return null;
        }
    }

    public class EntitySource_PrayerAbility : IEntitySource
    {
        public string Context => Ability.GetType().Name;
        public readonly PrayerAbility Ability;

        public EntitySource_PrayerAbility(PrayerAbility ability)
        {
            Ability = ability;
        }
    }

    public interface IPrayerItem
    {
        string AbilityType { get; }
    }

    public abstract class PrayerItem : ModItem, IPrayerItem
    {
        public abstract string AbilityType { get; }

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
            {
                Main.NewText("You've learned the " + PrayerContent.GetAbility(AbilityType).DisplayName + " prayer.");
                prayerSystem.Abilities.Add(PrayerContent.GetAbility(AbilityType));
            }
                

            return shouldAddAbility;
        }
    }
}
