#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/Classes/ChronicleDiscPriest.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Bots.BGBuddy.Helpers;
using Oracle.Classes.Priest;
using Oracle.Core;
using Oracle.Core.Managers;
using Oracle.Core.Spells.Auras;
using Oracle.UI.Settings;
using Styx;
using Styx.WoWInternals.WoWObjects;
using Logger = Oracle.Shared.Logging.Logger;

namespace Oracle.Healing.Chronicle.Classes
{
    internal class ChronicleDiscPriest : ChronicleHealing
    {
        // TODO: Consider Divine Insight
        // When you cast Penance, there is a 100% chance your next Power Word: Shield will both ignore and not cause the Weakened Soul effect.

        #region settings

        private static OracleSettings OSetting { get { return OracleSettings.Instance; } }

        private static bool EnableProvingGrounds { get { return OSetting.EnableProvingGrounds; } }

        private static DiscPriestSettings Setting { get { return OracleSettings.Instance.DiscPriest; } }

        private static bool EnableOffensiveDps { get { return Setting.EnableOffensiveDps; } }

        private static int FlashHealDSPPct { get { return Setting.FlashHealDSPPct; } }

        private static int BindingHealPct { get { return Setting.BindingHealPct; } }

        private static int HealDSPPct { get { return Setting.HealDSPPct; } }

        private static bool GreaterHealPrioEnabled { get { return Setting.GreaterHealPrioEnabled; } }

        private static bool HealPrioEnabled { get { return Setting.HealPrioEnabled; } }

        private static bool FlashHealPrioEnabled { get { return Setting.FlashHealPrioEnabled; } }

        private static bool RenewPrioEnabled { get { return Setting.RenewPrioEnabled; } }

        private static bool SmitePrioEnabled { get { return Setting.SmitePrioEnabled; } }

        private static bool PowerWordSolacePrioEnabled { get { return Setting.PowerWordSolacePrioEnabled; } }

        private static bool HolyFirePrioEnabled { get { return Setting.HolyFirePrioEnabled; } }

        private static bool BindingHealPrioEnabled { get { return Setting.BindingHealPrioEnabled; } }

        private static bool PenancePrioEnabled { get { return Setting.PenancePrioEnabled; } }

        private static bool PowerWordShieldPrioEnabled { get { return Setting.PowerWordShieldPrioEnabled; } }

        #endregion settings

        #region Helpers

        public static float GetGrace(int points)
        {
            return points * 0.10f;
        }

        public static float GetEvangelism(int points)
        {
            return points * 0.06f;
        }

        private static int EVANGELISM_STACKS { get { return (int)SpellAuras.GetAuraStackCount("Evangelism"); } }

        private static int GRACE_STACKS { get { return (int)SpellAuras.StackCount(OracleHealTargeting.HealableUnit, "Grace"); } } // BUG: taxing ?

        private static bool ARCHANGEL { get { return StyxWoW.Me.HasAura("Archangel"); } }

        private static WoWUnit HealTarget { get { return OracleHealTargeting.HealableUnit ?? StyxWoW.Me; } }

        private static bool CanShield(WoWUnit u)
        {
            return OracleRoutine.IsViable(u) && (!u.ActiveAuras.ContainsKey("Weakened Soul") /*|| StyxWoW.Me.ActiveAuras.ContainsKey("Divine Insight")*/);
        }

        #endregion

        public static void GenerateSpellList()
        {
            AvailableChronicleSpells.Clear();

            const double atonement_modifier = 0.1; // Base is 1f so we end up with 0.9 to start with. Patch 5.3: Atonement now heals nearby friendly targets for 90% of the damage dealt, down from 100%

            //Spell POWA!
            var localPlayerSpellPower = BaseStats.GetPlayerSpellPower;
            var localPlayerSpellModifiers = BaseStats.GetPlayerSpellModifiers(StyxWoW.Me);

            //Crit chance
            const bool enable_crit_multi = true;
            var bcrit = BaseStats.GetPlayerCrit / 100;

            //Mastery sun!
            const bool enable_mast_multi = true;
            var bmast = BaseStats.GetPlayerMastery * 0.016; // Mastery: Shield Discipline

           
            
            const float mastery_heal_modifier = 0.5f;
            //const float mastery_shield_modifier = 0.016f;

           // Logger.Output("Mastery: {0} : [{1}] ---- calc: {2}", bmast, BaseStats.GetPlayerMastery, (1 + (float)bmast));

            //Haste!
            var hasteModifier = BaseStats.GetHasteModifier;

            var Renew = new Periodicheal
                {
                    SpellId = 774,
                    SpellName = "Renew",
                    BaseManaCost = (int)(0.026f * BaseStats.BaseMana),
                    Instant = true,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.PeriodicHeal,
                    BasePeriodicCoefficient = 0.207f,
                    BasePeriodicTickFrequency = 3f,
                    BasePeriodicHeal = 2152,
                    BasePeriodicDuration = 12,
                    TickType = TickType.Interval,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            Renew.HealModifier += GetGrace(GRACE_STACKS);
            Renew.HealModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel
            Renew.HealModifier += (float)0.25; // Spiritual Healing
            Renew.HealModifier += (enable_mast_multi ? (float)bmast * mastery_heal_modifier : (float)(0.0));

            Renew.Calculate();

            if (RenewPrioEnabled) AvailableChronicleSpells[Renew.SpellName] = Renew;

            var GreaterHeal = new DirectHeal
                {
                    SpellId = 2060,
                    SpellName = "Greater Heal",
                    BaseManaCost = (int)(0.059f * BaseStats.BaseMana),
                    BaseCastTime = 2.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 2.19f,
                    BaseHeal = (21022f + 24430f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            GreaterHeal.HealModifier += GetGrace(GRACE_STACKS);
            GreaterHeal.HealModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel
            //GreaterHeal.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            GreaterHeal.Calculate();

            if (GreaterHealPrioEnabled) AvailableChronicleSpells[GreaterHeal.SpellName] = GreaterHeal;

            var Smite = new DirectHeal
                {
                    SpellId = 585,
                    SpellName = "Smite",
                    BaseManaCost = (int)(0.027f * BaseStats.BaseMana),
                    ManaCostScale = (float)(1 - (EVANGELISM_STACKS * 0.06)),
                    BaseCastTime = 1.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.856f,
                    BaseHeal = (2226 + 2496) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectDamage,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            Smite.HasteModifier = 2.2F;// A little cheating to make Smite lower MPS therfore increasing its prio.

            //Smite.HealModifier += (enable_mast_multi ? (float)bmast * mastery_heal_modifier : (float)(0.0)) + 0.25f; // lets assume a 25% chance to crit.

            Smite.Calculate();

            Smite.PostCalculationModifier -= (float)(atonement_modifier); // Patch 5.3: Atonement now heals nearby friendly targets for 90% of the damage dealt, down from 100%
            Smite.PostCalculationModifier += GetEvangelism(EVANGELISM_STACKS);
            Smite.PostCalculationModifier += GetGrace(GRACE_STACKS);
            Smite.PostCalculationModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel

            if (SmitePrioEnabled && (EnableOffensiveDps || EVANGELISM_STACKS < 5)) AvailableChronicleSpells[Smite.SpellName] = Smite;

            var PowerWordSolaceHolyFire = new Periodicheal
                {
                    SpellId = 14914, //129250
                    SpellName = "Power Word: Solace",
                    BaseManaCost = 0,
                    ManaCostScale = (float)(1 - (EVANGELISM_STACKS * 0.06)),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 1.11f,
                    BaseHeal = (1072 + 1201) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.HybridDamage,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                    BasePeriodicCoefficient = 0.02184f,
                    BasePeriodicTickFrequency = 1f,
                    BasePeriodicHeal = 399,
                    BasePeriodicDuration = 7,
                    TickType = TickType.Interval,
                };

            //PowerWordSolaceHolyFire.HealModifier += (enable_mast_multi ? (float)bmast * mastery_heal_modifier : (float)(0.0)) + 0.25f; // lets assume a 25% chance to crit.

            PowerWordSolaceHolyFire.Calculate();

            //PowerWordSolaceHolyFire.PostCalculationModifier -= (float)(atonement_modifier); // Patch 5.3: Atonement now heals nearby friendly targets for 90% of the damage dealt, down from 100%
            PowerWordSolaceHolyFire.PostCalculationModifier += GetEvangelism(EVANGELISM_STACKS);
            PowerWordSolaceHolyFire.PostCalculationModifier += GetGrace(GRACE_STACKS);
            PowerWordSolaceHolyFire.PostCalculationModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel

            if (PowerWordSolacePrioEnabled && EnableOffensiveDps && PriestCommon.HasTalent(PriestTalents.SolaceAndInsanity)) AvailableChronicleSpells[PowerWordSolaceHolyFire.SpellName] = PowerWordSolaceHolyFire;

            var HolyFire = new Periodicheal
                {
                    SpellId = 14914,
                    SpellName = "Holy Fire",
                    BaseManaCost = (int)(0.018f * BaseStats.BaseMana),
                    ManaCostScale = (float)(1 - (EVANGELISM_STACKS * 0.06)),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 1.11f,
                    BaseHeal = (1002 + 1271) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.HybridDamage,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                    BasePeriodicCoefficient = 0.02184f,
                    BasePeriodicTickFrequency = 1f,
                    BasePeriodicHeal = 399,
                    BasePeriodicDuration = 7,
                    TickType = TickType.Interval,
                };

            //HolyFire.HealModifier += (enable_mast_multi ? (float)bmast * mastery_heal_modifier : (float)(0.0)) + 0.25f; // lets assume a 25% chance to crit.

            HolyFire.Calculate();

            HolyFire.PostCalculationModifier -= (float)(atonement_modifier); // Patch 5.3: Atonement now heals nearby friendly targets for 90% of the damage dealt, down from 100%
            HolyFire.PostCalculationModifier += GetEvangelism(EVANGELISM_STACKS);
            HolyFire.PostCalculationModifier += GetGrace(GRACE_STACKS);
            HolyFire.PostCalculationModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel

            if (HolyFirePrioEnabled && EnableOffensiveDps && !PriestCommon.HasTalent(PriestTalents.SolaceAndInsanity)) AvailableChronicleSpells[HolyFire.SpellName] = HolyFire;

            var FlashHeal = new DirectHeal
                {
                    SpellId = 2060,
                    SpellName = "Flash Heal",
                    BaseManaCost = (int)(0.059f * BaseStats.BaseMana),
                    BaseCastTime = 1.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 1.314f,
                    BaseHeal = (12619f + 14664f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            FlashHeal.HealModifier += GetGrace(GRACE_STACKS);
            FlashHeal.HealModifier += (float)(0.25); //Spiritual healing
            FlashHeal.HealModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel
            FlashHeal.HealModifier += (enable_mast_multi ? (float)bmast * mastery_heal_modifier : (float)(0.0));

            FlashHeal.Calculate();

            if (FlashHealPrioEnabled && OracleRoutine.IsViable(HealTarget) && (HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= FlashHealDSPPct)) AvailableChronicleSpells[FlashHeal.SpellName] = FlashHeal;

            var BindingHeal = new DirectHeal
                {
                    SpellId = 32546,
                    SpellName = "Binding Heal",
                    BaseManaCost = (int)(0.054f * BaseStats.BaseMana), // calculate glyph for 35% more mana cost?...meh
                    BaseCastTime = 1.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.899f,
                    BaseHeal = 11207f,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            BindingHeal.HasteModifier += StyxWoW.Me.HealthPercent < 60 ? 1.6f : 0; // more weight if we need to be healed as well.
            BindingHeal.HealModifier += GetGrace(GRACE_STACKS);
            BindingHeal.HealModifier += (float)(0.25); //Spiritual healing
            BindingHeal.HealModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel
            BindingHeal.HealModifier += (enable_mast_multi ? (float)bmast * mastery_heal_modifier : (float)(0.0));

            BindingHeal.Calculate();

            if (BindingHealPrioEnabled && (StyxWoW.Me.HealthPercent < BindingHealPct) && OracleRoutine.IsViable(HealTarget) && (HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= FlashHealDSPPct)) AvailableChronicleSpells[BindingHeal.SpellName] = BindingHeal;

            var Penance = new Periodicheal
                {
                    SpellId = 47540,
                    SpellName = "Penance",
                    BaseManaCost = (int)(0.031f * BaseStats.BaseMana),
                    ManaCostScale = (float)(1 - (EVANGELISM_STACKS * 0.06)),
                    BaseCastTime = 2f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.838f,
                    BaseHeal = (8188f + 9250f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.HybridHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                    BasePeriodicCoefficient = 0.838f,
                    BasePeriodicTickFrequency = 1f,
                    BasePeriodicHeal = (8188f + 9250f) / 2,
                    BasePeriodicDuration = 2,
                    TickType = TickType.Interval,
                };

            Penance.ManaCostScale += (TalentManager.HasGlyph("Penance") ? 0.20f : 0.0f);

            Penance.Calculate();

            Penance.PostCalculationModifier += GetEvangelism(EVANGELISM_STACKS);
            Penance.PostCalculationModifier += GetGrace(GRACE_STACKS);
            Penance.PostCalculationModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel

            if (PenancePrioEnabled) AvailableChronicleSpells[Penance.SpellName] = Penance;

            var PrayerofMending = new DirectHeal
                {
                    SpellId = 33076,
                    SpellName = "Prayer of Mending",
                    BaseManaCost = (int)(0.035f * BaseStats.BaseMana),
                    Instant = true,
                    BaseCoefficient = 0.571f,
                    BaseHeal = 5919f,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                    ChargeModifier = 6,
                };

            //PrayerofMending.HealModifier += (enable_mast_multi ? (float)bmast * mastery_heal_modifier : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            BindingHeal.HealModifier += (float)(0.25); //Spiritual healing

            PrayerofMending.Calculate();

            //AvailableChronicleSpells[PrayerofMending.SpellName] = PrayerofMending;

            var Heal = new DirectHeal
                {
                    SpellId = 2050,
                    SpellName = "Heal",
                    BaseManaCost = (int)(0.019f * BaseStats.BaseMana),
                    BaseCastTime = 0.8f,
                    //HasteModifier = hasteModifier,
                    BaseCoefficient = 1.024f,
                    BaseHeal = (9848f + 11443f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            Heal.HealModifier += GetGrace(GRACE_STACKS);
            Heal.HealModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel
            Heal.HealModifier += (enable_mast_multi ? (float)bmast * mastery_heal_modifier : (float)(0.0));

            Heal.Calculate();

            if (HealPrioEnabled && OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= HealDSPPct) AvailableChronicleSpells[Heal.SpellName] = Heal;

            var PrayerofHealing = new DirectHeal
                {
                    SpellId = 596,
                    SpellName = "Prayer of Healing",
                    BaseManaCost = (int)(0.045f * BaseStats.BaseMana),
                    BaseCastTime = 2.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.838f,
                    BaseHeal = (8450f + 8927f) / 2,
                    SpellPower = localPlayerSpellPower,
                    FriendlyCount = 5f,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            PrayerofHealing.HealModifier += ARCHANGEL ? (float)(0.25) : (float)(0.0); // archangel
            //PrayerofHealing.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            PrayerofHealing.Calculate();

            AvailableChronicleSpells[PrayerofHealing.SpellName] = PrayerofHealing;

            var PowerWordShield = new DirectHeal
                {
                    SpellId = 17,
                    SpellName = "Power Word: Shield",
                    BaseManaCost = (int)(0.061f * BaseStats.BaseMana),
                    HasteModifier = (EnableProvingGrounds ? 0.9f : hasteModifier),
                    ManaCostScale = (float)0.75,
                    Instant = true,
                    BaseCoefficient = 1.871f,
                    BaseHeal = 19428,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            PowerWordShield.BaseHeal *= (enable_mast_multi ? 1 + (float)bmast: 1 + (float)(0.0));

            PowerWordShield.Calculate();

            if (PowerWordShieldPrioEnabled && OracleRoutine.IsViable(HealTarget) && CanShield(HealTarget)) AvailableChronicleSpells[PowerWordShield.SpellName] = PowerWordShield;

            foreach (var spell in AvailableChronicleSpells)
            {
                //Console.WriteLine("{0}", spell.ToString());
            }
        }
    }
}