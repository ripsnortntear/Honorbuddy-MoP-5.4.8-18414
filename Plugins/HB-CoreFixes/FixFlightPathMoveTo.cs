using System;
using System.Windows.Media;
using Styx.Common;
using Styx.Pathing;
using Styx.Plugins;

namespace HBCoreFixes
{
    // HB 5.4.8 core bug workaround:
    // Styx.CommonBot.FlightPaths.smethod_0 can throw
    // InvalidOperationException("Sequence contains no matching element")
    // when the nearest start flight node has no connected destination in the
    // filtered continent node list. MeshNavigator.MoveTo calls
    // ShouldTakeFlightpath/smethod_0 directly, which can crash BotBase.Root.Tick.
    //
    // This plugin wraps the active navigation provider and catches that specific
    // failure so the bot can continue (returns MoveResult.Failed for that tick).
    public class FixFlightPathMoveTo : HBPlugin
    {
        private static readonly object Sync = new object();
        private static FlightPathSafeNavigationProvider _wrapper;
        private static INavigationProvider _wrappedInner;

        public override string Name { get { return "HB Core Fix: FlightPath MoveTo"; } }
        public override string Author { get { return "repo maintenance"; } }
        public override Version Version { get { return new Version(1, 0, 0, 0); } }
        public override bool WantButton { get { return false; } }

        public override void Initialize()
        {
            TryInstall();
        }

        public override void Pulse()
        {
            TryInstall();
        }

        private static void TryInstall()
        {
            try
            {
                lock (Sync)
                {
                    var current = Navigator.NavigationProvider;
                    if (current == null)
                        return;

                    var ours = current as FlightPathSafeNavigationProvider;
                    if (ours != null)
                    {
                        // Keep wrapper's inner in sync if HB replaced the real provider
                        // in an unexpected way? Navigator setter usually swaps us out.
                        return;
                    }

                    // Avoid wrapping our own stale wrapper state.
                    if (_wrapper != null && _wrappedInner == current)
                        return;

                    var wrapper = new FlightPathSafeNavigationProvider(current);
                    Navigator.NavigationProvider = wrapper;
                    _wrapper = wrapper;
                    _wrappedInner = current;
                    Logging.Write("[HB Core Fix] Installed FlightPath MoveTo safety wrapper around " + current.GetType().Name + ".");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDiagnostic("[HB Core Fix] FlightPath wrapper install failed: " + ex.Message);
            }
        }

        private static void TryUninstall()
        {
            try
            {
                lock (Sync)
                {
                    var current = Navigator.NavigationProvider as FlightPathSafeNavigationProvider;
                    if (current != null && current.Inner != null)
                    {
                        Navigator.NavigationProvider = current.Inner;
                        Logging.Write("[HB Core Fix] Removed FlightPath MoveTo safety wrapper.");
                    }
                    _wrapper = null;
                    _wrappedInner = null;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDiagnostic("[HB Core Fix] FlightPath wrapper uninstall failed: " + ex.Message);
            }
        }

        // HBPlugin does not expose a consistent virtual shutdown method across all
        // 5.4.8 builds; Pulse/Initialize manage installation. If Dispose exists in
        // this HB build, it can be added later.
        public void Shutdown()
        {
            TryUninstall();
        }

        public void Dispose()
        {
            TryUninstall();
        }

        private sealed class FlightPathSafeNavigationProvider : INavigationProvider
        {
            private static DateTime _lastLogUtc = DateTime.MinValue;
            private readonly INavigationProvider _inner;

            public FlightPathSafeNavigationProvider(INavigationProvider inner)
            {
                _inner = inner;
            }

            public INavigationProvider Inner { get { return _inner; } }

            public float PathPrecision
            {
                get { return _inner != null ? _inner.PathPrecision : 0.5f; }
                set { if (_inner != null) _inner.PathPrecision = value; }
            }

            public IStuckHandler StuckHandler
            {
                get { return _inner != null ? _inner.StuckHandler : null; }
                set { if (_inner != null) _inner.StuckHandler = value; }
            }

            public MoveResult MoveTo(WoWPoint location)
            {
                try
                {
                    if (_inner == null)
                        return MoveResult.Failed;
                    return _inner.MoveTo(location);
                }
                catch (InvalidOperationException ex)
                {
                    if (IsFlightPathSequenceBug(ex))
                    {
                        LogOnce("Caught HB core FlightPaths.smethod_0 Sequence-contains-no-matching-element bug during MoveTo; returning Failed for this tick. " + ex.Message);
                        return MoveResult.Failed;
                    }
                    throw;
                }
            }

            public bool Clear()
            {
                try { return _inner != null && _inner.Clear(); }
                catch (InvalidOperationException ex)
                {
                    if (IsFlightPathSequenceBug(ex)) return false;
                    throw;
                }
            }

            public WoWPoint[] GeneratePath(WoWPoint from, WoWPoint to)
            {
                if (_inner == null)
                    return new WoWPoint[0];
                return _inner.GeneratePath(from, to);
            }

            public bool CanNavigateFully(WoWPoint from, WoWPoint to, int maxHops)
            {
                try { return _inner != null && _inner.CanNavigateFully(from, to, maxHops); }
                catch (InvalidOperationException ex)
                {
                    if (IsFlightPathSequenceBug(ex)) return false;
                    throw;
                }
            }

            public bool AtLocation(WoWPoint point1, WoWPoint point2)
            {
                return _inner != null && _inner.AtLocation(point1, point2);
            }

            private static bool IsFlightPathSequenceBug(InvalidOperationException ex)
            {
                if (ex == null)
                    return false;
                string msg = ex.Message ?? string.Empty;
                string trace = ex.StackTrace ?? string.Empty;
                // "Sequence contains no matching element" from Enumerable.First(predicate)
                // inside Styx.CommonBot.FlightPaths (obfuscated smethod_0).
                if (msg.IndexOf("Sequence contains no matching element", StringComparison.OrdinalIgnoreCase) >= 0
                    && (trace.IndexOf("FlightPaths", StringComparison.OrdinalIgnoreCase) >= 0 || trace.IndexOf("ShouldTakeFlightpath", StringComparison.OrdinalIgnoreCase) >= 0))
                    return true;
                if (trace.IndexOf("Styx.CommonBot.FlightPaths", StringComparison.OrdinalIgnoreCase) >= 0
                    && msg.IndexOf("no matching element", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                return false;
            }

            private static void LogOnce(string message)
            {
                try
                {
                    if ((DateTime.UtcNow - _lastLogUtc).TotalSeconds < 15)
                        return;
                    _lastLogUtc = DateTime.UtcNow;
                    Logging.Write(Colors.OrangeRed, "[HB Core Fix] " + message);
                }
                catch { }
            }
        }
    }
}
