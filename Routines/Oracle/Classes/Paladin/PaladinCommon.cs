#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Paladin/PaladinCommon.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.Spells.Auras;
using Oracle.Healing.Chronicle.Classes;
using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.UI.Settings;
using Styx;
using Styx.WoWInternals.WoWObjects;

namespace Oracle.Classes.Paladin
{
    public enum PaladinTalents
    {
        SpeedOfLight = 1,
        LongArmOfTheLaw,
        PursuitOfJustice,
        FistOfJustice,
        Repentance,
        BurdenOfGuilt,
        SelflessHealer,
        EternalFlame,
        SacredShield,
        HandOfPurity,
        UnbreakableSpirit,
        Clemency,
        HolyAvenger,
        SanctifiedWrath,
        DivinePurpose,
        HolyPrism,
        LightsHammer,
        ExecutionSentence
    }

    public static class PaladinCommon
    {
        #region Settings

        private static PaladinSettings Setting { get { return OracleSettings.Instance.Paladin; } }

        public static int DivineLightPct { get { return Setting.DivineLightPct; } }

        public static int GuardianofAncientKingsPct { get { return Setting.GuardianofAncientKingsPct; } }

        public static int DivineProtectionPct { get { return Setting.DivineProtectionPct; } }

        public static int DivineShieldPct { get { return Setting.DivineShieldPct; } }

        public static int DivinePleaPct { get { return Setting.DivinePleaPct; } }

        public static int ExecutionSentencePct { get { return Setting.ExecutionSentencePct; } }

        public static int FlashofLightTankPct { get { return Setting.FlashofLightTankPct; } }

        public static int HandOfProtectionPct { get { return Setting.HandOfProtectionPct; } }

        public static int HandofPurityPct { get { return Setting.HandofPurityPct; } }

        public static int HandOfSalvationPct { get { return Setting.HandOfSalvationPct; } }

        public static int HandOfSacrificePct { get { return Setting.HandOfSacrificePct; } }

        public static bool UseHandOfFreedom { get { return Setting.UseHandOfFreedom; } }

        public static int LayOnHandsTankPct { get { return Setting.LayOnHandsTankPct; } }

        public static int LayOnHandsHealTargetPct { get { return Setting.LayOnHandsHealTargetPct; } }

        public static bool UseCrusaderStrike { get { return Setting.UseCrusaderStrike; } }

        public static int BuildIlluminatedHealingManaPct { get { return Setting.BuildIlluminatedHealingManaPct; } }

        public static int EternalFlameBlanketCount { get { return Setting.EternalFlameBlanketCount; } }

        public static int EternalFlameBlanketHPCount { get { return Setting.EternalFlameBlanketHPCount; } }

        public static int EternalFlameTankHP { get { return Setting.EternalFlameTankHP; } }

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)
        public static int HolyPrismPct { get { return Setting.HolyPrismPct; } }

        public static int HolyPrismLimit { get { return Setting.HolyPrismLimit; } }

        public static int LightsHammerPct { get { return Setting.LightsHammerPct; } }

        public static int LightsHammerLimit { get { return Setting.LightsHammerLimit; } }

        public static int LowManaPct { get { return Setting.LowManaPct; } }

        public static int HolyRadiancePct { get { return ((ChroniclePaladin.HasSelflessHealer && ChroniclePaladin.SelflesshealerStacks == 3) ? Setting.HolyRadianceSHPct : StyxWoW.Me.CurrentHolyPower == 5 ? 100 : Setting.HolyRadiancePct); } }

        public static int HolyRadianceLimit { get { return ((ChroniclePaladin.HasSelflessHealer && ChroniclePaladin.SelflesshealerStacks == 3) ? Setting.HolyRadianceSHLimit : StyxWoW.Me.CurrentHolyPower == 5 ? 0 : Setting.HolyRadianceLimit); } }

        public static int LightofDawnPct { get { return HasDivinePurpose ? 98 : Setting.LightofDawnPct; } }

        public static int LightofDawnLimit { get { return Setting.LightofDawnLimit; } }

        // we ignore all settings and start healing like hell!!!
        public static int UrgentHealthPercentage { get { return Setting.UrgentHealthPercentage; } }

        // Oh shit moments!
        public static int DivineFavorLimit { get { return Setting.DivineFavorLimit; } }

        public static int DivineFavorPct { get { return Setting.DivineFavorPct; } }

        public static int AvengingWrathLimit { get { return Setting.AvengingWrathLimit; } }

        public static int AvengingWrathPct { get { return Setting.AvengingWrathPct; } }

        public static int DevotionAuraLimit { get { return Setting.DevotionAuraLimit; } }

        public static int DevotionAuraPct { get { return Setting.DevotionAuraPct; } }

        public static int GuardianofAncientKingsAoELimit { get { return Setting.GuardianofAncientKingsAoELimit; } }

        public static int GuardianofAncientKingsAoEPct { get { return Setting.GuardianofAncientKingsAoEPct; } }

        #endregion Settings

        #region booleans

        public static bool CanCastHPConsumer { get { return StyxWoW.Me.CurrentHolyPower >= 3 || HasDivinePurpose; } }

        public static bool HasDivinePurpose { get { return StyxWoW.Me.ActiveAuras.ContainsKey("Divine Purpose"); } }

        #endregion booleans

        #region Monk CreateClusteredHealBehavior

        public static void LoadClusterSpells()
        {
            OracleRoutine.Instance.ClusteredSpells.Clear();

            var key = 1;

            if (PaladinCommon.HasTalent(PaladinTalents.HolyPrism))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.HolyPrism, SpellType.Proximity, HolyPrismLimit, 0, HolyPrismPct)); key++; }

            if (PaladinCommon.HasTalent(PaladinTalents.LightsHammer))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.LightsHammer, SpellType.GroundEffect, LightsHammerLimit, 0, LightsHammerPct)); key++; }

            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.LightofDawn, SpellType.NearbyLowestHealth, LightofDawnLimit, 0, LightofDawnPct)); key++;

            OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.HolyRadiance, SpellType.Proximity, HolyRadianceLimit, 0, HolyRadiancePct));
        }

        #endregion Monk CreateClusteredHealBehavior

        #region Monk CreateCooldownBehavior

        public static void LoadCooldownSpells()
        {
            OracleRoutine.Instance.CooldownSpells.Clear();

            var key = 1;

            // we use NearbyLowestHealth so that we capture the units around us that are in trouble.
            // ClusterSpell accepts a delegate to pass into the composite during hook creation.

            OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.AvengingWrath, SpellType.NearbyLowestHealth, AvengingWrathLimit, 0, AvengingWrathPct, ret => !StyxWoW.Me.HasAura(RotationBase.DivineFavor) && !StyxWoW.Me.HasAura(RotationBase.DevotionAura))); key++;
            OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.DevotionAura, SpellType.NearbyLowestHealth, DevotionAuraLimit, 0, DevotionAuraPct, ret => !StyxWoW.Me.HasAura(RotationBase.DivineFavor) && !StyxWoW.Me.HasAura(RotationBase.DevotionAura))); key++;
            OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.GuardianofAncientKings, SpellType.NearbyLowestHealth, GuardianofAncientKingsAoELimit, 0, GuardianofAncientKingsAoEPct, ret => !StyxWoW.Me.HasAura(RotationBase.DivineFavor) && !StyxWoW.Me.HasAura(RotationBase.AvengingWrath))); key++;
            OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.DivineFavor, SpellType.NearbyLowestHealth, DivineFavorLimit, 0, DivineFavorPct, ret => !StyxWoW.Me.HasAura(RotationBase.DivineFavor) && !StyxWoW.Me.HasAura(RotationBase.DevotionAura)));
        }

        #endregion Monk CreateCooldownBehavior

        public static bool HasTalent(PaladinTalents tal)
        {
            return TalentManager.IsSelected((int)tal);
        }

        // Used to keep track of when we are building Illuminated Healing Buffs (to get through stopcast.)
        public static bool BuildingIlluminatedHealingBuffs { get; set; }

        public static bool CanUseHandAbility(WoWUnit player, int handToCheck)
        {
            const int handOfPurity = RotationBase.HandOfPurity;
            const int handofFreedom = RotationBase.HandofFreedom;
            const int handofProtection = RotationBase.HandofProtection;
            const int handofSacrifice = RotationBase.HandofSacrifice;
            const int handofSalvation = RotationBase.HandofSalvation;

            switch (handToCheck)
            {
                case handOfPurity:
                    return OracleRoutine.IsViable(player) && !player.HasAnyAura(handofFreedom, handofProtection, handofSacrifice, handofSalvation);

                case handofFreedom:
                    return OracleRoutine.IsViable(player) && !player.HasAnyAura(handOfPurity, handofProtection, handofSacrifice, handofSalvation);

                case handofProtection:
                    return OracleRoutine.IsViable(player) && !player.HasAnyAura(handofFreedom, handOfPurity, handofSacrifice, handofSalvation);

                case handofSacrifice:
                    return OracleRoutine.IsViable(player) && !player.HasAnyAura(handofFreedom, handofProtection, handOfPurity, handofSalvation);

                case handofSalvation:
                    return OracleRoutine.IsViable(player) && !player.HasAnyAura(handofFreedom, handofProtection, handofSacrifice, handOfPurity);
            }

            return false;
        }
    }
}