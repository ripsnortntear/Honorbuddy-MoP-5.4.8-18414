#region Revision info
/*
 * $Author: Alex & Wulf$
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1$
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Mage/Arcane.cs $
 * $LastChangedBy:  Wulf$
 * $ChangesMade: $
 */
#endregion

using System.Linq;
using CommonBehaviors.Actions;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Managers;
using PureRotation.Helpers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;

namespace PureRotation.Classes.Mage
{
    [UsedImplicitly]
    class Arcane : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Common.HandleCommonDefensiveCooldowns());
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                Common.HandleCommonOffensiveCooldowns(),
                Spell.PreventDoubleCast("Alter Time", 5, ret => NeedAlterTime),
                Spell.Cast("Presence of Mind", ret => UseAlterTime && NeedPresenceofMind),
                Spell.Cast("Arcane Power", on => Me, ret => NeedArcanePower));
        }

        private static Composite HandleSingleTarget()
        {

            return new PrioritySelector(
                Common.HandleSingleTargetMageBombs(),
                new Decorator(ret => MageSettings.MultiDoTSingleTarget, Common.HandleBombMultiDoT()),
                Spell.ChannelSpell("Arcane Missiles", on => Me.CurrentTarget, ret => NeedArcaneMissiles),
                Spell.Cast("Arcane Barrage", ret => Spell.GetAuraStackCount("Arcane Charge") > 3),
                Spell.Cast("Arcane Blast"));
        }

        private static Composite AoESmallPack()
        {
            return new PrioritySelector(
                Spell.Cast("Arcane Barrage", ret => Spell.GetAuraStackCount("Arcane Charge") > 3),
                Spell.CastOnGround("Flamestrike", on => Me.CurrentTarget.Location, ret => true),
                Common.HandleBombMultiDoT(),
                HandleSingleTarget()
                );
        }

        private static Composite AoEBigPack()
        {
            return new PrioritySelector(
                Spell.CastOnGround("Flamestrike", on => Me.CurrentTarget.Location, ret => true),
                Common.HandleBombMultiDoT(),
                Spell.Cast("Arcane Explosion"));
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                new Decorator(ret => Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() < 5, AoESmallPack()),
                new Decorator(ret => Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() >= 5, AoEBigPack()));
        }


        #region booleans

        private static bool NeedArcaneMissiles { get { return Spell.GetAuraStackCount("Arcane Missiles!") == 2 || Spell.GetAuraStackCount("Arcane Charge") >= 5; } }
        private static bool NeedArcanePower { get { return !Me.HasAura("Arcane Power") && Me.HasAura(116014) && Spell.GetAuraStackCount("Arcane Charge") > 2; } }
        private static bool NeedAlterTime { get { return UseAlterTime && Me.HasAnyAura(116257, 116014) && Me.HasAura("Arcane Power") && Spell.GetAuraStackCount("Arcane Missiles!") == 2 || Spell.GetAuraStackCount("Arcane Charge") >= 3 && !Me.HasAura("Alter Time"); } }
        private static bool NeedPresenceofMind { get { return UsePresenceofMind && StyxWoW.Me.ActiveAuras.ContainsKey("Pyroblast!") && !Spell.SpellOnCooldown("Alter Time"); } }

        #endregion

        #region Settings

        private static MageSettings MageSettings { get { return PRSettings.Instance.Mage; } }
        private static bool UseAlterTime { get { return MageSettings.EnableAlterTime; } }
        private static bool UsePresenceofMind { get { return MageSettings.EnablePresenceofMind; } }

        #endregion

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.MageArcane; }
        }

        public override string Name
        {
            get { return "Arcane Mage"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => (MageSettings.EnableLagFix && SpellManager.GlobalCooldown) || HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    HandleDefensiveCooldowns(), // Stay Alive! 
                    Racials.UseRacials(),
                    Item.HandleItems(),         // Pop Trinkets, Drink potions...
                    Common.HandleCommon(),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget != null && (Me.CurrentTarget.IsBoss() && !EncounterSpecific.HasBadAura), HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() > PRSettings.Instance.Mage.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),

                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsSpecialKey, Spell.Cast("Time Warp")),
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() > PRSettings.Instance.Mage.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),

                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsSpecialKey, Spell.Cast("Time Warp")),
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
                        Common.HandleCommonPreCombat())
                        );
            }
        }

        internal override string Help
        {
            get
            {
                return @"
---------------------------------------------------------
Special Key: Will activate Time Warp

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
