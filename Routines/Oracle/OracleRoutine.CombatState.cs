#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/OracleRoutine.CombatState.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Styx;
using System;

namespace Oracle
{
    partial class OracleRoutine
    {
        public event EventHandler<CombatStateEventArgs> OnCombatStateChange;

        private static bool CurrentCombatState { get { return StyxWoW.Me.Combat; } }

        private static bool LastCombatState { get; set; }

        private void HandleCombatStateChange()
        {
            var current = CurrentCombatState;

            if (current != LastCombatState && OnCombatStateChange != null)
            {
                try
                {
                    OnCombatStateChange(this, new CombatStateEventArgs(current, LastCombatState));
                }
                catch
                {
                    // Eat any exceptions thrown.
                }
                LastCombatState = current;
            }
        }

        public class CombatStateEventArgs : EventArgs
        {
            public readonly bool CurrCombatState;
            public readonly bool PrevCombatState;

            public CombatStateEventArgs(bool currentCombatState, bool prevCombatState)
            {
                CurrCombatState = currentCombatState;
                PrevCombatState = prevCombatState;
            }
        }
    }
}