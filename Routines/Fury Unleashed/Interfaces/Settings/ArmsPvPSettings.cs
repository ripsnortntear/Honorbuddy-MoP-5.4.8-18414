using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuryUnleashed.Interfaces.Settings
{
    internal class PvPSettingsA : Styx.Helpers.Settings
    {
        public PvPSettingsA()
            : base(InternalSettings.SettingsPath + "_Arms_PvP.xml")
        {
        }
    }
}
