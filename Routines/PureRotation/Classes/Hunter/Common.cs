#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-08-30 20:20:43 +0200 (Fr, 30 Aug 2013) $
 * $ID$
 * $Revision: 1705 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Hunter/Common.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade: $
 */

#endregion Revision info

using System;
using System.Collections.Generic;
using System.Linq;
using PureRotation.Core;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;
using PRHelpers = PureRotation.Helpers;
using CommonBehaviors.Actions;

namespace PureRotation.Classes.Hunter
{
    #region Buff Defines

    public static class Defines
    {
        public static int ThrillOfTheHunt = 34720;
        public static int Bombardment = 35110;
        public static int SteadyFocus = 53224;
        public static int LockAndLoad = 56453;
        public static int Fire = 82926;
        public static int DPSPotionAura = 105697;  	// Virmen's Potion as of 5.2
        public static int DPSPotion = 76089;		// Viremen's Bite
        public static int HealingPot = 76097; 		// Master Healing Potion
        public static int SerpentStingAura = 118253;
        public static int HeartOfThePhoenix = 55709;
        public static int Rabid = 53401;
        public static int BeastWithin = 34471; //34692;
    }

    #endregion Buff Defines

    internal static class Common
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static WoWUnit CurrentTarget { get { return Me.CurrentTarget; } }

        private static readonly List<WoWPetSpell> PetSpells = new List<WoWPetSpell>();
        private static ulong _petGUID;

        // When true, applies hunters mark.  When false, assume it will be applied via the glyph
        //public static bool NeedHuntersMark { get { return PRSettings.Instance.Hunter.ApplyHuntersMark && !Me.CurrentTarget.CachedHasAura(Defines.HuntersMark); } }

        // Returns true if Kill Shot is available
        public static bool UseKillShot { get { return Me.CurrentTarget.HealthPercent <= 20; } }

        // Returns true if we can/should use Glaive Toss
        public static bool UseGT { get { return PRSettings.Instance.Hunter.UseGlaiveToss && TalentManager.HasTalent(16); } }

        // Returns true if we can/should use Dire Beast
        public static bool UseDB { get { return PRSettings.Instance.Hunter.UseDireBeast && TalentManager.HasTalent(11) && Me.CurrentFocus < 90; } }

        // Returns true if we have the Thrill of the Hunt Buff
        public static bool ThrillBuff { get { return Me.HasAura(Defines.ThrillOfTheHunt); } }

        // returns true if we have enough focus to fire a MS
        public static bool AoEMulti { get { return Me.CurrentFocus > 40 || (ThrillBuff && Me.CurrentFocus > 20); } }

        // Returns true if WoWEquipSlot currently have the Rapid Fire buff
        public static bool WaitForRapidFire { get { return Me.HasAura(SpellBook.RapidFire); } }

        // Returns false if we are under any heroism type speed buffs
        public static bool WaitForSpeedBuff { get { return !Me.HasAnyAura(Spell.HeroismBuff);/*!Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Time Warp");*/ } }

        // Returns true if we are under any heroism type speed buffs
        public static bool HasSpeedBuff { get { return Me.HasAnyAura(Spell.HeroismBuff) || Me.HasAura("Rapid Fire");/*Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Time Warp", "Rapid Fire");*/ } }

        // Returns true if we have plenty of focus available to dump via AS
        public static bool ArcaneShotDump { get { return Me.CurrentFocus >= 61; } }

        public static bool Tier5TalentOnCooldown
        {
            get
            {
                return (TalentManager.HasTalent(13) && Spell.GetSpellCooldown("A Murder of Crows").Seconds > 15) ||
                       (TalentManager.HasTalent(14) /*&& Spell.GetSpellCooldown("Blink Strikes").Seconds > 5*/) ||
                       (TalentManager.HasTalent(15) && Spell.GetSpellCooldown("Lynx Rush").Seconds > 15);
            }
        }

        public static bool UsingPet
        {
            get
            {
                if (Me.Specialization == WoWSpec.HunterBeastMastery) return Me.GotAlivePet;
                if (Me.Specialization == WoWSpec.HunterSurvival) return Me.GotAlivePet;

                return true;
            }
        }

        public static void CastPetAction(string action)
        {
            UpdatePetSpells();
            WoWPetSpell spell = PetSpells.FirstOrDefault(p => p.ToString() == action);
            if (spell == null) return;

            Lua.DoString("CastPetAction({0})", spell.ActionBarIndex + 1);
        }

        private static void UpdatePetSpells()
        {
            // if (!Timers.SpellOkToCast("PetSpells", 500)) return;
            if (!Me.GotAlivePet) return;
            if (_petGUID == Me.Pet.Guid) return;

            _petGUID = Me.Pet.Guid;
            PetSpells.Clear();
            PetSpells.AddRange(StyxWoW.Me.PetSpells);

            //  Timers.Reset("PetSpells");
        }

        // Returns true if target has at least nTTL seconds to live - useful to prevent blowing long DPS cooldowns that won't have time to reach full effect
        public static bool TimeToLive(int nTTL)
        {
            return PRHelpers.DpsMeter.GetCombatTimeLeft(Me.CurrentTarget).TotalSeconds > nTTL;
        }

        public static int GetPlayerFocus()
        {
            //return Me.GetPowerInfo(WoWPowerType.Focus).CurrentI;
            //return Lua.GetReturnVal<int>("return UnitPower(\"player\", SPELL_POWER_FOCUS);", 0);
			return (int)PureRotation.Helpers.Lua.PlayerPower;
        }

        public static int PredictFocus()
        {
            if (Me.IsCasting)
            {
                switch (Me.CastingSpell.Id)
                {
                    case 77767:
                        return GetPlayerFocus() + 21; //21
                    case 56641:
                        return GetPlayerFocus() + 21; //21
                }
            }
            return GetPlayerFocus();
        }

        internal static Composite ClearExplosiveTrap()
        {
            return new Decorator(ret => !StyxWoW.Me.HasPendingSpell("Explosive Trap"), new Action(ret => Lua.DoString("SpellStopTargeting()")));
        }

        public static Composite HuntersMark()
        {
            return new Decorator(ret => !CurrentTarget.HasMyAura(SpellBook.HuntersMark) && CurrentTarget.Distance < 100,
                                new PrioritySelector(
                                    Spell.Cast(SpellBook.HuntersMark))
                                );
        }

        #region Dots

        internal static bool HasSerpentSting
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                //return Me.CurrentTarget.HasCachedAura(Defines.SerpentStingAura, 0, 2000);	// ID doesn't work - have to use string...why?
                //return Me.CurrentTarget.CachedHasAura("Serpent Sting", 0, true, 2000);
				return Me.CurrentTarget!=null && Me.CurrentTarget.HasAura("Serpent Sting", 0, true, 2000);
            }
        }

        internal static bool HasSerpentStingCobra
        {
            get
            {
                if (!Me.GotTarget)
                    return false;
                //return Me.CurrentTarget.HasCachedAura(Defines.SerpentStingAura, 0, 3000);	// ID doesn't work - have to use string...why?
                return Me.CurrentTarget.HasAura("Serpent Sting", 0, true, 3000);
            }
        }

        #endregion Dots

        // Returns best Misdirection target
        // Returns pet if not in group, otherwise:
        // Focus > Tank > pet
        public static WoWUnit BestMisdirectionTarget
        {
            get
            {
                if (!PRSettings.Instance.Hunter.UseMisdirection) return null;

                WoWUnit bestTarget = Unit.Tanks.FirstOrDefault(t => t.IsAlive && t.Distance < 100);
                if (bestTarget == Me && Me.GotAlivePet)
                    bestTarget = Me.Pet;
                else
                    bestTarget = null;

                return bestTarget;
            }
        }

        public static Composite HandleMiscShots()
        {
            return new PrioritySelector
            (
                new Decorator(ret => Me.CurrentTarget == null, new ActionAlwaysSucceed()),
                new Decorator(ret => PRSettings.Instance.Hunter.UseMisdirection,
                    new PrioritySelector
                    (
                        Spell.Cast(SpellBook.Misdirection, u => BestMisdirectionTarget, ret => BestMisdirectionTarget != null && Me.CurrentTarget != null && !Me.HasAura(SpellBook.Misdirection) && Me.CurrentTarget.ThreatInfo.RawPercent > 90)
                    )
                ),
                new Decorator(ret => PRSettings.Instance.Hunter.UseTranq,
                    new PrioritySelector
                    (
                        Spell.Cast(SpellBook.TranquilizingShot, ret => Me.CurrentTarget.CharmedByUnitGuid == 0 && Me.CurrentTarget.Auras.Values.Any(a => a.Cancellable && a.TimeLeft > TimeSpan.FromSeconds(3)))
                    )
                )
            );
        }

        // If enabled, uses Misdirection on the best target if threat is too high on the current target
        // See BestMisdirectionTarget
        public static Composite HandleMisdirection()
        {
            return new PrioritySelector
            (
                new Decorator(ret => PRSettings.Instance.Hunter.UseMisdirection,
                    new PrioritySelector
                    (
                        Spell.Cast(SpellBook.Misdirection, u => BestMisdirectionTarget, ret => BestMisdirectionTarget != null && Me.CurrentTarget != null && !Me.HasAura(SpellBook.Misdirection) && Me.CurrentTarget.ThreatInfo.RawPercent > 90)
                    )
                )
            );
        }

        // If we have FeignDeath or Deterrence, clear the target and return ActionAlwaysSucceeded() to pause the routine
        public static Composite RotationPauseCheck()
        {
            return new PrioritySelector
            (
                new Decorator
                (
                    ret => Me.HasAura(SpellBook.FeignDeath),
                    new Sequence
                    (
                        new Action(ret => Me.ClearTarget()),
                        new ActionAlwaysSucceed()
                    )
                ),
                new Decorator(ret => Me.HasAura(SpellBook.Deterrence), new ActionAlwaysSucceed())
            );
        }

        public static Composite CheckAspect()
        {
            return new PrioritySelector
            (
                Spell.Cast(SpellBook.AspectoftheHawk, ret => PRSettings.Instance.Hunter.ForceHawk && !TalentManager.HasTalent(8) && !Me.HasAura(SpellBook.AspectoftheHawk)),
                Spell.Cast("Aspect of the Iron Hawk", ret => PRSettings.Instance.Hunter.ForceHawk && TalentManager.HasTalent(8) && !Me.HasAura("Aspect of the Iron Hawk"))
            );
        }

        // If enabled, uses Tranquilizing Shot on the current target
        public static Composite HandleTranq()
        {
            //We have to look deeper into details for a better logic, but this should work for now ... Thanks to Wile91
            return new PrioritySelector
            (
                new Decorator(ret => PRSettings.Instance.Hunter.UseTranq,
                    new PrioritySelector
                    (
                        Spell.Cast(SpellBook.TranquilizingShot, ret => Me.CurrentTarget.CharmedByUnitGuid == 0 // We don't want to purge mind controlled friends. Must be a better way to check for this?
                            && Me.CurrentTarget.Auras.Values.Any(a => a.Cancellable && a.TimeLeft > TimeSpan.FromSeconds(3)))
                    )
                )
            );
        }

        public static Composite HandleDeterrence()
        {
            return new PrioritySelector
            (
                Spell.Cast(SpellBook.Deterrence, ret => Me.HealthPercent < PRSettings.Instance.Hunter.DeterrencePercent)
            );
        }

        // Uses items to self heal if needed, as determined by user settings.  Supported items are:
        //  Healthestone
        //  Master Healing Potion
        public static Composite HandleBagHealingItems()
        {
            return new PrioritySelector
            (
                new Decorator
                (
                    ret => Me.HealthPercent < 100,
                    new PrioritySelector
                    (
                        Item.UseBagItem(5512, ret => Me.HealthPercent < PRSettings.Instance.Hunter.HealthstonePercent, "Healthstone [" + Me.HealthPercent + "% HP]"),
                        Item.UseBagItem(Defines.HealingPot, ret => Me.HealthPercent < PRSettings.Instance.Hunter.PotionPercent, "Master Healing Potion [" + Me.HealthPercent + "% HP]")
                    )
                )
            );
        }
    }
};