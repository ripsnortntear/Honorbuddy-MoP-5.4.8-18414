//=================================================================
//
//				      EliteHelper - Plugin
//						Autor: Naut
//			Honorbuddy Plugin - www.thebuddyforum.com
//
//==================================================================
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Windows.Media;

using Styx;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Pathing;
using Styx.Helpers;
using Styx.CommonBot;


namespace EliteHelper
{
    public class RarekillerMoPRares
    {
        private static LocalPlayer Me = StyxWoW.Me;

        #region Units
        public WoWUnit Warscout
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => (
                    o.Entry == 69768) && o.Distance < 100 && !o.IsDead).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

		#endregion

       
    }
}
