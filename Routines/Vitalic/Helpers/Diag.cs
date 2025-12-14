using System;
using System.Linq;
using System.Reflection;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Settings;

namespace VitalicRotation.Helpers
{
    internal static class Diag
    {
        // Always write line: concatenates args with spaces
        public static void Always(params object[] args)
        {
            try
            {
                string s = string.Join(" ", (args ?? new object[0]).Select(a => a == null ? string.Empty : a.ToString()));
                Logger.Write(s.Trim());
            }
            catch { }
        }

        // Diagnostic-only write
        public static void Log(params object[] args)
        {
            if (!VitalicSettings.Instance.DiagnosticMode)
                return;
            Always(args);
        }

        // Cast logger (by name)
        public static void Cast(string spell, WoWUnit unit)
        {
            try
            {
                var me = StyxWoW.Me;
                string targetName = (unit != null && unit.IsValid) ? (unit.Guid == (me != null ? me.Guid : 0UL) ? "Me" : unit.SafeName) : "?";

                if (!VitalicSettings.Instance.DiagnosticMode)
                {
                    Logger.Write(string.Format("[Casting] {0} on {1}", spell ?? "?", targetName));
                    return;
                }

                int cp = 0; try { cp = me.ComboPoints; } catch { }
                int energy = 0; try { energy = (int)me.CurrentEnergy; } catch { }
                double dist = 0; try { if (unit != null) dist = unit.Distance; } catch { }
                double gcd = 0; try { gcd = SpellManager.GlobalCooldownLeft.TotalSeconds; } catch { }

                string sdr = "?"; string sidr = "?";
                try { sdr = DRTracker.GetState(unit, DRTracker.DrCategory.Stun).ToString(); } catch { }
                try { sidr = DRTracker.GetState(unit, DRTracker.DrCategory.Silence).ToString(); } catch { }

                Logger.Write(string.Format(
                    "[Casting] {0} on {1} (CPs: {2}, Energy: {3}, Dist: {4:0.0}, GCD: {5:0.00}s), SDR: {6}, SiDR: {7}",
                    spell ?? "?", targetName, cp, energy, dist, gcd, sdr, sidr));
            }
            catch { }
        }

        // Cast logger (by id)
        public static void Cast(int spellId, WoWUnit unit)
        {
            string name = "?";
            try { name = WoWSpell.FromId(spellId).Name; } catch { }
            Cast(name, unit);
        }

        // Dump all settings as a JSON-like snapshot (no external deps)
        public static void DumpSettings(string header)
        {
            if (!VitalicSettings.Instance.DiagnosticMode) return;
            try
            {
                var s = VitalicSettings.Instance;
                var props = s.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.CanRead)
                    .ToArray();
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("{");
                for (int i = 0; i < props.Length; i++)
                {
                    var p = props[i];
                    object v = null; try { v = p.GetValue(s, null); } catch { }
                    string vs = v == null ? "null" : (v is string ? ("\"" + ((string)v).Replace("\"", "\\\"") + "\"") : v.ToString());
                    // normalize booleans casing
                    if (v is bool) vs = ((bool)v) ? "true" : "false";
                    sb.Append("\"" + p.Name + "\":" + vs);
                    if (i < props.Length - 1) sb.Append(",");
                }
                sb.Append("}");
                Logger.Write((header ?? "Settings") + " " + sb.ToString());
            }
            catch { }
        }
    }
}
