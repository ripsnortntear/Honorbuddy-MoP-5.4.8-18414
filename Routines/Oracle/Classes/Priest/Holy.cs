#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Priest/Holy.cs $
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
using Action = Styx.TreeSharp.Action;

namespace Oracle.Classes.Priest
{
    [UsedImplicitly]
    internal class Holy : RotationBase
    {
        static Holy()
        {
        }

        #region Inherited Members

        public override string Name
        {
            get { return "Holy Priest"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.PriestHoly; }
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
                        Spell.Cast("Power Word: Fortitude", on => Me, ret => !Me.HasAnyAura("Power Word: Fortitude", "Qiraji Fortitude", "Blood Pact", "Commanding Shout") && PriestCommon.HolyUsePowerWordFortitude)));
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(

                    //HandleLightWell(),

                    DispelManager.CreateDispelBehavior(),

                    //Stop casting healing spells if our heal target is above a certain health percent!
                    Spell.StopCasting(),

                    HandleDefensiveCooldowns(),             // Cooldowns.

                    PriestCommon.CreatePriestMovementBuff(),

                    HandleRenewOnTank(),

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
                    Spell.Heal("Guardian Spirit", on => Tank, PriestCommon.HolyGuardianSpiritPct), // should be used on a tank
                    Spell.Heal("Power Word: Shield", on => Tank, PriestCommon.HolyUrgentHealthPercentage, ret => PriestCommon.CanShield(Tank)),
                    Spell.Heal("Flash Heal", on => Tank, PriestCommon.HolyUrgentHealthPercentage),
                    Spell.Heal("Flash Heal", PriestCommon.HolyFlashHealSoLPct, ret => Me.ActiveAuras.ContainsKey("Surge of Light")),
                    Spell.Heal("Prayer of Mending", on => Tank, PriestCommon.HolyPrayerOfMendingPct, ret => Unit.GetbuffCount(PrayerofMendingbuff) < 1), //PoM
                    HandleLucidity()
                );
        }

        private static Composite PostHeals()
        {
            return new ActionAlwaysSucceed();
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                    Spell.Heal("Desperate Prayer", on => Me, PriestCommon.HolyDesperatePrayerPct),
                    Spell.Heal("Void Shift", on => HealTarget, PriestCommon.HolyVoidShiftPct, ret => Me.HealthPercent > 80), // should be used to save the life of a raid member by trading health percentages with them.
                    Spell.Cast("Fade", on => Me, ret => PriestCommon.HolyUseFade && !StyxWoW.Me.IsChanneling && (PriestCommon.FadePvE() || PriestCommon.FadePvP())),
                    Item.UseBagItem("Healthstone", ret => Me.HealthPercent < OracleSettings.Instance.HealthStonePct, "Healthstone"));
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(Racials.UseRacials(), Item.UsePotion());
        }

        private static Composite HandleRenewOnTank()
        {
            return new PrioritySelector(
                        new Decorator(ret => Me.HasAura("Chakra: Serenity"), Spell.Cast("Holy Word: Chastise", on => Tank, ret => !Spell.SpellOnCooldown(HolyWordSerenity) && Tank.HasAura(Renew, 0, true, 1800))),
                        Spell.HoT("Renew", on => Tank, 100, ret => OracleRoutine.IsViable(Tank) && !Tank.IsMe)
                );
        }

        private static WoWUnit LucidityTarget { get; set; }

        private static Composite HandleLucidity()
        {
            return new Decorator(ret =>
            {
                // no unit? gtfo
                if (!StyxWoW.Me.HasAnyAura(Lucidity))
                    return false;

                return true;
            },
                new Sequence(
                    new Action(ret => LucidityTarget = Unit.GetUnbuffedTarget(WeakenedSoul)),
                    new PrioritySelector(new Decorator(ret => !OracleRoutine.IsViable(LucidityTarget), new ActionAlwaysSucceed()), Spell.Cast("Power Word: Shield", on => LucidityTarget))
                    ));
        }

        #region LightWell/LightSpring

        private static readonly WaitTimer LightWellTimer = new WaitTimer(TimeSpan.FromSeconds(PriestCommon.HolyLightwellWaitTime));

        private static bool NeedLightWell()
        {
            // Dont do shit if its not been x seconds.
            //if (!LightWellTimer.IsFinished)
            //    return false;

            // derp..
            if (!Me.Combat || Me.IsMoving)
                return false;

            // Spell on cooldown derp..
            if (TalentManager.HasGlyph("Lightspring") ? CooldownTracker.SpellOnCooldown(Lightspring) : CooldownTracker.SpellOnCooldown(Lightwell))
                return false;

            const int lightspringObject = 64571;
            const int lightwellObject = 31897;

            var entry = TalentManager.HasGlyph("Lightspring") ? lightspringObject : lightwellObject;

            // Grab dat..fast!
            var myLightwell = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().FirstOrDefault(p => p.CreatedByUnitGuid == Me.Guid && p.Entry == entry);

            // if we dont have one..then get dat shit.
            if (myLightwell == null)
            {
                LightWellTimer.Reset();
                return true;
            }

            // Distance...
            if (myLightwell.Distance > PriestCommon.HolyMaxLightwellDistance)
            {
                LightWellTimer.Reset();
                return true;
            }

            return false;
        }

        private static Composite HandleLightWell()
        {
            return new Decorator(ret => PriestCommon.HolyEnableLightwellUsage && NeedLightWell(),
                CooldownTracker.CastOnGround(TalentManager.HasGlyph("Lightspring") ? Lightspring : Lightwell, on => WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, Tank.Location, 5f), ret => OracleRoutine.IsViable(Tank), 0, false, true, false)); // && !Tank.IsMe
        }

        #endregion LightWell/LightSpring

        #endregion Composites
    }
}