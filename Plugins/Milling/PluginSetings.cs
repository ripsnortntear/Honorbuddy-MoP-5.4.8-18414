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

namespace Milling
{
    public class CRSettings : Settings
    {
        public static readonly CRSettings myPrefs = new CRSettings();

        public CRSettings()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Plugins/Milling/Herbkeuze.xml")))
        {
        }
        [Setting, DefaultValue("Peacebloom")]
        public string HerbToMill { get; set; }

        [Setting, DefaultValue(1)]
        public int Herbie { get; set; }

        [Setting, DefaultValue(3000)]
        public int wachtTijd { get; set; }
    }
}
