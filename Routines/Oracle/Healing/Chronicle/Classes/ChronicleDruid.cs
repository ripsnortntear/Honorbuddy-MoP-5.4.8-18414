#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 16:25:58 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 208 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/Classes/ChronicleDruid.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Classes;
using Oracle.Core;
using Oracle.Core.Managers;
using Oracle.Core.Spells.Auras;
using Oracle.UI.Settings;
using Styx;
using System;
using Styx.WoWInternals.WoWObjects;

namespace Oracle.Healing.Chronicle.Classes
{
    internal class ChronicleDruid : ChronicleHealing
    {
        #region settings

        private static DruidSettings Setting { get { return OracleSettings.Instance.Druid; } }

        private static bool DisableNourish { get { return Setting.DisableNourish; } }

        private static int RegrowthPct { get { return Setting.RegrowthPct; } }

        private static int HealingTouchPct { get { return Setting.HealingTouchPct; } }

        private static bool EnableTier16Support { get { return Setting.EnableTier16Support; } }

        
        #endregion settings

        // druid Tier 16 Support
        private static float GetSageMenderCost(int stacks)
        {
            if (stacks == 0)
                return 0f;

            return 0.20f * stacks;
        }

        // druid Tier 16 Support
        private static float GetSageMenderCastTime(int stacks)
        {
            if (stacks == 0)
                return 0f;

            return stacks * 0.20f;
        }

        public static int SageMenderStacks { get { return EnableTier16Support ? (int)SpellAuras.GetAuraStackCount(RotationBase.SageMender) : 0; } } // druid Tier 16 Support

        private static WoWUnit HealTarget { get { return OracleHealTargeting.HealableUnit; } }

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
            var bmast = Math.Round(BaseStats.GetPlayerMastery * 0.0125, 2); // Harmony Multiplier.

            //Logger.Output("Mastery: {0} : [{1}] ---- calc: {2}", bmast, BaseStats.GetPlayerMastery, (1 + (float)bmast));

            //Haste!
            var hasteModifier = BaseStats.GetHasteModifier;

            //Shrooms//(4031 + (SpellPower*0.414) + MaxHealth*0.67) * (masterymodifier+Healmodifier

            var Lifebloom = new Periodicheal
                {
                    SpellId = 774,
                    SpellName = "Lifebloom",
                    BaseManaCost = (int)(0.059f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.PeriodicHeal,
                    BasePeriodicCoefficient = 0.855f,
                    BasePeriodicTickFrequency = 1f,
                    BasePeriodicHeal = 9315,
                    BasePeriodicDuration = 15,
                    TickType = TickType.Duration,
                    HealModifier = (float)localPlayerSpellModifiers, // Naturalist, etc
                    ChargeModifier = 1, //TODO: IMPLEMNT STACK COUNT * 3
                };

            Lifebloom.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? bcrit : (float)(0.0));

            Lifebloom.Calculate();

            //AvailableChronicleSpells[Lifebloom.SpellName] = Lifebloom;

            var Nourish = new DirectHeal
                {
                    SpellId = 50464,
                    SpellName = "Nourish",
                    BaseManaCost = (int)(0.102f * BaseStats.BaseMana),
                    BaseCastTime = 2.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.614f,
                    BaseHeal = (6151f + 7148f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Naturalist, etc
                };

            Nourish.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? bcrit : (float)(0.0));

            Nourish.Calculate();

            if (!DisableNourish) AvailableChronicleSpells[Nourish.SpellName] = Nourish;

            var HealingTouch = new DirectHeal
                {
                    SpellId = 5185,
                    SpellName = "Healing Touch",
                    BaseManaCost = (int)((TalentManager.HasGlyph("Regrowth") && (SageMenderStacks == 0) ? 1.4f : 0.289f) * BaseStats.BaseMana), // A little cheating to make HT crap when regrowth is glyphed. //.
                    BaseCastTime = 2.5f,
                    HasteModifier = ((TalentManager.HasGlyph("Regrowth") && (SageMenderStacks == 0)) ? 1.8f : hasteModifier),
                    BaseCoefficient = 1.86f,
                    BaseHeal = (18460f + 21800f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Naturalist, etc
                };

            HealingTouch.BaseManaCost -= (int)(HealingTouch.BaseManaCost * GetSageMenderCost(SageMenderStacks)); // druid Tier 16 Support
            HealingTouch.BaseCastTime -= (2.5f * GetSageMenderCastTime(SageMenderStacks)); // druid Tier 16 Support
            HealingTouch.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0));

            HealingTouch.Calculate();

            if (OracleRoutine.IsViable(HealTarget) && (HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= HealingTouchPct)) AvailableChronicleSpells[HealingTouch.SpellName] = HealingTouch;

            var WildMushroomBloom = new DirectHeal
                {
                    SpellId = 102791,
                    SpellName = "Wild Mushroom: Bloom",
                    BaseManaCost = (int)(0.102f * BaseStats.BaseMana),
                    Instant = true,
                    BaseCoefficient = 0.414f,
                    BaseHeal = (4043f + 4890f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Naturalist, etc
                    ChargeModifier = 1, // 3 shrooms out.
                    FriendlyCount = 7, // TODO: Detect how many players in group.
                };

            WildMushroomBloom.Calculate();

            AvailableChronicleSpells[WildMushroomBloom.SpellName] = WildMushroomBloom;

            var Rejuvenation = new Periodicheal
                {
                    SpellId = 774,
                    SpellName = "Rejuvenation",
                    BaseManaCost = (int)(0.005f * BaseStats.BaseMana), //0.145f 
                    Instant = true,
                    HasteModifier = hasteModifier, // A little cheating to make Rejuv lower MPS therfore increasing its prio.
                    BaseHeal = 4234,
                    BaseCoefficient = 0.392f,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.HybridHeal,
                    BasePeriodicCoefficient = 0.392f,
                    BasePeriodicTickFrequency = 3f,
                    BasePeriodicHeal = 4234,
                    BasePeriodicDuration = 12,
                    TickType = TickType.Interval,
                    HealModifier = (float)localPlayerSpellModifiers, // Naturalist, etc
                };

            //Rejuvenation.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            Rejuvenation.Calculate();

            AvailableChronicleSpells[Rejuvenation.SpellName] = Rejuvenation;

            var Swiftmend = new Periodicheal
                {
                    SpellId = 774,
                    SpellName = "Swiftmend",
                    BaseManaCost = (int)(0.085f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseHeal = 13966,
                    BaseCoefficient = 1.29f,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.HybridHeal,
                    BasePeriodicCoefficient = 1.29f,
                    BasePeriodicTickFrequency = 2f, //patch 5.4.1 up from 1 seconds.
                    BasePeriodicHeal = 13966,
                    BasePeriodicDuration = 8, // patch 5.4.1 up from 7 seconds.
                    TickType = TickType.Duration,
                    HealModifier = (float)localPlayerSpellModifiers, // Naturalist, etc
                    FriendlyCount = 1f,
                };

            Swiftmend.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0));

            Swiftmend.BasePeriodicDuration = (TalentManager.HasGlyph("Efflorescence") ? 1f : 8f); // added in patch 5.4 this should remove the HoT aspect of the heal.
            Swiftmend.BasePeriodicHeal = (TalentManager.HasGlyph("Efflorescence") ? 0 : 13966); // added in patch 5.4 this should remove the HoT aspect of the heal.

            Swiftmend.Calculate();

            Swiftmend.CalculatedHeal *= (TalentManager.HasGlyph("Efflorescence") ? 1.20f : 1f); // added in patch 5.4
            Swiftmend.CalculatedPeriodicHeal *= 0.5f; // added in patch 5.4

            AvailableChronicleSpells[Swiftmend.SpellName] = Swiftmend;

            var WildGrowth = new Periodicheal
                {
                    SpellId = 48438,
                    SpellName = "Wild Growth",
                    BaseManaCost = (int)(0.229f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BasePeriodicCoefficient = 0.644f,
                    BasePeriodicTickFrequency = 1f,
                    BasePeriodicHeal = 6930f,
                    BasePeriodicDuration = 7,
                    TickType = TickType.Duration,
                    SpellPower = localPlayerSpellPower,
                    FriendlyCount = 5f,
                    SpellType = ChronicleSpellType.PeriodicHeal,
                    HealModifier = (float)localPlayerSpellModifiers, // Naturalist, etc
                };

            WildGrowth.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? bcrit : (float)(0.0));

            WildGrowth.Calculate();

            AvailableChronicleSpells[WildGrowth.SpellName] = WildGrowth;

            var Regrowth = new Periodicheal
                {
                    SpellId = 8936,
                    SpellName = "Regrowth",
                    BaseManaCost = (int)(0.297f * BaseStats.BaseMana),
                    BaseCastTime = 1.5f,
                    HasteModifier = hasteModifier,
                    CanCrit = true,
                    EffectiveCritModifier = TalentManager.HasGlyph("Regrowth") ? 2f : 1.2f,
                    BaseHeal = (9813f + 10954f) / 2,
                    BaseCoefficient = 0.958f,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.HybridHeal,
                    BasePeriodicCoefficient = 0.219f,
                    BasePeriodicTickFrequency = 2f,
                    BasePeriodicHeal = 2361,
                    BasePeriodicDuration = 6,
                    TickType = TickType.Interval,
                    HealModifier = (float)localPlayerSpellModifiers, // Naturalist, etc
                };

            Regrowth.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0));
            Regrowth.HasteModifier += TalentManager.HasGlyph("Regrowth") ? 1.1F : 0; // A little cheating to make regrth lower MPS therfore increasing its prio.

            Regrowth.Calculate();

            Regrowth.CalculatedPeriodicHeal *= TalentManager.HasGlyph("Regrowth") ? 0 : 1;

            if (OracleRoutine.IsViable(HealTarget) && (HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= RegrowthPct)) AvailableChronicleSpells[Regrowth.SpellName] = Regrowth;

            foreach (var spell in AvailableChronicleSpells)
            {
                //Console.WriteLine("{0}", spell.ToString());
            }
        }
    }
}