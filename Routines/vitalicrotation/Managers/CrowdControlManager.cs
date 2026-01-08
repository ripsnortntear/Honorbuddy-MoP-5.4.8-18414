using System;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Settings;
using VitalicRotation.Helpers;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    public static class CrowdControlManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        
        // Raison d'échec Kidney pour feedback UI/macros
        public enum KidneyFailReason { None, Gcd, Timing, DR, Position, ComboPoints, Energy, CanCast }

        // CC qui se cassent aux dégâts (Sap/Blind/Poly/Hex/Repent/Trap/Wyvern…)
        private static readonly int[] BreakableCcAuras = {
            SpellBook.Sap,                  // 6770 - Sap
            SpellBook.Blind,                // 2094 - Blind
            118,                            // Polymorph
            28272,                          // Polymorph (pig)
            28271,                          // Polymorph (turtle) 
            61305,                          // Polymorph (cat)
            61721,                          // Polymorph (rabbit)
            61780,                          // Polymorph (turkey)
            51514,                          // Hex
            20066,                          // Repentance
            3355,                           // Freezing Trap
            19386,                          // Wyvern Sting
            31661,                          // Dragon's Breath (disorient)
            19503,                          // Scatter Shot
            82691,                          // Ring of Frost
            99,                             // Disorienting Roar
            115078                          // Paralysis
        };

        // Demandé par VitalicRotationRoutine.cs
        public static Composite Build()
        {
            return new Action(delegate
            {
                Execute();
                // IMPORTANT: Do not consume the tick if nothing was done.
                // Returning Success here short-circuits the PrioritySelector and prevents core rotation.
                // Vitalic's original managers only return Success when they actually perform an action.
                return RunStatus.Failure;
            });
        }

        public static void Execute()
        {
            // Pause offense: éviter le spam des sous-appels si cast manuel récent
            if (EventHandlers.ShouldPauseOffense()) return;
            // vide : les appels directs TryKidney/TryBlind/TryGouge sont faits depuis la rotation
        }

        // Marqueur de temps pour le dernier opener (comme dans Vitalic original)
        private static DateTime _lastOpenerUtc = DateTime.MinValue;

        /// <summary>
        /// Signaler qu'un opener a été exécuté (Garrote ou Cheap Shot)
        /// </summary>
        public static void NotifyOpenerUsed()
        {
            _lastOpenerUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Signaler qu'un Cheap Shot a été exécuté
        /// </summary>
        public static void NotifyCheapShotUsed()
        {
            NotifyOpenerUsed();
        }

        /// <summary>Kidney Shot selon les réglages Vitalic (CP et énergie requis). Hotkey force la tentative même si AutoKidney est off.</summary>
        public static bool TryKidney(WoWUnit target)
        {
            KidneyFailReason reason;
            return TryKidney(target, out reason);
        }

        /// <summary>
        /// Variante avec raison d'échec pour feedback précis des macros.
        /// </summary>
        public static bool TryKidney(WoWUnit target, out KidneyFailReason failReason)
        {
            failReason = KidneyFailReason.None;
            if (target == null || !target.IsValid || !target.IsAlive) { failReason = KidneyFailReason.CanCast; return false; }

            // GCD check - ne pas tenter pendant GlobalCooldown
            if (SpellManager.GlobalCooldown)
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[KS][Gate] FAIL(gcd) - GlobalCooldown actif");
                failReason = KidneyFailReason.Gcd;
                return false;
            }

            // Vérifier le délai après opener (logique de Vitalic: 2500ms)
            if ((DateTime.UtcNow - _lastOpenerUtc).TotalMilliseconds < 2500)
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[KS][Gate] FAIL(timing) - Récent opener ({0:0.0}ms ago, besoin 2500ms)",
                        (DateTime.UtcNow - _lastOpenerUtc).TotalMilliseconds);
                failReason = KidneyFailReason.Timing;
                return false;
            }

            if (!DRTracker.CanApplyStun(target, 0.5))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                {
                    var dr = DRTracker.GetState(target, DRTracker.DrCategory.Stun);
                    Logger.Write("[KS][Gate] FAIL(dr) - DR state: {0}", dr);
                }
                failReason = KidneyFailReason.DR;
                return false;
            }

            // Vérifications mêlée et LOS (3.5y comme l'original)
            bool inMelee = SpellBook.InMeleeRange(target);
            bool hasLos = target.InLineOfSpellSight;
            if (!inMelee || !hasLos)
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[KS][Gate] FAIL(position) - inMelee={0} los={1} dist={2:0.1}", inMelee, hasLos, target.Distance);
                failReason = KidneyFailReason.Position;
                return false;
            }

            // v.zip : AutoRedirect si CP ailleurs (focus) avant Kidney - utilisable aussi en PvP
            if (VitalicSettings.Instance.AutoRedirect && SpellBook.CanCast(SpellBook.Redirect))
            {
                int cpOnTarget = Me.ComboPoints;
                int cpOnFocus = 0;
                try { cpOnFocus = Lua.GetReturnVal<int>("return GetComboPoints('player','focus') or 0", 0); } catch { }

                if (cpOnTarget == 0 && cpOnFocus > 0)
                {
                    if (SpellBook.Cast(SpellBook.Redirect)) { LuaHelper.SleepForLag(); }
                }
            }

            int needCp = VitalicSettings.Instance.KidneyShotCPs;      // défaut 4
            int needEnergy = VitalicSettings.Instance.KidneyShotEnergy;   // défaut 25

            if (Me.ComboPoints < needCp)
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[KS][Gate] FAIL(cp) - cp={0} need={1}", Me.ComboPoints, needCp);
                failReason = KidneyFailReason.ComboPoints;
                return false;
            }

            // Energy : selon l'API, CurrentEnergy est disponible sur LocalPlayer (HB MoP)
            int energy = (int)Me.CurrentEnergy;
            if (energy < needEnergy)
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[KS][Gate] FAIL(energy) - energy={0} need={1}", energy, needEnergy);
                failReason = KidneyFailReason.Energy;
                return false;
            }

            if (!SpellBook.CanCast(SpellBook.KidneyShot, target))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[KS][Gate] FAIL(cancast) - SpellBook.CanCast failed");
                failReason = KidneyFailReason.CanCast;
                return false;
            }

            // Diagnostic complet pour validation
            if (VitalicSettings.Instance.DiagnosticMode)
            {
                var dr = DRTracker.GetState(target, DRTracker.DrCategory.Stun);
                Logger.Write("[KS][Gate] SUCCESS - target={0} cp={1} energy={2} DR={3} inMelee={4} los={5}",
                    target.Name, Me.ComboPoints, energy, dr, inMelee, hasLos);
            }

            if (VitalicSettings.Instance.LogMessagesEnabled)
                Logger.Write("[CC] Kidney Shot sur " + target.Name + " (CP " + Me.ComboPoints + ", Énergie " + energy + ")");

            bool cast = SpellBook.Cast(SpellBook.KidneyShot, target);
            if (cast)
                DRTracker.Applied(target, DRCategory.Stun);

            return cast;
        }

        /// <summary>Blind : usage situationnel (peel / cast non interruptible) avec DR Disorient. Pas de toggle dans l'original.</summary>
        public static bool TryBlind(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsAlive) return false;
            
            // avant Blind - DR Disorient (MoP: Blind partage DR avec Disorient/Sap/Gouge)
            if (!DRTracker.Can(target, DRTracker.DrCategory.Disorient))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                {
                    var dr = DRTracker.GetState(target, DRTracker.DrCategory.Disorient);
                    Logger.Write("[Blind][Gate] FAIL(dr) - DR state: {0}", dr);
                }
                return false;
            }

            // v.zip : ne pas Blind si hard CC déjà présent
            if (DRTracker.HasHardCcNoDisarm(target))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Blind][Gate] FAIL(hardCC) - target already hard-CC'd");
                return false;
            }
            
            if (!SpellBook.CanCast(SpellBook.Blind, target))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Blind][Gate] FAIL(cancast) - SpellBook.CanCast failed");
                return false;
            }

            // Cas principal Vitalic : hard cast non interruptible
            bool hardCast = target.IsCasting && target.CanInterruptCurrentSpellCast == false;
            if (!hardCast)
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Blind][Gate] FAIL(condition) - target not hard-casting");
                return false;
            }

            if (VitalicSettings.Instance.LogMessagesEnabled)
                Logger.Write("[CC] Blind sur " + target.Name + " (cast non interruptible)");

            bool cast = SpellBook.Cast(SpellBook.Blind, target);
            if (cast)
                DRTracker.Applied(target, DRTracker.DrCategory.Disorient);

            return cast;
        }

        /// <summary>
        /// Blind when explicitly requested via manual macro. Keeps DR and existing-hard-CC gates, but does NOT require a non-interruptible cast.
        /// This mirrors Vitalic manual behavior where user intent can force a peel Blind within range/LOS.
        /// </summary>
        public static bool TryBlindManual(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsAlive) return false;

            // DR and hard CC guards still apply
            if (!DRTracker.Can(target, DRTracker.DrCategory.Disorient))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                {
                    var dr = DRTracker.GetState(target, DRTracker.DrCategory.Disorient);
                    Logger.Write("[Blind][Manual][Gate] FAIL(dr) - DR state: {0}", dr);
                }
                return false;
            }

            if (DRTracker.HasHardCcNoDisarm(target))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Blind][Manual][Gate] FAIL(hardCC) - target already hard-CC'd");
                return false;
            }

            if (!SpellBook.CanCast(SpellBook.Blind, target))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Blind][Manual][Gate] FAIL(cancast) - SpellBook.CanCast failed");
                return false;
            }

            bool cast = SpellBook.Cast(SpellBook.Blind, target);
            if (cast)
            {
                if (VitalicSettings.Instance.LogMessagesEnabled)
                    Logger.Write("[CC] Blind (manual) sur " + target.Name);
                DRTracker.Applied(target, DRTracker.DrCategory.Disorient);
            }
            return cast;
        }

        /// <summary>Gouge opportuniste (mêlée face à nous ou cast non interruptible), DR incapacitate per v.zip. Pas de toggle dans l'original.</summary>
        public static bool TryGouge(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsAlive) return false;
            
            // avant Gouge - DR Incapacitate (comme v.zip: Gouge (1776) est dans DR_INCAP)
            if (!DRTracker.CanApplyIncap(target, 0.5)) return false;

            // v.zip : éviter d'empiler sur Incap/Disorient existant
            if (DRTracker.HasIncapOrDisorient(target)) return false;
            
            if (!SpellBook.CanCast(SpellBook.Gouge, target)) return false;

            bool melee = target.Distance <= 5 && LosFacingCache.InLineOfSpellSightCached(target, 2000);
            bool facingMe = target.IsFacing(Me);
            bool hardCast = target.IsCasting && target.CanInterruptCurrentSpellCast == false;

            if (!((melee && facingMe) || hardCast))
                return false;

            if (VitalicSettings.Instance.LogMessagesEnabled)
                Logger.Write("[CC] Gouge sur " + target.Name + (hardCast ? " (cast non interruptible)" : " (peel mêlée)"));

            // Forcer le facing avant l'envoi de Gouge
            var t = target as WoWUnit;
            if (t != null && t.IsValid && t.IsAlive)
            {
                if (!StyxWoW.Me.IsSafelyFacing(t))
                    StyxWoW.Me.SetFacing(t);
            }

            bool cast = SpellBook.Cast(SpellBook.Gouge, target);
            if (cast)
                DRTracker.Applied(target, DRTracker.DrCategory.Incapacitate); // v.zip: Gouge is Incapacitate

            return cast;
        }

        public static bool AnyBreakableCcWithin(double yards)
        {
            try
            {
                var me = Styx.StyxWoW.Me;
                if (me == null) return false;

                // Ennemis autour de nous
                var enemies = Styx.WoWInternals.ObjectManager.GetObjectsOfType<Styx.WoWInternals.WoWObjects.WoWUnit>(false, false);
                foreach (var u in enemies)
                {
                    if (u == null || !u.IsValid || u.IsDead || !u.Attackable) continue;
                    if (u.Distance > yards) continue;

                    // Si l'ennemi a l'une des auras cassables (peu importe l'appliqueur)
                    foreach (var aura in u.Auras)
                    {
                        if (System.Array.IndexOf(BreakableCcAuras, aura.Value.SpellId) >= 0)
                        {
                            if (VitalicSettings.Instance.DiagnosticMode)
                            {
                                Logger.Write("[AntiBreak] Found breakable CC: {0} on {1} at {2:0.1}y", 
                                    aura.Value.Name, u.Name, u.Distance);
                            }
                            return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }
    }
}


