using System;
using System.Diagnostics;
using FuryUnleashed.Interfaces.Settings;
using Enum = FuryUnleashed.Core.Helpers.Enum;

namespace FuryUnleashed.Core.Utilities
{
    // CREDIT TO unifiedtrinity && Wulf
    [DebuggerStepThrough]
    public class PerformanceLogger : IDisposable
    {
        private readonly string _blockName;
        private readonly Stopwatch _stopwatch;
        private bool _isDisposed;

        public PerformanceLogger(string blockName)
        {
            _blockName = blockName;
            if (InternalSettings.Instance.General.PerformanceLogging.HasFlag(Enum.LogCategory.Performance))
            {
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (InternalSettings.Instance.General.PerformanceLogging.HasFlag(Enum.LogCategory.Performance))
                {
                    _stopwatch.Stop();
                    if (_stopwatch.Elapsed.TotalMilliseconds > 1)
                    {
                        Logger.CombatLogWh("[Performance] Execution of the block {0} took {1:00.00}ms.", _blockName,
                                      _stopwatch.Elapsed.TotalMilliseconds);
                    }
                }

                GC.SuppressFinalize(this);
            }
        }

        ~PerformanceLogger()
        {
            Dispose();
        }
        #endregion IDisposable Members
    }
}