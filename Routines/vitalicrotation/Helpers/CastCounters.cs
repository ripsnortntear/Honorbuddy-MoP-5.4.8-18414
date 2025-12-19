using System.Threading;

namespace VitalicRotation.Helpers
{
    // Minimal counters for diagnostics overlay. Thread-safe increments.
    public static class CastCounters
    {
        private static int _gcdDenied;
        private static int _losDenied;
        private static int _rangeDenied;

        public static int GcdDenied { get { return _gcdDenied; } }
        public static int LineOfSightDenied { get { return _losDenied; } }
        public static int RangeDenied { get { return _rangeDenied; } }

        public static void IncGcd() { Interlocked.Increment(ref _gcdDenied); }
        public static void IncLos() { Interlocked.Increment(ref _losDenied); }
        public static void IncRange() { Interlocked.Increment(ref _rangeDenied); }

        public static void Reset()
        {
            Interlocked.Exchange(ref _gcdDenied, 0);
            Interlocked.Exchange(ref _losDenied, 0);
            Interlocked.Exchange(ref _rangeDenied, 0);
        }
    }
}
