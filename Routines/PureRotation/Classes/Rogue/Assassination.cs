#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Rogue/Assassination.cs $
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
    public class Assassination : RotationBase
    {
        //private static WoWUnit _tricksTarget;
        //private static bool _tricksTargetChecked;
        private static RogueSettings RogueSettings { get { return PRSettings.Instance.Rogue; } }

        private static Composite HandleCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Vendetta", ret => !Me.IsStealthed && Me.IsWithinMeleeRange),
                Item.HandleItems(),
                Spell.Cast("Shadow Blades", ret => Spell.GetAuraTimeLeft("Slice and Dice") < 12 && Me.IsWithinMeleeRange)
                );
        }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Recuperate", ret => RogueSettings.UseRecuperate && Me.ComboPoints >= RogueSettings.RecuperatePoints && Me.HealthPercent < RogueSettings.RecuperatePercent)
                );
        }

        //SPELLID Blindside :: 121153
        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                new Decorator(ret => HotKeyManager.IsSpecialKey, new PrioritySelector(Spell.Cast("Tricks of the Trade", u => TricksTarget, ret => TricksTarget != null))),
                new Decorator(ret => !Me.CurrentTarget.HasMyAura("Rupture"),
                new PrioritySelector(
                    Spell.Cast("Mutilate", ret => Lua.PlayerComboPts < 5),
                    Spell.Cast("Dispatch", ret => Lua.PlayerComboPts < 5 && Me.HasAura(121153)),
                    Spell.Cast("Rupture", ret => Lua.PlayerComboPts > 4)
                    )),
                new Decorator(ret => !Me.HasAura(5171) && Me.CurrentTarget.HasMyAura("Rupture"),
                    new PrioritySelector(
                        Spell.Cast("Mutilate", ret => Lua.PlayerComboPts < 2),
                        Spell.Cast("Slice and Dice", ret => Lua.PlayerComboPts > 1 && Me.CurrentTarget.HasMyAura("Rupture") && !Me.HasAura(5171))
                        )),
                Spell.Cast("Redirect", ret => Me.RawComboPoints > 0 && Lua.PlayerComboPts < 1),
                Spell.Cast("Expose Armor", ret => RogueSettings.UseExposeArmor && !Unit.UnitHasWeakenedArmor(Me.CurrentTarget)),
                Spell.Cast("Marked for Death", ret => Me.ComboPoints == 0 && !Me.CurrentTarget.HasAura("Marked for Death") && !Spell.SpellOnCooldown("Marked for Death")),
                Spell.Cast("Dispatch", ret => (Me.CurrentTarget.HealthPercent < 35 && Lua.PlayerComboPts < 5) || (Me.HasAura(5171) && Me.HasAura(121153))),
                Spell.Cast("Mutilate", ret => Me.IsStealthed && Me.IsBehind(Me.CurrentTarget) || Me.HasAura(108209) || (!Me.HasAura(121153) && Lua.PlayerComboPts < 5 && Me.CurrentTarget.HealthPercent > 35)),
                Spell.Cast("Rupture", ret => (Lua.PlayerComboPts > 1 && Spell.GetMyAuraTimeLeft("Rupture", Me.CurrentTarget) < 3) || (Lua.PlayerComboPts >= 5 && !Common.RuptureGoingUp)),
                Spell.Cast("Envenom", ret => Lua.PlayerComboPts > 4 && Me.HasAura("Slice and Dice")),
                Spell.Cast("Envenom", ret => Lua.PlayerComboPts >= 2 && !Common.SliceAndDiceEnevenom));
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                Spell.Cast("Fan of Knives", ret => Me.IsWithinMeleeRange && Lua.PlayerComboPts < 5),
                Spell.Cast("Crimson Tempest", ret => Lua.PlayerComboPts > 4)
                );
        }

        #region Tricks of Trade

        private static WoWUnit TricksTarget
        {
            get
            {
                return Rogue.BestTricksTarget;
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
            get { return WoWSpec.RogueAssassination; }
        }

        public override string Name
        {
            get { return "Assasination Rogue"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    Racials.UseRacials(),
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    HandleDefensiveCooldowns(), // Stay Alive!
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
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities &&
                                          Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Rogue.AoECount,
                                          HandleAoeCombat()),
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
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