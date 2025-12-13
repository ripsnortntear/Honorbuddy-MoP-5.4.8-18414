#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/Classes/BaseStats.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.CombatLog;
using Oracle.Shared.Utilities;
using Styx;
using Styx.Common.Helpers;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Healing.Chronicle.Classes
{
    public class BaseStats
    {
        public static readonly float BaseMana = StyxWoW.Me.BaseMana;

        static BaseStats()
        {
            // Make sure we have a singleton instance!
            Instance = new BaseStats();
        }

        public static readonly WaitTimer SpellPowerUpdateTimer = new WaitTimer(TimeSpan.FromSeconds(2));

        public void Initialize()
        {
            CombatLogHandler.Register("UNIT_STATS", HandleSpellPowerUpdateLua);
            CombatLogHandler.Register("UNIT_AURA", HandleSpellPowerUpdateLua);
            CombatLogHandler.Register("FORGE_MASTER_ITEM_CHANGED", HandleSpellPowerUpdateLua);
            CombatLogHandler.Register("ACTIVE_TALENT_GROUP_CHANGED", HandleSpellPowerUpdateLua);
            CombatLogHandler.Register("PLAYER_TALENT_UPDATE", HandleSpellPowerUpdateLua);

            CurrentSpellPower = 0;
            UpdateSpellPower();
        }

        private void HandleSpellPowerUpdateLua(CombatLogEventArgs args)
        {
            //Logger.Output("Event Fired {0}", args.EventName);
            if (SpellPowerUpdateTimer.IsFinished || args.Event == "UNIT_STATS")
            {
                UpdateSpellPower();
                SpellPowerUpdateTimer.Reset();
            }
        }

        private static void UpdateSpellPower()
        {
            using (new PerformanceLogger("UpdateSpellPower"))
            {
                using (StyxWoW.Memory.AcquireFrame())
                {
                    //Logger.Output("Checked Spell Power at : {0} and current: {1}", DateTime.Now, CurrentSpellPower);
                    CurrentSpellPower = Styx.WoWInternals.Lua.GetReturnVal<float>("return math.max(GetSpellBonusDamage(7),GetSpellBonusHealing())", 0);
                }
            }
        }

        public static BaseStats Instance { get; private set; }

        public static float CurrentSpellPower { get; private set; }

        public static float GetPlayerSpellPower { get { return CurrentSpellPower; } }

        public static float GetPlayerMastery
        {
            get
            {
                using (new PerformanceLogger("GetPlayerMastery"))
                {
                    return StyxWoW.Me.Mastery; // Styx.WoWInternals.Lua.GetReturnVal<float>("return GetCombatRating(CR_MASTERY)", 0);
                }
            }
        }

        public static float GetPlayerIntellect
        {
            get
            {
                using (new PerformanceLogger("GetPlayerIntellect"))
                {
                    return StyxWoW.Me.Intellect;
                }
            }
        }

        public static float GetPlayerCrit
        {
            get
            {
                using (new PerformanceLogger("GetPlayerCrit"))
                {
                    return StyxWoW.Me.CritPercent;
                    //Styx.WoWInternals.Lua.GetReturnVal<float>("return GetCombatRating(CR_CRIT_SPELL)", 0);
                }
            }
        }

        public static float GetHasteModifier
        {
            get
            {
                using (new PerformanceLogger("GetHasteModifier"))
                {
                    return StyxWoW.Me.HasteModifier;
                }
            }
        }

        #region Player Moddifiers

        private static readonly Dictionary<int, double> Modifiers = new Dictionary<int, double>
            {
                    {33891, 0.15}, // Heals increased by 15% //"Incarnation: Tree of Life"
                    {108288, 0.25}, // Heals increased by 25% - introduced in patch 5.4 //"Heart of the Wild"
                    {124974, 0.12}, // Heals increased by 12% - introduced in patch 5.4 //"Nature's Vigil"
                    {108373, 0.30}, // Wrath, Starfire, Starsurge, and melee abilities increase healing done by your next healing spell by 30%. //"Dream of Cenarius"
                    {73685, 0.30}, // Heals increased by 30% //"Unleash Life"
                };

        public static double GetPlayerSpellModifiers(WoWUnit unit)
        {
            using (new PerformanceLogger("GetPlayerSpellModifiers"))
            {
                double totalmodifier = 1;

                if (!OracleRoutine.IsViable(unit)) return totalmodifier;

                try
                {
                    totalmodifier += Modifiers.Where(mod => unit.HasAura(mod.Key)).Sum(mod => mod.Value);

                    totalmodifier += GetBaseClassModifiers;
                }
                catch { return totalmodifier; }

                return totalmodifier;
            }
        }

        private static double GetBaseClassModifiers
        {
            get
            {
                double mod = 0;
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Paladin:
                        mod += 0.25; // Holy Insight
                        mod += 0.05; //Seal of Insight
                        break;

                    case WoWClass.Priest:
                        mod += 0.1; // Inner Fire
                        break;

                    case WoWClass.Shaman:
                        mod += 0.25; // Purification
                        //mod += 24; // + Mastery .// Mastery: Deep Healing
                        break;

                    case WoWClass.Druid:
                        mod += 0.1; // Naturalist
                        //mod += 0.15; // Mastery: Harmony
                        break;

                    case WoWClass.Monk:
                        mod += 0.2; // Stance of the Wise Serpent
                        break;
                }

                return mod;
            }
        }

        #endregion Player Moddifiers

        #region Mastery Rating Multiplier

        //internal readonly float Mastery = GetMasteryFromRating(2000, 90);

        private static readonly float[] _masteryRatingMultiplier = new float[]
        {
              0.538461983203888f,    0.538461983203888f,    0.538461983203888f,    0.538461983203888f,    0.538461983203888f,
              0.538461983203888f,    0.538461983203888f,    0.538461983203888f,    0.538461983203888f,    0.538461983203888f,
              0.807691991329193f,    1.076923012733459f,    1.346153974533081f,    1.615385055541992f,    1.884614944458008f,
              2.153846025466919f,    2.423077106475830f,    2.692307949066162f,    2.961538076400757f,    3.230768918991089f,
              3.500000000000000f,    3.769231081008911f,    4.038462162017822f,    4.307692050933838f,    4.576922893524170f,
              4.846154212951660f,    5.115385055541992f,    5.384614944458008f,    5.653845787048340f,    5.923077106475830f,
              6.192306995391846f,    6.461537837982178f,    6.730769157409668f,    7.000000000000000f,    7.269230842590332f,
              7.538462162017822f,    7.807693004608154f,    8.076923370361328f,    8.346154212951660f,    8.615384101867676f,
              8.884614944458008f,    9.153845787048340f,    9.423076629638672f,    9.692307472229004f,    9.961538314819336f,
             10.230770111083984f,   10.500000000000000f,   10.769231796264648f,   11.038461685180664f,   11.307692527770996f,
             11.576923370361328f,   11.846155166625977f,   12.115385055541992f,   12.384616851806641f,   12.653846740722656f,
             12.923078536987305f,   13.192308425903320f,   13.461539268493652f,   13.730770111083984f,   14.000000000000000f,
             14.531646728515625f,   15.105264663696289f,   15.726029396057129f,   16.399999618530273f,   17.134328842163086f,
             17.937500000000000f,   18.819673538208008f,   19.793104171752930f,   20.872728347778320f,   22.076923370361328f,
             23.753721237182617f,   25.557874679565430f,   27.499055862426758f,   29.587677001953125f,   31.834934234619141f,
             34.252872467041016f,   36.854465484619141f,   39.653648376464844f,   42.665439605712891f,   45.905986785888672f,
             60.278423309326172f,   79.155647277832031f,  103.985641479492188f,  136.538131713867188f,  179.280044555664062f,
            228.000000000000000f,  290.000000000000000f,  370.000000000000000f,  470.000000000000000f,  600.000000000000000f
        };

        public static float GetMasteryFromRating(float Rating, int Level)
        {
            //Logger.Output("{0} / {1} : level = {2}", Rating, MasteryRatingMultiplier(Level), Level);
            return Rating / MasteryRatingMultiplier(Level);
        }

        // Takes in the level of the user and returns the required mastery rating for 1 mastery
        public static float MasteryRatingMultiplier(int Level)
        {
            return _masteryRatingMultiplier[Level - 1];
        }

        #endregion Mastery Rating Multiplier

        #region Spell Crit Rating Multiplier

        // USAGE: SpellCrit += GetSpellCritFromIntellect(Intellect) + GetSpellCritFromRating(CritRating);

        // Same for all classes
        public const float INT_PER_SPELLCRIT = 2533.66f;

        private static readonly float[] _spellCritRatingMultiplier = new float[]
        {
              0.538461983203888f,    0.538461983203888f,    0.538461983203888f,    0.538461983203888f,    0.538461983203888f,
              0.538461983203888f,    0.538461983203888f,    0.538461983203888f,    0.538461983203888f,    0.538461983203888f,
              0.807691991329193f,    1.076923012733459f,    1.346153974533081f,    1.615385055541992f,    1.884614944458008f,
              2.153846025466919f,    2.423077106475830f,    2.692307949066162f,    2.961538076400757f,    3.230768918991089f,
              3.500000000000000f,    3.769231081008911f,    4.038462162017822f,    4.307692050933838f,    4.576922893524170f,
              4.846154212951660f,    5.115385055541992f,    5.384614944458008f,    5.653845787048340f,    5.923077106475830f,
              6.192306995391846f,    6.461537837982178f,    6.730769157409668f,    7.000000000000000f,    7.269230842590332f,
              7.538462162017822f,    7.807693004608154f,    8.076923370361328f,    8.346154212951660f,    8.615384101867676f,
              8.884614944458008f,    9.153845787048340f,    9.423076629638672f,    9.692307472229004f,    9.961538314819336f,
             10.230770111083984f,   10.500000000000000f,   10.769231796264648f,   11.038461685180664f,   11.307692527770996f,
             11.576923370361328f,   11.846155166625977f,   12.115385055541992f,   12.384616851806641f,   12.653846740722656f,
             12.923078536987305f,   13.192308425903320f,   13.461539268493652f,   13.730770111083984f,   14.000000000000000f,
             14.531646728515625f,   15.105264663696289f,   15.726029396057129f,   16.399999618530273f,   17.134328842163086f,
             17.937500000000000f,   18.819673538208008f,   19.793104171752930f,   20.872728347778320f,   22.076923370361328f,
             23.753721237182617f,   25.557874679565430f,   27.499055862426758f,   29.587677001953125f,   31.834934234619141f,
             34.252872467041016f,   36.854465484619141f,   39.653648376464844f,   42.665439605712891f,   45.905986785888672f,
             60.278423309326172f,   79.155647277832031f,  103.985641479492188f,  136.538131713867188f,  179.280044555664062f,
            228.000000000000000f,  290.000000000000000f,  370.000000000000000f,  470.000000000000000f,  600.000000000000000f
        };

        public static float GetSpellCritFromIntellect(float Intellect)
        {
            return Intellect / INT_PER_SPELLCRIT * 0.01f;
        }

        public static float GetSpellCritFromRating(float Rating, int Level)
        {
            return Rating / (SpellCritRatingMultiplier(Level) * 100);
        }

        // Takes in the level of the user and returns the required Spell Crit rating for 1% Spell Crit
        public static float SpellCritRatingMultiplier(int Level)
        {
            return _spellCritRatingMultiplier[Level - 1];
        }

        #endregion Spell Crit Rating Multiplier
    }
}