#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/WoWObjects/Racials.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Bots.DungeonBuddy.Helpers;
using JetBrains.Annotations;
using Oracle.Core.DataStores;
using Oracle.Core.Spells.Auras;
using Oracle.UI.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Linq;

namespace Oracle.Core.WoWObjects
{
    // Credits to PureRotation team!!..wait..erm..thats me! thanks Paul ;)

    public enum RacialUsage
    {
        Never,
        OnCooldown,
        OnBoss
    }

    [UsedImplicitly]
    internal static class Racials
    {
        public static WoWRace CurrentRace;

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static Composite UseRacials()
        {
            return new Decorator(ret => ((OracleSettings.Instance.UseRacials == RacialUsage.OnCooldown) ||
                                         (OracleRoutine.IsViable(StyxWoW.Me.CurrentTarget) && OracleSettings.Instance.UseRacials == RacialUsage.OnBoss &&
                                          StyxWoW.Me.CurrentTarget.IsBoss)),

                                 new Action(delegate
                                     {
                                         if (string.IsNullOrEmpty(CurrentRacialSpell()) || !RacialUsageSatisfied(CurrentRacialSpell())) return RunStatus.Failure;
                                         if (!SpellManager.CanCast(CurrentRacialSpell())) return RunStatus.Failure;
                                         SpellManager.Cast(CurrentRacialSpell());
                                         return RunStatus.Failure;
                                     }));
        }

        private static string CurrentRacialSpell()
        {
            switch (CurrentRace)
            {
                case WoWRace.BloodElf:
                    return "Arcane Torrent";
                case WoWRace.Draenei:
                    return "Gift of the Naaru";
                case WoWRace.Dwarf:
                    return "Stoneform";
                case WoWRace.Gnome:
                    return "Escape Artist";
                case WoWRace.Goblin:
                    return "Rocket Barrage";
                case WoWRace.Human:
                    return "Every Man for Himself";
                case WoWRace.NightElf:
                    return "Shadowmeld";
                case WoWRace.Orc:
                    return "Blood Fury";
                case WoWRace.Pandaren:
                    return null; // Fuck of Quaking Palm, you have no power here!
                case WoWRace.Tauren:
                    return "War Stomp";
                case WoWRace.Troll:
                    return "Berserking";
                case WoWRace.Undead:
                    return "Will of the Forsaken";
                case WoWRace.Worgen:
                    return "Darkflight";
                default:
                    return null;
            }
        }

        private static bool RacialUsageSatisfied(string racial)
        {
            if (racial != null)
            {
                switch (racial)
                {
                    case "Stoneform":
                        return StyxWoW.Me.GetAllAuras().Any(a => a.Spell.Mechanic == WoWSpellMechanic.Bleeding || a.Spell.DispelType == WoWDispelType.Disease || a.Spell.DispelType == WoWDispelType.Poison);
                    case "Escape Artist":
                        return StyxWoW.Me.Rooted;
                    case "Every Man for Himself":
                        return StyxWoW.Me.GetAllAuras().Any(a => a.Spell.Mechanic == WoWSpellMechanic.Fleeing || a.Spell.Mechanic == WoWSpellMechanic.Asleep || a.Spell.Mechanic == WoWSpellMechanic.Banished || a.Spell.Mechanic == WoWSpellMechanic.Charmed || a.Spell.Mechanic == WoWSpellMechanic.Frozen || a.Spell.Mechanic == WoWSpellMechanic.Horrified || a.Spell.Mechanic == WoWSpellMechanic.Incapacitated || a.Spell.Mechanic == WoWSpellMechanic.Polymorphed || a.Spell.Mechanic == WoWSpellMechanic.Rooted || a.Spell.Mechanic == WoWSpellMechanic.Sapped || a.Spell.Mechanic == WoWSpellMechanic.Stunned);
                    case "Shadowmeld":
                        return Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 15) >= 1 && StyxWoW.Me.HealthPercent < 80 && !StyxWoW.Me.IsMoving;
                    case "Gift of the Naaru":
                        return StyxWoW.Me.HealthPercent <= 75;
                    case "Darkflight":
                        return StyxWoW.Me.IsMoving;
                    case "Blood Fury":
                        return OracleRoutine.IsViable(StyxWoW.Me.CurrentTarget) && ((StyxWoW.Me.IsMelee() && StyxWoW.Me.CurrentTarget.IsWithinMeleeRange) || !StyxWoW.Me.IsMelee());
                    case "War Stomp":
                        return Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 8) >= 1;
                    case "Berserking":
                        return !StyxWoW.Me.HasAnyAura(HashSets.HeroismBuff) && ((OracleRoutine.IsViable(StyxWoW.Me.CurrentTarget) && Me.IsMelee() && Me.CurrentTarget.IsWithinMeleeRange) || !Me.IsMelee());
                    case "Will of the Forsaken":
                        return StyxWoW.Me.GetAllAuras().Any(a => a.Spell.Mechanic == WoWSpellMechanic.Fleeing || a.Spell.Mechanic == WoWSpellMechanic.Asleep || a.Spell.Mechanic == WoWSpellMechanic.Charmed);
                    case "Arcane Torrent":
                        return StyxWoW.Me.ManaPercent < 91 && StyxWoW.Me.Class != WoWClass.DeathKnight;
                    case "Rocket Barrage":
                        return true;

                    default:
                        return false;
                }
            }

            return false;
        }
    }
}