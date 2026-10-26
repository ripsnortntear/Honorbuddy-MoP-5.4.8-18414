using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Buddy_lib;

namespace HB_Auth
{
	public class AuthImpl
	{
		private delegate d0 ReqHandler(SessInfo currentSess, object[] args);

		private static readonly AuthImpl instance = new AuthImpl();

		private readonly object _sessionLock = new object();
		private readonly Random rand = new Random();
		private readonly ReqHandler[] ReqHandlers = new ReqHandler[60];
		private readonly List<SessInfo> Sessions = new List<SessInfo>();

		private string strMeshesFolder;
		private string strOffsetsFolder;

		public static AuthImpl Instance => instance;

		public List<SessInfo> GetSessions
		{
			get
			{
				lock (_sessionLock)
				{
					return new List<SessInfo>(Sessions);
				}
			}
		}

		private AuthImpl()
		{
			LoadSettings("Settings.xml");
			SetHandlers();
		}

		public void ReloadSettings(string settingsFile = "Settings.xml")
		{
			LoadSettings(settingsFile);
		}

		private void LoadSettings(string settingsFile)
		{
			HBSettings settings = new HBSettings(settingsFile);
			strOffsetsFolder = HBSettings.ResolvePath(settings.OffsetsPath);
			strMeshesFolder = HBSettings.ResolvePath(settings.MeshesPath);
			Logging.Info(string.Format("Configured Offsets Directory: {0}", strOffsetsFolder));
			Logging.Info(string.Format("Configured Meshes Directory: {0}", strMeshesFolder));
		}

		private void SetHandlers()
		{
			ReqHandlers[1] = Login;
			ReqHandlers[2] = Logout;
			ReqHandlers[4] = SendOffsets;
			ReqHandlers[8] = SendMeshFile;
			ReqHandlers[16] = SendTileFile;
			ReqHandlers[18] = CheckTime;
			ReqHandlers[52] = CheckVersion;
			ReqHandlers[56] = FlyTo;
		}

		private SessInfo FindSession(string sid)
		{
			if (string.IsNullOrEmpty(sid))
				return null;

			lock (_sessionLock)
			{
				for (int i = 0; i < Sessions.Count; i++)
				{
					if (Sessions[i].SID == sid)
					{
						return Sessions[i];
					}
				}
			}
			return null;
		}

		public void RemoveExpiredSessions()
		{
			lock (_sessionLock)
			{
				for (int i = Sessions.Count - 1; i >= 0; i--)
				{
					if (Sessions[i].IsExpired())
					{
						Logging.Info(string.Format("Session expired: {0} ({1})", Sessions[i].SID, Sessions[i].BotSignature));
						Sessions.RemoveAt(i);
					}
				}
			}
		}

		public d0 Request(byte num, object[] args)
		{
			RemoveExpiredSessions();

			if (args == null || args.Length < 1)
			{
				return new d0 { Success = false };
			}

			string sid = args[0] as string;
			SessInfo sessInfo = FindSession(sid);
			if (sessInfo != null)
			{
				sessInfo.Update();
			}

			ReqHandler handler = null;
			if (num > 0 && num < ReqHandlers.Length)
			{
				handler = ReqHandlers[num];
			}

			if (handler != null)
			{
				return handler(sessInfo, args);
			}

			return new d0 { Success = true };
		}

		private d0 Login(SessInfo currentSess, object[] args)
		{
			d0 response = new d0();

			if (args.Length < 3)
			{
				response.Success = false;
				response.Body = "Authentication failed: invalid arguments";
				return response;
			}

			string user = args[0] as string ?? string.Empty;
			string pass = args[1] as string ?? string.Empty;
			byte bot = Convert.ToByte(args[2]);

			byte[] userPassBytes = Encoding.ASCII.GetBytes(user + pass);
			byte[] timeBytes = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
			byte[] randBytes;
			lock (rand)
			{
				randBytes = BitConverter.GetBytes(rand.Next());
			}

			byte[] payload = new byte[userPassBytes.Length + timeBytes.Length + randBytes.Length];
			Buffer.BlockCopy(userPassBytes, 0, payload, 0, userPassBytes.Length);
			Buffer.BlockCopy(timeBytes, 0, payload, userPassBytes.Length, timeBytes.Length);
			Buffer.BlockCopy(randBytes, 0, payload, userPassBytes.Length + timeBytes.Length, randBytes.Length);

			string sid = DataLib.MD5(payload);

			SessInfo newSession = new SessInfo(sid, bot);
			lock (_sessionLock)
			{
				Sessions.Add(newSession);
			}

			Logging.Info(string.Format("Bot logged in successfully: Type={0}, SID={1}", newSession.BotSignature, sid));

			response.Success = true;
			response.Body = string.Empty;
			response.Data = Encoding.ASCII.GetBytes(sid);
			return response;
		}

		private d0 Logout(SessInfo currentSess, object[] args)
		{
			if (currentSess != null)
			{
				lock (_sessionLock)
				{
					Sessions.Remove(currentSess);
				}
				Logging.Info(string.Format("Bot logged out: Type={0}, SID={1}", currentSess.BotSignature, currentSess.SID));
			}

			return new d0 { Success = true };
		}

		private d0 SendOffsets(SessInfo currentSess, object[] args)
		{
			d0 response = new d0();
			try
			{
				if (currentSess == null)
				{
					response.Success = false;
					return response;
				}

				string sid = (string)args[0];
				string buildStr = (string)args[1];
				uint build = Convert.ToUInt32(buildStr);

				string offsetFileName = string.Format("{0}_{1}.dat", currentSess.BotSignature, build);
				string offsetFilePath = Path.Combine(strOffsetsFolder, offsetFileName);

				if (!File.Exists(offsetFilePath))
				{
					Logging.Warn(string.Format("Offsets file not found for build {0}: {1}", build, offsetFilePath));
					response.Success = false;
					return response;
				}

				byte[] rawData = File.ReadAllBytes(offsetFilePath);
				byte[] compressed = DataLib.Compress(rawData);
				byte[] key = Encoding.ASCII.GetBytes(sid);
				byte[] iv = Encoding.ASCII.GetBytes(sid);
				Array.Reverse(iv);

				byte[] encrypted = DataLib.AES_Crypt(compressed, key, iv);

				Logging.Info(string.Format("Served offsets for {0} build {1} ({2} bytes)", currentSess.BotSignature, build, rawData.Length));

				response.Success = true;
				response.Data = encrypted;
				response.Body = DataLib.MD5(rawData);
				return response;
			}
			catch (Exception ex)
			{
				Logging.Ex("Error in SendOffsets: {0}", ex.Message);
				response.Success = false;
				return response;
			}
		}

		private d0 SendMeshFile(SessInfo currentSess, object[] args)
		{
			d0 response = new d0();
			try
			{
				if (currentSess == null)
				{
					response.Success = false;
					return response;
				}

				string meshName = (string)args[1];
				string mapFolder = Regex.Split(meshName, "(_\\d{1,2}_\\d{1,2})\\b")[0];
				string meshFilePath = Path.Combine(strMeshesFolder, currentSess.BotSignature, mapFolder, meshName + ".etm");

				if (!File.Exists(meshFilePath))
				{
					Logging.Warn(string.Format("Mesh file not found: {0}", meshFilePath));
					response.Success = false;
					return response;
				}

				byte[] rawData = File.ReadAllBytes(meshFilePath);
				byte[] compressed = DataLib.Compress(rawData);

				Logging.Info(string.Format("Served mesh: {0}/{1} ({2} bytes raw, {3} compressed)", mapFolder, meshName, rawData.Length, compressed.Length));

				response.Success = true;
				response.Data = compressed;
				response.Body = DataLib.MD5(rawData);
				return response;
			}
			catch (Exception ex)
			{
				Logging.Ex("Error in SendMeshFile: {0}", ex.Message);
				response.Success = false;
				return response;
			}
		}

		private d0 SendTileFile(SessInfo currentSess, object[] args)
		{
			d0 response = new d0();
			try
			{
				if (currentSess == null)
				{
					response.Success = false;
					return response;
				}

				string tileName = (string)args[1];
				string mapFolder = tileName.Length > 3 ? tileName.Substring(0, tileName.Length - 3) : tileName;
				string tileFilePath = Path.Combine(strMeshesFolder, currentSess.BotSignature, mapFolder, tileName + ".tilemap");

				if (!File.Exists(tileFilePath))
				{
					Logging.Warn(string.Format("Tilemap file not found: {0}", tileFilePath));
					response.Success = false;
					return response;
				}

				byte[] rawData = File.ReadAllBytes(tileFilePath);

				Logging.Info(string.Format("Served tilemap: {0}/{1} ({2} bytes)", mapFolder, tileName, rawData.Length));

				response.Success = true;
				response.Data = rawData;
				response.Body = DataLib.MD5(rawData);
				return response;
			}
			catch (Exception ex)
			{
				Logging.Ex("Error in SendTileFile: {0}", ex.Message);
				response.Success = false;
				return response;
			}
		}

		private d0 CheckTime(SessInfo currentSess, object[] args)
		{
			d0 response = new d0();
			if (currentSess != null)
			{
				response.Success = true;
				response.Body = "Honorbuddy Authenticated - Desconocido was here :)";
			}
			else
			{
				response.Success = false;
			}
			return response;
		}

		private d0 CheckVersion(SessInfo currentSess, object[] args)
		{
			d0 response = new d0();
			response.Success = (currentSess != null);
			return response;
		}

		private d0 FlyTo(SessInfo currentSess, object[] args)
		{
			d0 response = new d0();
			if (currentSess != null && args.Length > 3 && args[3] is byte[] buffer)
			{
				float x = 0f;
				float y = 0f;
				using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
				{
					x = reader.ReadSingle();
					y = reader.ReadSingle();
				}

				byte[] output = new byte[12];
				using (BinaryWriter writer = new BinaryWriter(new MemoryStream(output)))
				{
					writer.Write(1);
					writer.Write(x);
					writer.Write(y);
				}

				response.Success = true;
				response.Data = output;
			}
			else
			{
				response.Success = false;
			}
			return response;
		}
	}
}
