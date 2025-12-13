#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Shared/Utilities/Performance/PerformanceTester.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Diagnostics;

namespace Oracle.Shared.Utilities.Performance
{
    public class PerformanceTester
    {
        /*
         // usage
         var tester = new PerformanceTester(() => SomeMethod());
         tester.MeasureExecTimeWithMetrics(1000);
         Logger.Output(string.Format("Executed in {0} milliseconds", tester.AverageTime.TotalMilliseconds));

         */

        public PerformanceTester(Action action)
        {
            Action = action;
            MaxTime = TimeSpan.MinValue;
            MinTime = TimeSpan.MaxValue;
        }

        public Action Action { get; set; }

        public TimeSpan AverageTime { get; private set; }

        public TimeSpan MaxTime { get; private set; }

        public TimeSpan MinTime { get; private set; }

        public TimeSpan TotalTime { get; private set; }

        public void MeasureExecTime()
        {
            var sw = Stopwatch.StartNew();
            Action();
            sw.Stop();
            AverageTime = sw.Elapsed;
            TotalTime = sw.Elapsed;
        }

        public void MeasureExecTime(int iterations)
        {
            Action(); // warm up
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                Action();
            }
            sw.Stop();
            AverageTime = new TimeSpan(sw.Elapsed.Ticks / iterations);
            TotalTime = sw.Elapsed;
        }

        public void MeasureExecTimeWithMetrics(int iterations)
        {
            var total = new TimeSpan(0);

            Action(); // warm up
            for (int i = 0; i < iterations; i++)
            {
                var sw = Stopwatch.StartNew();

                Action();

                sw.Stop();
                var thisIteration = sw.Elapsed;
                total += thisIteration;

                if (thisIteration > MaxTime) MaxTime = thisIteration;
                if (thisIteration < MinTime) MinTime = thisIteration;
            }

            TotalTime = total;
            AverageTime = new TimeSpan(total.Ticks / iterations);
        }
    }
}