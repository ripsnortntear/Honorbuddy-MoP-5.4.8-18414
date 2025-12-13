#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Priest/Discipline.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using JetBrains.Annotations;
using Oracle.Core;
using Oracle.Core.CombatLog;
using Oracle.Core.Hooks;
using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.Spells.Auras;
using Oracle.Core.WoWObjects;
using Oracle.Healing;
using Oracle.UI.Settings;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Classes.Priest
{
    [UsedImplicitly]
    internal partial class Discipline : RotationBase
    {
        static Discipline()
        {
           if(Me.Specialization == WoWSpec.PriestDiscipline) CombatLogHandler.Register("SPELL_ENERGIZE", HandleRapture);
        }

        #region Rapture

        private static readonly WaitTimer RaptureTimer = new WaitTimer(TimeSpan.FromSeconds(12));

        private static DateTime LastRapture { get; set; }

        private static int RaptureCount { get; set; }

        private static bool RaptureReady { get; set; }

        private static void RaptureCheck()
        {
            if (RaptureTimer.IsFinished)
            {
                RaptureReady = true;
                //Logger.Output("LastRapture: {0} : Rapture Count: {1}", LastRapture - DateTime.Now, RaptureCount);
            }
        }

        private static void HandleRapture(CombatLogEventArgs args)
        {
            if (args.DestName == Me.Name && args.SourceName == Me.Name && args.SpellId == 47755)
            {
                RaptureReady = false;
                LastRapture = DateTime.Now;
                RaptureCount = RaptureCount + 1;
                RaptureTimer.Reset();
            }
        }

        #endregion Rapture

        #region Booleons

        private static bool CanShield(WoWUnit u)
        {
            return OracleRoutine.IsViable(u) && (!u.ActiveAuras.ContainsKey("Weakened Soul") /*|| StyxWoW.Me.ActiveAuras.ContainsKey("Divine Insight")*/);
        }

        private static bool FadePvE()
        {
            return !OracleSettings.Instance.PvPSupport && Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 30) > 0;
        }

        private static bool FadePvP()
        {
            return OracleSettings.Instance.PvPSupport && PriestCommon.HasTalent(PriestTalents.Phantasm) && Me.ActiveAuras.Any(aura => aura.Value.ApplyAuraType == WoWApplyAuraType.ModRoot || aura.Value.ApplyAuraType == WoWApplyAuraType.ModDecreaseSpeed);
        }

        #endregion Booleons

        #region Inherited Members

        public override string Name
        {
            get { return "Discipline Priest"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.PriestDiscipline; }
        }

        public override Composite Medic
        {
            get { return new PrioritySelector(); }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                    ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        Spell.Cast("Inner Fire", on => Me, ret => !Me.HasAura("Inner Fire")),
                        Spell.Cast("Power Word: Fortitude", on => Me, ret => !Me.HasAnyAura("Power Word: Fortitude", "Qiraji Fortitude", "Blood Pact", "Commanding Shout") && PriestCommon.UsePowerWordFortitude)));
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(

                    HandleSpiritShell(), // blanket the raid.

                    new Action(delegate { RaptureCheck(); return RunStatus.Failure; }),

                    DispelManager.CreateDispelBehavior(),

                    //Stop casting healing spells if our heal target is above a certain health percent!
                    Spell.StopCasting(),

                    HandleDefensiveCooldowns(),             // Cooldowns.

                    PriestCommon.CreatePriestMovementBuff(),

                    Spell.Heal("Power Word: Shield", on => Tank, 100, ret => RaptureReady && CanShield(Tank)), // maintain rapture!
                    //Spell.Heal("Power Word: Shield", on => SecondTank, 100, ret => CanShield(SecondTank)), // maintain rapture!

                    // Do some damage while we wait for a healtarget.
                     new Decorator(r => !StyxWoW.Me.IsChanneling && ((OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 99) && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 99)), PostHeals()),

                    // Don't do anything if we're Channeling casting, or we're waiting for the GCD.
                    new Decorator(a => StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown() || PvPSupport() || ((OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 99) && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 99)), new ActionAlwaysSucceed()),

                    TrinketManager.CreateTrinketBehaviour(),

                    // Cooldowns here..
                    OracleHooks.ExecCooldownsHook(),        // Evaluate the Cooldown healing PrioritySelector hook

                    HandleOffensiveCooldowns(),
                    PriestCommon.HandleManaCooldowns(),

                    // Shit that needs priority over AoE healing.
                    PreHeals(),

                    //AoE Spells..
                    new Decorator(r => OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 70, OracleHooks.ExecClusteredHealHook()),
                    //OracleHooks.ExecClusteredHealHook(),    // Evaluate the Clustered Healing PrioritySelector

                    //Ordered Single Target Spells
                    OracleHooks.ExecSingleTargetHealHook(), // Evaluate the Single Target Healing PrioritySelector

                    // Everything else.
                    PostHeals());
            }
        }

        public override Composite PVPRotation
        {
            get { return PVERotation; }
        }

        #endregion Inherited Members

        #region Composites

        private static Composite PreHeals()
        {
            return new PrioritySelector(
                    Spell.Heal("Power Word: Shield", on => Tank, PriestCommon.UrgentHealthPercentage, ret => CanShield(Tank)),
                    Spell.Heal("Penance", on => Tank, PriestCommon.UrgentHealthPercentage),
                    Spell.Heal("Prayer of Mending", on => Tank, PriestCommon.PrayerOfMendingPct, ret => Unit.GetbuffCount(PrayerofMendingbuff) < 1), //PoM
                    Spell.Heal("Flash Heal", PriestCommon.FlashHealSoLPct, ret => Me.ActiveAuras.ContainsKey("Surge of Light")),
                    Spell.Heal("Flash Heal", on => Tank, PriestCommon.UrgentHealthPercentage),
                    //Spell.Heal("Power Word: Shield", on => SecondTank, 100, ret => RaptureReady && CanShield(SecondTank)), // maintain rapture!
                    HandleLucidity(),
                    new Decorator(ret => Me.Combat, CreateHolyFireBehavior()),
                    HandleGreaterHeal(PriestCommon.GreaterHealPct)
                );
        }

        private static Composite PostHeals()
        {
            return new Decorator(ret => PriestCommon.EnableOffensiveDps && Me.Combat,
                    new PrioritySelector( // Do some DMG!
                        new Decorator(ret => OracleSettings.Instance.PvPSupport, Spell.Cast("Shadow Word: Death", on => Unit.GetEnemy, ret => OracleRoutine.IsViable(Unit.GetEnemy) && Unit.GetEnemy.HealthPercent < 20 && Unit.FaceTarget(Unit.GetEnemy), 0.5, true)),
                        CreateHolyFireBehavior(),
                        Spell.Cast("Penance", on => Unit.GetEnemy, ret => PriestCommon.UsePenanceOnEnemy && OracleRoutine.IsViable(Unit.GetEnemy) && Unit.FaceTarget(Unit.GetEnemy)),
                        Spell.Cast("Smite", on => Unit.GetEnemy, ret => OracleRoutine.IsViable(Unit.GetEnemy) && Unit.FaceTarget(Unit.GetEnemy))
                ));
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                    Spell.Heal("Desperate Prayer", on => Me, PriestCommon.DesperatePrayerPct),
                    Spell.Heal("Pain Suppression", on => Tank, PriestCommon.PainSuppressionPercent), // should be used on a tank
                    Spell.Heal("Void Shift", on => HealTarget, PriestCommon.VoidShiftPct, ret => Me.HealthPercent > 80), // should be used to save the life of a raid member by trading health percentages with them.
                    Spell.Cast("Fade", on => Me, ret => PriestCommon.UseFade && !StyxWoW.Me.IsChanneling && (FadePvE() || FadePvP())),
                    Item.UseBagItem("Healthstone", ret => Me.HealthPercent < OracleSettings.Instance.HealthStonePct, "Healthstone"));
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(Racials.UseRacials(), Item.UsePotion());
        }

        private static Composite HandleGreaterHeal(int hp)
        {
            return new PrioritySelector( // Use InnerFocus too boost our GH
                new Decorator(ret => OracleHealTargeting.HealthCheck(HealTarget, hp) && !Me.ActiveAuras.ContainsKey("Inner Focus") && !CooldownTracker.SpellOnCooldown("Inner Focus"),
                              new Sequence(
                                  CooldownTracker.Cast("Inner Focus", on => Me),
                                  Spell.CreateWaitForLagDuration(),
                                  Spell.Heal("Greater Heal")))); // dont bother checking HP twice just cast it.
        }

        private static WoWUnit SpiritShellTarget { get; set; }

        private static Composite HandleSpiritShell()
        {
            return new Decorator(ret =>
            {
                // no unit? gtfo
                if (!Me.ActiveAuras.ContainsKey("Spirit Shell"))
                    return false;

                if (StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown())
                    return false;

                return true;
            },
                new Sequence(
                    new Action(ret => SpiritShellTarget = Unit.GetActiveUnbuffedTarget("Spirit Shell")),
                    new PrioritySelector(new Decorator(ret => !OracleRoutine.IsViable(SpiritShellTarget), new ActionAlwaysSucceed()), Spell.Cast("Prayer of Healing", on => SpiritShellTarget))
                    ));
        }

        private static Composite CreateHolyFireBehavior()
        {
            if (PriestCommon.HasTalent(PriestTalents.SolaceAndInsanity)) // BUG: HB is not checking the correct cooldown for "Power Word: Solace"...
                return Spell.Cast("Holy Fire", on => Unit.GetEnemy, ret => !Spell.SpellOnCooldown(PowerWordSolace) && OracleRoutine.IsViable(Unit.GetEnemy) && Unit.FaceTarget(Unit.GetEnemy));

            return Spell.Cast("Holy Fire", on => Unit.GetEnemy, ret => OracleRoutine.IsViable(Unit.GetEnemy) && Unit.FaceTarget(Unit.GetEnemy));
        }

        private static WoWUnit LucidityTarget { get; set; }

        private static Composite HandleLucidity()
        {
            return new Decorator(ret =>
            {
                // no unit? gtfo
                if (!StyxWoW.Me.HasAnyAura(Lucidity))
                    return false;

                if (StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown())
                    return false;

                return true;
            },
                new Sequence(
                    new Action(ret => LucidityTarget = Unit.GetUnbuffedTarget(WeakenedSoul)),
                    new PrioritySelector(new Decorator(ret => !OracleRoutine.IsViable(LucidityTarget), new ActionAlwaysSucceed()), Spell.Cast("Power Word: Shield", on => LucidityTarget))
                    ));
        }

        #endregion Composites
    }
}