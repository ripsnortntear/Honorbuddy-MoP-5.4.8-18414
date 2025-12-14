using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using VitalicRotation.Helpers;
using VitalicRotation.Settings;
using VitalicRotation.UI;
using Action = Styx.TreeSharp.Action;

namespace VitalicRotation.Managers
{
    internal static class CooldownManager
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        // Basic list of full immunities (original style simple aura check)
        private static readonly int[] BadTargetAuraIds = new[]
        {
            642, 45438, 19263, 47585, 1022, 110700, 110715, 110696, 122465, 148467, 110617
        };

        public static event Action<object, string> OnMajorCooldownUsed;
        private static void RaiseMajor(string spellName)
        {
            try { var h = OnMajorCooldownUsed; if (h != null) h(typeof(CooldownManager), spellName); } catch { }
        }

        // === Public mirror API stubs kept for callers (return neutral values) ===
        public static void NotifyHoldClearedSoon() { }
        public static bool InPostHoldWindow() { return false; }
        public static double PreBurstRemaining() { return 0.0; }
        public static bool IsPreBurstPooling() { return false; }

        public static Composite Build()
        {
            return new PrioritySelector(
                new Decorator(ret => Me != null && Me.IsAlive && Me.GotTarget,
                    new Action(ret => ExecuteInternal() ? RunStatus.Success : RunStatus.Failure))
            );
        }

        public static void Execute() { ExecuteInternal(); }

        private static bool ExecuteInternal()
        {
            if (Me == null || !Me.IsAlive || !Me.GotTarget) return false;
            if (ToggleState.PauseDamage || ToggleState.Lazy) return false;

            var target = Me.CurrentTarget as WoWUnit;
            if (target == null || !target.IsAlive || !target.Attackable) return false;
            if (HasAnyAuraId(target, BadTargetAuraIds)) return false;
            if (!ShouldBurstNow(target)) return false;

            int spec = GetRogueSpecIndex();
            bool cast = false;
            try
            {
                switch (spec)
                {
                    case 1: // Assassination: Vendetta -> Shadow Blades -> Trinkets
                        if (CastVendetta(target)) cast = true;
                        if (CanUseShadowBlades(target) && CastShadowBlades(target)) cast = true;
                        if (TryUseOnUseItems()) cast = true;
                        break;
                    case 2: // Combat: Killing Spree -> Adrenaline Rush -> Shadow Blades -> Trinkets
                        if (CastKillingSpree(target)) cast = true;
                        if (CastAdrenalineRush(target)) cast = true;
                        if (CanUseShadowBlades(target) && CastShadowBlades(target)) cast = true;
                        if (TryUseOnUseItems()) cast = true;
                        break;
                    case 3: // Subtlety: Shadow Dance -> Shadow Blades -> Trinkets
                        if (CastShadowDance(target)) cast = true;
                        if (CanUseShadowBlades(target) && CastShadowBlades(target)) cast = true;
                        if (TryUseOnUseItems()) cast = true;
                        break;
                    default: // Fallback generic order
                        if (CanUseShadowBlades(target) && CastShadowBlades(target)) cast = true;
                        if (TryUseOnUseItems()) cast = true;
                        break;
                }
            }
            catch (Exception ex) { Logger.Write("[Cooldown] Exception: " + ex.Message); }
            return cast;
        }

        // === Basic gating ===
        private static bool ShouldBurstNow(WoWUnit target)
        {
            if (!ToggleState.Burst) return false;
            if (SpellManager.GlobalCooldown) return false;
            if (target == null || !target.IsAlive || !target.Attackable || !target.InLineOfSpellSight) return false;

            // Energy threshold (simple) – opener vs combat
            var S = VitalicSettings.Instance;
            int energy = 0; try { energy = (int)Me.CurrentEnergy; } catch { }
            if (Me.Combat)
            {
                if (energy < S.BurstEnergy) return false;
            }
            else
            {
                if (energy < S.BurstEnergyOpener) return false;
            }

            // DR Stun gating (original simple threshold on factor)
            try
            {
                var state = DRTracker.GetState(target, DRCategory.Stun);
                double factor = 1.0;
                if (state == DRState.Half) factor = 0.5;
                else if (state == DRState.Quarter) factor = 0.25;
                else if (state == DRState.Immune) factor = 0.0;
                if (factor < VitalicSettings.Instance.BurstStunDR) return false;
            }
            catch { }

            return true;
        }

        private static bool CanUseShadowBlades(WoWUnit target)
        {
            if (ToggleState.NoShadowBlades) return false; // hotkey toggle
            if (!SpellManager.HasSpell(SpellBook.ShadowBlades)) return false;
            if (!SpellManager.CanCast(SpellBook.ShadowBlades, target)) return false;
            if (target != null && HasAnyAuraId(target, BadTargetAuraIds)) return false;
            if (SpellManager.GlobalCooldown) return false;
            return true;
        }

        // === Casting wrappers (simple) ===
        private static bool CastVendetta(WoWUnit target)
        {
            if (target == null) return false;
            if (!SpellManager.CanCast(SpellBook.Vendetta, target)) return false;
            if (SpellManager.Cast(SpellBook.Vendetta, target)) { Logger.Write("[Cooldown] Vendetta -> " + target.SafeName); RaiseMajor("Vendetta"); return true; }
            return false;
        }
        private static bool CastShadowBlades(WoWUnit target)
        {
            if (target == null || !CanUseShadowBlades(target)) return false;
            if (SpellManager.Cast(SpellBook.ShadowBlades, target)) { Logger.Write("[Cooldown] Shadow Blades"); try { VitalicUi.ShowBigBanner("Shadow Blades !"); } catch { } RaiseMajor("Shadow Blades"); return true; }
            return false;
        }
        private static bool CastAdrenalineRush(WoWUnit target)
        {
            if (target == null) return false;
            if (!SpellManager.CanCast(SpellBook.AdrenalineRush, target)) return false;
            if (SpellManager.Cast(SpellBook.AdrenalineRush, target)) { Logger.Write("[Cooldown] Adrenaline Rush"); RaiseMajor("Adrenaline Rush"); return true; }
            return false;
        }
        private static bool CastKillingSpree(WoWUnit target)
        {
            if (target == null) return false;
            if (!SpellManager.CanCast(SpellBook.KillingSpree, target)) return false;
            if (SpellManager.Cast(SpellBook.KillingSpree, target)) { Logger.Write("[Cooldown] Killing Spree -> " + target.SafeName); RaiseMajor("Killing Spree"); return true; }
            return false;
        }
        private static bool CastShadowDance(WoWUnit target)
        {
            if (target == null) return false;
            if (!SpellManager.CanCast(SpellBook.ShadowDance, target)) return false;
            if (SpellManager.Cast(SpellBook.ShadowDance, target)) { Logger.Write("[Cooldown] Shadow Dance"); RaiseMajor("Shadow Dance"); return true; }
            return false;
        }

        private static bool TryUseOnUseItems()
        {
            bool used = false;
            try { if (TryUseTrinket(13)) used = true; } catch { }
            try { if (TryUseTrinket(14)) used = true; } catch { }
            try { if (TryUseGloves()) used = true; } catch { }
            return used;
        }
        private static bool TryUseTrinket(int slot)
        {
            try
            {
                string throttleKey = "Trinket" + slot;
                if (!Throttle.Check(throttleKey, 1500)) return false;
                string lua = "local slot=" + slot + " local start,d=GetInventoryItemCooldown('player',slot) local usable=IsUsableItem(slot) return (start==0 or (GetTime()-start)>=d) and usable";
                bool canUse = Lua.GetReturnVal<bool>(lua, 0);
                if (!canUse) return false;
                Lua.DoString("UseInventoryItem(" + slot + ")");
                Throttle.Mark(throttleKey);
                Logger.Write("[Cooldown] Used trinket slot " + slot);
                return true;
            }
            catch { return false; }
        }
        private static bool TryUseGloves()
        {
            try
            {
                if (!Throttle.Check("Gloves", 1500)) return false;
                const int slot = 10;
                string lua = "local slot=10 local start,d=GetInventoryItemCooldown('player',slot) local usable=IsUsableItem(slot) return (start==0 or (GetTime()-start)>=d) and usable";
                bool canUse = Lua.GetReturnVal<bool>(lua, 0);
                if (!canUse) return false;
                Lua.DoString("UseInventoryItem(10)");
                Throttle.Mark("Gloves");
                Logger.Write("[Cooldown] Used gloves");
                return true;
            }
            catch { return false; }
        }

        private static bool HasAnyAuraId(WoWUnit unit, int[] ids)
        {
            if (unit == null || !unit.IsValid) return false;
            try
            {
                var auras = unit.GetAllAuras();
                for (int i = 0; i < auras.Count; i++)
                {
                    var a = auras[i]; if (a == null) continue; int sid = a.SpellId;
                    for (int j = 0; j < ids.Length; j++) if (ids[j] == sid) return true;
                }
            }
            catch { }
            return false;
        }

        private static int GetRogueSpecIndex()
        {
            try { return Lua.GetReturnVal<int>("local i=GetSpecialization(); if not i then return 0 end; return i", 0); } catch { return 0; }
        }

        static CooldownManager()
        {
            OnMajorCooldownUsed += (s, spellName) =>
            {
                try
                {
                    if (VitalicSettings.Instance.SoundAlertsEnabled)
                        AudioBus.PlayEvent();
                    VitalicUi.ShowBigBanner(string.Format("{0} !", spellName));
                }
                catch { }
            };
        }
    }
}
