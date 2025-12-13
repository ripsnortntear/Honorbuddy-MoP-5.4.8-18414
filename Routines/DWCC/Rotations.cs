using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;


using Styx;
using Styx.TreeSharp;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Common;
using Styx.Resources;
using Styx.Loaders;
using Styx.Common.Helpers;
using Styx.Pathing;
using Styx.XmlEngine;
using Styx.CommonBot.POI;
using Styx.WoWInternals.World;
using CommonBehaviors.Actions;

using Action = Styx.TreeSharp.Action;

namespace DWCC
{
    public partial class Warrior
    {
        protected Composite CreateDWCCBehavior()
        {
            return new PrioritySelector(
                new Decorator(ret => StyxWoW.Me.CurrentTarget == null || Me.CurrentTarget.IsDead || !Me.CurrentTarget.Attackable || Me.IsDead || Me.Mounted || Me.IsCasting,
                    new ActionAlwaysSucceed()),
                new Decorator(ret => Me.Mounted, new Action(a => { Mount.Dismount("dismounting"); return RunStatus.Failure; })),
                FaceTarget(),
                CastCharge(),
                HeroicLeap(),
                MoveToTarget(),
                //MoveBehindTarget(),
                StopMoving(),
                SynapseSprings(),
            #region HealthRegen
                new Decorator(ret => (DunatanksSettings.Instance.useHealthStone && Me.HealthPercent <= DunatanksSettings.Instance.HealthStonePercent && HaveHealthStone() && HealthStoneNotCooldown()),
                new Action(ret => UseHealthStone())),
                new Decorator(ret => (DunatanksSettings.Instance.usePotion && Me.HealthPercent <= DunatanksSettings.Instance.PotionPercent && HaveHealthPotion() && HealthPotionReady()),
                new Action(ret => UseHealthPotion())),
            #endregion
                PvP(),
                PvE());
        }

        public Composite PvE()
        {
            return new Decorator(ret => !DunatanksSettings.Instance.usePvPRotation,
                    new PrioritySelector(
            #region General
                StanceChange(),
                CreateAutoAttack(),
                UseRacialComposite(),
                AoEPummel(),
            #endregion
            #region Interrupt
                CreateSpellCheckAndCast("Disrupting Shout", ret => DunatanksSettings.Instance.usePummel && Me.CurrentTarget.IsCasting && Me.CurrentTarget.CanInterruptCurrentSpellCast && !SpellManager.CanCast("Pummel")),
                CreateSpellCheckAndCast("Pummel", ret => DunatanksSettings.Instance.usePummel && Me.CurrentTarget.IsCasting && Me.CurrentTarget.CanInterruptCurrentSpellCast),
            #endregion
            #region IV/VR
                CreateSpellCheckAndCast("Impending Victory", ret => Me.HealthPercent < DunatanksSettings.Instance.IVHealth),
                CreateSpellCheckAndCast("Victory Rush", ret => !SpellManager.HasSpell("Impending Victory") && Me.HealthPercent < 100),
            #endregion
            #region Cooldowns
                CreateSpellCheckAndCast("Die by the Sword", ret => !DunatanksSettings.Instance.useProt && Me.HealthPercent < DunatanksSettings.Instance.DbtSHealth),
            #region MS
                new Decorator(ret => DunatanksSettings.Instance.useArms,
                    new PrioritySelector(
                CreateSpellCheckAndCast("Recklessness", ret => !ExecutePhase || (ExecutePhase && ((ColossusSmash5s || ColossusSmashCD < 1500) && BloodBathCD < 1)) && (DunatanksSettings.Instance.ArmsReckOnCD || (DunatanksSettings.Instance.ArmsReckOnBoss && (Me.CurrentTarget.IsBoss || Me.CurrentTarget.Name.Contains("Dummy"))))),
                CreateSpellCheckAndCast("Bloodbath"),
                CreateSpellCheckAndCast("Avatar", ret => (DunatanksSettings.Instance.ArmsAvaOnCD || (DunatanksSettings.Instance.ArmsAvaOnBoss && (Me.CurrentTarget.IsBoss || Me.CurrentTarget.Name.Contains("Dummy"))))),
                CreateSpellCheckAndCast("Skull Banner", ret => (DunatanksSettings.Instance.ArmsSB && (Me.CurrentTarget.IsBoss || Me.CurrentTarget.Name.Contains("Dummy")))))),
            #endregion
            #region Fury
                new Decorator(ret => DunatanksSettings.Instance.useFury,
                    new PrioritySelector(
                CreateSpellCheckAndCast("Recklessness", ret => !ExecutePhase || (ExecutePhase && ((ColossusSmash5s || ColossusSmashCD < 1500) && BloodBathCD < 1)) && (DunatanksSettings.Instance.FuryReckOnCD || (DunatanksSettings.Instance.FuryReckOnBoss && (Me.CurrentTarget.IsBoss || Me.CurrentTarget.Name.Contains("Dummy"))))),
                CreateSpellCheckAndCast("Bloodbath"),
                CreateSpellCheckAndCast("Avatar", ret => (DunatanksSettings.Instance.FuryAvaOnCD || (DunatanksSettings.Instance.FuryAvaOnBoss && (Me.CurrentTarget.IsBoss || Me.CurrentTarget.Name.Contains("Dummy"))))),
                CreateSpellCheckAndCast("Skull Banner", ret => (DunatanksSettings.Instance.FurySB && (Me.CurrentTarget.IsBoss || Me.CurrentTarget.Name.Contains("Dummy")))))),
            #endregion
            #region Prot
                new Decorator(ret => DunatanksSettings.Instance.useProt,
                    new PrioritySelector(
                CreateSpellCheckAndCast("Recklessness", ret => (DunatanksSettings.Instance.ProtReckOnBoss && Me.CurrentTarget.IsBoss) || DunatanksSettings.Instance.ProtReckOnCD),
                CreateSpellCheckAndCast("Bloodbath"),
                CreateSpellCheckAndCast("Avatar", ret => (DunatanksSettings.Instance.ProtAvaOnBoss && Me.CurrentTarget.IsBoss) || DunatanksSettings.Instance.ProtAvaOnCD),
                CreateSpellCheckAndCast("Skull Banner", ret => (DunatanksSettings.Instance.ProtSB && Me.HasAura("Recklessness")) && DunatanksSettings.Instance.useProt && DunatanksSettings.Instance.ProtSB),

                CreateSpellCheckAndCast("Shield Wall", ret => Me.HealthPercent < DunatanksSettings.Instance.SWHealth && DunatanksSettings.Instance.useSW && DunatanksSettings.Instance.useProt),
                CreateSpellCheckAndCast("Shield Block", ret => Me.HealthPercent < DunatanksSettings.Instance.SBHealth && DunatanksSettings.Instance.useSB && DunatanksSettings.Instance.useProt),
                CreateSpellCheckAndCast("Shield Barrier", ret => !Me.HasAura("Shield Barrier") && Me.HealthPercent < DunatanksSettings.Instance.SBarrHealth && DunatanksSettings.Instance.useSBarr && DunatanksSettings.Instance.useProt),
                CreateSpellCheckAndCast("Last Stand", ret => Me.HealthPercent < DunatanksSettings.Instance.LStandHealth && DunatanksSettings.Instance.useLStand && DunatanksSettings.Instance.useProt))),
            #endregion
            #endregion
                FuryRotation(),
                ArmsRotation(),
                ProtRotation(),
                FuryLeveling(),
                ArmsLeveling(),
                ProtLeveling(),
                Lowbie()));
        }

        public Composite FuryRotation()
        {
            return new Decorator(ret => Me.Level == 90 && DunatanksSettings.Instance.useFury,
                new PrioritySelector(
                    new Decorator(ret => true, // detectAdds().Count <= 2,
                        new PrioritySelector(
                            CreateSpellCheckAndCast("Berserker Rage", ret => !EnrageAura && RagingBlowStacks < 2),
                            CreateSpellCheckAndCast("Bloodthirst"),
                            CreateSpellCheckAndCast("Colossus Smash", ret => !ColossusSmash1s),
                            CreateSpellCheckAndCast("Dragon Roar", ret => !ColossusSmashAura),
                            CreateSpellCheckAndCast("Storm Bolt", ret => ColossusSmashAura),
                            CreateSpellCheckAndCast("Shockwave", ret => !ColossusSmashAura && Me.IsSafelyFacing(Me.CurrentTarget)),
                            CreateSpellCheckAndCast("Heroic Strike", ret => ColossusSmashAura && Me.CurrentRage > 40 && !ExecutePhase),
                            CreateSpellCheckAndCast("Execute", ret => Me.CurrentRage > 40 && ExecutePhase),
                            CreateSpellCheckAndCast("Raging Blow"),
                            CreateSpellCheckAndCast("Wild Strike", ret => BloodsurgeAura),
                            CreateSpellCheckAndCast("Heroic Throw", ret => ColossusSmashAura),
                            CreateSpellCheckAndCast("Impending Victory", ret => !ExecutePhase),
                            CreateSpellCheckAndCast("Battle Shout", ret => Me.CurrentRage <= 70))),
                    new Decorator(ret => detectAdds().Count > 2 && detectAdds().Count < 6,
                        new PrioritySelector(
                            CreateSpellCheckAndCast("Berserker Rage", ret => !EnrageAura && RagingBlowStacks < 2),
                            CreateSpellCheckAndCast("Whirlwind", ret => Stacks("Meat Cleaver") < 3),
                            CreateSpellCheckAndCast("Raging Blow", ret => true),
                            CreateSpellCheckAndCast("Dragon Roar", ret => true))),
                    new Decorator(ret => detectAdds().Count >= 6,
                        new PrioritySelector(
                            CreateSpellCheckAndCast("Berserker Rage", ret => !EnrageAura),
                            CreateSpellCheckAndCast("Bladestorm"),
                            CreateSpellCheckAndCast("Whirlwind", ret => true),
                            CreateSpellCheckAndCast("Dragon Roar", ret => true)))));
        }
        public Composite ArmsRotation()
        {
            return new Decorator(ret => Me.Level == 90 && DunatanksSettings.Instance.useArms,
                new PrioritySelector(
                    CreateSpellCheckAndCast("Berserker Rage", ret => !EnrageAura && Me.CurrentRage < 90),
                    CreateSpellCheckAndCast("Sweeping Strikes", ret => detectAdds().Count > 1),
                    CreateSpellCheckAndCast("Bladestorm", ret => detectAdds().Count > 3),
                    CreateSpellCheckAndCast("Colossus Smash", ret => !ColossusSmash1s),
                    CreateSpellCheckAndCast("Mortal Strike"),
                    CreateSpellCheckAndCast("Execute", ret => !SuddenExecuteAura),
                    CreateSpellCheckAndCast("Heroic Strike", ret => (ColossusSmashAura && Me.CurrentRage >= 80 && !ExecutePhase) || Me.CurrentRage >= 105),
                    CreateSpellCheckAndCast("Dragon Roar", ret => BloodBathAura && !ColossusSmashAura && !ExecutePhase),
                    CreateSpellCheckAndCast("Storm Bolt", ret => ColossusSmashAura),
                    CreateSpellCheckAndCast("Execute", ret => ColossusSmashAura || RecklessnessAura || Me.CurrentRage > 95),
                    CreateSpellCheckAndCast("Dragon Roar", ret => (BloodBathAura && !ExecutePhase) || (!ColossusSmashAura && ExecutePhase)),
                    CreateSpellCheckAndCast("Overpower", ret => (!ExecutePhase && TasteForBloodStacks >= 3) || !ExecutePhase || SuddenExecuteAura),
                    CreateSpellCheckAndCast("Slam", ret => (Me.CurrentRage >= 40 || RecklessnessAura || (ColossusSmash25s)) && !ExecutePhase),
                    CreateSpellCheckAndCast("Battle Shout"),
                    CreateSpellCheckAndCast("Heroic Throw")));
        }
        public Composite ProtRotation()
        {
            return new Decorator(ret => Me.Level == 90 && DunatanksSettings.Instance.useProt,
                new PrioritySelector(
                // Ultimatum == Me.HasAura(122510)
                    CreateSpellCheckAndCast("Impending Victory", ret => Me.HealthPercent < 65),
                    CreateSpellCheckAndCast("Taunt", ret => DunatanksSettings.Instance.useTaunt && (!Me.CurrentTarget.Aggro && Me.CurrentTarget.GotTarget && !TargetTargetIsTank())),
                    CreateSpellCheckAndCast("Shockwave", ret => true),
                    CreateSpellCheckAndCast("Dragon Roar", ret => detectAdds().Count > 2 || Me.CurrentTarget.IsBoss),
                    CreateSpellCheckAndCast("Deadly Calm", ret => Me.CurrentTarget.IsBoss || detectAdds().Count > 2),
                    CreateSpellCheckAndCast("Cleave", ret => detectAdds().Count > 1 && Me.HasAura(122510) || detectAdds().Count > 1 && Me.CurrentRage > 70),
                    CreateSpellCheckAndCast("Execute", ret => Me.CurrentTarget.HealthPercent < 20 && detectAdds().Count < 3 && Me.CurrentRage > 70),
                    CreateSpellCheckAndCast("Avatar", ret => true),
                    CreateSpellCheckAndCast("Heroic Strike", ret => Me.HasAura("Ultimatum")),
                    CreateSpellCheckAndCast("Berserker Rage", ret => true),
                    CreateSpellCheckAndCast("Shield Slam", ret => Me.CurrentRage < 75),
                    CreateSpellCheckAndCast("Revenenge", ret => Me.CurrentRage < 75),
                    CreateSpellCheckAndCast("Commanding Shout", ret => Me.CurrentRage < 80),
                    CreateSpellCheckAndCast("Shield Block", ret => Me.HealthPercent <= DunatanksSettings.Instance.SBHealth),
                    CreateSpellCheckAndCast("Shield Barrier", ret => !Me.HasAura("Shield Barrier") && Me.CurrentRage > 80 && Me.HealthPercent < DunatanksSettings.Instance.SBarrHealth),
                    CreateSpellCheckAndCast("Thunder Clap", ret => !Me.CurrentTarget.HasAura("Weakened Blows")),
                    CreateSpellCheckAndCast("Shield Wall", ret => !Me.HasAura("Shield Block") && Me.HealthPercent <= DunatanksSettings.Instance.SWHealth),
                    CreateSpellCheckAndCast("Demoralizing Shout", ret => true),
                    CreateSpellCheckAndCast("Impending Victory", ret => true),
                    CreateSpellCheckAndCast("Victory Rush", ret => !SpellManager.HasSpell("Impending Victory")),
                    CreateSpellCheckAndCast("Devastate", ret => true)));
        }
        public Composite FuryLeveling()
        {
            return new Decorator(ret => Me.Level >= 10 && Me.Level < 90 && DunatanksSettings.Instance.useFury,
                new PrioritySelector(
                    CreateSpellCheckAndCast("Berserker Rage", ret => (!IsEnraged(Me) || Me.CurrentTarget.HealthPercent >= 20)),
                    CreateSpellCheckAndCast("Deadly Calm", ret => Me.CurrentRage >= 40),
                    CreateSpellCheckAndCast("Heroic Strike", ret => Me.CurrentTarget.HealthPercent >= 20 && Me.PowerPercent > 95 && detectAdds().Count == 1),
                    CreateSpellCheckAndCast("Cleave", ret => Me.CurrentTarget.HealthPercent >= 20 && Me.PowerPercent > 95 && detectAdds().Count == 2),
                    CreateSpellCheckAndCast("Whirlwind", ret => Me.CurrentTarget.HealthPercent >= 20 && Me.PowerPercent > 95 && detectAdds().Count > 2),
                    CreateSpellCheckAndCast("Bloodthirst", ret => true),
                    CreateSpellCheckAndCast("Colossus Smash", ret => true),
                    CreateSpellCheckAndCast("Execute", ret => true),
                    CreateSpellCheckAndCast("Raging Blow", ret => true),
                    CreateSpellCheckAndCast("Shockwave", ret => true),
                    CreateSpellCheckAndCast("Dragon Roar", ret => true),
                    CreateSpellCheckAndCast("Bladestorm", ret => detectAdds().Count > 1),
                    CreateSpellCheckAndCast("Heroic Strike", ret => Me.CurrentTarget.HealthPercent >= 20 && detectAdds().Count == 1),
                    CreateSpellCheckAndCast("Cleave", ret => Me.CurrentTarget.HealthPercent >= 20 && detectAdds().Count == 2),
                    CreateSpellCheckAndCast("Whirlwind", ret => Me.CurrentTarget.HealthPercent >= 20 && detectAdds().Count > 2),
                    CreateSpellCheckAndCast("Wild Strike", ret => Bloodsurge()),
                    CreateSpellCheckAndCast("Battle Shout", ret => Me.CurrentRage < 70 && !Me.CurrentTarget.HasAura("Colossus Smash")),
                    CreateSpellCheckAndCast("Bladestorm", ret => detectAdds().Count > 1),
                    CreateSpellCheckAndCast("Battle Shout", ret => Me.CurrentTarget.HealthPercent < 70)));
        }
        public Composite ArmsLeveling()
        {
            return new Decorator(ret => Me.Level >= 10 && Me.Level < 90 && DunatanksSettings.Instance.useArms,
                new PrioritySelector(
                    CreateSpellCheckAndCast("Berserker Rage", ret => Me.CurrentTarget.HealthPercent >= 20 && !IsEnraged(Me)),
                    CreateSpellCheckAndCast("Sweeping Strikes", ret => detectAdds().Count > 1),
                    CreateSpellCheckAndCast("Mortal Strike", ret => true),
                    CreateSpellCheckAndCast("Colossus Smash", ret => true),
                    CreateSpellCheckAndCast("Execute", ret => true),
                    CreateSpellCheckAndCast("Bladestorm", ret => detectAdds().Count > 1),
                    CreateSpellCheckAndCast("Overpower", ret => Me.CurrentTarget.HealthPercent >= 20),
                    CreateSpellCheckAndCast("Slam", ret => Me.CurrentTarget.HealthPercent >= 20 && detectAdds().Count == 1),
                    CreateSpellCheckAndCast("Cleave", ret => Me.CurrentTarget.HealthPercent >= 20 && detectAdds().Count > 1),
                    CreateSpellCheckAndCast("Thunder Clap", ret => Me.CurrentTarget.HealthPercent >= 20 && detectAdds().Count > 3),
                    CreateSpellCheckAndCast("Dragon Roar", ret => true),
                    CreateSpellCheckAndCast("Shockwave", ret => true),
                    CreateSpellCheckAndCast("Heroic Strike", ret => Me.CurrentTarget.HealthPercent >= 20 && !SpellManager.HasSpell("Slam") || TasteForBloodStacks >= 1),
                    CreateSpellCheckAndCast("Heroic Throw", ret => Me.CurrentTarget.HealthPercent >= 20),
                    CreateSpellCheckAndCast("Battle Shout", ret => Me.CurrentTarget.HealthPercent >= 20 && Me.CurrentRage < 70)));
        }
        public Composite ProtLeveling()
        {
            return new Decorator(ret => Me.Level >= 10 && Me.Level < 90 && DunatanksSettings.Instance.useProt,
                new PrioritySelector(
                    CreateSpellCheckAndCast("Impending Victory", ret => Me.HealthPercent < 65),
                    CreateSpellCheckAndCast("Taunt", ret => DunatanksSettings.Instance.useTaunt && (!Me.CurrentTarget.Aggro && Me.CurrentTarget.GotTarget && !TargetTargetIsTank())),
                    CreateSpellCheckAndCast("Thunder Clap", ret => true),
                    CreateSpellCheckAndCast("Shockwave", ret => true),
                    CreateSpellCheckAndCast("Dragon Roar", ret => detectAdds().Count > 2 || Me.CurrentTarget.IsBoss),
                    CreateSpellCheckAndCast("Berserker Rage", ret => true),
                    CreateSpellCheckAndCast("Deadly Calm", ret => Me.CurrentTarget.IsBoss || detectAdds().Count > 2),
                    CreateSpellCheckAndCast("Cleave", ret => detectAdds().Count > 1 && Me.HasAura(122510) || detectAdds().Count > 1 && Me.CurrentRage > 70),
                    CreateSpellCheckAndCast("Heroic Strike", ret => Me.HasAura(122510)),
                    CreateSpellCheckAndCast("Execute", ret => Me.CurrentTarget.HealthPercent < 20 && detectAdds().Count < 3 && Me.CurrentRage > 70),
                    CreateSpellCheckAndCast("Shield Slam", ret => true),
                    CreateSpellCheckAndCast("Revenge", ret => true),
                    CreateSpellCheckAndCast("Demoralizing Shout", ret => true),
                    CreateSpellCheckAndCast("Devastate", ret => true)));
        }
        public Composite Lowbie()
        {
            return new Decorator(ret => Me.Level < 10,
                new PrioritySelector(
                    CreateSpellCheckAndCast("Execute", ret => true),
                    CreateSpellCheckAndCast("Heroic Strike", ret => true)));
        }
    }
}