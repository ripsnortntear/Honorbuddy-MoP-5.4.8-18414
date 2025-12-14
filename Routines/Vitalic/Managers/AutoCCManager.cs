using System;
using System.Collections.Generic;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using VitalicRotation.UI;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    /// <summary>
    /// Auto-CC Manager (équivalent exact Class116 de l'original)
    /// Gère auto-sap et auto-blind selon les conditions Vitalic exactes
    /// Compatible .NET Framework 4.5.1 / C# 5
    /// </summary>
    internal static class AutoCCManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static DateTime _lastAutoSap = DateTime.MinValue;
        private static DateTime _lastAutoBlind = DateTime.MinValue;
        
        // Cache pour les unités (équivalent Class116.woWUnit_0/1/2)
        private static WoWUnit _candidateHealerForBlind = null;
        private static WoWUnit _lastSappedTarget = null;
        private static WoWUnit _selectedBlindTarget = null;
        
        // Throttles pour éviter spam
        private const int AutoSapThrottleMs = 500;
        private const int AutoBlindThrottleMs = 600;

        // Classes Healer exactes selon l'original (hashSet_0)
        // ORIGINAL: seulement Warrior(4) et Druid(11) - PAS toutes les classes heal !
        private static readonly HashSet<WoWClass> HealerClasses = new HashSet<WoWClass>
        {
            WoWClass.Warrior, // 4 - Second Wind healing capabilities
            WoWClass.Druid    // 11 - Primary healer
        };

        /// <summary>
        /// Composite principal pour intégration dans la rotation
        /// </summary>
        public static Composite Build()
        {
            return new PrioritySelector(
                // Auto-Sap en stealth hors combat (Boolean_21)
                new Decorator(ret => ShouldAutoSap(),
                    new Action(ret =>
                    {
                        var target = Me.CurrentTarget as WoWUnit;
                        if (TryAutoSap(target))
                        {
                            return RunStatus.Success;
                        }
                        return RunStatus.Failure;
                    })),
                
                // Auto-Blind sur healer trinket (Boolean_23)
                new Decorator(ret => ShouldAutoBlind(),
                    new Action(ret =>
                    {
                        var healer = FindHealerForAutoBlind();
                        if (TryAutoBlind(healer))
                        {
                            return RunStatus.Success;
                        }
                        return RunStatus.Failure;
                    })),
                
                // Fallback
                new Action(ret => RunStatus.Failure)
            );
        }

        /// <summary>
        /// Boolean_21 original - Auto-Sap en stealth hors combat
        /// </summary>
        private static bool ShouldAutoSap()
        {
            try
            {
                if (Me == null || !Me.IsValid || !Me.IsAlive) return false;
                if (Me.Combat) return false;
                
                // Pas en stealth - pas d'auto-sap
                if (!Me.IsStealthed) return false;
                
                // Throttle anti-spam
                if ((DateTime.UtcNow - _lastAutoSap).TotalMilliseconds < AutoSapThrottleMs) return false;
                
                // Doit avoir une cible
                var target = Me.CurrentTarget as WoWUnit;
                if (target == null || !target.IsValid || !target.IsAlive) return false;
                if (!target.Attackable || target.IsFriendly) return false;
                
                // Conditions énergétiques et de portée
                if (Me.CurrentEnergy < 40) return false; // Sap = 40 énergie
                if (!SpellBook.InMeleeRange(target, 10.0)) return false; // Sap 10y range
                
                // Pas déjà sappé
                if (target.HasAura(6770)) return false; // Sap aura
                
                // Peut caster Sap
                if (!SpellBook.CanCast(SpellBook.Sap, target)) return false;
                
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Boolean_23 original - Auto-Blind sur healer après trinket
        /// Implémentation selon Class116.Boolean_22/23 de l'original
        /// </summary>
        private static bool ShouldAutoBlind()
        {
            try
            {
                if (Me == null || !Me.IsValid || !Me.IsAlive) return false;
                if (!VitalicSettings.Instance.AutoBlindHealerTrinket) return false;
                
                // Throttle anti-spam  
                if ((DateTime.UtcNow - _lastAutoBlind).TotalMilliseconds < AutoBlindThrottleMs) return false;
                
                // Conditions énergétiques
                if (Me.CurrentEnergy < 30) return false; // Blind = 30 énergie
                
                // Pas en stealth (Blind sort de stealth)
                if (Me.IsStealthed) return false;
                
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Recherche un healer ennemi avec fenêtre trinket active
        /// Équivalent Class116.Boolean_22 wrapper
        /// </summary>
        private static WoWUnit FindHealerForAutoBlind()
        {
            try
            {
                var units = ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
                for (int i = 0; i < units.Count; i++)
                {
                    var unit = units[i];
                    if (unit == null || !unit.IsValid || !unit.IsAlive) continue;
                    if (!unit.Attackable || unit.IsFriendly) continue;
                    if (unit.Distance > 20.0) continue; // Blind range
                    
                    // LOS (cache)
                    if (!LosFacingCache.InLineOfSpellSightCached(unit, 2000)) continue;
                    
                    // Vérifier si c'est un healer (parité hashSet_0)
                    if (!IsHealerClass(unit)) continue;
                    
                    // Vérifier fenêtre trinket (P1.4 correction)
                    if (!EventHandlers.IsHealerTrinketWindowActive(unit.Guid)) continue;
                    
                    // Pas déjà blindé
                    if (unit.HasAura(2094)) continue; // Blind aura
                    
                    // Peut être ciblé pour Blind
                    if (!SpellBook.CanCast(SpellBook.Blind, unit)) continue;
                    
                    return unit;
                }
                
                return null;
            }
            catch { return null; }
        }

        /// <summary>
        /// Tentative d'Auto-Sap
        /// </summary>
        private static bool TryAutoSap(WoWUnit target)
        {
            try
            {
                if (target == null) return false;
                
                // LOS/facing minimal requis (cache LOS)
                if (!LosFacingCache.InLineOfSpellSightCached(target, 2000)) return false;
                
                // Immunités: éviter Sap sur cibles immunes (Cyclone/Banish/etc.)
                if (ImmunityGuard.TargetIsEffectivelyImmune(target, true)) return false;
                
                // Vérifications finales
                if (!SpellBook.CanCast(SpellBook.Sap, target)) return false;
                
                // Cast avec délai aléatoire (mimique comportement original)
                var rng = new Random();
                int delay = rng.Next(50, 150);
                try { LuaHelper.SleepForLag(delay); } catch { }
                
                if (SpellBook.Cast(SpellBook.Sap, target))
                {
                    _lastAutoSap = DateTime.UtcNow;
                    _lastSappedTarget = target;
                    
                    // UI feedback
                    try { AudioBus.PlayEvent(); } catch { }
                    try { VitalicUi.ShowBigBanner("Auto Sap -> " + target.SafeName); } catch { }
                    
                    // DR application
                    try { DRTracker.Applied(target.Guid, DRTracker.DrCategory.Disorient); } catch { }
                    
                    return true;
                }
            }
            catch { }
            
            return false;
        }

        /// <summary>
        /// Tentative d'Auto-Blind sur healer trinket
        /// Implémentation P1.1 selon le plan de correction
        /// </summary>
        private static bool TryAutoBlind(WoWUnit healer)
        {
            try
            {
                if (healer == null) return false;
                
                // LOS (cache) et immunités
                if (!LosFacingCache.InLineOfSpellSightCached(healer, 2000)) return false;
                if (ImmunityGuard.TargetIsEffectivelyImmune(healer, true)) return false;
                
                // DR ok (parité DRTracker table: Blind => Disorient)
                try { if (!DRTracker.Can(healer, DRTracker.DrCategory.Disorient)) return false; } catch { }
                
                // Vérifications finales
                if (!SpellBook.CanCast(SpellBook.Blind, healer)) return false;
                
                // Cast avec délai aléatoire
                var rng = new Random();
                int delay = rng.Next(40, 120);
                try { LuaHelper.SleepForLag(delay); } catch { }
                
                if (SpellBook.Cast(SpellBook.Blind, healer))
                {
                    _lastAutoBlind = DateTime.UtcNow;
                    _selectedBlindTarget = healer;
                    
                    // Clear trinket window (P1.4 correction)
                    EventHandlers.ClearHealerTrinketWindow(healer.Guid);
                    
                    // UI feedback
                    try { AudioBus.PlayEvent(); } catch { }
                    try { VitalicUi.ShowBigBanner("Auto Blind -> " + healer.SafeName); } catch { }
                    
                    // DR application: Disorient pour Blind
                    try { DRTracker.Applied(healer.Guid, DRTracker.DrCategory.Disorient); } catch { }
                    
                    return true;
                }
            }
            catch { }
            
            return false;
        }

        /// <summary>
        /// Vérifie si une unité est un healer selon les classes originales
        /// </summary>
        private static bool IsHealerClass(WoWUnit unit)
        {
            if (unit == null) return false;
            return HealerClasses.Contains(unit.Class);
        }

        /// <summary>
        /// Reset des caches (utile pour changement de zone/combat)
        /// </summary>
        public static void Reset()
        {
            _candidateHealerForBlind = null;
            _lastSappedTarget = null;
            _selectedBlindTarget = null;
            _lastAutoSap = DateTime.MinValue;
            _lastAutoBlind = DateTime.MinValue;
        }

        /// <summary>
        /// Mise à jour périodique (appelée depuis la pulse)
        /// </summary>
        public static void Update()
        {
            try
            {
                // Validation des caches
                if (_candidateHealerForBlind != null && (!_candidateHealerForBlind.IsValid || _candidateHealerForBlind.IsDead))
                    _candidateHealerForBlind = null;
                    
                if (_lastSappedTarget != null && (!_lastSappedTarget.IsValid || _lastSappedTarget.IsDead))
                    _lastSappedTarget = null;
                    
                if (_selectedBlindTarget != null && (!_selectedBlindTarget.IsValid || _selectedBlindTarget.IsDead))
                    _selectedBlindTarget = null;
            }
            catch { }
        }
    }
}