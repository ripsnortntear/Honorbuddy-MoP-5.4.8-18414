using System;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;

namespace VitalicRotation.Managers
{
    internal static class ShroudManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static DateTime _lastCast = DateTime.MinValue;

        private static DateTime _lastShroud = DateTime.MinValue;
        private const int ShroudIcdMs = 15000;

        private static bool EnoughEnemiesNearMe(float radius, int minCount)
        {
            int c = 0;
            try
            {
                var list = ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
                foreach (var u in list)
                {
                    if (u != null && u.IsAlive && u.Attackable)
                    {
                        bool hostile = true; try { hostile = u.IsHostile; } catch { hostile = true; }
                        if (hostile && u.Distance <= radius) c++;
                    }
                }
            }
            catch { }
            return c >= minCount;
        }

        public static bool TryCastShroudIfUseful()
        {
            try
            {
                if (!VitalicSettings.Instance.AutoShroud) return false;
                if ((DateTime.UtcNow - _lastShroud).TotalMilliseconds < ShroudIcdMs) return false;
                if (Me == null || !Me.IsAlive) return false;
                if (Me.Combat) return false;
                try { if (Me.MovementInfo == null || Me.MovementInfo.ForwardSpeed < 2.0f) return false; } catch { }

                if (!EnoughEnemiesNearMe(15f, 4)) return false;

                if (SpellBook.CanCast(SpellBook.ShroudOfConcealment, Me) && SpellBook.Cast(SpellBook.ShroudOfConcealment, Me))
                {
                    _lastShroud = DateTime.UtcNow;
                    try { AudioBus.PlayEvent(); } catch { }
                    Logger.Write("Casted Shroud of Concealment");
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static void Execute()
        {
            TryCastShroudIfUseful();
        }
    }
}
