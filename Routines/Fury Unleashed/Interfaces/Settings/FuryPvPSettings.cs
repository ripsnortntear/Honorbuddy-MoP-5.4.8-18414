using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuryUnleashed.Interfaces.Settings
{
    internal class PvPSettingsF : Styx.Helpers.Settings
    {
        public PvPSettingsF()
            : base(InternalSettings.SettingsPath + "_Fury_PvP.xml")
        {
        }
    }
}
