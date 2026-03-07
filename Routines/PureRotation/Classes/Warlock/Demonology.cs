#region Revision info
/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Warlock/Demonology.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade: $
 */
#endregion

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
using Action = Styx.TreeSharp.Action;
using Lua = PureRotation.Helpers.Lua;

/* *** Warlock Demonology Overrides ***
 * 
 * Standard
 * ---------------------------------------------------------
 * Felstorm (119914)             --> Command Demon (119898)
 * Dark Soul: Knowledge (113861) --> Dark Soul (77801)
 * Soul Link (108415)            --> Health Funnel (755)
 * 
 * Normal                --> Metamorphosis          --> Dark Apotheosis
 * --------------------------------------------------------------------------
 * Corruption            --> Doom                   --> Corruption
 * Hand of Gul'dan       --> Chaos Wave             --> Hand of Gul'dan
 * Shadow Bolt           --> Touch of Chaos         --> Demonic Slash
 * Curse of the Elements --> Aura of the Elements   --> Aura of the Elements
 * Curse of Enfeeblement --> Aura of Enfeeblement   --> Aura of Enfeeblement
 * Hellfire              --> Immolation Aura        --> Immolation Aura
 * Fel Flame             --> Void Ray               --> Void Ray
 * Drain Life            --> Harvest Life           --> Drain Life
 * Soulshatter           --> Soulshatter            --> Provocation
 * Fear                  --> Fear                   --> Sleep
 * Twilight Ward         --> Twilight Ward          --> Fury Ward
 * 
 * Spell IDs
 * -------------------------
 * 114168 = Dark Apotheosis
 * 172    = Corruption
 * 47960  = Shadowflame
 * 122355 = Molten Core
 * 104025 = Immolaton Aura
 * 103958 = Metamorphosis
 */

namespace PureRotation.Classes.Warlock
{
    [UsedImplicitly]
    class Demonology : RotationBase
    {
        private static WarlockSettings WarlockSettings { get { return PRSettings.Instance.Warlock; } }
        private delegate T Selection<out T>(object context);

        #region Cooldowns

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Common.CommonDefensiveCooldowns()
                );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Imp Swarm", ret => NeedImpSwarm),
                Spell.Cast("Dark Soul: Knowledge", on => Me, ret => NeedDarkSoulKnowledge),
                Common.CommonOffensiveCooldowns()
                );
        }

        #endregion Cooldowns

        #region Standard Rotation

        private static Composite HandleSingleTarget()
        {

            return new PrioritySelector(
                Spell.PreventDoubleCast("Curse of the Elements", 2, ret => Common.NeedCurseOfElements),
                Spell.PreventDoubleCast("Felstorm", 45, ret => NeedPetFelStorm),

            #region Shape shifted
                new Decorator(x => Me.CurrentTarget != null && Me.HasAura("Metamorphosis"),
                    new PrioritySelector(
                        Spell.PreventDoubleCast("Shadow Bolt", 0.1, on => Me.CurrentTarget, ret => Spell.GetMyAuraTimeLeft("Corruption",Me.CurrentTarget)<2, true), //Touch of Chaos
                        Spell.CastHack("Metamorphosis: Doom", "Doom", on => Me.CurrentTarget, ret => NeedDoom),
                        Spell.ForceCast(104027, on => Me.CurrentTarget, ret => Me.HasAura("Metamorphosis") && (_haveMoltenCoreBuff || Me.CurrentTarget.HealthPercent < 25) && (Me.HasAura(SpellBook.DarkSoulKnowledge) || Me.HasAura("Perfect Aim"))),
                        Spell.PreventDoubleCast("Shadow Bolt", 0.1, on => Me.CurrentTarget, ret => Spell.GetMyAuraTimeLeft("Corruption", Me.CurrentTarget) < 10, true), //Touch of Chaos
                        Spell.PreventDoubleCast("Shadow Bolt", 0.5, on => Me.CurrentTarget, ret => Me.HasAura("Metamorphosis"), true)
                        )),
                HandleShapeShifting(),
            #endregion

            #region Not-Shapeshifted
                Spell.Cast("Corruption", ret => NeedCorruption),
                CooldownTracker.PreventDoubleCast("Metamorphosis", 2, ret => (Me.HasAura(SpellBook.DarkSoulKnowledge) && CurrentDemonicFury%32>Spell.GetAuraTimeLeft(SpellBook.DarkSoulKnowledge)) ||
                    Spell.GetMyAuraTimeLeft("Corruption",Me.CurrentTarget)<5 || 
                    !Me.CurrentTarget.HasMyAura("Doom") ||
                    CurrentDemonicFury >=PRSettings.Instance.Warlock.MetamorphosisFuryValue
                    ),
                Spell.PreventDoubleCast("Hand of Gul'dan", 2, ret => !TalentManager.HasGlyph("Hand of Gul'dan") && NeedHandofGuldan),
                Spell.CastOnGround("Hand of Gul'dan", on => Me.CurrentTarget.Location, ret => TalentManager.HasGlyph("Hand of Gul'dan") && NeedHandofGuldan, true),
                Spell.PreventDoubleCast("Soul Fire", 1.3, on => Me.CurrentTarget, ret => (_haveMoltenCoreBuff || Me.CurrentTarget.HealthPercent < 25), UseKilJaedensCunningPassive),
                Spell.PreventDoubleCast("Shadow Bolt", 0.5, on => Me.CurrentTarget, ret => Me.CurrentTarget.HasMyAura("Corruption") && !DemonicFuryDump, UseKilJaedensCunningPassive),
            #endregion
                Spell.Cast("Life Tap", ret => Me.HealthPercent > 50 && (Me.ManaPercent < 20 || Common.CanLifeTap && Me.IsMoving())),
                Spell.Cast("Fel Flame", ret => Me.IsMoving() && !UseKilJaedensCunningPassive));
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                new Decorator(ret => _nearbyAoEUnitsNearMeCount >= 4 || _nearbyAoEUnitCount >= 4, HandleAoEHighUnitCount()),
                new Decorator(ret => _nearbyAoEUnitsNearMeCount < 4 || _nearbyAoEUnitCount < 4, HandleAoELowUnitCount())
                );
        }

        private static Composite HandleAoELowUnitCount()
        {
            return new PrioritySelector(
                new Decorator(ret => Me.HasAura("Hellfire"), CancelHellfireAura()),
                HandleShapeShifting(false),
                Spell.PreventDoubleCast("Felstorm", 45, ret => NeedPetFelStorm),
                Spell.PreventDoubleCast("Shadow Bolt", 0.5, on => Me.CurrentTarget, ret => Me.HasAura("Metamorphosis") && Me.CurrentTarget.HasAura("Corruption") && Spell.GetAuraTimeLeft("Corruption", Me.CurrentTarget) <= 5, true),
                Spell.MultiDoTCastHack("Metamorphosis: Doom", "Doom", Me, 40, 41, ret => Me.HasAura("Perfect Aim")),
                Spell.MultiDoTCastHack("Metamorphosis: Doom", "Doom", Me, 40, 3, ret => Me.HasAura("Metamorphosis")),
                Spell.PreventDoubleMultiDoT("Corruption", 1, Me, 40, 3, ret => !Me.HasAura("Metamorphosis")),

                HandleSingleTarget()

                );
        }

        private static Composite HandleAoEHighUnitCount()
        {
            return new PrioritySelector(
            new Decorator(ret => Me.HasAura("Hellfire") && ((NeedMetamorphosis && CanMetamorphosis) || NeedMultiCorruption || NeedCorruption), CancelHellfireAura()),
            new Decorator(ret => Me.HasAura("Hellfire"), new ActionAlwaysSucceed()),
            HandleShapeShifting(false),
            Spell.PreventDoubleCast("Felstorm", 45, ret => NeedPetFelStorm),

           new Decorator(ret => !Me.HasAura("Metamorphosis"),
                new PrioritySelector(
                        Spell.Cast("Corruption", ret => NeedCorruption),
                        Spell.PreventDoubleCast("Hand of Gul'dan", 2, ret => !TalentManager.HasGlyph("Hand of Gul'dan") && NeedHandofGuldan),
                        Spell.CastOnGround("Hand of Gul'dan", on => Me.CurrentTarget.Location, ret => TalentManager.HasGlyph("Hand of Gul'dan") && NeedHandofGuldan, true),
                        Spell.PreventDoubleMultiDoT("Corruption", 1, Me, 40, 3, ret => true),
                        Spell.PreventDoubleChannel("Hellfire", 1, true, ret => !Me.IsChanneling && Me.HealthPercent >= 60 && (!NeedMetamorphosis || !CanMetamorphosis) && !NeedMultiCorruption),
                        Spell.PreventDoubleChannel("Drain Life", 0.5, true, on => Me.CurrentTarget, ret => !Me.IsChanneling && TalentManager.HasTalent(3) && !NeedMetamorphosis && !CanMetamorphosis && !NeedMultiCorruption)
                        )),

            new Decorator(ret => Me.HasAura("Metamorphosis"),
                new PrioritySelector(
                        Spell.CastHack("Metamorphosis: Doom", "Doom", on => Me.CurrentTarget, ret => NeedDoom),
                        Spell.PreventDoubleCast("Hellfire", 2, ret => !Me.HasAura("Immolation Aura") && CurrentDemonicFury >= 500),
                        Spell.Cast("Carrion Swarm", ret => Me.IsFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance < 15 && TalentManager.HasGlyph("Carrion Swarm")),
                //Spell.CompositePerformance(HandleVoidRayRefresh(), "HandleVoidRayRefresh()"),
                        Spell.FaceAndCast("Fel Flame"/*Void Ray - Refresh corruption*/, on => BestVoidRayUnit(), ret => WarlockSettings.FaceForVoidRay && BestVoidRayUnit() != null),
                        Spell.MultiDoTCastHack("Metamorphosis: Doom", "Doom", Me, 40, 20, ret => true),
                //Spell.CompositePerformance(HandleVoidRayFiller(), "HandleVoidRayFiller()"),
                        Spell.FaceAndCast("Fel Flame"/*Void Ray - Any unit in range - Filler*/, on => BestVoidRayUnit(false), ret => WarlockSettings.FaceForVoidRay && BestVoidRayUnit(false) != null && !NeedMultiDoom),
                        Spell.Cast("Fel Flame" /* Filler when setting disabled */, ret => !WarlockSettings.FaceForVoidRay && Me.CurrentTarget.Distance <= 20 && !NeedMultiDoom),
                        Spell.PreventDoubleChannel("Drain Life", 0.5, true, on => Me.CurrentTarget, ret => !Me.IsChanneling && TalentManager.HasTalent(3) && !NeedMultiDoom)
                    )),


            Spell.Cast("Life Tap", ret => Me.HealthPercent > 50 && (Me.ManaPercent < 20 || Common.CanLifeTap && Me.IsMoving()))
            );

        }

        private static Composite HandleVoidRayRefresh()
        {
            return new PrioritySelector(
                Spell.FaceAndCast("Fel Flame"/*Void Ray - Refresh corruption*/, on => BestVoidRayUnit(), ret => WarlockSettings.FaceForVoidRay && BestVoidRayUnit() != null)
                );
        }

        private static Composite HandleVoidRayFiller()
        {
            return new PrioritySelector(
                Spell.FaceAndCast("Fel Flame"/*Void Ray - Any unit in range - Filler*/, on => BestVoidRayUnit(false), ret => WarlockSettings.FaceForVoidRay && BestVoidRayUnit(false) != null && !NeedMultiDoom)
                );
        }

        private static Composite HandleShapeShifting(bool singleTarget = true)
        {
            return new PrioritySelector(
                 CooldownTracker.PreventDoubleCast("Metamorphosis", 2, ret => singleTarget && NeedMetamorphosis && CanMetamorphosis),
                 CooldownTracker.PreventDoubleCast("Metamorphosis", 2, ret => !singleTarget && NeedMetamorphosis && CanMetamorphosis && (!NeedMultiCorruption || DemonicFuryDump)),
                 CancelMetamorphosisAura(ret => ((singleTarget && CancelMetamorphosis) || (!singleTarget && CancelMetamorphosisAoE))));
        }

        #endregion Standard Rotation

        #region Tank Mode
        private static Composite HandleTankMode()
        {
            return new PrioritySelector(
                                   Spell.PreventDoubleCast("Dark Apotheosis", 1, ret => !Me.HasAura("Dark Apotheosis")),
                                   Spell.PreventDoubleCast("Hellfire", 1, ret => !Me.HasAura("Immolation Aura") && CurrentDemonicFury >= 900 && _nearbyAoEUnitsNearMeCount >= 1),
                                   Spell.PreventDoubleCast("Curse of the Elements", 1, ret => !Me.HasAura("Aura of the Elements") && _nearbyAoEUnitsNearMeCount >= 1), // Don't cast if we don't have anyone near. 
                                   Spell.PreventDoubleCast("Corruption", 1, ret => !Me.CurrentTarget.HasMyAura("Corruption") || Spell.GetAuraTimeLeft("Corruption", Me.CurrentTarget) < 1.89),
                                   Spell.PreventDoubleCast("Hand of Gul'dan", 2, ret => !TalentManager.HasGlyph("Hand of Gul'dan") && !Me.HasAura("Metamorphosis") && !Me.CurrentTarget.MovementInfo.IsMoving && !Me.CurrentTarget.HasMyAura("Shadowflame")),
                                   Spell.CastOnGround("Hand of Gul'dan", on => Me.CurrentTarget.Location, ret => TalentManager.HasGlyph("Hand of Gul'dan") && !Me.HasAura("Metamorphosis") && !Me.CurrentTarget.MovementInfo.IsMoving && !Me.CurrentTarget.HasMyAura("Shadowflame")),
                                   Spell.PreventDoubleCast("Twilight Ward", 1, ret => !Me.HasAura("Fury Ward") && CurrentDemonicFury >= 220 && Me.HealthPercent <= 90),  // Stay Alive!
                                   Spell.PreventDoubleCast("Soul Fire", 1.3, ret => !Me.IsMoving && _haveMoltenCoreBuff && CurrentDemonicFury > 385/*Me.HasAura("Molten Core")*/),
                                   Spell.PreventDoubleCast("Shadow Bolt", 3, ret => CurrentDemonicFury < 940),
                                   Spell.Cast("Fel Flame", ret => Me.IsFacing(Me.CurrentTarget) && CurrentDemonicFury > 265), // filler when facing
                                   Spell.Cast("Life Tap", ret => Me.ManaPercent < 30 && Me.HealthPercent > 50) // filler when we aren't facing
               );
        }

        private static Composite HandleAoeTankMode()
        {
            return new PrioritySelector(
                       Spell.PreventDoubleCast("Dark Apotheosis", 1, ret => !Me.HasAura("Dark Apotheosis")),
                       Spell.PreventDoubleCast("Curse of Enfeeblement", 1, ret => !Me.HasAura("Aura of Enfeeblement") && _nearbyAoEUnitsNearMeCount >= 1), // Don't cast if we don't have anyone near. Only has 30s duration.

                       // Stay Alive!
                       Spell.PreventDoubleCast("Twilight Ward", 1, ret => !Me.HasAura("Fury Ward") && CurrentDemonicFury >= 225 && Me.HealthPercent <= 90),

                       // AoE
                       Spell.PreventDoubleCast("Wrathstorm", 45, ret => NeedPetFelStorm),
                       Spell.PreventDoubleCast("Hellfire", 1, ret => !Me.HasAura("Immolation Aura") && CurrentDemonicFury >= 500 && _nearbyAoEUnitsNearMeCount >= 1), //9s duration @ 25 fury per sec = 225 total.
                       Spell.PreventDoubleCast("Hand of Gul'dan", 2, ret => !TalentManager.HasGlyph("Hand of Gul'dan") /*&& !Me.CurrentTarget.MovementInfo.IsMoving*/ && !Me.CurrentTarget.HasMyAura("Shadowflame")),
                       Spell.CastOnGround("Hand of Gul'dann", on => Me.CurrentTarget.Location, ret => TalentManager.HasGlyph("Hand of Gul'dan") /*&& !Me.CurrentTarget.MovementInfo.IsMoving*/ && !Me.CurrentTarget.HasMyAura("Shadowflame"), true),
                       Spell.Cast("Carrion Swarm", ret => Me.IsFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance < 15 && TalentManager.HasGlyph("Carrion Swarm")), // CS has a 15 yard range
                       Spell.PreventDoubleMultiDoT("Corruption", 1, Me, 35, 3, ret => true),

                       // Single Target Nuke
                       Spell.PreventDoubleCast("Soul Fire", 1.3, ret => !Me.IsMoving && _haveMoltenCoreBuff && CurrentDemonicFury > 385/*Me.HasAura("Molten Core")*/),

                       // Single Target - Generate Fury
                       Spell.PreventDoubleCast("Shadow Bolt", 3, ret => CurrentDemonicFury < 940),
                       Spell.Cast("Fel Flame", ret => Me.IsFacing(Me.CurrentTarget) && CurrentDemonicFury > 265), // filler, but only works if we're facing target
                       Spell.Cast("Life Tap", ret => Me.ManaPercent < 30 && Me.HealthPercent > 60) // filler when we aren't facing
               );
        }
        #endregion Tank Mode

        #region Cancel Auras

        private static Composite CancelMetamorphosisAura(Selection<bool> reqs = null)
        {
            return new Decorator(ret => ((reqs != null && reqs(ret)) || (reqs == null)),
            new Action(delegate
            {
                Logger.InfoLog("Cancelling Metamorphosis Form");
                Me.CancelAura("Metamorphosis"); return RunStatus.Failure;
            }));
        }

        private static Composite CancelDarkApothesisAura()
        {
            return new Action(delegate
            {
                Logger.InfoLog("Cancelling Dark Apotheosis");
                Me.CancelAura("Dark Apotheosis"); return RunStatus.Failure;
            });
        }

        private static Composite CancelHellfireAura()
        {
            return new Action(delegate
            {
                Logger.InfoLog("Cancelling Hellfire");
                Me.CancelAura("Hellfire"); return RunStatus.Failure;
            });
        }


        #endregion

        #region Units

        private static WoWUnit BestVoidRayUnit(bool onlyForRefresh = true)
        {
            // Get a unit that needs corruption refreshing
            var bestUnit = Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 20)
                        .Where(x => x != null && x.CachedHasAura("Corruption") && x.CachedGetAuraTimeLeft("Corruption", true) <= 15000)
                        .OrderBy(x => x.CachedGetAuraTimeLeft("Corruption", true))
                        .FirstOrDefault();

            // Can't refresh corruption, pick our current target if it's close enough.
            if (bestUnit == null && !onlyForRefresh)
            {
                if (Me.CurrentTarget != null && Me.CurrentTarget.Distance <= 20)
                {
                    return Me.CurrentTarget;
                }

                // If current target is no good, pick one thats close enough.
                bestUnit = Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 20)
                    .FirstOrDefault(x => x != null);
            }

            return bestUnit;

        }

        #endregion

        #region Conditions

        private static int _nearbyAoEUnitCount;
        private static int _nearbyAoEUnitsNearMeCount;
        private static bool _haveMoltenCoreBuff;
        private static Composite SetCounts
        {
            get
            {
                return new Action(delegate
                {
                    _nearbyAoEUnitCount = Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count();
                    _nearbyAoEUnitsNearMeCount = Unit.CachedNearbyAttackableUnits(Me.Location, 30).Count();
                    _haveMoltenCoreBuff = Spell.StackCount(Me,"Molten Core")>0; //Lua.PlayerBuffTimeLeft("Molten Core") > 2;
                    return RunStatus.Failure;
                });
            }
        }

        private static double CurrentDemonicFury { get { return Me.GetPowerInfo(WoWPowerType.DemonicFury).CurrentI;/* Lua.PlayerUnitPower("SPELL_POWER_DEMONIC_FURY");*/} }
        private static bool NeedDoom 
        {
            get
            {
                return Me.HasAura("Metamorphosis") 
                    && (!Me.CurrentTarget.HasAura("Doom") ||
                        Me.CurrentTarget.HasAura("Doom") && Spell.GetAuraTimeLeft("Doom", Me.CurrentTarget) < 29 ||
                        Me.HasAnyAura("Dark Soul: Knowledge", "Perfect Aim") && Me.CurrentTarget.HasAura("Doom") && Spell.GetAuraTimeLeft("Doom", Me.CurrentTarget) < 42);
            } 
        }
        private static bool DemonicFuryDump { get { return CurrentDemonicFury >= WarlockSettings.MetamorphosisFuryValue; } }
        private static bool CanMetamorphosis { get { return !Me.HasAura("Metamorphosis") && !CooldownTracker.SpellOnCooldown("Metamorphosis") && Me.CurrentTarget.HasAura("Corruption"); } }

        private static bool NeedMetamorphosis { get { 
            return   Me.HasAnyAura("Dark Soul: Knowledge", "Perfect Aim") || 
                    !Me.CurrentTarget.HasMyAura("Doom") || 
                     DemonicFuryDump || 
                     Me.CurrentTarget.HasAura("Corruption") && Spell.GetAuraTimeLeft("Corruption", Me.CurrentTarget) <= 4.5; } }
        private static bool CancelMetamorphosis { get { return Me.HasAura("Metamorphosis") && ((!Me.HasAnyAura("Dark Soul: Knowledge", "Perfect Aim") && CurrentDemonicFury <= WarlockSettings.CancelMetamorphosisValue && Me.CurrentTarget.HasMyAura("Doom")) || !Me.CurrentTarget.HasAura("Corruption")); } }
        private static bool CancelMetamorphosisAoE { get { return CancelMetamorphosis && !Me.HasAura("Immolation Aura") && !NeedMultiDoom; } }

        private static bool NeedImpSwarm { get { return TalentManager.HasGlyph("Imp Swarm") && 
                                                        (Me.HasAura("Dark Soul: Knowledge") || Me.CurrentTarget.HealthPercent<25); } }
        private static bool NeedCorruption 
        {
            get
            {
                return Me.CurrentTarget != null && 
                    (!Me.CurrentTarget.HasAura("Corruption") || 
                     (Me.CurrentTarget.HasAura("Corruption") && Spell.GetAuraTimeLeft("Corruption", Me.CurrentTarget) <= 3) ||
                     DoTTracker.NeedRefresh(Me.CurrentTarget,SpellBook.Corruption)
                     );
            } 
        }
        private static bool NeedHandofGuldan { get { return !Me.HasAura("Metamorphosis") && !Me.CurrentTarget.MovementInfo.IsMoving && !Me.CurrentTarget.HasAura("Shadowflame"); } }
        private static bool NeedDarkSoulKnowledge { get { return WarlockSettings.UseDarkSoulKnowledge; } }
        private static bool NeedPetFelStorm { get { return Me.GotAlivePet && Unit.CachedNearbyAttackableUnits(Me.Pet.Location, 8).Count() >= WarlockSettings.PetFelstormCount && SpellManager.CanCast("Command Demon"); } }
        private static bool UseKilJaedensCunningPassive { get { return TalentManager.HasTalent(17); } }
        private static bool NeedMultiCorruption { get { return Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 40).Any(x => !x.HasAura("Corruption")); } }
        private static bool NeedMultiDoom { get { return Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 40).Any(x => !x.HasAura("Doom")); } }
        private static bool NeedAoEMode
        {
            get
            {
                return PRSettings.Instance.UseAoEAbilities && (
                    _nearbyAoEUnitsNearMeCount >= WarlockSettings.AoECount ||
                       _nearbyAoEUnitCount >= WarlockSettings.AoECount);
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
            get { return WoWSpec.WarlockDemonology; }
        }

        public override string Name
        {
            get { return "Demonology Warlock"; }
        }

        internal override string Help
        {
            get
            {
                return
@"
-----------------------------------------------
Special Key: When 'Glyph of Demon Hunting' is 
             active, special key will
             enable/disable tanking rotation.
            
Recommended Spec: http://www.wowhead.com/talent#oxJi|oziomk
        
Latest Updates [26-Jun-2013]
- Added experimental setting to reduce in game lag.

Enjoy! ~ Millz
-----------------------------------------------
";
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => (WarlockSettings.EnableLagFix && SpellManager.GlobalCooldown) || HotKeyManager.IsPaused, new ActionAlwaysSucceed()),

                    CachedUnits.Pulse,
                    HandleDefensiveCooldowns(),
                    Common.HandleCommonAbilities(),
                    new Decorator(ret => !SpellManager.GlobalCooldown,
                        new PrioritySelector(
                                Racials.UseRacials(),
                                EncounterSpecific.HandleActionBarInterrupts(),
                                Item.HandleItems()
                            )),
                    SetCounts,
                    // Auto Mode: Standard
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto && (!WarlockSettings.TankMode || !TalentManager.HasGlyph("Demon Hunting")),
                                  new PrioritySelector(
                                      new Decorator(ret => Me.HasAura("Dark Apotheosis"), CancelDarkApothesisAura()),
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && NeedAoEMode, HandleAoeCombat()),
                                      new Decorator(ret => Me.CurrentTarget != null && !NeedAoEMode, HandleSingleTarget()))),

                    // Auto Mode: Tank
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto && (WarlockSettings.TankMode && TalentManager.HasGlyph("Demon Hunting")),
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && NeedAoEMode, HandleAoeTankMode()),
                                      new Decorator(ret => Me.CurrentTarget != null && !NeedAoEMode, HandleTankMode()))),

                    // Semi-Auto Mode: Standard
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto && (!WarlockSettings.TankMode || !TalentManager.HasGlyph("Demon Hunting")),
                                  new PrioritySelector(
                                      new Decorator(ret => Me.HasAura("Dark Apotheosis"), CancelDarkApothesisAura()),
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && NeedAoEMode, HandleAoeCombat()),
                                      new Decorator(ret => Me.CurrentTarget != null && !NeedAoEMode, HandleSingleTarget()))),

                    // Semi-Auto Mode: Tank
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto && (WarlockSettings.TankMode && TalentManager.HasGlyph("Demon Hunting")),
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && NeedAoEMode, HandleAoeTankMode()),
                                      new Decorator(ret => Me.CurrentTarget != null && !NeedAoEMode, HandleTankMode()))),

                    // Hotkey Mode
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => Me.HasAura("Dark Apotheosis") && !HotKeyManager.IsSpecialKey, CancelDarkApothesisAura()),
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe && HotKeyManager.IsSpecialKey && TalentManager.HasGlyph("Demon Hunting"), HandleAoeTankMode()),
                                      new Decorator(ret => !HotKeyManager.IsAoe && HotKeyManager.IsSpecialKey && TalentManager.HasGlyph("Demon Hunting"), HandleTankMode()),
                                      new Decorator(ret => HotKeyManager.IsAoe && !HotKeyManager.IsSpecialKey, HandleAoeCombat()),
                                      HandleSingleTarget()
                                      )));

            }
        }

        public override Composite PVPRotation
        {
            get { return PVERotation; }
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
        internal override void OnPulse()
        {
            if (!DoTTracker.Initialized)
            {
                DoTTracker.Initialize();
            }
        }
        public override Composite Medic
        {
            get { return new PrioritySelector(); }
        }

        #endregion
    }
}
