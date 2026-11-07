using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Xml.Linq;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;

namespace HBCoreFixes
{
    // HB 5.4.8 core bug fix for Styx.CommonBot.FlightPaths:
    //
    // Root cause (verified via decompilation of Honorbuddy.exe):
    // 1) When TAXIMAP_OPENED fires, FlightPaths.smethod_4 iterates all DBC taxi
    //    nodes and adds every node to FlightPaths.XmlNodes (and saves them to
    //    Settings\<Realm>\<Char>\FlightPaths.xml), even when !taxiFrameNode.Reachable.
    //    This pollutes XmlNodes with unreachable, opposite-faction, and debug DBC
    //    nodes (e.g. "Generic, World Target 002", "Flame Crest, Burning Steppes")
    //    that have empty <Connections />.
    // 2) FlightPaths.smethod_0 picks the closest node on the current continent as
    //    the start flight node (xmlFlightNode_2) and then calls:
    //      list.OrderBy(...).First(n => xmlFlightNode_2.Connections.Contains(n.Name))
    //    When the closest node has no connections in `list`, Enumerable.First throws
    //    InvalidOperationException("Sequence contains no matching element").
    // 3) NOTE: Wrapping Navigator.NavigationProvider with a non-MeshNavigator class
    //    causes Navigator.set_NavigationProvider to call MeshNavigator.OnRemoveAsCurrent(),
    //    which unhooks BotEvents.OnPulse (UpdateMaps) and calls WowNavigator.Dispose(),
    //    setting Nav.Mesh = null and Nav.MeshQuery = null and causing WowNavigator.FindPath
    //    to throw NullReferenceException. Therefore we MUST NOT replace MeshNavigator.
    //
    // Fix:
    // Leave MeshNavigator completely untouched and sanitize FlightPaths.XmlNodes
    // so every active node in smethod_0's candidate list is guaranteed to have at
    // least one valid connected partner on the same continent.
    public class FixFlightPathMoveTo : HBPlugin
    {
        private bool _attachedEvents;
        private DateTime _lastSanitizeUtc = DateTime.MinValue;
        private int _lastSanitizedNodeCount = -1;
        private uint _lastSanitizedMapId = uint.MaxValue;

        public override string Name { get { return "HB Core Fix: FlightPath MoveTo"; } }
        public override string Author { get { return "repo maintenance"; } }
        public override Version Version { get { return new Version(2, 0, 0, 0); } }
        public override bool WantButton { get { return false; } }

        public override void Initialize()
        {
            RestoreMeshNavigatorIfWrapped();
            AttachEvents();
            SanitizeFlightNodes(true);
        }

        public override void Pulse()
        {
            RestoreMeshNavigatorIfWrapped();
            AttachEvents();
            SanitizeFlightNodes(false);
        }

        public void Shutdown()
        {
            DetachEvents();
        }

        public void Dispose()
        {
            DetachEvents();
        }

        private void AttachEvents()
        {
            if (_attachedEvents)
                return;

            try
            {
                Lua.Events.AttachEvent("TAXIMAP_OPENED", OnTaxiMapOpened);
                _attachedEvents = true;
            }
            catch
            {
            }
        }

        private void DetachEvents()
        {
            if (!_attachedEvents)
                return;

            try
            {
                Lua.Events.DetachEvent("TAXIMAP_OPENED", OnTaxiMapOpened);
            }
            catch
            {
            }

            _attachedEvents = false;
        }

        private void OnTaxiMapOpened(object sender, LuaEventArgs args)
        {
            _lastSanitizedNodeCount = -1;
            SanitizeFlightNodes(true);
        }

        private static void RestoreMeshNavigatorIfWrapped()
        {
            try
            {
                var current = Navigator.NavigationProvider;
                if (current == null)
                    return;

                // If an earlier version of this plugin wrapped MeshNavigator,
                // MeshNavigator.OnRemoveAsCurrent() disposed its WowNavigator.
                // Replace it with a fresh MeshNavigator so OnSetAsCurrent()
                // reinitializes WowNavigator and UpdateMaps().
                if (current.GetType().Name == "FlightPathSafeNavigationProvider")
                {
                    var meshNav = new MeshNavigator();
                    Navigator.NavigationProvider = meshNav;
                    if (StyxWoW.IsInGame && StyxWoW.Me != null && StyxWoW.Me.IsValid)
                    {
                        meshNav.Nav.SetFactionQueryFilter(StyxWoW.Me.IsHorde);
                        meshNav.UpdateMaps();
                    }
                    Logging.Write(Colors.DeepSkyBlue, "[HB Core Fix] Restored native MeshNavigator (reinitialized WowNavigator).");
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDiagnostic("[HB Core Fix] Failed to restore MeshNavigator: " + ex.Message);
            }
        }

        private void SanitizeFlightNodes(bool force)
        {
            try
            {
                var nodes = FlightPaths.XmlNodes;
                if (nodes == null || nodes.Count == 0)
                    return;

                uint mapId = 0;
                try
                {
                    if (StyxWoW.Me != null && StyxWoW.Me.IsValid)
                        mapId = StyxWoW.Me.MapId;
                }
                catch
                {
                }

                DateTime now = DateTime.UtcNow;
                if (!force
                    && nodes.Count == _lastSanitizedNodeCount
                    && mapId == _lastSanitizedMapId
                    && (now - _lastSanitizeUtc).TotalSeconds < 2.0)
                    return;

                _lastSanitizeUtc = now;

                int removedGarbage = 0;
                int ignoredUnconnected = 0;

                // Step 1: Remove bogus/unlearned nodes dumped by FlightPaths.smethod_4
                // that have no MasterEntry and no connections (or empty coordinates).
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    var node = nodes[i];
                    if (node == null
                        || string.IsNullOrEmpty(node.Name)
                        || node.Location == WoWPoint.Empty
                        || (node.MasterEntry == 0 && (node.Connections == null || node.Connections.Count == 0)))
                    {
                        nodes.RemoveAt(i);
                        removedGarbage++;
                    }
                }

                // Step 2: Ensure that on every continent, every active node in
                // FlightPaths.smethod_0's candidate list has at least one connected
                // partner in that same candidate list. Iterate to a fixed point.
                bool changed = true;
                while (changed)
                {
                    changed = false;

                    var activeByContinent = nodes
                        .Where(n => n != null && !FlightPaths.IsIgnored(n.MasterEntry) && !FlightPaths.IsIgnored(n.Location))
                        .GroupBy(n => n.Continent)
                        .ToList();

                    foreach (var group in activeByContinent)
                    {
                        var activeNames = new HashSet<string>(
                            group.Select(n => n.Name).Where(name => !string.IsNullOrEmpty(name)),
                            StringComparer.Ordinal);

                        foreach (var node in group)
                        {
                            bool hasValidPartner = node.Connections != null
                                && node.Connections.Any(c => !string.Equals(c, node.Name, StringComparison.Ordinal) && activeNames.Contains(c));

                            if (!hasValidPartner)
                            {
                                if (node.MasterEntry == 0)
                                {
                                    if (nodes.Remove(node))
                                    {
                                        removedGarbage++;
                                        changed = true;
                                    }
                                }
                                else if (FlightPaths.IgnoreFp(node.Location))
                                {
                                    ignoredUnconnected++;
                                    changed = true;
                                }
                            }
                        }
                    }
                }

                _lastSanitizedNodeCount = nodes.Count;
                _lastSanitizedMapId = mapId;

                if (removedGarbage > 0 || ignoredUnconnected > 0)
                {
                    Logging.Write(
                        Colors.DeepSkyBlue,
                        string.Format(
                            "[HB Core Fix] Sanitized FlightPaths.XmlNodes: removed {0} unconnected/bogus node(s), ignored {1} isolated node location(s) ({2} valid nodes remain).",
                            removedGarbage,
                            ignoredUnconnected,
                            nodes.Count));

                    if (removedGarbage > 0)
                        TrySaveCleanedXml(nodes);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDiagnostic("[HB Core Fix] SanitizeFlightNodes error: " + ex.Message);
            }
        }

        private static void TrySaveCleanedXml(List<XmlFlightNode> nodes)
        {
            try
            {
                if (StyxWoW.Me == null || !StyxWoW.Me.IsValid)
                    return;

                string realm = StyxWoW.Me.RealmName;
                string name = StyxWoW.Me.Name;
                if (string.IsNullOrEmpty(realm) || string.IsNullOrEmpty(name))
                    return;

                string xmlPath = Path.Combine(Utilities.AssemblyDirectory, string.Format("Settings\\{0}\\{1}\\FlightPaths.xml", realm, name));
                if (!File.Exists(xmlPath))
                    return;

                var root = new XElement("FlightNodes");
                foreach (var node in nodes)
                {
                    if (node != null)
                        root.Add(node.ToXml());
                }
                root.Save(xmlPath);
            }
            catch (Exception ex)
            {
                Logging.WriteDiagnostic("[HB Core Fix] Could not save cleaned FlightPaths.xml: " + ex.Message);
            }
        }
    }
}
