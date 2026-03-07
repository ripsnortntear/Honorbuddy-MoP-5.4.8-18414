using System;
using System.Collections.Generic;
using System.Linq;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.Common;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Lua = PureRotation.Helpers.Lua;

namespace PureRotation.Classes.DeathKnight
{
    internal static class Common
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region Runes

        internal static int DeathRuneSlotsActive
        {
            get { return StyxWoW.Me.DeathRuneCount; }
        }

        internal static int BloodRuneSlotsActive
        {
            get
            {
               // Logger.InfoLog("B {0}, b {1}", StyxWoW.Me.GetRuneCount(0) != 0 ? "Ready" : "depleted", StyxWoW.Me.GetRuneCount(1) != 0 ? "Ready" : "depleted");
                return StyxWoW.Me.BloodRuneCount;
            }
        }

        internal static int FrostRuneSlotsActive
        {
            get
            {
               // Logger.InfoLog("F {0}, f {1}", StyxWoW.Me.GetRuneCount(2) != 0 ? "Ready" : "depleted", StyxWoW.Me.GetRuneCount(3) != 0 ? "Ready" : "depleted");
                return StyxWoW.Me.FrostRuneCount;
            }
        }

        internal static int UnholyRuneSlotsActive
        {
            get
            {
              //  Logger.InfoLog("U {0}, u {1}", StyxWoW.Me.GetRuneCount(4) != 0 ? "Ready" : "depleted", StyxWoW.Me.GetRuneCount(5) != 0 ? "Ready" : "depleted");
                return StyxWoW.Me.UnholyRuneCount;
            }
        }

        internal static int ActiveRuneCount
        {
            get
            {
                return StyxWoW.Me.BloodRuneCount + StyxWoW.Me.FrostRuneCount + StyxWoW.Me.UnholyRuneCount +
                       StyxWoW.Me.DeathRuneCount;
            }
        }

        internal static bool CanBloodTap
        {
            get
            {
                
                return TalentManager.HasTalent(13) && (Lua.GetRuneCooldown(1) <= 1 || Lua.GetRuneCooldown(2) <= 1 || Lua.GetRuneCooldown(3) <= 1 ||
                       Lua.GetRuneCooldown(4) <= 1 || Lua.GetRuneCooldown(5) <= 1 || Lua.GetRuneCooldown(6) <= 1);
                
                // This produced Lua errors..dont know why :/

                //int MAX_RUNES = 6;
                //for (int l = 0; l < MAX_RUNES; ++l)
                //{
                //    if ((Lua.GetRuneCooldown(l) <= 1) || (Lua.GetRuneCooldown(l + 1) <= 1))
                //    {
                //        return true;
                //    }
                //}
            }
        }

        private static readonly HashSet<int> Tier15Ids = new HashSet<int>
        {
            -724, // 535 ilvl
            1152, // 522 ilvl
            -701,  // 502 ilvl
        };

        internal static bool Has4PcTier15 { get { return Tier15Ids.Any(Item.Has4PcTeirBonus); } }

        internal static bool Has2PcTier15 { get { return Tier15Ids.Any(Item.Has2PcTeirBonus); } }

        #endregion Runes

        #region Diseases

        internal static bool HasBothDis
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura frostFever =
                    Me.CurrentTarget.GetAllAuras().FirstOrDefault(
                        u => u.CreatorGuid == Me.Guid && u.SpellId == 55095);
                WoWAura bloodPlague =
                    Me.CurrentTarget.GetAllAuras().FirstOrDefault(
                        u => u.CreatorGuid == Me.Guid && u.SpellId == 55078);
                return frostFever != null && frostFever.TimeLeft >= TimeSpan.FromSeconds(2) ||
                       (bloodPlague != null && bloodPlague.TimeLeft >= TimeSpan.FromSeconds(2));
            }
        }

        internal static bool HasffDis
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura frostFever = Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 55095);
                return frostFever != null && frostFever.TimeLeft >= TimeSpan.FromSeconds(2);
            }
        }

        public static WoWUnit BestUnholyFrenzyTarget
        {
            get
            {
                // If the player has a focus target set, use it instead.
                if (StyxWoW.Me.FocusedUnitGuid != 0 && StyxWoW.Me.FocusedUnit.IsAlive)
                    return StyxWoW.Me.FocusedUnit;

                return Me;
            }
        }

        internal static bool HasbpDis
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura bloodPlague = Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 55078);
                return bloodPlague != null && bloodPlague.TimeLeft >= TimeSpan.FromSeconds(2);
            }
        }

        internal static bool HasbpDisPL1
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura bloodPlague = Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 55078);
                return bloodPlague != null && bloodPlague.TimeLeft >= TimeSpan.FromSeconds(1);
            }
        }

        internal static bool HasbpDisPL2
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura bloodPlague = Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 55078);
                return bloodPlague != null && bloodPlague.TimeLeft >= TimeSpan.FromSeconds(3);
            }
        }

        public static bool CanCastPlagueLeech
        {
            get
            {
                if (!Me.GotTarget)
                    return false;

                WoWAura frostFever =
                    Me.CurrentTarget.GetAllAuras().FirstOrDefault(
                        u => u.CreatorGuid == Me.Guid && u.SpellId == 55095);
                WoWAura bloodPlague =
                    Me.CurrentTarget.GetAllAuras().FirstOrDefault(
                        u => u.CreatorGuid == Me.Guid && u.SpellId == 55078);

                // if there is 3 or less seconds left on the diseases and we have a fully depleted rune then return true.
                return frostFever != null && frostFever.TimeLeft < TimeSpan.FromSeconds(2) ||
                       bloodPlague != null && bloodPlague.TimeLeft < TimeSpan.FromSeconds(2);
            }
        }

        #endregion Diseases

        #region Misc

        internal static bool CanEmpowerRuneWeapon
        {
            get
            {
                return StyxWoW.Me.CurrentRunicPower < 26 && DeathRuneSlotsActive + FrostRuneSlotsActive == 0 && UnholyRuneSlotsActive < 2;
            }
        }

        internal static bool CanEmpowerRuneWeaponDW
        {
            get
            {
                return Lua.PlayerPower < 20 && Lua.GetSpellCooldown(49020) > 2 && Lua.GetSpellCooldown(49184) > 2 && Me.CurrentTarget.IsBoss() && Me.IsWithinMeleeRange;
            }
        }

        public static IEnumerable<WoWPlayer> GroupMembers
        {
            get { return ObjectManager.GetObjectsOfType<WoWPlayer>(true, true).Where(u => !u.IsDead && u.CanSelect && u.IsInMyPartyOrRaid); }
        }

        internal static bool IsWieldingTwoHandedWeapon
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
                    Logging.Write("IsWieldingBigWeapon : {0}", ex);
                }

                return false;
            }
        }

        internal static WoWUnit BestSrTarget
        {
            get
            {
                if (Me.CurrentTarget == null) return null;
                if (!Me.CurrentTarget.IsBoss()) return Me.CurrentTarget;
                var bestTarget = Unit.AttackableMeleeUnits.Where(a => a.IsAlive).OrderBy(a => a.HealthPercent < 35 && !a.HasAura(130735)).FirstOrDefault();
                return bestTarget;
            }
        }
        internal static Composite HandlePestilence()
        {
            return new PrioritySelector(
                new Decorator(ret => Me.CurrentTarget != null && Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 10).Count(x => !x.HasMyAura(55078)) >= PRSettings.Instance.DeathKnight.AoECount,
                    new PrioritySelector(
                        Spell.PreventDoubleCast("Pestilence", 2, ret => Me.CurrentTarget != null && Me.CurrentTarget.CachedHasAura(55078)))));
        }
        internal static Composite HandleTierSix()
        {
            return new Decorator(ret => HotKeyManager.IsSpecialKey || ( HotkeySettings.Instance.ModeChoice == Mode.Auto ||  HotkeySettings.Instance.ModeChoice == Mode.SemiAuto),
                new PrioritySelector(
                    Spell.Cast("Gorefriend's Grasp", ret => Me.CurrentTarget, ret => TalentManager.HasTalent(16)),
                    Spell.Cast("Remorseless Winter", ret => Me, ret => TalentManager.HasTalent(17)),
                    Spell.Cast("Desecrated Ground", ret => Me, ret => TalentManager.HasTalent(18))
                    )
                    );
        }
        #endregion Misc
    }
}