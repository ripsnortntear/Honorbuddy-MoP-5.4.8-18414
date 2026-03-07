#region Revision info

/*
 * $Author: Alex, nomnomnom, warena & Wulf$
 * $Date: 2013-09-21 16:32:20 +0200 (Sa, 21 Sep 2013) $
 * $ID$
 * $Revision: 1$
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Warrior/Arms.cs $
 * $LastChangedBy:  nomnomnom$
 * $ChangesMade: Copy n Pasted Apocs routine, atm no support for anything, only copied to have a working arms rotation $
 */

#endregion Revision info

using CommonBehaviors.Actions;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.TreeSharp;
using System.Linq;
using System.Windows.Forms;

namespace PureRotation.Classes.Warrior
{
    [UsedImplicitly]
    internal class Arms : RotationBase
    {
        //private static WarriorSettings WarriorSettings { get { return PRSettings.Instance.Warrior; } }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.ImpendingVictory, ret => TalentManager.HasTalent(6) && Common.Default && PRSettings.Instance.Warrior.ArmsImpVic && Common.NonExecuteCheck && Me.HealthPercent <= PRSettings.Instance.Warrior.ArmsImpVicPercent),
                Spell.Cast(SpellBook.VictoryRush, ret => !TalentManager.HasTalent(6) && Common.Default && PRSettings.Instance.Warrior.ArmsImpVic && Common.NonExecuteCheck && Me.HealthPercent <= PRSettings.Instance.Warrior.ArmsImpVicPercent),
                Item.UseBagItem(5512, ret => Me.HealthPercent <= PRSettings.Instance.Warrior.HealthstonePercent, "Healthstone"),
                Spell.Cast(SpellBook.RallyingCry, ret => Common.NeedsRallyingCry),
                Spell.Cast(SpellBook.DiebytheSword, ret => PRSettings.Instance.Warrior.ArmsDiebytheSword && Me.HealthPercent <= PRSettings.Instance.Warrior.ArmsDiebytheSwordPercent && !Me.ActiveAuras.ContainsKey("Shield Wall"))
                );
        }

        private static Composite HandleDefensiveCooldownsPvp()
        {
            return new PrioritySelector(

                Spell.Cast(SpellBook.Disarm, ret => PRSettings.Instance.Warrior.ArmsDisarm && Common.Attackable && Common.NeedsDisarm),
                new Decorator(ret => Common.NeedsFreedom && Unit.BestUnitForSafeguard == null,
                    new PrioritySelector(
                        Spell.CastOnGround(SpellBook.MockingBanner, on => Me.CurrentTarget.Location, ret => true, true),
                        Spell.CastOnGround(SpellBook.DemoralizingBanner, on => Me.CurrentTarget.Location, ret => true, true)
                        )
                    ),
                new Decorator(ret => PRSettings.Instance.Warrior.AP_EnableBannerFreedom && !Spell.SpellOnCooldown(SpellBook.Intervene),
                    new PrioritySelector(
                        Spell.Cast(SpellBook.Intervene, on => Unit.BestUnitForSafeguardResult, ret => Common.NeedsFreedom && Unit.BestUnitForSafeguard != null),
                        Spell.Cast(SpellBook.Intervene, on => Unit.SafeguardLowHealthResult, ret => Unit.SafeguardLowHealth != null),
                        Spell.Cast(SpellBook.Intervene, on => Unit.SafeguardScatterShotResult, ret => Unit.SafeguardScatterShot != null)
                        )
                    ),
                Spell.CastOnGround(SpellBook.HeroicLeap, on => Unit.LeapScatterShotResult.Location, ret => Unit.LeapScatterShot != null),
                Spell.CastOnGround(SpellBook.DemoralizingBanner, on => Unit.DemoBannerLowHealthResult.Location, ret => Unit.DemoBannerLowHealth != null, true),
                Spell.Cast(SpellBook.Vigilance, on => Unit.VigilanceLowHealthResult, ret => PRSettings.Instance.Warrior.AP_UseVigilance && TalentManager.HasTalent(15) && Unit.VigilanceLowHealth != null),
                Spell.Cast(122294, ret => Common.NeedsStampedingShout)
            );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Recklessness", ret => Common.NonExecuteCheck || (Common.ExecuteCheck && ((Common.ColossusSmashAuraT || Common.CSCD <= 1.5) && (!Common.BBTalent || Common.BBCD < 0)))),
                Spell.Cast("Bloodbath", ret => Common.BBTalent && (Common.RecklessnessAura || Common.NonExecuteCheck || Common.ExecuteCheck)),
                Spell.Cast("Avatar", ret => Common.AVTalent && (Common.RecklessnessAura || Common.NonExecuteCheck || Common.ExecuteCheck)),
                Spell.CastOnGround(SpellBook.SkullBanner, ret => Me.Location, ret => Common.RecklessnessAura && !Spell.SpellOnCooldown(SpellBook.SkullBanner)),
                Spell.Cast("Berserker Rage", ret => !Common.EnrageAura && Me.CurrentRage < 90)
                );
        }

        private static Composite HandleOffensiveCooldownsPvp()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.Recklessness),
                Spell.Cast(SpellBook.Bloodbath, ret => Common.BBTalent),
                Spell.Cast(SpellBook.Avatar, ret => Common.AVTalent),
                Spell.Cast(SpellBook.Bladestorm, ret => Common.BSTalent),
                Spell.Cast(SpellBook.SkullBanner)
                );
        }

        private static Composite HandleCommon()
        {
            return new PrioritySelector(
            );
        }

        private static Composite HandleCommonPvp()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.BattleStance, ret => PRSettings.Instance.Warrior.ArmsStanceDance && Common.NeedsBattleStance),
                Spell.Cast(SpellBook.DefensiveStance, ret => PRSettings.Instance.Warrior.ArmsStanceDance && Common.NeedsDefensiveStance),
                Spell.Cast(SpellBook.BattleShout, ret => PRSettings.Instance.Warrior.ArmsShoutSelection == WarriorShout.Battle && (Common.NeedsShout || !Me.HasAura(SpellBook.BattleShout))),
                Spell.Cast(SpellBook.CommandingShout, ret => PRSettings.Instance.Warrior.ArmsShoutSelection == WarriorShout.Commanding && (Common.NeedsShout || !Me.HasAura(SpellBook.CommandingShout))),
                Spell.Cast(SpellBook.ShatteringThrow, ret => PRSettings.Instance.Warrior.AP_UseShatter && Common.NeedsShatteringThrow),
                Spell.Cast(SpellBook.BerserkerRage, ret => PRSettings.Instance.Warrior.AP_UseBerserkerRage && Common.ImFleeing),
                Spell.Cast(SpellBook.HeroicThrow, on => Me.CurrentTarget, ret => Me.GotTarget && Me.CurrentTarget.HealthPercent <= 5 && Me.CurrentTarget.IsAlive && !Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.Distance <= 30 && Me.CurrentTarget.InLineOfSight && Me.IsFacing(Me.CurrentTarget)),
                Spell.Cast(SpellBook.HeroicThrow, on => Unit.TotemToKillResult, ret => PRSettings.Instance.Warrior.AP_EnableHeroicThrowTotem && Unit.TotemToKill != null),
                Spell.Cast(SpellBook.Throw, on => Unit.TotemToKillResult, ret => PRSettings.Instance.Warrior.AP_EnableHeroicThrowTotem && Unit.TotemToKill != null && Unit.TotemToKillResult.CurrentHealth < 50 && !Unit.TotemToKillResult.IsWithinMeleeRange && !Me.IsMoving),
                new Decorator(ret => PRSettings.Instance.Warrior.AP_EnableHeroicThrowTotem && Unit.TotemToKill != null && Me.CurrentTarget != Unit.TotemToKillResult  && Unit.TotemToKillResult.IsWithinMeleeRange, new Action(delegate { Unit.TotemToKillResult.Target(); }))
            );
        }

        // Updated by nomnomnom - 13 june 2013
        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.HeroicStrike, ret => Unit.AttackableMeleeUnits.Count() <= 2 && (Me.CurrentRage >= Me.MaxRage - 15 || (Common.ColossusSmashAura && Me.CurrentRage >= Me.MaxRage - 40 && Common.NonExecuteCheck))),
                Spell.Cast(SpellBook.MortalStrike),
                Spell.Cast(SpellBook.DragonRoar, ret => Common.DRTalent && (Common.BloodbathAura || Common.AVTalent || Common.SBTalent) && !Common.ColossusSmashAura && Common.NonExecuteCheck),
                Spell.Cast(SpellBook.StormBolt, ret => Common.SBTalent && Common.ColossusSmashAura),
                Spell.Cast(SpellBook.ColossusSmash, ret => !Common.ColossusSmashAura || Common.FadingCs(1000)),
                Spell.Cast(SpellBook.Execute, ret => Common.ColossusSmashAura || Common.RecklessnessAura || Me.CurrentRage >= Me.MaxRage - 25),
                Spell.Cast(SpellBook.DragonRoar, ret => Common.DRTalent && (((Common.BloodbathAura || Common.AVTalent || Common.SBTalent) && Common.NonExecuteCheck) || (!Common.ColossusSmashAura && Common.ExecuteCheck))),
                Spell.Cast(SpellBook.Slam, ret => Common.ColossusSmashAura && (Common.FadingCs(1000) || Common.RecklessnessAura) && Common.NonExecuteCheck),
                Spell.Cast(SpellBook.Overpower, ret => Common.TasteForBloodS3 && Common.NonExecuteCheck),
                Spell.Cast(SpellBook.Slam, ret => Common.ColossusSmashAura && Common.FadingCs(2500) && Common.NonExecuteCheck),
                Spell.Cast(SpellBook.Execute, ret => !Common.SuddenExecAura),
                Spell.Cast(SpellBook.Overpower, ret => Common.NonExecuteCheck || Common.SuddenExecAura),
                Spell.Cast(SpellBook.Slam, ret => Me.CurrentRage >= 40 && Common.NonExecuteCheck),
                Spell.Cast(SpellBook.BattleShout, ret => PRSettings.Instance.Warrior.ArmsShoutSelection == WarriorShout.Battle),
                Spell.Cast(SpellBook.CommandingShout, ret => PRSettings.Instance.Warrior.ArmsShoutSelection == WarriorShout.Commanding),
                Spell.Cast(SpellBook.ImpendingVictory, ret => !Common.IVOC && Common.IVTalent && PRSettings.Instance.Warrior.ArmsImpVic),
                Spell.Cast(SpellBook.HeroicThrow)
                );
        }

        //private static Composite HandleSingleTarget()
        //{
        //    return new PrioritySelector(
        //       Spell.Cast("Colossus Smash", ret => Common.ColossusSmash1Sec || !Common.ColossusSmashAura),
        //       Spell.Cast("Mortal Strike"),
        //       Spell.Cast("Execute", ret => !Common.SuddenExecAura),
        //       Spell.Cast("Heroic Strike", ret => (Me.CurrentTarget.HasAura(86346) && Me.CurrentRage >= 80 && Common.NonExecuteCheck) || Me.CurrentRage >= 105),
        //       Spell.Cast("Dragon Roar", ret => Common.DRTalent && Common.BBTalent && Common.BloodbathAura && !Common.ColossusSmashAura && Common.NonExecuteCheck),
        //       Spell.Cast("Storm Bolt", ret => Common.SBTalent && Common.ColossusSmashAura),
        //       Spell.Cast("Execute", ret => Common.ColossusSmashAura || Common.RecklessnessAura || Me.CurrentRage >= Me.CurrentRageMax - 25),
        //       Spell.Cast("Dragon Roar", ret => Common.DRTalent && (Common.BBTalent && Common.BloodbathAura && Common.NonExecuteCheck) || (!Common.ColossusSmashAura && Common.ExecuteCheck)),
        //       Spell.Cast("Overpower", ret => (Common.NonExecuteCheck && Common.TasteForBloodStacks3) || Common.NonExecuteCheck || Common.SuddenExecAura),
        //       Spell.Cast("Slam", ret => (Me.CurrentRage >= 40 || Common.RecklessnessAura || (Common.ColossusSmashAura && Common.ColossusSmash25Sec)) && Common.NonExecuteCheck),
        //       Spell.Cast("Battle Shout"),
        //       Spell.Cast("Heroic Throw")
        //        );

        //}

        private static Composite HandleSingleTargetPvp()
        {
            return 
            new Decorator(ret => Me.CurrentTarget != null,
                new PrioritySelector(
                   Spell.Cast(SpellBook.Overpower, ret => Common.Attackable && Common.NeedsOP),
                   Spell.Cast(SpellBook.Execute, ret => Common.Attackable && Me.CurrentTarget.IsWithinMeleeRange),
                   Spell.PreventDoubleCast(SpellBook.Hamstring, 5, ret => PRSettings.Instance.Warrior.AP_EnableHamstring && Common.NonExecuteCheck && !TalentManager.HasTalent(8) && Common.Attackable && Me.CurrentTarget.IsWithinMeleeRange && Common.NeedsSlow),
                   Spell.PreventDoubleCast(SpellBook.PiercingHowl, 5, ret => PRSettings.Instance.Warrior.AP_EnableHamstring && Common.NonExecuteCheck && TalentManager.HasTalent(8) && Common.Attackable && Common.NeedsSlow && Me.CurrentTarget.Distance <= 15),
                   Spell.Cast(SpellBook.HeroicStrike, ret => Common.Attackable && Me.CurrentTarget.IsWithinMeleeRange && Common.NonExecuteCheck && Me.CurrentRage >= 85),
                   Spell.Cast(SpellBook.MortalStrike, ret => Common.Attackable && Me.CurrentTarget.IsWithinMeleeRange && (Me.CurrentRage <= 90 || !Me.CurrentTarget.HasAura(115804))), //Mortal Wounds
                   Spell.Cast(SpellBook.ColossusSmash, ret => Common.Attackable && Me.CurrentTarget.IsWithinMeleeRange),
                   Spell.Cast(SpellBook.Slam, ret => Common.Attackable && Me.CurrentTarget.IsWithinMeleeRange && Common.NonExecuteCheck && Me.CurrentTarget.HasAura(SpellBook.ColossusSmash)),
                   Spell.Cast(SpellBook.Overpower, ret => Common.Attackable && Me.CurrentTarget.IsWithinMeleeRange && Common.NonExecuteCheck && !Me.CurrentTarget.HasAura(SpellBook.ColossusSmash))
                )
            );
        }

        private static Composite InterruptsPvp()
        {
            return
                new PrioritySelector(
                    Spell.Cast(SpellBook.Pummel, ret => Common.Attackable && Common.NeedsPummel),
                    Spell.FaceAndCast("Pummel", OnUnit => Me.FocusedUnit, ret => Common.FocusAttackable && Common.NeedsFocusPummel),
                    Spell.Cast(SpellBook.DisruptingShout, ret => Common.NeedsDisruptingShout),
                    Spell.Cast(SpellBook.Shockwave, ret => Common.NeedsShockwave),
                    Spell.Cast(SpellBook.StormBolt, ret => Common.Attackable && Common.NeedsStormBolt),
                    Spell.FaceAndCast("Storm Bolt", OnUnit => Me.FocusedUnit, ret => Common.FocusAttackable && Common.NeedsFocusStormBolt),
                    Spell.FaceAndCast("Charge", OnUnit => Me.FocusedUnit, ret => Common.FocusAttackable && Common.NeedsFocusCharge),
                    Spell.Cast(SpellBook.SpellReflection, ret => Common.NeedsSpellReflect),
                    Spell.Cast(SpellBook.MassSpellReflection, ret => Common.NeedsMassSpellReflect)

                    );
        }


        // Updated by nomnomnom - 13 june 2013
        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                new Decorator(ret => Unit.AttackableMeleeUnits.Count() == 2,
                    new PrioritySelector(
                        Spell.Cast(SpellBook.SweepingStrikes, ret => Common.DeepWoundsAura),
                        Spell.Cast(SpellBook.ThunderClap, ret => Common.NeedThunderClap),
                        HandleSingleTarget()
                        )),
                new Decorator(ret => Unit.AttackableMeleeUnits.Count() == 3,
                    new PrioritySelector(
                        Spell.Cast(SpellBook.SweepingStrikes, ret => Common.DeepWoundsAura),
                        Spell.Cast(SpellBook.ThunderClap, ret => Common.NeedThunderClap),
                        Spell.Cast(SpellBook.Bladestorm, ret => Common.BSTalent),
                        Spell.Cast(SpellBook.DragonRoar, ret => Common.DRTalent),
                        Spell.Cast(SpellBook.Shockwave, ret => Common.SWTalent),
                        Spell.Cast(SpellBook.Cleave, ret => Unit.AttackableMeleeUnits.Count() >= 3 && (Me.CurrentRage >= Me.MaxRage - 15 || (Common.ColossusSmashAura && Me.CurrentRage >= Me.MaxRage - 40 && Common.NonExecuteCheck))),
                        HandleSingleTarget()
                        )),
                new Decorator(ret => Unit.AttackableMeleeUnits.Count() >= 4,
                    new PrioritySelector(
                        Spell.Cast(SpellBook.SweepingStrikes, ret => Common.DeepWoundsAura),
                        Spell.Cast(SpellBook.ThunderClap, ret => Common.NeedThunderClap),
                        Spell.Cast(SpellBook.Bladestorm, ret => Common.BSTalent),
                        Spell.Cast(SpellBook.DragonRoar, ret => Common.DRTalent),
                        Spell.Cast(SpellBook.Shockwave, ret => Common.SWTalent),
                        Spell.Cast(SpellBook.Whirlwind),
                        Spell.Cast(SpellBook.MortalStrike),
                        Spell.Cast(SpellBook.ColossusSmash, ret => !Common.ColossusSmashAura || Common.FadingCs(1000)),
                        Spell.Cast(SpellBook.Overpower)
                        ))
                );
        }

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1733 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.WarriorArms; }
        }

        public override string Name
        {
            get { return "Arms Warrior"; }
        }

        internal override string Help
        {
            get
            {
                return
                    @"
                    -----------------------------------------------------------------

                    Arms PVE
                    --------
                    -

                    Arms PVP
                    --------
                    -Select ""Force PVP Rotation"" in settings to enable.
                    -The cooldown hotkey will force Avatar or Bloodbath or Bladestorm, Skull Banner, and Recklessness.
                    -There are hotkeys you can review and set in the class config settings.

                    * Recently updated for 5.4
                    * Recently added support for Storm Bolt, Vigilance, Mass Spell Reflection and cooldown key burst.
                    * Expect issues until it can be tested.
                    * Please post issues in the Warena thread of the Warrior forum.
                    -----------------------------------------------------------------
                     ";
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    new Decorator(ret => PRSettings.Instance.Warrior.AP_ForcePVP, PVPRotation),
                    new Decorator(ret => !PRSettings.Instance.Warrior.AP_ForcePVP,
                    new PrioritySelector(
                    HandleCommon(),
                    HandleDefensiveCooldowns(),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    Racials.UseRacials(),
                    Item.HandleItems(),       // Pop Trinkets, Drink potions...
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Warrior.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown && Me.CurrentTarget.IsWithinMeleeRange, HandleOffensiveCooldowns()),
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Warrior.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown && Me.CurrentTarget.IsWithinMeleeRange, HandleOffensiveCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      HandleSingleTarget())))));
            }
        }

        public override Composite PVPRotation
        {
            get 
            { 
                return
                    new PrioritySelector(
                    new Decorator(ret => HotKeyManager.KeyboardPolling.IsKeyDown(Keys.K), Spell.TreePerformance(true)),
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    HandleHotkeys(),
                    HandleCommonPvp(),
                    HandleDefensiveCooldowns(),
                    HandleDefensiveCooldownsPvp(),
                    InterruptsPvp(),
                    new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count(a => !PvP.IsInCC(a)) > PRSettings.Instance.Warrior.AoECount,
                        new PrioritySelector(
                            Spell.Cast(SpellBook.SweepingStrikes),
                            Spell.Cast(SpellBook.ThunderClap, ret => Unit.AttackableMeleeUnits.Count(a => !a.HasAura(115767)) > 1)
                            )
                        ),
                    HandleSingleTargetPvp()
                    ); 
            }
        }

        public override Composite Medic
        {
            get
            {
                return new PrioritySelector();
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        HandleHotkeys()
                        ));
            }
        }
      
        private static Composite HandleHotkeys()
        {
            return 
                new PrioritySelector(
                    new Decorator(ret => HotKeyManager.KeyboardPolling.IsKeyDown(PRSettings.Instance.Warrior.ArmsHLMod) && HotKeyManager.KeyboardPolling.IsKeyDown(Keys.LShiftKey) && Me.CurrentTarget != null && Me.CurrentTarget.Distance >= 8 && Me.CurrentTarget.Distance <= 40, Spell.CastOnGround(SpellBook.HeroicLeap, on => Me.CurrentTarget.Location)),
                    new Decorator(ret => HotKeyManager.KeyboardPolling.IsKeyDown(PRSettings.Instance.Warrior.ArmsSRMod) && !Spell.SpellOnCooldown(SpellBook.SpellReflection), Spell.Cast(SpellBook.SpellReflection)),
                    new Decorator(ret => HotKeyManager.KeyboardPolling.IsKeyDown(PRSettings.Instance.Warrior.ArmsSWMod) && !Spell.SpellOnCooldown(SpellBook.ShieldWall), Spell.Cast(SpellBook.ShieldWall)),
                    new Decorator(ret => HotKeyManager.KeyboardPolling.IsKeyDown(HotkeySettings.Instance.CooldownKeyChoice), HandleOffensiveCooldownsPvp())
                    );
        }

        #endregion Overrides of RotationBase

    }
}