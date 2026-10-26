using System;
using System.IO;
using Buddy_lib;

namespace Buddy_dump
{
	internal static class Logging
	{
		private static readonly object _fileLock = new object();
		private static string _logFolder;
		private static string _logFilename;

		static Logging()
		{
			try
			{
				_logFolder = HBSettings.ResolvePath(Path.Combine("Logs", "Buddy_dump"));
				if (!Directory.Exists(_logFolder))
				{
					Directory.CreateDirectory(_logFolder);
				}
				_logFilename = Path.Combine(_logFolder, DateTime.Now.ToString("dd.MM_HH_mm_ss") + ".log");
			}
			catch
			{
				// Ignore initialization failure in constrained environment
			}
		}

		private static void ToLogfile(string msg)
		{
			try
			{
				lock (_fileLock)
				{
					if (!string.IsNullOrEmpty(_logFilename))
					{
						string line = string.Format("{0}: {1}", DateTime.Now.ToString("HH:mm:ss"), msg);
						File.AppendAllText(_logFilename, line + Environment.NewLine);
					}
				}
			}
			catch { }
		}

		public static void Ex(string msg)
		{
			SafeConsole.WriteLine(msg, ConsoleColor.Red);
			DbgMsg(msg);
		}

		public static void Msg(string m)
		{
			SafeConsole.WriteLine(m);
			ToLogfile(m);
		}

		public static void DbgMsg(string m)
		{
			ToLogfile(m);
		}

		public static void Ex(string exFormat, params object[] exParams)
		{
			try
			{
				Ex(string.Format(exFormat, exParams));
			}
			catch (Exception ex)
			{
				Ex(exFormat + " (Formatting error: " + ex.Message + ")");
			}
		}

		public static void Ex(Exception e)
		{
			if (e != null)
			{
				Ex(string.Format("An exception was raised: {0}{1}Stack: {2}", e.Message, Environment.NewLine, e.StackTrace));
			}
		}
	}
}
