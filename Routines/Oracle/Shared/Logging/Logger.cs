#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-17 12:14:14 +1000 (Tue, 17 Sep 2013) $
 * $ID$
 * $Revision: 227 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Logging/Logger.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Shared.Utilities;
using Oracle.UI.Settings;
using Styx.Common;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Oracle.Shared.Logging
{
    internal static class Logger
    {
        // Look at Logging.WriteToFileSync

        private static readonly CapacityQueue<string> LogQueue = new CapacityQueue<string>(5);

        public static void Output(string format, params object[] args)
        { Write(LogLevel.Normal, Colors.Aqua, format, args); }

        public static void Dispel(string format, params object[] args)
        {
            if (OracleSettings.Instance.EnableDispelLogging)
                Write(LogLevel.Normal, Colors.GreenYellow, format, args);
        }

        public static void Performance(string format, params object[] args)
        {
            if (OracleSettings.Instance.PerformanceLogging == LogCategory.Performance)
                Write(LogLevel.Normal, Colors.HotPink, format, args);
        }

        public static void PrioLogging(string format, params object[] args)
        {
            if (OracleSettings.Instance.EnablePriorityLogging)
                Write(LogLevel.Normal, Colors.Crimson, format, args);
        }

        public static void Warning(string format, params object[] args)
        {
            Write(LogLevel.Normal, Colors.LightPink, format, args);
        }

        private static void Write(LogLevel level, Color color, string format, params object[] args)
        {
            if (LogQueue.Contains(string.Format(format, args))) return;
            LogQueue.Enqueue(string.Format(format, args));

            Styx.Common.Logging.Write(level, color, string.Format("[{0}]: {1}", OracleRoutine.Instance.Name, format), args);
        }

        internal static void StatCounter()
        {
            try
            {
                var statcounterDate = DateTime.Now.DayOfYear.ToString(CultureInfo.InvariantCulture);
                if (!statcounterDate.Equals(OracleSettings.Instance.LastStatCounted))
                {
                    Parallel.Invoke(
                        () => new WebClient().DownloadData("http://c.statcounter.com/9163330/0/bc0014de/1/"),
                        () => Output(" StatCounter has been updated!"));
                    OracleSettings.Instance.LastStatCounted = statcounterDate;
                    OracleSettings.Instance.Save();
                }
            }
            catch { /* Catch all errors */ }
        }
    }
}