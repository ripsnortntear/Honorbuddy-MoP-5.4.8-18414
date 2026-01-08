using System;
using System.Collections.Generic;
using Styx; // for StyxWoW
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Managers
{
    [Flags]
    public enum DefensiveFlags { None = 0, Feint = 1, Cloak = 2, Evasion = 4 }

    public enum ThreatTrigger { AuraApplied, AuraRemoved, CastStart }

    public sealed class ThreatEntry
    {
        public int SpellId;                 // ID aura ou sort ennemi
        public ThreatTrigger Trigger;       // Quand ouvrir la fenêtre
        public bool UntilAuraRemoved;       // Fenêtre = durée de l'aura
        public double WindowSeconds;        // Fenêtre fixe (casts)
        public double MaxRange = 0;         // 0 = ignore portée
        public DefensiveFlags Responses;    // Feint/Cloak/Evasion
    }

    public static class ThreatTable
    {
        // --- THREAT TABLE (MoP 5.4.8) ---
        public enum DangerType
        {
            MagicNuke,
            PhysicalWhirl,
            TeleportBurst
        }

        public class DangerRule
        {
            public int SpellId;
            public string Name;
            public DangerType Type;
            public float MaxDangerRange; // yards (0 = ignore range)
            public int MinCastTimeMs;    // pour éviter de réagir à de faux positifs instant
            public bool IsAura;          // true = aura dangereuse portée par l'ennemi
        }

        // Casts ciblés (sur nous) / projectiles dangereux
        private static readonly DangerRule[] DangerCasts = new[]
        {
            new DangerRule{ SpellId=116858, Name="Chaos Bolt",  Type=DangerType.MagicNuke,  MaxDangerRange=45f, MinCastTimeMs=800,  IsAura=false },
            new DangerRule{ SpellId=11366,  Name="Pyroblast",   Type=DangerType.MagicNuke,  MaxDangerRange=40f, MinCastTimeMs=800,  IsAura=false },
            new DangerRule{ SpellId=51505,  Name="Lava Burst",  Type=DangerType.MagicNuke,  MaxDangerRange=40f, MinCastTimeMs=500,  IsAura=false },
            new DangerRule{ SpellId=78674,  Name="Starsurge",   Type=DangerType.MagicNuke,  MaxDangerRange=40f, MinCastTimeMs=500,  IsAura=false }
        };

        // Auras/mêlées à éviter (tourbillon, burst téléporté)
        private static readonly DangerRule[] DangerAuras = new[]
        {
            new DangerRule{ SpellId=46924,  Name="Bladestorm",     Type=DangerType.PhysicalWhirl, MaxDangerRange=8f,  MinCastTimeMs=0, IsAura=true },
            new DangerRule{ SpellId=51690,  Name="Killing Spree",  Type=DangerType.TeleportBurst, MaxDangerRange=6f,  MinCastTimeMs=0, IsAura=true },
            new DangerRule{ SpellId=113656, Name="Fists of Fury",  Type=DangerType.PhysicalWhirl, MaxDangerRange=8f,  MinCastTimeMs=0, IsAura=true }
        };

        private static bool HasAuraId(WoWUnit u, int id)
        {
            if (u == null) return false;
            var auras = u.GetAllAuras();
            for (int i = 0; i < auras.Count; i++)
            {
                var a = auras[i];
                if (a != null && a.IsActive && a.SpellId == id) return true;
            }
            return false;
        }

        private static bool IsCastingSpellId(WoWUnit u, int id, int minCastMs)
        {
            if (u == null || !u.IsCasting) return false;
            if (u.CurrentCastTimeLeft.TotalMilliseconds < minCastMs) return false;
            try { return u.CastingSpellId == id || (u.CastingSpell != null && u.CastingSpell.Id == id); }
            catch { return false; }
        }

        // API publique pour managers
        public static bool AnyDangerCastOnMe(out WoWUnit caster, out DangerRule rule)
        {
            caster = null; rule = null;
            var me = StyxWoW.Me;
            var list = ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
            for (int i = 0; i < list.Count; i++)
            {
                var u = list[i];
                if (u == null || !u.IsAlive || !u.Attackable || !u.IsHostile) continue;
                for (int j = 0; j < DangerCasts.Length; j++)
                {
                    var r = DangerCasts[j];
                    if (!IsCastingSpellId(u, r.SpellId, r.MinCastTimeMs)) continue;
                    if (r.MaxDangerRange > 0f) { try { if (u.Distance > r.MaxDangerRange) continue; } catch { } }

                    // On vérifie idéalement que la cible est bien "me"
                    try { if (u.CurrentTargetGuid != me.Guid) continue; } catch { continue; }

                    caster = u; rule = r; return true;
                }
            }
            return false;
        }

        public static bool AnyDangerAuraNearMe(out WoWUnit source, out DangerRule rule)
        {
            source = null; rule = null;
            var list = ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
            for (int i = 0; i < list.Count; i++)
            {
                var u = list[i];
                if (u == null || !u.IsAlive || !u.Attackable || !u.IsHostile) continue;

                for (int j = 0; j < DangerAuras.Length; j++)
                {
                    var r = DangerAuras[j];
                    if (!HasAuraId(u, r.SpellId)) continue;
                    if (r.MaxDangerRange > 0f) { try { if (u.Distance > r.MaxDangerRange) continue; } catch { } }

                    source = u; rule = r; return true;
                }
            }
            return false;
        }

        // Utilisé par InterruptManager.TryDeadlyThrowEnhanced()
        public static bool IsDangerousCast(WoWUnit u)
        {
            if (u == null || !u.IsCasting) return false;
            for (int j = 0; j < DangerCasts.Length; j++)
                if (IsCastingSpellId(u, DangerCasts[j].SpellId, DangerCasts[j].MinCastTimeMs))
                    return true;
            return false;
        }

        // Liste minifiée MoP conforme v.zip (exemples clés)
        public static readonly List<ThreatEntry> Entries = new List<ThreatEntry>
        {
            // --- AURAS OFFENSIVES (valable tant que l'aura est up) ---
            new ThreatEntry{ SpellId=107574, Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, Responses=DefensiveFlags.Feint },   // Avatar (War)
            new ThreatEntry{ SpellId=1719,   Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, Responses=DefensiveFlags.Feint },   // Recklessness (War)
            new ThreatEntry{ SpellId=12472,  Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, Responses=DefensiveFlags.Cloak },   // Icy Veins (Mage)
            new ThreatEntry{ SpellId=114049, Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, Responses=DefensiveFlags.Feint },   // Ascendance (Shaman)
            new ThreatEntry{ SpellId=3045,   Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, Responses=DefensiveFlags.Feint },   // Rapid Fire (Hunter)

            // --- CASTS DANGEREUX (fenêtre courte + portée) ---
            new ThreatEntry{ SpellId=11366,  Trigger=ThreatTrigger.CastStart, WindowSeconds=2.0, Responses=DefensiveFlags.Cloak,  MaxRange=35 }, // Pyroblast
            new ThreatEntry{ SpellId=30451,  Trigger=ThreatTrigger.CastStart, WindowSeconds=1.5, Responses=DefensiveFlags.Cloak,  MaxRange=35 }, // Arcane Blast
            new ThreatEntry{ SpellId=51505,  Trigger=ThreatTrigger.CastStart, WindowSeconds=1.6, Responses=DefensiveFlags.Cloak,  MaxRange=35 }, // Lava Burst
            new ThreatEntry{ SpellId=116858, Trigger=ThreatTrigger.CastStart, WindowSeconds=2.1, Responses=DefensiveFlags.Cloak,  MaxRange=40 }, // Chaos Bolt

            // --- CONTACT / MÊLÉE COURTE ---
            new ThreatEntry{ SpellId=5211,   Trigger=ThreatTrigger.CastStart, WindowSeconds=0.8, Responses=DefensiveFlags.Evasion, MaxRange=8 }, // Bash (Druid)
            new ThreatEntry{ SpellId=8676,   Trigger=ThreatTrigger.CastStart, WindowSeconds=0.8, Responses=DefensiveFlags.Evasion, MaxRange=8 }, // Ambush (Rogue)
        };

        // Collections internes pour le tracking des menaces
        private static readonly HashSet<int> _recentCasts = new HashSet<int>();
        private static readonly List<DangerAura> _dangerAuras = new List<DangerAura>();
        private static readonly Dictionary<ulong, ThreatSnapshot> _snapshots = new Dictionary<ulong, ThreatSnapshot>();

        // Classes internes pour le tracking
        private class DangerAura
        {
            public ulong CasterGuid;
            public int SpellId;
            public DateTime StartUtc;
            public bool Expired(DateTime now) 
            { 
                return (now - StartUtc).TotalSeconds > 30; // timeout générique
            }
        }

        private class ThreatSnapshot
        {
            public DateTime LastSeenUtc;
            public bool WasThreatening;
        }

        /// <summary>
        /// Reset complet de la table des menaces (utilisé lors d'un hard reset).
        /// </summary>
        public static void Reset()
        {
            _recentCasts.Clear();
            _dangerAuras.Clear();
            _snapshots.Clear();
        }

        /// <summary>
        /// Purge les entrées expirées de la table des menaces (utilisé lors des soft resets).
        /// </summary>
        public static void Prune()
        {
            DateTime now = DateTime.UtcNow;

            // Purge les auras dangereuses expirées
            for (int i = _dangerAuras.Count - 1; i >= 0; i--)
            {
                if (_dangerAuras[i].Expired(now))
                    _dangerAuras.RemoveAt(i);
            }

            // Purge les snapshots anciens (plus de 30 secondes)
            var keysToRemove = new List<ulong>();
            foreach (var kv in _snapshots)
            {
                if ((now - kv.Value.LastSeenUtc).TotalSeconds > 30)
                    keysToRemove.Add(kv.Key);
            }

            foreach (var key in keysToRemove)
                _snapshots.Remove(key);
        }
    }
}
