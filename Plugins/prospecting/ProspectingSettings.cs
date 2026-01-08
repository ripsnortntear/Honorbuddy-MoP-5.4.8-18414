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

namespace Prospecting //replace Pasteur with the namespace name of your main cs file.
{
    public class ProspectingSettings : Settings
    {
        public static readonly ProspectingSettings myPrefs = new ProspectingSettings();

        public ProspectingSettings()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Settings/{0}/{1}/Prospecting-Settings-{1}.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name)))
        {
        }

        //use to check string value. example : use spell if (ThatSpellToUse == "always")
        [Setting, DefaultValue(5)]
        public int Jump { get; set; }

         

    }
}