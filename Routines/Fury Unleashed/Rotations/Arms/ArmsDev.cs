using FuryUnleashed.Core;
using FuryUnleashed.Interfaces.Settings;
using Styx.TreeSharp;
using Enum = FuryUnleashed.Core.Helpers.Enum;

namespace FuryUnleashed.Rotations.Arms
{
    class ArmsDev
    {
        //private static LocalPlayer Me
        //{
        //    get { return StyxWoW.Me; }
        //}

        internal static Composite DevArmsCombat
        {
            get
            {
                return new PrioritySelector(
                    new Action(delegate { Item.RefreshSecondaryStats(); return RunStatus.Failure; }),
                    new Switch<Enum.Mode>(ctx => SettingsH.Instance.ModeSelection,
                        new SwitchArgument<Enum.Mode>(Enum.Mode.Auto,
                            new PrioritySelector(
                                ArmsRel.RelArmsCombat
                                )),
                        new SwitchArgument<Enum.Mode>(Enum.Mode.SemiHotkey,
                            new PrioritySelector(
                                ArmsRel.RelArmsCombat
                                )),
                        new SwitchArgument<Enum.Mode>(Enum.Mode.Hotkey,
                            new PrioritySelector(
                                ArmsRel.RelArmsCombat
                                ))));
            }
        }

        internal static Composite Dev_ArmsSt()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ArmsHeroicStrike()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ArmsExec()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ArmsMt()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ArmsOffensive()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ArmsGcdUtility()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ArmsRacials()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ArmsDefensive()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ArmsNonGcdUtility()
        {
            return new PrioritySelector(
                );
        }
    }
}
