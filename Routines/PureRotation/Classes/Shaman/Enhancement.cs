

#region Revision info

/*
 * $Author: Alex & Wulf$
 * $Date: 2013-09-18 08:33:50 +0200 (Mi, 18 Sep 2013) $
 * $ID$
 * $Revision: 1$
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Shaman/Enhancement.cs $
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
using Styx.WoWInternals;
using Lua = PureRotation.Helpers.Lua;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.Shaman
{
    [UsedImplicitly]
    internal class Enhancement : RotationBase
    {
        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(
            Spell.Cast("Healing Surge", on => Me, ret => Me.HasAura(53817, 3) && Me.HealthPercent <= 81),
            Spell.Cast("Shamanistic Rage", ret => Me.HealthPercent <= 70)

                );
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new PrioritySelector(

                // actions.single+=/ancestral_swiftness,if=talent.ancestral_swiftness.enabled&buff.maelstrom_weapon.react<2
                // actions.single+=/feral_spirit
                //actions+=/elemental_mastery,if=talent.elemental_mastery.enabled&(!glyph.fire_elemental_totem.enabled|cooldown.fire_elemental_totem.remains=0|cooldown.fire_elemental_totem.remains>70)
                Spell.Cast("Elemental Mastery", ret => TalentManager.HasTalent(10) && (!TalentManager.HasGlyph("Fire Elemental Totem") || !Spell.SpellOnCooldown(2894) || Spell.GetSpellCooldown(2894).TotalMilliseconds > 70000)),
                Spell.Cast("Feral Spirit"),
                Spell.Cast("Fire Elemental Totem"),
                Spell.PreventDoubleCast("Ascendance", 1, ret => Me.CurrentTarget != null && AscendanceGo)

                );
        }

        private static Composite HandleCommon()
        {
            return new PrioritySelector(
                Spell.Cast("Something")
            );
        }

        private static Composite HandleSingleTarget()
        {
            //Elemental Mastery ID :: 16166
            //Unleash Flames  ID :: 73683
            return new PrioritySelector(
                // Add Stormlash totem during Heroism. Once it's on CD, go back to Searing totem. (not sure how to code)
                Spell.PreventDoubleCast("Stormlash Totem", 1, ret => Hasted && !Totems.Exist(WoWTotemType.Air)),
                Spell.PreventDoubleCast("Searing Totem", 1, ret => NeedSearingTotem),                
                Spell.PreventDoubleCast("Unleash Elements", 1, ret => TalentManager.HasTalent(16)),
                Spell.PreventDoubleCast("Elemental Blast", 1, ret => TalentManager.HasTalent(18) && Me.HasAura(53817, 1)),
                Spell.PreventDoubleCast("Lightning Bolt", 1, ret => Me.HasAura(53817, 5)),
                // feral_spirit,if=set_bonus.tier15_4pc_melee=1
                // Fixed NeedStormBlast to ensure it's only cast when Stormblast is ready
                Lua.RunMacroText("/cast Stormblast", ret => NeedStormBlast),
                // stormstrike
                Spell.Cast("Stormstrike"),
                // flame_shock,if=buff.unleash_flame.up&!ticking
                Spell.Cast("Flame Shock", ret => Me.HasAura("Unleash Flame") && !Spell.HasMyAura("Flame Shock", StyxWoW.Me.CurrentTarget)),
                // lava_lash
                Spell.Cast("Lava Lash"),
                // lightning_bolt,if=set_bonus.tier15_2pc_melee=1&buff.maelstrom_weapon.react>=4&!buff.ascendance.up
                 Spell.Cast("Flame Shock", ret => Me.HasAura("Unleash Flame") || (!Me.HasAura("Unleash Flame") && !Spell.HasMyAura("Flame Shock", StyxWoW.Me.CurrentTarget) && Spell.GetSpellCooldown("Unleash Elements").TotalSeconds > 5)),
                Spell.PreventDoubleCast("Unleash Elements", 1, ret => true),
                // lightning_bolt,if=buff.maelstrom_weapon.react>=3&!buff.ascendance.up
                Spell.PreventDoubleCast("Lightning Bolt", 1, ret => Me.HasAura(53817, 3) && !Me.HasAura(128201)),
                // ancestral_swiftness,if=talent.ancestral_swiftness.enabled&buff.maelstrom_weapon.react<2
                // lightning_bolt,if=buff.ancestral_swiftness.up
                Spell.PreventDoubleCast("Lightning Bolt", 1, ret => Me.HasAura("Ancestral Swiftness")),
                // earth_shock
                Spell.Cast("Earth Shock"),
                // feral_spirit
                // earth_elemental_totem,if=!active&cooldown.fire_elemental_totem.remains>=50
                // spiritwalkers_grace,moving=1
                // lightning_bolt,if=buff.maelstrom_weapon.react>1&!buff.ascendance.up
                Spell.PreventDoubleCast("Lightning Bolt", 1, ret => EverythingOnCooldown && Me.HasAura("Maelstrom Weapon", 1) && !Me.HasAura(128201))
                 );

            // Let's start from scratch

        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                Spell.Cast("Lightning Shield", ret => !Me.HasAura("Lightning Shield")),
                Spell.PreventDoubleCast("Magma Totem", 1, ret => NeedMagmaTotem),
                Spell.PreventDoubleCast("Searing Totem", 1, ret => NeedSearingTotem && !NeedMagmaTotem),
                Spell.PreventDoubleCast("Fire Nova", 1, ret => Me.CurrentTarget.HasMyAura(8050)),
                 Spell.PreventDoubleCast("Chain Lightning", 1, ret => Me.HasAura(53817, 5)),
                 Spell.Cast("Stormstrike", ret => !Me.HasAura(128201)),
                 Lua.RunMacroText("/cast Stormblast", ret => NeedStormBlast),
                 Spell.Cast("Flame Shock", ret => Me.HasAura("Unleash Flame") || (!Me.HasAura("Unleash Flame") && Spell.GetSpellCooldown("Unleash Elements").TotalSeconds > 5)),
                 Spell.Cast("Lava Lash"),
                 Spell.Cast("Unleash Elements"),
                 Spell.Cast("Earth Shock"),
                 Spell.PreventDoubleCast("Chain Lightning", 1, ret => Me.HasAura(53817, 1))



                );
        }

        #region randomstraw-pvp
        public override Composite PVPRotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new Decorator(ret => Me.CurrentTarget != null && Me.CurrentTarget.IsFriendly,
                                    new PrioritySelector(
                                        PvPBehaviourDefensive()))),
                                new Decorator(ret => Me.CurrentTarget != null && !Me.CurrentTarget.IsFriendly,
                                    new PrioritySelector(
                                        PvPBehaviourOffensive())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                   new Decorator(ret => Me.CurrentTarget != null && Me.HealthPercent < 99,
                                    new PrioritySelector(
                                        PvPBehaviourDefensive()))),
                                new Decorator(ret => Me.CurrentTarget != null && !Me.CurrentTarget.IsFriendly,
                                    new PrioritySelector(
                                        PvPBehaviourOffensive())));
            }
        }

        private static Composite PvPBehaviourDefensive()
        {
            return new PrioritySelector(
                            HandleTremorTotemSpiritWalkInsigniaGroundingTotemPvP(),
                            HandleWindShearPvP(),
                            HandleDefensiveCooldownsPvP(),
                            HandleHealPvP(),
                            HandleDamagePvP(),
                            HandleHexPvP(),
                            HandlePurgePvP(),
                            HandleDecursePvP()
            );
        }
        private static Composite PvPBehaviourOffensive()
        {
            return new PrioritySelector(
                            HandleTremorTotemSpiritWalkInsigniaGroundingTotemPvP(),
                            HandleOffensiveCooldownsPvP(),
                            HandleWindShearPvP(),
                            HandleDefensiveCooldownsPvP(),
                            HandleDamagePvP(),
                            HandleHealPvP(),
                            HandleHexPvP(),
                            HandlePurgePvP(),
                            HandleDecursePvP()

                            
            );
        }
        #endregion

        #region pvp-Handles
        private static Composite HandleTremorTotemSpiritWalkInsigniaGroundingTotemPvP()
        {
            return new PrioritySelector(
                Spell.Cast("Tremor Totem", ret => ((Me.IsFeared() && !Me.IsStunned() && !Me.IsRooted()) || (FocusHeal && Me.FocusedUnit.Distance < 25 && Me.FocusedUnit.IsFeared() && !Me.FocusedUnit.IsStunned()))),
                Spell.Cast("Spirit Walk", ret => Me.IsRooted() && !Me.IsFeared() && !Me.IsStunned()),
                Lua.RunMacroText("/use 13", ret => (Me.IsFeared() && CooldownTracker.GetSpellCooldown("Tremor Totem").Seconds > 2) || (Me.IsRooted() && CooldownTracker.GetSpellCooldown("Spirit Walk").Seconds > 2) || Me.IsStunned() || Me.IsCrowdControlled()),
                Spell.Cast("Grounding Totem", ret => NeedGroundingTotem)
                );
        }
        private static Composite HandleOffensiveCooldownsPvP()
        {
            return new Decorator(ret => NeedOffensiveCooldownsPvP || Me.CurrentTarget.IsDummy(),
                        new PrioritySelector(
                          //  Lua.RunMacroText("/use 14", ret => true),
                            Spell.Cast("Feral Spirit"),
                            Spell.Cast("Heroism", ret => Battlegrounds.IsInsideBattleground || Me.CurrentTarget.IsDummy()),
                            Spell.Cast("Fire Elemental Totem"),
                            Spell.Cast("Stormlash Totem", ret => Battlegrounds.IsInsideBattleground || Me.CurrentTarget.IsDummy()),
                            Spell.Cast("Ascendance")
                            )
                        );
        } //for now disabeld in the rotation
        private static Composite HandleDefensiveCooldownsPvP()
        {
            return new PrioritySelector(
                Spell.Cast("Ancestral Guidance", ret => (FocusHeal && Me.FocusedUnit.HealthPercent < 65) || Me.HealthPercent < 65),
                Spell.Cast("Shamanistic Rage", ret => Me.HealthPercent < 70),
                Spell.Cast("Stone Bulwark Totem", ret => Me.HealthPercent < 75)
                );
        }
        private static Composite HandleDamagePvP()
        {
            return new PrioritySelector(
                Spell.Cast("Frost Shock", ret => Me.CurrentTarget, ret => Me.CurrentTarget.Distance < 30 /*!Me.CurrentTarget.CachedHasAura("Frost Shock", 0, false, 1000)*/),
                Spell.Cast("Unleash Elements", ret => Me.CurrentTarget, ret => Me.CurrentTarget.Distance < 40 && Me.Inventory.Equipped.MainHand.TemporaryEnchantment != null),
                Spell.Cast("Stormstrike", ret => Me.CurrentTarget, ret => Me.CurrentTarget.IsWithinMeleeRange || (Me.CachedHasAura("Ascendance") && Me.CurrentTarget.Distance < 30)),
                Spell.Cast("Lava Lash", ret => Me.CurrentTarget, ret => Me.CurrentTarget.IsWithinMeleeRange),
                Spell.Cast("Lightning Bolt", ret => Me.CurrentTarget, ret => NeedLightningBoltInTheFace)
                );
        }
        private static Composite HandleHealPvP()
        {
            return new PrioritySelector(
                Spell.Cast("Spiritwalker's Grace", ret => NeedSpiritwalkersGrace),

                Spell.PreventDoubleCast("Healing Surge", 1, ret => NeedHealingSurge),
                Spell.PreventDoubleCast("Healing Surge", 1, ret => Me.FocusedUnit, ret => NeedHealingSurgeFocus),

                Spell.Cast("Gift of the Naaru", ret => NeedGiftOfTheNaaru),
                Spell.Cast("Gift of the Naaru", ret => Me.FocusedUnit, ret => NeedGiftOfTheNaaruFocus)
                );
        }
        
        private static Composite HandleWindShearPvP()
        {
            return new PrioritySelector(
                Spell.Cast("Wind Shear", ret => Me.CurrentTarget, ret => Me.CurrentTarget.IsCasting && Me.CurrentTarget.CanInterruptCurrentSpellCast && Me.CurrentTarget.Distance < 30 && Me.CurrentTarget.InLineOfSpellSight),
                Spell.Cast("Wind Shear", ret => Me.FocusedUnit, ret => FocusHarm && Me.FocusedUnit.IsCasting && Me.FocusedUnit.CanInterruptCurrentSpellCast && Me.FocusedUnit.Distance < 30 && Me.FocusedUnit.InLineOfSpellSight)
                );
        }
        private static Composite HandleHexPvP()
        {
            return new PrioritySelector(
                Spell.Cast("Hex", ret => Me.CurrentTarget, ret => FocusHeal && !Me.IsMoving && Me.CurrentTarget.Distance < 30),
                Spell.Cast("Hex", ret => Me.FocusedUnit, ret => FocusHarm && !Me.IsMoving && Me.FocusedUnit.Distance < 30)
                );
        }
        private static Composite HandlePurgePvP()
        {
            return new PrioritySelector(
                Spell.PreventDoubleCast("Purge", 2, ret => Me.CurrentTarget, ret => Me.CurrentTarget.HasAnyAura(new[] { "Avenging Wrath"/*,"more auras here","and here"*/}) && Me.CurrentTarget.Distance < 30),
                Spell.PreventDoubleCast("Purge", 2, ret => Me.FocusedUnit, ret => FocusHarm && Me.FocusedUnit.HasAnyAura(new[] { "Avenging Wrath"/*,"more auras here","and here"*/}) && Me.FocusedUnit.Distance < 30)
                );
        }
        private static Composite HandleDecursePvP()
        {
            return new PrioritySelector(
                Spell.PreventDoubleCast("Cleanse Spirit", 2, ret => Me.Debuffs.Any(retb => retb.Value.Spell.DispelType.Equals(WoWDispelType.Curse))),
                Spell.PreventDoubleCast("Cleanse Spirit", 2, ret => Me.FocusedUnit, ret => FocusHeal && Me.FocusedUnit.Debuffs.Any(retb => retb.Value.Spell.DispelType.Equals(WoWDispelType.Curse)))
                );
        }
        #endregion

        #region pvp-booleans

        private static bool FocusHarm { get { return Me.CurrentTarget != null && !Me.FocusedUnit.IsFriendly; } }
        private static bool FocusHeal { get { return Me.CurrentTarget != null && Me.HealthPercent < 99; } }
    
        /*private static bool FocusExists
        {
            get
            {
                return StyxWoW.Me.FocusedUnit != null ? true : false;
            }
        }*/
        private static bool NeedOffensiveCooldownsPvP
        {
            get
            {
                return Me.CurrentTarget.HealthPercent < 70 && Me.HealthPercent > 50;
            }
        }
        private static bool NeedLightningBoltInTheFace
        {
            get
            {
                return ((FocusHeal && Me.CurrentTarget.Distance < 30 && Me.CurrentTarget.InLineOfSpellSight && Me.HasAura(53817, 5)) && Me.HealthPercent > 70) || //defensive
                        (((FocusHarm && Me.CurrentTarget.Distance < 30 && Me.CurrentTarget.InLineOfSpellSight &&  Me.HasAura(53817, 5)) && Me.HealthPercent >= 50)) || //offensive 1
                        (FocusHarm && Me.CurrentTarget.Distance > 14 && Me.CurrentTarget.InLineOfSpellSight && Me.HealthPercent >= 50); //offensive 2
            }
        }
        private static bool NeedSpiritwalkersGrace //if we don't have 5 maelstromstacks and need healing, grace
        {
            get
            {
                return  (FocusHeal && 
                                (!Me.HasAura(53817, 5) && (Me.HealthPercent < 70 ||  Me.FocusedUnit.HealthPercent < 70) ||
                                (Me.HealthPercent < 35 || Me.FocusedUnit.HealthPercent < 35)) ||
                        (FocusHarm &&
                                (!Me.HasAura(53817, 5) && Me.HealthPercent < 50 && Me.CurrentTarget.HealthPercent > Me.HealthPercent) ||
                                (Me.HealthPercent < 35)));
            }
        }
        private static bool NeedHealingSurge
        {
            get
            {
                return  (FocusHeal && 
                                (Me.HealthPercent < 70 && Me.HasAura(53817, 5)) ||
                                ((Me.HealthPercent < 60 && Me.HasAura(53817, 4)) && (!Me.IsMoving || Me.HasAura("Spiritwalker's Grace"))) ||
                                ((Me.HealthPercent < 50 && Me.HasAura(53817, 3)) && (!Me.IsMoving || Me.HasAura("Spiritwalker's Grace"))) ||
                                ((Me.HealthPercent < 35) && (!Me.IsMoving || Me.CachedHasAura("Spiritwalker's Grace"))) ||
                        (FocusHarm &&
                                (Me.HealthPercent < 50 && Me.CurrentTarget.HealthPercent > Me.HealthPercent && Me.HasAura(53817, 5)) ||
                                ((Me.HealthPercent < 40 && Me.CurrentTarget.HealthPercent > Me.HealthPercent && Me.HasAura(53817, 4)) && (!Me.IsMoving || Me.CachedHasAura("Spiritwalker's Grace"))) ||
                                ((Me.HealthPercent < 35) && (!Me.IsMoving || Me.CachedHasAura("Spiritwalker's Grace")))));
            }
        }
        private static bool NeedHealingSurgeFocus
        {
            get
            {
                return  (FocusHeal &&
                                (Me.FocusedUnit.HealthPercent < 69 && Me.HasAura(53817, 5)) ||
                                ((Me.FocusedUnit.HealthPercent < 59 && Me.HasAura(53817, 4)) && (!Me.IsMoving || Me.CachedHasAura("Spiritwalker's Grace"))) ||
                                ((Me.FocusedUnit.HealthPercent < 49 && Me.HasAura(53817, 3)) && (!Me.IsMoving || Me.CachedHasAura("Spiritwalker's Grace"))) ||
                                ((Me.FocusedUnit.HealthPercent < 34) && (!Me.IsMoving || Me.CachedHasAura("Spiritwalker's Grace"))));
            }
        }
        private static bool NeedGiftOfTheNaaru
        {
            get
            {
                return  (FocusHeal &&
                                (Me.Race.Equals(WoWRace.Draenei) && Me.HealthPercent < 70)) ||
                        (FocusHarm &&
                                (Me.Race.Equals(WoWRace.Draenei) && Me.HealthPercent < 70));
            }
        }
        private static bool NeedGiftOfTheNaaruFocus
        {
            get
            {
                return  (FocusHeal &&
                                (Me.Race.Equals(WoWRace.Draenei) && Me.FocusedUnit.HealthPercent < 70));
            }
        }
        private static bool NeedGroundingTotem
        {
            get
            {
                return (Me.CurrentTarget.IsCasting && !Me.CurrentTarget.IsCastingHealingSpell && CooldownTracker.GetSpellCooldown("Wind Shear").Seconds > 1) ||
                       (FocusHarm && Me.FocusedUnit.IsCasting && !Me.FocusedUnit.IsCastingHealingSpell && CooldownTracker.GetSpellCooldown("Wind Shear").Seconds > 1);
            }
        }
        #endregion


        #region booleans

        private static bool WaitForSpeedBuff { get { return Me.CurrentTarget != null && !Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Timewarp"); } }

        private static bool FireElementalStuff1
        {
            get
            {
                return (TalentManager.HasTalent(10) && (CooldownTracker.GetSpellCooldown("Elemental Mastery").Seconds > 0 || CooldownTracker.GetSpellCooldown("Elemental Mastery").Seconds > 80));
            }
        }

        private static bool NeedStormBlast { get { return Me.CurrentTarget != null && Me.HasAura(128201) && Spell.GetSpellCooldown("Stormblast").TotalSeconds == 0; } }

        private static bool NeedFlameShock
        {
            get
            {
                return Me.CurrentTarget != null &&
                       !Me.CurrentTarget.HasMyAura("Flame Shock") && Me.ActiveAuras.ContainsKey("Unleash Flame"); // refresh early if we can do 30% more damage with buff. Not worth refreshing if we've still got a lot of time left on debuff.
            }
        }

        private static bool NeedFlameShock2
        {
            get
            { 
                return Me.CurrentTarget != null && Me.ActiveAuras.ContainsKey("Unleash Flame") || (!Me.ActiveAuras.ContainsKey("Unleash Flame") && !Me.CurrentTarget.CachedHasAura("Flame Shock") && CooldownTracker.GetSpellCooldown(73680).TotalSeconds > 5);
            }
        }

        private static bool NeedMagmaTotem
        {
            get
            {
                return
                    Me.CurrentTarget != null &&
                    !Me.HasAura("Ascendance") &&
                    !Totems.Exist(WoWTotemType.Fire) &&
                    (Unit.NearbyAttackableUnits(Me.Location, 8).Count() >= 6 ||
                     Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 8).Count() >= 6);

                // Can do a check here if Totemic Projection is off cooldown, then cast anyway and move the totem using
                // CastOnGround
            }
        }

        private static bool FireElementalStuff2 { get { return !Totems.Exist(WoWTotem.FireElemental) || Me.ActiveAuras.ContainsKey("Elemental Mastery"); } }

        private static bool EverythingOnCooldown { get { return Spell.GetSpellCooldown("Stormstrike").TotalMilliseconds > 1000 && Spell.GetSpellCooldown("Earth Shock").TotalMilliseconds > 1000 && Spell.GetSpellCooldown("Lava Lash").TotalMilliseconds > 1000 && Spell.GetSpellCooldown("Flame Shock").TotalMilliseconds > 1000; } }

        private static bool AscendanceGo { get { return !Me.HasAura("Ascendance") && !Spell.SpellOnCooldown("Ascendance") && Spell.GetSpellCooldown(17364).TotalSeconds >= 3 && Me.CurrentTarget.IsBoss(); } }

        private static bool EleBlastGo { get { return Me.CachedHasAura(53817, 1); } }

        private static bool MaelstormstacksCL1 { get { return Me.CachedHasAura(53817, 1); } }

        private static bool LightningBoltGo { get { return Me.CachedHasAura(53817, 5); } }

        private static bool LavaLashGo { get { return Me.CachedHasAura(77661, 5); } }

        private static bool Ascendancelightning { get { return Me.CachedHasAura(53817, 3) && !Me.HasAura(128201); } }

        private static bool LightningBoltFiller { get { return Me.CachedHasAura(53817, 1) && !Me.HasAura(128201); } }

        private static bool MaelstormstacksCL { get { return Me.CachedHasAura(53817, 3); } }

        private static bool Hasted { get { return Me.CurrentTarget != null && Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Timewarp"); } }

        private static bool NeedSearingTotem
        {
            get
            {
                return Me.CurrentTarget.Distance <= 39 && // is close enough to be hit from searing totem (totem = 40yd range)
                    (!Totems.Exist(WoWTotem.Magma) && !Totems.Exist(WoWTotem.FireElemental) && !Totems.Exist(WoWTotem.Searing)) || // dont cast if we have a fire totem active (like fire elemental)
                    // or we have one already, but it's not in range of our target
                    (Totems.Exist(WoWTotem.Searing) && !Totems.ExistInRange(Me.CurrentTarget.Location, WoWTotem.Searing));
            }
        }

        #endregion booleans

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.ShamanEnhancement; }
        }

        public override string Name
        {
            get { return "Enhancement Shaman"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    new Decorator(ret => PRSettings.Instance.Shaman.EnhanceForcePVP, PVPRotation),
                    new Decorator(ret => !PRSettings.Instance.Shaman.EnhanceForcePVP,
                    new PrioritySelector(
                    Racials.UseRacials(),
                    HandleCommon(),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    HandleDefensiveCooldowns(), // Stay Alive!
                    Item.HandleItems(),       // Pop Trinkets, Drink potions...
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),
                                      new Decorator(ret => Me.CurrentTarget != null && PRSettings.Instance.UseAoEAbilities && Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 25).Count(x => !x.IsBoss()) > PRSettings.Instance.Shaman.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))))));
            }
        }


        public override Composite Medic
        {
            get
            {
                return new PrioritySelector(
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
                        Spell.Cast("Lightning Shield", ret => !StyxWoW.Me.HasAura("Lightning Shield")),
                        Spell.Cast("Windfury Weapon", ret => Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 0),
                        Spell.Cast("Flametongue Weapon", ret => Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id == 0)
                        ));
            }
        }

        #endregion Overrides of RotationBase
    }
}