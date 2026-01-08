// This Plugin saps mobs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Styx.TreeSharp;
using CommonBehaviors.Actions;
using Styx;
using Styx.WoWInternals;
// HB Stuff
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Inventory;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals.WoWObjects;

using System.Collections.Generic;

namespace SAP
{
    public class SAP : HBPlugin
    {
        #region Globals

        public override string Name { get { return "PatrolSapper"; } }
        public override string Author { get { return "Glenn"; } }
        public override Version Version { get { return new Version(1, 0, 0); } }
        public override bool WantButton { get { return false; } }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private int count = 0;
        private int stuckCount = 0;
        private WaitTimer Delay;
        private List<WoWUnit> unitsToSap;
        private List<WoWUnit> AllUnits;
        private WoWUnit SelectedAliveTarget { get; set; }
        public int MobCount { get; private set; }

        #endregion
        public override void Pulse()
        {
            //&& Me.IsAlive && !Me.IsGhost && !Me.IsOnTransport && !Me.OnTaxi && !Me.Stunned && !(Me.Mounted && Me.IsFlying) && !Me.IsCasting && Me.IsStealthed
            if (!Me.Combat && Me.IsAlive && !Me.IsGhost && !Me.IsOnTransport && !Me.OnTaxi && !Me.Stunned && !(Me.Mounted && Me.IsFlying) && !Me.IsCasting)
            {
                SapNearestMoving();
            }
			
			//cast stealth
			if (!Me.Combat && !Me.HasAura (1784) && Me.IsAlive && !Me.IsGhost && !(Me.Mounted && Me.IsFlying))
		    {
			
		
			SpellManager.Cast(1784);
			
			}
			
			   
			
			
			
			
			
			
            if (Me.IsActuallyInCombat)
            {
                // is vanish on cd?
                if (!SpellManager.GlobalCooldown && SpellManager.Spells["Vanish"].Cooldown)
                {
                    //cast prep
                    if (SpellManager.CanCast(14185))
                        SpellManager.Cast(14185);
                }

                //vanish
                if (Delay.IsFinished)
                {
                    #region BlacklistUnit: 10/27/2013
                    if (StyxWoW.Me.CurrentTarget != null)
                    {
                        //this stops the bot attacking a mob for 10 secs
                        
                    }
                    #endregion
                    Delay.Reset();
                    //shadow walk
                    if(SpellManager.CanBuff(114842))
                    {
                        SpellManager.Cast(114842);
                    }
                    if (SpellManager.CanCast(1856))
                        SpellManager.Cast(1856);
                }
            }
        }

        public override void Initialize()
        {
            Delay = new WaitTimer(TimeSpan.FromSeconds(1));
            Delay.Stop();
            base.Initialize();
        }
        public override void Dispose()
        {
            Delay.Stop();
            base.Dispose();
        }

        private void SapNearestMoving()
        {
            unitsToSap = new List<WoWUnit>();
            unitsToSap = GetClosestUnits();
            for (int i = 0; i < unitsToSap.Count(); i++)
            {
                if ((unitsToSap[i].Distance < 10))
                {
                    unitsToSap[i].Target();
                    if (!unitsToSap[i].Stunned)
                    {
                        SpellManager.Cast(6770);
                        Thread.Sleep(200);
                        SpellManager.Cast(921);
                        
                        Thread.Sleep(100);

                    }
                }
            }
        }

        private List<WoWUnit> GetClosestUnits()
        {

            AllUnits = (ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                        .Where(unit =>
                        !unit.IsDead &&
                        !unit.IsCritter &&
                        unit.IsHumanoid &&
                        unit.IsMoving &&
                        !unit.IsFriendly)
                        .OrderBy(unit => unit.Distance)).ToList<WoWUnit>();
            List<WoWUnit> units = new List<WoWUnit>();

            if (AllUnits != null && AllUnits.Count >= 1)
            {
                for (int index = 0; index < 1; index++)
                {
                    units.Add(AllUnits[index]);
                }
            }
            else if (AllUnits != null)
            {
                for (int index = 0; index < AllUnits.Count; index++)
                {
                    units.Add(AllUnits[index]);
                }
            }
            return units;
        }  
    }

}
