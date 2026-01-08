using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Plugins;
using Styx.WoWInternals.WoWObjects;
using Styx;
using Styx.WoWInternals;
using Styx.Common;
using Styx.CommonBot;
using System.Windows.Media;

namespace NoBotHere
{
    class NoBotHere : HBPlugin
    {
        public override string Name { get { return "NoBotHere "+Version; } }
        public override string Author { get { return "randomstraw"; } }
        public override Version Version { get { return new Version(1, 0, 3); } }
        private bool _initialized = false;

        private static Color _color = Colors.Orange;
        private static int _targetDistance = 30;                                    //max-range to look for targets - hardcoded max = 50 (!) even if you set the value higher, it will not care.
        private static TimeSpan _targetBlacklistFor = TimeSpan.FromSeconds(70);     //how long shall we blacklist someone we recently targetted?
        private static int _targetTimeMin = 4000;                                   //keep target for min 4000ms
        private static int _targetTimeMax = 18000;                                  //keep target for max 18000ms
        private static DateTime _targetTime;                                        //holds the time we actually keep our target, don't touch
        private static TimeSpan _lastRnd;                                           //magic / me lazy
        private static TimeSpan _scanFrequency = TimeSpan.FromMilliseconds(5000);   //frequency for finding suitable targets, 5000ms standard - dont set too low, we dont need to scan everything around us every 2ms/tick...
        private static DateTime _lastScan;
        private static DateTime _nextTick;
        private static TimeSpan _tick = TimeSpan.FromMilliseconds(1000);
        private static int _totalTargets;
        
        public override void Initialize()
        {
            if (_initialized)
                return;

            Logging.Write(_color, "[NBH]: [{0} initialized]", Name);
            _initialized = true;
            _totalTargets = 0;
        }
        public override void Dispose()
        {
            if (!_initialized)
                return;

            Logging.Write(_color, "[NBH]: [Targetted a total of {0} Players!]", _totalTargets);
            Logging.Write(_color, "[NBH]: [{0} disposed]", Name);
            _totalTargets = 0;
            _initialized = false;
        }

        public override void Pulse()
        {
            if (_initialized && DateTime.UtcNow > _nextTick)
            {
                if (StyxWoW.Me.Combat)
                    return;
                
                if (StyxWoW.Me.CurrentTarget != null && !StyxWoW.Me.CurrentTarget.IsPlayer)
                    return;

                if (StyxWoW.Me.IsAlive && !StyxWoW.Me.OnTaxi && !StyxWoW.Me.Combat)
                {
                    if (StyxWoW.Me.CurrentTarget == null && DateTime.UtcNow > _lastScan)
                    {
                        _lastScan = DateTime.UtcNow.Add(_scanFrequency);
                        AquireTarget(_targetDistance);
                    }

                    if (StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.IsPlayer && DateTime.UtcNow > _targetTime)
                    {
                        ClearTarget();
                    }

                    _nextTick = DateTime.UtcNow.Add(_tick);
                }
            }
        }

        private void ClearTarget()
        {
            Blacklist.Add(StyxWoW.Me.CurrentTarget, _targetBlacklistFor);
            StyxWoW.Me.ClearTarget();
        }
        private void AquireTarget(int d)
        {
            WoWUnit unit = TargetableUnitsDistance(d).FirstOrDefault(u => u != null && u.IsPlayer && u.InLineOfSpellSight);
            
            if (unit != null)
            {
                unit.Target();
                _lastRnd = TimeSpan.FromMilliseconds(new Random().Next(_targetTimeMin, _targetTimeMax));
                _targetTime = DateTime.UtcNow.Add(_lastRnd);
                Logging.Write(_color, "[NBH]: [Targetting: {0}.{1} @ {2}yd for {3}s]", unit.Class, unit.Guid.ToString().Substring(8,4), Math.Round(unit.Distance, 1), _lastRnd.Seconds);
                _totalTargets = _totalTargets + 1;
            }

            if (unit == null)
            {
                Logging.WriteDiagnostic(_color, "[NBH]: no suitable targets");
            }
        }
        
        internal static IEnumerable<WoWUnit> TargetableUnits
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(u => u.IsPlayer && u.CanSelect && u.Distance < 50 && !Blacklist.Contains(u)); }
        }

        internal static IEnumerable<WoWUnit> TargetableUnitsDistance(int d)
        {
            var _t = TargetableUnits;
            return _t.Where(x => x.Distance < d);
        }
    }
}
