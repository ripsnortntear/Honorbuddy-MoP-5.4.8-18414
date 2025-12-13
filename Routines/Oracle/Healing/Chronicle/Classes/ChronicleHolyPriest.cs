#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-17 12:14:14 +1000 (Tue, 17 Sep 2013) $
 * $ID$
 * $Revision: 227 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/Classes/ChronicleHolyPriest.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using Oracle.Core;
using Oracle.Core.Managers;
using Oracle.Core.Spells.Auras;
using Oracle.Shared.Logging;
using Oracle.UI.Settings;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace Oracle.Healing.Chronicle.Classes
{
    internal class ChronicleHolyPriest : ChronicleHealing
    {
        #region settings

        private static HolyPriestSettings Setting { get { return OracleSettings.Instance.HolyPriest; } }

        private static int FlashHealDSPPct { get { return Setting.FlashHealDSPPct; } }

        private static int BindingHealPct { get { return Setting.BindingHealPct; } }

        private static int HealDSPPct { get { return Setting.HealDSPPct; } }

        private static bool GreaterHealPrioEnabled { get { return Setting.GreaterHealPrioEnabled; } }

        private static bool HealPrioEnabled { get { return Setting.HealPrioEnabled; } }

        private static bool FlashHealPrioEnabled { get { return Setting.FlashHealPrioEnabled; } }

        private static bool RenewPrioEnabled { get { return Setting.RenewPrioEnabled; } }

        private static int RenewPrioPct { get { return Setting.RenewPrioPct; } }

        private static bool BindingHealPrioEnabled { get { return Setting.BindingHealPrioEnabled; } }

        private static bool PowerWordShieldPrioEnabled { get { return Setting.PowerWordShieldPrioEnabled; } }

        private static bool HolyWordSerenityPrioEnabled { get { return Setting.HolyWordSerenityPrioEnabled; } }

        #endregion settings


        public static float GetSerendipityCastTime(int points)
        {
            return points * 0.20f;
        }

        public static float GetSerendipityMana(int points)
        {
            return points * 0.20f;
        }

        private static int SERENDIPITY_STACKS { get { return (int)SpellAuras.GetAuraStackCount("Serendipity"); } }

        private static bool CHAKRA_SANCTUARY { get { return StyxWoW.Me.HasAura("Chakra: Sanctuary"); } }

        private static bool CHAKRA_SERENITY { get { return StyxWoW.Me.HasAura("Chakra: Serenity"); } }

        private static WoWUnit HealTarget { get { return OracleHealTargeting.HealableUnit ?? StyxWoW.Me; } }

        public static void GenerateSpellList()
        {
            AvailableChronicleSpells.Clear();

            //Spell POWA!
            var localPlayerSpellPower = BaseStats.GetPlayerSpellPower;
            var localPlayerSpellModifiers = BaseStats.GetPlayerSpellModifiers(StyxWoW.Me);

            //Crit chance
            const bool enable_crit_multi = true;
            var bcrit = BaseStats.GetPlayerCrit / 100;

            //Mastery sun!
            const bool enable_mast_multi = true;
            var bmast = Math.Round(BaseStats.GetPlayerMastery * 0.0125, 2); // Mastery: Echo of Light

             //Logger.Output("Mastery: {0} : [{1}] ---- calc: {2}", bmast, BaseStats.GetPlayerMastery, (1 + (float)bmast));

            //Haste!
            var hasteModifier = BaseStats.GetHasteModifier;

            var Renew = new Periodicheal
                {
                    SpellId = 774,
                    SpellName = "Renew",
                    BaseManaCost = (int)(0.026f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = 2.2F, // A little cheating to make Rejuv lower MPS therfore increasing its prio.
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.PeriodicHeal,
                    BasePeriodicCoefficient = 0.259f,
                    BasePeriodicTickFrequency = 3f,
                    BasePeriodicHeal = 2690,
                    BasePeriodicDuration = TalentManager.HasGlyph("Renew") ? 9 : 12,
                    TickType = TickType.Interval,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            Renew.HealModifier += (CHAKRA_SERENITY ? (float)0.25 : 0);
            Renew.HealModifier += (float)0.25; // Spiritual Healing
            Renew.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0));

            Renew.Calculate();

            Renew.PostCalculationModifier += (float)0.15; // Renew Heals for 15%


            if (RenewPrioEnabled && OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= RenewPrioPct) AvailableChronicleSpells[Renew.SpellName] = Renew;

            var CircleofHealing = new DirectHeal
                {
                    SpellId = 34861,
                    SpellName = "Circle of Healing",
                    BaseManaCost = (int)(0.032f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.467f,
                    BaseHeal = (4599f + 5082f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    FriendlyCount = 5f,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            CircleofHealing.HealModifier += CHAKRA_SANCTUARY ? (float)0.25 : 0;

            CircleofHealing.Calculate();

            AvailableChronicleSpells[CircleofHealing.SpellName] = CircleofHealing;

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

            GreaterHeal.HealModifier += CHAKRA_SERENITY ? (float)0.25 : 0;
            GreaterHeal.HasteModifier -= GetSerendipityCastTime(SERENDIPITY_STACKS);
            GreaterHeal.ManaCostScale -= GetSerendipityMana(SERENDIPITY_STACKS);
            //GreaterHeal.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0));

            GreaterHeal.Calculate();

            if (GreaterHealPrioEnabled && CHAKRA_SERENITY) AvailableChronicleSpells[GreaterHeal.SpellName] = GreaterHeal;

            var FlashHeal = new DirectHeal
                {
                    SpellId = 2060,
                    SpellName = "Flash Heal",
                    BaseManaCost = (int)(0.059f * BaseStats.BaseMana),
                    BaseCastTime = 1.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 1.314f,
                    BaseHeal = 14664f,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            // FlashHeal.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0));
            FlashHeal.HealModifier += CHAKRA_SERENITY ? (float)0.25 : 0;
            FlashHeal.HealModifier += (float)0.25; // Spiritual Healing

            FlashHeal.Calculate();

            if (FlashHealPrioEnabled && CHAKRA_SERENITY && OracleRoutine.IsViable(HealTarget) && (HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= FlashHealDSPPct)) AvailableChronicleSpells[FlashHeal.SpellName] = FlashHeal;

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

            //BindingHeal.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0));
            BindingHeal.HealModifier += CHAKRA_SERENITY ? (float)0.25 : 0;
            BindingHeal.HealModifier += (float)0.25; // Spiritual Healing

            BindingHeal.Calculate();

            if (BindingHealPrioEnabled && (StyxWoW.Me.HealthPercent < BindingHealPct) && OracleRoutine.IsViable(HealTarget) && (HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= FlashHealDSPPct)) AvailableChronicleSpells[BindingHeal.SpellName] = BindingHeal;

            var HolyWordSerenity = new DirectHeal
                {
                    SpellId = 88684,
                    SpellName = "Holy Word: Serenity",
                    SpellNameOverload = "Holy Word: Chastise",
                    BaseManaCost = (int)(0.02f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 1.30f,
                    BaseHeal = (12367f + 14517f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            HolyWordSerenity.HasteModifier += 1.8f; // Increase its prio
            HolyWordSerenity.HealModifier += CHAKRA_SERENITY ? (float)0.25 : 0;

            HolyWordSerenity.Calculate();

            if (HolyWordSerenityPrioEnabled && CHAKRA_SERENITY) AvailableChronicleSpells[HolyWordSerenity.SpellName] = HolyWordSerenity;

            var PrayerofMending = new DirectHeal
                {
                    SpellId = 33076,
                    SpellName = "Prayer of Mending",
                    BaseManaCost = (int)(0.035f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.571f,
                    BaseHeal = 5919f,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                    ChargeModifier = 6,
                };

            //PrayerofMending.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0));
            PrayerofMending.HealModifier += (float)0.25; // Spiritual Healing

            PrayerofMending.Calculate();

            //AvailableChronicleSpells[PrayerofMending.SpellName] = PrayerofMending;

            var Heal = new DirectHeal
                {
                    SpellId = 2050,
                    SpellName = "Heal",
                    BaseManaCost = (int)(0.019f * BaseStats.BaseMana),
                    BaseCastTime = 2.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 1.024f,
                    BaseHeal = (9848f + 11443f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            //Heal.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0));
            Heal.HealModifier += CHAKRA_SERENITY ? (float)0.25 : 0;

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

            //PrayerofHealing.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            PrayerofHealing.HealModifier += CHAKRA_SANCTUARY ? (float)0.25 : 0;
            PrayerofHealing.HasteModifier -= GetSerendipityCastTime(SERENDIPITY_STACKS);
            PrayerofHealing.ManaCostScale -= GetSerendipityMana(SERENDIPITY_STACKS);

            PrayerofHealing.Calculate();

            AvailableChronicleSpells[PrayerofHealing.SpellName] = PrayerofHealing;

            var DivineHymn = new Periodicheal
                {
                    SpellId = 64843,
                    SpellName = "Divine Hymn",
                    BaseManaCost = (int)(0.063f * BaseStats.BaseMana),
                    BaseCastTime = 8f,
                    HasteModifier = hasteModifier,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.PeriodicHeal,
                    BasePeriodicCoefficient = 1.542f,
                    BasePeriodicTickFrequency = 2f,
                    BasePeriodicHeal = 7987,
                    BasePeriodicDuration = 8,
                    FriendlyCount = 5, // 12 in 25 man
                    TickType = TickType.Interval,
                    HealModifier = (float)localPlayerSpellModifiers, //  Inner Fire, etc
                };

            //DivineHymn.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            DivineHymn.HealModifier += CHAKRA_SANCTUARY ? (float)0.25 : 0;

            DivineHymn.Calculate();

            AvailableChronicleSpells[DivineHymn.SpellName] = DivineHymn;

            var HolyWordSanctuary = new Periodicheal
                {
                    SpellId = 88685,
                    SpellName = "Holy Word: Sanctuary",
                    BaseManaCost = (int)(0.038f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.PeriodicHeal,
                    BasePeriodicCoefficient = 0.0583f,
                    BasePeriodicTickFrequency = 2f,
                    BasePeriodicHeal = (461f + 547f) / 2,
                    BasePeriodicDuration = 30,
                    TickType = TickType.Interval,
                    HealModifier = (float)localPlayerSpellModifiers, //  Inner Fire, etc
                    FriendlyCount = 6f,
                };

            //HolyWordSanctuary.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            HolyWordSanctuary.HealModifier += CHAKRA_SANCTUARY ? (float)0.25 : 0;

            HolyWordSanctuary.Calculate();

            AvailableChronicleSpells[HolyWordSanctuary.SpellName] = HolyWordSanctuary;

            var PowerWordShield = new DirectHeal
                {
                    SpellId = 17,
                    SpellName = "Power Word: Shield",
                    BaseManaCost = (int)(0.061f * BaseStats.BaseMana),
                    HasteModifier = hasteModifier,
                    ManaCostScale = (float)0.75,
                    Instant = true,
                    BaseCoefficient = 1.871f,
                    BaseHeal = 12000, //19428 -  this is doctored to match in game tooltip
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Inner Fire, etc
                };

            PowerWordShield.BaseHeal *= (enable_mast_multi ? 1 + (float)bmast : 1 + (float)(0.0));

            PowerWordShield.Calculate();

            if (PowerWordShieldPrioEnabled && CHAKRA_SERENITY) AvailableChronicleSpells[PowerWordShield.SpellName] = PowerWordShield;

            foreach (var spell in AvailableChronicleSpells)
            {
                //Console.WriteLine("{0}", spell.ToString());
            }
        }
    }
}