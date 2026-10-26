using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text.RegularExpressions;
using System.Threading;
using Buddy_lib;

namespace HB_Auth
{
	internal class AuthSvc
	{
		private static readonly ManualResetEventSlim _shutdownEvent = new ManualResetEventSlim(false);

		private static void Main(string[] args)
		{
			// Anchor current directory to assembly location
			Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

			HBSettings settings = new HBSettings("Settings.xml");

			string serverIp = string.IsNullOrEmpty(settings.ServerIP) ? "localhost" : settings.ServerIP;
			int authPort = settings.AuthPort > 0 ? settings.AuthPort : 5031;
			int meshPort = settings.MeshPort > 0 ? settings.MeshPort : 5034;

			Uri authBaseUri = new Uri(string.Format("net.tcp://{0}:{1}/", serverIp, authPort));
			Uri meshBaseUri = new Uri(string.Format("net.tcp://{0}:{1}/", serverIp, meshPort));

			ServiceHost authHost = new ServiceHost(typeof(HBChannel), authBaseUri);
			ServiceHost meshHost = new ServiceHost(typeof(HBChannel), meshBaseUri);

			// Register process exit and cancel key handlers
			AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
			{
				_shutdownEvent.Set();
			};

			try
			{
				Console.CancelKeyPress += (sender, eventArgs) =>
				{
					eventArgs.Cancel = true;
					_shutdownEvent.Set();
				};
			}
			catch
			{
				// In headless / non-interactive mode, CancelKeyPress registration can fail
			}

			try
			{
				NetTcpBinding binding = new NetTcpBinding(SecurityMode.None)
				{
					MaxReceivedMessageSize = 67108864, // 64 MB
					MaxBufferSize = 67108864,
					ReaderQuotas =
					{
						MaxArrayLength = 67108864,
						MaxBytesPerRead = 67108864,
						MaxStringContentLength = 67108864
					}
				};

				authHost.AddServiceEndpoint(typeof(IA), binding, "AuthService.svc");
				meshHost.AddServiceEndpoint(typeof(IA), binding, "AuthService.svc");

				ServiceMetadataBehavior mexAuth = new ServiceMetadataBehavior();
				ServiceMetadataBehavior mexMesh = new ServiceMetadataBehavior();
				authHost.Description.Behaviors.Add(mexAuth);
				meshHost.Description.Behaviors.Add(mexMesh);

				authHost.AddServiceEndpoint("IMetadataExchange", MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
				meshHost.AddServiceEndpoint("IMetadataExchange", MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

				authHost.Open();
				meshHost.Open();

				string version = "1.5.0";
				try
				{
					AssemblyFileVersionAttribute fileVersion = (AssemblyFileVersionAttribute)Assembly.GetEntryAssembly()
						.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0];
					version = Regex.Match(fileVersion.Version, "^\\d+\\.\\d+\\.\\d+").Value;
				}
				catch { }

				SafeConsole.Clear();
				Logging.Info(string.Format("Buddy Auth Emulator v{0} [Online]", version));
				Logging.Info(string.Format("Listening on: {0}AuthService.svc (Auth: {1}, Mesh: {2})", authBaseUri, authPort, meshPort));

				if (SafeConsole.IsInteractiveConsole)
				{
					SafeConsole.WriteLine();
					SafeConsole.WriteLine("Commands: [S] Show active sessions | [ENTER] Exit", ConsoleColor.Green);
					SafeConsole.WriteLine();

					while (!_shutdownEvent.IsSet)
					{
						if (SafeConsole.IsKeyAvailable)
						{
							ConsoleKeyInfo? keyInfo = SafeConsole.ReadKey(true);
							if (keyInfo.HasValue)
							{
								if (keyInfo.Value.Key == ConsoleKey.Enter)
								{
									_shutdownEvent.Set();
									break;
								}
								if (keyInfo.Value.Key == ConsoleKey.S)
								{
									AuthImpl.Instance.RemoveExpiredSessions();
									PrintStats(AuthImpl.Instance.GetSessions);
								}
							}
						}
						Thread.Sleep(100);
					}
				}
				else
				{
					Logging.Info("Running in headless / background service mode.");
					Logging.Info("Waiting for termination signal (ProcessExit / Service Stop)...");
					_shutdownEvent.Wait();
				}

				Logging.Info("Shutting down service hosts...");
				CloseHostSafe(authHost);
				CloseHostSafe(meshHost);
				Logging.Info("Buddy Auth stopped cleanly.");
			}
			catch (AddressAlreadyInUseException ex)
			{
				Logging.Ex(string.Format("CRITICAL: Port {0} or {1} is already in use by another application or previous Buddy_Auth instance.", authPort, meshPort));
				Logging.Ex("Details: " + ex.Message);
				AbortHostSafe(authHost);
				AbortHostSafe(meshHost);
			}
			catch (CommunicationException ex)
			{
				Logging.Ex("WCF Communication Error: " + ex.Message);
				AbortHostSafe(authHost);
				AbortHostSafe(meshHost);
			}
			catch (Exception ex)
			{
				Logging.Ex("Unhandled Exception in AuthSvc: " + ex);
				AbortHostSafe(authHost);
				AbortHostSafe(meshHost);
			}
		}

		private static void CloseHostSafe(ServiceHost host)
		{
			if (host == null)
				return;

			try
			{
				if (host.State == CommunicationState.Opened)
				{
					host.Close(TimeSpan.FromSeconds(3));
				}
				else
				{
					host.Abort();
				}
			}
			catch
			{
				host.Abort();
			}
		}

		private static void AbortHostSafe(ServiceHost host)
		{
			if (host == null)
				return;

			try
			{
				host.Abort();
			}
			catch { }
		}

		private static void PrintStats(List<SessInfo> sessions)
		{
			SafeConsole.WriteLine("--- Current Active Sessions ---", ConsoleColor.Yellow);
			Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			foreach (SessInfo session in sessions)
			{
				if (!dictionary.ContainsKey(session.BotSignature))
				{
					dictionary.Add(session.BotSignature, 1);
				}
				else
				{
					dictionary[session.BotSignature]++;
				}
			}

			if (dictionary.Count > 0)
			{
				foreach (KeyValuePair<string, int> item in dictionary)
				{
					SafeConsole.WriteLine(string.Format("  '{0}': {1} active session{2}", item.Key, item.Value, (item.Value > 1) ? "s" : ""));
				}
			}
			else
			{
				SafeConsole.WriteLine("  No active sessions.");
			}
			SafeConsole.WriteLine("-------------------------------", ConsoleColor.Yellow);
		}
	}
}
