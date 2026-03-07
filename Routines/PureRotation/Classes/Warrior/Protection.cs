using System.Linq;
using CommonBehaviors.Actions;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.TreeSharp;
using Lua = PureRotation.Helpers.Lua;

namespace PureRotation.Classes.Warrior
{
    [UsedImplicitly]
    internal class Protection : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                Spell.PreventDoubleCast("Heroic Strike", ThrottleTime, ret => Common.UltimatumAura),
                Spell.Cast("Shield Slam", ret => Me.CurrentRage <= 90),
                Spell.Cast("Revenge", ret => Me.CurrentRage <= 100),
                Spell.Cast("Commanding Shout", ret => ShoutSelection == WarriorShout.Commanding && Me.CurrentRage <= 100),
                Spell.Cast("Battle Shout", ret => ShoutSelection == WarriorShout.Battle && Me.CurrentRage <= 100),
                Spell.Cast("Storm Bolt", ret => TalentManager.HasTalent(18)),
                Spell.Cast("Thunder Clap", ret => NeedWeakenedBlows),
                Spell.Cast("Devastate"));
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                Spell.Cast("Cleave", ret => Common.UltimatumAura),
                Spell.Cast("Thunder Clap", ret => NeedWeakenedBlows),
                Spell.Cast("Dragon Roar", ret => Common.DRTalent),
                Spell.Cast("Shockwave", ret => Common.SWTalent && Me.IsSafelyFacing(Me.CurrentTarget) && Unit.AttackableMeleeUnits.Count() > 2),
                Spell.Cast("Bladestorm", ret => Common.BSTalent),
                Spell.Cast("Revenge", ret => Me.CurrentRage < 100),
                Spell.Cast("Shield Slam", ret => Me.CurrentRage < 90),
                Spell.Cast("Commanding Shout", ret => ShoutSelection == WarriorShout.Commanding && Me.CurrentRage < 100),
                Spell.Cast("Battle Shout", ret => ShoutSelection == WarriorShout.Battle && Me.CurrentRage < 100),
                Spell.Cast("Devastate"),
                Spell.Cast("Intimidating Shout", ret => TalentManager.HasGlyph("Intimidating Shout") && !Me.CurrentTarget.IsBoss()));
        }

        private static Composite HandleOffensiveCd()
        {
            return new PrioritySelector(
                Spell.Cast("Skull Banner", ret => !Common.SkullBannerAura && Spell.Lastspellcast != "Demoralizing Banner" && !Me.CurrentTarget.HasAura(SpellBook.DemoralizingBanner) && Me.CurrentTarget.IsBoss()),
                Spell.Cast("Avatar", ret => NeedToTruckWithAvatar),
                Spell.Cast("Berserker Rage", ret => !Common.EnrageAura && UseBerserkerRage),
                Spell.Cast("Bloodbath", ret => Common.BBTalent && UseBloodbath),
                Spell.Cast("Recklessness", ret => UseRecklessness),
                Spell.Cast("Berserking", ret => Me.Race == WoWRace.Troll),
                Spell.Cast("Blood Fury", ret => Me.Race == WoWRace.Orc),
                Spell.Cast("Rocket Barrage", ret => Me.Race == WoWRace.Goblin),
                Spell.Cast("Shattering Throw", ret => UseShatteringThrow)
                );
        }

        private static Composite HandleUtility()
        {
            return new PrioritySelector(
                Spell.Cast("Demoralizing Shout", ret => Me.CurrentTarget.IsTargetingMeOrPet),
                Spell.CastOnGround("Demoralizing Banner", on => Me.CurrentTarget.Location, ret => !Common.SkullBannerAura && Me.CurrentTarget.IsBoss() && !Me.CurrentTarget.IsOverrideDnD(), true));
        }

        private static Composite HandleDefensiveCd()
        {
            return new PrioritySelector(
                Spell.Cast("Last Stand", on => Me, ret => NeedLastStand),
                Spell.Cast("Rallying Cry", on => Me, ret => NeedRallyingCry),
                new Decorator(ret => HotKeyManager.IsSpecialKey, new PrioritySelector(Spell.Cast("Shield Barrier", on => Me, ret => !Me.ActiveAuras.ContainsKey("Shield Barrier")))),
                new Decorator(ret => !HotKeyManager.IsSpecialKey, new PrioritySelector(Spell.Cast("Shield Block", on => Me, ret => !Me.ActiveAuras.ContainsKey("Shield Block")))),
                Spell.Cast("Shield Wall", on => Me, ret => NeedShieldWall),
                Spell.Cast("Enraged Regeneration", on => Me, ret => Common.ERTalent && NeedEnragedRegeneration),
                Spell.Cast("Impending Victory", ret => NeedImpendingVictory),
                Spell.Cast("Victory Rush", ret => NeedVictoryRush),
                Spell.Cast("Spell Reflection", ret => NeedToSpellReflect),
                Spell.Cast("Mass Spell Reflection", ret => NeedToSpellReflect && Spell.SpellOnCooldown("Spell Reflection")));
        }

        #region Booleans & Doubles

        internal static bool NeedToTruckWithAvatar { get { return Common.AVTalent && Me.CurrentTarget.IsBoss() && (Common.RecklessnessAura || Common.SkullBannerAura); } }

        internal static bool NeedWeakenedBlows { get { return (!Me.CurrentTarget.CachedHasAura(115798) || Common.FadingWb(1500)) && Me.CurrentTarget.IsWithinMeleeRange; } }

        internal static bool NeedLastStand { get { return Me.HealthPercent < LastStandPercent && !Me.HasAura("Shield Wall") && !Me.HasAura("Rallying Cry") && !Me.HasAura("Enraged Regeneration"); } }

        internal static bool NeedRallyingCry { get { return Me.HealthPercent > RallyingCryPercent && !Me.HasAura("Last Stand") && !Me.HasAura("Shield Wall") && Unit.NeedRallyingCry; } }

        internal static bool NeedShieldWall { get { return Me.HealthPercent < ShieldWallPercent && !Me.HasAura("Last Stand") && !Me.HasAura("Rallying Cry"); } }

        internal static bool NeedEnragedRegeneration { get { return Me.HasAura("Enrage") && Me.HealthPercent < EnragedRegenerationPercent; } }

        internal static bool NeedImpendingVictory { get { return !Common.IVOC && Common.IVTalent && Me.HealthPercent < ImpendingVictoryPercent; } }

        internal static bool NeedVictoryRush { get { return !Common.VROC && !Common.IVTalent && Me.ActiveAuras.ContainsKey("Victorious") && Me.HealthPercent < ImpendingVictoryPercent; } }

        internal static bool NeedToSpellReflect { get { return Me.CurrentTarget != null && Me.CurrentTarget.CurrentTarget == Me && Me.CurrentTarget.IsCasting && UseSpellReflection; } }

        #endregion Booleans & Doubles

        #region Settings

        private static WarriorSettings WarriorSettings { get { return PRSettings.Instance.Warrior; } }

        private static int EnragedRegenerationPercent { get { return WarriorSettings.ProtEnragedRegenerationRagePercent; } }

        private static bool UseBerserkerRage { get { return WarriorSettings.UseBerserkerRage; } }

        private static bool UseRecklessness { get { return WarriorSettings.UseRecklessness; } }

        private static bool UseShatteringThrow { get { return WarriorSettings.UseShatteringThrow; } }

        private static bool UseBloodbath { get { return WarriorSettings.UseBloodbath; } }

        private static WarriorShout ShoutSelection { get { return WarriorSettings.ProtShoutSelection; } }

        private static int LastStandPercent { get { return WarriorSettings.LastStandPercent; } }

        private static int ImpendingVictoryPercent { get { return WarriorSettings.ImpendingVictoryPercent; } }

        private static int ShieldWallPercent { get { return WarriorSettings.ShieldWallPercent; } }

        private static int RallyingCryPercent { get { return WarriorSettings.RallyingCryPercent; } }

        private static bool UseSpellReflection { get { return WarriorSettings.UseSpellReflection; } }

        #endregion Settings

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.WarriorProtection; }
        }

        public override string Name
        {
            get { return "ProtectionBuddy"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                        new Decorator(ret => (HotKeyManager.IsPaused || !Common.Default), new ActionAlwaysSucceed()),
                                Racials.UseRacials(),
                                EncounterSpecific.HandleActionBarInterrupts(),
                                Item.HandleItems(),
                                HandleDefensiveCd(),
                                HandleUtility(),
                                Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                        new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                      new PrioritySelector(
                                          new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCd()),
                                          new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > WarriorSettings.ProtAoECount, HandleAoeCombat()),
                                          HandleSingleTarget())),
                        new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                      new PrioritySelector(
                                          new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCd()),
                                          new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > WarriorSettings.ProtAoECount, HandleAoeCombat()),
                                          HandleSingleTarget())),
                        new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                      new PrioritySelector(
                                          new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCd()),
                                          new Decorator(ret => PRSettings.Instance.UseAoEAbilities && HotKeyManager.IsAoe, HandleAoeCombat()),
                                          HandleSingleTarget())));
            }
        }

        public override Composite PVPRotation
        {
            get { return null; }
        }

        public override Composite Medic
        {
            get { return null; }
        }

        public override Composite PreCombat
        {
            get
            {
                return new Decorator(ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        Spell.Cast("Commanding Shout", ret => ShoutSelection == WarriorShout.Commanding && !StyxWoW.Me.HasAura("Commanding Shout")),
                        Spell.Cast("Battle Shout", ret => ShoutSelection == WarriorShout.Battle && !StyxWoW.Me.HasAura("Battle Shout"))));
            }
        }

        #endregion Overrides of RotationBase
    }
}