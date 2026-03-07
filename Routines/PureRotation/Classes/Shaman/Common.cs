using System;
using System.Linq;
using JetBrains.Annotations;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.Shaman
{
    [UsedImplicitly]
    internal static class Common
    {
        private static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        #region Dots

        internal static bool FlameTick
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura Flameshock = Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 8050);
                return Flameshock != null && Flameshock.TimeLeft >= TimeSpan.FromSeconds(2);
            }
        }

        #endregion Dots
    }
}