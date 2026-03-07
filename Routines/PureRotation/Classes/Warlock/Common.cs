#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-24 21:54:34 +0200 (Di, 24 Sep 2013) $
 * $ID$
 * $Revision: 1737 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Warlock/Common.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.Warlock
{
    [UsedImplicitly]
    internal class Common
    {
        private static WarlockSettings WarlockSettings { get { return PRSettings.Instance.Warlock; } }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static string _sacrificeAbility = "Unknown";
        
        #region Cooldowns / Common Composites
        public static Composite CommonDefensiveCooldowns()
        {
            return new PrioritySelector(
                Spell.Cast("Soulstone", on => Me, ret => Me.HealthPercent < WarlockSettings.SoulstonePercent && Targeting.GetAggroOnMeWithin(Me.Location, 40) >= 1),
                Spell.Cast("Soulshatter", on => StyxWoW.Me, ret => WarlockSettings.UseSoulshatter && !WarlockSettings.KanrethadEbonlockeFight && Me.HealthPercent < 70 && (StyxWoW.Me.CurrentTarget.ThreatInfo.RawPercent > 95 || Targeting.GetAggroOnMeWithin(StyxWoW.Me.Location, 10) >= 1) && !Spell.IsChanneling), // 08/01/2013 Millz: Moved to defensive cooldown
                //Spell.PreventDoubleCast("Command Demon", 10, on => StyxWoW.Me, ret => !WarlockSettings.KanrethadEbonlockeFight && StyxWoW.Me.HealthPercent < WarlockSettings.ShadowBulwarkPercent && TalentManager.HasTalent(15)),
                Spell.Cast("Dark Bargain", on => StyxWoW.Me, ret => StyxWoW.Me.HealthPercent < WarlockSettings.DarkBargainPercent),
                Spell.Cast("Unending Resolve", on => StyxWoW.Me, ret => StyxWoW.Me.HealthPercent < WarlockSettings.UnendingResolvePercent && !StyxWoW.Me.HasAura("Dark Bargain")),
                Spell.Cast("Mortal Coil", ret => StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.HealthPercent <= WarlockSettings.MortalCoilPercent),
                Item.UseBagItem(5512, ret => StyxWoW.Me.HealthPercent <= WarlockSettings.HealthstonePercent, "Healthstone [" + Me.HealthPercent + "% HP]"),
                Spell.Cast("Twilight Ward", on => StyxWoW.Me, ret => NeedTwilightWard),

                Spell.Cast("Blood Fear", ret => !Me.HasAura("Blood Fear") && TalentManager.HasTalent(10)),
                Spell.Cast("Dark Regeneration", ret => TalentManager.HasTalent(1) && Me.HealthPercent <= WarlockSettings.DarkRegenerationPercent),
                Spell.Cast("Howl of Terror", ret => TalentManager.HasTalent(4) && Me.HealthPercent <= WarlockSettings.HowlofTerrorPercent && Unit.CachedNearbyAttackableUnits(Me.Location, 10).Count() >= WarlockSettings.HowlofTerrorUnitCount),
                Spell.Cast("Unbound Will", ret => TalentManager.HasTalent(12) && WarlockSettings.UseUnboundWill &&
                    (Me.HasAuraWithMechanic(
                    WoWSpellMechanic.Asleep, 
                    WoWSpellMechanic.Banished, 
                    WoWSpellMechanic.Charmed, 
                    WoWSpellMechanic.Disoriented,
                    WoWSpellMechanic.Fleeing,
                    WoWSpellMechanic.Sapped,
                    WoWSpellMechanic.Polymorphed,
                    WoWSpellMechanic.Incapacitated,
                    WoWSpellMechanic.Horrified)))
                
                );
        }

        public static Composite CommonOffensiveCooldowns()
        {
            return new Decorator(ret => !SpellManager.GlobalCooldown,
                    new PrioritySelector(
                    //Item.UseBagItem("Potion of the Jade Serpent", ret => PRSettings.Instance.UsePotion && StyxWoW.Me.CurrentTarget.IsBoss() && Hasted, "Potion of the Jade Serpent"),
                    Spell.Cast("Summon Doomguard", ret => NeedToSummonDoomguard),
                    Spell.PreventDoubleCastOnGround("Summon Infernal", 2, ret => StyxWoW.Me.CurrentTarget.Location, ret => NeedToSummonInferno),
                    Spell.Cast("Lifeblood", ret => !Hasted))
                );
        }

        public static Composite HandleCommonAbilities()
        {
            return
                new PrioritySelector(
                    new Decorator(ret =>
                            !Me.GotAlivePet && (TalentManager.HasTalent(13) || TalentManager.HasTalent(14)) ||
                            !Me.GotAlivePet && TalentManager.HasTalent(15) && !Me.HasAura("Grimoire of Sacrifice"), HandlePet()),
                    HandleGrimoireOfSacrifice,
                    HandleGrimoireOfService,
                    Spell.PreventDoubleCast("Soul Link", 2, on => Me, ret => !Me.HasAura("Soul Link")),
                    HandleBurningRush,
                    new Decorator(ret => Me.FocusedUnit != null && !WarlockSettings.KanrethadEbonlockeFight, HandlePetAbility(Me.FocusedUnit)),
                    new Decorator(ret => !WarlockSettings.KanrethadEbonlockeFight, HandlePetAbility(Me.CurrentTarget))
                );
        }

        public static Composite HandleCommonPreCombat()
        {
            return new PrioritySelector(
                HandlePet(),
                HandleGrimoireOfSacrifice,
                Spell.PreventDoubleCast("Soul Link", 2, on => Me, ret => !Me.HasAura("Soul Link")),
                Spell.PreventDoubleCast("Dark Intent", 5, ret => !StyxWoW.Me.HasAura("Dark Intent")),
                Spell.Cast("Create Soulwell", ret => WarlockSettings.CastSoulwellDuringBuffing && StyxWoW.Me.BagItems.FirstOrDefault(x => x.Entry == 5512) == null),
                Spell.PreventDoubleCast("Create Healthstone", 3.5, ret => WarlockSettings.CastHealthstoneDuringBuffing && StyxWoW.Me.BagItems.FirstOrDefault(x => x.Entry == 5512) == null),
                Spell.Cast("Life Tap", ret => CanLifeTap && (StyxWoW.Me.Specialization == WoWSpec.WarlockAffliction || StyxWoW.Me.Specialization == WoWSpec.WarlockDemonology)),
                Spell.Cast("Blood Fear", ret => !Me.HasAura("Blood Fear"))
                );
        }
        #endregion

        #region Pet
        private static Composite HandlePet()
        {

            return new PrioritySelector(

                // Cancel summoning of pet if we already have one active.
                new Decorator(ret => Me.GotAlivePet &&
                       (Me.CastingSpellId == 688 ||   // Imp
                       Me.CastingSpellId == 697 ||    // Voidwalker
                       Me.CastingSpellId == 691 ||    // Felhunter
                       Me.CastingSpellId == 712 ||    // Succubus 
                       Me.CastingSpellId == 30146 ||  // Felguard

                       Me.CastingSpellId == 112866 ||    // Fel Imp
                       Me.CastingSpellId == 112867 ||    // Voidlord
                       Me.CastingSpellId == 112869 ||    // Observer
                       Me.CastingSpellId == 112868 ||  // Shivarra 
                       Me.CastingSpellId == 112870   // Wrathguard
                       ),
                       new Action(delegate
                       {
                           SpellManager.StopCasting();
                           return RunStatus.Failure;
                       })),

                // Health Funnel
                Spell.PreventDoubleCast("Health Funnel", 10.5, ret => Me.GotAlivePet && Me.Pet.HealthPercent < 50 && TalentManager.HasGlyph("Health Funnel")),

                // Handle Instant Casts
                //Spell.PreventDoubleCast("Soulburn", 2, ret => Lua.PlayerUnitPower("SPELL_POWER_SOUL_SHARDS") >= 1 && !StyxWoW.Me.GotAlivePet && TalentManager.HasTalent(15) && !StyxWoW.Me.HasAura("Grimoire of Sacrifice") && StyxWoW.Me.Specialization == WoWSpec.WarlockAffliction),
                Spell.PreventDoubleCast("Flames of Xoroth", 2, ret =>
                    !StyxWoW.Me.GotAlivePet && WarlockSettings.UseFlamesofXoroth && !StyxWoW.Me.HasAura("Grimoire of Sacrifice") && StyxWoW.Me.Specialization == WoWSpec.WarlockDestruction && Me.Combat),

                new Decorator(ret => (!Me.GotAlivePet && !TalentManager.HasTalent(15)) || (!Me.GotAlivePet && TalentManager.HasTalent(15) && !Me.HasAura("Grimoire of Sacrifice")),
                        new PrioritySelector(
                            new Decorator(ret => WarlockSettings.WarlockPet == WarlockPet.Auto, AutoPetSelection()),
                            Spell.PreventDoubleCast("Summon Imp", 6, ret => WarlockSettings.WarlockPet == WarlockPet.Imp),
                            Spell.PreventDoubleCast("Summon Voidwalker", 6, ret => WarlockSettings.WarlockPet == WarlockPet.Voidwalker),
                            Spell.PreventDoubleCast("Summon Felhunter", 6, ret => WarlockSettings.WarlockPet == WarlockPet.Felhunter),
                            Spell.PreventDoubleCast("Summon Succubus", 6, ret => WarlockSettings.WarlockPet == WarlockPet.Succubus),
                            Spell.PreventDoubleCast("Summon Felguard", 6, ret => WarlockSettings.WarlockPet == WarlockPet.Felguard)))

                );
        }

        private static Composite AutoPetSelection()
        {
            return new PrioritySelector(
                // Grimoire of Service
                Spell.PreventDoubleCast("Summon Felguard", 6, ret => !StyxWoW.Me.GotAlivePet &&
                                                       TalentManager.HasTalent(14) &&
                                                       StyxWoW.Me.Specialization == WoWSpec.WarlockDemonology),

                // Grimoire of Supremacy
                Spell.PreventDoubleCast("Summon Observer", 6, ret => !StyxWoW.Me.GotAlivePet &&
                                                       TalentManager.HasTalent(13) &&
                                                      (StyxWoW.Me.Specialization == WoWSpec.WarlockAffliction ||
                                                       StyxWoW.Me.Specialization == WoWSpec.WarlockDestruction)),
                Spell.PreventDoubleCast("Summon Wrathguard", 6, ret => !StyxWoW.Me.GotAlivePet &&
                                                       TalentManager.HasTalent(13) &&
                                                       StyxWoW.Me.Specialization == WoWSpec.WarlockDemonology),

                // Grimoire of Sacrifice
                Spell.PreventDoubleCast("Summon Imp", 6, ret => !StyxWoW.Me.GotAlivePet &&
                                                       TalentManager.HasTalent(15) && !StyxWoW.Me.HasAura("Grimoire of Sacrifice") &&
                                                      (StyxWoW.Me.Specialization == WoWSpec.WarlockAffliction ||
                                                       StyxWoW.Me.Specialization == WoWSpec.WarlockDestruction)
                                                       && WarlockSettings.KanrethadEbonlockeFight),

                Spell.PreventDoubleCast("Summon Voidwalker", 6, ret => !StyxWoW.Me.GotAlivePet &&
                                                       TalentManager.HasTalent(15) && !StyxWoW.Me.HasAura("Grimoire of Sacrifice") &&
                                                      (StyxWoW.Me.Specialization == WoWSpec.WarlockAffliction ||
                                                       StyxWoW.Me.Specialization == WoWSpec.WarlockDestruction)
                                                       && !WarlockSettings.KanrethadEbonlockeFight),
                Spell.PreventDoubleCast("Summon Felguard", 6, ret => !StyxWoW.Me.GotAlivePet && // Nobody should ever be hitting this condition !
                                                       TalentManager.HasTalent(15) && !StyxWoW.Me.HasAura("Grimoire of Sacrifice") &&
                                                       StyxWoW.Me.Specialization == WoWSpec.WarlockDemonology))
            ;
        }

        private static Composite HandlePetAbility(WoWUnit unit)
        {
            return new Decorator(ret => WarlockSettings.UsePetAbilities_Experimental && (Me.GotAlivePet && Me.Pet.Distance <= 40 || Me.HasAura("Grimoire of Sacrifice")) && unit != null,
                new PrioritySelector(
                    new Decorator(ret => Me.GotAlivePet,
                        new PrioritySelector(
                //Succubus/Shivarra: Fellash (25s CD) - Knock back all units within 5 yards.
                            Spell.PreventDoubleCastOnGround("Command Demon", 25, on => Me.Location, ret =>
                                (Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_Shivarra || Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_Succubus)
                                && unit.IsWithinMeleeRange),

                            //Felhunter/Observer:  (24s CD) - Interupt/Silence.
                            Spell.PreventDoubleCast("Command Demon", 24, on => unit, ret =>
                                Me.Pet!=null && (Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_Observer || Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_Felhunter)
                                && (unit.IsCasting || unit.IsChanneling) && unit.CanInterruptCurrentSpellCast),

                            //Voidlord: Disarm (1min CD) - Disarm target for 8 seconds.
                            Spell.PreventDoubleCast("Command Demon", 60, on => unit, ret =>
                                 Me.Pet != null && (Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_Voidlord || Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_Voidwalker)
                                 && !unit.Disarmed
                                 && unit.IsWithinMeleeRange
                                 && (!unit.IsPlayer || unit.IsPlayer && (unit.Class == WoWClass.DeathKnight
                                    || unit.Class == WoWClass.Hunter
                                    || unit.Class == WoWClass.Monk
                                    || unit.Class == WoWClass.Paladin
                                    || unit.Class == WoWClass.Warrior
                                    || unit.Class == WoWClass.Shaman
                                    || unit.Class == WoWClass.Rogue))),

                            //Felguard/Wrathguard: Wrathstorm (45s CD) - Attack all units in 8 yards.
                            Spell.PreventDoubleCast("Command Demon", 45, ret =>
                                Me.Pet != null && (Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_Felguard || Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_Wrathguard)
                                && Unit.CachedNearbyAttackableUnits(Me.Pet.Location, 8).Any()),

                            //Fel Imp: Cauterize Master (30s CD) - Small damage then heals 12% of HP over 12 seconds.
                            Spell.PreventDoubleCast("Command Demon", 30, ret =>
                                Me.Pet != null && Me.Pet.CreatureFamilyInfo.Id == SpellBook.PetId_FelImp
                                && Me.HealthPercent <= 88)
                            )),

                    new Decorator(ret => Me.HasAura("Grimoire of Sacrifice") /*&& !Spell.SpellOnCooldown(SpellBook.CommandDemon)*/,
                        new PrioritySelector(

                            new Action(delegate
                                {
                                    _sacrificeAbility = GetSacrificeAbility();
                                    /*
                                    Logger.FailLog("Ability:[{0}] - Icon:[{1}] - MeleeRange[{2}] - Facing[{3}] - Cond[{4}]", 
                                        _sacrificeAbility, 
                                        Helpers.Lua.GetSpellIconText(SpellBook.CommandDemon), 
                                        unit.IsWithinMeleeRange, 
                                        Me.IsSafelyFacing(unit, 120f),
                                        Unit.CachedNearbyAttackableUnits(Me.Location, 5).Any()); 
                                    */
                                    return RunStatus.Failure;
                                }),

                            //Imp: Singe Magic (10s CD) - Removes 1 harmful effect from target.
                            new Decorator(ret => _sacrificeAbility == "Singe Magic",
                                Spell.PreventDoubleCast("Command Demon", 10, on => Me, ret => Me.GetAllAuras().FirstOrDefault(a => a.IsHarmful && a.Duration > 0 && a.Spell.DispelType == WoWDispelType.Magic) != null)),

                            //Voidwalker: Shadow Bulwark (2min CD) - 30% HP for 20 seconds.
                            new Decorator(ret => _sacrificeAbility == "Shadow Bulwark",
                                Spell.PreventDoubleCast("Command Demon", 120, ret => Me.HealthPercent <= WarlockSettings.ShadowBulwarkPercent)),

                            //Whiplash (25s CD) - Knock back all enemies in 5 yards.
                            new Decorator(ret => _sacrificeAbility == "Whiplash",
                                Spell.PreventDoubleCastOnGround("Command Demon", 25, loc => Me.Location, ret => Unit.CachedNearbyAttackableUnits(Me.Location, 5).Any())),

                            //Spell Lock (24s CD) - Interupt/Silence
                            new Decorator(ret => _sacrificeAbility == "Spell Lock",
                                Spell.PreventDoubleCast("Command Demon", 24, on => unit, ret =>
                                    unit.CanInterruptCurrentSpellCast && (unit.IsCasting || unit.IsChanneling))),

                            //Pursuit (15s CD - 8-25yd Range) - Charge enemy 
                            new Decorator(ret => _sacrificeAbility == "Pursuit", Spell.PreventDoubleCast("Command Demon", 15, on => unit,
                                ret => unit.Distance >= 15 && unit.Distance <= 25 && Me.MovementInfo.MovingForward && Me.IsSafelyFacing(unit, 120f))))

                            ))

                );
        }

        private static Composite HandleGrimoireOfSacrifice
        {
            get
            {
                return new PrioritySelector(
                    Spell.Cast("Grimoire of Sacrifice", ret => Me,
                               ret => WarlockSettings.UseGrimoireOfSacrifice &&
                               Me.GotAlivePet && TalentManager.HasTalent(15) &&
                               !Me.HasAura("Grimoire of Sacrifice")
                    ));
            }
        }

        private static Composite HandleGrimoireOfService
        {
            get
            {
                return
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto && Me.CurrentTarget.IsBoss()
                                         || HotKeyManager.IsCooldown,
                                  new PrioritySelector(
                                      Spell.Cast("Grimoire: Felhunter", ret =>
                                                                        TalentManager.HasTalent(14) &&
                                                                        (Me.Specialization == WoWSpec.WarlockAffliction ||
                                                                         Me.Specialization == WoWSpec.WarlockDestruction)),

                                      Spell.Cast("Grimoire: Felguard", ret =>
                                                                       TalentManager.HasTalent(14) &&
                                                                       Me.Specialization == WoWSpec.WarlockDemonology)
                                      ));
            }
        }
        #endregion Pet

        private static Composite HandleBurningRush
        {
            get
            {
                return new PrioritySelector(
                    Spell.Cast("Burning Rush", ret =>
                                               TalentManager.HasTalent(11) &&
                                               StyxWoW.Me.Combat &&
                                              !StyxWoW.Me.HasAura("Burning Rush") &&
                                               StyxWoW.Me.HealthPercent >= WarlockSettings.BurningRushPercentage &&
                                               WarlockSettings.UseBurningRush &&
                                               StyxWoW.Me.IsMoving(200)

                    // Cast if we're in combat, dont have buff already, want to use it, HP is good, and we're moving
                        ),

                    new Decorator(ret => StyxWoW.Me.HasAura("Burning Rush") &&
                                         StyxWoW.Me.HealthPercent < WarlockSettings.BurningRushPercentage &&
                                         (WarlockSettings.UseBurningRush || WarlockSettings.AutoBurningRushCancel),
                                  new Action(
                                      delegate
                                      {
                                          Me.CancelAura("Burning Rush");
                                          return RunStatus.Failure;
                                      })),

                    new Decorator(ret => StyxWoW.Me.HasAura("Burning Rush") &&
                                         !StyxWoW.Me.IsMoving &&
                                         (WarlockSettings.UseBurningRush || WarlockSettings.AutoBurningRushCancel),
                                  new Action(
                                      delegate
                                      {
                                          Me.CancelAura("Burning Rush");
                                          return RunStatus.Failure;
                                      }))
                    /*

             // Cancel if our HP gets below threshold
             Lua.CancelMyAura("Burning Rush", ret => StyxWoW.Me.HasAura("Burning Rush") &&
                                                 StyxWoW.Me.HealthPercent < WarlockSettings.BurningRushPercentage &&
                                                 (WarlockSettings.UseBurningRush || WarlockSettings.AutoBurningRushCancel)),

            // Cancel if we stopped moving
             Lua.CancelMyAura("Burning Rush", ret => StyxWoW.Me.HasAura("Burning Rush") &&
                                                 !StyxWoW.Me.IsMoving &&
                                                 (WarlockSettings.UseBurningRush || WarlockSettings.AutoBurningRushCancel))
                    */

                    // Spell.CancelAura
                    );
            }
        }

        private static string GetSacrificeAbility()
        {
            //switch (Helpers.Lua.GetSpellIconText(SpellBook.CommandDemon))

            //Interface\Icons\INV_Misc_Herb_Felblossom
            //ToLower(): interface\icons\inv_misc_herb_felblossom
            switch (WoWSpell.FromId(SpellBook.CommandDemon).Icon.ToLower())
            {
                case @"interface\icons\spell_fel_elementaldevastation":
                    return "Singe Magic";
                case @"interface\icons\spell_shadow_antishadow":
                    return "Shadow Bulwark";
                case @"interface\icons\ability_warlock_whiplash":
                    return "Whiplash";
                case @"interface\icons\spell_shadow_mindrot":
                    return "Spell Lock";
                case @"interface\icons\ability_rogue_sprint":
                    return "Pursuit";
                default:
                    return "Unknown";
            }
        }
        
        #region statics

        public static readonly HashSet<int> DarkSoul = new HashSet<int>
        {
            113860, // Dark Soul: Misery
            113861, // Dark Soul: Knowledge
            113858  // Dark Soul: Instability
        };

        public static bool Hasted { get { return StyxWoW.Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Time Warp"); } }
        private static bool NeedToSummonDoomguard { get { return StyxWoW.Me != null && StyxWoW.Me.IsValid && StyxWoW.Me.IsAlive && StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.IsBoss() && WarlockSettings.UseSummonDoomguard && (StyxWoW.Me.CurrentTarget.HealthPercent <= 20 || Hasted); } }
        private static bool NeedToSummonInferno { get { return StyxWoW.Me != null && StyxWoW.Me.IsValid && StyxWoW.Me.IsAlive && StyxWoW.Me.CurrentTarget != null && WarlockSettings.UseSummonInferno && StyxWoW.Me.CurrentTarget.IsBoss() && Unit.CachedNearbyAttackableUnits(StyxWoW.Me.CurrentTarget.Location, 20).Count() >= WarlockSettings.InfernoCount; } }
        private static bool NeedTwilightWard { get { return StyxWoW.Me != null && StyxWoW.Me.IsValid && StyxWoW.Me.IsAlive && StyxWoW.Me.CurrentTarget != null && (StyxWoW.Me.CurrentTarget.CastingSpell != null && (StyxWoW.Me.CurrentTarget != null && ((StyxWoW.Me.CurrentTarget.CastingSpell.School == WoWSpellSchool.Shadow || StyxWoW.Me.CurrentTarget.CastingSpell.School == WoWSpellSchool.Holy || StyxWoW.Me.CurrentTarget.ChanneledCastingSpellId == 123461) && StyxWoW.Me.HealthPercent < WarlockSettings.TwilightWardPercent))); } }
        internal static bool NeedCurseOfElements { get { return StyxWoW.Me != null && StyxWoW.Me.IsValid && StyxWoW.Me.IsAlive && Me.CurrentTarget != null && !Me.CurrentTarget.HasAnyAura("Curse of the Elements", "Master Poisoner", "Fire Breath", "Lightning Breath") && (WarlockSettings.OnlyCoEOnBoss && Me.CurrentTarget.IsBoss || !WarlockSettings.OnlyCoEOnBoss); } }
        public static bool CanLifeTap { get { return StyxWoW.Me != null && StyxWoW.Me.IsValid && StyxWoW.Me.IsAlive && Me.CurrentMana < Me.MaxMana - ((15 * Me.MaxHealth) * 0.01); } }
        #endregion statics
    }
}