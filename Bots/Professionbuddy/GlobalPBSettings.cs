using System.IO;
using Styx.Common;
using Styx.Helpers;

namespace HighVoltz.Professionbuddy
{
	public class GlobalPBSettings : Settings
	{

        public static readonly GlobalPBSettings Instance = new GlobalPBSettings();
        public GlobalPBSettings() : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Settings\{0}\{0}.xml", ProfessionbuddyBot.Instance.Name)))
        {
            Load();
        }

		[Setting, Styx.Helpers.DefaultValueAttribute(0)]
		public int CurrentRevision { get; set; }

		[Setting, Styx.Helpers.DefaultValueAttribute(0u)]
		public uint KnownSpellsPtr { get; set; }

		[Setting, Styx.Helpers.DefaultValueAttribute(null)]
		public string DataStoreTable { get; set; }

        [Setting, Styx.Helpers.DefaultValueAttribute("")]
        public string WowVersion { get; set; }
    }
}