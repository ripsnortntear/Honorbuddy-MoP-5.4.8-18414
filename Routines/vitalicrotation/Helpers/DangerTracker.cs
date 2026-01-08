using System;
using System.Collections.Generic;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    /// <summary>
    /// Système centralisé de gestion des événements de menace pour Step 4
    /// Interface avec ThreatTable et DefensivesManager
    /// </summary>
    public static class DangerTracker
    {
        private static readonly object _lock = new object();
        private static readonly Dictionary<ulong, DateTime> _activeDangerWindows = new Dictionary<ulong, DateTime>();
        private static readonly Dictionary<ulong, int> _activeDangerSpells = new Dictionary<ulong, int>();

        /// <summary>
        /// Signale un danger détecté (cast/aura) depuis EventHandlers
        /// </summary>
        public static void RaiseDanger(int spellId, ulong srcGuid, ulong dstGuid)
        {
            try
            {
                // Pour l'instant, on se concentre sur les menaces visant le joueur
                var me = Styx.StyxWoW.Me;
                if (me == null || dstGuid != me.Guid) return;

                var src = Styx.WoWInternals.ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid);
                if (src == null || !src.IsValid || src.IsFriendly) return;

                // Lookup dans ThreatTable pour déterminer la réponse appropriée
                var entry = FindThreatEntry(spellId);
                if (entry != null)
                {
                    // Ouvre une fenêtre de danger
                    lock (_lock)
                    {
                        var until = DateTime.UtcNow.AddSeconds(entry.WindowSeconds > 0 ? entry.WindowSeconds : 2.0);
                        _activeDangerWindows[srcGuid] = until;
                        _activeDangerSpells[srcGuid] = spellId;
                    }

                    // Simple notification - pas d'appel direct DefensivesManager pour éviter dépendances
                    if (VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                    {
                        Styx.Common.Logging.Write("[DangerTracker] Danger detected: {0} from {1} (responses: {2})", 
                            spellId, src.Name, entry.Responses);
                    }
                }
            }
            catch { /* Always catch exceptions in event systems */ }
        }

        /// <summary>
        /// Vérifie si une fenêtre de danger est active pour un ennemi donné
        /// </summary>
        public static bool IsDangerActive(ulong srcGuid)
        {
            lock (_lock)
            {
                DateTime until;
                if (_activeDangerWindows.TryGetValue(srcGuid, out until))
                {
                    if (DateTime.UtcNow < until)
                        return true;
                    else
                    {
                        // Nettoie les entrées expirées
                        _activeDangerWindows.Remove(srcGuid);
                        _activeDangerSpells.Remove(srcGuid);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Vérifie s'il y a un danger actif quelconque
        /// </summary>
        public static bool IsAnyDangerActive()
        {
            lock (_lock)
            {
                var now = DateTime.UtcNow;
                var toRemove = new List<ulong>();

                foreach (var kv in _activeDangerWindows)
                {
                    if (now < kv.Value)
                        return true;
                    else
                        toRemove.Add(kv.Key);
                }

                // Nettoie les expirées
                foreach (var guid in toRemove)
                {
                    _activeDangerWindows.Remove(guid);
                    _activeDangerSpells.Remove(guid);
                }
            }
            return false;
        }

        /// <summary>
        /// Récupère le spell ID du danger actif pour un ennemi
        /// </summary>
        public static int GetActiveDangerSpell(ulong srcGuid)
        {
            if (IsDangerActive(srcGuid))
            {
                lock (_lock)
                {
                    int spellId;
                    if (_activeDangerSpells.TryGetValue(srcGuid, out spellId))
                        return spellId;
                }
            }
            return 0;
        }

        /// <summary>
        /// Force l'expiration d'une fenêtre de danger (utile pour les auras supprimées)
        /// </summary>
        public static void ClearDanger(ulong srcGuid)
        {
            lock (_lock)
            {
                _activeDangerWindows.Remove(srcGuid);
                _activeDangerSpells.Remove(srcGuid);
            }
        }

        /// <summary>
        /// Reset complet du système (changement de zone, etc.)
        /// </summary>
        public static void Reset()
        {
            lock (_lock)
            {
                _activeDangerWindows.Clear();
                _activeDangerSpells.Clear();
            }
        }

        // Trouve l'entrée ThreatTable correspondante (simplifiée)
        private static ThreatEntry FindThreatEntry(int spellId)
        {
            // Simulation d'une table simple - dans la vraie implem, on ferait appel à ThreatTable
            // Pour l'instant, retournons des réponses par défaut selon le type de spell
            var magicSpells = new HashSet<int> { 116858, 11366, 51505, 30451, 78674, 8092, 12472, 114050 };
            var physicalSpells = new HashSet<int> { 113656, 5211, 8676, 5308, 46924, 51690 };
            
            if (magicSpells.Contains(spellId))
            {
                return new ThreatEntry 
                { 
                    SpellId = spellId, 
                    WindowSeconds = 2.0, 
                    Responses = DefensiveFlags.Cloak
                };
            }
            else if (physicalSpells.Contains(spellId))
            {
                return new ThreatEntry 
                { 
                    SpellId = spellId, 
                    WindowSeconds = 2.0, 
                    Responses = DefensiveFlags.Evasion | DefensiveFlags.Feint
                };
            }

            return null;
        }

        // Enums locaux pour éviter dépendances
        [Flags]
        private enum DefensiveFlags { None = 0, Cloak = 1, Evasion = 2, Feint = 4 }

        // Classe helper interne
        private class ThreatEntry
        {
            public int SpellId;
            public double WindowSeconds;
            public DefensiveFlags Responses;
        }
    }
}