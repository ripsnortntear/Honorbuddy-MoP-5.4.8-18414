#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Rogue/Combat.cs $
 * $LastChangedBy: tumatauenga1980 $
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
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using Lua = PureRotation.Helpers.Lua;

namespace PureRotation.Classes.Rogue
{
    [UsedImplicitly]
    public class Combat : RotationBase
    {
        private static WoWUnit _tricksTarget;
        private static bool _tricksTargetChecked;

        private static Composite HandleCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Killing Spree", ret => Lua.PlayerPower < 35 && Lua.PlayerBuffTimeLeft("Slice and Dice") > 4 && !Me.HasAura(13750)),
                Item.HandleItems(),
                Spell.Cast("Adrenaline Rush", ret => Lua.PlayerPower < 35 || Me.HasAura(121471)),
                Spell.Cast("Shadow Blades", ret => true)
                );
        }

        // Anticipation :: 115189 (5 stacks)
        // Shallow Insight :: 84745
        // Moderate Insight :: 84746
        // Deep Insight :: 84747

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                new Decorator(ret => (HotKeyManager.IsSpecialKey || HotkeySettings.Instance.ModeChoice == Mode.Auto) && PRSettings.Instance.Rogue.UseExposeArmor, new PrioritySelector(Spell.Cast("Expose Armor", ret => TalentManager.HasGlyph("Expose Armor") && !Unit.UnitHasWeakenedArmor(Me.CurrentTarget)))),
                //new Action(delegate { Logger.InfoLog("[RotationInformation]: {0}", "HandleSingleTarget"); return RunStatus.Failure; }),
                Spell.Cast("Tricks of the Trade", u => TricksTarget, ret => TricksTarget != null),
                Spell.Cast("Redirect", ret => Me.RawComboPoints > 0 && Me.ComboPoints < 1),
                Spell.Cast("Ambush", ret => Me.IsStealthed),
                Spell.Cast("Marked for Death",ret=> Me.ComboPoints==0 && !Me.CurrentTarget.HasAura("Marked for Death") && !Spell.SpellOnCooldown("Marked for Death")),                
                Spell.Cast("Recuperate", ret=> PRSettings.Instance.Rogue.UseRecuperate && !Me.HasAura("Recuperate") && Me.HealthPercent <= PRSettings.Instance.Rogue.RecuperatePercent && Me.ComboPoints >= PRSettings.Instance.Rogue.RecuperatePoints),
                Spell.Cast("Revealing Strike", ret => Me.ComboPoints < 5 && !Common.RevealingStrike),
                Spell.Cast("Sinister Strike", ret => Me.ComboPoints < 5),
                Spell.Cast("Eviscerate", ret => Me.ComboPoints >= 5 && Me.HasAura("Slice and Dice")),
                Spell.Cast("Slice and Dice", ret => !Common.SliceAndDiceEnevenom));
        }

        private static Composite HandleSingleTargetRupture()
        {
            return new PrioritySelector(
                new Decorator(ret => (HotKeyManager.IsSpecialKey || HotkeySettings.Instance.ModeChoice == Mode.Auto) && PRSettings.Instance.Rogue.UseExposeArmor, new PrioritySelector(Spell.Cast("Expose Armor", ret => TalentManager.HasGlyph("Expose Armor") && !Unit.UnitHasWeakenedArmor(Me.CurrentTarget)))),
                //new Action(delegate { Logger.InfoLog("[RotationInformation]: {0}", "HandleSingleTargetRupture"); return RunStatus.Failure; }),
                Spell.Cast("Tricks of the Trade", u => TricksTarget, ret => TricksTarget != null),
                Spell.Cast("Redirect", ret => Me.RawComboPoints > 0 && Me.ComboPoints < 1),
                Spell.Cast("Ambush", ret => Me.IsStealthed),
                Spell.Cast("Marked for Death",ret=> Me.ComboPoints==0 && !Me.CurrentTarget.HasAura("Marked for Death") && !Spell.SpellOnCooldown("Marked for Death")),                
                Spell.Cast("Recuperate", ret => PRSettings.Instance.Rogue.UseRecuperate && !Me.HasAura("Recuperate") && Me.HealthPercent <= PRSettings.Instance.Rogue.RecuperatePercent && Me.ComboPoints >= PRSettings.Instance.Rogue.RecuperatePoints),
                Spell.Cast("Revealing Strike", ret => Me.ComboPoints < 5 && !Common.RevealingStrike),
                Spell.Cast("Sinister Strike", ret => Me.ComboPoints < 5),
                Spell.Cast("Rupture", ret => Me.ComboPoints == 5 && Me.HasAura("Deep Insight")),
                Spell.Cast("Eviscerate", ret => Me.ComboPoints >= 5 && Me.HasAura("Slice and Dice")),
                Spell.Cast("Slice and Dice", ret => !Common.SliceAndDiceEnevenom));
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                Spell.Cast("Blade Flurry",ret=> PRSettings.Instance.Rogue.CombatUseBladeFlurry && Me.IsWithinMeleeRange && Unit.AttackableMeleeUnits.Count()>=PRSettings.Instance.Rogue.CombatBladeFlurryCount),
                Spell.Cast("Fan of Knives", ret => Me.IsWithinMeleeRange && Me.ComboPoints <= 4),
                Spell.Cast("Crimson Tempest", ret => Me.ComboPoints > 4)
                );
        }

        #region Tricks of Trade

        private static WoWUnit TricksTarget
        {
            get
            {
                if (_tricksTargetChecked)
                {
                    return _tricksTarget;
                }

                if (_tricksTarget == null)
                {
                    _tricksTarget = Unit.BestTricksTarget;
                    _tricksTargetChecked = true;
                }

                return _tricksTarget;
            }
        }

        #endregion Tricks of Trade

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.RogueCombat; }
        }
        internal override string Help
        {
            get
            {
                return
                    @"
                     -----------------------------------------------
                     Special Key: Assigned to Expose Armor
                     Rotation Key: Switch between PVP and Rupture Rotation
                     Details:
                        Expose Armor is casted when
                        - Special Key is pressed once (enabling) AND
                        - Glyph Expose Armor is used in your talents screen AND
                        - Expose Armor is enabled in the Settings AND
                        - unit has no debuff for weakened armor

                        Rupture is used when
                        - Combo Points = 5 AND
                        - Aura Deep Inside is up AND
                        - PveRupture is set as rotation in the Rogue Settings AND
                        - Roation Key is pressed once (second press returns back to default rotation)
                     -----------------------------------------------
                     ";
            }
        }

        public override string Name
        {
            get { return "Combat Rogue"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()), //dont continue down the tree if paused or rotation selection enabled.
                    Racials.UseRacials(),
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    EncounterSpecific.HandleActionBarInterrupts(),                    
                    new Decorator(ret => HotKeyManager.IsRotationKey, RotationSelection),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleCooldowns()),
                                      new Decorator(
                                          ret =>
                                          PRSettings.Instance.UseAoEAbilities &&
                                          Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Rogue.AoECount,
                                          HandleAoeCombat()),
                                          HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                                      new Decorator(
                                          ret =>
                                          PRSettings.Instance.UseAoEAbilities &&
                                          Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Rogue.AoECount,
                                          HandleAoeCombat()),
                                          HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey && !HotKeyManager.IsRotationKey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))));
            }
        }

        private Composite PVERotationRupture
        {
            get
            {

                return new PrioritySelector(
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                                      new Decorator(
                                          ret =>
                                          PRSettings.Instance.UseAoEAbilities &&
                                          Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Rogue.AoECount,
                                          HandleAoeCombat()),
                                          HandleSingleTargetRupture())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTargetRupture())))
                                      );
                /*
                return new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTargetRupture()));
                 */
            }
        }

        private Composite RotationSelection
        {
            get
            {
                // You can add as many different composite rotations in here, it will toggle from PVE (default) to the composite you select within the GUI provided the hotkey is enabled. -- wulf
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsRotationKey,
                                  new PrioritySelector(
                                      //new Action(delegate { Logger.InfoLog("[RotationInformation]: user rotation activated {0}, activated rotation: {1}", HotKeyManager.IsRotationKey, PRSettings.Instance.Rogue.RotationSelections); return RunStatus.Failure; }),
                                      new Decorator(ret => PRSettings.Instance.Rogue.RotationSelections == RogueSettings.RotationSelection.PvP, PVPRotation))),
                                      new Decorator(ret => PRSettings.Instance.Rogue.RotationSelections == RogueSettings.RotationSelection.PvERupture, PVERotationRupture)
                    );
            }
        }

        public override Composite PVPRotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()), //dont continue down the tree if paused or rotation selection enabled.
                    Racials.UseRacials(),
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                        new PrioritySelector(
                            new Decorator(ret => Me.CurrentTarget.IsBoss(), HandleCooldowns()),
                            new Decorator( ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Rogue.AoECount, HandleAoeCombat()),
                            HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                        new PrioritySelector(
                            new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                            new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Rogue.AoECount, HandleAoeCombat()),
                            HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey && !HotKeyManager.IsRotationKey,
                        new PrioritySelector(
                            new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                            new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                            new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))));
            }
        }

        public override Composite Medic
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => Me.HealthPercent < 100,
                                  new PrioritySelector()));
            }
        }

        public override Composite PreCombat
        {
            get

            { return null; }
        }

        #endregion Overrides of RotationBase
    }
}