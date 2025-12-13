#region

using System;
using Styx;
using Styx.WoWInternals.DBC;

#endregion

namespace Superbad
{
    [Flags]
    public enum WoWContext
    {
        None = 0,
        Normal = 0x1,
        Instances = 0x2,
        Battlegrounds = 0x4,

        All = Normal | Instances | Battlegrounds,
    }

    internal class Context
    {
        internal class SuperbadRoutine
        {
            internal static WoWContext CurrentWoWContext
            {
                get
                {
                    if (!StyxWoW.IsInGame)
                        return WoWContext.None;

                    Map map = StyxWoW.Me.CurrentMap;

                    if (map.IsBattleground || map.IsArena)
                    {
                        return WoWContext.Battlegrounds;
                    }

                    return (map.IsDungeon || map.IsScenario || map.IsRaid) ? WoWContext.Instances : WoWContext.Normal;
                }
            }
        }
    }
}