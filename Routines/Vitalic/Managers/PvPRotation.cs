using Styx;
using CommonBehaviors.Actions;

using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    public static class PvPRotation
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static DateTime _lastOpenerUtc = DateTime.MinValue;
        private static DateTime _lastKidneyAttempt = DateTime.MinValue; // Debounce for hotkey
        private static DateTime _suppressAutoOpenerUntilUtc = DateTime.MinValue; // guard against double opener after manual cast

        // Uniform opener lock window (mirror KS gate expectation in logs: 2500ms)
        private const int OpenerLockMs = 2500;

        // Per-target opener lock to avoid double opener on the same opponent
        private static readonly Dictionary<ulong, DateTime> _openerLockPerTargetUtc = new Dictionary<ulong, DateTime>();

        private const int SliceAndDiceId = 5171;
        private const int RuptureId = 1943;
        private const int AoeReevalMs = 300; // fixed reevaluation window
        private const double SnDWindow = 5.5; // figé comme l'original
        private const double RuptureWindow = 4.0; // figé comme l'original

        private static WoWUnit _pvpTotemTarget = null; // cache PvP stomp target

        public static Composite Build()
        {
            if (VitalicSettings.Instance.DiagnosticMode)
            {
                Logger.Write("[Diag][PvP] Build() called - PvP rotation composite created");
            }
            return new PrioritySelector(
                // === PAUSE GLOBALE EN TÊTE D'ARBRE (comme Class84.smethod_1) ===
                new Action(ret =>
                {
                    if (VitalicSettings.Instance.DiagnosticMode)
                    {
                        Logger.Write("[Diag][PvP] Rotation tick started");
                    }
                    if (EventHandlers.ShouldPauseOffense())
                        return RunStatus.Success; // on court-circuite ce tick
                    return RunStatus.Failure;
                }),

                // === Pause si la cible est immunisée (Ice Block, Divine Shield, etc.) ===
                // Place avant TOUTE autre logique pour reproduire la sensation "pause totale" de Vitalic
                new Action(ret =>
                {
                    try
                    {
                        var me = StyxWoW.Me;
                        var t = me != null ? me.CurrentTarget as WoWUnit : null;
                        if (t != null && t.IsValid && t.IsAlive && VitalicRotation.Managers.ImmunityGuard.TargetIsEffectivelyImmune(t, true))
                        {
                            // Mirror original: cancel queued + stop attack while target is immune
                            VitalicRotation.Managers.ImmunityGuard.HandleIfImmune(me, t, true);
                            if (VitalicSettings.Instance.DiagnosticMode)
                                Logger.Write("[Diag][PvP] Paused due to target immunity ({0})", t.SafeName);
                            return RunStatus.Success; // short-circuit this tick entirely
                        }
                    }
                    catch { }
                    return RunStatus.Failure;
                }),

                // === ARCHITECTURE VITALIC ORIGINALE (32 MODULES) ===
                // Position 2: State management
                new Decorator(ctx => !StyxWoW.Me.Combat, RestealthManager.Build()),
                // Shroud auto (out of combat mass stealth)
                new Action(ret => { try { ShroudManager.Execute(); } catch { } return RunStatus.Failure; }),
                // Position 3: Sprint / mobility
                new Action(ret => { try { MobilityManager.Execute(); } catch { } return RunStatus.Failure; }),

                // Position 4: Auto-CC (sap / blind) handled later via CrowdControlManager

                // Position 5: Burst of Speed (already in MobilityManager)
                new Action(ret => { try { MobilityManager.Execute(); } catch { } return RunStatus.Failure; }),

                // === Totem Stomp (insertion P1.2) ===
                new Decorator(ret => FindTotemTargetPvP(),
                    new Action(ret =>
                    {
                        var t = _pvpTotemTarget;
                        if (t != null && t.IsValid && !t.IsDead && AttackTotemTargetPvP(t))
                            return RunStatus.Success;
                        return RunStatus.Failure;
                    })),

                // Position 6-23: Other managers
                new Action(ret => { try { FocusManager.Execute(); } catch { } return RunStatus.Failure; }),
                FocusManager.Build(),
                new Action(ret => { try { EnemyCountCache.UpdateIfDue(AoeReevalMs); } catch { } return RunStatus.Failure; }),
                new Decorator(ctx => true, FlagReturnManager.Build()),

                // Interrupts
                InterruptManager.Build(),

                DefensivesManager.Build(),
                PsyfiendManager.Build(),
                ShivManager.Build(),
                new Action(ret => { 
                    if (VitalicSettings.Instance.DiagnosticMode) 
                        Logger.Write("[Diag][PvP] After ShivManager, checking next..."); 
                    return RunStatus.Failure; 
                }),
                AutoCCManager.Build(),
                new Action(ret => { try { CooldownManager.Execute(); } catch { } return RunStatus.Failure; }),
                new Action(ret => { try { TricksManager.Execute(); } catch { } return RunStatus.Failure; }),
                new Action(ret => { 
                    try { 
                        bool result = TryPvpStealthOpener();
                        if (VitalicSettings.Instance.DiagnosticMode && result)
                        {
                            Logger.Write("[Diag][PvP] Opener executed, continuing to rotation...");
                        }
                        // IMPORTANT: toujours retourner Failure pour permettre à la rotation de base de continuer
                        return RunStatus.Failure; 
                    } catch { return RunStatus.Failure; } 
                }),
                new Action(ret => { try { MaintainSliceAndDice(); } catch { } return RunStatus.Failure; }),
                new Decorator(ctx => !StyxWoW.Me.Combat, PreparationManager.Build()),

                // Position 24: Burst logic
                BurstManager.Build(),

                // Positions 25+: Crowd control + core rotation
                // Kidney Shot (auto or hotkey), aligned with PvE gating and Vitalic original priority
                new Decorator(ret => ShouldTryKidneyShot(),
                    new Action(ret =>
                    {
                        var S = VitalicSettings.Instance;
                        bool isHotkey = InputPoller.IsHotkeyActive(S.KidneyShotKeyBind);

                        // Hotkey debounce (150ms)
                        if (isHotkey)
                        {
                            if ((DateTime.UtcNow - _lastKidneyAttempt).TotalMilliseconds < 150)
                                return RunStatus.Failure;
                            _lastKidneyAttempt = DateTime.UtcNow;
                        }

                        var t = StyxWoW.Me.CurrentTarget as WoWUnit;
                        if (t != null && CrowdControlManager.TryKidney(t))
                        {
                            Logger.Write("[PvP] Kidney Shot " + (isHotkey ? "(hotkey)" : "(auto)") + " -> " + t.Name);
                            return RunStatus.Success;
                        }
                        return RunStatus.Failure;
                    })),
                
                CrowdControlManager.Build(),
                new Action(ret => { return ExecuteCore() ? RunStatus.Success : RunStatus.Failure; })
            );
        }

        private static void MaintainSliceAndDice()
        {
            var me = Me; if (me == null || !me.IsAlive) return;

            // Vérifier la fenêtre d'opener pour éviter les conflits - basé sur l'aura uniquement
            bool hasStealthAura = false;
            try { hasStealthAura = me.HasAura("Stealth") || me.HasAura("Subterfuge") || me.HasAura("Shadow Dance"); } catch { }
            if (hasStealthAura) return; // laisser l'opener gérer SnD

            double sndRem = 0.0; try { sndRem = me.GetAuraTimeLeft(SliceAndDiceId, true); } catch { }
            if (sndRem <= SnDWindow && SpellBook.CanCast(SpellBook.SliceAndDice, me))
            {
                if (SpellBook.Cast(SpellBook.SliceAndDice, me)) { Logger.Write("[PvP] Slice and Dice (window)"); return; }
            }
            bool has = me.HasAura("Slice and Dice"); int cp = me.ComboPoints;
            if (!has && cp >= 1 && !SpellManager.GlobalCooldown && SpellBook.CanCast(SpellBook.SliceAndDice, me))
            {
                if (SpellBook.Cast(SpellBook.SliceAndDice, me)) Logger.Write("[PvP] Slice and Dice (apply)");
            }
        }

        private static bool TryPvpStealthOpener()
        {
            try
            {
                var me = Me; if (me == null || !me.IsAlive) return false;
                var target = me.CurrentTarget as WoWUnit; if (target == null || !target.IsAlive || !target.Attackable) return false;

                // If the user just pressed a manual macro (e.g., Cheap Shot / Garrote), do NOT auto-open right after.
                // This prevents double openers when Subterfuge is up and mirrors Vitalic's priority of manual input.
                try { if (MacroManager.WasManualMacroRecently(800)) return false; } catch { }

                // Global suppression window after an opener (manual or auto)
                if (DateTime.UtcNow < _suppressAutoOpenerUntilUtc)
                {
                    if (VitalicSettings.Instance.DiagnosticMode)
                        Logger.Write("[Diag][Opener] Suppressed by global opener window ({0}ms left)", (int)(_suppressAutoOpenerUntilUtc - DateTime.UtcNow).TotalMilliseconds);
                    return false;
                }

                // Per-target lock: avoid chaining two openers on the same target within OpenerLockMs
                if (HasOpenerLock(target))
                {
                    if (VitalicSettings.Instance.DiagnosticMode)
                        Logger.Write("[Diag][Opener] Suppressed by per-target lock on {0}", target.SafeName);
                    return false;
                }

                // Only while actually stealthed/subterfuge/shadow dance (mirror Vitalic)
                bool hasStealthAura = false;
                try { hasStealthAura = me.HasAura("Stealth") || me.HasAura("Subterfuge") || me.HasAura("Shadow Dance"); } catch { }
                if (!hasStealthAura) return false;

                int spec = GetRogueSpecIndex();
                if (spec == 3 && SpellBook.CanCast(SpellBook.Premeditation, me))
                {
                    Logger.Write("[PvP] Premeditation (opener)");
                    if (SpellBook.Cast(SpellBook.Premeditation, me))
                    {
                        _lastOpenerUtc = DateTime.UtcNow;
                        return true;
                    }
                }
                // Vitalic original applies Slice and Dice during opener if available (after premed) before CC opener
                if (spec == 3)
                {
                    bool hasSnD = false; try { hasSnD = me.HasAura("Slice and Dice"); } catch { }
                    int cp = 0; try { cp = me.ComboPoints; } catch { }
                    if (!hasSnD && cp >= 1 && SpellBook.CanCast(SpellBook.SliceAndDice, me))
                    {
                        Logger.Write("[PvP] Slice and Dice (opener)");
                        if (SpellBook.Cast(SpellBook.SliceAndDice, me))
                        {
                            _lastOpenerUtc = DateTime.UtcNow;
                            return true;
                        }
                    }
                }
                if (SpellBook.CanCast(SpellBook.Garrote, target))
                {
                    if (!OffensiveGuardOK(target)) return false;
                    Logger.Write("[PvP] Garrote (opener) -> " + target.Name);
                    if (SpellBook.Cast(SpellBook.Garrote, target))
                    {
                        _lastOpenerUtc = DateTime.UtcNow;
                        ArmOpenerLocks(target);
                        try { CrowdControlManager.NotifyOpenerUsed(); } catch { }
                        return true;
                    }
                }
                if (SpellBook.CanCast(SpellBook.CheapShot, target))
                {
                    if (!OffensiveGuardOK(target)) return false;
                    Logger.Write("[PvP] Cheap Shot (opener) -> " + target.Name);
                    if (SpellBook.Cast(SpellBook.CheapShot, target))
                    {
                        _lastOpenerUtc = DateTime.UtcNow;
                        ArmOpenerLocks(target);
                        try { CrowdControlManager.NotifyCheapShotUsed(); } catch { }
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        // Called from combat log/UNIT_SPELLCAST_SENT when the player manually casts Garrote/Cheap Shot
        public static void NotifyManualOpenerUsed()
        {
            _lastOpenerUtc = DateTime.UtcNow;
            _suppressAutoOpenerUntilUtc = _lastOpenerUtc.AddMilliseconds(OpenerLockMs);
            try
            {
                var t = Me != null ? Me.CurrentTarget as WoWUnit : null;
                if (t != null && t.IsAlive && t.Attackable)
                {
                    // Also set a per-target lock when opener was manually used on current target
                    _openerLockPerTargetUtc[t.Guid] = _lastOpenerUtc.AddMilliseconds(OpenerLockMs);
                    if (VitalicSettings.Instance.DiagnosticMode)
                        Logger.Write("[Diag][Opener] Manual opener: locking {0} and suppressing auto-openers for {1}ms", t.SafeName, OpenerLockMs);
                }
                else if (VitalicSettings.Instance.DiagnosticMode)
                {
                    Logger.Write("[Diag][Opener] Manual opener: global suppression for {0}ms (no target)", OpenerLockMs);
                }
            }
            catch { }
        }

        private static void ArmOpenerLocks(WoWUnit target)
        {
            try
            {
                _suppressAutoOpenerUntilUtc = DateTime.UtcNow.AddMilliseconds(OpenerLockMs);
                if (target != null && target.IsValid)
                {
                    _openerLockPerTargetUtc[target.Guid] = DateTime.UtcNow.AddMilliseconds(OpenerLockMs);
                    if (VitalicSettings.Instance.DiagnosticMode)
                        Logger.Write("[Diag][Opener] Armed locks: global+{0}ms and per-target on {1}", OpenerLockMs, target.SafeName);
                }
            }
            catch { }
        }

        private static bool HasOpenerLock(WoWUnit target)
        {
            try
            {
                if (target == null || !target.IsValid) return false;
                DateTime until;
                if (_openerLockPerTargetUtc.TryGetValue(target.Guid, out until))
                {
                    if (DateTime.UtcNow < until) return true;
                    // Expired: prune entry
                    _openerLockPerTargetUtc.Remove(target.Guid);
                }
            }
            catch { }
            return false;
        }


        private static bool ExecuteCore()
        {
            if (ShouldPauseRotation()) { TryAssaDump(); return false; }
            if (Me == null || !Me.IsAlive) return false;
            var target = Me.CurrentTarget as WoWUnit; if (target == null || !target.IsAlive || !target.Attackable) return false;
            var S = VitalicSettings.Instance;
            int cpNow = Me.ComboPoints;
            bool meleeNow = LosFacingCache.InMeleeRangeCached(target);
            bool gcdNow = SpellManager.GlobalCooldown;

            if (ToggleState.PauseDamage || ToggleState.Lazy) return false;

            // === AUTO-FACE : conditions "immobile only", sans "return true" ===
            if (VitalicSettings.Instance.EventAutoFace)
            {
                bool meMoving = Me != null && Me.MovementInfo != null && Me.MovementInfo.CurrentSpeed > 0.1;
                var t = target;
                bool tMoving = t != null && t.MovementInfo != null && t.MovementInfo.CurrentSpeed > 0.1;

                if (!meMoving && !tMoving
                    && LosFacingCache.InMeleeRangeCached(t)
                    && !LosFacingCache.IsSafelyFacingCached(t, 2000))
                {
                    try { t.Face(); } catch { }
                    // NE PAS retourner Success ici (v.zip ne "consomme" pas le tick)
                }
            }

            // Pré-checks : garde "mêlée + LOS" (ok), pas de facing hard-fail ici
            bool inMeleeCache = LosFacingCache.InMeleeRangeCached(target);
            bool losCheck = LosFacingCache.InLineOfSpellSightCached(target, 2000);
            if (VitalicSettings.Instance.DiagnosticMode)
            {
                double distance = target.Distance;
                bool melee5 = false; try { melee5 = SpellBook.InMeleeRange(target, 5.0); } catch { }
                Logger.Write("[Diag][PvP] inMeleeCache={0} melee5={1} los={2} dist={3:F1}", inMeleeCache, melee5, losCheck, distance);
            }
            // Gate on 5y melee and LOS (mirror PvE builder gating)
            if (!SpellBook.InMeleeRange(target, 5.0) || !losCheck) return false;

            int specIdx = GetRogueSpecIndex();
            if (specIdx == 1) return AssassinationCore(target);
            if (specIdx == 2) return CombatCore(target);
            if (specIdx == 3) return SubtletyCore(target);

            if (LosFacingCache.InMeleeRangeCached(target)
                && LosFacingCache.InLineOfSpellSightCached(target, 2000)
                && SpellBook.CanCast(SpellBook.Mutilate, target))
                return SpellBook.Cast(SpellBook.Mutilate, target);
            return false;
        }

        private static bool ShouldTryKidneyShot()
        {
            try
            {
                var t = Me.CurrentTarget as WoWUnit;
                if (t == null || !t.IsValid || !t.IsAlive) return false;

                var S = VitalicSettings.Instance;
                bool isHotkey = InputPoller.IsHotkeyActive(S.KidneyShotKeyBind);
                if (isHotkey) return true; // hotkey forces attempt
                if (S.AutoKidney) return true; // mirror PvE toggle
                return false;
            }
            catch { return false; }
        }

        private static bool AssassinationCore(WoWUnit target)
        {
            int cp = Me.ComboPoints;
            if (NeedRefreshRupture(target) && cp >= 3 && SpellBook.CanCast(SpellBook.Rupture, target)) { Logger.Write("[PvP][Assa] Rupture"); return SpellBook.Cast(SpellBook.Rupture, target); }
            bool blindside = false; try { blindside = Me.HasAura(121153); } catch { }
            if (blindside && SpellBook.CanCast(SpellBook.Dispatch, target)) { if (!OffensiveGuardOK(target)) return false; Logger.Write("[PvP][Assa] Dispatch"); return SpellBook.Cast(SpellBook.Dispatch, target); }
            if (SpellBook.CanCast(SpellBook.Mutilate, target)) { 
                if (VitalicSettings.Instance.DiagnosticMode) Logger.Write("[Diag][Assa] Mutilate CanCast OK, checking OffensiveGuard...");
                if (!OffensiveGuardOK(target)) return false; 
                Logger.Write("[PvP][Assa] Mutilate"); 
                return SpellBook.Cast(SpellBook.Mutilate, target); 
            }
            return false;
        }
        private static bool CombatCore(WoWUnit target)
        {
            if (NeedRefreshRupture(target) && ComboPointsEx.EffectiveCP(Me) >= 3 && SpellBook.CanCast(SpellBook.Rupture, target)) { Logger.Write("[PvP][Combat] Rupture"); return SpellBook.Cast(SpellBook.Rupture, target); }
            if (SpellBook.CanCast(SpellBook.RevealingStrike, target) && !target.HasAura("Revealing Strike")) { Logger.Write("[PvP][Combat] Revealing Strike"); return SpellBook.Cast(SpellBook.RevealingStrike, target); }
            if (ComboPointsEx.EffectiveCP(Me) >= 5 && SpellBook.CanCast(SpellBook.Eviscerate, target)) { if (!OffensiveGuardOK(target)) return false; Logger.Write("[PvP][Combat] Eviscerate"); return SpellBook.Cast(SpellBook.Eviscerate, target); }
            if (SpellBook.CanCast(SpellBook.SinisterStrike, target)) return SpellBook.Cast(SpellBook.SinisterStrike, target);
            return false;
        }
        private static bool SubtletyCore(WoWUnit target)
        {
            // Vitalic original includes a dedicated Garrote during Subterfuge window
            bool subterfuge = false; try { subterfuge = Me.HasAura("Subterfuge"); } catch { }
            if (subterfuge && SpellBook.CanCast(SpellBook.Garrote, target) && !target.HasMyAura(SpellBook.Garrote))
            {
                if (!OffensiveGuardOK(target)) return false;
                Logger.Write("[PvP][Sub] Garrote (Subterfuge)");
                return SpellBook.Cast(SpellBook.Garrote, target);
            }

            bool fwUp = false; try { fwUp = target != null && target.HasAura(91021); } catch { }
            if (!fwUp && NeedRefreshRupture(target) && ComboPointsEx.EffectiveCP(Me) >= 3 && SpellBook.CanCast(SpellBook.Rupture, target)) { Logger.Write("[PvP][Sub] Rupture"); return SpellBook.Cast(SpellBook.Rupture, target); }
            if (!fwUp && NeedRefreshSnD() && ComboPointsEx.EffectiveCP(Me) >= 1 && SpellBook.CanCast(SpellBook.SliceAndDice)) { SpellBook.Cast(SpellBook.SliceAndDice); return true; }
            if (fwUp && ComboPointsEx.EffectiveCP(Me) >= 5 && SpellBook.CanCast(SpellBook.Eviscerate, target)) { if (!OffensiveGuardOK(target)) return false; Logger.Write("[PvP][Sub] Eviscerate"); return SpellBook.Cast(SpellBook.Eviscerate, target); }
            if (SpellBook.CanCast(SpellBook.Hemorrhage, target) && !target.HasMyAura(SpellBook.Hemorrhage)) { if (!OffensiveGuardOK(target)) return false; Logger.Write("[PvP][Sub] Hemorrhage (SV)"); return SpellBook.Cast(SpellBook.Hemorrhage, target); }
            bool hemoDelayOk = true; try { var d = VitalicSettings.Instance.HemoDelay; if (d > 0) hemoDelayOk = (DateTime.UtcNow - _lastOpenerUtc).TotalSeconds >= d; } catch { }
            if (!VitalicSettings.Instance.AlwaysUseHemo && hemoDelayOk && SpellBook.CanCast(SpellBook.Backstab, target)) { if (!OffensiveGuardOK(target)) return false; Logger.Write("[PvP][Sub] Backstab"); return SpellBook.Cast(SpellBook.Backstab, target); }
            if (SpellBook.CanCast(SpellBook.Hemorrhage, target)) { if (!OffensiveGuardOK(target)) return false; return SpellBook.Cast(SpellBook.Hemorrhage, target); }
            return false;
        }

        private static bool OffensiveGuardOK(WoWUnit t)
        {
            if (t == null || !t.IsValid || !t.IsAlive) 
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][OffensiveGuard] Target invalid/dead");
                return false;
            }
            if (!LosFacingCache.InLineOfSpellSightCached(t, 2000)) 
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][OffensiveGuard] LOS check failed for {0}", t.SafeName);
                return false;
            }
            if (ImmunityGuard.TargetIsEffectivelyImmune(t, true)) 
            { 
                try { ImmunityGuard.HandleIfImmune(Me, t, true); } catch { } 
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[Diag][OffensiveGuard] Target immune: {0}", t.SafeName);
                return false; 
            }
            try
            {
                if (VitalicSettings.Instance.EventAutoFace)
                {
                    // On ne bloque plus sur "je bouge". On face si en mêlée et pas déjà facing.
                    bool inMelee = LosFacingCache.InMeleeRangeCached(t);
                    if (inMelee && !LosFacingCache.IsSafelyFacingCached(t, 2000))
                    {
                        try { t.Face(); Logger.Write("[AutoFace] Facing {0}", t.SafeName); } catch { }
                    }
                }
            }
            catch { }
            if (VitalicSettings.Instance.DiagnosticMode)
                Logger.Write("[Diag][OffensiveGuard] OK for {0}", t.SafeName);
            return true;
        }

        private static void TryAssaDump()
        {
            try
            {
                var me = Me; var t = me != null ? me.CurrentTarget as WoWUnit : null; if (t == null || !t.IsAlive) return;
                if (GetRogueSpecIndex() != 1) return;
                int cp = 0; try { cp = me.ComboPoints; } catch { }
                bool melee = SpellBook.InMeleeRange(t); bool facing = false; try { facing = me.IsSafelyFacing(t); } catch { }
                if (cp >= 4 && melee && facing && !SpellManager.GlobalCooldown && SpellBook.CanCast(SpellBook.Envenom, t)) { Logger.Write("[PvP][Assa] Envenom (Dump)"); SpellBook.Cast(SpellBook.Envenom, t); }
            }
            catch { }
        }

        private static bool NeedRefreshSnD() { double tl = 0.0; try { tl = Me.GetAuraTimeLeft(SliceAndDiceId, true); } catch { tl = 0.0; } return tl <= 0 || tl <= SnDWindow; }
        private static bool NeedRefreshRupture(WoWUnit target) { if (target == null) return false; double tl = 0.0; try { tl = target.GetAuraTimeLeft(RuptureId, true); } catch { tl = 0.0; } return tl <= 0 || tl <= RuptureWindow; }
        private static int GetRogueSpecIndex()
        {
            try { int idx = Lua.GetReturnVal<int>("local s=GetSpecialization(); if s then return s else return 0 end", 0); if (idx >= 1 && idx <= 3) return idx; } catch { }
            try { if (SpellManager.HasSpell(SpellBook.Vendetta) || SpellManager.HasSpell(SpellBook.Envenom)) return 1; if (SpellManager.HasSpell(SpellBook.KillingSpree) || SpellManager.HasSpell(SpellBook.BladeFlurry)) return 2; if (SpellManager.HasSpell(SpellBook.ShadowDance) || SpellManager.HasSpell(SpellBook.Backstab)) return 3; } catch { }
            return 0;
        }

        private static bool FindTotemTargetPvP()
        {
            try
            {
                TotemRecentCache.Prune();
                foreach (var u in ObjectManager.GetObjectsOfType<WoWUnit>(false))
                {
                    if (u == null || !u.IsValid || u.IsDead) continue;
                    if (!u.IsTotem) continue;
                    if (u == Me.CurrentTarget) continue; // ≠ current target (parité PvE)
                    if (TotemRecentCache.IsRecent(u)) continue; // anti-flicker
                    if (u.Distance > 8.0) continue; // short stomp radius identical to PvE
                    if (!LosFacingCache.InLineOfSpellSightCached(u, 2000)) continue;
                    if (!VitalicRotation.Helpers.LosFacingCache.IsSafelyFacingCached(u, 2000)) continue;
                    if (!u.Attackable || u.IsFriendly) continue;
                    int created = 0; try { created = (int)u.CreatedBySpellId; } catch { created = 0; }
                    if (created == 0) continue;
                    Totems flag;
                    if (!TotemMappings.SpellToTotemMap.TryGetValue(created, out flag)) continue;
                    if ((VitalicSettings.Instance.TotemStomp & flag) == Totems.None) continue;
                    _pvpTotemTarget = u; TotemRecentCache.Mark(u); return true;
                }
                _pvpTotemTarget = null; return false;
            }
            catch { _pvpTotemTarget = null; return false; }
        }

        private static bool AttackTotemTargetPvP(WoWUnit t)
        {
            try
            {
                if (t == null || !t.IsValid || t.IsDead) return false;
                // Prefer Fan of Knives if available (cleave stomp without retargeting)
                if (SpellBook.CanCast(SpellBook.FanOfKnives) && SpellBook.Cast(SpellBook.FanOfKnives))
                {
                    Logger.Write("[PvP] Totem stomped with Fan of Knives: {0}", t.SafeName);
                    return true;
                }
                // Otherwise quickly target the totem (will be naturally destroyed by next ability)
                if (Me.CurrentTarget != t) { t.Target(); Logger.Write("[PvP] Targeting totem: {0}", t.SafeName); }
                return false; // allow the rest of rotation to act
            }
            catch { return false; }
        }

        private static bool ShouldPauseRotation()
        {
            try
            {
                if (ToggleState.Pause) return true;
                if (ToggleState.PauseDamage) return true;
                if (EventHandlers.ShouldPauseOffense()) return true;

                // Gouge pause: don't attack while our Gouge is on current target
                var target = Me.CurrentTarget as WoWUnit;
                if (target != null && EventHandlers.ShouldPauseDamageForGouge(target))
                    return true;

                // Immunity pause: if target is immune (e.g., Ice Block), mirror Vitalic and pause rotation
                if (target != null && ImmunityGuard.TargetIsEffectivelyImmune(target, true))
                    return true;

                return false;
            }
            catch { return false; }
        }
    }
}

