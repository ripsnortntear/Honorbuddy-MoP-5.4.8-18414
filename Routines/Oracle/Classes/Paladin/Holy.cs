#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Paladin/Holy.cs $
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
using Oracle.Healing;
using Oracle.Shared.Logging;
using Oracle.UI.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Linq;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Classes.Paladin
{
    [UsedImplicitly]
    internal class HolyPaladin : RotationBase
    {
        static HolyPaladin()
        {
            DisplayMessage("Hi, I noticed that you do not have Glyph of Beacon of Light. Oracle is setup to handle this nicely, you should re-glyph.", "Missing Glyph of Beacon of Light", Me.Specialization == WoWSpec.PaladinHoly && !TalentManager.HasGlyph("Beacon of Light"));
        }

        #region Inherited Members

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.PaladinHoly; }
        }

        public override Composite Medic
        {
            get { return new PrioritySelector(); }
        }

        public override string Name
        {
            get { return "Holy Paladin"; }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    Spell.Cast("Seal of Insight", on => Me, ret => !Me.HasAura("Seal of Insight")));
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(

                    DispelManager.CreateDispelBehavior(),

                   //Stop casting healing spells if our heal target is above a certain health percent!
                    new Decorator(ret => !PaladinCommon.BuildingIlluminatedHealingBuffs, Spell.StopCasting()),

                    HandleDefensiveCooldowns(),             // Cooldowns.
                    CooldownTracker.Heal("Holy Shock", on => HealTarget, 100, ret => !CooldownTracker.SpellOnCooldown("Holy Shock") && (!PaladinCommon.CanCastHPConsumer || HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= 100)),

                    Spell.HoT("Sacred Shield", on => Tank, 100, ret => PaladinCommon.HasTalent(PaladinTalents.SacredShield) && !Tank.IsMe),
                    Spell.HoT("Eternal Flame", on => Tank, 100, ret => PaladinCommon.HasTalent(PaladinTalents.EternalFlame) && (Me.CurrentHolyPower >= PaladinCommon.EternalFlameTankHP || PaladinCommon.HasDivinePurpose || (Me.Combat && Me.CurrentHolyPower == 5))),

                    // Do some damage while we wait for a healtarget.
                    new Decorator(r => !StyxWoW.Me.IsChanneling && ((OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 99) && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 99)), new PrioritySelector(CreateJudgementBehavior(), PostHeals())),

                    // Don't do anything if we're Channeling casting, or we're waiting for the GCD.
                    new Decorator(a => StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown() || PvPSupport() || ((OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 99) && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 99)), new ActionAlwaysSucceed()),

                    TrinketManager.CreateTrinketBehaviour(),

                     // save the tank...period. Grab the glyph as well for a 10% mana return.
                    Spell.Heal("Lay on Hands", on => Tank, PaladinCommon.LayOnHandsTankPct, ret => OracleRoutine.IsViable(Tank) && !Tank.IsMe && !Tank.HasAnyAura(HandofSacrifice, Forbearance)),

                    // Cooldowns here..
                    OracleHooks.ExecCooldownsHook(),        // Evaluate the Cooldown healing PrioritySelector hook

                    HandleOffensiveCooldowns(),

                    //Beacon now
                    HandleBeaconOfLight(),                  // returns RunStatus.Failure

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
                    //Spell.Heal("Divine Light", DivineLightPct, ret => Me.ActiveAuras.ContainsKey("Infusion of Light")),
                   Spell.Cast("Flash of Light", on => HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 80 ? BeaconUnit : HealTarget, ret => StyxWoW.Me.HasAnyAura(Lucidity)),
                   Spell.Heal("Flash of Light", on => Tank, PaladinCommon.FlashofLightTankPct) //FoL on Tank
                );
        }

        private static Composite PostHeals()
        {
            // Free Holy Power.
            return new PrioritySelector(
                HandleIlluminatedHealing(),
                HandleEternalFlame(),
                new Decorator(ret => PaladinCommon.UseCrusaderStrike && Me.Combat, Spell.Cast("Crusader Strike", on => Me.CurrentTarget)));
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                    Spell.Heal("Divine Protection", on => Me, PaladinCommon.DivineProtectionPct), //Protect Me
                    Spell.Heal("Divine Shield", on => Me, PaladinCommon.DivineShieldPct), //Protect Me
                    Spell.Cast("Hand of Freedom", on => Me, ret => PaladinCommon.UseHandOfFreedom && Me.ActiveAuras.Any(aura => aura.Value.ApplyAuraType == WoWApplyAuraType.ModRoot || aura.Value.ApplyAuraType == WoWApplyAuraType.ModDecreaseSpeed) && PaladinCommon.CanUseHandAbility(Me, HandofFreedom)), //Protect Me
                    Spell.Heal("Hand of Salvation", on => Me, PaladinCommon.HandOfSalvationPct, ret => Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 30) > 0 && PaladinCommon.CanUseHandAbility(Me, HandOfPurity)), //Protect Me
                    Spell.Heal("Hand of Sacrifice", on => Tank, PaladinCommon.HandOfSacrificePct, ret => OracleRoutine.IsViable(Tank) && !Tank.IsMe && PaladinCommon.CanUseHandAbility(Tank, HandofSacrifice) && !Tank.HasAura(Forbearance)),
                    Spell.Heal("Hand of Purity", on => Tank, PaladinCommon.HandofPurityPct, ret => OracleRoutine.IsViable(Tank) && !Tank.IsMe && PaladinCommon.CanUseHandAbility(Tank, HandOfPurity) && !Tank.HasAura(Forbearance)),
                    Spell.Heal("Lay on Hands", on => HealTarget, PaladinCommon.LayOnHandsHealTargetPct, ret => !HealTarget.HasAnyAura(HandofSacrifice, Forbearance)),
                    Spell.Heal("Hand of Protection", on => HealTarget, PaladinCommon.HandOfProtectionPct, ret => OracleRoutine.IsViable(Tank) && (HealTarget.Guid != Tank.Guid) && PaladinCommon.CanUseHandAbility(HealTarget, HandofProtection) && !HealTarget.HasAura(Forbearance)),
                    Item.UseBagItem("Healthstone", ret => Me.HealthPercent < OracleSettings.Instance.HealthStonePct, "Healthstone"));
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                    Spell.Cast("Divine Plea", on => Me, ret => Me.ManaPercent < PaladinCommon.DivinePleaPct && StyxWoW.Me.Combat),
                    Spell.Cast("Avenging Wrath", on => Me, ret => HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= PaladinCommon.UrgentHealthPercentage && !Me.HasAura(DivineFavor) && !Me.HasAura(DevotionAura)),
                    Spell.Cast("Holy Avenger", on => Me, ret => PaladinCommon.HasTalent(PaladinTalents.HolyAvenger) && (HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= PaladinCommon.UrgentHealthPercentage) && !Me.HasAura(DivineFavor) && !Me.HasAura(DevotionAura)),
                    Spell.Cast("Divine Favor", on => Me, ret => (HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= (PaladinCommon.UrgentHealthPercentage - 10) || (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) <= PaladinCommon.UrgentHealthPercentage)) && !Me.HasAura(DivineFavor) && !Me.HasAura(DevotionAura)),
                    Spell.Cast("Execution Sentence", on => Tank, ret => PaladinCommon.HasTalent(PaladinTalents.ExecutionSentence) && OracleRoutine.IsViable(Tank) && !Tank.IsMe && Tank.HealthPercent(HPCheck.Tank) <= PaladinCommon.ExecutionSentencePct),
                    Spell.Cast("Guardian of Ancient Kings", on => Me, ret => !Me.HasAura(DivineFavor) && !Me.HasAura(AvengingWrath) && ((HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= PaladinCommon.GuardianofAncientKingsPct) || (OracleRoutine.IsViable(Tank) && !Tank.IsMe && Tank.HealthPercent(HPCheck.Tank) <= PaladinCommon.GuardianofAncientKingsPct))),
                    Racials.UseRacials(),
                    Item.UsePotion());
        }

        private static WoWUnit EfTarget { get; set; }

        // this gets called like 5-6 times..we only need one unbuffed target?!
        private static Composite HandleEternalFlame()
        {
            return new Decorator(ret =>
                {
                    if (!PaladinCommon.HasTalent(PaladinTalents.EternalFlame))
                        return false;

                    if (StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown())
                        return false;

                    if (Spell.BlanketSpellEntries.Count >= PaladinCommon.EternalFlameBlanketCount)
                        return false;

                    if ((Me.CurrentHolyPower <= (PaladinCommon.HasDivinePurpose ? 0 : PaladinCommon.EternalFlameBlanketHPCount)))
                        return false;

                    if ((OracleRoutine.IsViable(Tank) && (Tank.HealthPercent(HPCheck.Tank) < 50 || !Tank.HasAura(EternalFlame))))
                        return false;

                    return true;
                },
                new Sequence(
                    new Action(ret => EfTarget = Unit.GetUnbuffedTarget(EternalFlame)),
                    new PrioritySelector(new Decorator(ret => !OracleRoutine.IsViable(EfTarget), new ActionAlwaysSucceed()), Spell.HoTBlanket(EternalFlame, on => EfTarget))

                    ));
        }

        private static Composite HandleIlluminatedHealing()
        {
            return new Action(delegate
                     {
                         PaladinCommon.BuildingIlluminatedHealingBuffs = false;

                         if (Me.ManaPercent < PaladinCommon.BuildIlluminatedHealingManaPct) return RunStatus.Failure; // gtfo if he dosnt have the mana.

                         if (!BeaconUnit.HasMyAura("Beacon of Light")) return RunStatus.Failure; // gtfo if he  dosnt have it.

                         if (Me.CurrentHolyPower == Me.MaxHolyPower) return RunStatus.Failure; // max HP, that will do..

                         if (!SpellManager.CanCast("Divine Light", true)) return RunStatus.Failure; // can't cast it? gtfo

                         PaladinCommon.BuildingIlluminatedHealingBuffs = true;

                         SpellManager.Cast("Divine Light", BeaconUnit);

                         Logger.Output(" Building Illuminated Healing Buffs on {0}", BeaconUnit.SafeName);

                         return RunStatus.Success;
                     });
        }

        /// <summary> returns RunStatus.Failure </summary>
        private static Composite HandleBeaconOfLight()
        {
            return new Action(delegate
            {
                    if (OracleSettings.Instance.Paladin.BeaconUnitSelection == BeaconUnitSelection.Disable) return RunStatus.Failure;

                    if (!TalentManager.HasGlyph("Beacon of Light")) return RunStatus.Failure; // gtfo if he dosnt have the glyph.

                    if (OracleRoutine.IsViable(BeaconUnit) && BeaconUnit.HasMyAura("Beacon of Light")) return RunStatus.Failure; // gtfo if he has it.

                    if (!SpellManager.CanCast("Beacon of Light")) return RunStatus.Failure; // can't cast it? gtfo

                    SpellManager.Cast("Beacon of Light", BeaconUnit);

                    Logger.Output("Beaconed {0}", BeaconUnit.SafeName);
                    return RunStatus.Success;
                });
        }

        private static Composite CreateJudgementBehavior()
        {
            return new Decorator(ret => SpellManager.CanCast(Judgment) && Me.Combat && PaladinCommon.HasTalent(PaladinTalents.SelflessHealer),
                        new Sequence(
                            Spell.Cast("Judgment", on => Unit.GetEnemy, ret => OracleRoutine.IsViable(Unit.GetEnemy) && Unit.FaceTarget(Unit.GetEnemy)),
                            new ActionAlwaysFail()));
        }

        #endregion Composites
    }
}