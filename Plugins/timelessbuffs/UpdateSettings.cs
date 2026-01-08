using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Helpers;
using System.IO;
using Styx;
using Styx.Common;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.TreeSharp;

namespace TimelessBuffs
{
    public class UpdateSettingsTimelessBuffs : Settings
    {
        public static readonly UpdateSettingsTimelessBuffs myPrefs = new UpdateSettingsTimelessBuffs();

        public UpdateSettingsTimelessBuffs()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Plugins/Settings/Pasterke/Config/UpdateSettingsTimelessBuffs.xml")))
        {
        }
        [Setting]
        [DefaultValue(0)]
        [Category("Revision")]
        [DisplayName("Revision")]
        [Description("Revision")]
        public int Revision { get; set; }
    }
}
