using System;
using System.IO;
using Buddy_lib;

namespace HB_Auth
{
	internal static class Logging
	{
		private static readonly object _fileLock = new object();
		private static string _logDirectory;
		private static string _logFilePath;
		private static DateTime _currentLogDate;

		static Logging()
		{
			try
			{
				_logDirectory = HBSettings.ResolvePath(Path.Combine("Logs", "Buddy_Auth"));
				if (!Directory.Exists(_logDirectory))
				{
					Directory.CreateDirectory(_logDirectory);
				}
				EnsureLogFile();
			}
			catch
			{
				// Fallback to local
			}
		}

		private static void EnsureLogFile()
		{
			DateTime today = DateTime.UtcNow.Date;
			if (_logFilePath == null || today != _currentLogDate)
			{
				_currentLogDate = today;
				_logFilePath = Path.Combine(_logDirectory, string.Format("Auth_{0:yyyy-MM-dd}.log", today));
			}
		}

		private static void WriteToFile(string formattedLine)
		{
			try
			{
				lock (_fileLock)
				{
					EnsureLogFile();
					if (!string.IsNullOrEmpty(_logFilePath))
					{
						File.AppendAllText(_logFilePath, formattedLine + Environment.NewLine);
					}
				}
			}
			catch
			{
				// Ignore file write failures in constrained environments
			}
		}

		public static void Info(string message)
		{
			string timestamp = DateTime.Now.ToString("HH:mm:ss");
			string line = string.Format("[{0}] [INFO] {1}", timestamp, message);
			SafeConsole.WriteLine(line);
			WriteToFile(line);
		}

		public static void Warn(string message)
		{
			string timestamp = DateTime.Now.ToString("HH:mm:ss");
			string line = string.Format("[{0}] [WARN] {1}", timestamp, message);
			SafeConsole.WriteLine(line, ConsoleColor.Yellow);
			WriteToFile(line);
		}

		public static void Ex(string ex)
		{
			string timestamp = DateTime.Now.ToString("HH:mm:ss");
			string line = string.Format("[{0}] [ERROR] {1}", timestamp, ex);
			SafeConsole.WriteLine(line, ConsoleColor.Red);
			WriteToFile(line);
		}

		public static void Ex(string exFormat, params object[] args)
		{
			try
			{
				Ex(string.Format(exFormat, args));
			}
			catch (Exception ex)
			{
				Ex(exFormat + " (Formatting error: " + ex.Message + ")");
			}
		}

		public static void Req(string message)
		{
			string timestamp = DateTime.Now.ToString("HH:mm:ss");
			string line = string.Format("[{0}] [REQ] {1}", timestamp, message);
			SafeConsole.WriteLine(line, ConsoleColor.Cyan);
			WriteToFile(line);
		}
	}
}
