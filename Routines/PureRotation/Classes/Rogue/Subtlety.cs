#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-05-30 16:12:12 +0200 (Do, 30 Mai 2013) $
 * $ID$
 * $Revision: 1478 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Rogue/Subtlety.cs $
 * $LastChangedBy: millz $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Linq;

using CommonBehaviors.Actions;

using JetBrains.Annotations;

using PureRotation.Core;
using PureRotation.Managers;
using PureRotation.Settings.Settings;

using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;

using Action = Styx.TreeSharp.Action;

namespace PureRotation.Classes.Rogue
{
    [UsedImplicitly]
    internal class Subtlety : Rogue
    {
        #region Constants and Fields

        private static readonly TimeSpan GCD = TimeSpan.FromSeconds(1);

        //        private static readonly TimeSpan HATTick = TimeSpan.FromSeconds(2);
        private static readonly TimeSpan RuptureTick = TimeSpan.FromSeconds(2);

        //        private static readonly TimeSpan ShadowDanceLength = TimeSpan.FromSeconds(8);
        private static readonly TimeSpan SnDEnergyTick = TimeSpan.FromSeconds(2);

        private static TimeSpan? _hemoDebuff;
        private static TimeSpan? _ruptureRefrehTime;
        private static TimeSpan? _ruptureTimeLeft;
        private static TimeSpan? _sndRefreshTime;
        private static TimeSpan? _sndTimeLeft;
        private int? _cpBuilderEnergy;
        private int? _cpPerBuilder;
        private bool? _cpSafe;
        private float? _energyRegen;
        private bool? _poolShadowDance;
        private bool? _poolVanish;
        private bool? _stealthCDSafe;
        private bool? _stealthed;

        #endregion Constants and Fields

        #region Public Properties

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.RogueSubtlety; }
        }

        public override string Name
        {
            get { return "Subtlety Rogue"; }
        }

        internal override string Help
        {
            get
            {
                return
                    @"
-----------------------------------------------------------------
Please note: Subtlety rotation does not support Semi-Auto mode.
-----------------------------------------------------------------
                     ";
            }
        }

        public override Composite PVERotation
        {
            get
            {
                return new Decorator
                    (
                    cond =>
                    Me.CurrentTarget != null && Me.CurrentTarget.Attackable && Me.CurrentTarget.IsWithinMeleeRange,
                    new PrioritySelector
                        (new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                         Item.HandleItems(),    // Pop Trinkets, Drink potions...
                         Racials.UseRacials(),
                         new Action(context => Reset()),
                         StealthedRotation,
                         RegularRotation));
            }
        }

        public override Composite PVPRotation
        {
            get { return PVERotation; }
        }

        //        public override Composite PreCombat
        //        {
        //            get { return null; }
        //        }

        public override string Revision
        {
            get { return "$Rev: 1478 $"; }
        }

        #endregion Public Properties

        #region Properties

        protected override bool CPSafe
        {
            get
            {
                if (!_cpSafe.HasValue)
                {
                    if (!base.CPSafe)
                    {
                        _cpSafe = false;
                    }

                    int max = HasAnticipation ? 10 : 5;

                    // Ambush 2pts
                    if (PlayerStealthed && BehindTarget)
                    {
                        max--;
                    }

                    // All attacks that generate combo points + 1 pt.
                    if (Me.HasAura(RogueSpells.ShadowBlades) &&
                         Me.GetAuraById(RogueSpells.ShadowBlades).IsActive)
                    {
                        max--;
                    }

                    _cpSafe = ComboPoints < max;
                }

                return _cpSafe.Value;
            }
        }

        protected override float EnergyRegen
        {
            get
            {
                if (!_energyRegen.HasValue)
                {
                    _energyRegen = base.EnergyRegen;

                    if (SnDTimeLeft > SnDEnergyTick)
                    {
                        _energyRegen += 4;
                    }
                }

                return _energyRegen.GetValueOrDefault(10);
            }
        }

        protected override bool PlayerStealthed
        {
            get
            {
                if (!_stealthed.HasValue)
                {
                    _stealthed = base.PlayerStealthed || Me.HasAura(MySpells.ShadowDance) ||
                                      (Me.HasAura(MySpells.MasterOfSubtlety) &&
                                        !Me.GetAuraById(MySpells.MasterOfSubtlety).IsActive);
                }

                return _stealthed.Value;
            }
        }

        private static TimeSpan HemoDebuff
        {
            get
            {
                if (_hemoDebuff.HasValue)
                {
                    return _hemoDebuff.Value;
                }

                if (!Me.CurrentTarget.HasAura(MySpells.HemorrhageDebuff))
                {
                    _hemoDebuff = TimeSpan.Zero;
                    return _hemoDebuff.Value;
                }

                _hemoDebuff = Me.CurrentTarget.GetAuraById(MySpells.HemorrhageDebuff).TimeLeft;
                return _hemoDebuff.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether it's safe to use Preparation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if safe to cast Preparation; otherwise, <c>false</c>.
        /// </value>
        private static bool PreparationSafe
        {
            get
            {
                return WoWSpell.FromId(RogueSpells.Vanish).Cooldown && PRSettings.Instance.UseCooldowns &&
                       Me.CurrentTarget.IsBoss();
            }
        }

        private static TimeSpan RuptureRefreshTime
        {
            get
            {
                if (!_ruptureRefrehTime.HasValue)
                {
                    _ruptureRefrehTime = RuptureTimeLeft - RuptureTick;
                }

                return _ruptureRefrehTime.Value;
            }
        }

        private static TimeSpan RuptureTimeLeft
        {
            get
            {
                if (!_ruptureTimeLeft.HasValue)
                {
                    _ruptureTimeLeft = AuraTimeLeft(Me.CurrentTarget, RogueSpells.Rupture);
                }

                return _ruptureTimeLeft.Value;
            }
        }

        private static bool SafeToBreakStealth
        {
            get
            {
                return (Me.Combat || Me.RaidMembers.Any(rm => rm.Combat) || Me.CurrentTarget.IsBoss()) &&
                       PRSettings.Instance.UseCooldowns;
            }
        }

        private static bool ShadowBladesSafe
        {
            get
            {
                return SnDTimeLeft > TimeSpan.FromSeconds(12) && RuptureTimeLeft > TimeSpan.FromSeconds(12) &&
                       PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss();
            }
        }

        private static TimeSpan SnDRefreshTime
        {
            get
            {
                if (_sndRefreshTime.HasValue)
                {
                    return _sndRefreshTime.Value;
                }

                _sndRefreshTime = SnDTimeLeft - SnDEnergyTick;

                return _sndRefreshTime.Value;
            }
        }

        //        private static bool ShadowDanceSafe
        //        {
        //            get { return SnDTimeLeft > ShadowDanceLength && RuptureTimeLeft > ShadowDanceLength; }
        //        }

        private static TimeSpan SnDTimeLeft
        {
            get
            {
                if (!_sndTimeLeft.HasValue)
                {
                    _sndTimeLeft = AuraTimeLeft(Me, RogueSpells.SliceAndDice);
                }
                return _sndTimeLeft.Value;
            }
        }

        /// <summary>
        /// Gets the energy required by our current combo point builder
        /// </summary>
        /// <value>
        /// Energy cost
        /// </value>
        private int CPBuilderEnergy
        {
            get
            {
                if (!_cpBuilderEnergy.HasValue)
                {
                    // Fan of Knives, 35 energy
                    if (PRSettings.Instance.UseAoEAbilities &&
                         AoETargets.Count() >= PRSettings.Instance.Rogue.SubtletyFanOfKnivesCount)
                    {
                        _cpBuilderEnergy = 35;
                    }

                    if (PlayerStealthed)
                    {
                        // Ambush w/ Shadow Dance
                        if (Me.HasAura(MySpells.ShadowDance))
                        {
                            _cpBuilderEnergy = BehindTarget ? 20 : 30;
                        }

                        // Ambush w/o Shadow Dance
                        _cpBuilderEnergy = BehindTarget ? 60 : 30;
                    }

                    // Backstab, 35; Hemorrhage, 30
                    _cpBuilderEnergy = BehindTarget ? 35 : 30;
                }

                return _cpBuilderEnergy.Value;
            }
        }

        private int CPPerBuilder
        {
            get
            {
                if (!_cpPerBuilder.HasValue)
                {
                    // Ambush
                    if (PlayerStealthed && BehindTarget)
                    {
                        _cpPerBuilder = 2;
                    }

                    _cpPerBuilder = 1;

                    if (Me.HasAura(RogueSpells.ShadowBlades) &&
                         Me.GetAuraById(RogueSpells.ShadowBlades).IsActive)
                    {
                        _cpPerBuilder++;
                    }
                }

                return _cpPerBuilder.Value;
            }
        }

        private Composite ComboPointGen
        {
            get
            {
                return new Decorator
                    (cond =>
                     CPSafe && !PlayerStealthed && !PoolVanish && !PoolShadowDance &&
                     FinisherSafe,
                     new PrioritySelector
                         (Spell.PreventDoubleCast
                              (RogueSpells.FanOfKnives,
                               InstantDoubleCastTimer,
                               delegate
                               {
                                   return AoETargets.Count() >= PRSettings.Instance.Rogue.SubtletyFanOfKnivesCount &&
                                          (HotkeySettings.Instance.ModeChoice == Mode.Auto || HotKeyManager.IsAoe);
                               }),
                          Spell.PreventDoubleCast
                              (MySpells.Hemorrhage, InstantDoubleCastTimer, ret => HemoDebuff < TimeSpan.FromSeconds(3)),
                          Spell.PreventDoubleCast
                              (RogueSpells.Backstab,
                               InstantDoubleCastTimer,
                               ret => HemoDebuff >= TimeSpan.FromSeconds(3) && BehindTarget),
                          Spell.PreventDoubleCast(MySpells.Hemorrhage, InstantDoubleCastTimer, ret => Energy > 35)));
            }
        }

        /// <summary>
        /// Gets the cooldowns rotation.
        /// </summary>
        private Composite Cooldowns
        {
            get
            {
                return new Decorator
                    (cond => PRSettings.Instance.UseCooldowns,
                     new PrioritySelector
                         (Spell.Cast
                              (MySpells.ShadowDance,
                               cond =>
                               PoolShadowDance && StealthCDSafe && Energy >= 80),
                          Spell.Cast(RogueSpells.ShadowBlades, ret => ShadowBladesSafe),
                          Vanish,
                          Spell.Cast
                              (RogueSpells.Preparation,
                               ret =>
                               PreparationSafe)));
            }
        }

        /// <summary>
        /// Gets a value indicating whether it's safe to Eviscerate.
        /// </summary>
        /// <value>
        ///   <c>true</c> if safe to Eviscerate; otherwise, <c>false</c>.
        /// </value>
        private bool EviscerateSafe
        {
            get
            {
                if (SnDRefreshTime < EnergyCap &&
                     AnticipationCount < 4)
                {
                    return false;
                }

                if (RuptureRefreshTime < EnergyCap &&
                     AnticipationCount < 4)
                {
                    return false;
                }

                if (!CPSafe &&
                     Energy > 85)
                {
                    return true;
                }

                // Never Eviscerate if either Rupture or SnD are not safe.
                if (!RuptureSafe(10) ||
                     !SnDSafe(10))
                {
                    return false;
                }

                // Eviscerate if we're stealthed but combo points are near maxed.
                if (PlayerStealthed &&
                     !CPSafe)
                {
                    return true;
                }

                // If we're too close to SnD's refresh time to max out the combo points after Eviscerate,
                // only Eviscerate if we have them safely in storage.
                if (SnDRefreshTime <= TimeToMaxCP(0) + GCD &&
                     CPSafe)
                {
                    return false;
                }

                // If we're not stealthed, only Eviscerate if we have 5 combo points
                return !PlayerStealthed && Me.ComboPoints == 5;
            }
        }

        /// <summary>
        /// Gets the finisher rotation.
        /// </summary>
        private Composite Finisher
        {
            get
            {
                return new Decorator
                    (cond =>
                     Me.ComboPoints > 0 && !PoolVanish && !PoolShadowDance &&
                     PRSettings.Instance.Rogue.SubtletyRotation == SubtletyRogueRotation.Algorithmic,
                     new PrioritySelector
                         (Spell.Cast
                              (RogueSpells.CrimsonTempest,
                               delegate
                               {
                                   if (Me.ComboPoints < 5 || AoETargets.Count() < 4 || !CrimsonTempestDown ||
                                        (HotkeySettings.Instance.ModeChoice == Mode.Auto && !HotKeyManager.IsAoe))
                                   {
                                       return false;
                                   }

                                   const int energyCost = 10; // Crimson Tempest - Relentless Strikes

                                   if (!RuptureSafe(energyCost))
                                   {
                                       return false;
                                   }

                                   if (!SnDSafe(energyCost))
                                   {
                                       return false;
                                   }

                                   return true;
                               }),
                          Spell.Cast
                              (RogueSpells.Eviscerate,
                               cond => EviscerateSafe),
                          Spell.Cast // Rupture if SnD is falling off first but too close to Rupture.
                              (RogueSpells.Rupture,
                               cond =>
                               Me.ComboPoints == 5 && AnticipationCount > 0 && !SnDSafe(0, forRupture: true) &&
                               SnDTimeLeft < TimeSpan.FromSeconds(3)),
                          Spell.Cast // Rupture if falling off first and enough time to CP SnD
                              (RogueSpells.Rupture,
                               ret => Me.ComboPoints == 5 && SnDSafe(0, true) && RuptureTimeLeft < RuptureTick),
                          Spell.Cast // SnD if Rupture is safe and we're about to fall off.
                              (RogueSpells.SliceAndDice,
                               ret =>
                               (RuptureSafe(Me.ComboPoints == 5 ? 0 : 25, true) && SnDTimeLeft < SnDEnergyTick) ||
                               SnDTimeLeft == TimeSpan.Zero)));
            }
        }

        private Composite FinisherEstimated
        {
            get
            {
                return new Decorator
                    (cond =>
                     Me.ComboPoints > 0 && !PoolVanish && !PoolShadowDance &&
                     PRSettings.Instance.Rogue.SubtletyRotation == SubtletyRogueRotation.Estimated,
                     new PrioritySelector
                         (
                         Spell.Cast
                             (RogueSpells.SliceAndDice,
                              cond =>
                              (SnDTimeLeft < SnDEnergyTick && SnDTimeLeft < RuptureTimeLeft) ||
                              SnDTimeLeft == TimeSpan.Zero),
                         Spell.Cast(RogueSpells.Rupture, cond => UseRupture),
                         Spell.Cast(RogueSpells.Eviscerate, cond => UseEviscerate)));
            }
        }

        /// <summary>
        /// Gets a value indicating whether finishers are safe for another attack.
        /// </summary>
        /// <value>
        ///   <c>true</c> if we have time for another attack; otherwise, <c>false</c>.
        /// </value>
        private bool FinisherSafe
        {
            get
            {
                // Always attack if we have no combo point.
                if (Me.ComboPoints == 0)
                {
                    return true;
                }

                // If SnD or Rupture is going to fall off, don't attack.
                if ((SnDTimeLeft < GCD && SnDTimeLeft > TimeSpan.Zero) ||
                     (Me.ComboPoints == 5 && RuptureTimeLeft < RuptureTick && RuptureTimeLeft > TimeSpan.Zero))
                {
                    return false;
                }

                // Do we have the time and energy for one more before SnD?
                if (SnDTimeLeft < SnDEnergyTick && SnDTimeLeft > GCD &&
                     Energy < 25 + CPBuilderEnergy)
                {
                    return false;
                }

                // Do we have the time and energy for one more before Rupture?
                if (RuptureTimeLeft < RuptureTick && RuptureTimeLeft > GCD &&
                     Energy < 25 + CPBuilderEnergy)
                {
                    return false;
                }

                return true;
            }
        }

        //        private bool HATSafe
        //        {
        //            get
        //            {
        //                if ( !Me.GroupInfo.IsInParty &&
        //                     !Me.GroupInfo.IsInRaid )
        //                {
        //                    return true;
        //                }
        //
        //                if (Me.ComboPoints < 3)
        //                    return true;
        //
        //                return ( Me.ComboPoints == 3 && EnergyCap < TimeSpan.FromSeconds(4) ) ||
        //                       ( Me.ComboPoints == 4 && EnergyCap < HATTick );
        //            }
        //        }

        private bool PoolShadowDance
        {
            get
            {
                if (!_poolShadowDance.HasValue)
                {
                    // Do not pool if Vanish is available.
                    //                    if ( ( !WoWSpell.FromId(RogueSpells.Vanish).Cooldown ||
                    //                           !WoWSpell.FromId(RogueSpells.Preparation).Cooldown ) &&
                    //                         !Me.Aggro && PRSettings.Instance.UseCooldowns &&
                    //                         Me.CurrentTarget.IsBoss() )
                    //                    {
                    //                        _poolShadowDance = false;
                    //                    }
                    //                    else if ( PoolVanish )
                    //                    {
                    //                        _poolShadowDance = false;
                    //                    }
                    // Do not pool if Shadow Dance or Premeditation on cooldown.
                    //                    else
                    if (WoWSpell.FromId(MySpells.Premeditation).Cooldown)
                    {
                        _poolShadowDance = false;
                    }
                    else
                    {
                        //                        TimeSpan poolTime = EnergyCap - SnDEnergyTick;
                        //                        poolTime = poolTime > TimeSpan.Zero ? poolTime : TimeSpan.Zero;
                        //                        poolTime += TimeSpan.FromSeconds(10); // Length of Find Weakness.
                        //
                        //                        _poolShadowDance = SnDRefreshTime > ShadowDanceLength + poolTime &&
                        //                                                RuptureRefreshTime > ShadowDanceLength + poolTime;

                        // According to Ben: do not wait for SnD and Rupture to be "safe"
                        // before using Shadow Dance.
                        var needPooled = 80 - Energy;
                        needPooled = needPooled > 0 ? needPooled : 0;

                        var poolingTime = TimeToRegen(needPooled);

                        if (WoWSpell.FromId(MySpells.ShadowDance).CooldownTimeLeft > poolingTime)
                        {
                            _poolShadowDance = false;
                        }
                        else
                        {
                            poolingTime += TimeSpan.FromSeconds(2);
                            _poolShadowDance = SnDRefreshTime > poolingTime && RuptureRefreshTime > poolingTime;
                        }
                    }
                }

                return _poolShadowDance.Value;
            }
        }

        private bool PoolVanish
        {
            get
            {
                if (!_poolVanish.HasValue)
                {
                    // Only vanish on boss.
                    if (!PRSettings.Instance.UseCooldowns ||
                         !Me.CurrentTarget.IsBoss())
                    {
                        _poolVanish = false;
                    }

                    // Use ShadowDance before Vanish.
                    else if (PoolShadowDance)
                        _poolVanish = false;
                    else if (WoWSpell.FromId(RogueSpells.Vanish).Cooldown ||
                              WoWSpell.FromId(MySpells.Premeditation).Cooldown || PlayerStealthed ||
                              Me.Aggro)
                    {
                        _poolVanish = false;
                    }
                    else
                    {
                        TimeSpan poolTime;
                        TimeSpan vanishLength;

                        if (HasSubterfuge)
                        {
                            poolTime = EnergyCap - TimeSpan.FromSeconds(1);
                            poolTime = poolTime < TimeSpan.Zero ? TimeSpan.Zero : poolTime;
                            vanishLength = TimeSpan.FromSeconds(3);
                        }
                        else
                        {
                            poolTime = Energy > 60 ? TimeSpan.Zero : TimeToRegen(60 - Energy);
                            vanishLength = TimeSpan.FromSeconds(0);
                        }
                        poolTime += vanishLength + TimeSpan.FromSeconds(10); // Find Weakness
                        _poolVanish = SnDRefreshTime > poolTime && RuptureRefreshTime > poolTime;
                    }
                }

                return _poolVanish.Value;
            }
        }

        private Composite RegularRotation
        {
            get
            {
                return new Decorator
                    (cond => !PlayerStealthed,
                     new PrioritySelector
                         (Situationals, Cooldowns, Finisher, FinisherEstimated, ComboPointGen));
            }
        }

        private bool StealthCDSafe
        {
            get
            {
                if (!_stealthCDSafe.HasValue)
                {
                    _stealthCDSafe = !Me.CurrentTarget.HasAura(MySpells.FindWeakness) && !PlayerStealthed;
                }

                return _stealthCDSafe.Value;
            }
        }

        private Composite StealthCPGen
        {
            get
            {
                return new Decorator
                    (cond => CPSafe && !PoolVanish && !PoolShadowDance,
                     new PrioritySelector
                         (Spell.PreventDoubleCast
                              (RogueSpells.Garrote,
                               InstantDoubleCastTimer,
                               ret =>
                               AuraTimeLeft(Me.CurrentTarget, MySpells.FindWeakness) == TimeSpan.Zero && !BehindTarget),
                          Spell.PreventDoubleCast
                              (RogueSpells.Ambush,
                               InstantDoubleCastTimer,
                               ret => BehindTarget),
                          Spell.PreventDoubleCast
                              (MySpells.Hemorrhage,
                               InstantDoubleCastTimer,
                               ret => !BehindTarget)));
            }
        }

        private Composite StealthedRotation
        {
            get
            {
                return new Decorator
                    (cond => PlayerStealthed,
                     new PrioritySelector
                         (Spell.Cast(MySpells.Premeditation, ret => Me.ComboPoints < 4),
                          Finisher,
                          FinisherEstimated,
                          StealthCPGen));
            }
        }

        private bool UseEviscerate
        {
            get
            {
                if (SnDRefreshTime <= TimeToMaxCP(0)) // SnD going to fall off soon.
                {
                    if (HasAnticipation)
                    {
                        if (Me.ComboPoints < AnticipationCount) // We have more Anticipation than combo points.
                        {
                            return true;
                        }
                    }
                }

                if (!CPSafe) // Would waste combo points
                {
                    if (PlayerStealthed) // Stealthed?
                    {
                        if (RuptureSafe(10) &&
                             SnDSafe(10)) // Rupture and SnD
                        {
                            return true;
                        }
                    }
                }

                if (Me.ComboPoints == 5)
                {
                    if (RuptureSafe(10) &&
                         SnDSafe(10))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool UseRupture
        {
            get
            {
                if (Me.ComboPoints != 5)
                {
                    return false;
                }

                if (RuptureTimeLeft == TimeSpan.Zero &&
                     SnDTimeLeft > TimeSpan.Zero)
                {
                    return true;
                }

                if (SnDTimeLeft < RuptureTimeLeft) // SnD going to fall off first
                {
                    if (RuptureRefreshTime - SnDRefreshTime <=
                         TimeToMaxCP(0)) // Can't raise points before Rupture would need to refresh
                    {
                        if (SnDRefreshTime < TimeSpan.FromSeconds(5)) // Don't do it too early.
                        {
                            if (RuptureTimeLeft.Seconds % 2 != 0) // Do it right after a tick.
                            {
                                return true;
                            }
                        }
                    }
                }

                if (RuptureTimeLeft < SnDTimeLeft) // Rupture is going to fall first.
                {
                    if (SnDRefreshTime - RuptureRefreshTime <=
                         TimeToBuildCP()) // Don't have time to gain a combo point after Rupture
                    {
                        if (HasAnticipation && AnticipationCount > 0) // We have waiting Anticipation points
                        {
                            if (RuptureTimeLeft < RuptureTick) // Rupture about to fall off.
                            {
                                return true;
                            }
                        }

                        if (!HasAnticipation) // Don't have anticipation.
                        {
                            if (RuptureTimeLeft < TimeSpan.FromSeconds(4)) // Rupture under 4 seconds.
                            {
                                return true;
                            }
                        }
                    }
                    else if (RuptureTimeLeft < RuptureTick)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the vanish rotation.
        /// </summary>
        /// <value>
        /// The vanish rotation.
        /// </value>
        private Composite Vanish
        {
            get
            {
                return new Decorator
                    (delegate
                         {
                             if (!SpellManager.CanCast(RogueSpells.Vanish))
                             {
                                 return false;
                             }

                             // Ensure energy and cooldowns are ready
                             if (!PoolVanish || !VanishSafe ||
                                  !StealthCDSafe)
                             {
                                 return false;
                             }

                             // Ensure combo points are ready
                             if (Me.ComboPoints > 1 &&
                                  !HasAnticipation)
                             {
                                 return false;
                             }

                             if (HasAnticipation && ComboPoints > 6)
                             {
                                 return false;
                             }

                             // Do not vanish if I have aggro, unless this is a boss.
                             return ((!Me.Aggro) || Me.CurrentTarget.IsBoss());
                         },
                     new Sequence
                         (new Action
                              (delegate
                                   {
                                       SpellManager.Cast(RogueSpells.Vanish);
                                       return RunStatus.Success;
                                   }),
                          new WaitContinue
                              (TimeSpan.FromSeconds(2),
                               cond => PlayerStealthed,
                               new ActionAlwaysSucceed()),
                          new Action
                              (delegate
                                   {
                                       SpellManager.Cast(BehindTarget ? RogueSpells.Ambush : RogueSpells.Garrote);
                                       return RunStatus.Success;
                                   }))
                    );
            }
        }

        private bool VanishSafe
        {
            get
            {
                if (Energy < 60)
                {
                    return false;
                }

                if (HasSubterfuge)
                {
                    return EnergyCap < TimeSpan.FromSeconds(2);
                }

                return Energy >= 60;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Called when Pulse is called by the bot.
        /// </summary>
        internal override void OnPulse()
        {
            StealthedCombat();
        }

        protected override RunStatus Reset()
        {
            _hemoDebuff = null;
            _ruptureRefrehTime = null;
            _ruptureTimeLeft = null;
            _sndRefreshTime = null;
            _sndTimeLeft = null;
            _cpBuilderEnergy = null;
            _cpPerBuilder = null;
            _cpSafe = null;
            _energyRegen = null;
            _poolShadowDance = null;
            _poolVanish = null;
            _stealthCDSafe = null;
            _stealthed = null;

            return base.Reset();
        }

        /// <summary>
        /// Determines if a finisher with the given <paramref name="energyCost"/> energy cost is safe to use before
        /// Rupture has to be refreshed.
        /// </summary>
        /// <param name="energyCost">The energy cost of the finisher, modified for Relentless Strikes.</param>
        /// <param name="fromSnD">Whether we're checking if Rupture is safe after SnD. This is less conservative.</param>
        /// <returns><c>true</c> if the finisher is safe to use before Rupture.</returns>
        private bool RuptureSafe(int energyCost, bool fromSnD = false)
        {
            // We don't care about Slice and Dice here.
            if (SnDTimeLeft < RuptureTimeLeft)
            {
                return true;
            }

            // Total time required to execute finisher and then build 5 combo points
            int savedCP = fromSnD ? 0 : AnticipationCount;
            TimeSpan moveCosts = TimeToRegen(energyCost) + TimeToMaxCP(savedCP) -
                                 TimeToRegen(Energy);

            // Minimum time, counting GCDs
            TimeSpan minTime = TimeSpan.FromSeconds(1 + (5 - savedCP));

            if (RuptureRefreshTime <= moveCosts ||
                 RuptureRefreshTime <= minTime)
            {
                return false;
            }

            if (fromSnD)
            {
                return true;
            }

            TimeSpan minReqTime = TimeToBuildCP();

            TimeSpan gcd = RuptureTick;

            if (minReqTime < gcd)
            {
                minReqTime = gcd;
            }

            // Time enough for a fast refresh of SnD after Rupture?
            return moveCosts + minReqTime < SnDRefreshTime;
        }

        /// <summary>
        /// Determines if a finisher with the given <paramref name="energyCost"/> energy cost is safe to use before
        /// Slice and Dice needs to be refreshed.
        /// </summary>
        /// <param name="energyCost">The energy cost of the finisher, modified for Relentless Strikes.</param>
        /// <param name="fromRupture">Whether we're checking if SnD is safe after Rupture. This is less conservative.</param>
        /// <param name="forRupture">Whether we're checking if SnD is safe right away, before Rupture.</param>
        /// <returns><c>true</c> if the finisher is safe to use before SnD.</returns>
        private bool SnDSafe(int energyCost, bool fromRupture = false, bool forRupture = false)
        {
            if (SnDTimeLeft > RuptureTimeLeft)
            {
                // We don't care about Rupture here.  Let RuptureSafe take care of it.
                return true;
            }

            TimeSpan minReqTime = TimeToRegen(energyCost);

            // If it takes less time to recover energy than the GCD, this move costs a GCD of time.
            if (minReqTime < GCD &&
                 !forRupture)
            {
                minReqTime = GCD;
            }

            if (AnticipationCount == 0 &&
                 !forRupture)
            {
                TimeSpan cpTime = TimeToBuildCP();

                if (cpTime < GCD)
                {
                    cpTime = GCD;
                }

                minReqTime += cpTime;
            }

            if (SnDRefreshTime <= minReqTime)
            {
                //                var log = new StringBuilder();
                //                log.AppendLine("SnDSafe false.");
                //                log.AppendLine("energyCost: " + energyCost);
                //                log.AppendLine("fromRupture: " + fromRupture);
                //                log.AppendLine("forRupture: " + forRupture);
                //                log.AppendLine("minReqTime: " + minReqTime);
                //                Logger.InfoLog(log.ToString());

                return false;
            }

            if (fromRupture)
            {
                return true;
            }

            // Add cost of SnD since we can't rely on Relentless Strikes w/o 5 CP.
            TimeSpan timeToRegen = forRupture ? GCD : TimeToRegen(25);

            if (timeToRegen < GCD)
            {
                timeToRegen = GCD;
            }

            TimeSpan nextMoveCosts = TimeToMaxCP(0) + timeToRegen + minReqTime;

            // Time enough for this, SnD AND Rupture?
            if (RuptureRefreshTime <= nextMoveCosts)
            {
                //                var log = new StringBuilder();
                //                log.AppendLine("SnDSafe false.");
                //                log.AppendLine("energyCost: " + energyCost);
                //                log.AppendLine("fromRupture: " + false);
                //                log.AppendLine("forRupture: " + forRupture);
                //                log.AppendLine("nextMoveCosts: " + nextMoveCosts);
                //                Logger.InfoLog(log.ToString());

                return false;
            }
            return true;
        }

        private void StealthedCombat()
        {
            // Is the target valid?
            if (Me.CurrentTarget == null || Me.CurrentTarget.IsDead ||
                 (!Me.CurrentTarget.IsHostile && !Me.CurrentTarget.IsBoss() ||
                   !Me.CurrentTarget.Attackable))
            {
                return;
            }

            // Are we ready?
            if (!PlayerStealthed || Me.Combat ||
                 !SafeToBreakStealth ||
                 !Me.CurrentTarget.HasAura(2818))
            {
                return;
            }

            // If we're not behind, attempt to shadowstep and wait for next pulse.
            if (SpellManager.HasSpell(RogueSpells.Shadowstep) && !BehindTarget &&
                 SpellManager.CanCast(RogueSpells.Shadowstep, Me.CurrentTarget))
            {
                SpellManager.Cast(RogueSpells.Shadowstep);
            }
            else if (BehindTarget)
            {
                // If we're behind, Ambush
                SpellManager.Cast(RogueSpells.Ambush);
            }
            else if (!BehindTarget &&
                      AuraTimeLeft(Me.CurrentTarget, MySpells.FindWeakness) == TimeSpan.Zero)
            {
                // If we're not behind, but we need Find Weakness, Garrote
                SpellManager.Cast(RogueSpells.Garrote);
            }
            else
            {
                // If we're not behind and Find Weakness is up, Hemorrhage.
                SpellManager.Cast(MySpells.Hemorrhage);
            }
        }

        private TimeSpan TimeToBuildCP(int cpToBuild = 1)
        {
            TimeSpan attackCostTime = TimeToRegen(CPBuilderEnergy);

            if (attackCostTime.TotalSeconds < 1)
            {
                attackCostTime = GCD;
            }

            double hitsPerHAT = 2 / attackCostTime.TotalSeconds;
            int hatProcs = Me.GroupInfo.IsInParty || Me.GroupInfo.IsInRaid
                               ? (int)Math.Floor(cpToBuild / (1 + hitsPerHAT))
                               : 0;
            int hits = (cpToBuild - hatProcs) / CPPerBuilder;
            return (hits * attackCostTime.TotalSeconds) > (hatProcs * 2)
                       ? TimeSpan.FromSeconds(hits * attackCostTime.TotalSeconds)
                       : TimeSpan.FromSeconds(hatProcs * 2);
        }

        //        private static TimeSpan TimeToMaxCP()
        //        {
        //            return TimeToMaxCP(Me.ComboPoints);
        //        }

        private TimeSpan TimeToMaxCP(int startCP)
        {
            int neededCP = 5 - startCP;
            TimeSpan attackCostTime = TimeToRegen(CPBuilderEnergy);

            if (attackCostTime.TotalSeconds < 1)
            {
                attackCostTime = GCD;
            }

            double hitsPerHAT = 2 / attackCostTime.TotalSeconds;
            int hatProcs = Me.GroupInfo.IsInParty || Me.GroupInfo.IsInRaid
                               ? (int)Math.Floor(neededCP / (1 + hitsPerHAT))
                               : 0;
            int hits = (neededCP - hatProcs) / CPPerBuilder;
            return (hits * attackCostTime.TotalSeconds) > (hatProcs * 2)
                       ? TimeSpan.FromSeconds(hits * attackCostTime.TotalSeconds)
                       : TimeSpan.FromSeconds(hatProcs * 2);
        }

        #endregion Methods

        [UsedImplicitly]
        private sealed class MySpells : RogueSpells
        {
            #region Constants and Fields

            public const int FindWeakness = 91023;
            public const int Hemorrhage = 16511;
            public const int HemorrhageDebuff = 89775;
            public const int MasterOfSubtlety = 31223;
            public const int Premeditation = 14183;
            public const int ShadowDance = 51713;

            #endregion Constants and Fields
        }
    }
}