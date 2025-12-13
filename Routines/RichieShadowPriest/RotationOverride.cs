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

namespace RichieShadowPriestPvP
{
    public partial class Main
    {
        private static string RoutineName = "Richie's Shadow Priest PvP Combat Routine [Public Edition]";
        public static readonly bool IsProVersion = false;

        //Main Rotation
        #region MainRotation

        private static Composite MainRotation() {

            return new TracePrioritySelector("Main",
                // Hold
                new Decorator(ret => !Me.IsValid ||
                                     !StyxWoW.IsInWorld ||
                                     !Me.IsAlive ||
                                     !CombatLog.IsAttached ||
                                     Me.Stunned ||
                                     (Me.Mounted && TreeRoot.Current.Name != "BGBuddy") ||
                                     Me.IsFlying ||
                                     Me.HasAura("Food") ||
                                     Me.HasAura("Drink") ||
                                     Me.HasAura((int)SpellIDs.Dispersion) ||
                                     (Me.HasAura(SpellIDs.SpectralGuise) && (UnitManager.HealTarget == null || UnitManager.HealTarget.HealthPercent >= 40)) ||
                                     (UnitManager.Me.HasPendingSpell((int)SpellIDs.MassDispel) || UnitManager.Me.HasPendingSpell((int)SpellIDs.Psyfiend)
                                        || UnitManager.Me.HasPendingSpell((int)SpellIDs.AngelicFeather)),
                    new Action(ret => {
                        return RunStatus.Success;
                    })
                ),

                new Action(ret => {
                    using (var perf = PerfLogger.GetHelper("Orb Refresh"))
                        Orbs = Me.GetCurrentPower(WoWPowerType.ShadowOrbs);

                    if (!Burstmode && NextHotkeyAction == HotkeyAction.Burst) {
                        NextHotkeyAction = HotkeyAction.Nothing;
                        Burstmode = true;
                        LastBurstmodeActivated = DateTime.Now;
                        Logging.Write("Burst mode activated for 10 seconds!");
                    }

                    if (Burstmode && LastBurstmodeActivated.AddSeconds(10) < DateTime.Now) {
                        Burstmode = false;
                        Logging.Write("Burst mode deactivated!");
                    }

                    if (LastHotkeyPressed.AddSeconds(5) < DateTime.Now)
                        NextHotkeyAction = HotkeyAction.Nothing;

                    return RunStatus.Failure;
                }),

                new TraceDecorator("WillOfTheForsaken", ret => SPSettings.Instance.UseRacial && SPCoolDown.IsOff(SpellIDs.WillOfTheForsaken, 120) && Me.IsFearedOrCharmed(),
                    new Sequence(
                        new WaitContinue(TimeSpan.FromMilliseconds(500), ret => false, new Action(ret => RunStatus.Success)),
                        CastSpell(SpellIDs.WillOfTheForsaken, onGCD: false, needFacing: false)
                    )
                ),

                new TraceDecorator("RefilterUnitsForHeal", ret => !UnitManager.HealTarget.IsValidUnit(),
                    new Action(act => {
                        Logging.Write(System.Windows.Media.Colors.Red, "No Valid HealTarget. RefreshUnits in Main Rotation!");
                        UnitManager.RefilterUnitsForHeal();
                        SpellManager.StopCasting();

                        return RunStatus.Failure;
                    })
                ),

                CastSpell(SpellIDs.AngelicFeather, () => Me.Location, () => SPSettings.Instance.AngelicFeatherUsage > 1 && SPSettings.Instance.AngelicFeatherPriority == 0 && !Me.IsFalling && SPCoolDown.IsOff(SpellIDs.AngelicFeather, 10) && LastFeather.AddSeconds(SPSettings.Instance.AngelicFeatherDelay) < DateTime.Now && Me.IsMoving(100), additionalLogic: () => LastFeather = DateTime.Now, onGCD: true, needFacing: false, message: "High Priority"),

                CastSpell(SpellIDs.Levitate, GetMe, condition: () => SPSettings.Instance.CastLevitate && Me.MovementInfo.FallTime >= 1500 && 
                                                                      !Me.HasAura(SpellIDs.LevitateAura), needFacing: false, onGCD: true),

                new TraceDecorator("ASAP", ret => UnitManager.SelectASAPDamageTarget(),
                    new PrioritySelector(
                        CastSpell(SpellIDs.ShadowWordDeath, () => UnitManager.ASAPDamageTarget, condition: () => (UnitManager.ASAPDamageTarget.HealthPercent <= 20 || UnitManager.HasSWDGlyph.Value) && SPCoolDown.IsOff(SpellIDs.ShadowWordDeath, 8), needFacing: false, onGCD: true, ForceCast: true, message: "ASAP"),
                        CastSpell(SpellIDs.MindBlast, () => UnitManager.ASAPDamageTarget, condition: () => Me.GetAuraById((int)SpellIDs.DivineInsight_Shadow) != null, needFacing: true, onGCD: true, ForceCast: true, message: "ASAP-Instant"),
                        CastSpell(SpellIDs.MindSpike, () => UnitManager.ASAPDamageTarget, condition: () => Me.GetAuraById((int)SpellIDs.SurgeOfDarkness_1stack) != null, needFacing: true, onGCD: true, ForceCast: true, message: "ASAP-Instant"),
                        CastSpell(SpellIDs.DevouringPlague, () => UnitManager.ASAPDamageTarget, condition: () => SPSettings.Instance.AutoBurst && Orbs == 3, additionalLogic: () => Orbs = 0, needFacing: false, onGCD: true, ForceCast: true, message: "ASAP")
                    )
                ),

                new TraceDecorator("Defensive Cooldowns", ret => LastDefensiveCD.AddSeconds(SPSettings.Instance.DefensiveCDDelay) <= DateTime.Now && Me.HealthPercent < SPSettings.Instance.DefCDsBelow && Me.Combat,
                    new PrioritySelector(
                        UseHealthstone(),
                        CastSpell(SpellIDs.DespareatePrayer, unit: GetMe, condition: () => LastDefensiveCD.AddSeconds(SPSettings.Instance.DefensiveCDDelay) <= DateTime.Now && Me.HealthPercent < SPSettings.Instance.DesperatePrayer && SPCoolDown.IsOff(SpellIDs.DespareatePrayer, 120), additionalLogic: () => LastDefensiveCD = DateTime.Now, needFacing: false, onGCD: false),
                        CastSpell(SpellIDs.Dispersion, unit: GetMe, condition: () => LastDefensiveCD.AddSeconds(SPSettings.Instance.DefensiveCDDelay) <= DateTime.Now && Me.HealthPercent < SPSettings.Instance.DispersionHP && SPCoolDown.IsOff(SpellIDs.Dispersion, 105), additionalLogic: () => LastDefensiveCD = DateTime.Now, needFacing: false, onGCD: true, message: "Defensive"),
                        CastSpell(SpellIDs.VampiricEmbrace, unit: GetMe, condition: () => LastDefensiveCD.AddSeconds(SPSettings.Instance.DefensiveCDDelay) <= DateTime.Now && Me.HealthPercent < SPSettings.Instance.VampiricEmbracePercent && SPCoolDown.IsOff(SpellIDs.VampiricEmbrace, 180), additionalLogic: () => LastDefensiveCD = DateTime.Now, needFacing: false, onGCD: false)
                        
                    )
                ),

                new TraceDecorator("heal", ret => UnitManager.HealTarget != null &&
                    (UnitManager.HealTarget.HealthPercent <= SPSettings.Instance.InstantHealBelowHp || (UnitManager.HealTarget.HealthPercent <= SPSettings.Instance.CastedHealsBelowHp && !Me.IsMoving)),
                    new PrioritySelector(
                        new TraceDecorator("Burst-heal", ret => UnitManager.HealTarget.HealthPercent <= 30 && LastBurst.AddSeconds(5) <= DateTime.Now,
                            new PrioritySelector(
                                DPSTrinket("Burst-heal"),
                                CastSpell(SpellIDs.LifeBlood, unit: GetMe, condition: () => SPCoolDown.IsOff(SpellIDs.LifeBlood, 120), additionalLogic: () => LastBurst = DateTime.Now, needFacing: false, onGCD: false, message: "Burst-heal"),
                                SynapseSprings("Burst-heal"),
                                CastSpell(SpellIDs.Berserking, condition: () => SPSettings.Instance.UseRacial && SPCoolDown.IsOff(SpellIDs.Berserking, 180), additionalLogic: () => LastBurst = DateTime.Now, onGCD: false, needFacing: false, message: "Burst-heal"),
                                CastSpell(SpellIDs.PowerInfusion, condition: () => SPSettings.Instance.UsePowerInfusion && SPCoolDown.IsOff(SpellIDs.PowerInfusion, 120), additionalLogic: () => LastBurst = DateTime.Now, needFacing: false, onGCD: false, returnWhenCasted: RunStatus.Failure, message: "Burst-heal")
                            )),



                        Heal(SpellIDs.PowerWordShield, () => UnitManager.HealTarget.Combat && UnitManager.HealTarget.HealthPercent <= SPSettings.Instance.InstantHealBelowHp &&
                                                             !UnitManager.HealTarget.HasAura(SpellIDs.WeakenedSoul) &&
                                                             SPCoolDown.IsOff(SpellIDs.PowerWordShield, 6)),

                        CastSpell(SpellIDs.PrayerOfMending, unit: GetHealTarget, condition: () => UnitManager.HealTarget.Combat &&
                                                                                                   UnitManager.HealTarget.HealthPercent <= SPSettings.Instance.InstantHealBelowHp &&
                                                                                                   (UnitManager.HealTarget.HealthPercent > SPSettings.Instance.CastedHealsBelowHp || Me.IsMoving) &&
                                                                                                   !UnitManager.HealTarget.HasMyAura(SpellIDs.PrayerOfMending, 0) &&
                                                                                                   SPCoolDown.IsOff(SpellIDs.PrayerOfMending, 10), needFacing: false, onGCD: true),

                        Heal(SpellIDs.Renew, () => UnitManager.HealTarget.AuraTimeLeft(SpellIDs.Renew) < 1500 &&
                                                    UnitManager.HealTarget.HealthPercent <= SPSettings.Instance.InstantHealBelowHp &&
                                                    (UnitManager.HealTarget.HealthPercent > SPSettings.Instance.CastedHealsBelowHp || Me.IsMoving))
                                                    
                                                    ,

            #region Instant, lvl90 heals

                        Heal(SpellIDs.DivineStar_Holy, () => (UnitManager.HealTarget.Distance2D <= 30 || Me.IsMoving) && UnitManager.HealTarget.HealthPercent <= SPSettings.Instance.InstantHealBelowHp && SPCoolDown.IsOff(SpellIDs.DivineStar_Holy, 15)),

                        Heal(SpellIDs.Cascade_Holy, () => UnitManager.GetInjuredCount(80) >= SPSettings.Instance.Lvl90MinUnits && SPCoolDown.IsOff(SpellIDs.Cascade_Holy, 25) && (!SPSettings.Instance.Lvl90SafeCheck || UnitManager.IsHaloSafe())),

                        CastSpell(SpellIDs.Halo_Shadow, GetMe, condition: () => UnitManager.GetInjuredCount(80) >= SPSettings.Instance.Lvl90MinUnits && SPCoolDown.IsOff(SpellIDs.Halo_Shadow, 40) && (!SPSettings.Instance.Lvl90SafeCheck || UnitManager.IsHaloSafe()), needFacing: false, onGCD: true)
                        

            #endregion

                    )
                ),
                CastSpell(SpellIDs.Shadowform, GetMe, () => !Me.HasAura(SpellIDs.Shadowform), needFacing: false, onGCD: true),

                new TraceDecorator("Damage", ret => Me.CurrentTarget.IsEnemy() &&
                    !Me.CurrentTarget.IsInvulnerableSpell() &&
                    Me.CurrentTarget.InLineOfSight() &&
                    Me.CurrentTarget.Distance <= 40,
                    new PrioritySelector(
                        new TraceDecorator("Burst", ret => (Burstmode || (SPSettings.Instance.AutoBurst && Orbs == 3)) && LastBurst.AddSeconds(5) <= DateTime.Now,
                            new PrioritySelector(
                                DPSTrinket("Burst"),
                                CastSpell(SpellIDs.LifeBlood, unit: GetMe, condition: () => SPCoolDown.IsOff(SpellIDs.LifeBlood, 120), additionalLogic: () => LastBurst = DateTime.Now, needFacing: false, onGCD: false, message: "Burst"),
                                SynapseSprings("Burst"),
                                CastSpell(SpellIDs.Berserking, condition: () => SPSettings.Instance.UseRacial && SPCoolDown.IsOff(SpellIDs.Berserking, 180), additionalLogic: () => LastBurst = DateTime.Now, onGCD: false, needFacing: false, message: "Burst"),
                                CastSpell(SpellIDs.PowerInfusion, condition: () => SPSettings.Instance.UsePowerInfusion && SPCoolDown.IsOff(SpellIDs.PowerInfusion, 120), additionalLogic: () => LastBurst = DateTime.Now, needFacing: false, onGCD: false, returnWhenCasted: RunStatus.Failure, message: "Burst")
                            )
                        ),
                        CastSpell(SpellIDs.ShadowWordDeath, () => Me.CurrentTarget, condition: () => Me.CurrentTarget.HealthPercent <= 20 && SPCoolDown.IsOff(SpellIDs.ShadowWordDeath, 8), needFacing: false, onGCD: true),
                        CastSpell(SpellIDs.ShadowWordPain, () => Me.CurrentTarget, condition: () => (Burstmode || (SPSettings.Instance.AutoBurst && Orbs == 3)) && TalentManager.Has(Talents.SolaceAndInsanity) && Me.CurrentTarget.MyAuraTimeLeft(SpellIDs.ShadowWordPain) < 7500, needFacing: false, onGCD: true, message: "Refresh before insanity"),
                        CastSpell(SpellIDs.VampiricTouch, () => Me.CurrentTarget, condition: () => (Burstmode || (SPSettings.Instance.AutoBurst && Orbs == 3)) && TalentManager.Has(Talents.SolaceAndInsanity) && LastVTCastTime.AddSeconds(2) < DateTime.Now && !Me.IsMoving && Me.CurrentTarget.MyAuraTimeLeft(SpellIDs.VampiricTouch) < 7500, additionalLogic: () => LastVTCastTime = DateTime.Now, needFacing: false, onGCD: true, message: "Refresh before insanity"),
                        CastSpell(SpellIDs.Halo_Shadow, condition: () => (Burstmode || (SPSettings.Instance.AutoBurst && Orbs == 3)) && Me.CurrentTarget.Distance2D < 30 && SPCoolDown.IsOff(SpellIDs.Halo_Shadow, 40) && Me.CurrentTarget.Distance <= 30 && (!SPSettings.Instance.Lvl90SafeCheck || UnitManager.IsHaloSafe()), onGCD: true, needFacing: false, message: "Burst"),
                        CastSpell(SpellIDs.Cascade_Shadow, () => Me.CurrentTarget, condition: () => (Burstmode || (SPSettings.Instance.AutoBurst && Orbs == 3)) && Me.CurrentTarget.Distance2D < 40 && SPCoolDown.IsOff(SpellIDs.Cascade_Shadow, 25) && UnitManager.NumberOfEnemyInRange(30, false) >= SPSettings.Instance.Lvl90MinUnits && (!SPSettings.Instance.Lvl90SafeCheck || UnitManager.IsHaloSafe()), onGCD: true, needFacing: false, message: "Burst"),
                        CastSpell(SpellIDs.DivineStar_Shadow, () => Me.CurrentTarget, condition: () => (Burstmode || (SPSettings.Instance.AutoBurst && Orbs == 3)) && Me.CurrentTarget.Distance2D < 24 && SPCoolDown.IsOff(SpellIDs.DivineStar_Shadow, 15), onGCD: true, needFacing: false, message: "Burst"),
                        CastSpell(SpellIDs.DevouringPlague, () => Me.CurrentTarget, condition: () => (Burstmode && Orbs >= 1 && Me.CurrentTarget.AuraTimeLeft(SpellIDs.DevouringPlague) == 0) || (SPSettings.Instance.AutoBurst && Orbs == 3), additionalLogic: () => Orbs = 0, needFacing: false, onGCD: true),
                        CastSpell(SpellIDs.MindBlast, () => Me.CurrentTarget, condition: () => (!SPSettings.Instance.AutoBurst || Orbs <= 2) && Me.GetAuraById((int)SpellIDs.DivineInsight_Shadow) != null, needFacing: true, onGCD: true, message: "Instant"),
                        CastSpell(SpellIDs.MindBlast, () => Me.CurrentTarget, condition: () => (!SPSettings.Instance.AutoBurst || Orbs <= 2) && !Me.IsMoving && SPCoolDown.IsOff(SpellIDs.MindBlast, 8), needFacing: true, onGCD: true),
                        CastSpell(SpellIDs.MindFlay, () => Me.CurrentTarget, condition: () => !Me.IsMoving && UnitManager.Me.IsChanneling && UnitManager.Me.ChanneledCastingSpellId == (int)SpellIDs.MindFlay_Insanity && UnitManager.Me.CurrentChannelTimeLeft.TotalMilliseconds <= 1800 && Me.CurrentTarget.AuraTimeLeft(SpellIDs.DevouringPlague).Between(100, 500) && LastInsanityStopCast.AddSeconds(5) <= DateTime.Now, needFacing: true, onGCD: true, ForceCast: true, additionalLogic: () => LastInsanityStopCast = DateTime.Now, message: "FORCED-INSANITY!!!"),
                        CastSpell(SpellIDs.MindFlay, () => Me.CurrentTarget, condition: () => !Me.IsMoving && TalentManager.Has(Talents.SolaceAndInsanity) && Me.CurrentTarget.AuraTimeLeft(SpellIDs.DevouringPlague) > 0, needFacing: true, onGCD: true, message: "INSANITY!!!"),
                        CastSpell(SpellIDs.MindSpike, () => Me.CurrentTarget, condition: () => Me.GetAuraById((int)SpellIDs.SurgeOfDarkness_2stacks) != null, needFacing: true, onGCD: true, message: "Instant - 2 stacks"),
                        CastSpell(SpellIDs.ShadowWordPain, () => Me.CurrentTarget, condition: () => Me.CurrentTarget.MyAuraTimeLeft(SpellIDs.ShadowWordPain) < SPSettings.Instance.SWPRefresh, needFacing: false, onGCD: true, message: "target"),
                        CastSpell(SpellIDs.VampiricTouch, () => Me.CurrentTarget, condition: () => LastVTCastTime.AddSeconds(2) < DateTime.Now && !Me.IsMoving && Me.CurrentTarget.MyAuraTimeLeft(SpellIDs.VampiricTouch) < SPSettings.Instance.VampiricTouchRefresh, additionalLogic: () => LastVTCastTime = DateTime.Now, needFacing: false, onGCD: true, message: "target"),
                        CastSpell(SpellIDs.MindSpike, () => Me.CurrentTarget, condition: () => Me.GetAuraById((int)SpellIDs.SurgeOfDarkness_1stack) != null, needFacing: true, onGCD: true, message: "Instant - 1 stack"),
                        CastSpell(SpellIDs.MindFlay, () => Me.CurrentTarget, condition: () => !Me.IsMoving && (!SPSettings.Instance.Multidot || UnitManager.MultidotTarget == null) && UnitManager.Me.ChanneledCastingSpellId != 15407, needFacing: true, onGCD: true)
                    )
                ),

                new Decorator(ret => SPSettings.Instance.Multidot &&
                    Me.ManaPercent >= SPSettings.Instance.AoEMana &&
                    UnitManager.MultidotTarget.IsValidUnit(),
                    new PrioritySelector(
                        CastSpell(SpellIDs.ShadowWordPain, () => UnitManager.MultidotTarget, condition: () => UnitManager.MultidotTarget.MyAuraTimeLeft(SpellIDs.ShadowWordPain) < SPSettings.Instance.SWPRefresh, needFacing: false, onGCD: true, message: "Multi-dotting"),
                        CastSpell(SpellIDs.VampiricTouch, () => UnitManager.MultidotTarget, condition: () => LastVTCastTime.AddSeconds(2) < DateTime.Now && !Me.IsMoving && UnitManager.MultidotTarget.MyAuraTimeLeft(SpellIDs.VampiricTouch) < SPSettings.Instance.VampiricTouchRefresh, additionalLogic: () => LastVTCastTime = DateTime.Now, needFacing: false, onGCD: true, message: "Multi-dotting")                        
                    )
                ),

            #region Misc Buffs

                CastSpell(SpellIDs.InnerFire, () => !Me.HasAura(SpellIDs.InnerFire), false, true),

                CastSpell(SpellIDs.AngelicFeather, () => Me.Location, () => SPSettings.Instance.AngelicFeatherUsage > 1 && SPSettings.Instance.AngelicFeatherPriority == 1 && !Me.IsFalling && LastFeather.AddSeconds(SPSettings.Instance.AngelicFeatherDelay) < DateTime.Now && SPCoolDown.IsOff(SpellIDs.AngelicFeather, 10) && Me.IsMoving(100), additionalLogic: () => LastFeather = DateTime.Now, onGCD: true, needFacing: false, message: "Low Priority"),

                CastSpell(SpellIDs.FearWard, () => SPSettings.Instance.FearWardSelf &&
                                                           !Me.HasAura(SpellIDs.FearWard) &&
                                                           SPCoolDown.IsOff(SpellIDs.FearWard, FearWardCooldown), false, true),

                CastSpell(SpellIDs.PowerWordFortitude, () => SPSettings.Instance.PowerWordFortitude && !Me.HasAura(SpellIDs.PowerWordFortitude) &&
                                                                     !Me.HasAura(SpellIDs.DarkIntent) && !Me.HasAura(SpellIDs.CommandingShout), false, true)

            #endregion
            );
        }

        #endregion
    }
}