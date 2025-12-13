using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieHolyPriestPvP
{
    public enum CacheKeys : int
    {
        CircleOfHealing,
        HolyWordSanctuary,
    }

    public static class CacheHelper
    {
        private static Dictionary<int, KeyValuePair<DateTime, object>> Cache = new Dictionary<int, KeyValuePair<DateTime, object>>();

        public static bool Get<T>(CacheKeys key, int validMs, out T obj)
            where T : class
        {
            obj = null;
            KeyValuePair<DateTime, object> val;
            if (Cache.TryGetValue((int)key, out val) && val.Key.AddMilliseconds(validMs) > DateTime.Now)
            {
                obj = val.Value as T;
                return true;
            }

            return false;
        }

        public static T Store<T>(CacheKeys key, T obj)
        {
            Cache[(int)key] = new KeyValuePair<DateTime, object>(DateTime.Now, obj);

            return obj;
        }
    }

    public static class UnitManager
    {
        #region Privates

        private static List<WoWUnit> AllUnits = new List<WoWUnit>(80);
        private static readonly List<WoWPlayer> NearbyFriendlyPlayers = new List<WoWPlayer>(40);
        private static readonly List<WoWUnit> NearbyFriendlyUnits = new List<WoWUnit>(40);
        private static readonly List<WoWPlayer> NearbyUnFriendlyPlayers = new List<WoWPlayer>(40);
        private static readonly List<WoWUnit> NearbyUnFriendlyUnits = new List<WoWUnit>(40);
        private static readonly List<WoWUnit> NearbyTotems = new List<WoWUnit>(10);

        #endregion

        #region Public Properties

        public static LocalPlayer Me { get { return StyxWoW.Me; } }
        public static WoWUnit HealTarget { get; private set; }
        public static WoWUnit DamageTarget { get; private set; }
        public static WoWUnit PolyTarget { get; private set; }
        public static bool? HasSWDGlypth { get; set; }

        public static WoWPlayer ScatteredTeammate { get; private set; }
        public static WoWUnit MassDispelTarget { get; private set; }
        public static WoWUnit PurifyASAPTarget { get; private set; }
        public static WoWUnit PurifyLowPrioTarget { get; private set; }
        public static int PurifyLowPrioTargetDebuffCount { get; private set; }
        public static WoWUnit VoidShiftTarget { get; private set; }

        #endregion

        #region Refresh

        public static void Refresh()
        {
            AllUnits.Clear();

            Clear();

            NearbyFriendlyUnits.Add(Me);
            NearbyFriendlyPlayers.Add(Me);

            // Stores all valid players, pets and totems.
            var wowUnits = ObjectManager.GetObjectsOfTypeFast<WoWUnit>();

            for (int i = 0; i < wowUnits.Count; ++i)
            {
                WoWUnit unit = wowUnits[i];
                if (!unit.IsValidUnit() || unit.Distance2D > 40 || (!unit.IsPlayer && !unit.IsPet && !unit.IsTotem && !unit.IsTrainingDummy()))
                    continue;

                if (unit.IsMyPartyRaidMember() || unit.IsTrainingDummy())
                {
                    if (unit.IsPlayer || (Me.CurrentMap.IsArena && HolySettings.Instance.HealPets && unit.IsPet) || unit.IsTrainingDummy())
                    {
                        if (unit != Me)
                            NearbyFriendlyUnits.Add(unit);

                        // Search for Heal Target
                        if (unit.HealthPercent < HealTarget.HealthPercent && unit.InLineOfSight())
                            HealTarget = unit;

                        var player = unit as WoWPlayer;

                        if (player != null && player != Me)
                            NearbyFriendlyPlayers.Add(player);
                    }
                }
                else if (unit.IsEnemy())
                {
                    if (unit.IsPet || unit.IsPlayer || unit.IsTrainingDummy())
                        NearbyUnFriendlyUnits.Add(unit);
                    else if (unit.IsTotem)
                        NearbyTotems.Add(unit);

                    var player = unit as WoWPlayer;
                    if (player != null)
                        NearbyUnFriendlyPlayers.Add(player);
                }
            }
        }

        public static void RefilterUnitsForHeal()
        {
            HealTarget = Me;
            for (int i = 0; i < AllUnits.Count; ++i)
            {
                WoWUnit unit = AllUnits[i];

                if (unit.IsValidUnit() && (unit.IsMyPartyRaidMember() || unit.IsTrainingDummy()))
                {
                    if (unit.IsPlayer || (Me.CurrentMap.IsArena && HolySettings.Instance.HealPets && unit.IsPet) || unit.IsTrainingDummy())
                    {
                        NearbyFriendlyUnits.Add(unit);

                        // Search for Heal Target
                        if (unit.HealthPercent < HealTarget.HealthPercent)
                            HealTarget = unit;
                    }
                }
            }
        }

        public static void Clear()
        {
            NearbyFriendlyPlayers.Clear();
            NearbyUnFriendlyPlayers.Clear();
            NearbyFriendlyUnits.Clear();
            NearbyUnFriendlyUnits.Clear();
            NearbyTotems.Clear();

            HealTarget = Me;
            DamageTarget = null;
            numberOfMeleeTargetingMe = null;
        }

        #endregion

        #region Filtering Units

        public static bool NeedPolyReact()
        {
            if (!HasSWDGlypth.HasValue)
                HasSWDGlypth = GlyphManager.Has(Glyphs.GlyphOfShadowWordDeath);

            if (!(HasSWDGlypth ?? false))
                return false;

            for (int i = 0; i < NearbyUnFriendlyPlayers.Count; ++i)
            {
                var u = NearbyUnFriendlyPlayers[i];
                if (u.IsValidUnit() && u.IsTargetingMeOrPet && (u.CastingSpellId == (uint)SpellIDs.Polymorph ||
                                                                u.CastingSpellId == (uint)SpellIDs.Repentance ||
                                                                u.CastingSpellId == (uint)SpellIDs.BlindingLight))
                {
                    PolyTarget = u;
                    return true;
                }
            }
            return false;
        }

        private static uint[] TotemPriorities = new uint[] { (uint)SpellIDs.SpiritLinkTotem, (uint)SpellIDs.TremorTotem, (uint)SpellIDs.GroundingTotem, (uint)SpellIDs.CapacitorTotem, 
                                                             (uint)SpellIDs.HealingStreamTotem, (uint)SpellIDs.WindwalkTotem, (uint)SpellIDs.EarthgrabTotem, (uint)SpellIDs.EarthbindTotem };
        public static bool SelectDamageTarget()
        {
            if (DamageTarget != null)
                return true;

            if (NearbyTotems.Count > 0)
            {
                // Prefilter totems
                var priorityTargets = from totem in NearbyTotems
                                      where totem.IsValidUnit() && (totem.CreatedBySpellId == TotemPriorities[0] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[1] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[2] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[3] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[4] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[5] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[6] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[7])
                                      select totem;

                for (int i = 0; i < TotemPriorities.Length; ++i)
                {
                    DamageTarget = (from totem in priorityTargets where totem.CreatedBySpellId == TotemPriorities[i] select totem).FirstOrDefault();

                    if (DamageTarget != null)
                        break;
                }

                if (DamageTarget == null)
                    DamageTarget = NearbyTotems[0];
            }

            return DamageTarget != null;
        }
        
        #region Find By GUID

        public static WoWUnit FindByGUID(ulong guid)
        {
            return (from unit in NearbyUnFriendlyPlayers where unit.IsValidUnit() && unit.Guid == guid select unit).FirstOrDefault();
        }

        #endregion

        #region NumberOfMeleeTargeting

        public static int NumberOfMeleeTargeting(this WoWUnit u)
        {
            return NearbyUnFriendlyPlayers.Where(
                unit => unit.IsValidUnit() &&
                    //unit.Distance2D <= 10 &&                        
                    //Elvileg ez a jo megoldas, de tesztelni kellene
                        unit.Location.Distance(u.Location) <= 10 &&
                        unit.CurrentTarget != null &&
                        unit.CurrentTarget.IsValid && unit.CurrentTarget == u &&
                        unit.TalentSort() == SpecTypes.Melee
                ).Count();
        }

        #endregion

        #region CountEnemyTargetingUnit

        private static double CountEnemyTargetingUnit(WoWUnit target)
        {
            if (target == null || !target.IsValid)
                return 0;

            return NearbyUnFriendlyPlayers.Where(
                    unit =>
                        unit != null && unit.IsValid &&
                        unit.CurrentTarget != null && unit.CurrentTarget.IsValid && unit.CurrentTarget == target
                    ).Count();
        }

        #endregion

        #region NumberOfMeleeTargetingMe

        private static int? numberOfMeleeTargetingMe;
        private static DateTime lastNumberOfMeleeTargetingMeCheck = DateTime.MinValue;
        public static int NumberOfMeleeTargetingMe()
        {
            if (lastNumberOfMeleeTargetingMeCheck.AddMilliseconds(1500) > DateTime.Now && numberOfMeleeTargetingMe.HasValue)
                return numberOfMeleeTargetingMe.Value;
            lastNumberOfMeleeTargetingMeCheck = DateTime.Now;

            if (numberOfMeleeTargetingMe.HasValue)
                return numberOfMeleeTargetingMe ?? 0;

            numberOfMeleeTargetingMe = NearbyUnFriendlyPlayers.Count(unit => unit.IsValidUnit() && unit.Distance2D <= 10 && unit.CurrentTarget != null &&
                                                                             unit.CurrentTarget.IsValid && unit.CurrentTarget == Me &&
                                                                             unit.TalentSort() == SpecTypes.Melee);

            return numberOfMeleeTargetingMe ?? 0;
        }

        #endregion

        #region NumberOfMeleeInRange

        private static int? numberOfMeleeInRange;
        private static DateTime lastNumberOfMeleeInRangeCheck = DateTime.MinValue;
        public static int NumberOfMeleeInRange(int range = 10)
        {
            if (lastNumberOfMeleeInRangeCheck.AddMilliseconds(1500) > DateTime.Now && numberOfMeleeInRange.HasValue)
                return numberOfMeleeInRange.Value;

            numberOfMeleeInRange = NearbyUnFriendlyPlayers.Count(unit => unit.IsValidUnit() && unit.Distance2D <= range && unit.TalentSort() == SpecTypes.Melee);

            return numberOfMeleeInRange ?? 0;
        }

        #endregion

        #region NumberOfEnemyInRange

        private static int? numberOfEnemyInRange;
        private static DateTime lastNumberOfEnemyInRangeCheck = DateTime.MinValue;
        public static int NumberOfEnemyInRange(int range = 10)
        {
            if (lastNumberOfEnemyInRangeCheck.AddMilliseconds(1500) > DateTime.Now && numberOfEnemyInRange.HasValue)
                return numberOfEnemyInRange.Value;
            lastNumberOfEnemyInRangeCheck = DateTime.Now;

            numberOfEnemyInRange = NearbyUnFriendlyPlayers.Count(unit => unit.IsValidUnit() && unit.Distance2D < range);

            return numberOfEnemyInRange ?? 0;
        }

        #endregion

        #region GetFirstEnemyTargetingMe

        private static WoWUnit firstEnemyTargetingMe;
        private static DateTime lastGetFirstEnemyTargetingMeCheck = DateTime.MinValue;
        public static WoWUnit GetFirstEnemyTargetingMe(int range = 30)
        {
            if (lastGetFirstEnemyTargetingMeCheck.AddMilliseconds(1500) > DateTime.Now && (firstEnemyTargetingMe != null && firstEnemyTargetingMe.IsValidUnit()))
                return firstEnemyTargetingMe;
            lastGetFirstEnemyTargetingMeCheck = DateTime.Now;

            firstEnemyTargetingMe = (from unit in NearbyUnFriendlyPlayers where unit.IsValidUnit() && unit.CurrentTarget != null && unit.CurrentTarget == Me && unit.Distance2D <= range select unit).FirstOrDefault();

            return firstEnemyTargetingMe;
        }

        #endregion

        #region GetFirstValidEnemy

        private static WoWUnit firstValidEnemy;
        private static DateTime lastGetFirstValidEnemyCheck = DateTime.MinValue;
        public static WoWUnit GetFirstValidEnemy(Func<WoWUnit, bool> validator)
        {
            if (validator == null)
                throw new ArgumentNullException("GetFirstValidEnemy - validator is null! ");

            if (lastGetFirstValidEnemyCheck.AddMilliseconds(1500) > DateTime.Now && firstValidEnemy.IsValidUnit())
                return firstValidEnemy;
            lastGetFirstValidEnemyCheck = DateTime.Now;

            return firstValidEnemy = NearbyUnFriendlyPlayers.FirstOrDefault(unit => unit.IsValidUnit() && validator(unit));
        }

        #endregion

        #region GetInjuredCount

        private static int? injuredCount;
        private static DateTime lastGetInjuredCountCheck = DateTime.MinValue;
        private static List<WoWUnit> injuredUnits = new List<WoWUnit>();
        public static int GetInjuredCount(int hpLimit = 90)
        {
            if (lastGetInjuredCountCheck.AddMilliseconds(400) > DateTime.Now && injuredCount != null)
                return injuredCount ?? 0;
            lastGetInjuredCountCheck = DateTime.Now;

            int count = 0;
            injuredUnits.Clear();
            foreach (var unit in NearbyFriendlyUnits)
                if (unit.IsValidUnit() && unit.HealthPercent < hpLimit)
                {
                    count++;
                    injuredUnits.Add(unit);
                }

            injuredCount = count;

            return injuredCount ?? 0;
        }

        #endregion

        #region Get[MassDispel|Purify]Target

        public static bool GetMassDispelTarget()
        {
            if (GlyphManager.Has(Glyphs.GlyphOfMassDispel))
                MassDispelTarget = (from unit in NearbyUnFriendlyUnits where unit.MassDispelableInvulnerability() && unit.InLineOfSpellSight() && unit.Distance2D <= 30 select unit).FirstOrDefault();
            else
                MassDispelTarget = null;

            //only use it to dispel when purify is on CD
            if (MassDispelTarget == null && !HolyCoolDown.IsOff(SpellIDs.Purify, 8))
                MassDispelTarget = (from unit in NearbyFriendlyPlayers
                                    where unit.InLineOfSpellSight()
                                    where unit.Distance2D <= 30
                                    where unit.DebuffCCleanseASAP()
                                    where Me.CurrentMap.IsArena || unit.TalentSort() == SpecTypes.Healer //outside of arenas only use it to dispel healers
                                    where !(unit.IsChanneling && unit.ChanneledSpell.Id == (int)SpellIDs.DominateMind) //don't try to dispel DM if the teammate is casting it
                                    select unit).FirstOrDefault();

            return MassDispelTarget != null;
        }

        public static bool GetPurifyASAPTarget()
        {
            PurifyASAPTarget = null;

            if (NearbyFriendlyPlayers.Count >= 10)
            {
                if (!Me.HasAura(SpellIDs.Cyclone) && Me.DebuffCCleanseASAP() && !Me.DebuffDoNotDispel())
                {
                    PurifyASAPTarget = Me;
                    return true;
                }
                else
                    return false;
            }

            PurifyASAPTarget = (from unit in NearbyFriendlyPlayers
                                where unit.IsValidUnit() && unit.InLineOfSight() &&
                                      !unit.HasAura(SpellIDs.Cyclone) && Me.DebuffCCleanseASAP() &&
                                      !Me.DebuffDoNotDispel()
                                select unit).FirstOrDefault();

            return PurifyASAPTarget != null;
        }

        public static bool GetPurifyLowPrioTarget()
        {
            PurifyLowPrioTarget = null;

            if (NearbyFriendlyPlayers.Count >= 10)
            {
                if (!Me.HasAura(SpellIDs.Cyclone) && !Me.DebuffDoNotDispel())
                {
                    PurifyLowPrioTarget = Me;
                    PurifyLowPrioTargetDebuffCount = PurifyLowPrioTarget.CountDebuff();
                    return true;
                }
                else
                {
                    PurifyLowPrioTargetDebuffCount = 0;
                    return false;
                }
            }

            // CountDebuff should be called once per unit!
            PurifyLowPrioTarget = (from unit in NearbyFriendlyPlayers
                                   where unit.IsValidUnit() && unit.InLineOfSight() &&
                                         !unit.HasAura(SpellIDs.Cyclone) && !unit.DebuffDoNotDispel()
                                   orderby unit.CountDebuff() descending
                                   select unit).FirstOrDefault();

            if (PurifyLowPrioTarget != null)
            {
                PurifyLowPrioTargetDebuffCount = PurifyLowPrioTarget.CountDebuff();
                return true;
            }

            PurifyLowPrioTargetDebuffCount = 0;
            return false;
        }

        #endregion

        #region Select VoidShift Target

        public static bool SelectVoidShiftTarget()
        {
            VoidShiftTarget = null;
            foreach (WoWUnit unit in NearbyFriendlyUnits)
            {
                if (unit.IsValidUnit() && unit.HealthPercent > Me.HealthPercent && (VoidShiftTarget == null || VoidShiftTarget.CurrentHealth < unit.CurrentHealth) &&
                        unit.NumberOfMeleeTargeting() <= 1)
                    VoidShiftTarget = unit;
            }

            return VoidShiftTarget != null;
        }

        #endregion

        #region CC

        public static bool GetScatteredTeammate()
        {
            ScatteredTeammate = NearbyFriendlyPlayers.Where(u => u.CanSelect &&
                                                                 !u.IsDead &&
                                                                 u.IsInMyPartyOrRaid &&
                                                                 u.Distance <= 40 &&
                                                                 u.HasAura(SpellIDs.ScatterShot) &&
                                                                 u.TalentSort() == SpecTypes.Healer)
                                                        .FirstOrDefault();

            return ScatteredTeammate != null;
        }

        public static bool CanReflectCC()
        {
            return NearbyUnFriendlyPlayers.Any(unit => unit.Attackable &&
                                                        unit.CanSelect &&
                                                        !unit.IsFriendly &&
                                                        !unit.IsDead &&
                                                        unit.IsCasting &&
                                                        (unit.CastingSpell.Name.Contains("Poly") ||
                                                        unit.CastingSpellId == 20066 || //Repentance
                                                        unit.CastingSpellId == 115750 || //Blinding Light
                                                        unit.CastingSpellId == 33786) && //Cyclone
                                                        unit.GotTarget &&
                                                        (unit.IsTargetingMyPartyMember || unit.IsTargetingMeOrPet) &&
                                                        unit.CurrentTarget.Distance2D <= 20);
        }

        #endregion

        #region DebuffCCBreakonDamage

        private static readonly HashSet<int> DebuffCCBreakonDamageHS = new HashSet<int> {
            2094, //Blind
            105421, //Blinding Light
            99, //Disorienting Roar
            31661, //Dragon's Breath
            3355, //Freezing Trap
            1776, //Gouge
            2637, //Hibernate
            115268, //Mesmerize (Shivarra)
            115078, //Paralysis
            113953, //Paralysis (Paralytic Poison)
            126355, //Paralyzing Quill (Porcupine)
            126423, //Petrifying Gaze (Basilisk)
            118, //Polymorph
            61305, //Polymorph: Black Cat
            28272, //Polymorph: Pig
            61721, //Polymorph: Rabbit
            61780, //Polymorph: Turkey
            28271, //Polymorph: Turtle
            20066, //Repentance
            6770, //Sap
            19503, //Scatter Shot
            132412, //Seduction (Grimoire of Sacrifice)
            6358, //Seduction (Succubus)
            104045, //Sleep (Metamorphosis)
            19386, //Wyvern Sting
        };


        public static bool DebuffCCBreakonDamage(WoWUnit target)
        {
            return target.ActiveAuras.Any(a => DebuffCCBreakonDamageHS.Contains(a.Value.SpellId));
        }

        #endregion

        #region Get Dispel target

        private static WoWUnit dispelASAPTarget;
        private static DateTime lastGetDispelASAPTargetCheck = DateTime.MinValue;
        public static WoWUnit GetDispelASAPTarget()
        {
            if (lastGetDispelASAPTargetCheck.AddMilliseconds(500) > DateTime.Now && (dispelASAPTarget != null && dispelASAPTarget.IsValidUnit()))
                return dispelASAPTarget;
            lastGetDispelASAPTargetCheck = DateTime.Now;

            return dispelASAPTarget = (from unit in NearbyUnFriendlyPlayers where unit.IsValidUnit() && unit.InLineOfSpellSight() && unit.Distance <= 30 && !unit.HasInvulnerableSpell() && unit.ShouldDispelBuffASAP() select unit).FirstOrDefault();

        }

        #endregion

        #region GetPartyTargetForPowerWordShield

        private static WoWUnit partyTargetForPowerWordShield;
        private static DateTime lastGetPartyTargetForPowerWordShieldCheck = DateTime.MinValue;
        public static WoWUnit GetPartyTargetForPowerWordShield()
        {
            if (lastGetPartyTargetForPowerWordShieldCheck.AddMilliseconds(400) >= DateTime.Now && (partyTargetForPowerWordShield != null && partyTargetForPowerWordShield.IsValidUnit()))
                return partyTargetForPowerWordShield;
            lastGetPartyTargetForPowerWordShieldCheck = DateTime.Now;

            var party = Me.PartyMembers;
            if (!HolySettings.Instance.UseBlanketOnlyInArena)
                party.Concat(Me.RaidMembers);

            partyTargetForPowerWordShield = (from u in party where u.IsValidUnit() && u.Distance2D <= 40 &&
                                                  (!u.HasAura(SpellIDs.WeakenedSoul) || Me.HasAura(SpellIDs.DivineInsight)) && !u.HasAura(SpellIDs.PowerWordShield) && u.InLineOfSpellSight() &&
                                                  (from enemy in NearbyUnFriendlyPlayers where enemy.IsValidUnit() && enemy.CurrentTargetGuid == u.Guid && enemy.TalentSort() != SpecTypes.Healer select enemy).FirstOrDefault() != null 
                                             select u).FirstOrDefault();

            return partyTargetForPowerWordShield;
        }

        #endregion

        #region GetPartyTargetForRenew

        private static WoWUnit partyTargetForRenew;
        private static DateTime lastGetPartyTargetForRenewCheck = DateTime.MinValue;
        public static WoWUnit GetPartyTargetForRenew()
        {
            if (lastGetPartyTargetForRenewCheck.AddMilliseconds(400) >= DateTime.Now && (partyTargetForRenew != null && partyTargetForRenew.IsValidUnit()))
                return partyTargetForRenew;
            lastGetPartyTargetForRenewCheck = DateTime.Now;

            var party = Me.PartyMembers;
            if (!HolySettings.Instance.UseBlanketOnlyInArena)
                party.Concat(Me.RaidMembers);

            partyTargetForRenew = (from u in party where u.IsValidUnit() && u.Distance2D <= 40 && UnitManager.HealTarget.AuraTimeLeft(SpellIDs.Renew) < 1500 && u.InLineOfSpellSight() &&
                                        (from enemy in NearbyUnFriendlyPlayers where enemy.IsValidUnit() && enemy.CurrentTargetGuid == u.Guid && enemy.TalentSort() != SpecTypes.Healer select enemy).FirstOrDefault() != null
                                   select u).FirstOrDefault();

            return partyTargetForRenew;
        }

        #endregion

        #region Best Target For Circle Of Healing

        public static WoWUnit GetBestTargetForAOEHealing(CacheKeys key, int validMS, int range)
        {
            WoWUnit result = null;
            if (CacheHelper.Get(key, validMS, out result))
                return result;

            int count = GetInjuredCount(HolySettings.InjuredPercent);
            int bestCount = -1;

            if (count < HolySettings.Instance.MinInjuredCountToCastAOEHeal)
                return null;

            for (int i = 0; i < NearbyFriendlyPlayers.Count; ++i)
            {
                WoWUnit unit = NearbyFriendlyPlayers[i];
                if (!unit.IsValidUnit())
                    continue;

                int tmpCount = GetInjuredCountInRange(unit, range);
                if (tmpCount > bestCount)
                {
                    bestCount = tmpCount;
                    result = unit;
                }
            }

            if (bestCount < HolySettings.Instance.MinInjuredCountToCastAOEHeal)
                result = null;

            return CacheHelper.Store(key, result);
        }

        public static WoWUnit GetBestTargetForCircleOfHealing()
        {
            return GetBestTargetForAOEHealing(CacheKeys.CircleOfHealing, 1000, 30);
        }

        public static WoWUnit GetBestTargetForHolyWordSanctuary()
        {
            return GetBestTargetForAOEHealing(CacheKeys.HolyWordSanctuary, 1000, 8);
        }

        private static int GetInjuredCountInRange(WoWUnit unit, int range)
        {
            if (injuredUnits == null)
                return 0;

            int count = 0;

            for (int i = 0; i < injuredUnits.Count; ++i)
            {
                WoWUnit u = injuredUnits[i];
                if (!u.IsValidUnit())
                    continue;

                if (unit.Location.Distance(u.Location) < range)
                    count++;
            }

            return count;
        }

        #endregion

        #endregion
    }
}