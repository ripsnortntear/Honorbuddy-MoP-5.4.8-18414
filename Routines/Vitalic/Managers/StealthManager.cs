using System;
using Styx;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;

namespace VitalicRotation.Managers
{
    internal static class StealthManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static void Execute()
        {
            try
            {
                if (Me == null || !Me.IsValid || Me.IsDead) return;

                // v.zip : Stealth hors combat si option activée, rien d'autre ici.
                if (!VitalicSettings.Instance.AlwaysStealth) return;
                if (Me.Combat) return;
                if (Me.Mounted || Me.IsOnTransport || Me.IsFlying) return;

                if (!Me.HasAura("Stealth") && SpellBook.CanCast(SpellBook.Stealth))
                {
                    Logger.Write("[Stealth] Entering Stealth");
                    SpellBook.Cast(SpellBook.Stealth, Me);
                }
            }
            catch (Exception ex)
            {
                Logger.Write("[StealthManager] " + ex.Message);
            }
        }
    }
}
