#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Priest/PriestCommon.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.Spells.Auras;
using Oracle.Core.WoWObjects;
using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.UI.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Linq;

namespace Oracle.Classes.Priest
{
    public enum PriestTalents
    {
        VoidTendrils = 1,
        Psyfiend,
        DominateMind,
        BodyAndSoul,
        AngelicFeather,
        Phantasm,
        FromDarknessComesLight,
        Mindbender,
        SolaceAndInsanity,
        DesperatePrayer,
        SpectralGuise,
        AngelicBulwark,
        TwistOfFate,
        PowerInfusion,
        DivineInsight,
        Cascade,
        DivineStar,
        Halo
    }

    public static class PriestCommon
    {
        public static bool HasTalent(PriestTalents tal)
        {
            return TalentManager.IsSelected((int)tal);
        }

        #region OracleSettings

        private static OracleSettings OSettings { get { return OracleSettings.Instance; } }

        private static bool PvPSupport { get { return OSettings.PvPSupport; } }

        internal static bool EnableHymeofHope { get { return StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline ? DiscSetting.EnableHymeofHope : HolySetting.EnableHymeofHope; } }

        #endregion OracleSettings

        #region Disc Settings

        private static DiscPriestSettings DiscSetting { get { return OracleSettings.Instance.DiscPriest; } }

        internal static bool UseFade { get { return DiscSetting.UseFade; } }

        internal static bool UsePenanceOnEnemy { get { return DiscSetting.UsePenanceOnEnemy; } }

        internal static bool UsePowerWordFortitude { get { return DiscSetting.UsePowerWordFortitude; } }

        internal static bool EnableOffensiveDps { get { return DiscSetting.EnableOffensiveDps; } }

        internal static int PainSuppressionPercent { get { return DiscSetting.PainSuppressionPercent; } }

        internal static int FlashHealSoLPct { get { return DiscSetting.FlashHealSoLPct; } }

        internal static int GreaterHealPct { get { return DiscSetting.GreaterHealPct; } }

        internal static int PrayerOfMendingPct { get { return DiscSetting.PrayerOfMendingPct; } }

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)
        internal static int DivineStarPct { get { return DiscSetting.DivineStarPct; } }

        internal static int DivineStarCount { get { return DiscSetting.DivineStarCount; } }

        internal static int CascadePct { get { return DiscSetting.CascadePct; } }

        internal static int CascadeCount { get { return DiscSetting.CascadeCount; } }

        internal static int HaloPct { get { return DiscSetting.HaloPct; } }

        internal static int HaloCount { get { return DiscSetting.HaloCount; } }

        internal static int PrayerofHealingPct { get { return DiscSetting.PrayerofHealingPct; } }

        internal static int PrayerofHealingCount { get { return DiscSetting.PrayerofHealingCount; } }

        internal static int PowerWordBarrierPct { get { return DiscSetting.PowerWordBarrierPct; } }

        internal static int PowerWordBarrierCount { get { return DiscSetting.PowerWordBarrierCount; } }

        //Oh Shit Moments!
        internal static int DesperatePrayerPct { get { return DiscSetting.DesperatePrayerPct; } }

        internal static int ArchangelPct { get { return DiscSetting.ArchangelPct; } }

        internal static int ArchangelCount { get { return DiscSetting.ArchangelCount; } }

        internal static int PowerInfusionPct { get { return DiscSetting.PowerInfusionPct; } }

        internal static int PowerInfusionCount { get { return DiscSetting.PowerInfusionCount; } }

        internal static int VoidShiftPct { get { return DiscSetting.VoidShiftPct; } }

        internal static int SpiritShellPct { get { return DiscSetting.SpiritShellPct; } }

        internal static int SpiritShellCount { get { return DiscSetting.SpiritShellCount; } }

        // we ignore all settings and start healing like hell!!!
        internal static int UrgentHealthPercentage { get { return DiscSetting.UrgentHealthPercentage; } }

        // Dynamic Prio (AoE)
        internal static bool PowerWordBarrierPrioEnabled { get { return DiscSetting.PowerWordBarrierPrioEnabled; } }

        internal static bool DivineStarPrioEnabled { get { return DiscSetting.DivineStarPrioEnabled; } }

        internal static bool CascadePrioEnabled { get { return DiscSetting.CascadePrioEnabled; } }

        internal static bool HaloPrioEnabled { get { return DiscSetting.HaloPrioEnabled; } }

        internal static bool PrayerOfHealingPrioEnabled { get { return DiscSetting.PrayerOfHealingPrioEnabled; } }

        // Dynamic Prio (Cooldowns)
        internal static bool SpiritShellPrioEnabled { get { return DiscSetting.SpiritShellPrioEnabled; } }

        internal static bool InnerFocusPrioEnabled { get { return DiscSetting.InnerFocusPrioEnabled; } }

        internal static bool ArchangelPrioEnabled { get { return DiscSetting.ArchangelPrioEnabled; } }

        internal static bool DiscPowerInfusionPrioEnabled { get { return DiscSetting.PowerInfusionPrioEnabled; } }



        #endregion Disc Settings

        #region Holy Settings

        private static HolyPriestSettings HolySetting { get { return OracleSettings.Instance.HolyPriest; } }

        public static bool HolyUsePowerWordFortitude { get { return HolySetting.UsePowerWordFortitude; } }

        public static bool HolyEnableLightwellUsage { get { return HolySetting.EnableLightwellUsage; } }

        public static int HolyMaxLightwellDistance { get { return HolySetting.MaxLightwellDistance; } }

        public static int HolyLightwellWaitTime { get { return HolySetting.LightwellWaitTime; } }

        public static int HolyFlashHealSoLPct { get { return HolySetting.FlashHealSoLPct; } }

        public static int HolyGuardianSpiritPct { get { return HolySetting.GuardianSpiritPct; } }

        public static int HolyPrayerOfMendingPct { get { return HolySetting.PrayerOfMendingPct; } }

        public static bool HolyUseFade { get { return HolySetting.UseFade; } }

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)
        public static int HolyDivineStarPct { get { return HolySetting.DivineStarPct; } }

        public static int HolyDivineStarCount { get { return HolySetting.DivineStarCount; } }

        public static int HolyCascadePct { get { return HolySetting.CascadePct; } }

        public static int HolyCascadeCount { get { return HolySetting.CascadeCount; } }

        public static int HolyHaloPct { get { return HolySetting.HaloPct; } }

        public static int HolyHaloCount { get { return HolySetting.HaloCount; } }

        public static int HolyPrayerofHealingPct { get { return HolySetting.PrayerofHealingPct; } }

        public static int HolyPrayerofHealingCount { get { return HolySetting.PrayerofHealingCount; } }

        public static int DivineHymnPct { get { return HolySetting.DivineHymnPct; } }

        public static int DivineHymnCount { get { return HolySetting.DivineHymnCount; } }

        public static int CircleOfHealingPct { get { return HolySetting.CircleOfHealingPct; } }

        public static int CircleOfHealingCount { get { return HolySetting.CircleOfHealingCount; } }

        public static int HolyWordSanctuaryPct { get { return HolySetting.HolyWordSanctuaryPct; } }

        public static int HolyWordSanctuaryCount { get { return HolySetting.HolyWordSanctuaryCount; } }

        //Oh Shit Moments!

        public static int HolyDesperatePrayerPct { get { return HolySetting.DesperatePrayerPct; } }

        public static int HolyPowerInfusionPct { get { return HolySetting.PowerInfusionPct; } }

        public static int HolyPowerInfusionCount { get { return HolySetting.PowerInfusionCount; } }

        public static int HolyVoidShiftPct { get { return HolySetting.VoidShiftPct; } }

        // we ignore all settings and start healing like hell!!!
        public static int HolyUrgentHealthPercentage { get { return HolySetting.UrgentHealthPercentage; } }


        // Dynamic Prio (AoE)

        internal static bool HolyDivineStarPrioEnabled { get { return HolySetting.HolyDivineStarPrioEnabled; } }

        internal static bool HolyCascadePrioEnabled { get { return HolySetting.HolyCascadePrioEnabled; } }

        internal static bool HolyHaloPrioEnabled { get { return HolySetting.HolyHaloPrioEnabled; } }

        internal static bool HolyWordSanctuaryPrioEnabled { get { return HolySetting.HolyWordSanctuaryPrioEnabled; } }

        internal static bool CircleOfHealingPrioEnabled { get { return HolySetting.CircleOfHealingPrioEnabled; } }

        internal static bool DivineHymnPrioEnabled { get { return HolySetting.DivineHymnPrioEnabled; } }

        internal static bool HolyPrayerOfHealingPrioEnabled { get { return HolySetting.HolyPrayerOfHealingPrioEnabled; } }

        // Dynamic Prio (Cooldowns)

        internal static bool HolyPowerInfusionPrioEnabled { get { return HolySetting.HolyPowerInfusionPrioEnabled; } }

        #endregion Holy Settings

        #region Booleons

        public static bool CanShield(WoWUnit u)
        {
            return OracleRoutine.IsViable(u) && (!u.ActiveAuras.ContainsKey("Weakened Soul") /*|| StyxWoW.Me.ActiveAuras.ContainsKey("Divine Insight")*/);
        }

        public static bool FadePvE()
        {
            return !PvPSupport && Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 30) > 0;
        }

        public static bool FadePvP()
        {
            return PvPSupport && PriestCommon.HasTalent(PriestTalents.Phantasm) && StyxWoW.Me.ActiveAuras.Any(aura => aura.Value.ApplyAuraType == WoWApplyAuraType.ModRoot || aura.Value.ApplyAuraType == WoWApplyAuraType.ModDecreaseSpeed);
        }

        #endregion Booleons

        #region Holy CreateClusteredHealBehavior

        public static void LoadClusterSpellsHoly()
        {
            OracleRoutine.Instance.ClusteredSpells.Clear();

            var key = 1;

            if (HolyWordSanctuaryPrioEnabled)
            {OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.HolyWordSanctuary, SpellType.GroundEffect, HolyWordSanctuaryCount, 0, HolyWordSanctuaryPct)); key++;}

            if (CircleOfHealingPrioEnabled)
            {OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.CircleOfHealing, SpellType.Proximity, CircleOfHealingCount, 0, CircleOfHealingPct)); key++;}

            if (HolyDivineStarPrioEnabled && PriestCommon.HasTalent(PriestTalents.DivineStar))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.DivineStar, SpellType.GroundEffect, HolyDivineStarCount, 0, HolyDivineStarPct)); key++; }

            if (HolyCascadePrioEnabled && PriestCommon.HasTalent(PriestTalents.Cascade))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.Cascade, SpellType.Proximity, HolyCascadeCount, 0, HolyCascadePct)); key++; }

            if (HolyHaloPrioEnabled && PriestCommon.HasTalent(PriestTalents.Halo))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.Halo, SpellType.NearbyLowestHealth, HolyHaloCount, 0, HolyHaloPct)); key++; }

            if (DivineHymnPrioEnabled)
            {OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.DivineHymn, SpellType.NearbyLowestHealth, DivineHymnCount, 0, DivineHymnPct)); key++;}

            if (HolyPrayerOfHealingPrioEnabled)
            {OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.PrayerOfHealing, SpellType.Party, 0, HolyPrayerofHealingCount, HolyPrayerofHealingPct));}
        }

        #endregion Holy CreateClusteredHealBehavior

        #region Holy CreateCooldownBehavior

        public static void LoadCooldownSpellsHoly()
        {
            OracleRoutine.Instance.CooldownSpells.Clear();

            var key = 1;

            // we use NearbyLowestHealth so that we capture the units around us that are in trouble.
            // ClusterSpell accepts a delegate to pass into the composite during hook creation.
            if (HolyPowerInfusionPrioEnabled)
            {OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.PowerInfusion, SpellType.NearbyLowestHealth, HolyPowerInfusionCount, 0, HolyPowerInfusionPct));}
        }

        #endregion Holy CreateCooldownBehavior

        #region Discipline CreateClusteredHealBehavior

        public static void LoadClusterSpells()
        {
            OracleRoutine.Instance.ClusteredSpells.Clear();

            var key = 1;

            if (PowerWordBarrierPrioEnabled)
            {OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.PowerWordBarrier, SpellType.GroundEffect, PowerWordBarrierCount, 0, PowerWordBarrierPct)); key++;}

            if (DivineStarPrioEnabled && HasTalent(PriestTalents.DivineStar))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.DivineStar, SpellType.GroundEffect, DivineStarCount, 0, DivineStarPct)); key++; }

            if (CascadePrioEnabled && HasTalent(PriestTalents.Cascade))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.Cascade, SpellType.Proximity, CascadeCount, 0, CascadePct)); key++; }

            if (HaloPrioEnabled && HasTalent(PriestTalents.Halo))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.Halo, SpellType.NearbyLowestHealth, HaloCount, 0, HaloPct)); key++; }

            if (PrayerOfHealingPrioEnabled)
            {OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.PrayerOfHealing, SpellType.Party, 0, PrayerofHealingCount, PrayerofHealingPct));}
        }

        #endregion Discipline CreateClusteredHealBehavior

        #region Discipline CreateCooldownBehavior

        public static void LoadCooldownSpells()
        {
            OracleRoutine.Instance.CooldownSpells.Clear();

            var key = 1;

            // we use NearbyLowestHealth so that we capture the units around us that are in trouble.
            // ClusterSpell accepts a delegate to pass into the composite during hook creation.

            
            if (SpiritShellPrioEnabled)
            {OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.SpiritShell, SpellType.NearbyLowestHealth, SpiritShellCount, 0, SpiritShellPct)); key++;}

            // Having Inner Focus here should line up with Prayer Of Healing!!!
            if (InnerFocusPrioEnabled)
            {OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.InnerFocus, SpellType.Party, 0, PrayerofHealingCount, PrayerofHealingPct, ret => !StyxWoW.Me.ActiveAuras.ContainsKey("Inner Focus") && !Spell.SpellOnCooldown("Inner Focus"))); key++;}

            if (ArchangelPrioEnabled)
            {OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.Archangel, SpellType.NearbyLowestHealth, ArchangelCount, 0, ArchangelPct)); key++;}

            if (DiscPowerInfusionPrioEnabled)
            {OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.PowerInfusion, SpellType.NearbyLowestHealth, PowerInfusionCount, 0, PowerInfusionPct));}
        }

        #endregion Discipline CreateCooldownBehavior

        #region Mana

        public static Composite CreateShadowfiendBehavior()
        {
            if (HasTalent(PriestTalents.Mindbender))
                return CooldownTracker.Cast("Mindbender", on => Unit.GetEnemy, ret => OracleRoutine.IsViable(Unit.GetEnemy) && Unit.FaceTarget(Unit.GetEnemy));

            return CooldownTracker.Cast("Shadowfiend", on => Unit.GetEnemy, ret => OracleRoutine.IsViable(Unit.GetEnemy) && Unit.FaceTarget(Unit.GetEnemy));
        }

        private static int ShadowfiendPct { get { return StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline ? DiscSetting.ShadowfiendPct : HolySetting.ShadowfiendPct; } }

        private static int HymnofHopePct { get { return StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline ? DiscSetting.HymnofHopePct : HolySetting.HymnofHopePct; } }

        private static bool NeedShadowFiend { get { return StyxWoW.Me.ManaPercent <= ShadowfiendPct; } }

        private static bool NeedHymnOfHope { get { return EnableHymeofHope && StyxWoW.Me.ManaPercent < HymnofHopePct && (CooldownTracker.SpellOnCooldown("Shadowfiend") || CooldownTracker.SpellOnCooldown("Mindbender")); } }

        public static Composite HandleManaCooldowns()
        {
            return new PrioritySelector(
                    new Decorator(ret => NeedShadowFiend, CreateShadowfiendBehavior()),
                    Spell.Cast("Hymn of Hope", on => StyxWoW.Me, ret => NeedHymnOfHope));
        }

        #endregion Mana

        #region Movement

        private static bool EnablePriestMovementBuff { get { return StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline ? DiscSetting.EnablePriestMovementBuff : HolySetting.EnablePriestMovementBuff; } }

        public static Decorator CreatePriestMovementBuff()
        {
            return new Decorator(ret => EnablePriestMovementBuff && StyxWoW.Me.IsMoving && !StyxWoW.Me.HasAnyAura("Body and Soul", "Angelic Feather"),
                            new PrioritySelector(
                                    Spell.Cast("Power Word: Shield", on => StyxWoW.Me, ret => HasTalent(PriestTalents.BodyAndSoul) && !StyxWoW.Me.HasAnyAura("Body and Soul", "Weakened Soul")),
                                    new Decorator(ret => (HasTalent(PriestTalents.AngelicFeather) && !StyxWoW.Me.HasAura("Angelic Feather")),
                                        new Sequence(
                                            Spell.CastOnGround("Angelic Feather", ctx => StyxWoW.Me.Location, ret => true, 0.5, true, false, true),
                                            new Action(ret => Lua.DoString("SpellStopTargeting()"))))
                                )
                            );
        }

        #endregion Movement
    }
}