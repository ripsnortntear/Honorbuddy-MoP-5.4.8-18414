using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PureRotation.Core;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Helpers
{
    [UsedImplicitly]
    internal class EncounterSpecific
    {
        #region Tsulong

        private const uint TsulongId = 62442;

        public static WoWUnit GetTsulong
        {
            get
            {
                WoWUnit boss = null;
                if (StyxWoW.Me.GroupInfo.LfgDungeonId != 526) return null; // dont do shit unless we in the terrace yo!
                boss = ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Entry == TsulongId);
                if (boss != null && boss.IsFriendly) return boss; // return him if hes friendly.
                return null;
            }
        }

        #endregion Tsulong

        private bool _foundFriendlyTsulong;

        // Terrace of Endless Spring - Tsulong Friendly
        public WoWUnit Tsulong(float distance)
        {
            WoWPoint curTarLocation = StyxWoW.Me.Location;
            WoWUnit boss = null;

            // Check we are in the Terrace of Endless Spring
            if (StyxWoW.Me.GroupInfo.LfgDungeonId == 4)
            {
                if (!_foundFriendlyTsulong)
                {
                    // 62442 bad faction = 16 faction name = Monstor
                    // 62442 bad faction = 35 faction name = Friendly

                    boss = ObjectManager.GetObjectsOfType<WoWUnit>(false, false).FirstOrDefault(p => p.Location.Distance(curTarLocation) <= distance && p.Entry == 62442 && p.FactionId == 35); //63025
                    _foundFriendlyTsulong = true;
                }
            }

            return boss;
        }

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

        // Throne of Thunder - Tortos
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Humming Crystal Support</returns>
        public static Composite HandleTortosHC()
        {
            return new PrioritySelector(
                new Decorator(ret => true,
                              new PrioritySelector(
                                  //TODO: Copy over from leaves.
                                  )));
        }

        #region Tanking Encounter Specific checks

        // Terrace of Endless Spring - Lei Shi
        internal static bool LeiShiSprayStackCount(uint setStackCount)
        {
            {
                if (StyxWoW.Me.CurrentTarget != null)
                {
                    var result = StyxWoW.Me.CachedStackCount("Spray");
                    return result == setStackCount;
                }
                return false;
            }
        }

        // Terrace of Endless Spring - Tsulong
        internal static bool TsulongShadowBreath
        {
            get
            {
                return StyxWoW.Me.CurrentTarget != null &&
                       (StyxWoW.Me.CurrentTarget.IsCasting && StyxWoW.Me.CurrentTarget.CastingSpellId == 122752 &&
                        StyxWoW.Me.CurrentTarget.IsTargetingMeOrPet);
            }
        }

        // Mogu'shan Vaults - Elegon
        internal static bool ElegonCelestialBreath
        {
            get
            {
                return StyxWoW.Me.CurrentTarget != null &&
                       (StyxWoW.Me.CurrentTarget.IsCasting && StyxWoW.Me.CurrentTarget.CastingSpellId == 117960 &&
                        StyxWoW.Me.CurrentTarget.IsTargetingMeOrPet);
            }
        }

        #endregion Tanking Encounter Specific checks

        public static IEnumerable<WoWUnit> UrgentInteruptTarget()
        {
            return
                ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(
                    u => u.IsAlive &&
                         u.Guid != StyxWoW.Me.Guid &&
                         u.IsHostile &&
                         u.IsCasting &&
                         UrgentInterupts.Contains(u.CastingSpellId));
        }

        public static bool StopCasting()
        {
            if (StopCastingSpells.Contains(StyxWoW.Me.CurrentTarget.CastingSpellId))
            {
                Logger.InfoLog("Cancelling Cast! {0} is casting {1}", StyxWoW.Me.CurrentTarget.SafeName, StyxWoW.Me.CurrentTarget.CastingSpellId);
                SpellManager.StopCasting();
                return true;
            }

            return false;
        }

        // List of bad auras when to not cast expensive (CD style) spells
        internal static bool HasBadAura { get { return StyxWoW.Me.HasAnyAura(BadAuras); } }

        internal static readonly HashSet<int> StopCastingSpells = new HashSet<int>
            {
                138763, // Dark Animus - Interrupting Jolt
                137457 // Oondasta - Piercing Roar
            };

        internal static readonly HashSet<int> BadAuras = new HashSet<int>
         {
                116040, // Epicenter - Feng the Accursed (-75% chance to hit)
                116937  // Epicenter - Feng the Accursed (-75% chance to hit)
        };

        internal static readonly HashSet<int> UrgentInterupts = new HashSet<int>
            {
                117833, // Crazy Thought - Meng the Demented
                122193, // Mending - Wind lord Mel'jarah
                124077, // Dispatch - Grand Empress Shek
                117187, // Lighting bolt - Protectors of the Endless
                117163, // Water Bolt -  Elder Asani
                136797, //http://www.wowhead.com/spell=136797
                136587, //http://www.wowhead.com/spell=136587
                61909, //http://www.wowhead.com/spell=61909
            };
    }
}