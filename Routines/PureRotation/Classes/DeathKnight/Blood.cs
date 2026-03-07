#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/DeathKnight/Blood.cs $
 * $LastChangedBy: wulf$
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
using Styx.TreeSharp;

namespace PureRotation.Classes.DeathKnight
{
    [UsedImplicitly]
    public class Blood : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Anti-Magic Shell", on => Me, ret => NeedAntiMagicShell),
                Spell.Cast("Bone Shield", on => Me, ret => NeedBoneShield),
                new Decorator(ret => Me.HealthPercent < 100 && PRSettings.Instance.EnableSelfHealing,
                              new PrioritySelector(
                                  Spell.Cast("Rune Tap", on => Me, ret => NeedRuneTapWoTn), // Instant
                                  Spell.Cast("Rune Tap", ret => Me, ret => NeedRuneTap), // 30 sec cooldown - default 90 percentHP
                                  Spell.Cast("Death Pact", on => Me, ret => NeedDeathPact), // 2 min cooldown  - default 50 percentHP
                                  Spell.Cast("Raise Dead", on => Me, ret => NeedRaiseDead), // 2 min cooldown - default 50 percentHP
                                  Spell.Cast("Death Coil", on => Me, ret => NeedDeathCoilHeal), // 2 min cooldown  - default 60 percentHP
                                  Spell.Cast("Lichborne", on => Me, ret => NeedLichborne), // 2 min cooldown - default 60 percentHP
                                  Spell.Cast("Vampiric Blood", on => Me, ret => NeedVampiricBlood), // 1 min cooldown - default 60 percentHP
                                  Spell.Cast("Icebound Fortitude", on => Me, ret => NeedIceboundFortitude), // 3 min cooldown - default 60 percentHP
                                  Spell.Cast("Dancing Rune Weapon", ret => NeedDancingRuneWeapon), // 1.5 min cooldown - default 80 percentHP
                                  Spell.Cast("Empower Rune Weapon", on => Me, ret => NeedEmpowerRuneWeapon), // 5 min cooldown
                                  Item.UseBagItem(5512, ret => NeedHealthstone, "Healthstone"))));
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector();
        }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                Spell.Cast("Outbreak", ret => NeedEitherDis),
                Spell.PreventDoubleCast("Blood Boil", 0.5, ret => NeedToRefreshDiseasesWithBloodBoil && !NeedDeathStrike),
                Spell.PreventDoubleCast("Plague Strike", ThrottleTime, ret => NeedBloodPlague),
                Spell.PreventDoubleCast("Icy Touch", ThrottleTime, ret => NeedFrostFever),
                CooldownTracker.Cast("Death Strike", ret => NeedDeathStrike),
                Spell.Cast("Rune Strike", ret => NeedRuneStrike),
                //Spell.PreventDoubleCast("Blood Boil", 0.5, ret => HasCrimsonScourge), // get rid of the Proc
                //new Decorator(ret => HotKeyManager.IsSpecialKey, new PrioritySelector(Spell.PreventDoubleCast("Necrotic Strike", ThrottleTime, ret => NeedNecroticStrike))),
                Spell.Cast("Soul Reaper", ret => NeedSoulReaper && OkToUseBloodRuneForDamage),  // Never use Soul Reaping if you have no Blood runes, as this will cause Heart Strike to consume a Death rune, which should be saved for Death Strike.
                Spell.Cast(55050, ret => Me.BloodRuneCount > 0 && OkToUseBloodRuneForDamage), // Never use Heart Strike if you have no Blood runes, as this will cause Heart Strike to consume a Death rune, which should be saved for Death Strike.
                CooldownTracker.CastOnGround("Death and Decay", on => Me.CurrentTarget.Location, ret => Me.CurrentTarget != null &&  DpsMeter.GetCombatTimeLeft(Me.CurrentTarget).TotalSeconds > 10),
                Spell.Cast("Death Coil", ret => Me.CurrentTarget != null && !Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentRunicPower >= 90));
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                Common.HandleTierSix(),
                Spell.PreventDoubleCast("Blood Boil", 0.5, ret => HasCrimsonScourge && !NeedEitherDis), // get rid of the Proc
                Common.HandlePestilence(),
                Spell.Cast("Unholy Blight",ret=> PRSettings.Instance.DeathKnight.EnableUnholyBlight),
                Spell.Cast("Outbreak", ret => NeedEitherDis),
                Spell.PreventDoubleCast("Plague Strike", ThrottleTime, ret => NeedBloodPlague),
                Spell.PreventDoubleCast("Icy Touch", ThrottleTime, ret => NeedFrostFever),
                CooldownTracker.Cast("Death Strike", ret => NeedDeathStrike),
                CooldownTracker.CastOnGround("Death and Decay", on => Me.CurrentTarget.Location, ret => Me.CurrentTarget != null && DpsMeter.GetCombatTimeLeft(Me.CurrentTarget).TotalSeconds > 5),
                Spell.PreventDoubleCast("Blood Boil", 0.5, ret => ((Me.BloodRuneCount > 0 && OkToUseBloodRuneForDamage) || HasCrimsonScourge) && !NeedEitherDis),
                Spell.Cast("Rune Strike", ret => NeedRuneStrike));
        }

        #region booleans

        private static bool HasCrimsonScourge { get { return Me.ActiveAuras.ContainsKey("Crimson Scourge"); } } // StyxWoW.Me.ActiveAuras.ContainsKey("Crimson Scourge")

        // The Idea here is to not stack our defensive abilitys but use them one at a time for maximum effectiveness.
        private static bool NoOtherCooldownActive(string cooldownToCheck)
        {
            {
                switch (cooldownToCheck)
                {
                    case "VampiricBlood":
                        if (!Me.CachedHasAura("Bone Shield") &&
                            !Me.CachedHasAura("Icebound Fortitude") &&
                            !Me.CachedHasAura("Dancing Rune Weapon") &&
                            !Me.CachedHasAura("Lichborne")) return true;
                        break;

                    case "BoneShield":
                        if (!Me.CachedHasAura("Vampiric Blood") &&
                            !Me.CachedHasAura("Icebound Fortitude") &&
                            !Me.CachedHasAura("Dancing Rune Weapon") &&
                            !Me.CachedHasAura("Lichborne") &&
                            CooldownTracker.SpellOnCooldown(49998)) return true;
                        break;

                    case "IceboundFortitude":
                        if (!Me.CachedHasAura("Bone Shield") &&
                            !Me.CachedHasAura("Vampiric Blood") &&
                            !Me.CachedHasAura("Dancing Rune Weapon") &&
                            !Me.CachedHasAura("Lichborne") &&
                            CooldownTracker.SpellOnCooldown(49998)) return true;
                        break;

                    case "DeathPact":
                        if (!Me.CachedHasAura("Bone Shield") &&
                            !Me.CachedHasAura("Vampiric Blood") &&
                            !Me.CachedHasAura("Icebound Fortitude") &&
                            !Me.CachedHasAura("Dancing Rune Weapon") &&
                            !Me.CachedHasAura("Lichborne") &&
                            CooldownTracker.SpellOnCooldown(49998)) return true;
                        break;

                    case "DeathCoilHeal":
                        if (!Me.CachedHasAura("Bone Shield") &&
                            !Me.CachedHasAura("Vampiric Blood") &&
                            !Me.CachedHasAura("Icebound Fortitude") &&
                            !Me.CachedHasAura("Dancing Rune Weapon") &&
                            Spell.SpellOnCooldown(49998)) return true;
                        break;

                    case "DancingRuneWeapon":
                        if (!Me.CachedHasAura("Bone Shield") &&
                            !Me.CachedHasAura("Icebound Fortitude") &&
                            !Me.CachedHasAura("Vampiric Blood") &&
                            !Me.CachedHasAura("Lichborne") &&
                            CooldownTracker.SpellOnCooldown(49998)) return true;
                        break;
                }

                return false;
            }
        }

        // HandleDefensiveCooldowns
        private static bool NeedAntiMagicShell
        {
            get
            {
                if (!UseAntiMagicShell) return false;
                if (EncounterSpecific.LeiShiSprayStackCount(8)) return true; // If Lei Shi's Spray stacks are up to 8 hit AMS
                if (EncounterSpecific.TsulongShadowBreath) return true; // Saving AMS for Tsulongs Shadow Breath
                if (EncounterSpecific.ElegonCelestialBreath) return true; // Saving AMS for Elegon Celestial Breath

                return Me.CurrentTarget != null &&
                       Me.CurrentTarget.Entry != 62983 && // if the target is Lei Shi we want to save AMS for the above Spray stackcount
                       Me.CurrentTarget.Entry != 62442 && // if the target is Tsulong we want to save AMS for the above Shadow Breath
                       Me.CurrentTarget.Entry != 60410 && // if the target is Elegon we want to save AMS for the above Celestial Breath
                       (Me.CurrentTarget.IsCasting || Me.CurrentTarget.ChanneledCastingSpellId != 0) &&
                       Me.CurrentTarget.IsTargetingMeOrPet; // If there targeting us
            }
        }

        private static bool NeedBoneShield { get { return UseBoneShieldDefensively && NoOtherCooldownActive("BoneShield"); } }

        private static bool NeedDeathPact { get { return UsePetSacrifice && Me.HealthPercent < PetSacrificePercent && Me.Minions.FirstOrDefault(q => q.CreatureType == WoWCreatureType.Undead || q.CreatureType == WoWCreatureType.Totem) != null; } }

        private static bool NeedRuneTapWoTn { get { return UseRuneTapWoTn && Me.HealthPercent < RuneTapWoTnPercent && Me.CachedHasAura("Will of the Necropolis"); } }

        private static bool NeedRuneTap { get { return UseRuneTap && Me.HealthPercent <= RuneTapPercent && Me.BloodRuneCount > 0 && !Spell.SpellOnCooldown(48982); } }

        private static bool NeedDeathCoilHeal { get { return Me.HealthPercent < DeathCoilHealPercent && Me.CachedHasAura("Lichborne"); } }

        private static bool NeedVampiricBlood { get { return UseVampiricBlood && Me.HealthPercent < VampiricBloodPercent && NoOtherCooldownActive("VampiricBlood"); } }

        private static bool NeedDancingRuneWeapon { get { return UseDancingRuneWeapon && Me.HealthPercent < DancingRuneWeaponPercent && NoOtherCooldownActive("DancingRuneWeapon"); } }

        private static bool NeedLichborne { get { return UseLichborne && Me.HealthPercent < LichbornePercent && Me.CurrentRunicPower >= 60 && NoOtherCooldownActive("DeathCoilHeal"); } }

        private static bool NeedRaiseDead { get { return UsePetSacrifice && Me.HealthPercent < PetSacrificePercent && Me.Minions.FirstOrDefault(q => q.CreatureType == WoWCreatureType.Undead || q.CreatureType == WoWCreatureType.Totem) != null && NoOtherCooldownActive("DeathPact"); } }

        private static bool NeedIceboundFortitude { get { return UseIceboundFortitude && Me.HealthPercent < IceboundFortitudePercent && NoOtherCooldownActive("IceboundFortitude"); } }

        private static bool NeedEmpowerRuneWeapon { get { return Me.CurrentTarget != null && PRSettings.Instance.DeathKnight.UseEmpowerRuneWeapon && Me.CurrentTarget.IsBoss() && StyxWoW.Me.FrostRuneCount < 1 && StyxWoW.Me.UnholyRuneCount < 2 && StyxWoW.Me.DeathRuneCount < 1 && !Me.HasAnyAura(SpellLists.BurstHaste); } }

        private static bool NeedHealthstone { get { return Me.HealthPercent < PRSettings.Instance.DeathKnight.HealthstonePercent; } }


        // HandleSingleTarget
        private static bool NeedDeathStrike
        {
            get
            {
                return Me.HealthPercent < DeathStrikePercent ||
                        DeathStrikeTracker.DeathStrikeOK || // Tracks if its ok to deathstrike.
                       (Me.HealthPercent < DeathStrikeBloodShieldPercent &&
                       (Me.CachedHasAura("Blood Shield") && Me.ActiveAuras["Blood Shield"].TimeLeft.TotalSeconds < DeathStrikeBloodShieldTimeRemaining));
            }
        }

        private static bool NeedBloodTap { get { return Me.CachedHasAura("Blood Charge", 5) && Common.CanBloodTap ; } } 

        private static bool NeedRuneStrike { get { return (Me.CurrentRunicPower >= RuneStrikePercent || Me.HealthPercent > 90) && Me.CurrentRunicPower >= 30 && (Me.UnholyRuneCount == 0 || Me.FrostRuneCount == 0 || Me.CurrentRunicPower >= Me.MaxRunicPower) && !NeedDeathCoilHeal && !NeedDancingRuneWeapon; } }

        private static bool NeedSoulReaper { get { return Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && OkToUseBloodRuneForDamage && Me.BloodRuneCount > 0 && Me.CurrentTarget.HealthPercent < 35; } }

        private static bool NeedNecroticStrike { get { return ((StyxWoW.Me.UnholyRuneCount + StyxWoW.Me.FrostRuneCount + StyxWoW.Me.DeathRuneCount >= 2) || Me.CachedHasAura("Blood Charge", 9)) && StyxWoW.Me.HealthPercent > 96; } } // this is here for 5.2 as it will do more damage than heart trike.

        private static bool OkToUseBloodRuneForDamage { get { return !NeedRuneTap; } } // Lets not heart strike or use SoulReaper or blood boil if we need too use RuneTap

        // Diseases
        private static bool NeedToRefreshDiseasesWithBloodBoil { get { return ((Me.BloodRuneCount > 0 || HasCrimsonScourge || Me.DeathRuneCount >= 4) && NeedEitherDis); } }

        private static bool NeedEitherDis { get { return (NeedFrostFever || NeedBloodPlague); } }

        private static bool NeedFrostFever { get { return Me.CurrentTarget != null && !Me.CurrentTarget.CachedHasAura(55095, 0, true, 2000); } }

        private static bool NeedBloodPlague { get { return Me.CurrentTarget != null && !Me.CurrentTarget.CachedHasAura(55078, 0, true, 2000); } }

        #endregion booleans

        #region Settings

        private static DeathKnightSettings DeathKnightSettings { get { return PRSettings.Instance.DeathKnight; } }

        private static int PetSacrificePercent { get { return DeathKnightSettings.PetSacrificePercent; } }

        private static bool UsePetSacrifice { get { return DeathKnightSettings.UsePetSacrifice; } }

        private static int RuneTapPercent { get { return DeathKnightSettings.RuneTapPercent; } }

        private static bool UseRuneTap { get { return DeathKnightSettings.UseRuneTap; } }

        private static int RuneTapWoTnPercent { get { return DeathKnightSettings.RuneTapWoTNPercent; } }

        private static bool UseRuneTapWoTn { get { return DeathKnightSettings.UseRuneTapWoTN; } }

        private static int DeathCoilHealPercent { get { return DeathKnightSettings.DeathCoilHealPercent; } }

        private static int VampiricBloodPercent { get { return DeathKnightSettings.VampiricBloodPercent; } }

        private static bool UseVampiricBlood { get { return DeathKnightSettings.UseVampiricBlood; } }

        private static int DancingRuneWeaponPercent { get { return DeathKnightSettings.DancingRuneWeaponPercent; } }

        private static bool UseDancingRuneWeapon { get { return DeathKnightSettings.UseDancingRuneWeapon; } }

        private static int LichbornePercent { get { return DeathKnightSettings.LichbornePercent; } }

        private static bool UseLichborne { get { return DeathKnightSettings.UseLichborne; } }

        private static int IceboundFortitudePercent { get { return DeathKnightSettings.BloodIceboundFortitudePercent; } }

        private static bool UseIceboundFortitude { get { return DeathKnightSettings.UseIceboundFortitude; } }

        private static bool UseBoneShieldDefensively { get { return DeathKnightSettings.UseBoneShieldDefensively; } }

        private static bool UseAntiMagicShell { get { return DeathKnightSettings.UseAntiMagicShell; } }

        private static int DeathStrikePercent { get { return DeathKnightSettings.DeathStrikePercent; } }

        private static int DeathStrikeBloodShieldPercent { get { return DeathKnightSettings.DeathStrikeBloodShieldPercent; } }

        private static int DeathStrikeBloodShieldTimeRemaining { get { return DeathKnightSettings.DeathStrikeBloodShieldTimeRemaining; } }

        private static int RuneStrikePercent { get { return DeathKnightSettings.RuneStrikePercent; } }

        #endregion Settings

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }
        internal override string Help
        {
            get
            {
                return
                    @"
                     -----------------------------------------------
                     Special Key: Tier Six Talent in AOE-Mode
                     -----------------------------------------------
                     ";
            }
        }
        public override WoWSpec KeySpec
        {
            get { return WoWSpec.DeathKnightBlood; }
        }

        public override string Name
        {
            get { return "Deathknight Blood"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                        new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                        EncounterSpecific.HandleActionBarInterrupts(),
                        // Don't do anything if we have no target, nothing in melee range, or we're casting. (Includes vortex!)
                        new Decorator(ret => StyxWoW.Me.CurrentTarget != null && (!StyxWoW.Me.CurrentTarget.IsWithinMeleeRange || StyxWoW.Me.IsCasting), new ActionAlwaysSucceed()),
                        Spell.PreventDoubleCast(45529, ThrottleTime, ret => NeedBloodTap),
                        HandleDefensiveCooldowns(),                         // Stay Alive!
                        Item.HandleItems(),                                 // Pop Trinkets, Drink potions...
                        Racials.UseRacials(),
                        Spell.InterruptSpellCasts(ret => Me.CurrentTarget), // Interupt incomming damage!
                        new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                      new PrioritySelector(

                                          new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),
                                          new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.NearbyAttackableUnits(Me.Location, 10).Count() > DeathKnightSettings.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                          HandleSingleTarget())),

                        new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                      new PrioritySelector(
                                          new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                          new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.NearbyAttackableUnits(Me.Location, 10).Count() > DeathKnightSettings.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                          HandleSingleTarget())),

                        new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                      new PrioritySelector(

                                          new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                          new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                          new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))),
                        Spell.Cast("Horn of Winter", on => Me, ret => !Me.HasAura("Horn of Winter")));
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
                return new PrioritySelector(HandleDefensiveCooldowns());
            }
        }

        public override Composite PreCombat
        {
            get { return null; }
        }

        internal override void OnPulse()
        {
            if (StyxWoW.Me.CurrentTarget != null)
                DpsMeter.Update();
        }

        #endregion Overrides of RotationBase
    }
}