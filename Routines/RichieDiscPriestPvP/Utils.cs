using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace RichieDiscPriestPvP {

    public partial class Main {

	
        #region Global Variables

		private static int PeelDelay = 3;
        private static int CCDelay = 3;
        private static double MyLatency;
        private static int PurifyLowPrioTargetDebuffCount = 0;
        private static int InjuredUnitCount = 0;
        private static int SmiteRange = 30;
        private static int featherDelay = 10;
        private static int LifeSaverCDDelay = 6;

        private static WoWUnit MyLastTarget;
        private static WoWUnit LastCastUnit;
        private static WoWUnit MassDispelTarget;
        private static WoWUnit HealTarget;
        private static WoWUnit GriefTarget;
        private static WoWUnit ShackleTarget;
        private static WoWUnit LoFScatterTarget;
        private static WoWUnit ASAPAttackTarget;
        private static WoWUnit CCTarget;
        private static WoWUnit PurifyASAPTarget;
        private static WoWUnit PurifyLowPrioTarget;
        private static WoWUnit AtonementAttackTarget;
        private static WoWUnit PsyfiendPeelTarget;
        private static WoWUnit PWSTarget;
        private static WoWUnit RenewTarget;
        
        private static DateTime LastCastTime = DateTime.Now;
        private static DateTime LastUpdateMyLatency;
        private static readonly DateTime Now = DateTime.Now;
        private static DateTime LastPeel = DateTime.Now;
        private static DateTime LastCC = DateTime.Now;
        private static DateTime LastClickRemoteLocation;
        private static DateTime LastLifeSaverCDPopped = DateTime.Now;
        private static DateTime LastFeather = DateTime.Now;
        private static DateTime LastWaitForFacing = DateTime.Now;
        
        private static string LastCastSpell = "";
        private static string GlyphIDs = "";
        private static string GlyphNames = "";
        private static string TalentNames = "";
		
        private static readonly List<WoWPlayer> FarFriendlyPlayers = new List<WoWPlayer>();
        private static readonly List<WoWUnit> FarFriendlyUnits = new List<WoWUnit>();
        private static readonly List<WoWPlayer> NearbyFriendlyPlayers = new List<WoWPlayer>();
        private static readonly List<WoWUnit> NearbyFriendlyUnits = new List<WoWUnit>();
        private static readonly List<WoWPlayer> NearbyUnFriendlyPlayers = new List<WoWPlayer>();
        private static readonly List<WoWUnit> NearbyUnFriendlyUnits = new List<WoWUnit>();
        private static readonly List<WoWUnit> NearbyTotems = new List<WoWUnit>();

        #endregion	
		
		#region MiscSetups

        private static Composite MiscSetups() {
		
			return new Action(
				delegate {
					//Hold All Action On Mass Dispel
					if (Me.HasPendingSpell("Mass Dispel") || Me.HasPendingSpell("Psyfiend")) {
						Logging.Write(LogLevel.Diagnostic,
									  DateTime.Now.ToString("ss:fff ") +
									  "Hold All Action On Mass Dispel/Psyfiend");
						return RunStatus.Success;
					}

					//Hold All Action On Tranqulity
					if (Me.IsChanneling && Me.ChanneledCastingSpellId == 740) {
						Logging.Write(LogLevel.Diagnostic,
									  DateTime.Now.ToString("ss:fff ") +
									  "Hold All Action On Tranqulity");
						return RunStatus.Success;
					}

					//Hold rotation on Hymn of Hope
					if (Me.IsChanneling && Me.ChanneledCastingSpellId == 64901) {
						Logging.Write(LogLevel.Diagnostic,
									  DateTime.Now.ToString("ss:fff ") +
									  "Hold All Action On Hymn of Hope");
						return RunStatus.Success;
					}

					//Clear Target if dead and still in combat
					if (Me.CurrentTarget != null && !Me.CurrentTarget.IsAlive && Me.Combat) {
						Lua.DoString("RunMacroText(\"/cleartarget\")");
					}

					return RunStatus.Failure;
				}
			);
        }
	
		#endregion
	
        #region Attackable

        private static bool Attackable(WoWUnit target) {
            if (ValidUnit(target) && target.Distance - target.BoundingRadius < 40 &&
                IsEnemy(target) && target.InLineOfSpellSight) {
                return true;
            }
            return false;
        }

        //prevent double ValidUnit Check
        private static bool AttackableValid(WoWUnit target) {
            if (target.Distance - target.BoundingRadius < 40 && IsEnemy(target) &&
                target.InLineOfSpellSight && target.IsAlive) {
                return true;
            }
            return false;
        }

        #endregion



        #region CastSpell

        private static void CastSpell(string spellName, WoWUnit u) {
            CastSpell(spellName, u, WoWPoint.Zero, "");
        }

        private static void CastSpell(string spellName, WoWUnit u, string message) {
            CastSpell(spellName, u, WoWPoint.Zero, message);
        }


        private static void CastSpell(string spellName, WoWUnit u, WoWPoint location) {
            CastSpell(spellName, u, location, "");
        }

        private static void CastSpell(string spellName, WoWUnit u, WoWPoint location, string message) {

            if (u == null || !u.IsValid) {
                return;
            }

            while (GCDReady > DateTime.Now) { }

            SpellManager.Cast(spellName, u);

            if (location != WoWPoint.Zero) {
                LastClickRemoteLocation = DateTime.Now;
                while (LastClickRemoteLocation + TimeSpan.FromMilliseconds(100) < DateTime.Now &&
                       Me.CurrentPendingCursorSpell == null) {
                    Logging.Write("Waiting for getting the targeting circle for " + spellName + ".");
                }

                message += "-distance: " + location.Distance2D(u.Location);
                SpellManager.ClickRemoteLocation(location);
                Lua.DoString("SpellStopTargeting()");
            }            

            Color colorlog;
            if (u == Me) {
                colorlog = Colors.GreenYellow;
            } else {
                if (Me.CurrentTarget != null && u == Me.CurrentTarget) {
                    colorlog = Colors.Red;
                } else {
                    colorlog = Colors.Yellow;
                }
            }

            //Prevent spamming
            if (LastCastUnit != u || spellName != LastCastSpell || LastCastTime.AddMilliseconds(300) < DateTime.Now) {
                string barTwo = "Mana: " + Math.Round(Me.ManaPercent) + "%";
                string messageCol = message == null || message.Length == 0 ? "" : " (" + message + ")";

                string unitName;
                if (u == Me) {
                    unitName = "Me";
                } else {
                    if (u.IsPlayer) {
                        unitName = u.Name + "(" + u.Class.ToString() + ")";
                    } else {
                        unitName = u.Name;
                    }
                }

                Logging.Write(colorlog, DateTime.Now.ToString("ss:fff") + " - HP: " +
                                        Math.Round(Me.HealthPercent) + "% - " +
                                        barTwo + " - " +
                                        unitName + " - " + Math.Round(u.Distance) + "y - " +
                                        Math.Round(u.HealthPercent) + "% hp - " + spellName + messageCol + " - NU: " + NearbyUnFriendlyUnits.Count() + " - Injured: " + InjuredUnitCount + (HealTarget != null && HealTarget.IsValid ? " - Healtarget: " + Math.Round(HealTarget.HealthPercent) : "Invalid HealTarget"));
            }

            LastCastTime = DateTime.Now;
            LastCastSpell = spellName;
            LastCastUnit = u;

            UpdateGCD();

            if (LastUpdateMyLatency.AddMinutes(3) < DateTime.Now) {
                UpdateMyLatency();
                LastUpdateMyLatency = DateTime.Now;
            }
        }

        #endregion

        #region GCD

        private static DateTime GCDReady;

        private static void UpdateGCD() {
            GCDReady = DateTime.Now + SpellManager.Spells["Inner Fire"].CooldownTimeLeft;
        }

        private static bool GCDL() {
            return DateTime.Now + TimeSpan.FromMilliseconds(MyLatency) <= GCDReady;
        }

        private static bool Casting()
        {
            //Mind Flay
            if (Me.IsChanneling && Me.ChanneledCastingSpellId == 15407)
            {
                return false;
            }
            
            if (Me.HasPendingSpell("Mass Dispel") || Me.HasPendingSpell("Psyfiend"))
            {
                return true;
            }
			
            if (Me.IsCasting && Me.CurrentCastTimeLeft.TotalMilliseconds > MyLatency ||
                Me.IsChanneling && Me.CurrentChannelTimeLeft.TotalMilliseconds > MyLatency) {
                return true;
            }

            return false;
        }

        private static bool CastingorGCDL() {
            return Casting() || GCDL();
        }

        #endregion

        #region Stop Casting

        private static void StopCasting() {

            if (Me.IsCasting || Me.IsChanneling) {
                if (DiscSettings.Instance.DontCancelDominateMind && Me.IsChanneling && Me.ChanneledSpell.Name == "Dominate Mind") {
                    Logging.Write("Not cancelling Dominate Mind");
                    return;
                }
                SpellManager.StopCasting();
                Logging.Write("Spellcasting cancelled");
            }
        }

        #endregion

        #region Moving - Internal Extentions
        //Thanks for the PureRotation developers

        /// <summary>
        /// Internal IsMoving check which ignores turning on the spot.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool IsMoving(WoWUnit unit) {
            return unit.MovementInfo.MovingBackward || unit.MovementInfo.MovingForward || unit.MovementInfo.MovingStrafeLeft || unit.MovementInfo.MovingStrafeRight;
        }

        /// <summary>
        /// Internal IsMoving check which ignores turning on the spot, and allows specifying how long you've been moving for before accepting it as actually moving. 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="movingDuration">Duration in MS how long the unit has been moving before accepting it as a moving unit</param>
        /// <returns></returns>
        public static bool IsMoving(WoWUnit unit, int movingDuration) {
            //return unit.IsMoving && unit.MovementInfo.TimeMoved >= movingDuration;
            return IsMoving(unit) && unit.MovementInfo.TimeMoved >= movingDuration;
        }

        #endregion

        #region CountEnemyNearby

        private static double CountEnemyNearby(WoWObject unitCenter, float distance) {
            
			return NearbyUnFriendlyUnits.Where(
                    unit =>
                    unitCenter.Location.Distance(unit.Location) <= distance &&
                    !unit.IsPet
                    ).Count();
        }

        #endregion             

        #region CountEnemyTargetingUnit

        private static double CountEnemyTargetingUnit(WoWUnit target) {

            if (target == null || !target.IsValid) {
                return 0;
            }

            return NearbyUnFriendlyUnits.Where(
                    unit =>
                        unit != null && unit.IsValid &&
                        unit.CurrentTarget != null && unit.CurrentTarget.IsValid && unit.CurrentTarget == target
                    ).Count();
        }

        #endregion  

        #region Get PWS target

        private static bool GetPWSTarget() {


            PWSTarget = (from unit in NearbyFriendlyPlayers
                         //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                         where unit.IsValid
                         where unit.InLineOfSpellSight
                         where (!unit.HasAura("Weakened Soul") || Me.HasAura(123266)) && // Divine Insight
                            !unit.HasAura("Power Word: Shield")
                         orderby CountEnemyTargetingUnit(unit) descending
                         orderby unit.HealthPercent ascending
                         select unit).FirstOrDefault();

            return PWSTarget != null && CountEnemyTargetingUnit(PWSTarget) >= 1;
        }

        #endregion
        

        #region Get Renew target

        private static bool GetRenewTarget() {

            RenewTarget = (from unit in NearbyFriendlyPlayers
                         where unit.IsValid
                         where unit.InLineOfSpellSight
                         where !HasMyAura(unit, "Renew", 0)
                         orderby CountEnemyTargetingUnit(unit) descending
                         orderby unit.HealthPercent ascending
                         select unit).FirstOrDefault();

            return RenewTarget != null && CountEnemyTargetingUnit(RenewTarget) >= 1;
        }

        #endregion


        #region GetIsHaloSafe;


        private static bool IsHaloSafe() {

            return (from unit in NearbyUnFriendlyUnits
                            where unit.Distance2D < 33
                            where unit.InLineOfSpellSight
                            where unit.InLineOfSight
                            where DebuffCCBreakonDamage(unit)
                            select unit).FirstOrDefault() == null;
        }

        #endregion

        #region Get Psyfiend target

        private static bool GetPsyfiendPeelTarget(WoWUnit peelFrom) {

            if (peelFrom == null || !peelFrom.IsValid) {
                PsyfiendPeelTarget = null;
                return false;
            }

            PsyfiendPeelTarget = (from unit in NearbyUnFriendlyPlayers
                                  where unit.IsValid
                                  where AttackableValid(unit)
                                  where unit.CurrentTarget != null && unit.CurrentTarget.IsValid && ((peelFrom == Me && unit.CurrentTarget == Me) ||
                                     (peelFrom != Me && unit.CurrentTarget == peelFrom))
                                  where unit.Distance <= 25
                                  where !InvulnerableSpell(unit)
                                  orderby unit.Distance ascending
                                  select unit).FirstOrDefault();

            return PsyfiendPeelTarget != null;
        }

        #endregion

        #region GetShackleTarget

        private static bool GetShackleTarget()
        {

            ShackleTarget = (from unit in NearbyUnFriendlyUnits
                                where unit.IsValid
                                where AttackableValid(unit)                                                                  
                                where //unit.Entry == 59190 || //Psyfiend
                                        unit.HasAura("Lichborne") ||
                                        unit.HasAura("Dark Transformation") ||
                                        unit.Name == "Ebon Gargoyle"
                                where unit.Distance < 30
                                where !unit.Name.Contains("Army")
                                where !InvulnerableSpell(unit)
                                where !DebuffDot(unit)
                                where !DebuffCC(unit)
                                orderby unit.Distance ascending
                                select unit).FirstOrDefault();
            return ShackleTarget != null;
        }

        #endregion

        #region Get Atonement Target

        private static bool GetAtonementAttackTarget() {

            AtonementAttackTarget = (from unit in NearbyUnFriendlyPlayers
                                where unit.IsValid
                                where AttackableValid(unit)
                                where unit.Distance <= SmiteRange
                                where !InvulnerableSpell(unit)
                                where !DebuffCCBreakonDamage(unit)
                                orderby unit.HealthPercent ascending
                                select unit).FirstOrDefault();

            return AtonementAttackTarget != null;
        }

        #endregion

        #region GetLoFScatterTarget

        private static bool GetLoFScatterTarget()
        {

            LoFScatterTarget = (from unit in NearbyFriendlyUnits
                                where unit.IsValid
                                where unit.Distance < 40
                                where unit.HasAura(19503) //Scatter Shot
                                where TalentSort(unit) == 4
                                orderby unit.Distance ascending
                                select unit).FirstOrDefault();
            return LoFScatterTarget != null;
        }

        #endregion

        #region Get Void Shift griefing target

        private static bool GetGriefTarget()
        {

            GriefTarget = (from unit in NearbyFriendlyUnits
                         where unit.IsValid
                         orderby unit.HealthPercent descending
                         where unit.IsPlayer
                         where !unit.IsMe
                         where !unit.HasAura("Alliance Flag") && !unit.HasAura("Horde Flag") &&
                                !unit.HasAura("Netherstorm Flag") && !unit.HasAura("Orb of Power")
                         where unit.InLineOfSpellSight
                         select unit).FirstOrDefault();

            return GriefTarget != null;
        }

        #endregion   

        #region GetMassDispelTarget

        private static bool GetMassDispelTarget() {

            if (GlyphNames.Contains("Glyph of Mass Dispel")) {
                MassDispelTarget = (from unit in NearbyUnFriendlyUnits
                                    //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                                    where MassDispelableInvulnerability(unit)
                                    where unit.InLineOfSpellSight
                                    where unit.Distance2D <= 30
                                    select unit).FirstOrDefault();
            } else {
                MassDispelTarget = null;
            }

            //only use it to dispel when purify is on CD
            if (MassDispelTarget == null && SpellManager.Spells["Purify"].CooldownTimeLeft.TotalMilliseconds >= MyLatency) {
                MassDispelTarget = (from unit in NearbyFriendlyPlayers
                                    //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                                    where unit.InLineOfSpellSight
                                    where unit.Distance2D <= 30
                                    where DebuffCCleanseASAP(unit)
                                    where Me.CurrentMap.IsArena || TalentSort(unit) == 4 //outside of arenas only use it to dispel healers
                                    where !(unit.IsChanneling && unit.ChanneledSpell.Name == "Dominate Mind" ) //don't try to dispel DM if the teammate is casting it
                                    select unit).FirstOrDefault();

            }

            return MassDispelTarget != null;
        }

        #endregion   

        #region IsEnemyOrPartyMember

        private static bool IsMyPartyRaidMember(WoWUnit u) {

			if (u == null || !u.IsValid) {
				return false;
			}

			if (!u.IsPlayer) {
				if (Me.PartyMembers.Contains(u.CreatedByUnit) || Me.RaidMembers.Contains(u.CreatedByUnit)) {
					return true;
				}
			} else {
				if (Me.PartyMembers.Contains(u) || Me.RaidMembers.Contains(u)) {
					return true;
				}
			}

			return false;
        }

        private static bool IsEnemy(WoWUnit target) {
            
			if (target == null) {
                return false;
            }

            if (IsMyPartyRaidMember(target)) {
                return false;
            }

            if (Me.CurrentMap.IsArena || Me.CurrentMap.IsBattleground) {
                return true;
            }

            if (!target.IsFriendly || target.Name.Contains("Dummy")) {
                return true;
            }
			
            return false;
        }

        #endregion

        #region Get nearby totems and Mind Controlling priests

        private static bool GetASAPAttackTarget()
        {
            /*if (!Me.CurrentMap.IsArena) {
                return false;
            }*/


            ASAPAttackTarget = (from unit in NearbyUnFriendlyUnits
                                //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                                where unit.IsValid
                                where unit.HealthPercent <= 20 || unit.HasAura("Grounding Totem Effects")
                                where unit.InLineOfSpellSight
                                where !InvulnerableSpell(unit) || unit.HasAura("Grounding Totem Effects")
                                select unit).FirstOrDefault();


            if (ASAPAttackTarget == null) {
                ASAPAttackTarget = (from unit in NearbyTotems
                                    where unit.IsValid
                                    //where AttackableValid(unit)
                                    where unit.InLineOfSpellSight
                                    where
                                        (unit.Name == "Spirit Link Totem" ||
                                        unit.Name == "Grouding Totem" ||
                                        unit.Name == "Earthgrab Totem" ||
                                        unit.Name == "Healing Tide Totem" ||
                                        unit.Name == "Mana Tide Totem" ||
                                        unit.Name == "Capacitor Totem" ||
                                        unit.Name == "Earthbind Totem")
                                    select unit).FirstOrDefault();
            }

            /*if (ASAPAttackTarget == null) {
                ASAPAttackTarget = (from unit in NearbyUnFriendlyPlayers
                                    where unit.IsValid
                                    where AttackableValid(unit)
                                    where unit.HasAura("Dominate Mind")
                                    select unit).FirstOrDefault();
            }*/

            return ASAPAttackTarget != null;
            
        }

        #endregion

        #region Get CC target

        private static bool GetCCTarget() {
            /*if (!Me.CurrentMap.IsArena) {
                return false;
            }*/

            CCTarget = null;

            if (Me.FocusedUnit != null 
                && Me.FocusedUnit.IsValid                
                && Me.FocusedUnit.InLineOfSpellSight
                && !InvulnerableSpell(Me.FocusedUnit)
                && !DebuffCC(Me.FocusedUnit)
                && !DebuffSilence(Me.FocusedUnit)) {
                    CCTarget = Me.FocusedUnit;
            }

            if (CCTarget == null) {
                CCTarget = (from unit in NearbyUnFriendlyPlayers
                        where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                        where unit.IsValid
                        where unit.InLineOfSpellSight
                        where TalentSort(unit) == 4
                        where !InvulnerableSpell(unit)
                        where !DebuffCC(unit)
                        where !DebuffSilence(unit)
                        select unit).FirstOrDefault();
            }

            return CCTarget != null;

        }

        #endregion

        #region Get Purify target

        private static bool GetPurifyASAPTarget() {
            /*if (!Me.CurrentMap.IsArena) {
                return false;
            }*/

            PurifyASAPTarget = null;

            PurifyASAPTarget = (from unit in NearbyFriendlyPlayers
                        where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                        where unit.IsValid
                        where unit.InLineOfSpellSight
                        where !InvulnerableSpell(unit)
                        where DebuffCCleanseASAP(unit)
                        where !DebuffDoNotDispel(unit)
                        orderby TalentSort(unit) descending
                        select unit).FirstOrDefault();

            return PurifyASAPTarget != null;

        }


        private static bool GetPurifyLowPrioTarget() {

            PurifyLowPrioTarget = null;

            PurifyLowPrioTarget = (from unit in NearbyFriendlyPlayers
                        where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                        where unit.IsValid
                        where unit.InLineOfSpellSight
                        where !InvulnerableSpell(unit)
                        where !DebuffDoNotDispel(unit)                        
                        orderby CountDebuff(unit) descending
                        orderby TalentSort(unit) descending
                        select unit).FirstOrDefault();

            if (PurifyLowPrioTarget != null) {
                PurifyLowPrioTargetDebuffCount = CountDebuff(PurifyLowPrioTarget);
                return true;
            }

            PurifyLowPrioTargetDebuffCount = 0;
            return false;

        }
        #endregion

        #region CountDebuff

        private static int CountDebuff(WoWUnit u) {
            if (u == null || !u.IsValid || !u.IsAlive)
                return 0;

            int numberofDebuff =
                u.Debuffs.Values.Count(
                    debuff =>
                    (debuff.Spell.DispelType == WoWDispelType.Magic ||
                        debuff.Spell.DispelType == WoWDispelType.Disease));

            return numberofDebuff;
        }

        #endregion

        #region CanUseEquippedItem

        //Thanks Apoc
        private static bool CanUseEquippedItem(WoWItem item) {
            // Check for engineering tinkers!
            if (string.IsNullOrEmpty(Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0))) {
                return false;
            }

            return item.Usable && item.Cooldown <= MyLatency;
        }

        #endregion

        #region TalentSort

        private static byte TalentSort(WoWUnit target) {
            if (target == null) {
                return 0;
            }

            if (!target.IsPlayer) {
                return 0;
            }

            if (target.Class == WoWClass.DeathKnight) {
                return 1;
            }

            if (target.Class == WoWClass.Druid) {
                if (target.Buffs.ContainsKey("Moonkin Form"))
                    return 3;
                if (target.MaxMana < target.MaxHealth / 2 ||
                    (target.Buffs.ContainsKey("Leader of the Pack") &&
                     target.Buffs["Leader of the Pack"].CreatorGuid == target.Guid))
                    return 1;
                return 4;
            }

            if (target.Class == WoWClass.Hunter) {
                return 2;
            }

            if (target.Class == WoWClass.Mage) {
                return 3;
            }

            if (target.Class == WoWClass.Monk) {
                if (target.HasAura("Stance of the Wise Serpent"))
                    return 4;
                return 1;
            }

            if (target.Class == WoWClass.Paladin) {
                if (target.MaxMana > target.MaxMana / 2)
                    return 4;
                return 1;
            }

            if (target.Class == WoWClass.Priest) {
                if (target.HasAura("Shadowform"))
                    return 3;
                return 4;
            }

            if (target.Class == WoWClass.Rogue) {
                return 1;
            }

            if (target.Class == WoWClass.Shaman) {
                if (target.MaxMana < target.MaxHealth / 2)
                    return 1;
                if (target.Buffs.ContainsKey("Elemental Oath") &&
                    target.Buffs["Elemental Oath"].CreatorGuid == target.Guid)
                    return 3;
                return 4;
            }

            if (target.Class == WoWClass.Warlock) {
                return 3;
            }

            if (target.Class == WoWClass.Warrior) {
                return 1;
            }

            return 0;
        }

        private static byte TalentSortSimple(WoWUnit target) {
            byte sortSimple = TalentSort(target);

            if (sortSimple == 4) {
                return 4;
            }

            if (sortSimple < 4 && sortSimple > 0) {
                return 1;
            }

            return 0;
        }

        #endregion


        #region MyAuraTimeLeft

        private static double MyAuraTimeLeft(string auraName, WoWUnit u) {

            if (u == null || !u.IsValid || !u.IsAlive)
                return 0;

            WoWAura aura = u.GetAllAuras().FirstOrDefault(a => a.Name == auraName && a.CreatorGuid == Me.Guid);

            if (aura == null) {
                return 0;
            }

            return aura.TimeLeft.TotalMilliseconds;
        }

        private static double MyAuraTimeLeft(int auraId, WoWUnit u) {

            if (u == null || !u.IsValid || !u.IsAlive)
                return 0;

            WoWAura aura = u.GetAllAuras().FirstOrDefault(a => a.SpellId == auraId && a.CreatorGuid == Me.Guid);

            if (aura == null) {
                return 0;
            }

            return aura.TimeLeft.TotalMilliseconds;
        }

        public static bool Between(double num, double lower, double upper, bool inclusive = false) {
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }

        #endregion

        #region Has My Aura (can use stacks)

        public static bool HasMyAura(WoWUnit unit, string aura, int stacks) {
            return HasAura(unit, aura, stacks, StyxWoW.Me);
        }

        private static bool HasAura(WoWUnit unit, string aura, int stacks, WoWUnit creator) {
            return unit.GetAllAuras().Any(a => a.Name == aura && a.StackCount >= stacks && (creator == null || a.CreatorGuid == creator.Guid));
        }

        #endregion

        #region SafelyFaceTarget

        private static bool SafelyFaceTarget(WoWUnit unit) {

            if (unit == null || !unit.IsValid) {
                return false;
            }

            if (Me.IsFacing(unit) || Me.IsSafelyFacing(unit)) {
                return true;
            }

            if (!DiscSettings.Instance.AutoFace || Me.IsMoving) {
                return false;
            }

            if (!Me.IsFacing(unit) && IsEnemy(unit) && unit.InLineOfSpellSight) {
                unit.Face();
            }

            return Me.IsSafelyFacing(unit) || Me.IsFacing(unit);
        }

        #endregion

        #region UpdateMyLatency

        private static void UpdateMyLatency() {
            //If SLagTolerance enabled, start casting next spell MyLatency Millisecond before GlobalCooldown ready.

            MyLatency = (StyxWoW.WoWClient.Latency);

            //Use here because Lag Tolerance cap at 400
            //Logging.Write("----------------------------------");
            //Logging.Write("MyLatency: " + MyLatency);
            //Logging.Write("----------------------------------");

            if (MyLatency > 400) {
                //Lag Tolerance cap at 400
                MyLatency = 400;
            }
        }

        #endregion

        #region IsInRange

        private static bool IsInRange(WoWUnit target, int range) {
		
            if (target == null) {
                return false;
            }

            if (target.DistanceSqr < range * range) {
                return true;
            }
			
            return false;
        }

        #endregion

        #region UpdateMyTalent

        private static void UpdateMyTalent() {
		
            TalentNames = "";
            for (int i = 1; i <= 18; i++) {
			
                TalentNames = TalentNames +
                                Lua.GetReturnVal<String>(
                                    string.Format(
                                        "local t= select(5,GetTalentInfo({0})) if t == true then return '['..select(1,GetTalentInfo({0}))..'] ' end return nil",
                                        i), 0);
            }

            Logging.Write("----------------------------------");
            Logging.Write("Talent:");
            Logging.Write(TalentNames);
            Logging.Write("----------------------------------");
        }

        #endregion

        #region UpdateMyGlyph

        private static void UpdateMyTalentOrGlyphEvent(object sender, LuaEventArgs args) {
            UpdateMyGlyph();
            UpdateMyTalent();

            if (GlyphNames.Contains("Glyph of Holy Fire")) {
                SmiteRange = 40;
            } else {
                SmiteRange = 30;
            }
        }

        private static void UpdateMyGlyph() {
            
			GlyphIDs = "";
            GlyphNames = "";
			var glyphCount = Lua.GetReturnVal<int>("return GetNumGlyphSockets()", 0);

			if (glyphCount != 0) {
				for (int i = 1; i <= glyphCount; i++) {
					string lua =
						String.Format(
							"local enabled, glyphType, glyphTooltipIndex, glyphSpellID, icon = GetGlyphSocketInfo({0});if (enabled) then return glyphSpellID else return 0 end",
							i);

					var glyphSpellId = Lua.GetReturnVal<int>(lua, 0);

					try {
					
						if (glyphSpellId > 0) {
							GlyphNames = GlyphNames + "[" + (WoWSpell.FromId(glyphSpellId)) + " - " +
										   glyphSpellId +
										   "] ";
							GlyphIDs = GlyphIDs + "[" + glyphSpellId + "] ";
						} else {
							Logging.Write("Glyphdetection - No Glyph in slot " + i);
							//TreeRoot.Stop();
						}
					} catch (Exception ex) {
						Logging.Write("We couldn't detect your Glyphs");
						Logging.Write("Report this message to us: " + ex);
						//TreeRoot.Stop();
					}
				}
			}

			Logging.Write("----------------------------------");
			Logging.Write("Glyph:");
			Logging.Write(GlyphNames);
			Logging.Write("----------------------------------");
            
        }

        #endregion

        #region ValidUnit


        private static bool ValidUnit(WoWUnit u) {
            if (u != null && u.IsValid && u.Attackable && u.IsAlive) {
                return true;
            }

            return false;
        }

        #endregion

        #region Healable

        private static bool Healable(WoWUnit u) {
            
			if (u != null && u.IsValid) {
                return HealableValid(u);
            }
            return false;
        }

        //prevent double ValidUnit Check
        private static bool HealableValid(WoWUnit u) {
		
            if (u.IsAlive && u.Distance < 40 && !IsEnemy(u) && !DebuffDoNotHeal(u) && u.InLineOfSpellSight && !u.Name.Contains("Statue")) {
                return true;
            }
            return false;
        }

        #endregion        

        #region Do I LoS every enemy player?

        private static bool LoSingEveryEnemyPlayer()
        {

            return (from unit in NearbyUnFriendlyPlayers
                    where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                    where unit.IsValid
                    where unit.InLineOfSpellSight
                    select unit).Count() == 0;

        }

        #endregion

        #region Should I scream?

        private static bool ShouldScream() {

            return (from unit in NearbyUnFriendlyUnits
                    where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                    where unit.Distance2D <= 8
                    where !InvulnerableSpell(unit)
                    where !Unfearable(unit)                    
                    where unit.InLineOfSpellSight
                    select unit).Count() >= 1;

        }

        #endregion
        
        #region Can I peel with Void Tendrils?

        private static bool CanPeelWithTendrils() {

            return (from unit in NearbyUnFriendlyUnits
                    //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                    where unit.IsValid
                    where unit.Distance2D <= 8
                    where !InvulnerableSpell(unit)
                    where !Unrootable(unit)                    
                    where unit.InLineOfSpellSight
                    select unit).Count() >= 1;

        }

        #endregion

        #region Unrootable

        private static bool Unrootable(WoWUnit target) {
		
            if (target == null) {
                return false;
            }

            return target.HasAura("Master's Call") ||
                   target.HasAura("Bladestorm") ||
                   target.HasAura("Hand of Freedom");
        }

        #endregion

        #region DebuffRootorSnare

        private static readonly HashSet<int> DebuffRootorSnareHS = new HashSet<int>
            {
                96294, //Chains of Ice (Chilblains)
                116706, //Disable
                64695, //Earthgrab (Earthgrab Totem)
                339, //Entangling Roots
                113770, //Entangling Roots (Force of Nature - Balance Treants)
                19975, //Entangling Roots (Nature's Grasp)
                113275, //Entangling Roots (Symbiosis)
                19185, //Entrapment
                33395, //Freeze
                63685, //Freeze (Frozen Power)
                39965, //Frost Grenade
                122, //Frost Nova
                110693, //Frost Nova (Mage)
                55536, //Frostweave Net
                87194, //Glyph of Mind Blast
                111340, //Ice Ward
                45334, //Immobilized (Wild Charge - Bear)
                90327, //Lock Jaw (Dog)
                102359, //Mass Entanglement
                128405, //Narrow Escape
                13099, //Net-o-Matic
                115197, //Partial Paralysis
                50245, //Pin (Crab)
                91807, //Shambling Rush (Dark Transformation)
                123407, //Spinning Fire Blossom
                107566, //Staggering Shout
                54706, //Venom Web Spray (Silithid)
                114404, //Void Tendril's Grasp
                4167, //Web (Spider)
                50433, //Ankle Crack (Crocolisk)
                110300, //Burden of Guilt
                35101, //Concussive Barrage
                5116, //Concussive Shot
                120, //Cone of Cold
                3409, //Crippling Poison
                18223, //Curse of Exhaustion
                45524, //Chains of Ice
                50435, //Chilblains
                121288, //Chilled (Frost Armor)
                1604, //Dazed
                63529, //Dazed - Avenger's Shield
                50259, //Dazed (Wild Charge - Cat)
                26679, //Deadly Throw
                119696, //Debilitation
                116095, //Disable
                123727, //Dizzying Haze
                3600, //Earthbind (Earthbind Totem)
                77478, //Earthquake (Glyph of Unstable Earth)
                123586, //Flying Serpent Kick
                113092, //Frost Bomb
                54644, //Frost Breath (Chimaera)
                8056, //Frost Shock
                116, //Frostbolt
                8034, //Frostbrand Attack
                44614, //Frostfire Bolt
                61394, //Frozen Wake (Glyph of Freezing Trap)
                1715, //Hamstring
                13810, //Ice Trap
                58180, //Infected Wounds
                118585, //Leer of the Ox
                15407, //Mind Flay
                12323, //Piercing Howl
                115000, //Remorseless Winter
                20170, //Seal of Justice
                47960, //Shadowflame
                31589, //Slow
                129923, //Sluggish (Glyph of Hindering Strikes)
                61391, //Typhoon
                51490, //Thunderstorm
                127797, //Ursol's Vortex
                137637, //Warbringer
            };

        private static bool DebuffRootOrSnare(WoWUnit target) {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet) {
                return false;
            }

            using (StyxWoW.Memory.AcquireFrame()) {
                return target.ActiveAuras.Any(
                    a =>
                    a.Value.ApplyAuraType == WoWApplyAuraType.ModRoot ||
                    a.Value.ApplyAuraType == WoWApplyAuraType.ModDecreaseSpeed ||
                    DebuffRootorSnareHS.Contains(a.Value.SpellId));
            }
        }

        #endregion

        #region Unfearable

        private static bool Unfearable(WoWUnit target) {

            if (target == null) {
                return false;
            }

            return target.HasAura("Fear Ward") ||
                   target.HasAura("Berserker Rage");

        }

        #endregion

        #region DebuffDoNotHeal

        private static bool DebuffDoNotHeal(WoWUnit target) {

            if (target == null) {
                return false;
            }

            return target.HasAura("Dominate Mind") ||
                   target.HasAura("Cyclone");

        }

        #endregion

        #region DebuffDoNotCleanse

        private static bool DebuffDoNotDispel(WoWUnit target) {

            if (target == null) {
                return false;
            }

            return //target.HasAura("Flame Shock") ||
                   target.HasAura("Unstable Affliction") ||
                   target.HasAura("Vampiric Touch");

        }

        #endregion

        #region DebuffCCBreakonDamage

        private static bool DebuffCCBreakonDamage(WoWUnit target) {

            if (target == null) {
                return false;
            }

            return target.HasAura(2094) || //Blind
                   target.HasAura(105421) || //Blinding Light
                   target.HasAura(99) || //Disorienting Roar
                   target.HasAura(31661) || //Dragon's Breath
                   target.HasAura(3355) || //Freezing Trap
                   target.HasAura(1776) || //Gouge
                   target.HasAura(2637) || //Hibernate
                   target.HasAura(115268) || //Mesmerize (Shivarra)
                   target.HasAura(115078) || //Paralysis
                   target.HasAura(113953) || //Paralysis (Paralytic Poison)
                   target.HasAura(126355) || //Paralyzing Quill (Porcupine)
                   target.HasAura(126423) || //Petrifying Gaze (Basilisk)
                   target.HasAura(118) || //Polymorph
                   target.HasAura(61305) || //Polymorph: Black Cat
                   target.HasAura(28272) || //Polymorph: Pig
                   target.HasAura(61721) || //Polymorph: Rabbit
                   target.HasAura(61780) || //Polymorph: Turkey
                   target.HasAura(28271) || //Polymorph: Turtle
                   target.HasAura(20066) || //Repentance
                   target.HasAura(6770) || //Sap
                   target.HasAura(19503) || //Scatter Shot
                   target.HasAura(132412) || //Seduction (Grimoire of Sacrifice)
                   target.HasAura(6358) || //Seduction (Succubus)
                   target.HasAura(104045) || //Sleep (Metamorphosis)
                   target.HasAura(19386); //Wyvern Sting
        }

        #endregion

        #region InvulnerableSpell

        private static bool InvulnerableSpell(WoWUnit target) {

            if (target == null) {
                return false;
            }

            return target.HasAura("Anti-Magic Shell") ||
                   target.HasAura("Cloak of Shadows") ||
                   target.HasAura("Ice Block") ||
                   target.HasAura("Grounding Totem Effect") ||
                   target.HasAura("Mass Spell Reflection") ||
                   target.HasAura("Spell Reflection") ||
                   target.HasAura("Cyclone") ||
                   target.HasAura("Deterrence") ||
                   target.HasAura("Divine Shield") ||
                   target.HasAura("Zen Meditation");

        }

        #endregion

        #region MassDispelableInvulnerability

        private static bool MassDispelableInvulnerability(WoWUnit target) {

            if (target == null || !target.IsValid) {
                return false;
            }

            return target.HasAura("Ice Block") ||
                   target.HasAura("Divine Shield");
        }

        #endregion


        #region IsFeared

        private static bool IsFearedOrCharmed(WoWUnit target) {

            if (target == null) {
                return false;
            }

            return target.HasAura(8122) || //Psychic Scream
                       target.HasAura(113792) || //Psychic Terror
                       target.HasAura(5782) || //Fear
                       target.HasAura(118699) || //Fear
                       target.HasAura(130616) || //Fear (Glyph of Fear)
                       target.HasAura(5484) || //Howl of Terror
                       target.HasAura(113056) || //Intimidating Roar [Cowering in fear] (Warrior)
                       target.HasAura(113004) || //Intimidating Roar [Fleeing in fear] (Warrior)
                       target.HasAura(5246) || //Intimidating Shout (aoe)
                       target.HasAura(20511) || //Intimidating Shout (targeted)
                       target.HasAura(115268) || //Mesmerize (Shivarra)
                       target.HasAura(64044) || //Psychic Horror
                       target.HasAura(132412) || //Seduction (Grimoire of Sacrifice)
                       target.HasAura(6358) || //Seduction (Succubus)
                       target.HasAura(87204) || //Sin and Punishment
                       target.HasAura(104045); //Sleep (Metamorphosis)
        }

        #endregion

        #region DebuffCC

        private static readonly HashSet<int> DebuffCCHS = new HashSet<int>
            {
                30217, //Adamantite Grenade
                89766, //Axe Toss (Felguard/Wrathguard)
                90337, //Bad Manner (Monkey)
                710, //Banish
                113801, //Bash (Force of Nature - Feral Treants)
                102795, //Bear Hug
                76780, //Bind Elemental
                117526, //Binding Shot
                2094, //Blind
                105421, //Blinding Light
                115752, //Blinding Light (Glyph of Blinding Light)
                123393, //Breath of Fire (Glyph of Breath of Fire)
                126451, //Clash
                122242, //Clash (not sure which one is right)
                67769, //Cobalt Frag Bomb
                118271, //Combustion Impact
                33786, //Cyclone
                113506, //Cyclone (Symbiosis)
                7922, //Charge Stun
                119392, //Charging Ox Wave
                1833, //Cheap Shot
                44572, //Deep Freeze
                54786, //Demonic Leap (Metamorphosis)
                99, //Disorienting Roar
                605, //Dominate Mind
                118895, //Dragon Roar
                31661, //Dragon's Breath
                77505, //Earthquake
                5782, //Fear
                118699, //Fear
                130616, //Fear (Glyph of Fear)
                30216, //Fel Iron Bomb
                105593, //Fist of Justice
                117418, //Fists of Fury
                3355, //Freezing Trap
                91800, //Gnaw
                1776, //Gouge
                853, //Hammer of Justice
                110698, //Hammer of Justice (Paladin)
                51514, //Hex
                2637, //Hibernate
                88625, //Holy Word: Chastise
                119072, //Holy Wrath
                5484, //Howl of Terror
                22703, //Infernal Awakening
                113056, //Intimidating Roar [Cowering in fear] (Warrior)
                113004, //Intimidating Roar [Fleeing in fear] (Warrior)
                5246, //Intimidating Shout (aoe)
                20511, //Intimidating Shout (targeted)
                24394, //Intimidation
                408, //Kidney Shot
                119381, //Leg Sweep
                126246, //Lullaby (Crane)
                22570, //Maim
                115268, //Mesmerize (Shivarra)
                5211, //Mighty Bash
                91797, //Monstrous Blow (Dark Transformation)
                6789, //Mortal Coil
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
                9005, //Pounce
                102546, //Pounce (Incarnation)
                64044, //Psychic Horror
                8122, //Psychic Scream
                113792, //Psychic Terror (Psyfiend)
                118345, //Pulverize
                107079, //Quaking Palm
                13327, //Reckless Charge
                115001, //Remorseless Winter
                20066, //Repentance
                82691, //Ring of Frost
                6770, //Sap
                1513, //Scare Beast
                19503, //Scatter Shot
                132412, //Seduction (Grimoire of Sacrifice)
                6358, //Seduction (Succubus)
                9484, //Shackle Undead
                30283, //Shadowfury
                132168, //Shockwave
                87204, //Sin and Punishment
                104045, //Sleep (Metamorphosis)
                50519, //Sonic Blast (Bat)
                118905, //Static Charge (Capacitor Totem)
                56626, //Sting (Wasp)
                107570, //Storm Bolt
                10326, //Turn Evil
                20549, //War Stomp
                105771, //Warbringer
                19386, //Wyvern Sting
                108194, //Asphyxiate
            };

        private static bool DebuffCC(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet)
            {
                return false;
            }


            using (StyxWoW.Memory.AcquireFrame())
            {
                if (target.ActiveAuras.Any(a => DebuffCCHS.Contains(a.Value.SpellId)))
                {
                    //Logging.Write(target.Name + " got DebuffCC");
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region DebuffSilence

        private static readonly HashSet<int> DebuffSilenceHS = new HashSet<int>
            {
                129597, //Arcane Torrent (Chi)
                25046, //Arcane Torrent (Energy)
                80483, //Arcane Torrent (Focus)
                28730, //Arcane Torrent (Mana)
                69179, //Arcane Torrent (Rage)
                50613, //Arcane Torrent (Runic Power)
                31935, //Avenger's Shield
                114238, //Fae Silence (Glyph of Fae Silence)
                102051, //Frostjaw (also a root)
                1330, //Garrote - Silence
                115782, //Optical Blast (Observer)
                15487, //Silence
                18498, //Silenced - Gag Order
                55021, //Silenced - Improved Counterspell
                34490, //Silencing Shot
                81261, //Solar Beam
                113287, //Solar Beam (Symbiosis)
                116709, //Spear Hand Strike
                24259, //Spell Lock (Felhunter)
                132409, //Spell Lock (Grimoire of Sacrifice)
                47476, //Strangulate
                31117, //Unstable Affliction
            };

        private static bool DebuffSilence(WoWUnit target) {
            if (target == null || !target.IsValid || !target.IsPlayer) {
                return false;
            }


            using (StyxWoW.Memory.AcquireFrame()) {
                if (target.ActiveAuras.Any(
                    a =>
                    a.Value.ApplyAuraType == WoWApplyAuraType.ModSilence ||
                    DebuffSilenceHS.Contains(a.Value.SpellId))) {
                    //Logging.Write(target.Name + " got DebuffSilence");
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region DebuffCCleanseASAP

        private static readonly HashSet<int> DebuffCCleanseASAPHS = new HashSet<int>
            {
                105421, //Blinding Light
                123393, //Breath of Fire (Glyph of Breath of Fire)
                44572, //Deep Freeze
                605, //Dominate Mind
                31661, //Dragon's Breath
                5782, // target.Class != WoWClass.Warrior || //Fear
                118699, // target.Class != WoWClass.Warrior || //Fear
                130616, // target.Class != WoWClass.Warrior || //Fear (Glyph of Fear)
                3355, //Freezing Trap
                853, //Hammer of Justice
                110698, //Hammer of Justice (Paladin)
                2637, //Hibernate
                88625, //Holy Word: Chastise
                119072, //Holy Wrath
                5484, // target.Class != WoWClass.Warrior || //Howl of Terror
                115268, //Mesmerize (Shivarra)
                6789, //Mortal Coil
                115078, //Paralysis
                113953, //Paralysis (Paralytic Poison)
                126355, //Paralyzing Quill (Porcupine)
                118, //Polymorph
                61305, //Polymorph: Black Cat
                28272, //Polymorph: Pig
                61721, //Polymorph: Rabbit
                61780, //Polymorph: Turkey
                28271, //Polymorph: Turtle
                64044, // target.Class != WoWClass.Warrior || //Psychic Horror
                8122, // target.Class != WoWClass.Warrior || //Psychic Scream
                113792, // target.Class != WoWClass.Warrior || //Psychic Terror (Psyfiend)
                107079, //Quaking Palm
                115001, //Remorseless Winter
                20066, // target.Class != WoWClass.Warrior || //Repentance
                82691, //Ring of Frost
                1513, //Scare Beast
                132412, //Seduction (Grimoire of Sacrifice)
                6358, //Seduction (Succubus)
                9484, //Shackle Undead
                30283, //Shadowfury
                87204, //Sin and Punishment
                104045, //Sleep (Metamorphosis)
                118905, //Static Charge (Capacitor Totem)
                33395, //Freeze
                63685, //Freeze (Frozen Power)
                122, //Frost Nova
                110693, //Frost Nova (Mage)
                2944, //Devouring Plague
                10326 //Turn Evil
            };

        private static readonly HashSet<int> DebuffCCleanseASAPHSWarrior = new HashSet<int>
            {
                5782,
                118699,
                130616,
                64044,
                8122,
                113792
            };

        private static bool DebuffCCleanseASAP(WoWUnit target) {
            if (target == null || !target.IsValid || !target.IsPlayer) {
                return false;
            }

            if (target.IsPlayer &&
                target.Class == WoWClass.Warrior &&
                target.ActiveAuras.Any(
                    a =>
                    DebuffCCleanseASAPHSWarrior.Contains(a.Value.SpellId))) {
                return false;
            }


            using (StyxWoW.Memory.AcquireFrame()) {
                if (target.ActiveAuras.Any(
                    a =>
                    DebuffCCleanseASAPHS.Contains(a.Value.SpellId) &&
                    a.Value.TimeLeft.TotalMilliseconds > 3000)) {
                    //Logging.Write(target.Name + " got DebuffCCCleanseASAP");
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region DebuffDot

        /// <summary>
        /// Credit worklifebalance http://www.thebuddyforum.com/honorbuddy-forum/developer-forum/113473-wowapplyauratype-periodicleech-temporal-displacement.html#post1107923
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static bool DebuffDot(WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet)
            {
                return false;
            }

            return target.ActiveAuras.Any(a =>
                                          a.Value.SpellId != 57724 && //Sated
                                          a.Value.SpellId != 80354 && //Temporal Displacement
                                          (a.Value.ApplyAuraType == WoWApplyAuraType.PeriodicDamage ||
                                           a.Value.ApplyAuraType == WoWApplyAuraType.PeriodicDamagePercent ||
                                           a.Value.ApplyAuraType == WoWApplyAuraType.PeriodicLeech)
                );
        }

        #endregion

        #region WriteDebug

        private static Composite WriteDebug(string message)
        {
            return new Action(delegate
            {
                //Logging.Write(LogLevel.Diagnostic, message);
                Logging.Write(message);
                return RunStatus.Failure;
            });
        }

        #endregion

        #region Movement

        public delegate float DynamicRangeRetriever(object context);

        public delegate WoWPoint LocationRetriever(object context);

        public delegate bool SimpleBooleanDelegate(object context);

        public delegate WoWUnit UnitSelectionDelegate(object context);

        //Set Distance To Keep in Melee Range base on Unit type
        public static float Dtm(WoWUnit toUnit)
        {
            if (toUnit.IsPlayer)
            {
                return 3.5f;
            }
            return 5f;
        }

        public static Composite MovementMoveStop(UnitSelectionDelegate toUnit, double range)
        {
            return new Decorator(
                ret =>
                TreeRoot.Current.Name == "BGBuddy" &&
                toUnit != null &&
                toUnit(ret) != null &&
                toUnit(ret) != Me &&
                toUnit(ret).IsAlive &&
                Me.IsMoving &&
                (!toUnit(ret).IsMoving &&
                 toUnit(ret).Location.Distance(Me.Location) <= Dtm(toUnit(ret)) ||
                 toUnit(ret).Location.Distance(Me.Location) <= 1),
                new Action(ret =>
                {
                    Navigator.PlayerMover.MoveStop();
                    Logging.Write("Stopping movement");
                    return RunStatus.Failure;
                }));
        }

        private static TimeSpan MovementDelay = TimeSpan.FromSeconds(0);
        private static DateTime LastMovementTime = new DateTime(1970, 1, 1);
        public static WoWPoint LastMovementWoWPoint;
        private static float DistanceToUpdateMovement = 3f;
        private static DateTime DoNotMove;

        public static Composite MovementMoveToLoS(UnitSelectionDelegate toUnit)
        {
            return new Decorator(
                ret =>
                TreeRoot.Current.Name == "BGBuddy" &&
                !Me.IsCasting &&
                //(!Me.IsChanneling || (Me.IsChanneling && Me.ChanneledCastingSpellId == 15407)) &&
                toUnit != null &&
                toUnit(ret) != null &&
                toUnit(ret) != Me &&
                    //Only Move again After a certain delay or target move 3 yard from original posision
                (LastMovementTime + MovementDelay < DateTime.Now ||
                 LastMovementWoWPoint.Distance(toUnit(ret).Location) >
                 DistanceToUpdateMovement) &&
                !toUnit(ret).InLineOfSpellSight,
                new Action(ret =>
                {
                    LastMovementWoWPoint = toUnit(ret).Location;
                    Navigator.MoveTo(LastMovementWoWPoint);
                    LastMovementTime = DateTime.Now;
                    Logging.Write("Moving closer to " + toUnit(ret).Name);
                    return RunStatus.Failure;
                }));
        }

        #endregion


    }
}