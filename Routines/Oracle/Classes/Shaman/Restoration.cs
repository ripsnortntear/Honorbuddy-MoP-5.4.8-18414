#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Shaman/Restoration.cs $
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
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace Oracle.Classes.Shaman
{
    [UsedImplicitly]
    internal class RestorationShaman : RotationBase
    {
        static RestorationShaman()
        {
            DisplayMessage("Heya, I noticed you have glyph of Chaining, be aware that Healing Rain will not be cast as much as you want too.", "Heads Up!", Me.Specialization == WoWSpec.ShamanRestoration && TalentManager.HasGlyph("Chaining"));
        }

        #region Inherited Members

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.ShamanRestoration; }
        }

        public override string Name
        {
            get { return "Resto Shaman"; }
        }

        public override Composite Medic
        {
            get { return new PrioritySelector(); }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                         Spell.HoT("Water Shield", on => Me, 100, when => ShamanCommon.EnableWaterShield),
                         ShamanCommon.CreateShamanImbueMainHandBehavior(ShamanCommon.Imbue.Earthliving, ShamanCommon.Imbue.Flametongue)
                        ));
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(

                    DispelManager.CreateDispelBehavior(),

                    //Stop casting healing spells if our heal target is above a certain health percent!
                    Spell.StopCasting(),

                    // Do some damage while we wait for a healtarget.
                    new Decorator(r => !StyxWoW.Me.IsChanneling && ((OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 98) && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 99)), PostHeals()),

                    // Don't do anything if we're Channeling casting, or we're waiting for the GCD.
                    new Decorator(a => StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown() || PvPSupport() || ((OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) > 99) && (OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 99)), new ActionAlwaysSucceed()),

                    ShamanCommon.CreateHealingRainBehaviour(),  // may as well throw out a HR

                    //Spell.HoT("Earth Shield", on => (!Tank.HasMyAura("Earth Shield") && !Tank.IsMe) ? Tank : SecondTank, 100, when => (ShamanCommon.HandleBuffonTank == HandleTankBuff.Always) && !Tank.IsMe && !SecondTank.IsMe && ShamanCommon.HandleEarthShieldTarget && !Me.HasAura("Ancestral Swiftness")),  // ES the tank.
                    Spell.HoT("Earth Shield", on => Tank, 100, when => (ShamanCommon.HandleBuffonTank == HandleTankBuff.Always) && OracleRoutine.IsViable(Tank) && !Tank.IsMe && ShamanCommon.HandleEarthShieldTarget && !Me.HasAura("Ancestral Swiftness")),  // ES the tank.
                    Spell.HoT("Water Shield", on => Me, 100, when => ShamanCommon.EnableWaterShield),
                    HandleRiptide(),

                    //cast "reinforce" on primals bar for 10% healing increase and 20% damage decrease
                    new Decorator(ret => ShamanCommon.UseReinforcewithEarthElemental && ShamanCommon.HasTalent(ShamanTalents.PrimalElementalist) && PetManager.HavePet && !Me.HasAura("Reinforce"), PetManager.CastAction("Reinforce", on => Me)),

                    TrinketManager.CreateTrinketBehaviour(),

                    HandleDefensiveCooldowns(),             // Cooldowns.
                    HandleOffensiveCooldowns(),

                    //AncestralSwiftness here
                     OracleHooks.ExecCooldownsHook(),        // Evaluate the Cooldown healing PrioritySelector hook

                     // Shit that needs priority over AoE healing.
                     PreHeals(),

                    //AoE Spells..
                    new Decorator(r => OracleRoutine.IsViable(Tank) && Tank.HealthPercent(HPCheck.Tank) > 50, OracleHooks.ExecClusteredHealHook()),

                    //Ordered Single Target Spells
                    OracleHooks.ExecSingleTargetHealHook() // Evaluate the Single Target Healing PrioritySelector
                    );
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
                    Spell.HoT("Healing Stream Totem", ShamanCommon.HealingStreamTotemPercent, ret => !Me.IsMoving && (!ShamanCommon.Exist(WoWTotem.ManaTide) && !ShamanCommon.Exist(WoWTotem.HealingTide)) && !Me.HasAura("Ancestral Swiftness")),
                    Spell.Heal("Healing Surge", ShamanCommon.HealingSurgePercent, ret => ShamanCommon.IsNotAboutToCastHealingRain));
        }

        private static Composite PostHeals()
        {
            return new PrioritySelector(
                Spell.Cast("Elemental Blast", on => Unit.GetEnemy, ret => ShamanCommon.HasTalent(ShamanTalents.ElementalBlast) && Me.Combat && OracleRoutine.IsViable(Unit.GetEnemy) && Unit.FaceTarget(Unit.GetEnemy)),
                Spell.Cast("Lightning Bolt", on => Unit.GetEnemy, ret => TalentManager.HasGlyph("Telluric Currents") && Me.ManaPercent < ShamanCommon.LightningBoltPercent && Me.Combat && OracleRoutine.IsViable(Unit.GetEnemy) && Unit.FaceTarget(Unit.GetEnemy)));
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                    Spell.Heal("Stone Bulwark Totem", on => Me, ShamanCommon.StoneBulwarkTotemPercent),
                    Spell.Heal("Astral Shift", on => Me, ShamanCommon.AstralShiftPercent),
                    Item.UseBagItem("Healthstone", ret => Me.HealthPercent < OracleSettings.Instance.HealthStonePct, "Healthstone"));
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                    Spell.Cast("Mana Tide Totem", on => Me, ret => Me.ManaPercent <= ShamanCommon.ManaTideTotemPercent && (!ShamanCommon.Exist(WoWTotem.HealingTide) || !ShamanCommon.Exist(WoWTotem.HealingStream))),
                    Spell.Cast("Ancestral Swiftness", on => Me, ret => (HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= ShamanCommon.UrgentHealthPercentage)),
                    Racials.UseRacials(),
                    Item.UsePotion());
        }

        private static Composite HandleRiptide()
        {
            return new PrioritySelector(
                // consider keeping this on the tank.. - wulf.
                CooldownTracker.HoT("Riptide", on => Tank, 100,
                    ret =>
                        (ShamanCommon.HandleBuffonTank == HandleTankBuff.Always) && OracleRoutine.IsViable(Tank) && !Tank.IsMe &&
                        (!Me.HasAura("Ancestral Swiftness") || ShamanCommon.IsNotAboutToCastHealingRain)),
                // consider keeping this on the offtank aswell.. - handnavi.
                CooldownTracker.HoT("Riptide", on => SecondTank, 100,
                    ret =>
                        (ShamanCommon.HandleBuffonTank == HandleTankBuff.Always) && OracleRoutine.IsViable(SecondTank) && !SecondTank.IsMe &&
                        (!Me.HasAura("Ancestral Swiftness") || ShamanCommon.IsNotAboutToCastHealingRain)),
                HandleRiptide2());
        }

        private static WoWUnit RiptideTarget { get; set; }

        private static Composite HandleRiptide2()
        {
            return new Decorator(ret =>
            {
                // no unit? gtfo
                if ((Me.HasAura("Ancestral Swiftness") || !ShamanCommon.IsNotAboutToCastHealingRain))
                    return false;

                if (StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown())
                    return false;

                //we only cast riptime, if we wont waste uptime on tanks!
                if ((OracleRoutine.IsViable(Tank) && !Tank.IsMe && !Tank.HasAura("Riptide", 1, true, 5000)) || (OracleRoutine.IsViable(SecondTank) && !SecondTank.IsMe && !SecondTank.HasAura("Riptide", 1, true, 5000)))
                    return false;

                /*if (Me.HasAura("Tidal Waves", 1, true, 1000))
                    return false;
                */

                return true;
            },
                new Sequence(
                    new Action(ret => RiptideTarget = Unit.GetUnbuffedTarget(Riptide)),
                    new PrioritySelector(
                        new Decorator(ret => !OracleRoutine.IsViable(RiptideTarget), new ActionAlwaysSucceed()),
                        CooldownTracker.HoT("Riptide", on => RiptideTarget))
                    ));
        }

        #endregion Composites
    }
}