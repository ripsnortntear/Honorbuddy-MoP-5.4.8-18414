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
        #region Rotation
        public Composite PvP()
        {
            return new Decorator(ret => Me.Level == 90 && DunatanksSettings.Instance.usePvPRotation,
                new PrioritySelector(
                    EquipTwoHander(ret => Me.HealthPercent > 50),
                    FearBreak(),
                    SafeguardRoots(),
                    ShatteringThrow(),
                    PvPStanceChange(),
                    UseTrinketOnePvP(),
                    UseTrinketTwoPvP(),
                    InterruptHealCast(),
                    Slow(),
                    Disarm(),
                    Fear(),
                    Survive(),
                    SpellReflect(),
                    CreateSpellCheckAndCast("Victory Rush", ret => !SpellManager.HasSpell("Impending Victory")),
                    CreateSpellCheckAndCast("Sweeping Strikes", ret => detectPlayerAdds().Count > 1),
                    CreateSpellCheckAndCast("Recklessness", ret => !ExecutePhase || (ExecutePhase && ((ColossusSmash5s || ColossusSmashCD < 1500) && BloodBathCD < 1)) && (DunatanksSettings.Instance.ArmsReckOnCD || (DunatanksSettings.Instance.ArmsReckOnBoss && (Me.CurrentTarget.IsBoss || Me.CurrentTarget.Name.Contains("Dummy"))))),
                    CreateSpellCheckAndCast("Bloodbath"),
                    CreateSpellCheckAndCast("Avatar", ret => (DunatanksSettings.Instance.ArmsAvaOnCD || (DunatanksSettings.Instance.ArmsAvaOnBoss && (Me.CurrentTarget.IsBoss || Me.CurrentTarget.Name.Contains("Dummy"))))),
                    CreateSpellCheckAndCast("Skull Banner", ret => (DunatanksSettings.Instance.ArmsSB && (Me.CurrentTarget.IsBoss || Me.CurrentTarget.Name.Contains("Dummy")))),
                    ArmsRotation()));


        }
        #endregion
        
        #region Trinkets
        public Composite UseTrinketTwoPvP()
        {

            return new Decorator(ret => CheckTrinketTwo() && StyxWoW.Me.Inventory.Equipped.Trinket2.Cooldown == 0 && (DunatanksSettings.Instance.UseTrinketOneOnCd && Me.HasAura("Recklessness") && Me.CurrentTarget.Distance < 3.5f) || (DunatanksSettings.Instance.UseTrinketTwoBelow20 && Me.CurrentTarget.HealthPercent < 20) || (IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketTwoCC) || ((Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketTwoHero),
                                 new Action(a =>
                                 {
                                     StyxWoW.Me.Inventory.Equipped.Trinket2.Use();
                                     Logging.Write("[DWCC]: Using " + TrinketTwo.Name + " <--");
                                     return RunStatus.Failure;
                                 }
                                     ));
        }

        public Composite UseTrinketOnePvP()
        {
            return new Decorator(ret => CheckTrinketOne() && StyxWoW.Me.Inventory.Equipped.Trinket1.Cooldown == 0 && (DunatanksSettings.Instance.UseTrinketOneOnCd && Me.HasAura("Recklessness") && Me.CurrentTarget.Distance < 3.5f) || (DunatanksSettings.Instance.UseTrinketOneBelow20 && Me.CurrentTarget.HealthPercent < 20) || (IsPvPCrowdControlled(Me) && DunatanksSettings.Instance.useTrinketOneCC) || ((Me.HasAura("Bloodlust") || Me.HasAura("Heroism") || Me.HasAura("Time Warp")) && DunatanksSettings.Instance.UseTrinketOneHero),
                                 new Action(a =>
                                 {
                                     StyxWoW.Me.Inventory.Equipped.Trinket1.Use();
                                     Logging.Write("[DWCC]: Using " + TrinketOne.Name + " <--");
                                     return RunStatus.Failure;
                                 }
                                     ));
        }
        #endregion

        #region Mechanic Detection
        public bool HasAuraWithMechanic(WoWUnit unit, WoWSpellMechanic mechanic)
        {
            return unit.GetAllAuras().Any(
                a => a.Spell.Mechanic.Equals(mechanic)
                );
        }

        public bool IsPvPCrowdControlled(WoWUnit unit)
        {
            return Me.GetAllAuras().Any(
                a => a.IsHarmful &&
                     (a.Spell.Mechanic == WoWSpellMechanic.Shackled ||
                      a.Spell.Mechanic == WoWSpellMechanic.Polymorphed ||
                      a.Spell.Mechanic == WoWSpellMechanic.Horrified ||
                      a.Spell.Mechanic == WoWSpellMechanic.Rooted ||
                      a.Spell.Mechanic == WoWSpellMechanic.Frozen ||
                      a.Spell.Mechanic == WoWSpellMechanic.Stunned ||
                      a.Spell.Mechanic == WoWSpellMechanic.Fleeing ||
                      a.Spell.Mechanic == WoWSpellMechanic.Banished ||
                      a.Spell.Mechanic == WoWSpellMechanic.Sapped
                      ));
        }
        #endregion

        #region HealInterrupt
        public Composite InterruptHealCast()
        {
            return new Decorator(ret => Me.CurrentTarget != null && Me.CurrentTarget.IsCastingHealingSpell,
                new PrioritySelector(
                    new Decorator(ret => Me.CurrentTarget.IsWithinMeleeRange,
                       CreateSpellCheckAndCast("Pummel", ret => true)),
                    new Decorator(ret => (Me.CurrentTarget.Distance > 5f && !SpellManager.CanCast("Charge")) || (Me.CurrentTarget.Distance > 15f && !SpellManager.CanCast("Charge")),
                        CreateSpellCheckAndCast("Heroic Throw", ret => true))));
        }
        #endregion

        #region Shattering Throw
        public Composite ShatteringThrow()
        {
            return new Decorator(ret => Me.CurrentTarget != null && Me.CurrentTarget.HasAura("Divine Shield") || Me.CurrentTarget.HasAura("Ice Block") || Me.CurrentTarget.HasAura("Hand of Protection") && SpellManager.CanCast("Shattering Throw"),
                new Sequence(
                    new Action(ret => Navigator.PlayerMover.MoveStop()),
                    new Action(ret => SpellManager.Cast("Shattering Throw")),
                    new Action(ret => Logging.Write("Shattering Throw")),
                    new Action(ret => RunStatus.Success)
                    ));
        }
        #endregion

        #region XML
        public bool ReadNextXMLData;
        string SnareList;
        string CCList;
        string FearList;
        string RootList;

        public void ReadXML()
        {
            XmlTextReader XmlReader = new XmlTextReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                           string.Format(
                                                               @"Routines/DWCC/SnareList.xml")));
            while (XmlReader.Read())
            {
                if (XmlReader.NodeType == XmlNodeType.Element && XmlReader.Name == "SID")
                {
                    ReadNextXMLData = true;
                }
                if (XmlReader.NodeType == XmlNodeType.Text && ReadNextXMLData)
                {
                    SnareList = SnareList + "[" + XmlReader.Value + "] ";
                    ReadNextXMLData = false;
                }
            }
            XmlReader.Close();

            XmlReader = new XmlTextReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                           string.Format(
                                                               @"Routines/DWCC/CCList.xml")));
            while (XmlReader.Read())
            {
                if (XmlReader.NodeType == XmlNodeType.Element && XmlReader.Name == "SID")
                {
                    ReadNextXMLData = true;
                }
                if (XmlReader.NodeType == XmlNodeType.Text && ReadNextXMLData)
                {
                    CCList = CCList + "[" + XmlReader.Value + "] ";
                    ReadNextXMLData = false;
                }
            }
            XmlReader.Close();

            XmlReader = new XmlTextReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                           string.Format(
                                                               @"Routines/DWCC/FearList.xml")));
            while (XmlReader.Read())
            {
                if (XmlReader.NodeType == XmlNodeType.Element && XmlReader.Name == "SID")
                {
                    ReadNextXMLData = true;
                }
                if (XmlReader.NodeType == XmlNodeType.Text && ReadNextXMLData)
                {
                    FearList = FearList + "[" + XmlReader.Value + "] ";
                    ReadNextXMLData = false;
                }
            }
            XmlReader.Close();

            XmlReader = new XmlTextReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                           string.Format(
                                                               @"Routines/DWCC/RootList.xml")));
            while (XmlReader.Read())
            {
                if (XmlReader.NodeType == XmlNodeType.Element && XmlReader.Name == "SID")
                {
                    ReadNextXMLData = true;
                }
                if (XmlReader.NodeType == XmlNodeType.Text && ReadNextXMLData)
                {
                    RootList = RootList + "[" + XmlReader.Value + "] ";
                    ReadNextXMLData = false;
                }
            }
            XmlReader.Close();
        }
        #endregion

        #region Slow
        public bool TargetIsSlowed()
        {
            if (Me.CurrentTarget.Auras.Values.Where(a => SnareList.Contains("[" + a.SpellId + "]")).FirstOrDefault() != null) return true;
            else return false;
        }

        public Composite Slow()
        {
            return new Decorator(ret => Me.CurrentTarget.IsPlayer && (!HasAuraWithMechanic(Me.CurrentTarget, WoWSpellMechanic.Invulnerable) || !HasAuraWithMechanic(Me.CurrentTarget, WoWSpellMechanic.Invulnerable2)) && !TargetIsSlowed(),
                new PrioritySelector(
                    new Decorator(ret => Me.CurrentTarget != null && Me.CurrentTarget.Distance < 3.5f,
                        new Action(a =>
                        {
                            SpellManager.Cast("Hamstring");
                            Logging.Write("[DWCC]: Hamstring");
                            return RunStatus.Failure;
                        })),
                    new Decorator(ret => SpellManager.CanCast("Piercing Howl") && Me.CurrentTarget.Distance < 10f,
                        new Action(a =>
                        {
                            SpellManager.Cast("Piercing Howl");
                            Logging.Write("[DWCC]: Piercing Howl");
                            return RunStatus.Failure;
                        }))));
        }
        #endregion

        #region Heroic Strike
        //There's some logic behind this. If we've been near ragecap for 3 seconds we'll cast HS no matter how many stacks of tfb we have.
        System.Timers.Timer HSTimer = new System.Timers.Timer();
        public double[] LastRage = new Double[2];
        //initialisation in the Initialize() function!
        public void HSTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            LastRage[0] = LastRage[1];
            LastRage[1] = Me.CurrentRage;
        }
        public void HSUsed()
        {
            LastRage[0] = LastRage[1];
            LastRage[1] = Me.CurrentRage;
        }
        public bool HS()
        {
            if ((LastRage[0] > 95 && LastRage[1] > 95) || (TasteForBloodStacks > 4)) return true;
            else return false;
        }
        public Composite HeroicStrike()
        {
            return new Decorator(ret => SpellManager.CanCast("Heroic Strike") && HS(),
                new Action(a =>
                {
                    SpellManager.Cast("Heroic Strike");
                    HSUsed();
                    return RunStatus.Success;
                }));
        }
        #endregion

        #region Stances
        public Composite PvPStanceChange()
        {
            return new Decorator(
                     ret => !Me.IsDead,
                     new PrioritySelector(
                        CreateSpellCheckAndCast("Defensive Stance", ret => !DefStance, true)));
        }
        #endregion

        #region Survival
        public bool survivalonce;
        public Composite Survive()
        {
            return new Decorator(ret => Me.HealthPercent < 40 && !survivalonce,
                new Sequence(a =>
                {
                    survivalonce = true;
                    Logging.Write("[DWCC]: PvP Survival Sequence");
                    SpellManager.Cast("Die by the Sword");
                    SpellManager.Cast("Rallying Cry");
                    EquipOneHanderAndShield(ret => true);
                    SpellManager.Cast("Spell Reflect");
                    SpellManager.Cast("Shield Wall");
                    EquipTwoHander(ret => true);
                    SpellManager.Cast("Defensive Stance");
                    return RunStatus.Failure;
                }));
        }
        #endregion

        #region FearBreak + Fear
        public bool CanBreakFear()
        {
            if (Me.CurrentTarget.Auras.Values.Where(a => FearList.Contains("[" + a.SpellId + "]")).FirstOrDefault() != null) return true;
            else return false;
        }
        public Composite FearBreak()
        {
            return new Decorator(ret => IsPvPCCCanBreakBR(Me) && SpellManager.CanCast("Berserker Rage"),
                //return new Decorator(ret => HasAuraWithMechanic(Me, WoWSpellMechanic.Fleeing) || HasAuraWithMechanic(Me, WoWSpellMechanic.Sapped),
                new Action(a => { Logging.Write("fearbreak"); SpellManager.Cast("Berserker Rage"); return RunStatus.Failure; }));
        }

        public Composite Fear()
        {
            return new Decorator(ret => detectPlayerAdds().Count > 2 && SpellManager.CanCast("Intimidating Shout"),
                    new Action(a => { SpellManager.Cast("Intimidating Shout"); return RunStatus.Success; }));
        }

        public bool IsPvPCCCanBreakBR(WoWUnit unit)
        {
            return Me.GetAllAuras().Any(
                a => a.IsHarmful &&
                     (a.Spell.Mechanic == WoWSpellMechanic.Fleeing ||
                      a.Spell.Mechanic == WoWSpellMechanic.Incapacitated ||
                      a.Spell.Mechanic == WoWSpellMechanic.Sapped
                      ));
        }
        #endregion

        #region Safeguard out of CC
        WoWUnit preSGTarget = null;

        public bool AmIRooted()
        {
            if (Me.CurrentTarget.Auras.Values.Where(a => RootList.Contains("[" + a.SpellId + "]")).FirstOrDefault() != null) return true;
            else return false;
        }

        public Composite SafeguardRoots()
        {
            return new Decorator(ret => AmIRooted() && SpellManager.CanCast("Safeguard") && SpellManager.CanCast("Demoralizing Banner"),
                new Sequence(
                    new Action(a =>
                    {
                        Logging.Write("Rooted: Safeguard outta here!");
                        SpellManager.Cast("Demoralizing Banner");
                        SpellManager.ClickRemoteLocation(Me.CurrentTarget.Location);
                        preSGTarget = Me.CurrentTarget;
                        return RunStatus.Success;
                    }),
                    new WaitContinue(TimeSpan.FromMilliseconds(100), new ActionAlwaysSucceed()),
                    new Action(a =>
                    {
                        Lua.DoString("RunMacroText(\"/target Demoralizing Banner\")");
                        SpellManager.Cast("Safeguard");
                        preSGTarget.Target();
                        return RunStatus.Success;
                    })));
        }
        #endregion

        #region Spell Reflect
        public List<WoWUnit> detectCasting()
        {
            List<WoWUnit> castingList = ObjectManager.GetObjectsOfType<WoWUnit>(false).FindAll(unit =>
                        unit.Guid != Me.Guid &&
                        !unit.IsFriendly &&
                        unit.CurrentTargetGuid == Me.Guid &&
                        unit.IsCasting &&
                        !unit.CastingSpell.IsSelfOnlySpell &&
                        unit.CanInterruptCurrentSpellCast &&
                        !unit.IsCastingHealingSpell);
            return castingList;
        }

        public Composite SpellReflect()
        {
            return new Decorator(ret => detectCasting().Count > 0 && SpellManager.CanCast("Spell Reflect") || SpellManager.CanCast("Mass Spell Reflection") && Me.HealthPercent < 75,
                new PrioritySelector(
                new Decorator(ret => !SpellManager.CanCast("Mass Spell Reflection") && SpellManager.CanCast("Spell Reflect"),
                    new Action(a =>
                    {
                        Logging.Write("Spell Reflect");
                        Lua.DoString("RunMacroText(\"/equipslot 16 " + OneHander.Name + "\")");
                        Lua.DoString("RunMacroText(\"/equipslot 17 " + Shield.Name + "\")");
                        SpellManager.Cast("Spell Reflect");
                        Lua.DoString("RunMacroText(\"/equipslot 16 " + TwoHander.Name + "\")");
                        return RunStatus.Success;
                    })),
                new Decorator(ret => SpellManager.CanCast("Mass Spell Reflection"),
                    new Action(a =>
                    {
                        Logging.Write("Mass Spell Reflection");
                        SpellManager.Cast("Mass Spell Reflection");
                        return RunStatus.Success;
                    }))));
        }
        #endregion

        #region Disarm
        public bool CanDisarm()
        {
            if (Me.CurrentTarget != null && SpellManager.HasSpell("Disarm") && !SpellManager.Spells["Disarm"].Cooldown && SpellManager.CanCast("Disarm") && Me.CurrentTarget.Distance < MeleeDistance(Me.CurrentTarget))
                if (Me.CurrentTarget.Class == WoWClass.DeathKnight || Me.CurrentTarget.Class == WoWClass.Hunter || Me.CurrentTarget.Class == WoWClass.Rogue || Me.CurrentTarget.Class == WoWClass.Paladin || Me.CurrentTarget.Class == WoWClass.Warrior)
                    if(Me.CurrentTarget.IsPlayer && Me.CurrentTarget.CurrentTarget != null && Me.CurrentTarget.CurrentTarget.Guid == Me.Guid) return true;
            return false;

        }

        public Composite Disarm()
        {
            return new Decorator(ret => CanDisarm(),
                new Action(a =>
                    {
                        Logging.Write("Disarm");
                        SpellManager.Cast("Disarm");
                        return RunStatus.Success;
                    }));
        }
        #endregion
    }
}