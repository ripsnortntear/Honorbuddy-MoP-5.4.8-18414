using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx.CommonBot;

namespace RichieShadowPriestPvP
{
    public enum CacheKeys : int
    {
        CircleOfHealing,
        HolyWordSanctuary,
        PolyReact
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
        public static WoWUnit ASAPDamageTarget { get; private set; }
        public static WoWUnit PolyTarget { get; private set; }
        public static bool? HasSWDGlyph { get; set; }

        public static WoWPlayer ScatteredTeammate { get; private set; }
        public static WoWPlayer VIP { get; set; }
        public static WoWUnit MassDispelTarget { get; private set; }
        public static WoWUnit PurifyASAPTarget { get; private set; }
        public static WoWUnit PurifyLowPrioTarget { get; private set; }
        public static int PurifyLowPrioTargetDebuffCount { get; private set; }
        public static WoWUnit MultidotTarget { get; private set; }
        public static WoWUnit VTTarget { get; private set; }        
        public static WoWUnit VoidShiftTarget { get; private set; }

        #endregion

        #region Refresh

        public static void Refresh() {

            AllUnits.Clear();

            Clear();

            NearbyFriendlyUnits.Add(Me);
            NearbyFriendlyPlayers.Add(Me);

            HealTarget = Me;
            List<WoWUnit> wowUnits;
            // Stores all valid players, pets and totems.
            using (var perf = PerfLogger.GetHelper("___Get LoS units"))
                using (StyxWoW.Memory.AcquireFrame())
                    wowUnits = ObjectManager.GetObjectsOfTypeFast<WoWUnit>().Where(u => u.InLineOfSight()).ToList();

            for (int i = 0; i < wowUnits.Count; ++i) {
                WoWUnit unit = wowUnits[i];
                if (!unit.IsValidUnit() || unit.Distance2D > 40 || (!unit.IsPlayer && !unit.IsPet && !unit.IsTotem && !unit.IsTrainingDummy())/* || !unit.InLineOfSight()*/)
                    continue;

                if (unit.IsMyPartyRaidMember()) {

                    if (unit.IsPlayer || (Main.IsArena && SPSettings.Instance.HealPets && unit.IsPet)) {
                        if (unit != Me)
                            NearbyFriendlyUnits.Add(unit);

                        // Search for Heal Target
                        if (SPSettings.Instance.HealOthers && Main.IsProVersion) {

                            if (SPSettings.Instance.OnlyHealVIP && !Main.IsArena) {
                                if (unit.HealthPercent < HealTarget.HealthPercent && (unit.HasAura("Alliance Flag") ||
                                unit.HasAura("Horde Flag") || unit.HasAura("Netherstorm Flag") || unit.HasAura("Orb of Power") ||
                                (Me.FocusedUnit != null && Me.FocusedUnit.IsValidUnit() && Me.FocusedUnit == unit) ||
                                unit == Me))
                                    HealTarget = unit;
                            } else
                                if (unit.HealthPercent < HealTarget.HealthPercent)
                                    HealTarget = unit;
                        }

                        var player = unit as WoWPlayer;

                        if (player != null && player != Me)
                            NearbyFriendlyPlayers.Add(player);
                    }
                } else if (unit.IsEnemy()) {
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

        public static void RefilterUnitsForHeal() {

            HealTarget = Me;
            //Only heal myself
            if (!SPSettings.Instance.HealOthers || !Main.IsProVersion)
                return;

            //Only heal VIP, unless we're in arena
            if (SPSettings.Instance.OnlyHealVIP && !Main.IsArena) {

                foreach (WoWUnit unit in NearbyFriendlyPlayers) {
                    if (unit.IsValidUnit() &&
                        unit.HealthPercent < HealTarget.HealthPercent && (unit.HasAura("Alliance Flag") ||
                                unit.HasAura("Horde Flag") ||
                                unit.HasAura("Netherstorm Flag") ||
                                unit.HasAura("Orb of Power") ||
                                (Me.FocusedUnit != null &&
                                Me.FocusedUnit.IsValidUnit() &&
                                Me.FocusedUnit == unit) ||
                                unit == Me)) {

                        // Search for Heal Target
                        HealTarget = unit;
                        
                    }
                }

                if (!HealTarget.IsValidUnit())
                    HealTarget = Me;

                return;
            }

            for (int i = 0; i < AllUnits.Count; ++i) {
                WoWUnit unit = AllUnits[i];

                if (unit.IsValidUnit() && unit.IsMyPartyRaidMember()) {

                    if (unit.IsPlayer || (Main.IsArena && SPSettings.Instance.HealPets && unit.IsPet)) {
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
            ASAPDamageTarget = null;
            numberOfMeleeTargetingMe = null;
        }

        #endregion

        #region Filtering Units

        public static bool NeedPolyReact() {
            using (var perf = PerfLogger.GetHelper("NeedPolyReact")) {
                WoWUnit result;
                if (CacheHelper.Get<WoWUnit>(CacheKeys.PolyReact, 200, out result))
                    return result != null;

                if (!HasSWDGlyph.HasValue)
                    HasSWDGlyph = GlyphManager.Has(Glyphs.GlyphOfShadowWordDeath);

                if (!(HasSWDGlyph ?? false)) {
                    CacheHelper.Store(CacheKeys.PolyReact, PolyTarget = null);
                    return false;
                }

                for (int i = 0; i < NearbyUnFriendlyPlayers.Count; ++i) {
                    var u = NearbyUnFriendlyPlayers[i];
                    if (u.IsValidUnit() && u.IsTargetingMeOrPet && (u.CastingSpellId == (uint)SpellIDs.Polymorph ||
                                                                    u.CastingSpellId == (uint)SpellIDs.Repentance ||
                                                                    u.CastingSpellId == (uint)SpellIDs.BlindingLight)) {
                        CacheHelper.Store(CacheKeys.PolyReact, PolyTarget = u);
                        return true;
                    }
                }

                CacheHelper.Store(CacheKeys.PolyReact, PolyTarget = null);
                return false;
            }
        }

        private static uint[] TotemPriorities = new uint[] { (uint)SpellIDs.SpiritLinkTotem, (uint)SpellIDs.TremorTotem, (uint)SpellIDs.GroundingTotem, (uint)SpellIDs.CapacitorTotem, 
                                                             (uint)SpellIDs.HealingStreamTotem, (uint)SpellIDs.WindwalkTotem, (uint)SpellIDs.EarthgrabTotem, (uint)SpellIDs.EarthbindTotem };
        public static bool SelectASAPDamageTarget()
        {

            if (!ASAPDamageTarget.IsValidUnit())
                ASAPDamageTarget = null;

            if (ASAPDamageTarget != null)
                return true;


            if (NearbyUnFriendlyUnits.Count() > 0) {

                ASAPDamageTarget = (from unit in NearbyUnFriendlyUnits
                                    where unit.IsValidUnit() && ((unit.HealthPercent <= 20 && !unit.IsInvulnerableSpell()) || unit.HasAura(SpellIDs.GroundingTotemEffect))
                                    select unit).FirstOrDefault();

            }


            if (!ASAPDamageTarget.IsValidUnit() && NearbyTotems.Count > 0) {
                // Prefilter totems
                var priorityTargets = from totem in NearbyTotems
                                      where totem.IsValidUnit() && (totem.CreatedBySpellId == TotemPriorities[0] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[1] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[2] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[3] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[4] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[5] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[6] ||
                                                                    totem.CreatedBySpellId == TotemPriorities[7]) &&
                                                                    LoSer.InLineOfSight(totem)
                                      select totem;

                for (int i = 0; i < TotemPriorities.Length; ++i)
                {
                    ASAPDamageTarget = (from totem in priorityTargets where totem.CreatedBySpellId == TotemPriorities[i] select totem).FirstOrDefault();

                    if (ASAPDamageTarget != null)
                        break;
                }

                /*if (ASAPDamageTarget == null)
                    ASAPDamageTarget = NearbyTotems[0];*/
            }

            return ASAPDamageTarget != null;
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
        public static int NumberOfEnemyInRange(int range = 10, bool OnlyPlayers = true)
        {
            if (lastNumberOfEnemyInRangeCheck.AddMilliseconds(1500) > DateTime.Now && numberOfEnemyInRange.HasValue)
                return numberOfEnemyInRange.Value;
            lastNumberOfEnemyInRangeCheck = DateTime.Now;

            numberOfEnemyInRange = NearbyUnFriendlyUnits.Count(unit => unit.IsValidUnit() && unit.Distance2D < range && (!OnlyPlayers || (OnlyPlayers && unit.IsPlayer)));

            return numberOfEnemyInRange ?? 0;
        }

        #endregion

        #region Check if Halo is safe;

        public static bool IsHaloSafe() {

            return (from unit in NearbyUnFriendlyUnits
                    where unit.Distance2D < 33
                    where unit.HasBreakOnDamageCC()
                    select unit).FirstOrDefault() == null;
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
            else //only use it to dispel when purify is on CD
                MassDispelTarget = (from unit in NearbyFriendlyPlayers
                                    where unit.InLineOfSpellSight()
                                    where unit.Distance2D <= 30
                                    where unit.DebuffCCleanseASAP()
                                    where Main.IsArena || unit.TalentSort() == SpecTypes.Healer //outside of arenas only use it to dispel healers
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


        #region VIP

        public static bool GetVIP() {

            if (VIP == null && Me.FocusedUnit != null && Me.FocusedUnit.IsValidUnit() && Me.FocusedUnit.IsPlayer && Me.FocusedUnit.Distance <= 40)
                VIP = (WoWPlayer)Me.FocusedUnit;

            if (VIP == null)
                VIP = NearbyFriendlyPlayers.Where(unit => unit.CanSelect &&
                        !unit.IsDead &&
                        unit.IsInMyPartyOrRaid &&
                        unit.Distance <= 40 &&
                        (unit.HasAura("Alliance Flag") ||
                        unit.HasAura("Horde Flag") || 
                        unit.HasAura("Netherstorm Flag") || 
                        unit.HasAura("Orb of Power")))
                    .FirstOrDefault();

            return VIP != null;
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


        public static bool DebuffCCBreakonDamage(WoWUnit target) {
            return target.HasBreakOnDamageCC();
        }

        #endregion



        #region Get Dispel target

        private static WoWUnit dispelASAPTarget;
        private static DateTime lastGetDispelASAPTargetCheck = DateTime.MinValue;
        public static WoWUnit GetDispelASAPTarget()
        {
            if (lastGetDispelASAPTargetCheck.AddMilliseconds(500) > DateTime.Now && dispelASAPTarget.IsValidUnit())
                return dispelASAPTarget;
            lastGetDispelASAPTargetCheck = DateTime.Now;

            return dispelASAPTarget = (from unit in NearbyUnFriendlyPlayers where unit.IsValidUnit() && unit.InLineOfSpellSight() && unit.Distance <= 30 && !unit.IsInvulnerableSpell() && unit.ShouldDispelBuffASAP() select unit).FirstOrDefault();

        }

        #endregion

        #endregion

        #region NumberOfUndottedUnits

        private static int getNumberOfUndottedUnits() {

            if (!SPSettings.Instance.Multidot) {

                if (!Me.GotTarget)
                    return 0;

                return !(Me.CurrentTarget.MyAuraTimeLeft(SpellIDs.VampiricTouch) > SPSettings.Instance.VampiricTouchRefresh &&
                Me.CurrentTarget.MyAuraTimeLeft(SpellIDs.ShadowWordPain) > SPSettings.Instance.SWPRefresh) ? 1 : 0;
            }

            return (from unit in NearbyUnFriendlyUnits
                    //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                    where unit != null && unit.IsValid
                    where !(unit.MyAuraTimeLeft(SpellIDs.VampiricTouch) > SPSettings.Instance.VampiricTouchRefresh &&
                        unit.MyAuraTimeLeft(SpellIDs.ShadowWordPain) > SPSettings.Instance.SWPRefresh)
                    //where unit.InLineOfSpellSight
                    where !unit.IsInvulnerableSpell()
                    select unit).Count();

        }

        #endregion 

        #region Get Multidot target

        public static void GetMultidotTarget() {

            if (MultidotTarget != null && ((MultidotTarget.IsValidUnit() && MultidotTarget.Distance2D > 40)
                || !MultidotTarget.IsValidUnit()
                || !NearbyUnFriendlyUnits.Contains(MultidotTarget) //InLineOfSpellSight hack
                || (
                    (MultidotTarget.MyAuraTimeLeft(SpellIDs.VampiricTouch) > SPSettings.Instance.VampiricTouchRefresh || Me.IsMoving) &&
                        MultidotTarget.MyAuraTimeLeft(SpellIDs.ShadowWordPain) > SPSettings.Instance.SWPRefresh
                )
                || MultidotTarget.IsInvulnerableSpell())) {
                Blacklist.Add(MultidotTarget.Guid, BlacklistFlags.Combat, TimeSpan.FromSeconds(3));
                MultidotTarget = null;
            }

            if (MultidotTarget != null) {
                return;
            }

            MultidotTarget = (from unit in NearbyUnFriendlyUnits
                                //to prevent selecting the same target again for a few seconds
                                where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                                where unit.IsValidUnit()
                                where !unit.IsInvulnerableSpell()                                
                                where unit.Entry != 19833 && unit.Entry != 19921 //Don't target snake trap snakes                                
                                where unit.MyAuraTimeLeft(SpellIDs.VampiricTouch) <= SPSettings.Instance.VampiricTouchRefresh ||
                                    unit.MyAuraTimeLeft(SpellIDs.ShadowWordPain) <= SPSettings.Instance.SWPRefresh
                                orderby unit.IsPlayer ? 2 : 1 descending // target players first
                                select unit).FirstOrDefault();


            return;
        }

        #endregion

        #region Get High Priority Vampiric Touch target

        public static bool GetVTTarget() {

            VTTarget = (from unit in NearbyUnFriendlyUnits
                              //to prevent selecting the same target again for a few seconds
                              where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                              where unit.IsValidUnit()
                              where !unit.IsInvulnerableSpell()
                              where unit.Entry != 19833 && unit.Entry != 19921 //Don't target snake trap snakes
                              where unit.MyAuraTimeLeft(SpellIDs.VampiricTouch) <= SPSettings.Instance.VampiricTouchRefresh
                              orderby unit.IsPlayer ? 2 : 1 descending // target players first
                              select unit).FirstOrDefault();

            return VTTarget != null;
        }

        #endregion

    }
}