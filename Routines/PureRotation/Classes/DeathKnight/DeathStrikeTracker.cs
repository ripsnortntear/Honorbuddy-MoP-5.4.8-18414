using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PureRotation.Core.CombatLog;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using Styx;
using Styx.WoWInternals;
using Lua = PureRotation.Helpers.Lua;

namespace PureRotation.Classes.DeathKnight
{
    [UsedImplicitly]
    internal static class DeathStrikeTracker
    {
        // The goal is to track the damage taken over the last 5 seconds
        // if its predicted value is greater than the minumum DS Percent then DS.
        // the tracking of health is working well from the combat log and is accurate.
        // the mastery calculation checks out ok
        // shield percent is accurate and checks out ok
        // predicted value is fine and working well

        // issues:
        // Add detatch combat log event
        // -- wulf.

        # region variables

        private static bool _combatLogAttached;

        private const double scentBloodStackBuff = 0.2;
        private const double bloodShieldMasteryRatio = 96;
        private const double bloodShieldMasteryBase = 50;
        private static double scentBloodStacks;
        private static double bloodChargeStacks;
        private const double minDSPercent = 0.07; // 7% of base healths
        private const double dsHealModifier = 0.20; // Percent of the DS Heal from the tooltip.
        private const double vbGlyphedHealingInc = 0.4;
        private const double vbUnglyphedHealingInc = 0.25;
        private const double guardianSpiritHealBuff = 0.40;
        private const double LUCK_OF_THE_DRAW_MOD = 0.05;
        private static double Tier14Bonus = 1;
        private const double T14BonusAmt = 0.1;

        private static Dictionary<DateTime, double> damageTaken; // Stores the last 5 seconds of damage

        private static bool AddingDamageTaken { get; set; }

        private static bool RemovingDamageTaken { get; set; }

        private static readonly double shieldPercent = ((Lua._secondaryStats.Mastery / bloodShieldMasteryRatio) + bloodShieldMasteryBase) / 100;

        #endregion

        public static void Initialize()
        {
            damageTaken = new Dictionary<DateTime, double>(); // initialise our damage log
            Tier14Bonus = 1 + (Has4PcTier14 ? T14BonusAmt : 0); // set our tier bonus up, if they change gear without restarting, meh fuck em.
            AttachCombatLogEvent();
        }

        public static void Pulse()
        {
            RemoveDamageTaken(DateTime.Now);
        }

        #region TierChecks

        private static readonly HashSet<int> Tier14Ids = new HashSet<int>
        {
            -526, // 509 ilvl
            1124, // 496 ilvl
            -503,  // 483 ilvl
        };

        private static bool Has4PcTier14 { get { return Tier14Ids.Any(Item.Has4PcTeirBonus); } }

        private static bool Has2PcTier14 { get { return Tier14Ids.Any(Item.Has2PcTeirBonus); } }

        #endregion

        #region Combat Log Events

        private static void AttachCombatLogEvent()
        {
            CombatLogHandler.Register("SWING_DAMAGE", HandleCombatLog);
            CombatLogHandler.Register("SPELL_DAMAGE", HandleCombatLog);
        }

        private static void HandleCombatLog(CombatLogEventArgs args)
        {
            //var e = new CombatLogEventArgs(args.EventName, args.FireTimeStamp, args.Args);
            switch (args.Event)
            {
                case "SWING_DAMAGE":
                    if (args.DestGuid == StyxWoW.Me.Guid)
                    {
                        object damage = args.Amount;
                        if (!AddingDamageTaken)
                            AddDamageTaken(DateTime.Now, (int)(double)damage);
                    }
                    break;

                case "RANGE_DAMAGE":
                case "SPELL_DAMAGE":
                    if (args.DestGuid == StyxWoW.Me.Guid)
                    {
                        object damage = args.Amount;
                        string school = args.SpellSchool.ToString();
                        string spellname = args.SpellName;

                        // Do not count damage from no source or maybe this is just particular items like Shannox's Jagged Tear?
                        // Do not count Spirit Link damage since it doesn't affect DS.
                        bool countDamage = args.SourceName != null ||
                                           (args.SpellName == "Spirit Link" && args.SourceName == "Spirit Link Totem");

                        if (countDamage && !AddingDamageTaken)
                        {
                            AddDamageTaken(DateTime.Now, (int)(double)damage);
                        }
                    }
                    break;
            }
        }

        #endregion

        #region Add, Remove, Get damage from the damage log

        private static void AddDamageTaken(DateTime timestamp, int damage)
        {
            // Add the new damage taken data
            try
            {
                if (RemovingDamageTaken) return;

                AddingDamageTaken = true;

                damageTaken[timestamp] = damage;

                AddingDamageTaken = false;
            }
            catch (Exception ex)
            {
                damageTaken[timestamp] = damageTaken.ContainsKey(timestamp) ? damage : 0;

                AddingDamageTaken = false;

                Logger.DebugLog("AddDamageTaken : {0}", ex);
            }
        }

        private static void RemoveDamageTaken(DateTime timestamp)
        {
            TimeSpan lastSeconds = TimeSpan.FromSeconds(5); // our window

            try
            {
                if (AddingDamageTaken) return;

                RemovingDamageTaken = true;
                Dictionary<DateTime, double> RemoveList = new Dictionary<DateTime, double>();

                // Remove any data older than lastSeconds
                foreach (var entry in damageTaken.Where(entry => timestamp - entry.Key > lastSeconds + TimeSpan.FromSeconds(3)))
                {
                    RemoveList.Add(entry.Key, entry.Value);
                }
                foreach (var entry in RemoveList)
                {
                    damageTaken.Remove(entry.Key);
                }
                RemovingDamageTaken = false;
            }
            catch (Exception ex)
            {
                Logger.DebugLog("RemovingDamageTaken : {0}", ex);
            }
        }

        private static double GetDamageTaken(DateTime timestamp = default(DateTime))
        {
            DateTime current = timestamp;
            TimeSpan lastSeconds = TimeSpan.FromSeconds(5); // our windo

            // If the damage occured in the window,
            // then add it.

            return (from entry in damageTaken
                    let diff = current - entry.Key
                    where diff <= lastSeconds && diff >= TimeSpan.FromSeconds(0)
                    select entry).Aggregate<KeyValuePair<DateTime, double>, double>(0, (current1, entry) => current1 + entry.Value);
        }

        #endregion

        public static bool DeathStrikeOK
        {
            get
            {
                double predictedBSValue = 0.0;
                double predictedDSValue = 0.0;
                double minimumValue = 0.0;

                var damageOverFiveSeconds = GetDamageTaken(DateTime.Now); // the damage total over the last five seconds

                //Logger.DebugLog("DSTracker: Total Damage [{0}]", damageOverFiveSeconds);

                scentBloodStacks = StyxWoW.Me.CachedStackCount("Scent of Blood");
                bloodChargeStacks = StyxWoW.Me.CachedStackCount("Blood Charge");
                var healthDeficit = (StyxWoW.Me.MaxHealth - StyxWoW.Me.CurrentHealth);

                // This should return the predicted shield over the last 5 seconds
                double damagebaseValue = damageOverFiveSeconds * dsHealModifier * Tier14Bonus * (1 + (scentBloodStacks * scentBloodStackBuff));
                predictedBSValue = Math.Floor((damagebaseValue * shieldPercent) + 0.5);
                predictedDSValue = Math.Floor((damagebaseValue * GetEffectiveHealingBuffModifiers) + 0.5);

                // deathstrike heal/blood shield tracking
                double maxhealthbaseValue = StyxWoW.Me.MaxHealth * minDSPercent * Tier14Bonus * (1 + (scentBloodStacks * scentBloodStackBuff));
                double bsMinimum = Math.Floor((maxhealthbaseValue * shieldPercent) + 0.5);
                double dsHealMin = Math.Floor((maxhealthbaseValue * GetEffectiveHealingBuffModifiers) + 0.5);

                minimumValue = dsHealMin;
                double estimate = minimumValue;
                if (predictedDSValue > minimumValue)
                {
                    estimate = predictedDSValue;
                }

                // Logger.DebugLog("======================================================================================");
                // Logger.DebugLog("DSTracker: hpDeficit [{0}] >= minimumValue [{1}] estimate [{2}] predictedDSValue [{3}]", healthDeficit, minimumValue, estimate, predictedDSValue);
                // Logger.DebugLog("DSTracker: maxhealthbaseValue [{0}] * GetEffectiveHealingBuffModifiers [{1}] ", maxhealthbaseValue, GetEffectiveHealingBuffModifiers);
                // Logger.DebugLog("DSTracker: predictedBSValue [{0}] * bsMinimum [{1}] ", predictedBSValue, bsMinimum);
                // Logger.DebugLog("======================================================================================");

                // Death Strike for threat and nothing else to do
                var dsForThreat = ((StyxWoW.Me.UnholyRuneCount + StyxWoW.Me.FrostRuneCount + StyxWoW.Me.DeathRuneCount >= 4) || bloodChargeStacks > 9) && StyxWoW.Me.HealthPercent > 96;

                // Death strike for survival
                var dsForSurvival = healthDeficit >= estimate && StyxWoW.Me.HealthPercent < 96;

                return dsForSurvival || dsForThreat;
            }
        }

        private static double GetEffectiveHealingBuffModifiers
        {
            get
            {
                double vbHealingInc = 0.0;
                double gsHealModifier = 0.0;
                double luckOfTheDrawAmt = 0.0;
                uint lotDStackcount = 0;

                // Vampiric Blood
                double vbHealingIncModified = TalentManager.HasGlyph("Vampiric Blood") ? vbGlyphedHealingInc : vbUnglyphedHealingInc;
                if (StyxWoW.Me.CachedHasAura("Vampiric Blood")) vbHealingInc = vbHealingIncModified;

                // Luck of the Draw - MoP Dungeon bonus
                if (StyxWoW.Me.CachedHasAura("Luck of the Draw"))
                {
                    lotDStackcount = StyxWoW.Me.CachedStackCount("Luck of the Draw");
                }
                luckOfTheDrawAmt = LUCK_OF_THE_DRAW_MOD * lotDStackcount;

                // Guardian Spirit - from priest
                if (StyxWoW.Me.CachedHasAura("Guardian Spirit")) gsHealModifier = guardianSpiritHealBuff;

                // Logger.DebugLog("DSTracker: Vampiric Blood = [{0}]  Guardian Spirit = [{1}] Glyph of Vampiric Blood = [{2}]", StyxWoW.Me.HasAura("Vampiric Blood"), StyxWoW.Me.HasAura("Guardian Spirit"), TalentManager.HasGlyph("Vampiric Blood"));
                // Logger.DebugLog("DSTracker: (1 + vbHealingInc) = [{0}] * (1 + gsHealModifier) = [{1}] * (1 + luckOfTheDrawAmt) = [{2}]", (1 + vbHealingInc), (1 + gsHealModifier), (1 + luckOfTheDrawAmt));

                return (1 + vbHealingInc) * (1 + gsHealModifier) * (1 + luckOfTheDrawAmt);
            }
        }
    }
}