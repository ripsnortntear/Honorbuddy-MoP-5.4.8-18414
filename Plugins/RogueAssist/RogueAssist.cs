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
using CommonBehaviors.Actions;
using Action = Styx.TreeSharp.Action;

namespace RogueAssist
{
    public class RogueAssist : HBPlugin
    {
		private static LocalPlayer Me { get { return StyxWoW.Me; } }
		
		#region Default Overrides
        public override string Name { get { return "RogueAssist"; } }
        public override string Author { get { return "Naut"; } }
        public override Version Version { get { return new Version(1, 0, 0, 1); } }
        public override string ButtonText { get { return "Settings"; } }
        public override bool WantButton { get { return false; } }
		#endregion Default Overrides

		public int FeintPercent = 90; // Feint HP %
		public int DefensePercent = 40; // Feint HP %
		
        public override void Pulse()
        {
			// Premed Before Opening, When Vanished, or Shadow Dance - Will not cast if you have more than 3 combo points on your target so it isn't wasted.
			if (Me.HasAura("Stealth") && SpellManager.HasSpell("Premeditation") && SpellManager.CanCast("Premeditation") && Me.CurrentTarget.Distance < 20 && Me.ComboPoints <= 3)
			{
				SpellManager.Cast("Premeditation");
			}
			
			if (Me.HasAura("Vanish") && SpellManager.HasSpell("Premeditation") && SpellManager.CanCast("Premeditation") && Me.CurrentTarget.Distance < 20 && Me.ComboPoints <= 3)
			{
				SpellManager.Cast("Premeditation");
			}
			
			if (Me.HasAura("Shadow Dance") && SpellManager.HasSpell("Premeditation") && SpellManager.CanCast("Premeditation") && Me.CurrentTarget.Distance < 20 && Me.ComboPoints <= 3)
			{
				SpellManager.Cast("Premeditation");
			}
			
			// Keep Feint Up In Combat & Below 90% HP
			if (Me.Combat && (Me.HealthPercent < FeintPercent) && !Me.HasAura("Feint"))					
			{
				SpellManager.Cast("Feint");
			}
			
			// Shadow Walk if Stealthed and Target Within 8 Yards
			if (Me.HasAura("Stealth") && Me.CurrentTarget.Distance < 8 && SpellManager.CanCast("Shadow Walk"))					
			{
				SpellManager.Cast("Shadow Walk");
			}
			
			// Shadow Walk if Stealthed and Target Within 8 Yards
			if (Me.HasAura("Vanish") && Me.CurrentTarget.Distance < 8 && SpellManager.CanCast("Shadow Walk"))					
			{
				SpellManager.Cast("Shadow Walk");
			}
			
			// Burst of Speed if target is out of melee range
			if (Me.IsMoving && Me.CurrentTarget.Distance > 8 && SpellManager.HasSpell("Burst of Speeed")  && !Me.HasAura("Burst of Speed") && !Me.Mounted)					
			{
				SpellManager.Cast("Burst of Speed");
			}
			
			// Keep Slice and Dice Activated at all times in combat
			if (Me.Combat && !Me.HasAura("Slice and Dice") && Me.ComboPoints >= 3 && Me.CurrentEnergy >= 25)
			{
				SpellManager.Cast("Slice and Dice");
			}
			
			// Keep Recuperate Active Below 90% HP - Priority to check for Slice and Dice first
			if (Me.Combat && Me.HasAura("Slice and Dice") && !Me.HasAura("Recuperate") && Me.ComboPoints >= 3 && Me.CurrentEnergy >= 25 && (Me.HealthPercent < FeintPercent))
			{
				SpellManager.Cast("Recuperate");
			}
			
			// Keep Rupture Active on Target
			if (Me.Combat && !Me.CurrentTarget.HasAura("Rupture") && Me.ComboPoints >= 3 && Me.CurrentEnergy >= 25)
			{
				SpellManager.Cast("Rupture");
			}
			
			// Evasion below 40% HP if target within 10 yards (Melee Target)
			if (Me.Combat && (Me.HealthPercent < DefensePercent) && !Me.HasAura("Evasion") && SpellManager.CanCast("Evasion") && Me.CurrentTarget.Distance < 10)					
			{
				SpellManager.Cast("Evasion");
			}
			
			// Combat Readiness below 40% HP if target within 10 yards (Melee Target)
			if (Me.Combat && (Me.HealthPercent < DefensePercent) && !Me.HasAura("Combat Readiness") && SpellManager.CanCast("Combat Readiness") && Me.CurrentTarget.Distance < 10)					
			{
				SpellManager.Cast("Combat Readiness");
			}
			
			// Interupt	Enemy Casting
			while (Me.Combat && Me.CurrentTarget.IsCasting)
			{
				if (SpellManager.HasSpell("Kick") && SpellManager.CanCast("Kick") && Me.CurrentTarget.Distance < 5)
				{
					Thread.Sleep(500);
					SpellManager.Cast("Kick");
				}	
				if (SpellManager.HasSpell("Cheap Shot") && SpellManager.CanCast("Cheap Shot") && !SpellManager.CanCast("Kick") && Me.CurrentTarget.Distance < 5)
				{
					Thread.Sleep(500);
					SpellManager.Cast("Cheap Shot");
				}	
				if (SpellManager.HasSpell("Kidney Shot") && !SpellManager.CanCast("Cheap Shot") && !SpellManager.CanCast("Kick") && SpellManager.CanCast("Kidney Shot") && Me.ComboPoints >= 3 && Me.CurrentTarget.Distance < 5)
				{
					Thread.Sleep(500);
					SpellManager.Cast("Kidney Shot");
				}
				if (SpellManager.HasSpell("Gouge") && !SpellManager.CanCast("Kick") && !SpellManager.CanCast("Cheap Shot") && !SpellManager.CanCast("Kidney Shot") && SpellManager.CanCast("Gouge") && Me.CurrentTarget.Distance < 5)
				{
					Thread.Sleep(500);
					SpellManager.Cast("Gouge");
				}
			}
		}
    }
}
