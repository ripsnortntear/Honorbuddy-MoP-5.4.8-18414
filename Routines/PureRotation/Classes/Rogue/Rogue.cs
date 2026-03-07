#region Revision info

/*
 * $Author: millz $
 * $Date: 2013-05-17 10:23:04 +0200 (Fr, 17 Mai 2013) $
 * $ID$
 * $Revision: 1424 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Rogue/Rogue.cs $
 * $LastChangedBy: millz $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Collections.Generic;
using System.Linq;

using CommonBehaviors.Actions;

using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;

using Styx;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using Action = Styx.TreeSharp.Action;
using Lua = Styx.WoWInternals.Lua;

namespace PureRotation.Classes.Rogue
{
    internal abstract class Rogue : RotationBase
    {
        #region Constants and Fields

        protected const double InstantDoubleCastTimer = 0.5;
        private static int? _anticipationCount;

        private static IEnumerable<WoWUnit> _aoeTargets;
        private static bool? _behindTarget;

        private static int? _energy;

        private static WoWUnit _tricksTarget;

        private static bool _ttLoaded;
        private TimeSpan? _energyCap;

        #endregion Constants and Fields

        #region Public Properties

        public override Composite Medic
        {
            get
            {
                return new Decorator
                    (ret => Me.HealthPercent < 100 && PRSettings.Instance.EnableSelfHealing,
                     new PrioritySelector
                         (Item.UseBagItem(5512, ret => Me.HealthPercent < 40, "Healthstone"),
                          Spell.Cast
                              (RogueSpells.Recuperate,
                               reqs =>
                               PRSettings.Instance.Rogue.UseRecuperate &&
                               Me.HealthPercent <= PRSettings.Instance.Rogue.RecuperatePercent),
                          Spell.Cast
                              (RogueSpells.SmokeBomb,
                               ret =>
                               Me.CurrentTarget != null && Me.HealthPercent < 30 && Me.CurrentTarget.IsTargetingMeOrPet),
                          Spell.Cast
                              (RogueSpells.CombatReadiness,
                               ret =>
                               Me.CurrentTarget != null && Me.HealthPercent < 40 && Me.CurrentTarget.IsTargetingMeOrPet),
                          Spell.Cast
                              (RogueSpells.Evasion,
                               ret =>
                               Me.HealthPercent < 35 &&
                               Unit.AttackableMeleeUnits.Count(u => u.DistanceSqr < 6 * 6 && u.IsTargetingMeOrPet) >= 1),
                          Spell.Cast
                              (RogueSpells.CloakOfShadows,
                               ret => Unit.AttackableMeleeUnits.Count(u => u.IsTargetingMeOrPet && u.IsCasting) >= 1),
                          ApplyPoisons));
            }
        }

        public override Composite PreCombat
        {
            get { return new Decorator(ApplyPoisons); }
        }

        #endregion Public Properties

        #region Properties

        protected static int AnticipationCount
        {
            get
            {
                if (_anticipationCount.HasValue)
                {
                    return _anticipationCount.Value;
                }

                if (!HasAnticipation)
                {
                    _anticipationCount = 0;
                    return _anticipationCount.Value;
                }

                _anticipationCount = Me.HasAura(RogueSpells.AnticipationBuff)
                                         ? (int)Me.GetAuraById(RogueSpells.AnticipationBuff).StackCount
                                         : 0;

                return _anticipationCount.Value;
            }
        }

        /// <summary>
        ///     Gets the list of targets we're about to hit w/ an AoE.
        /// </summary>
        protected static IEnumerable<WoWUnit> AoETargets
        {
            get
            {
                if (_aoeTargets == null)
                {
                    try
                    {
                        _aoeTargets = Unit.AttackableUnits.ToList().Where
                            (unit => unit.DistanceSqr < 64 && unit.InLineOfSpellSight);
                        var woWUnits = _aoeTargets as IList<WoWUnit> ?? _aoeTargets.ToList();
                        _aoeTargets = woWUnits.Any
                                          (unit =>
                                           unit.IsCrowdControlled() || !unit.Combat)
                                          ? new List<WoWUnit>()
                                          : woWUnits;
                    }
                    catch
                    {
                        _aoeTargets = new HashSet<WoWUnit>();
                    }
                }
                return _aoeTargets;
            }
        }

        /// <summary>
        ///     Applies the poisons.
        /// </summary>
        /// <returns></returns>
        private static Composite ApplyPoisons
        {
            get
            {
                return new PrioritySelector
                    (new Decorator
                         (ret =>
                          NeedsPoison && !StyxWoW.Me.HasAura(MainHandPoison) && SpellManager.HasSpell(MainHandPoison),
                          new Sequence
                              (new Action
                                   (ret =>
                                    Logger.InfoLog
                                        ("Applying {0} to main hand", PRSettings.Instance.Rogue.MainHandPoison)),
                               new Action(ret => Navigator.PlayerMover.MoveStop()),
                               Spell.CreateWaitForLagDuration(),
                               new Action(ret => SpellManager.CastSpellById(MainHandPoison)),
                               Spell.CreateWaitForLagDuration(),
                               new WaitContinue(2, ret => StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                               new WaitContinue(10, ret => !StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                               new WaitContinue(1, ret => false, new ActionAlwaysSucceed()))),
                     new Decorator
                         (ret =>
                          NeedsPoison && !StyxWoW.Me.HasAura(OffHandHandPoison) &&
                          SpellManager.HasSpell(OffHandHandPoison),
                          new Sequence
                              (new Action
                                   (ret =>
                                    Logger.InfoLog("Applying {0} to off hand", PRSettings.Instance.Rogue.OffHandPoison)),
                               new Action(ret => Navigator.PlayerMover.MoveStop()),
                               Spell.CreateWaitForLagDuration(),
                               new Action(ret => SpellManager.CastSpellById(OffHandHandPoison)),
                               Spell.CreateWaitForLagDuration(),
                               new WaitContinue(2, ret => StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                               new WaitContinue(10, ret => !StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                               new WaitContinue(1, ret => false, new ActionAlwaysSucceed()))));
            }
        }

        /// <summary>
        ///     Gets a value indicating whether we're behind the current target..
        /// </summary>
        /// <value>
        ///     <c>true</c> if we're behind our target; otherwise, <c>false</c>.
        /// </value>
        protected static bool BehindTarget
        {
            get
            {
                if (_behindTarget.HasValue)
                {
                    return _behindTarget.Value;
                }

                _behindTarget = Me.CurrentTarget != null && Me.IsBehind(Me.CurrentTarget);
                return _behindTarget.Value;
            }
        }

        protected static int ComboPoints
        {
            get { return Me.ComboPoints + AnticipationCount; }
        }

        protected static bool CrimsonTempestDown
        {
            get { return AoETargets.Any(x => !x.HasAura(RogueSpells.CrimsonTempest)); }
        }

        /// <summary>
        ///     Gets our current energy.
        /// </summary>
        protected static int Energy
        {
            get
            {
                if (!_energy.HasValue)
                {
                    _energy = Lua.GetReturnVal<int>("return UnitPower(\"player\");", 0);
                }

                return _energy.Value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether our rogue has the anticipation talent.
        /// </summary>
        /// <value>
        ///     <c>true</c> if we have anticipation; otherwise, <c>false</c>.
        /// </value>
        protected static bool HasAnticipation
        {
            get { return TalentManager.HasTalent(18); }
        }

        protected static bool HasSubterfuge
        {
            get { return TalentManager.HasTalent(2); }
        }

        /// <summary>
        ///     Gets a rotation of moves that are very situation-dependant.
        /// </summary>
        protected static Composite Situationals
        {
            get
            {
                return new PrioritySelector
                    (Spell.Cast
                         (RogueSpells.TricksOfTheTrade,
                          u => TricksTarget,
                          ret => PRSettings.Instance.Rogue.UseTricksOfTheTrade),
                     Spell.InterruptSpellCasts(onUnit => Me.CurrentTarget),
                     Spell.Cast(RogueSpells.Redirect, ret => Me.RawComboPoints > 0 && Me.ComboPoints < 1),
                     Spell.Cast(RogueSpells.Shiv, cond => TargetEnraged));
            }
        }

        /// <summary>
        ///     Gets a value indicating whether anticipation is safe for another combo point.
        /// </summary>
        /// <value>
        ///     <c>true</c> if anticipation is safe; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool CPSafe
        {
            get { return HasAnticipation ? (ComboPoints < 10) : Me.ComboPoints < 5; }
        }

        /// <summary>
        ///     Calculate time to energy cap.
        /// </summary>
        protected TimeSpan EnergyCap
        {
            get
            {
                if (_energyCap.HasValue)
                {
                    return _energyCap.Value;
                }

                try
                {
                    _energyCap = TimeToRegen((int)(Me.MaxEnergy - Energy));
                    return _energyCap.Value;
                }
                catch
                {
                    Logger.FailLog(" Calculation Failed in TimetoEnergyCap");
                    return TimeSpan.Zero;
                }
            }
        }

        /// <summary>
        ///     Returns information about the player's mana/energy/etc regeneration rate
        /// </summary>
        protected virtual float EnergyRegen
        {
            get
            {
                try
                {
                    return Lua.GetReturnVal<float>("return GetPowerRegen()", 1);
                }
                catch
                {
                    Logger.FailLog(" Lua Failed in EnergyRegen");
                    return 0;
                }
            }
        }

        /// <summary>
        ///     Gets whether or not the player is stealthed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if we're stealthed; otherwise, <c>false</c>.
        /// </value>
        protected virtual bool PlayerStealthed
        {
            get
            {
                return Me.IsStealthed || Me.Auras.Any(x => x.Value.SpellId == 115191) ||
                       (HasSubterfuge && Me.GetAuraById(RogueSpells.Subterfuge).IsActive);
            }
        }

        /// <summary>
        ///     Gets the best tricks target.
        /// </summary>
        public static WoWUnit BestTricksTarget
        {
            get
            {
                if (!StyxWoW.Me.GroupInfo.IsInParty &&
                     !StyxWoW.Me.GroupInfo.IsInRaid)
                {
                    return null;
                }

                // If the player has a focus target set, use it instead.
                if (StyxWoW.Me.FocusedUnitGuid != 0)
                {
                    return StyxWoW.Me.FocusedUnit;
                }

                if (RaFHelper.Leader != null &&
                     !RaFHelper.Leader.IsMe)
                {
                    // Leader first, always. Otherwise, pick a rogue/DK/War pref. Fall back to others just in case.
                    return RaFHelper.Leader;
                }

                WoWPlayer bestTank =
                    StyxWoW.Me.GroupInfo.RaidMembers.Where
                        (member => member.HasRole(WoWPartyMember.GroupRole.Tank) && member.Health > 0).Select
                        (member => member.ToPlayer()).Where(player => player != null).OrderBy
                        (player => player.DistanceSqr).FirstOrDefault();

                if (bestTank != null)
                {
                    return bestTank;
                }

                WoWPlayer bestPlayer =
                    StyxWoW.Me.GroupInfo.RaidMembers.Where
                        (member => member.HasRole(WoWPartyMember.GroupRole.Damage) && member.Health > 0).Select
                        (member => member.ToPlayer()).Where(player => player != null).OrderBy
                        (player => player.DistanceSqr).FirstOrDefault();

                return bestPlayer;
            }
        }

        /// <summary>
        ///     Gets the main hand poison.
        /// </summary>
        private static int MainHandPoison
        {
            get
            {
                switch (PRSettings.Instance.Rogue.MainHandPoison)
                {
                    case MHPoisonType.Wound:
                        return 8679;
                    case MHPoisonType.Deadly:
                        return 2823;
                    default:
                        return 0;
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether we need to apply poison to our MH weapon.
        /// </summary>
        /// <value>
        ///     <c>true</c> if we need to apply poison; otherwise, <c>false</c>.
        /// </value>
        private static bool NeedsPoison
        {
            get
            {
                return StyxWoW.Me.Inventory.Equipped.MainHand != null &&
                       StyxWoW.Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 0 &&
                       StyxWoW.Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass != WoWItemWeaponClass.FishingPole;
            }
        }

        /// <summary>
        ///     Gets the off hand hand poison.
        /// </summary>
        private static int OffHandHandPoison
        {
            get
            {
                switch (PRSettings.Instance.Rogue.OffHandPoison)
                {
                    case OHPoisonType.MindNumbing:
                        return 5761;
                    case OHPoisonType.Crippling:
                        return 3408;
                    case OHPoisonType.Paralytic:
                        return 108215;
                    case OHPoisonType.Leeching:
                        return 108211;
                    default:
                        return 0;
                }
            }
        }

        private static bool TargetEnraged
        {
            get { return Me.CurrentTarget.Auras.Any(x => x.Value.Spell.Mechanic == WoWSpellMechanic.Enraged); }
        }

        /// <summary>
        ///     Gets the tricks target.
        /// </summary>
        private static WoWUnit TricksTarget
        {
            get
            {
                if (!_ttLoaded)
                {
                    _tricksTarget = BestTricksTarget;
                    _ttLoaded = true;
                }

                return _tricksTarget;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Auras the time left.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="auraId">The aura id.</param>
        /// <returns></returns>
        protected static TimeSpan AuraTimeLeft(WoWUnit source, int auraId)
        {
            if (!source.HasAura(auraId))
            {
                return TimeSpan.Zero;
            }

            return source.GetAuraById(auraId).TimeLeft;
        }

        /// <summary>
        ///     Gets the reset action, which resets all of our "ticking" variables.
        /// </summary>
        protected virtual RunStatus Reset()
        {
            _anticipationCount = null;
            _energy = null;
            _tricksTarget = null;
            _aoeTargets = null;
            _ttLoaded = false;
            _behindTarget = null;
            _energyCap = null;

            return RunStatus.Failure;
        }

        /// <summary>
        ///     Calculates how long it would take to regenerate the given amount of energy.
        /// </summary>
        /// <param name="energy">The amount of energy to regenerate.</param>
        /// <returns>
        ///     <see cref="TimeSpan" /> of how long it would take to regenerate <paramref name="energy" />-amounts of energy.
        /// </returns>
        protected TimeSpan TimeToRegen(int energy)
        {
            return TimeSpan.FromSeconds(energy / EnergyRegen);
        }

        #endregion Methods

        /// <summary>
        ///     List of all base rogue spells.
        /// </summary>
        protected class RogueSpells
        {
            #region Constants and Fields

            public const int Ambush = 8676;

            //            public const int Anticipation = 114015;
            public const int AnticipationBuff = 115189;

            public const int Backstab = 53;
            public const int CloakOfShadows = 31224;
            public const int CombatReadiness = 74001;
            public const int CrimsonTempest = 121411;
            public const int Evasion = 5277;
            public const int Eviscerate = 2098;
            public const int FanOfKnives = 51723;

            //            public const int Feint = 1966;
            public const int Garrote = 703;

            //            public const int Kick = 1766;
            public const int Preparation = 14185;

            public const int Recuperate = 73651;
            public const int Redirect = 73981;
            public const int Rupture = 1943;
            public const int ShadowBlades = 121471;
            public const int Shadowstep = 36554;
            public const int Shiv = 5938;
            public const int SliceAndDice = 5171;
            public const int SmokeBomb = 76577;
            public const int Subterfuge = 108208;
            public const int TricksOfTheTrade = 57934;
            public const int Vanish = 1856;

            //            public const int VanishBuff = 115193;

            #endregion Constants and Fields
        }
    }
}