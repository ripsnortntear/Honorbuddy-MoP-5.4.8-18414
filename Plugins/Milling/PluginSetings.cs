using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Helpers;
using System.IO;
using Styx;
using Styx.Common;
using System.ComponentModel;
using Styx.TreeSharp;

namespace Milling
{
    public class CRSettings : Settings
    {
        public static readonly CRSettings myPrefs = new CRSettings();

        public CRSettings()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Plugins/Milling/Herbkeuze.xml")))
        {
        }
        [Setting, Styx.Helpers.DefaultValue("Peacebloom")]
        public string HerbToMill { get; set; }

        [Setting, Styx.Helpers.DefaultValue(1)]
        public int Herbie { get; set; }

        [Setting, Styx.Helpers.DefaultValue(3000)]
        public int wachtTijd { get; set; }
    }
}
