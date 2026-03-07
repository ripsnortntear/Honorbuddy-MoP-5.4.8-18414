#region Revision info
/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Druid/Balance.cs $
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
using Lua = PureRotation.Helpers.Lua;
using SB = PureRotation.Classes.SpellBook;
using TM = PureRotation.Managers.TalentManager;
namespace PureRotation.Classes.Druid
{
    [UsedImplicitly]
    internal class Balance : RotationBase
    {
        private static DruidSettings DruidSettings { get { return PRSettings.Instance.Druid; } }
        private static int _clusterMobCount;
        private static int _hurricaneMobCount;
        private static WoWPoint _hurricaneLocation;

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Barkskin", on => Me, ret => Me.HealthPercent < DruidSettings.HP_Barkskin),
                Spell.Cast("Might of Ursoc", on => Me, ret => Me.HealthPercent < DruidSettings.HP_MoUrsoc),
                Spell.Cast("Innervate", on => Me, ret => Me.ManaPercent < DruidSettings.InnervateMana),
                Item.UseBagItem(5512, ret => Me.HealthPercent <= 40, "Healthstone [" + Me.HealthPercent + "% HP]"),
                Spell.Cast("Renewal", ret => TalentManager.HasTalent(5) && Me.HealthPercent < 30),
                // Self Healing if enabled.
                Spell.Cast("Rejuvenation", on => Me, ret => PRSettings.Instance.EnableSelfHealing && Me.HealthPercent < 50 && !Me.HasMyAura("Rejuvenation")),
                Spell.Cast("Healing Touch", on => Me, ret => PRSettings.Instance.EnableSelfHealing && Me.HealthPercent < 20)
                );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                //Spell.Cast(SpellBook.MirrorImage, ret => Me.HasAura("Symbiosis")), // Mage Buff
                Spell.Cast(SB.ForceOfNature),
                Spell.Cast("Nature's Swiftness", ret => DruidSettings.UsNSwiftness && TalentManager.HasTalent(4) && (TalentManager.HasTalent(17) || Me.HealthPercent < 30)),
                Spell.PreventDoubleCast("Healing Touch", 2.5, ret => Me, ret => !Me.HasAura(108381/*Dream of Cenarius*/) && Me.HasAura("Nature's Swiftness")),
                Spell.Cast("Incarnation", ret => DruidSettings.UseIncarnation && Me.CurrentTarget.IsBoss() && TalentManager.HasTalent(11) && (Lunar || Solar || !Spell.SpellOnCooldown("Celestial Alignment"))),
                Spell.Cast("Celestial Alignment", ret => DruidSettings.UseCelestial && !Lunar && !Solar), //&& (!TalentManager.HasTalent(11) || Me.HasAnyAura(102560, 106731) || Spell.SpellOnCooldown(102560))),
                Spell.Cast("Nature's Vigil", ret => DruidSettings.UseNvigil && TalentManager.HasTalent(18) && (Lunar || Solar || !TalentManager.HasTalent(11) && (Me.HasAura("Celestial Alignment") || Me.HasAura("Incarnation: Chosen of Elune"))))
                );
        }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                // can just check at top of tree if  eclipse power at 100 or -100 then MF or SF
                Spell.Cast("Moonkin Form", ret => !Me.HasAura("Moonkin Form") && !Me.HasAura("Heart of the Wild")),
                Spell.PreventDoubleCast("Starsurge", 0.1, on => Me.CurrentTarget, ret => true, Me.HasAura("Shooting Stars")),
            #region Moonfire/Sunfire selector
                // Force a refresh if DoT is dropping
                Spell.PreventDoubleCast("Moonfire", 1, on => Me.CurrentTarget, ret => NeedMoonfire, true),
                Spell.PreventDoubleCast("Sunfire", 1, on => Me.CurrentTarget, ret => NeedSunfire, true), 
            #endregion
                Spell.PreventDoubleCast("Starfall", 3, on => Me, ret => !Me.HasAura("Starfall")),
                new Decorator(ret => DruidSettings.MultiDoTInSingleTarget, MultiDoT()),
                new Decorator(ret => Me.HasAura("Celestial Alignment"),
                    new PrioritySelector(
                        Spell.Cast("Starfire", ret => Spell.GetSpellCastTime("Starfire") <= Spell.GetAuraTimeLeft("Celestial Alignment")),
                        Spell.Cast("Wrath", ret => Spell.GetSpellCastTime("Starfire") > Spell.GetAuraTimeLeft("Celestial Alignment"))
                )),
                // Filler spells
                Spell.Cast("Wrath", ret => Solar || !Lunar && EclipsePower < 0),
                Spell.Cast("Starfire", ret => Lunar || !Solar && EclipsePower >= 0),
                // While Moving
                new Decorator(req => Me.IsMoving(),new PrioritySelector(
                Spell.PreventDoubleCast("Wild Mushroom: Detonate", 0.4, on => Me.CurrentTarget, ret => DruidSettings.UseMushrooms && Solar && Common.MushroomCount == 3, true),
                Spell.CastOnGround(SpellBook.WildMushroom, loc => Me.CurrentTarget.Location, ret => DruidSettings.UseMushrooms && !Me.CurrentTarget.IsMoving && Common.MushroomCount < 3),
                Spell.PreventDoubleCast("Moonfire", 0.4, on => Me.CurrentTarget, ret => !Me.CurrentTarget.HasAura("Moonfire") ||  Spell.GetAuraTimeLeft("Moonfire", Me.CurrentTarget) <= 9, true),
                Spell.PreventDoubleCast("Sunfire", 0.4, on => Me.CurrentTarget, ret =>!Me.CurrentTarget.HasAura("Sunfire") || Spell.GetAuraTimeLeft("Sunfire", Me.CurrentTarget) <= 9, true),
                Spell.PreventDoubleCast("Moonfire", 0.4, on => Me.CurrentTarget, ret => Lunar , true),
                Spell.PreventDoubleCast("Sunfire", 0.4, on => Me.CurrentTarget, req => true ,true)))
                );
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                Spell.Cast("Moonkin Form", ret => !Me.HasAura("Moonkin Form") && !Me.HasAura("Heart of the Wild")),
            #region Hurricane Handler
                new Action(delegate
                    {
                        if (Me.IsChanneling && (Me.ChanneledCastingSpellId == SpellBook.Hurricane || Me.ChanneledCastingSpellId == SpellBook.AstralStorm))
                        {
                            // We're channeling
                            if (_hurricaneLocation == WoWPoint.Empty)
                            {
                                // We haven't stored the location, so store it.
                                _hurricaneLocation = Me.CurrentTarget.Location;
                            }
                            else
                            {
                                // We've already got the location, check how many units are there
                                try
                                {
                                    var countList = Unit.CachedNearbyAttackableUnits(_hurricaneLocation, 12); 
                                    _hurricaneMobCount = countList != null ? countList.Count() : 0;
                                }
                                catch (Exception ex)
                                {
                                    _hurricaneMobCount = 99;
                                }


                                if (_hurricaneMobCount < 4)
                                {
                                    // Less than 4 units, cancel..
                                    Logger.InfoLog("Units under hurricane is now [{0}] - Cancelling", _hurricaneMobCount);
                                    SpellManager.StopCasting();
                                }
                                else
                                {
                                    Logger.InfoLog("[{0}] units in our hurricane, keep channelling...", _hurricaneMobCount);
                                    // 4 or more units. Return success to prevent tree traverse
                                    return RunStatus.Success; 
                                }
                            }
                        }
                        if (_hurricaneLocation != WoWPoint.Empty && !Me.IsChanneling)
                        {
                            // We have hurricane location stored, but we're not channeling. Null it.
                            _hurricaneLocation = WoWPoint.Empty;
                        }
                        return RunStatus.Failure;
                    }),
            #endregion

                Spell.PreventDoubleCast("Starsurge", 0.1, on => Me.CurrentTarget, ret => true, Me.HasAura("Shooting Stars")),
                Spell.PreventDoubleCast("Starfall", 3, on => Me, ret => !Me.HasAura("Starfall")),
                Spell.PreventDoubleCastOnGround("Hurricane", 1, loc => Me.CurrentTarget.Location, ret => !Me.IsMoving() && Me.CurrentTarget.Distance < 40 &&
                                                        (_clusterMobCount > 5 && Me.ManaPercent > 25) ||
                                                        (_clusterMobCount > 4 && (Solar || Lunar) && Me.HasAura("Nature's Grace"))),
                Spell.CastOnGround(SpellBook.WildMushroom, loc => Me.CurrentTarget.Location, ret => DruidSettings.UseMushrooms && Me.CurrentTarget.Distance < 40 && Common.MushroomCount < 3 && _clusterMobCount > 2 && Me.IsMoving()),
                Spell.Cast("Wild Mushroom: Detonate", ret => DruidSettings.UseMushrooms && Solar && Common.MushroomCount == 3),
                MultiDoT(),
                HandleSingleTarget()
                );
        }

        #region Mirabis Simc
        private static Composite MiraSymc()
        {
            return new PrioritySelector(
                //Populate Stats
                //Cooldown Management
                Spell.Cast("Starfall",req => !Me.HasAura("Starfall")),
                Spell.Cast("Berserking", req => !Me.HasAura("Celestial Alignment")),
                Spell.Cast("Wild Mushroom: Detonate", req => !Me.IsMoving() && Solar  && Common.MushroomCount > 0),
                Spell.Cast("Incarnation", req => Lunar || Solar),
                //No Eclipse
                Spell.Cast("Celestial Alignment",req=> (!Lunar && !Solar) && (Me.CachedHasAura("Incarnation : Chosen of Elune") || Spell.GetSpellCooldown("Incarnation").Seconds > 10)),
                Spell.Cast("Nature's Vigil"),
                Spell.Cast("Starsurge", req => Me.HasAura("Shooting Stars") && !Solar /*|| Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location,30f).Count() < 5) BUG: Single Target Rotation atm*/),
                //Clip or Refresh a better one
                Spell.PreventDoubleCast("Moonfire",0.4,req => Lunar && !Me.CurrentTarget.HasAura("Moonfire",0,true,(int)Me.CachedGetAuraTimeLeft("Nature's Grace")-2)  ),
                Spell.PreventDoubleCast("Sunfire",0.4, req => Solar && !Me.CurrentTarget.HasAura("Sunfire",0, true, (int)Me.CachedGetAuraTimeLeft("Nature's Grace") - 2)),
                //Apply if not up
                Spell.PreventDoubleCast("Moonfire", 0.4, req => !Me.CurrentTarget.HasAura("Moonfire", 0, true, (int)Me.CachedGetAuraTimeLeft("Nature's Grace") - 2)),
                Spell.PreventDoubleCast("Sunfire", 0.4, req => !Me.CurrentTarget.HasAura("Sunfire", 0, true, (int)Me.CachedGetAuraTimeLeft("Nature's Grace") - 2)),
                //Refresh
                Spell.PreventDoubleCast("Moonfire", 0.4, req => Lunar && DotTicks("Moonfire") < 2),
                Spell.PreventDoubleCast("Sunfire", 0.4, req => Solar && DotTicks("Sunfire") < 2),
                Spell.Cast("Starsurge"),
                //Celestial Alignment is up
                new Decorator(req => Me.CachedHasAura("Celestial Alignment"), new PrioritySelector(
                    Spell.Cast("Starfire", req => Spell.GetSpellCastTime("Starfire") < Me.CachedGetAuraTimeLeft("Celestial Alignment")),
                    Spell.Cast("Wrath", req => Spell.GetSpellCastTime("Wrath") < Me.CachedGetAuraTimeLeft("Celestial Alignment")))),
                //Eclipse Handling
                Spell.Cast("Starfire", ret => Lua.GetEclipseDirection() == EclipseType.Lunar || (Lua.GetEclipseDirection() == EclipseType.None && EclipsePower > 0)),
                Spell.Cast("Wrath", ret => Lua.GetEclipseDirection() == EclipseType.Solar || (Lua.GetEclipseDirection() == EclipseType.None && EclipsePower <= 0)),
                // When Moving
                new Decorator(req=> Me.IsMoving(), new PrioritySelector(
                    Spell.PreventDoubleCast("Moonfire", 0.4, req => DotTicks("Moonfire") < 2),
                    Spell.PreventDoubleCast("Sunfire", 0.4, req => DotTicks("Sunfire") < 2),
                    Spell.CastOnGround("Wild Mushroom",on=> Me.CurrentTarget.Location,req => Common.MushroomCount < 3),
                    Spell.Cast("Stasurge"),
                    Spell.Cast("Moonfire", req=> Lunar),
                    Spell.Cast("Sunfire")))
                );
        }

        private static double DotTicks(string auraName)
        {
            var target = Me.CurrentTarget;
            if (target == null)
                return 9;
            double ticktime = Me.HasAura("Nature's Grace") ? 1472 : 1692;
            var ticksleft = (target.CachedGetAuraTimeLeft(auraName)/ticktime);
            return ticksleft;
        }
        #endregion

        private static Composite MultiDoT()
        {
            return new PrioritySelector(
                Spell.PreventDoubleMultiDoT("Moonfire", 0.5, Me, 40, 2, ret => Lunar),
                Spell.PreventDoubleMultiDoT("Sunfire", 0.5, Me, 40, 2, ret => Solar && !Me.HasAura("Celestial Alignment")),
                new Decorator(ret => !Lunar && !Solar,
                    new PrioritySelector(
                        Spell.PreventDoubleMultiDoT("Moonfire", 0.5, Me, 40, 2, ret => true),
                        Spell.PreventDoubleMultiDoT("Sunfire", 0.5, Me, 40, 2, ret => !Me.HasAura("Celestial Alignment"))))
                );
        }

        #region Symbiosis

        private static WoWUnit BestSymbTarget
        {
            get
            {
                return GetPlayerByClassPrio(30f, false,
                      WoWClass.Mage, // offensive
                      WoWClass.Warlock, // defensive
                      WoWClass.DeathKnight); // defensive
            }
        }

        private static WoWUnit GetPlayerByClassPrio(float range, bool includeDead, params WoWClass[] classes)
        {
            return (from WoWClass in classes select GroupMembers.FirstOrDefault(p => p.ToPlayer() != null && p.ToPlayer().Distance < range && p.ToPlayer().Class == WoWClass) into unit where unit != null where !includeDead && unit.Dead || unit.Ghost select unit.ToPlayer()).FirstOrDefault();
        }

        private static IEnumerable<WoWPartyMember> GroupMembers { get { return !Me.GroupInfo.IsInRaid ? Me.GroupInfo.PartyMembers : Me.GroupInfo.RaidMembers; } }

        #endregion Symbiosis

        #region statics

        private static bool Solar { get { return Me.HasAura(48517); } }
        private static bool Lunar { get { return Me.HasAura(48518); } }
        private static int EclipsePower { get { return Lua.PlayerUnitPower("SPELL_POWER_ECLIPSE"); } }
        private static bool NeedAstralCommunion { get { return (EclipsePower < 70 && EclipsePower > -70); } }
        private static bool NeedMoonfire { get { return !Me.CurrentTarget.HasAura("Moonfire") || (Me.CurrentTarget.HasAura("Moonfire") && Spell.GetAuraTimeLeft("Moonfire", Me.CurrentTarget) <= 3); } }
        private static bool NeedSunfire { get { return !Me.CurrentTarget.HasAura("Sunfire") || (Me.CurrentTarget.HasAura("Sunfire") && Spell.GetAuraTimeLeft("Sunfire", Me.CurrentTarget) <= 3); } }

        #endregion booleans

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.DruidBalance; }
        }

        public override string Name
        {
            get { return "Balance Druid"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused || Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                    CachedUnits.Pulse,
                    HandleDefensiveCooldowns(),
                    Racials.UseRacials(),
                    Item.HandleItems(),
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    new Action(delegate
                        {
                            try
                            {
                                var countList = Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 12); 
                                _clusterMobCount = countList != null ? countList.Count() : 0;
                            }
                            catch (Exception ex)
                            {
                                Logger.DebugLog("Unit count failed with: {0}", ex);
                                _clusterMobCount = 1;
                            }
                            
                            return RunStatus.Failure;
                        }),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss() && !EncounterSpecific.HasBadAura, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && _clusterMobCount >= PRSettings.Instance.Druid.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && _clusterMobCount >= PRSettings.Instance.Druid.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
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
                return new PrioritySelector();
            }
        }

        public override Composite PreCombat
        {
            get
            {
                return
                    new Decorator(
                        ret =>
                        !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport &&
                        !Me.HasAura("Food") && !Me.HasAura("Drink"),
                        new PrioritySelector(
                            Spell.CastRaidBuff("Mark of the Wild", ret => true),
                            Spell.PreventDoubleCast("Healing Touch", 5, ret => Me,
                                                    ret => TalentManager.HasTalent(17) && !Me.HasAura(108381)),
                            Spell.Cast("Moonkin Form", on => Me, ret => !Me.HasAura("Moonkin Form")),
                            Spell.PreventDoubleChannel("Astral Communion", 0.5, true, on => Me, ret => Me.CurrentTarget != null && !Me.CurrentTarget.IsDead && DruidSettings.UseAstralCommunion && NeedAstralCommunion),
                            new Action(delegate
                                {
                                    if (Me.IsChanneling && Me.ChanneledSpell.Id == 127663 && !NeedAstralCommunion)
                                    {
                                        Logger.InfoLog("Cancelling Astral Communion [Eclipse Power: {0}]", EclipsePower);
                                        SpellManager.StopCasting();
                                    }
                                })
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
Special Key: Unassigned

Recommended Spec: http://eu.battle.net/wow/en/tool/talent-calculator#Ua!012122!h

Latest Updates [19-July-2013]
- Force of Nature is now casted on current target, used as cooldown (usage of Cooldowns has to be enabled)
- removed some uncommented stuff
Enjoy! ~ Millz, Stormchasing
-----------------------------------------------------------------
                     ";
            }
        }

        #endregion Overrides of RotationBase
    }
}