using System;
using System.IO;
using System.Xml.Serialization;

namespace Buddy_lib
{
	[Serializable]
	public class HBSettings
	{
		public string ServerIP { get; set; }

		public int AuthPort { get; set; }

		public int MeshPort { get; set; }

		public string OffsetsPath { get; set; }

		public string MeshesPath { get; set; }

		public string MaplistPath { get; set; }

		public HBSettings()
		{
			Default();
		}

		public HBSettings(string settingsFile)
		{
			Default();
			try
			{
				string fullPath = ResolvePath(settingsFile);
				if (File.Exists(fullPath))
				{
					Load(fullPath);
				}
				else
				{
					Save(fullPath);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("[HBSettings] Warning loading {0}: {1}. Using defaults.", settingsFile, ex.Message);
				Default();
			}
		}

		public void Default()
		{
			ServerIP = "127.0.0.1";
			AuthPort = 5031;
			MeshPort = 5034;
			OffsetsPath = "Offsets\\";
			MeshesPath = "Mesh\\";
			MaplistPath = "Maplists\\";
		}

		public void Load(string settingsFile)
		{
			string fullPath = ResolvePath(settingsFile);
			XmlSerializer serializer = new XmlSerializer(typeof(HBSettings));
			using (StreamReader reader = File.OpenText(fullPath))
			{
				HBSettings loaded = (HBSettings)serializer.Deserialize(reader);
				if (loaded != null)
				{
					if (!string.IsNullOrEmpty(loaded.ServerIP))
						ServerIP = loaded.ServerIP;
					if (loaded.AuthPort > 0)
						AuthPort = loaded.AuthPort;
					if (loaded.MeshPort > 0)
						MeshPort = loaded.MeshPort;
					if (!string.IsNullOrEmpty(loaded.OffsetsPath))
						OffsetsPath = loaded.OffsetsPath;
					if (!string.IsNullOrEmpty(loaded.MeshesPath))
						MeshesPath = loaded.MeshesPath;
					if (!string.IsNullOrEmpty(loaded.MaplistPath))
						MaplistPath = loaded.MaplistPath;
				}
			}
		}

		public void Save(string settingsFile)
		{
			try
			{
				string fullPath = ResolvePath(settingsFile);
				string dir = Path.GetDirectoryName(fullPath);
				if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				ns.Add("", "");
				XmlSerializer serializer = new XmlSerializer(typeof(HBSettings));
				using (FileStream stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					serializer.Serialize(stream, this, ns);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("[HBSettings] Warning saving {0}: {1}", settingsFile, ex.Message);
			}
		}

		public static string ResolvePath(string path)
		{
			if (string.IsNullOrEmpty(path))
				return AppDomain.CurrentDomain.BaseDirectory;

			if (Path.IsPathRooted(path))
				return path;

			return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
		}
	}
}
