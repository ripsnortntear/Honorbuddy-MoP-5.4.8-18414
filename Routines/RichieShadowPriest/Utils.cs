#define DEBUG_MODE_
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

namespace RichieShadowPriestPvP
{
    public enum SpecTypes : byte
    {
        Invalid = 0,
        Melee = 1,
        Ranged = 2,
        Caster = 3,
        Healer = 4
    }

    enum HotkeyAction : int {
        Nothing = 0,
        Burst = 1,
        PsyfiendTarget = 2,
        PsyfiendFocus = 3,
        AngelicFeatherMe = 4,
        AngelicFeatherVIP = 5
    };

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
        

        public static bool IsArena = false;
        public static bool IsBG = false;
        public static bool IsRBG = false;
        private static int FearWardCooldown = 180;
        private static bool CanUseFadeAsDefCD = false;
        private static uint Orbs = 0;
        private static HotkeyAction NextHotkeyAction = HotkeyAction.Nothing;
        private static bool Burstmode = false;


        private static DateTime LastUpdateMyLatency;
        private static DateTime LastScan = DateTime.Now;
        private static DateTime LastOnGCDCast = DateTime.Now;
        private static DateTime LastBurst = DateTime.Now;
        private static DateTime LastBurstmodeActivated = DateTime.Now;        
        private static DateTime LastDefensiveCD = DateTime.Now;
        private static DateTime LastCC = DateTime.Now;
        private static DateTime LastFeather = DateTime.Now;
        private static DateTime DMcast = DateTime.Now;
        private static DateTime LastVTCastTime = DateTime.Now;
        private static DateTime LastHotkeyPressed = DateTime.Now;
        private static DateTime LastInsanityStopCast = DateTime.Now;

        

        #endregion

        static Main()
        {
#if DEBUG_MODE
            SPSettings.IsDebugMode = true;
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
            return UnitManager.ASAPDamageTarget;
        }

        public static WoWUnit GetMe()
        {
            return Me;
        }

		#endregion
	
        #region CastSpell

        private static Composite Heal(SpellIDs spellId, Func<bool> condition, string message = null) {
            return CastSpell(spellId, unit: GetHealTarget, condition: condition, needFacing: false, onGCD: true, message: message);
        }

        private static Composite CastSpell(SpellIDs spellId, Func<bool> condition, bool needFacing = true, bool onGCD = true) {
            return CastSpell(spellId, unit: GetMe, condition: condition, needFacing: needFacing, onGCD: onGCD, message: null);
        }

        private static Composite CastSpell(SpellIDs spellId, Func<WoWUnit> unit, Func<bool> condition, bool needFacing = true, bool onGCD = true) {
            return CastSpell(spellId, unit, condition: condition, needFacing: needFacing, onGCD: onGCD, message: null);
        }

        private static Composite CastSpell(SpellIDs spellId, Func<WoWPoint> location = null, Func<bool> condition = null, System.Action additionalLogic = null, bool onGCD = true, bool needFacing = true, RunStatus returnWhenCasted = RunStatus.Success, string message = null) {
            return CastSpell(spellId, GetMe, location, condition, additionalLogic, onGCD, needFacing, returnWhenCasted, false, message);
        }

        private static Composite CastSpell(SpellIDs spellId, Func<WoWUnit> unit, Func<WoWPoint> location = null, Func<bool> condition = null, System.Action additionalLogic = null, bool onGCD = true, bool needFacing = true, RunStatus returnWhenCasted = RunStatus.Success, bool ForceCast = false, string message = null) {
            return new TraceDecorator("Spell Cast: " + Spells.Get(spellId), ret =>
                unit != null && 
                (Me.CurrentMana >= ManaCosts[spellId]) &&
                Spells.Has(spellId) &&
                (ForceCast || !SPCoolDown.IsCasting()) &&
                (!onGCD || (onGCD && LastOnGCDCast.AddMilliseconds(200) <= DateTime.Now)) &&
                (!onGCD || !SPCoolDown.IsGlobalCD()) &&
                (condition == null || condition()),
                new Sequence(
                    new DecoratorContinue(ret => ForceCast,
                        new Action(ret => {
                            Lua.DoString("RunMacroText(\"/stopcasting\")");
                            Lua.DoString("RunMacroText(\"/stopcasting\")");
                            Logging.Write("Stopcasting.");
                            return RunStatus.Success;
                        })
                    ),
                    new DecoratorContinue(ret => SPSettings.Instance.AutoFace && needFacing && unit() != Me && unit().IsValidUnit() && !Me.IsSafelyFacing(unit()),
                        new TraceSequence("CastSpell - Facing",
                            new Action(ret => unit().Face()),
                            new WaitContinue(TimeSpan.FromMilliseconds(1500), ret => unit() != null && unit().IsValidUnit() && Me.IsSafelyFacing(unit()), new Action(ret => RunStatus.Success ))
                        )
                    ),
                    new DecoratorContinue(ret => !SPSettings.Instance.AutoFace && needFacing && unit() != Me && !Me.IsSafelyFacing(unit()),
                        new Action(ret => RunStatus.Failure)
                    ),
                    new TraceAction("CastSpell - Spell Cast", ret => { return SpellManager.Cast((int)spellId, unit()) ? RunStatus.Success : RunStatus.Failure; }),
                    new DecoratorContinue(ret => location != null,
                        new TraceSequence("CastSpell - Cast on Location",
                            new WaitContinue(TimeSpan.FromMilliseconds(200),
                                ret => Me.CurrentPendingCursorSpell != null && Me.CurrentPendingCursorSpell.Id == (int)spellId,
                                new Action(ret => RunStatus.Success)
                            ),
                            new Action(ret => SpellManager.ClickRemoteLocation(location())),
                            new Action(ret => Lua.DoString("SpellStopTargeting()"))
                        )
                    ),
                    new Action(ret => {
                        using (var perf = PerfLogger.GetHelper("CastSpell - Log"))
                        {
                            WoWUnit u = unit();
                            if (u.IsValidUnit())
                            {
                                Logging.Write((u == Me && location == null ? Colors.GreenYellow : (location == null && Me.CurrentTarget != null && u == Me.CurrentTarget ? Colors.Red : Colors.Yellow)),
                                    DateTime.Now.ToString("ss:fff") + " - HP: " + Math.Round(Me.HealthPercent) + "% - " + "Mana: " + Math.Round(Me.ManaPercent) + "% - " + "Orbs: " + Orbs + " - " + 
                                        (u == Me ? "Me" : (u.IsPlayer ? u.Name + "(" + u.Class.ToString() + ")" : u.Name)) +
                                        " - " + Math.Round(u.Distance) + "y - " + Math.Round(u.HealthPercent) + "% hp - " + Spells.Get(spellId) + (message == null || message.Length == 0 ? "" : " (" + message + ")"));
                            }
                        }

                        // Majd a combat log felulirja, de legalabb addig sem probaljuk ujra elcastolni
                        SPCoolDown.JustCasted(spellId);
                        if (onGCD)
                            LastOnGCDCast = DateTime.Now;

                        return RunStatus.Success;
                    }),
                    new TraceAction("CastSpell - UpdateVariablesAfterSpellcast", ret => {
                        SPCoolDown.UpdateGCD();

                        if (LastUpdateMyLatency.AddMinutes(0.5) < DateTime.Now)
                        {
                            SPCoolDown.UpdateLatency();
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

            return item.Usable && item.Cooldown <= SPCoolDown.Latency;
        }

        #endregion
        
        #region  DPS Trinket

        private static Composite DPSTrinket(String cause)
        {
            return new Decorator(ret => !SPCoolDown.IsCasting() && SPSettings.Instance.UseTrinketWithDP &&
                                        ((SPSettings.Instance.TrinketSlotNumber == 13 && Me.Inventory.Equipped.Trinket1.CooldownTimeLeft.TotalMilliseconds < SPCoolDown.Latency) ||
                                        (SPSettings.Instance.TrinketSlotNumber == 14 && Me.Inventory.Equipped.Trinket2.CooldownTimeLeft.TotalMilliseconds < SPCoolDown.Latency)),
                new Action(ret =>{
                        Lua.DoString("RunMacroText('/use " + SPSettings.Instance.TrinketSlotNumber + "');");
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
            return new Decorator(ret => !SPCoolDown.IsCasting() && Me.Inventory.Equipped.Hands != null && CanUseEquippedItem(Me.Inventory.Equipped.Hands),
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
                ret => Me.Combat && Me.HealthPercent < SPSettings.Instance.HealthstonePercent && LastDefensiveCD.AddSeconds(SPSettings.Instance.DefensiveCDDelay) <= DateTime.Now,
                new Action((ctx) =>
                {
                    WoWItem hs = Me.BagItems.FirstOrDefault(o => o.Entry == 5512); //5512 Healthstone
                    if (hs != null && hs.CooldownTimeLeft.TotalMilliseconds <= SPCoolDown.Latency)
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

        #region Hotkeys

        public static void RegisterHotkeys() {
            HotkeysManager.Register("Burst", SPSettings.Instance.KeyBurst, SPSettings.Instance.ModBurst, hk => {
                NextHotkeyAction = HotkeyAction.Burst;
                LastHotkeyPressed = DateTime.Now;
            });

            HotkeysManager.Register("PsyfiendOnTarget", SPSettings.Instance.KeyPsyfiendTarget, SPSettings.Instance.ModPsyfiendTarget, hk => {
                NextHotkeyAction = HotkeyAction.PsyfiendTarget;
                LastHotkeyPressed = DateTime.Now;
            });

            HotkeysManager.Register("PsyfiendOnFocus", SPSettings.Instance.KeyPsyfiendFocus, SPSettings.Instance.ModPsyfiendFocus, hk => {
                NextHotkeyAction = HotkeyAction.PsyfiendFocus;
                LastHotkeyPressed = DateTime.Now;
            });

            HotkeysManager.Register("AngelicFeather", SPSettings.Instance.KeyAngelicFeather, SPSettings.Instance.ModAngelicFeather, hk => {
                NextHotkeyAction = HotkeyAction.AngelicFeatherMe;
                LastHotkeyPressed = DateTime.Now;
            });

            HotkeysManager.Register("AngelicFeatherVIP", SPSettings.Instance.KeyAngelicFeatherVIP, SPSettings.Instance.ModAngelicFeatherVIP, hk => {
                NextHotkeyAction = HotkeyAction.AngelicFeatherVIP;
                LastHotkeyPressed = DateTime.Now;
            });

        }

        public static void RemoveHotkeys() {
            HotkeysManager.Unregister("Burst");
            HotkeysManager.Unregister("PsyfiendOnTarget");
            HotkeysManager.Unregister("PsyfiendOnFocus");
            HotkeysManager.Unregister("AngelicFeather");
            HotkeysManager.Unregister("AngelicFeatherVIP");
        }

        public static void ReRegisterHotkeys() {
            RemoveHotkeys();
            RegisterHotkeys();
        }

        #endregion

    }
}