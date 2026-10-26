using System;

namespace Buddy_dump
{
	public static class SafeConsole
	{
		private static readonly object _consoleLock = new object();

		public static bool IsInteractiveConsole
		{
			get
			{
				try
				{
					return Environment.UserInteractive && !Console.IsInputRedirected && !Console.IsOutputRedirected;
				}
				catch
				{
					return false;
				}
			}
		}

		public static void Clear()
		{
			try
			{
				if (IsInteractiveConsole)
				{
					Console.Clear();
				}
			}
			catch { }
		}

		public static void Write(string text, ConsoleColor? color = null)
		{
			if (text == null)
				return;

			lock (_consoleLock)
			{
				try
				{
					if (color.HasValue && !Console.IsOutputRedirected)
					{
						ConsoleColor prev = Console.ForegroundColor;
						Console.ForegroundColor = color.Value;
						Console.Write(text);
						Console.ForegroundColor = prev;
					}
					else
					{
						Console.Write(text);
					}
				}
				catch { }
			}
		}

		public static void WriteLine(string text = "", ConsoleColor? color = null)
		{
			lock (_consoleLock)
			{
				try
				{
					if (color.HasValue && !Console.IsOutputRedirected)
					{
						ConsoleColor prev = Console.ForegroundColor;
						Console.ForegroundColor = color.Value;
						Console.WriteLine(text);
						Console.ForegroundColor = prev;
					}
					else
					{
						Console.WriteLine(text);
					}
				}
				catch { }
			}
		}

		public static void SetCursorPosition(int left, int top)
		{
			try
			{
				if (IsInteractiveConsole)
				{
					Console.SetCursorPosition(left, top);
				}
			}
			catch { }
		}

		public static int CursorLeft
		{
			get
			{
				try
				{
					if (IsInteractiveConsole)
						return Console.CursorLeft;
				}
				catch { }
				return 0;
			}
		}

		public static int CursorTop
		{
			get
			{
				try
				{
					if (IsInteractiveConsole)
						return Console.CursorTop;
				}
				catch { }
				return 0;
			}
		}

		public static ConsoleKeyInfo? ReadKey(bool intercept = true)
		{
			try
			{
				if (!IsInteractiveConsole)
					return null;

				return Console.ReadKey(intercept);
			}
			catch
			{
				return null;
			}
		}

		public static string ReadLine()
		{
			try
			{
				return Console.ReadLine();
			}
			catch
			{
				return null;
			}
		}
	}
}
