#region Revision info
/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Warlock/Destruction.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade: $
 */
#endregion

using System;
using System.Collections.Generic;
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
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace PureRotation.Classes.Warlock
{
    [UsedImplicitly]
    class Destruction : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }
        private static WarlockSettings WarlockSettings { get { return PRSettings.Instance.Warlock; } }

        #region PvE

        #region Cool-downs
        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Ember Tap", on => Me, ret => Me.HealthPercent < WarlockSettings.EmberTapPercent && CurrentBurningEmbers > 1),
                Common.CommonDefensiveCooldowns()
                );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Dark Soul: Instability", 
                    ret => WarlockSettings.UseDarkSoulInstability 
                        && ((CurrentBurningEmbers >= WarlockSettings.DarkSoulMinimumEmbers) || Me.HasAura(SpellBook.FireandBrimstone))
                        && !WarlockSettings.KanrethadEbonlockeFight 
                        && !NeedImmolate 
                        && !Common.NeedCurseOfElements),
                Common.CommonOffensiveCooldowns()
                );
        }
        #endregion

        #region Rotation Handles [Single/AoE]
        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                new Decorator(ret => Me.HasAura(SpellBook.FireandBrimstone),
                    new Action(delegate { Me.CancelAura("Fire and Brimstone"); return RunStatus.Failure; })),

                Spell.PreventDoubleCast("Curse of the Elements", 2, ret => Common.NeedCurseOfElements),
                Spell.PreventDoubleCast("Immolate", 2, on => Me.CurrentTarget, ret => NeedImmolate, UseKilJaedensCunningPassive),

            #region Enhanced Single Target

                new Decorator(ret => (WarlockSettings.EnhancedSingleTargetMode || HotKeyManager.IsSpecialKey) && !SpellManager.GlobalCooldown,
                            HavocAoEHandler()),

                new Decorator(ret => (WarlockSettings.EnhancedSingleTargetMode|| HotKeyManager.IsSpecialKey) && !SpellManager.GlobalCooldown,
                            RainOfFireAllUnits()),

           #endregion Enhanced Single Target

                Spell.CastOnGround(SpellBook.RainofFire, ret => Me.CurrentTarget.Location, ret => WarlockSettings.SingleTargetRoF && RainOfFireConditions),
                Spell.PreventDoubleCast("Shadowburn", 0.5, on => Me.CurrentTarget, ret => NeedShadowburn, UseKilJaedensCunningPassive),
                Spell.Cast("Conflagrate", ret => !ChaosBoltAura && CurrentBurningEmbers < 3.5),
                Spell.PreventDoubleCast("Chaos Bolt", 0.5, on => Me.CurrentTarget, ret => NeedChaosBolt && !WarlockSettings.KanrethadEbonlockeFight, UseKilJaedensCunningPassive),
                Spell.Cast("Conflagrate"),
                Spell.PreventDoubleCast("Incinerate", 0.5, on => Me.CurrentTarget, ret => true, UseKilJaedensCunningPassive)
                );
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(

                new Decorator(ret => CurrentBurningEmbers < 1 && Me.HasAura(SpellBook.FireandBrimstone),
                    new Action(delegate { Me.CancelAura("Fire and Brimstone"); return RunStatus.Failure; })),

                Spell.CastOnGround(SpellBook.RainofFire, ret => Me.CurrentTarget.Location, ret => RainOfFireConditions),
                
                new Decorator(ret => !SpellManager.GlobalCooldown, HavocAoEHandler()),
                new Decorator(ret => !SpellManager.GlobalCooldown, RainOfFireAllUnits()),

                Spell.PreventDoubleCast("Fire and Brimstone", 0.5, on => Me, ret => !Me.HasAura(SpellBook.FireandBrimstone) && CurrentBurningEmbers > 1, true),
                Spell.PreventDoubleCast("Immolate", 3, on => Me.CurrentTarget, ret => Me.HasAura(SpellBook.FireandBrimstone) && (NeedImmolate || NeedAoEImmolate), UseKilJaedensCunningPassive),
                Spell.PreventDoubleCast("Conflagrate", 0.5, on => Me.CurrentTarget, ret => Me.HasAura(SpellBook.FireandBrimstone) && (!NeedAoEImmolate && !NeedImmolate), true),

                // Multi DoT Immolate (for low embers)
                Spell.MultiDoT("Immolate", Me.CurrentTarget, ret => !Me.HasAura(SpellBook.FireandBrimstone) && CurrentBurningEmbers < 1),

                // Filler
                Spell.PreventDoubleCast("Incinerate", 0.5, on => Me.CurrentTarget, ret => true, UseKilJaedensCunningPassive)
                );
        }

        private static Composite HavocAoEHandler()
        {
            return new PrioritySelector(
                new Decorator(ret => WarlockSettings.UseHavoc && !WarlockSettings.KanrethadEbonlockeFight && SpellManager.CanCast(SpellBook.Havoc),
                        Spell.PreventDoubleCast("Havoc", 0.5, on => HavocTarget(), ret => HavocTarget() != null, true)),

                new Decorator(ret => Me.HasAura("Havoc") && CurrentBurningEmbers >= 1,
                    Spell.PreventDoubleCast("Shadowburn", 0.5, on => BestShadowburnUnit(), ret => BestShadowburnUnit() != null, true))
                );

        }
        #endregion

        #region Unit Stuff
        private static Composite RainOfFireAllUnits()
        {
            return new Decorator(ret => Me.ManaPercent >= 50 && WarlockSettings.RoFEverythingInRange,
                new PrioritySelector(ctx => Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 31).FirstOrDefault(x => x.IsValid && !x.HasAura("Rain of Fire") && !x.IsMoving && (x.HealthPercent >= 15 || x.IsBoss()) && !x.IsPet),
                    Spell.CastOnGround(SpellBook.RainofFire, on => ((WoWUnit)on).Location, ret => ((WoWUnit)ret) != null)
                ));
        }

        private static WoWUnit HavocTarget()
        {
            return Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 35)
                    .Where(t => t.IsValid && t != Me.CurrentTarget)
                    .OrderByDescending(t => t.CurrentHealth) // We want highest HP value, not percent
                    .FirstOrDefault(x => !x.IsPet); // don't cast on our current target.
        }

        private static WoWUnit BestShadowburnUnit()
        {
            // See if we can use our current target first
            if (Me.CurrentTarget.HealthPercent < 20 && !Me.CurrentTarget.HasAura("Havoc"))
            {
                return Me.CurrentTarget;
            }

            // Can't use current target, scan for one.
            var bestHostileEnemy = Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 35)
                        .Where(t => t.HealthPercent < 20 && t.IsValid)
                        .OrderByDescending(t => t.HealthPercent)
                        .FirstOrDefault(t => !t.HasAura("Havoc") && !t.IsPet);

            return bestHostileEnemy;
        }
        #endregion

        #endregion PvE

        #region PvP

        private static Composite PvPSingleTarget()
        {
            return new PrioritySelector(
                //HotKeyManager.DeactivateSpecialKey(ret => Spell.Lastspellcast == "Fear")
                HandleSingleTarget()
                );
        }

        private static Composite PvPAoE()
        {
            return new PrioritySelector(
                PvPSingleTarget()
                );
        }


        #endregion PvP

        #region booleans

        private static double CurrentBurningEmbers { get { return (double)Me.GetPowerInfo(WoWPowerType.BurningEmbers).Current / 10; /*Styx.WoWInternals.Lua.PlayerUnitPower("SPELL_POWER_BURNING_EMBERS");*/ } }
        private static bool NeedShadowburn { get { return CurrentBurningEmbers >= 1 && Me.CurrentTarget.HealthPercent <= 20; } }
        private static bool NeedImmolate {get { return !Me.CurrentTarget.HasMyAura("Immolate") || (Me.CurrentTarget.HasMyAura("Immolate") && Spell.GetMyAuraTimeLeft("Immolate", Me.CurrentTarget) <= 7.5); } }
        private static bool NeedAoEImmolate
        {
            get
            {
                return
                    Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).FirstOrDefault(x => !x.HasMyAura("Immolate")) != null && Me.HasAura(SpellBook.RainofFire);
            }
        }
        private static bool NeedChaosBolt
        {
            get
            {
                return Me.CurrentTarget.HealthPercent > 20 &&
            (CurrentBurningEmbers >= 1 && (!Me.HasAura(SpellBook.Backdraft) || ChaosBoltAura)) || CurrentBurningEmbers >= 3.5;
            }
        }
        private static bool ChaosBoltAura { get { return Me.HasAnyAura(SpellBook.DarkSoulInstability, SpellBook.SkullBanner, 138963 /*Perfect Aim*/); } }
        private static bool UseKilJaedensCunningPassive { get { return TalentManager.HasTalent(17); } }
        private static bool RainOfFireConditions { get { return Me.CurrentTarget != null && Me.ManaPercent >= 50 && !Me.CurrentTarget.IsMoving && Me.CurrentTarget.Distance <= 35 && !Me.CurrentTarget.HasAura("Rain of Fire") && !Me.CurrentTarget.IsFriendly && !Me.CurrentTarget.IsDead; } }

        #endregion

        #region Kanrethad Ebonlocke - The Green Fire Quest

        // ReSharper disable InconsistentNaming
        private static WoWUnit _kanrethadEbonlocke;
        private static WoWUnit _pitLord;
        private static WoWUnit _doomLord;
        private static WoWUnit _felhunter;
        private readonly static WoWPoint gatewayLocation = new WoWPoint(648.1494, 304.7803, 353.829);
        private readonly static WoWPoint losLocation = new WoWPoint(632.5417, 262.8134, 352.996);
        private static int felhunterUnitCount = 0;
        //private const int FELHUNTER = 70072;
        //private const int DOOMLORD = 70072;
        //private const int KANRETHAD = 70072;
        //private const int PITLORD = 70075;

        // ReSharper restore InconsistentNaming

        private static Composite SetGreenFireUnits
        {
            get
            {
                return new Action(delegate
                {
                    /*
                    _kanrethadEbonlocke = GetAllUnits().FirstOrDefault(u => u.Entry == KANRETHAD);
                    _pitLord = GetAllUnits().FirstOrDefault(u => u.Entry == PITLORD);
                    _doomLord = GetAllUnits().FirstOrDefault(u => u.Entry == DOOMLORD && !u.HasAnyAura("Banish", "Fear"));
                    _felhunter = GetAllUnits().FirstOrDefault(u => u.Entry == FELHUNTER);
                    felhunterUnitCount = GetAllUnits().Count(u => u.Entry == FELHUNTER);
                    */

                    _kanrethadEbonlocke = GetAllUnits().FirstOrDefault(u => u.Name == "Kanrethad Ebonlocke");
                    _pitLord = GetAllUnits().FirstOrDefault(u => u.Name == "Pit Lord");
                    _doomLord = GetAllUnits().FirstOrDefault(u => u.Name == "Doom Lord" && !u.HasAnyAura("Banish", "Fear"));
                    _felhunter = GetAllUnits().FirstOrDefault(u => u.Name == "Felhunter");
                    felhunterUnitCount = GetAllUnits().Count(u => u.Name == "Felhunter");
                    return RunStatus.Failure;
                });

            }
        }

        public static Composite KanrethadEbonlockeHandler()
        {

            return new PrioritySelector(

                SetGreenFireUnits,

                // Chaos Bolt Avoidance
                Spell.Cast("Sacrificial Pact",
                    ret => //!SpellManager.CanCast("Demonic Circle: Teleport") && // Can't teleport
                         _kanrethadEbonlocke != null &&
                         _kanrethadEbonlocke.CastingSpellId == 138559 && // Protect against Chaos Bolt
                        !_kanrethadEbonlocke.IsTargetingPet && // If he isn't targetting Pet
                         _kanrethadEbonlocke.IsTargetingMeOrPet), // If he is targetting Me

                Spell.Cast("Demonic Circle: Teleport",
                    ret => !SpellManager.CanCast("Sacrificial Pact")
                        && (!StyxWoW.Me.HasAura("Sacrificial Pact") || !StyxWoW.Me.HasAura("Unending Resolve")) &&
                        _kanrethadEbonlocke != null &&
                         _kanrethadEbonlocke.CastingSpellId == 138559 && // Protect against Chaos Bolt
                        !_kanrethadEbonlocke.IsTargetingPet && // If he isn't targetting Pet
                         _kanrethadEbonlocke.IsTargetingMeOrPet), // If he is targetting Me

                Spell.Cast("Unending Resolve",
                    ret => !SpellManager.CanCast("Sacrificial Pact") && !SpellManager.CanCast("Demonic Circle: Teleport") && !StyxWoW.Me.HasAura("Sacrificial Pact") &&
                        _kanrethadEbonlocke != null &&
                         _kanrethadEbonlocke.CastingSpellId == 138559 && // Protect against Chaos Bolt
                        !_kanrethadEbonlocke.IsTargetingPet && // If he isn't targetting Pet
                         _kanrethadEbonlocke.IsTargetingMeOrPet), // If he is targetting Me

                // Pit Lord
                Spell.PreventDoubleCast("Enslave Demon", 0.5, on => _pitLord, ret => _pitLord != null && _pitLord.Attackable && !_pitLord.HasAura("Enslave Demon") && felhunterUnitCount == 0, true),// && !_pitLord.IsFriendly),
                Spell.Cast("Enslave Demon", on => _pitLord, ret => _pitLord != null && _pitLord.Attackable && !_pitLord.HasAura("Enslave Demon") && felhunterUnitCount == 0), // safety check for pit lord
                new Decorator(ret => _pitLord != null && _pitLord.HasAura("Enslave Demon"), PitLordActions()),

                // Anti-Enrage Counter - Purification Potion
                Item.UseBagItem("Purification Potion", ret => !Me.HasAura(138560/*agony*/) && Me.HasAura(138558/*doom*/) && Spell.GetAuraTimeLeft(138558/*doom*/) < 60, "Purification Potion -> Close to enrage"),

                // Felhunter
                Spell.Cast("Soulshatter", on => StyxWoW.Me, ret => _felhunter != null),
                Spell.CastOnGround(104232/*Rain of Fire*/, ret => gatewayLocation, ret => _kanrethadEbonlocke != null && _kanrethadEbonlocke.ChanneledCastingSpellId == 138751 && !StyxWoW.Me.HasAura(104232), true),
                Spell.Cast("Havoc", on => _felhunter, ret => _felhunter != null),
                Spell.Cast("Dark Soul: Instability", ret => _felhunter != null),

                Spell.Cast("Shadowburn", ret => StyxWoW.Me.CurrentTarget.Name == "Felhunter" && (!StyxWoW.Me.CurrentTarget.HasAura("Havoc") || felhunterUnitCount == 1)),
                Spell.PreventDoubleCast("Chaos Bolt", 0.5, on => Me.CurrentTarget, ret => Me.CurrentTarget == _kanrethadEbonlocke && CurrentBurningEmbers > 3.5, true),
                Spell.Cast("Chaos Bolt", ret => StyxWoW.Me.CurrentTarget.Name == "Felhunter" && (!StyxWoW.Me.CurrentTarget.HasAura("Havoc") || felhunterUnitCount == 1)),

                // Doom Lord
                Spell.PreventDoubleCast("Banish", 2.5, on => _doomLord, ret => _doomLord != null && WarlockSettings.KanrethadEbonlockeFightDoomLordCC && !_doomLord.HasAnyAura("Banish") && !HaveUnitBanished), // Banish Doom Lord
                Spell.PreventDoubleCast("Fear", 2.5, on => _doomLord, ret => _doomLord != null && WarlockSettings.KanrethadEbonlockeFightDoomLordCC && !_doomLord.HasAura("Banish") && !HaveUnitFeared && HaveUnitBanished), // Fear Doom Lord

                // Stuff to keep us alive
                Spell.Cast("Twilight Ward"), // Cast on CD to reduce incoming damage

                Spell.CastOnGround("Shadowfury", loc => StyxWoW.Me.CurrentTarget.Location,
                    ret => Unit.CachedNearbyAttackableUnits(StyxWoW.Me.CurrentTarget.Location, 8).Count() > 2), // Stun the shitty imps

                Spell.PreventDoubleCast("Command Demon", 10, ret => StyxWoW.Me.HasAura(138587) || StyxWoW.Me.HasAura(138560)), // sacrifice imp - dispell debuff

                Spell.Cast(108482/*unbound will*/, ret => StyxWoW.Me.HasAllAuras( // If we have all of the bad debuffs, use talent
                    138560, // Excruciating Agony
                    138587)) // seed of terrible destruction       

                );

        }

        public static Composite PitLordActions()
        {
            return new PrioritySelector(
                // Move Pit Lord out of LoS of felhunters
                Spell.PreventDoubleRunMacro("/petmoveto", 2, ret => _pitLord.Location != losLocation && _kanrethadEbonlocke.ChanneledCastingSpellId == 138751 && felhunterUnitCount == 0),

                // Charge
                //Spell.PreventDoubleRunMacro("/click PetActionButton 5", 21, on => _kanrethadEbonlocke, after => GetBestAfterUnit(), ret => _kanrethadEbonlocke != null && felhunterUnitCount == 0 && StyxWoW.Me.CurrentTarget == _kanrethadEbonlocke && _kanrethadEbonlocke.CastingSpellId == 138564), // Interupt Cataclysm
                Spell.PreventDoubleRunMacro("/cast Charge", 21, on => _kanrethadEbonlocke, after => GetBestAfterUnit(), ret => _kanrethadEbonlocke != null && felhunterUnitCount == 0 && StyxWoW.Me.CurrentTarget == _kanrethadEbonlocke && _kanrethadEbonlocke.CastingSpellId == 138564), // Interupt Cataclysm

                // Demonic Siphon
                //Spell.PreventDoubleRunMacro("/click PetActionButton 7", 21, ret => _kanrethadEbonlocke != null && felhunterUnitCount == 0 && (StyxWoW.Me.HealthPercent <= 60 || (Me.HasAura("Unending Resolve") && Me.HealthPercent <= 85))), // Heal Me!
                Spell.PreventDoubleRunMacro("/cast Demonic Siphon", 21, ret => _kanrethadEbonlocke != null && felhunterUnitCount == 0 && (StyxWoW.Me.HealthPercent <= 60 || (Me.HasAura("Unending Resolve") && Me.HealthPercent <= 85))), // Heal Me!

                // Fel Flame Breath
                //Spell.PreventDoubleRunMacro("/click PetActionButton 6", 11, ret => _kanrethadEbonlocke != null && felhunterUnitCount == 0)
                Spell.PreventDoubleRunMacro("/cast Fel Flame Breath", 11, ret => _kanrethadEbonlocke != null && felhunterUnitCount == 0)
                );

        }

        public static WoWUnit GetBestAfterUnit()
        {
            if (StyxWoW.Me.CurrentTarget == _pitLord)
            {
                return _kanrethadEbonlocke;
            }

            return StyxWoW.Me.CurrentTarget;
        }

        public static IEnumerable<WoWUnit> GetAllUnits()
        {
            return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(u => u.CanSelect && !u.IsDead && !u.IsMe);
        }

        public static bool HaveUnitBanished
        {
            get
            {

                if (GetAllUnits().Count(x => x.HasAura("Banish")) > 0)
                {
                    return true;
                }

                return false;

            }
        }

        public static bool HaveUnitFeared
        {
            get
            {
                if (GetAllUnits().Count(x => x.HasAura("Fear")) > 0)
                {
                    return true;
                }

                return false;
            }
        }

        #endregion

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.WarlockDestruction; }
        }

        public override string Name
        {
            get { return "Destruction Warlock"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => (WarlockSettings.EnableLagFix && SpellManager.GlobalCooldown) || HotKeyManager.IsPaused || Me.CurrentTarget == null, new ActionAlwaysSucceed()),

                    HandleDefensiveCooldowns(),

                    CachedUnits.Pulse,
                    new Decorator(ret => !SpellManager.GlobalCooldown, 
                        new PrioritySelector(
                            EncounterSpecific.HandleActionBarInterrupts()
                            )),


                    Racials.UseRacials(),
                    Common.HandleCommonAbilities(),
                    Item.HandleItems(),

                    new Decorator(ret => WarlockSettings.KanrethadEbonlockeFight, KanrethadEbonlockeHandler()),

                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() >= WarlockSettings.AoECount, HandleAoeCombat()),
                                      new Decorator(ret => Me.CurrentTarget != null && Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() < WarlockSettings.AoECount, HandleSingleTarget()))),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss() && !EncounterSpecific.HasBadAura, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() >= WarlockSettings.AoECount, HandleAoeCombat()),
                                      new Decorator(ret => Me.CurrentTarget != null && Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() < WarlockSettings.AoECount, HandleSingleTarget()))),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))));
            }
        }

        public override Composite PVPRotation
        {
            get
            {
                return new PrioritySelector(
                    PVERotation,
                    new Decorator(ret => HotKeyManager.IsPaused || Me.CurrentTarget == null, new ActionAlwaysSucceed()),

                    //HandleDefensiveCooldowns(),

                    new Decorator(ret => !SpellManager.GlobalCooldown,
                        new PrioritySelector(
                            Racials.UseRacials(),
                            CachedUnits.Pulse,
                            //Common.HandleCommonAbilities(),
                            Item.HandleItems()
                            )),
                    
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      //HandleOffensiveCooldowns(),
                                      new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() >= WarlockSettings.AoECount, PvPAoE()), //x => !x.IsBoss()
                                      new Decorator(ret => Me.CurrentTarget != null && Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() < WarlockSettings.AoECount, PvPSingleTarget()))),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      //new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, PvPAoE()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, PvPSingleTarget()))));
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
                return new Decorator(ret => !Me.IsMoving && !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        Common.HandleCommonPreCombat()
                        ));
            }
        }

        internal override string Help
        {
            get
            {
                return
                    @"
-----------------------------------------------------------------
Special Key: Will toggle 'Enhanced Single Target' mode.

             The setting [Enhanced Single Target] will 
                override this, so disable setting if you wish 
                to use it via the special hotkey.

If using 'Enhanced Single Target', change AoE count to a 
   minimum of 3 for best results.

PvE Recommended Spec: http://www.wowhead.com/talent#oyiU|mzcmMb

Green Fire [Kanrethad Ebonlocke]? http://tinyurl.com/greenfireinfo

Latest Updates [26-Jun-2013]
- Added experimental setting to reduce in game lag.

Enjoy! ~ Millz
-----------------------------------------------------------------
                     ";
            }
        }

        #endregion
    }
}
