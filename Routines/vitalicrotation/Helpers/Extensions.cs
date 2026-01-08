using System;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace VitalicRotation.Helpers
{
    /// <summary>
    /// Helpers “à la Vitalic”. Pas de LINQ pour rester safe HB 5.4.8 et éviter toute ambiguïté.
    /// </summary>
    public static class Extensions
    {
        /// <summary>Énergie actuelle du joueur (MoP expose CurrentEnergy).</summary>
        public static int Energy(this LocalPlayer me)
        {
            try { return (me != null) ? (int)me.CurrentEnergy : 0; }
            catch
            {
                // Fallback Lua si jamais
                try { return Lua.GetReturnVal<int>("return UnitPower('player', 3) or 0", 0); }
                catch { return 0; }
            }
        }

        /// <summary>Temps restant (secondes) d’une aura par NOM. fromMyself = true => uniquement mes auras.</summary>
        public static double GetAuraTimeLeft(this WoWUnit unit, string auraName, bool fromMyself)
        {
            try
            {
                if (unit == null || !unit.IsValid || string.IsNullOrEmpty(auraName)) return 0.0;
                var auras = unit.GetAllAuras();
                int n = (auras != null) ? auras.Count : 0;
                for (int i = 0; i < n; i++)
                {
                    var a = auras[i];
                    if (a == null || a.Name == null) continue;
                    if (!a.Name.Equals(auraName, StringComparison.OrdinalIgnoreCase)) continue;
                    if (fromMyself && a.CreatorGuid != StyxWoW.Me.Guid) continue;
                    double s = a.TimeLeft.TotalSeconds;
                    return (s > 0) ? s : 0.0;
                }
            }
            catch { }
            return 0.0;
        }

        /// <summary>Temps restant (secondes) d’une aura par SpellId. fromMyself = true => uniquement mes auras.</summary>
        public static double GetAuraTimeLeft(this WoWUnit unit, int spellId, bool fromMyself)
        {
            try
            {
                if (unit == null || !unit.IsValid || spellId <= 0) return 0.0;
                var auras = unit.GetAllAuras();
                int n = (auras != null) ? auras.Count : 0;
                for (int i = 0; i < n; i++)
                {
                    var a = auras[i];
                    if (a == null) continue;
                    if (a.SpellId != spellId) continue;
                    if (fromMyself && a.CreatorGuid != StyxWoW.Me.Guid) continue;
                    double s = a.TimeLeft.TotalSeconds;
                    return (s > 0) ? s : 0.0;
                }
            }
            catch { }
            return 0.0;
        }

        /// <summary>Vrai si la cible a MON aura (par SpellId).</summary>
        public static bool HasMyAura(this WoWUnit unit, int spellId)
        {
            try
            {
                if (unit == null || !unit.IsValid || spellId <= 0) return false;
                var auras = unit.GetAllAuras();
                int n = (auras != null) ? auras.Count : 0;
                var meGuid = StyxWoW.Me.Guid;
                for (int i = 0; i < n; i++)
                {
                    var a = auras[i];
                    if (a == null) continue;
                    if (a.SpellId == spellId && a.CreatorGuid == meGuid)
                        return true;
                }
            }
            catch { }
            return false;
        }
    }
}
