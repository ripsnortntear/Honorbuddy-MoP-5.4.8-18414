using System;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using Buddy_lib;

namespace Buddy_dump
{
	public class Dumper
	{
		private string strLogin;
		private string strPassword;
		private SessInfo Session;
		private byte _bot_type;
		private AClient HBClient;

		public bool IsLogged
		{
			get
			{
				if (Session != null && Session.SID != null && HBClient != null)
				{
					if (HBClient.State != CommunicationState.Created)
					{
						return HBClient.State == CommunicationState.Opened;
					}
					return true;
				}
				return false;
			}
		}

		public Dumper()
		{
			Session = null;
			HBClient = new AClient();
		}

		private d0 Request(byte b, object[] args)
		{
			d0 result = null;
			try
			{
				result = new d0 { Success = false };
				if (!IsLogged)
				{
					Logging.Msg("Communication fault, trying to reconnect...");
					Session = null;
					if (HBClient != null)
					{
						try { HBClient.Abort(); } catch { }
					}
					HBClient = new AClient();
					if (HBClient.State == CommunicationState.Created)
					{
						Login(strLogin, strPassword, _bot_type);
					}
					if (IsLogged)
					{
						Logging.Msg("Reconnected and re-authenticated successfully.");
					}
					else
					{
						Logging.Msg("Reconnection failed.");
					}
				}

				if (IsLogged)
				{
					result = HBClient.Do(b, args);
				}
			}
			catch (Exception ex)
			{
				Logging.Ex(string.Format("'{0}' in Request(), Type='{1}'", ex.Message, b));
			}
			return result;
		}

		public bool Login(string login, string password, byte botType)
		{
			bool result = false;
			_bot_type = botType;
			try
			{
				object[] args = new object[3]
				{
					login,
					DataLib.MD5(Encoding.ASCII.GetBytes(password ?? string.Empty)),
					botType
				};

				d0 response = HBClient.Do(1, args);
				if (response != null && response.Success && response.Data != null)
				{
					strLogin = login;
					strPassword = password;
					string sid = Encoding.ASCII.GetString(response.Data);
					Session = new SessInfo(sid, botType);
					result = true;
				}
			}
			catch (Exception ex)
			{
				Logging.Ex(string.Format("'{0}' in Login(), Login='{1}'", ex.Message, login));
			}
			return result;
		}

		public void Logout()
		{
			try
			{
				if (Session != null && HBClient != null)
				{
					object[] args = new object[1] { Session.SID };
					HBClient.Do(2, args);
				}
			}
			catch (Exception ex)
			{
				Logging.DbgMsg("Error during logout: " + ex.Message);
			}
			finally
			{
				Session = null;
			}
		}

		public byte[] DumpOffsets(string gameVersion, string clientVersion)
		{
			byte[] result = null;
			if (!Regex.IsMatch(gameVersion, "\\A\\d{4,5}\\z"))
			{
				throw new ApplicationException("Invalid GameVersion: " + gameVersion);
			}
			if (!Regex.IsMatch(clientVersion, "\\A\\d\\.\\d\\.\\d{1,4}\\.\\d{1,4}\\z"))
			{
				throw new ApplicationException("Invalid ClientVersion: " + clientVersion);
			}

			try
			{
				object[] args = new object[3] { Session.SID, gameVersion, clientVersion };
				d0 response = Request(4, args);
				if (response != null && response.Success && response.Data != null)
				{
					byte[] key = Encoding.ASCII.GetBytes(Session.SID);
					byte[] iv = Encoding.ASCII.GetBytes(Session.SID);
					Array.Reverse(iv);
					byte[] decrypted = DataLib.AES_Decrypt(response.Data, key, iv);
					result = DataLib.Decompress(decrypted);
				}
			}
			catch (Exception ex)
			{
				Logging.Ex(ex);
			}
			return result;
		}

		public byte[] DumpTilemap(string mapName)
		{
			byte[] result = null;
			try
			{
				object[] args = new object[2]
				{
					Session.SID,
					mapName + "_v2"
				};
				d0 response = Request(16, args);
				if (response != null && response.Success && response.Data != null)
				{
					result = response.Data;
				}
			}
			catch (Exception ex)
			{
				Logging.Ex(string.Format("'{0}' in DumpTilemap(), MapName='{1}'", ex.Message, mapName));
			}
			return result;
		}

		public byte[] DumpMesh(string mapName, int x = -1, int y = -1)
		{
			byte[] result = null;
			try
			{
				string text = mapName;
				if (x != -1 && y != -1)
				{
					text = string.Format("{0}_{1}_{2}", mapName, x, y);
				}

				object[] args = new object[2] { Session.SID, text };
				d0 response = Request(8, args);
				if (response != null && response.Success && response.Data != null)
				{
					result = DataLib.Decompress(response.Data);
					if (response.Body != DataLib.MD5(result))
					{
						Logging.DbgMsg("DumpMesh(): error while comparing hash");
						result = null;
					}
				}
			}
			catch (Exception ex)
			{
				Logging.Ex(string.Format("'{0}' in DumpMesh(), MapName='{1}', X='{2}', Y='{3}'", ex.Message, mapName, x, y));
			}
			return result;
		}

		public static void SaveToFile(byte[] data, string fileName)
		{
			if (data == null || string.IsNullOrEmpty(fileName))
				return;

			string dir = Path.GetDirectoryName(fileName);
			if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			File.WriteAllBytes(fileName, data);
		}
	}
}
