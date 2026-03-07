#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-22 17:17:33 +0200 (So, 22 Sep 2013) $
 * $ID$
 * $Revision: 1734 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Helpers/Logger.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using Styx;
using Styx.Common.Helpers;
using System.Windows.Media;
using Styx.Common;

namespace PureRotation.Helpers
{
    internal static class Logger
    {

        private static readonly CapacityQueue<string> LogQueue = new CapacityQueue<string>(5);

        static void Output(LogLevel level, Color color, string format, params object[] args)
        {
            if (LogQueue.Contains(string.Format(format, args))) return;
            LogQueue.Enqueue(string.Format(format, args));

            Logging.Write(level, color, string.Format("[{0}]: {1}", PureRotationRoutine.Instance.Name, format), args);
        }

        public static void InitLog(string format, params object[] args)
        { Output(LogLevel.Normal, Colors.Gainsboro, format, args); }

        public static void ItemLog(string format, params object[] args)
        { if (Styx.Helpers.GlobalSettings.Instance.LogLevel == LogLevel.Diagnostic) Output(LogLevel.Diagnostic, Colors.LawnGreen, format, args); }

        public static void FailLog(string format, params object[] args)
        { Output(LogLevel.Normal, Colors.GreenYellow, format, args); }

        public static void InfoLog(string format, params object[] args)
        { Output(LogLevel.Normal, Colors.CornflowerBlue, format, args); }

        public static void CombatLog(string format, params object[] args)
        { if (Styx.Helpers.GlobalSettings.Instance.LogLevel == LogLevel.Diagnostic) Output(LogLevel.Diagnostic, Colors.Pink, format, args); }

        public static void DebugLog(string format, params object[] args)
        {if(Styx.Helpers.GlobalSettings.Instance.LogLevel==LogLevel.Diagnostic) Output(LogLevel.Diagnostic, Colors.DarkGoldenrod, format, args); }

        private static WaitTimer logTimer = new WaitTimer(TimeSpan.FromMilliseconds(1000));

        //public static void LogStatistics()
        //{
        //    try
        //    {
        //        if (logTimer.IsFinished)
        //        {
        //            Logger.DebugLog("************************ START Personal Statistics  ************************");
        //            Logger.DebugLog("HealthPercent {0}, CurrentEnergy {1}, CurrentChi {2}, CurrentTarget {3}", StyxWoW.Me.HealthPercent, StyxWoW.Me.CurrentEnergy, StyxWoW.Me.CurrentChi, StyxWoW.Me.CurrentTarget.Name ?? "<none>");
        //            Lua.PopulateSecondryStats();
        //            foreach (var ar in StyxWoW.Me.ActiveAuras)
        //            {

        //                Logger.DebugLog("Name: {0} - ID: {1} - StackCount {2} - TimeLeft {3} - IsActive {4} - IsHarmful {5}", ar.Value.Name, ar.Value.SpellId, ar.Value.StackCount, ar.Value.TimeLeft, ar.Value.IsActive, ar.Value.IsHarmful);
        //            }
        //            Logger.DebugLog("************************ END Personal Statistics  ************************");
        //            logTimer.Reset();
        //        }

        //    }
        //    catch (Exception ex) 
        //    {
        //        Logger.DebugLog("Exception thrown in LogStatistics: {0}", ex);
        //    }

        //}
    }
}