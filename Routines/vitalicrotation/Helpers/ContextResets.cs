using System;
using VitalicRotation.Managers;
using VitalicRotation.UI;

namespace VitalicRotation.Helpers
{
    /// <summary>
    /// Centralise tous les "resets" et nettoyages de contexte lors d'événements critiques.
    /// Régroupe les purges/resets pour faciliter la maintenance.
    /// </summary>
    internal static class ContextResets
    {
        private static DateTime _lastSoftResetUtc = DateTime.MinValue;

        /// <summary>
        /// Reset complet lors d'événements majeurs (PLAYER_ENTERING_WORLD).
        /// Remet à zéro tous les systèmes de tracking.
        /// </summary>
        public static void HardReset(string reason)
        {
            Logger.Write("[Core] HardReset ({0})", reason);

            // DR, menaces, CC team, caches unitaires
            SafeReset(() => DRTracker.Reset(), "DRTracker.Reset");
            SafeReset(() => ThreatTable.Reset(), "ThreatTable.Reset");
            SafeReset(() => TeamCCManager.Reset(), "TeamCCManager.Reset");
            SafeReset(() => FocusManager.Reset(), "FocusManager.Reset");
            SafeReset(() => UnitExtensions.ClearCaches(), "UnitExtensions.ClearCaches");

            // Macros / files / toggles volatiles
            SafeReset(() => MacroManager.ResetQueue(), "MacroManager.ResetQueue");
            SafeReset(() => ToggleState.InitializeDefaults(), "ToggleState.InitializeDefaults");

            // Overlay: rafraîchit l'état
            SafeReset(() => VitalicUi.RefreshAll(), "VitalicUi.RefreshAll");
        }

        /// <summary>
        /// Reset partiel lors d'événements mineurs (ZONE_CHANGED_NEW_AREA).
        /// Nettoie seulement les données expirées ou obsolètes.
        /// </summary>
        public static void SoftReset(string reason)
        {
            // Anti-spam si on traverse rapidement plusieurs zones
            if (DateTime.UtcNow - _lastSoftResetUtc < TimeSpan.FromSeconds(2))
                return;

            _lastSoftResetUtc = DateTime.UtcNow;
            Logger.Write("[Core] SoftReset ({0})", reason);

            SafeReset(() => DRTracker.PruneExpired(), "DRTracker.PruneExpired");
            SafeReset(() => ThreatTable.Prune(), "ThreatTable.Prune");
            SafeReset(() => TeamCCManager.Prune(), "TeamCCManager.Prune");
            SafeReset(() => FocusManager.ValidateOrClear(), "FocusManager.ValidateOrClear");
        }

        /// <summary>
        /// Rafraîchissement spécial pendant la préparation d'arène.
        /// Remet les DR à clean + assigne automatiquement le focus sur un healer si possible.
        /// </summary>
        public static void ArenaPrepRefresh()
        {
            // En "prépa arène", on remet les DR à clean + focus heuristique
            Logger.Write("[Core] Arena prep refresh");
            SafeReset(() => DRTracker.Reset(), "DRTracker.Reset (ArenaPrep)");
            SafeReset(() => FocusManager.AutoAssignHealerFocusIfNone(), "FocusManager.AutoAssignHealerFocusIfNone");
        }

        /// <summary>
        /// Nettoyage léger à la sortie de combat.
        /// Purge les données expirées pour éviter qu'elles traînent.
        /// </summary>
        public static void OutOfCombatPrune()
        {
            Logger.Write("[Core] OOC prune");
            SafeReset(() => DRTracker.PruneExpired(), "DRTracker.PruneExpired (OOC)");
            SafeReset(() => ThreatTable.Prune(), "ThreatTable.Prune (OOC)");
            SafeReset(() => TeamCCManager.Prune(), "TeamCCManager.Prune (OOC)");
        }

        /// <summary>
        /// Nettoyage rapide à l'entrée en combat.
        /// Limite les DR obsolètes qui traîneraient à l'entrée de combat.
        /// </summary>
        public static void InCombatSanity()
        {
            // Limite les DR obsolètes qui traîneraient à l'entrée de combat
            SafeReset(() => DRTracker.PruneExpired(), "DRTracker.PruneExpired (InCombat)");
        }

        /// <summary>
        /// Enveloppe sécurisée pour exécuter les resets sans faire crasher le système.
        /// </summary>
        private static void SafeReset(Action act, string tag)
        {
            try { act(); }
            catch (Exception ex) { Logger.Write("[Core] Reset guard failed in {0}: {1}", tag, ex.Message); }
        }
    }
}