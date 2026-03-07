#region Revision info

/*
 * $Author: TreeK$
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1$
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Hunter/Survival.cs $
 * $LastChangedBy:  TreeK$
 * $ChangesMade: $
 */

#endregion Revision info

#region ChangeLog

/******************
2013-05-03 - Updated for V2
2013-03-22 - Continuation of optimizations started by Millz:
				- removal of all possible Lua calls
				- replace string spell calls/aur checks with IDs
				- removal of redundant checks
		   - Fixed/added Thrill of the Hunt usage in quasi-AoE and AoE modes.
2013-03-14 - Code clean up, a few optimizations
			 CC will will pause while Feign Death is active
2013-02-21 - Code clean up/comments
             Removed Amber-Shaper interrupts as it was added to all CCs via EnounterSpecific.HandleActionBarInterrupts()
2013-02-20 - Added Misdirection support (Focus > tank > pet)
2013-02-19 - Fixed Deterrence issue
			 Prevent Rapid Fire from being cast if already under a Heroism type effect
			 Added Tranq Shot support
2013-02-17 - Added auto interupts for Amber-Shaper Un'sok fight when transformed into a mutant construct (stolen from Millz who stole from Alxaw!)
2013-02-16 - Added Deterrence to defensive cooldowns & settings
2013-02-15 - Added Healthstone & Major Healing Potion use to SelfHealLogic & settings
			 Added Virmen's Bite use to HandleOffensiveCooldowns
			 Added 'Quasi-AoE' setting - when MOB count is below full AoE setting, and above this setting, will use single target rotation
				but will replace Black Arrow with Explosive Trap, and Arcane Shot with Multi Shot.

******************/

#endregion ChangeLog

using System.Linq;
using CommonBehaviors.Actions;
using JetBrains.Annotations;
using Styx;
using Styx.TreeSharp;
using PureRotation.Core;
using PureRotation.Managers;
using PureRotation.Helpers;
using PureRotation.Settings.Settings;

namespace PureRotation.Classes.Hunter
{
    [UsedImplicitly]
    internal class Survival : RotationBase
    {
        #region privateMembers
        private static HunterSettings HunterSettings { get { return PRSettings.Instance.Hunter; } }
        private const int FocusPool = 60;
        private static int _numNearByUnits;
        private static int _currentFocus;
        private static double _timeToLive;
        private static bool _useCooldowns;
        private static bool _useAoE;
        #endregion privateMembers

        #region Rotations
        private static Composite HandleCommon()
        {
            return new PrioritySelector
            (
                //CachedUnits.Pulse,
                new Action(delegate { CacheLocalVars(); return RunStatus.Failure; }),
                Common.RotationPauseCheck(),
                Common.CheckAspect(),
                Racials.UseRacials(),
                Common.HandleMiscShots(),
                Common.HandleBagHealingItems(),
                Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                //new Decorator(ret => HunterSettings.UseUrgentInterrupts, Spell.InterruptSpellCasts(ret => EncounterSpecific.UrgentInteruptTarget().FirstOrDefault())),
                //new Decorator(ret => HunterSettings.UseExhilaration, Spell.Cast(SpellBook.Exhileration, ret => TalentManager.HasTalent(7) && Me.Pet != null && Pet.IsAlive && Pet.HealthPercent < HunterSettings.ExhilarationHP)),
                Spell.Cast(SpellBook.MendPet, ret => Me.Pet != null && Pet.IsAlive && Pet.HealthPercent < HunterSettings.MendPetPercent && !Pet.HasAura(SpellBook.MendPet) && Pet.InLineOfSpellSight),
                new Decorator(ret => Me.Pet != null && !Pet.IsAlive && HunterSettings.UseRevivePet, new Action(context => Common.CastPetAction("Heart of the Phoenix"))),
                Item.HandleItems()
            );
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector
            (
                Common.HandleDeterrence()
            );
        }

        private static Composite HandleOffensiveCooldowns5_3()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                //Item.UseBagItem(Defines.DPSPotion, ret => PRSettings.Instance.UsePotion && Common.HasSpeedBuff && !Me.CachedHasAura(Defines.DPSPotionAura), "Virmen's Bite"),
                Spell.Cast(SpellBook.AMurderOfCrows, ret => _currentFocus >= 60 && TalentManager.HasTalent(13) && !Me.CurrentTarget.HasAura("A Murder of Crows", 0, true, 2000)),
                Spell.PreventDoubleCast(SpellBook.RapidFire, 1, ret => HunterSettings.UseRapidFire && !Common.HasSpeedBuff),
                Spell.Cast(SpellBook.Stampede, ret => HunterSettings.UseStampede && Common.HasSpeedBuff),
                Spell.Cast(Defines.Rabid, ret => HunterSettings.UseRabid && Me.Pet != null && Pet.IsAlive),
                Spell.Cast(SpellBook.LynxRush, ret => HunterSettings.UseLynxRush && TalentManager.HasTalent(15) && !Me.CurrentTarget.HasAura("Lynx Rush")),
                Spell.PreventDoubleCast(SpellBook.Readiness, 1, ret => HunterSettings.UseReadiness && Spell.GetSpellCooldown(SpellBook.RapidFire).Seconds > 20 && Common.Tier5TalentOnCooldown && (Spell.GetSpellCooldown(SpellBook.DireBeast).Seconds > 10 || !TalentManager.HasTalent(11)) && Spell.SpellOnCooldown("Explosive Shot"))
            );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
				new Decorator(ret => PRSettings.Instance.UsePotion && Common.HasSpeedBuff && !Me.CachedHasAura(Defines.DPSPotionAura), Item.UseItem(76089)),
                Spell.Cast(SpellBook.AMurderOfCrows, ret => _currentFocus >= 60 && TalentManager.HasTalent(13)),
                Spell.Cast(SpellBook.RapidFire, ret => HunterSettings.UseRapidFire && !Common.HasSpeedBuff),
                Spell.Cast(SpellBook.Stampede, ret => HunterSettings.UseStampede), //&& Common.HasSpeedBuff 5.4 Stampede no longer benefits from haste
                Spell.Cast(Defines.Rabid, ret => HunterSettings.UseRabid && Me.Pet != null && Pet.IsAlive),
                Spell.Cast(SpellBook.LynxRush, ret => HunterSettings.UseLynxRush && TalentManager.HasTalent(15) && !Me.CurrentTarget.HasAura("Lynx Rush"))
            );
        }
		
        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),

                // Rotation common to both versions
                Spell.PreventDoubleCast(SpellBook.SerpentSting, 2, ret => !Common.HasSerpentSting),
                Spell.Cast(SpellBook.ArcaneShot, ret => _currentFocus >= 90 && Me.CachedHasAura(Defines.LockAndLoad)), //Bleed focus so we don't cap
                Spell.Cast(SpellBook.ExplosiveShot, ret => EnoughFocusES),
                Spell.Cast(SpellBook.KillShot, ret => Common.UseKillShot)
            );
        }

        private static Composite HandleTrueSingleTarget()
        {
            return new PrioritySelector
            (
                // Rotation based on icy-veins single target.  

                // If a big shot is close to being available, delay following less important shots
                new Decorator(ret => NeedPause, new ActionAlwaysSucceed()),

                // For LnL procs & DoT
                Spell.Cast(SpellBook.BlackArrow), //, ret => TimeToLive > 8

                Spell.Cast(SpellBook.DireBeast, ret => Common.UseDB), // && TimeToLive > 15
                Spell.Cast(SpellBook.GlaiveToss, ret => Common.UseGT && Spell.SpellOnCooldown(SpellBook.ExplosiveShot)),
                Spell.Cast(SpellBook.Powershot, ret => TalentManager.HasTalent(17)),
                Spell.Cast(SpellBook.Barrage, ret => TalentManager.HasTalent(18)),

                // Arcane Shot filler & focus regen
                Spell.Cast(SpellBook.ArcaneShot, ret => _currentFocus >= FocusPool || Common.ThrillBuff),
                Spell.PreventDoubleCast("Cobra Shot", Spell.GetSpellCastTime(SpellBook.CobraShot), target => Me.CurrentTarget, ret => _currentFocus < FocusPool, true)
            );
        }

        private static Composite HandleQuasiAoE()
        {
            return new PrioritySelector
            (
                // Rotation based on icy-veins 2-3 targets.
                // Replaces black arrow with explosive trap, and Arcane Shot with Multi-shot when
                // number of mobs is >= SurvMultiShotCount (but less than AoECount, when the AoE
                // rotation takes over)

                // If a big shot is close to being available, delay following less important shots
                new Decorator(ret => NeedPause, new ActionAlwaysSucceed()),

				// Keep black arrow up for LnL procs
				Spell.Cast(SpellBook.BlackArrow), //, ret => TimeToLive > 8
                // Explosive trap for AoE damage
                Spell.CastHunterTrap("Explosive Trap", loc => Me.CurrentTarget.Location), // , ret => TimeToLive > 15

                Spell.Cast(SpellBook.GlaiveToss, ret => Common.UseGT && Spell.SpellOnCooldown(SpellBook.ExplosiveShot)),
                Spell.Cast(SpellBook.DireBeast, ret => Common.UseDB), // && TimeToLive > 15
                Spell.Cast(SpellBook.Powershot, ret => TalentManager.HasTalent(17)),
                Spell.Cast(SpellBook.Barrage, ret => TalentManager.HasTalent(18)),

                // Use MultiShot as our focus dump
                Spell.Cast(SpellBook.MultiShot, ret => Common.AoEMulti),
                Spell.PreventDoubleCast("Cobra Shot", Spell.GetSpellCastTime(SpellBook.CobraShot), target => Me.CurrentTarget, ret => _currentFocus < FocusPool, true)
            );
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),

                // Rotation based on icy-veins 4+ MOBS
                Spell.Cast(SpellBook.MultiShot, ret => Common.AoEMulti),
				// Keep black arrow up for LnL procs
				Spell.Cast(SpellBook.BlackArrow), //, ret => TimeToLive > 8
                Spell.CastHunterTrap("Explosive Trap", loc => Me.CurrentTarget.Location),
				
				Spell.Cast(SpellBook.DireBeast, ret => Common.UseDB), // && TimeToLive > 15
				
                Spell.PreventDoubleCast(SpellBook.ExplosiveShot, 1, ret => Me.CachedHasAura(Defines.LockAndLoad)),
                Spell.Cast(SpellBook.KillShot, ret => Common.UseKillShot),
                Spell.PreventDoubleCast("Cobra Shot", 0.5, target => Me.CurrentTarget, ret => NeedFocusAoe, true)
            );
        }
        #endregion Rotations

        #region Testing
        private static Composite HandleAllInOneRotation()
        {
            return new PrioritySelector
            (
                //new Action(a => Logger.InfoLog("IsBoss: {0}  ModeAuto: {1}  UseCooldowns: {2}  UseAoE: {3}", Me.CurrentTarget.IsBoss(), HotkeySettings.Instance.ModeChoice == Mode.Auto, UseCooldowns, UseAoE)),
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                //actions=virmens_bite_potion,if=buff.bloodlust.react|target.time_to_die<=60
                Item.UseBagItem(Defines.DPSPotion, ret => PRSettings.Instance.UsePotion && _useCooldowns && Common.HasSpeedBuff && !Me.CachedHasAura(Defines.DPSPotionAura) && _timeToLive > 60, "Virmen's Bite"),
                //actions+=/auto_shot
                //actions+=/explosive_trap,if=active_enemies>1
                Spell.CastHunterTrap("Explosive Trap", loc => Me.CurrentTarget.Location, ret => UseQuasiAoE || _useAoE),
                //actions+=/fervor,if=enabled&focus<=50
                //actions+=/a_murder_of_crows,if=enabled&!ticking
                Spell.Cast(SpellBook.AMurderOfCrows, ret => TalentManager.HasTalent(13) && _useCooldowns && !Me.CurrentTarget.CachedHasAura("A Murder of Crows", 0, true, 2000)),
                //actions+=/blink_strike,if=enabled
                //actions+=/lynx_rush,if=enabled&!dot.lynx_rush.ticking
                //actions+=/explosive_shot,if=buff.lock_and_load.react
                Spell.Cast(SpellBook.ExplosiveShot, ret => Me.CachedHasAura(Defines.LockAndLoad)),
                //actions+=/glaive_toss,if=enabled
                Spell.Cast(SpellBook.GlaiveToss, ret => Common.UseGT),
                //actions+=/powershot,if=enabled
                //actions+=/barrage,if=enabled
                //actions+=/barrage,if=enabled
                //actions+=/multi_shot,if=active_enemies>3
                Spell.Cast(SpellBook.MultiShot, ret => _useAoE && Common.AoEMulti),
                //actions+=/cobra_shot,if=active_enemies>3
                //Spell.PreventDoubleCast(Defines.CobraShot, 1, ret => UseAoE),
                //actions+=/serpent_sting,if=!ticking&target.time_to_die>=10
                Spell.PreventDoubleCast(SpellBook.SerpentSting, 1, ret => !Common.HasSerpentSting && _timeToLive < 10),
                //actions+=/explosive_shot,if=cooldown_react
                Spell.PreventDoubleCast(SpellBook.ExplosiveShot, 1, ret => EnoughFocusES),
                //actions+=/kill_shot
                Spell.Cast(SpellBook.KillShot, ret => Common.UseKillShot),
                //actions+=/black_arrow,if=!ticking&target.time_to_die>=8
                Spell.Cast(SpellBook.BlackArrow, ret => _timeToLive > 8),
                //actions+=/multi_shot,if=buff.thrill_of_the_hunt.react&dot.serpent_sting.remains<2
                Spell.Cast(SpellBook.MultiShot, ret => Common.ThrillBuff && !Me.CurrentTarget.CachedHasAura("Serpent Sting", 0, true, 2000)),
                //actions+=/arcane_shot,if=buff.thrill_of_the_hunt.react
                Spell.Cast(SpellBook.ArcaneShot, ret => Common.ThrillBuff),
                //actions+=/rapid_fire,if=!buff.rapid_fire.up
                Spell.PreventDoubleCast(SpellBook.RapidFire, 1, ret => HunterSettings.UseRapidFire && _useCooldowns && !Common.HasSpeedBuff),
                //actions+=/dire_beast,if=enabled
                Spell.Cast(SpellBook.DireBeast, ret => TalentManager.HasTalent(11) && Common.UseDB),
                //actions+=/stampede,if=buff.rapid_fire.up|buff.bloodlust.react|target.time_to_die<=25
                Spell.Cast(SpellBook.Stampede, ret => HunterSettings.UseStampede && _useCooldowns && Common.HasSpeedBuff && _timeToLive > 25),
                //actions+=/readiness,wait_for_rapid_fire=1
                Spell.PreventDoubleCast(SpellBook.Readiness, 1, ret => HunterSettings.UseReadiness && _useCooldowns && Spell.GetSpellCooldown(SpellBook.RapidFire).Seconds > 20),
                //actions+=/cobra_shot,if=dot.serpent_sting.remains<6
                //Spell.PreventDoubleCast(Defines.CobraShot, 1, ret => !Me.CurrentTarget.CachedHasAura("Serpent Sting", 0, true, 3000)),
                //actions+=/arcane_shot,if=focus>=67
                Spell.Cast(SpellBook.ArcaneShot, ret => _currentFocus > 66),
                //actions+=/cobra_shot			
                Spell.PreventDoubleCast("Cobra Shot", 2, target => Me.CurrentTarget, ret => _currentFocus < 60, true)
            );
        }
        #endregion Testing

        #region booleans

        private static void CacheLocalVars()
        {
            _useCooldowns = Me.CurrentTarget.IsBoss();
            _numNearByUnits = Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 20).Count(x => !x.IsDummy()); // 
            //_numNearByUnits = Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count(x => !x.IsDummy());
            _useAoE = PRSettings.Instance.UseAoEAbilities && _numNearByUnits >= HunterSettings.AoECount;
            _currentFocus = Common.PredictFocus(); //Common.GetPlayerFocus();
            _timeToLive = Me.CurrentTarget == null ? 0 : DpsMeter.GetCombatTimeLeft(Me.CurrentTarget).TotalSeconds;
        }

        /*
                private static Composite PauseForBigShot()
                {
                    return new Action(delegate
                    {
                        Logger.CombatLog("Pausing for Explosive shot: {0}", Spell.GetSpellCooldown("Explosive Shot").TotalMilliseconds);
                        return RunStatus.Success;
                    });
                }
        */

        // Return true if we have focus for ES or we have free ES available due to Lock & Load proc
        private static bool EnoughFocusES { get { return (_currentFocus > 25 || Me.CachedHasAura(Defines.LockAndLoad)); } }

        // Returns true if there are more than 'SurvMultiShot'.  When true, activates multi-mob single rotation (provided there are less than AoE mobs in range)
        private static bool UseQuasiAoE { get { return PRSettings.Instance.UseAoEAbilities && HunterSettings.SurvMultiShotCount != 0 && _numNearByUnits >= HunterSettings.SurvMultiShotCount; } }

        // Returns true if we need focus to fire a MS
        private static bool NeedFocusAoe { get { return _currentFocus < 40; } }

        // returns true if we are pausing the rotation for Explosive Shot to come off cooldown
        private static bool NeedPause { get { return Spell.GetSpellCooldown("Explosive Shot").TotalMilliseconds < HunterSettings.WaitTime && EnoughFocusES; } }

        #endregion booleans

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.HunterSurvival; }
        }

        public override string Name
        {
            get { return "Survival Hunter"; }
        }

        internal override string Help
        {
            get
            {
                return
@"
 --------------------------------------
 Recommended Spec:
 http://www.wowhead.com/talent#hyVE|zVq

* Mode.Auto:
   Uses cooldowns on boss
   Switches between single rotation and quasi-AoE based on number of mobs and user settings
   Switches between single rotation and AoE based on number of mobs and user settings
               
* Mode.SemiAuto
   Hotkey controls cooldowns
   Special hotkey switches quasi-AoE mode on/off
   Switches between single rotation and AoE based on number of mobs and user settings
               
* Mode.Hotkey
   Hotkey controls cooldowns
   Special hotkey switches quasi-AoE mode on/off
   AoE hotkey enables/disables AoE mode

 Notes:
 + Glyph choice should not matter
 --------------------------------------
 ";
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector
                (
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    HandleCommon(),
                    new Decorator(ret => HotKeyManager.IsRotationKey, RotationSelection),
                    new Decorator
                    (
                        ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                        new PrioritySelector
                        (
                            new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),	// Only use cooldowns on boss
                            new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && _numNearByUnits >= HunterSettings.AoECount, HandleAoeCombat()),
                            new Decorator
                            (
                                ret => Me.CurrentTarget != null && (_numNearByUnits < HunterSettings.AoECount || !PRSettings.Instance.UseAoEAbilities),
                                new PrioritySelector
                                (
                                    HandleSingleTarget(),
                                    new Decorator(ret => !UseQuasiAoE, HandleTrueSingleTarget()),
                                    new Decorator(ret => UseQuasiAoE, HandleQuasiAoE())
                                )
                            )
                        )
                    ),
                    new Decorator
                    (
                        ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                        new PrioritySelector
                        (
                            new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                            new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && _numNearByUnits >= HunterSettings.AoECount, HandleAoeCombat()),
                            new Decorator
                            (
                                ret => Me.CurrentTarget != null && (_numNearByUnits < HunterSettings.AoECount || !PRSettings.Instance.UseAoEAbilities),
                                new PrioritySelector
                                (
                                    HandleSingleTarget(),
                                    new Decorator(ret => !HotKeyManager.IsSpecialKey, HandleTrueSingleTarget()),
                                    new Decorator(ret => HotKeyManager.IsSpecialKey, HandleQuasiAoE())
                                )
                            )
                        )
                    ),
                    new Decorator
                    (
                        ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                        new PrioritySelector
                        (
                            new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                            new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                            new Decorator
                            (
                                ret => !HotKeyManager.IsAoe,
                                new PrioritySelector
                                (
                                    HandleSingleTarget(),
                                    new Decorator(ret => !HotKeyManager.IsSpecialKey, HandleTrueSingleTarget()),
                                    new Decorator(ret => HotKeyManager.IsSpecialKey, HandleQuasiAoE())
                                )
                            )
                        )
                    )
                );
            }
        }

        private Composite RotationSelection
        {
            get
            {
                // You can add as many different composite rotations in here, it will toggle from PVE (default) to the composite you select within the GUI provided the hotkey is enabled. -- wulf
                return new PrioritySelector
                (
                    new Decorator(ret => PRSettings.Instance.Hunter.RotationSelections == HunterSettings.RotationSelection.PvETest, HandleAllInOneRotation()),
                    new ActionAlwaysSucceed()
                );
            }
        }

        public override Composite PVPRotation
        {
            get { return PVERotation; }
        }

        public override Composite Medic
        {
            get
            {
                return new PrioritySelector
                (
                    new Decorator
                    (
                        ret => Me.HealthPercent < 100 && PRSettings.Instance.EnableSelfHealing,
                        new PrioritySelector
                        (
                            HandleDefensiveCooldowns()
                        )
                    )
                );
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        ));
            }
        }

        internal override void OnPulse()
        {
            if (StyxWoW.Me.CurrentTarget != null)
                DpsMeter.Update();
        }

        #endregion Overrides of RotationBase
    }
}