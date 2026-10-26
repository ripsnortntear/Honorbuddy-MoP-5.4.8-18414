using System;
using System.IO;
using Buddy_lib;

namespace Buddy_dump
{
	public class Tilemap
	{
		private static readonly int Signature = 1414091348;

		protected bool[,] _tiled = new bool[64, 64];

		protected readonly DateTime[,] Timestamps = new DateTime[64, 64];

		public int Count { get; private set; }

		public bool IsTiled { get; private set; }

		public bool this[int x, int y]
		{
			get
			{
				return _tiled[x, y];
			}
			set
			{
				if (!value && _tiled[x, y])
				{
					Count--;
				}
				if (value && !_tiled[x, y])
				{
					Count++;
				}
				_tiled[x, y] = value;
			}
		}

		private Tilemap()
		{
		}

		public Tilemap Compare(Tilemap cmpTilemap)
		{
			Tilemap tilemap = null;
			if (cmpTilemap != null)
			{
				tilemap = new Tilemap();
				tilemap.IsTiled = IsTiled;
				for (int i = 0; i < 64; i++)
				{
					for (int j = 0; j < 64; j++)
					{
						if (this[i, j] && !cmpTilemap[i, j])
						{
							tilemap[i, j] = true;
						}
						else if (this[i, j] && cmpTilemap[i, j] && Timestamps[i, j].CompareTo(cmpTilemap.Timestamps[i, j]) != 0)
						{
							tilemap[i, j] = true;
							tilemap.Timestamps[i, j] = Timestamps[i, j];
						}
					}
				}
			}
			else
			{
				tilemap = this;
			}
			return tilemap;
		}

		public static Tilemap Load(string fileName)
		{
			Tilemap result = null;
			try
			{
				if (!File.Exists(fileName))
					return null;

				using (FileStream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					byte[] buffer = DataLib.Decompress(inputStream);
					using (MemoryStream loadStream = new MemoryStream(buffer))
					{
						result = Load(loadStream);
					}
				}
			}
			catch (DirectoryNotFoundException) { }
			catch (FileNotFoundException) { }
			catch (Exception ex)
			{
				result = null;
				Logging.Msg("Tilemap loading error, check logfile");
				Logging.Ex(string.Format("\"{0}\" in Tilemap.Load(). FileName: {1}", ex.Message, fileName));
			}
			return result;
		}

		public static Tilemap Load(Stream loadStream)
		{
			Tilemap tilemap = null;
			try
			{
				using (BinaryReader binaryReader = new BinaryReader(loadStream))
				{
					int signature = binaryReader.ReadInt32();
					if (signature != Signature)
					{
						throw new Exception("Invalid Signature in Tilemap");
					}

					tilemap = new Tilemap();
					for (int i = 0; i < 64; i++)
					{
						ulong rowBits = binaryReader.ReadUInt64();
						for (int j = 0; j < 64; j++)
						{
							tilemap[i, j] = (rowBits & (1uL << j)) != 0;
							if (tilemap[i, j] && i != 0 && j != 0)
							{
								tilemap.IsTiled = true;
							}
						}
					}

					for (int k = 0; k < 64; k++)
					{
						for (int l = 0; l < 64; l++)
						{
							if (tilemap[k, l])
							{
								tilemap.Timestamps[k, l] = DateTime.FromFileTimeUtc(binaryReader.ReadInt64());
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				tilemap = null;
				Logging.Msg("Tilemap loading error, check logfile");
				Logging.Ex(ex);
			}
			return tilemap;
		}

		public void Save(string fileName)
		{
			try
			{
				string dir = Path.GetDirectoryName(fileName);
				if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}

				using (MemoryStream memoryStream = new MemoryStream())
				{
					Save(memoryStream);
					memoryStream.Seek(0L, SeekOrigin.Begin);
					byte[] compressed = DataLib.Compress(memoryStream);
					File.WriteAllBytes(fileName, compressed);
				}
			}
			catch (Exception ex)
			{
				Logging.Msg("Tilemap saving error, check logfile");
				Logging.Ex(string.Format("\"{0}\" in Tilemap.Save(). FileName: {1}", ex.Message, fileName));
			}
		}

		public void Save(Stream saveStream)
		{
			try
			{
				BinaryWriter binaryWriter = new BinaryWriter(saveStream);
				binaryWriter.Write(Signature);
				for (int i = 0; i < 64; i++)
				{
					ulong num = 0uL;
					for (int j = 0; j < 64; j++)
					{
						if (this[i, j])
						{
							num |= (1uL << j);
						}
					}
					binaryWriter.Write(num);
				}

				for (int k = 0; k < 64; k++)
				{
					for (int l = 0; l < 64; l++)
					{
						if (this[k, l])
						{
							binaryWriter.Write(Timestamps[k, l].ToFileTimeUtc());
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logging.Msg("Tilemap saving error, check logfile");
				Logging.Ex(ex);
			}
		}
	}
}
