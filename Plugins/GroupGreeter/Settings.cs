using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx;
using Styx.Helpers;
using Styx.Common;

namespace GroupGreet
{
    public class GroupGreeterSettings : Settings
    {
        public static readonly GroupGreeterSettings Instance = new GroupGreeterSettings();

        public GroupGreeterSettings()
            : base(System.IO.Path.Combine(Styx.Common.Utilities.AssemblyDirectory, "Settings", string.Format(@"GG-{0}-{1}.xml", StyxWoW.Me.Name, StyxWoW.Me.RealmName)))
        {
        }

        [Setting, DefaultValue(true)]
        public bool gg_enable { get; set; }

        [Setting, DefaultValue("hi")]
        public string greeting1 { get; set; }

        [Setting, DefaultValue("hey")]
        public string greeting2 { get; set; }

        [Setting, DefaultValue("greetings")]
        public string greeting3 { get; set; }

        [Setting, DefaultValue("hey guys")]
        public string greeting4 { get; set; }

        [Setting, DefaultValue("hello")]
        public string greeting5 { get; set; }

        [Setting, DefaultValue(false)]
        public bool tank_marking { get; set; }

        [Setting, DefaultValue(true)]
        public bool g1_enable { get; set; }

        [Setting, DefaultValue(true)]
        public bool g2_enable { get; set; }

        [Setting, DefaultValue(true)]
        public bool g3_enable { get; set; }

        [Setting, DefaultValue(true)]
        public bool g4_enable { get; set; }

        [Setting, DefaultValue(true)]
        public bool g5_enable { get; set; }
    }
}
