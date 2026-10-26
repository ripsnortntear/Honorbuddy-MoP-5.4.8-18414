using System;

namespace Buddy_lib
{
	public class SessInfo
	{
		private DateTime LastUse;

		public static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(5.0);

		public byte BotNum { get; private set; }

		public string BotSignature { get; private set; }

		public string SID { get; private set; }

		public bool IsExpired()
		{
			return (DateTime.UtcNow - LastUse) > DefaultTimeout;
		}

		public void Update()
		{
			LastUse = DateTime.UtcNow;
		}

		public SessInfo(string sid, byte bot)
		{
			SID = sid;
			BotNum = bot;
			BotSignature = BotType.GetSigByNum(bot);
			Update();
		}
	}
}
