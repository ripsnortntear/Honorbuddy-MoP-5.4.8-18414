#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-16 12:20:32 +1000 (Mon, 16 Sep 2013) $
 * $ID$
 * $Revision: 217 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/Classes/ChronicleShaman.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core;
using Oracle.Core.Managers;
using Oracle.Core.Spells.Auras;
using Oracle.UI.Settings;
using Styx;
using Styx.WoWInternals.WoWObjects;
using System;

namespace Oracle.Healing.Chronicle.Classes
{
    internal class ChronicleShaman : ChronicleHealing
    {
        /* TODO:
         * Conductivity
         * Requires Shaman
         * Requires level 75
         * When you cast Healing Wave, Greater Healing Wave, or Healing Surge, allies
         * within your Healing Rain share healing equal to 30% of the initial healing done.
         *
         * If your Lightning Bolt, Chain Lightning, Earth Shock, or Stormstrike damages an enemy,
         * allies within your Healing Rain share healing equal to 50% of the initial damage done. */

        #region settings

        private static ShamanSettings Setting { get { return OracleSettings.Instance.Shaman; } }

        private static bool HealingSurgePrioEnabled { get { return Setting.HealingSurgePrioEnabled; } }

        private static bool HealingWavePrioEnabled { get { return Setting.HealingWavePrioEnabled; } }

        private static bool GreaterHealingWavePrioEnabled { get { return Setting.GreaterHealingWavePrioEnabled; } }

        private static bool RiptidePrioEnabled { get { return Setting.RiptidePrioEnabled; } }

        #endregion settings

        private static WoWUnit HealTarget { get { return OracleHealTargeting.HealableUnit ?? StyxWoW.Me; } }

        private static bool TIDAL_WAVES { get { return SpellAuras.GetAuraStackCount("Tidal Waves") > 0; } }

        private static bool RIPTIDE { get { return OracleRoutine.IsViable(OracleHealTargeting.HealableUnit) && HealTarget.HasAura("Riptide"); } }

        private static bool EARTHSHIELD { get { return OracleRoutine.IsViable(OracleHealTargeting.HealableUnit) && HealTarget.HasAura("Earth Shield"); } }

        private const bool Resurgence = false;

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
            var target = OracleHealTargeting.HealableUnit;
            double mastHealthDeficit = Math.Round(target.HealthPercent(target.GetHPCheckType()), 0);
            var bmast = Math.Round((BaseStats.GetPlayerMastery * 3f) / 100, 3);
            var masteryBonusonHeal = (1 - (mastHealthDeficit / 100)) * bmast;

            //Haste!
            var hasteModifier = BaseStats.GetHasteModifier;

            var HealingWave = new DirectHeal
            {
                SpellId = 331,
                SpellName = "Healing Wave",
                BaseManaCost = (int)(0.099f * BaseStats.BaseMana),
                BaseCastTime = 2.5f,  // Tidal Waves ? 0.7 : 1
                HasteModifier = hasteModifier,
                BaseCoefficient = 0.756f,
                BaseHeal = (7790f + 8899f) / 2,
                SpellType = ChronicleSpellType.DirectHeal,
                HealModifier = (float)localPlayerSpellModifiers,
                SpellPower = localPlayerSpellPower,
            };

            HealingWave.BaseManaCost -= (int)(8849 * 0.01 * 1) * (Resurgence ? 1 : 0); // Resurgence
            HealingWave.HasteModifier += TIDAL_WAVES ? 0.30f : 0;
            HealingWave.HealModifier += EARTHSHIELD ? 0.20f : 0;
            HealingWave.HealModifier += (enable_mast_multi ? (float)masteryBonusonHeal : (float)(0.0));

            HealingWave.Calculate();

            if (HealingWavePrioEnabled && TIDAL_WAVES) AvailableChronicleSpells[HealingWave.SpellName] = HealingWave;

            var GreaterHealingWave = new DirectHeal
            {
                SpellId = 77472,
                SpellName = "Greater Healing Wave",
                BaseManaCost = (int)(0.269f * BaseStats.BaseMana),
                BaseCastTime = 2.5f,  // Tidal Waves ? 0.7 : 1
                HasteModifier = hasteModifier,
                BaseCoefficient = 1.377f,
                BaseHeal = (14172f + 16190f) / 2,
                SpellType = ChronicleSpellType.DirectHeal,
                HealModifier = (float)localPlayerSpellModifiers,
                SpellPower = localPlayerSpellPower,
            };

            GreaterHealingWave.BaseManaCost -= (int)(8849 * 0.01 * 1) * (Resurgence ? 1 : 0); // Resurgence
            GreaterHealingWave.HasteModifier += TIDAL_WAVES ? 0.30f : 0;
            GreaterHealingWave.HealModifier += EARTHSHIELD ? 0.20f : 0;
            GreaterHealingWave.HealModifier += (enable_mast_multi ? (float)masteryBonusonHeal : (float)(0.0));

            GreaterHealingWave.Calculate();

            if (GreaterHealingWavePrioEnabled && TIDAL_WAVES) AvailableChronicleSpells[GreaterHealingWave.SpellName] = GreaterHealingWave;

            var HealingSurge = new DirectHeal
            {
                SpellId = 8004,
                SpellName = "Healing Surge",
                BaseManaCost = (int)(0.343f * BaseStats.BaseMana),
                BaseCastTime = 1.5f,
                HasteModifier = hasteModifier,
                BaseCoefficient = 1.135f,
                BaseHeal = (11687f + 13351f) / 2,
                SpellType = ChronicleSpellType.DirectHeal,
                HealModifier = (float)localPlayerSpellModifiers,
                SpellPower = localPlayerSpellPower,
            };

            HealingSurge.BaseManaCost -= (int)(8849 * 0.01 * 0.6) * (Resurgence ? 1 : 0); // Resurgence
            HealingSurge.HealModifier += EARTHSHIELD ? 0.20f : 0;
            HealingSurge.HealModifier += (enable_mast_multi ? (float)masteryBonusonHeal : (float)(0.0));

            HealingSurge.Calculate();

            if (HealingSurgePrioEnabled && TIDAL_WAVES) AvailableChronicleSpells[HealingSurge.SpellName] = HealingSurge;

            var UnleashLife = new DirectHeal
            {
                SpellId = 73685,
                SpellName = "Unleash Elements",
                BaseManaCost = 0,
                Instant = true,
                BaseCoefficient = 0.286f,
                BaseHeal = (3028f + 3280f) / 2,
                SpellType = ChronicleSpellType.DirectHeal,
                HealModifier = (float)localPlayerSpellModifiers,
                SpellPower = localPlayerSpellPower,
            };

            UnleashLife.HealModifier += (enable_mast_multi ? (float)masteryBonusonHeal : (float)(0.0));
            UnleashLife.HealModifier += EARTHSHIELD ? 0.20f : 0;

            UnleashLife.Calculate();

            //AvailableChronicleSpells[UnleashLife.SpellName] = UnleashLife;

            var HealingRain = new Periodicheal
            {
                SpellId = 73920,
                SpellName = "Healing Rain",
                BaseManaCost = (int)(0.366f * BaseStats.BaseMana),
                BaseCastTime = 2.0f,
                HasteModifier = hasteModifier,
                SpellPower = localPlayerSpellPower,
                SpellType = ChronicleSpellType.PeriodicHeal,
                BasePeriodicCoefficient = 0.197f,
                BasePeriodicTickFrequency = 2f,
                BasePeriodicHeal = (1983 + 2358) / 2, //TODO: 1666 to 1981 (+ 16.5% of Spell power)
                BasePeriodicDuration = 10,
                TickType = TickType.Interval,
                FriendlyCount = 6f,
                HealModifier = (float)localPlayerSpellModifiers,
            };

            HealingRain.HealModifier += (enable_mast_multi ? (float)masteryBonusonHeal : (float)(0.0));
            HealingRain.HealModifier += 0.75f; // Purification - healing done by your Healing Rain by an additional 100%. (100-25 = 75)

            HealingRain.Calculate();

            AvailableChronicleSpells[HealingRain.SpellName] = HealingRain;

            var Riptide = new Periodicheal
            {
                SpellId = 8936,
                SpellName = "Riptide",
                BaseManaCost = (int)(0.12f * BaseStats.BaseMana), // patch 5.4 down from 0.16
                HasteModifier = 2.2F, // A little cheating to make Riptide lower MPS therfore increasing its prio.
                Instant = true,
                BaseHeal = 3735f,
                BaseCoefficient = 0.339f,
                SpellPower = localPlayerSpellPower,
                SpellType = ChronicleSpellType.HybridHeal,
                BasePeriodicCoefficient = 0.96f,
                BasePeriodicTickFrequency = 3f,
                BasePeriodicHeal = 10584,
                BasePeriodicDuration = 18,
                TickType = TickType.Duration,
                HealModifier = (float)localPlayerSpellModifiers,
            };

            Riptide.BaseManaCost -= (int)(8849 * 0.01 * 0.6) * (Resurgence ? 1 : 0); // Resurgence
            Riptide.HealModifier += EARTHSHIELD ? 0.20f : 0;
            Riptide.HealModifier += (enable_mast_multi ? (float)masteryBonusonHeal : (float)(0.0));

            Riptide.Calculate();

            Riptide.CalculatedHeal *= (TalentManager.HasGlyph("Riptide") ? 0.25f : 1f); // TODO: check riptide is calculating down to 75%

            if (RiptidePrioEnabled) AvailableChronicleSpells[Riptide.SpellName] = Riptide;

            var ChainHeal = new DirectHeal
            {
                SpellId = 1064,
                SpellName = "Chain Heal",
                BaseManaCost = (int)(0.225f * BaseStats.BaseMana),
                BaseCastTime = 2.5f,
                HasteModifier = hasteModifier,
                BaseCoefficient = 0.6876f,
                BaseHeal = (7086f + 8094f) / 2,
                SpellPower = localPlayerSpellPower,
                SpellType = ChronicleSpellType.DirectHeal,
                HealModifier = (float)localPlayerSpellModifiers,
                FriendlyCount = 4f,
            };

            ChainHeal.BaseManaCost -= (int)(8849 * 0.01 * (0.333 * 4)) * (Resurgence ? 1 : 0); // Resurgence
            ChainHeal.HealModifier += RIPTIDE ? 0.25f : 0; //Riptide
            ChainHeal.HealModifier += EARTHSHIELD ? 0.20f : 0;
            ChainHeal.HealModifier += (enable_mast_multi ? (float)masteryBonusonHeal : (float)(0.0));

            ChainHeal.Calculate();

            AvailableChronicleSpells[ChainHeal.SpellName] = ChainHeal;

            ////EarthShield base Charge Heal
            //const int baseChargeHeal = 2043;

            //var EarthShield = new Periodicheal()
            //{
            //    SpellId = 8936,
            //    SpellName = "Earth Shield",
            //    BaseManaCost = (int)(0.19f * BaseStats.BaseMana),
            //    Instant = true,
            //    BaseHeal = 3735f,
            //    BaseCoefficient = 0.339f,
            //    SpellPower = localPlayerSpellPower,
            //    SpellType = ChronicleSpellType.HybridHeal,
            //    BasePeriodicCoefficient = 0.13f,
            //    BasePeriodicTickFrequency = 4f,  // Start modeling at a charge procing every 4s
            //    BasePeriodicHeal = 9f * baseChargeHeal,
            //    BasePeriodicDuration = 4f * 9f,
            //    TickType = TickType.Duration,
            //    HealModifier = (float)localPlayerSpellModifiers,
            //};

            //EarthShield.HealModifier += (enable_mast_multi ? (float)MasteryBonusonHeal : (float)(0.0));

            //EarthShield.Calculate();

            //AvailableChronicleSpells[EarthShield.SpellName] = EarthShield;

            foreach (var spell in AvailableChronicleSpells)
            {
                //Console.WriteLine("{0}", spell.ToString());
            }
        }
    }
}