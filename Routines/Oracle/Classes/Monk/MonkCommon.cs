#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Classes/Monk/MonkCommon.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using CommonBehaviors.Actions;
using Oracle.Core;
using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.Spells.Auras;
using Oracle.Core.WoWObjects;
using Oracle.Healing;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.UI.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using Action = Styx.TreeSharp.Action;

namespace Oracle.Classes.Monk
{
    public enum MonkTalents
    {
        Celerity = 1,
        TigersLust,
        Momumentum,
        ChiWave,
        ZenSphere,
        ChiBurst,
        PowerStrikes,
        Ascension,
        ChiBrew,
        RingOfPeace,
        ChargingOxWave,
        LegSweep,
        HealingElixirs,
        DampenHarm,
        DiffuseMagic,
        RushingJadeWind,
        InvokeXuenTheWhiteTiger,
        ChiTorpedo
    }

    public static class MonkCommon
    {
        public static int MaxChi { get { return HasTalent(MonkTalents.Ascension) ? 5 : 4; } }

        private static WoWUnit Tank { get { return OracleTanks.PrimaryTank; } }

        private static WoWUnit HealTarget { get { return OracleHealTargeting.HealableUnit ?? StyxWoW.Me; } }

        #region Settings

        private static MonkSettings Setting { get { return OracleSettings.Instance.Monk; } }

        public static bool EnableUpliftOveride { get { return Setting.EnableUpliftOveride; } }

        public static int StopCastingManaTeapCt { get { return Setting.StopCastingManaTeapCt; } }

        public static int StopCastingHealthpCtforTank { get { return Setting.StopCastingHealthpCtforTank; } }

        public static bool EnableSoothingSpam { get { return Setting.EnableSoothingSpam; } }

        public static int ChiCountUpliftOveride { get { return Setting.ChiCountUpliftOveride; } }

        public static int ChiBrewCount { get { return Setting.ChiCountUpliftOveride; } }

        public static bool ThunderFocusTeaOnCooldown { get { return Setting.ThunderFocusTeaOnCooldown; } }

        public static bool EnableSerpentStatueUsage { get { return Setting.EnableSerpentStatueUsage; } }

        public static int MaxSerpentStatueDistance { get { return Setting.MaxSerpentStatueDistance; } }

        public static int SerpentStatueWaitTime { get { return Setting.SerpentStatueWaitTime; } }

        public static int SurgingVitalMistPercent { get { return Setting.SurgingVitalMistPercent; } }

        public static int ExpelHarmPct { get { return Setting.ExpelHarmPct; } }

        public static int ExpelHarmChiCount { get { return Setting.ExpelHarmChiCount; } }

        public static int DampenHarmPct { get { return Setting.DampenHarmPct; } }

        public static int LifeCocoonPct { get { return Setting.LifeCocoonPct; } }

        public static bool EnableLifeCocoon { get { return Setting.EnableLifeCocoon; } }

        public static int FortifyingBrewPct { get { return Setting.FortifyingBrewPct; } }

        public static int ManaTeaStackCount { get { return Setting.ManaTeaStackCount; } }

        public static int ManaTeaPct { get { return Setting.ManaTeaPct; } }

        public static bool UseThunderFocusTeaMaxChi { get { return Setting.UseThunderFocusTeaMaxChi; } }

        public static bool UseUpliftMaxChi { get { return Setting.UseUpliftMaxChi; } }

        public static int FistWeaveMana { get { return Setting.FistWeaveMana; } }

        public static bool UseNimbleBrew { get { return Setting.UseNimbleBrew; } }

        public static bool ChiBurstPrioEnabled { get { return Setting.ChiBurstPrioEnabled; } }

        public static bool RenewingMistPrioEnabled { get { return Setting.RenewingMistPrioEnabled; } }

        public static bool UpliftPrioEnabled { get { return Setting.UpliftPrioEnabled; } }

        public static bool RevivalPrioEnabled { get { return Setting.RevivalPrioEnabled; } }

        public static bool ThunderFocusTeaPrioEnabled { get { return Setting.ThunderFocusTeaPrioEnabled; } }

        public static bool InvokeXuentheWhiteTigerPrioEnabled { get { return Setting.InvokeXuentheWhiteTigerPrioEnabled; } }

        // AoE - Note: Limits are the amount of units (this will be evaluated as >=)

        internal static int RushingJadeWindPct { get { return Setting.RushingJadeWindPct; } }

        internal static int RushingJadeWindCount { get { return Setting.RushingJadeWindCount; } }

        internal static int ChiBurstPct { get { return Setting.ChiBurstPct; } }

        internal static int ChiBurstCount { get { return Setting.ChiBurstCount; } }

        public static int RenewingMistPct { get { return Setting.RenewingMistPct; } }

        public static int RenewingMistLimit { get { return Setting.RenewingMistLimit; } }

        public static int UpliftPct { get { return ((StyxWoW.Me.HasAura(RotationBase.ThunderFocusTea) || StyxWoW.Me.CurrentChi == MonkCommon.MaxChi) ? 100 : Setting.UpliftPct); } }

        public static int UpliftLimit { get { return ((StyxWoW.Me.HasAura(RotationBase.ThunderFocusTea) || StyxWoW.Me.CurrentChi == MonkCommon.MaxChi) ? 0 : Setting.UpliftLimit); } }

        public static int RevivalPct { get { return Setting.RevivalPct; } }

        public static int RevivalLimit { get { return ((OracleRoutine.GroupCount > 10) ? Setting.RevivalLimit25Man : Setting.RevivalLimit10Man); } }

        //Oh Shit Moments!

        public static int InvokeXuentheWhiteTigerPct { get { return Setting.InvokeXuentheWhiteTigerPct; } }

        public static int InvokeXuentheWhiteTigerLimit { get { return Setting.InvokeXuentheWhiteTigerLimit; } }

        public static int ThunderFocusTeaPct { get { return Setting.ThunderFocusTeaPct; } }

        public static int ThunderFocusTeaLimit { get { return Setting.ThunderFocusTeaLimit; } }

        // we ignore all settings and start healing like hell!!!
        public static int UrgentHealthPercentage { get { return Setting.UrgentHealthPercentage; } }

        #endregion Settings

        #region Booleans

        public static bool NeedManaTea { get { return StyxWoW.Me.ManaPercent < ManaTeaPct && !CooldownTracker.SpellOnCooldown("Mana Tea") && StyxWoW.Me.HasAura("Mana Tea", ManaTeaStackCount) && !NeedToStopCastingManaTea(); } }

        public static bool ChannelingSoothingMist { get { return (StyxWoW.Me.IsChanneling && StyxWoW.Me.CastingSpellId == RotationBase.SoothingMist); } }

        #endregion Booleans

        public static Composite StopCastingManaTea()
        {
            try
            {
                return new Decorator(a => (StyxWoW.Me.IsCasting && StyxWoW.Me.CastingSpellId == RotationBase.ManaTea && ManaTeaPct <= StopCastingManaTeapCt) && NeedToStopCastingManaTea(),
                                new Action(delegate
                                {
                                    Logger.Output(String.Format(" [DIAG] Stopped Casting {0} Mana @ {1:F1}%", WoWSpell.FromId(StyxWoW.Me.CastingSpellId).Name, StyxWoW.Me.ManaPercent));
                                    SpellManager.StopCasting();
                                    return RunStatus.Success;
                                }));
            }

            catch (AccessViolationException)
            {
                // empty
                return new ActionAlwaysFail();
            }
            catch (InvalidObjectPointerException)
            {
                // empty
                return new ActionAlwaysFail();
            }
        }

        private static bool NeedToStopCastingManaTea()
        {
            return ((OracleRoutine.IsViable(Tank) && Tank.HealthPercent < Math.Min(UrgentHealthPercentage, StopCastingHealthpCtforTank) || (OracleRoutine.IsViable(HealTarget) && HealTarget.HealthPercent(HealTarget.GetHPCheckType()) < UrgentHealthPercentage && StyxWoW.Me.ManaPercent > 30)) || StyxWoW.Me.ManaPercent > StopCastingManaTeapCt);
        }

        public static bool HasTalent(MonkTalents tal)
        {
            return TalentManager.IsSelected((int)tal);
        }

        #region Monk CreateClusteredHealBehavior

        public static void LoadClusterSpells()
        {
            OracleRoutine.Instance.ClusteredSpells.Clear();

            var key = 1;

            if (HasTalent(MonkTalents.ChiBurst) && ChiBurstPrioEnabled)
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.ChiBurst, SpellType.GroundEffect, ChiBurstCount, 0, ChiBurstPct)); key++; }

            if (HasTalent(MonkTalents.RushingJadeWind))
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.RushingJadeWind, SpellType.NearbyLowestHealth, RushingJadeWindCount, 0, RushingJadeWindPct)); key++; }

            // TODO: These two need Aura checks on the cluster centroid.
            if (RenewingMistPrioEnabled)
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.RenewingMist, SpellType.NearbyLowestHealth, RenewingMistLimit, 0, RenewingMistPct)); key++; }

            if (UpliftPrioEnabled)
            { OracleRoutine.Instance.ClusteredSpells.Add(key, new ClusterSpell(RotationBase.Uplift, SpellType.NearbyLowestHealth, UpliftLimit, 0, UpliftPct)); }
        }

        #endregion Monk CreateClusteredHealBehavior

        #region Monk CreateCooldownBehavior

        public static void LoadCooldownSpells()
        {
            OracleRoutine.Instance.CooldownSpells.Clear();

            var key = 1;

            // we use NearbyLowestHealth so that we capture the units around us that are in trouble.
            // ClusterSpell accepts a delegate to pass into the composite during hook creation.

            if (RevivalPrioEnabled)
            { OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.Revival, SpellType.NearbyLowestHealth, RevivalLimit, 0, RevivalPct)); key++; }

            if (ThunderFocusTeaPrioEnabled)
            { OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.ThunderFocusTea, SpellType.NearbyLowestHealth, ThunderFocusTeaLimit, 0, ThunderFocusTeaPct)); key++; }

            if (InvokeXuentheWhiteTigerPrioEnabled)
            { OracleRoutine.Instance.CooldownSpells.Add(key, new ClusterSpell(RotationBase.InvokeXuentheWhiteTiger, SpellType.NearbyLowestHealth, InvokeXuentheWhiteTigerLimit, 0, InvokeXuentheWhiteTigerPct)); }
        }

        #endregion Monk CreateCooldownBehavior
    }
}