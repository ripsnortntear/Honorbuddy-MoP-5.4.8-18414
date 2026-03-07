using CommonBehaviors.Actions;
using FuryUnleashed.Core;
using FuryUnleashed.Core.Helpers;
using FuryUnleashed.Core.Managers;
using FuryUnleashed.Core.Utilities;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using G = FuryUnleashed.Rotations.Global;
using IS = FuryUnleashed.Interfaces.Settings.InternalSettings;
using U = FuryUnleashed.Core.Unit;

namespace FuryUnleashed.Rotations.Arms
{
    class ArmsGlobal
    {
        private static LocalPlayer Me
        {
            get { return StyxWoW.Me; }
        }

        internal static Composite InitializeArmsPreCombat
        {
            get
            {
                return new PrioritySelector(
                    new PrioritySelector(ret => !Me.Combat,
                        G.InitializeCaching(),
                        new Decorator(ret => IS.Instance.General.CrArmsRotVersion == Enum.ArmsRotationVersion.PvP && !StyxWoW.Me.IsInInstance && U.IsViable(Me.CurrentTarget) && StyxWoW.Me.CurrentTarget.IsPlayer && !StyxWoW.Me.CurrentTarget.IsFriendly && StyxWoW.Me.CurrentTarget.Distance > 12 && StyxWoW.Me.CurrentTarget.Distance < 30,
                            new PrioritySelector(
                                Spell.Cast(SpellBook.Charge),
                                Spell.CastOnGround(SpellBook.HeroicLeap, ret => StyxWoW.Me.CurrentTarget.Location))),
                        new Decorator(ret => IS.Instance.General.CheckPreCombatHk, G.InitializeOnKeyActions())),
                    new Decorator(ret => U.DefaultBuffCheck && ((IS.Instance.General.CheckPreCombatBuff && !Me.Combat) || Me.Combat),
                        new Switch<Enum.Shouts>(ctx => IS.Instance.Arms.ShoutSelection,
                            new SwitchArgument<Enum.Shouts>(Enum.Shouts.BattleShout,
                                Spell.Cast(SpellBook.BattleShout, on => Me, ret => !G.BattleShoutAura)),
                            new SwitchArgument<Enum.Shouts>(Enum.Shouts.CommandingShout,
                                Spell.Cast(SpellBook.CommandingShout, on => Me, ret => !G.CommandingShoutAura)))));
            }
        }

        internal static Composite InitializeArmsCombat
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => IS.Instance.General.CheckTreePerformance, TreeSharp.Tree(true)),
                    new Decorator(ret => (HotKeyManager.IsPaused || !U.DefaultCheck), new ActionAlwaysSucceed()),
                    G.InitializeCaching(),
                    G.InitializeOnKeyActions(),
                    new Decorator(ret => IS.Instance.Arms.CheckInterrupts && U.CanInterrupt, G.InterruptLogic()),
                    new Switch<Enum.ArmsRotationVersion>(ctx => IS.Instance.General.CrArmsRotVersion,
                        new SwitchArgument<Enum.ArmsRotationVersion>(Enum.ArmsRotationVersion.Development, ArmsDev.DevArmsCombat),
                        new SwitchArgument<Enum.ArmsRotationVersion>(Enum.ArmsRotationVersion.PvP, ArmsPvP.PvPArmsCombat),
                        new SwitchArgument<Enum.ArmsRotationVersion>(Enum.ArmsRotationVersion.Release, ArmsRel.RelArmsCombat)));
            }
        }

        internal static bool BerserkerRageUsage
        {
            get
            {
                return ((IS.Instance.Arms.BerserkerRage == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.BerserkerRage == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Arms.BerserkerRage == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool EnragedRegenerationUsage
        {
            get
            {
                return IS.Instance.Arms.CheckEnragedRegen;
            }
        }

        internal static bool HeroicThrowUsage
        {
            get
            {
                return IS.Instance.Arms.CheckHeroicThrow;
            }
        }

        internal static bool ImpendingVictoryUsage
        {
            get
            {
                return IS.Instance.Arms.CheckRotImpVic;
            }
        }

        internal static bool MassSpellReflectionUsage
        {
            get
            {
                return ((U.NearbyCastingUnits(StyxWoW.Me.Location, 45)) != null || (IS.Instance.Arms.MassSpellReflection == Enum.MsrTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.MassSpellReflection == Enum.MsrTrigger.Always && G.PummelOnCooldown && G.DisruptingShoutOnCooldown));
            }
        }

        internal static bool RacialUsage
        {
            get
            {
                return ((IS.Instance.Arms.ClassRacials == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.ClassRacials == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Arms.ClassRacials == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool RecklessnessUsage
        {
            get
            {
                return ((IS.Instance.Arms.Recklessness == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.Recklessness == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Arms.Recklessness == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool RotationalImpendingVictoryUsage
        {
            get
            {
                return IS.Instance.Arms.CheckRotImpVic;
            }
        }

        internal static bool ShatteringThrowUsage
        {
            get
            {
                return ((IS.Instance.Arms.ShatteringThrow == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.ShatteringThrow == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Arms.ShatteringThrow == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool SkullBannerUsage
        {
            get
            {
                return ((IS.Instance.Arms.SkullBanner == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.SkullBanner == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Arms.SkullBanner == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier4AbilityUsage
        {
            get
            {
                return ((IS.Instance.Arms.Tier4Abilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.Tier4Abilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Arms.Tier4Abilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier6AbilityUsage
        {
            get
            {
                return ((IS.Instance.Arms.Tier6Abilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.Tier6Abilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Arms.Tier6Abilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier4AbilityAoEUsage
        {
            get
            {
                return ((IS.Instance.Arms.Tier4AoeAbilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.Tier4AoeAbilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Arms.Tier4AoeAbilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier6AbilityAoEUsage
        {
            get
            {
                return ((IS.Instance.Arms.Tier6AoeAbilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Arms.Tier6AoeAbilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Arms.Tier6AoeAbilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool VigilanceUsage
        {
            get
            {
                return ((IS.Instance.General.Vigilance == Enum.VigilanceTrigger.OnRaidMember) ||
                        (IS.Instance.General.Vigilance == Enum.VigilanceTrigger.OnTank));
            }
        }

        internal static bool RecklessnessSync
        {
            get
            {
                return ((G.RecklessnessAura) || (G.DeterminationAura || G.OutrageAura));
            }
        }

        internal static bool BloodbathSync
        {
            get
            {
                return ((G.BloodbathAura || G.AvatarTalent || G.StormBoltTalent) || (G.DeterminationAura || G.OutrageAura));
            }
        }
    }
}
