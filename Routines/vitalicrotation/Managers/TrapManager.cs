using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    internal static class TrapManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        // v.zip : Liste des totems/objets ciblés (exact v.zip)
        private static readonly HashSet<string> StompNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Shaman
            "Grounding Totem",
            "Capacitor Totem",
            "Spirit Link Totem",
            "Healing Tide Totem",
            "Healing Stream Totem",
            "Windwalk Totem",
            "Earthgrab Totem",

            // Hunter traps (objets/auras qu'on voit en MoP)
            "Explosive Trap",
            "Freezing Trap",
            "Snake Trap",

            // Divers (selon v.zip)
            "Ring of Frost",     // mage (objet au sol)
            "Lightwell",         // prêtre (cible prioritaire utilitaire)
        };

        // Mapping v.zip : CreatedBySpellId -> Flag (Totems) - gardé pour compatibilité settings
        private static readonly Dictionary<int, Totems> _totemByCreatedSpell = new Dictionary<int, Totems>
        {
            { 98008 , Totems.SpiritLink    }, // Spirit Link Totem
            { 51485 , Totems.Earthgrab     }, // Earthgrab Totem
            { 5394  , Totems.HealingStream }, // Healing Stream Totem
            { 108280, Totems.HealingTide   }, // Healing Tide Totem
            { 108269, Totems.Capacitor     }, // Capacitor Totem
            { 8177  , Totems.Grounding     }, // Grounding Totem
            { 108273, Totems.Windwalk      }, // Windwalk Totem
        };

        // Dernier stomp par type (évite le spam)
        private static readonly Dictionary<Totems, DateTime> _lastStomp = new Dictionary<Totems, DateTime>();

        // Petit composite utilisable dans la BT
        public static Composite Build()
        {
            return new Action(ret =>
            {
                Execute();
                return RunStatus.Failure;
            });
        }

        public static void Execute()
        {
            if (Me == null || !Me.IsAlive)
                return;

            // Pause offense: éviter le spam pendant un cast manuel récent
            if (EventHandlers.ShouldPauseOffense()) return;

            // Throttle léger comme v.zip
            if (!Throttle.Check("Trap.Stomp", 250))
                return;

            var me = StyxWoW.Me;

            // 1) Stomp objets/GO
            var go = ObjectManager.GetObjectsOfType<WoWGameObject>()
                .FirstOrDefault(o =>
                    o != null && o.IsValid &&
                    StompNames.Contains(o.Name) &&
                    o.Distance <= 8.0);

            if (go != null)
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                {
                    try { Logger.Write("[Diag][Totem] GameObject detected: " + go.Name + " d=" + go.Distance.ToString("0.0")); } catch { }
                }
                if (SpellBook.CanCast(SpellBook.FanOfKnives))
                {
                    Logger.Write("[Totem] Stomp GameObject -> " + go.Name + " (Fan of Knives)");
                    SpellBook.Cast(SpellBook.FanOfKnives);
                    return;
                }
            }

            // 2) Stomp unités (certains totems sont des unités)
            var u = ObjectManager.GetObjectsOfType<WoWUnit>()
                .FirstOrDefault(x =>
                    x != null && x.IsValid && !x.IsDead &&
                    StompNames.Contains(x.Name) &&
                    x.Distance <= 8.0);

            if (u != null)
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                {
                    try { Logger.Write("[Diag][Totem] Unit detected: " + u.Name + " d=" + u.Distance.ToString("0.0")); } catch { }
                }
                if (SpellBook.CanCast(SpellBook.FanOfKnives))
                {
                    Logger.Write("[Totem] Stomp Unit -> " + u.Name + " (Fan of Knives)");
                    SpellBook.Cast(SpellBook.FanOfKnives);
                    return;
                }
            }

            // Legacy: Recherche d'un totem ennemi proche à stomp (garde pour compatibilité)
            WoWUnit totem = FindEnemyTotem(25.0);
            if (totem == null)
                return;

            // Cast Fan of Knives pour le stomp (miroir v.zip)
            if (SpellBook.CanCast(SpellBook.FanOfKnives))
            {
                Logger.Write("[Totem] Legacy Stomp -> " + totem.Name + " (Fan of Knives)");
                SpellBook.Cast(SpellBook.FanOfKnives); // 51723
            }
        }

        // Helper: detect Grounding Totem near a unit (or near player if unit is null)
        public static bool HasGroundingNear(WoWUnit relativeTo, double within)
        {
            try
            {
                var center = (relativeTo != null && relativeTo.IsValid) ? relativeTo.Location : Me.Location;

                // Check by unit name proximity
                var units = ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
                for (int i = 0; i < units.Count; i++)
                {
                    var u = units[i];
                    if (u == null || !u.IsValid || u.IsDead) continue;
                    string name = string.Empty; try { name = u.Name; } catch { name = string.Empty; }
                    if (!string.Equals(name, "Grounding Totem", StringComparison.OrdinalIgnoreCase)) continue;
                    double d = 9999.0; try { d = u.Location.Distance(center); } catch { }
                    if (d <= within)
                        return true;
                }

                // Fallback: check CreatedBySpellId mapping
                var all = ObjectManager.GetObjectsOfType<WoWUnit>(true, false);
                for (int i = 0; i < all.Count; i++)
                {
                    var u = all[i]; if (u == null || !u.IsValid || u.IsDead) continue;
                    int createdId = 0; try { createdId = (int)u.CreatedBySpellId; } catch { createdId = 0; }
                    Totems flag;
                    if (createdId != 0 && _totemByCreatedSpell.TryGetValue(createdId, out flag) && flag == Totems.Grounding)
                    {
                        double d = 9999.0; try { d = u.Location.Distance(center); } catch { }
                        if (d <= within)
                            return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private static WoWUnit FindEnemyTotem(double within)
        {
            try
            {
                foreach (WoWUnit u in ObjectManager.GetObjectsOfType<WoWUnit>(true, false))
                {
                    if (u == null || !u.IsValid || u.IsDead)
                        continue;

                    if (u.Distance > within)
                        continue;

                    // Hostile/attaquable (évite les totems alliés)
                    try
                    {
                        if (!u.Attackable)
                            continue;
                    }
                    catch
                    {
                        if (u.IsFriendly)
                            continue;
                    }

                    // Identifie par CreatedBySpellId
                    int createdId = (int)u.CreatedBySpellId;
                    Totems flag;
                    if (!_totemByCreatedSpell.TryGetValue(createdId, out flag) || flag == Totems.None)
                        continue;

                    // Throttle par type de totem (5 s ~ observé dans v.zip)
                    DateTime last;
                    if (_lastStomp.TryGetValue(flag, out last))
                    {
                        if ((DateTime.UtcNow - last).TotalMilliseconds < 5000)
                            continue;
                    }

                    // Respecte les flags utilisateur (v.zip)
                    if ((VitalicSettings.Instance.TotemStomp & flag) != 0)
                    {
                        _lastStomp[flag] = DateTime.UtcNow;
                        return u;
                    }
                }
            }
            catch { }
            return null;
        }
    }
}
