using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.WoWInternals.WoWObjects;
using GroupRole = Styx.WoWInternals.WoWObjects.WoWPartyMember.GroupRole;

namespace PureRotation.Helpers
{
    public static class PartyExtensions
    {
        private static IEnumerable<WoWPartyMember> GroupMembers { get { return !StyxWoW.Me.GroupInfo.IsInRaid ? StyxWoW.Me.GroupInfo.PartyMembers : StyxWoW.Me.GroupInfo.RaidMembers; } }

        public static GroupRole Role(this WoWPlayer p)
        {
            if (!GroupMembers.Any()) return GroupRole.None;
            var partyMember = GroupMembers.FirstOrDefault(pm => pm.Guid == p.Guid);
            if (partyMember != null)
            {
                var role = partyMember.Role;
                return role;
            }
            return GroupRole.None;
        }

        public static bool IsMainTank(this WoWPlayer p)
        {
            if (!GroupMembers.Any()) return false;
            var partyMember = GroupMembers.FirstOrDefault(pm => pm.Guid == p.Guid);
            var result = partyMember != null && partyMember.IsMainTank;
            return result;
        }

        public static bool IsAssistTank(this WoWPlayer p)
        {
            if (!GroupMembers.Any()) return false;
            var partyMember = GroupMembers.FirstOrDefault(pm => pm.Guid == p.Guid);
            var result = partyMember != null && partyMember.IsMainAssist;
            return result;
        }
    }
}
