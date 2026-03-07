#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade: Stormchasing$
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
using Styx.WoWInternals.WoWObjects;
using Styx.CommonBot;
using T = PureRotation.Managers.TalentManager;
namespace PureRotation.Classes.DeathKnight
{
    [UsedImplicitly]
    internal class Unholy : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }
        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                        Spell.Cast("Death's Advance", ret => Me, ret => PRSettings.Instance.DeathKnight.EnableDeathsAdvance && (Me.HasAura(116784) || Me.HasAura(116417))),
                        Spell.Cast("Anti-Magic Shell", ret => Me, ret => PRSettings.Instance.DeathKnight.UseAntiMagicShell && Me.CurrentTarget != null && Me.CurrentTarget.IsCasting),    
                        Spell.Cast("Icebound Fortitude", ret => Me, ret => PRSettings.Instance.DeathKnight.UseIceboundFortitude && Me.CurrentTarget != null && Me.HealthPercent <= PRSettings.Instance.DeathKnight.FrostIceboundFortitudePercent),
                        Spell.Cast("Gift of the Naaru", ret => Me, ret => PRSettings.Instance.EnableGiftoftheNaaru && Me.CurrentTarget != null && Me.HealthPercent <= PRSettings.Instance.GiftNaaruHp),
                        Spell.Cast("Raise Dead", ret => Me, ret => NeedRaiseDead && Me.HealthPercent <= PRSettings.Instance.DeathKnight.PetSacrificePercent),
                        Spell.Cast("Death Pact", ret => Me, ret => PRSettings.Instance.DeathKnight.UsePetSacrifice && Me.HealthPercent <= PRSettings.Instance.DeathKnight.PetSacrificePercent && Me.Minions.FirstOrDefault(q => (q.CreatureType == WoWCreatureType.Undead || q.CreatureType == WoWCreatureType.Totem) && !q.HasAura("Control Undead")) != null && Spell.SpellOnCooldown("Raise Dead")),
                        Item.UseBagItem(5512, ret => NeedHealthstone, "Healthstone"));
        }
        private static Composite HandleDefensiveCooldownsPvP()
        {
            return new PrioritySelector(
                        new Decorator(ret => Me.HasAura("Pillar of Frost"), Item.UseEngineerGloves()),
                        CooldownTracker.Cast("Blood Presence", ret => Me.HealthPercent <= 36 && !Me.HasAura("Blood Presence")),
                        CooldownTracker.Cast("Death's Advance", ret => Me, ret => PRSettings.Instance.DeathKnight.EnableDeathsAdvance && (Me.CachedHasAura(116784) || Me.CachedHasAura(116417))),
                        CooldownTracker.Cast("Anti-Magic Shell", ret => Me, ret => PRSettings.Instance.DeathKnight.UseAntiMagicShell && Me.CurrentTarget != null && Me.CurrentTarget.IsCasting),
                        CooldownTracker.Cast("Anti-Magic Shell", ret => Me, ret => PRSettings.Instance.DeathKnight.AntiMagicShellOnHealth && Me.CurrentTarget != null && Me.HealthPercent <= PRSettings.Instance.DeathKnight.AntiMagicShellvalue),
                        CooldownTracker.Cast("Icebound Fortitude", ret => Me, ret => PRSettings.Instance.DeathKnight.UseIceboundFortitude && Me.CurrentTarget != null && Me.HealthPercent <= PRSettings.Instance.DeathKnight.FrostIceboundFortitudePercent),
                        CooldownTracker.Cast("Lichborne", ret => TalentManager.HasTalent(4) && Me.HealthPercent < PRSettings.Instance.DeathKnight.LichbornePercent),
                        CooldownTracker.Cast("Death Coil", ret => Me.CachedHasAura("Lichborne")),
                        CooldownTracker.Cast("Gift of the Naaru", ret => Me, ret => PRSettings.Instance.EnableGiftoftheNaaru && Me.CurrentTarget != null && Me.HealthPercent <= PRSettings.Instance.GiftNaaruHp),
                        CooldownTracker.Cast("Raise Dead", ret => Me, ret => PRSettings.Instance.DeathKnight.UsePetSacrifice && Me.HealthPercent <= PRSettings.Instance.DeathKnight.PetSacrificePercent),
                        CooldownTracker.Cast("Death Pact", ret => Me, ret => PRSettings.Instance.DeathKnight.UsePetSacrifice && Me.HealthPercent <= PRSettings.Instance.DeathKnight.PetSacrificePercent && Me.Minions.FirstOrDefault(q => (q.CreatureType == WoWCreatureType.Undead || q.CreatureType == WoWCreatureType.Totem) && !q.HasAura("Control Undead")) != null && Spell.SpellOnCooldown("Raise Dead")),
                        CooldownTracker.Cast("Death Strike", ret => Me.HasAura("Blood Presence")),
                        Item.UseBagItem("Healthstone", ret => NeedHealthstone, "Healthstone")
                        );
        }
        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(
                        Spell.PreventDoubleCast("Unholy Frenzy", 0.7, ret => Common.BestUnholyFrenzyTarget, ret => true),
                        Spell.Cast("Summon Gargoyle"),
                        CooldownTracker.Cast("Empower Rune Weapon", ret => Me, ret => PRSettings.Instance.DeathKnight.EmpowerOnCooldown && EmpowerRuneWeapon),
                        CooldownTracker.Cast("Empower Rune Weapon", ret => Me, ret => PRSettings.Instance.DeathKnight.EmpowerOnBoss && EmpowerRuneWeapon),
                        new Decorator(ret => PRSettings.Instance.UsePotion && Me.HasAnyAura("Bloodlust", "Heroism", "Time Warp"), Item.UseItem(76095)));
        }

        private static Composite HandleCommon()
        {
            return new PrioritySelector(
                        Spell.PreventDoubleCast("Raise Dead", 0.7, ret => Me, ret => Me.CurrentTarget != null && !Me.GotAlivePet)
            );
        }

        //SpellID :: 47541 == DeathCoil
        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
               Spell.PreventDoubleCast("Outbreak", 1, ret => Me.CurrentTarget!=null && DoTTracker.NeedRefresh(Me.CurrentTarget, SpellBook.BloodPlague)),
               Spell.PreventDoubleCast("Plague Strike", 0.7, ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Spell.SpellOnCooldown("Outbreak") && DoTTracker.NeedRefresh(Me.CurrentTarget, SpellBook.BloodPlague)),
               Spell.Cast("Unholy Blight", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && T.HasTalent(3) && NeedUnholyBlight),
               Spell.Cast("Outbreak", ret => NeedUnholyBlight && UnholyBlightCheck),
               Spell.Cast("Soul Reaper", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.HealthPercent <= PRSettings.Instance.DeathKnight.SoulReaperHP),
               Spell.PreventDoubleCast("Plague Strike", 0.7, ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && NeedUnholyBlight && UnholyBlightCheck),
               Spell.Cast("Dark Transformation"),
               Spell.Cast(47541, ret => Me.CurrentRunicPower > 90),
               Spell.CastOnGround("Death and Decay", ret => Me.CurrentTarget.Location, ret => Common.UnholyRuneSlotsActive >= 2),
               Spell.Cast("Scourge Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive >= 2),
               Spell.Cast("Festering Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.BloodRuneSlotsActive >= 2 && Common.FrostRuneSlotsActive >= 2),
               Spell.CastOnGround("Death and Decay", ret => Me.CurrentTarget.Location, ret => Common.UnholyRuneSlotsActive > 0),
               Spell.Cast(47541, ret => Me.CurrentTarget != null && (SuddenDoomProc || (TimmyDoesntHaveBuff && Common.UnholyRuneSlotsActive <= 1))),
               Spell.Cast("Scourge Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange),
               Spell.Cast("Festering Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange),
               Spell.Cast("Horn of Winter"),
               Spell.Cast(47541, ret => Me.CurrentTarget != null && (TimmyDoesntHaveBuff || (GargoyleCooldown8seconds && TimmyBuffRemains8seconds))),
               Spell.PreventDoubleCast("Blood Tap", 0.5, ret => CanBloodTap && BloodTapStacks5)
                );
        }

        private static Composite HandleSingleTargetPvP()
        {
            return new PrioritySelector(
                new Decorator(ret => HotKeyManager.IsSpecialKey, new PrioritySelector(CooldownTracker.Cast("Remorseless Winter", ret => Me, ret => true))),
                CooldownTracker.Cast("Death Grip", ret => PRSettings.Instance.DeathKnight.UseDeathGrip && !Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.DistanceSqr <= 30 * 30 && !Me.CurrentTarget.HasAura("Chains of Ice")),
                CooldownTracker.Cast("Necrotic Strike", ret => Me.CurrentTarget != null && !Me.CurrentTarget.HasAura("Necrotic Strike") && Me.CurrentTarget.IsWithinMeleeRange),
               Spell.PreventDoubleCast("Outbreak", 1, ret => Me.CurrentTarget != null && DoTTracker.NeedRefresh(Me.CurrentTarget, SpellBook.BloodPlague)),
               Spell.PreventDoubleCast("Plague Strike", 0.7, ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Spell.SpellOnCooldown("Outbreak") && DoTTracker.NeedRefresh(Me.CurrentTarget, SpellBook.BloodPlague)),
               Spell.Cast("Unholy Blight", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && T.HasTalent(3) && NeedUnholyBlight),
               Spell.Cast("Outbreak", ret => NeedUnholyBlight && UnholyBlightCheck),
               Spell.Cast("Soul Reaper", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.HealthPercent <= PRSettings.Instance.DeathKnight.SoulReaperHP),
               Spell.PreventDoubleCast("Plague Strike", 0.7, ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && NeedUnholyBlight && UnholyBlightCheck),
               Spell.Cast("Dark Transformation"),
               Spell.Cast(47541, ret => Me.CurrentRunicPower > 90),
               Spell.CastOnGround("Death and Decay", ret => Me.CurrentTarget.Location, ret => Me.CurrentTarget!=null && Common.UnholyRuneSlotsActive >= 2),
               Spell.Cast("Scourge Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive >= 2),
               Spell.Cast("Festering Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.BloodRuneSlotsActive >= 2 && Common.FrostRuneSlotsActive >= 2),
               Spell.CastOnGround("Death and Decay", ret => Me.CurrentTarget.Location, ret => Common.UnholyRuneSlotsActive > 0),
               Spell.Cast(47541, ret => Me.CurrentTarget != null && (SuddenDoomProc || (TimmyDoesntHaveBuff && Common.UnholyRuneSlotsActive <= 1))),
               Spell.Cast("Scourge Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange),
               Spell.Cast("Festering Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange),
               CooldownTracker.Cast("Necrotic Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange),
               Spell.Cast("Horn of Winter"),
               Spell.Cast(47541, ret => Me.CurrentTarget != null && (TimmyDoesntHaveBuff || (GargoyleCooldown8seconds && TimmyBuffRemains8seconds))),
               Spell.PreventDoubleCast("Blood Tap", 0.5, ret => CanBloodTap && BloodTapStacks5)
               );

            //CooldownTracker.Cast("Rocket Barrage"));
        }

        private static Composite PvPBehaviour
        {
            get
            {
                return new PrioritySelector(
                            StayAlive(),        // Keep our shit alive
                            AssistPartner(),    // Assist our partner with any spells/cooldowns we can provide.
                            InteruptEnemy(),    // Stop harmful spells
                            ControlEnemy(),     // control the enemy with our CC's
                            Burst(),            // Time to bring the rain..lets dance MoFo
                            Damage());          // nothing else..lets put the smackdown on these bitches.
            }
        }

        private static Composite StayAlive()
        {
            return HandleDefensiveCooldownsPvP();
        }
        private static Composite AssistPartner()
        {
            return null;
        }

        private static Composite InteruptEnemy()
        {
            return Spell.InterruptSpellCasts(on => PvP.GetPvPInteruptTarget(PvPInteruptMode.CurrentTarget, PvP.Orderby("Health")));
        }

        private static Composite ControlEnemy()
        {
            return PvP.CrowdControlSpellCasts(on => PvP.GetPvPCCTarget(PvPCCMode.CurrentTarget, PvP.Orderby("Health")));
        }

        private static Composite Burst()
        {
            return new Decorator(ret => HotKeyManager.IsCooldown && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange, HandleOffensiveCooldowns());
        }

        private static Composite Damage()
        {
            return new PrioritySelector(
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                        new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.DeathKnight.AoECount, HandleAoeCombatPvP()),
                                        new Decorator(ret => Common.IsWieldingTwoHandedWeapon, HandleSingleTargetPvP()))),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                        new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombatPvP()),
                                        new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTargetPvP()))));
        }


        private static Composite HandleAoeCombatPvP()
        {
            return new PrioritySelector(
                     Common.HandleTierSix(),
                     Spell.Cast("Unholy Blight", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && NeedBothDisUpAoE),
                     CooldownTracker.Cast("Necrotic Strike", ret => !Me.CurrentTarget.HasAura("Necrotic Strike")),
                     Spell.PreventDoubleCast("Plague Strike", 0.7, ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && NeedBothDisUpAoE && UnholyBlightCheck),
                     Spell.PreventDoubleCast("Pestilence", 2, ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && AoeBPCheck && UnholyBlightCheck && OutBreakCooldown),
                     Spell.Cast("Summon Gargoyle", ret => PRSettings.Instance.DeathKnight.UseGargoyleAoE),
                     Spell.Cast("Dark Transformation"),
                     Spell.Cast("Blood Boil", ret => Common.BloodRuneSlotsActive == 2 || Common.DeathRuneSlotsActive == 2),
                     Spell.CastOnGround("Death and Decay", ret => Me.CurrentTarget.Location, ret => Common.UnholyRuneSlotsActive > 0),
                     Spell.Cast("Soul Reaper", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive > 1 && Me.CurrentTarget.HealthPercent <= PRSettings.Instance.DeathKnight.SoulReaperHP),
                     Spell.Cast("Scourge Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive > 1),
                     Spell.PreventDoubleCast(47541, 0.5, ret => Me.CurrentTarget != null && (Me.CurrentRunicPower > 90 || Me.HasAura("Sudden Doom") || (!Me.Pet.HasAura("Dark Transformation") && Common.UnholyRuneSlotsActive <= 1))),
                     Spell.Cast("Blood Boil", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange),
                     Spell.Cast("Icy Touch"),
                     Spell.Cast("Soul Reaper", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive > 0 && Me.CurrentTarget.HealthPercent <= PRSettings.Instance.DeathKnight.SoulReaperHP),
                     Spell.Cast("Scourge Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive > 0),
                     Spell.PreventDoubleCast(47541, 0.5, ret => Me.CurrentTarget != null && Me.CurrentRunicPower > 20),
                     Spell.Cast("Horn of Winter")
                     );
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                     Common.HandleTierSix(),
                     Spell.Cast("Unholy Blight", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && NeedBothDisUpAoE),
                     Spell.PreventDoubleCast("Plague Strike", 0.7, ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && NeedBothDisUpAoE && UnholyBlightCheck), 
                     Spell.PreventDoubleCast("Pestilence", 2, ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && AoeBPCheck && UnholyBlightCheck && OutBreakCooldown),
                     Spell.Cast("Summon Gargoyle", ret => PRSettings.Instance.DeathKnight.UseGargoyleAoE),
                     Spell.Cast("Dark Transformation"),
                     Spell.Cast("Blood Boil", ret => Common.BloodRuneSlotsActive == 2 || Common.DeathRuneSlotsActive == 2),
                     Spell.CastOnGround("Death and Decay", ret => Me.CurrentTarget.Location, ret => Common.UnholyRuneSlotsActive > 0),
                     Spell.Cast("Soul Reaper", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive >1 && Me.CurrentTarget.HealthPercent <= PRSettings.Instance.DeathKnight.SoulReaperHP),
                     Spell.Cast("Scourge Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive > 1),
                     Spell.PreventDoubleCast(47541, 0.5, ret => Me.CurrentTarget != null && (Me.CurrentRunicPower > 90 || Me.HasAura("Sudden Doom") || (!Me.Pet.HasAura("Dark Transformation") && Common.UnholyRuneSlotsActive <= 1))),
                     Spell.Cast("Blood Boil", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange),
                     Spell.Cast("Icy Touch"),
                     Spell.Cast("Soul Reaper", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive > 0 && Me.CurrentTarget.HealthPercent <= PRSettings.Instance.DeathKnight.SoulReaperHP),
                     Spell.Cast("Scourge Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Common.UnholyRuneSlotsActive > 0),
                     Spell.PreventDoubleCast(47541, 0.5, ret => Me.CurrentTarget != null && Me.CurrentRunicPower > 20),
                     Spell.Cast("Horn of Winter")
                     );
        }

        #region Misc Composites

        private static WoWUnit CurTarget
        {
            get { return Me.CurrentTarget; }
        }

        #endregion Misc Composites

        #region booleans
        //BloodTapStuffHere
        private static bool CanBloodTap
        {
            get
            {
                return T.HasTalent(13) && (Lua.GetRuneCooldown(1) <= 1 || Lua.GetRuneCooldown(2) <= 1 || Lua.GetRuneCooldown(3) <= 1 ||
                    Lua.GetRuneCooldown(4) <= 1 || Lua.GetRuneCooldown(5) <= 1 || Lua.GetRuneCooldown(6) <= 1);
            }
        }
        private static bool BloodTapStacks10 { get { return Me.HasAura("Blood Charge", 10); } }
        private static bool BloodTapStacks5 { get { return Me.HasAura("Blood Charge", 5); } }
        private static bool BloodTapStacks8 { get { return Me.HasAura("Blood Charge", 8); } }
        // Diseases
        private static bool NeedUnholyBlight { get { return Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && (!Me.CurrentTarget.HasAura(55095, 0, true, 1000)) || !Me.CurrentTarget.HasAura(55078, 0, true, 1000); } }
        private static bool NeedEitherDis { get { return Me.CurrentTarget != null && !Me.CurrentTarget.HasAura(55078, 0, true, 3000); } }
        private static bool NeedFrostFever { get { return Me.CurrentTarget != null && !Me.CurrentTarget.HasAura("Frost Fever", 0); } }
        private static bool NeedBloodPlague { get { return Me.CurrentTarget != null && !Me.CurrentTarget.HasAura("Blood Plague", 0); } }
        private static bool NeedBothDisUp { get { return Me.CurrentTarget != null && (NeedFrostFever || NeedBloodPlague); } }
        private static bool NeedBothDisUpAoE { get { return Me.CurrentTarget != null && Spell.GetMyAuraTimeLeft("Blood Plague", Me.CurrentTarget) < 2; } }
        internal static bool AoeBPCheck { get { return Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 10).Count(x => !x.HasMyAura("Blood Plague")) >= 3; } }
        //RunicCorruptionBuffCheck
        private static bool RunicCorruptionDown { get { return !Me.HasAura("Runic Corruption", 0); } }

        //HowlingBlastProc and ObliterateProc
        private const int KillingMachine = 51124;
        private static bool HowlingBlastProc { get { return Me.HasAura("Freezing Fog"); } }
        private static bool ObliterateProc { get { return Me.HasAura(51124); } }
        private static bool EmpowerRuneWeapon { get { return Me.CurrentTarget != null && Me.IsWithinMeleeRange && Me.CurrentTarget.IsBoss() && Spell.GetSpellCooldown("Festering Strike").TotalMilliseconds > 1200 && Spell.GetSpellCooldown("Scourge Strike").TotalMilliseconds > 1200 && Me.CurrentRunicPower< 32; } }

        //RunesCheck
        private static bool ObliterateRunes6 { get { return Common.DeathRuneSlotsActive + Common.FrostRuneSlotsActive + Common.UnholyRuneSlotsActive == 6; } }
        private static bool FrostRunes0 { get { return Common.FrostRuneSlotsActive == 0; } }

        //DeathAndDecay
        private static bool DeathAndDecayComingOffCooldown { get { return Spell.GetSpellCooldown("Death and Decay").TotalMilliseconds > 0; } }

        //GargoyleCooldown
        private static bool GargoyleCooldown8seconds { get { return Spell.GetSpellCooldown("Summon Gargoyle").TotalMilliseconds > 8000; } }

        //DeathCoil
        private static bool SuddenDoomProc { get { return Me.HasAura(81340, 0, false); } }

        //TimmyChecks
        private static bool ShadowInfusionStack5 { get { return Me.HasAura("Shadow Infusion", 5); } }
        private static bool TimmyDoesntHaveBuff { get { return Me.Pet!=null && !Me.Pet.HasAura("Dark Transformation", 0); } }
        private static bool TimmyBuffRemains8seconds { get { return Me.Pet != null && !Me.Pet.HasAura("Dark Transformation", 0, false, 8000); } }


        //OutbreakCooldownCheck
        private static bool OutBreakCooldown { get { return Spell.SpellOnCooldown("Outbreak"); } }

        //UnholyBlight Check (For Aura)
        private static bool UnholyBlightCheck { get { return !Me.HasAura(115989); } }

        //SoulReaper Checks
        private static bool SoulReaperNon4SetBonusHPCheck { get { return Me.CurrentTarget != null && Me.CurrentTarget.HealthPercent <= 36; } }
        private static bool SoulReaperIsComplete { get { return Spell.GetSpellCooldown("Soul Reaper").TotalMilliseconds > 0; } }

        // Healing shit
        private static bool NeedHealthstone { get { return PRSettings.Instance.EnableSelfHealing && Me.HealthPercent < PRSettings.Instance.DeathKnight.HealthstonePercent; } }


        private static bool DeathCoilRP { get { return Me.CurrentRunicPower > 85; } }

        //Cooldownchecks
        private static bool OutbreakCooldown { get { return CooldownTracker.SpellOnCooldown(77575); } }

        //Handle Defensive

        private static bool NeedRaiseDead { get { return UsePetSacrifice && Me.HealthPercent < PetSacrificePercent && Me.Pet==null; } }

        #endregion booleans

        #region Settings

        private static DeathKnightSettings DeathKnightSettings { get { return PRSettings.Instance.DeathKnight; } }

        private static int PetSacrificePercent { get { return DeathKnightSettings.PetSacrificePercent; } }

        private static bool UsePetSacrifice { get { return DeathKnightSettings.UsePetSacrifice; } }

        #endregion Settings

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Revision: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.DeathKnightUnholy; }
        }

        internal override string Help
        {
            get
            {
                return
                    @"
                     -----------------------------------------------
                     Special Key: Tier Six Talent in AOE-Mode

                     Recommended Spec: http://www.wowhead.com/talent#k^T|

                     -----------------------------------------------
                     ";
            }
        }

        public override string Name
        {
            get { return "Unholy DeahtKnight"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()), //dont continue down the tree if paused or rotation selection enabled.
                    new Decorator(ret => HotKeyManager.IsRotationKey, RotationSelection),
                    new Decorator(ret => HotKeyManager.IsPaused || HotKeyManager.IsRotationKey, new ActionAlwaysSucceed()), //dont continue down the tree if paused or rotation selection enabled.
                      EncounterSpecific.HandleActionBarInterrupts(),
                        Racials.UseRacials(),
                        HandleCommon(),
                        HandleDefensiveCooldowns(),                         // Stay Alive!
                        Item.UseBagItem("Healthstone", ret => NeedHealthstone, "Healthstone"),
                        Item.HandleItems(),
                        Spell.InterruptSpellCasts(ret => Me.CurrentTarget), // Interupt incomming damage!
                        new Decorator(ret => !Spell.IsGlobalCooldown() && HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                      new PrioritySelector(
                                          new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),
                                          new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.NearbyAttackableUnits(Me.Location, 10).Count() > DeathKnightSettings.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                          HandleSingleTarget())),
                        new Decorator(ret => !Spell.IsGlobalCooldown() && HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                      new PrioritySelector(
                                          new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                          new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.NearbyAttackableUnits(Me.Location, 10).Count() > DeathKnightSettings.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                          HandleSingleTarget())),
                        new Decorator(ret => !Spell.IsGlobalCooldown() && HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                      new PrioritySelector(
                                          new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                          new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                          HandleSingleTarget())));
            }
        }


        public override Composite PVPRotation
        {
            get
            {
                return new PrioritySelector(
                            new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                            PvPBehaviour);
            }
        }

        private Composite RotationSelection
        {
            get
            {
                // You can add as many different composite rotations in here, it will toggle from PVE (default) to the composite you select within the GUI provided the hotkey is enabled. -- wulf
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsRotationKey,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.DeathKnight.RotationSelections == DeathKnightSettings.RotationSelection.PvP, PVPRotation)))
                    );
            }
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
            if (!DoTTracker.Initialized) DoTTracker.Initialize();
        }

        #endregion Overrides of RotationBase
    }
}