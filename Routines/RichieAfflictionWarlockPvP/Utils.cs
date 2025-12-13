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

namespace RichieAfflictionWarlock {

    public partial class Main {

        #region Global Variables

		private static int PeelDelay = 3;
        private static int CCDelay = 3;
        private static int defensiveDelay = 6;
        private static int summonDelay = 10;
        private static int fearDelay = 15;
        private static double MyLatency;
        private static bool supremacy = false;
        private static bool sacrifice = false;
        private static bool combatLogAttached = false;

        private static WoWGameObject Soulwell;
        private static WoWGameObject DemonicCircle;

        private static WoWUnit MyLastTarget;
        private static WoWUnit LastCastUnit;
        private static WoWUnit ASAPAttackTarget;
        private static WoWUnit CCTarget;
        private static WoWUnit MortalCoilPeelTarget;
        private static WoWUnit ShadowfuryPeelTarget;        
        private static WoWUnit FearPeelTarget;        
        private static WoWUnit CapInterruptTarget;
        private static WoWUnit MultidotTarget;
        private static WoWUnit DrainSoulTarget;
        private static WoWUnit LastUATarget;
        private static WoWUnit LastFearTarget;
        private static WoWUnit BanishTarget;
        private static WoWUnit PreviousBanishTarget = null;
        private static WoWUnit DispelTarget;
        private static WoWUnit CoEnPeelTarget;
        private static WoWUnit SoulSwapInhaleTarget;
        


        private static DateTime LastCastTime = DateTime.Now;
        private static DateTime LastUpdateMyLatency;
        private static readonly DateTime Now = DateTime.Now;
        private static DateTime LastPeel = DateTime.Now;
        private static DateTime LastCC = DateTime.Now;
        private static DateTime LastClickRemoteLocation;
        private static DateTime LastHauntCast = DateTime.Now;
        private static DateTime LastUACast = DateTime.Now;
        private static DateTime LastDefensive = DateTime.Now;
        private static DateTime LastHealthstoneCreated = DateTime.Now;
        private static DateTime LastSummon = DateTime.Now;
        private static DateTime tempFearTime = DateTime.Now;
        private static DateTime tempBanishTime = DateTime.Now;
        private static DateTime LastSilence = DateTime.Now;
        private static DateTime lastDispel = DateTime.Now;
        private static DateTime lastCauterize = DateTime.Now;
        private static DateTime LastSoulSwapInhaleTargetSet = DateTime.Now;    
        
        

        private static string LastCastSpell = "";
        private static string GlyphIDs = "";
        private static string GlyphNames = "";
        private static string TalentNames = "";
		
        private static readonly List<WoWPlayer> NearbyFriendlyPlayers = new List<WoWPlayer>();
        private static readonly List<WoWUnit> NearbyFriendlyUnits = new List<WoWUnit>();
        private static readonly List<WoWPlayer> NearbyUnFriendlyPlayers = new List<WoWPlayer>();
        private static readonly List<WoWUnit> NearbyUnFriendlyUnits = new List<WoWUnit>();
        private static readonly List<WoWUnit> NearbyTotems = new List<WoWUnit>();

        private static Dictionary<ulong, DateTime> banishTracker = new Dictionary<ulong, DateTime>();
        private static Dictionary<ulong, DateTime> fearTracker = new Dictionary<ulong, DateTime>();

        #endregion	
		
		#region MiscSetups

        private static Composite MiscSetups() {
		
			return new Action(
				delegate {
					//Hold All Action While targeting
					if (Me.HasPendingSpell("Shadowfury")
                        || Me.HasPendingSpell("Summon Infernal")
                        || Me.HasPendingSpell("Demonic Gateway")
                        || Me.HasPendingSpell("Command Demon")
                        || Me.HasPendingSpell("Rain of Fire")
                        ) {
						Logging.Write(LogLevel.Diagnostic,
									  DateTime.Now.ToString("ss:fff ") +
									  "Hold All Action While Targeting");
						return RunStatus.Success;
					}

                    if ((!AfflictionSettings.Instance.DrainSoulForShards || (AfflictionSettings.Instance.DrainSoulForShards && Me.CurrentSoulShards >= 2) || (AfflictionSettings.Instance.DrainSoulForShards && DrainSoulTarget != null && DrainSoulTarget.IsPet)) && DrainSoulTarget != null && DrainSoulTarget.IsValid && DrainSoulTarget.HealthPercent > 20) {
                        DrainSoulTarget = null;
                        if (Me.IsChanneling && Me.ChanneledCastingSpellId == 1120) {
                            StopCasting("Drain Soul target is above 20% and we have enough Soul Shards.");
                        }
                    }

                    if ((!AfflictionSettings.Instance.DrainSoulForShards || (AfflictionSettings.Instance.DrainSoulForShards && Me.CurrentSoulShards >= 2)) && DrainSoulTarget == null && Me.IsChanneling && Me.ChanneledCastingSpellId == 1120 && Me.CurrentTarget != null && Me.CurrentTarget.IsValid && Me.CurrentTarget.HealthPercent >= 20) {
                        StopCasting("Player casted Drain Soul on target that is above 20%.");
                    }

                    if ((Me.HealthPercent < AfflictionSettings.Instance.DontCastSpellsForHpBelowHp || !Me.Combat || !Me.IsMoving) && Me.HasAura("Burning Rush")) {
                        Lua.DoString("RunMacroText(\"/cancelaura Burning Rush\")");
                        Logging.Write("Cancelling Burning Rush at configured health percent/got out of combat/stopped movement.");
                    }

                    if (Me.GotAlivePet && Me.IsCasting &&
                       (Me.CastingSpellId == 688 ||   // Imp
                       Me.CastingSpellId == 697 ||    // Voidwalker
                       Me.CastingSpellId == 691 ||    // Felhunter
                       Me.CastingSpellId == 712 ||    // Succubus 
                       Me.CastingSpellId == 30146 ||  // Felguard

                       Me.CastingSpellId == 112866 ||    // Fel Imp
                       Me.CastingSpellId == 112867 ||    // Voidlord
                       Me.CastingSpellId == 112869 ||    // Observer
                       Me.CastingSpellId == 112868 ||  // Shivarra 
                       Me.CastingSpellId == 112870   // Wrathguard
                       ) && (getPreferredPetSpellId() != Me.CastingSpellId || Me.Pet.CreatureFamilyInfo.Id == getPreferredPetId())) {
                           Logging.Write("Preferred pet: " + getPreferredPetName() + ", but we're summoning: " + Me.CastingSpell.Name);
                           StopCasting("Pet is already summoned.");
                            
                    }


					//Clear Target if dead and still in combat
					if (Me.CurrentTarget != null && !Me.CurrentTarget.IsAlive && Me.Combat) {
						Lua.DoString("RunMacroText(\"/cleartarget\")");
					}

					return RunStatus.Failure;
				}
			);
        }
	
        private static int getPreferredPetId() {

            switch (AfflictionSettings.Instance.PreferredPet) {
                case 0: return (supremacy ? SpellIds.Instance.PetId_Observer : (sacrifice ? SpellIds.Instance.PetId_Voidwalker : SpellIds.Instance.PetId_Felhunter));
                case 1: return (supremacy ? SpellIds.Instance.PetId_FelImp : SpellIds.Instance.PetId_Imp);
                case 2: return (supremacy ? SpellIds.Instance.PetId_Voidlord : SpellIds.Instance.PetId_Voidwalker);
                case 3: return (supremacy ? SpellIds.Instance.PetId_Observer : SpellIds.Instance.PetId_Felhunter);
                case 4: return (supremacy ? SpellIds.Instance.PetId_Shivarra : SpellIds.Instance.PetId_Succubus);                    
            }
            return SpellIds.Instance.PetId_Felhunter;
        }

        private static int getPreferredPetSpellId() {

            switch (AfflictionSettings.Instance.PreferredPet) {
                case 0: return (supremacy ? 112869 : (sacrifice ? 697 : 691));
                case 1: return (supremacy ? 112866 : 688);
                case 2: return (supremacy ? 112867 : 697);
                case 3: return (supremacy ? 112869 : 691);
                case 4: return (supremacy ? 112868 : 712);                    
            }
            return 691;
        }

        private static String getPreferredPetName() {

            switch (AfflictionSettings.Instance.PreferredPet) {
                case 0: return sacrifice ? "Voidwalker" : "Felhunter";
                case 1: return "Imp";
                case 2: return "Voidwalker";
                case 3: return "Felhunter";
                case 4: return "Succubus";                    
            }
            return "Felhunter";
        }


        //Thanks to the purerotation team for this
        private static string GetSacrificeAbility() {

            switch (WoWSpell.FromId(SpellIds.Instance.CommandDemon).Icon.ToLower()) {
                case @"interface\icons\spell_fire_elementaldevastation":
                    return "Singe Magic";
                case @"interface\icons\spell_shadow_antishadow":
                    return "Shadow Bulwark";
                case @"interface\icons\ability_warlock_whiplash":
                    return "Whiplash";
                case @"interface\icons\spell_shadow_mindrot":
                    return "Spell Lock";
                case @"interface\icons\ability_rogue_sprint":
                    return "Pursuit";
                default:
                    return "Unknown";
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

        #region Attackable

        private static bool Attackable(WoWUnit target) {
            if (ValidUnit(target) && target.Distance < 40 &&
                IsEnemy(target) && target.InLineOfSpellSight) {
                return true;
            }
            
            return false;
        }

        //prevent double ValidUnit Check
        private static bool AttackableValid(WoWUnit target) {
            if (target.Distance < 40 && IsEnemy(target) &&
                target.InLineOfSpellSight) {
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

                //Logging.Write(spellName + " Target's distance: " + location.Distance2D(u.Location));
                message += (message.Length > 0 ? "-" : "") + "distance: " + location.Distance2D(u.Location);
                SpellManager.ClickRemoteLocation(location);
                Lua.DoString("SpellStopTargeting()");
            }

            if (spellName == "Soulburn") {
                DateTime SoulBurnWait = DateTime.Now;
                while (SoulBurnWait + TimeSpan.FromMilliseconds(100) < DateTime.Now && !Me.HasAura("Soulburn")) {
                    Logging.Write("Waiting for soulburn.");
                }
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
                string mana = "Mana: " + Math.Round(Me.ManaPercent) + "%";
                string orbs = "Shards: " + Me.CurrentSoulShards; 
                //string barFour = "NU: " + NearbyUnFriendlyPlayers.Count;
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
                                        mana + " - " +
                                        orbs + " - " +
                                        //barFour + " - " +
                                        unitName + " - " + Math.Round(u.Distance) + "y - " +
                                        Math.Round(u.HealthPercent) + "% hp - " + spellName + messageCol);
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

        #region Casting and GCD

        private static DateTime GCDReady;

        private static void UpdateGCD() {
            GCDReady = DateTime.Now + SpellManager.Spells["Corruption"].CooldownTimeLeft;
        }

        private static bool GCDL() {
            return DateTime.Now + TimeSpan.FromMilliseconds(MyLatency) <= GCDReady;
        }

        private static bool Casting()
        {
            //Malefic Grasp
            if (Me.IsChanneling && Me.ChanneledCastingSpellId == 103103 && !Me.HasAura("Dark Soul: Misery"))
            {
                return false;
            }
            //Drain Soul
            /*if (Me.IsChanneling && Me.ChanneledCastingSpellId == 1120)
            {
                return false;
            }*/

            /*if (Me.HasPendingSpell("Mass Dispel") || Me.HasPendingSpell("Psyfiend"))
            {
                return true;
            }*/
			
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

        private static void StopCasting(String cause) {
            if (Me.IsCasting || Me.IsChanneling) {
                SpellManager.StopCasting();
                Logging.Write("Spellcasting cancelled. Cause: " + cause);
            }
        }

        #endregion

        #region GetSpellCooldown

        private static TimeSpan GetSpellCooldown(string spell) {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results)) {
                return results.Override != null ? results.Override.CooldownTimeLeft : results.Original.CooldownTimeLeft;
            }

            return TimeSpan.MaxValue;
        }

        private static double GetSpellCooldown(int spellId) {
            SpellFindResults results;
            if (SpellManager.FindSpell(spellId, out results)) {
                return results.Override != null ? results.Override.CooldownTimeLeft.TotalMilliseconds : results.Original.CooldownTimeLeft.TotalMilliseconds;
            }

            return double.MaxValue;
        }


        #endregion

        #region Get Nearest Soulwell

        private static bool GetSoulwell() {

            Soulwell = ObjectManager.GetObjectsOfType<WoWGameObject>().OrderBy(i => i.Distance).FirstOrDefault(i => i.Entry == 181621 && (StyxWoW.Me.PartyMembers.Any(p => p.Guid == i.CreatedByGuid) || StyxWoW.Me.Guid == i.CreatedByGuid));

            return Soulwell != null;

        }

        #endregion
        
        #region Get My Demonic Circle

        private static bool GetMyDemonicCircle() {

            DemonicCircle = ObjectManager.GetObjectsOfType<WoWGameObject>().FirstOrDefault(i => i.Entry == 191083 && Me.Guid == i.CreatedByGuid);

            return DemonicCircle != null;
        }

        #endregion


        #region CountEnemyNearby

        private static int CountEnemyNearby(WoWObject unitCenter, float distance) {
            
			return NearbyUnFriendlyUnits.Where(
                    unit =>
                    unitCenter.Location.Distance(unit.Location) <= distance &&
                    !unit.IsPet
                    ).Count();
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

        #region NumberOfUndottedUnits

        private static int getNumberOfUndottedUnits()
        {

            if (!AfflictionSettings.Instance.Multidot) {
                return !(MyAuraTimeLeft("Agony", Me.CurrentTarget) > AfflictionSettings.Instance.AgonyRefresh &&
                MyAuraTimeLeft(146739, Me.CurrentTarget) > AfflictionSettings.Instance.CorruptionRefresh &&
                MyAuraTimeLeft("Unstable Affliction", Me.CurrentTarget) > AfflictionSettings.Instance.UnstableAfflictionRefresh) ? 1 : 0;
            }                           

            return (from unit in NearbyUnFriendlyUnits
                        //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                    where unit != null && unit.IsValid
                    where !(MyAuraTimeLeft("Agony", unit) > AfflictionSettings.Instance.AgonyRefresh &&
                            MyAuraTimeLeft(146739, unit) > AfflictionSettings.Instance.CorruptionRefresh &&
                            MyAuraTimeLeft("Unstable Affliction", unit) > AfflictionSettings.Instance.UnstableAfflictionRefresh)
                    where unit.InLineOfSpellSight
                    where !InvulnerableSpell(unit)
                    select unit).Count();
            
        }

        #endregion     

        #region Get Unit capping a flag

        private static bool GetCapInterruptTarget() {

            CapInterruptTarget = (from unit in NearbyUnFriendlyPlayers
                        //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                        where unit != null && unit.IsValid
                        where !InvulnerableSpell(unit)
                        where unit.InLineOfSpellSight
                        where unit.IsCasting &&
                            (unit.CastingSpell != null && 
                            unit.CastingSpell.Name != null && 
                            (unit.CastingSpell.Name.Contains("Opening") || unit.CastingSpell.Name.Contains("Capturing")))
                            //(unit.CastingSpellId == 98322 || unit.CastingSpellId == 98323 || unit.CastingSpellId == 98324 || 
                            //(unit.CastingSpell != null && unit.CastingSpell.Name != null && (unit.CastingSpell.Name.Contains("Opening") || unit.CastingSpell.Name.Contains("Capturing"))))
                        select unit).FirstOrDefault();

            if (CapInterruptTarget != null) {
                Logging.Write(CapInterruptTarget.Name);
            }

            return CapInterruptTarget != null;
        }

        #endregion

        #region Get Multidot target

        private static bool GetMultidotTarget() {

            if (MultidotTarget != null && ((MultidotTarget.IsValid && MultidotTarget.IsAlive && MultidotTarget.Distance2D > 40)
                || !MultidotTarget.IsValid
                || !MultidotTarget.IsAlive
                || !MultidotTarget.InLineOfSpellSight                
                || (
                    MyAuraTimeLeft("Agony", MultidotTarget) > AfflictionSettings.Instance.AgonyRefresh &&
                    MyAuraTimeLeft(146739, MultidotTarget) > AfflictionSettings.Instance.CorruptionRefresh &&
                    (MyAuraTimeLeft("Unstable Affliction", MultidotTarget) > AfflictionSettings.Instance.UnstableAfflictionRefresh || Me.IsMoving)
                )
                || InvulnerableSpell(MultidotTarget))) {
                Blacklist.Add(MultidotTarget.Guid, BlacklistFlags.Combat, TimeSpan.FromSeconds(3));
                MultidotTarget = null;                
                //Logging.Write("Multidot target reset" + Me.IsMoving);
            }

            if (MultidotTarget != null) {
                //MultidotTarget.Target();
                return true;
            }

            if (MultidotTarget == null && Me.CurrentSoulShards >= 3) {
                //Logging.Write("Getting new undotted multidot target");
                MultidotTarget = (from unit in NearbyUnFriendlyUnits
                                  //to prevent selecting the same target again for a few seconds
                                  where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                                  where unit != null && unit.IsValid
                                  where unit.HealthPercent > 10
                                  where !InvulnerableSpell(unit)
                                  where unit.InLineOfSpellSight
                                  where !DebuffCCBreakonDamage(unit)
                                  //where !(Me.CurrentTarget != null  && Me.CurrentTarget == unit)
                                  where MyAuraTimeLeft("Agony", unit) == 0 &&
                                          MyAuraTimeLeft(146739, unit) == 0 &&
                                          MyAuraTimeLeft("Unstable Affliction", unit) == 0

                                  orderby unit.IsPlayer ? 2 : 1 descending // target players first
                                  select unit).FirstOrDefault();

            }

            if (MultidotTarget == null) {
                //Logging.Write("Getting new multidot target");
                MultidotTarget = (from unit in NearbyUnFriendlyUnits
                        //to prevent selecting the same target again for a few seconds
                        where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                        where unit != null && unit.IsValid
                        where !InvulnerableSpell(unit)
                        where unit.InLineOfSpellSight
                        //where !(Me.CurrentTarget != null  && Me.CurrentTarget == unit)
                        where MyAuraTimeLeft("Agony", unit) <= AfflictionSettings.Instance.AgonyRefresh ||
                                MyAuraTimeLeft(146739, unit) <= AfflictionSettings.Instance.CorruptionRefresh ||
                                MyAuraTimeLeft("Unstable Affliction", unit) <= AfflictionSettings.Instance.UnstableAfflictionRefresh
                        orderby unit.IsPlayer ? 2 : 1 descending // target players first
                        orderby MyAuraTimeLeft("Agony", unit) ascending //TODO tweak it
                        select unit).FirstOrDefault();

            }

            return MultidotTarget != null;
        }

        #endregion


        #region Should I scream?

        private static bool ShouldScream() {

            return (from unit in NearbyUnFriendlyUnits
                    //where !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                    where unit.Distance2D <= 10
                    where !InvulnerableSpell(unit)
                    where !Unfearable(unit)
                    where unit.InLineOfSpellSight
                    select unit).Count() >= 1;

        }

        #endregion

        #region Get Banish target

        private static bool GetBanishTarget() {

            BanishTarget = (from unit in NearbyUnFriendlyUnits
                              where unit != null && unit.IsValid
                              where unit.InLineOfSpellSight
                              where unit.IsDemon || unit.IsElemental || unit.Entry == 59190 //Psyfiend
                              where !unit.HasAura("Banish")
                              select unit).FirstOrDefault();


            return BanishTarget != null;

        }

        #endregion

        #region Am I targeted by a magic damage dealer?

        private static bool TargetedByMagicDamageDealer() {
            using (StyxWoW.Memory.AcquireFrame()) {
                return NearbyUnFriendlyPlayers.Any(u => u.GotTarget && u.CurrentTargetGuid == Me.Guid && (TalentSort(u) == 3 || (TalentSort(u) == 1 && (u.Class == WoWClass.Paladin || u.Class == WoWClass.Shaman || u.Class == WoWClass.DeathKnight))));
            }
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
									where unit.InLineOfSpellSight
                                    //where AttackableValid(unit)
                                    where
                                        (unit.Name == "Spirit Link Totem" ||
                                        unit.Name == "Grouding Totem" ||
                                        unit.Name == "Earthgrab Totem" ||
                                        unit.Name == "Tremor Totem" ||
                                        //unit.Name == "Healing Tide Totem" ||
										//unit.Name == "Mana Tide Totem" ||
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

        #region Get Mortal Coil target

        private static bool GetMortalCoilPeelTarget(WoWUnit peelFrom) {

            if (peelFrom == null || !peelFrom.IsValid) {
                MortalCoilPeelTarget = null;
                return false;
            }

            MortalCoilPeelTarget = (from unit in NearbyUnFriendlyPlayers
                                  where unit.IsValid
                                  //where AttackableValid(unit)
                                  //where unit.CurrentTarget != null && unit.CurrentTarget.IsValid && ((peelFrom == Me && unit.CurrentTarget == Me) || (peelFrom != Me && unit.CurrentTarget == peelFrom))
                                  where unit.CurrentTarget != null && unit.CurrentTarget.IsValid && unit.CurrentTarget == peelFrom
                                  where unit.Distance <= 30
                                  where !InvulnerableSpell(unit)
                                  orderby unit.Distance ascending
                                  select unit).FirstOrDefault();

            return MortalCoilPeelTarget != null;
        }

        #endregion

        #region Get fear target

        private static bool GetFearPeelTarget(WoWUnit peelFrom) {

            if (peelFrom == null || !peelFrom.IsValid) {
                FearPeelTarget = null;
                return false;
            }

            FearPeelTarget = (from unit in NearbyUnFriendlyPlayers
                                    where unit.IsValid
                                    //where AttackableValid(unit)
                                    //where unit.CurrentTarget != null && unit.CurrentTarget.IsValid && ((peelFrom == Me && unit.CurrentTarget == Me) || (peelFrom != Me && unit.CurrentTarget == peelFrom))
                                    where unit.CurrentTarget != null && unit.CurrentTarget.IsValid && unit.CurrentTarget == peelFrom
                                    where unit.Distance <= 30
                                    where !InvulnerableSpell(unit)
                                    where !Unfearable(unit)
                                    orderby unit.Distance ascending
                                    select unit).FirstOrDefault();

            return FearPeelTarget != null;
        }

        #endregion

        #region Get Curse of Enfeeblement target

        private static bool GetCoEnPeelTarget(WoWUnit peelFrom) {

            if (peelFrom == null || !peelFrom.IsValid) {
                CoEnPeelTarget = null;
                return false;
            }

            CoEnPeelTarget = (from unit in NearbyUnFriendlyPlayers
                              where unit.IsValid
                              where unit.CurrentTarget != null && unit.CurrentTarget.IsValid && unit.CurrentTarget == peelFrom
                              where unit.Distance <= 30
                              where !InvulnerableSpell(unit)
                              where !unit.HasAura("Curse of Enfeeblement")
                              orderby unit.Distance ascending
                              select unit).FirstOrDefault();

            return CoEnPeelTarget != null;
        }

        #endregion

        #region Get Shadowfury target

        private static bool GetShadowfuryPeelTarget(WoWUnit peelFrom) {

            if (peelFrom == null || !peelFrom.IsValid) {
                ShadowfuryPeelTarget = null;
                return false;
            }

            ShadowfuryPeelTarget = (from unit in NearbyUnFriendlyPlayers
                                  where unit.IsValid
                                  //where AttackableValid(unit)
                                  //where unit.CurrentTarget != null && unit.CurrentTarget.IsValid && ((peelFrom == Me && unit.CurrentTarget == Me) || (peelFrom != Me && unit.CurrentTarget == peelFrom))
                                  where unit.CurrentTarget != null && unit.CurrentTarget.IsValid && unit.CurrentTarget == peelFrom
                                  where unit.Distance <= 30
                                  where !InvulnerableSpell(unit)
                                  orderby unit.Distance ascending
                                  select unit).FirstOrDefault();

            return ShadowfuryPeelTarget != null;
        }

        #endregion


        #region Count Melee Targeting Me

        private static double NumberOfMeleeTargetingMe() {

            return NearbyUnFriendlyPlayers.Where(
                    unit =>
                        unit != null && unit.IsValid &&
                        unit.CurrentTarget != null && unit.CurrentTarget.IsValid && unit.CurrentTarget == Me && TalentSort(unit) == 1 &&
                        unit.Distance2D <= 5
                    ).Count();
        }

        #endregion 

        #region Count Enemy DD Targeting Me

        private static double NumberOfEnemyDDTargetingMe() {

            return NearbyUnFriendlyPlayers.Where(
                    unit =>
                        unit != null && unit.IsValid &&
                        unit.CurrentTarget != null && unit.CurrentTarget.IsValid && unit.CurrentTarget == Me && TalentSort(unit) != 4
                    ).Count();
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
                if (target.MaxMana > target.MaxHealth / 2)
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

        private static double MyCorruptionTimeLeft(WoWUnit u) {

            if (u == null || !u.IsValid || !u.IsAlive)
                return 0;

            WoWAura aura = u.GetAllAuras().FirstOrDefault(a => (a.SpellId == 146739 || a.SpellId == 172) && a.CreatorGuid == Me.Guid);

            if (aura == null) {
                return 0;
            }

            return aura.TimeLeft.TotalMilliseconds;
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

            if (!AfflictionSettings.Instance.AutoFace || Me.IsMoving) {
                return false;
            }

            if (!Me.IsFacing(unit) && IsEnemy(unit) && unit.InLineOfSpellSight) {
                unit.Face();
            }

            return Me.IsSafelyFacing(unit);
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

        #region Update Glyphs and Talents

        private static void UpdateMyTalentOrGlyphEvent(object sender, LuaEventArgs args) {
            UpdateMyGlyph();
            UpdateMyTalent();

            if (AfflictionSettings.Instance.FakeCast) {
                AttachCombatLogEvent();
            } else {
                DetachCombatLogEvent();
            }

            banishTracker.Clear();
            fearTracker.Clear();
        }

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

            supremacy = TalentNames.Contains("Grimoire of Supremacy");
            sacrifice = TalentNames.Contains("Grimoire of Sacrifice");

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


        #region GetDispelTarget

        private static bool GetDispelTarget() {

            DispelTarget = (from unit in NearbyFriendlyPlayers
                                where unit.IsValid
                                where unit.InLineOfSpellSight
                                where unit.Distance2D <= 30
                                where DebuffCCleanseASAP(unit)
                                where TalentSort(unit) == 4
                                select unit).FirstOrDefault();

            if (DispelTarget == null && DebuffCCleanseASAP(Me)) {
                DispelTarget = Me;
            }

            return DispelTarget != null;
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

            return target.HasAura("Flame Shock") ||
                   target.HasAura("Unstable Affliction") ||
                   target.HasAura("Vampiric Touch");

        }

        #endregion

        #region buffs to dispel

        public readonly HashSet<string> DispellEnemyASAPHS = new HashSet<string>
            {
                "Power Word: Shield",
                "Ice Barrier",
                "Divine Aegis",
                "Hand of Freedom", 
                "Hand of Protection",
                "Presence of Mind",
                "Illuminated Healing"
            };

        public bool NeedDispellEnemyASAPTarget(WoWUnit target) {
            using (StyxWoW.Memory.AcquireFrame()) {
                if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet) {
                    return false;
                }

                if (target.ActiveAuras.Any(a => DispellEnemyASAPHS.Contains(a.Value.Name))) {
                    return true;
                }
                return false;
            }
        }

        #endregion

        #region DebuffRootorSnare

        private static readonly HashSet<int> DebuffRootorSnareHS = new HashSet<int> {
			96294, //Chains of Ice (Chilblains)
			116706, //Disable
			64695, //Earthgrab (Earthgrab Totem)
			339, //Entangling Roots
			113770, //Entangling Roots (Force of Nature - Balance Treants)
			19975, //Entangling Roots (Nature's Grasp)
			113275, //Entangling Roots (Symbiosis)
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

            if (target == null || !target.IsValid) {
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
                   target.HasAura("Banish") ||
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
                    a.Value.TimeLeft.TotalMilliseconds > 2000)) {
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

        private static bool DebuffMagicDot(WoWUnit target) {
            if (target == null || !target.IsValid || !target.IsPlayer && !target.IsPet) {
                return false;
            }

            return target.ActiveAuras.Any(a => (a.Value.Spell.DispelType == WoWDispelType.Magic || 
                                                a.Value.Spell.DispelType == WoWDispelType.Curse || 
                                                a.Value.Spell.DispelType == WoWDispelType.Disease) &&
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

        #region Combat Log

        private static void DetachCombatLogEvent() {
            if (!combatLogAttached)
                return;

            Lua.Events.RemoveFilter("COMBAT_LOG_EVENT_UNFILTERED");
            Logging.Write("Removed combat log filter");
            Lua.Events.DetachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);
            Logging.Write("Detached combat log");

            Logging.Write("Detached combat log");
            combatLogAttached = false;
        }

        private static void AttachCombatLogEvent() {
            if (combatLogAttached)
                return;

            Lua.Events.AttachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);

            string filterCriteria =
                "return args[8] == UnitGUID('player')"
                + " and args[4] ~= UnitGUID('player')"
                + " and args[2] == 'SPELL_CAST_SUCCESS'"
                + " and (args[12] == 102060 or" //Disrupting Shout
                + " args[12] == 106839 or" //Skull Bash
                + " args[12] == 80964 or"  //Skull Bash
                + " args[12] == 115781 or" //Optical Blast
                + " args[12] == 116705 or" //Spear Hand Strike
                + " args[12] == 1766 or"   //Kick
                + " args[12] == 19647 or"  //Spell Lock
                + " args[12] == 2139 or"   //Counterspell
                + " args[12] == 47528 or"  //Mind Freeze
                + " args[12] == 57994 or"  //Wind Shear
                + " args[12] == 6552 or"   //Pummel
                + " args[12] == 147362 or" //Counter Shot
                + " args[12] == 96231)";   //Rebuke
            

            if (!Lua.Events.AddFilter("COMBAT_LOG_EVENT_UNFILTERED", filterCriteria)) {
                Logging.Write("ERROR: Could not add combat log event filter!");
                Lua.Events.DetachEvent("COMBAT_LOG_EVENT_UNFILTERED", HandleCombatLog);
                Logging.Write("Detached combat log");
            }

            Logging.Write("Attached combat log");
            combatLogAttached = true;
        }


        private static void HandleCombatLog(object sender, LuaEventArgs args) {
  
            SpellManager.StopCasting();
            Logging.Write(DateTime.Now.ToString("ss:fff ") + "- [CombatLog] Stop casting because of an incoming " + args.Args[12].ToString());


            if (SpellManager.Spells["Corruption"].CooldownTimeLeft.TotalMilliseconds > 1500) {
                Logging.Write(DateTime.Now.ToString("ss:fff ") + "- [CombatLog] Couldn't stop casting in time.");
                UpdateGCD();
            } else {
                Logging.Write(DateTime.Now.ToString("ss:fff ") + "- [CombatLog] Cast successfully stopped before the interrupt.");
            }
        }

        #endregion


        #region log settings

        public static void printSettings() {
            Logging.WriteDiagnostic("Printing settings...");
            foreach (KeyValuePair<string, Object> kvp in AfflictionSettings.Instance.GetSettings()) {
                Logging.WriteDiagnostic("{0} = {1}", kvp.Key, kvp.Value);
            }
            Logging.WriteDiagnostic("Done");
        }

        #endregion

    }
}