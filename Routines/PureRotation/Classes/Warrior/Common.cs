using System;
using System.Linq;

// ReSharper disable InconsistentNaming
using PureRotation.Core.CombatLog;
using PureRotation.Core;
using PureRotation.Managers;
using PureRotation.Helpers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;
using Lua = Styx.WoWInternals.Lua;

namespace PureRotation.Classes.Warrior
{
    internal static class Common
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static DateTime LastInterrupt = DateTime.Now;

        #region Booleans & Doubles

        // Some checks
        internal static bool AlmostDead { get { return Me.CurrentTarget.HealthPercent <= 5; } }
        internal static bool DumpAllRage { get { return Me.CurrentTarget.HealthPercent <= 1; } }
        public static bool ExecuteCheck { get { return Me.CurrentTarget.HealthPercent <= 20; } }
        public static bool NonExecuteCheck { get { return Me.CurrentTarget.HealthPercent >= 20; } }
        internal static bool TargettingMe { get { return Me.CurrentTarget.IsTargetingMeOrPet; } }
        internal static bool HasteAbilities { get { return (BloodLustAura || TimeWarpAura || HeroismAura); } }

        // Cached Aura's - Can only be used with MY aura's (HasCachedAura).
        internal static bool AvatarAura { get { return Me.CachedHasAura(107574); } }
        internal static bool BloodbathAura { get { return Me.CachedHasAura(12292); } }
        internal static bool BloodsurgeAura { get { return Me.CachedHasAura(46916); } }
        internal static bool EnrageAura { get { return Me.CachedHasAura(13046); } }
        internal static bool LastStandAura { get { return Me.CachedHasAura(12975); } }
        internal static bool MeatCleaverAura { get { return Me.CachedHasAura(85739); } }
        internal static bool RagingBlowAura { get { return Me.CachedHasAura(131116); } }
        internal static bool RecklessnessAura { get { return Me.CachedHasAura(1719); } }
        internal static bool ShieldBarrierAura { get { return Me.CachedHasAura(112048); } }
        internal static bool ShieldBlockAura { get { return Me.CachedHasAura(2565); } }
        internal static bool SuddenExecAura { get { return Me.CachedHasAura(139958); } }
        internal static bool TasteforBloodAura { get { return Me.CachedHasAura(56636); } }
        internal static bool UltimatumAura { get { return Me.CachedHasAura(122510); } }
        internal static bool VictoriousAura { get { return Me.CachedHasAura(32216); } }


        public static bool ColossusSmashAura { get { return Me.CurrentTarget.CachedHasAura(86346); } }
        internal static bool TasteForBloodStacks3 { get { return Spell.GetAuraStackCount("Taste for Blood") >= 3; } }
        internal static bool ColossusSmashAuraT { get { return Me.CurrentTarget.CachedHasAura(86346, 0, true, 5000); } }
        internal static bool ColossusSmash25Sec { get { return Me.CurrentTarget.CachedHasAura(86346, 0, true, 2500); } }
        internal static bool ColossusSmash1Sec { get { return Me.CurrentTarget.CachedHasAura(86346, 0, true, 1000); } }
        internal static bool DeepWoundsAura { get { return Me.CurrentTarget.CachedHasAura(115767); } }
        internal static bool RecklessnessAuraT { get { return Me.CachedHasAura(1719, 0, false, 10000); } }

        // Cached Stacked Aura's - Can only be used with MY aura's.
        internal static bool MeatCleaverAuraS1 { get { return Me.CachedHasAura(85739, 1); } }
        internal static bool MeatCleaverAuraS2 { get { return Me.CachedHasAura(85739, 2); } }
        internal static bool MeatCleaverAuraS3 { get { return Me.CachedHasAura(85739, 3); } }
        internal static bool RagingBlowStacks { get { return Me.CachedHasAura(131116, 2); } }
        internal static bool TasteForBloodS3 { get { return Me.CachedHasAura(56636, 3); } }

        // Aura's not created by Me.GUID.
        internal static bool BattleShoutAura { get { return Me.CachedHasAura(6673); } }
        internal static bool BloodLustAura { get { return Me.CachedHasAura(2825); } }
        internal static bool CommandingShoutAura { get { return Me.CachedHasAura(469); } }
        internal static bool HeroismAura { get { return Me.CachedHasAura(32182); } }
        internal static bool TimeWarpAura { get { return Me.CachedHasAura(80353); } }
        internal static bool SkullBannerAura { get { return Me.HasAura(114207); } } // Do not cache this.
        internal static bool RallyingCryAura { get { return Me.HasAura(97462); } }  // Do not cache this.

        internal static bool WeakenedBlowsAura { get { return Me.CurrentTarget.HasAura(115798); } }

        // Talentmanager - HasTalent
        internal static bool JNTalent { get { return TalentManager.HasTalent(1); } }                        // Juggernaut
        internal static bool DTTalent { get { return TalentManager.HasTalent(2); } }                        // Double Time
        internal static bool WBTalent { get { return TalentManager.HasTalent(3); } }                        // Warbringer

        internal static bool ERTalent { get { return TalentManager.HasTalent(4); } }                        // Enraged Regeneration
        internal static bool SCTalent { get { return TalentManager.HasTalent(5); } }                        // Second Wind
        internal static bool IVTalent { get { return TalentManager.HasTalent(6); } }                        // Impending Victory

        internal static bool SSTalent { get { return TalentManager.HasTalent(7); } }                        // Staggering Shout
        internal static bool PHTalent { get { return TalentManager.HasTalent(8); } }                        // Piercing Howl
        internal static bool DSTalent { get { return TalentManager.HasTalent(9); } }                        // Disrupting Shout

        internal static bool BSTalent { get { return TalentManager.HasTalent(10); } }                       // Bladestorm
        internal static bool SWTalent { get { return TalentManager.HasTalent(11); } }                       // Shockwave
        internal static bool DRTalent { get { return TalentManager.HasTalent(12); } }                       // Dragon Roar

        internal static bool MRTalent { get { return TalentManager.HasTalent(13); } }                       // Mass Spell Reflection
        internal static bool SGTalent { get { return TalentManager.HasTalent(14); } }                       // Safeguard
        internal static bool VGTalent { get { return TalentManager.HasTalent(15); } }                       // Vigilance

        internal static bool AVTalent { get { return TalentManager.HasTalent(16); } }                       // Avatar
        internal static bool BBTalent { get { return TalentManager.HasTalent(17); } }                       // Bloodbath
        internal static bool SBTalent { get { return TalentManager.HasTalent(18); } }                       // Storm Bolt

        // Talentmanager - HasGlyphs
        internal static bool URGlyph { get { return TalentManager.HasGlyph("Unending Rage"); } }            // Unending Rage
        internal static bool ISGlyph { get { return TalentManager.HasGlyph("Intimidating Shout"); } }       // Intimidating Shout

        // Cooldown Tracker ( Translate: Impending Victory CoolDown On Cooldown)
        internal static bool BTOC { get { return Spell.SpellOnCooldown(23881); } }                      // Bloodthirst
        internal static bool DBOC { get { return Spell.SpellOnCooldown(114203); } }                     // Demoralizing Banner
        internal static bool HLOC { get { return Spell.SpellOnCooldown(6544); } }                       // Heroic Leap
        internal static bool IVOC { get { return Spell.SpellOnCooldown(103840); } }                     // Impending Victory
        internal static bool PUOC { get { return Spell.SpellOnCooldown(6552); } }                       // Pummel
        internal static bool VROC { get { return Spell.SpellOnCooldown(34428); } }                      // Victory Rush

        // Cooldown Tracker ( Translate: Bloodbath Cooldown)
        internal static double AVCD { get { return Spell.GetSpellCooldown(107574).TotalSeconds; } }       // Avatar
        internal static double BBCD { get { return Spell.GetSpellCooldown(12292).TotalSeconds; } }        // Bloodbath
        internal static double BTCD { get { return Spell.GetSpellCooldown(23881).TotalSeconds; } }        // Bloodthirst
        internal static double CSCD { get { return Spell.GetSpellCooldown(86346).TotalSeconds; } }        // Colossus Smash
        internal static double PUCD { get { return Spell.GetSpellCooldown(6552).TotalSeconds; } }         // Pummel
        internal static double SBCD { get { return Spell.GetSpellCooldown(114207).TotalSeconds; } }       // Skull Banner
        internal static double SRCD { get { return Spell.GetSpellCooldown(23920).TotalSeconds; } }        // Spell Reflection
        internal static double RCCD { get { return Spell.GetSpellCooldown(1719).TotalSeconds; } }         // Recklessness
        #endregion Booleans & Doubles

        #region CombatLog

        internal static readonly HashSet<int> PvPCcBreakables = new HashSet<int>
        {
            //taken from mirabis
            28272, // Pig Poly  (cast)
            118, // Sheep Poly  (cast)
            61305, // Cat Poly  (cast)
            61721, // Rabbit Poly  (cast)
            61780, // Turkey Poly  (cast)
            28271, // Turtle Poly  (cast)
            20066, //Repentance  (cast)
            115078, // Paralysis
            104045, // Sleep (Metamorphosis)
            115268, // Mesmerize (Shivarra)
            82691, // Ring of Frost
            6358, // Seduction (Succubus)
            2094, // Blind
            115750, // Blinding Light
            6770, // Sap
            2637, // Hibernate
            113056, // Intimidating Roar
            3355, // Freezing Trap
            19503, // Scatter Shot
            19386, // Wyvern Sting
            126246, // Lullaby
            90337, // Bad Manner
            24394, // Intimidation
            126355, // Paralyzing Quill
            126423, // Petrifying Gaze
            50519, // Sonic Blast
            56626, // Sting
            96201, // Web Wrap
            82691, // Ring of Frost
            9484, // Shackle Undead
            88625, // Holy Word: Chastise
            115268, // Mesmerize
            6358, // Seduction
            20511 // Intimidating Shout
        };

        internal static readonly HashSet<int> PvPDefStance = new HashSet<int>
        {
            110698,		// Hammer of Justice (Paladin)
            853,        // Hammer of Justice
            105593,		// Fist of Justice
			108194,		// Asphyxiate
			5211,		// Mighty Bash
			44572,		// Deep Freeze
			119381,		// Leg Sweep
			1833,		// Cheap Shot
			408,		// Kidney Shot
			89766,		// Axe Toss (Felguard/Wrathguard)
			132168		// Shockwave
        };

        private static bool _combatLogAttached;

        public static void Initialize()
        {
            CombatLogHandler.Register("SPELL_CAST_SUCCESS", SPELL_CAST_SUCCESS);
        }
        
        internal static void SPELL_CAST_SUCCESS(CombatLogEventArgs args)
        {
            if (PRSettings.Instance.Warrior.AP_CL_Mechanics && args.DestGuid == StyxWoW.Me.Guid)
            {
                //intervene to break CC
                if (PvPCcBreakables.Contains(args.SpellId))
                {
                    if (Unit.BestUnitForInterveneCCBreak != null)
                    {
                        SpellManager.Cast(SpellBook.Intervene, Unit.BestUnitForInterveneCCBreakResult);
                        Logger.InfoLog("Intervene to " + Unit.BestUnitForInterveneCCBreakResult.Name + "to get out of " + args.SpellName);
                    }
                }
                //go defensive stance
                if (PvPDefStance.Contains(args.SpellId))
                {
                    if (Me.Shapeshift != ShapeshiftForm.DefensiveStance)
                    {
                        SpellManager.Cast(SpellBook.DefensiveStance);
                        Logger.InfoLog("Going Defensive Stance while in " + args.SpellName);
                    }
                }

            }
        }

        #endregion


        #region Misc

        internal static bool Default
        {
            get
            {
                return 
                    Me.CurrentTarget != null 
                    && Me.CurrentTarget.Attackable 
                    && !Me.Mounted 
                    && !Me.CurrentTarget.IsDead 
                    && Me.CurrentTarget.IsWithinMeleeRange
                    && !IsImmune(Me.CurrentTarget);
            }
        }

        internal static HashSet<int> HS_ImmuneSpells = new HashSet<int>()
        {
            SpellBook.IceBlock, SpellBook.Cyclone, SpellBook.EntanglingRoots, SpellBook.HandofProtection, SpellBook.Deterrence, SpellBook.DivineShield
        };

        internal static bool IsImmune(WoWUnit unit)
        {
            if (unit != null)
            {

                var auraList = Me.CurrentTarget.GetAllAuras();

                foreach (WoWAura aura in auraList)
                {
                    if (!aura.IsActive)
                        continue;
                    if (HS_ImmuneSpells.Contains(aura.SpellId))
                        return true;
                }

            }
            return false;
        }

        internal static bool Attackable
        {
            get
            {
                return
                    Me.CurrentTarget != null
                    && Me.CurrentTarget != Me
                    && !Me.CurrentTarget.IsDead
                    && !Me.Mounted;
            }
        }

        internal static bool FocusAttackable
        {
            get
            {
                return
                    Me.FocusedUnit != null
                    && Me.FocusedUnit != Me
                    && !Me.FocusedUnit.IsDead
                    && !Me.Mounted;
            }
        }

        internal static bool FadingRb(int fadingtime)
        {
                if (!Me.GotTarget)
                    return false;
                WoWAura ragingBlow =
                    Me.CurrentTarget.CachedGetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 131116);
                return ragingBlow != null && ragingBlow.TimeLeft <= TimeSpan.FromMilliseconds(fadingtime);
        }

        internal static bool FadingCs(int fadingtime)
        {
                if (!Me.GotTarget)
                    return false;
                WoWAura colossusSmash = Me.CurrentTarget.CachedGetAllAuras().FirstOrDefault(a => a.SpellId == 86346 && a.CreatorGuid == StyxWoW.Me.Guid);
                return colossusSmash != null && colossusSmash.TimeLeft <= TimeSpan.FromMilliseconds(fadingtime);
        }


        internal static bool FadingWb(int fadingtime)
        {
                if (!Me.GotTarget)
                    return false;
                WoWAura weakenedBlows =
                    Me.CurrentTarget.GetAllAuras().FirstOrDefault(u => u.CreatorGuid == Me.Guid && u.SpellId == 115798);
                return weakenedBlows != null && weakenedBlows.TimeLeft <= TimeSpan.FromMilliseconds(fadingtime);
        }

        public static bool NeedThunderClap
        {
            get { return Unit.AttackableMeleeUnits.Count(u => !u.HasAura(115767)) > 0; }
        }

        internal static bool Needs2H
        {
            get
            {
                if (Me.Inventory.Equipped.OffHand != null && !Me.HasAura(SpellBook.SpellReflection) && !Me.HasAura(SpellBook.ShieldWall))
                    
                    //!Me.ActiveAuras.ContainsKey("Spell Reflection") && !Me.ActiveAuras.ContainsKey("Shield Wall"))
                    return true;

                return false;
            }
        }

        internal static bool NeedsRallyingCry
        {
            get
            {
                if (!PRSettings.Instance.Warrior.ArmsRallyingCry || Spell.SpellOnCooldown(SpellBook.RallyingCry))
                    return false;

                if (Me.HealthPercent <= 25) { return true; }

                if (Unit.GroupMembers.Any(p => p.IsAlive && p.Combat && p.HealthPercent <= 25 && !p.HasAura("Rallying Cry") && p.Location.DistanceSqr(Me.Location) <= 30 * 30))
                { 
                    return true; 
                }
                return false;
            }
        }

        internal static HashSet<int> HS_Disarm = new HashSet<int>()
        {
		    SpellBook.Avatar,
            SpellBook.Bloodbath,
            SpellBook.Recklessness,
            SpellBook.ShadowDance,
            SpellBook.ShadowBlades,
            SpellBook.KillingSpree,
            SpellBook.Incarnation,
            SpellBook.VampiricBlood
        };

        internal static bool NeedsDisarm
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                if (Me.CurrentTarget.IsPlayer && Me.CurrentTarget.IsWithinMeleeRange)
                {
                    

                    var auraList = Me.CurrentTarget.GetAllAuras();

                    foreach (WoWAura aura in auraList)
                    {
                        if (!aura.IsActive)
                            continue;
                        if (HS_Disarm.Contains(aura.SpellId))
                            return true;
                    }
                }
                return false;
            }
        }

        internal static HashSet<int> HS_CantSlow = new HashSet<int>()
        {
		    SpellBook.HandofFreedom,
            SpellBook.WindwalkTotem,
            SpellBook.MastersCall,
            SpellBook.Dispersion,
            SpellBook.Bladestorm,
            SpellBook.Hamstring,
            SpellBook.PiercingHowl
        };

        internal static bool NeedsSlow
        {
            get
            {
                if (Me.GotTarget)
                {
                    if (!Me.CurrentTarget.IsPlayer)
                        return false;

                    if (PvP.IsSlowed(Me.CurrentTarget))
                        return false;

                    var auraList = Me.CurrentTarget.GetAllAuras();

                    foreach (WoWAura aura in auraList)
                    {
                        if (!aura.IsActive)
                            continue;
                        if (HS_CantSlow.Contains(aura.SpellId))
                            return false;
                    }
                }
                return true;
            }
        }

        internal static HashSet<int> HS_ForceOP = new HashSet<int>()
        {
		    SpellBook.Evasion,
            SpellBook.DiebytheSword
        };

        internal static bool NeedsOP
        {
            get
            {
                if (Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange)
                {
                    if (!Me.CurrentTarget.IsPlayer)
                        return false;

                    

                    var auraList = Me.CurrentTarget.GetAllAuras();

                    foreach (WoWAura aura in auraList)
                    {
                        if (!aura.IsActive)
                            continue;
                        if (HS_ForceOP.Contains(aura.SpellId))
                            return true;
                    }
                }
                return false;
            }
        }

        internal static bool NeedsFreedom
        {
            get
            {
                if (!PRSettings.Instance.Warrior.AP_EnableBannerFreedom)
                    return false;

                if (IsMovementImpaired(Me)
                    && Me.CurrentTarget != null
                    && !Me.CurrentTarget.IsWithinMeleeRange
                    && Me.CurrentTarget.InLineOfSight
                    && Me.CurrentTarget.Distance <= 25)
                {
                    return true;
                }
                return false;
            }
        }

        internal static bool NeedsStampedingShout
        {
            get
            {
                if (PRSettings.Instance.Warrior.ArmsStampedingShout
                    && IsMovementImpaired(Me)
                    && Me.CurrentTarget != null
                    && !Me.CurrentTarget.IsWithinMeleeRange
                    && Me.CurrentTarget.InLineOfSight
                    && Me.CurrentTarget.Distance <= 25
                    && !Me.HasAura("Stampeding Shout") 
                    && Me.HasAura("Symbiosis") 
                    && !Spell.SpellOnCooldown(122294))
                {
                    return true;
                }
                return false;
            }
        }

        internal static bool NeedsPummel
        {
            get
            {

                if (Me.CurrentTarget != null
                    && (LastInterrupt == null || (LastInterrupt + TimeSpan.FromMilliseconds(1500)) < DateTime.Now)
                    && Me.CurrentTarget.IsWithinMeleeRange
                    && ((Me.CurrentTarget.IsCasting && Me.CurrentTarget.CurrentCastTimeLeft.TotalMilliseconds < PRSettings.Instance.Warrior.ArmsCastTimeLeft) || Me.CurrentTarget.IsChanneling)
                    && Me.CurrentTarget.CanInterruptCurrentSpellCast
                    && !IsInterruptImmune(Me.CurrentTarget))
                {
                    LastInterrupt = DateTime.Now;
                    return true;
                }

                return false;
            }
        }

        internal static bool NeedsFocusPummel
        {
            get
            {
                if (!PRSettings.Instance.Warrior.ArmsFocusPummel)
                    return false;

                if (Me.FocusedUnit != null
                    && (LastInterrupt == null || (LastInterrupt + TimeSpan.FromMilliseconds(1500)) < DateTime.Now)
                    && Me.FocusedUnit.IsWithinMeleeRange
                    && ((Me.FocusedUnit.IsCasting && Me.FocusedUnit.CurrentCastTimeLeft.TotalMilliseconds < PRSettings.Instance.Warrior.ArmsCastTimeLeft) || Me.FocusedUnit.IsChanneling)
                    && Me.FocusedUnit.CanInterruptCurrentSpellCast
                    && !IsInterruptImmune(Me.FocusedUnit)
                    && Me.CurrentTarget != null
                    && Me.CurrentTarget.HealthPercent <= 75)
                {
                    LastInterrupt = DateTime.Now;
                    return true;
                }

                return false;
            }
        }

        internal static bool NeedsFocusCharge
        {
            get
            {
                if (!PRSettings.Instance.Warrior.ArmsFocusCharge)
                    return false;

                if (Me.FocusedUnit != null
                    && (LastInterrupt == null || (LastInterrupt + TimeSpan.FromMilliseconds(1500)) < DateTime.Now)
                    && ((Me.FocusedUnit.IsCasting 
                    && Me.FocusedUnit.IsCastingHealingSpell
                    && Me.FocusedUnit.CurrentCastTimeLeft.TotalMilliseconds < PRSettings.Instance.Warrior.ArmsCastTimeLeft) || Me.FocusedUnit.IsChanneling)
                    && Me.FocusedUnit.CanInterruptCurrentSpellCast
                    && !IsInterruptImmune(Me.FocusedUnit)
                    && Me.FocusedUnit.Distance > 8 
                    && (Me.FocusedUnit.Distance <= 25 || Me.FocusedUnit.Distance <= 30 && TalentManager.HasGlyph("Long Charge"))
                    && Me.CurrentTarget != null
                    && Me.CurrentTarget.HealthPercent <= 75)
                {
                    LastInterrupt = DateTime.Now;
                    return true;
                }

                return false;
            }
        }

        internal static bool NeedsStormBolt
        {
            get
            {
                if (!PRSettings.Instance.Warrior.AP_UseStormBoltInterrupt || !TalentManager.HasTalent(18))
                    return false;

                if (Me.CurrentTarget != null
                    && (LastInterrupt == null || (LastInterrupt + TimeSpan.FromMilliseconds(1500)) < DateTime.Now)
                    && ((Me.CurrentTarget.IsCasting
                    && Me.CurrentTarget.IsCastingHealingSpell
                    && Me.CurrentTarget.HealthPercent <= 75
                    && Me.CurrentTarget.CurrentCastTimeLeft.TotalMilliseconds < PRSettings.Instance.Warrior.ArmsCastTimeLeft) || Me.CurrentTarget.IsChanneling)
                    && Me.CurrentTarget.Distance <= 30)
                {
                    LastInterrupt = DateTime.Now;
                    return true;
                }

                return false;
            }
        }

        internal static bool NeedsFocusStormBolt
        {
            get
            {
                if (!PRSettings.Instance.Warrior.AP_UseStormBoltInterrupt || !TalentManager.HasTalent(18))
                    return false;

                if (Me.FocusedUnit != null
                    && (LastInterrupt == null || (LastInterrupt + TimeSpan.FromMilliseconds(1500)) < DateTime.Now)
                    && ((Me.FocusedUnit.IsCasting
                    && Me.FocusedUnit.CurrentCastTimeLeft.TotalMilliseconds < PRSettings.Instance.Warrior.ArmsCastTimeLeft) || Me.FocusedUnit.IsChanneling)
                    && Me.FocusedUnit.Distance <= 30
                    && Me.CurrentTarget != null
                    && Me.CurrentTarget.HealthPercent <= 75)
                {
                    LastInterrupt = DateTime.Now;
                    return true;
                }

                return false;
            }
        }

        internal static bool NeedsShockwave
        {
            get
            {
                if (!TalentManager.HasTalent(11))
                    return false;

                if (Unit.AttackableMeleeUnits.Count(u => Me.IsFacing(u)) >= 3)
                    return true;

                if (Me.CurrentTarget != null
                    && Me.CurrentTarget.Distance <= 10
                    && (LastInterrupt == null || (LastInterrupt + TimeSpan.FromMilliseconds(1500)) < DateTime.Now)
                    && ((Me.CurrentTarget.IsCasting && Me.CurrentTarget.CurrentCastTimeLeft.TotalMilliseconds < PRSettings.Instance.Warrior.ArmsCastTimeLeft) || Me.CurrentTarget.IsChanneling)
                    && Me.IsFacing(Me.CurrentTarget))
                {
                    LastInterrupt = DateTime.Now;
                    return true;
                }

                return false;
            }
        }

        internal static bool NeedsShout
        {
            get
            {
                if (Default && ExecuteCheck && Me.Combat && Me.CurrentRage < 30)
                    return true;

                return false;
            }
        }

        internal static bool NeedsDisruptingShout
        {
            get
            {
                if (!TalentManager.HasTalent(9))
                    return false;

                if (Me.CurrentTarget != null
                    && Me.CurrentTarget.Distance <= 10
                    && ((LastInterrupt + TimeSpan.FromMilliseconds(1500)) < DateTime.Now)
                    && ((Me.CurrentTarget.IsCasting && Me.CurrentTarget.CurrentCastTimeLeft.TotalMilliseconds < PRSettings.Instance.Warrior.ArmsCastTimeLeft) || Me.CurrentTarget.IsChanneling)
                    && Me.CurrentTarget.CanInterruptCurrentSpellCast
                    && !IsInterruptImmune(Me.CurrentTarget))
                {
                    LastInterrupt = DateTime.Now;
                    return true;
                }

                if (Unit.NearbyAttackableUnits(Me.Location, 10).Any(p =>
                    p.CanInterruptCurrentSpellCast
                    && !IsInterruptImmune(p)
                    && (p.IsCastingHealingSpell || (HS_CrowdControl.Contains(p.CastingSpellId) && Spell.SpellOnCooldown(SpellBook.SpellReflection)))
                    && (p.CurrentCastTimeLeft.TotalMilliseconds < PRSettings.Instance.Warrior.ArmsCastTimeLeft || Me.CurrentTarget.IsChanneling)))
                {
                    LastInterrupt = DateTime.Now;
                    return true;
                }

                return false;
            }
        }

        internal static bool NeedsDefensiveStance
        {
            get
            {
                if (Me.HasAura("Defensive Stance"))
                    return false;

                if (Me.HealthPercent < 30)
                    return true;

                if (Unit.AttackableUnits.Count(p => p.IsTargetingMeOrPet) >= 2)
                    return true;

                return false;
            }
        }

        internal static bool NeedsBattleStance
        {
            get
            {
                if (Me.HasAura("Battle Stance"))
                    return false;

                if (Me.HasAura("Shield Wall"))
                    return false;

                if (Me.HealthPercent > 30 && Unit.AttackableUnits.Count(p => p.IsTargetingMeOrPet) < 2)
                    return true;

                return false;
            }
        }

        internal static HashSet<int> HS_Invulnerable = new HashSet<int>()
        {
		    SpellBook.IceBlock,
            SpellBook.HandofProtection,
            SpellBook.DivineShield
        };

        internal static bool NeedsShatteringThrow
        {
            get
            {
                if (!Me.GotTarget)
                    return false;

                if (Me.CurrentTarget.IsPlayer && Me.CurrentTarget.Distance <= 30 && Me.IsFacing(Me.CurrentTarget))
                {

                    var auraList = Me.CurrentTarget.GetAllAuras();

                    foreach (WoWAura aura in auraList)
                    {
                        if (!aura.IsActive)
                            continue;
                        if (HS_Invulnerable.Contains(aura.SpellId))
                            return true;
                    }
                }
                return false;
            }
        }

        internal static bool NeedsSpellReflect
        {
            get
            {
                if (PRSettings.Instance.Warrior.AP_EnableSpellReflect || Spell.SpellOnCooldown(SpellBook.SpellReflection))
                {
                    return false;
                }
                    

                    var casters = ObjectManager.GetObjectsOfTypeFast<WoWPlayer>().Where(u =>
                       !u.IsMe
                       && u.IsCasting
                       && u.IsTargetingMeOrPet
                       && u.Distance <= 40
                       && u.InLineOfSight
                       && !u.IsInMyPartyOrRaid
                       && u.CurrentCastTimeLeft.TotalMilliseconds < PRSettings.Instance.Warrior.ArmsCastTimeLeft
                       && HS_CrowdControl.Contains(u.CastingSpellId));
                   

                    if (casters.Any())
                    {
                        return true;
                    }

                return false;
            }
        }

        internal static HashSet<int> HS_CrowdControl = new HashSet<int>()
        {
		    SpellBook.Polymorph,
            SpellBook.Repentance,
            SpellBook.Cyclone,
            SpellBook.Fear,
            SpellBook.Hex
        };

        internal static IEnumerable<WoWPlayer> SearchAreaPlayers()
        {
            return ObjectManager.GetObjectsOfTypeFast<WoWPlayer>();
        }

        internal static bool NeedsMassSpellReflect
        {
            get
            {
                if (!PRSettings.Instance.Warrior.AP_EnableMassSpellReflect || !TalentManager.HasTalent(13) || Spell.SpellOnCooldown(SpellBook.MassSpellReflection))
                {
                    return false;
                }

                var retEnemyCasters = new List<WoWPlayer>();
                var retFriends = new List<WoWPlayer>();

                foreach (var p in SearchAreaPlayers())
                {
                    // Dead. Ignore 'em
                    if (p.IsDead || p.IsGhost || p.Distance > 60)
                        continue;

                    if (p.IsInMyPartyOrRaid)
                    {
                        if (!p.IsPet && p.Distance <= 20)
                        {
                            retFriends.Add(p); // add our friendlies
                        }
                        continue;
                    }

                    // triple check seeing we are past the friends check.
                    if (p.IsMe)
                        continue;

                    // not casting gtfo
                    if (!p.IsCasting)
                        continue;

                    // casting but not our spell..gtfo
                    if (!HS_CrowdControl.Contains(p.CastingSpellId))
                        continue;

                    // casting our spell and time is greater than ArmsCastTimeLeft
                    if (p.CurrentCastTimeLeft.TotalMilliseconds >= PRSettings.Instance.Warrior.ArmsCastTimeLeft)
                        continue;

                    if (p.IsTargetingMeOrPet || p.IsTargetingMyPartyMember || p.IsTargetingMyRaidMember)
                    {
                        retEnemyCasters.Add(p); // add the enemy
                    }
                }

                var result = retEnemyCasters.Any(u => retFriends.Contains(u.CurrentTarget));

                return result;
            }
        }

        internal static HashSet<int> HS_InterruptImmune = new HashSet<int>()
        {
		    89489, //Glyph of Inner Focus
		    SpellBook.UnendingResolve,
		    SpellBook.DevotionAura
        };

        internal static bool IsInterruptImmune(WoWUnit unit)
        {
            if (unit != null)
            {
                var auraList = unit.GetAllAuras();

                foreach (WoWAura aura in auraList)
                {
                    if (!aura.IsActive)
                        continue;
                    if (HS_InterruptImmune.Contains(aura.SpellId))
                        return true;
                }
            }
            return false;
        }

        internal static bool IsMovementImpaired(WoWUnit unit)
        {
            if (unit != null)
            {

                var auraList = unit.GetAllAuras();

                foreach (WoWAura aura in auraList)
                {
                    if (!aura.IsActive)
                        continue;
                    if (aura.Spell.Mechanic == WoWSpellMechanic.Rooted)
                        return true;
                }
            }
            return false;
        }

        internal static bool ImFleeing
        {
            get
            {
                return StyxWoW.Me.ActiveAuras.Any(a => a.Value.Spell.Mechanic == WoWSpellMechanic.Fleeing);
            }
        }
        #endregion Misc
    }
}