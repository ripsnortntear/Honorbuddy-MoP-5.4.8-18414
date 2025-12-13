using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;


using Styx;
using Styx.TreeSharp;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.CommonBot.Inventory;
using Styx.Common;
using Styx.Resources;
using Styx.Loaders;
using Styx.Common.Helpers;
using Styx.Pathing;
using Styx.XmlEngine;
using Styx.CommonBot.POI;
using Styx.WoWInternals.World;
using CommonBehaviors.Actions;

using Action = Styx.TreeSharp.Action;

namespace DWCC
{
    public partial class Warrior
    {
        #region SpellCaster
        public Composite CreateSpellCheckAndCast(string name, CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => SpellManager.HasSpell(name) && SpellManager.CanCast(name) && Me.CurrentTarget != null && Me.IsFacing(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange && extra(ret),
                                 new Action(delegate
                                 {
                                     SpellManager.Cast(name);
                                     Logging.Write("[DWCC]: " + name);
                                     Logging.WriteDiagnostic("[DWCC]: Attempting to cast " + name + " on " + Me.CurrentTarget.Class.ToString() + " @ " + Me.CurrentTarget.CurrentHealth + "/" + Me.CurrentTarget.MaxHealth + " (" + Math.Round(Me.CurrentTarget.HealthPercent, 2) + "%)");
                                     Logging.WriteDiagnostic("[DWCC]: Target: IsCasting: " + Me.CurrentTarget.IsCasting + " | IsPlayer: " + Me.CurrentTarget.IsPlayer + " | Distance: " + Math.Round(Me.CurrentTarget.Distance, 2) + " | Level: " + Me.CurrentTarget.Level + " | IsElite: " + Me.CurrentTarget.Elite + " | Adds: " + detectAdds().Count);
                                     Logging.WriteDiagnostic("[DWCC]: We are in: " + Me.ZoneText + " | Instance: " + Me.IsInInstance + " | Outdoors: " + Me.IsOutdoors + " | Battleground: " + Styx.WoWInternals.Battlegrounds.IsInsideBattleground + " | Indoors: " + Me.IsIndoors + " | Party: " + Me.GroupInfo.IsInParty + " | Raid: " + Me.GroupInfo.IsInRaid + " | Members: " + Me.PartyMembers.Count + "/" + Me.RaidMembers.Count + " | Health: " + Me.CurrentHealth + "/" + Me.MaxHealth + " (" + Math.Round(Me.HealthPercent, 2) + "%) | BattleStance: " + BattleStance + " | DefStance: " + DefStance + " | BerserkerStance: " + BersiStance);
                                     return RunStatus.Success;
                                 }
                                     ));
        }

        public Composite CreateSpellCheckAndCast(string name)
        {
            return CreateSpellCheckAndCast(name, ret => true);
        }

        public Composite CreateSpellCheckAndCast(int name, CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && SpellManager.HasSpell(name) && SpellManager.CanCast(name) && Me.CurrentTarget != null && Me.IsFacing(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange,
                                 new Action(delegate
                                 {
                                     SpellManager.Cast(name);
                                     Logging.Write("[DWCC]: " + name);
                                     Logging.WriteDiagnostic("[DWCC]: Attempting to cast " + name + " on " + Me.CurrentTarget.Class.ToString() + " @ " + Me.CurrentTarget.CurrentHealth + "/" + Me.CurrentTarget.MaxHealth + " (" + Math.Round(Me.CurrentTarget.HealthPercent, 2) + "%)");
                                     Logging.WriteDiagnostic("[DWCC]: Target: IsCasting: " + Me.CurrentTarget.IsCasting + " | IsPlayer: " + Me.CurrentTarget.IsPlayer + " | Distance: " + Math.Round(Me.CurrentTarget.Distance, 2) + " | Level: " + Me.CurrentTarget.Level + " | IsElite: " + Me.CurrentTarget.Elite + " | Adds: " + detectAdds().Count);
                                     Logging.WriteDiagnostic("[DWCC]: We are in: " + Me.ZoneText + " | Instance: " + Me.IsInInstance + " | Outdoors: " + Me.IsOutdoors + " | Battleground: " + Styx.WoWInternals.Battlegrounds.IsInsideBattleground + " | Indoors: " + Me.IsIndoors + " | Party: " + Me.GroupInfo.IsInParty + " | Raid: " + Me.GroupInfo.IsInRaid + " | Members: " + Me.PartyMembers.Count + "/" + Me.RaidMembers.Count + " | Health: " + Me.CurrentHealth + "/" + Me.MaxHealth + " (" + Math.Round(Me.HealthPercent, 2) + "%) | BattleStance: " + BattleStance + " | DefStance: " + DefStance + " | BerserkerStance: " + BersiStance);
                                     return RunStatus.Success;
                                 }
                                     ));
        }

        public Composite CreateSpellCheckAndCast(string name, CanRunDecoratorDelegate extra, bool RunStatusSuccess)
        {
            return new Decorator(ret => extra(ret) && SpellManager.HasSpell(name) && SpellManager.CanCast(name) && Me.IsFacing(Me.CurrentTarget) && Me.CurrentTarget.IsWithinMeleeRange,
                                 new Action(delegate
                                 {
                                     SpellManager.Cast(name);
                                     Logging.Write("[DWCC]: " + name);
                                     Logging.WriteDiagnostic("[DWCC]: Attempting to cast " + name + " on " + Me.CurrentTarget.Class.ToString() + " @ " + Me.CurrentTarget.CurrentHealth + "/" + Me.CurrentTarget.MaxHealth + " (" + Math.Round(Me.CurrentTarget.HealthPercent, 2) + "%)");
                                     Logging.WriteDiagnostic("[DWCC]: Target: IsCasting: " + Me.CurrentTarget.IsCasting + " | IsPlayer: " + Me.CurrentTarget.IsPlayer + " | Distance: " + Math.Round(Me.CurrentTarget.Distance, 2) + " | Level: " + Me.CurrentTarget.Level + " | IsElite: " + Me.CurrentTarget.Elite + " | Adds: " + detectAdds().Count);
                                     Logging.WriteDiagnostic("[DWCC]: We are in: " + Me.ZoneText + " | Instance: " + Me.IsInInstance + " | Outdoors: " + Me.IsOutdoors + " | Battleground: " + Styx.WoWInternals.Battlegrounds.IsInsideBattleground + " | Indoors: " + Me.IsIndoors + " | Party: " + Me.GroupInfo.IsInParty + " | Raid: " + Me.GroupInfo.IsInRaid + " | Members: " + Me.PartyMembers.Count + "/" + Me.RaidMembers.Count + " | Health: " + Me.CurrentHealth + "/" + Me.MaxHealth + " (" + Math.Round(Me.HealthPercent, 2) + "%) | BattleStance: " + BattleStance + " | DefStance: " + DefStance + " | BerserkerStance: " + BersiStance);
                                     return RunStatus.Failure;
                                 }
                                     ));
        }
        #endregion

        #region Helpers
        #region New Helpers
        public uint Stacks(string aura)
        {
            if (Me.HasAura(aura))
                return Me.GetAuraByName(aura).StackCount;
            return 0;
        }

        public bool UnitHasMyAura(WoWUnit unit, string aura)
        {
            if (unit.GetAllAuras().Any(a => a.Name == aura && a.CreatorGuid == Me.Guid)) return true;
            return false;
        }

        public bool TargetHasMyAura(string aura) { return UnitHasMyAura(Me.CurrentTarget, aura); }

        public bool UnitHasMyAuraTimeLeft(WoWUnit unit, string aura, int ms)
        {
            if (unit.GetAllAuras().Any(a => a.Name == aura && a.CreatorGuid == Me.Guid && a.TimeLeft.Milliseconds > ms)) return true;
            return false;
        }

        public bool TargetHasMyAuraTimeLeft(string aura, int ms) { return UnitHasMyAuraTimeLeft(Me.CurrentTarget, aura, ms); }

        public bool MyCoolDown(string spell)
        {
            if (SpellManager.HasSpell(spell) && SpellManager.Spells[spell].Cooldown) return true;
            return false;
        }

        public double MyCoolDownLeft(string spell)
        {
            if (MyCoolDown(spell)) return SpellManager.Spells[spell].CooldownTimeLeft.TotalMilliseconds;
            return 0;
        }

        public bool IsEnraged(WoWUnit unit)
        {
            return unit.GetAllAuras().Any(a => a.Spell.Mechanic == WoWSpellMechanic.Enraged);
        }

        //----------------------------------------------------------------------------------------------------------
        public bool ExecutePhase { get { return Me.CurrentTarget.HealthPercent < 20; } }

        public bool ColossusSmashAura { get { return TargetHasMyAura("Colossus Smash"); } }
        public bool ColossusSmash5s { get { return TargetHasMyAuraTimeLeft("Colossus Smash", 5000); } }
        public bool ColossusSmash25s { get { return TargetHasMyAuraTimeLeft("Colossus Smash", 2500); } }
        public bool ColossusSmash1s { get { return TargetHasMyAuraTimeLeft("Colossus Smash", 1000); } }

        public double ColossusSmashCD { get { return MyCoolDownLeft("Colossus Smash"); } }
        public double BloodBathCD { get { return MyCoolDownLeft("Bloodbath"); } }

        public bool RecklessnessAura { get { return Me.HasAura("Recklessness"); } }
        public bool EnrageAura { get { return IsEnraged(Me); } }
        public bool SuddenExecuteAura { get { return Me.HasAura("Sudden Execute"); } }
        public bool BloodBathAura { get { return Me.HasAura("Bloodbath"); } }

        public uint TasteForBloodStacks
        {
            get
            {
                if (Me.HasAura("Taste for Blood"))
                    return Me.GetAuraByName("Taste for Blood").StackCount;
                return 0;
            }
        }
        //---------------------------------------------------------------------------------------------------------
        public uint RagingBlowStacks
        {
            get
            {
                if (Me.HasAura("Raging Blow"))
                    return Me.GetAuraByName("Raging Blow!").StackCount;
                return 0;
            }
        }
        public double RagingBlowTimeLeft { get { return Me.GetAuraByName("Raging Blow!").TimeLeft.TotalMilliseconds; } }

        public bool BloodsurgeAura { get { return Me.HasAura("Bloodsurge"); } }

        public double BloodthirstCD { get { return MyCoolDownLeft("Bloodthirst"); } }


        public double AuraTimeLeft(string aura)
        {
            if (Me.HasAura(aura))
                return Me.GetAuraByName(aura).TimeLeft.TotalMilliseconds;
            return 0;
        }
        #endregion
        #endregion

        #region Movement
        // System.Windows.Media.Color.FromRgb(0xFF, 0x00, 0x00) = RED
        TimeSpan MovementDelay = TimeSpan.FromSeconds(3);
        TimeSpan PvPMovementDelay = TimeSpan.FromSeconds(1.5);
        DateTime LastMovement = DateTime.Now;
        WoWPoint LastMovementPoint = Me.Location;
        public static float MeleeDistance(WoWUnit target)
        {
            if (target.IsPlayer) return 2.0f;
            else return 5f;
        }

        public WoWPoint pointBehind()
        {
            return Me.CurrentTarget.Location.RayCast(
                                Me.CurrentTarget.Rotation + WoWMathHelper.DegreesToRadians(150), 2f);
        }

        public Composite MoveToTarget()
        {
            return new Decorator(ret =>
                !DunatanksSettings.Instance.DisableMovement &&
                !Me.IsCasting &&
                !Me.IsChanneling &&
                Me.CurrentTarget != null &&
                Me.CurrentTarget.Attackable &&
                Me.CurrentTarget.IsHostile &&
                Me.CurrentTarget.InLineOfSight &&
                (!Me.CurrentTarget.IsPlayer || (Me.CurrentTarget.IsPlayer && Me.CurrentTarget.Distance >= 12)) &&
                (Me.CurrentTarget.Distance >= 4) &&
                ((DateTime.Now > LastMovement + MovementDelay) ||
                (LastMovementPoint.Distance(Me.CurrentTarget.Location) > 3f)),
                new Action(ret =>
                    {
                        Logging.WriteDiagnostic("MoveToTarget()");
                        LastMovementPoint = Me.CurrentTarget.Location;
                        LastMovement = DateTime.Now;
                        Me.CurrentTarget.Face();
                        Navigator.MoveTo(LastMovementPoint);
                        return RunStatus.Failure;
                    }));
        }

        public Composite MoveBehindTarget()
        {
            
            return
                new Decorator(
                    ret =>
                    !DunatanksSettings.Instance.DisableMovement &&
                    !Me.IsCasting &&
                    !Me.IsChanneling &&
                    Me.CurrentTarget != null &&
                    Me.CurrentTarget.IsAlive &&
                    Me.CurrentTarget.IsPlayer &&
                    //(!Me.CurrentTarget.IsPlayer && Me.CurrentTarget.CurrentTarget != Me && Me.CurrentTarget.Combat) &&
                        //only MovementMoveBehind if IsWithinMeleeRange
                    Me.IsWithinMeleeRange &&
                    !Me.IsBehind(Me.CurrentTarget),
                    new Action(ret =>
                    {
                        Logging.WriteDiagnostic("move behind");
                        LastMovementPoint = pointBehind();
                        Navigator.MoveTo(LastMovementPoint);
                        return RunStatus.Failure;
                    }));
        }

        public Composite StopMoving()
        {
            return new Decorator(
                ret =>
                !DunatanksSettings.Instance.DisableMovement &&
                Me.CurrentTarget != null &&
                Me.CurrentTarget.IsAlive &&
                Me.IsMoving &&
                !Me.CurrentTarget.IsMoving &&
                Me.CurrentTarget.Distance <= 2,
                new Action(ret =>
                {
                    Navigator.PlayerMover.MoveStop();
                    return RunStatus.Failure;
                }));
        }
        #endregion

        #region Heroic Leap
        WoWPoint preLeap;
        public Composite HeroicLeap()
        {
            return new Decorator(ret => Me.CurrentTarget != null && 
                !Me.CurrentTarget.Name.Contains("Elegon") && 
                Me.CurrentTarget.IsHostile && 
                Me.CurrentTarget.InLineOfSight && 
                !Me.CurrentTarget.IsFlying && 
                Me.CurrentTarget.Attackable && 
                SpellManager.CanCast("Heroic Leap") && 
                !Me.CurrentTarget.HasAura("Charge Stun") && 
                SpellManager.Spells["Charge"].CooldownTimeLeft.TotalSeconds < SpellManager.Spells["Charge"].BaseCooldown - 1.5 &&
                Me.CurrentTarget.Distance > 13f && 
                Me.CurrentTarget.Distance < 25f  && 
                !DunatanksSettings.Instance.DisableMovement,
            new Sequence(
                new Action(a =>
                    {
                        //preLeap = Me.Location;
                        SpellManager.Cast("Heroic Leap");
                        SpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                        Logging.Write("[DWCC]: Heroic Leap. Distance: " + Math.Round(preLeap.Distance(Me.CurrentTarget.Location), 2));
                        //new WaitContinue(TimeSpan.FromMilliseconds(500), wc => preLeap.Distance(Me.Location) < 2f, new ActionAlwaysFail());
                    })
            ));
        }
        #endregion

        #region Charge
        WoWPoint preCharge;
        public Composite CastCharge()
        {
            return new Decorator(ret => SpellManager.CanCast("Charge") && !DunatanksSettings.Instance.DisableMovement && Me.CurrentTarget != null && Me.CurrentTarget.InLineOfSight && Me.CurrentTarget.InLineOfSpellSight && Me.CurrentTarget.Distance > 13f && Me.CurrentTarget.Distance < 25f && !DunatanksSettings.Instance.DisableMovement,
                new Sequence(
                    new Action(a =>
                        {
                            preCharge = Me.Location;
                            SpellManager.Cast("Charge");
                            Logging.Write("[DWCC]: Charge. Distance: " + Math.Round(preCharge.Distance(Me.CurrentTarget.Location), 2));
                            Logging.WriteDiagnostic("[DWCC]: Attempting to cast Charge on " + Me.CurrentTarget.Name.Remove(3, Me.CurrentTarget.Name.Length - 3) + "*** @ " + Me.CurrentTarget.CurrentHealth + "/" + Me.CurrentTarget.MaxHealth + " (" + Math.Round(Me.CurrentTarget.HealthPercent, 2) + "%)");
                            Logging.WriteDiagnostic("[DWCC]: Target: IsCasting: " + Me.CurrentTarget.IsCasting + " | IsPlayer: " + Me.CurrentTarget.IsPlayer + " | Distance: " + Math.Round(Me.CurrentTarget.Distance, 2) + " | Level: " + Me.CurrentTarget.Level + " | IsElite: " + Me.CurrentTarget.Elite + " | Adds: " + detectAdds().Count);
                            Logging.WriteDiagnostic("[DWCC]: We are in: " + Me.ZoneText + " | Instance: " + Me.IsInInstance + " | Outdoors: " + Me.IsOutdoors + " | Battleground: " + Styx.WoWInternals.Battlegrounds.IsInsideBattleground + " | Indoors: " + Me.IsIndoors + " | Party: " + Me.GroupInfo.IsInParty + " | Raid: " + Me.GroupInfo.IsInRaid + " | Members: " + Me.PartyMembers.Count + "/" + Me.RaidMembers.Count + " | Health: " + Me.CurrentHealth + "/" + Me.MaxHealth + " (" + Math.Round(Me.HealthPercent, 2) + "%) | BattleStance: " + BattleStance + " | DefStance: " + DefStance + " | BerserkerStance: " + BersiStance);
                            new WaitContinue(TimeSpan.FromMilliseconds(500), wc => preCharge.Distance(Me.Location) < 2f, new ActionAlwaysFail());
                        })));
        }
        #endregion

        #region Add Detection
        //Credit to CodeNameGamma for detectAdds code
        private List<WoWUnit> detectAdds()
        {
            List<WoWUnit> addList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                        unit.Guid != Me.Guid &&
                        unit.Distance < DunatanksSettings.Instance.CombatDistance &&
                        unit.IsAlive &&
                        unit.Combat &&
                        (unit.Name.Contains("Training Dummy") || unit.IsPlayer) &&
                        !unit.IsPet &&
                        !unit.IsFriendly &&
                        unit.Attackable &&
                        !Styx.CommonBot.Blacklist.Contains(unit.Guid, BlacklistFlags.All));
            //Logging.Write(addList.Count.ToString());


            return addList;
        }

        private List<WoWPlayer> detectPlayerAdds()
        {
            List<WoWPlayer> addList = ObjectManager.GetObjectsOfType<WoWPlayer>(false).FindAll(unit =>
                        unit.Guid != Me.Guid &&
                        unit.Distance < DunatanksSettings.Instance.CombatDistance &&
                        unit.IsAlive &&
                        !unit.IsFriendly &&
                        unit.Attackable &&
                        !Styx.CommonBot.Blacklist.Contains(unit.Guid, BlacklistFlags.All));
            //Logging.Write(addList.Count.ToString());


            return addList;
        }
        #endregion

        #region Bloodsurge
        public bool Bloodsurge()
        {
            if (Me.HasAura(46916)) return true;
            else return false;
        }
        #endregion

        #region Stances
        public bool BattleStance
        {
            get
            {
                if (StyxWoW.Me.HasAura(2457))
                {
                    return true;
                }
                return false;
            }
        }

        public bool DefStance
        {
            get
            {
                if (StyxWoW.Me.HasAura(71))
                {
                    return true;
                }
                return false;
            }
        }

        public bool BersiStance
        {
            get
            {
                if (StyxWoW.Me.HasAura(2458))
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region IsTank
        public bool TargetTargetIsTank()
        {
            WoWPlayer PotentialTank = null;
            if (Me.CurrentTarget.CurrentTarget.IsPlayer && Me.CurrentTarget.CurrentTarget != Me)
            {
                PotentialTank = Me.CurrentTarget.CurrentTarget.ToPlayer();
                if (PotentialTank.Type.Equals(SpecType.Tank))
                {
                    Logging.WriteDiagnostic("[DWCC]: IsTank Check: My target: " + Me.CurrentTarget.Name + " . His target is: " + Me.CurrentTarget.CurrentTarget.Name + ". He is a Tank.");
                    return true;
                }
                else
                {
                    Logging.WriteDiagnostic("[DWCC]: IsTank Check: My target: " + Me.CurrentTarget.Name + " . His target is: " + Me.CurrentTarget.CurrentTarget.Name + ". He is NOT a Tank.");
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region AutoAttack
        public static Composite CreateAutoAttack()
        {

            return new Decorator(ret => !Me.IsAutoAttacking && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange,
                new Action(ret => Me.ToggleAttack()));
        }
        #endregion

        #region UnitSelection
        public delegate WoWUnit UnitSelectDelegate(object context);
        #endregion

        #region FaceTarget
        public Composite FaceTarget()
        {
            return new Decorator(ret => !DunatanksSettings.Instance.DisableMovement,
                new Action(a =>
                    {
                        Me.CurrentTarget.Face();
                        return RunStatus.Failure;
                    }));
        }
        #endregion

        #region StanceCheck
        public Composite StanceChange()
        {
            return new Decorator(
                     ret => !Me.IsDead && !DunatanksSettings.Instance.usePvPRotation,
                     new PrioritySelector(
                        new Decorator(
                            ret => !BattleStance && (DunatanksSettings.Instance.useArms || DunatanksSettings.Instance.useFury),
                            new Action(ret => SpellManager.Cast("Battle Stance"))),
                        new Decorator(ret => !DefStance && DunatanksSettings.Instance.useProt,
                            new Action(ret => SpellManager.Cast("Defensive Stance")))
                         ));
        }
        #endregion

        #region StoneFormCheck
        public bool StoneFormCheck(WoWUnit unit)
        {
            return Me.GetAllAuras().Any(
                a => a.IsHarmful &&
                     (a.Spell.Mechanic == WoWSpellMechanic.Bleeding ||
                      a.Spell.Mechanic == WoWSpellMechanic.Infected
                      ));
        }
        #endregion

        #region EscapeArtistCheck
        public bool EscapeArtistCheck(WoWUnit unit)
        {
            return Me.GetAllAuras().Any(
                a => a.IsHarmful &&
                     (a.Spell.Mechanic == WoWSpellMechanic.Rooted ||
                      a.Spell.Mechanic == WoWSpellMechanic.Slowed
                      ));
        }
        #endregion

        #region ForsakenArtistCheck
        public bool ForsakenCheck(WoWUnit unit)
        {
            return Me.GetAllAuras().Any(
                a => a.IsHarmful &&
                     (a.Spell.Mechanic == WoWSpellMechanic.Charmed ||
                      a.Spell.Mechanic == WoWSpellMechanic.Horrified ||
                      a.Spell.Mechanic == WoWSpellMechanic.Asleep
                      ));
        }
        #endregion

        #region FleeingCheck
        public bool IsFleeing(WoWUnit unit)
        {
            return Me.CurrentTarget.GetAllAuras().Any(
                a => a.Spell.Mechanic == WoWSpellMechanic.Fleeing
                );
        }
        #endregion

        #region SlowCheck
        public bool IsSlowed(WoWUnit unit)
        {
            return Me.CurrentTarget.GetAllAuras().Any(
                a => a.Spell.Mechanic == WoWSpellMechanic.Slowed
                    || a.Spell.Mechanic == WoWSpellMechanic.Invulnerable
                    || a.Spell.Mechanic == WoWSpellMechanic.Invulnerable2
                );
        }
        #endregion

        #region Trinkets
        #region Useable
        private static bool CanUseEquippedItem(WoWItem item)
        {
            // Check for engineering tinkers!
            string itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;


            return item.Usable && item.Cooldown <= 0;
        }
        #endregion

        WoWItem TrinketOne, TrinketTwo = null;

        public bool CheckTrinketOne()
        {
            if (Me.Inventory.Equipped.Trinket1 != null)
            {
                if (CanUseEquippedItem(StyxWoW.Me.Inventory.Equipped.Trinket1))
                {
                    WoWItem TrinketOne = StyxWoW.Me.Inventory.Equipped.Trinket1;
                    return true;
                }
                return false;
            }
            return false;
        }

        public Composite UseTrinketOne()
        {
            return new Decorator(ret => CheckTrinketOne() && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown == 0 && (DunatanksSettings.Instance.UseTrinketOneOnCd) || (DunatanksSettings.Instance.UseTrinketOneBelow20 && Me.CurrentTarget.HealthPercent < 20) || (IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketOneCC) || ((Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketOneHero),
                                 new Action(a =>
                                 {
                                     StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                                     Logging.Write("[DWCC]: Using " + TrinketOne.Name + " <--");
                                     return RunStatus.Failure;
                                 }
                                     ));
        }

        public bool CheckTrinketTwo()
        {
            if (Me.Inventory.Equipped.Trinket2 != null)
            {
                if (CanUseEquippedItem(StyxWoW.Me.Inventory.Equipped.Trinket2))
                {
                    WoWItem TrinketTwo = StyxWoW.Me.Inventory.Equipped.Trinket2;
                    return true;
                }
                return false;
            }
            return false;
        }

        public Composite UseTrinketTwo()
        {

            return new Decorator(ret => CheckTrinketTwo() && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown == 0 && (DunatanksSettings.Instance.UseTrinketOneOnCd && Me.HasAura("Recklessness")) || (DunatanksSettings.Instance.UseTrinketTwoBelow20 && Me.CurrentTarget.HealthPercent < 20) || (IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketTwoCC) || ((Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketTwoHero),
                                 new Action(a =>
                                 {
                                     StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                                     Logging.Write("[DWCC]: Using " + TrinketTwo.Name + " <--");
                                     return RunStatus.Failure;
                                 }
                                     ));
        }

        public bool TrinketOneReady()
        {
            if (StyxWoW.Me.Inventory.GetItemBySlot(12) != null && StyxWoW.Me.Inventory.GetItemBySlot(12).BaseAddress != null)
            {
                if (StyxWoW.Me.Inventory.GetItemBySlot(12).Cooldown == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TrinketTwoReady()
        {
            if (StyxWoW.Me.Inventory.GetItemBySlot(13) != null && StyxWoW.Me.Inventory.GetItemBySlot(13).BaseAddress != null)
            {
                if (StyxWoW.Me.Inventory.GetItemBySlot(13).Cooldown == 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region AoEPummel
        public WoWUnit AoEPummelLastTarget = null;

        public Composite AoEPummel()
        {
            SetCurrentTarget();
            return new Decorator(ret => DunatanksSettings.Instance.usePummelAoEAuto && SpellManager.CanCast("Pummel", (WoWUnit)AoECastingAdds().FirstOrDefault()) && (WoWUnit)AoECastingAdds().FirstOrDefault() != null,
                                 new Sequence(
                                     TargetAoEPummel(ret => Me.CurrentTarget != ((WoWUnit)AoECastingAdds().FirstOrDefault()) && (WoWUnit)AoECastingAdds().FirstOrDefault() != null),
                                     FaceAoEPummel(ret => !Me.IsFacing((WoWUnit)AoECastingAdds().FirstOrDefault()) && (WoWUnit)AoECastingAdds().FirstOrDefault() != null),
                                     new Action(ret => SpellManager.Cast("Pummel", (WoWUnit)AoECastingAdds().FirstOrDefault())),
                                     new Action(ret => AoEPummelLastTarget.Target()),
                                     new Action(ret => AoEPummelLastTarget.Face())));
        }

        public void SetCurrentTarget()
        {
            AoEPummelLastTarget = Me.CurrentTarget;
        }

        public Composite FaceAoEPummel(CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && !Me.IsFacing((WoWUnit)AoECastingAdds().FirstOrDefault()),
                                 new Action(delegate
                                 {
                                     ((WoWUnit)AoECastingAdds().FirstOrDefault()).Face();
                                     Logging.Write("[DWCC]: Facing AoE Pummel target.");
                                 }
                                     ));
        }

        public Composite TargetAoEPummel(CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && Me.CurrentTarget != ((WoWUnit)AoECastingAdds().FirstOrDefault()),
                                 new Action(delegate
                                 {
                                     ((WoWUnit)AoECastingAdds().FirstOrDefault()).Target();
                                     Logging.Write("[DWCC]: Targeting AoE Pummel target.");
                                 }
                                     ));
        }
        #endregion

        #region AoECastingAdds
        private List<WoWUnit> AoECastingAdds()
        {
            List<WoWUnit> addList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                        unit.Guid != Me.Guid &&
                        unit.Distance < DunatanksSettings.Instance.CombatDistance &&
                        unit.IsAlive &&
                        unit.Combat &&
                        unit.IsCasting &&
                        !unit.IsFriendly &&
                        !unit.IsPet &&
                        !Styx.CommonBot.Blacklist.Contains(unit.Guid, BlacklistFlags.All));

            return addList;
        }
        #endregion

        #region HealthRegen
        public static ulong LastTarget;
        public static ulong LastTargetHPPot;
        WoWItem CurrentHealthPotion;
        public bool HaveHealthPotion()
        {
            //whole idea is to make sure CurrentHealthPotion is not null, and to check once every battle. 
            if (CurrentHealthPotion == null)
            {
                if (LastTargetHPPot == null || Me.CurrentTarget.Guid != LastTargetHPPot) //Meaning they are not the same. 
                {
                    LastTarget = Me.CurrentTarget.Guid; // set guid to current target. 
                    List<WoWItem> HPPot =
                    (from obj in
                         Me.BagItems.Where(
                             ret => ret != null && ret.BaseAddress != null &&
                             (ret.ItemInfo.ItemClass == WoWItemClass.Consumable) &&
                             (ret.ItemInfo.ContainerClass == WoWItemContainerClass.Potion) &&
                             (ret.ItemSpells[0].ActualSpell.SpellEffect1.EffectType == WoWSpellEffectType.Heal))
                     select obj).ToList();
                    if (HPPot.Count > 0)
                    {

                        //on first check, set CurrentHealthPotion so we dont keep running the list looking for one, 
                        CurrentHealthPotion = HPPot.FirstOrDefault();
                        Logging.Write("Potion Found {0}", HPPot.FirstOrDefault().Name);
                        return true;

                    }
                }


                return false;
            }
            else
            {
                return true;
            }
        }

        public bool HealthPotionReady()
        {
            if (CurrentHealthPotion != null && CurrentHealthPotion.BaseAddress != null)
            {
                if (CurrentHealthPotion.Cooldown == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void UseHealthPotion()
        {
            if (CurrentHealthPotion != null && CurrentHealthPotion.BaseAddress != null)
            {
                if (CurrentHealthPotion.Cooldown == 0)
                {
                    Logging.Write("[DWCC]: Low HP! Using --> {0} <--!", CurrentHealthPotion.Name.ToString());
                    CurrentHealthPotion.Use();
                }
            }
        }

        public static WoWItem HealthStone;
        public static bool HaveHealthStone()
        {

            if (HealthStone == null)
            {
                foreach (WoWItem item in Me.BagItems)
                {
                    if (item.Entry == 5512)
                    {
                        HealthStone = item;
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool HealthStoneNotCooldown()
        {
            if (HealthStone != null && HealthStone.BaseAddress != null)
            {

                if (HealthStone.Cooldown == 0)
                {
                    return true;
                }

            }
            return false;
        }

        public static void UseHealthStone()
        {
            if (HealthStone != null && HealthStoneNotCooldown())
            {
                Logging.Write("[DWCC]: Swallowing the green pill! Using --> Healthstone <--");
                HealthStone.Use();
            }
        }
        #endregion

        #region LifeSpirit
        public bool LifeSpiritRegen()
        {
            if ((Me.CurrentHealth <= (Me.MaxHealth - 60000)) && HaveLifeSpirit() && LifeSpiritNotCooldown() && !Me.Mounted && !Me.OnTaxi && !Me.IsFlying && !DunatanksSettings.Instance.pureDPS)
            {
                UseLifeSpirit();
                return true;
            }
            return false;
        }

        public static WoWItem LifeSpirit;
        public static bool HaveLifeSpirit()
        {

            if (LifeSpirit == null)
            {
                foreach (WoWItem item in Me.BagItems)
                {
                    if (item.Entry == 89640)
                    {
                        LifeSpirit = item;
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool LifeSpiritNotCooldown()
        {
            if (LifeSpirit != null && LifeSpirit.BaseAddress != null)
            {

                if (LifeSpirit.Cooldown == 0)
                {
                    return true;
                }

            }
            return false;
        }

        public static void UseLifeSpirit()
        {
            if (LifeSpirit != null && LifeSpiritNotCooldown())
            {
                Logging.Write("[DWCC]: Using --> Life Spirit <--");
                LifeSpirit.Use();
            }
        }
        #endregion

        #region Racials
        public Composite UseRacialComposite()
        {
            return new Decorator(ret => !Me.IsDead && DunatanksSettings.Instance.UseRacials && !DunatanksSettings.Instance.pureDPS,
                                 new PrioritySelector(
                        new Decorator(
                            ret => Me.Race == WoWRace.Human && IsPvPCrowdControlled(Me) && SpellManager.HasSpell("Every Man for Himself") && !SpellManager.Spells["Every Man for Himself"].Cooldown && SpellManager.CanCast("Every Man for Himself"),
                            new Action(ret => SpellManager.Cast("Every Man for Himself"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.NightElf && Me.IsInMyPartyOrRaid && (DunatanksSettings.Instance.useArms || DunatanksSettings.Instance.useFury) && Me.CurrentTarget.Aggro && SpellManager.HasSpell("Shadowmeld") && !SpellManager.Spells["Shadowmeld"].Cooldown && SpellManager.CanCast("Shadowmeld"),
                            new Action(ret => SpellManager.Cast("Shadowmeld"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.Dwarf && StoneFormCheck(Me) && SpellManager.HasSpell("Stoneform") && !SpellManager.Spells["Stoneform"].Cooldown && SpellManager.CanCast("Stoneform"),
                            new Action(ret => SpellManager.Cast("Stoneform"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.Gnome && EscapeArtistCheck(Me) && SpellManager.HasSpell("Escape Artist") && !SpellManager.Spells["Escape Artist"].Cooldown && SpellManager.CanCast("Escape Artist"),
                            new Action(ret => SpellManager.Cast("Escape Artist"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.Worgen && (Me.CurrentTarget.Distance > 35 || (Me.CurrentTarget.Distance > 20 && IsFleeing(Me.CurrentTarget))) && SpellManager.HasSpell("Darkflight") && !SpellManager.Spells["Darkflight"].Cooldown && SpellManager.CanCast("Darkflight"),
                            new Action(ret => SpellManager.Cast("Darkflight"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.Draenei && Me.HealthPercent < 70 && SpellManager.HasSpell("Gift of the Naaru") && !SpellManager.Spells["Gift of the Naaru"].Cooldown && SpellManager.CanCast("Gift of the Naaru"),
                            new Action(ret => SpellManager.Cast("Gift of the Naaru"))),
                //Horde
                        new Decorator(
                            ret => Me.Race == WoWRace.Orc && Me.Combat && SpellManager.HasSpell("Blood Fury") && !SpellManager.Spells["Blood Fury"].Cooldown && SpellManager.CanCast("Blood Fury"),
                            new Action(ret => SpellManager.Cast("Blood Fury"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.Tauren && Me.Combat && AoECastingAdds().Count > 0 && SpellManager.HasSpell("War Stomp") && !SpellManager.Spells["War Stomp"].Cooldown && SpellManager.CanCast("War Stomp") && !Me.IsMoving,
                            new Action(ret => SpellManager.Cast("War Stomp"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.Undead && ForsakenCheck(Me) && SpellManager.HasSpell("Will of the Forsaken") && !SpellManager.Spells["Will of the Forsaken"].Cooldown && SpellManager.CanCast("Will of the Forsaken"),
                            new Action(ret => SpellManager.Cast("Will of the Forsaken"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.Troll && Me.Combat && SpellManager.CanCast("Berserking"),
                            new Action(ret => SpellManager.Cast("Berserking"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.BloodElf && Me.Combat && AoECastingAdds().Count > 0 && SpellManager.HasSpell("Arcane Torrent") && !SpellManager.Spells["Arcane Torrent"].Cooldown && SpellManager.CanCast("Arcane Torrent"),
                            new Action(ret => SpellManager.Cast("Arcane Torrent"))),
                        new Decorator(
                            ret => Me.Race == WoWRace.Goblin && Me.Combat && Me.CurrentTarget.Attackable && Me.CurrentTarget.IsHostile && SpellManager.HasSpell("Rocket Barrage") && !SpellManager.Spells["Rocket Barrage"].Cooldown && SpellManager.CanCast("Rocket Barrage"),
                            new Action(ret => SpellManager.Cast("Rocket Barrage", Me.CurrentTarget)))
                                     ));
        }
        #endregion

        #region AreaSpells
        #region Bool
        public bool AreaSpells()
        {
            if (StyxWoW.Me.HasAura(130774) ||
        StyxWoW.Me.HasAura(116040) ||
        StyxWoW.Me.HasAura(116583) ||
        StyxWoW.Me.HasAura(116586) ||
        StyxWoW.Me.HasAura(116924) ||
        StyxWoW.Me.HasAura(119610) ||
        StyxWoW.Me.HasAura(13810) ||
        StyxWoW.Me.HasAura(43265))
            {
                Logging.WriteDiagnostic("[DWCC]: Standing in AoE: true.");
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region FindHealers
        public List<WoWPlayer> Healers
        {
            get
            {
                if (!StyxWoW.Me.GroupInfo.IsInParty)
                    return new List<WoWPlayer>(); ;

                return StyxWoW.Me.GroupInfo.RaidMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Healer))
                    .Select(p => p.ToPlayer())
                    .Where(p => p != null && p.IsAlive && p.IsFriendly && (!p.HasAura(130774) || !p.HasAura(116040) || !p.HasAura(116583) || !p.HasAura(116586) || !p.HasAura(116924) || !p.HasAura(119610) || !p.HasAura(13810) || !p.HasAura(43265)) && Navigator.CanNavigateFully(Me.Location, p.Location)).ToList();
            }
        }
        #endregion
        #region MoveOut
        public Composite MoveOutOfAoE()
        {
            return new Decorator(ret => AreaSpells() && !DunatanksSettings.Instance.DisableMovement && !(Me.Specialization == WoWSpec.WarriorProtection) && DunatanksSettings.Instance.MoveOutOfAoE,
                                 new Action(delegate
                                 {
                                     Logging.Write("[DWCC]: Moving out of AoE.");
                                     //Logging.Write("[DWCC]: Healer: " + Healers.FirstOrDefault().ToString() + " | CanNav: " + Navigator.CanNavigateFully(Me.Location, Healers.FirstOrDefault().Location));
                                     Navigator.PlayerMover.MoveTowards(Healers.FirstOrDefault().Location);
                                 }
                                ));
        }

        public Composite StopAoEMovement()
        {
            {
                return new Decorator(ret => !AreaSpells() && !DunatanksSettings.Instance.DisableMovement && MoveOutOfAoE().IsRunning && Me.IsMoving,
                                     new Action(delegate
                                     {
                                         Navigator.PlayerMover.MoveStop();
                                     }
                                    ));
            }
        }
        #endregion
        #endregion

        #region Survival
        #region FindShield
        WoWItem Shield;
        public bool HaveShield()
        {
                if (Shield == null) 
                {
                    List<WoWItem> Shields =
                    (from obj in
                         Me.BagItems.Where(
                             ret => ret != null && ret.BaseAddress != null &&
                             (ret.ItemInfo.ItemClass == WoWItemClass.Armor) &&
                             (ret.ItemInfo.ArmorClass == WoWItemArmorClass.Shield))
                     select obj).ToList();
                    if (Shields.Count > 0)
                    {
                        Shields.Sort(CompareShieldsByArmor);
                        //on first check, set Shield so we dont keep running the list looking for one, 
                        Shield = Shields.FirstOrDefault();
                        Logging.Write("[DWCC]: Shield found: {0}", Shields.FirstOrDefault().Name);
                        return true;
                    }
                return false;
            }
            else
            {
                return true;
            }
        }
        #region SortShieldByArmor
        private static int CompareShieldsByArmor(WoWItem x, WoWItem y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y.ItemInfo.Armor == null)
                {
                    return 1;
                }
                else
                {
                    int retval = x.ItemInfo.Armor.CompareTo(y.ItemInfo.Armor);

                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        return x.CompareTo(y);
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Find1h
        WoWItem OneHander;
        public bool HaveOneHander()
        {
            if (OneHander == null)
            {
                List<WoWItem> OneHanders =
                (from obj in
                     Me.BagItems.Where(
                         ret => ret != null && ret.BaseAddress != null &&
                         (ret.ItemInfo.ItemClass == WoWItemClass.Weapon) &&
                         ((ret.ItemInfo.WeaponClass == WoWItemWeaponClass.Axe) ||
                         (ret.ItemInfo.WeaponClass == WoWItemWeaponClass.Mace) ||
                         (ret.ItemInfo.WeaponClass == WoWItemWeaponClass.Sword) ||
                         (ret.ItemInfo.WeaponClass == WoWItemWeaponClass.Dagger)) &&
                         ret.ItemInfo.IsWeapon
                         )
                 select obj).ToList();
                if (OneHanders.Count > 0)
                {
                    OneHanders.Sort(CompareOneHandersByDPS);
                    //on first check, set Shield so we dont keep running the list looking for one, 
                    OneHander = OneHanders.FirstOrDefault();
                    Logging.Write("[DWCC]: OneHander found: {0}", OneHanders.FirstOrDefault().Name);
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        #region CompareOneHandersByDPS
        private static int CompareOneHandersByDPS(WoWItem x, WoWItem y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y.ItemInfo.DPS == null)
                {
                    return 1;
                }
                else
                {
                    int retval = x.ItemInfo.DPS.CompareTo(y.ItemInfo.DPS);

                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        return x.CompareTo(y);
                    }
                }
            }
        }
        #endregion

        #endregion

        #region ChangeWeapons
        #region StoreTwoHander
        WoWItem TwoHander;
        public bool StoreTwoHander()
        {
            if (TwoHander == null)
            {
                if (Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass == WoWItemWeaponClass.AxeTwoHand)
                {
                    TwoHander = Me.Inventory.Equipped.MainHand;
                    Logging.Write("[DWCC]: Found 2H weapon: " + Me.Inventory.Equipped.MainHand.Name);
                    return true;
                }
                else if (Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass == WoWItemWeaponClass.MaceTwoHand)
                {
                    TwoHander = Me.Inventory.Equipped.MainHand;
                    Logging.Write("[DWCC]: Found 2H weapon: " + Me.Inventory.Equipped.MainHand.Name);
                    return true;
                }
                else if (Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass == WoWItemWeaponClass.Polearm)
                {
                    TwoHander = Me.Inventory.Equipped.MainHand;
                    Logging.Write("[DWCC]: Found 2H weapon: " + Me.Inventory.Equipped.MainHand.Name);
                    return true;
                }
                else if (Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass == WoWItemWeaponClass.SwordTwoHand)
                {
                    TwoHander = Me.Inventory.Equipped.MainHand;
                    Logging.Write("[DWCC]: Found 2H weapon: " + Me.Inventory.Equipped.MainHand.Name);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        public Composite EquipOneHanderAndShield(CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && HaveShield() && HaveOneHander() && StoreTwoHander() && Me.Inventory.Equipped.MainHand != OneHander,
                                new Action(delegate
                                {
                                    new Action(ret => EquipItem(OneHander, 16));
                                    new Action(ret => EquipItem(Shield, 17));
                                    new Action(ret => Logging.Write("[DWCC]: Equipping 1H + Shield"));
                                    return RunStatus.Success;
                                }
                                    ));
        }
        public Composite EquipTwoHander(CanRunDecoratorDelegate extra)
        {
            return new Decorator(ret => extra(ret) && StoreTwoHander() && Me.Inventory.Equipped.MainHand != TwoHander,
                                new Action(delegate
                                {
                                    new Action(ret => EquipItem(TwoHander, 16));
                                    new Action(ret => Logging.Write("[DWCC]: Equipping 2H"));
                                    return RunStatus.Success;
                                }
                                    ));
        }
        #endregion

        #region Equip Item
        private static void EquipItem(WoWItem item, int targetSlot)
        {
            Lua.DoString("RunMacroText(\"/equipslot " + targetSlot + " " + item.Name);
        }
        #endregion
        #endregion

        #region SynapseSprings
        private static Composite UseEquippedItem(uint slot)
        {
            return new PrioritySelector(
                       ctx => StyxWoW.Me.Inventory.GetItemBySlot(slot),
                       new Decorator(
                           ctx => ctx != null && CanUseEquippedItem((WoWItem)ctx),
                           new Action(ctx => UseItem((WoWItem)ctx))));
        }

        private static Composite UseItem(uint id)
        {
            return new PrioritySelector(
                       ctx => ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(item => item.Entry == id),
                       new Decorator(
                           ctx => ctx != null && CanUseItem((WoWItem)ctx),
                           new Action(ctx => UseItem((WoWItem)ctx))));
        }

        private static void UseItem(WoWItem item)
        {
            if (item != null)
            {
                item.Use();
            }
        }

        private static bool CanUseItem(WoWItem item)
        {
            return item != null && item.Usable && item.Cooldown <= 0;
        }

        private static bool CanUseEquippedItemSynapse(WoWItem item)
        {
            try
            {
                var itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
                if (string.IsNullOrEmpty(itemSpell))
                    return false;

                return item.Usable && item.Cooldown <= 0;
            }
            catch
            {
                return false;
            }

        }

        private static Composite SynapseSprings()
        {
            return new PrioritySelector(
                new Decorator(ret => DunatanksSettings.Instance.UseSynapseSpringsOnBurst && Me.Combat && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange, UseEquippedItem(9)),
                new Decorator(ret => DunatanksSettings.Instance.UseSynapseSpringsOnBurst && Me.HasAura("Recklessness") && Me.Combat && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange, UseEquippedItem(9)));
        }
        #endregion

        #region Write Settings.xml to log
        public void WriteSettingsToLog()
        {
            TextReader SettingsReader = new StreamReader(Path.Combine(Styx.Common.Utilities.AssemblyDirectory, "Settings", string.Format(@"DWCC-{0}-{1}.xml", StyxWoW.Me.Name, StyxWoW.Me.RealmName)));
            string line;
            while ((line = SettingsReader.ReadLine()) != null)
                Logging.WriteDiagnostic(line);
        }
        #endregion
    }
}