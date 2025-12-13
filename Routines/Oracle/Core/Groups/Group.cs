#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Groups/Group.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using JetBrains.Annotations;
using Oracle.Shared.Logging;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using GroupRole = Styx.WoWInternals.WoWObjects.WoWPartyMember.GroupRole;

namespace Oracle.Core.Groups
{
    [UsedImplicitly]
    internal static class Group
    {
        private const int MAX_RAID_SUBGROUPS = MAXRAIDSIZE / MAXGROUPSIZE;
        private const int MAXGROUPSIZE = 5;
        private const int MAXRAIDSIZE = 40;

        public static int GetMembersCount { get { return WoWPartyMembers.Count(); } }

        // Our Initial list of wowpartymembers.
        public static IEnumerable<WoWPartyMember> WoWPartyMembers { get { return StyxWoW.Me.GroupInfo.RaidMembers.Union(StyxWoW.Me.GroupInfo.PartyMembers).Distinct(); } }

        internal static uint GetGroupNumber(this WoWPlayer member)
        {
            if (!OracleRoutine.IsViable(member)) return 0;
            var result = WoWPartyMembers.FirstOrDefault(a => a.Guid == member.Guid); return result != null ? result.GroupNumber : 0;
        }

        public static GroupRole GetRole(this WoWPlayer member)
        {
            if (!OracleRoutine.IsViable(member)) return GroupRole.None;
            var result = WoWPartyMembers.FirstOrDefault(a => a.Guid == member.Guid); return result != null ? result.Role : GroupRole.None;
        }

        public static WoWSpec GetSpecialization(this WoWPlayer member)
        {
            if (!OracleRoutine.IsViable(member)) return WoWSpec.None;
            var result = WoWPartyMembers.FirstOrDefault(a => a.Guid == member.Guid); return result != null ? result.Specialization : WoWSpec.None;
        }

        public static bool IsAssistTank(this WoWPlayer member)
        {
            if (!OracleRoutine.IsViable(member)) return false;
            var result = WoWPartyMembers.FirstOrDefault(a => a.Guid == member.Guid); return result != null && result.IsMainAssist;
        }

        public static bool IsMainTank(this WoWPlayer member)
        {
            if (!OracleRoutine.IsViable(member)) return false;
            var result = WoWPartyMembers.FirstOrDefault(a => a.Guid == member.Guid); return result != null && result.IsMainTank;
        }

        #region OraclePartyMember Entries

        internal static readonly Dictionary<ulong, OraclePartyMember> OraclePartyMemberEntries = new Dictionary<ulong, OraclePartyMember>();

        public static void OutputOraclePartyMemberEntries()
        {
            if (OraclePartyMemberEntries.Values.Count < 1) return;
            Logger.Output(" --> We have {0} Oracle Party Members", OraclePartyMemberEntries.Count);
        }

        internal static void PulseOraclePartyMember()
        {
            OraclePartyMemberEntries.RemoveAll(t => DateTime.UtcNow.Subtract(t.CurrentTime).Milliseconds >= t.ExpiryTime);
        }

        internal static void UpdateOraclePartyMember(ulong targetGUID, double expiryTime)
        {
            if (!OraclePartyMemberEntries.ContainsKey(targetGUID)) OraclePartyMemberEntries.Add(targetGUID, new OraclePartyMember(targetGUID, expiryTime, DateTime.UtcNow));
        }

        internal struct OraclePartyMember
        {
            public OraclePartyMember(ulong targetGUID, double expiryTime, DateTime currentTime)
                : this()
            {
                GUID = targetGUID;
                ExpiryTime = expiryTime;
                CurrentTime = currentTime;
            }

            public DateTime CurrentTime { get; private set; }

            public ulong GUID { get; set; }

            public double ExpiryTime { get; set; }
        }

        #endregion OraclePartyMember Entries
    }
}