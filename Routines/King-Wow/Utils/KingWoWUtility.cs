using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Styx.WoWInternals.DBC;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Styx.CommonBot;
using System.Windows.Forms;
using Styx.TreeSharp;

namespace KingWoW
{
    public class KingWoWUtility
    {
        public LocalPlayer Me { get { return Styx.StyxWoW.Me; } }
        public string[,] parties = new string[5, 8];

        public int total_group_number = 1;

        HealInterruptDispellsCleanse_Manager healInterrDispClea_Manager = null;
    
        public const int PW_BARRIER_RADIUS = 7;
        public const int HW_SANCTUARY_RADIUS = 8;
        public const int POM_RADIUS = 20;
        public const int COH_RADIUS = 30;
        public const int HEALING_RAIN_RADIUS = 10;
        public const int CHAIN_HEAL_RADIUS = 40;

        #region CC

        public bool IsCrowdControlledTarget(WoWUnit unit)
        {
            Dictionary<string, WoWAura>.ValueCollection auras = unit.Auras.Values;

            return unit.Fleeing
                || HasAuraWithEffect(unit,
                        WoWApplyAuraType.ModConfuse,
                        WoWApplyAuraType.ModCharm,
                        WoWApplyAuraType.ModFear,
                        WoWApplyAuraType.ModPossess);
        }

        public bool IsCrowdControlledPlayer(WoWUnit unit)
        {
            Dictionary<string, WoWAura>.ValueCollection auras = unit.Auras.Values;

            return unit.Stunned
                || unit.Rooted
                || unit.Fleeing
                || HasAuraWithEffect(unit,
                        WoWApplyAuraType.ModConfuse,
                        WoWApplyAuraType.ModCharm,
                        WoWApplyAuraType.ModFear,
                        WoWApplyAuraType.ModDecreaseSpeed,
                        WoWApplyAuraType.ModPacify,
                        WoWApplyAuraType.ModPacifySilence,
                        WoWApplyAuraType.ModPossess,
                        WoWApplyAuraType.ModRoot,
                        WoWApplyAuraType.ModStun);
        }

        // this one optimized for single applytype lookup
        public bool HasAuraWithEffect(WoWUnit unit, WoWApplyAuraType applyType)
        {
            return unit.Auras.Values.Any(a => a.Spell != null && a.Spell.SpellEffects.Any(se => applyType == se.AuraType));
        }

        public static bool HasAuraWithEffect(WoWUnit unit, params WoWApplyAuraType[] applyType)
        {
            var hashes = new HashSet<WoWApplyAuraType>(applyType);
            return unit.Auras.Values.Any(a => a.Spell != null && a.Spell.SpellEffects.Any(se => hashes.Contains(se.AuraType)));
        }

        #endregion

        private readonly uint[] MageFoodIds = new uint[]
                                                         {
                                                             65500,
                                                             65515,
                                                             65516,
                                                             65517,
                                                             43518,
                                                             43523,
                                                             65499, //Conjured Mana Cake - Pre Cata Level 85
                                                             80610, //Conjured Mana Pudding - MoP Lvl 85+
                                                             80618  //Conjured Mana Buns 
                                                             //This is where i made a change.
                                                         };

        public bool GotMagefood { get { return StyxWoW.Me.BagItems.Any(item => MageFoodIds.Contains(item.Entry)); } }

        public void UpdateTotalGroupNumber()
        {
            total_group_number = GetGroupMembers().Count() / 5;
            if ((GetGroupMembers().Count() % 5) > 0)
                total_group_number = total_group_number + 1;
        }

        public WoWItem FindFirstBagItemByName(string name)
        {
            List<WoWItem> carried = StyxWoW.Me.CarriedItems;
            
            return (from i in carried
                    where i.Name.Equals(name)
                    select i).FirstOrDefault();
        }

        public void RunMacroText(string macro)
        {
            Lua.DoString("RunMacroText(\"" + RealLuaEscape(macro) + "\")");
        }

        public string RealLuaEscape(string luastring)
        {
            var bytes = Encoding.UTF8.GetBytes(luastring);
            return bytes.Aggregate(String.Empty, (current, b) => current + ("\\" + b));
        }

        public bool UseBagItem(string name)
        {
            WoWItem item = null;
            item = Me.BagItems.FirstOrDefault(x => x.Name == name && x.Usable && x.Cooldown <= 0);
            if( item != null)
            {
                LogActivity("Using " + name);
                item.UseContainerItem();
                return true;
            }
            return false;
        }
       
        //string
        public uint PlayerCountBuff(string name)
        {
            return GetAuraStack(Me, name, true);
        }

        //int
        public uint PlayerCountBuff(int buffid)
        {
            return GetAuraStack(Me, buffid, true);
        }

        public bool CanCastPetAction(string action)
        {
            WoWPetSpell petAction = StyxWoW.Me.PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (petAction == null || petAction.Spell == null)
            {
                return false;
            }
            return !petAction.Spell.Cooldown;
        }

        public IEnumerable<WoWPlayer> NearbyFriendlyPlayersInRange(float range)
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(p => p.DistanceSqr <= range * range && p.IsFriendly).ToList();
        }

        public void CastPetAction(string action)
        {
            WoWPetSpell spell = StyxWoW.Me.PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null)
                return;

            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);
        }

        public bool MeIsCastingWithLag()
        {
            return (Me.IsCasting && Me.CurrentCastTimeLeft.Milliseconds <= StyxWoW.WoWClient.Latency);
        }

        //string
        public double MyAuraTimeLeft(string auraName, WoWUnit u)
        {           
            if (u == null || !u.IsValid || !u.IsAlive)
                return 0;

            var aura = u.GetAllAuras().FirstOrDefault(a => a.Name == auraName && a.CreatorGuid == Me.Guid);
            return aura != null ? aura.TimeLeft.TotalMilliseconds : 0;

        }

        //int
        public double MyAuraTimeLeft(int auraid, WoWUnit u)
        {
            if (u == null || !u.IsValid || !u.IsAlive)
                return 0;

            var aura = u.GetAllAuras().FirstOrDefault(a => a.SpellId == auraid && a.CreatorGuid == Me.Guid);
            return aura != null ? aura.TimeLeft.TotalMilliseconds : 0;

        }

        public bool HaveManaGem()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWItem>().Any(item => item.Entry == 36799 || item.Entry == 81901);
        }

        public int HowManyManaGem()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWItem>().Count(item => item.Entry == 36799 || item.Entry == 81901);
        }

        public bool MeIsChanneling { get { return StyxWoW.Me.ChanneledCastingSpellId != 0 && StyxWoW.Me.IsChanneling; } }

        public int COHRadius()
        {
            return COH_RADIUS;
        }

        public int PW_SanctuaryRadius()
        {
            return HW_SANCTUARY_RADIUS;
        }

        public int HEALING_RAIN_Radius()
        {
            return HEALING_RAIN_RADIUS;
        }

        public int POM_Radius()
        {
            return POM_RADIUS;
        }

        public int CHAIN_HEAL_Radius()
        {
            return CHAIN_HEAL_RADIUS;
        }

        //string
        public uint GetAuraStack(WoWUnit unit, string auraName, bool fromMyAura)
        {
            if (unit != null)
            {
                var wantedAura = unit.GetAllAuras().FirstOrDefault(a => a.Name == auraName && a.StackCount > 0 && (!fromMyAura || a.CreatorGuid == Me.Guid));
                return wantedAura != null ? wantedAura.StackCount : 0;
            }

            LogActivity(" [GetAuraStack] Unit is null ");
            return 0;
        }

        //int
        public uint GetAuraStack(WoWUnit unit, int auraid, bool fromMyAura)
        {
            if (unit != null)
            {
                var wantedAura = unit.GetAllAuras().FirstOrDefault(a => a.SpellId == auraid && a.StackCount > 0 && (!fromMyAura || a.CreatorGuid == Me.Guid));
                return wantedAura != null ? wantedAura.StackCount : 0;
            }

            LogActivity(" [GetAuraStack] Unit is null ");
            return 0;
        }
        
        //string
        public bool HasAura(WoWUnit unit, string aura, WoWUnit creator)
        {
            return unit != null && unit.GetAllAuras().Any(a => a.Name == aura && (creator == null || a.CreatorGuid == creator.Guid));
        }

        //int
        public bool HasAura(WoWUnit unit, int spellId, WoWUnit creator)
        {
            return unit != null && unit.GetAllAuras().Any(a => a.SpellId == spellId && (creator == null || a.CreatorGuid == creator.Guid));
        }

        public bool HasAnyAura(WoWUnit unit, params int[] auraIDs)
        {
            return auraIDs.Any(unit.HasAura);
        }

        //string
        public bool PlayerHasBuff(string name)
        {
            return HasAura(Me, name, null);
        }

        //int
        public bool PlayerHasBuff(int auraid)
        {
            return HasAura(Me, auraid, null);
        }

        public WoWUnit getTargetToAttack(double range,WoWUnit tank_passed, bool avoidCC=false)
        {
            WoWUnit target = Me.CurrentTarget;
            if (target == null || target.IsFriendly || !target.Attackable || !target.IsValid || !target.IsAlive || !target.InLineOfSpellSight || target.Distance - target.CombatReach -1  > range
                || (avoidCC && IsCrowdControlledTarget(target)))                
            {
                if (tank_passed != null && tank_passed.IsAlive)
                {
                    target = tank_passed.CurrentTarget;
                    if (target == null || target.IsFriendly || !target.Attackable || !target.IsValid || !target.IsAlive || !target.InLineOfSpellSight || target.Distance - target.CombatReach -1  > range
                        || (avoidCC && IsCrowdControlledTarget(target)))                        
                    {
                        target = NearestAttaccableEnemyMobInRangeTargettingMyParty(range,avoidCC);
                    }
                    return target;

                }
                target = NearestAttaccableEnemyMobInRangeTargettingMyParty(range,avoidCC);
            }
            return target;
        }

        public WoWUnit getTargetPVPToAttack(double range, WoWUnit tank_passed, bool avoidCC = false)
        {
            WoWUnit target = Me.CurrentTarget;
            if (target == null || target.IsFriendly || !target.Attackable || !target.IsValid || !target.IsAlive || !target.InLineOfSpellSight || target.Distance - target.CombatReach -1  > range
                || (avoidCC && IsCrowdControlledTarget(target)) 
                || healInterrDispClea_Manager.InvulnerableTarget(target)
                || healInterrDispClea_Manager.InvulnerableToSpellTarget(target))
            {
                if (tank_passed != null && tank_passed.IsAlive)
                {
                    target = tank_passed.CurrentTarget;
                    if (target == null || target.IsFriendly || !target.Attackable || !target.IsValid || !target.IsAlive || !target.InLineOfSpellSight || target.Distance - target.CombatReach -1  > range
                        || (avoidCC && IsCrowdControlledTarget(target))
                        || healInterrDispClea_Manager.InvulnerableTarget(target)
                        || healInterrDispClea_Manager.InvulnerableToSpellTarget(target))
                    {
                        target = NearestAttaccableEnemyInRangePVPTargettingMyParty(range, avoidCC);
                    }
                    return target;

                }
                target = NearestAttaccableEnemyInRangePVPTargettingMyParty(range, avoidCC);
            }
            return target;
        }
       
        public IEnumerable<WoWUnit> AllAttaccableEnemyMobsInRange (double range)
        {
            return AllEnemyMobs.Where(u =>
                        (u.Distance - u.CombatReach -1  <= range && u.InLineOfSpellSight && u.Attackable));
        }

       

        public IEnumerable<WoWUnit> AllAttaccableEnemyMobsInRangeFromTarget(WoWUnit target, double range)
        {
            return AllEnemyMobs.Where(u =>
                        (u.Location.Distance(target.Location) - u.CombatReach -1  <= range && u.InLineOfSpellSight && u.Attackable));
        }

        public IEnumerable<WoWUnit> AllAttaccableEnemyMobsInRangeTragettingMyParty(double range, bool avoidCC=false)
        {         
            if(avoidCC)
                return AllEnemyMobs.Where(u =>
                    (u.Distance - u.CombatReach -1  <= range && u.InLineOfSpellSight && u.Combat && u.Attackable && u.IsAlive && u.IsValid && !u.IsPet && !IsCrowdControlledTarget(u) &&
                    (u.IsTargetingMeOrPet || u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember)) || (!Me.IsInInstance && u.Name.Contains("Dummy")));
            return AllEnemyMobs.Where(u =>
                    (u.Distance - u.CombatReach -1  <= range && u.InLineOfSpellSight && u.Combat && u.Attackable && u.IsAlive && u.IsValid && !u.IsPet &&
                    (u.IsTargetingMeOrPet || u.IsTargetingMyPartyMember || u.IsTargetingMyRaidMember)) || (!Me.IsInInstance && u.Name.Contains("Dummy")));
        }

        public WoWUnit NearestAttaccableEnemyMobInRangeTargettingMyParty(double range, bool avoidCC=false)
        {
            return AllAttaccableEnemyMobsInRangeTragettingMyParty(range,avoidCC).OrderBy(uu => uu.Distance).FirstOrDefault();
        }

        public WoWUnit NearestAttaccableEnemyInRangePVPTargettingMyParty(double range, bool avoidCC = false)
        {
            return AllAttaccableEnemyMobsInRangeTragettingMyParty(range, avoidCC).Where(uu => !healInterrDispClea_Manager.InvulnerableTarget(uu) && !healInterrDispClea_Manager.InvulnerableToSpellTarget(uu)).OrderBy(uu => uu.Distance).FirstOrDefault();
        }

        public WoWUnit NearestAttaccableEnemyMobInRange(double range)
        {
            return AllAttaccableEnemyMobsInRange(range).OrderBy(uu => uu.Distance).FirstOrDefault();
        }

        public bool IsGlobalCooldown(bool allowLagTollerance = true)
        {
            uint latency = allowLagTollerance ? StyxWoW.WoWClient.Latency*2 : 0;
            TimeSpan gcdTimeLeft = SpellManager.GlobalCooldownLeft;
            return gcdTimeLeft.TotalMilliseconds > latency;
            //return SpellManager.GlobalCooldown;
        }

        //string
        public IEnumerable<WoWUnit> AllEnemyMobsHasMyAura(string aura)
        {
            return AllEnemyMobs.Where(u => MyAuraTimeLeft(aura, u) > 0);
        }

        //int
        public IEnumerable<WoWUnit> AllEnemyMobsHasMyAura(int auraid)
        {
            return AllEnemyMobs.Where(u => MyAuraTimeLeft(auraid, u) > 0);
        }

        //string
        public IEnumerable<WoWUnit> AllEnemyMobsHasMyAuraInRange(string aura,double range)
        {
            return AllEnemyMobs.Where(u => MyAuraTimeLeft(aura, u) > 0 && u.Distance - u.CombatReach - 1 <= range);
        }

        //int
        public IEnumerable<WoWUnit> AllEnemyMobsHasMyAuraInRange(int auraid, double range)
        {
            return AllEnemyMobs.Where(u => MyAuraTimeLeft(auraid, u) > 0 && u.Distance - u.CombatReach - 1 <= range);
        }

        //string
        public WoWUnit NextApplyAuraTarget(string aura, double range, double timeToRefresh, bool avoidCC)
        {
            WoWUnit target = (from unit in AllAttaccableEnemyMobsInRangeTragettingMyParty(range,avoidCC) 
                       //where unit.ThreatInfo.RawPercent < 90
                       where MyAuraTimeLeft(aura, unit) < timeToRefresh
                       select unit).FirstOrDefault();
            return target;
        }

        //int
        public WoWUnit NextApplyAuraTarget(int auraid, double range, double timeToRefresh, bool avoidCC)
        {
            WoWUnit target = (from unit in AllAttaccableEnemyMobsInRangeTragettingMyParty(range, avoidCC)
                              //where unit.ThreatInfo.RawPercent < 90
                              where MyAuraTimeLeft(auraid, unit) < timeToRefresh
                              select unit).FirstOrDefault();
            return target;
        }

        public bool IsBotBaseInUse(string botname)
        {
            return BotManager.Current.Name == botname || BotManager.Current.Name.StartsWith(botname);
        }

        public IEnumerable<WoWUnit> AllEnemyMobsAttackingMe
        {
            get
            {
                return AllEnemyMobs.Where(u =>
                        (u.CurrentTargetGuid == Me.Guid || Me.CurrentTargetGuid == u.Guid)
                        && !u.IsPet);
            }
        }

        public bool IsEnemy(WoWUnit u)
        {
            if (u == null || !u.CanSelect || !u.Attackable || !u.IsAlive || u.IsNonCombatPet || u.IsCritter)
                return false;

            if (!u.IsPlayer)
                return u.IsHostile || u.Aggro || u.PetAggro || (!Me.IsInInstance && u.Name.Contains("Dummy"));

            WoWPlayer p = u.ToPlayer();
            if(p!=null)
                return p.IsHorde != StyxWoW.Me.IsHorde;

            return false;
        }

        public IEnumerable<WoWUnit> AllEnemyMobs
        {
            get
            {
                return ObjectManager.ObjectList.Where(o => o is WoWUnit && IsEnemy(o.ToUnit())).Select(o => o.ToUnit()).ToList();
            }
        }

        public WoWUnit NearestEnemyMobAttackingMe
        {
            get
            {
                return AllEnemyMobsAttackingMe.OrderBy(uu => uu.Distance).FirstOrDefault();
            }
        } 
        
        public KingWoWUtility()
        {
            if (healInterrDispClea_Manager == null)
                healInterrDispClea_Manager = new HealInterruptDispellsCleanse_Manager();
        }

        public bool NeedsCure(WoWUnit unit)
        {
            if (unit.ActiveAuras.Any(a => a.Value.IsHarmful && a.Value.Spell.DispelType == WoWDispelType.Poison))
                return true;
            return false;
        }

        public bool NeedsDeCurse(WoWUnit unit)
        {
            if (unit.ActiveAuras.Any(a => a.Value.IsHarmful && a.Value.Spell.DispelType == WoWDispelType.Curse))
                return true;
            return false;
        }

        public bool EnemyNeedDispellASAP(WoWUnit unit)
        {
            return healInterrDispClea_Manager.NeedDispellEnemyASAPTarget(unit);
        }

        public bool EnemyHasShield(WoWUnit unit)
        {
            return healInterrDispClea_Manager.AnyShieldTarget(unit);
        }

        public bool NeedsDispelPriest(WoWUnit unit)
        {
            if (healInterrDispClea_Manager.TargetDontDispell(unit))
                return false;
            if (unit.ActiveAuras.Any(a => a.Value.IsHarmful && a.Value.Spell.DispelType == WoWDispelType.Magic || a.Value.Spell.DispelType == WoWDispelType.Disease))
                return true;
            return false;
        }

        public bool NeedsDispelShammy(WoWUnit unit)
        {
            if (healInterrDispClea_Manager.TargetDontDispell(unit))
                return false;
            if (unit.ActiveAuras.Any(a => a.Value.IsHarmful && a.Value.Spell.DispelType == WoWDispelType.Magic || a.Value.Spell.DispelType == WoWDispelType.Curse))
                return true;
            return false;
        }

        public bool HasRootSnare(WoWUnit unit)
        {
            return healInterrDispClea_Manager.AnyRootorSnareAtTarget(unit);
        }

        public bool NeedsDispelASAP(WoWUnit unit, double time = 0)
        {
            if (healInterrDispClea_Manager.TargetDontDispell(unit))
                return false;
            if (healInterrDispClea_Manager.TargetDispellASAP(unit,time))
                return true;
            return false;
        }

        public int GetMemberCountBelowThreshold(int threshold,double range=40)
        {
            return Enumerable.Count((from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                                         orderby unit.HealthPercent ascending
                                         where (!unit.IsDead
                                         && (unit.IsInMyPartyOrRaid || unit.IsMe)
                                         && !unit.IsGhost
                                         && unit.Distance-unit.CombatReach -1  <= range
                                         && unit.HealthPercent <= threshold)
                                         select unit));
        }

        public int GetMemberCountBelowThresholdInRangeFromPlayer(int threshold, WoWUnit from_player, double range)
        {
            
            if (from_player != null)
                return Enumerable.Count((from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                                            orderby unit.HealthPercent ascending
                                            where (!unit.IsDead
                                            && (unit.IsInMyPartyOrRaid || unit.IsMe)
                                            && !unit.IsGhost
                                            && unit.HealthPercent <= threshold
                                            && GetDistance(unit.Location, from_player.Location) - unit.CombatReach -1  <= range)
                                            select unit));

            return 0;
        }

        public WoWUnit GetHealTarget(double range)
        {
            //Tsulong override
            /*if (Me.GotTarget && Me.CurrentTarget.Entry == 62442 && (Me.CurrentTarget.Distance - Me.CurrentTarget.CombatReach -1  <= range) &&
                (!Me.CurrentTarget.Attackable || Me.CurrentTarget.IsFriendly)
                && Me.CurrentTarget.InLineOfSpellSight)
            {
                return Me.CurrentTarget;
            }*/                         
                return (from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                        orderby unit.HealthPercent ascending
                        where ((unit.IsInMyPartyOrRaid || unit.IsMe)
                        && !healInterrDispClea_Manager.TargetDontHeal(unit)
                        && !unit.IsDead
                        && !unit.IsGhost
                        && unit.IsFriendly
                        && unit.Distance - unit.CombatReach -1  <= range
                        && unit.InLineOfSpellSight)
                        select unit).FirstOrDefault();
        }

        public IEnumerable<WoWPlayer> GetResurrectTargets(double range)
        {           
            return (from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                    orderby unit.MaxHealth descending
                    where (unit.IsDead
                    && unit.IsInMyPartyOrRaid
                    && unit.InLineOfSpellSight
                    && unit.Distance - unit.CombatReach -1  <= range
                    && !Blacklist.Contains(unit.Guid, BlacklistFlags.All))
                    select unit);
        }

        public WoWPlayer GetDispellTargetShammy(double range)
        {
            return (from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                    orderby unit.HealthPercent ascending
                    where (!unit.IsDead
                    && !unit.IsGhost
                    && unit.Distance - unit.CombatReach -1  <= range
                    && unit.InLineOfSpellSight
                    && NeedsDispelShammy(unit)
                    && (unit.IsInMyPartyOrRaid || unit.IsMe))
                    select unit).FirstOrDefault();
        }

        public WoWPlayer GetCureTarget(double range)
        {
                return (from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                        orderby unit.HealthPercent ascending
                        where (!unit.IsDead
                        && !unit.IsGhost
                        && unit.Distance - unit.CombatReach -1  <= range
                        && unit.InLineOfSpellSight
                        && NeedsCure(unit)
                        && (unit.IsInMyPartyOrRaid || unit.Guid == Me.Guid))
                        select unit).FirstOrDefault();
        }

        public WoWPlayer GetDispelTargetPriest(double range)
        {
            return (from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                    orderby unit.HealthPercent ascending
                    where (!unit.IsDead
                    && !unit.IsGhost
                    && unit.Distance - unit.CombatReach -1  <= range
                    && unit.InLineOfSpellSight
                    && NeedsDispelPriest(unit)
                    && (unit.IsInMyPartyOrRaid || unit.Guid == Me.Guid))
                    select unit).FirstOrDefault();
        }

        public WoWPlayer GetDispelASAPTarget(double range, double time=0)
        {
            return (from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                    orderby unit.HealthPercent ascending
                    where (!unit.IsDead
                    && !unit.IsGhost
                    && unit.Distance - unit.CombatReach -1  <= range
                    && unit.InLineOfSpellSight
                    && NeedsDispelASAP(unit,time)
                    && (unit.IsInMyPartyOrRaid || unit.Guid == Me.Guid))
                    select unit).FirstOrDefault();
        }

        public WoWPlayer GetDecurseTarget(double range)
        {
            return (from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                    orderby unit.HealthPercent ascending
                    where (!unit.IsDead
                    && !unit.IsGhost
                    && unit.Distance - unit.CombatReach -1  <= range
                    && unit.InLineOfSpellSight
                    && NeedsDeCurse(unit)
                    && (unit.IsInMyPartyOrRaid || unit.Guid == Me.Guid))
                    select unit).FirstOrDefault();
        }

        public int MassDispelCountForPlayer(WoWPlayer player, double range)
        {
            if (player != null)
                return Enumerable.Count((from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                        where (!unit.IsDead
                        && (unit.IsInMyPartyOrRaid || unit.IsMe)
                        && !unit.IsGhost
                        && NeedsDispelPriest(unit)
                        && GetDistance(player.Location, unit.Location - unit.CombatReach -1 ) <= range)
                        select unit));
            return 0;
        }

        public int MassDispelASAPCountForPlayer(WoWPlayer player, double range, double time=0)
        {
            if (player != null)
                return Enumerable.Count((from unit in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                                         where (!unit.IsDead
                                         && (unit.IsInMyPartyOrRaid || unit.IsMe)
                                         && !unit.IsGhost
                                         && NeedsDispelASAP(unit, time)
                                         && GetDistance(player.Location, unit.Location - unit.CombatReach -1 ) <= range)
                                         select unit));
            return 0;
        }

        //string
        public bool isAuraActive(string name)
        {
            //return isAuraActive(name, Me);
            return Me.HasAura(name);
        }

        //int
        public bool isAuraActive(int auraid)
        {
            //return isAuraActive(auraid, Me);
            return Me.HasAura(auraid);
        }

        //string
        public bool isAuraActive(string name, WoWUnit unit)
        {
            return HasAura(unit, name, null);
        }

        //int
        public bool isAuraActive(int auraid, WoWUnit unit)
        {
            return HasAura(unit, auraid, null);
        }

        public void LogActivity(params string[] words)
        {
            Logging.Write(DateTime.Now.ToString("ss:fff") + " my HP% " + Math.Round(Me.HealthPercent, 2) +
            "    MANA% " + Math.Round(Me.ManaPercent, 2) + "     " + String.Join(": ", words));
        }

        public void LogHealActivity(WoWUnit unit,params string[] words)
        {
            Logging.Write(DateTime.Now.ToString("ss:fff") + "target   HP%=" + Math.Round(unit.HealthPercent,2) + "   " + String.Join(": ", words));
        }

        //int
        public bool CanCast(int spellid, bool range_ck)
        {
            //return SpellManager.CanCast(spellid, range_ck);
            return SpellManager.CanCast(spellid);
        }

        //string
        public bool CanCast(string spell, bool range_ck)
        {
            //return SpellManager.CanCast(spell, range_ck);
            return SpellManager.CanCast(spell);
        }

        //int
        public bool CanCast(int spellid, WoWUnit target)
        {
            return SpellManager.CanCast(spellid, target);
            //return SpellManager.CanCast(spellid);
        }

        //string
        public bool CanCast(string spell, WoWUnit target)
        {
            return SpellManager.CanCast(spell, target);
            //return SpellManager.CanCast(spell);
        }

        //int
        public bool CanCast(int spellid, WoWUnit target, bool range_ck)
        {
            //return SpellManager.CanCast(spellid, target, range_ck);
            return SpellManager.CanCast(spellid, target);
        }

        //string
        public bool CanCast(string spell, WoWUnit target, bool range_ck)
        {
            //return SpellManager.CanCast(spell, target, range_ck);
            return SpellManager.CanCast(spell, target);
        }

        //string
        public bool CanCast(string spell)
        {
            return SpellManager.CanCast(spell);
        }

        //int
        public bool CanCast(int spellid)
        {
            return SpellManager.CanCast(spellid);
        }

        //string
        public bool Cast(string spell, WoWUnit target)
        {
            return SpellManager.Cast(spell, target);
        }

        //int
        public bool Cast(int spellid, WoWUnit target)
        {
            return SpellManager.Cast(spellid, target);
        }

        //string
        public TimeSpan GetSpellCooldown(string spell)
        {
            SpellFindResults sfr;
            if (SpellManager.FindSpell(spell, out sfr))
                return (sfr.Override ?? sfr.Original).CooldownTimeLeft;

            return TimeSpan.MaxValue;
        }

        //int
        public TimeSpan GetSpellCooldown(int spellid)
        {
            SpellFindResults sfr;
            if (SpellManager.FindSpell(spellid, out sfr))
                return (sfr.Override ?? sfr.Original).CooldownTimeLeft;

            return TimeSpan.MaxValue;
        }

        //string
        public bool Cast(string spell)
        {
            return SpellManager.Cast(spell, Me);
        }

        //int
        public bool Cast(int spellid)
        {
            return SpellManager.Cast(spellid, Me);
        }

        //string
        public bool GroupNeedsMyAura(string aura)
        {
            if (GroupMembers.Any(p => p.ToPlayer() != null && (p.ToPlayer().IsInMyPartyOrRaid || p.ToPlayer().IsMe) && p.ToPlayer().HasAura(aura) && p.ToPlayer().Auras[aura].CreatorGuid == StyxWoW.Me.Guid))
                return false;
            return true;
        }

        //int
        public bool GroupNeedsMyAura(int auraid)
        {
            if (GroupMembers.Any(p => p.ToPlayer() != null && (p.ToPlayer().IsInMyPartyOrRaid || p.ToPlayer().IsMe) && p.ToPlayer().HasAura(auraid) && p.ToPlayer().GetAuraById(auraid).CreatorGuid == StyxWoW.Me.Guid))
                return false;
            return true;

        }

        public void ResetParties()
        {
            using (StyxWoW.Memory.AcquireFrame())
            {
                for (int player_number = 0; player_number < 5; player_number++)
                {
                    for (int group_number = 0; group_number < 8; group_number++)
                    {
                        parties[player_number, group_number] = " ";
                    }
                }
            }
        }

        internal static IEnumerable<WoWPartyMember> GroupPartyMembers
        {
            get { return StyxWoW.Me.GroupInfo.PartyMembers; }
        }

        internal static IEnumerable<WoWPartyMember> GroupRaidMembers
        {
            get { return StyxWoW.Me.GroupInfo.RaidMembers; }
        }

        internal static IEnumerable<WoWPartyMember> GroupMembers
        {
            get { return !StyxWoW.Me.GroupInfo.IsInRaid ? StyxWoW.Me.GroupInfo.PartyMembers : StyxWoW.Me.GroupInfo.RaidMembers; }
        }

        public IEnumerable<WoWPartyMember> GetGroupMembers()
        {
            return GroupMembers;
        }

        public void FillParties()
        {
            int group_1_index,group_2_index,group_3_index,group_4_index,group_5_index,group_6_index,group_7_index,group_8_index;
            group_1_index=group_2_index=group_3_index=group_4_index=group_5_index=group_6_index=group_7_index=group_8_index=0;

            ResetParties();

            //using (StyxWoW.Memory.AcquireFrame())
            //{
                foreach (var woWPartyMember in GroupMembers)
                {
                    if (woWPartyMember.ToPlayer() != null)
                    {
                        switch (woWPartyMember.GroupNumber)
                        {
                            case 0:
                                {
                                    parties[group_1_index, woWPartyMember.GroupNumber] = woWPartyMember.ToPlayer().Name;
                                    group_1_index++;
                                }
                                break;
                            case 1:
                                {
                                    parties[group_2_index, woWPartyMember.GroupNumber] = woWPartyMember.ToPlayer().Name;
                                    group_2_index++;
                                }
                                break;
                            case 2:
                                {
                                    parties[group_3_index, woWPartyMember.GroupNumber] = woWPartyMember.ToPlayer().Name;
                                    group_3_index++;
                                }
                                break;
                            case 3:
                                {
                                    parties[group_4_index, woWPartyMember.GroupNumber] = woWPartyMember.ToPlayer().Name;
                                    group_4_index++;
                                }
                                break;
                            case 4:
                                {
                                    parties[group_5_index, woWPartyMember.GroupNumber] = woWPartyMember.ToPlayer().Name;
                                    group_5_index++;
                                }
                                break;
                            case 5:
                                {
                                    parties[group_6_index, woWPartyMember.GroupNumber] = woWPartyMember.ToPlayer().Name;
                                    group_6_index++;
                                }
                                break;
                            case 6:
                                {
                                    parties[group_7_index, woWPartyMember.GroupNumber] = woWPartyMember.ToPlayer().Name;
                                    group_7_index++;
                                }
                                break;
                            case 7:
                                {
                                    parties[group_8_index, woWPartyMember.GroupNumber] = woWPartyMember.ToPlayer().Name;
                                    group_8_index++;
                                }
                                break;
                        }
                    }

                }
            //}
        }

        public int GetPlayerParty(WoWPlayer p)
        {
            int party = 0;
            //using (StyxWoW.Memory.AcquireFrame())
            //{
                for (int player_number = 0; player_number < 5; player_number++)
                {
                    for (int group_number = 0; group_number < 8; group_number++)
                    {
                        if (p.Name.Equals(parties[player_number, group_number]))
                        {
                            return party;
                        }
                    }
                    party++;
                }
            //}
            return -1;
        }

        public int GetDistance(WoWPoint p1, WoWPoint p2)
        {
            double XDif = Math.Pow((p2.X - p1.X), 2);
            double YDif = Math.Pow((p2.Y - p1.Y), 2);
            //double ZDif = Math.Pow((p2.Z - p1.Z), 2);
            int distance = (int)Math.Sqrt(XDif + YDif);

            return distance;
        }

        public int AOEHealCount(WoWPlayer player, int hp, int dist)
        {
            if (player == null)
                return 0;
            return Enumerable.Count((from p in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>()
                                     where ((p.IsInMyPartyOrRaid || p.IsMe)
                                     && GetDistance(player.Location, p.Location) - p.CombatReach -1  <= dist
                                     && p.IsAlive
                                     && p.HealthPercent <= hp)
                                     select p));
        }

        public int GetPartyAOECount(WoWPartyMember player, int hp)
        {
            if (player == null || player.ToPlayer() == null)
                return 0;
            return Enumerable.Count((from p in GroupMembers
                                    where (p.ToPlayer()!=null
                                    && p.GroupNumber == player.GroupNumber
                                    && GetDistance(player.ToPlayer().Location, p.ToPlayer().Location) - p.ToPlayer().CombatReach -1  <= 30
                                    && p.ToPlayer().IsAlive
                                    && p.ToPlayer().HealthPercent <= hp)
                                    select p));
        }

        //string
        /*public bool AnyOnePlayerGroupPartyHasAura(WoWPlayer player, string AURA)
        {
            int reference_player_group_number = GetPlayerParty(player);
            foreach (var woWPartyMember in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>(true, true))
            {
                WoWPlayer p = woWPartyMember.ToPlayer();
                if (p != null)
                {
                    int curr_player_group = GetPlayerParty(p);
                    if (p.IsAlive
                        && p.IsInMyPartyOrRaid
                        && p.HasAura(AURA)
                        && p.Auras[AURA].CreatorGuid == StyxWoW.Me.Guid
                        && curr_player_group == reference_player_group_number)
                    {
                        return true;
                    }
                }
            }
            return false;

        }*/

        //int
        public bool AnyOnePlayerGroupPartyHasAura(WoWPlayer player, int AURA)
        {
            int reference_player_group_number = GetPlayerParty(player);
            foreach (var woWPartyMember in GroupMembers)
            {
                WoWPlayer p = woWPartyMember.ToPlayer();
                if (p != null)
                {
                    int curr_player_group = GetPlayerParty(p);
                    if (p.IsAlive
                        && p.IsInMyPartyOrRaid
                        && HasAura(p,AURA,Me)
                        && curr_player_group == reference_player_group_number)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        /*public WoWPlayer NextSpiritShieldTarget(int SPIRIT_SHELL_SPELL)
        {
            if (Me.CurrentTarget != null && Me.CurrentTarget.IsFriendly && Me.CurrentTarget.IsAlive && Me.CurrentTarget.IsPlayer
                && Me.CurrentTarget.Distance - Me.CurrentTarget.CombatReach -1  <= 40f)
                return Me.CurrentTarget.ToPlayer();
            //using (StyxWoW.Memory.AcquireFrame())
            //{
                foreach (var woWPartyMember in GroupMembers)
                {
                    WoWPlayer p = woWPartyMember.ToPlayer();
                    if (p != null
                        && p.Distance2DSqr < 40*40
                        && p.IsAlive
                        && p.InLineOfSight
                        && p.IsInMyPartyOrRaid
                        && (!p.HasAura(SPIRIT_SHELL_SPELL) || !HasAura(p,SPIRIT_SHELL_SPELL,Me))
                        && !AnyOnePlayerGroupPartyHasAura(p, SPIRIT_SHELL_SPELL))
                    {
                        return p;
                    }
                }
            //}
            return null;
        }*/

        WoWPartyMember PoHPlayerCandidate()
        {
            return (from unit in GroupMembers
                    where unit.ToPlayer() != null
                    orderby unit.ToPlayer().HealthPercent ascending
                    where (                  
                    !healInterrDispClea_Manager.TargetDontHeal(unit.ToPlayer())
                    && !unit.ToPlayer().IsDead
                    && !unit.ToPlayer().IsGhost
                    && unit.ToPlayer().IsFriendly
                    && unit.ToPlayer().Distance - unit.ToPlayer().CombatReach -1  <= 40f
                    && unit.ToPlayer().InLineOfSpellSight)
                    select unit).FirstOrDefault();
        }

        public WoWPlayer BestPoHTarget(int hp, int count, bool lightFind=false)
        {
            if (lightFind)
            {
                WoWPartyMember playerCandidate = PoHPlayerCandidate();
                if (playerCandidate != null && playerCandidate.ToPlayer() != null && GetPartyAOECount(playerCandidate, hp) >= count)
                    return playerCandidate.ToPlayer();
                else
                    return null;
            }
            else
            {
                WoWPartyMember tar = null;
                //using (StyxWoW.Memory.AcquireFrame())
                //{
                foreach (var p in GroupMembers)
                {
                    if (p.ToPlayer() != null
                        && p.ToPlayer().Distance2DSqr <= 40 * 40
                        && p.ToPlayer().IsAlive
                        && p.ToPlayer().InLineOfSpellSight
                        && GetPartyAOECount(p, hp) > GetPartyAOECount(tar, hp))
                    {
                        tar = p;
                    }
                }

                if (GetPartyAOECount(tar, hp) >= count)
                {
                    return tar.ToPlayer();
                }
                else
                {
                    return null;
                }
                //}
            }
        }

        public IEnumerable<WoWUnit> FacingUnit(float range, int hp, bool friendly, bool enemy)
        {
            if (friendly && enemy)
                return ObjectManager.ObjectList.Where(o => o is WoWUnit && ((o.ToUnit().IsFriendly && o.ToUnit().HealthPercent <= hp) || (IsEnemy(o.ToUnit()) && o.ToUnit().Attackable))  
                && o.ToUnit().Distance - o.ToUnit().CombatReach -1  <= range && o.ToUnit().InLineOfSpellSight && Me.IsFacing(o)).Select(o => o.ToUnit()).ToList();
            else if (friendly && !enemy)
                return ObjectManager.ObjectList.Where(o => o is WoWUnit && o.ToUnit().IsFriendly && o.ToUnit().HealthPercent <= hp
                && o.ToUnit().Distance - o.ToUnit().CombatReach - 1 <= range && o.ToUnit().InLineOfSpellSight && Me.IsFacing(o)).Select(o => o.ToUnit()).ToList();
            else if (!friendly && enemy)
                return ObjectManager.ObjectList.Where(o => o is WoWUnit && IsEnemy(o.ToUnit()) && o.ToUnit().Attackable
                && o.ToUnit().Distance - o.ToUnit().CombatReach - 1 <= range && o.ToUnit().InLineOfSpellSight && Me.IsFacing(o)).Select(o => o.ToUnit()).ToList();
            return null;
        }

        public WoWUnit BestDSTarget(int hp, int count, bool offensive)
        {
            IEnumerable<WoWUnit> FacingTargets = FacingUnit(30, hp, true, offensive);
            if (FacingTargets.Count() >= count)
            {
                //LogActivity("I'm Facing " + FacingTargets.Count());
                return FacingTargets.OrderBy(uu => uu.Distance).LastOrDefault();
            }
            else
                return null;

        }

        public WoWPlayer BestPWBTargetLocation(int hp, int count,bool lightFind=false)
        {
            if (lightFind)
            {
                WoWUnit playerCandidate = GetHealTarget(40f);
                if (playerCandidate != null && playerCandidate.IsAlive && AOEHealCount(playerCandidate.ToPlayer(), hp, PW_BARRIER_RADIUS) >= count)
                    return playerCandidate.ToPlayer();
                else
                    return null;
            }
            else
            {
                WoWPlayer tar = StyxWoW.Me;
                foreach (WoWPlayer p in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>())
                {
                    if (p.Distance2DSqr < 40 * 40
                        && p.IsAlive
                        && p.InLineOfSight
                        && AOEHealCount(p, hp, PW_BARRIER_RADIUS) > AOEHealCount(tar, hp, PW_BARRIER_RADIUS))
                    {
                        tar = p;
                    }
                }
                if (AOEHealCount(tar, hp, PW_BARRIER_RADIUS) >= count)
                {
                    return tar;
                }
                else
                {
                    return null;
                }
            }
        }

        public WoWPlayer BestHealingRainTargetLocation(bool lockframe, int hp, int count)
        {
            WoWPlayer tar = StyxWoW.Me;
            WoWUnit playerCandidate = GetHealTarget(40f);
            if (playerCandidate != null && playerCandidate.IsAlive && AOEHealCount(playerCandidate.ToPlayer(), hp, HEALING_RAIN_RADIUS) >= count)
                return playerCandidate.ToPlayer();
            else
                return null;
            /*
            if (lockframe)
            {
                //using (StyxWoW.Memory.AcquireFrame())
                //{
                    foreach (WoWPlayer p in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>(true, true))
                    {
                        if (p.Distance2DSqr < 40*40
                            && p.IsAlive
                            && p.InLineOfSpellSight
                            && AOEHealCount(p, hp, HEALING_RAIN_RADIUS) > AOEHealCount(tar, hp, HEALING_RAIN_RADIUS))
                        {
                            tar = p;
                        }
                    }
                //}
            }
            else
            {
                foreach (WoWPlayer p in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>(true, true))
                {
                    if (p.Distance2DSqr < 40*40
                        && p.IsAlive
                        && p.InLineOfSpellSight
                        && AOEHealCount(p, hp, HEALING_RAIN_RADIUS) > AOEHealCount(tar, hp, HEALING_RAIN_RADIUS))
                    {
                        tar = p;
                    }
                }
            }
            if (AOEHealCount(tar, hp, HEALING_RAIN_RADIUS) >= count)
            {
                return tar;
            }
            else
            {
                return null;
            }*/
        }


        public WoWPlayer BestHWSanctuaryTargetLocation(bool lockframe,int hp, int count)
        {
            WoWPlayer tar = StyxWoW.Me;
            if (lockframe)
            {
                //using (StyxWoW.Memory.AcquireFrame())
                //{
                foreach (WoWPlayer p in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>())
                    {
                        if (p.Distance2DSqr < 40*40
                            && p.IsAlive
                            && p.InLineOfSpellSight
                            && AOEHealCount(p, hp, HW_SANCTUARY_RADIUS) > AOEHealCount(tar, hp, HW_SANCTUARY_RADIUS))
                        {
                            tar = p;
                        }
                    }
                //}
            }
            else
            {
                foreach (WoWPlayer p in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>())
                {
                    if (p.Distance2DSqr < 40*40
                        && p.IsAlive
                        && p.InLineOfSpellSight
                        && AOEHealCount(p, hp, HW_SANCTUARY_RADIUS) > AOEHealCount(tar, hp, HW_SANCTUARY_RADIUS))
                    {
                        tar = p;
                    }
                }
            }
            if (AOEHealCount(tar, hp, HW_SANCTUARY_RADIUS) >= count)
            {
                return tar;
            }
            else
            {
                return null;
            }
        }

        public WoWPlayer BestPOM_PROC_DIVINE_INSIGHT_Target(int hp, int count)
        {
            WoWPlayer tar = StyxWoW.Me;
            //using (StyxWoW.Memory.AcquireFrame())
            //{
                foreach (WoWPlayer p in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>())
                {
                    if (p.Distance2DSqr < 40*40
                        && p.IsAlive
                        && p.InLineOfSpellSight
                        && AOEHealCount(p, hp, POM_RADIUS) > AOEHealCount(tar, hp, POM_RADIUS))
                    {
                        tar = p;
                    }
                }
            //}
            if (AOEHealCount(tar, hp, POM_RADIUS) >= count)
            {
                return tar;
            }
            else
            {
                return null;
            }
        }

        public WoWPlayer BestCircleOfHealing_Target(int hp, int count)
        {
            WoWPlayer tar = StyxWoW.Me;
            //using (StyxWoW.Memory.AcquireFrame())
            //{
                foreach (WoWPlayer p in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>())
                {
                    if (p.Distance2DSqr < 40*40
                        && p.IsAlive
                        && p.InLineOfSpellSight
                        && AOEHealCount(p, hp, COH_RADIUS) > AOEHealCount(tar, hp, COH_RADIUS))
                    {
                        tar = p;
                    }
                }
            //}
            if (AOEHealCount(tar, hp, COH_RADIUS) >= count)
            {
                return tar;
            }
            else
            {
                return null;
            }
        }

        public WoWPlayer BestChaiHeal_Target(int hp, int count)
        {
            WoWPlayer tar = StyxWoW.Me;
            WoWUnit playerCandidate = GetHealTarget(40f);
            if (playerCandidate != null && playerCandidate.IsAlive && AOEHealCount(playerCandidate.ToPlayer(), hp, CHAIN_HEAL_RADIUS) >= count)
                return playerCandidate.ToPlayer();
            else
                return null;
            /*WoWPlayer tar = StyxWoW.Me;
            //using (StyxWoW.Memory.AcquireFrame())
            //{
                foreach (WoWPlayer p in ObjectManager.GetObjectsOfTypeFast<WoWPlayer>(true, true))
                {
                    if (p.Distance2DSqr < 40*40
                        && p.IsAlive
                        && p.InLineOfSpellSight
                        && AOEHealCount(p, hp, CHAIN_HEAL_RADIUS) > AOEHealCount(tar, hp, CHAIN_HEAL_RADIUS))
                    {
                        tar = p;
                    }
                }
            //}
            if (AOEHealCount(tar, hp, CHAIN_HEAL_RADIUS) >= count)
            {
                return tar;
            }
            else
            {
                return null;
            }*/
        }

        public void LogParties()
        {
            for (int player_number = 0; player_number < 5; player_number++)
            {
                for (int group_number = 0; group_number < 8; group_number++)
                {
                    Logging.Write(parties[player_number, group_number] + " in party: " + group_number);
                }
            }
        }

        public bool InRaid()
        {
            return Me.RaidMembers.Count > 0;
        }


        public WoWPlayer FirstTankInRange(double range)
        {
                if (StyxWoW.Me.GroupInfo.IsInRaid)
                    return StyxWoW.Me.GroupInfo.RaidMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank))
                        .Select(p => p.ToPlayer())
                        .Where(p => p != null && p.Distance - p.CombatReach -1  <= range).FirstOrDefault();
                else if (StyxWoW.Me.GroupInfo.IsInParty)
                    return StyxWoW.Me.GroupInfo.PartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank))
                        .Select(p => p.ToPlayer())
                        .Where(p => p != null && p.Distance - p.CombatReach -1  <= range).FirstOrDefault();
                return null;
        }

        public WoWPlayer FirstOffTankInRange(double range,WoWUnit MainTank)
        {
            if (MainTank != null)
            {
                if (StyxWoW.Me.GroupInfo.IsInRaid)
                    return StyxWoW.Me.GroupInfo.RaidMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank) && p.Guid != MainTank.Guid)
                        .Select(p => p.ToPlayer())
                        .Where(p => p != null && p.Distance - p.CombatReach - 1 <= range).FirstOrDefault();
                else if (StyxWoW.Me.GroupInfo.IsInParty)
                    return StyxWoW.Me.GroupInfo.PartyMembers.Where(p => p.HasRole(WoWPartyMember.GroupRole.Tank) && p.Guid != MainTank.Guid)
                        .Select(p => p.ToPlayer())
                        .Where(p => p != null && p.Distance - p.CombatReach - 1 <= range).FirstOrDefault();
            }
            return null;
        }

        public WoWUnit OtherPlayerAsTank(double range)
        {
            if (StyxWoW.Me.GroupInfo.IsInRaid)
                return StyxWoW.Me.GroupInfo.RaidMembers.Select(p => p.ToPlayer())
                    .Where(p => p != null && p.InLineOfSpellSight && p.IsAlive && p.Combat && p.Distance - p.CombatReach -1  <= range).OrderBy(p => p.HealthPercent).LastOrDefault();
            else if (StyxWoW.Me.GroupInfo.IsInParty)
                return StyxWoW.Me.GroupInfo.PartyMembers.Select(p => p.ToPlayer())
                    .Where(p => p != null && p.InLineOfSpellSight && p.IsAlive && p.Combat && p.Distance - p.CombatReach -1  <= range).OrderBy(p => p.HealthPercent).LastOrDefault();
            return Me;

        }

        public WoWUnit SimpleGetTank(double range)
        {
            //Tsulong override
            /*if (Me.GotTarget && Me.CurrentTarget.Entry == 62442 && Me.CurrentTarget.Distance - Me.CurrentTarget.CombatReach -1  <= range &&
                (!Me.CurrentTarget.Attackable || Me.CurrentTarget.IsFriendly)
                && Me.CurrentTarget.InLineOfSpellSight)
            {
                return Me.CurrentTarget;
            }*/

            WoWUnit focused_unit = Me.FocusedUnit;
            if (focused_unit != null && focused_unit.IsFriendly && focused_unit.IsAlive && focused_unit.InLineOfSpellSight && focused_unit.Distance - focused_unit.CombatReach -1  <= range)
            {
                return focused_unit;
            }
            else
            {
                WoWPlayer TankCandidate = FirstTankInRange(range);
                if (TankCandidate != null && TankCandidate.IsFriendly && TankCandidate.IsAlive && TankCandidate.InLineOfSpellSight && TankCandidate.Distance - TankCandidate.CombatReach -1  <= range)
                    return TankCandidate;
            }
            //return OtherPlayerAsTank(40);
            return null;
        }

        public WoWUnit SimpleGetOffTank(double range, WoWUnit MainTank)
        {
            WoWPlayer TankCandidate = FirstOffTankInRange(range,MainTank);
                if (TankCandidate != null && TankCandidate.IsFriendly && TankCandidate.IsAlive && TankCandidate.InLineOfSpellSight && TankCandidate.Distance - TankCandidate.CombatReach - 1 <= range)
                    return TankCandidate;
            return null;
        }

        public bool Mounted()
        {
            return Me.Mounted;
        }

     }
}
