using System;
using System.Collections.Generic;
using Styx;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.UI;

namespace VitalicRotation.Managers
{
    public static class ImmunityGuard
    {
        // === Listes alignées sur v.zip ===
        // Immunités franches (éviter d’attaquer)
        private static readonly HashSet<int> HardImmunity = new HashSet<int>
        {
            642,     // Divine Shield
            1022,    // BoP / Blessing of Protection
            45438,   // Ice Block
            19263,   // Deterrence
            47585,   // Dispersion
            110700,  122465, 148467, 110617, 110715, 110696 // variants MoP relevées dans v.zip
        };

        // CC/états « immunisant » (ex: Cyclone) — inclut Cyclone comme demandé
        private static readonly HashSet<int> SpecialImmune = new HashSet<int>
        {
            33786,   // Cyclone (immune dégâts/soins)
            113506   // (liste courte v.zip)
        };

        // À éviter / fenêtres dangereuses côté cible (ex. Bladestorm)
        private static readonly HashSet<int> AvoidModes = new HashSet<int>
        {
            48792,   // Icebound Fortitude (réduction lourde / antistun)
            46924,   // Bladestorm
            51690,   // Killing Spree (cible saute)
            108201, 118009, 116849, 110575 // divers flags relevés
        };

        // Immunités spécifiques aux snares/roots
        private static readonly HashSet<int> SlowImmune = new HashSet<int>
        {
            1044,    // Hand of Freedom
            19574,   // Bestial Wrath
            46924,   // Bladestorm
            49039,   // Lichborne (partiel)
            111397,  // Blood Horror (éviter interactions)
        };

        // Réflexions/dangers projectiles
        private static readonly HashSet<int> ReflectDanger = new HashSet<int>
        {
            23920,   // Spell Reflection
            114028,  // Mass Spell Reflection
            122784,  // Cloak of Shadows (fortes résistances magiques)
        };

        private static DateTime _lastBanner = DateTime.MinValue;

        public static bool TargetIsEffectivelyImmune(WoWUnit target, bool includeAvoid = true)
        {
            if (target == null || !target.IsAlive) return false;

            try
            {
                var auras = target.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i];
                    if (a == null) continue;
                    int id = a.SpellId;
                    if (HardImmunity.Contains(id) || SpecialImmune.Contains(id))
                        return true;

                    if (includeAvoid && AvoidModes.Contains(id))
                        return true;
                }
            }
            catch
            {
                // Fallback via Auras.Values if GetAllAuras fails
                try
                {
                    foreach (var kv in target.Auras)
                    {
                        var a = kv.Value;
                        if (a == null) continue;
                        int id = a.SpellId;
                        if (HardImmunity.Contains(id) || SpecialImmune.Contains(id))
                            return true;
                        if (includeAvoid && AvoidModes.Contains(id))
                            return true;
                    }
                }
                catch { }
            }

            return false;
        }

        public static void HandleIfImmune(WoWUnit me, WoWUnit target, bool includeAvoid = true)
        {
            if (!TargetIsEffectivelyImmune(target, includeAvoid)) return;

            // Bannière "Target is immune" (throttle 10s comme v.zip)
            if ((DateTime.UtcNow - _lastBanner).TotalSeconds > 10)
            {
                try { VitalicUi.ShowBigBanner("Target is immune"); } catch { }
                _lastBanner = DateTime.UtcNow;
            }

            // v.zip : SpellCancelQueuedSpell + StopAttack
            try { LuaHelper.CancelQueuedSpellAndStopAttack(); } catch { /* silencieux */ }
        }

        // Helpers complémentaires pour la logique de snare
        public static bool IsSlowImmune(WoWUnit target)
        {
            if (target == null || !target.IsAlive) return false;
            try
            {
                var auras = target.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i]; if (a == null) continue;
                    if (SlowImmune.Contains(a.SpellId) || AvoidModes.Contains(a.SpellId))
                        return true;
                }
            }
            catch { }
            return false;
        }

        public static bool IsReflectRisky(WoWUnit target)
        {
            if (target == null || !target.IsAlive) return false;
            try
            {
                var auras = target.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i]; if (a == null) continue;
                    if (ReflectDanger.Contains(a.SpellId))
                        return true;
                }
            }
            catch { }
            return false;
        }
    }
}
