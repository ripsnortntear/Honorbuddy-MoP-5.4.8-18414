using System;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;
using Styx.CommonBot.POI;

namespace BurstOSpeed 
{
    public class BurstOSpeed : HBPlugin 
    {

        public static LocalPlayer Me = StyxWoW.Me;

        public override string Name 
        { 
            get { return "Burst'O'Speed"; }
        }

        public override string Author 
        { 
            get { return "Proto"; }
        }

        public override Version Version 
        {
            get { return new Version(2, 2); }
        }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "None"; } }
		public override void OnEnable()
        {
            Logging.Write(Colors.Orange, "Burst'O'Speed - Enabled v" + Version);
            base.OnEnable();
        }

        public override void OnDisable()
		{
            Logging.Write(Colors.OrangeRed, "Burst'O'Speed - Disabled");
            base.OnDisable();
		}



        public override void Pulse()
	{
		if (!StyxWoW.IsInGame 
		|| !StyxWoW.Me.IsAlive 
		|| !StyxWoW.Me.IsMoving 
		|| StyxWoW.Me.Combat 
		|| StyxWoW.Me.IsCasting 
		|| StyxWoW.Me.Mounted 
		|| StyxWoW.Me.IsOnTransport 
		|| StyxWoW.Me.OnTaxi 
		|| Me.IsSwimming 
		|| Me.HasAura(68992) 
		|| Me.HasAura(2983) 
		|| Me.HasAura(137573))
		{ 
		return;
		}
		else if (Me.CurrentEnergy > 99 
			&& SpellManager.HasSpell(68992) 
			&& SpellManager.CanCast(68992) 
			&& !Me.HasAura(68992) 
			&& !Me.HasAura(2983) 
			&& !Me.HasAura(137573))
		{ 
		SpellManager.Cast(68992); // Darkflight
		return;
		}
		else if (Me.CurrentEnergy > 99 
			&& SpellManager.HasSpell(2983) 
			&& SpellManager.CanCast(2983) 
			&& !Me.HasAura(68992) 
			&& !Me.HasAura(2983) 
			&& !Me.HasAura(137573)) 
		{ 
		SpellManager.Cast(2983); // Sprint
		return;
		}
		else if (Me.CurrentEnergy > 99 
			&& SpellManager.HasSpell(108212) 
			&& SpellManager.CanCast(108212) 
			&& !Me.HasAura(68992) 
			&& !Me.HasAura(2983) 
			&& !Me.HasAura(137573)) 
		{ 
		SpellManager.Cast(108212); // Burst of Speed
		StyxWoW.Sleep(250);
		return;
		}
	}
    }
}
