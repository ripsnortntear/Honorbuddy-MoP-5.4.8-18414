using System;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;
using System.Linq;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using Action = Styx.TreeSharp.Action;
using CommonBehaviors.Actions;

namespace VitalicRotation.Managers
{
    internal static class PvERotation
    {
        private static DateTime _lastOpenerUtc = DateTime.MinValue;
        private static DateTime _lastManualCast = DateTime.MinValue;
        private static DateTime _lastKidneyAttempt = DateTime.MinValue; // Debounce for hotkey
        private const double SnDWindow = 5.5;
        private const double RuptureWindow = 4.0;
        private static readonly int[] FindWeaknessIds = { 91021 }; // MoP target debuff
        private static readonly int ShallowInsightId = 84745;  // Bandit's Guile tier 1
        private static readonly int ModerateInsightId = 84746; // tier 2
        private static readonly int DeepInsightId = 84747;     // tier 3
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static WoWUnit CurrentTarget { get { return Me != null ? Me.CurrentTarget as WoWUnit : null; } }
        private static WoWUnit FocusTarget { get { return Me != null ? Me.FocusedUnit : null; } }
        private static WoWUnit _totemTarget = null;
        private static readonly TimeSpan TotemRecencyTtl = TimeSpan.FromSeconds(5);
        private const int AoeReevalMs = 300; // parit� plan 1.5

        public static Composite Build()
        {
            return new PrioritySelector(
                // Module 1: Pause check
                new Decorator(ret => ShouldPauseRotation(), new ActionAlwaysSucceed()),
                
                // Module 2: State management & targeting  
                new Action(ret => { HandleTargetChange(); return RunStatus.Failure; }),
                
                // Module 3: Manual cast pause
                new PrioritySelector(
                    new Decorator(ret => IsInManualCastPause(), new Action(ret => { Logger.Write("[PvE] Manual cast pause active"); return RunStatus.Success; })),
                    new Decorator(ret => DetectManualCast(), new Action(ret => { _lastManualCast = DateTime.UtcNow; return RunStatus.Success; }))
                ),
                
                // Module 4: Focus management
                new Action(ret => { try { FocusManager.Execute(); } catch { } return RunStatus.Failure; }),
                
                // Module 5: Defensive/utility  
                new Decorator(ret => DefensivesManager.ShouldExecute(), new Action(ret => { DefensivesManager.Execute(); return RunStatus.Success; })),
                
                // Module 5.1: AOE context + Blade Flurry auto toggle
                new Action(ret => { try { UpdateAoeAndBladeFlurry(); } catch { } return RunStatus.Failure; }),
                
                // Module 6: Totem targeting
                new PrioritySelector(
                    new Decorator(ret => FindTotemTarget(),
                        new Action(ret =>
                        {
                            var t = _totemTarget;
                            if (t != null && t.IsValid && !t.IsDead && AttackTotemTarget(t))
                                return RunStatus.Success;
                            return RunStatus.Failure;
                        })),
                    new Action(ret => RunStatus.Failure)
                ),
                
                // Additional managers
                PsyfiendManager.Build(),
                new Action(r=>{ try { ShroudManager.Execute(); } catch { } return RunStatus.Failure; }),
                
                // Module 7: Core rotation
                new PrioritySelector(
                    AutoCCManager.Build(),
                    
                    // Burst manager (Class104 equivalent)
                    BurstManager.Build(),
                    
                    // === KIDNEY SHOT LOGIC (PRIORIT� ABSOLUE - AVANT TOUT) ===
                    new Decorator(ret => ShouldTryKidneyShot(),
                        new Action(ret =>
                        {
                            var S = VitalicSettings.Instance;
                            bool isHotkey = InputPoller.IsHotkeyActive(S.KidneyShotKeyBind);
                            
                            // Hotkey debounce
                            if (isHotkey && (DateTime.UtcNow - _lastKidneyAttempt).TotalMilliseconds >= 150)
                            {
                                _lastKidneyAttempt = DateTime.UtcNow;
                            }
                            else if (isHotkey)
                            {
                                return RunStatus.Failure; // Debounce actif
                            }
                            
                            if (CrowdControlManager.TryKidney(CurrentTarget))
                            {
                                Logger.Write("[PvE] Kidney Shot " + (isHotkey ? "(hotkey)" : "(auto)") + " -> " + CurrentTarget.Name);
                                return RunStatus.Success;
                            }
                            return RunStatus.Failure;
                        })),
                    
                    // Stealth opener
                    new Decorator(ret => ShouldAttemptStealthOpener(),
                        new PrioritySelector(
                            new Decorator(ret => SpellBook.CanCast(SpellBook.Garrote, CurrentTarget) && Me.HasAura("Stealth"), 
                                new Action(ret => { if(!OffensiveGuardOK(CurrentTarget)) return RunStatus.Failure; SpellBook.Cast(SpellBook.Garrote, CurrentTarget); _lastOpenerUtc = DateTime.UtcNow; return RunStatus.Success; })),
                            new Decorator(ret => SpellBook.CanCast(SpellBook.CheapShot, CurrentTarget) && Me.HasAura("Stealth"), 
                                new Action(ret => { if(!OffensiveGuardOK(CurrentTarget)) return RunStatus.Failure; SpellBook.Cast(SpellBook.CheapShot, CurrentTarget); _lastOpenerUtc = DateTime.UtcNow; return RunStatus.Success; }))
                        )),
                    
                    // Spec-specific rotations
                    new PrioritySelector(
                        new Decorator(ret => Me.Specialization == WoWSpec.RogueAssassination,
                            new PrioritySelector(
                                new Decorator(ret => NeedRefreshRupture(), new Action(ret => CastFinisher(SpellBook.Rupture))),
                                new Decorator(ret => ShouldDispatch(), new Action(ret => { TryAutoFace(CurrentTarget); return CastFinisher(SpellBook.Dispatch); })),
                                new Decorator(ret => ShouldMutilate(), new Action(ret => { TryAutoFace(CurrentTarget); return SpellBook.Cast(SpellBook.Mutilate, CurrentTarget) ? RunStatus.Success : RunStatus.Failure; }))
                            )),
                        new Decorator(ret => Me.Specialization == WoWSpec.RogueCombat,
                            new PrioritySelector(
                                new Decorator(ret => ShouldRevealingStrike(), new Action(ret => SpellBook.Cast(SpellBook.RevealingStrike, CurrentTarget))),
                                new Decorator(ret => ShouldSinisterStrike(), new Action(ret => SpellBook.Cast(SpellBook.SinisterStrike, CurrentTarget)))
                            )),
                        new Decorator(ret => Me.Specialization == WoWSpec.RogueSubtlety,
                            new PrioritySelector(
                                // Subtlety: Pooling for Find Weakness window takes absolute priority over builders
                                new Decorator(ret => ShouldLazyPoolSubtlety(), new Action(ret => RunStatus.Success)),
                                // Hemo/Backstab: Backstab uniquement si parfaitement derri�re; sinon Hemorrhage
                                new Decorator(ret => ShouldHemorrhage(), new Action(ret => { if(!OffensiveGuardOK(CurrentTarget)) return RunStatus.Failure; return SpellBook.Cast(SpellBook.Hemorrhage, CurrentTarget) ? RunStatus.Success : RunStatus.Failure; })),
                                new Decorator(ret => ShouldBackstab(), new Action(ret => { if(!OffensiveGuardOK(CurrentTarget)) return RunStatus.Failure; return SpellBook.Cast(SpellBook.Backstab, CurrentTarget) ? RunStatus.Success : RunStatus.Failure; }))
                            ))
                    ),
                    
                    // AoE abilities (muted if breakable CC nearby)
                    new Decorator(ret => ShouldUseAoe(),
                        new PrioritySelector(
                            new Decorator(ret => ShouldFanOfKnives(), new Action(ret => SpellBook.Cast(SpellBook.FanOfKnives))),
                            new Decorator(ret => ShouldCrimsonTempest(), new Action(ret => CastFinisher(SpellBook.CrimsonTempest)))
                        )),
                    
                    // Buff maintenance
                    new PrioritySelector(
                        new Decorator(ret => NeedRefreshSnD(), new Action(ret => CastFinisher(SpellBook.SliceAndDice))),
                        new Decorator(ret => NeedRefreshRupture(CurrentTarget), new Action(ret => CastFinisher(SpellBook.Rupture)))
                    ),
                    
                    // Finishers at 5 CP
                    new PrioritySelector(
                        new Decorator(ret => Me.ComboPoints >= 5,
                            new PrioritySelector(
                                new Decorator(ret => ShouldEviscerate(), new Action(ret => CastFinisher(SpellBook.Eviscerate))),
                                new Decorator(ret => ShouldEnvenomAssassination(), new Action(ret => CastFinisher(SpellBook.Envenom)))
                            ))
                    ),
                    
                    // Builders
                    new PrioritySelector(
                        new Decorator(ret => Me.Specialization == WoWSpec.RogueSubtlety && ShouldLazyPoolSubtlety(), 
                            new Action(ret => RunStatus.Success)),
                        new Decorator(ret => Me.ComboPoints < 5,
                            new PrioritySelector(
                                new Decorator(ret => Me.Specialization == WoWSpec.RogueAssassination && ShouldMutilate(), 
                                    new Action(ret => SpellBook.Cast(SpellBook.Mutilate, CurrentTarget))),
                                new Decorator(ret => Me.Specialization == WoWSpec.RogueCombat && ShouldSinisterStrike(), 
                                    new Action(ret => SpellBook.Cast(SpellBook.SinisterStrike, CurrentTarget))),
                                new Decorator(ret => Me.Specialization == WoWSpec.RogueSubtlety && ShouldBackstab(), 
                                    new Action(ret => SpellBook.Cast(SpellBook.Backstab, CurrentTarget)))
                            ))
                    ),
                    
                    // Auto-attack fallback
                    new Action(ret => { 
                        if (Me != null && !Me.IsAutoAttacking && CurrentTarget != null && CurrentTarget.IsValid) 
                            Lua.DoString("StartAttack()"); 
                        return RunStatus.Failure; 
                    })
                ),
                
                // Module 8: Emergency finishers (dump)
                new PrioritySelector(
                    new Decorator(ret => ShouldEmergencyEnvenom(),
                        new Action(ret => {
                            if (SpellBook.Cast(SpellBook.Envenom)) {
                                Logger.Write("[PvE] Emergency Envenom dump - CP: {0}", Me.ComboPoints);
                                return RunStatus.Success;
                            }
                            return RunStatus.Failure;
                        })),
                    new Decorator(ret => ShouldEmergencyRecuperate(),
                        new Action(ret => {
                            if (SpellBook.Cast(SpellBook.Recuperate)) {
                                Logger.Write("[PvE] Emergency Recuperate dump - HP: {0}%, CP: {1}", (int)Me.HealthPercent, Me.ComboPoints);
                                return RunStatus.Success;
                            }
                            return RunStatus.Failure;
                        })),
                    new Decorator(ret => ShouldEmergencySnD(),
                        new Action(ret => {
                            if (SpellBook.Cast(SpellBook.SliceAndDice)) {
                                Logger.Write("[PvE] Emergency SnD dump - CP: {0}", Me.ComboPoints);
                                return RunStatus.Success;
                            }
                            return RunStatus.Failure;
                        })),
                    new Action(ret => RunStatus.Failure)
                )
            );
        }

        public static void Execute()
        {
            try
            {
                if (Me == null || !Me.IsValid) return;
                var target = CurrentTarget; if (target == null || !target.IsValid) return;
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[PvE] Execute - Target: {0}, CP: {1}, Energy: {2}", target.SafeName, Me.ComboPoints, Me.CurrentEnergy);
            }
            catch (Exception ex) { Logger.WriteException(ex, "PvERotation.Execute"); }
        }

        #region Condition Methods
        private static bool ShouldPauseRotation() {
            try {
                // Standard pauses
                if (ToggleState.Pause) return true;
                if (ToggleState.PauseDamage) return true;
                if (EventHandlers.ShouldPauseOffense()) return true;

                // Gouge pause (Class141 behavior): don't attack while our Gouge is on current target
                var target = CurrentTarget;
                if (target != null && EventHandlers.ShouldPauseDamageForGouge(target))
                    return true;

                if (IsInManualCastPause()) return true;

                return false;
            }
            catch { return false; }
        }

        private static bool ShouldTryKidneyShot()
        {
            try
            {
                if (CurrentTarget == null || !CurrentTarget.IsValid || !CurrentTarget.IsAlive) return false;
                
                var S = VitalicSettings.Instance;
                bool isHotkey = InputPoller.IsHotkeyActive(S.KidneyShotKeyBind);
                
                // Hotkey prioritaire - force la tentative m�me si AutoKidney est off
                if (isHotkey) return true;
                
                // AutoKidney si activ� dans les param�tres
                if (S.AutoKidney) return true;
                
                return false;
            }
            catch { return false; }
        }
        private static bool IsTargetInMeleeRange()
        {
            try
            {
                var target = CurrentTarget;
                if (target == null || !target.IsValid || !target.IsAlive) return false;
                return LosFacingCache.InMeleeRangeCached(target);
            }
            catch { return false; }
        }

        private static bool IsReadyForMelee()
        {
            try
            {
                var target = CurrentTarget;
                if (target == null || !target.IsValid || !target.IsAlive) return false;
                if (!LosFacingCache.InMeleeRangeCached(target)) return false;
                if (!LosFacingCache.IsSafelyFacingCached(target, 2000)) return false;
                if (!LosFacingCache.InLineOfSpellSightCached(target, 2000)) return false;
                return true;
            }
            catch { return false; }
        }
        private static bool IsInManualCastPause() { var elapsed = (DateTime.UtcNow - _lastManualCast).TotalMilliseconds; return elapsed < VitalicSettings.Instance.ManualCastPause; }
        private static bool DetectManualCast() { return false; }
        private static void HandleTargetChange() { if (CurrentTarget == null) return; if (!Me.Combat && Me.HasAura("Stealth") && CurrentTarget.IsHostile && !CurrentTarget.Combat && CurrentTarget.Distance <= 10 && !CurrentTarget.HasAura("Sap")) { if (SpellBook.CanCast(SpellBook.Sap, CurrentTarget)) { SpellBook.Cast(SpellBook.Sap, CurrentTarget); Logger.Write("[PvE] Auto-sap on target change"); } } }
        #endregion

        #region Emergency Dumps
        private static bool ShouldEmergencyEnvenom() { try { if (Me.Specialization != WoWSpec.RogueAssassination) return false; if (Me.ComboPoints < VitalicSettings.Instance.EnvenomCPs) return false; if (Me.CurrentEnergy < VitalicSettings.Instance.EnvenomEnergy) return false; if (Me.HasAura("Slice and Dice")) return false; return true; } catch { return false; } }
        private static bool ShouldEmergencyRecuperate() { try { return Me.ComboPoints >= 1 && Me.HealthPercent <= VitalicSettings.Instance.RecuperateHealth && Me.CurrentEnergy >= 30; } catch { return false; } }
        private static bool ShouldEmergencySnD() { try { if (Me.ComboPoints < 1) return false; if (Me.CurrentEnergy < 25) return false; if (Me.HasAura("Slice and Dice")) return false; return true; } catch { return false; } }
        #endregion

        #region Rotation Helpers
        // Bandit's Guile tier helper (1=Shallow,2=Moderate,3=Deep)
        private static int GetBanditsGuileTier()
        {
            try
            {
                if (Me.HasAura(DeepInsightId)) return 3;
                if (Me.HasAura(ModerateInsightId)) return 2;
                if (Me.HasAura(ShallowInsightId)) return 1;
            }
            catch { }
            return 0;
        }
        private static bool ShouldAttemptStealthOpener() { if (!Me.HasAura("Stealth")) return false; // Subterfuge dure ~3.0s, pas 3.5s - ajuster fen�tre pour �viter retard builder
            if ((DateTime.UtcNow - _lastOpenerUtc).TotalSeconds < 3.0) return false; 
            return CurrentTarget != null && CurrentTarget.IsValid && CurrentTarget.IsAlive && CurrentTarget.Distance <= 5; }
        private static bool NeedRefreshSnD() { var tl = Me.GetAuraTimeLeft("Slice and Dice"); if (tl > SnDWindow) return false; return Me.ComboPoints >= 1; }
        private static bool NeedRefreshRupture(WoWUnit t) { if (t == null || !t.IsValid) return false; double tl = t.GetAuraTimeLeft("Rupture", true); if (tl > RuptureWindow) return false; return Me.ComboPoints >= 2; }
        private static bool ShouldUseAoe() 
        { 
            try
            {
                // Mute AoE if breakable CC nearby
                if (CrowdControlManager.AnyBreakableCcWithin(10.0)) return false;
                if (AoESuppressionHelper.IsAoeSuppressed()) return false;
                EnemyCountCache.UpdateIfDue(AoeReevalMs);
                int f = 3; int ct = 4;
                try { f = VitalicSettings.Instance.AoeThresholdFoK; } catch { }
                try { ct = VitalicSettings.Instance.AoeThresholdCT; } catch { }
                return EnemyCountCache.Aoe8() >= f || EnemyCountCache.Aoe10() >= ct;
            }
            catch { return false; }
        }
        private static bool ShouldMutilate() 
        { 
            if (CurrentTarget == null || !CurrentTarget.IsValid) return false;
            if (CurrentTarget.Distance > 5) return false;
            if (Me.ComboPoints >= 5) return false;
            
            // Vérifier Blindside proc - priorité à Dispatch si Blindside actif
            bool blindside = false; try { blindside = Me.HasAura(121153); } catch { }
            if (blindside && SpellBook.CanCast(SpellBook.Dispatch, CurrentTarget)) return false;
            
            // Ne pas exiger IsSafelyFacing, juste LOS
            if (!LosFacingCache.InLineOfSpellSightCached(CurrentTarget, 2000)) return false;
            
            return true; 
        }
        private static bool ShouldDispatch() {
            try {
                if (CurrentTarget == null || !CurrentTarget.IsValid) return false;
                if (Me.Specialization != WoWSpec.RogueAssassination) return false;
                bool blindside = false; try { blindside = Me.HasAura(121153); } catch { }
                // Builder selector per original: Dispatch only on Blindside proc; otherwise Mutilate
                if (!blindside) return false;
                return SpellBook.CanCast(SpellBook.Dispatch, CurrentTarget);
            } catch { return false; }
        }
        private static bool ShouldEviscerate() {
            try {
                // P2.3 - Utiliser CP effectifs pour Combat spec avec Anticipation
                int effectiveCP = (Me.Specialization == WoWSpec.RogueCombat) ? GetEffectiveComboPoints() : Me.ComboPoints;
                
                // Base requirement
                if (effectiveCP < 5) return false;
                if (VitalicSettings.Instance.LazyEviscerate) return false;

                // Subtlety Find Weakness gating: if Sub, no FW, and safe to delay (SnD & Rupture healthy, energy not about to cap) then pool
                if (Me.Specialization == WoWSpec.RogueSubtlety) {
                    bool hasFW = HasFindWeakness();
                    if (!hasFW) {
                        double sndTl = Me.GetAuraTimeLeft("Slice and Dice");
                        double rupTl = 0; try { if (CurrentTarget != null) rupTl = CurrentTarget.GetAuraTimeLeft("Rupture", true); } catch { }
                        double energy = Me.CurrentEnergy;
                        if (sndTl > 6.0 && rupTl > 6.0 && energy < 90) {
                            // Delay to wait for FW (e.g., after next stealth effect) � skip eviscerate
                            return false;
                        }
                    }
                }

                // Combat Bandit's Guile tier tracking: prefer Deep Insight for dumps unless near cap / buffs expiring
                if (Me.Specialization == WoWSpec.RogueCombat) {
                    int tier = GetBanditsGuileTier();
                    if (tier < 3) {
                        double energy = Me.CurrentEnergy;
                        double sndTl = Me.GetAuraTimeLeft("Slice and Dice");
                        // If not deep insight and we are not going to cap energy soon and SnD is safe, delay
                        if (energy < 95 && sndTl > 4.0) return false;
                    }
                }

                return true;
            } catch { return false; }
        }
        private static bool ShouldEnvenomAssassination() { return Me.Specialization == WoWSpec.RogueAssassination && Me.ComboPoints >= 5; }
        private static bool ShouldRevealingStrike() { return CurrentTarget != null && CurrentTarget.IsValid && !CurrentTarget.HasMyAura(SpellBook.RevealingStrike) && Me.ComboPoints < 5; }
        private static bool ShouldSinisterStrike() { return Me.ComboPoints < 5; }
        // Subtlety Hemo/Backstab with advanced gating
        private static bool ShouldHemorrhage()
        {
            if (Me.Specialization != WoWSpec.RogueSubtlety) return false;
            if (VitalicSettings.Instance.AlwaysUseHemo) return true;
            if (ShouldDelayHemorrhage()) return false; // on retarde h�morragie
            if (!IsBehindSafe(CurrentTarget)) return true; // hors derri�re -> Hemo
            return false; // sinon on pr�f�re Backstab
        }
        private static bool ShouldBackstab()
        {
            if (Me.Specialization != WoWSpec.RogueSubtlety) return false;
            if (ShouldDelayHemorrhage()) return false; // on attend avant d'utiliser Hemo/Backstab
            if (ShouldHemorrhage()) return false; // Hemo prioritaire si conditions remplies
            return CurrentTarget != null && CurrentTarget.IsValid && IsBehindSafe(CurrentTarget);
        }
        private static bool ShouldFanOfKnives() 
        { 
            try
            {
                if (CrowdControlManager.AnyBreakableCcWithin(10.0)) return false;
                if (AoESuppressionHelper.IsAoeSuppressed()) return false;
                EnemyCountCache.UpdateIfDue(AoeReevalMs);
                int f = 3; try { f = VitalicSettings.Instance.AoeThresholdFoK; } catch { }
                return EnemyCountCache.Aoe8() >= f;
            }
            catch { return false; }
        }
        private static bool ShouldCrimsonTempest() 
        { 
            try
            {
                if (CrowdControlManager.AnyBreakableCcWithin(10.0)) return false;
                if (AoESuppressionHelper.IsAoeSuppressed()) return false;
                if (Me.ComboPoints < 5) return false;
                EnemyCountCache.UpdateIfDue(AoeReevalMs);
                int ct = 4; try { ct = VitalicSettings.Instance.AoeThresholdCT; } catch { }
                return EnemyCountCache.Aoe10() >= ct;
            }
            catch { return false; }
        }
        private static bool NeedRefreshRupture() { if (CurrentTarget == null || !CurrentTarget.IsValid) return false; if (Me.ComboPoints < 2) return false; double tl = CurrentTarget.GetAuraTimeLeft("Rupture", true); return tl <= RuptureWindow; }
        private static bool HasFindWeakness() 
        { 
            try 
            { 
                if (CurrentTarget == null || !CurrentTarget.IsValid) return false; 
                for (int i = 0; i < FindWeaknessIds.Length; i++) 
                    if (CurrentTarget.HasAura(FindWeaknessIds[i])) return true; 
                return false; 
            } 
            catch { return false; } 
        }
        private static RunStatus CastFinisher(int id) 
        { 
            if (CurrentTarget != null && CurrentTarget.IsValid && SpellBook.CanCast(id, CurrentTarget)) 
            { 
                // Immunity/LOS guard only for key offensives (Eviscerate/Envenom)
                if (id == SpellBook.Eviscerate || id == SpellBook.Envenom)
                {
                    if (!OffensiveGuardOK(CurrentTarget)) return RunStatus.Failure;
                }
                SpellBook.Cast(id, CurrentTarget); return RunStatus.Success; 
            } 
            return RunStatus.Failure; 
        }
        private static bool IsBehindSafe(WoWUnit u) { return u != null && Me.IsSafelyBehind(u); }
        
        /// <summary>
        /// P2.3 - Helper pour points de combo effectifs (Combat Anticipation)
        /// Retourne Me.ComboPoints + stacks d'Anticipation pour gestion correcte
        /// </summary>
        private static int GetEffectiveComboPoints()
        {
            try
            {
                if (Me == null) return 0;
                
                int baseCP = Me.ComboPoints;
                int anticipationStacks = 0;
                
                // R�cup�rer les stacks d'Anticipation si le talent est pris (114015)
                try 
                { 
                    var aura = Me.GetAuraById(114015); 
                    if (aura != null) anticipationStacks = (int)aura.StackCount; 
                } 
                catch { }
                
                return Math.Min(baseCP + anticipationStacks, 5); // Cap � 5 CP effectifs
            }
            catch { return Me != null ? Me.ComboPoints : 0; }
        }

        private static int CountEnemiesInRange(double r) 
        { 
            try 
            { 
                // P3.1 - Performance: foreach manual au lieu de LINQ
                int count = 0;
                var units = ObjectManager.GetObjectsOfType<WoWUnit>();
                foreach (var u in units)
                {
                    if (u == null || !u.IsValid || !u.IsAlive) continue;
                    if (!u.IsHostile) continue;
                    if (u.Distance <= r)
                    {
                        count++;
                        // Optimisation: break early si on d�passe le seuil AOE commun
                        if (count >= 10) break;
                    }
                }
                return count;
            } 
            catch { return 0; } 
        }
        #endregion

        #region Subtlety Advanced
        // Lazy pooling logique (Boolean_25 revisit� pour parit� P1.6)
        private static bool ShouldLazyPoolSubtlety()
        {
            try
            {
                if (Me == null) return false;
                if (Me.Specialization != WoWSpec.RogueSubtlety) return false;
                if (!VitalicSettings.Instance.LazyPooling) return false;
                if (EventHandlers.ShouldPauseOffense()) return false; // pause g�n�rale
                if (CurrentTarget == null || !CurrentTarget.IsValid || !CurrentTarget.IsAlive) return false;

                // P2.2 - Pooling Avanc� pour Find Weakness
                // Si Vanish ou Shadow Dance bient�t disponible (< 2s), forcer le pooling
                if (ShouldPoolForFindWeakness())
                    return true;

                // Cible stun -> on ne pool pas (on profite du stun pour builder)
                if (TargetHasStunAura(CurrentTarget)) return false;

                // Distance > 30y requise
                if (CurrentTarget.Distance <= 30.0) return false;

                // Pas Find Weakness (91021)
                try { if (CurrentTarget.HasAura(91021)) return false; } catch { }

                // Slice and Dice > 4s
                if (Me.GetAuraTimeLeft("Slice and Dice") <= 4.0) return false;

                // Branche totem / entit� mineure
                bool isTotemLike = false;
                try { isTotemLike = CurrentTarget.IsTotem || CurrentTarget.CreatedBySpellId == 108921; } catch { }
                double energy = 0; try { energy = Me.CurrentEnergy; } catch { }
                if (isTotemLike)
                    return energy < 35.0; // seuil fixe totem

                // Seuil variable: conditions mode "bas" -> 40 sinon 70
                // Approximation des flags originaux: HP bas, Shadow Dance, ou buff �nergie (placeholder 185422)
                bool lowMode = false;
                try { lowMode = Me.HealthPercent <= 50.0 || Me.HasAura(51713) || Me.HasAura(185422); } catch { }
                double threshold = lowMode ? 40.0 : 70.0;
                return energy < threshold;
            }
            catch { return false; }
        }

        /// <summary>
        /// P2.2 - Pooling Avanc� pour Find Weakness (Subtlety)
        /// V�rifie si Vanish ou Shadow Dance sera bient�t disponible pour forcer le pooling
        /// </summary>
        private static bool ShouldPoolForFindWeakness()
        {
            try
            {
                if (Me == null) return false;
                if (Me.Specialization != WoWSpec.RogueSubtlety) return false;

                // V�rifier les CDs de Vanish et Shadow Dance
                double vanishCD = 0, shadowDanceCD = 0;
                
                try { vanishCD = SpellBook.GetSpellCooldown(SpellBook.Vanish); } catch { }
                try { shadowDanceCD = SpellBook.GetSpellCooldown(SpellBook.ShadowDance); } catch { }

                // Si l'un des sorts sera disponible dans < 2s, forcer le pooling (miroir plan)
                bool vanishSoon = vanishCD > 0 && vanishCD <= 2.0;
                bool shadowDanceSoon = shadowDanceCD > 0 && shadowDanceCD <= 2.0;

                return vanishSoon || shadowDanceSoon;
            }
            catch { return false; }
        }

        private static bool ShouldDelayHemorrhage()
        {
            try
            {
                if (Me == null) return false;
                if (Me.Specialization != WoWSpec.RogueSubtlety) return false;
                double hemoDelaySetting = 0; try { hemoDelaySetting = VitalicSettings.Instance.HemoDelay; } catch { hemoDelaySetting = 0; }
                if (hemoDelaySetting <= 0.0) return false;

                // Timer : attendre au moins HemoDelay secondes apr�s l'opener (Cheap/Garrote)
                if ((DateTime.UtcNow - _lastOpenerUtc).TotalSeconds < hemoDelaySetting)
                    return true; // on retarde encore Hemorrhage tant que fen�tre n'est pas �coul�e

                // SnD doit �tre confortable (>6s)
                if (Me.GetAuraTimeLeft("Slice and Dice") <= 6.0) return false;
                if (EventHandlers.ShouldPauseOffense()) return false; // pas de pause damage distincte, on r�utilise pause offense
                if (CurrentTarget == null || !CurrentTarget.IsValid) return false;

                // Conditions principales (distance / HP cible / cible quasi statique / �nergie basse)
                bool distanceOk = false; try { distanceOk = CurrentTarget.Distance > 20.0; } catch { }
                bool targetHpOk = false; try { targetHpOk = CurrentTarget.HealthPercent > 40.0; } catch { }
                bool targetStatic = false; try { targetStatic = CurrentTarget.MovementInfo != null && CurrentTarget.MovementInfo.CurrentSpeed < 7.0; } catch { }
                bool energyLow = false; try { energyLow = Me.CurrentEnergy < (Me.MaxEnergy - 20); } catch { }

                bool stunned = TargetHasStunAura(CurrentTarget); // cible stun = on peut retarder hemo pour aligner backstab plus tard
                bool notTargetingMe = true; try { notTargetingMe = !CurrentTarget.IsTargetingMeOrPet; } catch { }

                bool primaryGroup = distanceOk && targetHpOk && targetStatic && energyLow;
                bool gate = primaryGroup || stunned || notTargetingMe;
                if (!gate) return false;

                // Exclusion : si Garrote silence actif ET (en stealth/subterfuge) on ne retarde pas (on applique Hemo tout de suite pour saignement)
                bool hasGarrote = false; try { hasGarrote = CurrentTarget.HasAura(1330); } catch { }
                bool inStealthLike = false; try { inStealthLike = Me.HasAura("Stealth") || Me.HasAura(51713); } catch { }
                if (hasGarrote && inStealthLike)
                    return false;

                // Si �nergie d�j� tr�s haute (> threshold backstab pooling) on ne retarde pas, on veut consommer
                try { if (Me.CurrentEnergy > (Me.MaxEnergy - 10)) return false; } catch { }

                return true; // on retarde Hemorrhage -> pr�f�rer attendre/backstab plus tard
            }
            catch { return false; }
        }
        private static bool TargetHasStunAura(WoWUnit u)
        {
            if (u == null || !u.IsValid) return false;
            try
            {
                var auras = u.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i];
                    if (a == null) continue;
                    // Liste simplifi�e d'IDs stun MoP utilis�s ailleurs (Kidney 408, Cheap 1833, Bash 5211, Hammer 853, Shockwave 46968, Storm Bolt 107570)
                    int id = a.SpellId;
                    if (id == 408 || id == 1833 || id == 5211 || id == 853 || id == 46968 || id == 107570)
                        if (a.TimeLeft.TotalSeconds > 0.2) return true;
                }
            }
            catch { }
            return false;
        }
        #endregion

        #region Totem Stomp
        private static bool FindTotemTarget()
        {
            try
            {
                TotemRecentCache.Prune();
                foreach (var u in ObjectManager.GetObjectsOfType<WoWUnit>(false))
                {
                    if (u == null || !u.IsValid || u.IsDead) continue;
                    if (!u.IsTotem) continue;
                    if (u == CurrentTarget) continue; // ? current target (parit� Class84.Boolean_0)
                    if (TotemRecentCache.IsRecent(u)) continue; // anti-flicker
                    if (u.Distance > 8.0) continue;
                    if (!LosFacingCache.InLineOfSpellSightCached(u, 2000)) continue;
                    if (!LosFacingCache.IsSafelyFacingCached(u, 2000)) continue;
                    if (!CanAttackUnit(u)) continue;

                    int created = 0; try { created = (int)u.CreatedBySpellId; } catch { created = 0; }
                    if (created == 0) continue;

                    Totems flag;
                    if (!TotemMappings.SpellToTotemMap.TryGetValue(created, out flag)) continue;

                    // V�rifier mapping utilisateur
                    if ((VitalicSettings.Instance.TotemStomp & flag) == Totems.None) continue;

                    // V�rifier r�cence (parit� Class141.dictionary_0 TTL ~5s)
                    DateTime seen;
                    if (!EventHandlers.TotemRecency.TryGetValue(flag, out seen) || (DateTime.UtcNow - seen) > TotemRecencyTtl)
                        continue;

                    _totemTarget = u; TotemRecentCache.Mark(u); return true;
                }
                _totemTarget = null; return false;
            }
            catch { _totemTarget = null; return false; }
        }
        private static bool AttackTotemTarget(WoWUnit t)
        {
            try
            {
                if (t == null || !t.IsValid || t.IsDead) return false;
                if (SpellBook.CanCast(SpellBook.FanOfKnives) && SpellBook.Cast(SpellBook.FanOfKnives)) { Logger.Write("[PvE] Totem stomped with Fan of Knives: {0}", t.SafeName); return true; }
                if (Me.CurrentTarget != t) { t.Target(); Logger.Write("[PvE] Targeting totem: {0}", t.SafeName); }
                return false;
            }
            catch (Exception ex) { Logger.WriteException(ex, "PvERotation.AttackTotemTarget"); return false; }
        }
        private static bool CanAttackUnit(WoWUnit u) { try { return u != null && u.IsValid && u.IsAlive && u.Attackable && !u.IsFriendly; } catch { return false; } }
        #endregion

        private static void UpdateAoeAndBladeFlurry()
        {
            try
            {
                AoESuppressionHelper.ResetSuppressionFlag();
                EnemyCountCache.UpdateIfDue(AoeReevalMs);
                if (AoESuppressionHelper.ShouldSuppressAoeThisTick())
                    AoESuppressionHelper.SuppressAoeThisTick();
                if (Me != null && Me.Specialization == WoWSpec.RogueCombat)
                {
                    bool suppressed = AoESuppressionHelper.IsAoeSuppressed();
                    int melee = EnemyCountCache.Melee();
                    int threshold = 2; try { threshold = VitalicSettings.Instance.AoeThresholdBladeFlurry; } catch { }
                    bool antiBreak = false; try { antiBreak = CrowdControlManager.AnyBreakableCcWithin(10.0); } catch { }
                    bool wantOn = (!suppressed) && melee >= threshold && !antiBreak;
                    try { BladeFlurryToggle.SetDesired(wantOn); } catch { }
                }
            }
            catch { }
        }

        private static bool OffensiveGuardOK(WoWUnit t)
        {
            if (t == null || !t.IsValid || !t.IsAlive) return false;
            if (!LosFacingCache.InLineOfSpellSightCached(t, 2000)) return false;
            if (ImmunityGuard.TargetIsEffectivelyImmune(t, true)) { try { ImmunityGuard.HandleIfImmune(Me, t, true); } catch { } return false; }
            // Auto-face only before melee casts, if immobile, LOS OK, and not facing
            try
            {
                if (VitalicSettings.Instance.EventAutoFace)
                {
                    bool meMoving = Me != null && Me.MovementInfo != null && Me.MovementInfo.CurrentSpeed > 0.1;
                    bool inMelee = LosFacingCache.InMeleeRangeCached(t);
                    // Auto-face seulement bas� sur l'immobilit� du joueur, pas de la cible
                    if (!meMoving && inMelee && !LosFacingCache.IsSafelyFacingCached(t, 2000))
                    {
                        try { t.Face(); } catch { }
                    }
                }
            }
            catch { }
            return true;
        }

        private static void TryAutoFace(WoWUnit u)
        {
            try
            {
                if (u == null || !u.IsValid) return;
                if (!VitalicSettings.Instance.EventAutoFace) return;
                bool meMoving = Me != null && Me.MovementInfo != null && Me.MovementInfo.CurrentSpeed > 0.1;
                // Auto-face seulement bas� sur l'immobilit� du joueur, pas de la cible
                if (!meMoving && !Me.IsSafelyFacing(u))
                {
                    try { u.Face(); } catch { }
                }
            }
            catch { }
        }
    }
}
