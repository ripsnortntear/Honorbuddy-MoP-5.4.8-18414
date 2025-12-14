using System;
using System.Collections.Generic;
using System.Linq;
using Styx.WoWInternals; // ObjectManager
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    public enum DRCategory { Stun, Incapacitate, Disorient, Silence, Root, Horror, Fear }
    public enum DRState { None, Half, Quarter, Immune }

    public static class DRTracker
    {
        // === Compat rétro ===
        public enum DrCategory
        {
            Stun,
            Incapacitate,
            Disorient,
            Fear,
            Horror,
            Silence,
            Root,
            Cyclone,
            Disarm,
            Charm,
            Sleep,
            Taunt,
            Knockback,
            Scatter,
            DragonBreath,
            Frozen,
            RandomStun,
            RandomRoot
        }

        // === Snapshot UI attendu par DRTrackerForm ===
        public class DrSnapshotRow
        {
            public string TargetName { get; set; }
            public ulong TargetGuid { get; set; }
            public string Category { get; set; } // string (UI fait "??" dessus)
            public int Stack { get; set; }
            public string TimeLeft { get; set; } // déjà formaté "Xs"
            public DRState State { get; set; }
            public DateTime LastAppliedUtc { get; set; }
        }

        // Fenêtre DR (MoP)
        private static readonly TimeSpan Window = TimeSpan.FromSeconds(18);
        private static readonly TimeSpan WindowStun = TimeSpan.FromSeconds(19); // padding like v.zip

        // ===== v.zip — Tables DR par SpellId (MoP 5.4.8) =====

        // STUN (hashSet_5) + Paralytic Stun (113953, hashSet_12)
        private static readonly HashSet<int> DR_STUN = new HashSet<int>
        {
            108194,115001,91800,47481,91797,102795,22570,5211,9005,110698,117526,24394,
            118271,44572,119381,105593,853,64044,1833,408,30283,89766,132168,46968,
            105771,20549,118905,107570,145585,132169,120086,119392,115752,96201,126355,126423,
            50519,56626,90337,113953
        };

        // INCAPACITATE (hashSet_10) – Poly/Sap/Hex/Wyvern/Repentance…
        private static readonly HashSet<int> DR_INCAP = new HashSet<int>
        {
            3355,6770,1776,51514,19386,118,61305,28272,61721,61780,28271,82691,115078,20066
        };

        // DISORIENT (hashSet_11) – DB/Scatter/Chastise/Roar(99)
        private static readonly HashSet<int> DR_DISORIENT = new HashSet<int>
        {
            19503,99,31661,88625
        };

        // FEAR / HORROR (+ Blind) (hashSet_9)
        private static readonly HashSet<int> DR_FEAR = new HashSet<int>
        {
            113004,113056,5246,20511,1513,145067,8122,113792,2094,137143,5782,118699,
            5484,132412,115268,6358,105421
        };

        // SILENCE (hashSet_7)
        private static readonly HashSet<int> DR_SILENCE = new HashSet<int>
        {
            47476,78675,81261,34490,55021,102051,116709,31935,15487,1330,24259,115782,
            18498,25046,28730,50613,69179,80483
        };

        // ROOT (hashSet_6)
        private static readonly HashSet<int> DR_ROOT = new HashSet<int>
        {
            96294,91807,339,19975,113770,45334,102359,110693,19185,136634,50245,54706,
            4167,122,111340,116706,113275,123407,113275,87194,114404,115197,64695,63685,
            107566,39965,55536,13099,33395
        };

        // DISARM (hashSet_8)
        private static readonly HashSet<int> DR_DISARM = new HashSet<int>
        {
            91644,50541,117368,140023,64058,51722,118093,676
        };

        // Liste "trinketable / hard CC" (hashSet_13) — utilisée côté original pour tests globaux
        private static readonly HashSet<int> DR_TRINKET_CC = new HashSet<int>
        {
            115001,91800,91797,102795,5211,110698,853,44572,1833,408,113953,30283,
            89766,132168,105771,50245,54706,4167,19503,126246,96201,136634,7922,117526,
            107570,132169,111397,6789,19386
        };

        // ===== v.zip — Unions (hashSet_0..4) =====

        // Incapacitate ∪ Disorient
        private static readonly HashSet<int> DR_INCAP_OR_DISORIENT =
            new HashSet<int>(DR_INCAP.Concat(DR_DISORIENT));

        // Hard CC (sans Disarm): Stun ∪ Incapacitate ∪ Disorient ∪ Fear/Horror
        private static readonly HashSet<int> DR_HARDCC_NO_DISARM =
            new HashSet<int>(DR_STUN.Concat(DR_INCAP).Concat(DR_DISORIENT).Concat(DR_FEAR));

        // Hard CC (avec Disarm)
        private static readonly HashSet<int> DR_HARDCC_WITH_DISARM =
            new HashSet<int>(DR_HARDCC_NO_DISARM.Concat(DR_DISARM));

        // Contrôle "général" (v.zip regroupe ici Fear/Horror ∪ Root ∪ Disarm ∪ liste trinketable)
        private static readonly HashSet<int> DR_CONTROL_GENERAL =
            new HashSet<int>(DR_FEAR.Concat(DR_ROOT).Concat(DR_DISARM).Concat(DR_TRINKET_CC));

        // Guid -> Catégorie -> timestamps d'application
        private static readonly Dictionary<ulong, Dictionary<DrCategory, List<DateTime>>> _hits
            = new Dictionary<ulong, Dictionary<DrCategory, List<DateTime>>>();

        // Simple touch map (last applied per unit/category) for quick window checks
        private static readonly Dictionary<string, DateTime> _lastApplied = new Dictionary<string, DateTime>();
        private static string GetKey(WoWUnit u, DRCategory cat)
        {
            if (u == null) return string.Empty;
            return u.Guid.ToString("X") + ":" + cat.ToString();
        }

        // ======== Public API utilitaire ========

        public static void Cleanup()
        {
            Reset();
        }

        public static void Reset()
        {
            _hits.Clear();
            _lastApplied.Clear();
        }

        /// <summary>
        /// Purge les entrées DR expirées (pour éviter l'accumulation de données obsolètes).
        /// Utilisé lors des resets "doux" (changement de zone, sortie de combat, etc.).
        /// </summary>
        public static void PruneExpired()
        {
            DateTime now = DateTime.UtcNow;
            var keysToRemove = new List<ulong>();

            foreach (var kv in _hits)
            {
                ulong guid = kv.Key;
                var byCat = kv.Value;
                var catsToRemove = new List<DRTracker.DrCategory>();

                foreach (var kv2 in byCat)
                {
                    DRTracker.DrCategory cat = kv2.Key;
                    List<DateTime> times = kv2.Value;
                    if (times != null)
                    {
                        Prune(times, now);
                        if (times.Count == 0)
                            catsToRemove.Add(cat);
                    }
                }

                // Supprime les catégories vides
                foreach (var cat in catsToRemove)
                    byCat.Remove(cat);

                // Si plus de catégories pour ce GUID, marque pour suppression
                if (byCat.Count == 0)
                    keysToRemove.Add(guid);
            }

            // Supprime les GUIDs vides
            foreach (var guid in keysToRemove)
                _hits.Remove(guid);

            // Purge aussi _lastApplied des entrées expirées
            var lastAppliedKeysToRemove = new List<string>();
            foreach (var kv in _lastApplied)
            {
                if ((now - kv.Value) > Window)
                    lastAppliedKeysToRemove.Add(kv.Key);
            }

            foreach (var key in lastAppliedKeysToRemove)
                _lastApplied.Remove(key);
        }

        public static bool IsImmune(WoWUnit unit, DrCategory cat)
        {
            return GetState(unit, cat) == DRState.Immune;
        }

        public static bool IsImmune(WoWUnit unit, DRCategory cat)
        {
            return GetState(unit, cat) == DRState.Immune;
        }

        // Quick touch/reset helpers (18s window, ±1s tolerance already handled for stun via WindowStun)
        public static void Touch(WoWUnit u, DRCategory cat)
        {
            var key = GetKey(u, cat);
            if (string.IsNullOrEmpty(key)) return;
            _lastApplied[key] = DateTime.UtcNow;
        }

        public static bool CanApply(WoWUnit u, DRCategory cat)
        {
            var key = GetKey(u, cat);
            if (string.IsNullOrEmpty(key)) return true;
            DateTime t;
            if (!_lastApplied.TryGetValue(key, out t)) return true;
            return (DateTime.UtcNow - t) > Window;
        }

        // ======== SURCHARGES GUID (compat TeamCCManager) ========

        public static bool IsImmune(ulong guid, DrCategory cat)
        {
            return GetState(guid, cat) == DRState.Immune;
        }

        public static bool IsImmune(ulong guid, DRCategory cat)
        {
            return GetState(guid, ToDr(cat)) == DRState.Immune;
        }

        public static DRState GetState(ulong guid, DrCategory cat)
        {
            DateTime now = DateTime.UtcNow;
            DrCategory ncat = Normalize(cat);
            List<DateTime> list = GetList(guid, ncat, false);
            if (list == null || list.Count == 0) return DRState.None;
            Prune(list, now);

            int n = list.Count;
            if (n <= 0) return DRState.None;
            if (n == 1) return DRState.Half;
            if (n == 2) return DRState.Quarter;
            return DRState.Immune;
        }

        public static DRState GetState(ulong guid, DRCategory cat)
        {
            return GetState(guid, ToDr(cat));
        }

        public static void Applied(ulong guid, DrCategory cat)
        {
            DateTime now = DateTime.UtcNow;
            DrCategory ncat = Normalize(cat);
            List<DateTime> list = GetList(guid, ncat, true);
            Prune(list, now);
            list.Add(now);
        }

        public static void Applied(ulong guid, DRCategory cat)
        {
            Applied(guid, ToDr(cat));
        }

        public static bool Can(ulong guid, DrCategory cat)
        {
            return GetState(guid, cat) != DRState.Immune;
        }

        public static bool Can(ulong guid, DRCategory cat)
        {
            return GetState(guid, ToDr(cat)) != DRState.Immune;
        }

        // ======== Snapshot pour l’UI ========

        public static System.Collections.Generic.List<DrSnapshotRow> GetSnapshot()
        {
            DateTime now = DateTime.UtcNow;
            var rows = new System.Collections.Generic.List<DrSnapshotRow>();

            foreach (var kv in _hits)
            {
                ulong guid = kv.Key;
                var byCat = kv.Value;

                WoWUnit unit = ObjectManager.GetObjectByGuid<WoWUnit>(guid);
                string name = (unit != null && unit.IsValid) ? unit.Name : "(unknown)";

                foreach (var kv2 in byCat)
                {
                    DrCategory cat = kv2.Key;
                    List<DateTime> times = kv2.Value;
                    if (times == null || times.Count == 0) continue;

                    Prune(times, now);
                    if (times.Count == 0) continue;

                    DateTime last = times[times.Count - 1];
                    TimeSpan rem = Window - (now - last);
                    if (rem.TotalSeconds < 0) rem = TimeSpan.Zero;

                    int n = times.Count;
                    DRState state = DRState.None;
                    if (n == 1) state = DRState.Half;
                    else if (n == 2) state = DRState.Quarter;
                    else if (n >= 3) state = DRState.Immune;

                    var row = new DrSnapshotRow();
                    row.TargetName = name;
                    row.TargetGuid = guid;
                    row.Category = cat.ToString();
                    row.Stack = n;
                    row.TimeLeft = string.Format("{0:0.0}s", rem.TotalSeconds);
                    row.State = state;
                    row.LastAppliedUtc = last;

                    rows.Add(row);
                }
            }

            return rows;
        }

        // ======== API principale (unit) ========

        public static void Applied(WoWUnit unit, DrCategory cat)
        {
            if (unit == null || !unit.IsValid) return;
            DateTime now = DateTime.UtcNow;
            DrCategory ncat = Normalize(cat);
            List<DateTime> list = GetList(unit.Guid, ncat, true);
            Prune(list, now);
            list.Add(now);
        }

        // Surcharge compat : certains appels inversent (cat, unit)
        public static void Applied(DrCategory cat, WoWUnit unit)
        {
            Applied(unit, cat);
        }

        public static void Applied(WoWUnit unit, DRCategory cat)
        {
            Applied(unit, ToDr(cat));
        }

        public static DRState GetState(WoWUnit unit, DrCategory cat)
        {
            if (unit == null || !unit.IsValid) return DRState.None;
            DateTime now = DateTime.UtcNow;
            DrCategory ncat = Normalize(cat);
            List<DateTime> list = GetList(unit.Guid, ncat, false);
            if (list == null || list.Count == 0) return DRState.None;
            Prune(list, now);

            int n = list.Count;
            if (n <= 0) return DRState.None;
            if (n == 1) return DRState.Half;
            if (n == 2) return DRState.Quarter;
            return DRState.Immune;
        }

        public static DRState GetState(WoWUnit unit, DRCategory cat)
        {
            return GetState(unit, ToDr(cat));
        }

        public static bool Can(WoWUnit unit, DrCategory cat)
        {
            return GetState(unit, cat) != DRState.Immune;
        }

        public static bool Can(WoWUnit unit, DRCategory cat)
        {
            return GetState(unit, cat) != DRState.Immune;
        }

        public static bool CanStun(WoWUnit unit) { return Can(unit, DRCategory.Stun); }
        public static bool CanDisorient(WoWUnit unit) { return Can(unit, DRCategory.Disorient); }
        public static bool CanIncap(WoWUnit unit) { return Can(unit, DRCategory.Incapacitate); }

        // Retourne la catégorie DR d'un spellId si connue (null sinon)
        public static DrCategory? GetCategory(int spellId)
        {
            if (DR_STUN.Contains(spellId))      return DrCategory.Stun;
            if (DR_INCAP.Contains(spellId))     return DrCategory.Incapacitate;
            if (DR_DISORIENT.Contains(spellId)) return DrCategory.Disorient;
            if (DR_FEAR.Contains(spellId))      return DrCategory.Fear;
            if (DR_SILENCE.Contains(spellId))   return DrCategory.Silence;
            if (DR_ROOT.Contains(spellId))      return DrCategory.Root;
            if (DR_DISARM.Contains(spellId))    return DrCategory.Disarm;
            return null;
        }

        // Vrai si l'unité a actuellement une aura d'une catégorie DR donnée (par SpellId)
        public static bool HasActive(WoWUnit unit, DrCategory cat)
        {
            if (unit == null || !unit.IsValid) return false;

            foreach (var a in unit.GetAllAuras())
            {
                if (a == null || !a.IsActive) continue;

                var c = GetCategory(a.SpellId);
                if (!c.HasValue) continue;

                // Normalise (Scatter/DB -> Disorient, Frozen -> Root, etc.)
                var norm = Normalize(c.Value);
                if (norm == cat)
                    return true;
            }
            return false;
        }

        // Alias "nom" si nécessaire pour compatibilité
        public static DrCategory? GetCategoryByAura(WoWUnit unit, WoWAura aura)
        {
            return aura != null ? GetCategory(aura.SpellId) : (DrCategory?)null;
        }

        // Test "trinketable CC" (v.zip hashSet_13)
        public static bool IsTrinketableCc(int spellId)
        {
            return DR_TRINKET_CC.Contains(spellId);
        }

        // Variante aura → bool (overload v.zip)
        public static bool IsTrinketableCc(WoWAura aura)
        {
            return aura != null && aura.IsActive && DR_TRINKET_CC.Contains(aura.SpellId);
        }

        // Helpers publics v.zip (unions DR)
        public static bool HasIncapOrDisorient(WoWUnit unit)
        {
            return HasAny(unit, DR_INCAP_OR_DISORIENT);
        }

        public static bool HasHardCcNoDisarm(WoWUnit unit)
        {
            return HasAny(unit, DR_HARDCC_NO_DISARM);
        }

        public static bool HasHardCcWithDisarm(WoWUnit unit)
        {
            return HasAny(unit, DR_HARDCC_WITH_DISARM);
        }

        public static bool HasControlGeneral(WoWUnit unit)
        {
            return HasAny(unit, DR_CONTROL_GENERAL);
        }

        // ======== New: DR helpers for UI (stack + remaining) ========

        public static int GetDR(WoWUnit unit, DRCategory cat)
        {
            if (unit == null || !unit.IsValid) return 0;
            DateTime now = DateTime.UtcNow;
            var list = GetList(unit.Guid, ToDr(cat), false);
            if (list == null) return 0;
            Prune(list, now);
            return list.Count;
        }

        public static int GetDR(ulong guid, DRCategory cat)
        {
            DateTime now = DateTime.UtcNow;
            var list = GetList(guid, ToDr(cat), false);
            if (list == null) return 0;
            Prune(list, now);
            return list.Count;
        }

        public static double Remaining(WoWUnit unit, DRCategory cat)
        {
            if (unit == null || !unit.IsValid) return 0.0;
            return Remaining(unit.Guid, cat);
        }

        public static double Remaining(ulong guid, DRCategory cat)
        {
            DateTime now = DateTime.UtcNow;
            var list = GetList(guid, ToDr(cat), false);
            if (list == null || list.Count == 0) return 0.0;
            Prune(list, now);
            if (list.Count == 0) return 0.0;
            DateTime last = list[list.Count - 1];
            double rem = (Window - (now - last)).TotalSeconds;
            return rem > 0 ? rem : 0.0;
        }

        // ======== Internes ========

        // Track last spellId per target/category (for HUD/diagnostic)
        private static readonly Dictionary<ulong, Dictionary<DrCategory, int>> _lastSpellId
            = new Dictionary<ulong, Dictionary<DrCategory, int>>();

        private static List<DateTime> GetList(ulong guid, DrCategory cat, bool create)
        {
            Dictionary<DrCategory, List<DateTime>> byCat;
            if (!_hits.TryGetValue(guid, out byCat))
            {
                if (!create) return null;
                byCat = new Dictionary<DrCategory, List<DateTime>>();
                _hits[guid] = byCat;
            }

            List<DateTime> list;
            if (!byCat.TryGetValue(cat, out list))
            {
                if (!create) return null;
                list = new List<DateTime>(3);
                byCat[cat] = list;
            }
            return list;
        }

        private static void Prune(List<DateTime> list, DateTime now)
        {
            if (list == null) return;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (now - list[i] > Window)
                    list.RemoveAt(i);
            }
        }

        private static void SetLastSpell(ulong guid, DrCategory cat, int spellId)
        {
            Dictionary<DrCategory, int> byCat;
            if (!_lastSpellId.TryGetValue(guid, out byCat))
            {
                byCat = new Dictionary<DrCategory, int>();
                _lastSpellId[guid] = byCat;
            }
            byCat[Normalize(cat)] = spellId;
        }

        private static int GetLastSpell(ulong guid, DrCategory cat)
        {
            Dictionary<DrCategory, int> byCat;
            if (_lastSpellId.TryGetValue(guid, out byCat))
            {
                int id;
                if (byCat.TryGetValue(Normalize(cat), out id)) return id;
            }
            return 0;
        }

        private static DrCategory ToDr(DRCategory cat)
        {
            switch (cat)
            {
                case DRCategory.Stun: return DrCategory.Stun;
                case DRCategory.Incapacitate: return DrCategory.Incapacitate;
                case DRCategory.Disorient: return DrCategory.Disorient;
                case DRCategory.Silence: return DrCategory.Silence;
                case DRCategory.Root: return DrCategory.Root;
                case DRCategory.Horror: return DrCategory.Horror;
                default: return DrCategory.Stun;
            }
        }

        private static DrCategory Normalize(DrCategory cat)
        {
            switch (cat)
            {
                case DrCategory.Scatter: return DrCategory.Disorient;
                case DrCategory.DragonBreath: return DrCategory.Disorient;
                case DrCategory.Frozen: return DrCategory.Root;
                case DrCategory.RandomStun: return DrCategory.Stun;
                case DrCategory.RandomRoot: return DrCategory.Root;
                case DrCategory.Sleep: return DrCategory.Incapacitate; // Poly/Hex/Sleep
                case DrCategory.Cyclone: return DrCategory.Disorient;  // treat Cyclone as Disorient for unified DR gating
                default: return cat;
            }
        }

        private static bool HasAny(WoWUnit unit, HashSet<int> set)
        {
            if (unit == null || !unit.IsValid) return false;
            foreach (var a in unit.GetAllAuras())
            {
                if (a == null || !a.IsActive) continue;
                if (set.Contains(a.SpellId))
                    return true;
            }
            return false;
        }

        // ======== New: Detailed API like v.zip (smethod_6 / smethod_5) ========

        public struct DrInfo
        {
            public bool Active;         // any DR hits within window
            public double NextFactor;   // 1.0, 0.5, 0.25, 0.0
            public TimeSpan Remaining;  // time until reset
            public int LastSpellId;     // last spell that bumped this DR
        }

        public static DrInfo GetInfo(WoWUnit unit, DrCategory cat)
        {
            var info = new DrInfo { Active = false, NextFactor = 1.0, Remaining = TimeSpan.Zero, LastSpellId = 0 };
            if (unit == null || !unit.IsValid) return info;

            DateTime now = DateTime.UtcNow;
            var ncat = Normalize(cat);
            var list = GetList(unit.Guid, ncat, false);
            if (list != null)
            {
                Prune(list, now);
                int n = list.Count;
                info.Active = (n > 0);
                if (n <= 0) info.NextFactor = 1.0;
                else if (n == 1) info.NextFactor = 0.5;
                else if (n == 2) info.NextFactor = 0.25;
                else info.NextFactor = 0.0;

                if (n > 0)
                {
                    var last = list[n - 1];
                    var win = (ncat == DrCategory.Stun) ? WindowStun : Window;
                    var rem = win - (now - last);
                    if (rem < TimeSpan.Zero) rem = TimeSpan.Zero;
                    info.Remaining = rem;
                }
            }
            info.LastSpellId = GetLastSpell(unit.Guid, ncat);
            return info;
        }

        public static bool CanApply(WoWUnit unit, DrCategory cat, double minFactor)
        {
            // true if next application will have at least minFactor effectiveness
            var i = GetInfo(unit, cat);
            return i.NextFactor >= minFactor;
        }

        public static bool CanApplyStun(WoWUnit unit, double minFactor)
        {
            return CanApply(unit, DrCategory.Stun, minFactor);
        }

        public static bool CanApplyIncap(WoWUnit unit, double minFactor)
        {
            return CanApply(unit, DrCategory.Incapacitate, minFactor);
        }

        public static bool CanApplyDisorient(WoWUnit unit, double minFactor)
        {
            return CanApply(unit, DrCategory.Disorient, minFactor);
        }

        // Overloads without factor (legacy) keep "not immune" semantics
        // Bump API with spellId (v.zip Class68.smethod_8)
        public static void Applied(WoWUnit unit, DrCategory cat, int spellId)
        {
            if (unit == null || !unit.IsValid) return;
            DateTime now = DateTime.UtcNow;
            var ncat = Normalize(cat);
            var list = GetList(unit.Guid, ncat, true);
            Prune(list, now);
            list.Add(now);
            SetLastSpell(unit.Guid, ncat, spellId);
        }

        // ================= Stable skeleton API (string-guid based) =================
        private struct DrSlot { public DRState state; public DateTime until; }

        // dstGuid -> cat -> state + expiry
        private static readonly Dictionary<string, Dictionary<DRCategory, DrSlot>> _byTarget
            = new Dictionary<string, Dictionary<DRCategory, DrSlot>>();

        // Minimal MoP mapping (extend elsewhere as needed)
        private static readonly Dictionary<int, DRCategory> _spellToCat = new Dictionary<int, DRCategory>
        {
            // Stuns
            { 408, DRCategory.Stun },      // Kidney Shot
            { 1833, DRCategory.Stun },     // Cheap Shot
            { 853, DRCategory.Stun },      // Hammer of Justice
            { 44572, DRCategory.Stun },    // Deep Freeze
            { 5211, DRCategory.Stun },     // Bash / Mighty Bash
            { 30283, DRCategory.Stun },    // Shadowfury
            { 46968, DRCategory.Stun },    // Shockwave
            { 107570, DRCategory.Stun },   // Storm Bolt
            { 19577, DRCategory.Stun },    // Intimidation (pet)
            { 105593, DRCategory.Stun },   // Fist of Justice
            { 119381, DRCategory.Stun },   // Leg Sweep
            { 22570, DRCategory.Stun },    // Maim (Feral)

            // Incapacitate (Sap / Poly / Hex / Wyvern / Paralysis / Repentance / Freezing Trap)
            { 6770, DRCategory.Incapacitate }, // Sap
            { 118,  DRCategory.Incapacitate }, // Polymorph
            { 61305, DRCategory.Incapacitate }, // Polymorph (cat)
            { 28272, DRCategory.Incapacitate }, // Polymorph (pig)
            { 28271, DRCategory.Incapacitate }, // Polymorph (turtle)
            { 61721, DRCategory.Incapacitate }, // Polymorph (rabbit)
            { 61780, DRCategory.Incapacitate }, // Polymorph (turkey)
            { 51514, DRCategory.Incapacitate }, // Hex (mapped sometimes as Disorient but treat incapacitate)
            { 19386, DRCategory.Incapacitate }, // Wyvern Sting (sleep/incap phase)
            { 3355, DRCategory.Incapacitate },  // Freezing Trap
            { 20066, DRCategory.Incapacitate }, // Repentance
            { 115078, DRCategory.Incapacitate }, // Paralysis
            { 82691, DRCategory.Incapacitate }, // Ring of Frost (incap phase)
            { 99, DRCategory.Incapacitate },    // Disorienting Roar (treated as disorient/incap hybrid)

            // Disorient (Blind / Fear variants sometimes here; keep pure disorients)
            // Removed duplicate 2094 mapping here to avoid duplicate key; Blind handled as Fear below per original parity
            { 31661, DRCategory.Disorient }, // Dragon's Breath
            { 19503, DRCategory.Disorient }, // Scatter Shot
            { 88625, DRCategory.Disorient }, // Holy Word: Chastise (disorient effect)

            // Fear / Horror
            { 5782, DRCategory.Fear },     // Fear (Warlock)
            { 118699, DRCategory.Fear },   // Fear (new id)
            { 8122, DRCategory.Fear },     // Psychic Scream
            { 5484, DRCategory.Fear },     // Howl of Terror
            { 5246, DRCategory.Fear },     // Intimidating Shout
            { 2094, DRCategory.Fear },     // Blind (treated as Fear DR in original
        };

        private static readonly TimeSpan DrWindow = TimeSpan.FromSeconds(18);
        private static TimeSpan GetCatWindow(DRCategory cat) { return cat == DRCategory.Stun ? TimeSpan.FromSeconds(19) : DrWindow; }

        // === Extended exhaustive mapping builder (E24) ===
        static DRTracker()
        {
            try { EnsureExpandedMapping(); } catch { }
        }

        private static void EnsureExpandedMapping()
        {
            // Build exhaustive mapping from existing DR hash sets to _spellToCat (only add if missing)
            Action<IEnumerable<int>, DRCategory> addRange = (ids, cat) =>
            {
                foreach (var id in ids)
                {
                    if (id <= 0) continue;
                    if (!_spellToCat.ContainsKey(id))
                        _spellToCat[id] = cat;
                }
            };
            addRange(DR_STUN, DRCategory.Stun);
            addRange(DR_INCAP, DRCategory.Incapacitate);
            addRange(DR_DISORIENT, DRCategory.Disorient);
            addRange(DR_FEAR, DRCategory.Fear); // Horror handled as Fear category here (MoP grouping)
            addRange(DR_SILENCE, DRCategory.Silence);
            addRange(DR_ROOT, DRCategory.Root);
            // Disarm has no DRCategory counterpart in this condensed model – ignored.

            // Special cases not in sets or needing explicit clarity
            if (!_spellToCat.ContainsKey(33786)) _spellToCat[33786] = DRCategory.Disorient; // Cyclone (own DR, approximated to Disorient for gating)
            if (!_spellToCat.ContainsKey(64044)) _spellToCat[64044] = DRCategory.Fear;      // Psychic Horror (treat with Fear DR for gating simplification)
        }

        public static void OnAuraApplied(string dstGuid, int spellId)
        {
            if (dstGuid == null) return;
            DRCategory cat;
            if (!_spellToCat.TryGetValue(spellId, out cat)) return;

            var now = DateTime.UtcNow;
            Dictionary<DRCategory, DrSlot> map;
            if (!_byTarget.TryGetValue(dstGuid, out map))
            {
                map = new Dictionary<DRCategory, DrSlot>();
                _byTarget[dstGuid] = map;
            }

            DrSlot cur;
            if (!map.TryGetValue(cat, out cur))
                cur = new DrSlot { state = DRState.None, until = DateTime.MinValue };

            // Reset if expired
            if (now > cur.until) cur.state = DRState.None;

            // Step up DR
            switch (cur.state)
            {
                case DRState.None:    cur.state = DRState.Half;    break;
                case DRState.Half:    cur.state = DRState.Quarter; break;
                case DRState.Quarter: cur.state = DRState.Immune;  break;
                case DRState.Immune:  cur.state = DRState.Immune;  break;
            }

            cur.until = now + GetCatWindow(cat);
            map[cat] = cur;
        }

        public static void OnAuraRemoved(string dstGuid, int spellId)
        {
            // No-op: state naturally expires; optional tweak could reduce 'until' slightly.
        }

        public static DRState GetState(WoWUnitLike unit, DRCategory cat)
        {
            return GetState(unit != null ? unit.Guid : null, cat);
        }

        public static DRState GetState(string dstGuid, DRCategory cat)
        {
            if (dstGuid == null) return DRState.None;
            Dictionary<DRCategory, DrSlot> map;
            if (!_byTarget.TryGetValue(dstGuid, out map)) return DRState.None;

            DrSlot v;
            if (!map.TryGetValue(cat, out v)) return DRState.None;

            var now = DateTime.UtcNow;
            if (now > v.until) return DRState.None;
            return v.state;
        }

        public static bool IsHardCCImmune(WoWUnitLike unit)
        {
            if (unit == null) return false;
            var g = unit.Guid;
            return GetState(g, DRCategory.Stun) == DRState.Immune
                || GetState(g, DRCategory.Incapacitate) == DRState.Immune
                || GetState(g, DRCategory.Disorient) == DRState.Immune
                || GetState(g, DRCategory.Fear) == DRState.Immune
                || GetState(g, DRCategory.Horror) == DRState.Immune;
        }

        // AoE suppression: y a-t-il un CC fragile allié proche ? (radius unused here; provider decides)
        public static bool HasFriendlyHardCcNearby(Func<IEnumerable<WoWUnitLike>> alliesProvider, float radius)
        {
            if (alliesProvider == null) return false;
            foreach (var a in alliesProvider())
            {
                if (a == null) continue;
                if (GetState(a.Guid, DRCategory.Incapacitate) != DRState.None) return true;
                if (GetState(a.Guid, DRCategory.Disorient) != DRState.None)     return true;
                if (GetState(a.Guid, DRCategory.Fear) != DRState.None)          return true;
            }
            return false;
        }

    }

    // Petit wrapper pour éviter de référencer HB dans le helper
    public sealed class WoWUnitLike
    {
        public string Guid { get; set; }
        public float Distance { get; set; }
        public bool IsFriendly { get; set; }
    }
}
