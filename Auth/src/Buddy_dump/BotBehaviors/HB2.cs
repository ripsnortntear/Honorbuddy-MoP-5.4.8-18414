using System;
using System.Collections.Generic;
using System.IO;
using Buddy_lib;

namespace Buddy_dump.BotBehaviors
{
	public class HB2 : BotBase
	{
		public new static string GetBotSig => "HB2";

		public override bool GetDumpMapData => true;

		public HB2(string meshPath, string offsetsPath, string mapListPath)
			: base(meshPath, offsetsPath, mapListPath)
		{
			BotBase.bot_sig = "HB2";
		}

		public override void UpdateMapData()
		{
			try
			{
				string mapListFile = Path.Combine(_mapListPath, GetBotSig + ".list");
				if (!File.Exists(mapListFile))
				{
					Logging.Msg(string.Format("Error: Maplist file '{0}' not found", mapListFile));
					return;
				}

				List<string> list = LoadMapList(mapListFile);
				int num = 0;
				DateTime now = DateTime.Now;

				if (SafeConsole.IsInteractiveConsole)
				{
					SafeConsole.Write("Processing maps: ");
					int cursorLeft = SafeConsole.CursorLeft;
					int cursorTop = SafeConsole.CursorTop;
					SafeConsole.Write(string.Format("{0,3}/{1}\r\n", 0, list.Count));

					for (int i = 0; i < list.Count; i++)
					{
						string mapName = list[i].Trim();
						if (string.IsNullOrEmpty(mapName))
							continue;

						byte[] rawMapData = dump.DumpTilemap(mapName);
						if (rawMapData != null)
						{
							Logging.DbgMsg(string.Format("Dumping map: {0}", mapName));
							byte[] decompressed = DataLib.Decompress(rawMapData);
							using (MemoryStream stream = new MemoryStream(decompressed))
							{
								Tilemap serverTilemap = Tilemap.Load(stream);
								if (serverTilemap == null)
								{
									Logging.Msg(string.Format("Error: incorrect tilemap for \"{0}\", skipped", mapName));
									continue;
								}

								string fileName = Path.Combine(_meshPath, GetBotSig, mapName, mapName + "_v2.tilemap");
								Tilemap localTilemap = Tilemap.Load(fileName);
								Tilemap diffTilemap = serverTilemap.Compare(localTilemap);
								if (localTilemap != null)
								{
									Logging.DbgMsg("Found local tilemap");
								}

								try
								{
									ProcessMesh(mapName, diffTilemap, serverTilemap);
								}
								catch (Exception ex)
								{
									Logging.Msg(string.Format("Error: map '{0}' skipped: {1}", mapName, ex.Message));
									continue;
								}

								num++;
								int currentLeft = SafeConsole.CursorLeft;
								int currentTop = SafeConsole.CursorTop;
								SafeConsole.SetCursorPosition(cursorLeft, cursorTop);
								SafeConsole.Write(string.Format("{0,3}", num));
								SafeConsole.SetCursorPosition(currentLeft, currentTop);

								serverTilemap.Save(fileName);
							}
						}
						else
						{
							Logging.Msg(string.Format("Error: tilemap for \"{0}\" not loaded", mapName));
						}
					}
				}
				else
				{
					// Headless / non-interactive progress reporting
					for (int i = 0; i < list.Count; i++)
					{
						string mapName = list[i].Trim();
						if (string.IsNullOrEmpty(mapName))
							continue;

						byte[] rawMapData = dump.DumpTilemap(mapName);
						if (rawMapData != null)
						{
							byte[] decompressed = DataLib.Decompress(rawMapData);
							using (MemoryStream stream = new MemoryStream(decompressed))
							{
								Tilemap serverTilemap = Tilemap.Load(stream);
								if (serverTilemap == null)
								{
									Logging.Msg(string.Format("[{0}/{1}] Error: incorrect tilemap for \"{2}\", skipped", i + 1, list.Count, mapName));
									continue;
								}

								string fileName = Path.Combine(_meshPath, GetBotSig, mapName, mapName + "_v2.tilemap");
								Tilemap localTilemap = Tilemap.Load(fileName);
								Tilemap diffTilemap = serverTilemap.Compare(localTilemap);

								try
								{
									ProcessMesh(mapName, diffTilemap, serverTilemap);
									num++;
									serverTilemap.Save(fileName);
									Logging.Msg(string.Format("[{0}/{1}] Processed map: {2}", i + 1, list.Count, mapName));
								}
								catch (Exception ex)
								{
									Logging.Msg(string.Format("[{0}/{1}] Error processing map '{2}': {3}", i + 1, list.Count, mapName, ex.Message));
								}
							}
						}
					}
				}

				Logging.Msg(string.Format("Meshes processed for {0} maps, skipped: {1}. Time elapsed: {2}",
					num, list.Count - num, (DateTime.Now - now).ToString("hh\\:mm\\:ss")));
			}
			catch (Exception ex)
			{
				Logging.Msg("An exception was raised while updating meshes. Check logfile.");
				Logging.Ex(ex);
			}
		}

		private static List<string> LoadMapList(string fileName)
		{
			List<string> list = new List<string>();
			using (StreamReader reader = new StreamReader(fileName))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (!string.IsNullOrWhiteSpace(line))
					{
						list.Add(line.Trim());
					}
				}
			}
			return list;
		}

		private void ProcessMesh(string mapName, Tilemap diffTilemap, Tilemap serverTilemap)
		{
			if (diffTilemap == null || !diffTilemap.IsTiled)
				return;

			for (int x = 0; x < 64; x++)
			{
				for (int y = 0; y < 64; y++)
				{
					if (diffTilemap[x, y])
					{
						byte[] meshData = dump.DumpMesh(mapName, x, y);
						if (meshData != null)
						{
							string fileName = string.Format("{0}_{1}_{2}.etm", mapName, x, y);
							string filePath = Path.Combine(_meshPath, GetBotSig, mapName, fileName);
							Dumper.SaveToFile(meshData, filePath);
						}
					}
				}
			}
		}
	}
}
