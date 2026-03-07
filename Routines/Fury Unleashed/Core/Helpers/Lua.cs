using System;
using System.Linq;
using System.Text;
using FuryUnleashed.Core.Utilities;
using FuryUnleashed.Interfaces.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Action = Styx.TreeSharp.Action;

// Credits for this code go out to the PureRotation Team!
namespace FuryUnleashed.Core.Helpers
{
    internal static class LuaClass
    {
        #region Start AutoAttack
        internal static Composite StartAutoAttack
        {
            get
            {
                return new Action(ret =>
                    {
                        if (!StyxWoW.Me.IsAutoAttacking)
                            Lua.DoString("StartAttack()");
                        return RunStatus.Failure;
                    });
            }
        }
        #endregion

        #region Run Macro text
        public static string RealLuaEscape(string luastring)
        {
            var bytes = Encoding.UTF8.GetBytes(luastring);
            return bytes.Aggregate(String.Empty, (current, b) => current + ("\\" + b));
        }

        public static Composite RunMacroText(string macro, CanRunDecoratorDelegate cond)
        {
            return new Decorator(
                       cond,
                       new PrioritySelector(
                           new Action(a => Lua.DoString("RunMacroText(\"" + RealLuaEscape(macro) + "\")"))));
        }
        #endregion

        #region PlayerPower Rage
        public static uint PlayerPower
        {
            get
            {
                try
                {
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Lua.GetReturnVal<uint>("return UnitPower(\"player\");", 0);
                    }
                }
                catch { Logger.DiagLogPu("FU: Lua Failed in PlayerPower"); return StyxWoW.Me.CurrentPower; }
            }
        }

        public static uint PlayerPowerMax
        {
            get
            {
                try
                {
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Lua.GetReturnVal<uint>("return UnitPowerMax(\"player\",1);", 0);
                    }
                }
                catch
                {
                    Logger.DiagLogPu("FU: Lua Failed in PlayerPowerMax"); return 0;
                }
            }
        }
        #endregion

        #region Disable Click-To-Move
        public static void DisableClickToMove()
        {
            if (InternalSettings.Instance.General.CheckDisableClickToMove)
            {
                Lua.DoString("SetCVar('autoInteract', '0')");
            }
        }

        public static void EnableClickToMove()
        {
            if (InternalSettings.Instance.General.CheckDisableClickToMove)
            {
                Lua.DoString("SetCVar('autoInteract', '1')");
            }
        }
        #endregion

        #region Other LUA
        internal static uint GetFps()
        {
            try
            {
                return (uint)Lua.GetReturnVal<float>("return GetFramerate()", 0);
            }
            catch (Exception ex)
            {
                Logger.DiagLogWh("FU: {0} - GetFps()", ex);
            }
            return 0;
        }
        #endregion
    }
}
