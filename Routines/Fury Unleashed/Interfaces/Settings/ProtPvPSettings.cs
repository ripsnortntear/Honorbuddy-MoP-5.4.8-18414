using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuryUnleashed.Interfaces.Settings
{
    internal class PvPSettingsP : Styx.Helpers.Settings
    {
        public PvPSettingsP()
            : base(InternalSettings.SettingsPath + "_Prot_PvP.xml")
        {
        }
    }
}
