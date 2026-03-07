#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/DeathKnight/Frost.cs $
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
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.DeathKnight
{
    [UsedImplicitly]
    internal class Frost : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
                        CooldownTracker.Cast("Death's Advance", ret => Me, ret => PRSettings.Instance.DeathKnight.EnableDeathsAdvance && (Me.CachedHasAura(116784) || Me.CachedHasAura(116417))),
                        CooldownTracker.Cast("Anti-Magic Shell", ret => Me, ret => PRSettings.Instance.DeathKnight.UseAntiMagicShell && Me.CurrentTarget != null && Me.CurrentTarget.IsCasting),
                        CooldownTracker.Cast("Anti-Magic Shell", ret => Me, ret => PRSettings.Instance.DeathKnight.AntiMagicShellOnHealth && Me.CurrentTarget != null && Me.HealthPercent <= PRSettings.Instance.DeathKnight.AntiMagicShellvalue),
                        CooldownTracker.Cast("Icebound Fortitude", ret => Me, ret => PRSettings.Instance.DeathKnight.UseIceboundFortitude && Me.CurrentTarget != null && Me.HealthPercent <= PRSettings.Instance.DeathKnight.FrostIceboundFortitudePercent),
                        CooldownTracker.Cast("Gift of the Naaru", ret => Me, ret => PRSettings.Instance.EnableGiftoftheNaaru && Me.CurrentTarget != null && Me.HealthPercent <= PRSettings.Instance.GiftNaaruHp),
                        CooldownTracker.Cast("Raise Dead", ret => Me, ret => PRSettings.Instance.DeathKnight.UsePetSacrifice && Me.HealthPercent <= PRSettings.Instance.DeathKnight.PetSacrificePercent),
                        CooldownTracker.Cast("Death Pact", ret => Me, ret => PRSettings.Instance.DeathKnight.UsePetSacrifice && Me.HealthPercent <= PRSettings.Instance.DeathKnight.PetSacrificePercent && Me.Minions.FirstOrDefault(q => (q.CreatureType == WoWCreatureType.Undead || q.CreatureType == WoWCreatureType.Totem) && !q.HasAura("Control Undead")) != null && Spell.SpellOnCooldown("Raise Dead")),
                        CooldownTracker.Cast("Death Strike", ret => Me.CachedHasAura(101568)),
                        Item.UseBagItem("Healthstone", ret => NeedHealthstone, "Healthstone"));
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
                        Item.UseBagItem("Healthstone", ret => NeedHealthstone, "Healthstone"));
        }

        private static Composite HandleOffensiveCooldownsTwoHander()
        {
            return new PrioritySelector(
                         Spell.Cast("Pillar of Frost", ret => Me, ret => PRSettings.Instance.DeathKnight.UsePillarOfFrostOnCooldown && Me.CurrentTarget != null),
                         new Decorator(ret => Me.HasAura("Pillar of Frost"), Item.UseEngineerGloves()),
                          Spell.Cast("Pillar of Frost", ret => Me, ret => PRSettings.Instance.DeathKnight.UsePillarOfFrostOnBoss && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss()),
                          Spell.Cast("Raise Dead", ret => Me, ret => PRSettings.Instance.DeathKnight.RaiseDeadPillar && Me.CurrentTarget != null),
                          Spell.Cast("Empower Rune Weapon", ret => Me, ret => PRSettings.Instance.DeathKnight.EmpowerOnCooldown && Me.CurrentTarget != null && Common.CanEmpowerRuneWeapon),
                          Spell.Cast("Empower Rune Weapon", ret => Me, ret => PRSettings.Instance.DeathKnight.EmpowerOnBoss && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss() && Common.CanEmpowerRuneWeapon));
        }

        private static Composite HandleOffensiveCooldownsDW()
        {
            return new PrioritySelector(
                         CooldownTracker.Cast("Pillar of Frost", ret => Me, ret => PRSettings.Instance.DeathKnight.UsePillarOfFrostOnCooldown && Me.CurrentTarget != null),
                         new Decorator(ret => Me.HasAura("Pillar of Frost"), Item.UseEngineerGloves()),
                         Spell.Cast("Blood Fury", ret => Me.HasAura("Pillar of Frost")),
                         Spell.Cast("Berserking", ret => Me.HasAura("Pillar of Frost")),
                         CooldownTracker.Cast("Pillar of Frost", ret => Me, ret => PRSettings.Instance.DeathKnight.UsePillarOfFrostOnBoss && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss()),
                         CooldownTracker.Cast("Raise Dead", ret => Me, ret => PRSettings.Instance.DeathKnight.RaiseDeadBoth && Me.CurrentTarget != null && Me.CachedHasAura("Pillar of Frost") && Me.CachedHasAura("Unholy Strength")),
                         CooldownTracker.Cast("Raise Dead", ret => Me, ret => PRSettings.Instance.DeathKnight.RaiseDeadPillar && Me.CurrentTarget != null && Me.CachedHasAura("Pillar of Frost")),
                         CooldownTracker.Cast("Raise Dead", ret => Me, ret => PRSettings.Instance.DeathKnight.RaiseDeadUnholy && Me.CurrentTarget != null && Me.CachedHasAura("Unholy Strength")),
                         CooldownTracker.Cast("Empower Rune Weapon", ret => Me, ret => PRSettings.Instance.DeathKnight.EmpowerOnCooldown && Me.CurrentTarget != null && Common.CanEmpowerRuneWeaponDW),
                         CooldownTracker.Cast("Empower Rune Weapon", ret => Me, ret => PRSettings.Instance.DeathKnight.EmpowerOnBoss && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss() && Common.CanEmpowerRuneWeaponDW),
                         new Decorator(ret => PRSettings.Instance.UsePotion && Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Time Warp") || (Me.CurrentTarget.HealthPercent < 20 && Me.HasAura("Pillar of Frost")), Item.UseItem(76095)));
        }

        internal static Composite TwoHandedCombat()
        {
            return new PrioritySelector(
                        new Decorator(ret => HotKeyManager.IsSpecialKey, new PrioritySelector(CooldownTracker.Cast("Remorseless Winter", ret => Me, ret => true))),
                        Spell.Cast("Unholy Blight", ret => PRSettings.Instance.DeathKnight.EnableUnholyBlight && Me.CurrentTarget!=null && Me.CurrentTarget.IsWithinMeleeRange && UnholyBlightCheck && NeedEitherDisRefresh && NeedEitherDis),
                    //    Spell.Cast("Plague Leech", ret => TalentManager.HasTalent(2) && (!PLBP || !PLFF)),
                        CooldownTracker.Cast("Outbreak", ret => (UnholyBlightCheck || !TalentManager.HasTalent(3)) && NeedEitherDis),
                      Spell.Cast("Soul Reaper", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.HealthPercent <= PRSettings.Instance.DeathKnight.SoulReaperHP),
                        Spell.PreventDoubleCast("Blood Tap", ThrottleTime, ret => TalentManager.HasTalent(13) && (Me.CurrentTarget.HealthPercent < 36 && !CurTarget.HasRaidDebuff() && !Common.Has4PcTier15) || ((Me.CurrentTarget.HealthPercent < 46 && !CurTarget.HasRaidDebuff() && Common.Has4PcTier15))),
                        Spell.Cast("Howling Blast", ret => NeedFrostFever && OutBreakCooldown && UnholyBlightCheck),
                        Spell.PreventDoubleCast("Plague Strike", 0.7, ret => UnholyBlightCheck && OutBreakCooldown && NeedBloodPlague && !NeedFrostFever),
                        Spell.Cast("Howling Blast", ret => HowlingBlastProc),
                        Spell.Cast("Obliterate", ret => ObliterateProc && !NeedBloodPlague && !NeedFrostFever),
                        Spell.PreventDoubleCast("Blood Tap", ThrottleTime, ret => TalentManager.HasTalent(13) && KMProcBloodTap),
                        Spell.PreventDoubleCast("Blood Tap", ThrottleTime, ret => TalentManager.HasTalent(13) && BloodTapStackOver10 && RP76),
                        Spell.Cast("Frost Strike", ret => FSRP76),
                        Spell.Cast("Obliterate", ret => ObliterateRunes6 && !NeedBloodPlague && !NeedFrostFever),
                      //  Spell.Cast("Plague Leech", ret => TalentManager.HasTalent(2) && (!PLBP3 || !PLFF3)),
                        Spell.Cast("Unholy Blight", ret => PRSettings.Instance.DeathKnight.EnableUnholyBlight && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && UnholyBlightCheck && (!PLBP3 || !PLFF3)),
                        Spell.Cast("Frost Strike", ret => TalentManager.HasTalent(14) && FrostRunes0),
                        Spell.Cast("Frost Strike", ret => TalentManager.HasTalent(13) && BloodTapChargesUnder10),
                        Spell.Cast("Horn of Winter",ret=> !Me.HasAura("Horn of Winter")),
                        Spell.Cast("Frost Strike", ret => TalentManager.HasTalent(15) && RunicCorruptionDown),
                        Spell.Cast("Obliterate",ret=> !NeedBloodPlague && !NeedFrostFever),
                        Spell.PreventDoubleCast("Blood Tap", ThrottleTime, ret => TalentManager.HasTalent(13) && BloodTapStackOver10 && RP20),
                        Spell.Cast("Frost Strike"),
                        Spell.Cast("Horn of Winter", ret => true)
                        );
        }

        private static Composite TwoHandedCombatPvP()
        {
            return new PrioritySelector(
                        new Decorator(ret => HotKeyManager.IsSpecialKey, new PrioritySelector(CooldownTracker.Cast("Remorseless Winter", ret => Me, ret => true))),
                        CooldownTracker.Cast("Death Grip", ret => PRSettings.Instance.DeathKnight.UseDeathGrip && !Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.DistanceSqr <= 30 * 30 && !Me.CurrentTarget.HasAura("Chains of Ice")),
                        CooldownTracker.Cast("Necrotic Strike", ret => !Me.CurrentTarget.HasAura("Necrotic Strike")),
                        CooldownTracker.Cast("Unholy Blight", ret => PRSettings.Instance.DeathKnight.EnableUnholyBlight && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && UnholyBlightCheck && NeedUnholyBlight),
                        CooldownTracker.Cast("Outbreak", ret => (UnholyBlightCheck || !TalentManager.HasTalent(3)) && NeedEitherDis),
                        CooldownTracker.Cast("Plague Leech", ret => Common.DeathRuneSlotsActive < 2 && NeedPlagueLeech1 && Me.CurrentTarget.HasMyAura(55078)),
                        CooldownTracker.Cast("Soul Reaper", ret => CurTarget.IsWithinMeleeRange && Me.CurrentTarget.HealthPercent <= 30),
                        CooldownTracker.Cast("Plague Strike", ret => !Me.CurrentTarget.CachedHasAura("Blood Plague") && Spell.SpellOnCooldown("Outbreak")),
                        CooldownTracker.Cast("Howling Blast", ret => !Me.CurrentTarget.CachedHasAura("Frost Fever") && Spell.SpellOnCooldown("Outbreak") && !CurTarget.HasRaidDebuff()),
                        CooldownTracker.Cast("Howling Blast", ret => Me.CachedHasAura(59052) && !CurTarget.HasRaidDebuff()),
                        CooldownTracker.Cast("Obliterate", ret => Me.CurrentRunicPower <= 76),
                        CooldownTracker.Cast("Frost Strike", ret => Me.CurrentRunicPower > 76 || Spell.SpellOnCooldown(49020)),
                        CooldownTracker.Cast("Plague Leech", ret => Common.DeathRuneSlotsActive < 2 && Me.CurrentTarget.HasMyAura(55078) && !Me.CurrentTarget.CachedHasAura(55078, 0, true, 2000)),
                        CooldownTracker.Cast("Outbreak", ret => !Me.CurrentTarget.CachedHasAura(55095, 0, true, 2000) || !Me.CurrentTarget.CachedHasAura(55078, 0, true, 2000)),
                        CooldownTracker.Cast("Frost Strike", ret => TalentManager.HasTalent(14) && Common.FrostRuneSlotsActive == 0 && Spell.SpellOnCooldown(49020)),
                        CooldownTracker.Cast("Necrotic Strike", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange),
                        CooldownTracker.Cast("Horn of Winter", ret => !Me.HasAura("Horn of Winter")));

            //CooldownTracker.Cast("Rocket Barrage"));
        }

        internal static Composite OneHandedCombat()
        {
            return new PrioritySelector(
                //actions.single_target=blood_tap,if=talent.blood_tap.enabled&buff.blood_charge.stack>10&(runic_power>76|(runic_power>=20&buff.killing_machine.react))
                        Spell.PreventDoubleCast("Blood Tap", 0.5, ret => NeedBloodTapFirstCheck),
                        Spell.Cast("Unholy Blight", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && OutBreakCooldown && UnholyBlightCheck && NeedEitherDis),
                        Spell.Cast("Frost Strike", ret => ObliterateProc || Me.CurrentRunicPower > 88),
               //         CooldownTracker.Cast("Plague Leech", ret => NeedPlagueLeechDW && Me.CurrentTarget.CachedHasAura(55078)),
                        Spell.Cast("Outbreak", ret => UnholyBlightCheck && NeedEitherDis),
                //actions.single_target+=/blood_tap,if=talent.blood_tap.enabled&(target.health.pct-3*(target.health.pct%target.time_to_die)<=45&cooldown.soul_reaper.remains=0)
                        Spell.PreventDoubleCast("Blood Tap", 0.5, ret => NeedBloodTapSecondCheck),
                        Spell.Cast("Soul Reaper", ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.HealthPercent <= PRSettings.Instance.DeathKnight.SoulReaperHP),
                        Spell.Cast("Howling Blast", ret => UnholyBlightCheck && NeedFrostFever),
                        Spell.PreventDoubleCast("Plague Strike", 0.7, ret => UnholyBlightCheck && OutBreakCooldown && NeedBloodPlague),
                        Spell.Cast("Howling Blast", ret => HowlingBlastProc),
                        Spell.Cast("Frost Strike", ret => Me.CurrentRunicPower > 76),
                        Spell.Cast("Howling Blast", ret => Common.DeathRuneSlotsActive == 2 || Common.FrostRuneSlotsActive == 2),
                        Spell.Cast("Obliterate", ret => Common.UnholyRuneSlotsActive == 2),
                        Spell.Cast("Horn of Winter", ret => Me.CurrentRunicPower < 20 || !Me.HasAura("Horn of Winter")),
                        Spell.Cast("Obliterate", ret => Common.UnholyRuneSlotsActive == 1),
                        Spell.Cast("Howling Blast", ret => Common.DeathRuneSlotsActive > 0 || Common.FrostRuneSlotsActive > 0),
                        Spell.Cast("Frost Strike", ret => TalentManager.HasTalent(14) && Common.FrostRuneSlotsActive == 0 || Common.BloodRuneSlotsActive == 0),
                        Spell.PreventDoubleCast("Blood Tap", 0.5, ret => NeedBloodTapThirdCheck),
                        Spell.CastOnGround("Death and Decay", ret => Me.CurrentTarget.Location, ret => true),
                        Spell.Cast("Frost Strike", ret => Me.CurrentRunicPower > 40),
                        Spell.Cast("Horn of Winter", ret => true));
        }


        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                        Common.HandleTierSix(),
                        Common.HandlePestilence(),
                        Spell.Cast("Unholy Blight", ret => PRSettings.Instance.DeathKnight.EnableUnholyBlight && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && UnholyBlightCheck && NeedUnholyBlightAoE),
                        CooldownTracker.Cast("Outbreak", ret => (UnholyBlightCheck || !TalentManager.HasTalent(3)) && (NeedBloodPlague || NeedFrostFever)),
                        Spell.Cast("Howling Blast"),
                        Spell.Cast("Frost Strike", ret => Me.CurrentRunicPower > 76),
                        Spell.CastOnGround("Death and Decay", ret => CurTarget.Location, ret => Common.UnholyRuneSlotsActive == 1),
                        Spell.Cast("Plague Strike", ret => Common.UnholyRuneSlotsActive == 2),
                        Spell.Cast("Frost Strike"),
                        Spell.Cast("Horn of Winter", ret => !Me.HasAura("Horn of Winter")));
        }

        private static WoWUnit CurTarget
        {
            get { return Me.CurrentTarget; }
        }

        #region PvP

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
            return new PrioritySelector(
                    new Decorator(ret => Common.IsWieldingTwoHandedWeapon && HotKeyManager.IsCooldown && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange, HandleOffensiveCooldownsTwoHander()),
                    new Decorator(ret => !Common.IsWieldingTwoHandedWeapon && HotKeyManager.IsCooldown && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange, HandleOffensiveCooldownsDW()));
        }

        private static Composite Damage()
        {
            return new PrioritySelector(
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                        new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.DeathKnight.AoECount, HandleAoeCombat()),
                                        new Decorator(ret => Common.IsWieldingTwoHandedWeapon, TwoHandedCombatPvP()),
                                        new Decorator(ret => !Common.IsWieldingTwoHandedWeapon, OneHandedCombat()))),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                        new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                        new Decorator(ret => !HotKeyManager.IsAoe && Common.IsWieldingTwoHandedWeapon, TwoHandedCombatPvP()),
                                        new Decorator(ret => !HotKeyManager.IsAoe && !Common.IsWieldingTwoHandedWeapon, OneHandedCombat()))));
        }

        #endregion PvP

        #region Booleans

        //Blood Tap
        private static bool NeedBloodTapFirstCheck { get { return TalentManager.HasTalent(13) && Me.CachedHasAura("Blood Charge", 10) && (Me.CurrentRunicPower > 76 || (Me.HasAura(51124) && Me.CurrentRunicPower >= 20)); } }
        //Me.CurrentTarget.HealthPercent <= SG.Instance.Frost.SoulReaperHP
        private static bool NeedBloodTapSecondCheck { get { return TalentManager.HasTalent(13) && Me.CachedHasAura("Blood Charge", 5) && Me.CurrentTarget.HealthPercent <= PRSettings.Instance.DeathKnight.SoulReaperHP && !Me.CurrentTarget.HasAura(130735); } }
        //actions.single_target+=/blood_tap,if=talent.blood_tap.enabled&(target.health.pct-3*(target.health.pct%target.time_to_die)>45|buff.blood_charge.stack>=8)
        private static bool NeedBloodTapThirdCheck { get { return TalentManager.HasTalent(13) && Me.CurrentTarget != null && Me.CurrentTarget.HealthPercent >= PRSettings.Instance.DeathKnight.SoulReaperHP && Me.CachedHasAura("Blood Charge", 5) || Me.CachedHasAura("Blood Charge", 8); } }
        private static bool NeedBloodTap { get { return TalentManager.HasTalent(13) && Me.HasAura("Blood Charge", 5); } }
        private static bool SoulReaperBloodTap { get { return TalentManager.HasTalent(13) && Me.CurrentTarget != null && Me.HasAura("Blood Charge", 5); } }
        private static bool KMProcBloodTap { get { return TalentManager.HasTalent(13) && Me.HasAura("Blood Charge", 5); } }
        private static bool BloodTapRP20 { get { return TalentManager.HasTalent(13) && Me.HasAura("Blood Charge", 8); } }
        private static bool BloodTapStackOver10 { get { return TalentManager.HasTalent(13) && Me.CachedHasAura("Blood Charge", 11); } }
        private static bool BloodTapChargesUnder10 { get { return TalentManager.HasTalent(13) && Me.CachedStackCount("Blood Charge") < 10; } }
            
        //BloodTapCheckLua
        private static bool CanBloodTap { get { return TalentManager.HasTalent(13) && (Lua.GetRuneCooldown(1) <= 1 || Lua.GetRuneCooldown(2) <= 1 || Lua.GetRuneCooldown(3) <= 1 || Lua.GetRuneCooldown(4) <= 1 || Lua.GetRuneCooldown(5) <= 1 || Lua.GetRuneCooldown(6) <= 1); } }

        // Diseases
        private static bool NeedEitherDis { get { return Me.CurrentTarget != null && !Me.CurrentTarget.HasAllMyAuras(new string[]{"Frost Fexer","Blood Plague"}); } }
        private static bool NeedEitherDisRefresh { get { return !Me.CurrentTarget.CachedHasAura("Frost Fever", stacks: 0, fromMyAura: true, msLeft: 2000) || !Me.CurrentTarget.CachedHasAura("Blood Plague", stacks: 0, fromMyAura: true, msLeft: 2000); } }
        private static bool NeedFrostFever { get { return Me.CurrentTarget != null && !Me.CurrentTarget.HasMyAura("Frost Fever"); } }
        private static bool NeedBloodPlague { get { return Me.CurrentTarget != null && !Me.CurrentTarget.HasMyAura("Blood Plague"); } }
        private static bool PLBP { get { return Me.CurrentTarget != null && Me.CurrentTarget.CachedHasAura("Blood Plague", stacks: 0, fromMyAura: true, msLeft: 1000); } }
        private static bool PLFF { get { return Me.CurrentTarget != null && Me.CurrentTarget.CachedHasAura("Frost Fever", stacks: 0, fromMyAura: true, msLeft: 1000); } }
        private static bool PLBP3 { get { return Me.CurrentTarget != null && Me.CurrentTarget.CachedHasAura("Blood Plague", stacks: 0, fromMyAura: true, msLeft: 3000); } }
        private static bool PLFF3 { get { return Me.CurrentTarget != null && Me.CurrentTarget.CachedHasAura("Frost Fever", stacks: 0, fromMyAura: true, msLeft: 3000); } }
        private static bool NeedBothDisUp { get { return Me.CurrentTarget != null && (!Me.CurrentTarget.CachedHasAura("Frost Fever", fromMyAura: true) || !Me.CurrentTarget.CachedHasAura("Blood Plague", fromMyAura: true)); } }
        private static bool NeedBothDisUpAoE { get { return Me.CurrentTarget != null && (Spell.GetMyAuraTimeLeft("Blood Plague", Me.CurrentTarget) < 2 || Spell.GetMyAuraTimeLeft("Frost Fever", Me.CurrentTarget) < 2); } }
        private static bool NeedUnholyBlightAoE { get { return PRSettings.Instance.DeathKnight.EnableUnholyBlight && !CurTarget.HasRaidDebuff() && Me.IsWithinMeleeRange; } }
        private static bool AoeBPCheck { get { return Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 10).Count(x => !x.HasMyAura("Blood Plague")) >= 3; } }

        //RunicCorruptionBuffCheck
        private static bool RunicCorruptionDown { get { return !Me.CachedHasAura("Runic Corruption"); } }

        //HowlingBlastProc and ObliterateProc
        private static bool HowlingBlastProc { get { return Me.HasAura("Freezing Fog"); } }
        private static bool ObliterateProc { get { return Me.HasAura("Killing Machine"); } } 

        //RunesCheck
        private static bool ObliterateRunes6 { get { return Common.DeathRuneSlotsActive + Common.FrostRuneSlotsActive + Common.UnholyRuneSlotsActive == 6; } }
        private static bool FrostRunes0 { get { return Common.FrostRuneSlotsActive == 0; } }

        //RunicPowerChecks
        private static bool RP76 { get { return Me.CurrentRunicPower > 76; } }
        private static bool FSRP76 { get { return Me.CurrentRunicPower > 76; } }
        private static bool RP20 { get { return Me.CurrentRunicPower >= 20; } }

        //OutbreakCooldownCheck
        private static bool OutBreakCooldown { get { return CooldownTracker.SpellOnCooldown("Outbreak"); } }

        //UnholyBlight Check (For Aura)
        private static bool UnholyBlightCheck { get { return !Me.HasAura(115989); } }

        //SoulReaper Checks
        private static bool SoulReaperNon4SetBonusHPCheck { get { return Me.CurrentTarget != null && Me.CurrentTarget.HealthPercent <= 36; } }
        private static bool SoulReaperCooldownCheck { get { return CooldownTracker.SpellOnCooldown("Soul Reaper"); } }

        // Healing shit
        private static bool NeedHealthstone { get { return PRSettings.Instance.EnableSelfHealing && Me.HealthPercent < PRSettings.Instance.DeathKnight.HealthstonePercent; } }

        // Abilities
        private static bool NeedDeathStrike { get { return PRSettings.Instance.DeathKnight.EnableDarkSuccor && Me.CachedHasAura(101568) && Me.HealthPercent < PRSettings.Instance.DeathKnight.DeathStrikePercentCommon; } }
        private static bool NeedPlagueLeech1 { get { return !PRSettings.Instance.DeathKnight.EnableUnholyBlight && Common.DeathRuneSlotsActive < 2 && !Me.CurrentTarget.CachedHasAura(55078, 0, true, 2000); } }
        private static bool NeedPlagueLeech2 { get { return !PRSettings.Instance.DeathKnight.EnableUnholyBlight && !Me.CurrentTarget.CachedHasAura(55078, 0, true, 3000); } }
        private static bool NeedPlagueLeechDW { get { return !PRSettings.Instance.DeathKnight.EnableUnholyBlight && !Me.CurrentTarget.CachedHasAura(55078, 0, true, 2000) && Spell.GetSpellCooldown("Outbreak").TotalMilliseconds < 1000; } }
        private static bool NeedUnholyBlight { get { return  Me.CurrentTarget != null && (!Me.CurrentTarget.HasAura(55095, 0, true, 2000)) || !Me.CurrentTarget.HasAura(55078, 0, true, 2000); } }

        #endregion Booleans

        #region Overrides of RotationBase

        public override string Name
        {
            get { return "Awesome Frost DK"; }
        }

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
            get { return WoWSpec.DeathKnightFrost; }
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
                    Medic,
                    Item.UseBagItem("Healthstone", ret => NeedHealthstone, "Healthstone"),
                    Item.HandleItems(),
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    new Decorator(ret => !Spell.IsGlobalCooldown() && HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                        new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss() && Common.IsWieldingTwoHandedWeapon, HandleOffensiveCooldownsTwoHander()),
                                        new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss() && !Common.IsWieldingTwoHandedWeapon, HandleOffensiveCooldownsDW()),
                                        new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.DeathKnight.AoECount, HandleAoeCombat()),
                                        new Decorator(ret => Common.IsWieldingTwoHandedWeapon, TwoHandedCombat()),
                                        new Decorator(ret => !Common.IsWieldingTwoHandedWeapon, OneHandedCombat()))),
                    new Decorator(ret => !Spell.IsGlobalCooldown() && HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                        new Decorator(ret => HotKeyManager.IsCooldown && Common.IsWieldingTwoHandedWeapon && Me.CurrentTarget!=null && Me.CurrentTarget.IsWithinMeleeRange, HandleOffensiveCooldownsTwoHander()),
                                        new Decorator(ret => HotKeyManager.IsCooldown && !Common.IsWieldingTwoHandedWeapon && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange, HandleOffensiveCooldownsDW()),
                                        new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.DeathKnight.AoECount, HandleAoeCombat()),
                                        new Decorator(ret => Common.IsWieldingTwoHandedWeapon, TwoHandedCombat()),
                                        new Decorator(ret => !Common.IsWieldingTwoHandedWeapon, OneHandedCombat()))),
                    new Decorator(ret => !Spell.IsGlobalCooldown() && HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                        new Decorator(ret => HotKeyManager.IsCooldown && Common.IsWieldingTwoHandedWeapon && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange, HandleOffensiveCooldownsTwoHander()),
                                        new Decorator(ret => HotKeyManager.IsCooldown && !Common.IsWieldingTwoHandedWeapon && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange, HandleOffensiveCooldownsDW()),
                                        new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                        new Decorator(ret => !HotKeyManager.IsAoe && Common.IsWieldingTwoHandedWeapon, TwoHandedCombat()),
                                        new Decorator(ret => !HotKeyManager.IsAoe && !Common.IsWieldingTwoHandedWeapon, OneHandedCombat()))));
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

        public override Composite Medic
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => Me.HealthPercent < 100 && PRSettings.Instance.EnableSelfHealing,
                        HandleDefensiveCooldowns()));
            }
        }

        public override Composite PreCombat
        {
            get { return null; }
        }

        #endregion Overrides of RotationBase
    }
}