#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Managers/PetManager.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info


using Bots.BGBuddy.Helpers;
using JetBrains.Annotations;
using Styx;
using Styx.Common.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Core.Managers
{
    // Raped from Singular..good job guys.

    [UsedImplicitly]
    internal class PetManager
    {
        #region Nested type: Selection

        internal delegate T Selection<out T>(object context);

        #endregion Nested type: Selection

        public static readonly WaitTimer PetSummonAfterDismountTimer = new WaitTimer(TimeSpan.FromSeconds(2));
        private static readonly List<WoWPetSpell> PetSpells = new List<WoWPetSpell>();
        private static ulong _petGuid;
        private static bool _wasMounted;

        static PetManager()
        {
            // Empty
        }

        public static bool HavePet { get { return StyxWoW.Me.GotAlivePet; } }

        public static bool CanCastPetAction(string action)
        {
            WoWPetSpell petAction = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (petAction == null || petAction.Spell == null)
            {
                return false;
            }

            return !petAction.Spell.Cooldown;
        }

        public static Composite CastAction(string action, Selection<WoWUnit> onUnit = null)
        {
            return new Action(ret =>
                {
                    if (!CanCastPetAction(action))
                        return RunStatus.Failure;

                    WoWUnit target;
                    if (onUnit == null)
                        target = StyxWoW.Me.CurrentTarget;
                    else
                        target = onUnit(ret);

                    if (!OracleRoutine.IsViable(target))
                        return RunStatus.Failure;

                    if (target.Guid == StyxWoW.Me.CurrentTargetGuid)
                        CastPetAction(action);
                    else
                        CastPetAction(action, target);

                    return RunStatus.Failure; // Im returnning a Failure I want it to continue down the tree.
                });
        }

        public static void CastPetAction(string action)
        {
            WoWPetSpell spell = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Logger.Write(string.Format("[Pet] Casting {0}", action));
            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);
        }

        public static void CastPetAction(string action, WoWUnit on)
        {
            // target is currenttarget, then use simplified version (to avoid setfocus/setfocus
            if (on == StyxWoW.Me)
            {
                CastPetAction(action);
                return;
            }

            // target is currenttarget, then use simplified version (to avoid setfocus/setfocus
            if (on == StyxWoW.Me.CurrentTarget)
            {
                CastPetAction(action);
                return;
            }

            WoWPetSpell spell = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Logger.Write(string.Format("[Pet] Casting {0} on {1}", action, on.SafeName));
            WoWUnit save = StyxWoW.Me.FocusedUnit;
            StyxWoW.Me.SetFocus(on);
            Lua.DoString("CastPetAction({0}, 'focus')", spell.ActionBarIndex + 1);
            StyxWoW.Me.SetFocus(save == null ? 0 : save.Guid);
        }

        internal static void Pulse()
        {
            if (StyxWoW.Me.Pet != null)
            {
                if (_petGuid != StyxWoW.Me.Pet.Guid)
                {
                    // clear any existing spells
                    PetSpells.Clear();

                    // only load spells if we have one that is non-null
                    // .. as initial load happens before Me.PetSpells is initialized and we were saving 'null' spells
                    if (StyxWoW.Me.PetSpells.Any(s => s.Spell != null))
                    {
                        // Cache the list. yea yea, we should just copy it, but I'd rather have shallow copies of each object, rather than a copy of the list.
                        PetSpells.AddRange(StyxWoW.Me.PetSpells);
                        PetSummonAfterDismountTimer.Reset();
                        _petGuid = StyxWoW.Me.Pet.Guid;

                        Logger.WriteDebug("---PetSpells Loaded---");
                        foreach (var sp in PetSpells)
                        {
                            if (sp.Spell == null)
                                Logger.WriteDebug("   {0} spell={1}  Action={0}", sp.ActionBarIndex, sp.ToString(), sp.Action.ToString());
                            else
                                Logger.WriteDebug("   {0} spell={1} #{2}", sp.ActionBarIndex, sp.ToString(), sp.Spell.Id);
                        }
                        Logger.WriteDebug(" ");
                    }
                }
            }

            if (!StyxWoW.Me.GotAlivePet)
            {
                PetSpells.Clear();
            }
        }
    }
}