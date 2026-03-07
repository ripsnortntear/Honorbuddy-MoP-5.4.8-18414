#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Warrior/Fury.cs $
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
using Styx.CommonBot;
using Styx.TreeSharp;
using Lua = PureRotation.Helpers.Lua;

namespace PureRotation.Classes.Warrior
{
    [UsedImplicitly]
    internal class Fury : RotationBase
    {
        //private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }
        private static WarriorSettings WarriorSettings { get { return PRSettings.Instance.Warrior; } }

        #region Single Target
        internal static Composite FurySt1H()
        {
            return new PrioritySelector(
                // SimulationCraft 520-8 (r16290) - 1H Fury - Slightly adjusted.
                new Decorator(ret => Common.AlmostDead,
                        new PrioritySelector(
                                Spell.Cast(SpellBook.Execute),
                                Spell.Cast(SpellBook.HeroicStrike, ret => Common.DumpAllRage)
                                )),
                Spell.Cast(SpellBook.HeroicStrike, ret => Me.CurrentRage >= 110 || (Common.NonExecuteCheck && Common.ColossusSmashAura && Me.CurrentRage >= 40)),
                Spell.Cast(SpellBook.RagingBlow, ret => Common.RagingBlowStacks && Common.ColossusSmashAura && Common.NonExecuteCheck),
                Spell.Cast(SpellBook.Bloodthirst),
                Spell.Cast(SpellBook.WildStrike, ret => Common.BloodsurgeAura && Common.ExecuteCheck && Common.BTCD <= 1),
                Spell.Cast(SpellBook.DragonRoar, ret => Common.DRTalent && (Common.BloodbathAura || Common.AvatarAura || Common.SBTalent) && !Common.ColossusSmashAura),
                Spell.Cast(SpellBook.ColossusSmash),
                Spell.Cast(SpellBook.Execute, ret => Common.EnrageAura || Common.ColossusSmashAura || Common.RecklessnessAura || Me.CurrentRage >= 90 || Common.ExecuteCheck),
                Spell.Cast(SpellBook.StormBolt, ret => Common.SBTalent),
                Spell.Cast(SpellBook.RagingBlow, ret => Common.RagingBlowStacks || (Common.RagingBlowAura && (Common.ColossusSmashAura || Common.CSCD >= 3 || (Common.BTCD >= 1 && Common.FadingRb(3000))))),
                Spell.Cast(SpellBook.WildStrike, ret => Common.BloodsurgeAura),
                Spell.Cast(SpellBook.Shockwave, ret => Common.SWTalent && Me.IsSafelyFacing(Me.CurrentTarget)),
                Spell.Cast(SpellBook.HeroicThrow, ret => WarriorSettings.FuryHeroicThrow && !Common.ColossusSmashAura),
                Spell.Cast(SpellBook.BattleShout, ret => Me.CurrentRage < 70 && !Common.ColossusSmashAura && WarriorSettings.FuryShoutSelection == WarriorShout.Battle),
                Spell.Cast(SpellBook.CommandingShout, ret => Me.CurrentRage < 70 && !Common.ColossusSmashAura && WarriorSettings.FuryShoutSelection == WarriorShout.Commanding),
                Spell.Cast(SpellBook.WildStrike, ret => Common.ColossusSmashAura && Common.NonExecuteCheck),
                Spell.Cast(SpellBook.ImpendingVictory, ret => !Common.IVOC && Common.IVTalent && Common.NonExecuteCheck && (WarriorSettings.FuryImpVicRot || WarriorSettings.FuryImpVic && Me.HealthPercent <= WarriorSettings.FuryImpVicPercent)),
                Spell.Cast(SpellBook.WildStrike, ret => Common.CSCD >= 2 && Me.CurrentRage >= 80 && Common.NonExecuteCheck),
                Spell.Cast(SpellBook.BattleShout, ret => Me.CurrentRage < 70 && WarriorSettings.FuryShoutSelection == WarriorShout.Battle),
                Spell.Cast(SpellBook.CommandingShout, ret => Me.CurrentRage < 70 && WarriorSettings.FuryShoutSelection == WarriorShout.Commanding)
                );
        }
        #endregion Single Target

        #region HandleMultiTarget

        private static Composite FuryMt(int amuc)
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.DragonRoar, ret => Common.DRTalent),
                Spell.Cast(SpellBook.Whirlwind, ret => (!Common.MeatCleaverAuraS3 || amuc >= 6)),
                Spell.Cast(SpellBook.RagingBlow, ret =>
                    (Common.MeatCleaverAuraS3 && amuc >= 4) ||
                    (Common.MeatCleaverAuraS2 && amuc == 3) ||
                    (Common.MeatCleaverAuraS1 && amuc == 2) // Just for when one Unit dies during traverse.
                    ),
                Spell.Cast(SpellBook.Shockwave, ret => Common.SWTalent && Me.IsSafelyFacing(Me.CurrentTarget) && amuc >= 3),
                Spell.Cast(SpellBook.Bladestorm, ret => Common.BSTalent),
                Spell.Cast(SpellBook.Cleave, ret => (Common.URGlyph && Me.CurrentRage >= 110) || (Common.NonExecuteCheck && !Common.URGlyph && Me.CurrentRage >= 90)),
                Spell.Cast(SpellBook.Execute, ret => Common.ExecuteCheck && (Common.EnrageAura || Common.ColossusSmashAura || Common.RecklessnessAura || Me.CurrentRage >= 90)),
                Spell.Cast(SpellBook.Bloodthirst),
                Spell.Cast(SpellBook.ColossusSmash)
                );
        }

        #endregion HandleMultiTarget

        #region Defensive Cooldowns

        private static Composite FuryDefensive()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.DiebytheSword, ret => WarriorSettings.FuryDiebytheSword && Me.HealthPercent <= WarriorSettings.FuryDiebytheSwordPercent),
                Spell.Cast(SpellBook.EnragedRegeneration, ret => WarriorSettings.FuryEnragedRegen && Me.HealthPercent <= WarriorSettings.FuryEnragedRegenPercent && Common.EnrageAura),
                Spell.Cast(SpellBook.ImpendingVictory, ret => Common.Default && Me.CurrentRage >= 40 && !Common.IVOC && TalentManager.HasTalent(6) && Common.NonExecuteCheck && WarriorSettings.FuryImpVic && Me.HealthPercent <= WarriorSettings.FuryImpVicPercent),
                Spell.Cast(SpellBook.VictoryRush, ret => Common.Default && Me.CurrentRage >= 40 && !Common.VROC && !TalentManager.HasTalent(6) && Common.NonExecuteCheck && WarriorSettings.FuryImpVic && Me.HealthPercent <= WarriorSettings.FuryImpVicPercent)
                );
        }

        #endregion Defensive Cooldowns

        #region Offensive Cooldowns

        private static Composite FuryOffensive()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.Bloodbath, ret => Me.CurrentTarget.IsBoss(HotKeyManager.IsCooldown) && Common.BBTalent && (Common.CSCD < 2 || Common.ColossusSmashAuraT)),
                Spell.Cast(SpellBook.Recklessness, ret => Me.CurrentTarget.IsBoss(HotKeyManager.IsCooldown) && (Common.ColossusSmashAuraT || Common.CSCD <= 4) && ((Common.SBTalent) || (Common.AVTalent && (Common.AvatarAura || Common.AVCD <= 3)) || (Common.BBTalent && (Common.BloodbathAura || Common.BBCD <= 3)))),
                Spell.Cast(SpellBook.Avatar, ret => Me.CurrentTarget.IsBoss(HotKeyManager.IsCooldown) && Common.AVTalent && (Common.RecklessnessAura || Common.RCCD >= 180)),
                Spell.Cast(SpellBook.SkullBanner, ret => Common.Default && Me.CurrentTarget.IsBoss(HotKeyManager.IsCooldown) && !Common.SkullBannerAura && Common.RecklessnessAura),
                Spell.Cast(SpellBook.BerserkerRage, ret => (!Common.EnrageAura || (Common.RagingBlowStacks && Common.NonExecuteCheck) || (Common.RecklessnessAuraT && !Common.RagingBlowAura)))
                );
        }

        #endregion Offensive Cooldowns

        #region Utility

        private static Composite FuryUtility()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.IntimidatingShout, ret => WarriorSettings.FuryIntiShout && Common.ISGlyph && !Me.CurrentTarget.IsBoss()),
                Spell.Cast(SpellBook.RallyingCry, ret => WarriorSettings.FuryRallyingCry && Me.HealthPercent <= WarriorSettings.FuryRallyingCryPercent && !Common.RallyingCryAura),
                Spell.Cast(SpellBook.ShatteringThrow, ret => WarriorSettings.UseShatteringThrow && Me.CurrentTarget.IsBoss() && (Common.CSCD <= 3 || Common.SBCD <= 3)),
                Spell.CastOnGround(SpellBook.DemoralizingBanner, on => Me.CurrentTarget.Location, ret => WarriorSettings.FuryDemoBanner && Me.HealthPercent <= WarriorSettings.FuryDemoBannerPercent && !Me.CurrentTarget.IsOverrideDnD(), true)
                );
        }

        #endregion Utility

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.WarriorFury; }
        }

        public override string Name
        {
            get { return "FuryBuddy"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                        new Decorator(ret => (HotKeyManager.IsPaused || !Common.Default), new ActionAlwaysSucceed()),
                        EncounterSpecific.HandleActionBarInterrupts(),
                        Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                        new Decorator(ret => !SpellManager.GlobalCooldown,
                                new PrioritySelector(                                      
                                        Racials.UseRacials(),
                                        CachedUnits.Pulse,
                                        Item.HandleItems()
                                        )),
                        new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                new PrioritySelector(
                                        new Decorator(ret => WarriorSettings.FuryAutoAttack, Lua.StartAutoAttack),
                                        new Decorator(ret => Me.HealthPercent < 100, FuryDefensive()),
                                        FuryUtility(),
                                        new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), FuryOffensive()),
                                        new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() >= 3, FuryMt(Unit.AttackableMeleeUnits.Count())),
                                        FurySt1H()
                                    )),
                        new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                new PrioritySelector(
                                        new Decorator(ret => WarriorSettings.FuryAutoAttack, Lua.StartAutoAttack),
                                        new Decorator(ret => Me.HealthPercent < 100, FuryDefensive()),
                                        FuryUtility(),
                                        new Decorator(ret => HotKeyManager.IsCooldown, FuryOffensive()),
                                        new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() >= 3, FuryMt(Unit.AttackableMeleeUnits.Count())),
                                        FurySt1H()
                                    )),
                        new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                new PrioritySelector(
                                        new Decorator(ret => WarriorSettings.FuryAutoAttack, Lua.StartAutoAttack),
                                        new Decorator(ret => Me.HealthPercent < 100, FuryDefensive()),
                                        FuryUtility(),
                                        new Decorator(ret => HotKeyManager.IsCooldown, FuryOffensive()),
                                        new Decorator(ret => PRSettings.Instance.UseAoEAbilities && HotKeyManager.IsAoe && Unit.AttackableMeleeUnits.Count() >= 3, FuryMt(Unit.AttackableMeleeUnits.Count())),
                                        FurySt1H()
                                    )));
            }
        }

        public override Composite PVPRotation
        {
            get { return PVERotation; }
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
                        Spell.Cast("Commanding Shout", ret => WarriorSettings.FuryShoutSelection == WarriorShout.Commanding && !Common.CommandingShoutAura),
                        Spell.Cast("Battle Shout", ret => WarriorSettings.FuryShoutSelection == WarriorShout.Battle && !Common.BattleShoutAura)));
            }
        }

        #endregion Overrides of RotationBase
    }
}