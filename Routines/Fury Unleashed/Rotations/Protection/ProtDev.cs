using FuryUnleashed.Core.Helpers;
using FuryUnleashed.Interfaces.Settings;
using Styx.TreeSharp;

namespace FuryUnleashed.Rotations.Protection
{
    class ProtDev
    {
        //private static LocalPlayer Me
        //{
        //    get { return StyxWoW.Me; }
        //}

        internal static Composite DevProtCombat
        {
            get
            {
                return new PrioritySelector(
                    new Switch<Enum.Mode>(ctx => SettingsH.Instance.ModeSelection,
                        new SwitchArgument<Enum.Mode>(Enum.Mode.Auto,
                            new PrioritySelector(
                                ProtRel.RelProtCombat
                                )),
                        new SwitchArgument<Enum.Mode>(Enum.Mode.SemiHotkey,
                            new PrioritySelector(
                                ProtRel.RelProtCombat
                                )),
                        new SwitchArgument<Enum.Mode>(Enum.Mode.Hotkey,
                            new PrioritySelector(
                                ProtRel.RelProtCombat
                                ))));
            }
        }

        internal static Composite Dev_ProtRageDump()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ProtSt()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ProtMt()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ProtDefensive()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ProtGcdUtility()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ProtNonGcdUtility()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ProtRacials()
        {
            return new PrioritySelector(
                );
        }

        internal static Composite Dev_ProtOffensive()
        {
            return new PrioritySelector(
                );
        }
    }
}
