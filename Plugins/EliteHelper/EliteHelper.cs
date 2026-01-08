// Plugin Developed by Naut

// HB Stuff
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Net;
using System.Globalization;
using System.Windows.Media;
using System.Media;
using System.Linq;

using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;
using Styx.Pathing;
using Styx.WoWInternals.World;

namespace EliteHelper
{
    public class EliteHelper : HBPlugin
    {
		private static LocalPlayer Me { get { return StyxWoW.Me; } }
		
		#region Default Overrides
        public override string Name { get { return "Elite Helper"; } }
        public override string Author { get { return "Naut"; } }
        public override Version Version { get { return new Version(1, 0, 0, 1); } }
        public override string ButtonText { get { return "Settings"; } }
        public override bool WantButton { get { return false; } }
		#endregion Default Overrides

        public static RarekillerMoPRares MoPRares = new RarekillerMoPRares();

        public override void Pulse()
        {
			// Vengeful Spirit Interupt
			if (MoPRares.Warscout.CastingSpellId == 138044)					
				{
				if (SpellManager.HasSpell("Shockwave") && SpellManager.CanCast("Shockwave"))
					{
					SpellManager.Cast("Shockwave");
					}	
				if (SpellManager.HasSpell("Storm Bolt") && !SpellManager.CanCast("Shockwave") && SpellManager.CanCast("Storm Bolt"))
					{
					SpellManager.Cast("Storm Bolt");
					}
				if (SpellManager.HasSpell("Pummel") && !SpellManager.CanCast("Shockwave") && !SpellManager.CanCast("Storm Bolt") && SpellManager.CanCast("Pummel"))
					{
					SpellManager.Cast("Pummel");
					}
				}
				
			// Thunder Crush Interupt	
				if (MoPRares.Warscout.CastingSpellId == 138043)
				{
					if (SpellManager.HasSpell("Shockwave") && SpellManager.CanCast("Shockwave"))
					{
						SpellManager.Cast("Shockwave");
					}	
					if (SpellManager.HasSpell("Storm Bolt") && !SpellManager.CanCast("Shockwave") && SpellManager.CanCast("Storm Bolt"))
					{
						SpellManager.Cast("Storm Bolt");
					}
					if (SpellManager.HasSpell("Pummel") && !SpellManager.CanCast("Shockwave") && !SpellManager.CanCast("Storm Bolt") && SpellManager.CanCast("Intimidating Shout"))
					{
						SpellManager.Cast("Pummel");
					}	
					if (SpellManager.HasSpell("Heroic Throw") && !SpellManager.CanCast("Pummel") && !SpellManager.CanCast("Shockwave") && !SpellManager.CanCast("Storm Bolt") && SpellManager.CanCast("Heroic Throw"))
					{
						SpellManager.Cast("Heroic Throw");
					}	
				}
        }
    }

}
