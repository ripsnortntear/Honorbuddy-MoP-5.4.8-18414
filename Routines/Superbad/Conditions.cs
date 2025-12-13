#region

using System.Linq;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    public partial class Superbad
    {
        public static bool BotbaseManualMode()
        {
            string name = BotManager.Current.Name;
            switch (name)
            {
                case "LazyRaider":
                    return true;
                case "Raid Bot":
                    return true;
                case "Combat Bot":
                    return true;
                case "Tyrael":
                    return true;
                default:
                    return false;
            }
        }

        public static int CalcRakeTicksRemaining()
        {
            if (!dot.rake.ticking)
                return 0;
            if (dot.rake.remains < 3)
                return 1;
            if (dot.rake.remains < 6)
                return 2;
            if (dot.rake.remains < 9)
                return 3;
            if (dot.rake.remains < 12)
                return 4;
            if (dot.rake.remains < 15)
                return 5;
            if (dot.rake.remains < 18)
                return 5;
            return 0;
        }

        private static bool NeedCat()
        {
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.BEAR)
                return false;
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.CAT)
                return true;
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.MANUAL &&
                StyxWoW.Me.Shapeshift == ShapeshiftForm.Cat)
                return true;
            if (buff.might_or_ursoc.up)
                return false;
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.AUTO &&
                (StyxWoW.Me.GroupInfo.IsInParty || StyxWoW.Me.GroupInfo.IsInRaid) && Group.Tanks.Any() &&
                StyxWoW.Me.Specialization == WoWSpec.DruidFeral)
                return true;
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.AUTO &&
                (StyxWoW.Me.GroupInfo.IsInParty || StyxWoW.Me.GroupInfo.IsInRaid) && Group.Tanks.Any() &&
                StyxWoW.Me.Specialization == WoWSpec.DruidGuardian)
                return false;
            return SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.AUTO &&
                   (StyxWoW.Me.HealthPercent > SuperbadSettings.Instance.HealthBearSwitch ||
                    !SuperbadSettings.Instance.ShiftOnLowHealth) &&
                   (_unitCount < SuperbadSettings.Instance.AddsBearSwitch || ! SuperbadSettings.Instance.ShiftOnMobCount);
        }

        private static bool NeedBear()
        {
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.CAT)
                return false;
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.BEAR)
                return true;
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.MANUAL &&
                StyxWoW.Me.Shapeshift == ShapeshiftForm.Bear)
                return true;
            if (buff.might_or_ursoc.up)
                return true;
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.AUTO &&
                (StyxWoW.Me.GroupInfo.IsInParty || StyxWoW.Me.GroupInfo.IsInRaid) && Group.Tanks.Any() &&
                StyxWoW.Me.Specialization == WoWSpec.DruidFeral)
                return false;
            if (SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.AUTO &&
                (StyxWoW.Me.GroupInfo.IsInParty || StyxWoW.Me.GroupInfo.IsInRaid) && Group.Tanks.Any() &&
                StyxWoW.Me.Specialization == WoWSpec.DruidGuardian)
                return true;
            return SuperbadSettings.Instance.Form == SuperbadSettings.Shapeshift.AUTO &&
                   ((StyxWoW.Me.HealthPercent <= SuperbadSettings.Instance.HealthBearSwitch &&
                     SuperbadSettings.Instance.ShiftOnLowHealth) ||
                    (SuperbadSettings.Instance.ShiftOnMobCount && _unitCount >= SuperbadSettings.Instance.AddsBearSwitch));
        }

        internal class buff
        {
            public class berserk
            {
                public static bool down { get; set; }

                public static bool up { get; set; }
            }

            public class bloodlust
            {
                public static bool up { get; set; }
            }

            public class darkflight
            {
                public static bool up { get; set; }
            }

            public class dash
            {
                public static bool up { get; set; }
            }

            public class dream_of_cenarius
            {
                public static bool down { get; set; }
            }

            public class dream_of_cenarius_damage
            {
                public static bool up { get; set; }
            }

            public class feral_fury
            {
                public static bool react { get; set; }
            }

            public class feral_rage
            {
                public static bool up;
                public static double remains { get; set; }
            }

            public class flag
            {
                public static bool up { get; set; }
            }

            public class king_of_the_jungle
            {
                public static bool up { get; set; }
                public static bool down { get; set; }
            }

            public class mark_of_the_wild
            {
                public static bool down { get; set; }
            }

            public class might_or_ursoc
            {
                public static bool up { get; set; }
            }

            public class natures_vigil
            {
                public static bool up { get; set; }
            }

            public class omen_of_clarity
            {
                public static bool react { get; set; }
            }

            public class predatory_swiftness
            {
                public static bool up { get; set; }
                public static double remains { get; set; }
                public static bool down { get; set; }
            }

            public class prowl
            {
                public static bool up { get; set; }
            }

            public class rejuvenation
            {
                public static bool up { get; set; }
            }

            public class resurrection_sickness
            {
                public static bool up { get; set; }
            }

            public class rooted
            {
                public static bool up { get; set; }
            }

            public class rune_of_reorigination
            {
                public static bool up { get; set; }
                public static double remains { get; set; }
            }

            public class savage_defense
            {
                public static bool down { get; set; }
                public static bool up { get; set; }
            }

            public class savage_roar
            {
                public static bool up { get; set; }
                public static bool down { get; set; }
                public static double remains { get; set; }
            }

            public class stampede
            {
                public static bool up { get; set; }
            }

            public class stampeding_roar
            {
                public static bool up { get; set; }
            }

            public class tigers_fury
            {
                public static bool up;
            }

            public class tooth_and_claw
            {
                public static bool up { get; set; }
            }

            public class trinket
            {
                public static bool procagilityreact { get; set; }
            }
        }

        internal class cooldown
        {
            public class FF
            {
                public static double remains { get; set; }
            }

            public class berserk
            {
                public static double remains { get; set; }
            }

            public class incarnation
            {
                public static double remains { get; set; }
            }

            public class lacerate
            {
                public static double remains { get; set; }
            }

            public class thrash
            {
                public static double remains { get; set; }
            }

            public class tigers_fury
            {
                public static double remains { get; set; }
            }
        }

        internal class debuff
        {
            public class cc
            {
                public static bool up { get; set; }
            }

            public class weakened_armor
            {
                public static double stack { get; set; }
            }

            public class weakened_armor_cycle
            {
                public static double stack(WoWUnit woWunit)
                {
                    WoWAura aura =
                        woWunit.GetAllAuras()
                            .FirstOrDefault(u => u.CreatorGuid == StyxWoW.Me.Guid && u.Name == "Weakened Armor");
                    return aura == null ? 0 : aura.StackCount;
                }
            }

            public class weakened_blows
            {
                public static double remains { get; set; }
            }
        }

        internal class dot
        {
            public class lacerate_cycle
            {
                public static double remains(WoWUnit woWunit)
                {
                    WoWAura aura =
                        woWunit.GetAllAuras()
                            .FirstOrDefault(u => u.CreatorGuid == StyxWoW.Me.Guid && u.SpellId == 33745);
                    return aura == null ? 0 : aura.TimeLeft.TotalSeconds;
                }

                public static double stacks(WoWUnit woWunit)
                {
                    WoWAura aura =
                        woWunit.GetAllAuras()
                            .FirstOrDefault(u => u.CreatorGuid == StyxWoW.Me.Guid && u.SpellId == 33745);
                    return aura == null ? 0 : aura.StackCount;
                }

                public static bool up(WoWUnit woWunit)
                {
                    return woWunit != null && woWunit.HasMyDebuffCycle(33745);
                }
            }

            public class rake
            {
                public static bool ticking { get; set; }
                public static double remains { get; set; }
                public static double ticks_remain { get; set; }
            }

            public class rake_cycle
            {
                public static double remains(WoWUnit woWunit)
                {
                    WoWAura aura =
                        woWunit.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == StyxWoW.Me.Guid && u.SpellId == 1822);
                    return aura == null ? 0 : aura.TimeLeft.TotalSeconds;
                }
            }

            public class rip
            {
                public static bool ticking { get; set; }
                public static double remains { get; set; }

                public static bool ticking_unit(WoWUnit woWUnit)
                {
                    return woWUnit != null && woWUnit.HasMyDebuffCycle(1079);
                }
            }

            public class thrash_cat
            {
                public static double remains { get; set; }
            }

            public class thrash_cat_cycle
            {
                public static double remains(WoWUnit woWunit)
                {
                    WoWAura aura =
                        woWunit.GetAllAuras()
                            .FirstOrDefault(u => u.CreatorGuid == StyxWoW.Me.Guid && u.Name == ("Thrash"));
                    return aura == null ? 0 : aura.TimeLeft.TotalSeconds;
                }
            }
        }

        internal class target
        {
            public static double time_to_die { get; set; }

            public static double time_to_die_circle(WoWUnit woWUnit)
            {
                return woWUnit != null ? DpsMeter.GetCombatTimeLeft(woWUnit).TotalSeconds : 16;
            }

            public class health
            {
                public static double pct { get; set; }
            }
        }
    }


    internal class talent
    {
        internal enum DruidTalents
        {
            FelineSwiftness = 1,
            DisplacerBeast,
            WildCharge,
            NaturesSwiftness,
            Renewal,
            CenarionWard,
            FaerieSwarm,
            MassEntanglement,
            Typhoon,
            SoulOfTheForest,
            Incarnation,
            ForceOfNature,
            DisorientingRoar,
            UrsolsVortex,
            MightyBash,
            HeartOfTheWild,
            DreamOfCenarius,
            NaturesVigil
        }

        public class dream_of_cenarius
        {
            public static bool enabled
            {
                get { return TalentManager.DreamOfCenarius; }
            }
        }

        public class force_of_nature
        {
            public static bool enabled
            {
                get { return TalentManager.ForceOfNature; }
            }
        }

        public class heart_of_the_wild
        {
            public static bool enabled
            {
                get { return TalentManager.HeartOfTheWild; }
            }
        }

        public class incarnation
        {
            public static bool enabled
            {
                get { return TalentManager.Incarnation; }
            }
        }

        public class natures_swiftness
        {
            public static bool enabled
            {
                get { return TalentManager.NaturesSwiftness; }
            }
        }

        public class natures_vigil
        {
            public static bool enabled
            {
                get { return TalentManager.NaturesVigil; }
            }
        }

        public class soul_of_the_forest
        {
            public static bool enabled
            {
                get { return TalentManager.SoulOfTheForest; }
            }
        }
    }
}