#define DEBUG_MODE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using System.Data;
using CommonBehaviors.Actions;
using System.Diagnostics;
using System.Text;
using System.Globalization;
using JetBrains.Annotations;

namespace RichieHolyPriestPvP
{
    public enum SpecTypes : byte
    {
        Invalid = 0,
        Melee = 1,
        Ranged = 2,
        Caster = 3,
        Healer = 4
    }

    public partial class Main
    {
        #region Global Variables

        private static ManaCosts manaCosts;
        private static ManaCosts ManaCosts
        {
            get
            {
                if (manaCosts == null)
                    manaCosts = new ManaCosts();
                return manaCosts;
            }
        }

        private static int LifeSaverCDDelay = 6;

        public static bool IsArena = false;
        private static bool IsBG = false;
        public static bool IsRBG = false;
        private static int FearWardCooldown = 180;
        public static int ChakraCooldown = 5;

        private static DateTime LastUpdateMyLatency;

        private static DateTime LastScan = DateTime.Now;
        private static DateTime LastBurst = DateTime.Now;
        private static DateTime LastDefensiveCD = DateTime.Now;

        private static DateTime LastFeather = DateTime.Now;
        private static DateTime DMcast = DateTime.Now;

        private static int DMDelayMs = 400;

        #endregion

        static Main()
        {
#if DEBUG_MODE
            HolySettings.IsDebugMode = true;
#endif
        }

        #region Helpers

        public static bool Eval(string Name, System.Func<bool> func)
        {
            return func();
        }

        public static WoWUnit GetHealTarget()
        {
            return UnitManager.HealTarget;
        }

        public static WoWUnit GetDamageTarget()
        {
            return UnitManager.DamageTarget;
        }

        public static WoWUnit GetMe()
        {
            return Me;
        }

		#endregion
	
        #region CastSpell

        private static Composite Heal(SpellIDs spellId, Func<bool> condition, string message = null)
        {
            return CastSpell(spellId, unit: GetHealTarget, condition: condition, needFacing: false, onGCD: true, message: message);
        }

        private static Composite CastSpell(SpellIDs spellId, Func<WoWUnit> unit, Func<bool> condition, bool needFacing = true, bool onGCD = true)
        {
            return CastSpell(spellId, unit, condition: condition, needFacing: needFacing, onGCD: onGCD, message: null);
        }

        private static Composite CastSpell(SpellIDs spellId, Func<WoWPoint> location = null, Func<bool> condition = null, System.Action additionalLogic = null, bool onGCD = true, bool needFacing = true, RunStatus returnWhenCasted = RunStatus.Success, string message = null)
        {
            return CastSpell(spellId, GetMe, location, condition, additionalLogic, onGCD, needFacing, returnWhenCasted, message);
        }

        private static Composite CastSpell(SpellIDs spellId, Func<WoWUnit> unit, Func<WoWPoint> location = null, Func<bool> condition = null, System.Action additionalLogic = null, bool onGCD = true, bool needFacing = true, RunStatus returnWhenCasted = RunStatus.Success, string message = null)
        {
            return new TraceDecorator("Spell Cast: " + Spells.Get(spellId), ret =>
                unit != null && 
                (Me.CurrentMana >= ManaCosts[spellId] || Me.HasSpiritOfRedemption()) &&
                Spells.Has(spellId) &&
                !HolyCoolDown.IsCasting() &&
                (condition == null || condition()) &&
                (!onGCD || !HolyCoolDown.IsGlobalCD()),
                new Sequence(
                    new DecoratorContinue(ret => HolySettings.Instance.AutoFace && needFacing && unit() != Me && unit().IsValidUnit() && !Me.IsSafelyFacing(unit()),
                        new TraceSequence("CastSpell - Facing",
                            new Action(ret => unit().Face()),
                            new WaitContinue(TimeSpan.FromMilliseconds(1500), ret => unit() != null && unit().IsValidUnit() && Me.IsSafelyFacing(unit()), new Action(ret => { /*Logging.Write("Yup, we're facing."); */return RunStatus.Success; }))
                        )
                    ),
                    new DecoratorContinue(ret => !HolySettings.Instance.AutoFace && needFacing && unit() != Me && !Me.IsSafelyFacing(unit()),
                        new Action(ret => RunStatus.Failure)
                    ),
                    new TraceAction("CastSpell - Spell Cast", ret => { return SpellManager.Cast((int)spellId, unit()) ? RunStatus.Success : RunStatus.Failure; }),
                    new DecoratorContinue(ret => location != null,
                        new TraceSequence("CastSpell - Cast on Location",
                            new WaitContinue(TimeSpan.FromMilliseconds(200),
                                ret => Me.CurrentPendingCursorSpell != null && Me.CurrentPendingCursorSpell.Id == (int)spellId,
                                new Action(ret => RunStatus.Success/*Logging.Write("Got the targeting circle for " + spellName + ".")*/)
                            ),
                            new Action(ret => SpellManager.ClickRemoteLocation(location())),
                            new Action(ret => Lua.DoString("SpellStopTargeting()"))
                        )
                    ),
                    new Action(ret => {
                        WoWUnit u = unit();
                        if (u.IsValidUnit())
                        {
                            Logging.Write((u == Me && location == null ? Colors.GreenYellow : (location == null && Me.CurrentTarget != null && u == Me.CurrentTarget ? Colors.Red : Colors.Yellow)),
                                DateTime.Now.ToString("ss:fff") + " " + Spells.Get(spellId) + " - HP: " + Math.Round(Me.HealthPercent) + "% - " + "Mana: " + Me.ManaPercent.ToString("F2") + "% - " +
                                    (u == Me ? "Me" : (u.IsPlayer ? u.Name + "(" + u.Class.ToString() + ")" : u.Name)) +
                                    " - " + Math.Round(u.Distance) + "y - " + Math.Round(u.HealthPercent) + "% hp " + (message == null || message.Length == 0 ? "" : "- (" + message + ")"));
                        }

                        // Majd a combat log felulirja, de legalabb addig sem probaljuk ujra elcastolni
                        HolyCoolDown.SetCasting(spellId);
                        return RunStatus.Success;
                    }),
                    new TraceAction("CastSpell - UpdateVariablesAfterSpellcast", ret => {
                        HolyCoolDown.UpdateGCD();

                        if (LastUpdateMyLatency.AddMinutes(0.5) < DateTime.Now)
                        {
                            HolyCoolDown.UpdateLatency();
                            LastUpdateMyLatency = DateTime.Now;
                        }

                        if (additionalLogic != null)
                            additionalLogic();

                        return returnWhenCasted;
                    })
                )
            );
        }

        #endregion
       
        #region CanUseEquippedItem

        //Thanks Apoc
        private static bool CanUseEquippedItem(WoWItem item)
        {
            // Check for engineering tinkers!
            if (string.IsNullOrEmpty(Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0)))
                return false;

            return item.Usable && item.Cooldown <= HolyCoolDown.Latency;
        }

        #endregion
        
        #region  DPS Trinket

        private static Composite DPSTrinket(String cause)
        {
            return new Decorator(ret => HolySettings.Instance.UseTrinket &&
                                        ((HolySettings.Instance.TrinketSlotNumber == 13 && Me.Inventory.Equipped.Trinket1.CooldownTimeLeft.TotalMilliseconds < HolyCoolDown.Latency) ||
                                        (HolySettings.Instance.TrinketSlotNumber == 14 && Me.Inventory.Equipped.Trinket2.CooldownTimeLeft.TotalMilliseconds < HolyCoolDown.Latency)),
                new Action(ret =>{
                        Lua.DoString("RunMacroText('/use " + HolySettings.Instance.TrinketSlotNumber + "');");
                        Logging.Write("Using DPS trinket to " + cause + ".");
                        LastBurst = DateTime.Now;
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region  Synapse Springs

        private static Composite SynapseSprings(string cause)
        {
            return new Decorator(ret => Me.Inventory.Equipped.Hands != null && CanUseEquippedItem(Me.Inventory.Equipped.Hands),
                new Action(ret =>{
                        Me.Inventory.Equipped.Hands.Use();
                        Logging.Write("Used Synapse Springs to " + cause + ".");
                        LastBurst = DateTime.Now;
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        // Heals
        #region UseHealthstone

        private static Composite UseHealthstone()
        {
            return new Decorator(
                ret => Me.Combat && Me.HealthPercent < HolySettings.Instance.HealthstonePercent && LastDefensiveCD.AddSeconds(5) <= DateTime.Now,
                new Action((ctx) =>
                {
                    WoWItem hs = Me.BagItems.FirstOrDefault(o => o.Entry == 5512); //5512 Healthstone
                    if (hs != null && hs.CooldownTimeLeft.TotalMilliseconds <= HolyCoolDown.Latency)
                    {
                        hs.Use();
                        Logging.Write("Use Healthstone at " + Me.HealthPercent + "%");
                        LastDefensiveCD = DateTime.Now;
                    }
                    return RunStatus.Failure;
                }
                )
            );
        }

        #endregion
    }
}