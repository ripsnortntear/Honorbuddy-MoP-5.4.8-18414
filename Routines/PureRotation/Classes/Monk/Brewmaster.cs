#region Revision info

/*
 * $Author: wulf$
 * $Date: 31/10/2012$
 * $ID$
 * $Revision: 1$
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Monk/Brewmaster.cs $
 * $LastChangedBy: wulf$
 * $ChangesMade$
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
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.Monk
{
    [UsedImplicitly]
    public class Brewmaster : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }
        private static double? _time_to_max;
        private static double? _EnergyRegen;
        private static double? _energy;

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                     Spell.Cast("Purifying Brew", ret => Common.CanUsePurifyingBrew), // Top Priority
                     Spell.PreventDoubleCast("Guard", ThrottleTime, ret => NeedGuard), // Debating..if we dont check for shuffle it steals Chi from Blackout Kick
                     Spell.PreventDoubleCast("Dampen Harm", ThrottleTime, ret => NeedDampenHarm),
                     Spell.Cast("Fortifying Brew", ret => NeedFortifyingBrew),
                     new Decorator(ret => Me.HealthPercent < 100, HandleHealingCooldowns()),
                     Spell.PreventDoubleCast("Chi Wave", ThrottleTime, ret => NeedChiWave),
                     Spell.PreventDoubleCast("Zen Sphere", ThrottleTime, ret => NeedZenSphere),
                     Item.UseBagItem(5512, ret => NeedHealthstone, "Healthstone"),
                     Spell.Cast("Zen Meditation", ret => NeedZenMeditation), // yeah yeah..its channeled..but it’ll reduce that one melee hit by 90%, which might save our life.
                     Spell.Cast("Elusive Brew", ret => NeedElusiveBrew));
        }


        private static Composite HandleUtility()
        {
            return new PrioritySelector(

                // Thinking about not taking Invoke Xuen, the White Tiger, it will fuck up our positioning if used against dragons and shit.
                    Spell.CastOnGround("Summon Black Ox Statue", ret => Me.CurrentTarget.Location, ret => Common.CanPlaceBlackOxStatue, true), // Checks target is not flying and we are not fighting elegon.
                    Spell.Cast("Disable", ret => Common.UnitIsFleeing));
        }

        public static Composite BrewMasterUseHealthStone()
        {
            return new PrioritySelector(
                new Decorator(ret => PRSettings.Instance.UsePotion && Me.HealthPercent < PRSettings.Instance.Monk.HealthstonePercent,
                    new PrioritySelector(ctx => Item.FindFirstUsableItemBySpell("Healthstone"),
                        new Decorator(ret => ret != null,
                            new Action(ret =>
                            {
                                ((WoWItem)ret).UseContainerItem();
                              //  WaLogger.CombatLogG("Using {0}", ((WoWItem)ret).Name);
                            }
                                )))));
        }

        private static Composite HandleHealingCooldowns()
        {
            return new PrioritySelector(
                    Spell.CastOnGround("Healing Sphere", ret => Me.Location, ret => NeedHealingSphere));
        }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                    Spell.Cast("Blackout Kick", ret => NeedBlackoutKick), // Apply Shuffle if not active or MaxChi
                    Spell.PreventDoubleCast("Tiger Palm", ThrottleTime, ret => NeedBuildStacksForGaurd), // Build PG and TP for Guard
                    Spell.PreventDoubleCast("Blackout Kick", ThrottleTime, ret => Common.Chi > 2),
                    Spell.Cast("Rushing Jade Wind", ret => NeedRushingJadeWind), // Pop this shit on cooldown
                    new Decorator(ret => Me.CurrentChi < Me.MaxChi, ChiBuilders()),
                    Spell.Cast("Touch of Death", ret => NeedTouchofDeath), // Meh - but dont waste Survivability over Dps.
                    Spell.PreventDoubleCast("Tiger Palm", ThrottleTime, ret => Common.Chi < 3)); // Our Filler.
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                     Spell.Cast("Rushing Jade Wind", ret => NeedRushingJadeWind), // Pop this shit on cooldown
                     Spell.PreventDoubleCast("Tiger Palm", ThrottleTime, ret => NeedBuildStacksForGaurd), // Build PG and TP for Guard
                     new Decorator(ret => !Me.HasAura("Shuffle"), new Action(delegate { Me.CancelAura("Spinning Crane Kick"); return RunStatus.Failure; })), // If we loose shuffle, STOP
                     Spell.CastOnGround("Dizzying Haze", ret => Me.CurrentTarget.Location, ret => NeedDizzyingHaze),
                     new Decorator(ret => Me.CurrentChi < Me.MaxChi, ChiBuilders()),
                     Common.ClearDizzyingHaze(), // hackish but that fucking circle shit pisses me off.. -- wulf.
                     Spell.Cast("Spinning Crane Kick", ret => NeedSpinningCraneKick && !Common.CanApplyBreathofFire),  // gtfo if Shuffle isnt up!
                     Spell.PreventDoubleCast("Breath of Fire", ThrottleTime, ret => NeedBreathofFire)); // Dont waste Chi if Shuffle isnt up!
        }

        private static Composite ChiBuilders()
        {
            return new PrioritySelector(
                     Spell.Cast("Keg Smash"),
                     Spell.Cast("Expel Harm", ret => Me.HealthPercent < 100),
                     Spell.Cast("Jab", ret => CanJab || Lua.PlayerPower > 80));
        }

        #region Settings

        private static MonkSettings MonkSettings { get { return PRSettings.Instance.Monk; } }

        private static int FortifyingBrewPercent { get { return MonkSettings.BrewFortifyingBrewPercent; } }

        private static int DampenHarmPercent { get { return MonkSettings.BrewDampenHarmPercent; } }

        private static int ZenMeditationPercent { get { return MonkSettings.BrewZenMeditationPercent; } }

        private static int ElusiveBrewCount { get { return MonkSettings.BrewElusiveBrewCount; } }

        private static int SpinningCraneKickCount { get { return MonkSettings.BrewSpinningCraneKickCount; } }

        private static int ChiWavePercent { get { return MonkSettings.BrewChiWavePercent; } }

        private static int HealingSpherePercent { get { return MonkSettings.BrewHealingSpherePercent; } }

        #endregion Settings

        #region Booleans

        private static bool NeedGuard { get { return Me.HasAura("Power Guard") && Me.HasAura("Shuffle") && Me.HealthPercent <= MonkSettings.GuardHPPercent; } }
        
        private static bool NeedDampenHarm { get { return TalentManager.HasTalent(14) && Me.HealthPercent <= DampenHarmPercent && !Me.HasAura("Fortifying Brew"); } }

        private static bool NeedFortifyingBrew { get { return Me.HealthPercent < FortifyingBrewPercent && !Me.HasAura("Fortifying Brew") && !Me.HasAura("Dampen Harm"); } }

        private static bool NeedZenMeditation { get { return Me.HealthPercent < ZenMeditationPercent; } }

        private static bool NeedElusiveBrew { get { return Me.CachedHasAura("Elusive Brew", ElusiveBrewCount) ; } }

        private static bool NeedBuildStacksForGaurd { get { return (!Me.HasAura("Power Guard") || !Me.HasAura("Tiger Power")) && Me.HasAura("Shuffle"); } }

        private static bool NeedRushingJadeWind { get { return Common.Chi >= 2 && TalentManager.HasTalent(16); } }

        private static bool NeedBlackoutKick { get { return Common.CanApplyShuffle; } }

        private static bool NeedTouchofDeath { get { return Me.HasAura("Death Note") && (Me.HealthPercent > 60 || TalentManager.HasGlyph("Touch of Death")); } }

        private static bool NeedDizzyingHaze { get { return Lua.PlayerPower >= 40 && Common.CanApplyDizzyingHaze; } }

        private static bool NeedSpinningCraneKick { get { return Unit.AttackableMeleeUnits.Count() >= SpinningCraneKickCount && Me.HasAura(115307, 0, false, 4000); } }

        private static bool CanJab { get { return Styx.WoWInternals.WoWSpell.FromId(121253).Cooldown; } }

        private static bool NeedBreathofFire { get { return Common.CanApplyBreathofFire && Me.HasAura("Shuffle"); } }

        private static bool NeedChiWave { get { return TalentManager.HasTalent(4) && Me.HealthPercent <= ChiWavePercent; } }

        private static bool NeedHealingSphere { get { return Lua.PlayerPower >= 60 && Me.HealthPercent <= HealingSpherePercent; } }

        private static bool NeedZenSphere { get { return TalentManager.HasTalent(5) && Me.HealthPercent <= MonkSettings.BrewWithZenSphere; } }

        private static bool NeedHealthstone { get { return Me.HealthPercent < PRSettings.Instance.Monk.HealthstonePercent; } }

        #endregion Booleans

        #region Overrides of RotationBase

        public override string Name
        {
            get { return "Brewmaster Monk"; }
        }

        public override string Revision
        {
            get { return "$Rev: 1735 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.MonkBrewmaster; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(

                    // Don't do anything if we have no target, nothing in melee range, or we're casting. (Includes vortex!)
                    new Decorator(ret => StyxWoW.Me.CurrentTarget != null && (!StyxWoW.Me.CurrentTarget.IsWithinMeleeRange || StyxWoW.Me.IsCasting || SpellManager.GlobalCooldown),
                        new ActionAlwaysSucceed()),
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    //new Action(delegate { Logger.InfoLog("Shuffle {0}", Me.HasTheAura(115307, 0, false, 5000)); return RunStatus.Failure; }),
                    HandleDefensiveCooldowns(), 
                    BrewMasterUseHealthStone(), // Stay Alive!
                    EncounterSpecific.HandleActionBarInterrupts(),
                 //   Item.HandleItems(),                                     // Pop Trinkets, Drink potions...
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),     // Interupt incomming damage!
                    HandleUtility(),
                    Racials.UseRacials(),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() >= PRSettings.Instance.Monk.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() >= PRSettings.Instance.Monk.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))));
            }
        }

        public override Composite Medic
        {
            get { return null; }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        Spell.Cast("Stance of the Sturdy Ox", ret => !Me.HasAura("Stance of the Sturdy Ox")),
                        Spell.CastRaidBuff("Legacy of the Emperor", ret => true)));
            }
        }

        public override Composite PVPRotation
        {
            get { return PVERotation; }
        }

        #endregion Overrides of RotationBase
    }
}