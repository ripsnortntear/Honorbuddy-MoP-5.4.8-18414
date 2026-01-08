using System.IO;
using Styx;
using Styx.Helpers;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.WoWInternals;

namespace MageHelper
{
    public class MageHelperSettings : Settings
    {
        public static readonly MageHelperSettings Instance = new MageHelperSettings();

        public MageHelperSettings()
            : base(Path.Combine(Settings.SettingsDirectory, string.Format(@"MageHelper/{0}-{1}.xml", StyxWoW.Me.Name, Lua.GetReturnVal<string>("return GetRealmName()", 0))))
        {
        }
        //BuddyManager\\{1}-{2}\\TaskList.xml", Settings.SettingsDirectory, StyxWoW.Me.Name, Lua.GetReturnVal<string>("return GetRealmName()", 0));

        [Setting, DefaultValue(true)]
        public bool UseBlink { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseSlowFall { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseInvis { get; set; }

        [Setting, DefaultValue(3)]
        public int InvisAdds { get; set; }

        [Setting, DefaultValue(20)]
        public int InvisHP { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseBlinkKillPOI { get; set; }

        [Setting, DefaultValue(true)]
        public bool AlwaysHaveIceBarrier { get; set; }

        }

    }


