#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Monk/Mistweaver.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using JetBrains.Annotations;
using Oracle.Core;
using Oracle.Core.Hooks;
using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.Spells.Auras;
using Oracle.Core.WoWObjects;
using Oracle.UI.Settings;
using Styx;
using Styx.Common.Helpers;
using Styx.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Linq;

namespace Oracle.Classes.Monk
{
    [UsedImplicitly]
    internal class Mistweaver : RotationBase
    {
        static Mistweaver()
        {
            DisplayMessage("Heya, I noticed you have glyph of Targeted Expulsion, You may want to change the default settings for Expel harm as they are setup to be cast on you.", "Heads Up!", Me.Specialization == WoWSpec.MonkMistweaver && TalentManager.HasGlyph("Targeted Expulsion"));
        }

        #region Inherited Members

        public override string Name
        {
            get { return "Mistweaver Monk"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.MonkMistweaver; }
        }

        public override Composite Medic
        {
            get { return new PrioritySelector(); }
        }

        public override Composite PreCombat
        {
            get { return new PrioritySelector(); }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(

                    DispelManager.CreateDispelBehavior(),

                    //Stop casting healing spells if our heal target is above a certain health percent!
                    Spell.StopCasting(),

                    MonkCommon.StopCastingManaTea(),

                    HandleJadeSerpentStatue(),

                    HandleDefensiveCooldowns(),             // Cooldowns.

                    PrioritySpells(),

                    // Do some damage while we wait for a healtarget.
                    new Decorator(r => !MonkCommon.ChannelingSoothingMist && ((OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 99) && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 99)), PostHeals()),

                    // Don't do anything if we're Channeling casting, or we're waiting for the GCD.
                    new Decorator(a => !MonkCommon.ChannelingSoothingMist && (StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown() || PvPSupport() || ((OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 99) && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 99))), new ActionAlwaysSucceed()),

                    TrinketManager.CreateTrinketBehaviour(),

                    // Cooldowns here..
                    OracleHooks.ExecCooldownsHook(),        // Evaluate the Cooldown healing PrioritySelector hook

                    HandleOffensiveCooldowns(),

                    // Shit that needs priority over AoE healing.
                    PreHeals(),

                    //AoE Spells..
                    new Decorator(r => OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 60, OracleHooks.ExecClusteredHealHook()),

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

        private Composite PrioritySpells()
        {
            return new PrioritySelector(
                    CooldownTracker.Cast(RenewingMist, on => Tank.HasAnyAura(RenewingMists) ? HealTarget : Tank, ret => !CooldownTracker.SpellOnCooldown(RenewingMist), 0.5, true, true), // Get this shit out!!
                    Spell.Cast(ChiWave, on => Me, ret => Me.Combat), // Get this shit out!!
                    Spell.Heal("Expel Harm", on => TalentManager.HasGlyph("Targeted Expulsion") ? HealTarget : Me, MonkCommon.ExpelHarmPct, ret => Me.CurrentChi < MonkCommon.ExpelHarmChiCount),
                    Spell.Cast(Uplift, on => Tank, ret => (MonkCommon.EnableUpliftOveride && Me.CurrentChi >= MonkCommon.ChiCountUpliftOveride && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 70)) || (MonkCommon.UseUpliftMaxChi && Me.CurrentChi == MonkCommon.MaxChi), 0.5, true, true), // Get this shit out!!
                    CooldownTracker.Cast(ChiBrew, on => null, ret => MonkCommon.HasTalent(MonkTalents.ChiBrew) && Me.CurrentChi < MonkCommon.ChiBrewCount && !CooldownTracker.SpellOnCooldown(ChiBrew) && Me.Combat, 0.5, true, true),
                    CooldownTracker.Cast("Mana Tea", on => Me, ret => MonkCommon.NeedManaTea),
                    CooldownTracker.Cast(ThunderFocusTea, on => Me, ret => (MonkCommon.ThunderFocusTeaOnCooldown && !CooldownTracker.SpellOnCooldown(ThunderFocusTea)) || (MonkCommon.UseThunderFocusTeaMaxChi && Spell.GetSpellCooldown(RenewingMist).TotalSeconds > 3 && Me.CurrentChi == MonkCommon.MaxChi) && Me.Combat, 0.5, true, true) // Get this shit out!!
                );
        }

        private static Composite PreHeals()
        {
            return new PrioritySelector(
                    Spell.Heal("Surging Mist", MonkCommon.SurgingVitalMistPercent, ret => Me.HasAura(VitalMists, 5, true, 1000)));
        }

        private static Composite PostHeals()
        {
            return new Decorator(ret => Me.Combat, new PrioritySelector(HandleSoothingMist(), HandleFistLucidity(), HandleFistwithMana(), HandleFistwithNoMana()));
        }

        private static Composite HandleFistwithMana()
        {
            return new Decorator(ret => Me.ManaPercent > MonkCommon.FistWeaveMana,
                                new PrioritySelector(
                                     Spell.Cast("Jab", on => Me.CurrentTarget, ret => !Me.HasAura("Muscle Memory") && Me.CurrentChi < MonkCommon.MaxChi),
                                     Spell.Cast("Tiger Palm", on => Me.CurrentTarget, ret => Me.HasAura("Serpent's Zeal") && Me.CurrentChi > 1),
                                     Spell.Cast("Blackout Kick", on => Me.CurrentTarget, ret => Me.HasAura("Muscle Memory") && Me.HasAura("Tiger Power") && Me.CurrentChi > 2),
                                     Spell.Cast("Tiger Palm", on => Me.CurrentTarget, ret => Me.HasAura("Muscle Memory") && Me.CurrentChi > 1)));
        }

        private static Composite HandleFistwithNoMana()
        {
            return new Decorator(ret => Me.ManaPercent < MonkCommon.FistWeaveMana,
                                new PrioritySelector(
                                     Spell.Cast("Tiger Palm", on => Me.CurrentTarget, ret => !Me.HasAura("Tiger Power") && Me.CurrentChi > 1),
                                     Spell.Cast("Blackout Kick", on => Me.CurrentTarget, ret => !Me.HasAura("Serpent's Zeal") && Me.CurrentChi > 2)));
        }

        private static Composite HandleFistLucidity()
        {
            return new Decorator(ret => StyxWoW.Me.HasAnyAura(Lucidity),
                    new Sequence(
                            Spell.Cast("Jab", on => Me.CurrentTarget),
                            Spell.Cast("Tiger Palm", on => Me.CurrentTarget),
                            Spell.Cast("Jab", on => Me.CurrentTarget)));
        }

        private static Composite HandleSoothingMist()
        {
            return new Decorator(r => MonkCommon.EnableSoothingSpam && Me.Combat && Me.CurrentChi < MonkCommon.MaxChi, Spell.Cast("Soothing Mist", on => Tank));
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                        new Decorator(ret => Me.Combat, HandleCombatCooldowns()),
                        Spell.HoT("Life Cocoon", on => Tank, MonkCommon.LifeCocoonPct, ret => MonkCommon.EnableLifeCocoon),
                        Item.UseBagItem("Healthstone", ret => Me.HealthPercent < OracleSettings.Instance.HealthStonePct, "Healthstone"));
            // Spell.Cast("Nimble Brew", on => Me, ret => UseNimbleBrew && Me.Stunned || Me.Fleeing || Me.HasAuraWithMechanic(WoWSpellMechanic.Horrified))
        }

        private static Composite HandleCombatCooldowns()
        {
            return new PrioritySelector(
                         Spell.Cast("Dampen Harm", on => Me, ret => Me.HealthPercent < MonkCommon.DampenHarmPct),
                         Spell.Cast("Diffuse Magic", on => Me, ret => Me.HealthPercent < MonkCommon.DampenHarmPct),
                         Spell.Cast("Fortifying Brew", on => Me, ret => !Me.HasAura("Fortifying Brew") && Me.HealthPercent < MonkCommon.FortifyingBrewPct));
        }

        #region Serpent Statue

        private static readonly WaitTimer SerpentStatueTimer = new WaitTimer(TimeSpan.FromSeconds(MonkCommon.SerpentStatueWaitTime));

        private static bool NeedSerpentStatue()
        {
            if (!MonkCommon.EnableSerpentStatueUsage)
                return false;
            
            // Dont do shit if its not been x seconds.
            if (!SerpentStatueTimer.IsFinished)
                return false;

            // derp..
            if (!Me.Combat || Me.IsMoving)
                return false;

            // Spell on cooldown derp..BUG: Expensive
            if (CooldownTracker.SpellOnCooldown("Summon Jade Serpent Statue"))
                return false;

            // Grab dat..fast!
            var serpentStatue = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().FirstOrDefault(p => p.CreatedByUnitGuid == Me.Guid && p.Entry == 60849); //BUG: CreatedByUnitGuid = Expensive

            // if we dont have one..then get dat shit.
            if (serpentStatue == null)
            {
                SerpentStatueTimer.Reset();
                return true;
            }

            // Distance...
            if (serpentStatue.Distance > MonkCommon.MaxSerpentStatueDistance)
            {
                SerpentStatueTimer.Reset();
                return true;
            }

            return false;
        }

        private static Composite HandleJadeSerpentStatue()
        {
            return new Decorator(ret => MonkCommon.EnableSerpentStatueUsage && NeedSerpentStatue(),
                CooldownTracker.CastOnGround("Summon Jade Serpent Statue", on => WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, Tank.Location, 5f), ret => OracleRoutine.IsViable(Tank) && !Tank.IsMe));
        }

        #endregion Serpent Statue

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                Racials.UseRacials(),
                Item.UsePotion());
        }

        #endregion Composites
    }
}