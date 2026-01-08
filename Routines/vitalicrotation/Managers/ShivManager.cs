using System;
using System.Collections.Generic;
using Styx;
using Styx.CommonBot; // SpellManager
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    internal static class ShivManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        // Known MoP enrage-like auras (fallback if Lua dispel check unavailable)
        private static readonly HashSet<int> _enrageAuraIds = new HashSet<int>
        {
            12880,  // Enrage (Warrior generic proc)
            18499,  // Berserker Rage (Warrior) – treated as Enrage in original
            12292,  // Bloodbath
            13750,  // Adrenaline Rush (Rogue)
            107574, // Avatar (Warrior)
            1719,   // Recklessness (Warrior)
            19574,  // Bestial Wrath (Hunter)
            34471,  // The Beast Within (BW effect – ensures fallback if flag missed)
            106951, // Berserk (Feral)
            51271   // Pillar of Frost (DK) – high offensive, sometimes flagged enrage-like
        };

        // Basic snares to detect if target already slowed enough (avoid redundant Shiv)
        private static readonly HashSet<int> _snareAuraIds = new HashSet<int>
        {
            3409,   // Crippling Poison (debuff)
            45524,  // Chains of Ice
            1715,   // Hamstring
            12323,  // Piercing Howl
            8056,   // Frost Shock
            116,    // Frostbolt
            31589,  // Slow
            5116,   // Concussive Shot
        };

        private const int ShivMaxRange = 6;

        public static Composite Build()
        {
            return new Action(ret => { Execute(); return RunStatus.Failure; });
        }

        public static void Execute()
        {
            try
            {
                var S = VitalicSettings.Instance;
                if (Me == null || !Me.IsAlive) return;
                if (EventHandlers.ShouldPauseOffense()) return;
                if (!S.AutoShiv) return; // only enrage dispel retained
                if (SpellManager.GlobalCooldown) return;

                var t = Me.CurrentTarget as WoWUnit;
                if (t == null || !t.IsValid || !t.IsAlive || !t.Attackable) return;
                if (!t.InLineOfSpellSight) return;

                try { if (t.Distance > ShivMaxRange) return; } catch { }

                // Avoid if immune to slows (for the snare branch)
                bool slowImmune = false; try { slowImmune = ImmunityGuard.IsSlowImmune(t); } catch { slowImmune = false; }

                if (CanShivNow(t) && HasEnrage(t))
                {
                    if (!Throttle.Check("Shiv.Enrage", 1500)) return;
                    if (SpellBook.Cast(SpellBook.Shiv, t))
                    {
                        if (VitalicSettings.Instance.DiagnosticMode) Logger.Write("[Diag][Shiv] Enrage dispel -> " + t.Name);
                        Logger.Write("[Utility] Shiv (enrage) -> " + t.Name);
                        Throttle.Mark("Shiv.Enrage");
                        return;
                    }
                }

                // Optional chase snare removed (settings deleted). Keep minimal path: apply Crippling if no snare and not immune.
                if (!slowImmune && CanShivNow(t) && !HasSnare(t))
                {
                    if (!Throttle.Check("Shiv.Crippling", 1500)) return;
                    if (SpellBook.Cast(SpellBook.Shiv, t))
                    {
                        Logger.Write("[Utility] Shiv (snare) -> " + t.Name);
                        Throttle.Mark("Shiv.Crippling");
                        return;
                    }
                }
            }
            catch { }
        }

        private static bool CanShivNow(WoWUnit t)
        {
            try
            {
                if (t == null || !t.IsValid || !t.IsAlive) return false;
                if (!SpellBook.CanCast(SpellBook.Shiv, t)) return false;
                return true;
            }
            catch { return false; }
        }

        private static bool HasEnrage(WoWUnit t)
        {
            if (t == null || !t.IsValid) return false;
            try
            {
                // Prefer Lua dispel-type check (covers all auras flagged Enrage)
                bool enr = Lua.GetReturnVal<bool>(
                    "for i=1,40 do local _,_,_,_,dtype,_,_,_,_,id=UnitAura('target',i,'HELPFUL'); if not id then break end if dtype=='Enrage' then return true end end return false", 0);
                if (enr) return true;
            }
            catch { }

            // Fallback: scan auras by ID
            try
            {
                var auras = t.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i];
                    if (a == null || !a.IsActive) continue;
                    int sid = 0; try { sid = a.SpellId; } catch { }
                    if (_enrageAuraIds.Contains(sid)) return true;
                }
            }
            catch { }
            return false;
        }

        private static bool HasSnare(WoWUnit t)
        {
            if (t == null || !t.IsValid) return false;
            try
            {
                // Fast check by ID
                var auras = t.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i]; if (a == null || !a.IsActive) continue;
                    int sid = 0; try { sid = a.SpellId; } catch { sid = 0; }
                    if (_snareAuraIds.Contains(sid)) return true;
                }
            }
            catch { }

            // Fallback by name (Crippling Poison only)
            try { if (t.HasAura("Crippling Poison")) return true; } catch { }
            return false;
        }
    }
}
