using System.Threading;
using System.Text;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    public static class LuaHelper
    {
        public static void SleepForLag(int extraMs = 0)
        {
            try
            {
                StyxWoW.SleepForLagDuration();
                if (extraMs > 0) Thread.Sleep(extraMs);
            }
            catch
            {
                Thread.Sleep(150 + extraMs);
            }
        }

        public static void Do(string lua)
        {
            try
            {
                if (VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                {
                    // Journaliser quelques scripts sensibles (UseInventoryItem/SetCVar) en mode diag
                    if (!string.IsNullOrEmpty(lua))
                    {
                        if (lua.IndexOf("UseInventoryItem", System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                            lua.IndexOf("SetCVar", System.StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Logger.Write("[Diag][Lua.Do] {0}", lua.Length > 120 ? (lua.Substring(0, 120) + "...") : lua);
                        }
                    }
                }
                Lua.DoString(lua);
            }
            catch
            {
                // silence en production
            }
        }

        // LuaHelper.cs  — remplace seulement cette méthode

        public static T Get<T>(string lua, int retIndex = 0)
        {
            try
            {
                if (VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                {
                    if (!string.IsNullOrEmpty(lua) && lua.IndexOf("GetSpellCooldown", System.StringComparison.OrdinalIgnoreCase) >= 0)
                        Logger.Write("[Diag][Lua.Get] {0}", lua.Length > 120 ? (lua.Substring(0, 120) + "...") : lua);
                }
                return Lua.GetReturnVal<T>(lua, (uint)retIndex);
            }
            catch
            {
                return default(T);
            }
        }

        public static void SetVar(string name, string valueLiteral)
        {
            Do("_G['" + name + "'] = " + valueLiteral);
        }

        public static string GetVar(string name, string defaultLiteral = "nil")
        {
            string lua = "local v=_G['" + name + "']; if v==nil then return " + defaultLiteral + " else return tostring(v) end";
            return Get<string>(lua, 0);
        }

        public static void RegisterSlash(string commandWithoutSlash, string functionName, string functionBodyLua)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SLASH_" + functionName + "1 = '/" + commandWithoutSlash + "';");
            sb.AppendLine("SlashCmdList['" + functionName + "'] = function(msg)");
            sb.AppendLine(functionBodyLua);
            sb.AppendLine("end");
            Do(sb.ToString());
        }

        public static void UnregisterSlash(string functionName)
        {
            Do("SlashCmdList['" + functionName + "'] = nil\nSLASH_" + functionName + "1 = nil");
        }

        public static void CastSpell(string spellName)
        {
            if (string.IsNullOrEmpty(spellName)) return;
            string safe = spellName.Replace(@"\", @"\\").Replace("\"", "\\\"");
            Do("CastSpellByName(\"" + safe + "\")");
        }

        public static void CastSpell(int spellId)
        {
            Do("CastSpellByID(" + spellId + ")");
        }

        public static void CastSpell(string spellName, string unitToken)
        {
            if (string.IsNullOrEmpty(spellName) || string.IsNullOrEmpty(unitToken)) return;
            string safe = spellName.Replace(@"\", @"\\").Replace("\"", "\\\"");
            Do("CastSpellByName(\"" + safe + "\", \"" + unitToken + "\")");
        }

        public static void UseInventoryItem(int slot)
        {
            Do("UseInventoryItem(" + slot + ")");
        }

        public static int GetLatencyMs()
        {
            const string lua = @"
local _,_,home,world = GetNetStats()
if home and world then
  if home > world then return home else return world end
else
  return 150
end";
            return Get<int>(lua, 0);
        }

        public static void CancelQueuedSpellAndStopAttack()
        {
            try { Lua.DoString("SpellCancelQueuedSpell(); StopAttack();"); } catch { }
        }
    }

    public static class LuaHelpers
    {
        public static WoWUnit GetUnitById(string unitId)
        {
            if (string.IsNullOrEmpty(unitId)) return null;

            try
            {
                // On lit le GUID puis on retrouve l'unité côté ObjectManager
                string lua = "local u='" + unitId + "'; if not UnitExists(u) then return '0' end return UnitGUID(u) or '0'";
                string guidStr = LuaHelper.Get<string>(lua, 0);
                if (guidStr == "0" || string.IsNullOrEmpty(guidStr)) return null;

                // Utiliser l'API directe si dispo
                try
                {
                    ulong guid;
                    if (guidStr.StartsWith("0x"))
                    {
                        if (ulong.TryParse(guidStr.Substring(2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out guid))
                            return ObjectManager.GetObjectByGuid<WoWUnit>(guid);
                    }
                    else if (ulong.TryParse(guidStr, out guid))
                    {
                        return ObjectManager.GetObjectByGuid<WoWUnit>(guid);
                    }
                }
                catch { }

                // Fallback: boucle
                var list = ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
                for (int i = 0; i < list.Count; i++)
                {
                    var u = list[i];
                    if (u == null) continue;
                    try { if (u.Guid.ToString("X").Equals(guidStr, System.StringComparison.OrdinalIgnoreCase)) return u; } catch { }
                }
            }
            catch { }
            return null;
        }
    }
}
