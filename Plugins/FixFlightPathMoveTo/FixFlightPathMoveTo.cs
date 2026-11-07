using System;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.Pathing;
using Styx.Plugins;

namespace HBCoreFixes
{
    // HB 5.4.8 core bug workarounds:
    // 1) Styx.CommonBot.FlightPaths.smethod_0 can throw
    //    InvalidOperationException("Sequence contains no matching element") when
    //    MeshNavigator.MoveTo evaluates ShouldTakeFlightpath with no connected
    //    destination node in the filtered continent node list.
    // 2) Tripper/Styx path generation can throw "Failed to generate path" /
    //    NullReferenceException inside obfuscated MeshNavigator/WowNavigator,
    //    causing repeated BotBase.Root.Tick crashes and POI loops.
    //
    // This plugin wraps the active navigation provider and catches these
    // HB-core navigation crashes so the bot can continue with safe failures
    // (MoveResult.Failed / empty path / false) instead of crash-looping.
    public class FixFlightPathMoveTo : HBPlugin
    {
        private static readonly object Sync = new object();
        private static FlightPathSafeNavigationProvider _wrapper;
        private static INavigationProvider _wrappedInner;

        public override string Name { get { return "HB Core Fix: FlightPath MoveTo"; } }
        public override string Author { get { return "repo maintenance"; } }
        public override Version Version { get { return new Version(1, 1, 0, 0); } }
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
                catch (Exception ex)
                {
                    if (IsFlightPathSequenceBug(ex))
                    {
                        LogOnce("Caught HB core FlightPaths Sequence-contains-no-matching-element bug during MoveTo; returning Failed for this tick. " + ex.Message);
                        return MoveResult.Failed;
                    }

                    if (IsPathGenerationCrash(ex))
                    {
                        LogOnce("Caught HB core path-generation crash during MoveTo; returning Failed for this tick. " + Describe(ex));
                        return MoveResult.Failed;
                    }

                    throw;
                }
            }

            public bool Clear()
            {
                try { return _inner != null && _inner.Clear(); }
                catch (Exception ex)
                {
                    if (IsFlightPathSequenceBug(ex) || IsPathGenerationCrash(ex))
                    {
                        LogOnce("Caught HB core navigation crash during Clear; returning false. " + Describe(ex));
                        return false;
                    }
                    throw;
                }
            }

            public WoWPoint[] GeneratePath(WoWPoint from, WoWPoint to)
            {
                try
                {
                    if (_inner == null)
                        return new WoWPoint[0];
                    var path = _inner.GeneratePath(from, to);
                    return path ?? new WoWPoint[0];
                }
                catch (Exception ex)
                {
                    if (IsFlightPathSequenceBug(ex) || IsPathGenerationCrash(ex))
                    {
                        LogOnce("Caught HB core path-generation crash during GeneratePath; returning empty path. " + Describe(ex));
                        return new WoWPoint[0];
                    }
                    throw;
                }
            }

            public bool CanNavigateFully(WoWPoint from, WoWPoint to, int maxHops)
            {
                try { return _inner != null && _inner.CanNavigateFully(from, to, maxHops); }
                catch (Exception ex)
                {
                    if (IsFlightPathSequenceBug(ex) || IsPathGenerationCrash(ex))
                    {
                        LogOnce("Caught HB core navigation crash during CanNavigateFully; returning false. " + Describe(ex));
                        return false;
                    }
                    throw;
                }
            }

            public bool AtLocation(WoWPoint point1, WoWPoint point2)
            {
                try { return _inner != null && _inner.AtLocation(point1, point2); }
                catch (Exception ex)
                {
                    if (IsFlightPathSequenceBug(ex) || IsPathGenerationCrash(ex)) return false;
                    throw;
                }
            }

            private static bool IsFlightPathSequenceBug(Exception ex)
            {
                if (ex == null)
                    return false;
                string msg = ex.Message ?? string.Empty;
                string trace = ex.ToString() ?? string.Empty;
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

            private static bool IsPathGenerationCrash(Exception ex)
            {
                if (ex == null)
                    return false;

                string text = ex.ToString() ?? string.Empty;
                if (text.IndexOf("Tripper.Navigation.WowNavigator", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                if (text.IndexOf("Styx.Pathing.MeshNavigator", StringComparison.OrdinalIgnoreCase) >= 0
                    && (text.IndexOf("Failed to generate path", StringComparison.OrdinalIgnoreCase) >= 0
                        || text.IndexOf("System.NullReferenceException", StringComparison.OrdinalIgnoreCase) >= 0
                        || text.IndexOf("FindPath", StringComparison.OrdinalIgnoreCase) >= 0))
                    return true;
                if (ex.Message != null && ex.Message.IndexOf("Failed to generate path", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                return false;
            }

            private static string Describe(Exception ex)
            {
                try
                {
                    if (ex == null) return "(null)";
                    string msg = ex.Message ?? ex.GetType().Name;
                    Exception inner = ex.InnerException;
                    if (inner != null && !string.IsNullOrEmpty(inner.Message))
                        msg = ex.GetType().Name + ": " + msg + " ---> " + inner.GetType().Name + ": " + inner.Message;
                    else
                        msg = ex.GetType().Name + ": " + msg;
                    return msg;
                }
                catch
                {
                    try { return ex.GetType().Name; }
                    catch { return "exception"; }
                }
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
