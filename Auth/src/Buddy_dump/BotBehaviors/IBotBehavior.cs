namespace Buddy_dump.BotBehaviors
{
	public interface IBotBehavior
	{
		bool GetDumpMapData { get; }

		bool Login(string login, string password);

		string DumpOffsets(string gameVersion, string clientVersion);

		void UpdateMapData();

		void Logout();
	}
}
