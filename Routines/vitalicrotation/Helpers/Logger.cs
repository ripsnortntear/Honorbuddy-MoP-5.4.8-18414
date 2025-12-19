using Styx.Common;
using System;
using System.Windows.Media;

namespace VitalicRotation.Helpers
{
    internal static class Logger
    {
        public static void Write(string message)
        {
            Logging.Write(LogLevel.Normal, Colors.White, "[Vitalic] " + message);
        }

        public static void Write(string message, Color color)
        {
            Logging.Write(LogLevel.Normal, color, "[Vitalic] " + message);
        }

        public static void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        public static void WriteException(Exception ex, string context = null)
        {
            if (ex == null) return;

            string header = string.IsNullOrEmpty(context)
                ? "[Vitalic] Exception"
                : "[Vitalic] Exception in " + context;

            Logging.Write(LogLevel.Normal, Colors.Red, header + ": " + ex.Message);
            Logging.Write(LogLevel.Normal, Colors.Red, ex.ToString());
        }
    }
}
