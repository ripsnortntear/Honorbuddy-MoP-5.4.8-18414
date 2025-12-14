using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VitalicRotation.Settings;
using VitalicRotation.UI;
using VitalicRotation.Managers; // added for MacroManager
using VitalicRotation.Helpers;  // for TotemMappings/Totems

namespace VitalicRotation.Helpers
{
    internal static class EventHandlers
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private static bool _attached;
        private static bool _qolHooksAttached = false;
        private static bool _roleReadyHooksAttached = false;
        
        // Flag pour les hooks core
        private static bool _coreHooksAttached = false;

        private const string LogPrefix = "[EventHandlers] ";
        private const int SpellId_DeathGrip = 49576;
    private const int SpellId_IceBlock = 45438; // Mage Ice Block (MoP)
    private const int SpellId_SmokeBomb = 76577; // Rogue Smoke Bomb (MoP)

        // Emergency layer spell IDs
        private const int SpellId_Blink = 1953;
        private const int SpellId_Shimmer = 212653;
        private const int SpellId_DeepFreeze = 44572;
        private const int SpellId_Polymorph = 118;
        private const int SpellId_PsychicScream = 8122;
        private const int SpellId_CloakOfShadows = 31224;
        private const int SpellId_Vanish = 1856;
        private const int SpellId_Shadowstep = 36554;
        private const int SpellId_Sprint = 2983;

        // Lightweight diagnostic helper (guards Logger.Write with DiagnosticMode)
        private static void Trace(string message)
        {
            try { if (VitalicSettings.Instance != null && VitalicSettings.Instance.DiagnosticMode) Logger.Write(message); }
            catch { }
        }
        private static void Trace(string format, params object[] args)
        {
            try { if (VitalicSettings.Instance != null && VitalicSettings.Instance.DiagnosticMode) Logger.Write(format, args); }
            catch { }
        }

        // === Emergency layer state ===
        private static DateTime _lastEmergencyTick = DateTime.MinValue;
        private static DateTime _lastBlinkSeen = DateTime.MinValue;


        // === Psyfiend tracking (CreatedBySpellId 108921) ===
        private static DateTime _lastPsyfiendSeenAt = DateTime.MinValue;
        private static ulong _lastPsyfiendGuid = 0UL;

        // Healer trinket auto-blind gating + throttle
        private static bool _healerTrinketWatcherEnabled;
        private static DateTime _lastHealerTrinketAuto = DateTime.MinValue;
    // Throttle for instance enter sound
    private static DateTime _lastInstanceSound = DateTime.MinValue;

        // Expose a small registrar to mirror v.zip API while reusing our combat-log handler
        public static class HealerTrinketWatcher
        {
            public static void Register()
            {
                _healerTrinketWatcherEnabled = true;
            }
        }

        // === Auto-Blind Healer Trinket ===
        // .NET 4.5.1 friendly: pas d'interpolation, pas d'opérateur ?.
        private static readonly HashSet<int> _healerTrinketBreaks = new HashSet<int>
        {
            // MoP 5.4.8 (parité immuable plan: seulement ces deux IDs)
            42292,   // PvP Trinket
            59752    // Every Man for Himself (Humain)
        };

        // Fenêtre "trinket -> blind" par cible
        private static readonly Dictionary<ulong, DateTime> _healerTrinketWindow = new Dictionary<ulong, DateTime>();

        internal static bool HasHealerTrinketWindow(ulong guid)
        {
            DateTime until;
            if (_healerTrinketWindow.TryGetValue(guid, out until))
                return until > DateTime.UtcNow;
            return false;
        }

        // Alias demandé par plan P1.4 (parité nomenclature Boolean_22 wrapper)
        internal static bool IsHealerTrinketWindowActive(ulong guid)
        {
            return HasHealerTrinketWindow(guid);
        }

        internal static void ClearHealerTrinketWindow(ulong guid)
        {
            if (_healerTrinketWindow.ContainsKey(guid))
                _healerTrinketWindow.Remove(guid);
        }

        // Heuristique simple (fidèle à v.zip: on vise les classes heal)
        internal static bool IsHealerClass(WoWUnit u)
        {
            if (u == null || !u.IsValid) return false;
            
            // Original parity: {4, 11} => Warrior, Druid (kept for backward compatibility)
            bool originalHealer = u.Class == WoWClass.Warrior || u.Class == WoWClass.Druid;
            
            // Extended mapping for actual healing classes/specs
            bool actualHealer = u.Class == WoWClass.Priest ||      // Priest (5)
                               u.Class == WoWClass.Paladin ||     // Paladin (2) 
                               u.Class == WoWClass.Shaman ||      // Shaman (7)
                               u.Class == WoWClass.Monk ||        // Monk (10)
                               u.Class == WoWClass.Druid;         // Druid (11) - already covered above
            
            return originalHealer || actualHealer;
        }

        // === Totem recency map (parité Class141.dictionary_0) ===
        // Stocke l'heure de détection récente par famille Totems
        public static readonly Dictionary<Totems, DateTime> TotemRecency = new Dictionary<Totems, DateTime>();

        // === Moteur menace défensifs (commun Cloak/Evasion/Feint) ===
        [Flags]
        private enum DefensiveFlags { None = 0, Cloak = 1, Evasion = 2, Feint = 4 }
        private enum ThreatTrigger { AuraApplied, AuraRemoved, CastStart, CastSuccess, ChannelTick, Periodic }
        private sealed class ThreatEntry
        {
            public int SpellId;            // ID sort/effet dangereux
            public ThreatTrigger Trigger;  // Type d’événement
            public double WindowSeconds;   // Durée fenêtre (si > 0)
            public double MaxRange;        // Portée max (0 = ignoré)
            public DefensiveFlags Responses; // Flags de réponses possibles
            public bool UntilAuraRemoved;  // Fenêtre tenue jusqu'à remove
        }

        // === Défensifs event-driven (MoP 5.4.8) ===
        // Définition simple (4.5.1 friendly)
        private sealed class DangerCast
        {
            public int SpellId;
            public DefensiveFlags Flags;
            public double MaxRange;      // 0 = ignorer la portée
            public double WindowSeconds; // fallback si on ne peut pas lire le cast time exact
        }

        private sealed class DangerAura
        {
            public int SpellId;
            public DefensiveFlags Flags;
            public double MaxRange; // 0 = ignorer
        }

        // Tables MoP complètes (étape 4 - expansion)
        private static readonly Dictionary<int, DangerCast> _dangerCasts = new Dictionary<int, DangerCast>
        {
            // Chaos Bolt (Démo) → Cloak
            { 116858, new DangerCast { SpellId = 116858, Flags = DefensiveFlags.Cloak, MaxRange = 40, WindowSeconds = 2.2 } },

            // Fists of Fury (Moine) → Evasion ou Feint (physique canal, mêlée)
            { 113656, new DangerCast { SpellId = 113656, Flags = DefensiveFlags.Evasion | DefensiveFlags.Feint, MaxRange = 8, WindowSeconds = 3.0 } },

            // Pyroblast → Cloak
            { 11366, new DangerCast { SpellId = 11366, Flags = DefensiveFlags.Cloak, MaxRange = 40, WindowSeconds = 2.0 } },

            // Lava Burst → Cloak  
            { 51505, new DangerCast { SpellId = 51505, Flags = DefensiveFlags.Cloak, MaxRange = 40, WindowSeconds = 1.8 } },

            // Arcane Blast → Cloak
            { 30451, new DangerCast { SpellId = 30451, Flags = DefensiveFlags.Cloak, MaxRange = 40, WindowSeconds = 1.5 } },

            // Execute → Feint/Evasion (physique mêlée burst)
            { 5308, new DangerCast { SpellId = 5308, Flags = DefensiveFlags.Feint | DefensiveFlags.Evasion, MaxRange = 8, WindowSeconds = 1.2 } },

            // Starsurge → Cloak
            { 78674, new DangerCast { SpellId = 78674, Flags = DefensiveFlags.Cloak, MaxRange = 40, WindowSeconds = 2.0 } },

            // Mind Blast → Cloak
            { 8092, new DangerCast { SpellId = 8092, Flags = DefensiveFlags.Cloak, MaxRange = 40, WindowSeconds = 1.5 } },

            // Bash/Mighty Bash → Evasion
            { 5211, new DangerCast { SpellId = 5211, Flags = DefensiveFlags.Evasion, MaxRange = 8, WindowSeconds = 0.8 } },

            // Ambush → Evasion
            { 8676, new DangerCast { SpellId = 8676, Flags = DefensiveFlags.Evasion, MaxRange = 8, WindowSeconds = 0.8 } },
        };

        private static readonly Dictionary<int, DangerAura> _dangerAuras = new Dictionary<int, DangerAura>
        {
            // Bladestorm (War) → Evasion (éviter les coups)
            { 46924, new DangerAura { SpellId = 46924, Flags = DefensiveFlags.Evasion | DefensiveFlags.Feint, MaxRange = 8 } },

            // Killing Spree (Voleur Combat) → Evasion
            { 51690, new DangerAura { SpellId = 51690, Flags = DefensiveFlags.Evasion, MaxRange = 10 } },

            // Recklessness → Feint
            { 1719, new DangerAura { SpellId = 1719, Flags = DefensiveFlags.Feint, MaxRange = 0 } },

            // Avatar → Feint
            { 107574, new DangerAura { SpellId = 107574, Flags = DefensiveFlags.Feint, MaxRange = 0 } },

            // Bestial Wrath → Feint
            { 19574, new DangerAura { SpellId = 19574, Flags = DefensiveFlags.Feint, MaxRange = 0 } },

            // Icy Veins → Cloak
            { 12472, new DangerAura { SpellId = 12472, Flags = DefensiveFlags.Cloak, MaxRange = 0 } },

            // Ascendance → Cloak/Feint
            { 114050, new DangerAura { SpellId = 114050, Flags = DefensiveFlags.Cloak | DefensiveFlags.Feint, MaxRange = 0 } },

            // Pillar of Frost → Feint
            { 51271, new DangerAura { SpellId = 51271, Flags = DefensiveFlags.Feint, MaxRange = 0 } },

            // Shadow Dance → Feint/Evasion
            { 51713, new DangerAura { SpellId = 51713, Flags = DefensiveFlags.Feint | DefensiveFlags.Evasion, MaxRange = 0 } },

            // Adrenaline Rush → Feint/Evasion
            { 13750, new DangerAura { SpellId = 13750, Flags = DefensiveFlags.Feint | DefensiveFlags.Evasion, MaxRange = 0 } },

            // Avenging Wrath → Cloak/Feint
            { 31884, new DangerAura { SpellId = 31884, Flags = DefensiveFlags.Cloak | DefensiveFlags.Feint, MaxRange = 0 } },

            // Incarnation forms → adapté
            { 102543, new DangerAura { SpellId = 102543, Flags = DefensiveFlags.Feint | DefensiveFlags.Evasion, MaxRange = 0 } }, // Feral
            { 102560, new DangerAura { SpellId = 102560, Flags = DefensiveFlags.Cloak, MaxRange = 0 } }, // Balance

            // Berserk → Feint/Evasion
            { 106951, new DangerAura { SpellId = 106951, Flags = DefensiveFlags.Feint | DefensiveFlags.Evasion, MaxRange = 0 } },
        };
        // NB: Extraits MoP à compléter selon besoins (Feint/Cloak/Evasion)
        private static readonly List<ThreatEntry> ThreatTable = new List<ThreatEntry>
        {
            // === AURAS OFFENSIVES (valable tant que l'aura est up) ===
            new ThreatEntry{ SpellId=107574, Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Feint },   // Avatar (War)
            new ThreatEntry{ SpellId=1719,   Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Feint },   // Recklessness (War)
            new ThreatEntry{ SpellId=12472,  Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Cloak },   // Icy Veins (Mage)
            new ThreatEntry{ SpellId=114050, Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Cloak | DefensiveFlags.Feint }, // Ascendance (Ele)
            new ThreatEntry{ SpellId=19574,  Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Feint },   // Bestial Wrath
            new ThreatEntry{ SpellId=51271,  Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Feint },   // Pillar of Frost (DK)
            new ThreatEntry{ SpellId=51713,  Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Feint | DefensiveFlags.Evasion }, // Shadow Dance
            new ThreatEntry{ SpellId=13750,  Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Feint | DefensiveFlags.Evasion }, // Adrenaline Rush
            new ThreatEntry{ SpellId=31884,  Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Cloak | DefensiveFlags.Feint }, // Avenging Wrath
            new ThreatEntry{ SpellId=102543, Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Feint | DefensiveFlags.Evasion }, // Incarnation Feral
            new ThreatEntry{ SpellId=102560, Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Cloak }, // Incarnation Balance
            new ThreatEntry{ SpellId=106951, Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=0, Responses=DefensiveFlags.Feint | DefensiveFlags.Evasion }, // Berserk
            new ThreatEntry{ SpellId=46924,  Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=8,  Responses=DefensiveFlags.Evasion | DefensiveFlags.Feint }, // Bladestorm
            new ThreatEntry{ SpellId=51690,  Trigger=ThreatTrigger.AuraApplied, UntilAuraRemoved=true, MaxRange=10, Responses=DefensiveFlags.Evasion }, // Killing Spree

            // === CASTS DANGEREUX (fenêtre courte + portée) ===
            new ThreatEntry{ SpellId=116858, Trigger=ThreatTrigger.CastStart, WindowSeconds=2.2, MaxRange=40, Responses=DefensiveFlags.Cloak },         // Chaos Bolt
            new ThreatEntry{ SpellId=11366,  Trigger=ThreatTrigger.CastStart, WindowSeconds=2.0, MaxRange=40, Responses=DefensiveFlags.Cloak },         // Pyroblast
            new ThreatEntry{ SpellId=51505,  Trigger=ThreatTrigger.CastStart, WindowSeconds=1.8, MaxRange=40, Responses=DefensiveFlags.Cloak },         // Lava Burst
            new ThreatEntry{ SpellId=30451,  Trigger=ThreatTrigger.CastStart, WindowSeconds=1.5, MaxRange=35, Responses=DefensiveFlags.Cloak },         // Arcane Blast
            new ThreatEntry{ SpellId=78674,  Trigger=ThreatTrigger.CastStart, WindowSeconds=2.0, MaxRange=40, Responses=DefensiveFlags.Cloak },         // Starsurge
            new ThreatEntry{ SpellId=8092,   Trigger=ThreatTrigger.CastStart, WindowSeconds=1.5, MaxRange=40, Responses=DefensiveFlags.Cloak },         // Mind Blast
            new ThreatEntry{ SpellId=113656, Trigger=ThreatTrigger.CastStart, WindowSeconds=3.0, MaxRange=8,  Responses=DefensiveFlags.Evasion | DefensiveFlags.Feint }, // Fists of Fury

            // === CONTACT / MÊLÉE COURTE ===
            new ThreatEntry{ SpellId=5211,   Trigger=ThreatTrigger.CastStart, WindowSeconds=0.8, MaxRange=8,  Responses=DefensiveFlags.Evasion },       // Bash (Druid)
            new ThreatEntry{ SpellId=8676,   Trigger=ThreatTrigger.CastStart, WindowSeconds=0.8, MaxRange=8,  Responses=DefensiveFlags.Evasion },       // Ambush
            new ThreatEntry{ SpellId=5308,   Trigger=ThreatTrigger.CastStart, WindowSeconds=1.2, MaxRange=8,  Responses=DefensiveFlags.Feint | DefensiveFlags.Evasion }, // Execute

            // === HEALS MAJEURS (pour interrupt prio) ===
            new ThreatEntry{ SpellId=2060,   Trigger=ThreatTrigger.CastStart, WindowSeconds=2.5, MaxRange=40, Responses=DefensiveFlags.None },          // Greater Heal (interrupt prio)
            new ThreatEntry{ SpellId=596,    Trigger=ThreatTrigger.CastStart, WindowSeconds=3.0, MaxRange=40, Responses=DefensiveFlags.None },          // Prayer of Healing
            new ThreatEntry{ SpellId=1064,   Trigger=ThreatTrigger.CastStart, WindowSeconds=2.5, MaxRange=40, Responses=DefensiveFlags.None },          // Chain Heal
            new ThreatEntry{ SpellId=73920,  Trigger=ThreatTrigger.CastStart, WindowSeconds=2.0, MaxRange=40, Responses=DefensiveFlags.None },          // Healing Rain
        };

        // === Matrice de contres (façon Vitalic) ==============================
        // Deux niveaux:
        // 1) _counterMatrixIds: mapping SpellId déclencheur -> liste ordonnée d'IDs de contres (parité dictionary_4)
        // 2) _counterMatrix: quelques règles "riches" déjà présentes (DF/Scatter) qui restent prioritaires
        // Le dispatch ID -> fonction applique les gardes (GCD/LOS/DR/énergie/range)
        private delegate bool CounterFunc(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId);

        private static readonly Dictionary<int, List<int>> _counterMatrixIds = new Dictionary<int, List<int>>();

        // ============================================================================
        // EXACT VITALIC v.zip LOGIC - smethod_2 recreation
        // ============================================================================
        
        // BYPASS spells - return true directly without any checks (from smethod_2 line 856)
        private static readonly HashSet<int> VitalicBypass = new HashSet<int>()
        {
            31224,  // Cloak of Shadows
            74001,  // Combat Readiness  
            1856,   // Vanish
            36554,  // Shadowstep
            1766,   // Kick
            58984   // Shadowmeld/Feint
        };
        
        // Spells that require CanCast check (from hashSet_5)
        private static readonly HashSet<int> VitalicNeedsCanCast = new HashSet<int>()
        {
            6770,   // Sap
            8676    // Ambush
        };
        
        // Spells that require melee range (from hashSet_4)  
        private static readonly HashSet<int> VitalicNeedsMelee = new HashSet<int>()
        {
            1776,   // Gouge
            51722,  // Dismantle
            703,    // Garrote
            1833,   // Cheap Shot
            408,    // Kidney Shot
            5938    // Shiv
        };
        
        // Spells with no distance check (from hashSet_3)
        private static readonly HashSet<int> VitalicNoDistanceCheck = new HashSet<int>()
        {
            1766,   // Kick
            36554,  // Shadowstep  
            76577,  // Smoke Bomb
            58984   // Shadowmeld/Feint
        };
        
        // Max ranges for spells (from dictionary_5)
        private static readonly Dictionary<int, double> VitalicMaxRange = new Dictionary<int, double>()
        {
            { 2094, 15.0 },   // Blind
            { 36554, 25.0 },  // Shadowstep
            { 6770, 10.0 }    // Sap
        };

        // Deep Freeze diagnostics: track timing between CAST_SUCCESS and AURA_APPLIED per caster
        private static readonly Dictionary<ulong, DateTime> _dfCastSuccessAt = new Dictionary<ulong, DateTime>();
        private static readonly Dictionary<ulong, DateTime> _dfAuraAppliedAt = new Dictionary<ulong, DateTime>();
        
        // Exact copy of Vitalic smethod_2 logic
        private static bool CanCastLikeVitalic(int spellId, WoWUnit target)
        {
            try
            {
                // BYPASS check first - these spells always return true
                if (VitalicBypass.Contains(spellId))
                {
                    Trace(string.Format("[Vitalic Check] {0} bypass=true", spellId));
                    return true;
                }
                
                // Get spell object
                var spell = WoWSpell.FromId(spellId);
                if (spell == null) return false;
                
                // Distance checks
                if (!VitalicNoDistanceCheck.Contains(spellId))
                {
                    if (VitalicNeedsMelee.Contains(spellId))
                    {
                        // Check melee range (target.smethod_6(true) in Vitalic)
                        if (target == null || target.Distance > 5.0)
                        {
                            Trace(string.Format("[Vitalic Check] {0} reject: needs melee, dist={1:0.0}", spellId, target != null ? target.Distance : -1.0));
                            return false;
                        }
                    }
                    else if (VitalicMaxRange.ContainsKey(spellId))
                    {
                        if (target == null || target.Distance > VitalicMaxRange[spellId])
                        {
                            Trace(string.Format("[Vitalic Check] {0} reject: range {1:0.0}>{2:0.0}", spellId, target != null ? target.Distance : -1.0, VitalicMaxRange[spellId]));
                            return false;
                        }
                    }
                }
                
                // CanCast check only for Sap/Ambush
                if (VitalicNeedsCanCast.Contains(spellId))
                {
                    bool can = spell.CanCast;
                    Trace(string.Format("[Vitalic Check] {0} NeedsCanCast -> {1}", spellId, can));
                    return can;
                }
                
                // All other spells: no CanCast check
                Trace(string.Format("[Vitalic Check] {0} ok: no CanCast needed", spellId));
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        // EXACT VITALIC MATRIX - HashSet for Deep Freeze  
        private static readonly Dictionary<int, HashSet<int>> VitalicCounterMatrix = new Dictionary<int, HashSet<int>>()
        {
            // Deep Freeze - exact copy from Class141.cs dictionary_6
            { 44572, new HashSet<int> { 1776, 76577, 2094, 31224, 58984, 1766 } },
            
            // Keep other threats as-is for now
            { 19503, new HashSet<int> { 1776, 2094, 58984, 5938 } }  // Scatter Shot
        };

        
        // VITALIC-STYLE DIRECT CASTING - no delegates, no complex checks
        private static bool TryVitalicCounter(int threatSpellId, WoWUnit caster)
        {
            try
            {
                // Get counter spells for this threat
                HashSet<int> counterSpells;
                if (!VitalicCounterMatrix.TryGetValue(threatSpellId, out counterSpells))
                    return false;
                
                // Environment snapshot
                bool gcd = false; try { gcd = SpellManager.GlobalCooldown; } catch { }
                bool meDFFlag = false; try { meDFFlag = (Me != null && Me.HasAura(44572)); } catch { }
                double dist = caster != null ? caster.Distance : -1.0;
                Logger.Write(string.Format("[Vitalic Counter] Threat {0} from {1} - trying {2} counters (GCD={3}, MeHasDF={4}, dist={5:0.0})",
                    threatSpellId, caster != null ? caster.SafeName : "Unknown", counterSpells.Count, gcd, meDFFlag, dist));
                
                // Build iteration order
                IEnumerable<int> iterate;
                if (VitalicSettings.Instance.DiagnosticMode)
                {
                    // Prefer: Gouge > Blind > Smoke > Cloak > Shadowmeld > Kick
                    int[] prefer = new int[] { 1776, 2094, 76577, 31224, 58984, 1766 };
                    List<int> seq = new List<int>();
                    // For testing, skip Smoke Bomb to surface Blind pre-stun behavior
                    for (int i = 0; i < prefer.Length; i++)
                    {
                        int id = prefer[i];
                        if (id == 76577) continue; // remove Smoke in DiagnosticMode tests
                        if (counterSpells.Contains(id)) seq.Add(id);
                    }
                    // Append any remaining counters
                    foreach (int id in counterSpells)
                    {
                        bool exists = false;
                        for (int k = 0; k < seq.Count; k++) if (seq[k] == id) { exists = true; break; }
                        if (!exists && id != 76577) seq.Add(id);
                    }
                    iterate = seq;
                    Logger.Write("[Diag][DF] Using preferred counter order (Smoke excluded) for test.");
                }
                else
                {
                    // Non-diagnostic: prioritize realistic counters during stun (Cloak first)
                    int[] realisticOrder = new int[] { 31224, 58984, 1766, 1776, 2094, 76577 }; // Cloak > Shadowmeld > Kick > Gouge > Blind > Smoke
                    List<int> seq = new List<int>();
                    for (int i = 0; i < realisticOrder.Length; i++)
                    {
                        int id = realisticOrder[i];
                        if (counterSpells.Contains(id)) seq.Add(id);
                    }
                    // Append any remaining counters not in the preferred order
                    foreach (int id in counterSpells)
                    {
                        bool exists = false;
                        for (int k = 0; k < seq.Count; k++) if (seq[k] == id) { exists = true; break; }
                        if (!exists) seq.Add(id);
                    }
                    iterate = seq;
                }

                // Try each counter spell
                foreach (int spellId in iterate)
                {
                    Trace(string.Format("[Vitalic Counter] Consider {0} (targeted={1})", spellId, IsTargetedSpell(spellId)));
                    if (!CanCastLikeVitalic(spellId, caster))
                    {
                        Trace(string.Format("[Vitalic Counter] Spell {0} failed CanCastLikeVitalic check", spellId));
                        continue;
                    }
                    
                    // Cast directly - no more checks, just like Vitalic
                    bool success = false;
                    if (IsTargetedSpell(spellId))
                    {
                        if (caster == null)
                        {
                            Trace("[Vitalic Counter] Targeted spell but caster is null");
                        }
                        success = SpellManager.CastSpellById(spellId, caster);
                        if (success)
                            Logger.Write(string.Format("[Vitalic Counter] Cast {0} -> {1} (threat {2})", 
                                spellId, caster != null ? caster.SafeName : "Unknown", threatSpellId));
                    }
                    else
                    {
                        success = SpellManager.Cast(spellId);
                        if (success)
                            Logger.Write(string.Format("[Vitalic Counter] Cast {0} (self) (threat {1})", 
                                spellId, threatSpellId));
                    }
                    
                    if (success)
                    {
                        // Response timing
                        try
                        {
                            DateTime t0;
                            if (_dfCastSuccessAt.TryGetValue(caster != null ? caster.Guid : 0UL, out t0))
                            {
                                var dt = DateTime.UtcNow - t0;
                                Trace(string.Format("[Vitalic Counter] Response time since DF CAST_SUCCESS: {0} ms", (int)dt.TotalMilliseconds));
                            }
                        }
                        catch { }
                        return true;
                    }
                        
                    Trace(string.Format("[Vitalic Counter] SpellManager.Cast failed for {0}", spellId));
                }
                
                Logger.Write(string.Format("[Vitalic Counter] No counters succeeded for threat {0}", threatSpellId));
                return false;
            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("[Vitalic Counter] Exception: {0}", ex.Message));
                return false;
            }
        }
        
        private static bool IsTargetedSpell(int spellId)
        {
            // Spells that need a target
            return spellId == 1776   // Gouge
                || spellId == 2094   // Blind  
                || spellId == 1766   // Kick
                || spellId == 5938;  // Shiv
        }

        // === Emergency Layer Helper Methods ===
        
        // Emergency cast helper - bypasses GCD, facing, LoS, fires directly in event handler
        private static bool TryCastEmergency(int spellId, WoWUnit target = null)
        {
            try
            {
                var spell = WoWSpell.FromId(spellId);
                if (spell == null)
                    return false;

                // Only respect cooldown - bypass GCD, movement, facing, LoS
                if (spell.CooldownTimeLeft > TimeSpan.Zero)
                    return false;

                // Fire immediately – skip all heavy gates
                if (target != null && target.IsValid)
                {
                    SpellManager.CastSpellById(spellId, target);
                    Trace("[Emergency] Cast {0} on {1}", spell.Name, target.Name);
                }
                else
                {
                    SpellManager.CastSpellById(spellId);
                    Trace("[Emergency] Cast {0}", spell.Name);
                }

                return true;
            }
            catch (Exception ex)
            {
                Trace("[Emergency] Exception: {0}", ex.Message);
                return false;
            }
        }
        
        // Try to cast spell by ID with minimal checks (only GCD and cooldown)
        private static bool TryCastId(int spellId)
        {
            try
            {
                if (Me == null || !Me.IsValid)
                    return false;

                // Resolve spell by ID (HB API is keyed by name)
                var spell = WoWSpell.FromId(spellId);
                if (spell == null)
                    return false;
                if (spell.CooldownTimeLeft.TotalMilliseconds > 0)
                    return false;

                // Emergency layer: bypass most checks; still respect cooldown
                return SpellManager.Cast(spellId);
            }
            catch (Exception ex)
            {
                Trace("[Emergency] TryCastId failed for {0}: {1}", spellId, ex.Message);
                return false;
            }
        }

        // Try to cast spell by ID on target with minimal checks
        private static bool TryCastId(int spellId, ulong targetGuid)
        {
            try
            {
                if (Me == null || !Me.IsValid || targetGuid == 0UL)
                    return false;

                // Resolve spell by ID (HB API is keyed by name)
                var spell = WoWSpell.FromId(spellId);
                if (spell == null)
                    return false;
                if (spell.CooldownTimeLeft.TotalMilliseconds > 0)
                    return false;

                var target = ObjectManager.GetObjectByGuid<WoWUnit>(targetGuid);
                if (target == null || !target.IsValid)
                    return false;

                // For targeted spells like Shadowstep, use CastSpellById to guarantee it hits the target
                if (IsTargetedSpell(spellId))
                {
                    return SpellManager.CastSpellById(spellId, target);
                }
                else
                {
                    // For self-cast spells, ensure target context if needed
                    if (Me.CurrentTarget == null || Me.CurrentTarget.Guid != targetGuid)
                    {
                        try { target.Target(); } catch { }
                    }
                    return SpellManager.Cast(spellId);
                }
            }
            catch (Exception ex)
            {
                Trace("[Emergency] TryCastId with target failed for {0}: {1}", spellId, ex.Message);
                return false;
            }
        }

        // Top priority emergency reaction method
        // Returns true if handled and should short-circuit deeper logic
        private static bool TopPriorityEmergencyReact(string ev, int spellId, ulong srcGuid, ulong dstGuid)
        {
            try
            {
                // De-dupe check (100-120ms window)
                var now = DateTime.UtcNow;
                if ((now - _lastEmergencyTick).TotalMilliseconds < 120)
                    return false;

                if (Me == null || !Me.IsValid || !Me.GotTarget)
                    return false;

                // Blink/Shimmer detection from current target (caster is the mover)
                if (ev == "SPELL_CAST_SUCCESS" && srcGuid == Me.CurrentTarget.Guid)
                {
                    if (spellId == SpellId_Blink || spellId == SpellId_Shimmer)
                    {
                        _lastBlinkSeen = now;
                        
                        // Try Shadowstep first, then Sprint
                        var blinkTarget = ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid);
                        if (TryCastEmergency(SpellId_Shadowstep, blinkTarget))
                        {
                            _lastEmergencyTick = now;
                            return true;
                        }
                        else if (TryCastEmergency(SpellId_Sprint))
                        {
                            _lastEmergencyTick = now;
                            return true;
                        }
                    }
                }

                // Distance-jump heuristic as Blink fallback (if combat-log event missed)
                if (Me.CurrentTarget != null && Me.CurrentTarget.IsValid && !Me.CurrentTarget.IsDead)
                {
                    try
                    {
                        double currentDistance = Me.CurrentTarget.Distance;
                        // If target suddenly moved >15 yards in one tick, treat as Blink
                        if (currentDistance > 15.0 && (now - _lastBlinkSeen).TotalMilliseconds > 1000)
                        {
                            _lastBlinkSeen = now;
                            
                            // Try Shadowstep first, then Sprint
                            if (TryCastEmergency(SpellId_Shadowstep, Me.CurrentTarget))
                            {
                                _lastEmergencyTick = now;
                                return true;
                            }
                            else if (TryCastEmergency(SpellId_Sprint))
                            {
                                _lastEmergencyTick = now;
                                return true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace("[Emergency] Distance-jump heuristic exception: {0}", ex.Message);
                    }
                }

                // CC windows detection
                if (ev == "SPELL_CAST_SUCCESS" || ev == "SPELL_AURA_APPLIED")
                {
                    if (spellId == SpellId_DeepFreeze || spellId == SpellId_Polymorph || spellId == SpellId_PsychicScream)
                    {
                        // Try Cloak first, then Vanish
                        if (TryCastEmergency(SpellId_CloakOfShadows))
                        {
                            _lastEmergencyTick = now;
                            return true;
                        }
                        else if (TryCastEmergency(SpellId_Vanish))
                        {
                            _lastEmergencyTick = now;
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Trace("[Emergency] TopPriorityEmergencyReact exception: {0}", ex.Message);
                return false;
            }
        }

        // Handle loss of control events
        private static bool TopPriorityEmergencyReactLoC()
        {
            try
            {
                // De-dupe check (100-120ms window)
                var now = DateTime.UtcNow;
                if ((now - _lastEmergencyTick).TotalMilliseconds < 120)
                    return false;

                if (Me == null || !Me.IsValid)
                    return false;

                // Try Cloak first, then Vanish
                if (TryCastEmergency(SpellId_CloakOfShadows))
                {
                    _lastEmergencyTick = now;
                    return true;
                }
                else if (TryCastEmergency(SpellId_Vanish))
                {
                    _lastEmergencyTick = now;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Trace("[Emergency] TopPriorityEmergencyReactLoC exception: {0}", ex.Message);
                return false;
            }
        }

        // (Removed custom immediate escape layer to keep exact Vitalic behavior)

        // OLD COMPLEX LOGIC - TO BE REMOVED AFTER TESTING
        private static readonly Dictionary<int, CounterFunc> _counterDispatch = new Dictionary<int, CounterFunc>();

        private static readonly Dictionary<int, List<CounterFunc>> _counterMatrix = new Dictionary<int, List<CounterFunc>>()
        {
            // Deep Freeze (44572): EXACT VITALIC HASHSET - All counters attempt, but only those with smethod_2 bypass will succeed
            // Bypass spells: Cloak (31224), Feint (58984), Kick (1766) - others fail due to stun
            { 44572, new List<CounterFunc> { Counter_GougeCaster, Counter_SmokeBomb, Counter_BlindCaster, Counter_CloakSelf, Counter_FeintSelf, Counter_KickCaster } },

            // Scatter Shot (19503): Gouge d'abord, sinon Blind; à défaut Feint / Shiv
            { 19503, new List<CounterFunc> { Counter_GougeCaster, Counter_BlindCaster, Counter_FeintSelf, Counter_ShivCaster } },
        };

        // === Contres (implémentations locales) ===============================
        private static bool Counter_GougeCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) return false;
                if (!caster.Attackable) return false;
                // Special-case: right after Ice Block (45438) removal we don't require the target to be facing us.
                // Vitalic allows an immediate Gouge if we're in melee and able to cast.
                bool isIceBlockRelease = (threatSpellId == SpellId_IceBlock);

                if (!Throttle.Check("Counter.Gouge." + threatSpellId, 350)) return false;

                if (isIceBlockRelease)
                {
                    // Minimal gating for IB release: melee range, CanCast, and force our facing toward the target
                    bool melee = false; try { melee = SpellBook.InMeleeRange(caster, 6.0); } catch { }
                    if (!melee) return false;
                    bool canCast = false; try { canCast = SpellBook.CanCast(SpellBook.Gouge, caster); } catch { canCast = false; }
                    if (!canCast) return false;

                    try { if (!StyxWoW.Me.IsSafelyFacing(caster)) StyxWoW.Me.SetFacing(caster); } catch { }

                    bool cast = SpellBook.Cast(SpellBook.Gouge, caster);
                    if (cast)
                    {
                        Throttle.Mark("Counter.Gouge." + threatSpellId);
                        // DR track like original TryGouge would
                        try { DRTracker.Applied(caster, DRTracker.DrCategory.Incapacitate); } catch { }
                        Logger.Write("[Counter] Gouge -> {0} (threat {1})", caster.SafeName, threatSpellId);
                    }
                    else if (VitalicSettings.Instance.DiagnosticMode)
                    {
                        try
                        {
                            Logger.Write("[Diag][IceBlock][Gouge] melee={0} canCast={1} gcd={2} attackable={3}",
                                melee,
                                canCast,
                                SpellManager.GlobalCooldown,
                                (caster != null && caster.Attackable));
                        }
                        catch { }
                    }
                    return cast;
                }
                else
                {
                    // Default path: let CrowdControlManager apply full guards (DR/LOS/facing/hard-cast allowance)
                    bool ok = VitalicRotation.Managers.CrowdControlManager.TryGouge(caster);
                    if (ok)
                    {
                        Throttle.Mark("Counter.Gouge." + threatSpellId);
                        Logger.Write("[Counter] Gouge -> {0} (threat {1})", caster.SafeName, threatSpellId);
                    }
                    return ok;
                }
            }
            catch { return false; }
        }

        private static bool Counter_SmokeBomb(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                // Vitalic-like gating: rely on WoWSpell.CanCast and cooldown, avoid hard GCD gate here
                if (!Throttle.Check("Counter.Smoke." + threatSpellId, 1500)) return false;

                var spell = WoWSpell.FromId(SpellId_SmokeBomb);
                if (spell == null) return false;
                if (spell.CooldownTimeLeft.TotalMilliseconds > 0) return false;
                if (!spell.CanCast) return false;

                // Cast
                SpellManager.Cast(SpellId_SmokeBomb);

                Throttle.Mark("Counter.Smoke." + threatSpellId);
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Counter] Smoke Bomb vs threat " + threatSpellId);
                return true;
            }
            catch { return false; }
        }

        private static bool Counter_BlindCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead)
                {
                    Trace("[Diag][Blind] Skip: caster invalid/dead (threat {0})", threatSpellId);
                    return false;
                }
                if (!caster.Attackable)
                {
                    Trace("[Diag][Blind] Skip: caster not attackable (threat {0})", threatSpellId);
                    return false;
                }
                if (!Throttle.Check("Counter.Blind." + threatSpellId, 800))
                {
                    Trace("[Diag][Blind] Skip: throttle active (threat {0})", threatSpellId);
                    return false;
                }
                // Manual = permet l'usage en peel sans exiger un hard-cast
                bool ok = VitalicRotation.Managers.CrowdControlManager.TryBlindManual(caster);
                if (ok)
                {
                    Throttle.Mark("Counter.Blind." + threatSpellId);
                    Logger.Write("[Counter] Blind -> {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                else
                {
                    Trace("[Diag][Blind] TryBlindManual returned false vs {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                return ok;
            }
            catch { return false; }
        }

        private static bool Counter_CloakSelf(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                // Vitalic behavior: Cloak can be cast even when stunned! No CanCast check for Cloak.
                if (SpellManager.GlobalCooldownLeft.TotalMilliseconds > 0) 
                {
                    Trace("[Diag][Cloak] Skip: GCD active (threat {0})", threatSpellId); 
                    return false; 
                }
                
                // Check cooldown only
                var spell = WoWSpell.FromId(SpellBook.CloakOfShadows);
                if (spell != null && spell.CooldownTimeLeft.TotalMilliseconds > 0)
                {
                    Trace("[Diag][Cloak] Skip: on cooldown {0}ms (threat {1})", (int)spell.CooldownTimeLeft.TotalMilliseconds, threatSpellId);
                    return false;
                }
                
                if (!Throttle.Check("Counter.Cloak." + threatSpellId, 600)) 
                { 
                    Trace("[Diag][Cloak] Skip: throttle active (threat {0})", threatSpellId); 
                    return false; 
                }
                
                // Cast directly without CanCast check - this is the Vitalic way!
                bool cast = SpellManager.Cast(SpellBook.CloakOfShadows);
                if (cast)
                {
                    Throttle.Mark("Counter.Cloak." + threatSpellId);
                    Logger.Write("[Counter] Cloak of Shadows (threat {0})", threatSpellId);
                }
                else 
                {
                    Trace("[Diag][Cloak] Cast returned false (threat {0})", threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_FeintSelf(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                // Vitalic smethod_2: Feint (58984) bypasses normal CanCast checks
                if (SpellManager.GlobalCooldown) { Trace("[Diag][Feint] Skip: GCD active (threat {0})", threatSpellId); return false; }
                var feintSpell = WoWSpell.FromId(58984);  // Feint spell ID
                if (feintSpell == null || feintSpell.Cooldown) { Trace("[Diag][Feint] Skip: cooldown active (threat {0})", threatSpellId); return false; }
                if (!Throttle.Check("Counter.Feint." + threatSpellId, 600)) { Trace("[Diag][Feint] Skip: throttle active (threat {0})", threatSpellId); return false; }
                
                bool cast = SpellBook.Cast(SpellBook.Feint);
                if (cast)
                {
                    Throttle.Mark("Counter.Feint." + threatSpellId);
                    Logger.Write("[Counter] Feint (bypassed stun checks, threat {0})", threatSpellId);
                }
                else Trace("[Diag][Feint] Cast returned false (threat {0})", threatSpellId);
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_ShivCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) return false;
                if (!caster.Attackable) return false;
                bool inMelee = false; try { inMelee = SpellBook.InMeleeRange(caster, 6.0); } catch { }
                if (!inMelee) return false;
                if (!SpellBook.CanCast(SpellBook.Shiv, caster)) return false;
                if (!Throttle.Check("Counter.Shiv." + threatSpellId, 800)) return false;
                bool cast = SpellBook.Cast(SpellBook.Shiv, caster);
                if (cast)
                {
                    Throttle.Mark("Counter.Shiv." + threatSpellId);
                    Logger.Write("[Counter] Shiv -> {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        // === Contres additionnels (pour dispatch ID→fonction) ================
        private static bool Counter_KickCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) { Trace("[Diag][Kick] Skip: caster invalid/dead (threat {0})", threatSpellId); return false; }
                if (!caster.Attackable) { Trace("[Diag][Kick] Skip: not attackable (threat {0})", threatSpellId); return false; }
                
                // Vitalic smethod_2: Kick (1766) bypasses normal CanCast checks
                if (SpellManager.GlobalCooldown) { Trace("[Diag][Kick] Skip: GCD active (threat {0})", threatSpellId); return false; }
                var kickSpell = WoWSpell.FromId(1766);  // Kick spell ID
                if (kickSpell == null || kickSpell.Cooldown) { Trace("[Diag][Kick] Skip: cooldown active (threat {0})", threatSpellId); return false; }
                if (!Throttle.Check("Counter.Kick." + threatSpellId, 300)) { Trace("[Diag][Kick] Skip: throttle active (threat {0})", threatSpellId); return false; }
                
                bool cast = SpellBook.Cast(SpellBook.Kick, caster);
                if (cast)
                {
                    Throttle.Mark("Counter.Kick." + threatSpellId);
                    Logger.Write("[Counter] Kick -> {0} (bypassed stun checks, threat {1})", caster.SafeName, threatSpellId);
                }
                else Trace("[Diag][Kick] Cast returned false vs {0} (threat {1})", caster.SafeName, threatSpellId);
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_DismantleCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) return false;
                if (!caster.Attackable) return false;
                bool melee = false; try { melee = SpellBook.InMeleeRange(caster, 5.0); } catch { }
                if (!melee) return false;
                if (!SpellBook.CanCast(SpellBook.Dismantle, caster)) return false;
                if (!Throttle.Check("Counter.Dismantle." + threatSpellId, 800)) return false;
                bool cast = SpellBook.Cast(SpellBook.Dismantle, caster);
                if (cast)
                {
                    Throttle.Mark("Counter.Dismantle." + threatSpellId);
                    Logger.Write("[Counter] Dismantle -> {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_EvasionSelf(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (!SpellBook.CanCast(SpellBook.Evasion)) return false;
                if (!Throttle.Check("Counter.Evasion." + threatSpellId, 800)) return false;
                bool cast = SpellBook.Cast(SpellBook.Evasion);
                if (cast)
                {
                    Throttle.Mark("Counter.Evasion." + threatSpellId);
                    Logger.Write("[Counter] Evasion (threat {0})", threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_VanishSelf(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (!SpellBook.CanCast(SpellBook.Vanish)) return false;
                if (!Throttle.Check("Counter.Vanish." + threatSpellId, 1200)) return false;
                bool cast = SpellBook.Cast(SpellBook.Vanish);
                if (cast)
                {
                    Throttle.Mark("Counter.Vanish." + threatSpellId);
                    Logger.Write("[Counter] Vanish (threat {0})", threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_ShadowmeldSelf(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            const int ShadowmeldId = 58984;
            try
            {
                if (!SpellBook.CanCast(ShadowmeldId)) return false;
                if (!Throttle.Check("Counter.Shadowmeld." + threatSpellId, 1500)) return false;
                bool cast = SpellBook.Cast(ShadowmeldId);
                if (cast)
                {
                    Throttle.Mark("Counter.Shadowmeld." + threatSpellId);
                    Logger.Write("[Counter] Shadowmeld (threat {0})", threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        // Added back clean implementations for counters referenced in dispatch
        private static bool Counter_CombatReadinessSelf(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (!SpellBook.CanCast(SpellBook.CombatReadiness)) return false;
                if (!Throttle.Check("Counter.CombatReadiness." + threatSpellId, 1200)) return false;
                bool cast = SpellBook.Cast(SpellBook.CombatReadiness);
                if (cast)
                {
                    Throttle.Mark("Counter.CombatReadiness." + threatSpellId);
                    Logger.Write("[Counter] Combat Readiness (threat {0})", threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_KidneyCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) return false;
                if (!caster.Attackable) return false;
                if (!Throttle.Check("Counter.Kidney." + threatSpellId, 800)) return false;
                bool ok = VitalicRotation.Managers.CrowdControlManager.TryKidney(caster);
                if (ok)
                {
                    Throttle.Mark("Counter.Kidney." + threatSpellId);
                    Logger.Write("[Counter] Kidney -> {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                return ok;
            }
            catch { return false; }
        }

        private static bool Counter_CheapShotCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) return false;
                if (!caster.Attackable) return false;
                bool melee = false; try { melee = SpellBook.InMeleeRange(caster, 5.0); } catch { }
                if (!melee) return false;
                if (!SpellBook.CanCast(SpellBook.CheapShot, caster)) return false;
                if (!Throttle.Check("Counter.Cheap." + threatSpellId, 800)) return false;
                bool cast = SpellBook.Cast(SpellBook.CheapShot, caster);
                if (cast)
                {
                    Throttle.Mark("Counter.Cheap." + threatSpellId);
                    Logger.Write("[Counter] Cheap Shot -> {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_GarroteCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) return false;
                if (!caster.Attackable) return false;
                bool melee = false; try { melee = SpellBook.InMeleeRange(caster, 5.0); } catch { }
                if (!melee) return false;
                if (!SpellBook.CanCast(SpellBook.Garrote, caster)) return false;
                if (!Throttle.Check("Counter.Garrote." + threatSpellId, 800)) return false;
                bool cast = SpellBook.Cast(SpellBook.Garrote, caster);
                if (cast)
                {
                    Throttle.Mark("Counter.Garrote." + threatSpellId);
                    Logger.Write("[Counter] Garrote -> {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_RuptureCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) return false;
                if (!caster.Attackable) return false;
                bool melee = false; try { melee = SpellBook.InMeleeRange(caster, 5.0); } catch { }
                if (!melee) return false;
                if (StyxWoW.Me.ComboPoints <= 0) return false;
                if (!SpellBook.CanCast(SpellBook.Rupture, caster)) return false;
                if (!Throttle.Check("Counter.Rupture." + threatSpellId, 1000)) return false;
                bool cast = SpellBook.Cast(SpellBook.Rupture, caster);
                if (cast)
                {
                    Throttle.Mark("Counter.Rupture." + threatSpellId);
                    Logger.Write("[Counter] Rupture -> {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_SapCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) return false;
                if (caster.Combat) return false; // Sap seulement hors combat
                if (!SpellBook.CanCast(SpellBook.Sap, caster)) return false;
                if (!Throttle.Check("Counter.Sap." + threatSpellId, 1200)) return false;
                bool cast = SpellBook.Cast(SpellBook.Sap, caster);
                if (cast)
                {
                    Throttle.Mark("Counter.Sap." + threatSpellId);
                    Logger.Write("[Counter] Sap -> {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        private static bool Counter_ShadowstepCaster(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (caster == null || !caster.IsValid || caster.IsDead) return false;
                if (!caster.Attackable) return false;
                if (!Throttle.Check("Counter.Step." + threatSpellId, 800)) return false;
                bool ok = false;
                try { MobilityManager.TryShadowstepSafe(caster); ok = true; } catch { ok = false; }
                if (ok)
                {
                    Throttle.Mark("Counter.Step." + threatSpellId);
                    Logger.Write("[Counter] Shadowstep -> {0} (threat {1})", caster.SafeName, threatSpellId);
                }
                return ok;
            }
            catch { return false; }
        }

        private static bool Counter_RecuperateSelf(WoWUnit caster, WoWUnit dest, string ev, int threatSpellId)
        {
            try
            {
                if (!SpellBook.CanCast(SpellBook.Recuperate)) return false;
                if (!Throttle.Check("Counter.Recup." + threatSpellId, 1500)) return false;
                bool cast = SpellBook.Cast(SpellBook.Recuperate);
                if (cast)
                {
                    Throttle.Mark("Counter.Recup." + threatSpellId);
                    Logger.Write("[Counter] Recuperate (threat {0})", threatSpellId);
                }
                return cast;
            }
            catch { return false; }
        }

        // === Initialisation du dispatch et de la table exhaustive ============
        static EventHandlers()
        {
            try
            {
                // Dispatch: ID de contre -> fonction
                _counterDispatch[1776]  = Counter_GougeCaster;       // Gouge
                _counterDispatch[76577] = Counter_SmokeBomb;          // Smoke Bomb
                _counterDispatch[2094]  = Counter_BlindCaster;        // Blind
                _counterDispatch[31224] = Counter_CloakSelf;          // Cloak of Shadows
                _counterDispatch[58984] = Counter_ShadowmeldSelf;     // Shadowmeld
                _counterDispatch[1966]  = Counter_FeintSelf;          // Feint
                _counterDispatch[1766]  = Counter_KickCaster;         // Kick
                _counterDispatch[51722] = Counter_DismantleCaster;    // Dismantle
                _counterDispatch[5277]  = Counter_EvasionSelf;        // Evasion
                _counterDispatch[36554] = Counter_ShadowstepCaster;   // Shadowstep
                _counterDispatch[408]   = Counter_KidneyCaster;       // Kidney
                _counterDispatch[1833]  = Counter_CheapShotCaster;    // Cheap Shot
                _counterDispatch[703]   = Counter_GarroteCaster;      // Garrote
                _counterDispatch[5938]  = Counter_ShivCaster;         // Shiv
                _counterDispatch[73651] = Counter_RecuperateSelf;     // Recuperate
                _counterDispatch[74001] = Counter_CombatReadinessSelf;// Combat Readiness
                _counterDispatch[1856]  = Counter_VanishSelf;         // Vanish
                _counterDispatch[6770]  = Counter_SapCaster;          // Sap (OOC)

                BuildVitalicCounterMatrix();
            }
            catch { }
        }

        private static void AddMap(int triggerId, params int[] counters)
        {
            List<int> list;
            if (!_counterMatrixIds.TryGetValue(triggerId, out list))
            {
                list = new List<int>();
                _counterMatrixIds[triggerId] = list;
            }
            for (int i = 0; i < counters.Length; i++) list.Add(counters[i]);
        }

        // Remplit _counterMatrixIds avec la table fournie (extraits clés + majorité Cloak)
        private static void BuildVitalicCounterMatrix()
        {
            // IMPORTANT: Nous reprenons l'ordre exact communiqué
            AddMap(17,    31224);
            AddMap(53,    31224);
            AddMap(66,    31224, 76577);
            AddMap(69,    31224);
            AddMap(71,    31224);
            AddMap(78,    31224);
            AddMap(79,    31224);
            AddMap(99,    31224);
            AddMap(100,   31224);
            AddMap(101,   31224);
            AddMap(102,   31224);
            AddMap(104,   31224);
            AddMap(113,   31224);
            AddMap(116,   31224);
            AddMap(118,   31224, 1766, 2094, 76577, 58984);
            AddMap(120,   31224);
            AddMap(122,   31224);
            AddMap(133,   31224);
            AddMap(136,   31224);
            AddMap(339,   31224);
            AddMap(348,   31224);
            AddMap(403,   31224);
            AddMap(408,   51722);
            AddMap(421,   31224);
            AddMap(475,   31224);
            AddMap(498,   31224);
            AddMap(527,   31224);
            AddMap(546,   31224);
            AddMap(561,   31224);
            AddMap(585,   31224);
            AddMap(588,   31224);
            AddMap(589,   31224);
            AddMap(603,   31224);
            AddMap(686,   31224);
            AddMap(688,   31224);
            AddMap(692,   31224);
            AddMap(699,   31224);
            AddMap(703,   51722);
            AddMap(710,   31224);
            AddMap(740,   31224);
            AddMap(770,   31224);
            AddMap(774,   31224);
            AddMap(871,   31224);
            AddMap(980,   31224);
            AddMap(1120,  31224);
            AddMap(1459,  31224);
            AddMap(1463,  31224);
            AddMap(1719,  51722);
            AddMap(1766,  5277);
            AddMap(1776,  5277);
            AddMap(1784,  31224);
            AddMap(1833,  5277);
            AddMap(1850,  31224);
            AddMap(1856,  31224);
            AddMap(1943,  51722);
            AddMap(1978,  31224);
            AddMap(2048,  31224);
            AddMap(2094,  5277);
            AddMap(2139,  31224);
            AddMap(2484,  31224);
            AddMap(2825,  31224);
            AddMap(28730, 31224);
            AddMap(2894,  31224);
            AddMap(2944,  31224);
            AddMap(2983,  31224);
            AddMap(3045,  51722);
            AddMap(3111,  31224);
            AddMap(33891, 31224);
            AddMap(34477, 31224);
            AddMap(34861, 31224);
            AddMap(34914, 31224);
            // Deep Freeze: Gouge > Smoke Bomb > Blind > Cloak > Shadowmeld > Kick
            AddMap(44572, 1776, 76577, 2094, 31224, 58984, 1766);
            // Ice Block (45438) — ne réagir qu'à la disparition (géré plus bas)
            AddMap(45438, 703, 1776, 5938);
            AddMap(46968, 5277);
            AddMap(47476, 31224);
            AddMap(48045, 31224);
            AddMap(48181, 31224);
            AddMap(48265, 31224);
            AddMap(48438, 31224);
            AddMap(48707, 31224);
            AddMap(48792, 31224);
            AddMap(49143, 31224);
            AddMap(49576, 31224);
            AddMap(49868, 31224);
            AddMap(51490, 31224);
            AddMap(51514, 31224, 1766, 76577);
            AddMap(51714, 31224);
            AddMap(51730, 31224);
            AddMap(51735, 31224);
            AddMap(5211,  5277);
            AddMap(5215,  31224);
            AddMap(5229,  31224);
            AddMap(53271, 31224);
            AddMap(53385, 31224);
            AddMap(53563, 31224);
            AddMap(53600, 31224);
            AddMap(5484,  31224, 1776, 1766);
            AddMap(5487,  31224);
            AddMap(55709, 31224);
            AddMap(5697,  31224);
            AddMap(57934, 31224);
            AddMap(5782,  31224, 1766, 2094, 76577, 58984);
            AddMap(586,   31224);
            AddMap(59052, 31224);
            AddMap(6143,  31224);
            AddMap(6343,  31224);
            AddMap(63560, 31224);
            AddMap(64044, 31224, 1766, 2094, 76577, 58984);
            AddMap(64382, 31224);
            AddMap(64843, 31224);
            AddMap(710,   31224);
            AddMap(73651, 31224);
            AddMap(76577, 31224);
            AddMap(78675, 31224);
            AddMap(80353, 31224);
            AddMap(82676, 31224, 76577);
            AddMap(82691, 31224);
            AddMap(853,   31224, 31224, 31224);
            AddMap(8983,  5277);
            AddMap(1022,  31224);
            AddMap(1044,  31224);
            AddMap(12042, 31224);
            AddMap(12043, 31224);
            AddMap(12051, 31224);
            AddMap(12472, 31224);
            AddMap(12654, 31224);
            AddMap(12292, 51722);
            AddMap(22703, 31224);
            AddMap(23920, 31224);
            AddMap(30283, 5277);
            AddMap(33395, 31224);
            AddMap(3355,  31224, 31224);
            AddMap(33702, 31224);
            AddMap(33745, 31224);
            AddMap(33763, 31224);
            AddMap(33786, 31224);
            AddMap(34433, 31224);
            AddMap(34914, 31224);
            AddMap(36554, 31224);
            AddMap(46924, 5277);
            AddMap(48181, 31224);
            AddMap(48265, 31224);
            AddMap(51722, 31224);
            AddMap(58984, 31224);
            AddMap(61295, 31224);
            AddMap(774,   31224);
            AddMap(8122,  31224, 1776, 1766);
            AddMap(82726, 31224);
        }

        private static bool TryCounterMatrix(string ev, int spellId, ulong srcGuid, ulong dstGuid)
        {
            try
            {
                // Résoudre caster/dest (ennemi/allié) si possible
                WoWUnit caster = null, dest = null;
                try { caster = ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid); } catch { caster = null; }
                try { dest = ObjectManager.GetObjectByGuid<WoWUnit>(dstGuid); } catch { dest = null; }

                Trace("[Diag][CounterMatrix] ev={0} spellId={1} src={2:X} dst={3:X} casterOk={4} destOk={5}", ev, spellId, srcGuid, dstGuid, caster != null && caster.IsValid, dest != null && dest.IsValid);

                // Feedback visuel minimal par menace
                try
                {
                    if (spellId == 44572)
                    {
                        if (VitalicSettings.Instance.SpellAlertsEnabled) VitalicUi.ShowBigBanner("Deep Freeze!");
                        // Pas de son ici: il sera joué uniquement quand un contre est effectivement lancé
                    }
                    else if (spellId == 19503)
                    {
                        if (VitalicSettings.Instance.SpellAlertsEnabled) VitalicUi.ShowNotify("Scatter Shot");
                    }
                }
                catch { }

                // 1) Règles riches spécifiques si présentes
                List<CounterFunc> rich;
                if (_counterMatrix.TryGetValue(spellId, out rich) && rich != null && rich.Count > 0)
                {
                    for (int i = 0; i < rich.Count; i++)
                    {
                        var fn = rich[i];
                        bool done = false;
                        try {
                            Trace("[Diag][CounterMatrix] rich[{0}] -> {1}", i, fn.Method.Name);
                            done = fn(caster, dest, ev, spellId);
                        } catch { done = false; }
                        if (done) return true;
                    }
                }

                // 2) Table exhaustive ID→contres (parité Vitalic)
                List<int> counters;
                if (_counterMatrixIds.TryGetValue(spellId, out counters) && counters != null && counters.Count > 0)
                {
                    // Miroir Vitalic: 45438 (Ice Block) et 19263 (Deterrence) ne déclenchent les contres qu'à la fin de l'aura.
                    if ((spellId == 45438 || spellId == 19263) && ev != "SPELL_AURA_REMOVED")
                        return false;

                    // Diagnostic: trace des tentatives de contre pour Ice Block
                    if (VitalicSettings.Instance.DiagnosticMode && spellId == 45438 && ev == "SPELL_AURA_REMOVED")
                    {
                        Logger.Write("[Diag][IceBlock] Removal detected -> evaluating {0} counters", counters.Count);
                    }
                    for (int i = 0; i < counters.Count; i++)
                    {
                        int cid = counters[i];
                        CounterFunc fn;
                        if (_counterDispatch.TryGetValue(cid, out fn) && fn != null)
                        {
                            bool done = false;
                            try 
                            { 
                                if (VitalicSettings.Instance.DiagnosticMode && spellId == 45438 && ev == "SPELL_AURA_REMOVED")
                                    Logger.Write("[Diag][IceBlock] Try counter id {0}", cid);
                                Trace("[Diag][CounterMatrix] try id={0} fn={1}", cid, fn.Method.Name);

                                done = fn(caster, dest, ev, spellId); 

                                if (VitalicSettings.Instance.DiagnosticMode && spellId == 45438 && ev == "SPELL_AURA_REMOVED")
                                    Logger.Write(done ? "[Diag][IceBlock] Counter {0} succeeded" : "[Diag][IceBlock] Counter {0} failed", cid);
                                Trace("[Diag][CounterMatrix] id={0} -> {1}", cid, done ? "SUCCESS" : "FAIL");
                            } 
                            catch 
                            { 
                                done = false; 
                                if (VitalicSettings.Instance.DiagnosticMode && spellId == 45438 && ev == "SPELL_AURA_REMOVED")
                                    Logger.Write("[Diag][IceBlock] Counter {0} threw exception", cid);
                                Trace("[Diag][CounterMatrix] id={0} -> EXCEPTION", cid);
                            }
                            if (done) return true;
                        }
                        else Trace("[Diag][CounterMatrix] dispatch miss for id={0}", cid);
                    }
                }
            }
            catch { }
            return false;
        }

        // === DEBUG: Deep Freeze -> blind direct (bypass matrix) ============
    private static void DebugForceBlindOnDeepFreeze(string ev, ulong srcGuid, ulong dstGuid)
        {
            try
            {
                if (!VitalicSettings.Instance.DiagnosticMode) return;
        // Only attempt at the earliest meaningful hook: CAST_SUCCESS
        if (ev != "SPELL_CAST_SUCCESS") { try { Logger.Write("[DF-DEBUG] Blind: skip on {0}", ev); } catch { } return; }

                // Resolve caster (mage) from srcGuid
                WoWUnit caster = null;
                try { caster = ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid); } catch { caster = null; }
                if (caster == null || !caster.IsValid)
                {
                    try { Logger.Write("[DF-DEBUG] Blind: skip, caster unresolved (ev={0})", ev); } catch { }
                    return;
                }

                if (!Throttle.Check("DF.DebugBlind." + srcGuid.ToString("X"), 400))
                {
                    try { Logger.Write("[DF-DEBUG] Blind: throttled (ev={0})", ev); } catch { }
                    return;
                }

                // Cooldown via Lua query (more faithful)
                double cdSec = 0.0;
                try { cdSec = SpellBook.GetSpellCooldown(SpellBook.Blind); } catch { cdSec = 0.0; }
                if (cdSec > 0.0)
                {
                    try { Logger.Write("[DF-DEBUG] Blind: cooldown {0} ms", (int)(cdSec * 1000)); } catch { }
                    return;
                }

                // Start log
                try 
                {
                    double d = 0; bool los = true; bool gcd = false; bool stunned = false; bool auraApplied = false;
                    try { d = StyxWoW.Me.Location.Distance(caster.Location); } catch { }
                    try { los = caster.InLineOfSpellSight; } catch { }
                    try { gcd = SpellManager.GlobalCooldown; } catch { }
                    // Aura/stun hints (best-effort): if this is CAST_SUCCESS, aura likely not applied yet, but log placeholders
                    try { auraApplied = false; } catch { }
                    Logger.Write("[DF-DEBUG] Blind: start -> {0} dist={1:0.0} los={2} gcd={3} stunned={4} auraApplied={5}", caster.SafeName, d, los, gcd, stunned, auraApplied);
                } catch { }

                // Force target and facing then try multiple cast paths quickly
                WoWUnit prevTarget = null;
                try { prevTarget = StyxWoW.Me.CurrentTarget; } catch { prevTarget = null; }
                try { if (prevTarget == null || prevTarget.Guid != caster.Guid) caster.Target(); } catch { }
                try { if (!StyxWoW.Me.IsSafelyFacing(caster)) StyxWoW.Me.SetFacing(caster); } catch { }

                bool ok = false;
                try
                {
                    // 1) Try SpellManager by name first (can sometimes queue faster)
                    var s = WoWSpell.FromId(SpellBook.Blind);
                    if (s != null)
                    {
                        ok = SpellManager.Cast(s.Name);
                    }
                }
                catch { ok = false; }

                if (!ok)
                {
                    try
                    {
                        // 2) Lua CastSpellByID on target
                        Lua.DoString("CastSpellByID(" + SpellBook.Blind + ", 'target')");
                        bool gcd2 = false; try { gcd2 = SpellManager.GlobalCooldown; } catch { gcd2 = false; }
                        ok = gcd2 || StyxWoW.Me.IsCasting;
                    }
                    catch { ok = false; }
                }

                if (!ok)
                {
                    try
                    {
                        // 3) Macro fallback
                        Lua.DoString("RunMacroText('/cast [@target,harm,nodead] Blind')");
                        bool gcd3 = false; try { gcd3 = SpellManager.GlobalCooldown; } catch { gcd3 = false; }
                        ok = gcd3 || StyxWoW.Me.IsCasting;
                    }
                    catch { ok = false; }
                }

                try 
                { 
                    Logger.Write(ok ? "[DF-DEBUG] Blind: cast requested/success" : "[DF-DEBUG] Blind: cast did not start"); 
                    if (ok && VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent();
                } catch { }
            }
            catch { }
        }

        private static bool _enabled = true;
        public static bool Enabled 
        { 
            get { return _enabled; } 
            set { _enabled = value; } 
        }

        // === AJOUTS EN HAUT DE LA CLASSE (champs privés) ===
        private static bool _luaHooksAttached;
        private static DateTime _lastQueueAction = DateTime.MinValue;
        private static DateTime _lastAfkTick = DateTime.MinValue;

        // Interrupts (Kick)
        private static DateTime _lastKickAttempt = DateTime.MinValue;

        // Feedback interrupt (anti-spam)
        private static DateTime _lastInterruptNotify = DateTime.MinValue;
        private static bool NotifyThrottle(int ms)
        {
            var now = DateTime.UtcNow;
            if ((now - _lastInterruptNotify).TotalMilliseconds < ms) return false;
            _lastInterruptNotify = now;
            return true;
        }

        // Throttle pour alertes CC
        private static DateTime _lastCcNotify = DateTime.MinValue;
        private static bool CcNotifyThrottle(int ms)
        {
            var now = DateTime.UtcNow;
            if ((now - _lastCcNotify).TotalMilliseconds < ms) return false;
            _lastCcNotify = now;
            return true;
        }

        // Dispatcher générique pour router les événements Lua existants vers OnLuaEvent
        private static void LuaDispatcher(object sender, LuaEventArgs e)
        {
            try { OnLuaEvent(e.EventName, e.Args); }
            catch { }
        }

        // Anti-AFK & autotarget throttles
        private static DateTime _lastAntiAfk = DateTime.MinValue;
        private static DateTime _lastAutoTarget = DateTime.MinValue;
        private static DateTime _lastStatusPosSave = DateTime.MinValue;

    // Fallback DF aura polling (in case COMBAT_LOG doesn't deliver the event)
    private static bool _dfAuraWasActive = false;
    private static DateTime _lastDfPoll = DateTime.MinValue;

        // Deep Freeze de-duplication: avoid double-processing on CAST_SUCCESS and then AURA_APPLIED
        private static readonly Dictionary<ulong, DateTime> _lastDfHandledByCaster = new Dictionary<ulong, DateTime>();
        private static bool ShouldHandleDeepFreeze(string ev, ulong srcGuid)
        {
            // Prefer handling on CAST_SUCCESS. Allow AURA_APPLIED only if we didn't handle very recently.
            if (ev == "SPELL_CAST_SUCCESS") return true;
            if (ev == "SPELL_AURA_APPLIED")
            {
                DateTime when;
                if (_lastDfHandledByCaster.TryGetValue(srcGuid, out when))
                {
                    // Suppress if we handled within the last 600 ms
                    return (DateTime.UtcNow - when).TotalMilliseconds > 20.0;
                }
                return true;
            }
            // CAST_START is rare for DF (instant), but allow if nothing recently
            if (ev == "SPELL_CAST_START")
            {
                DateTime when;
                if (_lastDfHandledByCaster.TryGetValue(srcGuid, out when))
                {
                    return (DateTime.UtcNow - when).TotalMilliseconds > 20.0;
                }
                return true;
            }
            return false;
        }

        // Removed Gouge-on-IceBlock delayed window for strict Vitalic parity

    // Pending Cloak retry removed for strict Vitalic parity

    // Ice Block polling removed; rely solely on combat log events

        // Manual cast pause (nouvelle implémentation: armé uniquement par input humain)
        private static DateTime _pauseOffenseUntil = DateTime.MinValue;
        internal static void ArmPauseOffense(int ms)
        {
            if (ms <= 0) return;
            if (ms > 300) ms = 300; // cap Vitalic
            _pauseOffenseUntil = DateTime.UtcNow.AddMilliseconds(ms);
        }
        public static bool ShouldPauseOffense()
        {
            int ms = 0; try { ms = VitalicSettings.Instance.ManualCastPause; } catch { ms = 0; }
            if (ms <= 0) return false;
            return DateTime.UtcNow < _pauseOffenseUntil;
        }

        // (legacy retained but unused now)
        private static DateTime _manualLastCastUtc = DateTime.MinValue;

        // === Blacklist (v.zip) ================================================
        private static readonly HashSet<int> _eventIdBlacklist = new HashSet<int>();

        // === Détection totems / pièges (id MoP) ===============================
        private static readonly HashSet<int> _totemSpellIds = new HashSet<int>
        {
            // Totems de base MoP
            8177,   // Grounding Totem
            5394,   // Healing Stream Totem
            108273, // Windwalk Totem
            98008,  // Spirit Link Totem
            8143,   // Tremor Totem
            16190,  // Mana Tide Totem
            108269, // Capacitor Totem
            108270, // Stone Bulwark Totem
            2894,   // Fire Elemental Totem
            2062,   // Earth Elemental Totem
            3599,   // Searing Totem
            8190,   // Magma Totem
        };

        private static readonly HashSet<int> _trapSpellIds = new HashSet<int>
        {
            3355,   // Freezing Trap (création)
            13813,  // Explosive Trap
            13812,  // Explosive Trap (spell alt)
            13809,  // Frost Trap
            82941,  // Ice Trap (MoP)
        };

        // AutoFace (v.zip)
        private static WoWUnit _autoFaceUnit;
        private static DateTime _autoFaceUntil;

        // --- READY / ROLE notifications (texte uniquement) ---
        private static bool _uiNotifHooked;

        // === Diminishing Returns mapping (MoP 5.4.8-ish) ======================
        private static readonly Dictionary<int, DRTracker.DrCategory> DrSpells =
            new Dictionary<int, DRTracker.DrCategory>()
        {
            // ---- STUNS ----
            { 408,    DRTracker.DrCategory.Stun },      // Kidney Shot
            { 1833,   DRTracker.DrCategory.Stun },      // Cheap Shot
            { 44572,  DRTracker.DrCategory.Stun },      // Deep Freeze (Mage)
            { 5211,   DRTracker.DrCategory.Stun },      // Mighty Bash
            { 22570,  DRTracker.DrCategory.Stun },      // Maim
            { 853,    DRTracker.DrCategory.Stun },      // Hammer of Justice
            { 30283,  DRTracker.DrCategory.Stun },      // Shadowfury
            { 46968,  DRTracker.DrCategory.Stun },      // Shockwave
            { 107570, DRTracker.DrCategory.Stun },      // Storm Bolt
            { 19577,  DRTracker.DrCategory.Stun },      // Intimidation (pet)
            { 105593, DRTracker.DrCategory.Stun },      // Fist of Justice

            // ---- INCAPACITATES ----
            { 3355,   DRTracker.DrCategory.Incapacitate }, // Freezing Trap
            { 115078, DRTracker.DrCategory.Incapacitate }, // Paralysis
            { 20066,  DRTracker.DrCategory.Incapacitate }, // Repentance
            { 19386,  DRTracker.DrCategory.Incapacitate }, // Wyvern Sting

            // ---- DISORIENTS ----
            { 6770,   DRTracker.DrCategory.Disorient }, // Sap (v.zip)
            { 2094,   DRTracker.DrCategory.Disorient }, // Blind (v.zip)
            { 1776,   DRTracker.DrCategory.Disorient }, // Gouge (v.zip)
            { 118,    DRTracker.DrCategory.Disorient }, // Polymorph (sheep)
            { 28272,  DRTracker.DrCategory.Disorient }, // Polymorph (pig)
            { 28271,  DRTracker.DrCategory.Disorient }, // Polymorph (turtle)
            { 61305,  DRTracker.DrCategory.Disorient }, // Polymorph (cat)
            { 61721,  DRTracker.DrCategory.Disorient }, // Polymorph (rabbit)
            { 61780,  DRTracker.DrCategory.Disorient }, // Polymorph (turkey)
            { 51514,  DRTracker.DrCategory.Disorient }, // Hex
            { 31661,  DRTracker.DrCategory.Disorient }, // Dragon's Breath
            { 19503,  DRTracker.DrCategory.Disorient }, // Scatter Shot
            { 99,     DRTracker.DrCategory.Disorient }, // Disorienting Roar
            { 82691,  DRTracker.DrCategory.Disorient }, // Ring of Frost

            // ---- CYCLONE ----
            { 33786,  DRTracker.DrCategory.Cyclone },   // Cyclone

            // ---- SILENCES ----
            { 1330,   DRTracker.DrCategory.Silence },   // Garrote - Silence
            { 15487,  DRTracker.DrCategory.Silence },   // Silence (Priest)
            { 47476,  DRTracker.DrCategory.Silence },   // Strangulate
            { 78675,  DRTracker.DrCategory.Silence },   // Solar Beam
            { 25046,  DRTracker.DrCategory.Silence },   // Arcane Torrent
            { 28730,  DRTracker.DrCategory.Silence },   // Arcane Torrent (alt)
            { 31935,  DRTracker.DrCategory.Silence },   // Avenger's Shield silence

            // ---- ROOTS ----
            { 339,    DRTracker.DrCategory.Root },      // Entangling Roots
            { 19975,  DRTracker.DrCategory.Root },      // Nature's Grasp root
            { 102359, DRTracker.DrCategory.Root },      // Mass Entanglement
            { 122,    DRTracker.DrCategory.Root },      // Frost Nova
            { 33395,  DRTracker.DrCategory.Root },      // Freeze (pet)
            { 64695,  DRTracker.DrCategory.Root },      // Earthgrab
            { 53148,  DRTracker.DrCategory.Root },      // Charge root

            // ---- FEARS ----
            { 5782,   DRTracker.DrCategory.Fear },      // Fear (Warlock)
            { 8122,   DRTracker.DrCategory.Fear },      // Psychic Scream
            { 5484,   DRTracker.DrCategory.Fear },      // Howl of Terror
            { 5246,   DRTracker.DrCategory.Fear },      // Intimidating Shout

            // ---- HORRORS ----
            { 64044,  DRTracker.DrCategory.Horror },    // Psychic Horror

            // ---- DISARM ----
            { 51722,  DRTracker.DrCategory.Disarm },    // Dismantle
            { 676,    DRTracker.DrCategory.Disarm },    // Disarm (Warrior)

            // ---- CHARM ----
            { 605,    DRTracker.DrCategory.Charm },     // Mind Control

            // ---- HIBERNATE -> Incap ----
            { 2637,   DRTracker.DrCategory.Incapacitate },
        };
        // ======================================================================

        public static void Initialize()
        {
            if (_attached) return;

            try
            {
                // Rebuild event blacklist on initialization (v.zip)
                RebuildEventIdBlacklist();

                // Combat log (DR + défensifs)
                Lua.Events.AttachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);

                // Loss of control events (emergency layer) - comprehensive coverage
                Lua.Events.AttachEvent("LOSS_OF_CONTROL_ADDED", HandleLossOfControl);
                Lua.Events.AttachEvent("LOSS_OF_CONTROL_UPDATE", HandleLossOfControl);
                Lua.Events.AttachEvent("LOSS_OF_CONTROL_REMOVED", HandleLossOfControl);

                // Files LFG/BG
                Lua.Events.AttachEvent("UPDATE_BATTLEFIELD_STATUS", OnBattlefieldStatus);
                Lua.Events.AttachEvent("LFG_PROPOSAL_SHOW", OnLfgProposal);

                // Arena opponent updates (refresh focus/detections)
                Lua.Events.AttachEvent("ARENA_OPPONENT_UPDATE", OnArenaOpponentUpdate);

                // Pause offensive quand le joueur lance un sort manuellement
                Lua.Events.AttachEvent("UNIT_SPELLCAST_SENT", OnUnitSpellcastSent);

                // Restealth auto-disable on combat (v.zip behavior)
                Lua.Events.AttachEvent("PLAYER_REGEN_DISABLED", OnEnterCombat);

                // v.zip : auto-accept Ready/Role checks
                if (!_roleReadyHooksAttached)
                {
                    Lua.Events.AttachEvent("LFG_ROLE_CHECK_SHOW", OnRoleCheck);
                    Lua.Events.AttachEvent("READY_CHECK", OnReadyCheck);
                    _roleReadyHooksAttached = true;
                }

                // Also attach dispatcher-based hooks for unified routing (LFG/BG/READY)
                AttachReadyRoleHooksIfNeeded();

                _attached = true;

                // CVars (exact v.zip)
                try { Lua.DoString("SetCVar('scriptErrors','0')"); } catch { }
                try { Lua.DoString("SetCVar('autoInteract','0')"); } catch { }

                AppDomain.CurrentDomain.ProcessExit += OnProcessExitOrUnload;
                AppDomain.CurrentDomain.DomainUnload += OnProcessExitOrUnload;
            }
            catch (Exception ex)
            {
                Logging.Write(LogPrefix + "Init error: " + ex.Message);
                _attached = false;
            }
        }

        public static void InitializeQoL()
        {
            if (_qolHooksAttached) return;

            Lua.Events.AttachEvent("LFG_PROPOSAL_SHOW", OnLfgProposal);
            Lua.Events.AttachEvent("CONFIRM_BATTLEFIELD_ENTRY", OnBattlefieldStatus);
            Lua.Events.AttachEvent("UPDATE_BATTLEFIELD_STATUS", OnBattlefieldStatus);

            _qolHooksAttached = true;
        }

        /// <summary>
        /// Initialize core event hooks for system resets and critical events.
        /// </summary>
        public static void InitializeCore()
        {
            if (_coreHooksAttached) return;

            Lua.Events.AttachEvent("PLAYER_ENTERING_WORLD", OnPlayerEnteringWorld);
            Lua.Events.AttachEvent("ZONE_CHANGED_NEW_AREA", OnZoneChangedNewArea);
            Lua.Events.AttachEvent("ARENA_OPPONENT_UPDATE", OnArenaOpponentUpdate);
            Lua.Events.AttachEvent("PLAYER_REGEN_ENABLED", OnPlayerRegenEnabled);   // leave combat
            Lua.Events.AttachEvent("PLAYER_REGEN_DISABLED", OnPlayerRegenDisabled); // enter combat

            _coreHooksAttached = true;
            Logger.Write("[Core] Hooks d'événements attachés.");
        }

        /// <summary>
        /// Shutdown core event hooks.
        /// </summary>
        public static void ShutdownCore()
        {
            if (!_coreHooksAttached) return;

            Lua.Events.DetachEvent("PLAYER_ENTERING_WORLD", OnPlayerEnteringWorld);
            Lua.Events.DetachEvent("ZONE_CHANGED_NEW_AREA", OnZoneChangedNewArea);
            Lua.Events.DetachEvent("ARENA_OPPONENT_UPDATE", OnArenaOpponentUpdate);
            Lua.Events.DetachEvent("PLAYER_REGEN_ENABLED", OnPlayerRegenEnabled);
            Lua.Events.DetachEvent("PLAYER_REGEN_DISABLED", OnPlayerRegenDisabled);

            _coreHooksAttached = false;
            Logger.Write("[Core] Hooks d'événements détachés.");
        }

        // Core event handlers
        private static void OnPlayerEnteringWorld(object _, LuaEventArgs __)
        {
            ContextResets.HardReset("PLAYER_ENTERING_WORLD");
            DangerTracker.Reset(); // Étape 4
            AutoAttack.Start(); // Ensure auto-attack on world enter
            // Reset focus caches on world enter (avoid stale arena unit ids)
            try { VitalicRotation.Managers.FocusManager.Reset(); } catch { }
            // Play a subtle ready sound when entering arena/BG (user feedback OOC)
            try
            {
                string instType = string.Empty;
                try { instType = Lua.GetReturnVal<string>("local a,t=IsInInstance(); return tostring(t or '')", 0) ?? string.Empty; } catch { instType = string.Empty; }
                bool isArenaOrBg = string.Equals(instType, "arena", StringComparison.OrdinalIgnoreCase) || string.Equals(instType, "pvp", StringComparison.OrdinalIgnoreCase);
                if (isArenaOrBg && VitalicSettings.Instance.AlertQueues && VitalicSettings.Instance.SoundAlertsEnabled)
                {
                    if ((DateTime.UtcNow - _lastInstanceSound).TotalMilliseconds > 2000)
                    { AudioBus.PlayReady(); _lastInstanceSound = DateTime.UtcNow; }
                }
            }
            catch { }
        }

        private static void OnZoneChangedNewArea(object _, LuaEventArgs __)
        {
            ContextResets.SoftReset("ZONE_CHANGED_NEW_AREA");
            DangerTracker.Reset(); // Étape 4
            // Also reset focus on zone change to clear arena placeholders
            try { VitalicRotation.Managers.FocusManager.Reset(); } catch { }
            // Optional: play ready sound if we zone into arena/BG (throttled)
            try
            {
                string instType = string.Empty;
                try { instType = Lua.GetReturnVal<string>("local a,t=IsInInstance(); return tostring(t or '')", 0) ?? string.Empty; } catch { instType = string.Empty; }
                bool isArenaOrBg = string.Equals(instType, "arena", StringComparison.OrdinalIgnoreCase) || string.Equals(instType, "pvp", StringComparison.OrdinalIgnoreCase);
                if (isArenaOrBg && VitalicSettings.Instance.AlertQueues && VitalicSettings.Instance.SoundAlertsEnabled)
                {
                    if ((DateTime.UtcNow - _lastInstanceSound).TotalMilliseconds > 2000)
                    { AudioBus.PlayReady(); _lastInstanceSound = DateTime.UtcNow; }
                }
            }
            catch { }
        }

        private static void OnArenaOpponentUpdate(object _, LuaEventArgs __)
        {
            // Opposition mise à jour pendant la préparation d'arène
            ContextResets.ArenaPrepRefresh();
        }

        private static void OnPlayerRegenEnabled(object _, LuaEventArgs __)
        {
            // Sortie de combat → opportunité de purge légère
            ContextResets.OutOfCombatPrune();
            AutoAttack.Start(); // Restart auto-attack on combat end
        }

        private static void OnPlayerRegenDisabled(object _, LuaEventArgs __)
        {
            // Entrée en combat : nettoie les DR expirés pour repartir propre
            ContextResets.InCombatSanity();
            AutoAttack.Start(); // Ensure auto-attack on combat start
        }

        public static void Shutdown()
        {
            if (!_attached) return;

            try
            {
                Lua.Events.DetachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);
                Lua.Events.DetachEvent("LOSS_OF_CONTROL_ADDED", HandleLossOfControl);
                Lua.Events.DetachEvent("LOSS_OF_CONTROL_UPDATE", HandleLossOfControl);
                Lua.Events.DetachEvent("LOSS_OF_CONTROL_REMOVED", HandleLossOfControl);
                Lua.Events.DetachEvent("UPDATE_BATTLEFIELD_STATUS", OnBattlefieldStatus);
                Lua.Events.DetachEvent("LFG_PROPOSAL_SHOW", OnLfgProposal);
                Lua.Events.DetachEvent("ARENA_OPPONENT_UPDATE", OnArenaOpponentUpdate);
                Lua.Events.DetachEvent("UNIT_SPELLCAST_SENT", OnUnitSpellcastSent);
                Lua.Events.DetachEvent("PLAYER_REGEN_DISABLED", OnEnterCombat);

                if (_roleReadyHooksAttached)
                {
                    Lua.Events.DetachEvent("LFG_ROLE_CHECK_SHOW", OnRoleCheck);
                    Lua.Events.DetachEvent("READY_CHECK", OnReadyCheck);
                    _roleReadyHooksAttached = false;
                }

                // Detach dispatcher hooks
                DetachReadyRoleHooksIfNeeded();
            }
            catch (Exception ex)
            {
                Logging.Write(LogPrefix + "Detach error: " + ex.Message);
            }

            _attached = false;

            AppDomain.CurrentDomain.ProcessExit -= OnProcessExitOrUnload;
            AppDomain.CurrentDomain.DomainUnload -= OnProcessExitOrUnload;

            // UI frames shutdown (v.zip: Class136/135 via OnBotStopped handlers)
            try { VitalicUi.StatusFrameShutdown(); } catch { }
            try { VitalicUi.NotifyFrameShutdown(); } catch { }

            // Cache propre des frames (miroir v.zip)
            try { Lua.DoString("if sf then sf:Hide() end; if ff then ff:Hide() end"); } catch { }
            try { Lua.DoString("if ZoneTextFrame and ZoneTextFrame.icon then ZoneTextFrame.icon:SetTexture(nil) end"); } catch { }
            try { Lua.DoString("SetCVar('scriptErrors','1')"); } catch { }

            // Kill banner system to avoid lingering ZoneText/PVPInfo hooks
            try { VitalicUi.KillBannerSystem(); } catch { }
        }

        public static void ShutdownQoL()
        {
            if (!_qolHooksAttached) return;

            Lua.Events.DetachEvent("LFG_PROPOSAL_SHOW", OnLfgProposal);
            Lua.Events.DetachEvent("CONFIRM_BATTLEFIELD_ENTRY", OnBattlefieldStatus);
            Lua.Events.DetachEvent("UPDATE_BATTLEFIELD_STATUS", OnBattlefieldStatus);

            _qolHooksAttached = false;
            Logger.Write("[QoL] Hooks LFG/BG détachés.");
        }

        private static void OnProcessExitOrUnload(object sender, EventArgs e)
        {
            Shutdown();
        }

        // === Gouge pause window (Class141 entry for spellId 1776) ===
        private static readonly Dictionary<ulong, DateTime> _gougePauseWindow = new Dictionary<ulong, DateTime>();

        /// <summary>
        /// Check if we should pause damage due to Gouge on target
        /// </summary>
        public static bool ShouldPauseDamageForGouge(WoWUnit target)
        {
            if (target == null || !target.IsValid) return false;
            DateTime until;
            if (_gougePauseWindow.TryGetValue(target.Guid, out until))
                return until > DateTime.UtcNow;
            return false;
        }

        /// <summary>
        /// Set Gouge pause window on target
        /// </summary>
        private static void SetGougePauseWindow(ulong targetGuid, double seconds)
        {
            // Event-driven: use long expiry, will be cleared on aura removal
            _gougePauseWindow[targetGuid] = DateTime.UtcNow.AddSeconds(60.0); // Will be cleared by SPELL_AURA_REMOVED
        }

        /// <summary>
        /// Clear Gouge pause window for target with mini buffer
        /// </summary>
        private static void ClearGougePauseWindow(ulong targetGuid)
        {
            // Mini buffer ~150ms after aura removal to ensure clean transition
            if (_gougePauseWindow.ContainsKey(targetGuid))
            {
                _gougePauseWindow[targetGuid] = DateTime.UtcNow.AddMilliseconds(150);
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][Gouge] Pause window cleared with 150ms buffer for {0:X}", targetGuid);
            }
        }

        /// <summary>
        /// Résolution d’une entrée menace et armement des défensifs (Feint/Cloak/Evasion)
        /// </summary>
        private static void ArmDefensiveThreat(string subEvent, int spellId, ulong srcGuid, ulong dstGuid)
        {
            try
            {
                if (spellId <= 0 || string.IsNullOrEmpty(subEvent)) return;

                ThreatTrigger trig;
                switch (subEvent)
                {
                    case "SPELL_AURA_APPLIED":
                    case "SPELL_AURA_REFRESH":
                        trig = ThreatTrigger.AuraApplied; break;
                    case "SPELL_AURA_REMOVED":
                        trig = ThreatTrigger.AuraRemoved; break;
                    case "SPELL_CAST_START":
                        trig = ThreatTrigger.CastStart; break;
                    case "SPELL_CAST_SUCCESS":
                        trig = ThreatTrigger.CastSuccess; break;
                    case "SPELL_PERIODIC_DAMAGE":
                        trig = ThreatTrigger.Periodic; break;
                    default:
                        return;
                }

                var matches = ThreatTable.Where(t => t.SpellId == spellId && t.Trigger == trig).ToList();
                if (matches.Count == 0) return;

                // Portée (si spécifiée)
                double dist = 0;
                if (matches.Any(m => m.MaxRange > 0))
                {
                    try
                    {
                        var caster = (srcGuid != 0UL) ? ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid) : null;
                        if (caster != null && caster.IsValid)
                            dist = Me.Location.Distance(caster.Location);
                        else dist = double.MaxValue;
                    }
                    catch { dist = double.MaxValue; }
                }

                foreach (var e in matches)
                {
                    if (e.MaxRange > 0 && dist > e.MaxRange)
                        continue;

                    int responsesFlags = (int)e.Responses;

                    if (trig == ThreatTrigger.AuraApplied)
                    {
                        VitalicRotation.Managers.DefensivesManager.DangerAuraApplied(srcGuid, e.SpellId, responsesFlags);
                        if (!e.UntilAuraRemoved && e.WindowSeconds > 0)
                            VitalicRotation.Managers.DefensivesManager.ActivateDangerWindow(e.WindowSeconds, srcGuid, e.SpellId, responsesFlags);
                        if (VitalicSettings.Instance.DiagnosticMode)
                            Logging.Write("[Def] DangerWindow: {0} {1}{2}", e.SpellId, subEvent, e.UntilAuraRemoved ? " until remove" : " for " + e.WindowSeconds + "s");
                    }
                    else if (trig == ThreatTrigger.AuraRemoved)
                    {
                        VitalicRotation.Managers.DefensivesManager.DangerAuraRemoved(srcGuid, e.SpellId);
                    }
                    else
                    {
                        if (e.WindowSeconds > 0)
                        {
                            VitalicRotation.Managers.DefensivesManager.ActivateDangerWindow(e.WindowSeconds, srcGuid, e.SpellId, responsesFlags);
                            if (VitalicSettings.Instance.DiagnosticMode)
                                Logging.Write("[Def] DangerWindow: {0} {1} for {2}s", e.SpellId, subEvent, e.WindowSeconds);
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// À appeler depuis la Pulse() de ta routine (hors combat).
        /// - Persistance position StatusFrame (StatusFrameMoved)
        /// - Anti-AFK (~15s)
        /// - AutoTarget si activé
        /// </summary>
        public static void Pulse()
        {
            if (!_attached) return;

            // AutoFace (v.zip) — tenter de faire face pendant une petite fenêtre
            if (VitalicSettings.Instance.EventAutoFace && _autoFaceUnit != null)
            {
                if (DateTime.UtcNow <= _autoFaceUntil && _autoFaceUnit.IsValid && _autoFaceUnit.IsAlive)
                {
                    try
                    {
                        if (!StyxWoW.Me.IsSafelyFacing(_autoFaceUnit))
                            StyxWoW.Me.SetFacing(_autoFaceUnit);
                    }
                    catch { /* ne jamais faire planter */ }
                }
                else
                {
                    _autoFaceUnit = null;
                }
            }

            var me = Me;
            if (me == null || !me.IsValid || me.IsDead) return;

            // --- Sauvegarde position StatusFrame (miroir v.zip) ---
            if (VitalicSettings.Instance.StatusFrameEnabled)
            {
                try
                {
                    bool moved = false;
                    try { moved = Styx.WoWInternals.Lua.GetReturnVal<bool>("return StatusFrameMoved ~= nil", 0); } catch { moved = false; }

                    if (moved)
                    {
                        var now = DateTime.UtcNow;
                        if ((now - _lastStatusPosSave).TotalSeconds >= 0.2)
                        {
                            _lastStatusPosSave = now;

                            double left = 0d, bottom = 0d;
                            try { left = Styx.WoWInternals.Lua.GetReturnVal<double>("if sf then return sf:GetLeft() end", 0); } catch { }
                            try { bottom = Styx.WoWInternals.Lua.GetReturnVal<double>("if sf then return sf:GetBottom() end", 0); } catch { }

                            if (left > 0d && bottom > 0d)
                            {
                                VitalicSettings.Instance.StatusFrameLeft = left;
                                VitalicSettings.Instance.StatusFrameBottom = bottom;
                                VitalicSettings.Instance.Save();
                            }

                            try { Styx.WoWInternals.Lua.DoString("StatusFrameMoved = nil"); } catch { }
                        }
                    }
                }
                catch { }
            }

            // Anti-AFK (toutes ~15s OOC) — seulement si activé
            if (VitalicSettings.Instance.AntiAFK && !me.Combat && !me.IsCasting && !me.IsChanneling)
            {
                var now = DateTime.UtcNow;
                if ((now - _lastAntiAfk).TotalSeconds >= 15.0)
                {
                    _lastAntiAfk = now;
                    try
                    {
                        Lua.DoString("JumpOrAscendStart();");
                        Lua.DoString("MoveAndSteerStop();");
                    }
                    catch { }
                }
            }

            // AutoTarget : si activé et pas de cible → /targetenemy
            if (VitalicSettings.Instance.AutoTarget)
            {
                if ((DateTime.UtcNow - _lastAutoTarget).TotalMilliseconds > 800)
                {
                    if (!me.GotTarget && !me.IsCasting && !me.IsChanneling && !me.Mounted)
                    {
                        try
                        {
                            Lua.DoString("TargetNearestEnemy()");
                            _lastAutoTarget = DateTime.UtcNow;
                        }
                        catch { }
                    }
                }
            }

            // Maintenir la fenêtre pour les canalisations en cours (ex. Fists of Fury)
            try
            {
                foreach (var kv in _dangerCasts)
                {
                    int id = kv.Key;
                    var dc = kv.Value;
                    var units = Styx.WoWInternals.ObjectManager.GetObjectsOfType<WoWUnit>();
                    for (int i = 0; i < units.Count; i++)
                    {
                        var u = units[i];
                        if (u != null && u.IsValid && !u.IsFriendly && u.IsChanneling)
                        {
                            int sid = GetUnitCurrentSpellIdSafe(u);
                            if (sid == id)
                            {
                                VitalicRotation.Managers.DefensivesManager.KeepAuraWindow(u.Guid, id, (VitalicRotation.Managers.DefensiveFlags)dc.Flags);
                            }
                        }
                    }
                }
            }
            catch { }

            // === AJOUT : anti-AFK minimal (style v.zip) ===
            AntiAfkTick();

            // Removed pending Cloak retry and all polling fallbacks for strict Vitalic parity

        }

        // Helper: tente de récupérer l'ID du sort actuellement casté/canalisé (API varie selon les builds HB)
        private static int GetUnitCurrentSpellIdSafe(WoWUnit u)
        {
            if (u == null) return 0;
            try
            {
                // 1) Propriété CastingSpellId (commune) si disponible
                var pi = typeof(WoWUnit).GetProperty("CastingSpellId");
                if (pi != null)
                {
                    object val = null;
                    try { val = pi.GetValue(u, null); } catch { val = null; }
                    int sid;
                    if (val is int) return (int)val;
                    if (val != null && int.TryParse(val.ToString(), out sid)) return sid;
                }

                // 2) Propriété ChanneledSpellId (certaines branches) si dispo
                var pi2 = typeof(WoWUnit).GetProperty("ChanneledSpellId");
                if (pi2 != null)
                {
                    object val = null;
                    try { val = pi2.GetValue(u, null); } catch { val = null; }
                    int sid;
                    if (val is int) return (int)val;
                    if (val != null && int.TryParse(val.ToString(), out sid)) return sid;
                }

                // 3) Objet CastingSpell avec propriété Id
                var pi3 = typeof(WoWUnit).GetProperty("CastingSpell");
                if (pi3 != null)
                {
                    var spellObj = pi3.GetValue(u, null);
                    if (spellObj != null)
                    {
                        var idProp = spellObj.GetType().GetProperty("Id");
                        if (idProp != null)
                        {
                            object val = null;
                            try { val = idProp.GetValue(spellObj, null); } catch { val = null; }
                            int sid;
                            if (val is int) return (int)val;
                            if (val != null && int.TryParse(val.ToString(), out sid)) return sid;
                        }
                    }
                }
            }
            catch { }
            return 0;
        }

        // Timestamp for Marked for Death usage (to detect MfD -> Envenom/Rupture sequencing reliably)
        private static DateTime _lastMfdUtc = DateTime.MinValue;
        private static ulong _lastMfdTargetGuid = 0UL; // last target GUID MfD was applied to
        internal static bool WasMfdRecent(int ms)
        {
            if (ms <= 0) ms = 1000;
            return (DateTime.UtcNow - _lastMfdUtc).TotalMilliseconds <= ms;
        }
        internal static bool IsLastMfdTarget(ulong guid)
        { return guid != 0UL && guid == _lastMfdTargetGuid; }
        internal static void ClearMfdIfTarget(ulong guid)
        {
            if (guid != 0UL && guid == _lastMfdTargetGuid)
            {
                _lastMfdTargetGuid = 0UL; // allow re-use on a new target (or same if spell off CD)
                // do not reset timestamp; only target association
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][MfD] Cleared target on UNIT_DIED {0:X}", guid);
            }
        }

        // === Combat Log handler (DR feed + défensifs) ========================
        private static void HandleCombatLog(object sender, LuaEventArgs args)
        {
            try
            {
                object[] a = args.Args;
                if (a == null || a.Length < 13)
                    return;

                // Indices MoP (COMBAT_LOG_EVENT_UNFILTERED varargs):
                // 0=timestamp, 1=subEvent, 2=hideCaster, 3=sourceGUID, 4=sourceName, 5=sourceFlags, 6=sourceRaidFlags,
                // 7=destGUID, 8=destName, 9=destFlags, 10=destRaidFlags, 11=spellId, 12=spellName, 13=spellSchool (for spell events)
                string ev = a[1] != null ? a[1].ToString() : null;
                if (string.IsNullOrEmpty(ev))
                    return;

                ulong srcGuid; TryToULong(a[3], out srcGuid);
                ulong dstGuid; TryToULong(a[7], out dstGuid);

                int spellId; TryToInt(a[11], out spellId);
                string spellName = (a.Length > 12 && a[12] != null) ? a[12].ToString() : string.Empty;

                // === EMERGENCY LAYER - RUNS FIRST ===
                if (TopPriorityEmergencyReact(ev, spellId, srcGuid, dstGuid))
                    return;

                // Early Deep Freeze debug: log ANY occurrence of id 44572 to validate indices and presence
                if (VitalicSettings.Instance.DiagnosticMode && spellId == 44572)
                {
                    try
                    {
                        Logger.Write("[Diag][DeepFreeze] Observed {0} (early hook) src={1:X} dst={2:X} name={3}", ev, srcGuid, dstGuid, string.IsNullOrEmpty(spellName)?"(nil)":spellName);
                    }
                    catch { }
                }

                // Early Ice Block debug: ensure we see 45438 even if later filters would skip
                if (VitalicSettings.Instance.DiagnosticMode && spellId == 45438 &&
                    (ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH" || ev == "SPELL_AURA_REMOVED"))
                {
                    try
                    {
                        Logger.Write("[Diag][IceBlock][Early] {0} src={1:X} dst={2:X} name={3}", ev, srcGuid, dstGuid, string.IsNullOrEmpty(spellName)?"(nil)":spellName);
                    }
                    catch { }
                }

                // === Détection TOTEMS (Shaman) — v.zip utilise SPELL_SUMMON ===
                if (ev == "SPELL_SUMMON" && spellId != 0 && _totemSpellIds.Contains(spellId))
                {
                    try
                    {
                        // Met à jour la récence par famille (parité Class141.dictionary_0)
                        Totems flag;
                        if (TotemMappings.SpellToTotemMap.TryGetValue(spellId, out flag))
                        {
                            TotemRecency[flag] = DateTime.UtcNow;
                        }
                    }
                    catch { }
                }

                // Track Marked for Death usage by player (reliable MfD detection like original)
                try
                {
                    if (ev == "SPELL_CAST_SUCCESS" && srcGuid == (Me != null ? Me.Guid : 0UL) && spellId == VitalicRotation.Helpers.SpellBook.MarkedForDeath)
                    {
                        _lastMfdUtc = DateTime.UtcNow;
                        _lastMfdTargetGuid = dstGuid;
                        if (VitalicSettings.Instance.DiagnosticMode)
                            Logger.Write("[Diag][MfD] Cast success on target {0:X}", dstGuid);
                    }
                }
                catch { }

                // Control tracker (Cheap Shot 1833, Kidney 408, Paralytic 113953) — record only when WE apply aura
                if ((ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH") && srcGuid == (Me != null ? Me.Guid : 0UL))
                {
                    try { ControlTracker.MarkIfTracked(spellId, dstGuid); } catch { }
                }

                // Detect death of MfD target (combat log UNIT_DIED)
                if (ev == "UNIT_DIED")
                {
                    if (dstGuid != 0UL && IsLastMfdTarget(dstGuid))
                    {
                        ClearMfdIfTarget(dstGuid);
                    }
                }

                // === Fenêtres danger (casts/aura) pilotées par EventHandlers (ÉTAPE 4) ===
                try
                {
                    var srcUnit = ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid);
                    if (!(srcUnit == null || !srcUnit.IsValid || srcUnit.IsFriendly))
                    {
                        // CASTS DANGEREUX
                        if (ev == "SPELL_CAST_START")
                        {
                            // Vérification dans les nouveaux arrays - ÉTAPE 4
                            if (DangerCasts.Contains(spellId))
                            {
                                DangerTracker.RaiseDanger(spellId, srcGuid, dstGuid);
                            }

                            // Système existant avec DangerCast
                            DangerCast dc;
                            if (_dangerCasts.TryGetValue(spellId, out dc))
                            {
                                double seconds = dc.WindowSeconds;
                                try
                                {
                                    if (srcUnit.IsCasting)
                                    {
                                        var left = srcUnit.CurrentCastTimeLeft; // TimeSpan
                                        if (left.TotalSeconds > 0.0)
                                            seconds = left.TotalSeconds + 0.15; // petite marge
                                    }
                                }
                                catch { }

                                VitalicRotation.Managers.DefensivesManager.OpenCastWindow(
                                    srcGuid,
                                    spellId,
                                    (VitalicRotation.Managers.DefensiveFlags)dc.Flags,
                                    seconds,
                                    dc.MaxRange);

                                // Also activate the generic danger window so TryCloak() gates pass immediately
                                try
                                {
                                    VitalicRotation.Managers.DefensivesManager.ActivateDangerWindow(seconds, srcGuid, spellId, (int)dc.Flags);
                                }
                                catch { }

                                // Diagnostics approfondis pour Deep Freeze (44572)
                                if (VitalicSettings.Instance.DiagnosticMode && spellId == 44572)
                                {
                                    try
                                    {
                                        double distDbg = 0.0;
                                        try { distDbg = Me.Location.Distance(srcUnit.Location); } catch { }
                                        Logger.Write("[Diag][DeepFreeze] OpenCastWindow: start -> seconds={0:0.00} maxRange={1} dist={2:0.0} src={3:X} dst={4:X}", seconds, dc.MaxRange, distDbg, srcGuid, dstGuid);
                                    }
                                    catch { }
                                }
                            }
                        }

                        // AURAS DANGEREUSES
                        if (ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH")
                        {
                            // Vérification dans les nouveaux arrays - ÉTAPE 4
                            if (DangerAuras.Contains(spellId))
                            {
                                DangerTracker.RaiseDanger(spellId, srcGuid, dstGuid);
                            }

                            // Système existant avec DangerAura
                            DangerAura da;
                            if (_dangerAuras.TryGetValue(spellId, out da))
                            {
                                VitalicRotation.Managers.DefensivesManager.OpenAuraWindow(
                                    srcGuid,
                                    spellId,
                                    (VitalicRotation.Managers.DefensiveFlags)da.Flags);
                            }
                        }
                        else if (ev == "SPELL_AURA_REMOVED")
                        {
                            // Nettoyage des fenêtres de danger - ÉTAPE 4
                            if (DangerAuras.Contains(spellId))
                            {
                                DangerTracker.ClearDanger(srcGuid);
                            }

                            if (_dangerAuras.ContainsKey(spellId))
                            {
                                VitalicRotation.Managers.DefensivesManager.CloseAuraWindow(srcGuid);
                            }
                        }
                    }
                }
                catch { }

                // Auto-Blind: ouverture de fenêtre si un healer ennemi utilise un "break-free"
                if (ev == "SPELL_CAST_SUCCESS")
                {
                    if (VitalicSettings.Instance.AutoBlindHealerTrinket && _healerTrinketBreaks.Contains(spellId))
                    {
                        try
                        {
                            var src = ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid);
                            if (src != null && src.IsValid && !src.IsFriendly && IsHealerClass(src))
                            {
                                _healerTrinketWindow[src.Guid] = DateTime.UtcNow.AddSeconds(2.5);
                            }
                        }
                        catch { }
                    }
                }

                // --- Break signals (EMfH / Trinket / WotF) ---
                if (ev == "SPELL_CAST_SUCCESS" && srcGuid == (Me != null ? Me.Guid : 0UL))
                {
                    if (spellId == 59752 || spellId == 42292 || spellId == 7744)
                    {
                        try
                        {
                            VitalicUi.ShowBigBanner("Break used");
                            if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent();
                        }
                        catch { }
                    }
                }

                // --- Blind Healer Trinket signal (notify-only) ---
                if ((ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH") && spellId == 2094)
                {
                    try
                    {
                        var dest = ObjectManager.GetObjectByGuid<WoWUnit>(dstGuid) as WoWPlayer;
                        if (dest != null && dest.IsValid && dest.IsFriendly && IsHealerClass(dest.Class))
                        {
                            VitalicUi.ShowBigBanner("Blind Healer Trinket");
                            if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent();
                        }
                    }
                    catch { }
                }

                // --- Moteur menaces (Feint/Cloak/Evasion) ---
                ArmDefensiveThreat(ev, spellId, srcGuid, dstGuid);

                // Interrupts (Kick)
                if (ev == "SPELL_CAST_START" || ev == "SPELL_CAST_CHANNEL_START")
                {
                    var t = Me.CurrentTarget as WoWUnit;
                    if (t != null && t.IsValid && t.IsAlive && srcGuid == t.Guid)
                    {
                        TryKickCurrentTarget();
                    }
                }

                // ====== AutoFace (v.zip) : face brièvement si un cast débute sur notre cible ======
                if (VitalicSettings.Instance.EventAutoFace && ev == "SPELL_CAST_START")
                {
                    var t = Me.CurrentTarget as WoWUnit;
                    if (t != null && t.IsValid && t.IsAlive && srcGuid == Me.Guid && dstGuid == t.Guid)
                    {
                        _autoFaceUnit = t;
                        _autoFaceUntil = DateTime.UtcNow.AddMilliseconds(350);
                    }
                }

                // ====== Death Grip notify si < 40% HP (v.zip) ======
                if (ev == "SPELL_CAST_SUCCESS" && spellId == SpellId_DeathGrip)
                {
                    if (dstGuid == Me.Guid && Me.HealthPercent < 40)
                    {
                        try 
                        { 
                            VitalicUi.ShowNotify(spellId, string.IsNullOrEmpty(spellName) ? "Death Grip" : spellName);
                            try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayAlarm(); } catch { }
                        } 
                        catch { }
                    }
                }

                // ====== Event-driven reactions (Vitalic parity extracts) ======
                // Minimal faithful subset wired from decompiled mappings:
                // - Deep Freeze (44572) handled exclusively by matrix
                // - Devotion Aura (31821), Inner Focus (89485) -> Shiv (5938)
                // - Avenging Wrath (31884) -> Dismantle (51722)
                // - Blink (1953) -> Shadowstep (36554)
                try
                {
                    // Resolve caster and dest units if needed (some reactions below use them)
                    WoWUnit srcUnit = null, dstUnit = null;
                    try { srcUnit = ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid); } catch { srcUnit = null; }
                    try { dstUnit = ObjectManager.GetObjectByGuid<WoWUnit>(dstGuid); } catch { dstUnit = null; }

                    // Quick Blind diagnostics: log server-side Blind events
                    if ((ev == "SPELL_CAST_SUCCESS" || ev == "SPELL_AURA_APPLIED") && spellId == 2094)
                    {
                        Logger.Write(string.Format("[Diag][Blind][CL] {0} src={1:X} dst={2:X}", ev, srcGuid, dstGuid));
                    }

                    // Deep Freeze timing diagnostics
                    if (spellId == 44572 && VitalicSettings.Instance.DiagnosticMode)
                    {
                        // DF diagnostics visible only in DiagnosticMode
                        Logger.Write(string.Format("[DF][Seen] {0}  src={1:X}  dst={2:X}", ev, srcGuid, dstGuid));
                        if (ev == "SPELL_CAST_SUCCESS")
                        {
                            _dfCastSuccessAt[srcGuid] = DateTime.UtcNow;
                            Logger.Write(string.Format("[DF][Timing] CAST_SUCCESS from {0:X}", srcGuid));
                        }
                        else if (ev == "SPELL_AURA_APPLIED" && dstGuid == (Me != null ? Me.Guid : 0UL))
                        {
                            _dfAuraAppliedAt[srcGuid] = DateTime.UtcNow;
                            DateTime t0;
                            if (_dfCastSuccessAt.TryGetValue(srcGuid, out t0))
                            {
                                var dt = DateTime.UtcNow - t0;
                                Logger.Write(string.Format("[DF][Timing] AURA_APPLIED delta={0} ms (src {1:X})", (int)dt.TotalMilliseconds, srcGuid));
                                Logger.Write(string.Format("[DF][Timing] CAST->AURA = {0} ms", (int)dt.TotalMilliseconds));
                            }
                        }
                    }

                    // Deep Freeze and Scatter handled by VITALIC v.zip logic
                    if (spellId == 44572 && (ev == "SPELL_CAST_START" || ev == "SPELL_CAST_SUCCESS" || ev == "SPELL_AURA_APPLIED") && dstGuid == (Me != null ? Me.Guid : 0UL))
                    {
                        // DEBUG path: in DiagnosticMode, force a direct Blind attempt with explicit logs
                        if (VitalicSettings.Instance.DiagnosticMode)
                        {
                            // Try an experimental Blind, but do NOT short-circuit the real counter flow
                            DebugForceBlindOnDeepFreeze(ev, srcGuid, dstGuid);
                        }
                        bool _shouldHandleDf = ShouldHandleDeepFreeze(ev, srcGuid);
                        if (VitalicSettings.Instance.DiagnosticMode)
                            Logger.Write(string.Format("[DF][Filter] ShouldHandleDeepFreeze -> {0} (ev={1}, src={2:X})", _shouldHandleDf, ev, srcGuid));
                        if (_shouldHandleDf)
                        {
                            if (ev == "SPELL_CAST_SUCCESS")
                            {
                                // Mark this caster handled now to suppress immediate AURA_APPLIED duplicate
                                _lastDfHandledByCaster[srcGuid] = DateTime.UtcNow;
                                if (VitalicSettings.Instance.DiagnosticMode)
                                    Logger.Write(string.Format("[DF][Filter] Marked handled to suppress duplicate AURA_APPLIED for src {0:X}", srcGuid));
                            }
                            
                            // USE NEW VITALIC v.zip LOGIC - direct counter casting
                            WoWUnit caster = null;
                            try { caster = ObjectManager.GetObjectByGuid<WoWUnit>(srcGuid); } catch { }
                            
                            if (TryVitalicCounter(spellId, caster))
                            {
                                Logger.Write(string.Format("[Vitalic Counter] Successfully countered Deep Freeze from {0}", 
                                    caster != null ? caster.SafeName : "Unknown"));
                            }
                            else
                            {
                                Logger.Write(string.Format("[Vitalic Counter] Failed to counter Deep Freeze from {0}", 
                                    caster != null ? caster.SafeName : "Unknown"));
                            }
                        }
                        else if (VitalicSettings.Instance.DiagnosticMode)
                        {
                            try { Logger.Write("[Diag][DeepFreeze] Suppressed duplicate on {0} for caster {1:X}", ev, srcGuid); } catch { }
                        }
                    }
                    else if (spellId == 19503 && (ev == "SPELL_CAST_START" || ev == "SPELL_CAST_SUCCESS" || ev == "SPELL_AURA_APPLIED"))
                    {
                        TryCounterMatrix(ev, spellId, srcGuid, dstGuid);
                    }

                    // 2) Devotion Aura / Inner Focus -> Shiv (dispel-like)
                    if ((ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH") && (spellId == 31821 || spellId == 89485))
                    {
                        // Caster gets the aura; attempt Shiv if in melee and attackable
                        var u = srcUnit;
                        if (u != null && u.IsValid && u.IsAlive && u.Attackable && !u.IsFriendly)
                        {
                            bool inMelee = false; try { inMelee = SpellBook.InMeleeRange(u, 6.0); } catch { }
                            if (inMelee && SpellBook.CanCast(SpellBook.Shiv, u))
                            {
                                if (Throttle.Check("React.Shiv.Aura", 1000))
                                {
                                    if (SpellBook.Cast(SpellBook.Shiv, u))
                                    {
                                        Logger.Write("[Event] Shiv in response to " + (spellId == 31821 ? "Devotion Aura" : "Inner Focus") + " -> " + u.SafeName);
                                        Throttle.Mark("React.Shiv.Aura");
                                    }
                                }
                            }
                        }
                    }

                    // 3) Avenging Wrath -> Dismantle
                    if ((ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH") && spellId == 31884)
                    {
                        var u = srcUnit;
                        if (u != null && u.IsValid && u.IsAlive && u.Attackable && !u.IsFriendly)
                        {
                            bool inMelee = false; try { inMelee = SpellBook.InMeleeRange(u, 5.0); } catch { }
                            if (inMelee && SpellBook.CanCast(SpellBook.Dismantle, u))
                            {
                                // DR gate if available
                                bool canDr = true; try { canDr = DRTracker.Can(u, DRTracker.DrCategory.Disarm); } catch { canDr = true; }
                                if (canDr && Throttle.Check("React.Dismantle.Wings", 1500))
                                {
                                    if (SpellBook.Cast(SpellBook.Dismantle, u))
                                    {
                                        Logger.Write("[Event] Dismantle in response to Avenging Wrath -> " + u.SafeName);
                                        Throttle.Mark("React.Dismantle.Wings");
                                    }
                                }
                            }
                        }
                    }

                    // 4) Blink -> Shadowstep (gap close)
                    if (ev == "SPELL_CAST_SUCCESS" && spellId == 1953)
                    {
                        var u = srcUnit;
                        if (u != null && u.IsValid && u.IsAlive && u.Attackable && !u.IsFriendly)
                        {
                            // Delegate to MobilityManager safe wrapper (handles LOS/range/throttle)
                            try
                            {
                                MobilityManager.TryShadowstepSafe(u);
                            }
                            catch { }
                        }
                    }

                    // 4b) Warrior gap/charge/leap and Shaman Thunderstorm -> Shadowstep (parité Blink)
                    if (ev == "SPELL_CAST_SUCCESS" && (spellId == 100 || spellId == 20253 || spellId == 6544 || spellId == 51490))
                    {
                        var u = srcUnit;
                        if (u != null && u.IsValid && u.IsAlive && u.Attackable && !u.IsFriendly)
                        {
                            try { MobilityManager.TryShadowstepSafe(u); } catch { }
                        }
                    }

                    // 5) AoE CC landing on us (Shadowfury/Howl/Scream) -> Cloak to break/prevent
                    if ((ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH") && (spellId == 30283 || spellId == 5484 || spellId == 8122))
                    {
                        if (dstGuid == (Me != null ? Me.Guid : 0UL))
                        {
                            if (VitalicSettings.Instance.DiagnosticMode)
                                Logger.Write("[Diag][AoECC] {0} on player -> attempt Cloak", spellId);

                            if (SpellBook.CanCast(SpellBook.CloakOfShadows))
                            {
                                if (Throttle.Check("React.Cloak.AoECC", 800))
                                {
                                    if (SpellBook.Cast(SpellBook.CloakOfShadows))
                                    {
                                        Logger.Write("[Event] Cloak in response to AoE CC (" + spellId + ")");
                                        Throttle.Mark("React.Cloak.AoECC");
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }

                // ====== Détection PIÈGES (Hunter) — v.zip utilise SPELL_CREATE ======
                if (ev == "SPELL_CREATE" && spellId != 0 && _trapSpellIds.Contains(spellId))
                {
                    // notify visuelle simple (v.zip le faisait aussi, avec sons si activés)
                    try
                    {
                        if (VitalicSettings.Instance.SpellAlertsEnabled)
                            VitalicUi.ShowNotify("Piège : " + (string.IsNullOrEmpty(spellName) ? ("ID " + spellId) : spellName));
                    }
                    catch { }
                }

                // ====== Auto-Blind Healer Trinket (window consume) ======
                try
                {
                    if (VitalicSettings.Instance.AutoBlindHealerTrinket)
                    {
                        // Find any active window and try Blind once per ~1.2s
                        foreach (var pair in _healerTrinketWindow.ToArray())
                        {
                            var until = pair.Value;
                            if (DateTime.UtcNow > until)
                            {
                                _healerTrinketWindow.Remove(pair.Key);
                                continue;
                            }

                            var u = ObjectManager.GetObjectByGuid<WoWUnit>(pair.Key);
                            if (u == null || !u.IsValid || u.IsDead)
                                continue;
                            if (!u.Attackable || u.IsFriendly)
                                continue;
                            if (!IsHealerClass(u))
                                continue;

                            if (Throttle.Check("React.AutoBlind.HealerTrinket", 1200))
                            {
                                bool cast = false;
                                try { cast = VitalicRotation.Managers.CrowdControlManager.TryBlind(u); } catch { cast = false; }
                                if (cast)
                                {
                                    Logger.Write("[Event] Auto-Blind healer after trinket -> " + u.SafeName);
                                    _healerTrinketWindow.Remove(pair.Key); // consume window on success
                                    break;
                                }
                                else if (VitalicSettings.Instance.DiagnosticMode)
                                {
                                    Logger.Write("[Diag][HealerTrinket] Blind attempt failed or gated for " + u.SafeName);
                                }
                            }
                        }
                    }
                }
                catch { }

                // ====== Détection TOTEMS (Shaman) — v.zip utilise SPELL_SUMMON ======
                if (ev == "SPELL_SUMMON" && spellId != 0 && _totemSpellIds.Contains(spellId))
                {
                    try
                    {
                        if (VitalicSettings.Instance.SpellAlertsEnabled)
                            VitalicUi.ShowNotify("Totem : " + (string.IsNullOrEmpty(spellName) ? ("ID " + spellId) : spellName));
                    }
                    catch { }
                }

                // ====== Feedback visuel quand on INTERROMPT un sort (v.zip faisait un son) ======
                if (ev == "SPELL_INTERRUPT")
                {
                    // srcGuid = qui a interrompu ; on ne notifie que si c'est nous
                    if (srcGuid == Me.Guid && VitalicSettings.Instance.SpellAlertsEnabled)
                    {
                        // Récupère bien l'ID et le nom du sort interrompu (MoP: [14]=id, [15]=name)
                        int extraSpellId = 0;
                        string interruptedName = string.Empty;

                        try
                        {
                            if (a.Length > 14 && a[14] != null)
                                TryToInt(a[14], out extraSpellId);

                            if (a.Length > 15 && a[15] != null)
                                interruptedName = a[15].ToString();

                            // fallback ultra défensif
                            if (string.IsNullOrEmpty(interruptedName) && !string.IsNullOrEmpty(spellName))
                                interruptedName = spellName;
                        }
                        catch { extraSpellId = 0; interruptedName = string.Empty; }

                        if (NotifyThrottle(250))
                        {
                            try
                            {
                                // v.zip parité : utilise la surcharge avec icône si extraSpellId > 0
                                if (extraSpellId > 0)
                                    VitalicUi.ShowNotify(extraSpellId, "Interrupt");
                                else
                                    VitalicUi.ShowNotify("Interrupt" + (string.IsNullOrEmpty(interruptedName) ? "" : (" : " + interruptedName)));
                            }
                            catch { }
                        }
                    }
                }

                // ====== Confirmations de casts du joueur (éviter faux positifs) ======
                if (ev == "SPELL_CAST_SUCCESS" && srcGuid == Me.Guid)
                {
                    // Smoke Bomb: confirme uniquement via CAST_SUCCESS pour déclencher bannière/son
                    if (spellId == SpellId_SmokeBomb || spellId == SpellBook.SmokeBomb)
                    {
                        try { VitalicUi.ShowMiniBanner("Smoke Bomb!", 1f, 0.8f, 0.2f, 1.0f); } catch { }
                        try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { }
                        Logger.Write("[Event] Smoke Bomb (cast success)");
                    }
                }

                // ====== CC sur nous (visuel) ======
                if ((ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH") && dstGuid == Me.Guid)
                {
                    DRTracker.DrCategory cat;
                    if (DrSpells.TryGetValue(spellId, out cat))
                    {
                        if (VitalicSettings.Instance.SpellAlertsEnabled && CcNotifyThrottle(300))
                        {
                            try
                            {
                                var name = !string.IsNullOrEmpty(spellName) ? spellName : ("ID " + spellId);
                                VitalicUi.ShowNotify("CC : " + name);
                            }
                            catch { }
                        }
                    }
                }

                // ====== DR tracking ======
                if (ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH" || ev == "SPELL_AURA_REMOVED")
                {
                    bool blacklisted = (spellId != 0 && _eventIdBlacklist.Contains(spellId));

                    if (dstGuid != 0UL)
                    {
                        WoWUnit dest = ObjectManager.GetObjectByGuid<WoWUnit>(dstGuid);
                        if (dest != null && dest.IsValid)
                        {
                            DRTracker.DrCategory cat;
                            if (!blacklisted && DrSpells.TryGetValue(spellId, out cat))
                            {
                                try { DRTracker.Applied(dest, cat, spellId); } catch { DRTracker.Applied(cat, dest); }
                            }
                        }

                        // === Gouge window management (Class141 entry for 1776) ===
                        if (spellId == 1776) // Gouge
                        {
                            if (ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH")
                            {
                                // Start damage pause window during Gouge - event driven
                                SetGougePauseWindow(dstGuid, 0); // Duration managed by aura removal
                                if (VitalicSettings.Instance.DiagnosticMode)
                                    Logger.Write("[Diag][Gouge] Damage pause window started (event-driven) on {0:X}", dstGuid);
                            }
                            else if (ev == "SPELL_AURA_REMOVED")
                            {
                                // Clear damage pause and restart auto-attack
                                ClearGougePauseWindow(dstGuid);
                                AutoAttack.Start(); // Restart auto-attack like Class77.smethod_8
                                if (VitalicSettings.Instance.DiagnosticMode)
                                    Logger.Write("[Diag][Gouge] Damage pause cleared, auto-attack restarted for {0:X}", dstGuid);
                            }
                        }

                        // === Ice Block handling (45438) -> stop offense on apply; counters only on removal ===
                        if (spellId == SpellId_IceBlock)
                        {
                            if (ev == "SPELL_AURA_APPLIED" || ev == "SPELL_AURA_REFRESH")
                            {
                                // Immediately stop any queued attacks/spells versus that target
                                try { if (dstGuid != 0UL) LuaHelper.CancelQueuedSpellAndStopAttack(); } catch { }
                                if (VitalicSettings.Instance.DiagnosticMode)
                                    Logger.Write("[Diag][IceBlock] Applied to {0:X} -> stop attack & wait release", dstGuid);
                            }
                            else if (ev == "SPELL_AURA_REMOVED")
                            {
                                // Vitalic parity: trigger immediate counters via matrix on removal
                                // Use the mage (aura holder) as both source and dest to resolve unit for melee checks
                                if (VitalicSettings.Instance.DiagnosticMode)
                                    Logger.Write("[Diag][IceBlock] Removed from {0:X} -> invoking counter matrix", dstGuid);
                                try { TryCounterMatrix(ev, spellId, dstGuid, dstGuid); } catch { }
                            }
                        }

                        // === Auto-attack restart on stealth state changes ===
                        if (dstGuid == (Me != null ? Me.Guid : 0UL))
                        {
                            // Restart auto-attack when exiting stealth/subterfuge with hostile target
                            if (ev == "SPELL_AURA_REMOVED" && (spellId == 1784 || spellId == 115192 || spellId == 115193)) // Stealth, Subterfuge variants
                            {
                                try
                                {
                                    var target = Me.CurrentTarget as WoWUnit;
                                    if (target != null && target.IsValid && target.IsAlive && target.Attackable && !target.IsFriendly)
                                    {
                                        AutoAttack.Start();
                                        if (VitalicSettings.Instance.DiagnosticMode)
                                            Logger.Write("[Diag][AutoAttack] Restarted on stealth exit with hostile target");
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Write(LogPrefix + "Exception: " + ex.Message);
            }
        }

        // === Loss of Control Handler (Emergency Layer) ===
        private static void HandleLossOfControl(object sender, LuaEventArgs args)
        {
            try
            {
                // Emergency layer for LoC events
                if (TopPriorityEmergencyReactLoC())
                    return;

                // Existing LoC handling would continue here if needed
            }
            catch (Exception ex)
            {
                Logging.Write(LogPrefix + "LoC Exception: " + ex.Message);
            }
        }

        // === LFG/BG events (existing direct handlers remain) ==================
        private static void OnBattlefieldStatus(object sender, LuaEventArgs e)
        {
            try
            {
                if (VitalicSettings.Instance.AlertQueues)
                {
                    try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayReady(); } catch { }
                }
                TryAcceptBattlefield();
            }
            catch { }
        }

        private static void OnLfgProposal(object sender, LuaEventArgs e)
        {
            try
            {
                if (VitalicSettings.Instance.AlertQueues)
                {
                    try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayReady(); } catch { }
                }
                TryAcceptLfg();
            }
            catch { }
        }

        // === Casts dangereux (projectiles / bursts canalisés) - ÉTAPE 4
        private static readonly HashSet<int> DangerCasts = new HashSet<int> {
            116858, // Chaos Bolt (MoP)
            113656, // Fists of Fury
            11366,  // Pyroblast (proc Hot Streak)
            78674,  // Starsurge
            51505,  // Lava Burst
            8092,   // Mind Blast (burst windows)
            30451,  // Arcane Blast
            5308,   // Execute (War)
            88625,  // Chastise
            2060,   // Heal (Greater Heal)
            596,    // Prayer of Healing
            73920,  // Healing Rain
            1064,   // Chain Heal
        };

        // Auras "burst" offensives ennemies - ÉTAPE 4
        private static readonly HashSet<int> DangerAuras = new HashSet<int> {
            1719,   // Recklessness
            107574, // Avatar
            19574,  // Bestial Wrath
            12472,  // Icy Veins
            114050, // Ascendance (Ele)
            51271,  // Pillar of Frost
            13750,  // Adrenaline Rush (Rogue Combat)
            51713,  // Shadow Dance
            31884,  // Avenging Wrath
            102543, // Incarnation: King of the Jungle (Feral)
            108978, // Alter Time (burst setups)
            3045,   // Rapid Fire (Hunter)
            114049, // Ascendance (Shaman autres specs)
            102560, // Incarnation: Chosen of Elune (Balance)
            106951, // Berserk (Feral/Guardian)
            34471,  // The Beast Within
        };


        // === Manual cast pause =================================================
        private static void OnUnitSpellcastSent(object sender, LuaEventArgs e)
        {
            try
            {
                if (e == null || e.Args == null) return;

                string unit = null;
                string spellName = null;
                int spellId = 0;

                var arr = e.Args as object[];
                if (arr != null)
                {
                    if (arr.Length >= 1 && arr[0] != null)
                        unit = arr[0].ToString();
                    if (arr.Length >= 2 && arr[1] != null)
                        spellName = arr[1].ToString();

                    // Try common indices for spellId (varies by HB build)
                    // Prefer last numeric arg if available
                    for (int i = arr.Length - 1; i >= 0; i--)
                    {
                        int sid;
                        if (TryToInt(arr[i], out sid) && sid > 0)
                        {
                            spellId = sid; break;
                        }
                    }
                }
                else
                {
                    var list = e.Args as System.Collections.IList;
                    if (list != null)
                    {
                        if (list.Count >= 1 && list[0] != null)
                            unit = list[0].ToString();
                        if (list.Count >= 2 && list[1] != null)
                            spellName = list[1].ToString();

                        for (int i = list.Count - 1; i >= 0; i++)
                        {
                            int sid;
                            if (TryToInt(list[i], out sid) && sid > 0)
                            { spellId = sid; break; }
                        }
                    }
                }

                if (unit != null && unit.Equals("player", StringComparison.OrdinalIgnoreCase))
                {
                    // Arm pause only if recent human input (hotkey/macro)
                    if (InputPoller.WasRecentHumanInput(250) || VitalicRotation.Managers.MacroManager.WasManualMacroRecently(250))
                    {
                        // Special case: Shadowmeld (58984) => fixed 500ms pause window
                        bool shadowmeld = (spellId == 58984) || (!string.IsNullOrEmpty(spellName) && spellName.IndexOf("shadowmeld", StringComparison.OrdinalIgnoreCase) >= 0);

                        int ms = 0; try { ms = VitalicSettings.Instance.ManualCastPause; } catch { ms = 0; }
                        if (shadowmeld) ms = 500;

                        ArmPauseOffense(ms);
                    }

                    // Manual opener guard: if player sends Cheap Shot or Garrote, notify PvPRotation to suppress auto opener
                    try
                    {
                        if (spellId == VitalicRotation.Helpers.SpellBook.CheapShot || spellId == VitalicRotation.Helpers.SpellBook.Garrote)
                        {
                            VitalicRotation.Managers.PvPRotation.NotifyManualOpenerUsed();
                            if (VitalicSettings.Instance.DiagnosticMode)
                                Logger.Write("[Diag][Opener] Manual opener sent (id {0}) -> suppress auto-opener for 2.5s", spellId);
                        }
                    }
                    catch { }
                }
            }
            catch
            {
                // silence
            }
        }

        // v.zip : LFG Role Check -> accepte silencieusement
        private static void OnRoleCheck(object sender, LuaEventArgs args)
        {
            try
            {
                if (Enabled)
                {
                    Lua.DoString("RunMacroText('/click RolePollPopupRoleButtonDPS'); if LFDRoleCheckPopupAccept_OnClick then LFDRoleCheckPopupAccept_OnClick() end; if LFDRoleCheckPopup and LFDRoleCheckPopup.Hide then LFDRoleCheckPopup:Hide() end;");
                }
            }
            catch { }
        }

        // v.zip : Ready Check -> confirme 'Ready' silencieusement
        private static void OnReadyCheck(object sender, LuaEventArgs args)
        {
            try
            {
                if (Enabled)
                {
                    Lua.DoString("RunMacroText('/click ReadyCheckFrameYesButton');");
                }
            }
            catch { }
        }

        // Restealth auto-disable on combat entry (v.zip behavior)
        private static void OnEnterCombat(object sender, LuaEventArgs args)
        {
            try
            {
                RestealthManager.Disarm();
            }
            catch { }
        }

        public static void OnLuaEvent(string eventName, params object[] args)
        {
            try
            {
                var Me = Styx.StyxWoW.Me;
                string ev = eventName;
                if (string.Equals(ev, "COMBAT_LOG_EVENT_UNFILTERED", StringComparison.OrdinalIgnoreCase))
                {
                    var a = args as object[] ?? new object[0];
                    string subEvent = a.Length > 1 && a[1] != null ? a[1].ToString() : string.Empty;
                    ulong srcGuid = 0, dstGuid = 0;
                    TryToULong(a.Length > 4 ? a[4] : null, out srcGuid);
                    TryToULong(a.Length > 8 ? a[8] : null, out dstGuid);

                    if (dstGuid == (Me != null ? Me.Guid : 0UL))
                    {
                        // Met à jour l’horodatage dégâts (Feint LastDamage)
                        if (subEvent == "SWING_DAMAGE" || subEvent == "RANGE_DAMAGE" || subEvent == "SPELL_DAMAGE" ||
                            subEvent == "SPELL_PERIODIC_DAMAGE" || subEvent == "DAMAGE_SHIELD" || subEvent == "ENVIRONMENTAL_DAMAGE")
                        {
                            VitalicRotation.Managers.DefensivesManager.NotifyPlayerDamaged();
                        }
                    }
                }
            }
            catch { }

            try
            {
                if (string.Equals(eventName, "COMBAT_LOG_EVENT_UNFILTERED", StringComparison.Ordinal))
                {
                    return;
                }

                if (eventName == "LFG_PROPOSAL_SHOW" || eventName == "LFG_PROPOSAL_UPDATE")
                {
                    if (VitalicSettings.Instance.AlertQueues)
                    {
                        try { VitalicUi.ShowNotify("File trouvée…"); } catch { }
                        try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayReady(); } catch { }
                    }
                    TryAcceptLfg();
                    return;
                }

                if (eventName == "UPDATE_BATTLEFIELD_STATUS")
                {
                    if (VitalicSettings.Instance.AlertQueues)
                    {
                        try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayReady(); } catch { }
                    }
                    TryAcceptBattlefield();
                    return;
                }

                if (eventName == "READY_CHECK" || eventName == "ROLE_POLL_BEGIN")
                {
                    if (VitalicSettings.Instance.AlertQueues)
                    {
                        try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayReady(); } catch { }
                    }
                    TryAutoReadyYes();
                    return;
                }
            }
            catch { }
        }

        // === Blacklist helpers ================================================
        private static void RebuildEventIdBlacklist()
        {
            _eventIdBlacklist.Clear();
            var s = VitalicSettings.Instance.EventBlacklist;
            if (string.IsNullOrWhiteSpace(s)) return;
            foreach (var tok in s.Split(',')){
                int id; if (int.TryParse(tok.Trim(), out id) && id > 0) _eventIdBlacklist.Add(id);
            }
        }

        // === Hooks attach/detach via dispatcher ================================
        private static void AttachReadyRoleHooksIfNeeded()
        {
            if (_luaHooksAttached) return;

            try
            {
                Lua.Events.AttachEvent("LFG_PROPOSAL_SHOW", LuaDispatcher);
                Lua.Events.AttachEvent("LFG_PROPOSAL_UPDATE", LuaDispatcher);
                Lua.Events.AttachEvent("UPDATE_BATTLEFIELD_STATUS", LuaDispatcher);
                Lua.Events.AttachEvent("READY_CHECK", LuaDispatcher);
                Lua.Events.AttachEvent("ROLE_POLL_BEGIN", LuaDispatcher);

                _luaHooksAttached = true;
                Logging.Write("[Events] Hooks LFG/BG/READY attachés.");
            }
            catch (Exception ex)
            {
                Logging.Write("[Events] Attach error: " + ex.Message);
            }
        }

        private static void DetachReadyRoleHooksIfNeeded()
        {
            if (!_luaHooksAttached) return;

            try
            {
                Lua.Events.DetachEvent("LFG_PROPOSAL_SHOW", LuaDispatcher);
                Lua.Events.DetachEvent("LFG_PROPOSAL_UPDATE", LuaDispatcher);
                Lua.Events.DetachEvent("UPDATE_BATTLEFIELD_STATUS", LuaDispatcher);
                Lua.Events.DetachEvent("READY_CHECK", LuaDispatcher);
                Lua.Events.DetachEvent("ROLE_POLL_BEGIN", LuaDispatcher);
            }
            catch { }

            _luaHooksAttached = false;
        }

        // === Kick helper =====================================================
        private static void TryKickCurrentTarget()
        {
            try
            {
                var t = Me.CurrentTarget as WoWUnit;
                if (t == null || !t.IsValid || !t.IsAlive) return;
                if (!t.Attackable) return;

                // doit être en train de caster ou channel
                if (!t.IsCasting && !t.IsChanneling) return;

                // portée mêlée (Kick est 5y en MoP)
                if (!SpellBook.InMeleeRange(t, 5.0)) return;

                // anti-spam
                var now = DateTime.UtcNow;
                if ((now - _lastKickAttempt).TotalMilliseconds < 150) return;
                _lastKickAttempt = now;

                // sort non-interromptible ?
                bool notInterruptible = false;
                try
                {
                    notInterruptible = Lua.GetReturnVal<bool>(
                        "local _,_,_,_,_,_,_,ni=UnitCastingInfo('target'); " +
                        "if ni==nil then local _,_,_,_,_,_,_,ni2=UnitChannelInfo('target'); return ni2 or false; end; " +
                        "return ni or false;", 0);
                }
                catch { notInterruptible = false; }
                if (notInterruptible) return;

                // Kick via SpellManager (ID du SpellBook), fallback Lua par ID
                int kickId = SpellBook.Kick;
                try
                {
                    if (SpellBook.CanCast(kickId, t))
                    {
                        SpellBook.Cast(kickId, t);
                        return;
                    }
                }
                catch { /* fallback */ }

                try { Lua.DoString("CastSpellByID(" + kickId + ", 'target')"); }
                catch { }
            }
            catch { }
        }

        // === Helpers de conversion sûrs ========================================
        private static bool TryToInt(object o, out int result)
        {
            try
            {
                if (o == null) { result = 0; return false; }
                if (o is int) { result = (int)o; return true; }
                if (o is double) { result = (int)(double)o; return true; }
                string s = o.ToString();
                return int.TryParse(s, out result);
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        private static bool TryToULong(object o, out ulong result)
        {
            try
            {
                if (o == null) { result = 0UL; return false; }
                if (o is ulong) { result = (ulong)o; return true; }
                if (o is double)
                {
                    double d = (double)o;
                    if (d < 0) { result = 0UL; return false; }
                    result = (ulong)d;
                    return true;
                }
                string s = o.ToString();
                if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    ulong parsed;
                    if (ulong.TryParse(s.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out parsed))
                    {
                        result = parsed;
                        return true;
                    }
                    result = 0UL;
                    return false;
                }
                return ulong.TryParse(s, out result);
            }
            catch
            {
                result = 0UL;
                return false;
            }
        }

        public static void RegisterUiNotifications()
        {
            if (_uiNotifHooked) return;
            Lua.Events.AttachEvent("READY_CHECK",         OnReadyCheckNotify);
            Lua.Events.AttachEvent("READY_CHECK_CONFIRM", OnReadyCheckConfirmNotify);
            Lua.Events.AttachEvent("ROLE_POLL_BEGIN",     OnRolePollBeginNotify);
            _uiNotifHooked = true;
        }

        public static void UnregisterUiNotifications()
        {
            if (!_uiNotifHooked) return;
            Lua.Events.DetachEvent("READY_CHECK",         OnReadyCheckNotify);
            Lua.Events.DetachEvent("READY_CHECK_CONFIRM", OnReadyCheckConfirmNotify);
            Lua.Events.DetachEvent("ROLE_POLL_BEGIN",     OnRolePollBeginNotify);
            _uiNotifHooked = false;
        }

        private static void OnReadyCheckNotify(object sender, LuaEventArgs args)
        {
            if (!Throttle.Check("Notify.Ready", 1000)) return;
            UiCompat.Notify("Ready Check");
            AudioBus.Play("ready.wav");
        }

        private static void OnReadyCheckConfirmNotify(object sender, LuaEventArgs args)
        {
            if (!Throttle.Check("Notify.Ready", 1000)) return;
            UiCompat.Notify("Ready Check");
            AudioBus.Play("ready.wav");
        }

        private static void OnRolePollBeginNotify(object sender, LuaEventArgs args)
        {
            if (!Throttle.Check("Notify.Role", 1000)) return;
            UiCompat.Notify("Role Check");
            AudioBus.PlayEvent();
        }

        // === AJOUT : petit throttle générique pour actions de file ===
        private static bool ThrottleQueueAction(int milliseconds)
        {
            var now = DateTime.UtcNow;
            if ((now - _lastQueueAction).TotalMilliseconds < milliseconds) return false;
            _lastQueueAction = now;
            return true;
        }

        // === AJOUT : anti-AFK minimal (style v.zip, sans dépendance MovementManager) ===
        private static void AntiAfkTick()
        {
            if (!VitalicSettings.Instance.AntiAFK) return;

            var now = DateTime.UtcNow;
            if ((now - _lastAfkTick).TotalSeconds < 75) return;
            _lastAfkTick = now;

            try
            {
                if (!Me.Combat && !Me.IsCasting)
                {
                    Lua.DoString("MoveViewRightStart(0.1)");
                    Lua.DoString("MoveViewRightStop()");
                }
            }
            catch { }
        }

        // === AJOUT : implémentations MoP 5.4.8 conformes à v.zip (sans sons) ===
        private static void TryAcceptLfg()
        {
            var S = VitalicSettings.Instance;
            if (!S.AlertQueues)
                return; // removed AutoAcceptLfg gate

            try
            {
                Lua.DoString("if LFGDungeonReadyDialog and LFGDungeonReadyDialogEnterDungeonButton and LFGDungeonReadyDialogEnterDungeonButton:IsEnabled() then LFGDungeonReadyDialogEnterDungeonButton:Click() end");
                Lua.DoString("if StaticPopup1 and StaticPopup1:IsVisible() and StaticPopup1.which=='LFG_ROLE_CHECK' and StaticPopup1Button1 then StaticPopup1Button1:Click() end");
                if (S.AlertQueues)
                    try { VitalicUi.ShowNotify("Entrée donjon/raid/scénario acceptée"); } catch { }
                Logger.Write("[QoL] LFG accepté automatiquement.");
            }
            catch { }
        }

        private static void TryAcceptBattlefield()
        {
            var S = VitalicSettings.Instance;
            // Gate by AcceptQueues only (AutoAcceptBg removed)
            if (!S.AcceptQueues) return;
            if (!ThrottleQueueAction(800)) return;

            try
            {
                Lua.DoString("for i=1, GetMaxBattlefieldID() do local status=GetBattlefieldStatus(i) if status=='confirm' then AcceptBattlefieldPort(i,1) end end if StaticPopup1 and StaticPopup1:IsVisible() and StaticPopup1.which=='CONFIRM_BATTLEFIELD_ENTRY' then if StaticPopup1Button1 then StaticPopup1Button1:Click() end end");
                if (S.AlertQueues) try { VitalicUi.ShowNotify("Entrée champ de bataille acceptée"); } catch { }
                Logger.Write("[QoL] BG accepté automatiquement.");
            }
            catch { }
        }

        private static void TryAutoReadyYes()
        {
            if (!VitalicSettings.Instance.AcceptQueues) return;
            if (!ThrottleQueueAction(800)) return;

            try
            {
                Lua.DoString("if ReadyCheckFrame and ReadyCheckFrame:IsShown() and ReadyCheckFrameYesButton and ReadyCheckFrameYesButton:IsEnabled() then ReadyCheckFrameYesButton:Click() end");
                Lua.DoString("if LFDRoleCheckPopup and LFDRoleCheckPopup:IsShown() and LFDRoleCheckPopupAcceptButton and LFDRoleCheckPopupAcceptButton:IsEnabled() then LFDRoleCheckPopupAcceptButton:Click() end");
                if (VitalicSettings.Instance.AlertQueues)
                    try { VitalicUi.ShowNotify("Ready/Role : accepté"); } catch { }
            }
            catch { }
        }

        // === Helpers ===
        private static bool IsHealerClass(WoWClass wowClass)
        {
            return wowClass == WoWClass.Priest
                || wowClass == WoWClass.Paladin
                || wowClass == WoWClass.Druid
                || wowClass == WoWClass.Shaman
                || wowClass == WoWClass.Monk;
        }

        public static bool TryResolvePsyfiend(double yards, out WoWUnit psy)
        {
            psy = null;
            try
            {
                foreach (var u in Styx.WoWInternals.ObjectManager.GetObjectsOfType<WoWUnit>(false, false))
                {
                    if (u == null || !u.IsValid || u.IsDead || !u.Attackable) continue;
                    if (u.CreatedBySpellId != 108921U) continue; // Psyfiend MoP
                    if (u.Distance > yards) continue;
                    psy = u;
                    _lastPsyfiendGuid = u.Guid;
                    _lastPsyfiendSeenAt = DateTime.UtcNow;
                    return true;
                }
            }
            catch { }
            return false;
        }
    }
}

