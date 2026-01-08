// Routine Rogue pour Honorbuddy 5.4.8 (MoP).
// Compatible .NET Framework 4.5.1.

using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Windows.Forms;
using VitalicRotation.Helpers;
using VitalicRotation.Managers;
using VitalicRotation.Settings;
using VitalicRotation.UI;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation
{
    public class VitalicRotationRoutine : CombatRoutine
    {
        private static VitalicRotationRoutine _instance;
        public static VitalicRotationRoutine Instance { get { return _instance; } }

        public VitalicRotationRoutine() { _instance = this; }

        public override string Name { get { return "Vitalic Rogue"; } }
        public override WoWClass Class { get { return WoWClass.Rogue; } }

        private LocalPlayer Me { get { return StyxWoW.Me; } }
        private SettingsForm _settingsForm;

        // CTM (Click-to-Move / autoInteract) â€” on mÃ©morise l'Ã©tat pour restaurer
        private static string _autoInteractOrig;
        private static bool _ctmChanged;
        private static bool _ctmInitialized;

        // Garde pour Ã©viter double abonnement aux events HB
        private bool _eventsHooked;

        // RÃ©fÃ©rences des hooks (IMPORTANT : RemoveHook nÃ©cessite la mÃªme instance)
        private Composite _hookCombatMain;
        private Composite _hookCombatOnly;
        private Composite _hookPreCombat;
        private Composite _hookRest;
        private Composite _hookPull;
        // Cached rotation composites (single-path execution)
        private Composite _pveRotationComposite;
        private Composite _pvpRotationComposite;

        // Throttle pour l'update du status overlay Lua
        private static DateTime _lastUiUpd = DateTime.MinValue;
        private static bool _bootLogged;

        public override void Initialize()
        {
            _instance = this;

            Logger.Write("[Vitalic] Routine initialized.");

            // Defaults (parité v): Burst/Lazy ON, Pause & PauseDamage OFF (apply on each init like original)
            try { ToggleState.InitializeDefaults(); } catch { }
            RuntimeToggles.ForceClearAll();

            // 1) Entrées (miroir v.zip)
            MacroManager.Initialize();
            InputPoller.Initialize();

            _settingsForm = new SettingsForm();

            // Disable CTM (Click-to-Move) memorization
            try
            {
                if (!_ctmInitialized)
                {
                    _autoInteractOrig = Lua.GetReturnVal<string>("return GetCVar('autoInteract')", 0);
                    _ctmInitialized = true;
                }
                if (VitalicSettings.Instance.DisableCTM)
                {
                    if (!string.Equals(_autoInteractOrig, "0", StringComparison.Ordinal))
                    {
                        Lua.DoString("SetCVar('autoInteract','0')");
                        _ctmChanged = true;
                    }
                }
            }
            catch { }

            AudioBus.Initialize();
            EventHandlers.Initialize();
            EventHandlers.RegisterUiNotifications();
            EventHandlers.InitializeCore();
            VanishManager.Initialize();

            // UI injection (banner + status frame)
            try
            {
                VitalicUi.InitializeBannerLuaInjected(
                    VitalicSettings.Instance.AlertFontsEnabled,
                    VitalicSettings.Instance.SpellAlertsEnabled);
            }
            catch { }
            try { VitalicUi.EnsureAll(); } catch { }
            try
            {
                // Theme support is an extended feature; keep silent failure if palette absent
                var pal = UiTheme.Resolve(VitalicSettings.Instance.UIColorStyle);
                VitalicUi.ApplyOverlayTheme(pal);
            }
            catch { }
            VitalicUi.ShowNotify("Vitalic UI");
            // Optional: quick diagnostic banner to verify overlay visibility when DiagnosticMode is ON
            try { if (VitalicSettings.Instance.DiagnosticMode) VitalicUi.ShowMiniBanner("[Diag] Overlay OK", 0.6f, 1f, 0.6f, 1.0f); } catch { }
            VitalicUi.UpdateStatusAuto();
            try { VitalicUi.DebugDumpBanner("after-init"); } catch { }

            // Poison validation: normalize and persist user selection instead of resetting to numeric 0..5
            try { VitalicSettings.Instance.ValidatePoisons(); VitalicSettings.Instance.Save(); } catch { }

            // Hook bot events
            if (!_eventsHooked)
            {
                BotEvents.OnBotStarted += OnBotStart;
                BotEvents.OnBotStopped += OnBotStop;
                _eventsHooked = true;
            }

            _hookCombatMain = BuildCombatComposite();
            _hookCombatOnly = BuildCombatComposite();
            _hookPreCombat = BuildPreCombatComposite();
            _hookRest = BuildRestComposite();
            _hookPull = BuildPullComposite();

            TreeHooks.Instance.InsertHook("Combat_Main", 0, _hookCombatMain);
            TreeHooks.Instance.InsertHook("Combat_Only", 0, _hookCombatOnly);
            TreeHooks.Instance.InsertHook("PreCombatBuffs", 0, _hookPreCombat);
            TreeHooks.Instance.InsertHook("Rest", 0, _hookRest);
            TreeHooks.Instance.InsertHook("Pull", 0, _hookPull);

            Logger.Write("[Vitalic] Hooks registered.");
            if (!_bootLogged)
            {
                _bootLogged = true;
                Logger.Write("[Diag][Boot] Burst={0} Lazy={1} Pause={2} PauseDamage={3}", ToggleState.Burst, ToggleState.Lazy, ToggleState.Pause, ToggleState.PauseDamage);
            }
        }

        private void OnBotStart(EventArgs args)
        {
            Logger.Write("[Vitalic] Bot started.");
            DRTracker.Reset();
            RuntimeToggles.ForceClearAll();
            if (VitalicSettings.Instance.DiagnosticMode)
                Logger.Write("[Diag][Boot] (OnStart) Burst={0} Lazy={1} Pause={2} PauseDamage={3}", ToggleState.Burst, ToggleState.Lazy, ToggleState.Pause, ToggleState.PauseDamage);

            // Theme reapply only
            try
            {
                var pal = UiTheme.Resolve(VitalicSettings.Instance.UIColorStyle);
                VitalicUi.ApplyOverlayTheme(pal);
            }
            catch { }

            // Anti-AFK original style: single flag AntiAFK
            if (VitalicSettings.Instance.AntiAFK)
            {
                try { AntiAfk.StartIf(true); } catch { }
            }

            VitalicUi.ShowStatus();
            VitalicUi.UpdateStatusAuto();
            UiNotifications.Register();
            try { EventHandlers.HealerTrinketWatcher.Register(); } catch { }
            try { ThreatEvents.Register(); } catch { }
            try { if (VitalicSettings.Instance.DiagnosticMode) Diag.DumpSettings("Settings on bot start:"); } catch { }
        }
        private void OnBotStop(EventArgs args)
        {
            Logger.Write("[Vitalic] Bot stopped.");
            DRTracker.Reset();

            if (VitalicSettings.Instance.AntiAFK)
            {
                try { AntiAfk.Stop(); } catch { }
            }

            try
            {
                if (_ctmChanged && !string.IsNullOrEmpty(_autoInteractOrig))
                    Lua.DoString("SetCVar('autoInteract','" + _autoInteractOrig + "')");
            }
            catch (Exception ex)
            {
                Logger.Write("[Vitalic] CTM restore failed: " + ex.Message);
            }
            finally { _ctmChanged = false; }

            try { VitalicUi.Cleanup(); } catch { }
            try { if (VitalicSettings.Instance.DiagnosticMode) Diag.DumpSettings("Settings on bot stop:"); } catch { }
        }
        public override void ShutDown()
        {
            Logger.Write("[Vitalic] Routine shutdown.");
            try
            {
                if (_ctmChanged && !string.IsNullOrEmpty(_autoInteractOrig))
                    Lua.DoString("SetCVar('autoInteract','" + _autoInteractOrig + "')");
            }
            catch (Exception ex)
            {
                Logger.Write("[Vitalic] CTM restore failed: " + ex.Message);
            }
            finally { _ctmChanged = false; }

            // Full UI cleanup (fonts + frames)
            try { VitalicUi.Cleanup(); } catch { }

            VanishManager.Shutdown();
            AudioBus.Shutdown();
            EventHandlers.UnregisterUiNotifications();
            EventHandlers.Shutdown();
            EventHandlers.ShutdownCore();

            if (_eventsHooked)
            {
                try
                {
                    BotEvents.OnBotStarted -= OnBotStart;
                    BotEvents.OnBotStopped  -= OnBotStop;
                }
                catch { }
                _eventsHooked = false;
            }

            try
            {
                if (_hookCombatMain != null) TreeHooks.Instance.RemoveHook("Combat_Main", _hookCombatMain);
                if (_hookCombatOnly != null) TreeHooks.Instance.RemoveHook("Combat_Only", _hookCombatOnly);
                if (_hookPreCombat != null) TreeHooks.Instance.RemoveHook("PreCombatBuffs", _hookPreCombat);
                if (_hookRest != null) TreeHooks.Instance.RemoveHook("Rest", _hookRest);
                if (_hookPull != null) TreeHooks.Instance.RemoveHook("Pull", _hookPull);
            }
            catch { }

            InputPoller.Shutdown();
            Logger.Write("[Vitalic] Hooks unregistered.");
            Logger.Write("All hooks removed.");
        }

        public override bool WantButton { get { return true; } }

        public override void OnButtonPress()
        {
            Logger.Write("[Vitalic] Opening settings form...");

            if (_settingsForm == null || _settingsForm.IsDisposed)
                _settingsForm = new SettingsForm();

            _settingsForm.StartPosition = FormStartPosition.CenterScreen;
            _settingsForm.Show();
            _settingsForm.BringToFront();
        }

        public override void Pulse()
        {
            try
            {
                if (!StyxWoW.IsInGame || !StyxWoW.IsInWorld)
                    return;

                InputPoller.Poll();
                MacroManager.Poll();
                EventHandlers.Pulse();
                try { CombatContext.Update(); } catch { }
                VanishManager.Execute();

                // OOC managers
                PoisonManager.Execute();
                StealthManager.Execute();
                CrowdControlManager.Execute();

                // Anti-AFK simple (mirror original) – ignore extended QoL master flag
                if (VitalicSettings.Instance.AntiAFK)
                {
                    try { AntiAfk.Pulse(); } catch { }
                }

                try { if (!StyxWoW.Me.Combat) RestealthManager.NotifyCombatEnded(); } catch { }

                var now = DateTime.UtcNow;
                if ((now - _lastUiUpd).TotalMilliseconds >= 200)
                {
                    _lastUiUpd = now;
                    VitalicUi.UpdateStatusAuto();
                }
            }
            catch (Exception ex)
            {
                Logger.Write("[Vitalic] Pulse error: " + ex.Message);
            }
        }

        // ================= Composites (on construit des instances pour TreeHooks) ================

        private Composite BuildCombatComposite()
        {
            // Build rotation composites once
            if (_pveRotationComposite == null) _pveRotationComposite = Managers.PvERotation.Build();
            if (_pvpRotationComposite == null) _pvpRotationComposite = Managers.PvPRotation.Build();

            return new PrioritySelector(
                HandleQueuedMacros(),
                // Mobilité en combat (Sprint / Shadowstep anti-gap) avant rotation (ne bloque pas)
                new Action(ret => { try { MobilityManager.Execute(); } catch { } return RunStatus.Failure; }),
                // Choix dynamique PvE / PvP (un seul chemin actif)
                new Decorator(ret => UsePvE(), _pveRotationComposite),
                new Decorator(ret => !UsePvE(), _pvpRotationComposite)
            );
        }

        private Composite BuildPreCombatComposite()
        {
            return new PrioritySelector(
                HandleQueuedMacros(),
                new Decorator(ret => !Me.IsAlive, new ActionAlwaysSucceed()),
                new Action(ret =>
                {
                    // OOC: Stealth + Poisons
                    StealthManager.Execute();
                    PoisonManager.Execute();
                    return RunStatus.Success;
                })
            );
        }

        private Composite BuildRestComposite()
        {
            return new PrioritySelector(
                HandleQueuedMacros(), // permet /garrote /cheapshot en furtif OOC
                new Decorator(ret => !Me.IsAlive, new ActionAlwaysSucceed()),
                // QoL: Anti-AFK pulse (hors combat seulement)
                new Action(ret => { try { AntiAfk.Pulse(); } catch { } return RunStatus.Failure; }),
                new Decorator(ret => VitalicSettings.Instance.AlwaysStealth && !Me.Combat && !Me.Mounted,
                    new Action(ret =>
                    {
                        if (!Me.HasAura("Stealth") && SpellBook.CanCast(SpellBook.Stealth))
                            SpellBook.Cast(SpellBook.Stealth, Me);
                        return RunStatus.Success;
                    }))
            );
        }

        private Composite BuildPullComposite()
        {
            return new PrioritySelector(
                HandleQueuedMacros(),
                new Decorator(ret => Me.CurrentTarget == null || !Me.CurrentTarget.IsAlive, new ActionAlwaysFail()),
                new Decorator(ret => !Me.HasAura("Stealth") && SpellBook.CanCast(SpellBook.Stealth),
                    new Action(ret =>
                    {
                        Logger.Write("[Vitalic] Pull -> Entering Stealth");
                        SpellBook.Cast(SpellBook.Stealth, Me);
                        return RunStatus.Success;
                    }))
            );
        }

        // ================= Overrides CombatRoutine (HB peut lire ces propriÃ©tÃ©s) =================

        public override Composite CombatBehavior
        {
            get { return _hookCombatMain ?? (_hookCombatMain = BuildCombatComposite()); }
        }

        public override Composite PreCombatBuffBehavior
        {
            get { return _hookPreCombat ?? (_hookPreCombat = BuildPreCombatComposite()); }
        }

        public override Composite RestBehavior
        {
            get { return _hookRest ?? (_hookRest = BuildRestComposite()); }
        }

        public override Composite PullBehavior
        {
            get { return _hookPull ?? (_hookPull = BuildPullComposite()); }
        }

        // ========================== Macros en file (slash-commands) ==============================

        private Composite HandleQueuedMacros()
        {
            return new PrioritySelector(
                new Action(delegate
                {
                    // Poll Lua macro variables each tick before consuming queue (parité v.zip)
                    try { MacroManager.Poll(); } catch { }

                    WoWUnit target;

                    // Fast Kick
                    if (MacroManager.TryDequeue("Fast Kick", out target))
                    {
                        // Resolve best unit (focus > mouseover > target)
                        var resolved = MacroManager.ResolveMacroUnit() ?? target;
                        if (resolved != null) try { resolved.Target(); } catch { }
                        // Use full interrupt pipeline (focus/mouseover/target + step/gouge)
                        if (InterruptManager.TryFastKickWithFallback())
                        {
                            Logger.Write("[Vitalic] Macro -> Fast Kick (pipeline)");
                            return RunStatus.Success;
                        }
                        else
                        {
                            // Banner feedback for failed macro
                            VitalicUi.ShowMiniBanner("Kick indisponible", 1f, 0.4f, 0.4f, 1f);
                        }
                        return RunStatus.Failure;
                    }

                    // Kidney Shot + Shuriken (logique originale de Vitalic)
                    if (MacroManager.TryDequeue("Kidney Shot + Shuriken", out target))
                    {
                        var resolved = MacroManager.ResolveMacroUnit() ?? target; if (resolved != null) try { resolved.Target(); } catch { }
                        
                        // Logique originale: si hors mêlée, lancer Shuriken/Throw
                        bool outOfMelee = !SpellBook.InMeleeRange(resolved);
                        if (outOfMelee)
                        {
                            if (SpellBook.HasShurikenTossTalent && SpellBook.CanCast(SpellBook.ShurikenToss, resolved))
                            {
                                Logger.Write("[Vitalic] Macro -> Shuriken Toss");
                                return SpellBook.Cast(SpellBook.ShurikenToss, resolved) ? RunStatus.Success : RunStatus.Failure;
                            }
                            if (SpellBook.CanCast(SpellBook.Throw, resolved))
                            {
                                Logger.Write("[Vitalic] Macro -> Throw");
                                return SpellBook.Cast(SpellBook.Throw, resolved) ? RunStatus.Success : RunStatus.Failure;
                            }
                            return RunStatus.Failure;
                        }

                        // Tentative Redirect si CP sur focus et 0 sur target (parité AutoRedirect)
                        try
                        {
                            if (SpellBook.CanCast(SpellBook.Redirect))
                            {
                                int cpOnTarget = Me.ComboPoints;
                                int cpOnFocus = 0;
                                try { cpOnFocus = Lua.GetReturnVal<int>("return GetComboPoints('player','focus') or 0", 0); } catch { }
                                if (cpOnTarget == 0 && cpOnFocus > 0)
                                {
                                    Logger.Write("[Vitalic] Macro -> Redirect (CP transfer)");
                                    SpellBook.Cast(SpellBook.Redirect);
                                    LuaHelper.SleepForLag();
                                }
                            }
                        }
                        catch { }

                        // Enchaîner sur le Kidney avec feedback
                        if (resolved != null)
                        {
                            CrowdControlManager.KidneyFailReason r;
                            if (CrowdControlManager.TryKidney(resolved, out r))
                                return RunStatus.Success;

                            // Raison précise + sticky retry pour échecs transitoires
                            switch (r)
                            {
                                case CrowdControlManager.KidneyFailReason.Gcd:
                                    VitalicUi.ShowMiniBanner("Kidney: GCD", 1f, .4f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { } MacroManager.ExtendSticky("Kidney Shot + Shuriken", 0.4); MacroManager.RequeueIfSticky("Kidney Shot + Shuriken"); break;
                                case CrowdControlManager.KidneyFailReason.Timing:
                                    VitalicUi.ShowMiniBanner("Kidney: attendre après opener", 1f, .6f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { } MacroManager.ExtendSticky("Kidney Shot + Shuriken", 0.5); MacroManager.RequeueIfSticky("Kidney Shot + Shuriken"); break;
                                case CrowdControlManager.KidneyFailReason.DR:
                                    VitalicUi.ShowMiniBanner("Kidney: DR Stun", 1f, .4f, .6f); break;
                                case CrowdControlManager.KidneyFailReason.Position:
                                    // Montrer bannière position et laisser sticky actif brièvement
                                    if (!LosFacingCache.InLineOfSpellSightCached(resolved, 2000)) VitalicUi.ShowMiniBanner("Kidney: LOS", 1f, .4f, .4f);
                                    else VitalicUi.ShowMiniBanner("Kidney: hors mêlée", 1f, .4f, .4f);
                                    try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { }
                                    MacroManager.ExtendSticky("Kidney Shot + Shuriken", 0.5); MacroManager.RequeueIfSticky("Kidney Shot + Shuriken");
                                    break;
                                case CrowdControlManager.KidneyFailReason.ComboPoints:
                                    VitalicUi.ShowMiniBanner("Kidney: CP insuffisants", 1f, .4f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { } MacroManager.ExtendSticky("Kidney Shot + Shuriken", 0.6); MacroManager.RequeueIfSticky("Kidney Shot + Shuriken"); break;
                                case CrowdControlManager.KidneyFailReason.Energy:
                                    VitalicUi.ShowMiniBanner("Kidney: énergie basse", 1f, .4f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { } MacroManager.ExtendSticky("Kidney Shot + Shuriken", 0.6); MacroManager.RequeueIfSticky("Kidney Shot + Shuriken"); break;
                                default:
                                    VitalicUi.ShowMiniBanner("Kidney indisponible", 1f, 0.4f, 0.4f, 1f); break;
                            }
                            return RunStatus.Failure;
                        }

                        // Pas de cible résolue
                            VitalicUi.ShowMiniBanner("Kidney: pas de cible", 1f, .4f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { }
                        return RunStatus.Failure;
                    }

                    // Kidney Shot (sans shuriken)
                    if (MacroManager.TryDequeue("Kidney Shot", out target))
                    {
                        var resolved = MacroManager.ResolveMacroUnit() ?? target; if (resolved != null) try { resolved.Target(); } catch { }
                        
                        // v.zip : ne pas Kidney si hard CC déjà présent
                        if (resolved != null && DRTracker.HasHardCcNoDisarm(resolved))
                        {
                            VitalicUi.ShowMiniBanner("Kidney impossible (hard CC)", 1f, 0.4f, 0.4f, 1f);
                            return RunStatus.Failure;
                        }

                        // Tentative Redirect si CP sur focus et 0 sur target (parité AutoRedirect)
                        try
                        {
                            if (SpellBook.CanCast(SpellBook.Redirect))
                            {
                                int cpOnTarget = Me.ComboPoints;
                                int cpOnFocus = 0;
                                try { cpOnFocus = Lua.GetReturnVal<int>("return GetComboPoints('player','focus') or 0", 0); } catch { }
                                if (cpOnTarget == 0 && cpOnFocus > 0)
                                {
                                    Logger.Write("[Vitalic] Macro -> Redirect (CP transfer)");
                                    SpellBook.Cast(SpellBook.Redirect);
                                    LuaHelper.SleepForLag();
                                }
                            }
                        }
                        catch { }
                        
                        // Utilise la même voie que le +Shuriken (CrowdControlManager) avec feedback
                        if (resolved != null)
                        {
                            CrowdControlManager.KidneyFailReason r;
                            if (CrowdControlManager.TryKidney(resolved, out r))
                            {
                                Logger.Write("[Vitalic] Macro -> Kidney Shot");
                                return RunStatus.Success;
                            }
                            switch (r)
                            {
                                case CrowdControlManager.KidneyFailReason.Gcd:
                                    VitalicUi.ShowMiniBanner("Kidney: GCD", 1f, .4f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { } MacroManager.ExtendSticky("Kidney Shot", 0.4); MacroManager.RequeueIfSticky("Kidney Shot"); break;
                                case CrowdControlManager.KidneyFailReason.Timing:
                                    VitalicUi.ShowMiniBanner("Kidney: attendre après opener", 1f, .6f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { } MacroManager.ExtendSticky("Kidney Shot", 0.5); MacroManager.RequeueIfSticky("Kidney Shot"); break;
                                case CrowdControlManager.KidneyFailReason.DR:
                                    VitalicUi.ShowMiniBanner("Kidney: DR Stun", 1f, .4f, .6f); break;
                                case CrowdControlManager.KidneyFailReason.Position:
                                    if (!LosFacingCache.InLineOfSpellSightCached(resolved, 2000)) VitalicUi.ShowMiniBanner("Kidney: LOS", 1f, .4f, .4f);
                                    else VitalicUi.ShowMiniBanner("Kidney: hors mêlée", 1f, .4f, .4f);
                                    try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { }
                                    MacroManager.ExtendSticky("Kidney Shot", 0.5); MacroManager.RequeueIfSticky("Kidney Shot");
                                    break;
                                case CrowdControlManager.KidneyFailReason.ComboPoints:
                                    VitalicUi.ShowMiniBanner("Kidney: CP insuffisants", 1f, .4f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { } MacroManager.ExtendSticky("Kidney Shot", 0.6); MacroManager.RequeueIfSticky("Kidney Shot"); break;
                                case CrowdControlManager.KidneyFailReason.Energy:
                                    VitalicUi.ShowMiniBanner("Kidney: énergie basse", 1f, .4f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { } MacroManager.ExtendSticky("Kidney Shot", 0.6); MacroManager.RequeueIfSticky("Kidney Shot"); break;
                                default:
                                    VitalicUi.ShowMiniBanner("Kidney indisponible", 1f, 0.4f, 0.4f, 1f); break;
                            }
                            return RunStatus.Failure;
                        }

                        VitalicUi.ShowMiniBanner("Kidney: pas de cible", 1f, .4f, .4f); try { if (VitalicSettings.Instance.SoundAlertsEnabled) AudioBus.PlayEvent(); } catch { }
                        return RunStatus.Failure;
                    }

                    // Gouge
                    if (MacroManager.TryDequeue("Gouge", out target))
                    {
                        var resolved = MacroManager.ResolveMacroUnit() ?? target; if (resolved != null) try { resolved.Target(); } catch { }
                        
                        // v.zip : éviter d'empiler sur Incap/Disorient existant
                        if (resolved != null && DRTracker.HasIncapOrDisorient(resolved))
                        {
                            VitalicUi.ShowMiniBanner("Gouge impossible (Incap/Disorient)", 1f, 0.4f, 0.4f, 1f);
                            return RunStatus.Failure;
                        }
                        
                        if (resolved != null && SpellBook.CanCast(SpellBook.Gouge, resolved))
                        {
                            Logger.Write("[Vitalic] Macro -> Gouge");
                            SpellBook.Cast(SpellBook.Gouge, resolved);
                            return RunStatus.Success;
                        }
                        // Bannières d'échec utiles
                        if (resolved == null) { VitalicUi.ShowMiniBanner("Gouge: pas de cible", 1f, .4f, .4f); return RunStatus.Failure; }
                        if (!LosFacingCache.InLineOfSpellSightCached(resolved, 2000)) { VitalicUi.ShowMiniBanner("Gouge: LOS", 1f, .4f, .4f); return RunStatus.Failure; }
                        if (!SpellBook.InMeleeRange(resolved, 5.0)) { VitalicUi.ShowMiniBanner("Gouge: hors mêlée", 1f, .4f, .4f); return RunStatus.Failure; }
                        VitalicUi.ShowMiniBanner("Gouge indisponible", 1f, 0.4f, 0.4f, 1f);
                        return RunStatus.Failure;
                    }

                    // Redirect Kidney (coller à l'original)
                    if (MacroManager.TryDequeue("Redirect Kidney", out target))
                    {
                        var resolved = MacroManager.ResolveMacroUnit() ?? target; if (resolved != null) try { resolved.Target(); } catch { }

                        // 1) Redirect si CP ailleurs (comme AutoRedirect)
                        if (SpellBook.CanCast(SpellBook.Redirect))
                        {
                            int cpOnTarget = Me.ComboPoints;
                            int cpOnFocus = 0;
                            try { cpOnFocus = Lua.GetReturnVal<int>("return GetComboPoints('player','focus') or 0", 0); } catch { }

                            // Redirect si CP sur focus et aucun sur target
                            if (cpOnTarget == 0 && cpOnFocus > 0)
                            {
                                Logger.Write("[Vitalic] Macro -> Redirect (CP transfer)");
                                SpellBook.Cast(SpellBook.Redirect);
                                LuaHelper.SleepForLag(50); // remplacé Sleep -> SleepForLag
                            }
                        }

                        // 2) Puis Kidney sur la cible choisie
                        if (resolved != null && CrowdControlManager.TryKidney(resolved))
                        {
                            Logger.Write("[Vitalic] Macro -> Redirect Kidney");
                            return RunStatus.Success;
                        }

                        VitalicUi.ShowMiniBanner("Redirect/Kidney indisponible", 1f, 0.4f, 0.4f, 1f);
                        return RunStatus.Failure;
                    }

                    // Blind
                    if (MacroManager.TryDequeue("Blind", out target))
                    {
                        var resolved = MacroManager.ResolveMacroUnit() ?? target; if (resolved != null) try { resolved.Target(); } catch { }

                        // Manual macro path first (no hard-cast requirement), then fallback to auto gating
                        if (resolved != null && (CrowdControlManager.TryBlindManual(resolved) || CrowdControlManager.TryBlind(resolved)))
                        {
                            Logger.Write("[Vitalic] Macro -> Blind");
                            return RunStatus.Success;
                        }

                        // Echec: feedback utilisateur
                        if (resolved == null) { VitalicUi.ShowMiniBanner("Blind: pas de cible", 1f, .4f, .4f); return RunStatus.Failure; }
                        if (!LosFacingCache.InLineOfSpellSightCached(resolved, 2000)) { VitalicUi.ShowMiniBanner("Blind: LOS", 1f, .4f, .4f); return RunStatus.Failure; }
                        VitalicUi.ShowMiniBanner("Blind indisponible", 1f, 0.4f, 0.4f, 1f);
                        return RunStatus.Failure;
                    }

                    // Garrote
                    if (MacroManager.TryDequeue("Garrote", out target))
                    {
                        var resolved = MacroManager.ResolveMacroUnit() ?? target; if (resolved != null) try { resolved.Target(); } catch { }
                        
                        // Vérification de furtivité comme dans Vitalic original
                        bool openerState = false; 
                        try { openerState = Me.HasAura("Stealth") || Me.HasAura("Subterfuge") || Me.HasAura("Shadow Dance"); } 
                        catch { }
                        
                        if (!openerState)
                        {
                            VitalicUi.ShowMiniBanner("Garrote: pas furtif", 1f, 0.4f, 0.4f, 1f);
                            return RunStatus.Failure;
                        }
                        
                        if (SpellBook.CanCast(SpellBook.Garrote, resolved))
                        {
                            Logger.Write("[Vitalic] Macro -> Garrote");
                            SpellBook.Cast(SpellBook.Garrote, resolved);
                            // Notifier qu'un opener a été utilisé
                            CrowdControlManager.NotifyOpenerUsed();
                            // Armer aussi les verrous d'opener (global + par cible) pour éviter le double opener
                            try { Managers.PvPRotation.NotifyManualOpenerUsed(); } catch { }
                            return RunStatus.Success;
                        }
                        VitalicUi.ShowMiniBanner("Garrote indisponible", 1f, 0.4f, 0.4f, 1f);
                        return RunStatus.Failure;
                    }

                    // Cheap Shot
                    if (MacroManager.TryDequeue("Cheap Shot", out target))
                    {
                        var resolved = MacroManager.ResolveMacroUnit() ?? target; if (resolved != null) try { resolved.Target(); } catch { }
                        
                        // Vérification de furtivité comme dans Vitalic original
                        bool openerState = false; 
                        try { openerState = Me.HasAura("Stealth") || Me.HasAura("Subterfuge") || Me.HasAura("Shadow Dance"); } 
                        catch { }
                        
                        if (!openerState)
                        {
                            VitalicUi.ShowMiniBanner("Cheap Shot: pas furtif", 1f, 0.4f, 0.4f, 1f);
                            return RunStatus.Failure;
                        }
                        
                        if (SpellBook.CanCast(SpellBook.CheapShot, resolved))
                        {
                            Logger.Write("[Vitalic] Macro -> Cheap Shot");
                            SpellBook.Cast(SpellBook.CheapShot, resolved);
                            // Notifier le gestionnaire CC qu'un Cheap Shot a été utilisé
                            CrowdControlManager.NotifyCheapShotUsed();
                            // Armer aussi les verrous d'opener (global + par cible) pour éviter le double opener
                            try { Managers.PvPRotation.NotifyManualOpenerUsed(); } catch { }
                            return RunStatus.Success;
                        }
                        VitalicUi.ShowMiniBanner("Cheap Shot indisponible", 1f, 0.4f, 0.4f, 1f);
                        return RunStatus.Failure;
                    }

                    // Smoke Bomb: gÃ©rÃ© par SmokeBombManager (Auto + Hotkey) comme dans l'original

                    // Restealth (Toggle)
                    if (MacroManager.TryDequeue("Restealth", out target))
                    {
                        // Active/désactive le mode Restealth (comme v.zip)
                        if (RestealthManager.Toggle())
                        {
                            VitalicUi.ShowMiniBanner("Restealth: Enabled", 0.7f, 1f, 0.7f);
                            try { Styx.CommonBot.Mount.Dismount(); } catch { }
                        }
                        else
                        {
                            VitalicUi.ShowMiniBanner("Restealth: Disabled", 1f, 0.4f, 0.4f);
                        }
                        return RunStatus.Success;
                    }

                    // Toggle (PauseDamage toggle - supprimé pour éviter bascule surprise via /toggle)
                    // (ancien bloc géré ici retiré; seul hotkey dédié conserve la fonction)

                    // (Pas de "Focus Macro" dans v.zip â€“ on ne gÃ¨re pas ce cas)

                    return RunStatus.Failure;
                })
            );
        }

        // Simplified UsePvE: only PveMode boolean + arena/bg/player checks
        private static bool UsePvE()
        {
            try { if (VitalicSettings.Instance.PveMode) return true; } catch { }

            // Instance type: arena or battleground => PvP
            try
            {
                string it = Lua.GetReturnVal<string>("local a,t=IsInInstance(); return tostring(t or '')", 0);
                if (string.Equals(it, "arena", StringComparison.OrdinalIgnoreCase)) return false;
                if (string.Equals(it, "pvp", StringComparison.OrdinalIgnoreCase)) return false;
            }
            catch { }

            // Target hostile player => PvP (arena/bg/world duel)
            try
            {
                var me = StyxWoW.Me; var t = me != null ? me.CurrentTarget as WoWUnit : null;
                if (t != null && t.IsAlive)
                {
                    bool isPlayer = false, friendly = false, attackable = false;
                    try { isPlayer = t.IsPlayer; } catch { }
                    try { friendly = t.IsFriendly; } catch { }
                    try { attackable = t.Attackable; } catch { }
                    if (VitalicSettings.Instance.DiagnosticMode)
                    {
                        Logger.Write("[Diag][UsePvE] Target={0} isPlayer={1} friendly={2} attackable={3}", 
                                   t.SafeName, isPlayer, friendly, attackable);
                    }
                    // If it's a player and we can attack it, prefer PvP path even outside instances (duel/world PvP)
                    if (isPlayer && attackable && !friendly) return false;
                }
            }
            catch { }

            bool result = true; // default PvE
            if (VitalicSettings.Instance.DiagnosticMode)
            {
                Logger.Write("[Diag][UsePvE] Result={0}", result);
            }
            return result;
        }
    }
}

