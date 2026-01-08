#region Revision info
/*
 * $Author: millz $
 * $Date: 2014-01-24 15:50:15 +0000 (Fri, 24 Jan 2014) $
 * $ID: $
 * $Revision: 50 $
 * $URL: file:///mnt/ec2-fs11-data1/svn/iwantmovement/trunk/IWantMovement/Helper/Log.cs $
 * $LastChangedBy: millz $
 * $ChangesMade: $
 */
#endregion

using System.Windows.Media;
using Styx.Common;

namespace IWantMovement.Helper
{
    static class Log
    {

        public static void Info(string logText, params object[] args)
        {
            if (logText == null) return;
            Logging.Write(LogLevel.Normal, Colors.LawnGreen, "[IWM]: {0}", string.Format(logText, args));
        }

        public static void Warning(string logText, params object[] args)
        {
            if (logText == null) return;
            Logging.Write(LogLevel.Normal, Colors.Fuchsia, "[IWM Warning]: {0}", string.Format(logText, args));
        }

        public static void Debug(string logText, params object[] args)
        {
            if (logText == null) return;
            Logging.Write(LogLevel.Diagnostic, Colors.Aqua, "[IWM Debug]: {0}", string.Format(logText, args));
        }
    }
}
