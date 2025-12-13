#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/Classes/ChronicleMonk.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Classes;
using Oracle.Classes.Monk;
using Oracle.Core.Spells.Auras;
using Oracle.UI.Settings;
using Styx;

namespace Oracle.Healing.Chronicle.Classes
{
    internal class ChronicleMonk : ChronicleHealing
    {
        #region settings

        private static MonkSettings Setting { get { return OracleSettings.Instance.Monk; } }

        private static int HealingSpherePrioManaPct { get { return Setting.HealingSpherePrioManaPct; } }

        private static int HealingSphereManaPct { get { return Setting.HealingSphereManaPct; } }

        private static bool EnvelopingMistPrioEnabled { get { return Setting.EnvelopingMistPrioEnabled; } }

        private static bool SurgingVitalPrioEnabled { get { return Setting.SurgingVitalPrioEnabled; } }

        private static bool HealingSpherePrioEnabled { get { return Setting.HealingSpherePrioEnabled; } }

        private static bool SoothingMistPrioEnabled { get { return Setting.SoothingMistPrioEnabled; } }

        #endregion settings

        private static bool EnvelopingMistBuff { get { return OracleRoutine.IsViable(OracleHealTargeting.HealableUnit) && OracleHealTargeting.HealableUnit.HasAura("Enveloping Mist"); } }

        private static bool SoothingMistBuff { get { return OracleRoutine.IsViable(OracleHealTargeting.HealableUnit) && OracleHealTargeting.HealableUnit.HasAura("Soothing Mist"); } }

        private static bool NeedChi { get { return StyxWoW.Me.CurrentChi < 1; } }

        private static bool NeedHealingSphere { get { return StyxWoW.Me.ManaPercent > HealingSpherePrioManaPct && StyxWoW.Me.CurrentChi != MonkCommon.MaxChi; } }

        private static bool HasLucidity { get { return StyxWoW.Me.HasAnyAura(RotationBase.Lucidity); } }

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
            var bmast = BaseStats.GetPlayerMastery; // handle multipliers independantly below.

            //Haste!
            var hasteModifier = BaseStats.GetHasteModifier;

            //Console.Write("Mastery: {0}\n", bmast);
            //Console.Write("bcrit: {0}\n", bcrit);

            var EnvelopingMist = new Periodicheal
                {
                    SpellId = 124682,
                    SpellName = "Enveloping Mist",
                    BaseManaCost = (int)(0.04f * BaseStats.BaseMana),
                    //add some mana here so we can calculate the spell correctly for prio
                    HasteModifier = hasteModifier,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.PeriodicHeal,
                    BasePeriodicCoefficient = 2.70f,
                    BasePeriodicTickFrequency = 1f,
                    BasePeriodicHeal = 44448,
                    BasePeriodicDuration = 6,
                    TickType = TickType.Duration,
                    HealModifier = (float)localPlayerSpellModifiers,
                    BaseCastTime = SoothingMistBuff ? 0f : 2,
                    Instant = SoothingMistBuff,
                };

            //EnvelopingMist.HealModifier += (enable_mast_multi ? (float)bmast * 0.02f : (float)(0.0));

            EnvelopingMist.Calculate();

            if (EnvelopingMistPrioEnabled) AvailableChronicleSpells[EnvelopingMist.SpellName] = EnvelopingMist;

            var SurgingMist = new DirectHeal
                {
                    SpellId = 116694,
                    SpellName = "Surging Mist",
                    BaseManaCost = NeedChi ? 0 : (int)(0.088f * BaseStats.BaseMana),
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 1.80f,
                    BaseHeal = (15949f + 18535f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    BaseCastTime = SoothingMistBuff ? 0f : 1.5f,
                    Instant = SoothingMistBuff,
                };

            //SurgingMist.HealModifier += (enable_mast_multi ? (float)bmast * 1f : (float)(0.0));

            SurgingMist.Calculate();

            if (SurgingVitalPrioEnabled) AvailableChronicleSpells[SurgingMist.SpellName] = SurgingMist;

            var HealingSphere = new DirectHeal
                {
                    SpellId = 115460,
                    SpellName = "Healing Sphere",
                    BaseManaCost = (int)(0.02f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = (NeedHealingSphere) ? 1.5F : 0.1f, // A little cheating to make Healing Sphere lower MPS therfore increasing its prio.  || HasLucidity
                    //HasteModifier = hasteModifier,
                    BaseCoefficient = 1.42f, //observed ingame...
                    BaseHeal = 9986,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                };

            //HealingSphere.HealModifier += (enable_mast_multi ? (float)bmast * 1f : (float)(0.0));

            HealingSphere.Calculate();

            if (HealingSpherePrioEnabled && StyxWoW.Me.ManaPercent > HealingSphereManaPct) AvailableChronicleSpells[HealingSphere.SpellName] = HealingSphere;

            var Revival = new DirectHeal
                {
                    SpellId = 115310,
                    SpellName = "Revival",
                    BaseManaCost = (int)(0.077f * BaseStats.BaseMana),
                    Instant = true,
                    BaseCastTime = 0f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 5.00f,
                    BaseHeal = 9578.8f, // patch 5.4 reduction by 30% (13684 - 30%)
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    FriendlyCount = 25f,
                };

            //Revival.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            Revival.Calculate();

            AvailableChronicleSpells[Revival.SpellName] = Revival;

            var RenewingMist = new Periodicheal
                {
                    SpellId = 115151,
                    SpellName = "Renewing Mist",
                    BaseManaCost = 0, //(int)(0.058f * BaseStats.BaseMana) we want this cast on cooldown.
                    Instant = true,
                    BaseCastTime = 0f,
                    //HasteModifier = 2.2F, // A little cheating to make RenewingMist lower MPS therfore increasing its prio.
                    //HasteModifier = hasteModifier,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.PeriodicHeal,
                    BasePeriodicCoefficient = 0.0107f,
                    BasePeriodicTickFrequency = 2f,
                    BasePeriodicHeal = 2266,
                    BasePeriodicDuration = 18,
                    TickType = TickType.Interval,
                    HealModifier = (float)localPlayerSpellModifiers,
                    FriendlyCount = 1f,
                };

            //RenewingMist.HealModifier += (enable_mast_multi ? (float)bmast * 0.15f : (float)(0.0));

            RenewingMist.Calculate();

            AvailableChronicleSpells[RenewingMist.SpellName] = RenewingMist;

            var Uplift = new DirectHeal
                {
                    SpellId = 116670,
                    SpellName = "Uplift",
                    Instant = true,
                    BaseCastTime = 0f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.68f,
                    BaseHeal = (7210f + 8379f) / 2,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    SpellPower = localPlayerSpellPower,
                    FriendlyCount = 8f,
                };

            //Uplift.HealModifier += (enable_mast_multi ? (float)bmast * 0.25f : (float)(0.0));

            Uplift.Calculate();

            AvailableChronicleSpells[Uplift.SpellName] = Uplift;

            var LifeCocoon = new DirectHeal
                {
                    SpellId = 116849,
                    SpellName = "Life Cocoon",
                    BaseManaCost = (int)(0.05f * BaseStats.BaseMana),
                    Instant = true,
                    BaseCastTime = 0f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 5.5f,
                    BaseHeal = 39958,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    SpellPower = localPlayerSpellPower,
                };

            //LifeCocoon.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            LifeCocoon.Calculate();

            //AvailableChronicleSpells[LifeCocoon.SpellName] = LifeCocoon;

            var SoothingMist = new Periodicheal
                {
                    SpellId = 8936,
                    SpellName = "Soothing Mist",
                    BaseManaCost = (int)(0.01f * BaseStats.BaseMana),
                    ManaCostScale = 7f, /* initial plus 8 ticks */
                    BaseCastTime = 8f,
                    HasteModifier = hasteModifier,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.PeriodicHeal,
                    BasePeriodicCoefficient = 1.4336f,
                    BasePeriodicTickFrequency = 1f,
                    BasePeriodicHeal = (20552f + 23872f) / 2,
                    BasePeriodicDuration = 8,
                    TickType = TickType.Duration,
                    HealModifier = (float)localPlayerSpellModifiers,
                };

            //SoothingMist.HealModifier += (enable_mast_multi ? (float)bmast * 0.45f : (float)(0.0)); // self and statue

            SoothingMist.HealModifier += EnvelopingMistBuff ? 0.30f : 0; //Increased healing

            SoothingMist.Calculate();

            if (SoothingMistPrioEnabled) AvailableChronicleSpells[SoothingMist.SpellName] = SoothingMist;

            foreach (var spell in AvailableChronicleSpells)
            {
                //Console.WriteLine("{0}", spell.ToString());
            }
        }
    }
}