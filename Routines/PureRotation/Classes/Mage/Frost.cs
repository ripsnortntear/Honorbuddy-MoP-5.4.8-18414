#region Revision info
/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Mage/Frost.cs $
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
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace PureRotation.Classes.Mage
{
    [UsedImplicitly]
    class Frost : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }
        private static MageSettings MageSettings { get { return PRSettings.Instance.Mage; } }
        private static int _blizzardMobCount;
        private static WoWPoint _blizzardLocation;
        private static int _unitsNearTarget;
        //private static int _unitsInArcaneExplosionRange;

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Ice Ward", on => Me, ret => !Me.IsCrowdControlled() && NeedIceWard && Me.CurrentTarget!=null && Me.CurrentTarget.IsWithinMeleeRange && MageSettings.UceIceWard),
                Spell.Cast("Frost Nova", ret => !Me.IsCrowdControlled() && MageSettings.UseFrostNova && Me.CurrentTarget.IsWithinMeleeRange && !Me.CurrentTarget.IsBoss && !Me.CurrentTarget.HasAnyAura(SpellBook.DeepFreeze, SpellBook.FrostNova, 33395 /* Pet Freeze */)),
                Spell.Cast("Cold Snap", ret => NeedColdSnap),
                //Spell.Cast("Flameglow", ret => NeedFlameglow),
                Common.HandleCommonDefensiveCooldowns() // Ice Block / Mage Ward / Ice Barrier / Temporal Shield
             );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                Common.HandleCommonOffensiveCooldowns(),
                Spell.Cast(12472, ret => !Hasted && !TalentManager.HasGlyph("Icy Veins") && !Me.IsMoving && Me.CurrentTarget.IsBoss()), // Icy Veins (unglyphed - 12472)
                Spell.Cast(131078, ret => TalentManager.HasGlyph("Icy Veins") && NeedIcyVeins && Me.CurrentTarget.IsBoss()), // Glyped version of icy veins - 131078
                Spell.Cast("Presence of Mind", on => Me, ret => NeedPresenceOfMind),
                Spell.PreventDoubleCast("Alter Time", 6.5, on => Me, ret => MageSettings.EnableAlterTime &&

                       (Me.HasAura("Icy Veins") && Me.HasAllAuras("Fingers of Frost", "Brain Freeze"))
                    || (Me.HasAura("Icy Veins") && Spell.GetAuraTimeLeft("Icy Veins") < 14 && Me.HasAnyAura("Fingers of Frost", "Brain Freeze"))
                    || (Me.HasAura("Icy Veins") && Spell.GetAuraTimeLeft("Icy Veins") < 9)
                    , true),

                Spell.PreventDoubleCast("Mirror Image", ThrottleTime, on => Me, ret => MageSettings.EnableMirrorImage && Me.CurrentTarget.IsBoss(), true),
                Spell.PreventDoubleCast("Ice Floes", ThrottleTime, on => Me, ret => NeedIceFlows, true)

                );
        }

        private static Composite HandleSingleTarget()
        {

            return new PrioritySelector(
                Spell.PreventDoubleCast("Summon Water Elemental", 2, ret => !Me.GotAlivePet),

                new Decorator(ret => !HaveFingersOfFrost && !HaveBrainFreeze, Common.HandleSingleTargetMageBombs()),

                Spell.Cast("Frozen Orb", ret => NeedFrozenOrb),
                Spell.PreventDoubleCastPetActionOnLocation("Freeze", 10, loc => Me.CurrentTarget.Location, ret => Me.CurrentTarget!=null && NeedPetFreeze),

                new Decorator(ret => MageSettings.MultiDoTSingleTarget, Common.HandleBombMultiDoT()),

                Spell.PreventDoubleCast("Frostbolt", ThrottleTime, on => Me.CurrentTarget, ret => !HaveFingersOfFrost && !HaveBrainFreeze && !Me.CurrentTarget.IsDead && Me.CurrentTarget.Distance <= 40, Me.HasAnyAura("Ice Floes", "Presence of Mind")),
                
                // Filler - Moving
                Spell.PreventDoubleCast("Blazing Speed", ThrottleTime, on => Me.CurrentTarget, ret => NeedBlazingSpeed, true),
                Spell.PreventDoubleCast("Fire Blast", ThrottleTime, on => Me.CurrentTarget, ret => Me.IsMoving(), true),
                Spell.PreventDoubleCast("Ice Lance", ThrottleTime, on => Me.CurrentTarget, ret => Me.IsMoving(), true)

                );
        }

        private static Composite HandleProcs()
        {
            return new PrioritySelector(
                Spell.PreventDoubleCast("Frostfire Bolt", 0.3, on => Me.CurrentTarget, ret => !Me.IsCrowdControlled() && HaveBrainFreeze, true),
                Spell.PreventDoubleCast("Ice Lance", 0.3, on => Me.CurrentTarget, ret => !Me.IsCrowdControlled() && (HaveFingersOfFrost || Me.CurrentTarget.HasAnyAura(SpellBook.DeepFreeze, SpellBook.FrostNova, 33395 /* Pet Freeze */)), true)
                );
        }

        private static Composite HandleBlizzardCancel()
        {
            return new PrioritySelector(
                new Action(delegate
                {
                    if (Me.IsChanneling && Me.ChanneledCastingSpellId == SpellBook.Blizzard)
                    {
                        // We're channeling
                        if (_blizzardLocation == WoWPoint.Empty)
                        {
                            // We haven't stored the location, so store it.
                            //Logger.DebugLog("Store Location..");
                            _blizzardLocation = Me.CurrentTarget.Location;
                            //Logger.DebugLog("Stored Location [{0}]", _blizzardLocation);
                        }
                        else
                        {
                            //Logger.DebugLog("Get unit count..");
                            // We've already got the location, check how many units are there

                            try
                            {
                                var countList = Unit.CachedNearbyAttackableUnits(_blizzardLocation, 9);
                                _blizzardMobCount = countList != null ? countList.Count() : 99;
                            }
                            catch (Exception ex)
                            {
                                Logger.DebugLog("Unit count failed with: {0}", ex);
                                _blizzardMobCount = 99;
                            }

                            if (HaveBrainFreeze)
                            {
                                Logger.InfoLog("Brain Freeze - Cancelling");
                                SpellManager.StopCasting();
                            }

                            if (HaveFingersOfFrost)
                            {
                                Logger.InfoLog("Fingers of Frost - Cancelling");
                                SpellManager.StopCasting();
                            }

                            if (_blizzardMobCount <= 4)
                            {
                                // Less than 4 units, cancel..
                                Logger.InfoLog("Units under blizzard is now [{0} (Need 5)] - Cancelling", _blizzardMobCount);
                                SpellManager.StopCasting();
                            }
                            else
                            {
                                Logger.InfoLog("[{0}] units in our blizzard, keep channelling...", _blizzardMobCount);
                                // 4 or more units. Return success to prevent tree traverse
                                return RunStatus.Success;
                            }
                        }
                    }

                    if (_blizzardLocation != WoWPoint.Empty && !Me.IsChanneling)
                    {
                        // We have hurricane location stored, but we're not channeling. Null it.
                        _blizzardLocation = WoWPoint.Empty;
                    }

                    return RunStatus.Failure;
                })
);

        }

        #region AoE
        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                new Decorator(ret => _unitsNearTarget <= 3, AoELowUnitCount()),
                new Decorator(ret => _unitsNearTarget >= 4, AoEHighUnitCount())
                );
        }

        private static Composite AoELowUnitCount()
        {
            return new PrioritySelector(
                Spell.PreventDoubleCast("Frozen Orb", ThrottleTime, ret => NeedFrozenOrb),
                Spell.PreventDoubleCast("Fire Blast", ThrottleTime, ret => Me.CurrentTarget.HasMyAura("Living Bomb") && TalentManager.HasGlyph("Inferno Blast")),
                Spell.CastOnGround("Flamestrike", loc => Me.CurrentTarget.Location, ret => !Me.CurrentTarget.IsMoving),
                Common.HandleBombMultiDoT(),
                HandleSingleTarget()
            );
        }

        private static Composite AoEHighUnitCount()
        {
            return new PrioritySelector(

                Spell.PreventDoubleCast("Frozen Orb", ThrottleTime, ret => NeedFrozenOrb),
                new Decorator(ret => Me.CurrentTarget.HasMyAura("Frost Bomb") && Spell.GetMyAuraTimeLeft("Frost Bomb", Me.CurrentTarget) <= 1.5, HandleFrostBombTactic()),
                Spell.CastOnGround("Flamestrike", loc => Me.CurrentTarget.Location, ret => !Me.CurrentTarget.IsMoving),
                Spell.PreventDoubleCast("Fire Blast", ThrottleTime, ret => Me.CurrentTarget.HasMyAura("Living Bomb") && TalentManager.HasGlyph("Inferno Blast")),
                Common.HandleBombMultiDoT(),

                //Spell.PreventDoubleCast("Arcane Explosion", ThrottleTime, ret => _unitsInArcaneExplosionRange >= _unitsNearTarget),
                Spell.PreventDoubleCastOnGround("Blizzard", ThrottleTime, loc => Me.CurrentTarget.Location, ret => _unitsNearTarget >= 5 && !Me.IsMoving()),
                HandleSingleTarget()
            );
        }
        #endregion

        #region statics

        

        private static bool HaveBrainFreeze { get { return Me.ActiveAuras.ContainsKey("Brain Freeze"); } }
        private static bool HaveFingersOfFrost { get { return Me.ActiveAuras.ContainsKey("Fingers of Frost"); } }
        private static bool Hasted { get { return Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Time Warp"); } }
        private static bool NeedFrostBoltDebuff { get { return Me.CurrentTarget != null && (Spell.StackCount(Me.CurrentTarget, "Frostbolt") < 3); } }
        private static bool NeedIcyVeins { get { return /*!NeedFrostBoltDebuff &&*/ !Me.IsMoving && Me.HasAnyAura("Brain Freeze", "Fingers of Frost"); } }
        private static bool NeedPetFreeze { get { return !Me.CurrentTarget.IsBoss() && MageSettings.UsePetFreeze && (Spell.GetAuraStackCount("Fingers of Frost") < 2); } }
        private static bool Invisible { get { return Me.HasAura(66) || Me.HasAura(110959); } }
        private static bool NeedFrozenOrb { get { return
            MageSettings.UseFrozenOrb && 
            (Me.CurrentTarget.HealthPercent >= 20 || Me.CurrentTarget.IsBoss()) && !HaveFingersOfFrost &&

            // Invocation
            ((TalentManager.HasTalent(16) && Me.HasAura(116257) && Spell.GetAuraTimeLeft(116257, Me) > 12)
            || !TalentManager.HasTalent(16));
        
        } }

        #region Tier 1 Talents
        private static bool NeedPresenceOfMind { get { return TalentManager.HasTalent(1) && !Me.HasAura("Alter Time") && (Me.IsMoving || Me.HasAura("Icy Veins")) && MageSettings.EnablePresenceofMind; } }
        //private static bool NeedScorch { get { return TalentManager.HasTalent(2) && Me.IsMoving; } }
        private static bool NeedBlazingSpeed { get { return TalentManager.HasTalent(2) && Me.IsMoving(200); } }
        private static bool NeedIceFlows { get { return TalentManager.HasTalent(3) && Me.IsMoving(200); } }
        #endregion

        #region Tier 2 Talents
        private static bool NeedFlameglow { get { return TalentManager.HasTalent(5) && Me.HealthPercent < 75; } }
        #endregion

        #region Tier 3 Talents
        private static bool NeedIceWard { get { return TalentManager.HasTalent(8); } } // Setting
        #endregion

        #region Tier 4 Talents
        private static bool NeedColdSnap { get { return TalentManager.HasTalent(12) && Spell.SpellOnCooldown(45438) /*Ice Block*/ && !Me.ActiveAuras.ContainsKey("Hypothermia") && Me.HealthPercent < 40; } }
        #endregion

        #region Glyph Modifiers
        public static int ArcaneExplosionRadius
        {
            get
            {
                if (TalentManager.HasGlyph("Arcane Explosion"))
                {
                    return 15;
                }

                return 10;
            }
        }

        #endregion

        #endregion

        #region Super Clever Frost Bomb Tactic

        private static Composite HandleFrostBombTactic()
        {
            //WoWUnit iceWardTarget = null;
            return new PrioritySelector(//ctx => iceWardTarget = HealManager.GetTank,
                Spell.Cast("Frost Nova", ret => Me.CurrentTarget.HasMyAura("Frost Bomb") && Me.CurrentTarget.Distance <= 12),
                Spell.PreventDoubleCastPetActionOnLocation("Freeze", 10, loc => Me.CurrentTarget.Location, ret => Me.CurrentTarget.HasMyAura("Frost Bomb") && MageSettings.UsePetFreeze)
                );
        }

        #endregion

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.MageFrost; }
        }

        public override string Name
        {
            get { return "Frost Mage"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    HandleBlizzardCancel(),
                    new Decorator(ret => (MageSettings.EnableLagFix && SpellManager.GlobalCooldown) || HotKeyManager.IsPaused || Invisible || (Me.IsChanneling && Me.ChanneledCastingSpellId == SpellBook.Evocation) || Me.CurrentTarget == null || Me.HasAura("Ice Block"), new ActionAlwaysSucceed()),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    CachedUnits.Pulse,
                    Racials.UseRacials(),
                    HandleDefensiveCooldowns(), // Stay Alive! 
                    //DispelManager.CreateDispelBehavior(),
                    Item.HandleItems(),         // Pop Trinkets, Drink potions...  
                    Common.HandleCommon(),
                    new Action(delegate
                        {
                            try
                            {
                                //var unitsNearUs = Unit.CachedNearbyAttackableUnits(Me.Location, ArcaneExplosionRadius); 
                                IEnumerable<WoWUnit> countList = null;
                                if (Me.CurrentTarget != null)
                                {
                                    countList = Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 9);
                                }
                                else
                                {
                                    countList = Unit.CachedNearbyAttackableUnits(Me.Location, 9);
                               
                                }
                                //_unitsInArcaneExplosionRange = unitsNearUs != null ? unitsNearUs.Count() : 0;
                                _unitsNearTarget = countList != null ? countList.Count() : 1;
                            }
                            catch (Exception ex)
                            {
                                Logger.DebugLog("Unit detection failed with: {0}", ex);
                                _unitsNearTarget = 1;
                                //_unitsInArcaneExplosionRange = 0;
                            }
                            return RunStatus.Failure;
                        }),
                    HandleProcs(), // Ice Lance / Frostfire Bolt

                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && !Me.IsCrowdControlled() && Me.CurrentTarget != null && !EncounterSpecific.HasBadAura, HandleOffensiveCooldowns()),
                                      new Decorator(ret => !Me.IsCrowdControlled() && Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && _unitsNearTarget >= MageSettings.AoECount, HandleAoeCombat()),
                                      new Decorator(ret => !Me.IsCrowdControlled() && Me.CurrentTarget != null && _unitsNearTarget < MageSettings.AoECount, HandleSingleTarget())
                                      )),

                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => !Me.IsCrowdControlled() && HotKeyManager.IsSpecialKey, Spell.Cast("Time Warp")),
                                      new Decorator(ret => !Me.IsCrowdControlled() && HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => !Me.IsCrowdControlled() && Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && _unitsNearTarget >= MageSettings.AoECount, HandleAoeCombat()),
                                      new Decorator(ret => !Me.IsCrowdControlled() && Me.CurrentTarget != null && _unitsNearTarget < MageSettings.AoECount, HandleSingleTarget())
                                      )),

                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => !Me.IsCrowdControlled() && HotKeyManager.IsSpecialKey, Spell.Cast("Time Warp")),
                                      new Decorator(ret => !Me.IsCrowdControlled() && HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => !Me.IsCrowdControlled() && Me.CurrentTarget != null && HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !Me.IsCrowdControlled() && Me.CurrentTarget != null && !HotKeyManager.IsAoe, HandleSingleTarget()))));

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
                        Spell.PreventDoubleCast("Summon Water Elemental", 2, ret => !Me.GotAlivePet),
                        Common.HandleCommonPreCombat()
                        ));
            }
        }


        internal override string Help
        {
            get
            {
                return @"
---------------------------------------------------------
Special Key: Will activate Time Warp

Recommended Talents: http://tinyurl.com/FrostMageTalents

Latest Updates [26-Jun-2013]
- Added experimental setting to reduce in game lag.

Enjoy! --Millz
---------------------------------------------------------
";
            }
        }
        #endregion
    }
}
