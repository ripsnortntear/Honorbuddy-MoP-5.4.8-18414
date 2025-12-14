using Bots.DungeonBuddy.Helpers;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using VitalicRotation.Helpers;

namespace VitalicRotation.Helpers
{
    public static class UnitExtensions
    {
        // Caches pour optimiser les appels répétés
        private static readonly Dictionary<ulong, bool> _losCache = new Dictionary<ulong, bool>();
        private static readonly Dictionary<ulong, double> _rangeCache = new Dictionary<ulong, double>();
        private static readonly HashSet<ulong> _blacklistGuids = new HashSet<ulong>();
        private static DateTime _lastCacheClear = DateTime.MinValue;

        /// <summary>
        /// Vide tous les caches d'unités (utilisé lors des resets).
        /// </summary>
        public static void ClearCaches()
        {
            _losCache.Clear();
            _rangeCache.Clear();
            _blacklistGuids.Clear();
            _lastCacheClear = DateTime.UtcNow;
        }

        // Name-based DR mapping removed. Use DRTracker (SpellId-based) exclusively as the authority.

        /// <summary>
        /// L'unité est-elle sous un CC "dur" connu ?
        /// </summary>
        public static bool IsCrowdControlled(this WoWUnit unit)
        {
            if (unit == null || !unit.IsAlive)
                return false;

            // Utilise DRTracker.HasActive au lieu de recherche par nom
            return DRTracker.HasActive(unit, DRTracker.DrCategory.Stun) ||
                   DRTracker.HasActive(unit, DRTracker.DrCategory.Incapacitate) ||
                   DRTracker.HasActive(unit, DRTracker.DrCategory.Disorient) ||
                   DRTracker.HasActive(unit, DRTracker.DrCategory.Fear) ||
                   DRTracker.HasActive(unit, DRTracker.DrCategory.Silence);
        }

        /// <summary>
        /// Catégorie DR pour un nom d'aura, si connue.
        /// </summary>
        [Obsolete("Use GetDrCategory(WoWAura) / DRTracker.GetCategory(SpellId) — name-based DR is not supported (see v.zip).")]
        public static DRTracker.DrCategory? GetDrCategory(string auraName)
        {
#if DEBUG
            // Fail-fast en debug pour éviter toute réintroduction par inadvertance
            throw new NotSupportedException("Name-based DR lookup is disabled. Use GetDrCategory(WoWAura) / DRTracker.GetCategory(SpellId).");
#else
            return null; // neutre en Release
#endif
        }

        // v.zip: DR par SpellId (tables HashSet<int>); pas de mapping par nom (localisation).
        /// <summary>
        /// Catégorie DR pour une aura, si connue (nouvelle méthode basée sur SpellId).
        /// </summary>
        public static DRTracker.DrCategory? GetDrCategory(WoWAura aura)
        {
            if (aura == null || !aura.IsActive)
                return null;
            return DRTracker.GetCategory(aura.SpellId);
        }

        /// <summary>
        /// L’unité a-t-elle une aura par nom exact ?
        /// </summary>
        public static bool HasAura(this WoWUnit unit, string auraName)
        {
            if (unit == null || !unit.IsValid || string.IsNullOrEmpty(auraName))
                return false;

            return unit.GetAllAuras().Any(a =>
                a != null &&
                a.Name == auraName &&
                a.IsActive &&
                a.TimeLeft.TotalMilliseconds > 0);
        }

        public static double GetAuraTimeLeft(this WoWUnit unit, string auraName)
        {
            if (unit == null || !unit.IsValid || string.IsNullOrEmpty(auraName))
                return 0;

            var aura = unit.GetAllAuras()
                           .FirstOrDefault(a => a != null && a.Name == auraName && a.IsActive);

            return aura != null ? aura.TimeLeft.TotalSeconds : 0;
        }

        /// <summary>
        /// Compte les ennemis autour de l’unité donnée.
        /// </summary>
        public static int CountEnemiesInRange(this WoWUnit unit, double range)
        {
            if (unit == null || !unit.IsValid)
                return 0;

            double rangeSqr = range * range;
            var center = unit.Location;

            return ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                .Count(u =>
                    u != null &&
                    u.IsValid &&
                    u.IsAlive &&
                    u.Attackable &&
                    u.Location.DistanceSqr(center) <= rangeSqr);
        }

        /// <summary>
        /// Pratique : compte autour du joueur local.
        /// </summary>
        public static int CountEnemiesInRange(double range)
        {
            var me = StyxWoW.Me;
            return me != null && me.IsValid ? me.CountEnemiesInRange(range) : 0;
        }

        /// <summary>
        /// Compte les alliés amicaux dans une portée (utile pour CD défensifs ou soins).
        /// </summary>
        public static int CountFriendlyInRange(this WoWUnit unit, double range)
        {
            if (unit == null || !unit.IsValid)
                return 0;

            double rangeSqr = range * range;
            var center = unit.Location;

            return ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                .Count(u =>
                    u != null &&
                    u.IsValid &&
                    u.IsAlive &&
                    u.IsFriendly &&
                    u.Location.DistanceSqr(center) <= rangeSqr);
        }

        /// <summary>
        /// Récupère les ennemis autour d’une unité.
        /// </summary>
        public static IEnumerable<WoWUnit> GetEnemiesInRange(this WoWUnit unit, double range)
        {
            if (unit == null || !unit.IsValid)
                return Enumerable.Empty<WoWUnit>();

            double rangeSqr = range * range;
            var center = unit.Location;

            return ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                .Where(u =>
                    u != null &&
                    u.IsValid &&
                    u.IsAlive &&
                    u.Attackable &&
                    u.Location.DistanceSqr(center) <= rangeSqr);
        }

        /// <summary>
        /// Nom sécurisé (évite les nullref en log).
        /// </summary>
        public static string SafeName(this WoWUnit unit)
        {
            return unit != null && unit.IsValid ? unit.Name : "Unknown";
        }
    }
}

