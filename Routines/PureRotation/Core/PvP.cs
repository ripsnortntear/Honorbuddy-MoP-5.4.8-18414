using System;
using System.Collections.Generic;
using System.Linq;
using PureRotation.Helpers;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace PureRotation.Core
{
    internal static class PvP
    {
        /*
        Priority == Me > Partner > Stop cast > Lock opponent > DPS > Burn
        Me == Stay alive
        Partner == assist, keep him alive
        Stop cast == stop worthy cast via any means
        Lock opponent == stop worthy opponent via any means for assist of burn or other
        DPS == hold a consistant DMG output
        Burn == Bring on the rain...
        */

        // this is here incase i want to cache the auras
        private static IEnumerable<WoWAura> AllAuras(this WoWUnit u)
        {
            return u.GetAllAuras();
        }

        private static IEnumerable<WoWPartyMember> GroupMembers { get { return !StyxWoW.Me.GroupInfo.IsInRaid ? StyxWoW.Me.GroupInfo.PartyMembers : StyxWoW.Me.GroupInfo.RaidMembers; } }

        public delegate double OrderByFilter(WoWUnit unit);

        #region Friendly UnitDetection

        // Healers
        private static readonly WoWSpec[] Healers = new[] { WoWSpec.PriestHoly, WoWSpec.DruidRestoration, WoWSpec.ShamanRestoration, WoWSpec.PaladinHoly, WoWSpec.PriestDiscipline, WoWSpec.MonkMistweaver };

        // Casters
        private static readonly WoWSpec[] Casters = new[] { WoWSpec.HunterBeastMastery, WoWSpec.HunterMarksmanship, WoWSpec.HunterSurvival, WoWSpec.WarlockAffliction, WoWSpec.WarlockDemonology, WoWSpec.WarlockDestruction, WoWSpec.PriestShadow, WoWSpec.ShamanElemental, WoWSpec.MageFrost, WoWSpec.MageFire, WoWSpec.MageArcane, WoWSpec.DruidBalance };

        // Melee
        private static readonly WoWSpec[] Melee = new[] { WoWSpec.PaladinRetribution, WoWSpec.DeathKnightFrost, WoWSpec.DeathKnightUnholy, WoWSpec.WarriorArms, WoWSpec.WarriorFury, WoWSpec.MonkWindwalker, WoWSpec.DruidFeral, WoWSpec.RogueAssassination, WoWSpec.RogueCombat, WoWSpec.RogueSubtlety, WoWSpec.ShamanEnhancement };

        // Tanks
        private static readonly WoWSpec[] Tanks = new[] { WoWSpec.DeathKnightBlood, WoWSpec.DruidGuardian, WoWSpec.PaladinProtection, WoWSpec.WarriorProtection, WoWSpec.MonkBrewmaster };

        #endregion Friendly UnitDetection

        #region Enemy UnitDetection

        private static readonly string[] EnemyHealerAuras = new[] { "", "" };
        private static readonly string[] EnemyCasterAuras = new[] { "", "" };
        private static readonly string[] EnemyMeleeAuras = new[] { "Unholy Presence", "Frost Presence" };
        private static readonly string[] EnemyTankAuras = new[] { "Blood Presence", "" };

        // Healers
        private static IEnumerable<WoWPlayer> EnemyHealers
        {
            get
            {
                try
                {
                    return (from u in ObjectManager.GetObjectsOfType<WoWPlayer>(false, false)
                            where !u.IsFriendly && u.Attackable && u.HasAnyAura(EnemyHealerAuras) && u.DistanceSqr <= 40.0
                            orderby u.HealthPercent
                            select u);
                }
                catch (Exception e)
                {
                    Logger.DebugLog("Exception thrown in EnemyHealers: {0}", e);
                    return null;
                }
            }
        }

        // Casters
        private static IEnumerable<WoWPlayer> EnemyCasters
        {
            get
            {
                try
                {
                    return (from u in ObjectManager.GetObjectsOfType<WoWPlayer>(false, false)
                            where !u.IsFriendly && u.Attackable && u.HasAnyAura(EnemyCasterAuras) && u.DistanceSqr <= 40.0
                            orderby u.HealthPercent
                            select u);
                }
                catch (Exception e)
                {
                    Logger.DebugLog("Exception thrown in EnemyCasters: {0}", e);
                    return null;
                }
            }
        }

        // Melee
        private static IEnumerable<WoWPlayer> EnemyMelee
        {
            get
            {
                try
                {
                    return (from u in ObjectManager.GetObjectsOfType<WoWPlayer>(false, false)
                            where !u.IsFriendly && u.Attackable && u.HasAnyAura(EnemyMeleeAuras) && u.DistanceSqr <= 40.0
                            orderby u.HealthPercent
                            select u);
                }
                catch (Exception e)
                {
                    Logger.DebugLog("Exception thrown in EnemyMelee: {0}", e);
                    return null;
                }
            }
        }

        // Tanks
        private static IEnumerable<WoWPlayer> EnemyTanks
        {
            get
            {
                try
                {
                    return (from u in ObjectManager.GetObjectsOfType<WoWPlayer>(false, false)
                            where !u.IsFriendly && u.Attackable && u.HasAnyAura(EnemyTankAuras) && u.DistanceSqr <= 40.0
                            orderby u.HealthPercent
                            select u);
                }
                catch (Exception e)
                {
                    Logger.DebugLog("Exception thrown in EnemyTanks: {0}", e);
                    return null;
                }
            }
        }

        // Enemys
        private static IEnumerable<WoWPlayer> Enemys
        {
            get
            {
                try
                {
                    return (from u in ObjectManager.GetObjectsOfType<WoWPlayer>(false, false)
                            where !u.IsFriendly && u.Attackable && u.DistanceSqr <= 40.0
                            orderby u.HealthPercent
                            select u);
                }
                catch (Exception e)
                {
                    Logger.DebugLog("Exception thrown in Enemys: {0}", e);
                    return null;
                }
            }
        }

        #endregion Enemy UnitDetection

        #region get Targets

        public static WoWUnit GetPvPHealTarget(string spell, PvPHealMode mode, double healPercent)
        {
            if (mode == PvPHealMode.Self && StyxWoW.Me.HealthPercent < healPercent && SpellManager.CanCast(spell, StyxWoW.Me))
            {
                return StyxWoW.Me;
            }

            if (mode == PvPHealMode.Focus)
            {
                if (StyxWoW.Me.GroupInfo.IsInParty)
                {
                    // If the player has a focus target set, use it instead.
                    if (StyxWoW.Me.FocusedUnitGuid != 0 && StyxWoW.Me.FocusedUnit.IsAlive && StyxWoW.Me.FocusedUnit.HealthPercent < healPercent && SpellManager.CanCast(spell, StyxWoW.Me.FocusedUnit))
                    {
                        return StyxWoW.Me.FocusedUnit;
                    }
                }
                return null;
            }

            if (mode == PvPHealMode.Healers)
            {
                var healer = GroupMembers.FirstOrDefault(friendly => Healers.Contains(friendly.Specialization) && (friendly.ToPlayer() != StyxWoW.Me && friendly.ToPlayer().HealthPercent < healPercent && SpellManager.CanCast(spell, friendly.ToPlayer())));
                if (healer != null) return healer.ToPlayer();
            }

            if (mode == PvPHealMode.Tanks)
            {
                var tank = GroupMembers.FirstOrDefault(friendly => Tanks.Contains(friendly.Specialization) && (friendly.ToPlayer() != StyxWoW.Me && friendly.ToPlayer().HealthPercent < healPercent && SpellManager.CanCast(spell, friendly.ToPlayer())));
                if (tank != null) return tank.ToPlayer();
            }

            if (mode == PvPHealMode.Casters)
            {
                var caster = GroupMembers.FirstOrDefault(friendly => Casters.Contains(friendly.Specialization) && (friendly.ToPlayer() != StyxWoW.Me && friendly.ToPlayer().HealthPercent < healPercent && SpellManager.CanCast(spell, friendly.ToPlayer())));
                if (caster != null) return caster.ToPlayer();
            }

            if (mode == PvPHealMode.Melee)
            {
                var melee = GroupMembers.FirstOrDefault(friendly => Melee.Contains(friendly.Specialization) && (friendly.ToPlayer() != StyxWoW.Me && friendly.ToPlayer().HealthPercent < healPercent && SpellManager.CanCast(spell, friendly.ToPlayer())));
                if (melee != null) return melee.ToPlayer();
            }

            if (mode == PvPHealMode.Others)
            {
                return StyxWoW.Me.RaidMembers.FirstOrDefault(friendly => friendly != StyxWoW.Me && friendly.HealthPercent < healPercent && SpellManager.CanCast(spell, friendly));
            }
            return mode != PvPHealMode.Anyone ? null : StyxWoW.Me.RaidMembers.FirstOrDefault(friendly => friendly.HealthPercent < healPercent && SpellManager.CanCast(spell, friendly));
        }

        public static WoWUnit GetPvPCCTarget(PvPCCMode mode, OrderByFilter filter)
        {
            if (mode == PvPCCMode.Focus)
            {
                if (StyxWoW.Me.GroupInfo.IsInParty)
                {
                    // If the player has a focus target set, use it instead.
                    if (StyxWoW.Me.FocusedUnitGuid != 0 && StyxWoW.Me.FocusedUnit.IsAlive && !StyxWoW.Me.FocusedUnit.IsCrowdControlled())
                    {
                        return StyxWoW.Me.FocusedUnit;
                    }
                }
                return null;
            }

            if (mode == PvPCCMode.Healers)
            {
                return EnemyHealers.Where(a => !a.IsCrowdControlled()).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPCCMode.Tanks)
            {
                return EnemyTanks.Where(a => !a.IsCrowdControlled()).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPCCMode.Casters)
            {
                return EnemyCasters.Where(a => !a.IsCrowdControlled()).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPCCMode.Melee)
            {
                return EnemyMelee.Where(a => !a.IsCrowdControlled()).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPCCMode.CurrentTarget)
            {
                return Enemys.Where(a => !a.IsCrowdControlled() && (a == StyxWoW.Me.CurrentTarget)).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPCCMode.NotCurrentTarget)
            {
                return Enemys.Where(a => !a.IsCrowdControlled() && (a != StyxWoW.Me.CurrentTarget)).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPCCMode.Anyone)
            {
                return Enemys.Where(a => !a.IsCrowdControlled()).OrderBy(u => filter(u)).FirstOrDefault();
            }

            return null;
        }

        public static WoWUnit GetPvPInteruptTarget(PvPInteruptMode mode, OrderByFilter filter)
        {
            if (mode == PvPInteruptMode.Focus)
            {
                if (StyxWoW.Me.GroupInfo.IsInParty)
                {
                    // If the player has a focus target set, use it instead.
                    if (StyxWoW.Me.FocusedUnitGuid != 0 && StyxWoW.Me.FocusedUnit.IsAlive && StyxWoW.Me.FocusedUnit.IsCasting && StyxWoW.Me.FocusedUnit.CanInterruptCurrentSpellCast)
                    {
                        return StyxWoW.Me.FocusedUnit;
                    }
                }
                return null;
            }

            if (mode == PvPInteruptMode.Healers)
            {
                return EnemyHealers.Where(a => a.IsCasting && a.CanInterruptCurrentSpellCast).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPInteruptMode.Tanks)
            {
                return EnemyTanks.Where(a => a.IsCasting && a.CanInterruptCurrentSpellCast).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPInteruptMode.Casters)
            {
                return EnemyCasters.Where(a => a.IsCasting && a.CanInterruptCurrentSpellCast).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPInteruptMode.Melee)
            {
                return EnemyMelee.Where(a => a.IsCasting && a.CanInterruptCurrentSpellCast).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPInteruptMode.CurrentTarget)
            {
                return Enemys.Where(a => a.IsCasting && a.CanInterruptCurrentSpellCast).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPInteruptMode.NotCurrentTarget)
            {
                return Enemys.Where(a => a.IsCasting && a.CanInterruptCurrentSpellCast).OrderBy(u => filter(u)).FirstOrDefault();
            }

            if (mode == PvPInteruptMode.Anyone)
            {
                return Enemys.Where(a => a.IsCasting && a.CanInterruptCurrentSpellCast).OrderBy(u => filter(u)).FirstOrDefault();
            }

            return null;
        }

        #endregion get Targets

        #region OrderbyFilters

        internal static OrderByFilter Orderby(string setting)
        {
            switch (setting)
            {
                case "Health":
                    return u => u.HealthPercent;
                case "Distance":
                    return u => u.Distance;
            }
            return u => u.Distance;
        }

        #endregion OrderbyFilters

        /// <summary>Creates Crowd control spell cast composite. It will attempt to crowd control if possible!</summary>
        public static Composite CrowdControlSpellCasts(Spell.UnitSelectionDelegate onUnit)
        {
            return
                new Decorator(
                    ret => onUnit != null && onUnit(ret) != null && !onUnit(ret).IsCrowdControlled(),
                    new PrioritySelector(

                // Class Specific first...
                        new Switch<WoWClass>(ctx => StyxWoW.Me.Class,
                                             new SwitchArgument<WoWClass>(WoWClass.Druid,
                                                                          new PrioritySelector(

                // do magic CC abilities.
                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.Hunter,
                                                                          new PrioritySelector(
                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.Mage,
                                                                          new PrioritySelector(
                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.Paladin,
                                                                          new PrioritySelector(
                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.Priest,
                                                                          new Action(
                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.Rogue,
                                                                          new PrioritySelector(
                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.Shaman,
                                                                          new PrioritySelector(

                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.Warlock,
                                                                          new PrioritySelector(
                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.Warrior,
                                                                          new PrioritySelector(

                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.DeathKnight,
                                                                          new PrioritySelector(
                                                                              Spell.Cast("Chains of Ice", ret => onUnit!=null && onUnit(ret)!=null && !onUnit(ret).HasAura(45524) && onUnit(ret).DistanceSqr > 5 * 5 && onUnit(ret).DistanceSqr <= 30 * 30)
                                                                              )),
                                             new SwitchArgument<WoWClass>(WoWClass.Monk,
                                                                          new PrioritySelector(
                                                                              ))
                            )));
        }

        #region Extensions

        public static int BleedCount(this WoWUnit u)
        {
            return u.AllAuras().Count(a => (a.Spell.Mechanic == WoWSpellMechanic.Bleeding));
        }

        public static int RootCount(this WoWUnit u)
        {
            return u.AllAuras().Count(a => (a.Spell.Mechanic == WoWSpellMechanic.Rooted || a.Spell.Mechanic == WoWSpellMechanic.Frozen));
        }

        public static int SlowCount(this WoWUnit u)
        {
            return u.AllAuras().Count(a => (a.Spell.Mechanic == WoWSpellMechanic.Slowed || a.Spell.Mechanic == WoWSpellMechanic.Snared));
        }

        public static bool IsBleeding(this WoWUnit unit)
        {
            return unit.HasAuraWithMechanic(WoWSpellMechanic.Bleeding);
        }

        public static bool IsStunned(this WoWUnit unit)
        {
            return unit.HasAuraWithMechanic(WoWSpellMechanic.Stunned, WoWSpellMechanic.Incapacitated);
        }

        public static bool IsFeared(this WoWUnit unit)
        {
            return unit.HasAuraWithMechanic(WoWSpellMechanic.Fleeing);
        }

        public static bool IsRooted(this WoWUnit unit)
        {
            return unit.HasAuraWithMechanic(WoWSpellMechanic.Rooted, WoWSpellMechanic.Shackled);
        }

        public static bool IsSilenced(WoWUnit unit)
        {
            return unit.AllAuras().Any(a => a.IsHarmful && (a.Spell.Mechanic == WoWSpellMechanic.Interrupted || a.Spell.Mechanic == WoWSpellMechanic.Silenced));
        }

        public static bool IsSlowed(this WoWUnit unit)
        {
            return unit.AllAuras().Any(a => a.Spell.SpellEffects.Any(e => e.AuraType == WoWApplyAuraType.ModDecreaseSpeed));
        }

        public static bool IsCarryingFlag(this WoWUnit unit)
        {
            return unit != null && (unit.HasAura("Alliance Flag") || unit.HasAura("Horde Flag") || unit.HasAura("Netherstorm Flag"));
        }

        internal static bool IsCrowdControlled(this WoWUnit unit)
        {
            if (unit != null)
            {
                return unit.AllAuras().Any(a =>
                                                    a.IsHarmful && (
                                                    a.Spell.Mechanic == WoWSpellMechanic.Banished ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Disoriented ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Charmed ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Horrified ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Incapacitated ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Polymorphed ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Sapped ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Shackled ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Asleep ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Frozen ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Invulnerable ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Invulnerable2 ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Turned ||
                                                    a.Spell.Mechanic == WoWSpellMechanic.Fleeing ||

                                                    // Really want to ignore hexed mobs.
                                                    a.Spell.Name == "Hex"));
            }
            return false;
        }

        internal static bool IsInCC(this WoWUnit unit)
        {
            if (unit != null)
            {
                if (unit.IsHostile &&
                   (unit.ActiveAuras.ContainsKey("Repentance") ||
                    unit.ActiveAuras.ContainsKey("Blinding Light") ||
                    unit.ActiveAuras.ContainsKey("Intimidating Shout") ||
                    unit.ActiveAuras.ContainsKey("Wyvern Sting") ||
                    unit.ActiveAuras.ContainsKey("Polymorph") ||
                    unit.ActiveAuras.ContainsKey("Freezing Trap")))
                    return true;
            }
            return false;
        }

        #endregion Extensions

        // Me.CurrentTarget.IsCastingHealingSpell <-- nice
    }
}