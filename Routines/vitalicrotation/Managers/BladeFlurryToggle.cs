using System;
using Styx.WoWInternals;
using VitalicRotation.Settings;
using VitalicRotation.Helpers;

namespace VitalicRotation.Managers
{
    internal static class BladeFlurryToggle
    {
        private static DateTime _nextToggleUtc = DateTime.MinValue;
        private const int ToggleThrottleMs = 1200; // fixed legacy value

        public static bool IsActive()
        {
            try
            {
                return Lua.GetReturnVal<bool>("return (UnitBuff('player','Blade Flurry') ~= nil)", 0);
            }
            catch
            {
                return false;
            }
        }

        public static bool CanToggle()
        {
            return DateTime.UtcNow >= _nextToggleUtc;
        }

        public static void SetDesired(bool wantOn)
        {
            if (!CanToggle()) return;

            var isOn = IsActive();
            if (wantOn == isOn) return;

            try
            {
                // Activer : cast le sort; Désactiver : cancel aura
                if (wantOn)
                {
                    if (SpellBook.CanCast(SpellBook.BladeFlurry))
                    {
                        if (SpellBook.Cast(SpellBook.BladeFlurry))
                        {
                            Logger.Write("[BladeFlurryToggle] Blade Flurry ON");
                        }
                    }
                }
                else
                {
                    // MoP: re-cast BF to toggle it off
                    if (SpellBook.CanCast(SpellBook.BladeFlurry))
                    {
                        if (SpellBook.Cast(SpellBook.BladeFlurry))
                        {
                            Logger.Write("[BladeFlurryToggle] Blade Flurry OFF");
                        }
                    }
                    else
                    {
                        // Fallback: cancel via Lua
                        try
                        {
                            Lua.DoString("CancelUnitBuff('player','Blade Flurry')");
                            Logger.Write("[BladeFlurryToggle] Blade Flurry OFF (Lua)");
                        }
                        catch { }
                    }
                }

                _nextToggleUtc = DateTime.UtcNow.AddMilliseconds(ToggleThrottleMs);
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex, "BladeFlurryToggle.SetDesired");
            }
        }
    }
}