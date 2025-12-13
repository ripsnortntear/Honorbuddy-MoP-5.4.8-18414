#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-15 12:34:43 +1000 (Sun, 15 Sep 2013) $
 * $ID$
 * $Revision: 212 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/RotationBase.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.Spells.Auras;
using Oracle.Core.WoWObjects;
using Oracle.Healing;
using Oracle.UI.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using System.Windows.Forms;

namespace Oracle.Classes
{
    public enum HandleTankBuff { Never = 0, Always }

    public enum UnitBuffSelection { None = 0, Tank, SecondTank, You }

    public abstract partial class RotationBase
    {
        public abstract WoWSpec KeySpec { get; }

        public abstract Composite Medic { get; }

        public abstract string Name { get; }

        public abstract Composite PreCombat { get; }

        public abstract Composite PVERotation { get; }

        public abstract Composite PVPRotation { get; }

        internal static void DisplayMessage(string message, string title, bool showMessage)
        {
            if (showMessage) MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        protected static bool PvPSupport()
        {
            return (OracleSettings.Instance.PvPSupport && (Me.Mounted || Me.HasAnyAura("Food", "Drink")));
        }

        protected static WoWUnit HealTarget { get { return OracleHealTargeting.HealableUnit ?? StyxWoW.Me; } }

        protected static WoWUnit BeaconUnit { get { return OracleHealTargeting.BeaconUnit ?? Tank; } }

        protected static LocalPlayer Me { get { return StyxWoW.Me; } }

        protected static WoWUnit Pet { get { return StyxWoW.Me.Pet; } }

        protected static WoWUnit Tank { get { return OracleTanks.PrimaryTank; } }

        protected static WoWUnit SecondTank { get { return OracleTanks.AssistTank; } }
    }
}