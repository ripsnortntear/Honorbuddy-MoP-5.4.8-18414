using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using VitalicRotation.Helpers; // pour DRTracker.*
using VitalicRotation.Settings;
using VitalicRotation.UI;

namespace VitalicRotation.Managers
{
    public static class TeamCCManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        // === Auto-Blind Healer Trinket ===
        private static readonly System.Random _rng = new System.Random();
        private static DateTime _lastAutoBlind = DateTime.MinValue;

        // Collections internes pour le tracking des CC d'équipe
        private static readonly HashSet<AllyStun> _allyStuns = new HashSet<AllyStun>();
        private static readonly HashSet<AllyIncap> _allyIncaps = new HashSet<AllyIncap>();
        private static readonly HashSet<AllySilence> _allySilences = new HashSet<AllySilence>();

        // Helper: DoT rogue présents (Rupture, Garrote, Hemorrhage, Crimson Tempest)
        private static bool HasRogueDot(WoWUnit u)
        {
            if (u == null || !u.IsValid) return false;
            try
            {
                // Préfère "mes auras" si exposé par la stack, sinon fallback générique
                bool any = false;
                try { any = u.HasMyAura(SpellBook.Rupture) || u.HasMyAura(SpellBook.Garrote) || u.HasMyAura(SpellBook.Hemorrhage) || u.HasMyAura(SpellBook.CrimsonTempest); } catch { any = false; }
                if (any) return true;

                // Fallback: scan des auras par ID
                var auras = u.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i]; if (a == null) continue;
                    int id = a.SpellId;
                    if (id == SpellBook.Rupture || id == SpellBook.Garrote || id == SpellBook.Hemorrhage || id == SpellBook.CrimsonTempest)
                        return true;
                }
            }
            catch { }
            return false;
        }

        // Classes internes pour le tracking
        private class AllyStun
        {
            public ulong AllyGuid;
            public ulong CasterGuid;
            public DateTime EndUtc;

            public override bool Equals(object obj)
            {
                var other = obj as AllyStun;
                return other != null && AllyGuid == other.AllyGuid && CasterGuid == other.CasterGuid;
            }

            public override int GetHashCode()
            {
                return (int)(AllyGuid ^ CasterGuid);
            }
        }

        private class AllyIncap
        {
            public ulong AllyGuid;
            public ulong CasterGuid;
            public DateTime EndUtc;

            public override bool Equals(object obj)
            {
                var other = obj as AllyIncap;
                return other != null && AllyGuid == other.AllyGuid && CasterGuid == other.CasterGuid;
            }

            public override int GetHashCode()
            {
                return (int)(AllyGuid ^ CasterGuid);
            }
        }

        private class AllySilence
        {
            public ulong AllyGuid;
            public ulong CasterGuid;
            public DateTime EndUtc;

            public override bool Equals(object obj)
            {
                var other = obj as AllySilence;
                return other != null && AllyGuid == other.AllyGuid && CasterGuid == other.CasterGuid;
            }

            public override int GetHashCode()
            {
                return (int)(AllyGuid ^ CasterGuid);
            }
        }

        /// <summary>
        /// Reset complet du tracking des CC d'équipe (utilisé lors d'un hard reset).
        /// </summary>
        public static void Reset()
        {
            _allyStuns.Clear();
            _allyIncaps.Clear();
            _allySilences.Clear();
        }

        /// <summary>
        /// Purge les entrées de CC d'équipe expirées (utilisé lors des soft resets).
        /// </summary>
        public static void Prune()
        {
            DateTime now = DateTime.UtcNow;

            _allyStuns.RemoveWhere(x => x.EndUtc <= now);
            _allyIncaps.RemoveWhere(x => x.EndUtc <= now);
            _allySilences.RemoveWhere(x => x.EndUtc <= now);
        }

        public static void OnFocusChanged(string unitId)
        {
            // vider caches DR/fenêtres si tu relies certains CC au focus
        }

        /// <summary>
        /// Tente un Blind automatique sur un soigneur ennemi qui vient d'utiliser un break (Trinket/EMfH/WotF...).
        /// Fenêtre pilotée par EventHandlers (2.5s). Appelée très tôt en PvP.
        /// NOTE: Dans TeamCCManager on classe Blind comme Disorient (asymétrie voulue v.zip).
        /// </summary>
        internal static bool TryAutoBlindHealerTrinket()
        {
            try
            {
                if (!VitalicSettings.Instance.AutoBlindHealerTrinket)
                    return false;

                // throttle anti-spam global (évite double déclenchement)
                if ((DateTime.UtcNow - _lastAutoBlind).TotalMilliseconds < 600)
                    return false;

                var units = ObjectManager.GetObjectsOfType<WoWUnit>();
                WoWUnit target = null;
                for (int i = 0; i < units.Count; i++)
                {
                    var u = units[i];
                    if (u == null || !u.IsValid || u.IsFriendly) continue;
                    if (!EventHandlers.IsHealerClass(u)) continue;
                    if (!EventHandlers.HasHealerTrinketWindow(u.Guid)) continue;

                    // Conditions minimales
                    try { if (!u.InLineOfSpellSight) continue; } catch { }
                    try { if (u.Distance > 10.0) continue; } catch { }
                    if (u.IsDead) continue;

                    // DR & immunités: Blind = Disorient ici (asymétrie voulue)
                    if (!DRTracker.Can(u, DRTracker.DrCategory.Disorient))
                        continue;

                    if (ImmunityGuard.TargetIsEffectivelyImmune(u, true))
                        continue;

                    // Ne pas blind si DoT rogue présent (évite de casser)
                    if (HasRogueDot(u))
                        continue;

                    target = u;
                    break;
                }

                if (target == null)
                    return false;

                try { if (MacroManager.HasPendingCCOn(target.Guid)) return false; } catch { }

                int energy = 0; try { energy = (int)Me.CurrentEnergy; } catch { energy = 0; }
                if (energy < 30) return false; // Blind = 30 énergie
                if (!SpellBook.CanCast(SpellBook.Blind, target))
                    return false;

                int delay = _rng.Next(40, 120);
                try { LuaHelper.SleepForLag(delay); } catch { }

                if (SpellBook.Cast(SpellBook.Blind, target))
                {
                    _lastAutoBlind = DateTime.UtcNow;
                    EventHandlers.ClearHealerTrinketWindow(target.Guid);
                    try { AudioBus.PlayEvent(); } catch { }
                    try { VitalicUi.ShowBigBanner("Auto Blind -> " + target.Name); } catch { }
                    // DR application: Disorient pour Blind dans TeamCCManager (asymétrie)
                    try { DRTracker.Applied(target.Guid, DRTracker.DrCategory.Disorient); } catch { }
                    return true;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Orchestrateur de peel (allié bas HP) — activé seulement si AutoKidney = true.
        /// Ordre Vitalic : Kidney (avec Redirect) -> Dismantle -> Gouge -> Blind.
        /// </summary>
        public static void Execute()
        {
            try
            {
                if (Me == null || !Me.IsAlive) return;
                if (EventHandlers.ShouldPauseOffense()) return;

                var S = VitalicSettings.Instance;
                if (!S.AutoKidney) return;
                int hpThresh = S.TeammateHP;
                if (hpThresh <= 0) return;

                WoWUnit ally = FindLowHpAlly(hpThresh);
                if (ally == null) return;

                WoWUnit threat = FindThreatOnAlly(ally);
                if (threat == null || !threat.IsAlive || !threat.Attackable) return;

                try { if (MacroManager.HasPendingCCOn(threat.Guid)) return; } catch { }

                int needCP = (S.KidneyShotCPs > 0 ? S.KidneyShotCPs : 4);
                int needEnergy = (S.KidneyShotEnergy > 0 ? S.KidneyShotEnergy : 25);
                int energy = 0; try { energy = Lua.GetReturnVal<int>("local e=UnitPower('player',3) or 0; return e", 0); } catch { }

                if (Me.ComboPoints >= needCP && energy >= needEnergy && TryKidney(threat, needCP, true, needEnergy))
                    return;
                if (TryDismantle(threat)) return;
                if (TryGouge(threat)) return;
                TryBlind(threat); // Blind (Disorient DR ici)
            }
            catch { }
        }

        /// <summary>
        /// Sélectionne l’allié (joueur du groupe) le plus bas en %HP sous le seuil.
        /// Miroir v.zip : on parcourt PartyMembers, pas tous les friendly units.
        /// </summary>
        private static WoWUnit FindLowHpAlly(int hpThresh)
        {
            WoWUnit best = null;
            double bestHp = 101.0;

            try
            {
                var group = Me.GroupInfo;
                if (group == null || group.PartyMembers == null)
                    return null;

                foreach (var pm in group.PartyMembers)
                {
                    if (pm == null) continue;

                    WoWPlayer u = null;
                    try { u = ObjectManager.GetObjectByGuid<WoWPlayer>(pm.Guid); } catch { }
                    if (u == null || !u.IsValid || !u.IsAlive) continue;
                    if (u.Guid == Me.Guid) continue;
                    if (u.Distance > 40) continue;

                    var hp = u.HealthPercent;
                    if (hp <= hpThresh && hp < bestHp)
                    {
                        bestHp = hp;
                        best = u;
                    }
                }
            }
            catch { }

            return best;
        }

        private static WoWUnit FindThreatOnAlly(WoWUnit ally)
        {
            WoWUnit best = null;
            try
            {
                var units = ObjectManager.GetObjectsOfType<WoWUnit>(true, true);
                for (int i = 0; i < units.Count; i++)
                {
                    var u = units[i];
                    if (u == null || !u.IsValid) continue;
                    if (!u.IsAlive || u.IsFriendly || !u.Attackable) continue;
                    bool onAlly = false; try { onAlly = (u.CurrentTarget != null && u.CurrentTarget.Guid == ally.Guid); } catch { }
                    if (onAlly && (best == null || u.Distance < best.Distance)) best = u;
                }
                if (best == null)
                {
                    var t = Me.CurrentTarget as WoWUnit;
                    if (t != null && t.IsValid && t.IsAlive)
                    {
                        try { if (t.CurrentTarget != null && t.CurrentTarget.Guid == ally.Guid) best = t; } catch { }
                    }
                }
            }
            catch { }
            return best;
        }

        // Vérifs DR (Stun / Incapacitate) — on évite seulement l’IMMUNE (miroir v.zip)
        private static bool DrOkStun(WoWUnit u) { return DRTracker.CanApplyStun(u, 0.5); }
        private static bool DrOkIncap(WoWUnit u) { return !DRTracker.IsImmune(u.Guid, DRTracker.DrCategory.Incapacitate); }

        /// <summary>
        /// Kidney Shot (408) avec Redirect (73981) si les CP ne sont pas sur la cible courante.
        /// minComboPoints : CP requis (réglage KidneyShotCPs).
        /// allowRedirect  : true pour autoriser Redirect.
        /// minEnergy      : énergie minimale (réglage KidneyShotEnergy).
        /// </summary>
        public static bool TryKidney(WoWUnit target, int minComboPoints = 4, bool allowRedirect = true, int minEnergy = 25)
        {
            if (target == null || !target.IsValid) return false;
            
            // GCD check - ne pas tenter pendant GlobalCooldown
            if (SpellManager.GlobalCooldown) 
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[TeamCC.Kidney][Gate] FAIL(gcd) - GlobalCooldown actif");
                return false;
            }
            
            if (!DrOkStun(target)) 
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                {
                    var dr = DRTracker.GetState(target, DRTracker.DrCategory.Stun);
                    Logger.Write("[TeamCC.Kidney][Gate] FAIL(dr) - DR state: {0}", dr);
                }
                return false;
            }
            
            // Vérifications mêlée et LOS (3.5y comme l'original)
            bool inMelee = SpellBook.InMeleeRange(target);
            bool hasLos = target.InLineOfSpellSight;
            if (!inMelee || !hasLos) 
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[TeamCC.Kidney][Gate] FAIL(position) - inMelee={0} los={1} dist={2:0.1}", inMelee, hasLos, target.Distance);
                return false;
            }

            // En MoP les CP sont sur la cible : si pas sur la bonne, Redirect d'abord
            bool cpsOnTarget = (Me.CurrentTargetGuid == target.Guid);
            if (!cpsOnTarget)
            {
                if (!allowRedirect || !SpellBook.CanCast(SpellBook.Redirect)) 
                {
                    if (VitalicSettings.Instance.DiagnosticMode)
                        Logger.Write("[TeamCC.Kidney][Gate] FAIL(redirect) - CP pas sur target, redirect unavailable");
                    return false;
                }
                Logger.Write("[CC] Redirect -> " + target.Name); 
                SpellBook.Cast(SpellBook.Redirect); 
                LuaHelper.SleepForLag();
            }

            // Seuil d'énergie optionnel
            int energy = 0; try { energy = Lua.GetReturnVal<int>("local e=UnitPower('player',3) or 0; return e", 0); } catch { }
            if (minEnergy > 0 && energy < minEnergy) 
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[TeamCC.Kidney][Gate] FAIL(energy) - energy={0} need={1}", energy, minEnergy);
                return false;
            }
            
            if (Me.ComboPoints < minComboPoints) 
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[TeamCC.Kidney][Gate] FAIL(cp) - cp={0} need={1}", Me.ComboPoints, minComboPoints);
                return false;
            }
            
            if (!SpellBook.CanCast(SpellBook.KidneyShot, target)) 
            {
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[TeamCC.Kidney][Gate] FAIL(cancast) - SpellBook.CanCast failed");
                return false;
            }

            // Diagnostic complet pour validation
            if (VitalicSettings.Instance.DiagnosticMode)
            {
                var dr = DRTracker.GetState(target, DRTracker.DrCategory.Stun);
                Logger.Write("[TeamCC.Kidney][Gate] SUCCESS - target={0} cp={1} energy={2} DR={3} inMelee={4} los={5}",
                    target.Name, Me.ComboPoints, energy, dr, inMelee, hasLos);
            }

            Logger.Write("[CC] Kidney -> " + target.Name + " (CP: " + Me.ComboPoints + ")");
            if (SpellBook.Cast(SpellBook.KidneyShot, target)) { DRTracker.Applied(target.Guid, DRTracker.DrCategory.Stun); return true; }
            return false;
        }
        /// <summary>
        /// Overload compat : délègue en lisant l'énergie min depuis les settings.
        /// </summary>
        public static bool TryKidney(WoWUnit target, int minComboPoints, bool allowRedirect)
        {
            int minEnergy = (VitalicSettings.Instance != null && VitalicSettings.Instance.KidneyShotEnergy > 0) ? VitalicSettings.Instance.KidneyShotEnergy : 25;
            return TryKidney(target, minComboPoints, allowRedirect, minEnergy);
        }

        /// <summary>
        /// Dismantle (51722) — peel mêlée à portée.
        /// </summary>
        public static bool TryDismantle(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsAlive) return false;
            if (DRTracker.HasHardCcWithDisarm(target)) return false;
            if (!SpellBook.InMeleeRange(target) || !target.InLineOfSpellSight) return false;
            if (!SpellBook.CanCast(SpellBook.Dismantle, target)) return false;
            Logger.Write("[CC] Dismantle -> " + target.Name); return SpellBook.Cast(SpellBook.Dismantle, target);
        }

        /// <summary>
        /// Gouge (1776) — frontal, mêlée, DR Incapacitate.
        /// </summary>
        public static bool TryGouge(WoWUnit target)
        {
            if (target == null || !target.IsValid) return false;
            if (!DrOkIncap(target)) return false;
            if (DRTracker.HasIncapOrDisorient(target)) return false;
            if (!SpellBook.InMeleeRange(target) || !target.InLineOfSpellSight) return false;

            // Forcer le facing pour les capacités frontales (miroir v.zip)
            var t = target as WoWUnit;
            if (t != null && t.IsValid && t.IsAlive)
            {
                if (!StyxWoW.Me.IsSafelyFacing(t))
                    StyxWoW.Me.SetFacing(t);
            }

            if (!SpellBook.CanCast(SpellBook.Gouge, target)) return false;
            Logger.Write("[CC] Gouge -> " + target.Name);
            if (SpellBook.Cast(SpellBook.Gouge, target)) { DRTracker.Applied(target.Guid, DRTracker.DrCategory.Incapacitate); return true; } return false;
        }

        /// <summary>
        /// Blind (2094) — portée 15y, DR Disorient dans TeamCCManager (asymétrie voulue par rapport à CrowdControlManager qui le traite avec Fear).
        /// </summary>
        public static bool TryBlind(WoWUnit target)
        {
            if (target == null || !target.IsValid) return false;
            // No-DoT: éviter de casser Blind si saignements/CT actifs
            if (HasRogueDot(target)) { try { VitalicUi.ShowMiniBanner("Blind annulé : DoT présent", 1f, 0.4f, 0.4f, 1f); } catch { } return false; }
            // Ici: Blind = Disorient (asymétrie v.zip)
            if (!DRTracker.Can(target, DRTracker.DrCategory.Disorient)) return false;
            if (DRTracker.HasHardCcNoDisarm(target)) return false;
            if (!target.InLineOfSpellSight || target.Distance > 15) return false; if (!SpellBook.CanCast(SpellBook.Blind, target)) return false;
            Logger.Write("[CC] Blind -> " + target.Name);
            if (SpellBook.Cast(SpellBook.Blind, target)) { DRTracker.Applied(target.Guid, DRTracker.DrCategory.Disorient); return true; }
            return false;
        }

        /// <summary>
        /// Sap (6770) — hors combat, humanoïde, DR Incapacitate.
        /// (fourni pour d'autres modules ; non appelé par Execute()).
        /// </summary>
        public static bool TrySap(WoWUnit target)
        {
            if (target == null || !target.IsValid) return false;
            // No-DoT: éviter de casser Sap si saignements/CT actifs
            if (HasRogueDot(target)) { try { VitalicUi.ShowMiniBanner("Sap annulé : DoT présent", 1f, 0.4f, 0.4f, 1f); } catch { } return false; }
            if (!DrOkIncap(target)) return false;
            if (DRTracker.HasHardCcNoDisarm(target)) return false;
            if (target.CreatureType != WoWCreatureType.Humanoid) return false;
            if (target.Combat) return false;
            if (!target.InLineOfSpellSight || target.Distance > 10) return false;

            // Vitalic force le facing avant Sap
            var t = target as WoWUnit;
            if (t != null && t.IsValid && t.IsAlive)
            {
                if (!StyxWoW.Me.IsSafelyFacing(t))
                    StyxWoW.Me.SetFacing(t);
            }

            if (!SpellBook.CanCast(SpellBook.Sap, target)) return false;
            Logger.Write("[CC] Sap -> " + target.Name);
            if (SpellBook.Cast(SpellBook.Sap, target)) { DRTracker.Applied(target.Guid, DRTracker.DrCategory.Incapacitate); return true; }
            return false;
        }
    }
}


