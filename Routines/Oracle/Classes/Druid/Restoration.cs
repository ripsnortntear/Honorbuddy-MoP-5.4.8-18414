#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-18 20:18:58 +1000 (Wed, 18 Sep 2013) $
 * $ID$
 * $Revision: 230 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Druid/Restoration.cs $
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
using Oracle.UI.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

namespace Oracle.Classes.Druid
{
    [UsedImplicitly]
    internal class RestorationDruid : RotationBase
    {
        static RestorationDruid()
        {
            DisplayMessage("Heya, I noticed you have dont have glyph of Regrowth, its recommended for optimal routine Performance", "Heads Up!", Me.Specialization == WoWSpec.DruidRestoration && !TalentManager.HasGlyph("Regrowth"));
            //DisplayMessage("Heya, I noticed you have dont have glyph of Lifebloom, its recommended for optimal routine Performance", "Heads Up!", Me.Specialization == WoWSpec.DruidRestoration && !TalentManager.HasGlyph("Lifebloom"));
        }

        //TODO:
        // Soul of the Forest
        // Mushrooms
        // --   Current Charge:
        //      Missing Health :
        //      Effective Heal % from the bloom:
        //      Minimum E_Heal for the bloom

        #region Inherited Members

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.DruidRestoration; }
        }

        public override string Name
        {
            get { return "Leaves Killer - Resto Druid"; }
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

                    new Decorator(a => StyxWoW.Me.IsChanneling, new ActionAlwaysSucceed()), // important for tranq.

                    DispelManager.CreateDispelBehavior(),

                    //Stop casting healing spells if our heal target is above a certain health percent!
                    Spell.StopCasting(),

                    HandleDefensiveCooldowns(),             // Cooldowns.

                     Spell.Cast("Lifebloom", on => Tank, ret => (DruidCommon.HandleBuffonTank == HandleTankBuff.Always) && !Tank.HasAura("Lifebloom", 3, true, 3000)),  // LB the tank.
                     Spell.HoT(Rejuvenation, on => Tank, 100, ret => (DruidCommon.HandleBuffonTank == HandleTankBuff.Always) && !Tank.IsMe && Tank.HealthPercent(HPCheck.Tank) < 100),
                     Spell.HoT(Rejuvenation, 100, ret => StyxWoW.Me.HasAnyAura(Lucidity)),

                     HandleRejuvBlanket(),

                    // Don't do anything if we're Channeling casting, or we're waiting for the GCD.
                    new Decorator(a =>  StyxWoW.Me.IsCasting || Spell.GlobalCooldown() || PvPSupport() || ((OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 99) && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 99)), new ActionAlwaysSucceed()),

                    DruidCommon.CreateEfflorescenceBehaviour(),  // may as well throw out a HR
                    
                    TrinketManager.CreateTrinketBehaviour(),

                    // Cooldowns here..
                    OracleHooks.ExecCooldownsHook(),        // Evaluate the Cooldown healing PrioritySelector hook

                    HandleOffensiveCooldowns(),

                    // Shit that needs priority over AoE healing.
                    PreHeals(),

                    //AoE Spells..
                    new Decorator(r => OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 65, OracleHooks.ExecClusteredHealHook()),
                    //OracleHooks.ExecClusteredHealHook(),    // Evaluate the Clustered Healing PrioritySelector

                    HandleNaturesSwiftnessHealingTouch(),

                    HandleTreeofLife(),

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
                    Spell.HoT("Regrowth", TalentManager.HasGlyph("Regrowth") ? DruidCommon.RegrowtClearcastingPercent : 100, ret => Me.HasAura("Clearcasting"), 0.5, true),
                    Spell.Cast(ForceOfNature, on => HealTarget, ret => DruidCommon.HasTalent(DruidTalents.ForceOfNature) && OracleHealTargeting.HealthCheck(HealTarget, DruidCommon.ForceOfNaturePct) && !HealTarget.IsMe && Spell.ForceofNatureWaitTimer.IsFinished && !CooldownTracker.SpellOnCooldown(ForceOfNature), 2, true, true, false),
                    Spell.Cast("Renewal", on => Me, ret => DruidCommon.HasTalent(DruidTalents.Renewal) && (Me.HealthPercent < DruidCommon.RenewalPct)),
                    Spell.HoT("Cenarion Ward", DruidCommon.CenarionWardPct, ret => DruidCommon.HasTalent(DruidTalents.CenarionWard))
                    
                );
        }

        private static Composite PostHeals()
        {
            return Spell.Cast(Nourish, on => HealTarget, ret => !Me.HasAura("Harmony", 0, true, 1200));
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                    Spell.Cast(Barkskin, on => Me, ret => Me.HealthPercent < DruidCommon.BarkskinPct && Me.Combat),
                    Spell.Heal("Ironbark", DruidCommon.IronBarkPct),
                    Spell.Cast(MightofUrsoc, on => Me, ret => Me.HealthPercent < DruidCommon.MightofUrsocPct),
                    Item.UseBagItem("Healthstone", ret => Me.HealthPercent < OracleSettings.Instance.HealthStonePct, "Healthstone"));
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                    CooldownTracker.Cast(Innervate, on => Me, ret => (Me.ManaPercent <= DruidCommon.InnervatePercent) || ((Me.HasAura(HymnOfHope) && Me.ManaPercent < DruidCommon.InnervateHymnOfHopePercent))),
                    Spell.Cast(NaturesVigil, on => Me, ret => DruidCommon.HasTalent(DruidTalents.NaturesVigil) && OracleHealTargeting.HealthCheck(HealTarget, DruidCommon.UrgentHealthPercentage)),
                    Racials.UseRacials(),
                    Item.UsePotion());
        }

        private static Composite HandleNaturesSwiftnessHealingTouch()
        {
            return new Decorator(ret => OracleHealTargeting.HealthCheck(HealTarget, DruidCommon.NaturesSwiftnessHealingTouchPct) && SpellManager.CanCast("Nature's Swiftness"),
                    new Sequence(
                        Spell.Cast(NaturesSwiftness, on => Me),
                        Spell.CreateWaitForLagDuration(),
                        Spell.Heal("Healing Touch", DruidCommon.NaturesSwiftnessHealingTouchPct, ret => Me.HasAura(NaturesSwiftness))));
        }

        private static Composite HandleTreeofLife()
        {
            return new Decorator(ret => DruidCommon.IsTreeofLife,
                   new PrioritySelector(
                        Spell.Heal("Wild Growth"), // just cast that shit.
                        Spell.HoT("Regrowth", Me.HasAura(Clearcasting) ? 100 : DruidCommon.RegrowthToLPercent),
                        Spell.Cast(Lifebloom, ret => HealTarget, ret => !HealTarget.HasAura(Lifebloom, DruidCommon.LifebloomstacksForToL, true, 1000))));
        }

        private static WoWUnit RejuvTarget { get; set; }

        // this gets called like 5-6 times..we only need one unbuffed target?!
        private static Composite HandleRejuvBlanket()
        {
            return new Decorator(ret =>
            {
                if (!DruidCommon.RejuvBlanketEnable)
                    return false;

                if (StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown())
                    return false;

                if (Spell.BlanketSpellEntries.Count >= DruidCommon.RejuvBlanketCount)
                    return false;

                if ((Me.ManaPercent <= DruidCommon.RejuvBlanketManaPct))
                    return false;

                return true;
            },
                new Sequence(
                    new Action(ret => RejuvTarget = Unit.GetUnbuffedTarget(Rejuvenation)),
                    new PrioritySelector(new Decorator(ret => !OracleRoutine.IsViable(RejuvTarget), new ActionAlwaysSucceed()), Spell.HoTBlanket(Rejuvenation, on => RejuvTarget))
                    ));
        }

        #endregion Composites
    }
}