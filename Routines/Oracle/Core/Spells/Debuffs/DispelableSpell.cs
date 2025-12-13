#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Spells/Debuffs/DispelableSpell.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.UI.Settings;
using System.IO;

namespace Oracle.Core.Spells.Debuffs
{
    public class DispelableSpell
    {
        private static DispelableSpell _instance;

        public SpellList SpellList;

        public DispelableSpell()
        {
            string file = Path.Combine(OracleSettings.GlobalSettingsPath, "DispelableSpells.xml");
            SpellList = new SpellList(file);
        }

        public static DispelableSpell Instance
        {
            get { return _instance ?? (_instance = new DispelableSpell()); }
            set { _instance = value; }
        }
    }
}