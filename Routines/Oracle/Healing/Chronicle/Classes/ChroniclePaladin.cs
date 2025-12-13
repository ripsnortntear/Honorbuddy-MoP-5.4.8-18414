#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/Classes/ChroniclePaladin.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info
using Oracle.Core;
using Oracle.Classes;
using Oracle.Classes.Paladin;
using Oracle.Core.Managers;
using Oracle.Core.Spells.Auras;
using Oracle.Core.WoWObjects;
using Oracle.UI.Settings;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace Oracle.Healing.Chronicle.Classes
{
    internal class ChroniclePaladin : ChronicleHealing
    {
        #region settings

        private static PaladinSettings Setting { get { return OracleSettings.Instance.Paladin; } }

        private static bool UseHolyPrismSingleTarget { get { return Setting.UseHolyPrismSingleTarget; } }

        private static int DivineLightDSPPct { get { return Setting.DivineLightDSPPct; } }

        private static int HolyLightDSPPct { get { return Setting.HolyLightDSPPct; } }

        private static int FlashofLightDSPPct { get { return Setting.FlashofLightDSPPct; } }

        private static int EternalFlamePlayerHP { get { return Setting.EternalFlamePlayerHP; } }

        #endregion settings

        private static float GetSelflesshealerCost(int stacks)
        {
            if (stacks == 3)
                return 0f;

            return 0.35f * stacks;
        }

        private static float GetSelflesshealerImprovement(int stacks)
        {
            return stacks * 0.20f;
        }

        private static uint CurrentHolyPower { get { return StyxWoW.Me.CurrentHolyPower; } }

        private static bool InfusionOfLight { get { return StyxWoW.Me.ActiveAuras.ContainsKey("Infusion of Light"); } }

        public static bool HasSelflessHealer { get { return PaladinCommon.HasTalent(PaladinTalents.SelflessHealer) && StyxWoW.Me.HasAura(RotationBase.SelflessHealer); } }

        public static int SelflesshealerStacks { get { return (int)SpellAuras.GetAuraStackCount(RotationBase.SelflessHealer); } }

        private static bool HasDivinePurpose { get { return StyxWoW.Me.ActiveAuras.ContainsKey("Divine Purpose"); } }

        private static WoWUnit HealTarget { get { return OracleHealTargeting.HealableUnit ?? StyxWoW.Me; } }

        private static WoWUnit Tank { get { return OracleTanks.PrimaryTank; } }

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
            var bmast = BaseStats.GetPlayerMastery * 0.015f;

            //Haste!
            var hasteModifier = BaseStats.GetHasteModifier;

            var DivineLight = new DirectHeal
                {
                    SpellId = 82326,
                    SpellName = "Divine Light",
                    BaseManaCost = (int)(0.36f * BaseStats.BaseMana),
                    BaseCastTime = InfusionOfLight ? (2.5f - 1.5f) : 2.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 1.49f,
                    BaseHeal = (15910f + 17725f) / 2,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    SpellPower = localPlayerSpellPower,
                };

            //DivineLight.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            DivineLight.HasteModifier -= HasSelflessHealer ? GetSelflesshealerCost(SelflesshealerStacks) : 0f;
            DivineLight.ManaCostScale -= HasSelflessHealer ? GetSelflesshealerCost(SelflesshealerStacks) : 0f;
            if (HasSelflessHealer && SelflesshealerStacks == 3)
            {
                DivineLight.ManaCostScale = 0f;
            }

            DivineLight.HealModifier += HasSelflessHealer ? GetSelflesshealerImprovement(SelflesshealerStacks) : 0f;

            DivineLight.Calculate();

            if (OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= DivineLightDSPPct) AvailableChronicleSpells[DivineLight.SpellName] = DivineLight;

            var HolyLight = new DirectHeal
                {
                    SpellId = 635,
                    SpellName = "Holy Light",
                    BaseManaCost = (int)(0.126f * BaseStats.BaseMana),
                    BaseCastTime = InfusionOfLight ? (2.5f - 1.5f) : 2.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.785f,
                    BaseHeal = (8390f + 9347f) / 2,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    SpellPower = localPlayerSpellPower,
                };

            // HolyLight.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            HolyLight.Calculate();

            if (OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= HolyLightDSPPct) AvailableChronicleSpells[HolyLight.SpellName] = HolyLight;

            var FlashofLight = new DirectHeal
                {
                    SpellId = 19750,
                    SpellName = "Flash of Light",
                    BaseManaCost = (int)(0.378f * BaseStats.BaseMana), // Calculate selfless healer?...YES!!
                    BaseCastTime = 1.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 1.12f,
                    BaseHeal = (11882f + 13331f) / 2,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    SpellPower = localPlayerSpellPower,
                };

            //FlashofLight.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            FlashofLight.HasteModifier -= HasSelflessHealer ? GetSelflesshealerCost(SelflesshealerStacks) : 0f;
            FlashofLight.ManaCostScale -= HasSelflessHealer ? GetSelflesshealerCost(SelflesshealerStacks) : 0f;
            if (HasSelflessHealer && SelflesshealerStacks == 3)
            {
                FlashofLight.ManaCostScale = 0f;
            }

            FlashofLight.HealModifier += HasSelflessHealer ? GetSelflesshealerImprovement(SelflesshealerStacks) : 0f;

            FlashofLight.Calculate();

            if (OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) <= FlashofLightDSPPct) AvailableChronicleSpells[FlashofLight.SpellName] = FlashofLight;

            var HolyShock = new DirectHeal
                {
                    SpellId = 20473,
                    SpellName = "Holy Shock",
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseManaCost = (int)(0.8f * BaseStats.BaseMana), // •Holy Shock's mana cost has been reduced by 50%. (5.4 change)
                    BaseCoefficient = 0.833f,
                    BaseHeal = (9014f + 9764f) / 2,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    SpellPower = localPlayerSpellPower,
                };

            //HolyShock.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            HolyShock.Calculate();

            AvailableChronicleSpells[HolyShock.SpellName] = HolyShock;

            var WordofGlory = new DirectHeal
                {
                    SpellId = 85673,
                    SpellName = "Word of Glory",
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseManaCost = 0,
                    BaseCoefficient = 0.49f,
                    BaseHeal = (4803f + 5350f) / 2,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    SpellPower = localPlayerSpellPower,
                    ChargeModifier = (CurrentHolyPower > 2 || HasDivinePurpose) ? 3 : CurrentHolyPower,
                };

            //WordofGlory.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            WordofGlory.HealModifier += 0.25f; // Holy Insight increases the effectiveness by 50% (added in patch 5.4)

            WordofGlory.Calculate();

            if (!PaladinCommon.HasTalent(PaladinTalents.EternalFlame)) AvailableChronicleSpells[WordofGlory.SpellName] = WordofGlory;

            var HolyRadiance = new DirectHeal
                {
                    SpellId = 82327,
                    SpellName = "Holy Radiance",
                    BaseManaCost = (int)(0.36f * BaseStats.BaseMana),
                    BaseCastTime = InfusionOfLight ? (2.5f - 1.5f) : 2.5f,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.675f,
                    BaseHeal = 5664,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    FriendlyCount = 7f,
                    AOEHealModifier = 0.50f, // and all allies within 10 yards for 50% of that amount
                };

            // HolyRadiance.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            HolyRadiance.HasteModifier -= HasSelflessHealer ? GetSelflesshealerCost(SelflesshealerStacks) : 0f;
            HolyRadiance.ManaCostScale -= HasSelflessHealer ? GetSelflesshealerCost(SelflesshealerStacks) : 0f;
            if (HasSelflessHealer && SelflesshealerStacks == 3)
            {
                HolyRadiance.ManaCostScale = 0f;
            }

            HolyRadiance.HealModifier += HasSelflessHealer ? GetSelflesshealerImprovement(SelflesshealerStacks) : 0f;

            HolyRadiance.Calculate();

            AvailableChronicleSpells[HolyRadiance.SpellName] = HolyRadiance;

            var LightofDawn = new DirectHeal
                {
                    SpellId = 85222,
                    SpellName = "Light of Dawn",
                    BaseManaCost = 0,
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseCoefficient = 0.152f,
                    BaseHeal = (1627f + 1812f) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    //Glyph of Light of Dawn (Holy), Light of Dawn affects 2 fewer targets, but heals each target for 25% more
                    FriendlyCount = TalentManager.HasGlyph("Light of Dawn") ? 4f : 6f,
                    AOEHealModifier = TalentManager.HasGlyph("Light of Dawn") ? 1.25f : 1f,
                    ChargeModifier = (CurrentHolyPower > 2 || HasDivinePurpose) ? 3 : CurrentHolyPower,
                };

            // LightofDawn.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            LightofDawn.HealModifier += 0.25f; // Holy Insight increases the effectiveness by 50% (added in patch 5.4)

            LightofDawn.Calculate();

            AvailableChronicleSpells[LightofDawn.SpellName] = LightofDawn;

            var HolyPrism = new DirectHeal
                {
                    SpellId = 114165,
                    SpellName = "Holy Prism",
                    BaseManaCost = (int)(0.054f * BaseStats.BaseMana),
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseHeal = 17750,
                    BaseCoefficient = 1.428f,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    FriendlyCount = 1f,
                };

            //HolyPrism.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            HolyPrism.Calculate();

            if (UseHolyPrismSingleTarget) AvailableChronicleSpells[HolyPrism.SpellName] = HolyPrism;

            var EternalFlame = new Periodicheal
                {
                    SpellId = 114163,
                    SpellName = "Eternal Flame",
                    BaseManaCost = 0,//(int)(0.10f * BaseStats.BaseMana) //add some mana here so we can calculate the spell correctly for prio
                    Instant = true,
                    HasteModifier = hasteModifier,
                    BaseHeal = (5240f + 5837f) / 2,
                    BaseCoefficient = 0.49f,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.HybridHeal,
                    BasePeriodicCoefficient = 0.0819f, // ernal Flame's periodic heal-over-time effect now heals for 40% more.
                    BasePeriodicTickFrequency = 3f * hasteModifier,
                    BasePeriodicHeal = 711, // ernal Flame's periodic heal-over-time effect now heals for 40% more.
                    BasePeriodicDuration = 30,
                    TickType = TickType.Interval,
                    HealModifier = (float)localPlayerSpellModifiers,
                    ChargeModifier = (CurrentHolyPower > 2 || HasDivinePurpose) ? 3 : CurrentHolyPower,
                };

            // EternalFlame.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));
            EternalFlame.HealModifier += 0.25f; // Holy Insight increases the effectiveness by 50% (added in patch 5.4)

            EternalFlame.Calculate();

            if (PaladinCommon.HasTalent(PaladinTalents.EternalFlame) && (CurrentHolyPower >= EternalFlamePlayerHP || HasDivinePurpose || (StyxWoW.Me.Combat && StyxWoW.Me.CurrentHolyPower == 5)) && (OracleRoutine.IsViable(Tank) && (Tank.HealthPercent(HPCheck.Tank) > 50 || Tank.HasAura(RotationBase.EternalFlame)))) AvailableChronicleSpells[EternalFlame.SpellName] = EternalFlame;

            //var SacredShield = new Periodicheal()
            //{
            //    SpellId = 20925,
            //    SpellName = "Sacred Shield",
            //    BaseManaCost = (int)(0.16f * BaseStats.BaseMana), // Mana cost changed in 5.4
            //    Instant = true,
            //    SpellPower = localPlayerSpellPower,
            //    SpellType = ChronicleSpellType.PeriodicHeal,
            //    BasePeriodicCoefficient = 1.17f, // 0.0117 ??
            //    BasePeriodicTickFrequency = 6f,
            //    BasePeriodicHeal = 343,
            //    BasePeriodicDuration = 30,
            //    TickType = TickType.Interval,
            //    HealModifier = (float)localPlayerSpellModifiers,
            //};

            //SacredShield.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            //SacredShield.Calculate();

            //AvailableChronicleSpells[SacredShield.SpellName] = SacredShield;

            var ExecutionSentence = new DirectHeal
                {
                    SpellId = 114157,
                    SpellName = "Execution Sentence",
                    BaseManaCost = 0,
                    Instant = true,
                    BaseCoefficient = 5.936f,
                    BaseHeal = (float)12989.4,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                };

            // ExecutionSentence.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            ExecutionSentence.Calculate();

            AvailableChronicleSpells[ExecutionSentence.SpellName] = ExecutionSentence;

            var LightsHammer = new DirectHeal
                {
                    SpellId = 114158,
                    SpellName = "Light's Hammer",
                    BaseManaCost = 0,
                    Instant = true,
                    BaseCoefficient = 0.321f,
                    BaseHeal = (3268 + 3993) / 2,
                    SpellPower = localPlayerSpellPower,
                    SpellType = ChronicleSpellType.DirectHeal,
                    HealModifier = (float)localPlayerSpellModifiers,
                    FriendlyCount = 7 * 6f,
                };

            // LightsHammer.HealModifier += (enable_mast_multi ? (float)bmast : (float)(0.0)) + (enable_crit_multi ? (float)bcrit : (float)(0.0));

            LightsHammer.Calculate();

            AvailableChronicleSpells[LightsHammer.SpellName] = LightsHammer;

            foreach (var spell in AvailableChronicleSpells)
            {
                //Console.WriteLine("{0}", spell.ToString());
            }
        }
    }
}