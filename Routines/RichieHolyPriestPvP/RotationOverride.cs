using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace RichieHolyPriestPvP
{
    public partial class Main
    {
        private static string RoutineName = "Richie's Holy Priest PvP Combat Routine [Public Edition]";
        public static readonly bool IsProVersion = false;

        //Main Rotation
        #region MainRotation

        private static Composite MainRotation()
        {

            return new TracePrioritySelector("Main",
                // Hold
                new Decorator(ret => !Me.IsValid || Me.Mounted ||
                                     !StyxWoW.IsInWorld ||
                                     !Me.IsAlive ||
                                     Me.Stunned || Me.Fleeing ||
                                     (Me.Mounted && TreeRoot.Current.Name != "BGBuddy") ||
                                     Me.IsFlying ||
                                     Me.HasAura("Food") ||
                                     Me.HasAura("Drink") ||
                                     (Me.HasAura(SpellIDs.SpectralGuise) && (UnitManager.HealTarget == null || UnitManager.HealTarget.HealthPercent >= 40)) ||
                                     Me.IsChanneling ||
                                     (Me.HasPendingSpell(SpellIDs.Lightwell) || 
                                        Me.HasPendingSpell(SpellIDs.MassDispel) || 
                                        Me.HasPendingSpell(SpellIDs.Resurrection) || 
                                        Me.HasPendingSpell(SpellIDs.AngelicFeather)) ||
                                    !CombatLog.IsAttached,
                    new Action(ret => { return RunStatus.Success; })
                ),

                //hold for dominate mind
                new Decorator(ret => HolySettings.Instance.DontCancelDominateMind &&
                                     (DMcast.AddMilliseconds(DMDelayMs + HolyCoolDown.Latency) > DateTime.Now ||
                                     (Me.IsCasting && Me.CastingSpellId == (int)SpellIDs.DominateMind)),
                    new Action(ret =>
                    {
                        if (Me.IsCasting && Me.CastingSpellId == (int)SpellIDs.DominateMind)
                            DMcast = DateTime.Now;
                        return RunStatus.Success;
                    })
                ),

                //Clear Target if dead and still in combat
                new TraceAction("MiscSetups", ret =>
                {
                    if (Me.CurrentTarget != null && (!Me.CurrentTarget.IsAlive || !Me.CurrentTarget.IsValid) && Me.Combat)
                        Lua.DoString("RunMacroText(\"/cleartarget\")");

                    return RunStatus.Failure;
                }),

                CastSpell(SpellIDs.Levitate, GetMe, condition: () => HolySettings.Instance.CastLevitate && Me.MovementInfo.FallTime >= HolySettings.Instance.CastLevitateAfterMs &&
                                                                      !Me.HasAura(SpellIDs.LevitateAura), needFacing: false, onGCD: true),

                new TraceDecorator("WillOfTheForsaken", ret => HolySettings.Instance.UseRacial && HolyCoolDown.IsOff(SpellIDs.WillOfTheForsaken, 120) && Me.IsFearedOrCharmed(),
                    new Sequence(
                        new WaitContinue(TimeSpan.FromMilliseconds(500), ret => false, new Action(ret => RunStatus.Success)),
                        CastSpell(SpellIDs.WillOfTheForsaken, onGCD: false, needFacing: false)
                   )),

                // Low priority Purify
                CastSpell(SpellIDs.Purify, () => UnitManager.PurifyLowPrioTarget, condition: () => HolySettings.Instance.DispelLowPrio && !Me.HasSpiritOfRedemption() &&
                                                                                            UnitManager.HealTarget.HealthPercent >= 50 && !Me.HasSpiritOfRedemption() &&
                                                                                            UnitManager.GetPurifyLowPrioTarget() &&
                                                                                            UnitManager.PurifyLowPrioTargetDebuffCount > HolySettings.Instance.PurifyWhenLowPrioDebuffCount &&
                                                                                            HolyCoolDown.IsOff(SpellIDs.Purify, 8), onGCD: true, needFacing: false),

                CastSpell(SpellIDs.Fade, GetMe, condition: () => HolySettings.Instance.FadeAuto && Me.Combat && TalentManager.Has(Talents.Phantasm) &&
                                                                  HolyCoolDown.IsOff(SpellIDs.Fade, 30) && Me.ShouldDebuffRootOrSnare(), needFacing: false, onGCD: false),

                //switch between inner will/fire
                CastSpell(SpellIDs.InnerWill, GetMe, () => (HolySettings.Instance.PreferredBuff == 2 || (HolySettings.Instance.PreferredBuff == 0 && UnitManager.NumberOfMeleeTargetingMe() == 0)) &&
                                                            !Me.HasSpiritOfRedemption() && !Me.HasAura(SpellIDs.InnerWill), false, true),
                CastSpell(SpellIDs.InnerFire, GetMe, () => (HolySettings.Instance.PreferredBuff == 1 || (HolySettings.Instance.PreferredBuff == 0 && UnitManager.NumberOfMeleeTargetingMe() >= 1)) &&
                                                            !Me.HasSpiritOfRedemption() && Me.Combat && !Me.HasAura(SpellIDs.InnerFire), false, true),

                CastSpell(SpellIDs.ChakraSerenity, GetMe, () => !Me.HasSpiritOfRedemption() && !Me.HasAura(SpellIDs.ChakraSerenity) && HolyCoolDown.IsOff(SpellIDs.ChakraSerenity, ChakraCooldown), false, false),

                new TraceDecorator("RefilterUnitsForHeal", ret => !UnitManager.HealTarget.IsValidUnit(),
                    new Action(act =>
                    {
                        Logging.Write(System.Windows.Media.Colors.Red, "No Valid HealTarget. RefreshUnits in Main Rotation!");
                        UnitManager.RefilterUnitsForHeal();
                        SpellManager.StopCasting();

                        return RunStatus.Failure;
                    })),

                CastSpell(SpellIDs.Shadowfiend, () => UnitManager.GetFirstValidEnemy((u) => !UnitManager.DebuffCCBreakonDamage(u)),
                                     condition: () => HolySettings.Instance.UseShadowFiend && Me.ManaPercent < HolySettings.Instance.UseShadowFiendIfMana && HolyCoolDown.IsOff(SpellIDs.Shadowfiend, 180) &&
                                                      !TalentManager.Has(Talents.Mindbender) && UnitManager.GetFirstValidEnemy((u) => !UnitManager.DebuffCCBreakonDamage(u)) != null, needFacing: false, onGCD: true),

                CastSpell(SpellIDs.Mindbender, () => UnitManager.GetFirstValidEnemy((u) => !UnitManager.DebuffCCBreakonDamage(u)),
                                    condition: () => HolySettings.Instance.UseShadowFiend && Me.ManaPercent < HolySettings.Instance.UseShadowFiendIfMana && HolyCoolDown.IsOff(SpellIDs.Mindbender, 60) &&
                                                      UnitManager.GetFirstValidEnemy((u) => !UnitManager.DebuffCCBreakonDamage(u)) != null, needFacing: false, onGCD: true),

                new TraceDecorator("heal", ret => UnitManager.HealTarget != null,
                    new PrioritySelector(
                        new TraceDecorator("Burst", ret => UnitManager.HealTarget.HealthPercent <= 30 && LastBurst.AddSeconds(5) <= DateTime.Now && !Me.HasSpiritOfRedemption(),
                            new PrioritySelector(
                                DPSTrinket("Burst"),
                                CastSpell(SpellIDs.LifeBlood, unit: GetMe, condition: () => HolyCoolDown.IsOff(SpellIDs.LifeBlood, 120), additionalLogic: () => LastBurst = DateTime.Now, needFacing: false, onGCD: false, message: "Burst"),
                                SynapseSprings("Burst"),
                                CastSpell(SpellIDs.Berserking, condition: () => HolySettings.Instance.UseRacial && HolyCoolDown.IsOff(SpellIDs.Berserking, 180), additionalLogic: () => LastBurst = DateTime.Now, onGCD: false, needFacing: false, message: "Burst"),
                                CastSpell(SpellIDs.PowerInfusion, condition: () => HolySettings.Instance.UsePowerInfusion && HolyCoolDown.IsOff(SpellIDs.PowerInfusion, 120), additionalLogic: () => LastBurst = DateTime.Now, needFacing: false, onGCD: false,
                                                                  returnWhenCasted: RunStatus.Failure, message: "Burst")
                            )),

                        new Decorator(ret => Me.HasSpiritOfRedemption(),
                                new PrioritySelector(
                                    Heal(SpellIDs.GreaterHeal, () => UnitManager.HealTarget.HealthPercent <= 95 && Me.HasAura(SpellIDs.Serendipity, 2)),
                                    Heal(SpellIDs.FlashHeal, () => UnitManager.HealTarget.HealthPercent <= 95),
                                    Heal(SpellIDs.Renew, () => UnitManager.HealTarget.HealthPercent > 95 && UnitManager.HealTarget.HealthPercent < 99 && UnitManager.HealTarget.AuraTimeLeft(SpellIDs.Renew) < 1500)
                                )),

                        CastSpell(SpellIDs.VoidShift, GetHealTarget, condition: () => HolySettings.Instance.UseVoidShiftToHeal && UnitManager.HealTarget.HealthPercent <= HolySettings.Instance.VoidShiftBelowHp &&
                                                                                      !UnitManager.HealTarget.IsMe && UnitManager.HealTarget.IsPlayer && UnitManager.HealTarget.Combat && HolyCoolDown.IsOff(SpellIDs.VoidShift, 300) &&
                                                                                      UnitManager.NumberOfMeleeTargetingMe() == 0, needFacing: true, onGCD: true),

                        CastSpell(SpellIDs.GuardianSpirit, GetHealTarget, condition: () => UnitManager.HealTarget.HealthPercent < HolySettings.Instance.UseGuardianSpiritBelowHealth &&
                                                                                                    HolyCoolDown.IsOff(SpellIDs.GuardianSpirit, 180), needFacing: false, onGCD: false),

                        Heal(SpellIDs.PowerWordShield, () => UnitManager.HealTarget.Combat && UnitManager.HealTarget.HealthPercent <= HolySettings.Instance.PowerWordShieldHp &&
                                                             !UnitManager.HealTarget.HasAura(SpellIDs.WeakenedSoul) &&
                                                             HolyCoolDown.IsOff(SpellIDs.PowerWordShield, 6)),

                        Heal(SpellIDs.HolyWordSerenity, () => UnitManager.HealTarget.HealthPercent <= 90 && HolyCoolDown.IsOff(SpellIDs.HolyWordSerenity, 10) && Me.HasAura(SpellIDs.ChakraSerenity)),

                        CastSpell(SpellIDs.PrayerOfMending, unit: GetHealTarget, condition: () => UnitManager.HealTarget.Combat &&
                                                                                                   UnitManager.HealTarget.HealthPercent <= HolySettings.Instance.PrayerOfMendingHp &&
                                                                                                   (UnitManager.HealTarget.HealthPercent > HolySettings.Instance.FlashHealHp || Me.IsMoving) &&
                                                                                                   !UnitManager.HealTarget.HasMyAura(SpellIDs.PrayerOfMending, 0) &&
                                                                                                   (HolyCoolDown.IsOff(SpellIDs.PrayerOfMending, 10) || (Me.HasAura(SpellIDs.DivineInsight) && !HolyCoolDown.SkipNextPoMCD)),
                                                                                            additionalLogic: () => HolyCoolDown.SkipNextPoMCD = Me.HasAura(SpellIDs.DivineInsight), needFacing: false, onGCD: true),

                        Heal(SpellIDs.FlashHeal, () => (UnitManager.HealTarget.HealthPercent <= HolySettings.Instance.InstantFlashHealBelowHP && Me.HasAura(SpellIDs.SurgeOfLight)) ||
                                                                        Me.MyAuraTimeLeft(SpellIDs.SurgeOfLight).Between(200, 2000), message: "Instant"),

                        Heal(SpellIDs.Renew, () => UnitManager.HealTarget.AuraTimeLeft(SpellIDs.Renew) < 1500 &&
                                                    UnitManager.HealTarget.HealthPercent <= HolySettings.Instance.RenewHp &&
                                                    (UnitManager.HealTarget.HealthPercent > HolySettings.Instance.FlashHealHp || Me.IsMoving)),

                        #region Instant, lvl90 heals

                        Heal(SpellIDs.DivineStar, () => (UnitManager.HealTarget.Distance2D <= 30 || Me.IsMoving) && UnitManager.HealTarget.HealthPercent <= HolySettings.Instance.DivineStarHP && HolyCoolDown.IsOff(SpellIDs.DivineStar, 15)),

                        Heal(SpellIDs.Cascade, () => UnitManager.GetInjuredCount(HolySettings.InjuredPercent) >= HolySettings.Instance.MinInjuredCountToCastCascadeHalo && HolyCoolDown.IsOff(SpellIDs.Cascade, 25)),

                        CastSpell(SpellIDs.Halo, GetMe, condition: () => UnitManager.GetInjuredCount(HolySettings.InjuredPercent) >= HolySettings.Instance.MinInjuredCountToCastCascadeHalo && HolyCoolDown.IsOff(SpellIDs.Halo, 40), needFacing: false, onGCD: true),

                        #endregion

                        CastSpell(SpellIDs.CircleOfHealing, unit: () => UnitManager.GetBestTargetForCircleOfHealing(),
                                                            condition: () => HolyCoolDown.IsOff(SpellIDs.CircleOfHealing, 10) && UnitManager.GetBestTargetForCircleOfHealing() != null,
                                                            needFacing: false, onGCD: true),

                        new Decorator(ret => (HolyCoolDown.IsOff(SpellIDs.ChakraSanctuary, ChakraCooldown) || Me.HasAura(SpellIDs.ChakraSanctuary)) && 
                                             HolyCoolDown.IsOff(SpellIDs.HolyWordSanctuary, 40) && 
                                             UnitManager.GetBestTargetForHolyWordSanctuary() != null,
                            new Sequence(
                                CastSpell(SpellIDs.ChakraSanctuary, () => Me, condition: () => !Me.HasAura(SpellIDs.ChakraSanctuary), needFacing: false, onGCD: true),
                                new WaitContinue(TimeSpan.FromMilliseconds(500), ret => Me.HasAura(SpellIDs.ChakraSanctuary), new Action(ret => RunStatus.Success)),
                                CastSpell(SpellIDs.HolyWordSanctuary, location: () => UnitManager.GetBestTargetForHolyWordSanctuary().Location,
                                                                      condition: () => UnitManager.GetBestTargetForHolyWordSanctuary().IsValidUnit(),
                                                                      needFacing: true, onGCD: false)
                            )),

                        Heal(SpellIDs.GreaterHeal, () => !Me.IsMoving && UnitManager.HealTarget.HealthPercent <= HolySettings.Instance.FlashHealHp && Me.HasAura(SpellIDs.Serendipity, 2)),

                        Heal(SpellIDs.BindingHeal, () => !Me.IsMoving && !UnitManager.HealTarget.IsMe && UnitManager.HealTarget.HealthPercent <= HolySettings.Instance.FlashHealHp && 
                                                                                                         Me.HealthPercent < HolySettings.Instance.UseBindingHealIfMyHP),

                        Heal(SpellIDs.FlashHeal, () => !Me.IsMoving && UnitManager.HealTarget.HealthPercent <= HolySettings.Instance.FlashHealHp),

                        CastSpell(SpellIDs.LeapOfFaith, GetHealTarget, condition: () => UnitManager.HealTarget.HealthPercent < HolySettings.Instance.UseLeapOfFaitHP &&
                                                                                        HolyCoolDown.IsOff(SpellIDs.LeapOfFaith, 90), needFacing: false, onGCD: true)

                    )
                ),

                #region Misc Buffs

                CastSpell(SpellIDs.AngelicFeather, GetMe, () => Me.Location, () => HolySettings.Instance.UseAngelicFeather && !Me.IsFalling && Me.MovementInfo.RunSpeed >= 4.5 &&
                                                                                   LastFeather.AddSeconds(HolySettings.Instance.AngelicFeatherDelay) < DateTime.Now &&
                                                                                   Me.IsMoving(100), () => { LastFeather = DateTime.Now; }, true, false),
 
                CastSpell(SpellIDs.FearWard, GetMe, () => HolySettings.Instance.FearWardSelf &&
                                                           !Me.HasAura(SpellIDs.FearWard) &&
                                                           HolyCoolDown.IsOff(SpellIDs.FearWard, FearWardCooldown) &&
                                                           !Me.HasSpiritOfRedemption(), false, true),

                CastSpell(SpellIDs.PowerWordFortitude, GetMe, () => HolySettings.Instance.PowerWordFortitude && !Me.HasAura(SpellIDs.PowerWordFortitude) &&
                                                                     !Me.HasSpiritOfRedemption() && !Me.HasAura(SpellIDs.DarkIntent) && !Me.HasAura(SpellIDs.CommandingShout), false, true)

                #endregion
            );
        }

        #endregion
    }
}