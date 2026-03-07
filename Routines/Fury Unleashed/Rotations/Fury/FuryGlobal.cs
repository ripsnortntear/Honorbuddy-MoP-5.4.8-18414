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

namespace FuryUnleashed.Rotations.Fury
{
    class FuryGlobal
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        internal static Composite InitializeFuryPreCombat
        {
            get
            {
                return new PrioritySelector(
                    new PrioritySelector(ret => !Me.Combat,
                        G.InitializeCaching(),
                        new Decorator(ret => IS.Instance.General.CheckPreCombatHk, G.InitializeOnKeyActions())),
                    new Decorator(ret => U.DefaultBuffCheck && ((IS.Instance.General.CheckPreCombatBuff && !Me.Combat) || Me.Combat),
                        new Switch<Enum.Shouts>(ctx => IS.Instance.Fury.ShoutSelection,
                            new SwitchArgument<Enum.Shouts>(Enum.Shouts.BattleShout,
                                Spell.Cast(SpellBook.BattleShout, on => Me, ret => !G.BattleShoutAura)),
                            new SwitchArgument<Enum.Shouts>(Enum.Shouts.CommandingShout,
                                Spell.Cast(SpellBook.CommandingShout, on => Me, ret => !G.CommandingShoutAura)))));
            }
        }

        internal static Composite InitializeFuryCombat
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => IS.Instance.General.CheckTreePerformance, TreeSharp.Tree(true)),
                    new Decorator(ret => (HotKeyManager.IsPaused || !U.DefaultCheck), new ActionAlwaysSucceed()),
                    G.InitializeCaching(),
                    G.InitializeOnKeyActions(),
                    new Decorator(ret => IS.Instance.Fury.CheckInterrupts && U.CanInterrupt, G.InterruptLogic()),
                    new Switch<Enum.FuryRotationVersion>(ctx => IS.Instance.General.CrFuryRotVersion,
                        new SwitchArgument<Enum.FuryRotationVersion>(Enum.FuryRotationVersion.Development, FuryDev.DevFuryCombat),
                        new SwitchArgument<Enum.FuryRotationVersion>(Enum.FuryRotationVersion.Release, FuryRel.RelFuryCombat)));
            }
        }

        #region Booleans
        internal static bool BerserkerRageUsage
        {
            get
            {
                return ((IS.Instance.Fury.BerserkerRage == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.BerserkerRage == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Fury.BerserkerRage == Enum.AbilityTrigger.Always));
            }
        }

        // Experimental
        internal static bool StanceDanceUsage
        {
            get
            {
                return IS.Instance.Fury.CheckStanceDance && !G.DefensiveStanceAura;
            }
        }

        internal static bool CancelBladestormAuraUsage
        {
            get
            {
                return IS.Instance.Fury.CheckBladeStormLogics && G.BladestormAura && U.NearbyAttackableUnitsCount == 1;
            }
        }

        // Non-Experimental
        internal static bool BloodbathSync
        {
            get
            {
                return (G.BloodbathAura || !G.BloodbathTalent);
            }
        }

        internal static bool DemoralizingBannerUsage
        {
            get
            {
                return IS.Instance.Fury.CheckDemoBanner;
            }
        }

        internal static bool DiebytheSwordUsage
        {
            get
            {
                return IS.Instance.Fury.CheckDiebytheSword;
            }
        }

        internal static bool EnragedRegenerationUsage
        {
            get
            {
                return IS.Instance.Fury.CheckEnragedRegen;
            }
        }

        internal static bool HeroicThrowUsage
        {
            get
            {
                return IS.Instance.Fury.CheckHeroicThrow;
            }
        }

        internal static bool ImpendingVictoryUsage
        {
            get
            {
                return IS.Instance.Fury.CheckImpVic;
            }
        }

        internal static bool IntimidatingShoutUsage
        {
            get
            {
                return IS.Instance.Fury.CheckIntimidatingShout;
            }
        }

        internal static bool MassSpellReflectionUsage
        {
            get
            {
                return ((IS.Instance.Fury.MassSpellReflection == Enum.MsrTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.MassSpellReflection == Enum.MsrTrigger.Always && G.PummelOnCooldown && G.DisruptingShoutOnCooldown));
            }
        }

        internal static bool MultiTargetUsage
        {
            get
            {
                return IS.Instance.Fury.CheckAoE;
            }
        }

        internal static bool PiercingHowlUsage
        {
            get
            {
                return IS.Instance.Fury.CheckPiercingHowl;
            }
        }

        internal static bool RacialUsage
        {
            get
            {
                return ((IS.Instance.Fury.ClassRacials == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.ClassRacials == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Fury.ClassRacials == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool RecklessnessUsage
        {
            get
            {
                return ((IS.Instance.Fury.Recklessness == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.Recklessness == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Fury.Recklessness == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool RotationalImpendingVictoryUsage
        {
            get
            {
                return IS.Instance.Fury.CheckRotImpVic;
            }
        }

        internal static bool ShatteringThrowUsage
        {
            get
            {
                return ((IS.Instance.Fury.ShatteringThrow == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.ShatteringThrow == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Fury.ShatteringThrow == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool ShieldWallUsage
        {
            get
            {
                return IS.Instance.Fury.CheckShieldWall;
            }
        }

        internal static bool SkullBannerUsage
        {
            get
            {
                return ((IS.Instance.Fury.SkullBanner == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.SkullBanner == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Fury.SkullBanner == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool SpellReflectUsage
        {
            get
            {
                return IS.Instance.Fury.CheckSpellReflect;
            }
        }

        internal static bool StaggeringShoutUsage
        {
            get
            {
                return IS.Instance.Fury.CheckStaggeringShout;
            }
        }

        internal static bool Tier4AbilityUsage
        {
            get
            {
                return ((IS.Instance.Fury.Tier4Abilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.Tier4Abilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Fury.Tier4Abilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier6AbilityUsage
        {
            get
            {
                return ((IS.Instance.Fury.Tier6Abilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.Tier6Abilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Fury.Tier6Abilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier4AbilityAoEUsage
        {
            get
            {
                return ((IS.Instance.Fury.Tier4AoeAbilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.Tier4AoeAbilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Fury.Tier4AoeAbilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool Tier6AbilityAoEUsage
        {
            get
            {
                return ((IS.Instance.Fury.Tier6AoeAbilities == Enum.AbilityTrigger.OnBossDummy && U.IsTargetBoss) ||
                        (IS.Instance.Fury.Tier6AoeAbilities == Enum.AbilityTrigger.OnBlTwHr && G.HasteAbilities) ||
                        (IS.Instance.Fury.Tier6AoeAbilities == Enum.AbilityTrigger.Always));
            }
        }

        internal static bool VictoryRushUsage
        {
            get
            {
                return IS.Instance.Fury.CheckVicRush;
            }
        }

        //internal static bool VigilanceUsage
        //{
        //    get
        //    {
        //        return ((IS.Instance.General.Vigilance == Enum.VigilanceTrigger.OnRaidMember) ||
        //                (IS.Instance.General.Vigilance == Enum.VigilanceTrigger.OnTank));
        //    }
        //}

        //internal static bool RecklessnessSync
        //{
        //    get
        //    {
        //        return ((G.RecklessnessAura) || (G.ReadinessAura));
        //    }
        //}

        //internal static bool UseWildStrike()
        //{
        //    if (Global.ColossusSmashSpellCooldown >= 7500 && Me.CurrentRage >= 80 && Global.BloodthirstSpellCooldown < 1500)
        //        return true;
        //    return Me.CurrentRage >= Me.MaxRage - 20;
        //}

        //internal static bool UseHeroicStrike(bool hasSmashAura)
        //{
        //    if (hasSmashAura)
        //    {
        //        if ((Global.RagingBlow1S || Global.RagingBlow2S) && Me.CurrentRage >= 40)
        //            return true;
        //        if ((!Global.RagingBlow1S && !Global.RagingBlow2S) && Me.CurrentRage >= 30)
        //            return true;
        //        if (Global.FadingColossusSmash(2000) && !Global.RagingBlow1S && !Global.RagingBlow2S && Me.CurrentRage >= 30)
        //            return true;
        //    }
        //    return Me.CurrentRage >= Me.MaxRage - 10;
        //}

        //internal static bool HamstringUsage
        //{
        //    get
        //    {
        //        return ((IS.Instance.Fury.HamString == Enum.Hamstring.Always) ||
        //                (IS.Instance.Fury.HamString == Enum.Hamstring.AddList && Unit.IsHamstringTarget));
        //    }
        //}

        //internal static bool TrinketProc
        //{
        //    get
        //    {
        //        return Global.DeterminationAura || Global.OutrageAura;
        //    }
        //}
        #endregion
    }
}
