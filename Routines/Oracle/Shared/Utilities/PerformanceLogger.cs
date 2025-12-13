#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/PerformanceLogger.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Diagnostics;
using Oracle.Shared.Logging;
using Oracle.UI.Settings;

namespace Oracle.Shared.Utilities
{
    // CREDIT TO unifiedtrinity
    [Flags]
    internal enum LogCategory
    {
        None = 0,
        Performance
    }

    [DebuggerStepThrough]
    public class PerformanceLogger : IDisposable
    {
        private readonly string _BlockName;
        private readonly Stopwatch _Stopwatch;
        private bool _IsDisposed;

        public PerformanceLogger(string blockName)
        {
            _BlockName = blockName;
            if (OracleSettings.Instance.PerformanceLogging.HasFlag(LogCategory.Performance))
            {
                _Stopwatch = new Stopwatch();
                _Stopwatch.Start();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;
                if (OracleSettings.Instance.PerformanceLogging.HasFlag(LogCategory.Performance))
                {
                    _Stopwatch.Stop();
                    if (_Stopwatch.Elapsed.TotalMilliseconds > 1)
                    {
                        Logger.Performance("[Performance] Execution of the block {0} took {1:00.00}ms.", _BlockName,
                                      _Stopwatch.Elapsed.TotalMilliseconds);
                    }
                }

                GC.SuppressFinalize(this);
            }
        }

        #endregion IDisposable Members

        ~PerformanceLogger()
        {
            Dispose();
        }
    }
}