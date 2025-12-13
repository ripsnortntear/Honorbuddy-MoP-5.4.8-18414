#region

using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    public partial class Superbad
    {
        private static void BearHandler()
        {
            if (bear_form())
                return;
            if (growl())
                return;
            if (BurstMode)
                DPSButtonPressed();

            DoCHealBear();
            ElusiveBrew();
            SpellReflection();
            BoneShield();
            use_item();

            if (SuperbadSettings.Instance.UseAoeKey)
            {
                if (AoeMode)
                    ThrashAoe();
                if (!AoeMode)
                    Sd();
                return;
            }

            if (_unitCount >= SuperbadSettings.Instance.BearAoe)
            {
                ThrashAoe();
                return;
            }
            Sd();
        }

        private static void Sd()
        {
            if (bear_hug())
                return;
            Maul();
            if (Frenzied_Regeneration())
                return;
            if (_rage > 59 && buff.savage_defense.down)
                savage_defense();
            berserk();
            if (talent.incarnation.enabled && cooldown.berserk.remains < 170 && cooldown.berserk.remains >= 30)
                incarnation();
            if (talent.natures_vigil.enabled)
                if (buff.berserk.up || cooldown.berserk.remains >= 85)
                    natures_vigil();
            if (_healthPercent < SuperbadSettings.Instance.MightofUrsoc)
                if (Might_of_Ursoc())
                    return;
            if (_healthPercent < SuperbadSettings.Instance.CenarionWard)
                if (cenarion_ward())
                    return;
            if (_rage < 80)
                enrage();
            if (Mangle())
                return;

            if (SuperbadSettings.Instance.LacerateCycle)
            {
                List<WoWUnit> lacerateTargets =
                    Unit.NearbyUnfriendlyUnits.Where(
                        u =>
                            (u.Combat || Unit.IsDummy(u)) && u.IsWithinMeleeRange && target.time_to_die_circle(u) > 1 &&
                            !u.IsFriendly && StyxWoW.Me.IsFacing(u)).ToList();
                WoWUnit lacerateUnit =
                    (lacerateTargets.Where(
                        u => !dot.lacerate_cycle.up(u)).ToList().FirstOrDefault() ?? lacerateTargets.Where(
                            u => dot.lacerate_cycle.up(u) && dot.lacerate_cycle.stacks(u) == 1)
                            .ToList()
                            .FirstOrDefault()) ??
                    lacerateTargets.Where(
                        u => dot.lacerate_cycle.up(u) && dot.lacerate_cycle.stacks(u) == 2).ToList().FirstOrDefault();

                if (lacerateUnit == null)
                {
                    List<WoWUnit> lacerateList = lacerateTargets.Where(dot.lacerate_cycle.up).ToList();
                    lacerateUnit = lacerateList.OrderBy(dot.lacerate_cycle.remains).FirstOrDefault();
                }
                if (Lacerate(lacerateUnit))
                    return;
            }
            else
            {
                if (StyxWoW.Me.CurrentTarget != null)
                    Lacerate(StyxWoW.Me.CurrentTarget);
            }
            if (debuff.weakened_blows.remains < 6)
                if (thrash_bear())
                    return;
            if (faerie_fire())
                return;
            Consecration();
        }

        private static void ThrashAoe()
        {
            if (_healthPercent < SuperbadSettings.Instance.MightofUrsoc)
                if (Might_of_Ursoc())
                    return;
            if (_healthPercent < SuperbadSettings.Instance.CenarionWard)
                if (cenarion_ward())
                    return;
            Maul();
            if (Frenzied_Regeneration())
                return;
            if (_rage > 59 && buff.savage_defense.down)
                savage_defense();
            if (_rage < 80)
                enrage();
            berserk();
            if (talent.incarnation.enabled && cooldown.berserk.remains < 170 && cooldown.berserk.remains >= 30)
                incarnation();
            if (talent.natures_vigil.enabled)
                if (buff.berserk.up || cooldown.berserk.remains >= 85)
                    natures_vigil();
            if (buff.berserk.up && _unitCount <= 3)
                if (Mangle())
                    return;
            if (thrash_bear())
                return;
            if (Swipe())
                return;
            if (Consecration())
                return;
            if (Mangle())
                return;
            if (StyxWoW.Me.CurrentTarget != null)
                if (Lacerate(StyxWoW.Me.CurrentTarget))
                    return;
            faerie_fire();
        }

        private static void BearPullHandler()
        {
            if (bear_form())
                return;
            if (faerie_fire())
                return;
            if (SpeedHandler())
                return;
            if (_targetAboveGround)
                if (moonfire())
                    return;
            if (Mangle())
                return;
            auto_attack();
        }
    }
}