using System;
using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Settings;
using VitalicRotation.Helpers;

namespace VitalicRotation.Helpers
{
    internal static class EnemyCountCache
    {
        private static DateTime _nextUpdateUtc = DateTime.MinValue;
        private static int _meleeCount, _aoe8Count, _aoe10Count;

        public static void UpdateIfDue(int reevaluateMs)
        {
            if (DateTime.UtcNow < _nextUpdateUtc) return;

            var me = StyxWoW.Me;
            if (me == null || !me.IsValid) 
            {
                _meleeCount = _aoe8Count = _aoe10Count = 0;
                return;
            }

            try
            {
                var units = ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                    .Where(u =>
                        u != null && u.IsValid && u.Attackable && !u.IsFriendly &&
                        !u.IsDead && u.Distance <= 12 &&
                        u.Guid != me.Guid);

                // Filtres optionnels si les extensions sont disponibles
                try
                {
                    // Tente d'utiliser l'extension HasCrowdControl si elle existe
                    var hasCcMethod = typeof(WoWUnit).GetMethod("HasCrowdControl");
                    if (hasCcMethod != null)
                    {
                        units = units.Where(u => {
                            try { return !(bool)hasCcMethod.Invoke(u, null); }
                            catch { return true; }
                        });
                    }
                }
                catch { /* extension pas disponible, on continue */ }

                try
                {
                    // Tente d'utiliser l'extension IsImmuneToPhysical si elle existe
                    var immuneMethod = typeof(WoWUnit).GetMethod("IsImmuneToPhysical");
                    if (immuneMethod != null)
                    {
                        units = units.Where(u => {
                            try { return !(bool)immuneMethod.Invoke(u, null); }
                            catch { return true; }
                        });
                    }
                }
                catch { /* extension pas disponible, on continue */ }

                var unitList = units.ToList();

                _meleeCount = unitList.Count(u => u.Distance <= 5);  // mêlée ~5y
                _aoe8Count  = unitList.Count(u => u.Distance <= 8);  // FoK range
                _aoe10Count = unitList.Count(u => u.Distance <= 10); // CT range
            }
            catch (Exception ex)
            {
                // En cas d'erreur, pas de changement brutal
                Logger.WriteException(ex, "EnemyCountCache.UpdateIfDue");
            }

            _nextUpdateUtc = DateTime.UtcNow.AddMilliseconds(Math.Max(100, reevaluateMs));
        }

        public static int Melee()  { return _meleeCount; }
        public static int Aoe8()   { return _aoe8Count; }
        public static int Aoe10()  { return _aoe10Count; }
    }
}