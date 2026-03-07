#region Revision info
/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Shaman/Elemental.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade: $
 */
#endregion

using System;
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
using Action = Styx.TreeSharp.Action;
namespace PureRotation.Classes.Shaman
{
    [UsedImplicitly]
    internal class Elemental : RotationBase
    {
        private static ShamanSettings ShamanSettings
        {
            get { return PRSettings.Instance.Shaman; }
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Item.UseBagItem(5512, ret => Me.HealthPercent <= 30, "Healthstone [" + Me.HealthPercent + "% HP]"),
                Spell.Cast("Healing Tide Totem",
                           ret =>
                           TalentManager.HasTalent(13) && Me.HealthPercent <= ShamanSettings.SelfHealingPercentage),
                Spell.Cast("Ancestral Guidance",
                           ret =>
                           TalentManager.HasTalent(14) && Me.HealthPercent <= ShamanSettings.SelfHealingPercentage),
                Spell.Cast("Grounding Totem", ret => Me.CurrentTarget.IsTargetingMeOrPet && Me.CurrentTarget.IsCasting),
                Spell.Cast("Astral Shift",
                           ret => TalentManager.HasTalent(3) && Me.HealthPercent <= ShamanSettings.SelfHealingPercentage),
                Spell.Cast("Stone Bulwark Totem",
                           ret => TalentManager.HasTalent(2) && Me.HealthPercent <= ShamanSettings.SelfHealingPercentage),

                Spell.CastOnGround("Healing Rain", on => Me.Location, ret => Me.HealthPercent < ShamanSettings.SelfHealingPercentage && PRSettings.Instance.EnableSelfHealing),
                Spell.Cast("Healing Surge", on => Me, ret => Me.HealthPercent < ShamanSettings.SelfHealingPercentage && PRSettings.Instance.EnableSelfHealing)
                
                );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Ancestral Swiftness", ret => TalentManager.HasTalent(11)),
                Spell.Cast("Elemental Mastery", ret => TalentManager.HasTalent(10) && !Hasted),
                Spell.Cast("Stormlash Totem", ret => Hasted && !Totems.Exist(WoWTotemType.Air) && !Me.HasAura("Stormlash")),
                Spell.Cast("Spiritwalker's Grace", ret => ShamanSettings.UseSpiritwalkersGrace && Me.IsMoving && ((NeedLavaBurst || !Me.HasAura("Lava Surge")) || NeedLavaBeam || NeedChainLightning || (NeedLightningBolt && !TalentManager.HasGlyph("Unleashed Lightning")))),
                Spell.Cast("Fire Elemental Totem", ret => (Hasted || TalentManager.HasGlyph("Fire Elemental Totem")) && Me.CurrentTarget.Distance <= 40 && ShamanSettings.UseFireElemental),
                Spell.Cast("Earth Elemental Totem", ret => NeedEarthElemental),
                Spell.PreventDoubleCast("Ascendance", 2, ret => Me.CurrentTarget != null && Me.CurrentTarget.Distance <= 40 && !Me.HasAura(114050) && ShamanSettings.UseAscendance && ((Me.CurrentTarget.IsBoss() && ShamanSettings.EleAscendanceOnBoss) || !ShamanSettings.EleAscendanceOnBoss))
                // in range, on cooldown
                );
        }

        private static Composite HandleCommon()
        {
            return new PrioritySelector(
                Spell.Cast("Shamanistic Rage", ret => Me.ManaPercent < 10 || Me.HealthPercent < 60)
                );
        }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                Spell.PreventDoubleCast("Searing Totem", 0.5, ret => NeedSearingTotem),
                Spell.PreventDoubleCast("Unleash Elements", 0.5, ret => NeedUnleashElements),
                Spell.PreventDoubleCast("Elemental Blast", 0.5, ret => NeedElementalBlast),
                Spell.PreventDoubleCast("Lava Burst", 0.5, ret => NeedLavaBurst),
                Spell.PreventDoubleCast("Lava Burst", 0.5, on => Me.CurrentTarget, ret => NeedLavaBurstMoving, true),
                Spell.PreventDoubleCast("Flame Shock", 0.5, ret => NeedFlameShock), // Flame shock after LvB to try and get double benefit from UE buff.
                Spell.PreventDoubleCast("Earth Shock", 0.5, ret => NeedEarthShock),
                Spell.PreventDoubleCast("Thunderstorm", 0.5, ret => NeedThunderstorm), // Generate 15% Mana
                Spell.PreventDoubleCast("Lightning Bolt", 0.2, on => Me.CurrentTarget, ret => NeedLightningBolt),
                Spell.PreventDoubleCast("Lightning Bolt", 0.2, on => Me.CurrentTarget, ret => Me.IsMoving() && !ShamanSettings.UseGhostWolf, true),
                Spell.PreventDoubleCast("Ghost Wolf", 1, ret => Me.IsMoving(500) && !Me.HasAura("Ghost Wolf") && ShamanSettings.UseGhostWolf)
                );
        }

        private static Composite HandleAoE()
        {
            return new PrioritySelector(
                Spell.Cast(114074, ret => NeedLavaBeam),
                Spell.PreventDoubleCast("Magma Totem", 1, ret => NeedMagmaTotem),
                Spell.PreventDoubleCast("Searing Totem", 1, ret => !NeedMagmaTotem && NeedSearingTotem),
                Spell.PreventDoubleCast("Lava Burst", 1, ret => _nearbyAoEUnitCount <= 2 && NeedLavaBurst),
                Spell.PreventDoubleCast("Lava Burst", 0.5, on => Me.CurrentTarget, ret => _nearbyAoEUnitCount <= 2 && NeedLavaBurstMoving, true),
                //new Decorator(ret => _nearbyAoEUnitCount <= 2, HandleMultiDoT(DebuffStandard)),
                Spell.PreventDoubleMultiDoT("Flame Shock", 1, Me, 35, 1, ret => _nearbyAoEUnitCount <= 2),
                Spell.CastOnGround("Earthquake", ret => Me.CurrentTarget.Location, ret => Me.CurrentTarget != null && _nearbyAoEUnitCount >= 4),
                Spell.Cast("Thunderstorm", ret => NeedThunderstorm),
                Spell.Cast("Chain Lightning", ret => NeedChainLightning),
                Spell.Cast("Lightning Bolt", ret => !NeedChainLightning),
                Spell.PreventDoubleCast("Lightning Bolt", 0.2, on => Me.CurrentTarget, ret => !NeedChainLightning && Me.IsMoving && (TalentManager.HasGlyph("Unleashed Lightning") || Me.HasAura("Spiritwalker's Grace")), true)
                );
        }

        
        #region MultiDoT Targets
        /*
        private static Composite HandleMultiDoT(string[] spellNames)
        {
            return new PrioritySelector(ctx => GetMultiDoTTarget(spellNames),
                        Spell.PreventDoubleCast("Flame Shock", 1, onUnit => ((WoWUnit)onUnit), ret => ret != null && !((WoWUnit)ret).HasMyAura("Flame Shock"))
                        );
        }

        private static WoWUnit GetMultiDoTTarget(string[] debuffs)
        {
            return Me.CurrentTarget != null ? Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 15).FirstOrDefault(x => !x.HasAllAuras(debuffs) && (x.Distance < 40) && !x.IsCrowdControlled() && x.InLineOfSpellSight && Me.IsFacing(x)) : null;
        }

        private static readonly string[] DebuffStandard = new[]
        {
            "Flame Shock"
        };
        */
        #endregion MultiDoT Targets

        private static Composite HandleTotemicProjection()
        {
            return new PrioritySelector(
                Spell.CastOnGround("Totemic Projection", ret => Me.CurrentTarget.Location,
                                   ret => Me.CurrentTarget != null &&
                                          HaveMagmaTotem &&
                                          CanUseTotemicProjection &&
                                          Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 8).Count() >= 3, true)

                // Need a way to handle this to not move totems if it's already hitting 3+ targets...
                );
        }

        #region booleans

        private static bool NeedEarthElemental
        {
            get
            {
                return Spell.GetSpellCooldown("Fire Elemental Totem").TotalSeconds >= 60 &&
                    !Totems.Exist(WoWTotemType.Earth) &&
                    Me.CurrentTarget.Distance <= 40 &&
                    ShamanSettings.UseEarthElemental;
            }
        }

        private static bool Hasted { get { return Me.CurrentTarget != null && Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Timewarp", "Elemental Mastery"); } }

        private static bool NeedFlameShock
        {
            get
            {
                return Me.CurrentTarget != null &&
                       (!Me.CurrentTarget.HasMyAura("Flame Shock") ||
                        Spell.GetMyAuraTimeLeft("Flame Shock", Me.CurrentTarget) < 3 ||
                        (Me.HasAura("Unleash Flame") && Spell.GetMyAuraTimeLeft("Flame Shock", Me.CurrentTarget) < 12)); // refresh early if we can do 30% more damage with buff. Not worth refreshing if we've still got a lot of time left on debuff.
            }
        }

        private static bool NeedLavaBurst
        {
            get
            {
                return Me.CurrentTarget != null && Me.CurrentTarget.HasMyAura("Flame Shock") &&
                       (Spell.GetMyAuraTimeLeft("Flame Shock", Me.CurrentTarget) > Spell.GetSpellCastTime("Lava Burst")) &&
                       !Me.IsMoving;
            }
        }

        private static bool NeedLavaBurstMoving
        {
            get
            {
                return Me.CurrentTarget != null && Me.CurrentTarget.HasMyAura("Flame Shock") && Me.IsMoving &&
                       Me.HasAnyAura("Lava Surge", "Spiritwalker's Grace");
            }
        }

        private static bool NeedEarthShock
        {
            get
            {
                return Me.CurrentTarget != null && !Me.HasAura(114050) && Spell.GetAuraStackCount("Lightning Shield") > 5 && !NeedFlameShock && Spell.GetMyAuraTimeLeft("Flame Shock", Me.CurrentTarget) >= 6;
            }
        }

        private static bool NeedUnleashElements { get { return !Spell.SpellOnCooldown("Unleash Elements") && !Me.HasAura(114050) && Me.CurrentTarget != null && TalentManager.HasTalent(16); } }

        private static bool NeedElementalBlast { get { return !Spell.SpellOnCooldown("Elemental Blast") && !Me.HasAura(114050) && Me.CurrentTarget != null && TalentManager.HasTalent(18); } }

        private static bool NeedLightningBolt { get { return Me.CurrentTarget != null && !Me.HasAura(114050); } }

        private static bool NeedChainLightning { get { return Me.CurrentTarget != null && Me.ManaPercent > 10 && (!Me.IsMoving || Me.HasAura("Spiritwalker's Grace")); } }

        private static bool NeedSearingTotem
        {
            get
            {
                return
                    Me.CurrentTarget != null && // we have a target
                    !Me.HasAura(114050) &&
                    Me.CurrentTarget.Distance <= 39 && // is close enough to be hit from searing totem (totem = 40yd range)
                    !Totems.Exist(WoWTotemType.Fire) && // dont cast if we have a fire totem active (like fire elemental)

                    //(!Spell.SpellOnCooldown("Fire Elemental Totem") || (Spell.SpellOnCooldown("Fire Elemental Totem") && Spell.GetSpellCooldown("Fire Elemental Totem").TotalSeconds > 15)) &&
                    // or we have one already, but it's not in range of our target
                    (Totems.Exist(WoWTotem.Searing) && !Totems.ExistInRange(Me.CurrentTarget.Location, WoWTotem.Searing));
            }
        }

        private static bool NeedMagmaTotem
        {
            get
            {
                return
                    Me.CurrentTarget != null &&
                    !Me.HasAura(114050) &&
                    !Totems.Exist(WoWTotemType.Fire) &&
                    (Unit.NearbyAttackableUnits(Me.Location, 8).Count() >= 3 || Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 8).Count() >= 3 && CanUseTotemicProjection) &&
                    ShamanSettings.UseMagmaTotem;
                // Can do a check here if Totemic Projection is off cooldown, then cast anyway and move the totem using
                // CastOnGround
            }
        }

        private static bool NeedThunderstorm { get { return !Spell.SpellOnCooldown("Thunderstorm") && Me.CurrentTarget != null && Me.ManaPercent < 85; } }

        private static bool CanUseTotemicProjection { get { return TalentManager.HasTalent(9) && !Spell.SpellOnCooldown("Totemic Projection"); } }

        private static bool HaveMagmaTotem { get { return Totems.Exist(WoWTotem.Magma); } }

        private static bool NeedLavaBeam { get { return Me.CurrentTarget != null && Me.HasAura(114050); } }

        private static int _nearbyAoEUnitCount;

        private Composite SetCounts
        {
            get
            {
                return new Action(delegate
                {
                    // Stolen from YourBuddy - Thanks NomNomNom / Alxaw!
                    //modified by Storm, we don't need null exceptions in our log!
                    try
                    {
                        var countList = Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 10);
                        _nearbyAoEUnitCount = countList != null ? countList.Count() : 0;
                    }
                    catch (Exception ex)
                    {
                        Logger.DebugLog("unit detection failed with: {0}", ex);
                        _nearbyAoEUnitCount = 0;
                    }
                    return RunStatus.Failure;
                });
            }
        }

        #endregion booleans

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.ShamanElemental; }
        }

        public override string Name
        {
            get { return "Elemental Shaman"; }
        }

        internal override string Help
        {
            get
            {
                return
                    @"
-----------------------------------------------
Special Key: Unassigned

Suggested Talents: http://www.wowhead.com/talent#s!Lj|kjVo

Note: These talents are what I use, but no
     guarantee's they're the best. Feel free to 
     recommend a better spec on the forums.

~ Millz
-----------------------------------------------
                     ";
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    Racials.UseRacials(),
                    HandleCommon(),
                    HandleDefensiveCooldowns(), // Stay Alive!
                    EncounterSpecific.HandleActionBarInterrupts(),
                    Item.HandleItems(),         // Pop Trinkets, Drink potions...
                    SetCounts,

                    Spell.InterruptSpellCasts(ret => Me.FocusedUnit),
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss() && !EncounterSpecific.HasBadAura, HandleOffensiveCooldowns()),
                                      HandleTotemicProjection(),
                                      new Decorator(ret => _nearbyAoEUnitCount >= ShamanSettings.AoECount && PRSettings.Instance.UseAoEAbilities, HandleAoE()),
                                      new Decorator(ret => Me.CurrentTarget != null && _nearbyAoEUnitCount < ShamanSettings.AoECount, HandleSingleTarget()))),

                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      HandleTotemicProjection(),
                                      new Decorator(ret => _nearbyAoEUnitCount >= ShamanSettings.AoECount && PRSettings.Instance.UseAoEAbilities, HandleAoE()),
                                      new Decorator(ret => Me.CurrentTarget != null && _nearbyAoEUnitCount < ShamanSettings.AoECount, HandleSingleTarget()))),                    
                                      
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleTotemicProjection()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoE()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))));
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
                        HandleOffensiveCooldowns(),                                    // Utility abilitys...
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
                        Spell.Cast("Lightning Shield", ret => !StyxWoW.Me.HasAura("Lightning Shield")),
                        Spell.Cast("Flametongue Weapon", ret => Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 0),

                    //Spell.Cast("Totemic Recall", ret => !Me.Combat && (ActiveNonFireTotem || Totems.Exist(WoWTotemType.Fire))), // recall totems out of combat
                        Spell.Cast("Ghost Wolf", ret => Me.IsMoving && !Me.HasAura("Ghost Wolf") && ShamanSettings.UseGhostWolf)
                        ));
            }
        }

        #endregion Overrides of RotationBase
    }
}