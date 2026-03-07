#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date $
 * $ID$
 * $Revision: 1729 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Classes/Monk/Windwalker.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info
using System;
using System.Linq;
using CommonBehaviors.Actions;
using JetBrains.Annotations;
using PureRotation.Core;
using PureRotation.Helpers;
using PureRotation.Managers;
using PureRotation.Settings.Settings;
using Styx;
using Styx.TreeSharp;
using Styx.CommonBot;
using Styx.Common;
using Styx.Common.Helpers;
using Action = Styx.TreeSharp.Action;
namespace PureRotation.Classes.Monk
{
    [UsedImplicitly]
    public class Windwalker : RotationBase
    {
        //actions+=/chi_sphere,if=talent.power_strikes.enabled&buff.chi_sphere.react&chi<4
        //actions+=/virmens_bite_potion,if=buff.bloodlust.react|target.time_to_die<=60
        //actions+=/use_item,name=firecharm_grips
        //actions+=/berserking
        //actions+=/chi_brew,if=talent.chi_brew.enabled&chi=0
        //actions+=/tiger_palm,if=buff.tiger_power.remains<=3
        //actions+=/tigereye_brew,line_cd=15,if=buff.rune_of_reorigination.react&(buff.rune_of_reorigination.remains<=1|(buff.tigereye_brew_use.down&cooldown.rising_sun_kick.remains=0&chi>=2&target.debuff.rising_sun_kick.remains&buff.tiger_power.remains))
        //actions+=/tigereye_brew,if=!buff.tigereye_brew_use.up&(buff.tigereye_brew.react>19|target.time_to_die<20)
        //actions+=/tigereye_brew,if=buff.tigereye_brew_use.down&cooldown.rising_sun_kick.remains=0&chi>=2&target.debuff.rising_sun_kick.remains&buff.tiger_power.remains
        //actions+=/energizing_brew,if=energy.time_to_max>5
        //actions+=/rising_sun_kick,if=!target.debuff.rising_sun_kick.remains
        //actions+=/tiger_palm,if=buff.tiger_power.down&target.debuff.rising_sun_kick.remains>1&energy.time_to_max>1
        //actions+=/invoke_xuen,if=talent.invoke_xuen.enabled
        //Tigereye Brew Use ID: 125195
        //Tigereye Buff ID : 116740
        //PowerStrike aura ID: 129914
        //rune_of_reorigination(TRINKET PROC drops in ToT!!! OP !!!) ID: 139116
        //Rising Sun Kick ID: 107428
        //Rising Sun Kick Debuff ID: 130320
        //Blackout Kick Combo Proc ID : 116768
        //Tiger Power ID : 125359

        private Composite HandleCooldowns()
        {
            return new PrioritySelector(
               Spell.Cast("Chi Sphere", ret => TalentManager.HasTalent(7) && Me.HasAura(129914) && Common.Chi < 4),
               Spell.Cast("Chi Brew", ret => TalentManager.HasTalent(9) && Common.Chi == 0),
               Spell.Cast("Invoke Xuen, the White Tiger", ret => TalentManager.HasTalent(17)),
               Spell.Cast("Rushing Jade Wind", ret => PRSettings.Instance.Monk.UseRJW && TalentManager.HasTalent(16) && Me.HasAura("Tiger Power") && Me.CurrentTarget.HasAura("Rising Sun Kick") && Me.CurrentEnergy <= 80 && StyxWoW.Me.CurrentTarget.IsBoss()));
        }

        private Composite HandleAoeCombat()
        {
            return new PrioritySelector(
                      Spell.Cast("Rising Sun Kick", ret => !Me.CurrentTarget.HasAura("Rising Sun Kick")),
                      Spell.Cast("Rushing Jade Wind", ret => PRSettings.Instance.Monk.UseRJW && TalentManager.HasTalent(16) && Me.HasAura("Tiger Power") && Me.CurrentTarget.HasAura("Rising Sun Kick")),
                      Spell.Cast("Spinning Crane Kick"),
                      Spell.PreventDoubleCast("Tiger Palm", 0.7, ret => Me.CurrentTarget.HasAura("Rising Sun Kick") && Me.HasAura("Tiger Power") && Lua.PlayerBuffTimeLeft("Tiger Power") < 3),
                      Spell.PreventDoubleCast("Blackout Kick", 1, ret => Me.CurrentTarget.HasAura("Rising Sun Kick") && Me.HasAura("Tiger Power") && Lua.PlayerBuffTimeLeft("Tiger Power") >= 3 && Me.CurrentEnergy < 40));
        }

       // actions+=/tigereye_brew,line_cd=15,if=buff.rune_of_reorigination.react&(buff.rune_of_reorigination.remains<=1|(buff.tigereye_brew_use.down&cooldown.rising_sun_kick.remains=0&chi>=2&target.debuff.rising_sun_kick.remains&buff.tiger_power.remains))
      //actions+=/tigereye_brew,if=!buff.tigereye_brew_use.up&(buff.tigereye_brew.react>19|target.time_to_die<20)

        private Composite SingleTargetWithFistsofFury()
        {
            return new PrioritySelector(
                        Spell.Cast("Rising Sun Kick"),
                        Spell.Cast("Touch of Death", ret => Me.HasAura("Death Note") && PRSettings.Instance.Monk.UseTouchOfDeath),
                        Spell.Cast("Tiger Palm", ret => !Me.HasAura(125359) || Spell.GetAuraTimeLeft(125359) <= 3),
                        Spell.Cast("Energizing Brew", ret => Me.CurrentEnergy < 40 && !Spell.IsChanneling && !Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Time Warp")),
                        Spell.Cast("Fists of Fury", ret => !Me.IsMoving && !Me.HasAura(139120) && !Me.HasAura("Energizing Brew") && Me.CurrentTarget.HasAura("Rising Sun Kick") && Me.CurrentEnergy < 50 && Spell.GetAuraTimeLeft(125359) > 4),
                        Spell.Cast("Chi Wave", ret => TalentManager.HasTalent(4) && Me.CurrentEnergy <= 80),
                        Spell.Cast("Blackout Kick", ret => Me.HasAura(116768)),
                        Spell.Cast("Tiger Palm", ret => Me.HasAura(118864)),
                        Spell.Cast("Jab", ret => Me.CurrentChi <= 3),
                        Spell.PreventDoubleCast("Blackout Kick", 0.7, ret => Spell.SpellOnCooldown("Rising Sun Kick")));
        }

        private Composite SingleTargetWithOutFistsofFury()
        {
            return new PrioritySelector( 
                        Spell.Cast("Rising Sun Kick"),
                        Spell.Cast("Touch of Death", ret => Me.HasAura("Death Note") && PRSettings.Instance.Monk.UseTouchOfDeath),
                        Spell.Cast("Tiger Palm", ret => !Me.HasAura(125359) || Spell.GetAuraTimeLeft(125359) <= 3),
                        Spell.Cast("Energizing Brew", ret => Me.CurrentEnergy < 40 && !Spell.IsChanneling && !Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Time Warp")),                
                        Spell.Cast("Chi Wave", ret => TalentManager.HasTalent(4) && Me.CurrentEnergy <= 80),
                        Spell.Cast("Blackout Kick", ret => Me.HasAura(116768)),
                        Spell.Cast("Tiger Palm", ret => Me.HasAura(118864)),
                        Spell.Cast("Jab", ret => Me.CurrentChi <= 3),
                        Spell.PreventDoubleCast("Blackout Kick", 0.7, ret => Spell.SpellOnCooldown("Rising Sun Kick")));
        }

        private Composite TigerEyeBrewHandle()
        {
            return new Decorator(ret => Me.CurrentTarget != null && (!Me.HasAura(116740) ||(Me.HasAura(116740) &&  Me.GetAuraById(116740).TimeLeft.Milliseconds <= 2500)) && PRSettings.Instance.Monk.TigerEyeBrewUsage != TeBUsage.None,
                new PrioritySelector(
                        new Decorator(ctx => PRSettings.Instance.Monk.TigerEyeBrewUsage == TeBUsage.Heroism,
                            Spell.PreventDoubleCast("Tigereye Brew", 0.7, ret => Me.CurrentChi >= 2 && Spell.GetSpellCooldown("Rising Sun Kick").TotalMilliseconds < 1000 && Me.CurrentTarget.HasAura("Rising Sun Kick") && Me.HasAura("Tiger Power") && Me.HasAnyAura("Bloodlust", "Heroism", "Ancient Hysteria", "Time Warp"))),
                        new Decorator(ctx => PRSettings.Instance.Monk.TigerEyeBrewUsage == TeBUsage.Cooldown,
                            new PrioritySelector(
                                Spell.PreventDoubleCast("Tigereye Brew", 0.7, ret => Me.CurrentChi >= 2 && Spell.GetSpellCooldown("Rising Sun Kick").TotalMilliseconds < 1000 && Me.CurrentTarget.HasAura("Rising Sun Kick") && Me.HasAura("Tiger Power") && Spell.StackCount(Me, "Tigereye Brew") >= PRSettings.Instance.Monk.TigerEyeBrewCount),
                                Spell.PreventDoubleCast("Tigereye Brew", 0.7, ret => (Spell.StackCount(Me, "Tigereye Brew") > 19 || Me.CurrentTarget.HealthPercent <= 10)))),
                        new Decorator(ret => PRSettings.Instance.Monk.TigerEyeBrewUsage == TeBUsage.RoRo && useTeBOnRoRo,
                            Spell.Cast("Tigereye Brew", ret => Me.HasAura("Tigereye Brew") && Me.ActiveAuras["Tigereye Brew"].StackCount >= PRSettings.Instance.Monk.TigerEyeBrewCount))
                        )
                    );
        }

        private Composite DefensiveCooldowns()
        {
            return new Decorator(
                     ret => Me.HealthPercent < 100 && PRSettings.Instance.EnableSelfHealing,
                     new PrioritySelector(
                         Spell.Cast("Expel Harm", ret => PRSettings.Instance.Monk.EnableExpelHarm && Me.HealthPercent < PRSettings.Instance.Monk.ExpelHarmPercentWW),
                         Spell.Cast("Fortifying Brew", ret => !StyxWoW.Me.HasAura("Fortifying Brew") && Me.HealthPercent < PRSettings.Instance.Monk.FortifyingBrewPercentWW),
                         Spell.Cast("Touch of Karma", ret => PRSettings.Instance.Monk.EnableTouchofKarmaWW && Me.HealthPercent < PRSettings.Instance.Monk.TouchofKarmaHPWW),
                         Item.UseBagItem(5512, ret => NeedHealthstone, "Healthstone"),
                         Spell.Cast("Nimble Brew", ret => (Me.IsRooted() || Me.IsFeared() || Me.IsStunned()) && PRSettings.Instance.Monk.UseNimbleBrewWW),
                         Spell.Cast("Diffuse Magic", ret => TalentManager.HasTalent(15) && !Me.HasAura("Diffuse Magic") && Me.HealthPercent < PRSettings.Instance.Monk.DiffuseMagicPercentWW),
                         Spell.Cast("Dampen Harm", ret => TalentManager.HasTalent(14) && !Me.HasAura("Dampen Harm") && Me.HealthPercent < PRSettings.Instance.Monk.DampenHarmPercentWW)));
        }

        private static bool NeedHealthstone { get { return Me.HealthPercent < PRSettings.Instance.Monk.HealthstonePercent; } }

        #region Intergers

        #endregion       
        #region Booleans
        bool useTeBOnRoRo { get { return Me.HasAura(139120) && Me.GetAuraById(139120).TimeLeft <= TimeSpan.FromSeconds(2); } }

        #endregion

        #region Overrides of RotationBase

        public override string Name
        {
            get { return "Windwalker Monk"; }
        }

        public override string Revision
        {
            get { return "$Rev: 1729 $"; }
        }

        public override WoWSpec KeySpec
        {
            get { return WoWSpec.MonkWindwalker; }
        }

        public override Composite PVERotation
        {
            get
            {
                return new PrioritySelector(
                  new Decorator(ret => HotKeyManager.IsPaused, new ActionAlwaysSucceed()),
                  EncounterSpecific.HandleActionBarInterrupts(),
                  Item.HandleItems(),
                  new Decorator(ret=> PRSettings.Instance.Monk.TigerEyeBrewUsage!=TeBUsage.None, TigerEyeBrewHandle()),
                  Medic,         // Pop Trinkets, Drink potions...
                  Racials.UseRacials(),
                    //      Item.HandleItems(),   // Pop Trinkets, Drink potions...
                  Spell.InterruptSpellCasts(ret => Me.CurrentTarget),
                  new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Auto,
                                new PrioritySelector(
                                    new Decorator(ret => PRSettings.Instance.UseCooldowns && Me.CurrentTarget.IsBoss(), HandleCooldowns()),
                                    new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Monk.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                    // IsSpecialKey in auto mode will never be called? -- Millz
                                    new Decorator(ret => HotKeyManager.IsSpecialKey, SingleTargetWithOutFistsofFury()),
                                    new Decorator(ret => !HotKeyManager.IsSpecialKey, SingleTargetWithFistsofFury()))),
                 new Decorator(ret =>  !SpellManager.GlobalCooldown && HotkeySettings.Instance.ModeChoice == Mode.SemiAuto,
                                new PrioritySelector(
                                    new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                                    new Decorator(ret => PRSettings.Instance.UseAoEAbilities && Unit.AttackableMeleeUnits.Count() > PRSettings.Instance.Monk.AoECount, HandleAoeCombat()), //x => !x.IsBoss()
                                    new Decorator(ret => HotKeyManager.IsSpecialKey, SingleTargetWithOutFistsofFury()),
                                    new Decorator(ret => !HotKeyManager.IsSpecialKey, SingleTargetWithFistsofFury()))),
                 new Decorator(ret => HotkeySettings.Instance.ModeChoice == Mode.Hotkey,
                                new PrioritySelector(
                                    new Decorator(ret => HotKeyManager.IsAoe, HandleAoeCombat()),
                                    new Decorator(ret => HotKeyManager.IsCooldown, HandleCooldowns()),
                                    new Decorator(ret => !HotKeyManager.IsSpecialKey, SingleTargetWithOutFistsofFury()),
                                    new Decorator(ret => HotKeyManager.IsSpecialKey, SingleTargetWithFistsofFury()))),
                                    new Decorator(ret => !HotKeyManager.IsAoe, SingleTargetWithFistsofFury()));
            }
        }


        public override Composite PreCombat
        {
            get
            {
                return new Decorator(
                    ret => !Me.Mounted && !Me.IsDead && !Me.Combat && !Me.IsFlying && !Me.IsOnTransport && !Me.HasAura("Food") && !Me.HasAura("Drink"),
                    new PrioritySelector(
                        Spell.Cast("Legacy of the White Tiger", On => Me, ret => !Me.HasAura("Legacy of the White Tiger") && !Me.HasAura("Arcane Brilliance") && !Me.HasAura("Dalaran Brilliance")),
                        Spell.Cast("Legacy of the Emperor", On => Me, ret => !Me.HasAura("Legacy of the Emperor") && !Me.HasAura("Blessing of Kings") && !Me.HasAura("Mark of the Wild"))));
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
                    new Decorator(ret => Me.HealthPercent < 100 && PRSettings.Instance.EnableSelfHealing,
                        DefensiveCooldowns()));
            }
        }

        #endregion Overrides of RotationBase
    }
}