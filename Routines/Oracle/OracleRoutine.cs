#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/OracleRoutine.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Classes.Druid;
using Oracle.Classes.Monk;
using Oracle.Classes.Paladin;
using Oracle.Classes.Shaman;
using Oracle.Core;
using Oracle.Core.DataStores;
using Oracle.Core.Groups;
using Oracle.Core.Managers;
using Oracle.Core.Spells;
using Oracle.Core.WoWObjects;
using Oracle.Healing;
using Oracle.Healing.Chronicle;
using Oracle.Healing.Chronicle.Classes;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities;
using Oracle.Shared.Utilities.Clusters;
using Oracle.Shared.Utilities.Clusters.Data;
using Oracle.UI;
using Oracle.UI.Settings;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Windows.Forms;

namespace Oracle
{
    public partial class OracleRoutine : CombatRoutine
    {
        public sealed override string Name { get { return "Oracle [" + GetOracleVersion() + "]"; } }

        #region Hidden Overrides

        public sealed override void Combat() { base.Combat(); }
        public sealed override void CombatBuff() { base.CombatBuff(); }
        public sealed override void Death() { base.Death(); }
        public sealed override void Heal() { base.Heal(); }
        public sealed override Composite MoveToTargetBehavior { get { return base.MoveToTargetBehavior; } }
        public sealed override bool NeedCombatBuffs { get { return base.NeedCombatBuffs; } }
        public sealed override bool NeedDeath { get { return base.NeedDeath; } }
        public sealed override bool NeedHeal { get { return base.NeedHeal; } }
        public sealed override bool NeedPreCombatBuffs { get { return base.NeedPreCombatBuffs; } }
        public sealed override bool NeedPullBuffs { get { return base.NeedPullBuffs; } }
        public sealed override bool NeedRest { get { return base.NeedRest; } }
        public sealed override void PreCombatBuff() { base.PreCombatBuff(); }
        public sealed override void Pull() { base.Pull(); }
        public sealed override void Rest() { base.Rest(); }

        #endregion Hidden Overrides

        #region Misc

        public override WoWClass Class { get { return StyxWoW.Me.Class; } }

        private static WoWSpec Specialization { get { return StyxWoW.Me.Specialization; } }

        internal static int GroupCount { get; private set; }

        #endregion Misc

        public static OracleRoutine Instance { get; private set; }

        public OracleRoutine()
        { Instance = this; }

        public override void Initialize()
        {
            try
            {
                GroupCount = 0;
                Logger.StatCounter();
                Logger.Output(" The {0} is watching...", RoutineManager.Current.Name);
                Logging.WriteDiagnostic(" [Oracle D] {0:F1} days since Windows was started.", TimeSpan.FromMilliseconds(Environment.TickCount).TotalHours / 24.0);
                Logging.WriteDiagnostic("[Oracle D] {0} ms Latency in WoW.", StyxWoW.WoWClient.Latency);
                Logger.Output(" we are in {0} ", StyxWoW.Me.RealZoneText);

                OracleSettings.Instance.EnableProvingGrounds = (StyxWoW.Me.RealZoneText == "Proving Grounds");
                OracleSettings.Instance.MAX_AOE_HP = Specialization == WoWSpec.DruidRestoration ? 99 : OracleSettings.Instance.MAX_AOE_HP;

                TreeHooks.Instance.ClearAll(); // Performance..

                TalentManager.Update();

                BaseStats.Instance.Initialize();

                RoutineManager.Reloaded += (s, e) =>
                    {
                        // Don't run this handler if we're not the current routine!
                        if (RoutineManager.Current.Name != Name)
                            return;

                        Logger.Output(" Routines were reloaded, re-creating behaviors");
                        BuildBehaviors();
                    };

                // Build Behaviors
                if (!BuildBehaviors()) { StopBot(" Could not build behaviors"); }

                // Racial support...
                Logger.Output(" Setting Race to {0} for Racial Spells", StyxWoW.Me.Race);
                Racials.CurrentRace = StyxWoW.Me.Race;

                // GCD Hack....
                if (OracleGCD.GetGlobalCooldownSpell != null)
                {
                    OracleGCD.GcdSpell = OracleGCD.GetGlobalCooldownSpell;
                    Logger.Output(" GcdSpell set to: {0}", OracleGCD.GcdSpell);
                }

                // Healing Selector...
                HealingSelector.Initialize();

                // Spell Calculations...
                Chronicle.Initialize();

                //LoS Checks.
                OracleLineOfSight.Initialize();


                OnCombatStateChange += (orig, ne) =>
                    {
                        // Don't run this handler if we're not the current routine!
                        if (RoutineManager.Current.Name != Name)
                            return;

                        ResetLists();
                        Chronicle.Pulse();
                        BossList.Update();
                        DispelManager._cachedCapabilities = DispelManager.Capabilities;
                        GroupCount = StyxWoW.Me.GroupInfo.NumRaidMembers;
                        Item.CooldownFinishsWhenWeLeaveCombat = false;
                        OracleSettings.Instance.EnableProvingGrounds = (StyxWoW.Me.RealZoneText == "Proving Grounds");
                        OracleSettings.Instance.MAX_AOE_HP = Specialization == WoWSpec.DruidRestoration ? 99 : OracleSettings.Instance.MAX_AOE_HP;
                    };

                ClusterManager.Pulse();
                DispelManager.Initialize();

                DruidCommon.Shroom = null;

                Logger.Output(" Initialize Complete.");

                OracleSettings.Instance.LogSettings();
            }
            catch (Exception e)
            {
                StopBot(e.ToString());
            }
        }

        public override void ShutDown()
        {
            Chronicle.Shutdown();
            OracleLineOfSight.Shutdown();
           
        }

        public override void Pulse()
        {
            var time = DateTime.UtcNow;

            // clean out teh cached value.
            OracleGCD.PulseGCDCache();

            if (!StyxWoW.IsInWorld || StyxWoW.Me == null || !StyxWoW.Me.IsValid || !StyxWoW.Me.IsAlive || StyxWoW.Me.IsChanneling || StyxWoW.Me.IsCasting || Spell.GlobalCooldown())
            {
                return;
            }

            // Update the current Combat State, and fire an event for the change.
            HandleCombatStateChange();

            using (new PerformanceLogger("PulseUnits"))
            {
                Unit.PulseFriendly();
                Unit.PulseNPC();
            }

            // intense if does work, so return if true
            if (TalentManager.Pulse())
                return;

            //Only pulse for Shaman's atm - Earth Elemental
            if (StyxWoW.Me.Class == WoWClass.Shaman && OracleSettings.Instance.Shaman.UseReinforcewithEarthElemental &&
                ShamanCommon.HasTalent(ShamanTalents.PrimalElementalist))
            {
                PetManager.Pulse();
            }

            //Spell.OutputDoubleCastEntries();
            if (OracleSettings.Instance.EnableEFLogging) Spell.OutputBlanketSpellEntries();

            Spell.PulseDoubleCastEntries();
            OracleExtensions.PulseRoleCacheEntries();
            Spell.PulseBlanketSpellEntries();
            OracleLineOfSight.PulseOracleBlackListEntries();
            Group.PulseOraclePartyMember();

            // Lets pulse enemys only when we need too...
            switch (Specialization)
            {
                case WoWSpec.PaladinHoly:
                    if (PaladinCommon.HasTalent(PaladinTalents.SelflessHealer)) Unit.PulseEnemy();
                    break;

                case WoWSpec.PriestDiscipline:
                case WoWSpec.PriestHoly:
                    Unit.PulseEnemy();
                    break;

                case WoWSpec.ShamanRestoration:
                    if (ShamanCommon.HasTalent(ShamanTalents.ElementalBlast)) Unit.PulseEnemy();
                    break;

                case WoWSpec.MonkMistweaver:
                    if (MonkCommon.HasTalent(MonkTalents.InvokeXuenTheWhiteTiger)) Unit.PulseEnemy();
                    break;
            }

            if ((DateTime.UtcNow - time).TotalMilliseconds > 1000)
                Logger.Warning("Warning Pulse Took: " + (DateTime.UtcNow - time).TotalMilliseconds + " ms.");

            Logger.Performance("Pulse Overide Took: " + (DateTime.UtcNow - time).TotalMilliseconds + " ms.");
        }

        // Object checks.
        public static bool IsViable(WoWObject wowObject)
        {
            return (wowObject != null) && wowObject.IsValid;
        }

        #region GUI

        public override bool WantButton { get { return true; } }

        private Form _newtempui;

        public override void OnButtonPress()
        {
            if (_newtempui == null || _newtempui.IsDisposed || _newtempui.Disposing) _newtempui = new Config(); //ChronicleUI // ChronicleViewPort
            if (_newtempui != null || _newtempui.IsDisposed) _newtempui.ShowDialog();
        }

        #endregion GUI

        #region Helpers

        public static void ResetLists()
        {
            GeneratedData.GroupNumbers.Clear();
            OracleTanks.GetMainTanks();
        }

        private static void StopBot(string reason)
        {
            Logger.Output(reason); TreeRoot.Stop(reason);
        }

        #endregion Helpers
    }
}