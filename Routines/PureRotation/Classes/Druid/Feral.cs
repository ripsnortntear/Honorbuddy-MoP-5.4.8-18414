#region Revision info

/*
 * $Author: Handnavi$
 * $Date: 2013-07-23 17:41:47 +0200 (Di, 23 Jul 2013) $
 * $ID$
 * $Revision: 1$
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Druid/Feral.cs $
 * $LastChangedBy:  wulf$
 * $ChangesMade: $
 */

#endregion Revision info

using System.Collections.Generic;
using System.Linq;
using CommonBehaviors.Actions;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.Druid
{
    [UsedImplicitly]
    internal class Feral : RotationBase
    {
        public delegate IEnumerable<WoWUnit> EnumWoWUnitDelegate(object context);

        #region Variables

        //general variables
        private static double? _rake_tick_multiplier;

        private static double? _rip_tick_multiplier;
        private static double? _rip_remains;
        private static double? _combo_points;
        private static double? _energy;
        private static double? _time_to_max;
        private static double? _EnergyRegen;
        private static double? _cast_time;

        //Debuff
        public static double? _debuff_weakened_armor_stack;

        //target
        public static double? _time_to_die;

        public static double? _pct;

        //Cooldown
        public static double? _cooldown_berserk_remains;

        public static double? _cooldown_tigers_fury_remains;

        //Dot
        public static double? _dot_rake_remains;

        public static bool? _dot_rip_ticking;
        public static double? _dot_rip_remains;
        public static double? _dot_thrash_cat_remains;

        //Buff
        public static bool? _buff_berserk_up;

        public static bool? _buff_berserk_down;
        public static double? _buff_berserk_remains;
        public static bool? _buff_cat_form_down;
        public static bool? _buff_dream_of_cenarius_damage_down;
        public static bool? _buff_dream_of_cenarius_damage_up;
        public static double? _buff_dream_of_cenarius_damage_stack;
        public static double? _buff_heart_of_the_wild_remains;
        public static bool? _buff_heart_of_the_wild_up;
        public static bool? _buff_king_of_the_jungle_up;
        public static bool? _buff_king_of_the_jungle_down;
        public static bool? _buff_natures_swiftness_up;
        public static bool? _buff_natures_vigil_up;
        public static bool? _buff_omen_of_clarity_react;
        public static bool? _buff_predatory_swiftness_up;
        public static bool? _buff_predatory_swiftness_down;
        public static double? _buff_predatory_swiftness_remains;
        public static double? _buff_savage_roar_remains;
        public static bool? _buff_savage_roar_down;
        public static bool? _buff_savage_roar_up;
        public static bool? _buff_tigers_fury_up;
        public static bool? _buff_virmens_bite_potion_up;

        //This values will not be resetted. Its managed by combatlogger.
        public static double _ExtendedRip;

        public static double _dot_rake_multiplier;
        public static double _dot_rip_multiplier;

        //NEW STUFF
        public static double? _rip_ratio;

        public static double? _rake_ratio;
        public static double? _time_til_bitw;
        public static double? _time;
        public static bool? _buff_rune_of_reorigination_up;
        public static double? _buff_rune_of_reorigination_remains;
        public static bool? _dot_rake_ticking;

        #endregion Variables

        #region Calculations

        public static double rip_calclogger
        {
            get
            {
                double mastery = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetMasteryEffect()", 0) / 100;
                return ((113 + (320 * 5) + (0.3872 * StyxWoW.Me.AttackPower * 5)) *
                        Common.DamageMultiplier()) * (1 + mastery);
            }
        }

        public static double rip_calc
        {
            get
            {
                double mastery = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetMasteryEffect()", 0) / 100;
                return ((113 + (320 * combo_points) + (0.3872 * StyxWoW.Me.AttackPower * combo_points)) *
                        Common.DamageMultiplier()) * (1 + mastery);
            }
        }

        public static double rake_calc
        {
            get
            {
                double mastery = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetMasteryEffect()", 0) / 100;
                return ((118 + (StyxWoW.Me.AttackPower * 0.368)) * Common.DamageMultiplier()) * (1 + mastery);
            }
        }

        protected static double rip_tick_multiplier
        {
            get
            {
                if (!_rip_tick_multiplier.HasValue)
                {
                    _rip_tick_multiplier = rip_calc;
                    return _rip_tick_multiplier.Value;
                }
                return _rip_tick_multiplier.Value;
            }
        }

        protected static double rake_tick_multiplier
        {
            get
            {
                if (!_rake_tick_multiplier.HasValue)
                {
                    _rake_tick_multiplier = rake_calc;
                    return _rake_tick_multiplier.Value;
                }
                return _rake_tick_multiplier.Value;
            }
        }

        protected static double rip_ratio
        {
            get
            {
                if (!_rip_ratio.HasValue)
                {
                    if (!dot.rip.ticking)
                    {
                        _rip_ratio = 2;
                        return _rip_ratio.Value;
                    }
                    _rip_ratio = rip_tick_multiplier / dot.rip.multiplier;
                    return _rip_ratio.Value;
                }
                return _rip_ratio.Value;
            }
        }

        protected static double rake_ratio
        {
            get
            {
                if (!_rake_ratio.HasValue)
                {
                    if (!dot.rake.ticking)
                    {
                        _rake_ratio = 2;
                        return _rake_ratio.Value;
                    }
                    _rake_ratio = rake_tick_multiplier / dot.rake.multiplier;
                    return _rake_ratio.Value;
                }
                return _rake_ratio.Value;
            }
        }

        protected static double time_til_bitw
        {
            get
            {
                if (!_time_til_bitw.HasValue)
                {
                    _time_til_bitw = (target.time_to_die * (target.health.pct - 25) / target.health.pct);
                    return _time_til_bitw.Value;
                }
                return _time_til_bitw.Value;
            }
        }

        //TODO:  Implement this time tracker.
        // Time -> Seconds we are actually in combat.
        protected static double time
        {
            get
            {
                if (!_time.HasValue)
                {
                    return _time.Value;
                }
                return _time.Value;
            }
        }

        protected static double rip_remains
        {
            get
            {
                if (!_rip_remains.HasValue)
                {
                    if (!StyxWoW.Me.CurrentTarget.HasMyAura("Rip"))
                    {
                        _rip_remains = 0;
                        return _rip_remains.Value;
                    }
                    if (ExtendedRip <= 2)
                        _rip_remains = (dot.rip.remains + (6 - (ExtendedRip * 2)));
                    if (ExtendedRip > 2)
                        _rip_remains = (dot.rip.remains);
                    return _rip_remains != null ? _rip_remains.Value : 0;
                }
                return _rip_remains.Value;
            }
        }

        protected static double ExtendedRip
        {
            get { return _ExtendedRip; }
        }

        public static double combo_points
        {
            get
            {
                if (!_combo_points.HasValue)
                {
                    //_combo_points = StyxWoW.Me.ComboPoints;
                    _combo_points = Styx.WoWInternals.Lua.GetReturnVal<int>("return GetComboPoints(\"player\",\"target\");", 0);
                    return _combo_points.Value;
                }
                return _combo_points.Value;
            }
        }

        protected static double energy
        {
            get
            {
                if (!_energy.HasValue)
                {
                    _energy = Styx.WoWInternals.Lua.GetReturnVal<int>("return UnitPower(\"player\");", 0);
                    return _energy.Value;
                }
                return _energy.Value;
            }
        }

        protected static double EnergyRegen
        {
            get
            {
                if (!_EnergyRegen.HasValue)
                {
                    _EnergyRegen = Styx.WoWInternals.Lua.GetReturnVal<float>("return GetPowerRegen()", 1);
                    return _EnergyRegen.Value;
                }
                return _EnergyRegen.Value;
            }
        }

        protected static double time_to_max
        {
            get
            {
                if (!_time_to_max.HasValue)
                {
                    _time_to_max = (100 - energy) * (1.0 / EnergyRegen);
                    return _time_to_max.Value;
                }
                return _time_to_max.Value;
            }
        }

        protected static double cast_time
        {
            get
            {
                if (!_cast_time.HasValue)
                {
                    _cast_time = SpellManager.Spells["Wrath"].CastTime * 0.001;
                    return _cast_time.Value;
                }
                return _cast_time.Value;
            }
        }

        #endregion Calculations

        private static Composite Filler()
        {
            return new PrioritySelector(
                new Decorator(ret => dot.thrash_cat.remains < 3 && target.time_to_die >= 6 && combo_points >= 5,
                              thrash_cat()),
                ravage(),
                new Decorator(ret => buff.omen_of_clarity.react && buff.king_of_the_jungle.down, shred()),
                new Decorator(
                    ret =>
                    ((combo_points < 5 && dot.rip.remains < 3.0) || (combo_points == 0 && buff.savage_roar.remains < 2)) &&
                    buff.king_of_the_jungle.down, mangle()),
                new Decorator(ret => buff.king_of_the_jungle.down, shred())
                );
        }

        private static Composite doc()
        {
            return new PrioritySelector(
                auto_attack(),
                Spell.Cast("Healing Touch", ret => Me.HasAura(69369) && Spell.GetAuraTimeLeft(69369) <= 1.5 && !Me.HasAura(108381)),
                new Decorator(ret => buff.savage_roar.down, savage_roar()),
                new Decorator(ret => debuff.weakened_armor.stack < 3, faerie_fire()),
                Spell.Cast("Healing Touch", ret => Me.HasAura(69369) && combo_points >= 4 && Me.GetAuraById(108381)!=null && Me.GetAuraById(108381).StackCount < 2),
                Spell.Cast("Healing Touch", ret => Me.HasAura(132158)),
                new Decorator(ret => talent.incarnation.enabled && energy <= 35 && !buff.omen_of_clarity.react && cooldown.tigers_fury.remains == 0 && cooldown.berserk.remains == 0, incarnation()),
                new Decorator(ret => buff.tigers_fury.up, Item.HandleItems()),
                new Decorator(ret => (energy <= 35 && !buff.omen_of_clarity.react) || buff.king_of_the_jungle.up, tigers_fury()),
                new Decorator(ret => buff.tigers_fury.up || (target.time_to_die < 18 && cooldown.tigers_fury.remains > 6), berserk()),
                new Decorator(ret => combo_points >= 1 && dot.rip.ticking && dot.rip.remains <= 3 && target.health.pct <= 25,ferocious_bite()),
                new Decorator(ret => target.time_to_die >= 6 && buff.omen_of_clarity.react && dot.thrash_cat.remains < 3, thrash_cat()),
                new Decorator(ret => (target.time_to_die <= 4 && combo_points >= 5) || (target.time_to_die <= 1 && combo_points >= 3), ferocious_bite()),
                new Decorator(ret => buff.savage_roar.remains <= 3 && combo_points > 0 && target.health.pct < 25, savage_roar()),
                new Decorator(ret => (combo_points >= 5 && (time_til_bitw) < 15 && buff.rune_of_reorigination.up && buff.dream_of_cenarius_damage.up) || target.time_to_die <= 40, virmens_bite_potion()),
                new Decorator(ret => talent.natures_swiftness.enabled && buff.dream_of_cenarius_damage.down && buff.predatory_swiftness.down && combo_points >= 5 && (rip_ratio) >= 0.92 && target.time_to_die > 30, natures_swiftness()),
                new Decorator(ret => combo_points >= 5 && (rip_ratio) >= 1.15 && target.time_to_die > 30, rip()),
                new Decorator(ret => talent.natures_swiftness.enabled && buff.dream_of_cenarius_damage.down && buff.predatory_swiftness.down && combo_points >= 5 && target.health.pct <= 25, natures_swiftness()),
                new Decorator(ret => combo_points >= 5 && dot.rip.ticking && target.health.pct <= 25 && ((energy < 50 && buff.berserk.down) || (energy < 25 && buff.berserk.remains > 1)),
                    new ActionAlwaysSucceed()),
                new Decorator(ret => combo_points >= 5 && dot.rip.ticking && target.health.pct <= 25, ferocious_bite()),
                new Decorator(ret => combo_points >= 5 && target.time_to_die >= 6 && dot.rip.remains < 2.0 && buff.dream_of_cenarius_damage.up, rip()),
                new Decorator(ret => combo_points >= 5 && target.time_to_die >= 6 && dot.rip.remains < 6.0 && buff.dream_of_cenarius_damage.up && (rip_ratio) >= 1, rip()),
                new Decorator(ret => talent.natures_swiftness.enabled && buff.dream_of_cenarius_damage.down && buff.predatory_swiftness.down && combo_points >= 5 && dot.rip.remains < 3 && (buff.berserk.up || dot.rip.remains + 1.9 <= cooldown.tigers_fury.remains) && target.health.pct > 25, natures_swiftness()),
                new Decorator(ret => combo_points >= 5 && target.time_to_die >= 6 && dot.rip.remains < 2.0 && (buff.berserk.up || dot.rip.remains + 1.9 <= cooldown.tigers_fury.remains), rip()),
                new Decorator(
                    ret =>
                    buff.savage_roar.remains <= 3 && combo_points > 0 && buff.savage_roar.remains + 2 > dot.rip.remains,
                    savage_roar()),
                new Decorator(
                    ret =>
                    buff.savage_roar.remains <= 6 && combo_points >= 5 && buff.savage_roar.remains + 2 <= (rip_remains),
                    savage_roar()),
                new Decorator(
                    ret =>
                    combo_points >= 5 &&
                    ((energy < 50 && buff.berserk.down) || (energy < 25 && buff.berserk.remains > 1)) &&
                    buff.savage_roar.remains - 6 >= (rip_remains) && (rip_remains) >= 4.5, new ActionAlwaysSucceed()),
                new Decorator(
                    ret =>
                    combo_points >= 5 &&
                    ((energy < 50 && buff.berserk.down) || (energy < 25 && buff.berserk.remains > 1)) &&
                    buff.savage_roar.remains + 6 >= (rip_remains) && (rip_remains) >= 6.5, new ActionAlwaysSucceed()),
                new Decorator(
                    ret => combo_points >= 5 && buff.savage_roar.remains - 6 >= (rip_remains) && (rip_remains) >= 4,
                    ferocious_bite()),
                new Decorator(
                    ret => combo_points >= 5 && buff.savage_roar.remains + 6 >= (rip_remains) && (rip_remains) >= 6,
                    ferocious_bite()),
                new Decorator(
                    ret =>
                    combo_points >= 5 &&
                    ((energy < 50 && buff.berserk.down) || (energy < 25 && buff.berserk.remains > 1)) &&
                    (rip_remains) >= 10.5, new ActionAlwaysSucceed()),
                new Decorator(ret => combo_points >= 5 && (rip_remains) >= 10, ferocious_bite()),
                new Decorator(ret => buff.rune_of_reorigination.up && (rake_ratio) >= 1, rake()),
                new Decorator(
                    ret =>
                    buff.rune_of_reorigination.up && dot.rake.remains < 9 && (buff.rune_of_reorigination.remains <= 1.5),
                    rake()),
                new Decorator(
                    ret =>
                    target.time_to_die - dot.rake.remains > 3 && dot.rake.remains < 6.0 &&
                    buff.dream_of_cenarius_damage.up && (rake_ratio) >= 1, rake()),
                new Decorator(ret => target.time_to_die - dot.rake.remains > 3 && (rake_ratio) > 1.12, rake()),
                new Decorator(
                    ret =>
                    target.time_to_die - dot.rake.remains > 3 && dot.rake.remains < 3.0 &&
                    (buff.berserk.up || (cooldown.tigers_fury.remains + 0.8) >= dot.rake.remains || energy > 60), rake()),

                //TODO: actions.doc+=/pool_resource,wait=0.1,for_next=1
                new Decorator(
                    ret =>
                    target.time_to_die >= 6 && dot.thrash_cat.remains < 3 && ((rip_remains) >= 4 || buff.berserk.up),
                    thrash_cat()),
                new Decorator(ret => buff.omen_of_clarity.react, Filler()),
                new Decorator(
                    ret =>
                    ((combo_points < 5 && dot.rip.remains < 3.0) || (combo_points == 0 && buff.savage_roar.remains < 2)),
                    Filler()),
                new Decorator(ret => buff.predatory_swiftness.remains > 1, Filler()),
                new Decorator(ret => target.time_to_die <= 8.5, Filler()),
                new Decorator(ret => (buff.tigers_fury.up || buff.berserk.up || buff.natures_vigil.up), Filler()),
                new Decorator(ret => cooldown.tigers_fury.remains <= 3.0, Filler()),
                new Decorator(ret => time_to_max <= 1.0, Filler()),
                new Decorator(ret => talent.force_of_nature.enabled, treants())
                );
        }

        private static Composite nondoc()
        {
            return new PrioritySelector(
                auto_attack(),
                new Decorator(ret => buff.heart_of_the_wild.up, Item.HandleItems()),
                new Decorator(ret => talent.heart_of_the_wild.enabled /*&& time < 10*/, heart_of_the_wild()),
                new Decorator(ret => buff.heart_of_the_wild.up || target.time_to_die <= 40, virmens_bite_potion()),
                new Decorator(ret => cast_time < buff.heart_of_the_wild.remains, wrath()),
                new Decorator(ret => buff.cat_form.down, cat_form()),
                new Decorator(ret => buff.savage_roar.down, savage_roar()),
                new Decorator(ret => debuff.weakened_armor.stack < 3, faerie_fire()),
                new Decorator(
                    ret =>
                    talent.incarnation.enabled && energy <= 35 && !buff.omen_of_clarity.react &&
                    cooldown.tigers_fury.remains == 0 && cooldown.berserk.remains == 0, incarnation()),
                new Decorator(ret => buff.tigers_fury.up, Item.HandleItems()),
                new Decorator(ret => (energy <= 35 && !buff.omen_of_clarity.react) || buff.king_of_the_jungle.up,
                              tigers_fury()),
                new Decorator(
                    ret => buff.tigers_fury.up || (target.time_to_die < 18 && cooldown.tigers_fury.remains > 6),
                    berserk()),
                new Decorator(
                    ret =>
                    talent.natures_vigil.enabled &&
                    (buff.tigers_fury.up || (target.time_to_die < 35 && cooldown.tigers_fury.remains > 6)),
                    natures_vigil()),
                new Decorator(
                    ret => combo_points >= 1 && dot.rip.ticking && dot.rip.remains <= 2 && target.health.pct <= 25,
                    ferocious_bite()),
                new Decorator(
                    ret => target.time_to_die >= 6 && buff.omen_of_clarity.react && dot.thrash_cat.remains < 3,
                    thrash_cat()),
                new Decorator(
                    ret =>
                    (target.time_to_die <= 4 && combo_points >= 5) || (target.time_to_die <= 1 && combo_points >= 3),
                    ferocious_bite()),
                new Decorator(ret => buff.savage_roar.remains <= 3 && combo_points > 0 && target.health.pct < 25,
                              savage_roar()),
                new Decorator(
                    ret =>
                    (combo_points >= 5 && (time_til_bitw) < 15 && buff.rune_of_reorigination.up) ||
                    target.time_to_die <= 40, virmens_bite_potion()),
                new Decorator(ret => combo_points >= 5 && (rip_ratio) >= 1.15 && target.time_to_die > 30, rip()),
                new Decorator(ret => combo_points >= 5 && dot.rip.ticking && target.health.pct <= 25, ferocious_bite()),
                new Decorator(
                    ret =>
                    combo_points >= 5 && target.time_to_die >= 6 && dot.rip.remains < 2.0 &&
                    (buff.berserk.up || dot.rip.remains + 1.9 <= cooldown.tigers_fury.remains), rip()),
                new Decorator(
                    ret =>
                    buff.savage_roar.remains <= 3 && combo_points > 0 && buff.savage_roar.remains + 2 > (rip_remains),
                    savage_roar()),
                new Decorator(
                    ret =>
                    buff.savage_roar.remains <= 6 && combo_points >= 5 && buff.savage_roar.remains + 2 <= (rip_remains),
                    savage_roar()),
                new Decorator(
                    ret =>
                    combo_points >= 5 && ((rip_remains) > 10 || ((rip_remains) > 6 && buff.berserk.up)) &&
                    dot.rip.ticking, ferocious_bite()),
                new Decorator(ret => buff.rune_of_reorigination.up && (rake_ratio) >= 1, rake()),
                new Decorator(
                    ret =>
                    buff.rune_of_reorigination.up && dot.rake.remains < 9 && (buff.rune_of_reorigination.remains <= 1.5),
                    rake()),
                new Decorator(ret => target.time_to_die - dot.rake.remains > 3 && (rake_ratio) > 1.12, rake()),
                new Decorator(
                    ret =>
                    target.time_to_die - dot.rake.remains > 3 && dot.rake.remains < 3.0 &&
                    (buff.berserk.up || (cooldown.tigers_fury.remains + 0.8) >= dot.rake.remains || energy > 60), rake()),

                //TODO: actions.doc+=/pool_resource,wait=0.1,for_next=1
                new Decorator(
                    ret =>
                    dot.thrash_cat.remains < 3 && target.time_to_die >= 6 && ((rip_remains) >= 4 || buff.berserk.up),
                    thrash_cat()),
                new Decorator(ret => buff.omen_of_clarity.react, Filler()),
                new Decorator(
                    ret =>
                    ((combo_points < 5 && dot.rip.remains < 3.0) || (combo_points == 0 && buff.savage_roar.remains < 2)),
                    Filler()),
                new Decorator(ret => target.time_to_die <= 8.5, Filler()),
                new Decorator(ret => (buff.tigers_fury.up || buff.berserk.up || buff.natures_vigil.up), Filler()),
                new Decorator(ret => cooldown.tigers_fury.remains <= 3.0, Filler()),
                new Decorator(ret => time_to_max <= 1.0, Filler()),
                new Decorator(ret => talent.soul_of_the_forest.enabled && combo_points < 5, Filler()),
                new Decorator(ret => talent.force_of_nature.enabled, treants()),
                new Decorator(
                    ret => talent.natures_swiftness.enabled && buff.natures_vigil.up && !buff.predatory_swiftness.up,
                    natures_swiftness()),
                new Decorator(
                    ret =>
                    buff.natures_vigil.up && (buff.predatory_swiftness.up || buff.natures_swiftness.up) &&
                    !buff.berserk.up, healing_touch())
                );
        }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                Spell.Cast("Cat Form",ret => StyxWoW.Me.Shapeshift != ShapeshiftForm.Cat),
                new Decorator(ret => TalentManager.HasTalent(17), doc()),
                new Decorator(ret => !TalentManager.HasTalent(17), nondoc())
                );
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(

                new Decorator(ret => buff.savage_roar.remains <= 1 &&
                                     (TalentManager.HasGlyph("Savagery") || StyxWoW.Me.RawComboPoints > 0),
                              savage_roar()),
                new Decorator(ret => buff.savage_roar.remains <= 3 && combo_points > 0, savage_roar()),
                new Decorator(ret => energy <= 35 && !buff.omen_of_clarity.react, tigers_fury()),
                new Decorator(ret => buff.tigers_fury.up, berserk()),
                new Decorator(ret => buff.tigers_fury.up && dot.thrash_cat.remains < 9, thrash_cat()),
                new Decorator(ret => dot.thrash_cat.remains < 3, thrash_cat()),
                new Decorator(ret => buff.savage_roar.remains < 9 && combo_points == 5, savage_roar()),
                new Decorator(ret => combo_points == 5 && target.time_to_die >= 6, rip()),

                //THIS CAN ONLY BE DONE IF AUTOMATIC TARGETING IS ALLOWED
                /*new Decorator(ret => Unit.NearbyAttackableUnits(StyxWoW.Me.Location, 10f).Count() < 8,
                              new PrioritySelector(
                                  HandleMultiDoT(Debuffs))),*/
                new Decorator(
                    ret => !dot.rake.ticking && Unit.NearbyAttackableUnits(StyxWoW.Me.Location, 10f).Count() < 8, rake()),
                new Decorator(ret => buff.savage_roar.remains <= 5, swipe()),
                new Decorator(ret => buff.tigers_fury.up || buff.berserk.up, swipe()),
                new Decorator(ret => cooldown.tigers_fury.remains < 3, swipe()),
                new Decorator(ret => buff.omen_of_clarity.react, swipe()),
                new Decorator(ret => time_to_max <= 1, swipe())
                );
        }

        #region Spells

        private static Composite swipe()
        {
            return Spell.Cast("Swipe");
        }

        private static Composite virmens_bite_potion()
        {
            return new PrioritySelector(
                new Decorator(ret => PRSettings.Instance.UsePotion && !buff.virmens_bite_potion.up, Item.UseItem(76089))
                );
        }

        private static Composite auto_attack()
        {
            return new PrioritySelector(
                new Decorator(
                    ret => !StyxWoW.Me.IsAutoAttacking,
                    new Action(ret =>
                        {
                            Styx.WoWInternals.Lua.DoString("StartAttack()");
                            return RunStatus.Failure;
                        }))
                );
        }

        private static Composite shred()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.Specialization == WoWSpec.DruidGuardian, mangle()),
                new Decorator(ret => StyxWoW.Me.CurrentTarget.MeIsBehind ||
                                     StyxWoW.Me.CurrentTarget.IsShredBoss() ||
                                     TalentManager.HasGlyph("Shred") &&
                                     (StyxWoW.Me.CachedHasAura(5217) ||
                                      StyxWoW.Me.CachedHasAura(106951)), Spell.Cast("Shred")),
                new Decorator(ret => !(StyxWoW.Me.CurrentTarget.MeIsBehind ||
                                       StyxWoW.Me.CurrentTarget.IsShredBoss() ||
                                       TalentManager.HasGlyph("Shred") &&
                                       (StyxWoW.Me.CachedHasAura(5217) ||
                                        StyxWoW.Me.CachedHasAura(106951))), mangle()),
                new ActionAlwaysFail());
        }

        private static Composite ravage()
        {
            return new PrioritySelector(
                new Decorator(ret => (StyxWoW.Me.CachedHasAura("Incarnation: King of the Jungle") ||
                                      (StyxWoW.Me.CachedHasAura("Prowl") &&
                                       StyxWoW.Me.CurrentTarget.MeIsBehind)), Spell.Cast("Ravage")),
                new ActionAlwaysFail()
                );
        }

        private static Composite mangle()
        {
            return Spell.Cast("Mangle");
        }

        private static Composite rip()
        {
            return Spell.Cast("Rip");
        }

        private static Composite natures_swiftness()
        {
            return new PrioritySelector(
                new Decorator(ret => !StyxWoW.Me.CachedHasAura(132158), Spell.Cast("Nature's Swiftness")),
                new ActionAlwaysFail()
                );
        }

        private static Composite thrash_cat()
        {
            return Spell.Cast(106832);
        }

        private static Composite ferocious_bite()
        {
            return new PrioritySelector(
                new Decorator(ret => _combo_points < 1, new ActionAlwaysFail()),
                Spell.Cast("Ferocious Bite")
                );
        }

        private static Composite berserk()
        {
            return Spell.Cast("Berserk", ret => PRSettings.Instance.UseCooldowns && StyxWoW.Me.CurrentTarget.IsBoss());
        }

        private static Composite tigers_fury()
        {
            return Spell.Cast("Tiger's Fury");
        }

        private static Composite incarnation()
        {
            return new PrioritySelector(
                new Decorator(ret => !Spell.SpellOnCooldown(102543) && PRSettings.Instance.UseCooldowns && StyxWoW.Me.CurrentTarget.IsBoss(),
                                  new Action(baum => SpellManager.Cast(102543))),
                                  new ActionAlwaysSucceed()
                );
        }

        private static Composite faerie_fire()
        {
            return Spell.Cast("Faerie Fire",
                              ret =>
                              !TalentManager.HasTalent(7) ||
                              (TalentManager.HasTalent(7) &&
                               !Spell.SpellOnCooldown(102355)));
        }

        private static Composite savage_roar()
        {
            return new PrioritySelector(
                new Decorator(ret => !TalentManager.HasGlyph("Savagery") && StyxWoW.Me.RawComboPoints < 1,
                              new ActionAlwaysFail()),
                new Decorator(ret => TalentManager.HasGlyph("Savagery"),
                              new Action(ret => SpellManager.Cast(127538))),
                new Decorator(ret => !TalentManager.HasGlyph("Savagery"),
                              new Action(ret => SpellManager.Cast(52610))));
        }

        private static Composite healing_touch()
        {
            return Spell.Cast("Healing Touch");
        }

        private static Composite cat_form()
        {
            return Spell.Cast("Cat Form");
        }

        private static Composite wrath()
        {
            return Spell.Cast("Wrath");
        }

        private static Composite heart_of_the_wild()
        {
            return Spell.Cast("Heart of the Wild", ret => PRSettings.Instance.UseCooldowns && StyxWoW.Me.CurrentTarget.IsBoss());
        }

        private static Composite natures_vigil()
        {
            return Spell.Cast("Nature's Vigil", ret => PRSettings.Instance.UseCooldowns && StyxWoW.Me.CurrentTarget.IsBoss());
        }

        private static Composite treants()
        {
            return Spell.CastOnGround("Force of Nature", ret => StyxWoW.Me.Location, ret => StyxWoW.Me.CurrentTarget.IsBoss());
        }

        private static Composite rake()
        {
            return Spell.Cast("Rake");
        }

        #endregion Spells

        #region MultiDoT Targets

        private static readonly string[] Debuffs = new[]
            {
                "Rake"
            };

        private static Composite HandleMultiDoT(string[] spellNames)
        {
            return new PrioritySelector(
                ctx => GetMultiDoTTarget(spellNames),
                Spell.Cast("Rake", onUnit => ((WoWUnit)onUnit),
                           ret =>
                           ret != null && !((WoWUnit)ret).HasMyAura("Rake") &&
                           DpsMeter.GetCombatTimeLeft(((WoWUnit)ret)).TotalSeconds > 6));
        }

        private static WoWUnit GetMultiDoTTarget(string[] debuffs)
        {
            return Me.CurrentTarget != null
                       ? Unit.AttackableMeleeUnits.FirstOrDefault(
                           x =>
                           (x.IsTargetingMyPartyMember || x.IsTargetingMeOrPet || x.IsTargetingAnyMinion ||
                            x.IsTargetingMyRaidMember || x.IsTargetingPet) &&
                           !x.HasAllAuras(debuffs))
                       : null; //!x.IsBoss() &&
        }

        private static RunStatus resetVariables()
        {
            _rake_tick_multiplier = null;
            _rip_tick_multiplier = null;
            _rip_remains = null;
            _combo_points = null;
            _energy = null;
            _time_to_max = null;
            _EnergyRegen = null;
            _cast_time = null;
            _debuff_weakened_armor_stack = null;
            _time_to_die = null;
            _pct = null;
            _cooldown_berserk_remains = null;
            _cooldown_tigers_fury_remains = null;
            _dot_rake_remains = null;
            _dot_rip_ticking = null;
            _dot_rip_remains = null;
            _dot_thrash_cat_remains = null;
            _buff_berserk_up = null;
            _buff_berserk_down = null;
            _buff_berserk_remains = null;
            _buff_cat_form_down = null;
            _buff_dream_of_cenarius_damage_down = null;
            _buff_dream_of_cenarius_damage_up = null;
            _buff_dream_of_cenarius_damage_stack = null;
            _buff_heart_of_the_wild_remains = null;
            _buff_heart_of_the_wild_up = null;
            _buff_king_of_the_jungle_up = null;
            _buff_king_of_the_jungle_down = null;
            _buff_natures_swiftness_up = null;
            _buff_natures_vigil_up = null;
            _buff_omen_of_clarity_react = null;
            _buff_predatory_swiftness_up = null;
            _buff_predatory_swiftness_down = null;
            _buff_predatory_swiftness_remains = null;
            _buff_savage_roar_remains = null;
            _buff_savage_roar_down = null;
            _buff_savage_roar_up = null;
            _buff_tigers_fury_up = null;
            _buff_virmens_bite_potion_up = null;
            _rip_ratio = null;
            _rake_ratio = null;
            _time_til_bitw = null;
            _time = null;
            _buff_rune_of_reorigination_up = null;
            _buff_rune_of_reorigination_remains = null;
            _dot_rake_ticking = null;
            return RunStatus.Failure;
        }

        #endregion MultiDoT Targets

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1609 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.DruidFeral; }
        }

        public override string Name
        {
            get { return "Druid Feral"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    Racials.UseRacials(),
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    new Action(context => resetVariables()),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(
                                          ret =>
                                          PRSettings.Instance.UseAoEAbilities &&
                                          Unit.NearbyAttackableUnits(Me.Location, 10f).Count() >
                                          PRSettings.Instance.Druid.AoECount,
                                          HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(
                                          ret =>
                                          PRSettings.Instance.UseAoEAbilities &&
                                          Unit.NearbyAttackableUnits(Me.Location, 10f).Count() >
                                          PRSettings.Instance.Druid.AoECount,
                                          HandleAoeCombat()),
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))));
            }
        }

        public override Composite PVPRotation
        {
            get { return PVERotation; }
        }

        public override Composite Medic
        {
            get { return null; }
        }

        public override Composite PreCombat
        {
            get { return null; }
        }

        internal override void OnPulse()
        {
            if (StyxWoW.Me.CurrentTarget != null)
                DpsMeter.Update();
        }

        #endregion Overrides of RotationBase
    }

    #region Class's

    internal class talent
    {
        public class force_of_nature
        {
            public static bool enabled
            {
                get { return TalentManager.HasTalent(12); }
            }
        }

        public class heart_of_the_wild
        {
            public static bool enabled
            {
                get { return TalentManager.HasTalent(16); }
            }
        }

        public class incarnation
        {
            public static bool enabled
            {
                get { return TalentManager.HasTalent(11); }
            }
        }

        public class natures_swiftness
        {
            public static bool enabled
            {
                get { return TalentManager.HasTalent(4); }
            }
        }

        public class natures_vigil
        {
            public static bool enabled
            {
                get { return TalentManager.HasTalent(18); }
            }
        }

        public class soul_of_the_forest
        {
            public static bool enabled
            {
                get { return TalentManager.HasTalent(10); }
            }
        }
    }

    internal class debuff
    {
        public class weakened_armor
        {
            public static double stack
            {
                get
                {
                    if (!Feral._debuff_weakened_armor_stack.HasValue)
                    {
                        Feral._debuff_weakened_armor_stack = Spell.CachedStackCount(StyxWoW.Me.CurrentTarget, "Weakened Armor", true);
                        return Feral._debuff_weakened_armor_stack.Value;
                    }
                    return Feral._debuff_weakened_armor_stack.Value;
                }
            }
        }
    }

    internal class target
    {
        public static double time_to_die
        {
            get
            {
                if (!Feral._time_to_die.HasValue)
                {
                    Feral._time_to_die = DpsMeter.GetCombatTimeLeft(StyxWoW.Me.CurrentTarget).TotalSeconds;
                    return Feral._time_to_die.Value;
                }
                return Feral._time_to_die.Value;
            }
        }

        public class health
        {
            public static double pct
            {
                get
                {
                    if (!Feral._pct.HasValue)
                    {
                        Feral._pct = StyxWoW.Me.CurrentTarget.HealthPercent;
                        return Feral._pct.Value;
                    }
                    return Feral._pct.Value;
                }
            }
        }
    }

    internal class cooldown
    {
        public class berserk
        {
            public static double remains
            {
                get
                {
                    if (!Feral._cooldown_berserk_remains.HasValue)
                    {
                        Feral._cooldown_berserk_remains = Spell.GetSpellCooldown("Berserk").TotalSeconds;
                        return Feral._cooldown_berserk_remains.Value;
                    }
                    return Feral._cooldown_berserk_remains.Value;
                }
            }
        }

        public class tigers_fury
        {
            public static double remains
            {
                get
                {
                    if (!Feral._cooldown_tigers_fury_remains.HasValue)
                    {
                        Feral._cooldown_tigers_fury_remains =
                            Spell.GetSpellCooldown("Tiger's Fury").TotalSeconds;
                        return Feral._cooldown_tigers_fury_remains.Value;
                    }
                    return Feral._cooldown_tigers_fury_remains.Value;
                }
            }
        }
    }

    internal class dot
    {
        public class rake
        {
            public static double remains
            {
                get
                {
                    if (!Feral._dot_rake_remains.HasValue)
                    {
                        Feral._dot_rake_remains = Spell.GetMyAuraTimeLeft("Rake", StyxWoW.Me.CurrentTarget);
                        return Feral._dot_rake_remains.Value;
                    }
                    return Feral._dot_rake_remains.Value;
                }
            }

            public static double multiplier
            {
                get { return Feral._dot_rake_multiplier; }
            }

            public static bool ticking
            {
                get
                {
                    if (!Feral._dot_rake_ticking.HasValue)
                    {
                        Feral._dot_rake_ticking = StyxWoW.Me.CurrentTarget.HasMyAura("Rake");
                        return Feral._dot_rake_ticking.Value;
                    }
                    return Feral._dot_rake_ticking.Value;
                }
            }
        }

        public class rip
        {
            public static bool ticking
            {
                get
                {
                    if (!Feral._dot_rip_ticking.HasValue)
                    {
                        Feral._dot_rip_ticking = StyxWoW.Me.CurrentTarget.HasMyAura("Rip");
                        return Feral._dot_rip_ticking.Value;
                    }
                    return Feral._dot_rip_ticking.Value;
                }
            }

            public static double remains
            {
                get
                {
                    if (!Feral._dot_rip_remains.HasValue)
                    {
                        Feral._dot_rip_remains = Spell.GetMyAuraTimeLeft("Rip", StyxWoW.Me.CurrentTarget);
                        return Feral._dot_rip_remains.Value;
                    }
                    return Feral._dot_rip_remains.Value;
                }
            }

            public static double multiplier
            {
                get { return !ticking ? 0 : Feral._dot_rip_multiplier; }
            }
        }

        public class thrash_cat
        {
            public static double remains
            {
                get
                {
                    if (!Feral._dot_thrash_cat_remains.HasValue)
                    {
                        Feral._dot_thrash_cat_remains = Spell.GetMyAuraTimeLeft("Thrash", StyxWoW.Me.CurrentTarget);
                        return Feral._dot_thrash_cat_remains.Value;
                    }
                    return Feral._dot_thrash_cat_remains.Value;
                }
            }
        }
    }

    /***** HERE WE GO*/
    /////////

    internal class buff
    {
        public class berserk
        {
            public static bool up
            {
                get
                {
                    if (!Feral._buff_berserk_up.HasValue)
                    {
                        Feral._buff_berserk_up = StyxWoW.Me.CachedHasAura(106951);
                        return Feral._buff_berserk_up.Value;
                    }
                    return Feral._buff_berserk_up.Value;
                }
            }

            public static bool down
            {
                get
                {
                    if (!Feral._buff_berserk_down.HasValue)
                    {
                        Feral._buff_berserk_down = !StyxWoW.Me.CachedHasAura(106951);
                        return Feral._buff_berserk_down.Value;
                    }
                    return Feral._buff_berserk_down.Value;
                }
            }

            public static double remains
            {
                get
                {
                    if (!Feral._buff_berserk_remains.HasValue)
                    {
                        Feral._buff_berserk_remains = Spell.GetAuraTimeLeft(106951);
                        return Feral._buff_berserk_remains.Value;
                    }
                    return Feral._buff_berserk_remains.Value;
                }
            }
        }

        public class cat_form
        {
            public static bool down
            {
                get
                {
                    if (!Feral._buff_cat_form_down.HasValue)
                    {
                        Feral._buff_cat_form_down = StyxWoW.Me.Shapeshift != ShapeshiftForm.Cat;
                        return Feral._buff_cat_form_down.Value;
                    }
                    return Feral._buff_cat_form_down.Value;
                }
            }
        }

        public class dream_of_cenarius_damage
        {
            public static bool down
            {
                get
                {
                    if (!Feral._buff_dream_of_cenarius_damage_down.HasValue)
                    {
                        Feral._buff_dream_of_cenarius_damage_down = !StyxWoW.Me.CachedHasAura(108381);
                        return Feral._buff_dream_of_cenarius_damage_down.Value;
                    }
                    return Feral._buff_dream_of_cenarius_damage_down.Value;
                }
            }

            public static double stack
            {
                get
                {
                    if (!Feral._buff_dream_of_cenarius_damage_stack.HasValue)
                    {
                        Feral._buff_dream_of_cenarius_damage_stack = StyxWoW.Me.CachedStackCount(108381);
                        return Feral._buff_dream_of_cenarius_damage_stack.Value;
                    }
                    return Feral._buff_dream_of_cenarius_damage_stack.Value;
                }
            }

            public static bool up
            {
                get
                {
                    if (!Feral._buff_dream_of_cenarius_damage_up.HasValue)
                    {
                        Feral._buff_dream_of_cenarius_damage_up = StyxWoW.Me.CachedHasAura(108381);
                        return Feral._buff_dream_of_cenarius_damage_up.Value;
                    }
                    return Feral._buff_dream_of_cenarius_damage_up.Value;
                }
            }
        }

        public class heart_of_the_wild
        {
            public static bool up
            {
                get
                {
                    if (!Feral._buff_heart_of_the_wild_up.HasValue)
                    {
                        Feral._buff_heart_of_the_wild_up = StyxWoW.Me.CachedHasAura(108292);
                        return Feral._buff_heart_of_the_wild_up.Value;
                    }
                    return Feral._buff_heart_of_the_wild_up.Value;
                }
            }

            public static double remains
            {
                get
                {
                    if (!Feral._buff_heart_of_the_wild_remains.HasValue)
                    {
                        Feral._buff_heart_of_the_wild_remains = Spell.GetAuraTimeLeft(108292);
                        return Feral._buff_heart_of_the_wild_remains.Value;
                    }
                    return Feral._buff_heart_of_the_wild_remains.Value;
                }
            }
        }

        public class king_of_the_jungle
        {
            public static bool up
            {
                get
                {
                    if (!Feral._buff_king_of_the_jungle_up.HasValue)
                    {
                        Feral._buff_king_of_the_jungle_up = StyxWoW.Me.CachedHasAura("Incarnation: King of the Jungle");
                        return Feral._buff_king_of_the_jungle_up.Value;
                    }
                    return Feral._buff_king_of_the_jungle_up.Value;
                }
            }

            public static bool down
            {
                get
                {
                    if (!Feral._buff_king_of_the_jungle_down.HasValue)
                    {
                        Feral._buff_king_of_the_jungle_down =
                            !StyxWoW.Me.CachedHasAura("Incarnation: King of the Jungle");
                        return Feral._buff_king_of_the_jungle_down.Value;
                    }
                    return Feral._buff_king_of_the_jungle_down.Value;
                }
            }
        }

        public class natures_swiftness
        {
            public static bool up
            {
                get
                {
                    if (!Feral._buff_natures_swiftness_up.HasValue)
                    {
                        Feral._buff_natures_swiftness_up = StyxWoW.Me.CachedHasAura(132158);
                        return Feral._buff_natures_swiftness_up.Value;
                    }
                    return Feral._buff_natures_swiftness_up.Value;
                }
            }
        }

        public class natures_vigil
        {
            public static bool up
            {
                get
                {
                    if (!Feral._buff_natures_vigil_up.HasValue)
                    {
                        Feral._buff_natures_vigil_up = StyxWoW.Me.CachedHasAura(124974);
                        return Feral._buff_natures_vigil_up.Value;
                    }
                    return Feral._buff_natures_vigil_up.Value;
                }
            }
        }

        public class omen_of_clarity
        {
            public static bool react
            {
                get
                {
                    if (!Feral._buff_omen_of_clarity_react.HasValue)
                    {
                        Feral._buff_omen_of_clarity_react = StyxWoW.Me.CachedHasAura(135700);
                        return Feral._buff_omen_of_clarity_react.Value;
                    }
                    return Feral._buff_omen_of_clarity_react.Value;
                }
            }
        }

        public class predatory_swiftness
        {
            public static bool up
            {
                get
                {
                    if (!Feral._buff_predatory_swiftness_up.HasValue)
                    {
                        Feral._buff_predatory_swiftness_up = StyxWoW.Me.CachedHasAura(69369);
                        return Feral._buff_predatory_swiftness_up.Value;
                    }
                    return Feral._buff_predatory_swiftness_up.Value;
                }
            }

            public static double remains
            {
                get
                {
                    if (!Feral._buff_predatory_swiftness_remains.HasValue)
                    {
                        Feral._buff_predatory_swiftness_remains = Spell.GetAuraTimeLeft(69369);
                        return Feral._buff_predatory_swiftness_remains.Value;
                    }
                    return Feral._buff_predatory_swiftness_remains.Value;
                }
            }

            public static bool down
            {
                get
                {
                    if (!Feral._buff_predatory_swiftness_down.HasValue)
                    {
                        Feral._buff_predatory_swiftness_down = !StyxWoW.Me.CachedHasAura(69369);
                        return Feral._buff_predatory_swiftness_down.Value;
                    }
                    return Feral._buff_predatory_swiftness_down.Value;
                }
            }
        }

        public class rune_of_reorigination
        {
            public static bool up
            {
                get
                {
                    if (!Feral._buff_rune_of_reorigination_up.HasValue)
                    {
                        Feral._buff_rune_of_reorigination_up = (StyxWoW.Me.CachedHasAura(139116) || StyxWoW.Me.CachedHasAura(139120));
                        return Feral._buff_rune_of_reorigination_up.Value;
                    }
                    return Feral._buff_rune_of_reorigination_up.Value;
                }
            }

            public static double remains
            {
                get
                {
                    if (!Feral._buff_rune_of_reorigination_remains.HasValue)
                    {

                        Feral._buff_rune_of_reorigination_remains = System.Math.Max(Spell.GetAuraTimeLeft(139116), Spell.GetAuraTimeLeft(139120));
                        return Feral._buff_rune_of_reorigination_remains.Value;
                    }
                    return Feral._buff_rune_of_reorigination_remains.Value;
                }
            }
        }

        public class savage_roar
        {
            public static double remains
            {
                get
                {
                    if (!Feral._buff_savage_roar_remains.HasValue)
                    {
                        Feral._buff_savage_roar_remains = TalentManager.HasGlyph("Savagery")
                                                              ? Spell.GetAuraTimeLeft(127538)
                                                              : Spell.GetAuraTimeLeft(52610);
                        return Feral._buff_savage_roar_remains.Value;
                    }
                    return Feral._buff_savage_roar_remains.Value;
                }
            }

            public static bool down
            {
                get
                {
                    if (!Feral._buff_savage_roar_down.HasValue)
                    {
                        Feral._buff_savage_roar_down = TalentManager.HasGlyph("Savagery")
                                                           ? !StyxWoW.Me.CachedHasAura(127538)
                                                           : !StyxWoW.Me.CachedHasAura(52610);
                        return Feral._buff_savage_roar_down.Value;
                    }
                    return Feral._buff_savage_roar_down.Value;
                }
            }

            public static bool up
            {
                get
                {
                    if (!Feral._buff_savage_roar_up.HasValue)
                    {
                        Feral._buff_savage_roar_up = TalentManager.HasGlyph("Savagery")
                                                         ? StyxWoW.Me.CachedHasAura(127538)
                                                         : StyxWoW.Me.CachedHasAura(52610);
                        return Feral._buff_savage_roar_up.Value;
                    }
                    return Feral._buff_savage_roar_up.Value;
                }
            }
        }

        public class tigers_fury
        {
            public static bool up
            {
                get
                {
                    if (!Feral._buff_tigers_fury_up.HasValue)
                    {
                        Feral._buff_tigers_fury_up = StyxWoW.Me.CachedHasAura(5217);
                        return Feral._buff_tigers_fury_up.Value;
                    }
                    return Feral._buff_tigers_fury_up.Value;
                }
            }
        }

        public class virmens_bite_potion
        {
            public static bool up
            {
                get
                {
                    if (!Feral._buff_virmens_bite_potion_up.HasValue)
                    {
                        Feral._buff_virmens_bite_potion_up = StyxWoW.Me.CachedHasAura(105697);
                        return Feral._buff_virmens_bite_potion_up.Value;
                    }
                    return Feral._buff_virmens_bite_potion_up.Value;
                }
            }
        }
    }

    #endregion Class's
}