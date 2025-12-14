using System;
using Styx;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;

namespace VitalicRotation.Managers
{
    internal static class MobilityManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        // Throttles (miroir v.zip : léger délai entre actions)
        private static DateTime _lastAnyUse = DateTime.MinValue;
        private static DateTime _lastTrapAction = DateTime.MinValue;
        private static DateTime _lastShadowstep = DateTime.MinValue;
        private static DateTime _lastBos = DateTime.MinValue;
    private static DateTime _lastSprint = DateTime.MinValue;

        // Shadowstep strict validation + throttle
    private static DateTime _lastStep = DateTime.MinValue;
    // Vitalic legacy throttle ~200ms (matches ShadowstepManager)
    private const int StepThrottleMs = 200; // anti spam
        private static readonly string[] _badUnits =
        {
            "Totem", "Dummy", "Training Dummy", "Wild Imp", "Spirit Wolf", "Snake Trap"
        };

        private static bool IsMovementImpaired()
        {
            try
            {
                var cur = Me.MovementInfo.CurrentSpeed;
                var run = Me.MovementInfo.RunSpeed;
                return cur < (run * 0.55); // proxy snare/root comme v.zip
            }
            catch { return false; }
        }

        public static void Execute()
        {
            try
            {
                if (Me == null || !Me.IsValid || Me.IsDead)
                    return;

                if (Me.Mounted || Me.IsOnTransport || Me.IsFlying)
                    return;

                var S = VitalicSettings.Instance;

                // 0) Anti-pièges (prioritaire)
                if (S.AutoMoveOnTraps && TryHandleTrapEscape())
                    return;

                var t = Me.CurrentTarget as WoWUnit;

                // 1) Shadowstep (gap-close) — respecte AutoShadowstep (0 = désactivé)
                int stepMax = 0; try { stepMax = S.AutoShadowstep; } catch { stepMax = 0; }
                // Vitalic: Step evaluated before Sprint to avoid overlap
                if (stepMax > 0 && t != null && t.IsAlive && t.Attackable && t.Distance > 5.0 && t.Distance <= stepMax)
                {
                    if (VitalicSettings.Instance.DiagnosticMode)
                        Logger.Write("[Diag][Step] Consider: dist={0:0.0} max={1} los={2}", t.Distance, stepMax, t.InLineOfSight);
                    // Utilise la version validée + throttle
                    if (TryShadowstepSafe(t))
                        return;
                }

                // 2) Sprint
                if (TrySprint(t)) return;

                // 3) Burst of Speed (amélioré)
                if (ShouldAutoBurstOfSpeed(t))
                {
                    if (SpellBook.Cast(SpellBook.BurstOfSpeed))
                    {
                        _lastBos = DateTime.UtcNow;
                        Logger.Write("[Mobility] Burst of Speed");
                        return;
                    }
                }
            }
            catch
            {
                // silence
            }
        }

        private static bool ShouldAutoBurstOfSpeed(WoWUnit target)
        {
            try
            {
                var S = VitalicSettings.Instance;
                if (S == null || !S.AutoBurstOfSpeed) return false;
                if (!SpellBook.CanCast(SpellBook.BurstOfSpeed)) return false;
                if (SpellManager.GlobalCooldown) return false;
                if (Me.HasAura(SpellBook.BurstOfSpeed)) return false; // déjà actif
                // éviter pendant stealth ou vanish
                try { if (Me.HasAura(SpellBook.Stealth) || Me.HasAura(SpellBook.Vanish)) return false; } catch { }

                // throttle interne (500ms)
                if ((DateTime.UtcNow - _lastBos).TotalMilliseconds < 500) return false;

                int energy = (int)Me.CurrentEnergy;
                bool snared = IsMovementImpaired();
                bool hasSprint = Me.HasAura(SpellBook.Sprint);

                // Voyage / gap close conditions
                bool needTravel = false;
                if (target != null && target.IsValid && target.IsAlive)
                {
                    double d = target.Distance;
                    // Gap close: hors mêlée et pas assez proche pour Shadowstep ou Step déjà en CD
                    if (d > 12 && d < 40) needTravel = true;
                }
                else
                {
                    // Pas de cible : déplacement out-of-combat (farm / déplacement) si on bouge
                    try { if (!Me.Combat && Me.MovementInfo.CurrentSpeed > 1.0f) needTravel = true; } catch { }
                }

                // Energie : plus permissif si snared (50) sinon 60
                int required = snared ? 50 : 60;
                if (energy < required) return false;

                // Ne pas gaspiller si on est déjà très proche et pas snared
                if (!snared && !needTravel)
                    return false;

                // Si Sprint déjà actif et pas snared, éviter doublon
                if (hasSprint && !snared)
                    return false;

                return true;
            }
            catch { return false; }
        }

        // v.zip: Sprint helper (2983)
        private static bool TrySprint(Styx.WoWInternals.WoWObjects.WoWUnit t)
        {
            try
            {
                if (t == null || !t.IsAlive || !t.Attackable)
                    return false;

                // Ne pas sprinter si la cible est sape et proche (≤ 28y)
                try { if (t.HasAura(SpellBook.Sap) && t.Distance <= 28.0) return false; } catch { }

                bool wantSprint;
                try
                {
                    double tspeed = 0.0; try { tspeed = t.MovementInfo.CurrentSpeed; } catch { tspeed = 0.0; }
                    wantSprint = (t.Distance > 8.0) || (tspeed >= 7.0);
                }
                catch { wantSprint = (t.Distance > 8.0); }

                if (!wantSprint)
                    return false;

                bool movingForward = false;
                try { movingForward = Me.MovementInfo.MovingForward; } catch { movingForward = (Me.MovementInfo.CurrentSpeed > 0.1); }

                bool longEnough = false;
                try { longEnough = Me.MovementInfo.TimeMoved > 0.25; } catch { longEnough = true; }

                bool strafing = false;
                try { strafing = Me.MovementInfo.IsStrafing; } catch { strafing = false; }

                if (!(movingForward && longEnough) || strafing)
                    return false;

                try { if (!Me.IsSafelyFacing(t)) Me.SetFacing(t); } catch { }

                if (SpellBook.CanCast(SpellBook.Sprint))
                {
                    if (SpellBook.Cast(SpellBook.Sprint))
                    {
                        _lastAnyUse = DateTime.UtcNow;
                        _lastSprint = _lastAnyUse;
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        // ======== Pièges (miroir v.zip) ========
        private static bool TryHandleTrapEscape()
        {
            // Throttle ~3s sur la séquence piège
            if ((DateTime.UtcNow - _lastTrapAction).TotalSeconds < 3.0)
                return false;

            WoWGameObject nearestTrap = null;
            double bestDist = 99999.0;

            try
            {
                var gos = ObjectManager.GetObjectsOfType<WoWGameObject>(false, false);
                for (int i = 0; i < gos.Count; i++)
                {
                    var go = gos[i];
                    if (go == null || !go.IsValid) continue;

                    // simple heuristique : nom contenant "Trap"
                    string name = "";
                    try { name = go.Name; } catch { name = ""; }
                    if (string.IsNullOrEmpty(name)) continue;
                    if (name.IndexOf("Trap", StringComparison.OrdinalIgnoreCase) < 0) continue;

                    double d = 0.0;
                    try { d = go.Distance; } catch { continue; }
                    if (d <= 6.0 && d < bestDist)
                    {
                        bestDist = d;
                        nearestTrap = go;
                    }
                }
            }
            catch { nearestTrap = null; }

            if (nearestTrap == null)
                return false;

            // On agit seulement si on est quasi "dessus" (≤ ~4y)
            double distToTrap = 0.0;
            try { distToTrap = nearestTrap.Location.Distance(Me.Location); }
            catch { distToTrap = bestDist; }

            if (distToTrap > 4.0)
                return false;

            // a) priorité : Shadowstep vers la cible si possible (si autorisé par réglage)
            var t = Me.CurrentTarget as WoWUnit;
            bool targetOk = (t != null && t.IsValid && t.IsAlive && t.Attackable && t.InLineOfSpellSight);
            if (targetOk)
            {
                double td = t.Distance;
                int stepRange = VitalicSettings.Instance.AutoShadowstep;
                if (VitalicSettings.Instance.ShadowstepTraps
                    && stepRange > 0 && td > 5.0 && td <= stepRange && CanShadowstepNow())
                {
                    if (TryShadowstepSafe(t))
                    {
                        _lastTrapAction = DateTime.UtcNow;
                        _lastAnyUse = _lastTrapAction;
                        return true;
                    }
                }
            }

            // b) sinon : micro-déplacement pour sortir du piège
            var meLoc = Me.Location;
            var trapLoc = nearestTrap.Location;

            // vecteur d’échappement 2D = (me - trap), normalisé
            double dx = meLoc.X - trapLoc.X;
            double dy = meLoc.Y - trapLoc.Y;
            double len = Math.Sqrt(dx * dx + dy * dy);
            if (len < 0.1)
            {
                // si superposé, avance dans le facing
                double rad = Me.RenderFacing;
                dx = Math.Cos(rad);
                dy = Math.Sin(rad);
                len = 1.0;
            }
            else
            {
                dx /= len;
                dy /= len;
            }

            // point cible à ~7y
            var escapePoint = new WoWPoint(meLoc.X + dx * 7.0,
                                           meLoc.Y + dy * 7.0,
                                           meLoc.Z);

            try
            {
                Logger.Write("[Mobility] Move off trap");
                Navigator.MoveTo(escapePoint);
                _lastTrapAction = DateTime.UtcNow;
                _lastAnyUse = _lastTrapAction;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool CanShadowstepNow()
        {
            // Keep this consistent with StepThrottleMs to avoid unnecessary gating
            return (DateTime.UtcNow - _lastShadowstep).TotalMilliseconds >= 200 && !SpellManager.GlobalCooldown;
        }

        // ======== Shadowstep safe wrappers ========
        private static bool IsBadName(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            for (int i = 0; i < _badUnits.Length; i++)
                if (name.IndexOf(_badUnits[i], StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            return false;
        }

        private static bool IsValidStepTarget(WoWUnit u)
        {
            if (u == null || !u.IsAlive) return false;
            try { if (!u.Attackable) return false; } catch { }
            // Allow duel targets: in duels, same-faction players are attackable but not flagged hostile.
            bool hostileOrDuel = true; 
            try { hostileOrDuel = u.IsHostile || (u.IsPlayer && u.Attackable); } catch { }
            if (!hostileOrDuel) return false;
            bool inLos = true; try { inLos = u.InLineOfSpellSight; } catch { inLos = true; }
            if (!inLos) return false;
            try { if (u.IsPet) return false; } catch { }
            try { if (IsBadName(u.Name)) return false; } catch { }
            try { if (ImmunityGuard.TargetIsEffectivelyImmune(u, true)) return false; } catch { }
            return true;
        }

        public static bool TryShadowstepSafe(WoWUnit target)
        {
            if (target == null) return false;
            if ((DateTime.UtcNow - _lastStep).TotalMilliseconds < StepThrottleMs)
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][Step] Throttled");
                return false;
            }
            if (!IsValidStepTarget(target))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][Step] Invalid target for Step");
                return false;
            }
            if (!SpellBook.CanCast(SpellBook.Shadowstep, target))
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][Step] CanCast failed");
                return false;
            }

            if (SpellBook.Cast(SpellBook.Shadowstep, target))
            {
                _lastStep = DateTime.UtcNow;
                _lastShadowstep = _lastStep;
                Logger.Write("[Step] Shadowstep -> " + target.SafeName);
                try { AutoAttack.Start(); } catch { }
                return true;
            }
            return false;
        }
    }
}
