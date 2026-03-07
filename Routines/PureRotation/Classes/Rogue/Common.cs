using System;
using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.Rogue
{
    internal static class Common
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        #region StuffRogue

        internal static bool SliceAndDiceEnevenom
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura SilceAndDiceEne = Me.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 5171);
                return SilceAndDiceEne != null && SilceAndDiceEne.TimeLeft >= TimeSpan.FromSeconds(3);
            }
        }

        internal static bool RuptureGoingUp
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura Rupture = Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 1943);
                return Rupture != null && Rupture.TimeLeft >= TimeSpan.FromSeconds(3);
            }
        }

        internal static bool RevealingStrike
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                WoWAura Reveal = Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 84617);
                return Reveal != null && Reveal.TimeLeft >= TimeSpan.FromSeconds(2);
            }
        }

        #endregion StuffRogue
    }
}