using Styx.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieHolyPriestPvP
{
    public sealed class PerfLogger
    {
        private const double NanoToMilliSec = 0.000001;

        #region Helper Class to Implement IDIsposable

        public sealed class PerfHelper : IDisposable
        {
            private PerfLogger Instance { get; set; }
            private bool disposed;

            internal PerfHelper(PerfLogger perf)
            {
                Instance = perf;
            }

            public void Start()
            {
                Instance.Start();
            }

            public void Stop()
            {
                Instance.Stop();
            }

            public void Dispose()
            {
                if (disposed)
                    return;
                disposed = true;

                Stop();
            }
        }

        #endregion

        #region Static Interface

        private static readonly long NanoSecsPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
        private static Dictionary<string, PerfLogger> Results = new Dictionary<string, PerfLogger>(150);
        private static DateTime LastPrint = DateTime.MinValue;

        public static PerfLogger Get(string name)
        {
            PerfLogger logger;
            if (!Results.TryGetValue(name, out logger))
                Results.Add(name, logger = new PerfLogger(name));

            return logger;
        }

        public static PerfHelper GetHelper(string name)
        {
            PerfHelper helper = new PerfHelper(Get(name));
            helper.Start();
            return helper;
        }

        public static void PrintAll()
        {
            if (LastPrint.AddSeconds(2) > DateTime.Now)
                return;
            LastPrint = DateTime.Now;

            var validResults = from perf in Results where perf.Value.Count > 0 orderby perf.Value.Avg descending select perf;

            if (validResults.Count() == 0)
                return;

            Logging.Write("\n-------------Performance Loggers--------------");

            foreach (var kvp in validResults)
                Logging.Write(kvp.Value.ToString());

            Logging.Write("---------End Of Performance Loggers-----------\n");
        }

        public static void ResetAll()
        {
            foreach (var kvp in Results)
                kvp.Value.Reset();
        }

        #endregion

        public string Name { get; private set; }
        public long Count { get; private set; }

        #region Statistics

        #region In Nanosec

        public long Min { get; private set; }
        public long Max { get; private set; }
        public ulong Sum { get; private set; }
        public long Avg { get { return Count == 0 ? 0 : (long)(Sum / (ulong)Count); } }
        public long Last { get; private set; }

        #endregion

        #region In Millisec

        public double MinMs { get { return Min * NanoToMilliSec; } }
        public double MaxMs { get { return Max * NanoToMilliSec; } }
        public double SumMs { get { return Sum * NanoToMilliSec; } }
        public double AvgMs { get { return Avg * NanoToMilliSec; } }
        public double LastMs { get { return Last * NanoToMilliSec; } }

        #endregion

        #endregion

        private Stopwatch Watch { get; set; }

        private PerfLogger(string name)
        {
            this.Name = name;
            this.Watch = new Stopwatch();
            this.Min = long.MaxValue;
            this.Max = long.MinValue;
        }

        public PerfLogger Start()
        {
            if (Watch != null && Watch.IsRunning)
                throw new ArgumentException("Stopwatch already running! " + Name);

            if (Watch == null)
                Watch = new Stopwatch();

            Watch.Restart();

            return this;
        }

        public void Stop()
        {
            if (Watch == null)
                throw new NullReferenceException("No Stopwatch created. Call Start() first! " + Name);

            if (!Watch.IsRunning)
                throw new NullReferenceException("Stopwatch not running. Call Start() first! " + Name);

            Watch.Stop();

            Count++;
            Last = Watch.ElapsedTicks * NanoSecsPerTick;
            Sum += (ulong)Last;

            if (Last < Min)
                Min = Last;
            if (Last > Max)
                Max = Last;
        }

        public void Reset()
        {
            if (Watch != null && Watch.IsRunning)
                throw new ArgumentException("Stopwatch already running! " + Name);

            Count = 0;
            Min = long.MaxValue;
            Max = long.MinValue;
            Sum = 0;
            Last = 0;
        }

        public override string ToString()
        {
            return string.Format("['{0}' - Last: {1:F4} Min: {2:F4} Max: {3:F4} Avg: {4:F4} Sum: {5:F4} Count: {6}]", Name, Last * NanoToMilliSec, Min * NanoToMilliSec, Max * NanoToMilliSec, Avg * NanoToMilliSec, Sum * NanoToMilliSec, Count);
        }
    }
}