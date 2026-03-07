#region Revision info

/*
 * $Author: Alex & Wulf$
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1$
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Hunter/BeastMastery.cs $
 * $LastChangedBy:  Wulf$
 * $ChangesMade: $
 */

#endregion Revision info

using System.Linq;
using CommonBehaviors.Actions;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Lua = PureRotation.Helpers.Lua;

namespace PureRotation.Classes.Hunter
{
    [UsedImplicitly]
    internal class BeastMastery : RotationBase
    {
        private static HunterSettings HunterSettings { get { return PRSettings.Instance.Hunter; } }
        private const int FocusPool = 60;
        private static int _numNearByUnits;
        private static int _currentFocus;
        private static double _timeToLive;

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

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                new Decorator(ret => PRSettings.Instance.UsePotion && Common.HasSpeedBuff && !Me.CachedHasAura(Defines.DPSPotionAura), Item.UseItem(76089)),
                Spell.Cast(Defines.Rabid, ret => HunterSettings.UseRabid),
                Spell.Cast(SpellBook.LynxRush, ret => HunterSettings.UseLynxRush && TalentManager.HasTalent(15) && !Me.CurrentTarget.HasAura("Lynx Rush")),
                Spell.Cast(SpellBook.Stampede, ret => HunterSettings.UseStampede), // && (Common.HasSpeedBuff || Me.HasAura("Rapid Fire")) 5.4 Stampede no longer benefits from haste spells
                Spell.Cast(SpellBook.AMurderOfCrows, ret => _currentFocus >= FocusPool && TalentManager.HasTalent(13) && !Me.CurrentTarget.HasAura("A Murder of Crows", 0, true, 2000)),
                Spell.Cast(SpellBook.RapidFire, ret => HunterSettings.UseRapidFire /*& !Me.HasAura("Bestial Wrath")*/ && !Common.HasSpeedBuff)
            );
        }

       
private static Composite HandleTrueSingleTarget()
        {
            return new PrioritySelector
            (
                /*new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                //Spell.Cast(Defines.HuntersMark, ret => Common.NeedHuntersMark),
                Spell.Cast(SpellBook.BestialWrath, ret => HunterSettings.UseBestialWrath && EnoughFocusBW && !Me.HasAura(Defines.BeastWithin)),
                Spell.Cast(SpellBook.KillCommand, ret => Pet != null && Pet.CurrentTarget != null && Pet.Location.Distance(Pet.CurrentTarget.Location) < 25f),
                Spell.Cast(SpellBook.KillShot, ret => Common.UseKillShot),
                Spell.PreventDoubleCast(SpellBook.SerpentSting, 1, ret => !Common.HasSerpentSting),

                // If a big shot is close to being available, delay following less important shots
                // or pool focus for upcoming Bestial Wrath
                new Decorator(ret => Spell.GetSpellCooldown("Kill Command").TotalMilliseconds < HunterSettings.WaitTime && EnoughFocusKC, new ActionAlwaysSucceed()),
                Spell.Cast(SpellBook.CobraShot, ret => Spell.GetSpellCooldown("Bestial Wrath").Seconds < 5 && !EnoughFocusBW && !Me.HasAura(Defines.BeastWithin)),
				
                Spell.Cast(SpellBook.GlaiveToss, ret => Spell.SpellOnCooldown("Kill Command") && Common.UseGT),
                Spell.Cast(SpellBook.Powershot, ret => TalentManager.HasTalent(17)),
                Spell.Cast(SpellBook.Barrage, ret => TalentManager.HasTalent(18)),
                Spell.Cast(SpellBook.DireBeast, ret => Common.UseDB), //&& _timeToLive > 15
				
                Spell.Cast(SpellBook.FocusFire, ret => !Me.HasAura(Defines.BeastWithin) && FocusFireStackCount == 5 && Spell.GetSpellCooldown("Bestial Wrath").Seconds > HunterSettings.SecsToHoldFrenzyStacks && !Common.HasSpeedBuff), // 34471 or 34692 for Beast Within...?
                
				Spell.PreventDoubleCast(SpellBook.ArcaneShot, 1, ret => Common.ThrillBuff),
                Spell.PreventDoubleCast("Cobra Shot", 1, target => Me.CurrentTarget, ret => _currentFocus <= FocusPool && !Common.HasSerpentStingCobra, true),
                Spell.PreventDoubleCast(SpellBook.ArcaneShot, 1, ret => Lua.PlayerPower >= 61 || Me.HasAura(Defines.BeastWithin)),// 34692 == Beast Within
                Spell.PreventDoubleCast(SpellBook.CobraShot, 1, ret => true) // Our Filler.*/

                //no target
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),

                //execute
                Spell.Cast(SpellBook.KillShot, ret => Common.UseKillShot),

                //Bestial Wrath
                Spell.Cast(SpellBook.BestialWrath, ret => HunterSettings.UseBestialWrath && EnoughFocusBW && !Me.HasAura(Defines.BeastWithin)),

                //Rotation?
                Spell.Cast(SpellBook.KillCommand, ret => Pet != null && Pet.CurrentTarget != null),

                // If a big shot is close to being available, delay following less important shots
                // or pool focus for upcoming Bestial Wrath
                new Decorator(ret => Spell.GetSpellCooldown("Kill Command").TotalMilliseconds < HunterSettings.WaitTime && EnoughFocusKC, new ActionAlwaysSucceed()),

                //AS!
                Spell.PreventDoubleCast(SpellBook.ArcaneShot, 1, ret => Me.HasAura(34720) /*Me.HasAura("Thrill of the Hunt")*/ || Lua.PlayerPower >= 54),

                //Nothing Else? Ok.
                Spell.Cast(SpellBook.FocusFire, ret => !Me.HasAura(Defines.BeastWithin) && FocusFireStackCount == 5 && Spell.GetSpellCooldown("Bestial Wrath").Seconds > HunterSettings.SecsToHoldFrenzyStacks && !Common.HasSpeedBuff), // 34471 or 34692 for Beast Within...?
                Spell.Cast(SpellBook.GlaiveToss, ret => Spell.SpellOnCooldown("Kill Command") && Common.UseGT),
                Spell.Cast(SpellBook.Powershot, ret => TalentManager.HasTalent(17)),
                Spell.Cast(SpellBook.Barrage, ret => TalentManager.HasTalent(18)),
                Spell.Cast(SpellBook.DireBeast, ret => Common.UseDB), //&& _timeToLive > 15

                Spell.PreventDoubleCast(SpellBook.SerpentSting, 1, ret => !Common.HasSerpentSting),
                //Spell.PreventDoubleCast("Cobra Shot", 1, target => Me.CurrentTarget, ret => Lua.PlayerPower < 55, true),
                //Spell.PreventDoubleCast(SpellBook.ArcaneShot, 1, ret => Me.HasAura(Defines.BeastWithin)),// 34692 == Beast Within
                Spell.PreventDoubleCast("Cobra Shot", 1, target => Me.CurrentTarget, ret => (_currentFocus < 80 && !Me.HasAura(34720)) || _currentFocus < 30, true) // Our Filler - if we run low on focus, else its a waste..
            );
        }

        private static Composite HandleQuasiAoE()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                //Spell.Cast(Defines.HuntersMark, ret => Common.NeedHuntersMark),
                Spell.Cast(SpellBook.BestialWrath, ret => HunterSettings.UseBestialWrath && EnoughFocusBW && !Me.HasAura(Defines.BeastWithin)),
                Spell.Cast(SpellBook.KillCommand, ret => Pet != null && Pet.CurrentTarget != null && Pet.Location.Distance(Pet.CurrentTarget.Location) < 25f),
                Spell.Cast(SpellBook.KillShot, ret => Common.UseKillShot),
                Spell.PreventDoubleCast(SpellBook.SerpentSting, 1, ret => !Common.HasSerpentSting),

                // If a big shot is close to being available, delay following less important shots
                // or pool focus for upcoming Bestial Wrath
                new Decorator(ret => Spell.GetSpellCooldown("Kill Command").TotalMilliseconds < HunterSettings.WaitTime && EnoughFocusKC, new ActionAlwaysSucceed()),
                Spell.PreventDoubleCast("Cobra Shot", 1, target => Me.CurrentTarget, ret => Spell.GetSpellCooldown("Bestial Wrath").Seconds < 5 && !EnoughFocusBW && !Me.HasAura(Defines.BeastWithin), true),
				
                Spell.CastHunterTrap("Explosive Trap", loc => Me.CurrentTarget.Location, ret => Me.CurrentTarget != null),
                Spell.PreventDoubleCast("Multi-Shot", 4, target => Me.CurrentTarget, ret => Common.AoEMulti, true),
				
                Spell.Cast(SpellBook.GlaiveToss, ret => Spell.SpellOnCooldown("Kill Command") && Common.UseGT),
                Spell.Cast(SpellBook.Powershot, ret => TalentManager.HasTalent(17)),
                Spell.Cast(SpellBook.Barrage, ret => TalentManager.HasTalent(18)),
                Spell.Cast(SpellBook.DireBeast, ret => Common.UseDB), //&& _timeToLive > 15
				
                Spell.Cast(SpellBook.FocusFire, ret => !Me.HasAura(Defines.BeastWithin) && FocusFireStackCount == 5 && Spell.GetSpellCooldown("Bestial Wrath").Seconds > HunterSettings.SecsToHoldFrenzyStacks && !Common.HasSpeedBuff), // 34471 or 34692 for Beast Within...?
                
				Spell.PreventDoubleCast(SpellBook.ArcaneShot, 1, ret => Common.ThrillBuff),
                Spell.PreventDoubleCast("Cobra Shot", 1, target => Me.CurrentTarget, ret => _currentFocus <= FocusPool && !Common.HasSerpentStingCobra, true),
                Spell.PreventDoubleCast(SpellBook.ArcaneShot, 1, ret => _currentFocus >= FocusPool || Me.HasAura(Defines.BeastWithin)),// 34692 == Beast Within
                Spell.PreventDoubleCast("Cobra Shot", 1, target => Me.CurrentTarget, ret => true, true) // Our Filler.
            );
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                Spell.CastHunterTrap("Explosive Trap", loc => Me.CurrentTarget.Location, ret => Me.CurrentTarget != null),
                Spell.Cast(SpellBook.MultiShot, ret => Common.AoEMulti),
                Spell.Cast(SpellBook.KillShot, ret => Common.UseKillShot),
                Spell.Cast(SpellBook.GlaiveToss, ret => Common.UseGT),
                Spell.PreventDoubleCast("Cobra Shot", 1, target => Me.CurrentTarget, ret => NeedFocusAoe, true) // Our Filler.
            );
        }
        /*    
                private Composite HandleAllInOneRotation()
                {
                    bool UseCooldowns = false;
                    bool UseAoE = false;
			
                    if (HotkeySettings.Instance.ModeChoice == Mode.Auto)
                    {	
                        if (Me.CurrentTarget.IsBoss())
                            UseCooldowns = true;
					
                        if (PRSettings.Instance.UseAoEAbilities && NumNearByUnits >= HunterSettings.AoECount)
                            UseAoE = true;
                    }
                    else
                    {
                        if (HotKeyManager.IsCooldown)
                            UseCooldowns = true;
					
                        if (HotKeyManager.IsAoe)
                            UseAoE = true;
                    }
			
                    return new PrioritySelector
                    (
                        new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                        //actions=virmens_bite_potion,if=buff.bloodlust.react|target.time_to_die<=60
                        Item.UseBagItem(Defines.DPSPotion, ret => PRSettings.Instance.UsePotion && UseCooldowns && (Common.WaitForRapidFire || !Common.WaitForSpeedBuff) && !Me.HasCachedAura(Defines.DPSPotionAura, 0), "Virmen's Bite"),
                        //actions+=/auto_shot
                        //actions+=/explosive_trap,if=active_enemies>1
                        Spell.CastHunterTrap("Explosive Trap", loc => Me.CurrentTarget.Location, ret => UseQuasiAoE || UseAoE),
                        //actions+=/focus_fire,five_stacks=1
                        //actions+=/serpent_sting,if=!ticking
                        Spell.PreventDoubleCast(Defines.SerpentSting, 1, ret => !Common.HasSerpentSting),
                        //actions+=/fervor,if=enabled&!ticking&focus<=65
                        //actions+=/bestial_wrath,if=focus>60&!buff.beast_within.up
                        //actions+=/multi_shot,if=active_enemies>5
                        Spell.Cast(Defines.MultiShot, ret => UseAoE && Common.AoEMulti),
                        //actions+=/cobra_shot,if=active_enemies>5
                        Spell.PreventDoubleCast(Defines.CobraShot, 1, ret => UseAoE && NeedFocusAoe),
                        //actions+=/rapid_fire,if=!buff.rapid_fire.up
                        Spell.PreventDoubleCast(Defines.RapidFire, 1, ret => HunterSettings.UseRapidFire && UseCooldowns && !Me.HasCachedAura(Defines.RapidFire, 0, 1000) && Common.WaitForSpeedBuff),
                        //actions+=/stampede,if=buff.rapid_fire.up|buff.bloodlust.react|target.time_to_die<=25
                        Spell.Cast(Defines.Stampede, ret => HunterSettings.UseStampede && UseCooldowns && (Common.WaitForRapidFire || !Common.WaitForSpeedBuff) && TimeToLive > 25),
                        //actions+=/kill_shot
                        Spell.Cast(Defines.KillShot, ret => Common.UseKillShot),
                        //actions+=/kill_command
                        //actions+=/a_murder_of_crows,if=enabled&!ticking
                        Spell.Cast(Defines.AMoC, ret => TalentManager.HasTalent(13) && UseCooldowns && !Me.CurrentTarget.HasCachedAura("A Murder of Crows", 0, 2000)),
                        //actions+=/glaive_toss,if=enabled
                        Spell.Cast(Defines.GlaiveToss, ret => TalentManager.HasTalent(16) && Common.UseGT),
                        //actions+=/lynx_rush,if=enabled&!dot.lynx_rush.ticking
                        //actions+=/dire_beast,if=enabled&focus<=90
                        Spell.Cast(Defines.DireBeast, ret =>  TalentManager.HasTalent(11) && Common.UseDB && Common.DireBeastFocus && TimeToLive > 15),
                        //actions+=/barrage,if=enabled
                        //actions+=/powershot,if=enabled
                        //actions+=/blink_strike,if=enabled
                        //actions+=/readiness,wait_for_rapid_fire=1
                        Spell.PreventDoubleCast(Defines.Readiness, 1, ret => HunterSettings.UseReadiness && UseCooldowns && Spell.GetSpellCooldown(Defines.RapidFire).Seconds > 20 && Spell.GetSpellCooldown(Defines.AMoC).Seconds > 20 && Spell.GetSpellCooldown(Defines.DireBeast).Seconds > 10 && Spell.SpellOnCooldown("Explosive Shot"))
                        //actions+=/arcane_shot,if=buff.thrill_of_the_hunt.react
                        Spell.Cast(Defines.ArcaneShot, ret => Common.ThrillBuff),
                        //actions+=/focus_fire,five_stacks=1,if=!ticking&!buff.beast_within.up
                        //actions+=/cobra_shot,if=dot.serpent_sting.remains<6
                        Spell.PreventDoubleCast(Defines.CobraShot, 1, ret => Me.CurrentTarget.HasCachedAura("Serpent Sting", 0, 6000);)
                        //actions+=/arcane_shot,if=focus>=61|buff.beast_within.up
                        Spell.Cast(Defines.ArcaneShot, ret => CurrentFocus > 60),
                        //actions+=/cobra_shot				
                        Spell.PreventDoubleCast(Defines.CobraShot, 1);
                    );
                }
        */
        #region booleans

        private static void CacheLocalVars()
        {
            _numNearByUnits = Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 20).Count(x => !x.IsBoss());
            //_numNearByUnits = Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 20).Count(x => !x.IsBoss());
            //_currentFocus = Common.PredictFocus();
            _currentFocus = (int)Me.CurrentFocus;// Common.GetPlayerFocus();
            _timeToLive = Me.CurrentTarget == null ? 0 : DpsMeter.GetCombatTimeLeft(Me.CurrentTarget).TotalSeconds;
        }

        private static uint FocusFireStackCount
        {
            get
            {
                return Spell.GetAuraStackCount("Frenzy");
            }
        }

        private static bool NeedFocusAoe { get { return _currentFocus < 35; } }
        private static bool EnoughFocusBW { get { return _currentFocus > 60; } }

		// Return true if we have focus for ES or we have free ES available due to Lock & Load proc
        private static bool EnoughFocusKC { get { return _currentFocus > 40; } }
        
        // Returns true if there are more than 'SurvMultiShot'.  When true, activates multi-mob single rotation (provided there are less than AoE mobs in range)
        private static bool UseQuasiAoE { get { return PRSettings.Instance.UseAoEAbilities && HunterSettings.BMMultiShotCount != 0 && _numNearByUnits >= HunterSettings.BMMultiShotCount; } }
		
        #endregion

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.HunterBeastMastery; }
        }

        public override string Name
        {
            get { return "BeastMastery Hunter"; }
        }

        internal override string Help
        {
            get
            {
                return
@"
 --------------------------------------
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
 + Talent & Glyph choice should not matter
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
                                    new Decorator(ret => !HotKeyManager.IsSpecialKey, HandleTrueSingleTarget()),
                                    new Decorator(ret => HotKeyManager.IsSpecialKey, HandleQuasiAoE())
                                )
                            )
                        )
                    )
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
                return new PrioritySelector(
                        new Decorator(ret => Me.HealthPercent < 100 && PRSettings.Instance.EnableSelfHealing,
                            new PrioritySelector(
                                HandleDefensiveCooldowns())));
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

        #endregion Overrides of RotationBase
    }
}