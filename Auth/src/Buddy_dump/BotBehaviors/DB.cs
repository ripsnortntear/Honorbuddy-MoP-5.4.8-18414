using System;

namespace Buddy_dump.BotBehaviors
{
	public class DB : BotBase
	{
		public new static string GetBotSig => "DB";

		public override bool GetDumpMapData => false;

		public DB(string meshPath, string offsetsPath, string mapListPath)
			: base(meshPath, offsetsPath, mapListPath)
		{
			BotBase.bot_sig = "DB";
		}

		public override void UpdateMapData()
		{
			throw new NotSupportedException("UpdateMapData() is not supported for this bot behavior");
		}
	}
}
