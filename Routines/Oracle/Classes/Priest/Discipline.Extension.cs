#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Priest/Discipline.Extension.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Styx.TreeSharp;

namespace Oracle.Classes.Priest
{
    internal partial class Discipline
    {
        // This will act as an extension to the Discipline class for readability and will also give the user the option to custimize rotations.
        // Code can be added to this class without having to recreate/modify the Discipline.cs file.

        // PvP TO DO
        // =============================
        // cancel cast if target has donotdamage auras too
        // PW: S on mind bender if it is less than 100% hp
        // leap of faith on anyone that gets scatter shot,garrotte,charged,disarm
        //ah u cast psyfiend on a friendly target who is low HP
        // so it fears whoever it hitting on them
        // or u cast on self at x hp %




        #region Composites

        private static Composite SomeComposite()
        {
            return new PrioritySelector(
                //..some actions
                );
        }

        #endregion Composites
    }
}