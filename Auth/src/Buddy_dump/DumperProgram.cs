using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Buddy_dump.BotBehaviors;
using Buddy_lib;

namespace Buddy_dump
{
	internal class DumperProgram
	{
		private static readonly string SettingsFile = "Settings.xml";
		private static string login = "1234567890ABCDEFGHIJ";
		private static string pass = "AUTH_KEY";
		private static string gamever = "15595";
		private static string bver = "2.0.0.5872";
		private static string bot = "HB2";
		private static bool autoConfirmMapUpdate = false;
		private static bool nonInteractive = false;

		private static void Main(string[] args)
		{
			// Anchor current directory to assembly location
			Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			bool showHelp = false;
			for (int i = 0; i < args.Length; i++)
			{
				string arg = args[i].ToLowerInvariant();
				if (arg == "-help" || arg == "-h" || arg == "--help" || arg == "/?")
				{
					showHelp = true;
				}
				else if (arg == "-y" || arg == "--yes")
				{
					autoConfirmMapUpdate = true;
				}
				else if (arg == "-noninteractive" || arg == "--non-interactive")
				{
					nonInteractive = true;
				}
				else if (i + 1 < args.Length)
				{
					switch (arg)
					{
						case "-l":
							login = args[i + 1];
							i++;
							break;
						case "-p":
							pass = args[i + 1];
							i++;
							break;
						case "-gv":
							gamever = args[i + 1];
							i++;
							break;
						case "-bv":
							bver = args[i + 1];
							i++;
							break;
						case "-bot":
							bot = args[i + 1].ToUpperInvariant();
							i++;
							break;
					}
				}
			}

			string version = "1.1";
			try
			{
				AssemblyFileVersionAttribute attr = (AssemblyFileVersionAttribute)Assembly.GetEntryAssembly()
					.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];
				version = Regex.Match(attr.Version, "^\\d+\\.\\d+").Value;
			}
			catch { }

			SafeConsole.Clear();
			SafeConsole.WriteLine("Buddy Data Dumper v" + version + " [Modernized]", ConsoleColor.Cyan);
			SafeConsole.WriteLine("To get usage help run: Buddy_dump.exe -help\r\n");

			if (showHelp)
			{
				SafeConsole.WriteLine("Usage:");
				SafeConsole.WriteLine("  Buddy_dump.exe -l LOGIN [-p PASS] [-gv GAMEVER] [-bv BOTVER] [-bot BOTNAME] [-y] [-noninteractive]\r\n");
				SafeConsole.WriteLine("Options:");
				SafeConsole.WriteLine("  -l <key>           Login key (e.g. 20-digit auth key)");
				SafeConsole.WriteLine("  -p <password>      Password (optional, default: AUTH_KEY)");
				SafeConsole.WriteLine("  -gv <version>      Game version (default: 15595 / MoP: 18414)");
				SafeConsole.WriteLine("  -bv <version>      Bot version (default: 2.0.0.5872)");
				SafeConsole.WriteLine("  -bot <name>        Bot signature: HB2 (default) or DB");
				SafeConsole.WriteLine("  -y                 Automatically confirm map update prompt");
				SafeConsole.WriteLine("  -noninteractive    Exit without prompting for ENTER");
				SafeConsole.WriteLine();
				SafeConsole.WriteLine("Examples:");
				SafeConsole.WriteLine("  Buddy_dump.exe -l test -p test -gv 18414 -bv 2.0.0.5872 -bot HB2");
			}
			else
			{
				JustDoIt();
				if (!nonInteractive && SafeConsole.IsInteractiveConsole)
				{
					SafeConsole.WriteLine("\r\nPress ENTER to exit...");
					SafeConsole.ReadLine();
				}
			}
		}

		private static void JustDoIt()
		{
			try
			{
				HBSettings settings = new HBSettings(SettingsFile);
				string offsetsPath = settings.OffsetsPath;
				string meshesPath = settings.MeshesPath;
				string maplistPath = settings.MaplistPath;

				IBotBehavior behavior = Selector.GetBehavior(bot, meshesPath, offsetsPath, maplistPath);
				if (behavior == null)
				{
					Logging.Msg(string.Format("Bot type '{0}' is not supported.", bot));
					return;
				}

				if (!behavior.Login(login, pass))
				{
					Logging.Msg("Failed to authenticate with auth service.");
					return;
				}

				Logging.Msg("Successfully logged in.");
				Logging.Msg(string.Format("Querying offsets for Game '{0}' and Bot build '{1}'...", gamever, bver));

				try
				{
					string savedPath = behavior.DumpOffsets(gamever, bver);
					if (savedPath != null)
					{
						Logging.Msg("Offsets successfully saved to: " + savedPath);
					}
					else
					{
						Logging.Msg("Could not retrieve offsets from auth host.");
					}
				}
				catch (Exception ex)
				{
					Logging.Ex(ex);
				}

				if (behavior.GetDumpMapData)
				{
					bool updateMap = autoConfirmMapUpdate;
					if (!autoConfirmMapUpdate && SafeConsole.IsInteractiveConsole)
					{
						SafeConsole.Write("Update map data? (y/n): ", ConsoleColor.Yellow);
						while (true)
						{
							ConsoleKeyInfo? key = SafeConsole.ReadKey(true);
							if (key.HasValue)
							{
								if (key.Value.Key == ConsoleKey.Y)
								{
									SafeConsole.WriteLine("Yes");
									updateMap = true;
									break;
								}
								if (key.Value.Key == ConsoleKey.N)
								{
									SafeConsole.WriteLine("No");
									updateMap = false;
									break;
								}
							}
						}
					}

					if (updateMap)
					{
						behavior.UpdateMapData();
					}
				}

				behavior.Logout();
				Logging.Msg("Logged out successfully.");
			}
			catch (Exception ex)
			{
				Logging.Msg("Exception raised during dump operation. Check logfile for details.");
				Logging.Ex(ex);
			}
		}
	}
}
