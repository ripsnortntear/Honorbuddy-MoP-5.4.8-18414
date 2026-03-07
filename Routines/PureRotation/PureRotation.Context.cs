#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-05-17 10:23:04 +0200 (Fr, 17 Mai 2013) $
 * $ID$
 * $Revision: 1424 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/PureRotation.Context.cs $
 * $LastChangedBy: millz $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using PureRotation.Core;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals.DBC;

namespace PureRotation
{
    #region Nested type: LocationContextEventArg

    public class WoWContextEventArg : EventArgs
    {
        public readonly WoWContext CurrentWoWContext;
        public readonly WoWContext PreviousWoWContext;

        public WoWContextEventArg(WoWContext currentWoWContext, WoWContext prevWoWContext)
        {
            CurrentWoWContext = currentWoWContext;
            PreviousWoWContext = prevWoWContext;
        }
    }

    #endregion Nested type: LocationContextEventArg

    partial class PureRotationRoutine
    {
        private bool _contextEventSubscribed;

        private static WoWContext LastWoWContext { get; set; }

        private static GroupType GroupType
        {
            get
            {
                if (Me.GroupInfo.IsInRaid)
                {
                    return GroupType.Raid;
                }

                return Me.GroupInfo.IsInParty ? GroupType.Party : GroupType.Solo;
            }
        }

        internal static WoWContext CurrentWoWContext
        {
            get
            {
                if (!StyxWoW.IsInGame)
                    return WoWContext.None;

                Map map = Me.CurrentMap;

                if (map.IsBattleground || map.IsArena)
                {
                    return WoWContext.Battleground;
                }

                if (Me.IsInGroup())
                {
                    if (Me.IsInInstance || map.IsDungeon || map.IsRaid || map.IsScenario)
                    {
                        return WoWContext.Instances;
                    }
                }

                return WoWContext.PVE;
            }
        }

        internal static event EventHandler<WoWContextEventArg> OnWoWContextChanged;

        private void UpdateContext()
        {
            // Subscribe to the map change event, so we can automatically update the context.
            if (!_contextEventSubscribed)
            {
                // Subscribe to OnBattlegroundEntered. Just 'cause.
                BotEvents.Battleground.OnBattlegroundEntered += e => UpdateContext();
                _contextEventSubscribed = true;
            }

            var current = CurrentWoWContext;

            // Can't update the context when it doesn't exist.
            if (current == WoWContext.None)
                return;

            if (current != LastWoWContext && OnWoWContextChanged != null)
            {
                try
                {
                    OnWoWContextChanged(this, new WoWContextEventArg(current, LastWoWContext));
                }
                catch
                {
                    // Eat any exceptions thrown.
                }
                LastWoWContext = current;
            }
        }
    }
}