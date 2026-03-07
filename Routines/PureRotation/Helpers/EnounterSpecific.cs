using JetBrains.Annotations;
using Styx;
using Styx.TreeSharp;

namespace PureRotation.Helpers
{
    [UsedImplicitly]
    internal class EnounterSpecific
    {


        // Heart of Fear - Amber-Shaper Un'sok
        public static Composite HandleActionBarInterrupts()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.HasAura(122370),
                              new PrioritySelector(
                                  Lua.RunMacroText("/click OverrideActionBarButton1", ret => StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.CastingSpellId == 122402),
                                  Lua.RunMacroText("/click OverrideActionBarButton2", ret => StyxWoW.Me.IsCasting),
                                  Lua.RunMacroText("/click OverrideActionBarButton4", ret => StyxWoW.Me.HealthPercent < 17)
                                  )));
        }
    }
}
