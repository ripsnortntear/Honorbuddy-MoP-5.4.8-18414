#region Revision info

/*
 * $Author: Alex & Wulf$
 * $Date: 2013-09-24 21:39:17 +0200 (Di, 24 Sep 2013) $
 * $ID$
 * $Revision: 1$
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Druid/Guardian.cs $
 * $LastChangedBy:  Wulf$
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
using Styx.TreeSharp;

namespace PureRotation.Classes.Druid
{
    [UsedImplicitly]
    internal class Guardian : RotationBase
    {

        /* UPDATES: 6/6/13
         * Fixed Thrash - Spell.Cast is not checking the CD on Thrash
         * Fixed Faerie Fire - Corrected useage in single target to match Inc. Bear
         * Updated AoE rotation - should provide much better threat
         * Added Nature's Vigial - either cast when we're low on HP (based on SI) or Berserking (for threat)
         * Berserk is now only used in Auto mode or if you have cooldowns enabled
        */

        /* TO DO
         * Find a good solution for auto FR based on FR math
         * Change default FR settings (80 dungeon / 60 lfr)
        */

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(

                Spell.Cast("Nature's Vigil", ret => Me.HasAura("Survival Instincts") || Me.HasAura("Berserk")),

                Spell.Cast("Might of Ursoc", ret => Me, ret => Me.HealthPercent <= PRSettings.Instance.Druid.UrsocHP),
                Spell.Cast("Survival Instincts", ret => Me, ret => Me.HealthPercent <= PRSettings.Instance.Druid.SiHP),

                // Smarter FR - will not cast unless we're going to maximize the heal based on FR math. Still WIP.
                //Spell.Cast("Frenzied Regeneration", ret => Me, ret => !Me.HasAura("Vengeance") && Me.HealthPercent < StyxWoW.Me.Stamina * 2.5 && Me.HealthPercent <= PRSettings.Instance.Druid.FrenziedHP),
                //Spell.Cast("Frenzied Regeneration", ret => Me, ret => Me.HasAura("Vengeance") && Me.HealthPercent < (StyxWoW.Me.AttackPower * 2.2 - StyxWoW.Me.Agility * 4.4) && Me.HealthPercent <= PRSettings.Instance.Druid.FrenziedHP),

                Spell.Cast("Frenzied Regeneration", ret => Me, ret => Me.HealthPercent <= PRSettings.Instance.Druid.FrenziedHP && Lua.PlayerPower >= 59 && !StyxWoW.Me.HasAura("Frenzied Regeneration")),
                Spell.Cast("Savage Defense", ret => Me, ret => !Me.HasAura("Savage Defense") && Me.HealthPercent > PRSettings.Instance.Druid.FrenziedHP),

                Spell.Cast("Barkskin", ret => Me, ret => Me.HealthPercent <= PRSettings.Instance.Druid.BarkskinHP),

                Spell.Cast("Cenarion Ward", ret => TalentManager.HasTalent(6) && Me.HealthPercent <= PRSettings.Instance.Druid.SiHP),
                Spell.Cast("Renewal", ret => Me, ret => Me.HealthPercent <= PRSettings.Instance.Druid.RenewalHP),

                // Symbiosis: Death Knight - Bone Shield
                Spell.Cast(122285, ret => !Me.HasAura("Bone Shield") && Me.HasAura("Symbiosis") && !Spell.SpellOnCooldown(122285)),
                // Symbiosis: Monk - Elusive Brew
                Spell.Cast(126453, ret => Me.HasAura("Symbiosis") && !Spell.SpellOnCooldown(126453)),

                Spell.Cast("Enrage", ret => Me, ret => Lua.PlayerPower < 40)

                );
        }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
               Spell.Cast("Bear Form", req => Me.Shapeshift != ShapeshiftForm.Bear && Me.Shapeshift != ShapeshiftForm.DireBear),
               Spell.Cast("Berserk", ret => HotKeyManager.IsCooldown || (HotkeySettings.Instance.ModeChoice == Mode.Auto && Me.CurrentTarget.IsBoss)),
               Spell.Cast("Mangle"),
               Spell.Cast("Lacerate"),
               Spell.Cast("Faerie Fire", ret => Spell.GetMyAuraTimeLeft(77758, StyxWoW.Me.CurrentTarget) >= 6),
               Spell.Cast("Thrash", ret => !Spell.HasMyAura("Thrash", StyxWoW.Me.CurrentTarget) && Spell.GetSpellCooldown("Thrash").TotalSeconds == 0),
               Spell.Cast("Maul", ret => Me.HasAura("Tooth and Claw") && Me.CurrentRage > 90 && Me.HealthPercent >= 80),
               Spell.Cast("Faerie Fire")

               );
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
               Spell.Cast("Bear Form", req => Me.Shapeshift != ShapeshiftForm.Bear && Me.Shapeshift != ShapeshiftForm.DireBear),
                Spell.Cast("Berserk", ret => HotKeyManager.IsCooldown || (HotkeySettings.Instance.ModeChoice == Mode.Auto)),
                Spell.Cast("Mangle"),
                Spell.Cast("Thrash", ret => Spell.GetSpellCooldown("Thrash").TotalSeconds == 0),
                Spell.Cast("Swipe"),
                Spell.Cast("Maul", ret => TalentManager.HasGlyph("Maul") && Me.HasAura("Tooth and Claw") && Me.CurrentRage > 90),
                // Symbiosis: Paladin - Consecration
                Spell.Cast(110701, ret => Me.HasAura("Symbiosis") && !Spell.SpellOnCooldown(110701))

                );
        }

        #region booleans

        private static bool WaitForSpeedBuff { get { return Me.CurrentTarget != null && !Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Timewarp"); } }

        #endregion booleans

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1736 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.DruidGuardian; }
        }

        public override string Name
        {
            get { return "Druid Guardian"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    Racials.UseRacials(),
                    HandleDefensiveCooldowns(), // Stay Alive!
                    Item.HandleItems(),         // Pop Trinkets, Drink potions...
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Druid.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Druid.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
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
                        ));
            }
        }

        #endregion Overrides of RotationBase
    }
}