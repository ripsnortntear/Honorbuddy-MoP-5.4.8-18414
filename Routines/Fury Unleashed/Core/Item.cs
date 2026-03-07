using FuryUnleashed.Core.Utilities;
using FuryUnleashed.Interfaces.Settings;
using FuryUnleashed.Rotations;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = Styx.TreeSharp.Action;
using Enum = FuryUnleashed.Core.Helpers.Enum;

namespace FuryUnleashed.Core
{
    internal static class Item
    {

        public static float MeleeHaste { get; set; }
        public static float MeleeHit { get; set; }

        public static float AttackSpeed { get; set; }
        public static float CritRating { get; set; }
        public static float CritChance { get; set; }
        public static float Expertise { get; set; }
        public static float Mastery { get; set; }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region Equipped Item Usage Wrappers
        private static bool CanUseEquippedItem(WoWItem item)
        {
            var itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;

            return item.Usable && item.Cooldown <= 0;
        }

        private static bool CanUseTrinket(Enum.AbilityTrigger usage, WoWItem trinket)
        {
            var canUseTrinket = trinket != null && CanUseEquippedItem(trinket);

            switch (usage)
            {
                case Enum.AbilityTrigger.OnBossDummy:
                    return canUseTrinket && Unit.IsTargetBoss;
                case Enum.AbilityTrigger.OnBlTwHr:
                    return canUseTrinket && Global.HasteAbilities;
                case Enum.AbilityTrigger.Always:
                    return canUseTrinket;
            }
            return false;
        }

        private static bool CanUseHands(Enum.AbilityTrigger usage, WoWItem hands)
        {
            var canUseHands = hands != null && CanUseEquippedItem(hands);

            switch (usage)
            {
                case Enum.AbilityTrigger.OnBossDummy:
                    return canUseHands && Unit.IsTargetBoss;
                case Enum.AbilityTrigger.OnBlTwHr:
                    return canUseHands && Global.HasteAbilities;
                case Enum.AbilityTrigger.Always:
                    return canUseHands;

            }
            return false;
        }

        private static void UseArmsItems()
        {
            var firstTrinket = StyxWoW.Me.Inventory.Equipped.Trinket1;
            var secondTrinket = StyxWoW.Me.Inventory.Equipped.Trinket2;
            var hands = StyxWoW.Me.Inventory.Equipped.Hands;

            if (InternalSettings.Instance.Arms.Trinket1 == Enum.AbilityTrigger.Never &&
                InternalSettings.Instance.Arms.Trinket2 == Enum.AbilityTrigger.Never &&
                InternalSettings.Instance.Arms.UseHands == Enum.AbilityTrigger.Never)
                return;

            if (CanUseTrinket(InternalSettings.Instance.Arms.Trinket1, firstTrinket))
            {
                firstTrinket.Use();
                Logger.CombatLogFb("Using: Slot 1 trinket is used.");
            }

            if (CanUseTrinket(InternalSettings.Instance.Arms.Trinket2, secondTrinket))
            {
                secondTrinket.Use();
                Logger.CombatLogFb("Using: Slot 2 trinket is used.");
            }

            if (CanUseHands(InternalSettings.Instance.Arms.UseHands, hands) && Global.ColossusSmashTracker &&
                        !Global.FadingOffensiveCooldowns(10000))
            {
                hands.Use();
                Logger.CombatLogFb("Using: Engineering hands are used.");
            }
        }

        private static void UseFuryItems()
        {
            var firstTrinket = StyxWoW.Me.Inventory.Equipped.Trinket1;
            var secondTrinket = StyxWoW.Me.Inventory.Equipped.Trinket2;
            var hands = StyxWoW.Me.Inventory.Equipped.Hands;

            if (InternalSettings.Instance.Fury.Trinket1 == Enum.AbilityTrigger.Never && 
                InternalSettings.Instance.Fury.Trinket2 == Enum.AbilityTrigger.Never &&
                InternalSettings.Instance.Fury.UseHands == Enum.AbilityTrigger.Never)
                return;

            if (CanUseTrinket(InternalSettings.Instance.Fury.Trinket1, firstTrinket))
            {
                firstTrinket.Use();
                Logger.CombatLogFb("Using: Slot 1 trinket is used.");
            }

            if (CanUseTrinket(InternalSettings.Instance.Fury.Trinket2, secondTrinket))
            {
                secondTrinket.Use();
                Logger.CombatLogFb("Using: Slot 2 trinket is used.");
            }

            if (CanUseHands(InternalSettings.Instance.Fury.UseHands, hands))
            {
                hands.Use();
                Logger.CombatLogFb("Using: Engineering hands are used.");
            }
        }

        private static void UseProtItems()
        {
            var firstTrinket = StyxWoW.Me.Inventory.Equipped.Trinket1;
            var secondTrinket = StyxWoW.Me.Inventory.Equipped.Trinket2;
            var hands = StyxWoW.Me.Inventory.Equipped.Hands;

            if (InternalSettings.Instance.Protection.Trinket1 == Enum.AbilityTrigger.Never &&
                InternalSettings.Instance.Protection.Trinket2 == Enum.AbilityTrigger.Never &&
                InternalSettings.Instance.Protection.UseHands == Enum.AbilityTrigger.Never)
                return;

            if (CanUseTrinket(InternalSettings.Instance.Protection.Trinket1, firstTrinket))
            {
                firstTrinket.Use();
                Logger.CombatLogFb("Using: Slot 1 trinket is used.");
            }

            if (CanUseTrinket(InternalSettings.Instance.Protection.Trinket2, secondTrinket))
            {
                secondTrinket.Use();
                Logger.CombatLogFb("Using: Slot 2 trinket is used.");
            }

            if (CanUseHands(InternalSettings.Instance.Protection.UseHands, hands))
            {
                hands.Use();
                Logger.CombatLogFb("Using: Engineering hands are used.");
            }
        }

        public static Composite CreateItemBehaviour()
        {
            return new Switch<WoWSpec>(ret => Me.Specialization,
                new SwitchArgument<WoWSpec>(WoWSpec.WarriorArms,
                    new Decorator(ret => (Global.BloodbathAura || !Global.BloodbathTalent) && Global.ColossusSmashAura,
                        new Action(ret => { UseArmsItems(); return RunStatus.Failure; } ))),
                new SwitchArgument<WoWSpec>(WoWSpec.WarriorFury,
                    new Decorator(ret => (Global.BloodbathAura || !Global.BloodbathTalent) && Global.ColossusSmashAura,
                        new Action(ret => { UseFuryItems(); return RunStatus.Failure; }))),
                new SwitchArgument<WoWSpec>(WoWSpec.WarriorProtection,
                    new Decorator(ret => Unit.IsTargettingMe && Me.HealthPercent <= InternalSettings.Instance.Protection.CheckTrinketsNum,
                        new Action(ret => { UseProtItems(); return RunStatus.Failure; }))));
        }
        #endregion

        #region Useable Item Wrappers
        public static WoWItem FindFirstUsableItemBySpell(params string[] spellNames)
        {
            List<WoWItem> carried = StyxWoW.Me.CarriedItems;
            var spellNameHashes = new HashSet<string>(spellNames);

            return (from i in carried
                    let spells = i.ItemSpells
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    where i.ItemInfo != null && spells != null && spells.Count != 0 && i.Usable && i.Cooldown == 0 && i.ItemInfo.RequiredLevel <= StyxWoW.Me.Level && spells.Any(s => s.IsValid && s.ActualSpell != null && spellNameHashes.Contains(s.ActualSpell.Name))
                    orderby i.ItemInfo.Level descending
                    select i).FirstOrDefault();
        }

        public static Composite ArmsUseHealthStone()
        {
            return new PrioritySelector(
                new Decorator(ret => InternalSettings.Instance.Arms.CheckHealthStone && Me.HealthPercent < InternalSettings.Instance.Arms.CheckHealthStoneNum,
                    new PrioritySelector(ctx => FindFirstUsableItemBySpell("Healthstone"),
                        new Decorator(ret => ret != null,
                            new Action(ret =>
                            {
                                ((WoWItem)ret).UseContainerItem();
                                Logger.CombatLogLg("Using {0}", ((WoWItem)ret).Name);
                            })))));
        }

        public static Composite FuryUseHealthStone()
        {
            return new PrioritySelector(
                new Decorator(ret => InternalSettings.Instance.Fury.CheckHealthStone && Me.HealthPercent < InternalSettings.Instance.Fury.CheckHealthStoneNum,
                    new PrioritySelector(ctx => FindFirstUsableItemBySpell("Healthstone"),
                        new Decorator(ret => ret != null,
                            new Action(ret =>
                            {
                                ((WoWItem)ret).UseContainerItem();
                                Logger.CombatLogLg("Using {0}", ((WoWItem)ret).Name);
                            })))));
        }

        public static Composite ProtUseHealthStone()
        {
            return new PrioritySelector(
                new Decorator(ret => InternalSettings.Instance.Protection.CheckHealthStone && Me.HealthPercent < InternalSettings.Instance.Protection.CheckHealthStoneNum,
                    new PrioritySelector(ctx => FindFirstUsableItemBySpell("Healthstone"),
                        new Decorator(ret => ret != null,
                            new Action(ret =>
                            {
                                ((WoWItem)ret).UseContainerItem();
                                Logger.CombatLogLg("Using {0}", ((WoWItem)ret).Name);
                            })))));
        }
        #endregion

        #region Other Item Functions
        internal static bool WieldsOneHandedWeapons
        {
            get
            {
                try
                {
                    switch (Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass)
                    {
                        case WoWItemWeaponClass.Axe:
                        case WoWItemWeaponClass.Exotic:
                        case WoWItemWeaponClass.Mace:
                        case WoWItemWeaponClass.Sword:
                            return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.DiagLogPu("Wielding TwoHander: {0}", ex);
                }
                return false;
            }
        }

        internal static bool WieldsTwoHandedWeapons
        {
            get
            {
                try
                {
                    switch (Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass)
                    {
                        case WoWItemWeaponClass.ExoticTwoHand:
                        case WoWItemWeaponClass.MaceTwoHand:
                        case WoWItemWeaponClass.AxeTwoHand:
                        case WoWItemWeaponClass.SwordTwoHand:
                        case WoWItemWeaponClass.Polearm:
                            return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Logger.DiagLogPu("Wielding TwoHander: {0}", ex);
                }
                return false;
            }
        }

        internal static void RefreshSecondaryStats()
        {
            AttackSpeed = Lua.GetReturnVal<float>("return UnitAttackSpeed('player')", 0);
            CritChance = Lua.GetReturnVal<float>("return GetCritChance()", 0);
            CritRating = Lua.GetReturnVal<float>("return GetCombatRating(CR_CRIT_MELEE)", 0);
            Mastery = Lua.GetReturnVal<float>("return GetCombatRating(CR_MASTERY)", 0);
            Expertise = Lua.GetReturnVal<float>("return GetCombatRating(CR_EXPERTISE)", 0);
            MeleeHit = Lua.GetReturnVal<float>("return GetCombatRating(CR_HIT_MELEE)", 0);
            MeleeHaste = Lua.GetReturnVal<float>("return GetCombatRating(CR_HASTE_MELEE)", 0);
        }
        #endregion
    }
}
