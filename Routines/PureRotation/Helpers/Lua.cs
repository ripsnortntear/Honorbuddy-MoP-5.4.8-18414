using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureRotation.Core;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace PureRotation.Helpers
{
    internal static class Lua
    {
        internal static SecondaryStats _secondaryStats;          //create within frame (does series of LUA calls)

        //-- Put all Lua Calls in here..For other Lua Calls that need to be used elsewhere put Styx.WoWInternals in front of it -- wulf

        internal static Composite StartAutoAttack
        {
            get
            {
                return new Action(ret =>
                {
                    if (!StyxWoW.Me.IsAutoAttacking)
                        Styx.WoWInternals.Lua.DoString("StartAttack()");
                    return RunStatus.Failure;
                });
            }
        }

        public static double GetSpellCooldown(string spell)
        {
            try
            {
                SpellFindResults results;
                if (SpellManager.FindSpell(spell, out results))
                {
                    var conv = results.Override != null ? results.Override.Name : results.Original.Name;
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Styx.WoWInternals.Lua.GetReturnVal<int>("return GetSpellCooldown(\"" + conv + "\");", 1);
                    }
                }

                return 0;
            }
            catch
            {
                Logger.FailLog(" Lua Failed in GetSpellCooldown"); 
                return 0;
            }
        } // may not even need to check for the existance of the spell ? --wulf

        public static double GetSpellCooldown(int spell)
        {
            try
            {
                SpellFindResults results;
                if (SpellManager.FindSpell(spell, out results))
                {
                    var conv = results.Override != null ? results.Override.Name : results.Original.Name;
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Styx.WoWInternals.Lua.GetReturnVal<int>("return GetSpellCooldown(\"" + conv + "\");", 1);
                    }
                }

                return 0;
            }
            catch
            {
                Logger.FailLog(" Lua Failed in GetSpellCooldown"); 
                return 0;
            }
        } // may not even need to check for the existance of the spell ? --wulf

        public static double GetRuneCooldown(int runeslot)
        {
            try
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    var lua = String.Format("local x=select(1, GetRuneCooldown({0})); if x==nil then return 0 else return x-GetTime() end", runeslot);
                    var t = Double.Parse(Styx.WoWInternals.Lua.GetReturnValues(lua)[0]);
                    return Math.Abs(t);
                }
            }
            catch
            {
                Logger.FailLog(" Lua Failed in GetRuneCooldown"); 
                return 0;
            }
        }

        #region Player and Target Debuffs and Buffs (Damn you alex for making me put these in :P -- wulf)

        public static double PlayerBuffTimeLeft(string name)
        {
            name = LocalizeSpellName(name);
            try
            {
                var lua = String.Format("local x=select(7, UnitBuff('player', \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x-GetTime() end", RealLuaEscape(name));
                var t = Double.Parse(Styx.WoWInternals.Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                Logger.FailLog("Lua failed in PlayerBuffTimeLeft");
                return 999999;
            }
        }

        public static int PlayerCountBuff(string name)
        {
            name = LocalizeSpellName(name);
            try
            {
                var lua = string.Format("local x=select(4, UnitBuff('player', \"{0}\")); if x==nil then return 0 else return x end", RealLuaEscape(name));
                var t = int.Parse(Styx.WoWInternals.Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                Logger.FailLog("Lua failed in PlayerCountBuff");
                return 0;
            }
        }

        public static double TargetDebuffTimeLeft(string name)
        {
            name = LocalizeSpellName(name);
            try
            {
                var lua = string.Format("local x=select(7, UnitDebuff(\"target\", \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x-GetTime() end", RealLuaEscape(name));
                var t = double.Parse(Styx.WoWInternals.Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                Logger.FailLog("Lua failed in TargetDebuffTimeLeft");
                return 999999;
            }
        }

        public static int TargetCountDebuff(string name)
        {
            name = LocalizeSpellName(name);
            try
            {
                var lua = string.Format("local x=select(4, UnitDebuff('target', \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x end", RealLuaEscape(name));
                var t = int.Parse(Styx.WoWInternals.Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                Logger.FailLog("Lua failed in TargetCountDebuff");
                return 0;
            }
        }

        public static int TargetCountBuff(string name)
        {
            name = LocalizeSpellName(name);
            try
            {
                var lua = string.Format("local x=select(4, UnitBuff('target', \"{0}\", nil, 'PLAYER')); if x==nil then return 0 else return x end", RealLuaEscape(name));
                var t = int.Parse(Styx.WoWInternals.Lua.GetReturnValues(lua)[0]);
                return t;
            }
            catch
            {
                Logger.FailLog("Lua failed in TargetCountBuff");
                return 0;
            }
        }

        #endregion Player and Target Debuffs and Buffs (Damn you alex for making me put these in :P -- wulf)

        #region Misc Lua Helpers

        public static string RealLuaEscape(string luastring)
        {
            var bytes = Encoding.UTF8.GetBytes(luastring);
            return bytes.Aggregate(String.Empty, (current, b) => current + ("\\" + b));
        }

        public static Composite RunMacroText(string macro, CanRunDecoratorDelegate cond)
        {
            return new Decorator(
                       cond,

                //new PrioritySelector(
                       new Sequence(
                           new Action(a => Styx.WoWInternals.Lua.DoString("RunMacroText(\"" + RealLuaEscape(macro) + "\")")),
                           new Action(a => Logger.DebugLog("Running Macro Text: {0}", macro))
                               )
                           );
        }

        public static Composite CancelMyAura(string name, CanRunDecoratorDelegate cond)
        {
            name = LocalizeSpellName(name);
            var macro = String.Format("/cancelaura {0}", name);
            return new Decorator(
                delegate(object a)
                {
                    if (name.Length == 0)
                        return false;

                    if (!cond(a))
                        return false;

                    return true;
                },
                new Sequence(
                    new Action(a => Styx.WoWInternals.Lua.DoString("RunMacroText(\"" + RealLuaEscape(macro) + "\")"))));
        }

        /// <summary>
        /// this will localise the spell name to the local client.
        /// </summary>
        private static readonly Dictionary<string, string> LocalizedSpellNames = new Dictionary<string, string>();

        public static string LocalizeSpellName(string name)
        {
            if (LocalizedSpellNames.ContainsKey(name))
                return LocalizedSpellNames[name];

            string loc;

            int id = 0;
            try
            {
                id = SpellManager.Spells[name].Id;
            }
            catch
            {
                return name;
            }

            try
            {
                loc = Styx.WoWInternals.Lua.GetReturnValues("return select(1, GetSpellInfo(" + id + "))")[0];
            }
            catch
            {
                Logger.FailLog("Lua failed in LocalizeSpellName");
                return name;
            }

            LocalizedSpellNames[name] = loc;
            Logger.DebugLog("Localized spell: '" + name + "' is '" + loc + "'.");
            return loc;
        }

        /// <summary>
        /// Returns the icon name for an ability, i.e. "interface\icons\spell_fel_elementaldevastation"
        /// </summary>
        /// <param name="spellId">ID of spell</param>
        /// <returns>Ability's Icon Label IN LOWER CASE</returns>
        public static string GetSpellIconText(int spellId)
        {
            var vals = Styx.WoWInternals.Lua.GetReturnValues("return select(3, GetSpellInfo(" + spellId + "))")[0];
            return vals.ToLower();
        }

        #endregion Misc Lua Helpers

        #region Energy Calls

        public static double PlayerPower
        {
            get
            {
                try
                {
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Styx.WoWInternals.Lua.GetReturnVal<int>("return UnitPower(\"player\");", 0);
                    }
                }
                catch { Logger.FailLog(" Lua Failed in PlayerPower"); return StyxWoW.Me.CurrentPower; }
            }
        }

        public static double PlayerPowerMax
        {
            get
            {
                try
                {
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Styx.WoWInternals.Lua.GetReturnVal<int>("return UnitPowerMax(\"player\",1);", 0);
                    }
                }
                catch { Logger.FailLog(" Lua Failed in PlayerPowerMax"); return StyxWoW.Me.MaxPower; }
            }
        }


        public static double PlayerChi
        {
            get
            {
                try
                {
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Styx.WoWInternals.Lua.GetReturnVal<int>("return UnitPower(\"player\");", 12);
                    }
                }
                catch { Logger.FailLog(" Lua Failed in PlayerChi"); return StyxWoW.Me.CurrentChi; }
            }
        }

        //Return the Chi for Monk's

        /// <summary>
        /// Not sure if this is the one you need for DK's but it works for druids cat form
        /// </summary>
        private static double PlayerEnergy
        {
            get
            {
                try
                {
                    return Styx.WoWInternals.Lua.GetReturnVal<int>("return UnitMana(\"player\");", 0);
                }
                catch
                {
                    Logger.FailLog(" Lua Failed in PlayerEnergy");
                    return StyxWoW.Me.CurrentMana;
                }
            }
        }

        public static double PlayerComboPts
        {
            get
            {
                try
                {
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Styx.WoWInternals.Lua.GetReturnVal<int>("return GetComboPoints(\"player\");", 0);
                    }
                }
                catch
                {
                    Logger.FailLog(" Lua Failed in PlayerComboPts");
                    return 0;
                }
            }
        }

        public static int RuneType(uint IdNumber)
        {
            {
                try
                {
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Styx.WoWInternals.Lua.GetReturnVal<int>("return GetRuneType(\"player\");", IdNumber);
                    }
                }
                catch
                {
                    Logger.FailLog(" Lua Failed in Runetypes");
                    return 0;
                }
            }
        }

        /// <summary>
        /// Returns a unit's current level of mana, rage, energy or other power type. Returns zero for non-existent units.
        /// </summary>

        public static int PlayerUnitPower(string powerType)
        {
            try
            {
                var myval = Styx.WoWInternals.Lua.GetReturnVal<int>(String.Format("return UnitPower(\"player\", {0})", powerType), 0);

                //Logger.InfoLog("Demonic Power = {0}", myval);
                return myval;
            }
            catch
            {
                Logger.FailLog(" Lua Failed in EVERYTHING");
                return 0;
            }
        }

        /// <summary>
        /// Returns information about the player's mana/energy/etc regeneration rate
        /// </summary>

        /// <summary>
        /// Calculate time to energy cap.
        /// </summary>
        public static double TimeToEnergyCap()
        {
            double timetoEnergyCap;

            double playerEnergy;

            double ER_Rate;

            playerEnergy = Styx.WoWInternals.Lua.GetReturnVal<int>("return UnitMana(\"player\");", 0); // current Energy

            ER_Rate = EnergyRegen();
            timetoEnergyCap = (100 - playerEnergy) * (1.0 / ER_Rate); // math

            return timetoEnergyCap;
        }

        public static double EnergyRegen()
        {
            double energyRegen;

            energyRegen = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetPowerRegen()", 1); // rate of energy regen

            return energyRegen;
        }


        #endregion Energy Calls


        #region SecondryStats - Credit: Singular

        internal static void PopulateSecondryStats()
        {
            using (StyxWoW.Memory.AcquireFrame())
            {
                _secondaryStats = new SecondaryStats();
            }

            // Haste Rating Required Per 1%
            // Level 60	 Level 70	 Level 80	 Level 85	 Level 90
            //   10	      15.77	      32.79	      128.125	 425.19
            if (Styx.Helpers.GlobalSettings.Instance.LogLevel == LogLevel.Diagnostic)
            {
                Logger.DebugLog("");
                Logger.DebugLog("Health: {0}", StyxWoW.Me.MaxHealth);
                Logger.DebugLog("Agility: {0}", StyxWoW.Me.Agility);
                Logger.DebugLog("Intellect: {0}", StyxWoW.Me.Intellect);
                Logger.DebugLog("Spirit: {0}", StyxWoW.Me.Spirit);
                Logger.DebugLog("");
                Logger.DebugLog("Attack Power: {0}", _secondaryStats.AttackPower);
                Logger.DebugLog("Power: {0:F2}", _secondaryStats.Power);
                Logger.DebugLog("Hit(M/R): {0}/{1}", _secondaryStats.MeleeHit, _secondaryStats.SpellHit);
                Logger.DebugLog("Expertise: {0}", _secondaryStats.Expertise);
                Logger.DebugLog("Mastery: {0:F2}", _secondaryStats.Mastery);
                Logger.DebugLog("Mastery (CR): {0:F2}", _secondaryStats.MasteryCR);
                Logger.DebugLog("Crit: {0:F2}", _secondaryStats.Crit);
                Logger.DebugLog("Haste(M/R): {0} (+{1} % Haste) / {2} (+{3} % Haste)", _secondaryStats.MeleeHaste, Math.Round(_secondaryStats.MeleeHaste / 425.19, 2), _secondaryStats.SpellHaste, Math.Round(_secondaryStats.SpellHaste / 425.19, 2));
                Logger.DebugLog("SpellPen: {0}", _secondaryStats.SpellPen);
                Logger.DebugLog("PvP Resil: {0}", _secondaryStats.Resilience);
                Logger.DebugLog("PvP Power: {0}", _secondaryStats.PvpPower);
                Logger.DebugLog("");
            }
        }

        internal class SecondaryStats
        {
            public float MeleeHit { get; set; }

            public float SpellHit { get; set; }

            public float Expertise { get; set; }

            public float MeleeHaste { get; set; }

            public float SpellHaste { get; set; }

            public float SpellPen { get; set; }

            public float Mastery { get; set; }

            public float MasteryCR { get; set; }

            public float Crit { get; set; }

            public float Resilience { get; set; }

            public float PvpPower { get; set; }

            public float AttackPower { get; set; }

            public float Power { get; set; }

            public float Intellect { get; set; }

            public float SpellPower { get; set; }

            public SecondaryStats()
            {
                Refresh();
            }

            public void Refresh()
            {
                try
                {
                    MeleeHit = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetCombatRating(CR_HIT_MELEE)", 0);
                    SpellHit = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetCombatRating(CR_HIT_SPELL)", 0);
                    Expertise = StyxWoW.Me.Expertise;
                    MeleeHaste = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetCombatRating(CR_HASTE_MELEE)", 0);
                    SpellHaste = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetCombatRating(CR_HASTE_SPELL)", 0);
                    SpellPen = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetSpellPenetration()", 0);
                    Mastery = StyxWoW.Me.Mastery;
                    MasteryCR = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetCombatRating(CR_MASTERY)", 0);
                    Crit = StyxWoW.Me.CritPercent;
                    Resilience = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetCombatRating(COMBAT_RATING_RESILIENCE_CRIT_TAKEN)", 0);
                    PvpPower = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetCombatRating(CR_PVP_POWER)", 0);
                    AttackPower = StyxWoW.Me.AttackPower;
                    Power = Styx.WoWInternals.Lua.GetReturnVal<float>("return select(7,UnitDamage(\"player\"))", 0);
                    Intellect = StyxWoW.Me.Intellect;
                    SpellPower = Styx.WoWInternals.Lua.GetReturnVal<float>("return math.max(GetSpellBonusDamage(1),GetSpellBonusDamage(2),GetSpellBonusDamage(3),GetSpellBonusDamage(4),GetSpellBonusDamage(5),GetSpellBonusDamage(6),GetSpellBonusDamage(7))", 0);
                }
                catch 
                {
                    Logger.FailLog(" Lua Failed in SecondaryStats");
                }
              
            }
        }

        #endregion SecondryStats - Credit: Singular

        /// <summary>
        /// 5-1-2013 Lua Eclipse Direction by Mirabis
        /// </summary>
        /// <returns>Eclipse Direction</returns>
        public static EclipseType GetEclipseDirection()
        {
            var dir = Styx.WoWInternals.Lua.GetReturnVal<string>("return GetEclipseDirection();", 0);

            switch (dir)
            {
                case "moon":
                    return EclipseType.Lunar;
                case "sun":
                    return EclipseType.Solar;
                default:
                    return EclipseType.None;
            }
        }
    }
}