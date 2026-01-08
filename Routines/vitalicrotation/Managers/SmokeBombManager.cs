using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Reflection;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    internal static class SmokeBombManager
    {
        private const int MicroThrottleMs = 200;
        private const string ThrottleKey   = "SmokeBomb.Cast";
        private static DateTime _lastBomb = DateTime.MinValue;
        private const int SmokeInternalCooldownMs = 13000; // 13s

        public static Composite Build()
        {
            return new PrioritySelector(
                new Action(delegate { return TryAutoSmokeBomb() ? RunStatus.Success : RunStatus.Failure; }),
                new Action(delegate { return TryCastedSmokeBomb() ? RunStatus.Success : RunStatus.Failure; })
            );
        }

        public static void Execute()
        {
            if (TryAutoSmokeBomb()) return;
            TryCastedSmokeBomb();
        }

        private static bool TryCastedSmokeBomb()
        {
            try
            {
                WoWUnit queuedTarget;
                if (!MacroManager.TryDequeue("Smoke Bomb", out queuedTarget) && !ConsumeHotkey("Smoke Bomb"))
                    return false;

                var me = StyxWoW.Me;
                var S = VitalicSettings.Instance;
                var target = me != null ? me.CurrentTarget as WoWUnit : null;

                if (!Throttle.Check(ThrottleKey, MicroThrottleMs)) return false;
                if (!SpellBook.CanCast(SpellBook.SmokeBomb)) return false;

                if (SpellBook.Cast(SpellBook.SmokeBomb))
                {
                    Logger.Write("[Bomb] Cast (hotkey/macro)");
                    Throttle.Mark(ThrottleKey);
                    _lastBomb = DateTime.UtcNow;
                    return true;
                }
            }
            catch { }
            return false;
        }

        private static bool TryAutoSmokeBomb()
        {
            try
            {
                var S = VitalicSettings.Instance;
                if (!S.AutoSmokeBomb) return false;
                var me = StyxWoW.Me; if (me == null || !me.IsAlive) return false;
                var t = me.CurrentTarget as WoWUnit; if (t == null || !t.IsAlive || !t.Attackable) return false;

                if ((DateTime.UtcNow - _lastBomb).TotalMilliseconds < SmokeInternalCooldownMs) return false;
                if (!SpellBook.CanCast(SpellBook.SmokeBomb)) return false;

                // Simple heuristique : cible low HP ou casting dangereux + n'est pas immunisée (basic check HasImmuneAuras)
                bool lowHp = false; try { lowHp = t.HealthPercent <= 35; } catch { }
                bool dangerousCast = false; try { dangerousCast = t.IsCasting && t.CanInterruptCurrentSpellCast; } catch { }
                if (!lowHp && !dangerousCast) return false;
                if (HasImmuneAuras(t)) return false;

                if (SpellBook.Cast(SpellBook.SmokeBomb))
                {
                    Logger.Write("[Bomb] Auto -> Smoke Bomb");
                    _lastBomb = DateTime.UtcNow;
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static bool TrySafeBomb(WoWUnit target)
        {
            if (target == null || !target.IsAlive || !target.Attackable) return false;
            if ((DateTime.UtcNow - _lastBomb).TotalMilliseconds < SmokeInternalCooldownMs) return false;
            if (HasImmuneAuras(target)) return false;
            if (!SpellBook.CanCast(SpellBook.SmokeBomb)) return false;
            if (SpellBook.Cast(SpellBook.SmokeBomb))
            {
                Logger.Write("[Bomb] Manual safe cast -> Smoke Bomb");
                _lastBomb = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        private static WoWUnit EnemyHealerNear(WoWUnit center, float radius)
        {
            try
            {
                if (center == null) return null;
                var list = Styx.WoWInternals.ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
                for (int i = 0; i < list.Count; i++)
                {
                    var u = list[i]; if (u == null || !u.IsValid || !u.IsAlive) continue;
                    if (!u.Attackable || u.IsFriendly) continue;
                    if (!IsHealerClass(u.Class)) continue;
                    double d = 9999; try { d = u.Location.Distance(center.Location); } catch { }
                    if (d <= radius) return u;
                }
            }
            catch { }
            return null;
        }

        private static bool HasImmuneAuras(WoWUnit u)
        {
            if (u == null) return false;
            try
            {
                var auras = u.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i]; if (a == null || !a.IsActive) continue;
                    int id = 0; try { id = a.SpellId; } catch { }
                    // Common immunities / untargetables (simplified set)
                    if (id == 45438 || id == 642 || id == 1022 || id == 110700) return true; // Ice Block / Divine Shield / HoP / Divine Protection variant
                }
            }
            catch { }
            return false;
        }

        private static bool IsEnemyHealerCasting()
        {
            try
            {
                var me = StyxWoW.Me; if (me == null) return false;
                var list = Styx.WoWInternals.ObjectManager.GetObjectsOfType<WoWUnit>(false, false);
                for (int i = 0; i < list.Count; i++)
                {
                    var u = list[i]; if (u == null || !u.IsValid || !u.IsAlive) continue;
                    if (!u.Attackable || u.IsFriendly) continue;
                    if (!IsHealerClass(u.Class)) continue;
                    if (u.IsCasting && u.CanInterruptCurrentSpellCast) return true;
                }
            }
            catch { }
            return false;
        }

        private static bool IsHealerClass(Styx.WoWClass wowClass)
        {
            return wowClass == Styx.WoWClass.Priest || wowClass == Styx.WoWClass.Druid || wowClass == Styx.WoWClass.Paladin || wowClass == Styx.WoWClass.Shaman || wowClass == Styx.WoWClass.Monk;
        }

        private static bool ConsumeHotkey(string actionName)
        {
            try
            {
                if (InputPoller.IsHotkeyActive(VitalicSettings.Instance.SmokeBombKeyBind))
                {
                    return true;
                }
            }
            catch { }
            return false;
        }
    }
}


