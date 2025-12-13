using System;
using System.Linq;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace RichieDiscPriestPvP {

    public partial class Main {

        //Damage

        /*        #region  Shadow Word: Death

        private static Composite SWD()
        {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.SWD &&
                Me.CurrentTarget.HealthPercent <= 20 &&
                Me.HealthPercent >= 10 &&
                SpellManager.HasSpell("Shadow Word: Death") &&
                SpellManager.Spells["Shadow Word: Death"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                GetSpellCooldown("Shadow Word: Death").TotalMilliseconds <= MyLatency,                
                new Action(delegate
                    {                        
                        CastSpell("Shadow Word: Death", Me.CurrentTarget, "Target");
                    })
                );
        }

        #endregion

        #region  Shadow Word: Pain

        private static Composite ShadowWordPain()
        {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.SWP &&
                SpellManager.HasSpell("Shadow Word: Pain") &&
                MyAuraTimeLeft("Shadow Word: Pain", Me.CurrentTarget) <= 3000,
                new Action(delegate { CastSpell("Shadow Word: Pain", Me.CurrentTarget, "Normal"); })
                );
        }

        #endregion

        #region  Shadow Word: Pain Multi Target

        private static Composite ShadowWordPainMulti()
        {
            return new Decorator(
                ret =>
                Me.ManaPercent >= DiscSettings.Instance.ManaSWP &&
                SpellManager.HasSpell("Shadow Word: Pain") &&
                GetUnitSWP() &&
                !CastingorGCDL(),
                new Action(delegate
                    {
                        CastSpell("Shadow Word: Pain", SWPTarget, "Multi");
                        //SWPTarget = null;
                    })
                );
        }

        #endregion*/

        //Utility

        #region  Fade

        private static Composite Fade()
        {
            return new Decorator(
                ret =>
                DiscSettings.Instance.FadeAuto &&
                Me.CurrentMana >= Manacosts.Instance.Fade &&
                SpellManager.HasSpell("Fade") &&
                SpellManager.Spells["Fade"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                SpellManager.HasSpell("Phantasm") &&                
                (Me.MovementInfo.RunSpeed < 4.5 || DebuffRootOrSnare(Me)) &&
                LastCastSpell != "Fade" &&
                Me.Combat &&
                !Casting(),
                new Action(
                    delegate
                    {
                        CastSpell("Fade", Me);
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region  Power Word: Fortitude

        private static Composite PowerWordFortitude()
        {
            return new Decorator(
                ret =>
                DiscSettings.Instance.PowerWordFortitude &&
                Me.CurrentMana >= Manacosts.Instance.PWF &&
                LastCastSpell != "Power Word: Fortitude" &&
                !CastingorGCDL() &&
                !Me.HasAura("Power Word: Fortitude") &&
                !Me.HasAura("Dark Intent") &&
                !Me.HasAura("Commanding Shout"),
                new Action(delegate { 
                    CastSpell("Power Word: Fortitude", Me);
                    return RunStatus.Success;
                })
                );
        }

        #endregion

        #region  Inner Fire

        private static Composite InnerFire()
        {
            return new Decorator(
                ret =>
                LastCastSpell != "Inner Fire" &&
                !CastingorGCDL() &&
                !Me.HasAura("Inner Fire"),
                new Action(delegate { 
                    CastSpell("Inner Fire", Me); 
                    return RunStatus.Success;
                })
            );
        }

        #endregion

        #region  Fear Ward self

        private static Composite FearWardSelf()
        {
            return new Decorator(
                ret =>
                DiscSettings.Instance.FearWardSelf &&
                !Me.Mounted &&
                Me.CurrentMana >= Manacosts.Instance.FearWard &&
                !CastingorGCDL() &&
                SpellManager.HasSpell("Fear Ward") &&
                SpellManager.Spells["Fear Ward"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                    //LastCastSpell != "Fear Ward" &&                
                !Me.HasAura("Fear Ward"),
                new Action(delegate {
                    CastSpell("Fear Ward", Me);
                    return RunStatus.Success;
                })
            );
        }

        #endregion

        #region  Will of the Forsaken

        private static Composite WotF()
        {
            return new Decorator(
                ret =>
                DiscSettings.Instance.UseRacial &&
                SpellManager.HasSpell("Will of the Forsaken") &&
                SpellManager.Spells["Will of the Forsaken"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                IsFearedOrCharmed(Me),
                new Action(
                    delegate { 
                        CastSpell("Will of the Forsaken", Me);
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region  Shackle

        private static Composite Shackle()
        {
            return new Decorator(
                ret =>
                DiscSettings.Instance.Shackle &&
                !CastingorGCDL() &&
                SpellManager.HasSpell("Shackle Undead") &&
                SpellManager.Spells["Shackle Undead"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                GetShackleTarget() &&
                HealTarget.HealthPercent >= 40,
                new Action(
                    delegate
                    {
                        CastSpell("Shackle Undead", ShackleTarget);
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region Power Word: Barrier

        private static Composite Barrier() {

            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.Barrier &&
                //Healable(HealTarget) &&
                HealTarget != null &&
                HealTarget.IsValid &&
                HealTarget.HealthPercent <= DiscSettings.Instance.barrierHp &&
                LastLifeSaverCDPopped.AddSeconds(LifeSaverCDDelay) <= DateTime.Now &&
                //!HealTarget.HasAura("Pain Suppression") &&
                SpellManager.HasSpell("Power Word: Barrier") &&
                SpellManager.Spells["Power Word: Barrier"].CooldownTimeLeft.TotalMilliseconds <= MyLatency,
                new Action(
                    delegate {
                        CastSpell("Power Word: Barrier", Me, HealTarget.Location);
                        LastLifeSaverCDPopped = DateTime.Now;
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region  Pain Suppression

        private static Composite PainSuppression() {

            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.PainSuppression &&
                HealTarget.HealthPercent <= DiscSettings.Instance.painSuppressionHp &&
                LastLifeSaverCDPopped.AddSeconds(LifeSaverCDDelay) <= DateTime.Now &&
                //Healable(HealTarget) &&
                //!HealTarget.HasAura("Power Word: Barrier") &&
                SpellManager.HasSpell("Pain Suppression") &&
                SpellManager.Spells["Pain Suppression"].CooldownTimeLeft.TotalMilliseconds <= MyLatency,
                new Action(
                    delegate {
                        CastSpell("Pain Suppression", HealTarget);
                        LastLifeSaverCDPopped = DateTime.Now;
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region  Purify

        private static Composite PurifyLowPrio() {

            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.Purify &&
                Me.CurrentMap.IsArena &&
                SpellManager.HasSpell("Purify") &&
                SpellManager.Spells["Purify"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                !CastingorGCDL() &&                
                GetPurifyLowPrioTarget() &&                
                PurifyLowPrioTarget != null &&                
                PurifyLowPrioTarget.InLineOfSpellSight &&
                ((HealTarget.HealthPercent >= 90 && PurifyLowPrioTargetDebuffCount >= 1)
                || (HealTarget.HealthPercent >= 70 && PurifyLowPrioTargetDebuffCount >= 2)
                || (HealTarget.HealthPercent >= 50 && PurifyLowPrioTargetDebuffCount >= 3)),
                new Action(
                    delegate {
                        CastSpell("Purify", PurifyLowPrioTarget, "Low Priority");
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region     Angelic Feather

        private static Composite AngelicFeather() {
            return new Decorator(
                ret =>
                !Me.MovementInfo.IsFalling &&
                !CastingorGCDL() &&
                Me.MovementInfo.RunSpeed >= 4.5 &&
                SpellManager.HasSpell("Angelic Feather") &&
                SpellManager.Spells["Angelic Feather"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                LastFeather.AddSeconds(DiscSettings.Instance.AngelicFeatherDelay) < DateTime.Now &&
                IsMoving(Me, 100) &&
                !Me.Mounted &&
                !Me.HasAura("Angelic Feather") &&
                !Me.HasAura("Preparation") &&
                !Me.HasAura("Arena Preparation"),
                new Action(
                    delegate {
                        CastSpell("Angelic Feather", Me, Me.Location);
                        LastFeather = DateTime.Now;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region  DPS Trinket

        private static Composite DPSTrinket(String cause) {
            return new Decorator(
                ret =>
                DiscSettings.Instance.useTrinket &&
                ((DiscSettings.Instance.trinketSlotNumber == 13 && Me.Inventory.Equipped.Trinket1.CooldownTimeLeft.TotalMilliseconds < MyLatency) ||
                (DiscSettings.Instance.trinketSlotNumber == 14 && Me.Inventory.Equipped.Trinket2.CooldownTimeLeft.TotalMilliseconds < MyLatency)),
                new Action(
                    delegate {
                        Lua.DoString("RunMacroText('/use " + DiscSettings.Instance.trinketSlotNumber + "');");
                        Logging.Write("Using DPS trinket to " + cause + ".");
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        #region Berserking

        private static Composite Berserking(string cause) {
            return new Decorator(
                ret =>
                DiscSettings.Instance.UseRacial &&
                LastCastSpell != "Berserking" &&
                SpellManager.HasSpell("Berserking") &&
                SpellManager.Spells["Berserking"].CooldownTimeLeft.TotalMilliseconds <= MyLatency,
                new Action(
                    delegate {
                        CastSpell("Berserking", Me, cause);
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

        //Heals

        #region  Divine Star

        private static Composite DivineStar() {
            return new Decorator(
                ret =>
                LastCastSpell != "Divine Star" &&
                Me.CurrentMana >= Manacosts.Instance.DivineStar &&
                HealTarget.Distance2D <= 30 &&
                SpellManager.HasSpell("Divine Star") &&
                SpellManager.Spells["Divine Star"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                SafelyFaceTarget(HealTarget) &&                
                HealTarget.HealthPercent <= 70,
                new Action(
                    delegate {

                        if (DiscSettings.Instance.AutoFace) {
                            HealTarget.Face();
                            while (!Me.IsSafelyFacing(HealTarget, 45f)) {
                                Logging.Write("Waiting for facing.");
                            }
                        }

                        CastSpell("Divine Star", HealTarget);
                    }
                )
            );
        }

        #endregion

        #region  Cascade

        private static Composite Cascade() {
            return new Decorator(
                ret =>
                LastCastSpell != "Cascade" &&
                Me.CurrentMana >= Manacosts.Instance.Cascade * 2 &&
                InjuredUnitCount >= 3 &&
                !CastingorGCDL() && 
                //Healable(HealTarget) &&                
                SpellManager.HasSpell("Cascade") &&
                SpellManager.Spells["Cascade"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                    //CountEnemyNearby(Me, 30) >= DiscSettings.Instance.Lvl90Units &&
                (!DiscSettings.Instance.Lvl90SafeCheck || IsHaloSafe()) &&
                HealTarget.HealthPercent >= 40,
                new Action(
                    delegate {
                        CastSpell("Cascade", HealTarget);
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region  Halo

        private static Composite Halo() {
            return new Decorator(
                ret =>
                InjuredUnitCount >= 2 &&
                Me.CurrentMana >= Manacosts.Instance.Halo * 2 &&
                LastCastSpell != "Halo" &&
                !CastingorGCDL() &&
                SpellManager.HasSpell("Halo") &&
                SpellManager.Spells["Halo"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                (HealTarget.HealthPercent >= 35) &&
                (!DiscSettings.Instance.Lvl90SafeCheck || IsHaloSafe()) &&
                (HealTarget.HealthPercent >= 40 || (HealTarget.Distance2D >= 25 && HealTarget.Distance2D <= 30)),
                new Action(
                    delegate {
                        CastSpell("Halo", Me);
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region UseHealthstone

        private static Composite UseHealthstone()
        {
            return new Decorator(
                ret =>
                Me.Combat && Me.HealthPercent < DiscSettings.Instance.HealthstonePercent,
                new Action(delegate
                {
                    WoWItem hs = Me.BagItems.FirstOrDefault(o => o.Entry == 5512); //5512 Healthstone
                    if (hs != null && hs.CooldownTimeLeft.TotalMilliseconds <= MyLatency)
                    {
                        hs.Use();
                        Logging.Write("Use Healthstone at " + Me.HealthPercent + "%");
                    }
                    return RunStatus.Failure;
                })
                );
        }

        #endregion

        #region  Power Word: Shield

        private static Composite PWS()
        {
            return new Decorator(
                ret =>                 
                Me.CurrentMana >= Manacosts.Instance.PWS &&
                HealTarget.HealthPercent <= DiscSettings.Instance.PWSHp &&
                LastCastSpell != "Power Word: Shield" &&
                (!HealTarget.HasAura("Weakened Soul") || Me.HasAura(123266)) &&
                !HealTarget.HasAura("Power Word: Shield") &&
                (Me.Combat || HealTarget.Combat),
                new Action(delegate
                {
                    CastSpell("Power Word: Shield", HealTarget);
                    HealTarget = null;
                    return RunStatus.Success;
                }));
        }

        #endregion

        #region     Binding Heal

        private static Composite BindingHeal() {
            return new Decorator(
                ret =>
                Me.CurrentMana >= Manacosts.Instance.BindingHeal &&
                !Me.IsMoving &&
                !HealTarget.IsMe &&                
                ((HealTarget.HealthPercent <= DiscSettings.Instance.FlashHealHp && Me.HealthPercent < 95)
                || Me.HasAura(109964)) && //spirit shell                
                SpellManager.HasSpell("Binding Heal"),
                new Action(delegate {
                    CastSpell("Binding Heal", HealTarget);
                    HealTarget = null;
                    return RunStatus.Success;
                }));
        }

        #endregion

        #region  Renew

        private static Composite Renew()
        {
            return new Decorator(
                ret =>
                LastCastSpell != "Renew" &&
                HealTarget.HealthPercent <= DiscSettings.Instance.renewHp &&
                HealTarget.HealthPercent > 40 &&
                Me.CurrentMana >= Manacosts.Instance.Renew &&
                !HealTarget.HasAura("Renew"),
                new Action(delegate
                {
                    CastSpell("Renew", HealTarget);
                    HealTarget = null;
                }));
        }

        #endregion

        #region   Prayer of Mending

        private static Composite PoM()
        {
            return new Decorator(
                ret =>
                HealTarget.HealthPercent <= DiscSettings.Instance.PoMHp &&
                !HasMyAura(HealTarget, "Prayer of Mending", 0) &&
                LastCastSpell != "Prayer of Mending" &&
                HealTarget.Combat &&
                Me.CurrentMana >= Manacosts.Instance.PoM &&
                SpellManager.HasSpell("Prayer of Mending") &&
                SpellManager.Spells["Prayer of Mending"].CooldownTimeLeft.TotalMilliseconds <= MyLatency,
                new Action(delegate
                {
                    CastSpell("Prayer of Mending", HealTarget);
                    HealTarget = null;
                }));
        }

        #endregion

        #region   Flash Heal
        private static Composite FlashHeal()
        {
            return new Decorator(
                ret =>
                !Me.IsMoving &&
                (HealTarget.HealthPercent <= DiscSettings.Instance.FlashHealHp
                || Me.HasAura(109964)) && //spirit shell
                Me.CurrentMana >= Manacosts.Instance.FlashHeal,
                new Action(
                    delegate {
                        if (!Me.IsMoving &&
                            HealTarget.HealthPercent <= DiscSettings.Instance.innerFocusHp &&
                            SpellManager.HasSpell("Inner Focus") &&
                            SpellManager.Spells["Inner Focus"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                            !Me.HasAura("Inner Focus")) {
                                CastSpell("Inner Focus", Me, "Flash Heal");
                        }
                        CastSpell("Flash Heal", HealTarget);
                        HealTarget = null;
                        return RunStatus.Success;
                    }
                )
            );
        }

        private static Composite FlashHealInstant() {
            return new Decorator(
                ret =>
                TalentNames.Contains("From Darkness, Comes Light") &&
                Me.HasAura(114255) &&
                (HealTarget.HealthPercent <= DiscSettings.Instance.InstantFlashHealBelowHP
                || (Between(MyAuraTimeLeft(114255, Me), 200, 2000) && HealTarget.HealthPercent <= 95)),
                new Action(
                    delegate {
                        if (HealTarget.HealthPercent <= DiscSettings.Instance.innerFocusHp &&
                            SpellManager.HasSpell("Inner Focus") &&
                            SpellManager.Spells["Inner Focus"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                            !Me.HasAura("Inner Focus")) {
                                CastSpell("Inner Focus", Me, "Flash Heal - Instant");
                        }
                        CastSpell("Flash Heal", HealTarget, "Instant");
                        HealTarget = null;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region    Prayer of Healing

        private static Composite PrayerOfHealing() {
            return new Decorator(
                ret =>
                !Me.IsMoving &&
                Me.CurrentMana >= Manacosts.Instance.PrayerOfHealing &&
                NearbyFriendlyPlayers.Count >= 3 && Me.HasAura(109964),
                new Action(
                    delegate {
                        if (Me.Combat &&
                            SpellManager.HasSpell("Inner Focus") &&
                            SpellManager.Spells["Inner Focus"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                            !Me.HasAura("Inner Focus")) {
                            CastSpell("Inner Focus", Me, "PoH");
                        }
                        CastSpell("Prayer of Healing", HealTarget, "Spirit Shell");
                        HealTarget = null;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion

        #region    Heal

        private static Composite Heal_() {
            return new Decorator(
                ret =>
                !Me.IsMoving &&
                (HealTarget.HealthPercent <= DiscSettings.Instance.VoidShiftBelowHp
                || Me.HasAura(109964)) &&
                Me.CurrentMana >= Manacosts.Instance.Heal,
                new Action(delegate {
                    CastSpell("Heal", HealTarget);
                    HealTarget = null;
                    return RunStatus.Success;
                }));
        }

        #endregion

        #region    Greater Heal

        private static Composite GreaterHeal() {
            return new Decorator(
                ret =>
                !Me.IsMoving &&
                (HealTarget.HealthPercent <= DiscSettings.Instance.InstantFlashHealBelowHP
                || Me.HasAura(109964)) && //Spirit Shell
                Me.CurrentMana >= Manacosts.Instance.GreaterHeal,
                new Action(delegate {
                CastSpell("Greater Heal", HealTarget);
                HealTarget = null;
            }));
        }

        #endregion

        #region    Penance

        private static Composite Penance() {
            return new Decorator(
                ret =>
                (!Me.IsMoving || GlyphNames.Contains("Glyph of Penance")) &&
                HealTarget.HealthPercent <= DiscSettings.Instance.penanceHp &&
                Me.CurrentMana >= Manacosts.Instance.Penance &&
                SpellManager.HasSpell("Penance") &&
                SpellManager.Spells["Penance"].CooldownTimeLeft.TotalMilliseconds <= MyLatency,
                new Action(
                    delegate {
                        //HealTarget.Face();
                        CastSpell("Penance", HealTarget);
                        HealTarget = null;
                        return RunStatus.Success;
                    }
                )
            );
        }

        #endregion 

        #region Inner Focus

        private static Composite InnerFocus() {

            return new Decorator(
                ret =>
                !Me.IsMoving &&
                HealTarget.HealthPercent <= DiscSettings.Instance.innerFocusHp &&
                SpellManager.HasSpell("Inner Focus") &&
                SpellManager.Spells["Inner Focus"].CooldownTimeLeft.TotalMilliseconds <= MyLatency &&
                !Me.HasAura("Inner Focus"),
                new Action(
                    delegate {
                        CastSpell("Inner Focus", Me);
                        return RunStatus.Failure;
                    }
                )
            );
        }

        #endregion

    }
}