using System;

namespace HB_Auth
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
			catch
			{
				// Ignore invalid handle or redirected stream errors
			}
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
				catch
				{
					// Ignore invalid handle or writing to broken pipe/redirect
				}
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
				catch
				{
					// Ignore invalid handle or writing to broken pipe/redirect
				}
			}
		}

		public static bool IsKeyAvailable
		{
			get
			{
				try
				{
					if (!IsInteractiveConsole)
						return false;

					return Console.KeyAvailable;
				}
				catch
				{
					return false;
				}
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
	}
}
