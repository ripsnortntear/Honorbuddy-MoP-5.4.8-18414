using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using PureRotation.Classes;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using Styx;
using Styx.Common;
using Styx.TreeSharp;

namespace PureRotation
{
    [UsedImplicitly]
    partial class PureRotationRoutine
    {
        public RotationBase _currentRotation; // the current Rotation
        private List<RotationBase> _rotations; // list of Rotations

        #region Behaviours

        private Composite _combatBehavior, _combatBuffBehavior, _preCombatBuffBehavior;

        public override Composite CombatBehavior { get { return _combatBehavior; } }

        public override Composite PreCombatBuffBehavior { get { return _preCombatBuffBehavior; } }

        public override Composite CombatBuffBehavior { get { return _combatBuffBehavior; } }

        private Composite Medic { get { return _currentRotation.Medic; } }

        private Composite PreCombat { get { return _currentRotation.PreCombat; } }

        public bool RebuildBehaviors()
        {
            try
            {
                Logger.DebugLog("RebuildBehaviors called.");

                _currentRotation = null; // clear current rotation

                SetRotation(); // set the new rotation
                Logger.DebugLog("LfgDungeonId: {0}", StyxWoW.Me.GroupInfo.LfgDungeonId);
                if (_combatBehavior != null) _combatBehavior = new Decorator(ret => AllowPulse && (Me.CurrentTarget != null ), new PrioritySelector(CurrentWoWContext == WoWContext.Battleground ? _currentRotation.PVPRotation : _currentRotation.PVERotation));
                if (_combatBuffBehavior != null) _combatBuffBehavior = new Decorator(ret => AllowPulse, new PrioritySelector(Medic));
                if (_preCombatBuffBehavior != null) _preCombatBuffBehavior = new Decorator(ret => AllowPulse, new PrioritySelector(PreCombat));

                return true;
            }
            catch (Exception ex)
            {
                Logger.DebugLog("[RebuildBehaviors] Exception was thrown: {0}", ex);
                return false;
            }
        }

        #endregion Behaviours

        #region Set & Get the current rotation - Also builds the rotations list if it hasnt already done so

        /// <summary>Set the Current Rotation</summary>
        private void SetRotation()
        {
            try
            {
                if (_rotations != null && _rotations.Count > 0)
                {
                    Logger.InfoLog(" We have rotations so lets use the best one...");
                    foreach (var rotation in _rotations)
                    {
                        if (rotation != null && rotation.KeySpec == TalentManager.CurrentSpec)
                        {
                            Logger.InfoLog(" Using " + rotation.Name + " rotation based on Character Spec " + rotation.KeySpec);
                            _currentRotation = rotation;
                            PulseEvent = null; // Clear pulse event from any delegates from the previous rotation
                            PulseEvent += _currentRotation.OnPulse; // Subscribe to the pulse event
                        }
                        else
                        {
                            if (rotation != null)
                            {
                                // Logger.FailLog(" Skipping " + rotation.Name + " rotation. Character is not in " + rotation.KeySpec);
                            }
                        }
                    }
                }
                else
                {
                    Logger.InfoLog(" We have no rotations -  calling GetRotations");
                    GetRotations();
                }
            }
            catch (Exception ex)
            {
                Logger.FailLog(" Failed to Set Rotation " + ex);
            }
        }

        /// <summary>Get & Set the Current Rotation</summary>
        private void GetRotations()
        {
            try
            {
                _rotations = new List<RotationBase>();
                _rotations.AddRange(new TypeLoader<RotationBase>());
                if (_rotations.Count == 0)
                {
                    Logger.InfoLog("No rotations loaded to List, count = 0");
                }
                foreach (var rotation in _rotations)
                {
                    if (rotation != null && rotation.KeySpec == TalentManager.CurrentSpec)
                    {
                        Logger.InfoLog(" Using " + rotation.Name + " rotation based on Character Spec " + rotation.KeySpec);
                        _currentRotation = rotation;
                        PulseEvent += _currentRotation.OnPulse; // Subscribe to the Pulse event.
                    }
                    else
                    {
                        if (rotation != null)
                        {
                            // Logger.FailLog(" Skipping " + rotation.Name + " rotation. Character is not in " + rotation.KeySpec);
                        }
                    }
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
                            sb.AppendLine("PureRotation Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                Logger.FailLog(" Woops, we could not set the rotation.");
                Logger.FailLog(errorMessage);
                StopBot(" Unable to find Active Rotation: " + ex);
            }
        }

        #endregion Set & Get the current rotation - Also builds the rotations list if it hasnt already done so

        #region Misc helper shit

        private static bool IsMounted
        {
            get
            {
                return StyxWoW.Me.Mounted;
            }
        }

        private static bool AllowPulse { get { return !IsMounted; } }

        #endregion Misc helper shit

        #region Useful code that is no longer implemented

        #endregion Useful code that is no longer implemented

        #region Nested type: LockSelector

        /// <summary>
        /// This behavior wraps the child behaviors in a 'FrameLock' which can provide a big performance improvement
        /// if the child behaviors makes multiple api calls that internally run off a frame in WoW in one CC pulse.
        /// </summary>
        private class LockSelector : PrioritySelector
        {
            public LockSelector(params Composite[] children)
                : base(children)
            {
            }

            public override RunStatus Tick(object context)
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    return base.Tick(context);
                }
            }
        }

        #endregion Nested type: LockSelector
    }
}
