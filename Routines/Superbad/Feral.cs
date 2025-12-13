#region

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Bots.DungeonBuddy.Helpers;
using Styx;
using Styx.Common;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    public partial class Superbad
    {
        //This values will not be resetted. Its managed by combatlogger.
        public static double _dot_rip_multiplier;
        public static double Damagemultiplier = 1;

        public static double Ferocious_Bite_sDamage()
        {
            const int FerociousBiteBaseDamage = 500;
            const int FerociousBiteDamagePerComboPoint = 762;
            const double FerociousBiteDamagePerComboPointAttackPower = 0.196;
            return (FerociousBiteBaseDamage +
                      (FerociousBiteDamagePerComboPoint + FerociousBiteDamagePerComboPointAttackPower*AP)*
                      combo_points)*Multiplier*FerociousBiteCritChanceMultiplier*2*ArmorReduction;
        }

        public static double Mangle_sDamage
        {
            get
            {
                double weaponDmg = dps;
                const double mangleCatBaseDmg = 78;
                return (mangleCatBaseDmg*Multiplier + weaponDmg)*5*(buff.feral_fury.react ? 1.5 : 1)*
                       CurrentCritChanceMultiplier*ArmorReduction;
            }
        }

        public static double Rake_sDamage
        {
            get
            {
                double Rake_sAP = AP;
                double Rake_sMastery = Mastery;
                double Rake_sMult = Multiplier;
                return (((99 + (Rake_sAP*0.3))*Rake_sMastery)*Rake_sMult*CurrentCritChanceMultiplier);
            }
        }


        public static double Rip_sDamage
        {
            get
            {
                double Rip_sAP = AP;
                double Rip_sMastery = Mastery;
                double Rip_sMult = Multiplier;
                return (((136*Rip_sMastery) + 384*5*Rip_sMastery + 0.05808*5*Rip_sAP*Rip_sMastery)*Rip_sMult*
                        CurrentCritChanceMultiplier);
            }
        }

        public static double ArmorReduction
        {
            get
            {
                if (StyxWoW.Me.CurrentTarget != null)
                {
                    if (debuff.weakened_armor.stack == 3)
                    {
                        if (StyxWoW.Me.CurrentTarget.Level == 93)
                            return 0.679;
                        if (StyxWoW.Me.CurrentTarget.Level == 92)
                            return 0.698;
                    }
                    else
                    {
                        if (StyxWoW.Me.CurrentTarget.Level == 93)
                            return 0.651;
                        if (StyxWoW.Me.CurrentTarget.Level == 92)
                            return 0.671;
                    }
                }
                return 1;
            }
        }

        public static double FerociousBiteCritChanceMultiplier
        {
            get { return FerociousBiteCritChanceRaw > 2 ? 2 : FerociousBiteCritChanceRaw; }
        }

        public static double FerociousBiteCritChanceRaw
        {
            get { return 0.25 + CurrentCritChanceMultiplier; }
        }

        public static double CurrentCritChanceMultiplier
        {
            get { return CurrentCritChanceRaw > 2 ? 2 : CurrentCritChanceRaw; }
        }

        public static double CurrentCritChanceRaw
        {
            get { return Crit; }
        }


        public static double LevelBasedCritSuppression
        {
            get { return RelativeLevel > 0 ? RelativeLevel/100 : 0; }
        }

        public static double RelativeLevel
        {
            get
            {
                if (StyxWoW.Me.CurrentTarget != null)
                {
                    int lvlDiff = StyxWoW.Me.CurrentTarget.Level - StyxWoW.Me.Level;
                    if (lvlDiff < 0)
                        lvlDiff = lvlDiff*-1;
                    return lvlDiff;
                }
                return 0;
            }
        }

        public static void CatHandler()
        {
            EventHandlers.CalcRealTimeOfRip();
            if (Soulswap())
                return;
            if (Redirect())
                return;
            if (BurstMode)
                DPSButtonPressed();

            if (SuperbadSettings.Instance.UseAoeKey)
            {
                if (AoeMode)
                    HandleAoeCombat();
                if (!AoeMode)
                    HandleSingleTarget();
                return;
            }
            if (_unitCount >= SuperbadSettings.Instance.CatAoe)
            {
                HandleAoeCombat();
                return;
            }
            HandleSingleTarget();
        }

        public static void HandleAoeCombat()
        {



            List<WoWUnit> thrashtargets = UnitList.Where(
                u => target.time_to_die_circle(u) > 1 && (u.Combat || Unit.IsDummy(u)) && u.SpellDistance() <= 8).ToList();
            List<WoWUnit> cycleTargets =
                thrashtargets.Where(
                    u =>
                        StyxWoW.Me.IsFacing(u) && u.IsWithinMeleeRange).ToList();
            if (cat_form())
                return;
            auto_attack();
            if (dot.rip.ticking && dot.rip.remains <= 3 && target.health.pct <= 25 && _energy <= 35)
                if (ferocious_bite())
                    return;
            if (talent.dream_of_cenarius.enabled && buff.predatory_swiftness.up && buff.dream_of_cenarius.down &&
                buff.predatory_swiftness.remains < 1.5)
                if (healing_touch())
                    return;
            if (((time_to_max > 2 && !buff.omen_of_clarity.react) ||
                 (time_to_max > 3 && buff.omen_of_clarity.react)) && target.time_to_die > 5)
                if (debuff.weakened_armor.stack < 3)
                    if (faerie_firecatcombat())
                        return;
            WoWUnit fffCycleTarget =
                cycleTargets.FirstOrDefault(u => debuff.weakened_armor_cycle.stack(u) < 3);
            if (fffCycleTarget != null)
                if ((time_to_max > 2 && !buff.omen_of_clarity.react) ||
                    (time_to_max > 3 && buff.omen_of_clarity.react))
                    if (fff_cycle(fffCycleTarget))
                        return;

            if (_energy < 25 && buff.savage_roar.down && buff.omen_of_clarity.react)
            {
                Spell.LogAction("Pooling energy for Savage Roar!", Color.Yellow);
                return;
            }
            if (buff.savage_roar.down || (buff.savage_roar.remains < 3 && combo_points > 0) ||
                (buff.feral_rage.up && buff.feral_rage.remains <= 1 && talent.soul_of_the_forest.enabled))
                if (savage_roar())
                    return;
            if (_energy <= 35 && !buff.omen_of_clarity.react)
                if (tigers_fury())
                    return;
            if (buff.tigers_fury.up)
                if (berserk())
                    return;

            if (thrashtargets.Count(u => buff.omen_of_clarity.react && dot.thrash_cat_cycle.remains(u) < 3 &&
                                         target.time_to_die_circle(u) >= 6) >= SuperbadSettings.Instance.CatAoe)
            {
                WoWUnit thrashUnit =
                    thrashtargets.Where(
                        u =>
                            buff.omen_of_clarity.react && dot.thrash_cat_cycle.remains(u) < 3 &&
                            target.time_to_die_circle(u) >= 6).ToList().FirstOrDefault();
                if (thrashUnit != null)
                    if (thrash_cat())
                        return;
            }
            if (buff.omen_of_clarity.react)
                if (Swipe_Cat())
                    return;
            if (buff.savage_roar.remains <= 3 && combo_points > 0 && target.health.pct < 25)
                if (savage_roar())
                    return;
            if (combo_points >= 5 && rip_ratio() >= 1.15 && target.time_to_die > 30 &&
                (talent.dream_of_cenarius.enabled || talent.soul_of_the_forest.enabled || _unitCount <= 6))
                if (rip())
                    return;
            if (combo_points >= 5 && dot.rip.ticking && target.health.pct <= 25 && _energy <= 35)
                if (ferocious_bite())
                    return;
            if (combo_points >= 4 && rip_ratio() >= 0.95 && target.time_to_die > 30 && buff.rune_of_reorigination.up &&
                buff.rune_of_reorigination.remains <= 1.5 &&
                (talent.dream_of_cenarius.enabled || talent.soul_of_the_forest.enabled || _unitCount <= 6))
                if (rip())
                    return;
            if (combo_points >= 5 && target.time_to_die >= 6 && dot.rip.remains < 2 &&
                (talent.dream_of_cenarius.enabled || talent.soul_of_the_forest.enabled || _unitCount <= 6))
                if (rip())
                    return;
            if (buff.savage_roar.remains <= 3 && combo_points > 0 && buff.savage_roar.remains + 2 > dot.rip.remains)
                if (savage_roar())
                    return;
            if (buff.savage_roar.remains <= 6 && combo_points >= 5 && buff.savage_roar.remains + 2 <= dot.rip.remains &&
                dot.rip.ticking)
                if (savage_roar())
                    return;
            if (buff.savage_roar.remains <= 12 && combo_points >= 5 && time_to_max <= 1.0 &&
                buff.savage_roar.remains <= dot.rip.remains + 6 && dot.rip.ticking)
                if (savage_roar())
                    return;
            if (talent.dream_of_cenarius.enabled && buff.predatory_swiftness.up && buff.dream_of_cenarius.down &&
                (buff.rune_of_reorigination.up && dot.thrash_cat.remains < 10 || dot.thrash_cat.remains < 4))
                if (healing_touch())
                    return;
            if (thrashtargets.Count(u => buff.rune_of_reorigination.up && dot.thrash_cat_cycle.remains(u) < 9 ||
                                         dot.thrash_cat_cycle.remains(u) < 3) >= SuperbadSettings.Instance.CatAoe)
            {
                WoWUnit thrashUnit2 =
                    thrashtargets.Where(
                        u =>
                            buff.rune_of_reorigination.up && dot.thrash_cat_cycle.remains(u) < 9 ||
                            dot.thrash_cat_cycle.remains(u) < 3).ToList().FirstOrDefault();
                if (thrashUnit2 != null)
                {
                    if (!HasEnergyForThrash)
                    {
                        Spell.LogAction("Pooling energy for Thrash", Color.DarkOrange);
                        return;
                    }
                }
                if (thrashUnit2 != null)
                    if (thrash_cat())
                        return;
            }
            if (buff.feral_fury.react)
                if (Swipe_Cat())
                    return;
            if (combo_points >= 5 && dot.rip.ticking && talent.dream_of_cenarius.enabled &&
                talent.soul_of_the_forest.enabled && buff.berserk.up)
                if (ferocious_bite())
                    return;

            var cpTarget = ObjectManager.GetObjectByGuid<WoWUnit>(StyxWoW.Me.ComboPointsTarget);
            if (combo_points == 0 && cpTarget != null && StyxWoW.Me.RawComboPoints > 1)
                if (buff.savage_roar.remains < (StyxWoW.Me.RawComboPoints*6 + 12))
                    if (savage_roar())
                    {
                        Spell.LogAction("Used CPs from old Target!", Color.LightSalmon);
                        return;
                    }

            /*Rake Cycle!*/
            WoWUnit rakeCycleTarget =
                cycleTargets.FirstOrDefault(
                    u =>
                        buff.rune_of_reorigination.up && _unitCount <= 8 && dot.rake_cycle.remains(u) < 3 &&
                        target.time_to_die_circle(u) >= 15 && StyxWoW.Me.ComboPointsTarget != u.Guid &&
                        u != StyxWoW.Me.CurrentTarget);
            if (combo_points < 3 && _unitCount > 1)
                if (SuperbadSettings.Instance.RakeCycle)
                    if (rakeCycleTarget != null)
                        if (rake_cycle(rakeCycleTarget))
                            return;

            rakeCycleTarget =
                cycleTargets.FirstOrDefault(
                    u =>
                        _unitCount <= 4 && dot.rake_cycle.remains(u) < 3 && target.time_to_die_circle(u) >= 15 &&
                        StyxWoW.Me.ComboPointsTarget != u.Guid && u != StyxWoW.Me.CurrentTarget);
            if (combo_points < 3 && _unitCount > 1)
                if (SuperbadSettings.Instance.RakeCycle)
                    if (rakeCycleTarget != null)
                        if (rake_cycle(rakeCycleTarget))
                            return;
            if (buff.rune_of_reorigination.up && _unitCount <= 8 && dot.rake.remains < 3 && target.time_to_die >= 15)
                if (rake())
                    return;
            if (_unitCount <= 4 && dot.rake.remains < 3 && target.time_to_die >= 15)
                if (rake())
                    return;

            if (dot.rip.ticking && dot.rip.remains <= 6 && target.health.pct <= 25)
                if (Swipe_Cat())
                    return;
            if (cooldown.tigers_fury.remains <= 3)
                if (Swipe_Cat())
                    return;
            if (buff.tigers_fury.up || buff.berserk.up)
                if (Swipe_Cat())
                    return;
            if (time_to_max <= 1.0)
                Swipe_Cat();
        }


        public static double rip_ratio()
        {
            if (!dot.rip.ticking)
                return 2;
            return Rip_sDamage/_dot_rip_multiplier;
        }


        public static double rake_ratio(WoWUnit unit)
        {
            if (dot.rake_cycle.remains(unit) == 0)
                return 2;
            return Rake_sDamage/Spell.GetRakeStrength(unit.Guid);
        }


        public static void HandleSingleTarget()
        {
            List<WoWUnit> cycleTargets =
                UnitList.Where(
                    u => target.time_to_die_circle(u) > 1 && 
                        (u.Combat || Unit.IsDummy(u)) && StyxWoW.Me.IsFacing(u) && u.IsWithinMeleeRange).ToList();
            if (ShatteringBlow())
                return;
            if (cat_form())
                return;
            auto_attack();

            if (combo_points > 0 && StyxWoW.Me.GotTarget && !StyxWoW.Me.CurrentTarget.IsDead &&
                Ferocious_Bite_sDamage() > StyxWoW.Me.CurrentTarget.CurrentHealth && !Unit.IsDummy(StyxWoW.Me.CurrentTarget))
            {
                if (ferocious_bite())
                    return;
            }

            if (talent.force_of_nature.enabled &&
                (charges == 3 || buff.trinket.procagilityreact || target.time_to_die < 20))
                force_of_nature();
            berserking();
            if (buff.prowl.up)
                if (ravage())
                    return;

            if (dot.rip.ticking && dot.rip.remains <= 3 && target.health.pct <= 25)
                if (ferocious_bite())
                    return;

            if (((time_to_max > 2 && !buff.omen_of_clarity.react) ||
                 (time_to_max > 3 && buff.omen_of_clarity.react)) && target.time_to_die > 5)
                if (debuff.weakened_armor.stack < 3)
                    if (faerie_firecatcombat())
                        return;


            WoWUnit fffCycleTarget =
                cycleTargets.FirstOrDefault(u => debuff.weakened_armor_cycle.stack(u) < 3);

            if (fffCycleTarget != null)
                if ((time_to_max > 2 && !buff.omen_of_clarity.react) ||
                    (time_to_max > 3 && buff.omen_of_clarity.react))
                    if (fff_cycle(fffCycleTarget))
                        return;

            if (talent.dream_of_cenarius.enabled && buff.predatory_swiftness.up && buff.dream_of_cenarius.down &&
                (buff.predatory_swiftness.remains < 1.5 || combo_points >= 4))
                if (healing_touch())
                    return;

            if (_energy < 25 && buff.savage_roar.down && buff.omen_of_clarity.react
                && _gcdTimeLeftTotalSeconds <= 0)
            {
                Spell.LogAction("Pooling energy for Savage Roar!", Color.Yellow);
                return;
            }

            if (buff.savage_roar.down)
                if (savage_roar())
                    return;
            if (talent.incarnation.enabled && _energy <= 35 && cooldown.tigers_fury.remains <= 1)
                incarnation();
            if (_energy <= 35 && !buff.omen_of_clarity.react)
                if (tigers_fury())
                    return;
            if (talent.natures_vigil.enabled && buff.tigers_fury.up)
                natures_vigil();
            if (buff.tigers_fury.up || (target.time_to_die < 18 && cooldown.tigers_fury.remains > 6))
                if (berserk())
                    return;
            if (buff.tigers_fury.up)
                use_item();
            lifeblood();
            if (buff.omen_of_clarity.react && dot.thrash_cat.remains < 3 && target.time_to_die >= 6)
                if (thrash_cat())
                    return;
            if (target.time_to_die <= 1 && combo_points >= 3)
                if (ferocious_bite())
                    return;
            if (buff.savage_roar.remains <= 3 && combo_points > 0 && target.health.pct < 25)
                if (savage_roar())
                    return;
            if ((combo_points >= 5 && (target.time_to_die*(target.health.pct - 25)/target.health.pct) < 15 &&
                 buff.rune_of_reorigination.up) || target.time_to_die <= 40)
                virmens_bite_potion();
            if (combo_points >= 5 && rip_ratio() >= 1.15 && target.time_to_die > 30)
                if (rip())
                    return;
            if (combo_points >= 4 && rip_ratio() >= 0.95 && target.time_to_die > 30 && buff.rune_of_reorigination.up &&
                buff.rune_of_reorigination.remains <= 1.5)
                if (rip())
                    return;
            if (combo_points >= 5 && target.health.pct <= 25 && dot.rip.ticking &&
                !(_energy >= 50 || (buff.berserk.up && _energy >= 25))
                && _gcdTimeLeftTotalSeconds <= 0)
            {
                Spell.LogAction("Pooling energy for Ferocious Bite #1", Color.DarkOrange);
                return;
            }
            if (combo_points >= 5 && dot.rip.ticking && target.health.pct <= 25)
                if (ferocious_bite())
                    return;
            if (combo_points >= 5 && target.time_to_die >= 6 && dot.rip.remains < 2 &&
                (buff.berserk.up || dot.rip.remains + 1.9 <= cooldown.tigers_fury.remains))
                if (rip())
                    return;
            /* //TODO: due to the rip bug, this seems to be a dmg loss!
            if (combo_points >= 5 && target.time_to_die >= 6 && dot.rip.remains < 2 && rip_ratio() >= 0.55)
                if (rip())
                    return;*/
            if (buff.savage_roar.remains <= 3 && combo_points > 0 && buff.savage_roar.remains + 2 > dot.rip.remains)
                if (savage_roar())
                    return;
            if (buff.savage_roar.remains <= 6 && combo_points >= 5 && buff.savage_roar.remains + 2 <= dot.rip.remains &&
                dot.rip.ticking)
                if (savage_roar())
                    return;
            if (buff.savage_roar.remains <= 12 && combo_points >= 5 && time_to_max <= 1.0 &&
                buff.savage_roar.remains <= dot.rip.remains + 6 && dot.rip.ticking)
                if (savage_roar())
                    return;

            var cpTarget = ObjectManager.GetObjectByGuid<WoWUnit>(StyxWoW.Me.ComboPointsTarget);
            if (combo_points == 0 && cpTarget != null && StyxWoW.Me.RawComboPoints > 1)
                if (buff.savage_roar.remains < (StyxWoW.Me.RawComboPoints*6 + 12))
                    if (savage_roar())
                    {
                        Spell.LogAction("Used CPs from old Target!", Color.LightSalmon);
                        return;
                    }


            //First we apply rake on all targets that got no rake at all

            WoWUnit rakeCycleTarget =
                cycleTargets.FirstOrDefault(
                    u =>
                        target.time_to_die_circle(u) - dot.rake_cycle.remains(u) > 3 && dot.rake_cycle.remains(u) <= 0 &&
                        StyxWoW.Me.ComboPointsTarget != u.Guid && u != StyxWoW.Me.CurrentTarget);
            if (combo_points < 3 && _unitCount > 1)
                if (SuperbadSettings.Instance.RakeCycle)
                    if (rakeCycleTarget != null)
                        if (rake_cycle(rakeCycleTarget))
                            return;

            if (target.time_to_die - dot.rake.remains > 3 && dot.rake.remains <= 0)
                if (rake())
                    return;


            /*Rake Cycle!*/
            rakeCycleTarget =
                cycleTargets.FirstOrDefault(
                    u =>
                        buff.rune_of_reorigination.up && dot.rake_cycle.remains(u) < 9 &&
                        buff.rune_of_reorigination.remains <= 1.5 && StyxWoW.Me.ComboPointsTarget != u.Guid
                        && u != StyxWoW.Me.CurrentTarget);

            if (combo_points < 3 && _unitCount > 1)
                if (SuperbadSettings.Instance.RakeCycle)
                    if (rakeCycleTarget != null)
                        if (rake_cycle(rakeCycleTarget))
                            return;

            rakeCycleTarget =
                cycleTargets.FirstOrDefault(u => target.time_to_die_circle(u) - dot.rake_cycle.remains(u) > 3 &&
                                                 (Rake_sDamage > Spell.GetRakeStrength(u.Guid)*1.12 ||
                                                  (dot.rake_cycle.remains(u) < 3 && rake_ratio(u) >= 0.75) ||
                                                  dot.rake_cycle.remains(u) == 0) &&
                                                 StyxWoW.Me.ComboPointsTarget != u.Guid
                                                 && u != StyxWoW.Me.CurrentTarget);
            if (combo_points < 3 && _unitCount > 1)
                if (SuperbadSettings.Instance.RakeCycle)
                    if (rakeCycleTarget != null)
                        if (rake_cycle(rakeCycleTarget))
                            return;


            if (buff.rune_of_reorigination.up && dot.rake.remains < 9 && buff.rune_of_reorigination.remains <= 1.5)
                if (rake())
                    return;
            if (StyxWoW.Me.CurrentTarget != null)
                if (target.time_to_die - dot.rake.remains > 3 &&
                    (Rake_sDamage > Spell.GetRakeStrength(StyxWoW.Me.CurrentTarget.Guid)*1.12 ||
                     (dot.rake.remains < 3 && rake_ratio(StyxWoW.Me.CurrentTarget) >= 0.75)))
                    if (rake())
                        return;


            if (target.time_to_die >= 6 && dot.thrash_cat.remains < 3 &&
                (dot.rip.remains >= 8 && buff.savage_roar.remains >= 12 || buff.berserk.up || combo_points >= 5) &&
                dot.rip.ticking && _gcdTimeLeftTotalSeconds <= 0)
            {
                if (!HasEnergyForThrash)
                {
                    Spell.LogAction("Pooling energy for Thrash", Color.DarkOrange);
                    return;
                }
            }
            if (target.time_to_die >= 6 && dot.thrash_cat.remains < 3 &&
                (dot.rip.remains >= 8 && buff.savage_roar.remains >= 12 || buff.berserk.up || combo_points >= 5) &&
                dot.rip.ticking)
                if (thrash_cat())
                    return;
            if (target.time_to_die >= 6 && dot.thrash_cat.remains < 9 && buff.rune_of_reorigination.up &&
                buff.rune_of_reorigination.remains <= 1.5 && dot.rip.ticking && _gcdTimeLeftTotalSeconds <= 0)
            {
                if (!HasEnergyForThrash)
                {
                    Spell.LogAction("Pooling energy for Thrash", Color.DarkOrange);
                    return;
                }
            }
            if (target.time_to_die >= 6 && dot.thrash_cat.remains < 9 && buff.rune_of_reorigination.up &&
                buff.rune_of_reorigination.remains <= 1.5 && dot.rip.ticking)
                if (thrash_cat())
                    return;
            if (combo_points >= 5 &&
                !(time_to_max <= 1 || (buff.berserk.up && _energy >= 25 || cooldown.tigers_fury.remains <= 3 && _energy >= 50) ||
                  (buff.feral_rage.up && buff.feral_rage.remains <= 1)) && dot.rip.ticking && dot.rip.remains > 3 && 
                _gcdTimeLeftTotalSeconds <= 0)
            {
                {
                    Spell.LogAction("Pooling energy for FB", Color.DarkOrange);
                    return;
                }
            }
            if (combo_points >= 5 && dot.rip.ticking && dot.rip.remains > 3)
                if (ferocious_bite())
                    return;
            if ((dot.rip.ticking && EventHandlers.Ripextends < 3))
                if (fillerExtendOnly())
                    return;
            if (buff.omen_of_clarity.react)
                if (filler())
                    return;
            if (buff.feral_fury.react)
                if (filler())
                    return;
            if ((combo_points < 5 && dot.rip.remains < 3.0) || (combo_points == 0 && buff.savage_roar.remains < 2))
                if (filler())
                    return;
            if (target.time_to_die <= 8.5)
                if (filler())
                    return;
            if (buff.tigers_fury.up || buff.berserk.up)
                if (filler())
                    return;
            if (cooldown.tigers_fury.remains <= 3)
                if (filler())
                    return;
            if (time_to_max <= 3.0 && buff.predatory_swiftness.up && buff.predatory_swiftness.remains < 3.3)
                if (filler())
                    return;
            if (time_to_max <= 2.0)
                if (filler())
                    return;
            if (FeralSpirit())
                return;
            DeathCoil();
        }

        public static bool filler()
        {
            if (ravage())
                return true;
            if (StyxWoW.Me.CurrentTarget != null && target.time_to_die - dot.rake.remains > 3 &&
                Rake_sDamage*(dot.rake.ticks_remain + 1) -
                Spell.GetRakeStrength(StyxWoW.Me.CurrentTarget.Guid)*dot.rake.ticks_remain > Mangle_sDamage)
                if (rake())
                    return true;
            if ((buff.omen_of_clarity.react || buff.berserk.up || EnergyRegen >= 15 || buff.feral_fury.react) &&
                buff.king_of_the_jungle.down)
                if (shred())
                    return true;
            if (buff.king_of_the_jungle.down)
                if (mangle_cat())
                    return true;
            return false;
        }

           public static bool fillerExtendOnly()
        {
            if (ravage())
                return true;
            if ((buff.omen_of_clarity.react || buff.berserk.up || EnergyRegen >= 15 || buff.feral_fury.react) &&
                buff.king_of_the_jungle.down)
                if (shred())
                    return true;
            if (buff.king_of_the_jungle.down)
                if (mangle_cat())
                    return true;
            return false;
        }

        public static void CatPullHandler()
        {
            if (StyxWoW.Me.Shapeshift == ShapeshiftForm.Aqua)
                if (StyxWoW.Me.CurrentTarget != null && _distance >= 16)
                    return;
            if (cat_form())
                return;
            if (Unit.IsAboveTheGround(StyxWoW.Me.CurrentTarget))
            {
                if (faerie_fire())
                    return;
            }
            if (Unit.IsAboveTheGround(StyxWoW.Me.CurrentTarget))
            {
                if (moonfire())
                    return;
            }
            if (!buff.prowl.up && SuperbadSettings.Instance.PullStealth)
                if (prowl())
                    return;
            if (buff.prowl.up && SuperbadSettings.Instance.StealthOpener == 1)
            {
                if (SpeedHandler())
                    return;
                if (Navigator.CanNavigateFully(StyxWoW.Me.Location, CalculatePointBehindTarget(), 4))
                {
                    if (StyxWoW.Me.IsBehind(StyxWoW.Me.CurrentTarget))
                    {
                        if (ravage())
                            return;
                    }
                    if (_distance > Unit.MeleeRange)
                    {
                        return;
                    }
                }

                if (!Navigator.CanNavigateFully(StyxWoW.Me.Location, CalculatePointBehindTarget(), 4))
                {
                    Spell.LogAction("Can't get behind the target, using Pounce instead!", Color.DarkOrange);
                    if (pounce())
                        return;
                }
            }
            if (buff.prowl.up && SuperbadSettings.Instance.StealthOpener == 0)
            {
                if (SpeedHandler())
                    return;
                if (pounce())
                    return;
            }
            if (SuperbadSettings.Instance.PullStealth) return;
            if (faerie_fire())
                return;
            if (SpeedHandler())
                return;
            if (Mangle())
                return;
            auto_attack();
        }
    }
}