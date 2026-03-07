#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-24 21:54:34 +0200 (Di, 24 Sep 2013) $
 * $ID$
 * $Revision: 1737 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Warlock/Affliction.cs $
 * $LastChangedBy: tumatauenga1980 $
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
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;
using Lua = PureRotation.Helpers.Lua;

namespace PureRotation.Classes.Warlock
{
    [UsedImplicitly]
    internal class Affliction : RotationBase
    {

        private static WarlockSettings WarlockSettings
        {
            get { return PRSettings.Instance.Warlock; }
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Common.CommonDefensiveCooldowns()
                );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Dark Soul: Misery",
                           ret =>
                           WarlockSettings.UseDarkSoulMisery && !Common.Hasted && !NeedAgony && !NeedCorruption &&
                           !NeedUnstableAffliction),
                Common.CommonOffensiveCooldowns()
                );
        }

        private static Composite HandleCommon()
        {
            return new PrioritySelector(
                Common.HandleCommonAbilities()
                );
        }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                HandleStopChannelToCurse(),
                new Decorator(ret => Me.HasAura("Soul Swap"), new Action(delegate { Me.CancelAura("Soul Swap"); return RunStatus.Failure; })), // If we accidently get Soul Swap Exhale
                Spell.PreventDoubleCast("Soulburn", 2, ret => NeedSoulBurn),
                Spell.ForceCast(SpellBook.SoulburnSoulswap, on => Me.CurrentTarget, ret => Spell.Lastspellcast!=SpellBook.SoulburnSoulswap.ToString() && Me.ActiveAuras.ContainsKey("Soulburn")),
                new Decorator(ret => Me.HasAura("Soulburn") || Me.ActiveAuras.ContainsKey("Soulburn") || Me.IsChanneling, new ActionAlwaysSucceed()),

                Spell.PreventDoubleCast("Curse of the Elements", 2.5, ret => Common.NeedCurseOfElements),
                // UA needs to be cast by ID as SpellManager.CanCast() does not contain overload to ignore moving.
                Spell.PreventDoubleCast("Unstable Affliction", 2, on => Me.CurrentTarget, ret => NeedUnstableAffliction, true),
                Spell.PreventDoubleCast(SpellBook.Corruption, 2, ret => NeedCorruption),
                Spell.PreventDoubleCast(SpellBook.Agony, 2, ret => NeedAgony),
                Spell.PreventDoubleCast("Haunt", 3, ret => NeedHaunt),

                Spell.PreventDoubleChannel("Drain Soul", 0.5, true, on => Me.CurrentTarget,
                                           ret => NeedDrainSoul && !NeedaCurse, UseKilJaedensCunningPassive),

                Spell.PreventDoubleChannel("Malefic Grasp", 0.5, true, on => Me.CurrentTarget, ret => !NeedaCurse,
                                           UseKilJaedensCunningPassive),

                Spell.Cast("Life Tap",
                           ret => !Me.IsChanneling && (Me.IsMoving() && Common.CanLifeTap || Me.ManaPercent < 10)),

                Spell.Cast("Fel Flame",
                           ret => Me.IsMoving() && !UseKilJaedensCunningPassive && Me.IsFacing(Me.CurrentTarget)));
        }

        private static Composite HandleLowUnitAoECombat()
        {
            WoWUnit soulSwapTarget = null;
            return new PrioritySelector(ctx => soulSwapTarget = GetSoulSwapTarget(),

                Spell.ForceCast(SpellBook.SoulburnSoulswap, on => soulSwapTarget, ret => Me.ActiveAuras.ContainsKey("Soul Swap")),
                new Decorator(ret => Me.HasAura("Soul Swap"), new Action(delegate{ Me.CancelAura("Soul Swap"); return RunStatus.Failure; })),

                // If target isn't boss, and less than 5% hp, and we have 3 or less shards, drain soul to get shards.
                Spell.PreventDoubleChannel("Drain Soul", 0.5, true, on => Me.CurrentTarget,
                                           ret =>
                                           !Me.CurrentTarget.IsBoss() && Me.CurrentTarget.HealthPercent < 5 &&
                                           Me.CurrentSoulShards <= 3),

                // Handle canceling drain soul to perform other actions
                new Decorator(
                    a => Me.ChanneledCastingSpellId == 1120 && soulSwapTarget != null && Me.CurrentSoulShards >= 1,
                    new Action(delegate
                        {
                            SpellManager.StopCasting();
                            return RunStatus.Failure;
                        })),

                // Soulburn
                Spell.PreventDoubleCast("Soulburn", 1,
                                        ret =>
                                        soulSwapTarget != null && !Me.ActiveAuras.ContainsKey("Soulburn") &&
                                        Me.CurrentSoulShards >= 1),

                // AoE Soul Swap
                new Decorator(ret => Me.HasAura(SpellBook.Soulburn) && soulSwapTarget != null,
                              Spell.ForceCast(SpellBook.SoulburnSoulswap, on => soulSwapTarget,
                                              ret => Me.ActiveAuras.ContainsKey("Soulburn"))
                    ),

                Spell.PreventDoubleMultiDoT("Corruption", 1, Me, 40, 3, ret => !Me.HasAura("Soulburn")),
                Spell.PreventDoubleMultiDoT("Agony", 1, Me, 40, 3, ret => !Me.HasAura("Soulburn")),
                Spell.PreventDoubleMultiDoT("Unstable Affliction", 1, Me, 40, 3, ret => !Me.HasAura("Soulburn")),

                HandleSingleTarget()
                );
        }

        private static Composite HandleHighUnitAoECombat()
        {
            WoWUnit seedTarget = null;
            return new PrioritySelector(ctx => seedTarget = GetSoCTarget(),
                new Decorator(ret => Me.HasAura("Soul Swap"), new Action(delegate { Me.CancelAura("Soul Swap"); return RunStatus.Failure; })),

                Spell.PreventDoubleCast("Soulburn", 1, ret => !Me.HasAura(SpellBook.Soulburn) && Me.CurrentSoulShards >= 1
                                        && (NeedSoulburnSoC || GetSoulSwapTarget() != null)),

                // SB:Seed of Corruption - Not in SpellManager so cast by ID.
                Spell.PreventDoubleCast(SpellBook.SoulburnSeedOfCorruption, 0.5, on => seedTarget, ret => seedTarget != null && Me.HasAura(SpellBook.Soulburn) && !seedTarget.HasAura(SpellBook.SoulburnSeedOfCorruption)),

                // Soul Swap current target
                Spell.ForceCast(SpellBook.SoulburnSoulswap, on => Me.CurrentTarget, ret => Me.CurrentTarget != null && Me.CurrentTarget.HasAllAuras("Agony", "Corruption") && Me.ActiveAuras.ContainsKey("Soulburn")),

                // AoE Soul Swap
                Spell.ForceCast(SpellBook.SoulburnSoulswap, on => GetSoulSwapTarget(), ret => GetSoulSwapTarget() != null && Me.ActiveAuras.ContainsKey("Soulburn")),

                // Spam SoC
                Spell.PreventDoubleCast(SpellBook.SeedofCorruption, 0.5, on => seedTarget, ret => seedTarget != null),

                Spell.Cast("Life Tap", ret => !Me.IsChanneling && Common.CanLifeTap),

                // Put Corruption on our current target if nothing to do - might get shards off it?
                Spell.PreventDoubleMultiDoT("Corruption", 1, Me, 40, 8, ret => Me.CurrentSoulShards == 0 || !Me.HasAura(SpellBook.Soulburn)),
                Spell.PreventDoubleMultiDoT("Agony", 1, Me, 40, 11, ret => seedTarget == null));
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                new Decorator(ret => _nearbyAoEUnitCount <= 3, HandleLowUnitAoECombat()),
                new Decorator(ret => _nearbyAoEUnitCount >= 4, HandleHighUnitAoECombat()));
        }

        private static Composite HandleStopChannelToCurse()
        {
            return
                new Decorator(
                    a =>
                    // Drain Soul, Boss, Need DoTs...
                    (Me.ChanneledCastingSpellId == SpellBook.DrainSoul &&
                    ((NeedaCurse && Me.CurrentTarget.IsBoss() && Me.CurrentSoulShards >= 1) || !NeedDrainSoul))
                    ||
                    // Malefic Grasp and target < 20% HP
                    (Me.ChanneledCastingSpellId == SpellBook.MaleficGrasp &&
                    Me.CurrentTarget.HealthPercent < 20)
                    ,
                    new Action(delegate
                        {
                            SpellManager.StopCasting();
                            return RunStatus.Failure;
                        }));
        }


        private static bool NeedaCurse
        {
            get { return NeedHaunt || NeedUnstableAffliction || NeedAgony || NeedCorruption; }
        }


        #region Targets

        private static WoWUnit GetSoulSwapTarget()
        {
            return Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 40)
                            .OrderByDescending(x => x.CurrentHealth)
                            .FirstOrDefault(x => !x.HasAllAuras("Corruption", "Agony", "Unstable Affliction")) 
                   ??
                   Unit.CachedNearbyAttackableUnitsAttackingUs(Me.Location, 40)
                             .OrderByDescending(x => x.CurrentHealth)
                             .FirstOrDefault(x => !x.HasAura("Agony"));
        }

        private static WoWUnit GetSoCTarget()
        {
            return Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15)
                            .OrderByDescending(x => x.CurrentHealth)
                            .FirstOrDefault(x => !x.HasAura("Seed of Corruption") && x.Distance < 40 && x.InLineOfSpellSight);
        }

        #endregion Targets

        #region booleans

        private static bool UseKilJaedensCunningPassive { get { return TalentManager.HasTalent(17); } }
        //private static double CurrentSoulShards { get { return Lua.PlayerUnitPower("SPELL_POWER_SOUL_SHARDS"); } } // Me.GetPowerInfo(WoWPowerType.SoulShards).CurrentI
        private static bool NeedSoulSwap
        {
            get
            {
                if (Me.CurrentTarget != null && !Me.IsChanneling && Me.CurrentTarget.HealthPercent <= 20 && Me.CurrentTarget.IsBoss() && (NeedAgony || NeedCorruption || NeedUnstableAffliction))
                {
                    return true;
                }

                return Me.CurrentTarget != null &&
                    (
                    !Me.CurrentTarget.HasAura("Unstable Affliction") &&
                    !Me.CurrentTarget.HasAura("Corruption") &&
                    !Me.CurrentTarget.HasAura("Agony")
                    );
            }
        }
        private static bool NeedHaunt
        {
            get
            {
                return !Me.IsMoving() &&
                    Spell.Lastspellcast!="Haunt" &&
                        // Check we don't already have haunt, or haunt is about to expire.
                       (!Me.CurrentTarget.HasMyAura("Haunt") || (Me.CurrentTarget.HasMyAura("Haunt") && Spell.GetAuraTimeLeft("Haunt", Me.CurrentTarget) < Spell.GetSpellCastTime("Haunt")))

                       // We have 3 shards (so close to cap)
                       && (Me.CurrentSoulShards >= 3

                       // We have 1 or more shards and dark soul is on cooldown for more than 35 seconds
                       || Me.CurrentSoulShards >= 1 && Spell.GetSpellCooldown(SpellBook.DarkSoulMisery).TotalSeconds > 35

                       // We have 2 or more shards, and the time left on dark soul misery is longer than the time it'll take to cast haunt
                       || Me.CurrentSoulShards >= 2 && Spell.GetAuraTimeLeft(SpellBook.DarkSoulMisery) > Spell.GetSpellCastTime(SpellBook.Haunt)
                       
                       // We have a shard, and target HP is less than, or equal to 20%
                       || Me.CurrentSoulShards >= 1 && Me.CurrentTarget.HealthPercent <= 20)
                       ;
            }
        }
        private static bool NeedUnstableAffliction
        {
            get { return Spell.Lastspellcast!="Unstable Affliction" && Spell.GetAuraTimeLeft("Unstable Affliction", Me.CurrentTarget) <= 6 /*|| CasterStatBuffActiveUnstableAffliction*/; }
        }
        private static bool NeedAgony
        {
            get { return (Spell.Lastspellcast != "Agony" && Spell.GetAuraTimeLeft("Agony", Me.CurrentTarget) <= 11) || CasterStatBuffActiveAgony; }
        }
        private static bool NeedCorruption
        {
            get { return (Spell.Lastspellcast != "Corruption" && Spell.GetAuraTimeLeft("Corruption", Me.CurrentTarget) <= 8) || CasterStatBuffActiveCorruption; }
        }
        
        private static bool NeedDrainSoul { get { return Me.CurrentTarget != null && Me.CurrentTarget.HealthPercent <= 20; } }
        private static bool NeedSoulBurn { get
        {
            return Me.CurrentTarget != null && !Me.CurrentTarget.IsDead && Me.CurrentSoulShards >= 1 && !Me.ActiveAuras.ContainsKey("Soulburn") 
                && (NeedSoulSwap || (CasterStatBuffActiveAgony && CasterStatBuffActiveCorruption /*&& CasterStatBuffActiveUnstableAffliction*/));
        } }
        private static bool NeedSoulburnSoC
        {
            get
            {
                return
                        !Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 20)
                            .Any(x => x.HasAura(SpellBook.SoulburnSeedOfCorruption))

                        && Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15)
                                .Count(x => x.CachedHasAura("Corruption")) >= 2;
            }
        }

        private static bool CasterStatBuffActiveCorruption
        {
            get
            {
                return
                    WarlockSettings.RefreshDoTsOnStatIncrease && DoTTracker.NeedRefresh(Me.CurrentTarget, SpellBook.Corruption);
            }
        }
        private static bool CasterStatBuffActiveAgony
        {
            get
            {
                return WarlockSettings.RefreshDoTsOnStatIncrease && DoTTracker.NeedRefresh(Me.CurrentTarget, SpellBook.Agony);
            }
        }

        private static int _nearbyAoEUnitCount;
        private static Composite SetCounts
        {
            get
            {
                return new Action(delegate
                {
                    var s = Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15);
                    _nearbyAoEUnitCount =s!=null?s.Count():1;//?Unit.CachedNearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count():1;
                    //_currentSoulShards = Me.CurrentSoulShards; //Lua.PlayerUnitPower("SPELL_POWER_SOUL_SHARDS");
                    //Logger.DebugLog("Stat Buff/Force Refresh [Corruption:{0}] [Agony:{1}] [UA:{2}]", CasterStatBuffActiveCorruption, CasterStatBuffActiveAgony, CasterStatBuffActiveUnstableAffliction);
                    return RunStatus.Failure;
                });
            }
        }

        #endregion booleans

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1737 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.WarlockAffliction; }
        }

        public override string Name
        {
            get { return "Affliction Warlock"; }
        }

        internal override string Help
        {
            get
            {
                return
@"
-----------------------------------------------
Special Key: Unassigned

Recommended Spec: http://www.wowhead.com/talent#o!i]|zia

Latest Updates [28-Jun-2013]
- [Rev: 1577] Attempt to fix manually applying DoTs when we need to Soul Swap
- [Rev: 1578] Fixed Haunt for execute phase (<=20% HP)
- [Rev: 1579] Now getting cast time for Haunt as refresh point, rather than fixed '< 1.5', allows it to adjust for heroism/haste etc.

Enjoy! - Millz
-----------------------------------------------
";
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => (WarlockSettings.EnableLagFix && SpellManager.GlobalCooldown) || HotKeyManager.IsPaused || Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                    CachedUnits.Pulse,
                    Racials.UseRacials(),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    HandleDefensiveCooldowns(), // Stay Alive!
                    HandleCommon(),
                    Item.HandleItems(),         // Pop Trinkets, Drink potions...
                    SetCounts, // Get AoE unit count & soulshard count
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),
                                      new Decorator(ret => _nearbyAoEUnitCount >= WarlockSettings.AoECount, HandleAoeCombat()),
                                      new Decorator(ret => _nearbyAoEUnitCount < WarlockSettings.AoECount, HandleSingleTarget()))),

                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => _nearbyAoEUnitCount >= WarlockSettings.AoECount, HandleAoeCombat()),
                                      new Decorator(ret => _nearbyAoEUnitCount < WarlockSettings.AoECount, HandleSingleTarget()))),

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
                return new Decorator(ret => !Me.IsMoving && !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        Common.HandleCommonPreCombat()
                        ));
            }
        }

        internal override void OnPulse()
        {
            if (!DoTTracker.Initialized && WarlockSettings.RefreshDoTsOnStatIncrease)
            {
                DoTTracker.Initialize();
            }

            if (DoTTracker.Initialized && !WarlockSettings.RefreshDoTsOnStatIncrease)
            {
                DoTTracker.Shutdown();
            }
        }

        #endregion Overrides of RotationBase
    }
}