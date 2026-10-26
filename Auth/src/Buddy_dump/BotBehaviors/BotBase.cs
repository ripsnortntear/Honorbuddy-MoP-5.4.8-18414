using System.IO;
using Buddy_lib;

namespace Buddy_dump.BotBehaviors
{
	public abstract class BotBase : IBotBehavior
	{
		protected string _meshPath;
		protected string _offsetsPath;
		protected string _mapListPath;
		protected string login;
		protected string pass;
		protected string gamever;
		protected string bver;
		protected static string bot_sig;
		protected Dumper dump;

		public static string GetBotSig => bot_sig;

		public static byte GetBotNum => BotType.GetNumBySig(bot_sig);

		public abstract bool GetDumpMapData { get; }

		public BotBase(string meshPath, string offsetsPath, string mapListPath)
		{
			dump = new Dumper();
			_meshPath = HBSettings.ResolvePath(meshPath);
			_offsetsPath = HBSettings.ResolvePath(offsetsPath);
			_mapListPath = HBSettings.ResolvePath(mapListPath);
		}

		public virtual bool Login(string login, string password)
		{
			return dump.Login(login, password, GetBotNum);
		}

		public virtual string DumpOffsets(string gameVersion, string clientVersion)
		{
			string resultPath = null;
			byte[] offsetsData = dump.DumpOffsets(gameVersion, clientVersion);
			if (offsetsData != null)
			{
				string fileName = string.Format("{0}_{1}.dat", GetBotSig, gameVersion);
				resultPath = Path.Combine(_offsetsPath, fileName);
				Dumper.SaveToFile(offsetsData, resultPath);
			}
			return resultPath;
		}

		public abstract void UpdateMapData();

		public void Logout()
		{
			dump.Logout();
		}
	}
}
