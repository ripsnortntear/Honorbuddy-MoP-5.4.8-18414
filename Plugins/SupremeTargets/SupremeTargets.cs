using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Plugins;
using Styx;
using Styx.WoWInternals.WoWObjects;
using Styx.Common;
using Styx.WoWInternals;
using Styx.CommonBot;

namespace SupremeTargets
{
    public class SupremeTargets : HBPlugin
    {
        #region customizable - someone make a gui eh?

        private bool PVPLogic = true;                   //set true/false to enable/disable player targets over mobs, suggestion: true

        private bool RemoveDeadTargets = true;          //if we may remove dead targets. i say yes!

        private bool FaceTarget = true;                 //face our target!
        
        private int FaceTargetFrequency = 3;            //seconds between each facing check, suggestion: 2-5, for pvp we need 0 .. but better use glue or something for pvp

        private bool FindNewTarget = true;              //finds a new target if we are in fight but have no target

        private bool RemoveTooFarAwayTargets = true;    //if we may lose our target if it gets too far away
        
        private int RemoveTooFarAwayTargetsDistance = 55;   //suggestion: 55yds or more away -> go for new target

        private bool RemoveInvulnerableTargets = false; //untested! will remove targets with bubble, deterrence, iceblock ... i recommend letting this FALSE!

        private bool FriendlyTargettingOutOfCombat = true;  //set true/false to enable/disable friendly player targetting and look less bottish, suggestion: true

        private int FriendyTargettingDistance = 30;     //distance to look for friendly targets
        
        private int FriendyTargettingFrequency = 10;    //set to anything between like 1 for ALWAYS look for targets no MATTER WHAT
                                                        //~10 to look every some seconds
                                                        //while values like 50 will take really long to find a friendly target. Suggestion: 10-15
        
        private int FriendlyTargetDuration = 5;         //time in seconds we will target a friendly target... not yet randomizable, maybe later; suggestion: 5

        #endregion

        #region do not touch
        #region I SAID DO NOT TOUCH
        public override string Name { get { return "SupremeTargets"; } }
        public string ShortName { get { return "SupTar"; } }
        public override Version Version { get { return new Version(2, 1, 2); } }
        public override string Author { get { return "ranomstraw"; } }
        private static LocalPlayer Me = StyxWoW.Me;
        public override string ButtonText { get { return "this is where the magic happens"; } }
        public override bool WantButton { get { return false; } }
        public override void Initialize()
        {
            Logging.Write(Colors.Lime, "+ " + Name + ", " + Version + ", enabled mods:");

            if (PVPLogic)
                Logging.Write(Colors.Lime, "+ PVP Targets > PVE Targets");

            if (RemoveDeadTargets)
                Logging.Write(Colors.Lime, "\tRemove Dead Targets");

            if (FriendlyTargettingOutOfCombat)
            {
                Logging.Write(Colors.Lime, "\tFriendly Targetting (out of combat)");

                if (FriendyTargettingDistance > 0)
                    Logging.Write(Colors.Lime, "\tw/ Range= " + FriendyTargettingDistance);

                if (FriendlyTargetDuration > 0)
                    Logging.Write(Colors.Lime, "\tfor " + FriendlyTargetDuration + " seconds");
            }

            Logging.Write(Colors.Yellow, "creating Timers...");
            Timers.Add("friendlyTarget");
            Timers.Add("faceTarget");
            Logging.Write(Colors.Orange, "done!");
        }

        public override void Pulse()
        {
            try
            {
                //if we're targetting an NPC we shall not remove him ...
                if (!Me.CurrentTarget.IsPlayer && Me.CurrentTarget.IsFriendly)
                    return;

                if (RemoveDeadTargets && Me.CurrentTarget.IsDead)
                {
                    Me.ClearTarget();
                    write("Dead Target removed (" + Me.CurrentTarget.Name + ")");
                    return;
                }

                if (!Me.Combat)
                {
                    //friendly targetting
                    if (FriendlyTargettingOutOfCombat)
                    {
                        if (Me.CurrentTarget.IsPlayer)
                        {
                            if (Timers.Expired("friendlyTarget", FriendlyTargetDuration * 1000))
                            {
                                write("removing friendly player target");
                                Me.ClearTarget();
                                Timers.Reset("friendlyTarget");
                                return;
                            }
                        }

                        if (Me.CurrentTarget == null && (Timers.Expired("friendlyTarget", 3000)))
                        {
                            Random rnd = new Random();
                            int nxt = rnd.Next(0, FriendyTargettingFrequency);  //this is to make sure we're not targetting stuff every new sec we dont have a target.
                            if (nxt == 1)
                            {
                                write("lets find a frienldy target...");
                                FindFriendlyTarget();
                                Timers.Reset("friendlyTarget");
                                return;
                            }

                            Timers.Reset("friendlyTarget");
                        }
                    }
                }

                if (Me.Combat)
                {
                    //so, we are in combat but have no target...
                    if (Me.CurrentTarget == null && FindNewTarget)
                    {
                        getTarget();
                        return;
                    }

                    //pvp > pve
                    if (PlayerAttackingMe != null && !Me.CurrentTarget.IsPlayer && PVPLogic)
                    {
                        WoWUnit pvpTarget = PlayerAttackingMe; 
                        pvpTarget.Target();
                        write("Player is attacking me (" + pvpTarget.Name + ")");
                        return;
                    }

                    //target got too far away...
                    if (RemoveTooFarAwayTargets && Me.CurrentTarget.Distance > RemoveTooFarAwayTargetsDistance)
                    {
                        Me.ClearTarget();
                        write("Target to far away, dropping (" + Me.CurrentTarget.Name + ")");
                        return;
                    }

                    //target is invulnerable? experimental one
                    if (RemoveInvulnerableTargets)
                    {
                        if (Me.CurrentTarget.ActiveAuras.ContainsKey("Deterrence") || Me.CurrentTarget.ActiveAuras.ContainsKey("Divine Shield") || Me.CurrentTarget.ActiveAuras.ContainsKey("Ice Block"))
                        {
                            Me.ClearTarget();
                            write("Target invulnerable, dropping (" + Me.CurrentTarget.Name + ")");
                            return;
                        }
                    }

                    if (!Me.IsSafelyFacing(Me.CurrentTarget) && FaceTarget && (Timers.Expired("faceTarget", FaceTargetFrequency * 1000)))
                    {
                        Me.CurrentTarget.Face();
                        Timers.Reset("faceTarget");
                        write("Facing Time!");
                    }
                }
            }
            catch (Exception e)
            {
                writeException("EXCEPTION: " + e.ToString());
            }
        }
        private void write(string text)
        {
            Logging.Write(Colors.Lime, "[" + ShortName + "] " + text);
        }
        private void writeException(string text)
        {
            Logging.WriteDiagnostic(Colors.Crimson, "[" + ShortName + "] " + text);
        }
        private WoWUnit PlayerAttackingMe
        {
            get
            {
                WoWUnit newTarget = (from o in ObjectManager.ObjectList
                                     where o is WoWUnit
                                     let unit = o.ToUnit()
                                     where
                                         unit.Distance < 40
                                         && unit.IsTargetingMeOrPet
                                         && unit.Combat
                                         && unit.PvpFlagged
                                         && unit.IsHostile
                                         && unit.IsPlayer
                                         && unit.IsAlive
                                         && !unit.IsFriendly
                                         && !unit.IsPet
                                         && !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                                     orderby unit.BaseHealth ascending
                                     select unit
                                 ).FirstOrDefault();
                if (newTarget != null)
                    return newTarget;

                return null;
            }
        }
        private void FindFriendlyTarget()
        {
            WoWUnit FriendlyTarget;

            FriendlyTarget = (
                from o in ObjectManager.ObjectList
                where o is WoWUnit
                let unit = o.ToUnit()
                where
                    unit.Distance < FriendyTargettingDistance
                    && unit.IsAlive
                    && !unit.IsMe
                    && unit.IsFriendly
                    && !unit.IsPet
                    && unit.IsPlayer
                    && unit.IsTargetingMeOrPet
                    && !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                orderby unit.Distance ascending
                select unit
                    ).FirstOrDefault();


            if (FriendlyTarget.Name.Length < 2) //means we have no target found, that is targetting us! Good one, lets look for a regular player then!
            {
                FriendlyTarget = (
                from o in ObjectManager.ObjectList
                where o is WoWUnit
                let unit = o.ToUnit()
                where
                    unit.Distance < FriendyTargettingDistance
                    && unit.IsAlive
                    && !unit.IsMe
                    && unit.IsFriendly
                    && !unit.IsPet
                    && unit.IsPlayer
                    //&& unit.IsTargetingMeOrPet
                    && !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                orderby unit.Distance ascending
                select unit
                    ).FirstOrDefault();
            }

            if (FriendlyTarget.Name.Length < 2) //means we have no target found around us at all, within the specified distance... return the sad matter!
            {
                write("...found no friendly Target in Range!");
            }
            else
            {
                write("...found " + FriendlyTarget.Name + " for " + FriendlyTargetDuration + " seconds!");
                FriendlyTarget.Target();

                if (!Timers.Exists("friendlyTarget"))
                    Timers.Add("friendlyTarget");
                else
                    Timers.Reset("friendlyTarget");

                Blacklist.Add(FriendlyTarget, BlacklistFlags.All, new TimeSpan(0, 3, 0)); //dont target this target again for 3 minutes. dont think this must be customizable..
            }
        }
        private void FindTarget()
        {
            WoWUnit newTarget = (from o in ObjectManager.ObjectList
                                 where o is WoWUnit
                                 let unit = o.ToUnit()
                                 where
                                     unit.Distance < 40
                                     && !unit.TaggedByOther
                                     && unit.Combat
                                     && unit.Attackable
                                     && unit.IsAlive
                                     && !unit.IsFriendly
                                     && !unit.IsPet
                                     && !Blacklist.Contains(unit.Guid,  BlacklistFlags.All)
                                 orderby unit.Distance ascending
                                 select unit
                                 ).FirstOrDefault();

            if (newTarget.Name.Length < 2)
            {
                write("...no suitable target found .. damn, will this cause a loop?");
                return;
            }
            else
            {
                newTarget.Target();
                write("...found " + newTarget.Name);
                return;
            }
        }
        private void getTarget()
        {
            if (!Me.GotAlivePet && FindNewTarget)
            {
                write("finding target...");
                FindTarget();
                return;
            }

            if (Me.GotAlivePet && Me.Pet.CurrentTarget != null)
            {
                Me.Pet.CurrentTarget.Target();
                write("no target, but pet has! -> (" + Me.Pet.CurrentTarget.Name + ")");
                return;
            }
        }
        #endregion
        #endregion
    }
}
