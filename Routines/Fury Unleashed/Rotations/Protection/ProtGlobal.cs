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
using Lua = FuryUnleashed.Core.Helpers.LuaClass;

namespace FuryUnleashed.Rotations.Protection
{
    class ProtGlobal
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        internal static Composite InitializeProtPreCombat
        {
            get
            {
                return new PrioritySelector(
                    new PrioritySelector(ret => !Me.Combat,
                        G.InitializeCaching(),
                        new Decorator(ret => IS.Instance.General.CheckPreCombatHk, G.InitializeOnKeyActions())),
                    new Decorator(ret => U.DefaultBuffCheck && ((IS.Instance.General.CheckPreCombatBuff && !Me.Combat) || Me.Combat),
                        new Switch<Enum.Shouts>(ctx => IS.Instance.Protection.ShoutSelection,
                            new SwitchArgument<Enum.Shouts>(Enum.Shouts.BattleShout,
                                Spell.Cast(SpellBook.BattleShout, on => Me, ret => !G.BattleShoutAura)),
                            new SwitchArgument<Enum.Shouts>(Enum.Shouts.CommandingShout,
                                Spell.Cast(SpellBook.CommandingShout, on => Me, ret => !G.CommandingShoutAura)))));
            }
        }

        internal static Composite InitializeProtCombat
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => IS.Instance.General.CheckTreePerformance, TreeSharp.Tree(true)),
                    new Decorator(ret => (HotKeyManager.IsPaused || !U.DefaultCheck), new ActionAlwaysSucceed()),
                    G.InitializeCaching(),
                    G.InitializeOnKeyActions(),
                    new Decorator(ret => IS.Instance.Protection.CheckInterrupts && U.CanInterrupt, G.InterruptLogic()),
                    new Switch<Enum.ProtRotationVersion>(ctx => IS.Instance.General.CrProtRotVersion,
                        new SwitchArgument<Enum.ProtRotationVersion>(Enum.ProtRotationVersion.Development, ProtDev.DevProtCombat),
                        new SwitchArgument<Enum.ProtRotationVersion>(Enum.ProtRotationVersion.Release, ProtRel.RelProtCombat)));
            }
        }

        internal static bool BerserkerRageUsage
        {
            get
            {
                return ((IS.Instance.Protection.BerserkerRage == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.BerserkerRage == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.BerserkerRage == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool CustomExecuteUsage
        {
            get
            {
                return (IS.Instance.Protection.CheckCustomExecuteLogic && Lua.PlayerPower >= IS.Instance.Protection.CheckCustomExecuteLogicNum);
            }
        }

        internal static bool DemoralizingShoutUsage
        {
            get
            {
                return ((IS.Instance.Protection.DemoralizeShout == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.DemoralizeShout == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.DemoralizeShout == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool MassSpellReflectionUsage
        {
            get
            {
                return ((IS.Instance.Protection.MassSpellReflection == Enum.MsrTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.MassSpellReflection == Enum.MsrTrigger.Always && G.PummelOnCooldown && G.DisruptingShoutOnCooldown));
            }
        }

        internal static bool SpellReflectionUsage
        {
            get
            {
                return ((IS.Instance.Protection.SpellReflection == Enum.MsrTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.SpellReflection == Enum.MsrTrigger.Always && G.PummelOnCooldown && G.DisruptingShoutOnCooldown));
            }
        }

        internal static bool RacialUsage
        {
            get
            {
                return ((IS.Instance.Protection.ClassRacials == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.ClassRacials == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.ClassRacials == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool RecklessnessUsage
        {
            get
            {
                return ((IS.Instance.Protection.Recklessness == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.Recklessness == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.Recklessness == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool ShatteringThrowUsage
        {
            get
            {
                return ((IS.Instance.Protection.ShatteringThrow == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.ShatteringThrow == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.ShatteringThrow == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool SkullBannerUsage
        {
            get
            {
                return ((IS.Instance.Protection.SkullBanner == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.SkullBanner == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.SkullBanner == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier4AbilityUsage
        {
            get
            {
                return ((IS.Instance.Protection.Tier4Abilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.Tier4Abilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.Tier4Abilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier6AbilityUsage
        {
            get
            {
                return ((IS.Instance.Protection.Tier6Abilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.Tier6Abilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.Tier6Abilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier4AbilityAoEUsage
        {
            get
            {
                return ((IS.Instance.Protection.Tier4AoeAbilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.Tier4AoeAbilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.Tier4AoeAbilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier6AbilityAoEUsage
        {
            get
            {
                return ((IS.Instance.Protection.Tier6AoeAbilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Protection.Tier6AoeAbilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Protection.Tier6AoeAbilities == Enum.AbilityTrigger.Always));
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
                return ((G.RecklessnessAura) || (G.ReadinessAura));
            }
        }

        internal static bool BloodbathSync
        {
            get
            {
                return ((G.BloodbathAura || G.AvatarTalent || G.StormBoltTalent) || (G.ReadinessAura));
            }
        }
    }
}
