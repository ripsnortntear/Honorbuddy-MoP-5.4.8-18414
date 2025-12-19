using System;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Settings;

namespace VitalicRotation.Managers
{
    public static class FocusManager
    {
        // Rafraîchissement ~1s, verrou après changement pour éviter le churn
        private static DateTime _lastScan = DateTime.MinValue;
        private static DateTime _lockUntil = DateTime.MinValue;
        private const int ScanIntervalMs = 950;
    private const int LockAfterChangeMs = 3000;

        // Identifiant logique (arena1, arena2, target, mouseover, focus)
        private static string _currentFocusUnitId = "focus";
        private static ulong _cachedFocusGuid = 0;

        // SpecIDs heal MoP (Pal Holy 65, Dru Resto 105, Cham Resto 264, Prêtre Disc 256, Holy 257, Moine MW 270)
        private static readonly int[] HealerSpecIds = { 65, 105, 264, 256, 257, 270 };

        public static string CurrentFocusUnitId { get { return _currentFocusUnitId; } }

        #region Public API
        public static void Reset()
        {
            _currentFocusUnitId = "focus";
            _cachedFocusGuid = 0;
            _lastScan = DateTime.MinValue;
            _lockUntil = DateTime.MinValue;
        }

        public static void ValidateOrClear()
        {
            if (!IsValidFocusGuid(_cachedFocusGuid))
            {
                _cachedFocusGuid = 0;
                _currentFocusUnitId = "focus";
                InterruptManager.OnFocusChanged(_currentFocusUnitId);
                TeamCCManager.OnFocusChanged(_currentFocusUnitId);
            }
        }

        public static void AutoAssignHealerFocusIfNone()
        {
            if (_cachedFocusGuid != 0 && IsValidFocusGuid(_cachedFocusGuid)) return;
            var healer = FindEnemyHealer();
            if (healer == null) return;
            _cachedFocusGuid = healer.Guid;
            string unitId = FindArenaHealerUnitId();
            if (unitId != null) { ApplyLogicalFocus(unitId); }
        }

        public static Styx.TreeSharp.Composite Build()
        {
            return new Styx.TreeSharp.Action(ret => { Execute(); return Styx.TreeSharp.RunStatus.Failure; });
        }

        /// <summary>
        /// Phase 1.2 - FocusManager Avancé selon Class63.smethod_0() original
        /// Logique de recherche automatique des cibles focus (healer priority)
        /// </summary>
        public static void Execute()
        {
            if ((DateTime.UtcNow - _lastScan).TotalMilliseconds < ScanIntervalMs) return;
            _lastScan = DateTime.UtcNow;

            ValidateCurrentFocus();
            if (DateTime.UtcNow < _lockUntil) return;

            try { AdvancedAutoFocus(); } catch { }
        }

        /// <summary>
        /// Implémentation complète de l'auto-focus avancé (P1.2)
        /// Recherche automatique healer/dps selon les critères configurés
        /// </summary>
        private static void AdvancedAutoFocus()
        {
            var s = VitalicSettings.Instance;
            int mode = 0; // 0 Always, 1 Arenas, 2 BGs, 3 Never
            int targetsMode = 0; // 0 Healers + DPS, 1 Healers only
            try { mode = s.AutoFocus; } catch { }
            try { targetsMode = s.AutoFocusTargets; } catch { }

            if (mode == 3) return; // Never

            bool inArena = IsArena();
            bool inBg = IsBattleground();
            if (mode == 1 && !inArena) return; // Arenas only
            if (mode == 2 && !inBg) return;    // Battlegrounds only

            // Si on a déjà un focus logique valide, on le garde
            if (HasValidLogicalFocus()) return;

            // Phase 1.2 - Priorité 1: Healer ennemi en arène (spec detection)
            WoWUnit arenaHealer = FindArenaHealerUnit();
            if (arenaHealer != null)
            {
                string arenaUnitId = UnitIdForArenaIndex(arenaHealer);
                if (!string.IsNullOrEmpty(arenaUnitId))
                {
                    ApplyLogicalFocus(arenaUnitId);
                    if (VitalicSettings.Instance.DiagnosticMode)
                        Logging.Write("[Focus] Arena healer focused: {0} ({1})", arenaUnitId, arenaHealer.SafeName);
                    return;
                }
            }

            // Phase 1.2 - Priorité 2: Healer général en BG/world
            WoWUnit generalHealer = FindBestHealerTarget();
            if (generalHealer != null)
            {
                string unitId = UnitIdForGuid(generalHealer.Guid) ?? "target";
                ApplyLogicalFocus(unitId);
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logging.Write("[Focus] General healer focused: {0}", generalHealer.SafeName);
                return;
            }

            // Si mode "Healers Only", on s'arrête ici
            if (targetsMode == 1) return;

            // Phase 1.2 - Priorité 3: DPS le plus bas HP dans la portée
            WoWPlayer bestDps = FindBestDpsTarget();
            if (bestDps != null)
            {
                string unitId = UnitIdForGuid(bestDps.Guid) ?? "target";
                ApplyLogicalFocus(unitId);
                if (VitalicSettings.Instance.DiagnosticMode)
                    Logging.Write("[Focus] DPS focused: {0}", bestDps.SafeName);
                return;
            }
        }

        /// <summary>
        /// Phase 1.2 - Recherche du meilleur healer target général (BG/World)
        /// P3.1 - Optimisé avec foreach manual au lieu de LINQ
        /// </summary>
        private static WoWUnit FindBestHealerTarget()
        {
            try
            {
                WoWUnit bestHealer = null;
                double lowestHp = double.MaxValue;

                // P3.1 - Performance: foreach manual au lieu de LINQ
                var units = ObjectManager.GetObjectsOfType<WoWPlayer>(false, false);
                foreach (var player in units)
                {
                    if (player == null || !player.IsValid || !player.IsAlive) continue;
                    if (!player.IsHostile) continue;
                    if (player.Distance > 40) continue;
                    if (!player.InLineOfSight) continue;

                    // Vérifier si c'est un healer potentiel
                    if (!IsLikelyHealer(player)) continue;

                    // Prendre le healer avec le HP le plus bas (priorité kill)
                    if (player.HealthPercent < lowestHp)
                    {
                        lowestHp = player.HealthPercent;
                        bestHealer = player;
                    }
                }

                return bestHealer;
            }
            catch { return null; }
        }

        /// <summary>
        /// Phase 1.2 - Recherche du meilleur DPS target
        /// P3.1 - Optimisé avec foreach manual au lieu de LINQ
        /// </summary>
        private static WoWPlayer FindBestDpsTarget()
        {
            try
            {
                WoWPlayer bestDps = null;
                double lowestHp = double.MaxValue;

                // P3.1 - Performance: foreach manual au lieu de LINQ
                var units = ObjectManager.GetObjectsOfType<WoWPlayer>(false, false);
                foreach (var player in units)
                {
                    if (player == null || !player.IsValid || !player.IsAlive) continue;
                    if (!player.IsHostile) continue;
                    if (player.Distance > 40) continue;
                    if (!player.InLineOfSight) continue;

                    // Exclure les healers (déjà traités)
                    if (IsLikelyHealer(player)) continue;

                    // Prendre le DPS avec le HP le plus bas
                    if (player.HealthPercent < lowestHp)
                    {
                        lowestHp = player.HealthPercent;
                        bestDps = player;
                    }
                }

                return bestDps;
            }
            catch { return null; }
        }

        private static bool IsArena()
        {
            try { string t = Lua.GetReturnVal<string>("local a,b=IsInInstance(); return tostring(b or '')",0); return string.Equals(t, "arena", StringComparison.OrdinalIgnoreCase); } catch { } return false;
        }
        private static bool IsBattleground()
        {
            try { string t = Lua.GetReturnVal<string>("local a,b=IsInInstance(); return tostring(b or '')",0); return string.Equals(t, "pvp", StringComparison.OrdinalIgnoreCase); } catch { } return false;
        }

        /// <summary>
        /// Phase 1.2 - Recherche healer en arène avec détection de spé avancée
        /// </summary>
        private static WoWUnit FindArenaHealerUnit()
        {
            for (int i = 1; i <= 3; i++)
            {
                string uid = "arena" + i;
                if (!UnitCanBeFocus(uid)) continue;
                
                // Récupération de la spé via API arène
                int specId = 0; 
                try { specId = Lua.GetReturnVal<int>("local id=GetArenaOpponentSpec(" + i + "); return id or 0",0); } catch { specId = 0; }
                
                if (specId != 0 && IsHealerSpec(specId))
                {
                    try
                    {
                        var guidStr = Lua.GetReturnVal<string>("if UnitExists('"+uid+"') then return UnitGUID('"+uid+"') else return '' end",0);
                        if (!string.IsNullOrEmpty(guidStr)) 
                        {
                            var guid = ulong.Parse(guidStr, System.Globalization.NumberStyles.HexNumber);
                            return ObjectManager.GetObjectByGuid<WoWUnit>(guid);
                        }
                    }
                    catch { }
                }
            }
            return null;
        }

        private static string UnitIdForArenaIndex(WoWUnit arenaUnit)
        {
            if (arenaUnit == null) return null;
            try
            {
                for (int i = 1; i <= 3; i++)
                {
                    string uid = "arena" + i;
                    bool match = Lua.GetReturnVal<bool>("return UnitExists('"+uid+"') and UnitGUID('"+uid+"')=='" + arenaUnit.Guid + "'",0);
                    if (match) return uid;
                }
            }
            catch { }
            return null;
        }
        #endregion

        #region Core Helpers
        private static bool IsValidFocusGuid(ulong guid)
        {
            if (guid == 0) return false;
            var unit = ObjectManager.GetObjectByGuid<WoWUnit>(guid);
            return unit != null && unit.IsValid && unit.IsAlive && unit.Attackable && !unit.IsFriendly;
        }

        private static void ValidateCurrentFocus()
        {
            if (!UnitCanBeFocus(_currentFocusUnitId))
            {
                _currentFocusUnitId = "focus";
                InterruptManager.OnFocusChanged(_currentFocusUnitId);
                TeamCCManager.OnFocusChanged(_currentFocusUnitId);
            }
        }

        private static bool NeedsFocusChange(string newUnitId)
        {
            return !string.Equals(_currentFocusUnitId, newUnitId, StringComparison.OrdinalIgnoreCase);
        }

        private static void ApplyLogicalFocus(string unitId)
        {
            _currentFocusUnitId = unitId;
            InterruptManager.OnFocusChanged(unitId);
            TeamCCManager.OnFocusChanged(unitId);
            _lockUntil = DateTime.UtcNow.AddMilliseconds(LockAfterChangeMs);
        }

        private static bool HasValidLogicalFocus()
        {
            if (string.IsNullOrEmpty(_currentFocusUnitId)) return false;
            if (_currentFocusUnitId == "focus") return false; // placeholder
            return UnitCanBeFocus(_currentFocusUnitId);
        }
        #endregion

        #region Detection
        private static WoWUnit FindEnemyHealer()
        {
            var units = ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
            for (int i = 0; i < units.Count; i++)
            {
                var u = units[i];
                if (u == null || !u.IsValid || !u.IsAlive) continue;
                if (!u.Attackable || u.IsFriendly) continue;
                if (IsLikelyHealer(u)) return u;
            }
            return null;
        }

        private static bool IsLikelyHealer(WoWUnit unit)
        {
            if (unit == null) return false;
            var cls = unit.Class;
            return cls == WoWClass.Priest || cls == WoWClass.Druid || cls == WoWClass.Paladin || cls == WoWClass.Shaman || cls == WoWClass.Monk;
        }

        private static string FindArenaHealerUnitId()
        {
            for (int i = 1; i <= 3; i++)
            {
                string uid = "arena" + i;
                if (!UnitCanBeFocus(uid)) continue;
                int specId = 0;
                try { specId = Lua.GetReturnVal<int>("local id=GetArenaOpponentSpec(" + i + "); return id or 0", 0); } catch { specId = 0; }
                if (specId != 0 && IsHealerSpec(specId)) return uid;
            }
            return null;
        }

        private static string FindAnyArenaEnemyUnitId()
        {
            for (int i = 1; i <= 3; i++)
            {
                string uid = "arena" + i;
                if (UnitCanBeFocus(uid)) return uid;
            }
            return null;
        }

        private static bool IsHealerSpec(int specId)
        {
            for (int i = 0; i < HealerSpecIds.Length; i++) if (HealerSpecIds[i] == specId) return true; return false;
        }
        #endregion

        #region Auto Focus Healer
        private static void AutoFocusHealerInArena()
        {
            try
            {
                if (!IsArenaOrBattleground()) return;
                if (_cachedFocusGuid != 0 && IsValidFocusGuid(_cachedFocusGuid)) return;

                string arenaHealer = FindArenaHealerUnitId();
                if (arenaHealer != null)
                {
                    ApplyLogicalFocus(arenaHealer);
                    return;
                }

                var units = ObjectManager.GetObjectsOfType<WoWPlayer>(false, false);
                WoWPlayer best = null;
                for (int i = 0; i < units.Count; i++)
                {
                    var p = units[i];
                    if (p == null || !p.IsValid || !p.IsAlive) continue;
                    if (!p.IsHostile) continue;
                    if (!IsLikelyHealer(p)) continue;
                    if (p.Distance > 40) continue;
                    if (!p.InLineOfSight) continue;
                    if (best == null || p.HealthPercent < best.HealthPercent) best = p;
                }
                if (best != null)
                {
                    string unitId = UnitIdForGuid(best.Guid) ?? "target";
                    ApplyLogicalFocus(unitId);
                }
            }
            catch { }
        }

        private static bool IsArenaOrBattleground()
        {
            try
            {
                string instType = Lua.GetReturnVal<string>("local a,t=IsInInstance(); return tostring(t or '')", 0);
                if (string.Equals(instType, "arena", StringComparison.OrdinalIgnoreCase)) return true;
                if (string.Equals(instType, "pvp", StringComparison.OrdinalIgnoreCase)) return true; // battleground
            }
            catch { }
            return false;
        }

        private static string UnitIdForGuid(ulong guid)
        {
            for (int i = 1; i <= 3; i++)
            {
                string uid = "arena" + i;
                try
                {
                    bool match = Lua.GetReturnVal<bool>("return UnitExists('" + uid + "') and UnitGUID('" + uid + "')=='" + guid + "'", 0);
                    if (match) return uid;
                }
                catch { }
            }
            return null;
        }
        #endregion

        #region Lua Helper
        private static bool UnitCanBeFocus(string unitId)
        {
            if (string.IsNullOrEmpty(unitId)) return false;
            string lua = "local u='" + unitId + "' if not UnitExists(u) then return false end if UnitIsDead(u) then return false end if not UnitCanAttack('player',u) then return false end if not UnitIsVisible(u) then return false end return true";
            bool ok = false;
            try { ok = Lua.GetReturnVal<bool>(lua, 0); } catch { ok = false; }
            return ok;
        }
        #endregion
    }
}



