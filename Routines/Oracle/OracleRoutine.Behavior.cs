#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/OracleRoutine.Behavior.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using JetBrains.Annotations;
using Oracle.Classes;
using Oracle.Classes.Druid;
using Oracle.Classes.Monk;
using Oracle.Classes.Paladin;
using Oracle.Classes.Priest;
using Oracle.Classes.Shaman;
using Oracle.Core.Hooks;
using Oracle.Core.Spells;
using Oracle.Healing;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities;
using Oracle.Shared.Utilities.Clusters;
using Oracle.UI.Settings;
using Styx;
using Styx.Common;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Action = Styx.TreeSharp.Action;

namespace Oracle
{
    [UsedImplicitly]
    partial class OracleRoutine
    {
        private RotationBase _currentRotation; // the current Rotation
        private List<RotationBase> _rotations; // list of Rotations

        private static WoWUnit HealTarget { get { return OracleHealTargeting.HealableUnit ?? StyxWoW.Me; } }

        public readonly Dictionary<int, ClusterSpell> CooldownSpells = new Dictionary<int, ClusterSpell>();
        public readonly Dictionary<int, ClusterSpell> ClusteredSpells = new Dictionary<int, ClusterSpell>();

        #region Behaviours

        private Composite _combat, _combatBuff, _preCombatBuff;

        public sealed override Composite CombatBehavior { get { return _combat; } }

        public sealed override Composite CombatBuffBehavior { get { return _combatBuff; } }

        //public sealed override Composite HealBehavior { get { return base.HealBehavior; } }

        public sealed override Composite PreCombatBuffBehavior { get { return _preCombatBuff; } }

        public bool BuildBehaviors()
        {
            //try
            // {
            Logger.Output(" BuildBehaviors called.");

            _currentRotation = null; // clear current rotation

            SetRotation(); // set the new rotation

            if (_currentRotation == null)
            {
                Logger.Output(" Sorry, there is no rotation available for this spec.");
                return false;
            }

            LoadSpells();

            foreach (var v in OracleRoutine.Instance.ClusteredSpells)
            {
                Logger.Output("ClusteredSpells: {0}", v.Value.SpellName);
            }

            if (_combat == null) _combat = new CallTrace(_currentRotation.Name,
                                                new Sequence(
                                                               new Action(delegate
                                                               {
                                                                    if (Spell.GlobalCooldown()) return RunStatus.Success;

                                                                   // OracleHealTargeting_Pulse
                                                                   using (new PerformanceLogger("OracleHealTargeting_Pulse"))
                                                                   {
                                                                       OracleHealTargeting.Instance.Pulse();
                                                                   }

                                                                   // SingleTargetHealBehaviorHook
                                                                   using (new PerformanceLogger("SingleTargetHealBehaviorHook"))
                                                                   {
                                                                       TreeHooks.Instance.ReplaceHook("SingleTargetHealBehaviorHook", new PrioritySelector());
                                                                       OracleHooks.PopulateSTHook(HealTarget, "SingleTargetHealBehaviorHook");
                                                                   }

                                                                   return RunStatus.Success;
                                                               }),
                                                               new Action(delegate
                                                               {
                                                                   if (Spell.GlobalCooldown()) return RunStatus.Success;

                                                                   // ClusterManager_Pulse
                                                                   using (new PerformanceLogger("ClusterManager_Pulse"))
                                                                   {
                                                                       ClusterManager.Pulse();
                                                                   }

                                                                   // ClusteredHealBehavior
                                                                   using (new PerformanceLogger("ClusteredHealBehavior"))
                                                                   {
                                                                       TreeHooks.Instance.ReplaceHook("ClusteredHealBehavior", new PrioritySelector());
                                                                       OracleHooks.PopulateClusterHook(ClusteredSpells, "ClusteredHealBehavior");
                                                                   }

                                                                   // CooldownsBehaviorHook
                                                                   using (new PerformanceLogger("CooldownsBehaviorHook"))
                                                                   {
                                                                       TreeHooks.Instance.ReplaceHook("CooldownsBehaviorHook", new PrioritySelector());
                                                                       OracleHooks.PopulateClusterHook(CooldownSpells, "CooldownsBehaviorHook");
                                                                   }

                                                                   return RunStatus.Success;
                                                               }),
                                                             _currentRotation.PVERotation
                                                    ));
            if (_combatBuff == null) _combatBuff = _currentRotation.Medic;
            if (_preCombatBuff == null) _preCombatBuff = _currentRotation.PreCombat;

            return true;
            // }
            //catch (Exception ex) { Logger.Output(" Could not build behaviours \n{0}", ex.Message); return false; }
        }

        public void LoadSpells()
        {
            try
            {
                switch (StyxWoW.Me.Specialization)
                {
                    case WoWSpec.MonkMistweaver:
                        MonkCommon.LoadClusterSpells();
                        Logger.Output(" Loaded AoE Spells.");
                        MonkCommon.LoadCooldownSpells();
                        Logger.Output(" Loaded Cooldown Spells.");
                        break;

                    case WoWSpec.DruidRestoration:
                        DruidCommon.LoadClusterSpells();
                        Logger.Output(" Loaded AoE Spells.");
                        DruidCommon.LoadCooldownSpells();
                        Logger.Output(" Loaded Cooldown Spells.");
                        break;

                    case WoWSpec.PriestHoly:
                        PriestCommon.LoadClusterSpellsHoly();
                        Logger.Output(" Loaded AoE Spells.");
                        PriestCommon.LoadCooldownSpellsHoly();
                        Logger.Output(" Loaded Cooldown Spells.");
                        break;

                    case WoWSpec.PriestDiscipline:
                        PriestCommon.LoadClusterSpells();
                        Logger.Output(" Loaded AoE Spells.");
                        PriestCommon.LoadCooldownSpells();
                        Logger.Output(" Loaded Cooldown Spells.");
                        break;

                    case WoWSpec.PaladinHoly:
                        PaladinCommon.LoadClusterSpells();
                        Logger.Output(" Loaded AoE Spells.");
                        PaladinCommon.LoadCooldownSpells();
                        Logger.Output(" Loaded Cooldown Spells.");
                        break;

                    case WoWSpec.ShamanRestoration:
                        ShamanCommon.LoadClusterSpells();
                        Logger.Output(" Loaded AoE Spells.");
                        ShamanCommon.LoadCooldownSpells();
                        Logger.Output(" Loaded Cooldown Spells.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Output(" Error in LoadSpells() : {0}", ex.StackTrace);
            }
        }

        #endregion Behaviours

        #region Set & Get the current rotation - Also builds the rotations list if it hasnt already done so

        /// <summary>Get/Set the Current Rotation</summary>
        private void GetRotations()
        {
            try
            {
                _rotations = new List<RotationBase>();
                _rotations.AddRange(new TypeLoader<RotationBase>());

                if (_rotations.Count == 0)
                {
                    Logger.Output(" No rotations loaded to List");
                }
                foreach (var rotation in _rotations.Where(rotation => rotation != null && rotation.KeySpec == Specialization))
                {
                    Logger.Output(" Using " + rotation.Name + " rotation based on Character Spec " + rotation.KeySpec);
                    _currentRotation = rotation;
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                foreach (var exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Oracle Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                var errorMessage = sb.ToString();
                Logger.Output(" Woops, we could not set the rotation.");
                Logger.Output(errorMessage);
                StopBot(" Unable to find Active Rotation " + ex);
            }
        }

        /// <summary>Set the Current Rotation</summary>
        private void SetRotation()
        {
            // try
            // {
            if (_rotations != null && _rotations.Count > 0)
            {
                Logger.Output(" We have rotations so lets use the best one...");
                foreach (var rotation in _rotations.Where(rotation => rotation != null && rotation.KeySpec == Specialization))
                {
                    Logger.Output(" Using " + rotation.Name + " rotation based on Character Spec " + rotation.KeySpec);
                    _currentRotation = rotation;
                }
            }
            else
            {
                Logger.Output(" We have no rotations -  calling GetRotations");
                GetRotations();
            }
            //}
            // catch (Exception ex) { Logger.Output(" Failed to Set Rotation \n {0}", ex.Message); }
        }

        #endregion Set & Get the current rotation - Also builds the rotations list if it hasnt already done so

        # region Trace - Ripped with pride from Singular - Cheers guys.

        public class CallTrace : PrioritySelector
        {
            public static DateTime LastCall { get; set; }

            public static ulong CountCall { get; set; }

            public static bool TraceActive { get { return OracleSettings.Instance.Trace; } }

            public string Name { get; set; }

            private static bool _init = false;

            private static void Initialize()
            {
                if (_init)
                    return;

                _init = true;
            }

            public CallTrace(string name, params Composite[] children)
                : base(children)
            {
                Initialize();

                Name = name;
                LastCall = DateTime.MinValue;
            }

            public override RunStatus Tick(object context)
            {
                RunStatus ret;
                CountCall++;

                if (!TraceActive)
                {
                    ret = base.Tick(context);
                }
                else
                {
                    DateTime started = DateTime.Now;
                    Logger.Output("... enter: {0}", Name);
                    ret = base.Tick(context);
                    Logger.Output("... leave: {0}, took {1} ms", Name, (ulong)(DateTime.Now - started).TotalMilliseconds);
                }

                return ret;
            }
        }

        #endregion
    }
}