#region Revision info

/*
 * $Author: Alex & Wulf$
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1$
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Hunter/Marksmanship.cs $
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

namespace PureRotation.Classes.Hunter
{
    [UsedImplicitly]
    internal class Markmanship : RotationBase
    {
        /*
                private static double TimeToLive = 0;
        */
        /*
                private static int NumNearByUnits = 0;
        */
        private static HunterSettings HunterSettings { get { return PRSettings.Instance.Hunter; } }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector
            (
                Common.HandleDeterrence()
            );
        }

        private static Composite SelfHealLogic()
        {
            return new PrioritySelector
            (
                Common.HandleBagHealingItems(),
                new Decorator
                (
                    ret => Common.UsingPet,
                    new PrioritySelector
                    (
                        new Decorator(ret => (Me.Pet == null || !Pet.IsAlive), new ActionAlwaysSucceed()),
                        Spell.Cast(SpellBook.Exhileration, ret => TalentManager.HasTalent(7) && Pet.HealthPercent < HunterSettings.ExhilarationHP && HunterSettings.UseExhilaration),
                        Spell.Cast(SpellBook.MendPet, ret => Pet.HealthPercent < HunterSettings.MendPetPercent && !Pet.HasAura(SpellBook.MendPet) && Pet.InLineOfSpellSight)
                    )
                )
            );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                Item.UseBagItem(Defines.DPSPotion, ret => PRSettings.Instance.UsePotion && Common.HasSpeedBuff && !Me.CachedHasAura(Defines.DPSPotionAura), "Virmen's Bite"),
                Spell.Cast(Defines.Rabid, ret => HunterSettings.UseRabid),
                Spell.Cast(SpellBook.LynxRush, ret => HunterSettings.UseLynxRush && TalentManager.HasTalent(15) && !Me.CurrentTarget.CachedHasAura("Lynx Rush")),
                Spell.Cast(SpellBook.AMurderOfCrows, ret => Me.CurrentFocus >= 60 && TalentManager.HasTalent(13) && !Me.CurrentTarget.CachedHasAura(SpellBook.AMurderOfCrows, 0, true, 2000)),/*Spell.GetAuraTimeLeft(Defines.AMoC, Me.CurrentTarget) < 2*/
                Spell.Cast(SpellBook.Stampede, ret => HunterSettings.UseStampede && Common.HasSpeedBuff),
                Spell.PreventDoubleCast(SpellBook.RapidFire, 1, ret => HunterSettings.UseRapidFire && !Me.CachedHasAura(SpellBook.RapidFire, 0, true, 1000) && Common.WaitForSpeedBuff),//Spell.GetAuraTimeLeft(Defines.RapidFire, Me) < 1
                Spell.PreventDoubleCast(SpellBook.Readiness, 1, ret => HunterSettings.UseReadiness && Spell.GetSpellCooldown(SpellBook.RapidFire).Seconds > 20 && Common.Tier5TalentOnCooldown && (Spell.GetSpellCooldown(SpellBook.DireBeast).Seconds > 10 || !TalentManager.HasTalent(11)))
            );
        }

        private static Composite HandleCommon()
        {
            return new PrioritySelector
            (
                Common.RotationPauseCheck(),
                Racials.UseRacials(),
                Common.HandleMisdirection(),
                Common.HandleTranq(),
                EncounterSpecific.HandleActionBarInterrupts(),
                SelfHealLogic(),
                Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                Item.HandleItems()
            );
        }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                new Decorator(ret => Me.CurrentTarget.HealthPercent >= 80, SingleTagetCarefulAimPhase()),
                new Decorator(ret => Me.CurrentTarget.HealthPercent < 80, SingleTagetNormal())
            );
        }

        private static Composite SingleTagetCarefulAimPhase()
        {
            return new PrioritySelector(
                //Spell.Cast(Defines.HuntersMark, ret => Common.NeedHuntersMark),
                Spell.PreventDoubleCast(SpellBook.SerpentSting, 1, ret => !Common.HasSerpentSting),
                Spell.Cast(SpellBook.SteadyShot, ret => !Me.CachedHasAura(Defines.SteadyFocus) || (Me.CachedHasAura(Defines.SteadyFocus) && Spell.GetAuraTimeLeft(Defines.SteadyFocus, Me) < 6)),
                Spell.Cast(SpellBook.ChimeraShot, ret => CanCastChimera),
                Spell.Cast(SpellBook.AimedShot, ret => ((Me.CurrentFocus >= 50 || Me.CachedHasAura(Defines.Fire)) && Me.CachedHasAura("Steady Focus", 0, true, 6000))),
                Spell.Cast(SpellBook.SteadyShot)
            );
        }

        private static Composite SingleTagetNormal()
        {
            return new PrioritySelector(
                //Spell.Cast(Defines.HuntersMark, ret => Common.NeedHuntersMark),
                Spell.PreventDoubleCast(SpellBook.SerpentSting, 1, ret => !Common.HasSerpentSting),
                Spell.Cast(SpellBook.KillShot, ret => Common.UseKillShot),
                Spell.Cast(SpellBook.SteadyShot, ret => !Me.CachedHasAura(Defines.SteadyFocus) || (Me.CachedHasAura(Defines.SteadyFocus) && Spell.GetAuraTimeLeft(Defines.SteadyFocus, Me) < 6)),
                Spell.Cast(SpellBook.DireBeast, ret => TalentManager.HasTalent(11) && !Me.CachedHasAura(SpellBook.DireBeast, 0, true, 2000) && Common.UseDB && Common.TimeToLive(15)),
                Spell.Cast(SpellBook.ChimeraShot, ret => CanCastChimera),

                new Decorator(ret => Spell.GetSpellCooldown(SpellBook.ChimeraShot).TotalMilliseconds < HunterSettings.WaitTime, new ActionAlwaysSucceed()),

                Spell.Cast(SpellBook.GlaiveToss, ret => TalentManager.HasTalent(16) && Common.UseGT),
                Spell.Cast(SpellBook.Powershot, ret => TalentManager.HasTalent(17)),
                Spell.Cast(SpellBook.Barrage, ret => TalentManager.HasTalent(18)),
                Spell.Cast(SpellBook.AimedShot, ret => CanCastAimedShot),
                Spell.PreventDoubleCast(SpellBook.ArcaneShot, 1, ret => Common.ThrillBuff),
                Spell.Cast(SpellBook.SteadyShot, ret => Me.CurrentFocus <= 60 && !Common.HasSerpentStingCobra),
                Spell.PreventDoubleCast(SpellBook.ArcaneShot, 1, ret => Common.ArcaneShotDump),
                Spell.Cast(SpellBook.SteadyShot)
            );
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                Spell.Cast(SpellBook.KillShot, ret => Common.UseKillShot),
                Spell.Cast(SpellBook.SteadyShot, ret => !Me.CachedHasAura(Defines.SteadyFocus) || (Me.CachedHasAura(Defines.SteadyFocus) && Spell.GetAuraTimeLeft(Defines.SteadyFocus, Me) < 6)),
                Spell.Cast(SpellBook.DireBeast, ret => TalentManager.HasTalent(11) && !Me.CachedHasAura(SpellBook.DireBeast, 0, true, 2000) && Common.UseDB && Common.TimeToLive(15)),
                Spell.Cast(SpellBook.GlaiveToss, ret => TalentManager.HasTalent(16) && Common.UseGT),
                Spell.Cast(SpellBook.Powershot, ret => TalentManager.HasTalent(17)),
                Spell.Cast(SpellBook.Barrage, ret => TalentManager.HasTalent(18)),
                Spell.CastHunterTrap("Explosive Trap", loc => Me.CurrentTarget.Location),
                Spell.Cast(SpellBook.AimedShot, ret => Me.CachedHasAura(Defines.Fire)),
                Spell.Cast(SpellBook.MultiShot, ret => Common.AoEMulti || (Me.CachedHasAura(Defines.Bombardment) && Me.CurrentFocus > 25)),
                Spell.Cast(SpellBook.SteadyShot)
            );
        }

        // If we have FeignedDeath, clear the target and return ActionAlwaysSucceeded() to pause the routine
        public static Composite FeignDeath()
        {
            return new Decorator(ret => Me.CachedHasAura(SpellBook.FeignDeath),
             new Sequence(
                new Action(ret => Me.ClearTarget()),
                new ActionAlwaysSucceed()
                ));
        }

        #region booleans

        private static bool CanCastChimera { get { return Me.CurrentFocus > 45; } }

        private static bool CanCastAimedShot { get { return Me.CachedHasAura(Defines.Fire) || (Me.CurrentFocus > 50 && Spell.GetSpellCastTime(SpellBook.AimedShot) < 1.3); } }

        #endregion booleans

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.HunterMarksmanship; }
        }

        public override string Name
        {
            get { return "Markmanship Hunter"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector
                (
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    HandleCommon(),
                    new Decorator
                    (
                        ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                        new PrioritySelector
                         (

                            // If current target is a boss and we have enough combat time left to make it worthwhile, use cooldowns
                            new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),
                            new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 25).Count(x => !x.IsBoss()) > HunterSettings.AoECount, HandleAoeCombat()),
                            new Decorator(ret => Me.CurrentTarget != null, HandleSingleTarget())
                         )
                    ),
                    new Decorator
                    (
                        ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                        new PrioritySelector
                         (
                            new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                            new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 25).Count(x => !x.IsBoss()) > HunterSettings.AoECount, HandleAoeCombat()),
                            new Decorator(ret => Me.CurrentTarget != null, HandleSingleTarget())
                         )
                    ),
                    new Decorator
                    (
                        ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                        new PrioritySelector
                        (
                            new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                            new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                            new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget())
                        )
                    )
                );
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
                        ));
            }
        }

        #endregion Overrides of RotationBase
    }
}