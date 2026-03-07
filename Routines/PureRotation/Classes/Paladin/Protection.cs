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
using Styx.CommonBot;
using System;

namespace PureRotation.Classes.Paladin
{
    [UsedImplicitly]
    internal class Protection : RotationBase
    {
        private static double ThrottleTime { get { return PRSettings.ThrottleTime; } }

        private static PaladinSettings PaladinSettings { get { return PRSettings.Instance.Paladin; } }

        private static Composite HandleDefensiveCooldowns()
        {
            return new PrioritySelector(

                // TODO: Devotion Aura, Selfless healer
                      Spell.Cast("Hand of Freedom", on => Me, ret => NeedHandofFreedom), // not much point in trying to cast shit if we are incapacitated.
                      Spell.Cast("Ardent Defender", on => Me, ret => NeedArdentDefender),
                      Spell.PreventDoubleCast("Sacred Shield", ThrottleTime, on => Me, ret => !Me.HasAura(65148) || !Me.CachedHasAura(SpellBook.VengeancePala, 0, false, 3000) || DoTTracker.SpellStats(StyxWoW.Me, SpellBook.VengeancePala).AttackPower + 10000 < Me.AttackPower), // TODO: check for buff to just maintain uptime or spam it ?
                      Spell.Cast("Eternal Flame", on => Me, ret => NeedWordofGlory), // TODO: This should be used idententicly to word of glory
                      Spell.Cast("Word of Glory", on => Me, ret => NeedWordofGlory),
                      Spell.Cast("Guardian of Ancient Kings", on => Me, ret => NeedGuardianofAncientKings),
                      Spell.Cast("Divine Protection", on => Me, ret => NeedDivineProtection),
                      Spell.Cast("Holy Avenger", on => Me, ret => NeedHolyAvenger),
                      Spell.Cast("Avenging Wrath", ret => NeedAvengingWrath), // this can be offensive or defensive dependant on user settings. TODO: (if used as a defensive ability do we need two health settings to cover teh additional Healing with Sanctified Wrath?)
                      Spell.Cast("Execution Sentence", ret => NeedExecutionSentence), // this can be offensive or defensive dependant on user settings.
                      HandleForbearanceAbilitys());
        }

        private static Composite HandleOffensiveCooldowns()
        {
            return new Decorator(ret => Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange, // Dont blow shit when we arnt near anything
                    new PrioritySelector(
                        Spell.Cast("Holy Prism", on => Common.BestHolyPrismTarget, ret => NeedHolyPrism), // TODO: Check the state of the raid..if people need healing..hit it!
                        Spell.Cast("Light's Hammer", on => Common.BestHolyPrismTarget, ret => NeedLightsHammer),// TODO: Check the state of the raid..if people need healing..hit it!
                        Spell.Cast("Execution Sentence")));
        }

        private static Composite HandleSingleTarget()
        {
            return new PrioritySelector(
                    HandleShieldoftheRighteous(),
                    Spell.Cast("Hammer of the Righteous", ret => NeedHammeroftheRighteous),
                    Spell.Cast("Crusader Strike"),
                    Spell.Cast("Judgment"),
                    Spell.Cast("Avenger's Shield"),
                    Spell.Cast("Hammer of Wrath", ret => NeedHammerofWrath),
                    Spell.Cast("Holy Wrath"),
                    HandleConsecration(false));
        }

        private static Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                    HandleShieldoftheRighteous(),
                    Spell.Cast("Hammer of the Righteous"),
                    Spell.Cast("Judgment"),
                    Spell.Cast("Avenger's Shield", ret => Me.HasAura("Grand Crusader")),
                    HandleConsecration(true),
                    Spell.Cast("Avenger's Shield"),
                    Spell.Cast("Hammer of Wrath", ret => NeedHammerofWrath),
                    Spell.Cast("Holy Wrath"));
        }

        private static Composite HandleForbearanceAbilitys()
        {
            return new Decorator(
                    ret => !Me.HasAura("Forbearance") && !Me.HasAura("Ardent Defender") && !Me.HasAura("Guardian of Ancient Kings"),
                    new PrioritySelector(
                        Spell.Cast("Lay on Hands", on => Me, ret => Me.HealthPercent <= PaladinSettings.ProtectionLoHPercent),
                        Spell.Cast("Divine Shield", ret => Me.HealthPercent <= PaladinSettings.ProtectionDSPercent)));
        }

        private static Composite HandleConsecration(bool forAoE)
        {
            return new PrioritySelector(
                    Spell.CastOnGround("Consecration", on => Me.CurrentTarget.Location, ret => TalentManager.HasGlyph("Consecration") && forAoE ? NeedConsecrationAoE : NeedConsecrationSingle), // todo: this needs to be cast on the central target regardless of distance to target.
                    Spell.Cast("Consecration", ret => !TalentManager.HasGlyph("Consecration") && forAoE ? NeedConsecrationAoE : NeedConsecrationSingle)
                );
        }

        private static Composite HandleSeals()
        {
            return new PrioritySelector(
                 new Decorator(ret => PaladinSettings.UseSealSwapping, // bit of fun..can be used on high add count..default is >= 5
                        new PrioritySelector(
                            Spell.PreventDoubleCast("Seal of Righteousness", ThrottleTime, ret => PaladinSettings.UseSealSwapping && !Me.HasAura("Seal of Righteousness") && Unit.NearbyAttackableUnits(Me.Location, 10f).Count() >= PaladinSettings.SealofRighteousnessCount),
                            Spell.PreventDoubleCast("Seal of Insight", ThrottleTime, ret => PaladinSettings.UseSealSwapping && !Me.HasAura("Seal of Insight") && Unit.NearbyAttackableUnits(Me.Location, 10f).Count() < PaladinSettings.SealofRighteousnessCount))));
        }

        private static Composite HandleShieldoftheRighteous()
        {
            return new PrioritySelector(
                   Spell.Cast("Shield of the Righteous", ret => Me.CurrentHolyPower >= 3 && (Me.HasAura("Sacred Shield")) || Me.HasAura("Divine Purpose")),
                   Spell.Cast("Shield of the Righteous", ret => Me.HealthPercent < PaladinSettings.ShoRPercent || Me.CurrentHolyPower == 5 || !Me.HasAura("Bastion of Glory") || !Me.HasAura("Shield of the Righteous")) //TODO: Tracking Bastion of Glory for Word Of Glory
                );
        }

        #region booleans

        private static bool NeedWordofGlory { get { return Me.HealthPercent <= PaladinSettings.ProtWordofGloryPercent && ( Me.CurrentHolyPower > 1 || Me.HasAura("Divine Purpose")); } }

        private static bool NeedAvengingWrath { get { return PaladinSettings.UseAvengingWrath && ((PaladinSettings.UseAvengingWrathforThreat && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.IsBoss()) || !PaladinSettings.UseAvengingWrathforThreat && Me.HealthPercent <= PaladinSettings.UseAvengingWrathHealthPercent); } }

        private static bool NeedExecutionSentence { get { return PaladinSettings.UseExecutionSentence && ((PaladinSettings.UseExecutionSentenceforThreat && Me.CurrentTarget != null && Me.CurrentTarget.IsWithinMeleeRange && Me.CurrentTarget.IsBoss()) || !PaladinSettings.UseExecutionSentenceforThreat && Me.HealthPercent <= PaladinSettings.UseExecutionSentenceHealthPercent); } }

        private static bool NeedHolyPrism { get { return Me.HealthPercent <= PaladinSettings.HolyPrismPercent && Unit.AttackableMeleeUnits.Count() >= PaladinSettings.HolyPrismCount; } }

        private static bool NeedLightsHammer { get { return Me.HealthPercent <= PaladinSettings.LightsHammerPercent && Unit.NearbyAttackableUnits(Me.CurrentTarget.Location, 15).Count() >= PaladinSettings.LightsHammerCount; } }

        private static bool NeedHolyAvenger { get { return Me.HealthPercent <= PaladinSettings.HolyAvengerHealthProt && !Me.HasAura("Ardent Defender") && !Me.HasAura("Guardian of Ancient Kings"); } }

        private static bool NeedGuardianofAncientKings { get { return Me.HealthPercent <= PaladinSettings.GuardianofAncientKingsHealthProt && !Me.HasAura("Ardent Defender"); } }

        private static bool NeedArdentDefender { get { return Me.HealthPercent <= PaladinSettings.ArdentDefenderHealth; } }

        private static bool NeedDivineProtection { get { return Me.HealthPercent <= PaladinSettings.ProtectionDPPercent; } }

        private static bool NeedHammeroftheRighteous { get { return StyxWoW.Me.CurrentTarget != null && (!StyxWoW.Me.CurrentTarget.ActiveAuras.ContainsKey("Weakened Blows") && Me.CurrentTarget.IsWithinMeleeRange); } }

        private static bool NeedHammerofWrath { get { return Me.CurrentTarget != null && Me.CurrentTarget.HealthPercent <= 20; } }

        private static bool NeedConsecrationAoE { get { return Me.CurrentTarget != null && !BossList.IgnoreAoE.Contains(Common.CurrentTargetEntry) && Me.ManaPercent >= PaladinSettings.ConsecrationManaPercent && Unit.AttackableMeleeUnits.Count() >= PaladinSettings.ConsecrationCount; } }

        private static bool NeedConsecrationSingle { get { return Me.CurrentTarget != null && !BossList.IgnoreAoE.Contains(Common.CurrentTargetEntry) && Me.ManaPercent >= PaladinSettings.ConsecrationManaPercent; } }

        private static bool NeedHandofFreedom { get { return StyxWoW.Me.HasAuraWithMechanic(WoWSpellMechanic.Dazed, WoWSpellMechanic.Disoriented, WoWSpellMechanic.Frozen, WoWSpellMechanic.Incapacitated, WoWSpellMechanic.Rooted, WoWSpellMechanic.Slowed, WoWSpellMechanic.Snared); } }

        public static bool IsGlobalCooldown(bool allowLagTollerance = true)
        {
            uint latency = allowLagTollerance ? StyxWoW.WoWClient.Latency : 0;
            TimeSpan gcdTimeLeft = SpellManager.GlobalCooldownLeft;
            return gcdTimeLeft.TotalMilliseconds > latency;
        }

        
        
        #endregion booleans

        #region Overrides of RotationBase

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.PaladinProtection; }
        }

        public override string Name
        {
            get { return "Protection Paladin"; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                    new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                    EncounterSpecific.HandleActionBarInterrupts(),
                    Racials.UseRacials(),
                    HandleDefensiveCooldowns(),                         // Stay Alive!
                    Item.HandleItems(),                                 // Pop Trinkets, Drink potions...
                    Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                    HandleSeals(),
                    new Decorator(ret => !IsGlobalCooldown() && HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                  new PrioritySelector(
                                      new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleOffensiveCooldowns()),
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() >= PaladinSettings.ProtAoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => !IsGlobalCooldown() && HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() >= PaladinSettings.ProtAoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                      HandleSingleTarget())),
                    new Decorator(ret => !IsGlobalCooldown() && HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                  new PrioritySelector(
                                      new Decorator(ret => HotKeyManager.IsCooldown, HandleOffensiveCooldowns()),
                                      new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                      new Decorator(ret => !HotKeyManager.IsAoe, HandleSingleTarget()))));
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
                        //HandleOffensiveCooldowns(),                                    // Utility abilitys...
                        new Decorator(ret => Me.HealthPercent < 100,
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
                        Spell.Cast("Righteous Fury", ret => !Me.HasAura("Righteous Fury")),
                        Spell.Cast("Seal of Insight", ret => !Me.HasAura("Seal of Insight")),
                        Spell.Cast("Blessing of Kings", ret => PaladinSettings.BlessingSelection == PaladinBlessing.Kings && !StyxWoW.Me.HasAura("Blessing of Kings")), //todo: hasanyaura
                        Spell.Cast("Blessing of Might", ret => PaladinSettings.BlessingSelection == PaladinBlessing.Might && !StyxWoW.Me.HasAura("Blessing of Might")))); //todo: hasanyaura
            }
        }

        #endregion Overrides of RotationBase
    }
}