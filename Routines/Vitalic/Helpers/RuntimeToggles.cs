using System;
using Styx.WoWInternals;

namespace VitalicRotation.Helpers
{
    internal static class RuntimeToggles
    {
        // Legacy single flag (non persistant, comme v.zip)
        public static volatile bool PauseDamage; 

        // Mirror accessors to existing ToggleState / Settings for consistency (requested patch)
        public static bool BurstMode
        {
            get { return ToggleState.Burst; }
            set { ToggleState.Burst = value; }
        }
        public static bool LazyMode
        {
            get { return ToggleState.Lazy; }
            set { ToggleState.Lazy = value; }
        }
        public static bool AutoKidney
        {
            get { try { return VitalicRotation.Settings.VitalicSettings.Instance.AutoKidney; } catch { return false; } }
            set { try { VitalicRotation.Settings.VitalicSettings.Instance.AutoKidney = value; VitalicRotation.Settings.VitalicSettings.Instance.Save(); } catch { } }
        }
        public static bool PauseDamageMode
        {
            get { return ToggleState.PauseDamage; }
            set { ToggleState.PauseDamage = value; }
        }

        // Ne réinitialise plus Lazy; seul PauseDamage est forcé OFF (prévenir blocage offense inattendu)
        public static void ForceClearAll()
        {
            try
            {
                ToggleState.PauseDamage = false;   // enlève toute pause offensive
            }
            catch { }
        }

        // Contexte PvP fiable: arènes / champs de bataille
        public static bool IsPvpContext()
        {
            try
            {
                string t = Lua.GetReturnVal<string>("local a,t=IsInInstance(); return tostring(t or '')", 0);
                if (string.Equals(t, "arena", StringComparison.OrdinalIgnoreCase)) return true;
                if (string.Equals(t, "pvp", StringComparison.OrdinalIgnoreCase)) return true; // battleground
            }
            catch { }
            return false;
        }
    }
}

