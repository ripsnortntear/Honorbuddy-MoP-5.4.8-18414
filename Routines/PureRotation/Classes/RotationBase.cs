#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-08-30 20:20:43 +0200 (Fr, 30 Aug 2013) $
 * $ID$
 * $Revision: 1705 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/RotationBase.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

using PureRotation.Managers;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes
{
    public abstract class RotationBase
    {
        protected static LocalPlayer Me { get { return StyxWoW.Me; } }

        protected static WoWUnit Pet { get { return StyxWoW.Me.Pet; } }
        
        public abstract string Revision { get; }

        public abstract WoWSpec KeySpec { get; }

        public abstract string Name { get; }

        internal virtual string Help { get { return " No help available for this rotation."; } }

        internal virtual void OnPulse()
        {
        }

        public abstract Composite PVPRotation { get; }

        public abstract Composite PVERotation { get; }

        public abstract Composite Medic { get; }

        public abstract Composite PreCombat { get; }
    }
}