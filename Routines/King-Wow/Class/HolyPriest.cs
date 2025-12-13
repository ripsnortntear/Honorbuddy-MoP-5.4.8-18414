using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Styx.WoWInternals.DBC;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Styx.CommonBot;
using System.Windows.Forms;
using Styx.CommonBot.Inventory;

namespace KingWoW
{
    public class HolyPriestCombatClass : KingWoWAbstractBaseClass
    {

        private static string Name = "KingWoW HolyPriest";

        #region CONSTANT AND VARIABLES
        //START OF CONSTANTS AND VARIABLES ==============================
        private KingWoWUtility utils = null;
        private Movement movement = null;
        private ExtraUtils extra = null;

        private WoWUnit tank = null;
        private WoWUnit lastTank = null;
        private bool LazyRaiderBotType = true;
        private bool RaidBotBotType = false;
        private bool SoloBotType = false;
        private bool PVPBotType = false;
        private string BaseBot = "unknown";
        private TalentManager talents = null;

        WoWUnit CurrentHealtarget = null;

        private const string TANK_CHANGE = "TANK CHANGED";
        private const string FACING = "FACING";

        //START OF SPELLS AND AURAS ==============================
        private const string EVANGELISM = "Evangelism";
        private const string GUARDIAN_SPIRIT = "Guardian Spirit";
        private const string SERENDIPITY = "Serendipity";
        private const string DIVINE_INSIGHT = "Divine Insight";
        private const string LIGHTWELL = "Lightwell";
        private const string LIGHTSPRING = "Lightspring";
        private const string CIRCLE_OF_HEALING = "Circle of Healing";
        private const string HOLY_WORD_CHASTISE = "Holy Word: Chastise";
        private const string HOLY_WORD_SERENITY = "Holy Word: Serenity";
        private const string HOLY_WORD_SANCTUARY = "Holy Word: Sanctuary";
        //private const string POWER_WORD_SOLACE = "Power Word: Solace";
        private const int POWER_WORD_SOLACE = 129250;
        private const int DIVINE_STAR = 110744;
        private const string CHAKRA_CHASTISE = "Chakra: Chastise";
        private const string CHAKRA_SANCTUARY = "Chakra: Sanctuary";
        private const string CHAKRA_SERENITY = "Chakra: Serenity";
        private const string INNER_WILL = "Inner Will";
        private const string INNER_FIRE = "Inner Fire";        
        private const string HYMN_OF_HOPE = "Hymn of Hope";
        private const string PRAYER_OF_HEALING = "Prayer of Healing";
        private const string PRAYER_OF_MENDING = "Prayer of Mending";
        private const string POWER_WORD_SHIELD = "Power Word: Shield";
        private const string POWER_WORD_FORTITUDE = "Power Word: Fortitude";
        private const string SHADOW_WORD_PAIN = "Shadow Word: Pain";
        private const string SHADOW_WORD_DEATH = "Shadow Word: Death";
        private const string FEAR_WARD = "Fear Ward";
        private const string MASS_DISPEL = "Mass Dispel";
        private const string DIVINE_HYMN = "Divine Hymn";
        private const string POWER_INFUSION = "Power Infusion";
        private const string PURIFY = "Purify";
        private const string FLASH_HEAL = "Flash Heal";
        private const string GREATER_HEAL = "Greater Heal";
        private const string HEAL = "Heal";
        private const string RENEW = "Renew";
        private const string SURGE_OF_LIGHT = "Surge of Light";
        private const string HOLY_FIRE = "Holy Fire";
        private const string DESPERATE_PRAYER = "Desperate Prayer";
        private const string RESURRECTION = "Resurrection";
        private const string SMITE = "Smite";
        private const string FADE = "Fade";
        private const string SHADOWFIEND = "Shadowfiend";
        private const string MINDBENDER = "Mindbender";
        private const string WEAKENED_SOUL = "Weakened Soul";
        private const string LEVITATE = "Levitate";
        private const string CASCADE = "Cascade";
        private const string HALO = "Halo";
        private const string SPIRIT_OF_REDEMPTION = "Spirit of Redemption";
        private const string VOID_SHIFT = "Void Shift";
        private const string ANGELIC_FEATHER = "Angelic Feather";
        private const string BINDING_HEAL = "Binding Heal";

        private const string DRINK = "Drink";
        private const string FOOD = "Food";

        //END OF SPELLS AND AURAS ==============================

        //END OF CONSTANTS ==============================
        #endregion

        public HolyPriestCombatClass()
        {
            utils = new KingWoWUtility();
            movement = new Movement();
            extra = new ExtraUtils();
            tank = null;
            lastTank = null;
            LazyRaiderBotType = true;
            RaidBotBotType = false;
            SoloBotType = false;
            PVPBotType = false;
            BaseBot = "unknown";
            talents = new TalentManager();
        }

        public override bool Pulse
        {
            get
            {               
                if (HolyPriestSettings.Instance.TrySaveMana)
                    StopCastingCheck();
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */!StyxWoW.IsInGame || !StyxWoW.IsInWorld /*|| utils.IsGlobalCooldown(true)*/ || utils.MeIsChanneling || Me.IsCasting
                || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD))
                    return false;

                //UPDATE TANK
                //tank = utils.GetTank();
                tank = utils.SimpleGetTank(40f);
                if (tank == null || !tank.IsValid || !tank.IsAlive) tank = Me;

                if (tank != null && (lastTank == null || lastTank.Guid != tank.Guid))
                {
                    lastTank = tank;
                    utils.LogActivity(TANK_CHANGE, tank.Class.ToString());
                }

                //full party or me
                if (!Me.Combat && !Me.Mounted && HolyPriestSettings.Instance.OOCHealing && !utils.isAuraActive(DRINK) && !utils.isAuraActive(FOOD))
                {
                    WoWUnit healTarget = utils.GetHealTarget(40f);

                    Resurrect();
                    CastHW_Sanctuary(false);

                    if (healTarget != null && healTarget.HealthPercent < 98)
                    {
                        double hp = healTarget.HealthPercent;
                        if (utils.isAuraActive(SURGE_OF_LIGHT, Me)
                            && utils.CanCast(FLASH_HEAL)
                            && (hp <= HolyPriestSettings.Instance.SoLPercent
                                || Me.GetAuraByName(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                                || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                        {
                            utils.LogHealActivity(healTarget, FLASH_HEAL, healTarget.Class.ToString());
                            return utils.Cast(FLASH_HEAL, healTarget);
                        }
                        if (!WoWSpell.FromId(88684).Cooldown && utils.isAuraActive(CHAKRA_SERENITY))
                        {
                            utils.LogHealActivity(healTarget, HOLY_WORD_SERENITY, healTarget.Class.ToString());
                            return utils.Cast(HOLY_WORD_SERENITY, healTarget);
                        }
                        if (!Me.IsMoving && hp <= HolyPriestSettings.Instance.GHPercent && utils.CanCast(GREATER_HEAL))
                        {
                            utils.LogHealActivity(healTarget, GREATER_HEAL, healTarget.Class.ToString());
                            CurrentHealtarget = healTarget;
                            return utils.Cast(GREATER_HEAL, healTarget);
                        }
                        if (!Me.IsMoving && utils.CanCast(HEAL) && hp <= HolyPriestSettings.Instance.HealPercent)
                        {
                            utils.LogHealActivity(healTarget, HEAL, healTarget.Class.ToString());
                            CurrentHealtarget = healTarget;
                            return utils.Cast(HEAL, healTarget);
                        }
                        if (HolyPriestSettings.Instance.UseRenewOnMoving && !healTarget.HasAura(RENEW) && utils.CanCast(RENEW))
                        {
                            utils.LogHealActivity(healTarget, RENEW, healTarget.Class.ToString());
                            return utils.Cast(RENEW, healTarget);
                        }
                    }
                }
                if (tank != null && tank.Combat && !Me.GotTarget)
                    return CombatRotation();
                return false;
            }
        }

        public override bool Combat
        {
            get
            {               
                    if (HolyPriestSettings.Instance.TrySaveMana)
                        StopCastingCheck();
                    extra.AnyKeyBindsPressed();
                    if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !Me.Mounted && !StyxWoW.IsInGame || !StyxWoW.IsInWorld /*|| utils.IsGlobalCooldown(true)*/ || utils.MeIsChanneling || Me.IsCasting
                    || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD))
                        return false;
                
                    //UPDATE TANK
                    //tank = utils.GetTank();
                    tank = utils.SimpleGetTank(40f);
                    if (tank == null || !tank.IsValid || !tank.IsAlive) tank = Me;

                    if (tank != null && (lastTank == null || lastTank.Guid != tank.Guid))
                    {
                        lastTank = tank;
                        utils.LogActivity(TANK_CHANGE, tank.Class.ToString());
                    }
                    return CombatRotation();


            }
        }

        public override bool Initialize
        {
            get
            {
                extra.GoStopWatchUpdateKeyBinds();
                if (ExtraUtilsSettings.Instance.SoundsEnabled)
                {
                    try
                    {
                        SoundManager.LoadSoundFilePath(@"\Routines\King-wow\Sounds\Welcome.wav");
                        SoundManager.SoundPlay();
                    }
                    catch { }
                }
                Logging.Write("Ciao " + Me.Class.ToString());
                Logging.Write("Welcome to " + Name + " custom class");
                Logging.Write("Tanks All HonorBuddy Forum developers for code inspiration!");
                Logging.Write("Powered by Attilio76");
                BotEvents.OnBotStart += new BotEvents.OnBotStartDelegate(BotEvents_OnBotStart);
                Lua.Events.AttachEvent("GROUP_ROSTER_UPDATE", UpdateGroupChangeEvent);

                utils.FillParties();
                return true; ;
            }
        }

        private void UpdateGroupChangeEvent(object sender, LuaEventArgs args)
        {
            Logging.Write("Update Groups composition");
            utils.FillParties();
        }

        void BotEvents_OnBotStart(EventArgs args)
        {
            talents.Update();
            BotUpdate();
        }

        public void StopCastingCheck()
        {
            //Stop Casting Healing Spells
            if (Me.CastingSpell != null && Me.CurrentCastTimeLeft != null &&
                (Me.CastingSpell.Name == HEAL || Me.CastingSpell.Name == FLASH_HEAL ||
                 Me.CastingSpell.Name == GREATER_HEAL) && Me.CurrentCastTimeLeft.TotalMilliseconds > 500)
            {
                if (CurrentHealtarget != null && (!CurrentHealtarget.IsValid || !CurrentHealtarget.IsAlive))
                {
                    CurrentHealtarget = null;
                    SpellManager.StopCasting();
                    Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Unit is Dead");
                }
                else
                {
                    if (Me.IsCasting && CurrentHealtarget != null && CurrentHealtarget.IsValid &&
                        Me.CastingSpell.Name == GREATER_HEAL &&
                        (CurrentHealtarget.HealthPercent > 80 ||
                        CurrentHealtarget.HealthPercent >= HolyPriestSettings.Instance.GHPercent + 20))
                    {
                        SpellManager.StopCasting();
                        Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Save Mana Greater heal");
                    }

                    if (Me.IsCasting && CurrentHealtarget != null && CurrentHealtarget.IsValid &&
                        Me.CastingSpell.Name == FLASH_HEAL &&
                        (CurrentHealtarget.HealthPercent > 80 ||
                        CurrentHealtarget.HealthPercent >= HolyPriestSettings.Instance.FlashHealPercent + 20))
                    {
                        SpellManager.StopCasting();
                        Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Save Mana Flash Heal");
                    }

                    if (Me.IsCasting && CurrentHealtarget != null && CurrentHealtarget.IsValid &&
                        Me.CastingSpell.Name == HEAL &&
                        Me.CurrentCastTimeLeft.TotalMilliseconds > 300 &&
                        CurrentHealtarget.HealthPercent < HolyPriestSettings.Instance.GHPercent &&
                        (CurrentHealtarget.Combat || Me.Combat))
                    {
                        SpellManager.StopCasting();
                        Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting Heal. Need Greater Heal!");
                    }

                    if (Me.IsCasting && CurrentHealtarget != null && CurrentHealtarget.IsValid &&
                        CurrentHealtarget.HealthPercent > 99)
                    {
                        CurrentHealtarget = null;
                        SpellManager.StopCasting();
                        Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Unit Full HP");
                    }

                    if (Me.IsCasting && CurrentHealtarget != null && CurrentHealtarget.IsValid && !CurrentHealtarget.InLineOfSpellSight)
                    {
                        CurrentHealtarget = null;
                        SpellManager.StopCasting();
                        Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Unit not In Line of Spell Sight");
                    }

                    if (Me.IsCasting && CurrentHealtarget != null && CurrentHealtarget.IsValid && CurrentHealtarget.Distance > 40)
                    {
                        CurrentHealtarget = null;
                        SpellManager.StopCasting();
                        Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Unit out of range");
                    }
                }
            }
        }

        public bool CombatRotation()
        {
            ProcWork();
            extra.UseHealthstone();
            extra.UseRacials();
            extra.UseTrinket1();
            extra.UseTrinket2();
            extra.UseEngineeringGloves();
            extra.UseLifeblood();
            extra.UseAlchemyFlask();
            extra.WaterSpirit();
            extra.LifeSpirit();

            if (PVPBotType || HolyPriestSettings.Instance.RotationType == 3)
            {
                Self();
                ManaRegen();
                TankHealing();
                AoE();
                Cleansing();
                Healing();
                Dps();
                return true;
            }

            else if (SoloBotType)
            {
                Self();
                ManaRegen();
                Cleansing();
                Healing();
                Dps();
                return true;
            }
            //Use 0 for NORMAL ROTATION, Use 1 For First Tank and SINGLE TARGET ROTATION, Use 2 for AOE PRIORITY ROTATION
            else if ((LazyRaiderBotType || RaidBotBotType) && HolyPriestSettings.Instance.RotationType == 0)
            {
                Self();
                ManaRegen();
                TankHealing();
                AoE();
                Cleansing();
                Healing();
                return true;
            }
            else if ((LazyRaiderBotType || RaidBotBotType) && HolyPriestSettings.Instance.RotationType == 1)
            {
                if (Self()) return true;
                else if (ManaRegen()) return true;
                else if (TankHealing()) return true;
                else if (Cleansing()) return true;
                else if (Healing()) return true;
                else if (AoE()) return true;

            }
            else if ((LazyRaiderBotType || RaidBotBotType) && HolyPriestSettings.Instance.RotationType == 2)
            {
                if (Self()) return true;
                else if (ManaRegen()) return true;
                else if (AoE()) return true;
                else if (Cleansing()) return true;
                else if (TankHealing()) return true;
                else if (Healing()) return true;
            }
            else
                utils.LogActivity("NO VALID ROTATION ");
            return false;

        }

        public override bool NeedRest
        {
            get
            {
                if ((utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD)) && (Me.ManaPercent < 100 || Me.HealthPercent < 100))
                    return true;
                if (Me.ManaPercent <= HolyPriestSettings.Instance.ManaPercent &&
                !utils.isAuraActive(DRINK) && !Me.Combat && !Me.IsMoving && !Me.IsCasting)
                {
                    WoWItem mydrink = Consumable.GetBestDrink(false);
                    if (mydrink != null)
                    {
                        utils.LogActivity("Drinking/Eating");
                        Styx.CommonBot.Rest.DrinkImmediate();
                        return true;
                    }
                }
                if (Me.HealthPercent <= HolyPriestSettings.Instance.HealthPercent &&
                !utils.isAuraActive(FOOD) && !Me.Combat && !Me.IsMoving && !Me.IsCasting)
                {
                    WoWItem myfood = Consumable.GetBestFood(false);
                    if (myfood != null)
                    {
                        utils.LogActivity("Eating");
                        Styx.CommonBot.Rest.DrinkImmediate();
                        return true;
                    }
                }
                return false;
            }
        }

        public override bool NeedPullBuffs
        {
            get
            {
                return HolyPullBuff();
            }
        }

        public override bool NeedCombatBuffs { get { return Buff(); } }

        public override bool NeedPreCombatBuffs { get { return Buff(); } }

        public override bool Pull
        {
            get
            {
                WoWUnit target = Me.CurrentTarget;
                if (target != null && !target.IsFriendly && target.Attackable && !target.IsDead)
                {
                    if (!target.InLineOfSpellSight || !target.InLineOfSight || target.Distance > HolyPriestSettings.Instance.PullDistance)
                    {
                        //Logging.Write("pull: target distance=" + target.Distance + " moving in range");
                        movement.KingHealMove(target.Location, HolyPriestSettings.Instance.PullDistance, true, true, target);
                    }
                    if (utils.CanCast(HOLY_FIRE) /*&& !Me.IsMoving*/)
                    {
                        if (!Me.IsFacing(target))
                        {
                            utils.LogActivity(FACING, target.Name);
                            Me.SetFacing(target);
                        }

                        utils.LogActivity(HOLY_FIRE, target.Name);
                        return utils.Cast(HOLY_FIRE, target);
                    }

                    if (utils.CanCast(SMITE) && !Me.IsMoving)
                    {
                        if (!Me.IsFacing(target))
                        {
                            utils.LogActivity(FACING, target.Name);
                            Me.SetFacing(target);
                        }

                        utils.LogActivity(SMITE, target.Name);
                        return utils.Cast(SMITE, target);
                    }
                }
                return false;
            }
        }

        private bool HolyPullBuff()
        {
            if (utils.Mounted())
                return false;
            if (!utils.isAuraActive(POWER_WORD_FORTITUDE) && utils.CanCast(POWER_WORD_FORTITUDE))
            {
                utils.LogActivity(POWER_WORD_FORTITUDE);
                utils.Cast(POWER_WORD_FORTITUDE);
            }

            if (!utils.isAuraActive(FEAR_WARD, tank) && utils.CanCast(FEAR_WARD) && HolyPriestSettings.Instance.UseFearWard && tank.InLineOfSight)
            {
                utils.LogActivity(FEAR_WARD, tank.Class.ToString());
                utils.Cast(FEAR_WARD, tank);
            }
            if (!utils.isAuraActive(POWER_WORD_SHIELD, Me) && !utils.isAuraActive(WEAKENED_SOUL, Me) && utils.CanCast(POWER_WORD_SHIELD))
            {
                utils.LogActivity(POWER_WORD_SHIELD, Me.Class.ToString());
                utils.Cast(POWER_WORD_SHIELD, Me); //no return see below
            }
            return false; //return false or loop trying cast PULLBUFF (not implemented)
        }

        private bool BotUpdate()
        {
            if (BaseBot.Equals(BotManager.Current.Name))
                return false;
            if (utils.IsBotBaseInUse("LazyRaider") || utils.IsBotBaseInUse("Tyrael"))
            {
                Logging.Write("Detected LazyRaider/tyrael:");
                Logging.Write("Disable all movements");
                Movement.DisableAllMovement = true;
                LazyRaiderBotType = true;
                RaidBotBotType = false;
                SoloBotType = false;
                PVPBotType = false;
                BaseBot = BotManager.Current.Name;
                return true;
            }

            if (utils.IsBotBaseInUse("Raid Bot"))
            {
                Logging.Write("Detected RaidBot:");
                Logging.Write("Disable all movements");
                Movement.DisableAllMovement = true;
                LazyRaiderBotType = false;
                RaidBotBotType = true;
                SoloBotType = false;
                PVPBotType = false;
                BaseBot = BotManager.Current.Name;
                return true;
            }

            if (utils.IsBotBaseInUse("BGBuddy"))
            {
                Logging.Write("Detected BGBuddy Bot:");
                Logging.Write("Enable PVP Rotation");
                Movement.DisableAllMovement = true;
                LazyRaiderBotType = false;
                RaidBotBotType = false;
                SoloBotType = false;
                PVPBotType = true;
                BaseBot = BotManager.Current.Name;
                return true;
            }

            Logging.Write("Base bot detected: " + BotManager.Current.Name);
            LazyRaiderBotType = false;
            RaidBotBotType = false;
            SoloBotType = true;
            PVPBotType = false;
            BaseBot = BotManager.Current.Name;
            return true;


        }

        private bool ManaRegen()
        {
            WoWUnit target = Me.CurrentTarget;
            if (Me.Combat && (target == null || !target.Attackable || target.IsFriendly || !target.InLineOfSight || target.Distance > 40 || target.IsDead))
            {
                if (tank != null)
                    target = tank.CurrentTarget;
            }
            if (Me.Combat && target != null && target.Attackable && target.InLineOfSight && target.Distance < 40 && !target.IsDead)
            {
                if (Me.ManaPercent <= HolyPriestSettings.Instance.ShadowFiendPercent && utils.CanCast(SHADOWFIEND))
                {
                    utils.LogActivity(SHADOWFIEND, target.Name);
                    return utils.Cast(SHADOWFIEND, target);
                }

                else if (Me.ManaPercent <= HolyPriestSettings.Instance.ShadowFiendPercent && utils.CanCast(MINDBENDER))
                {
                    utils.LogActivity(MINDBENDER, target.Name);
                    return utils.Cast(MINDBENDER, target);
                }
            }

            if (Me.ManaPercent <= HolyPriestSettings.Instance.HymnOfHopePercent && utils.CanCast(HYMN_OF_HOPE) && !Me.IsMoving)
            {
                utils.LogActivity(HYMN_OF_HOPE);
                return utils.Cast(HYMN_OF_HOPE);
            }

            return false;
        }
        
        private bool Ligthwell()
        {
            if (Me.Combat && (utils.CanCast(LIGHTWELL) || utils.CanCast(LIGHTSPRING)) && HolyPriestSettings.Instance.AutoCastLightwell && utils.GetMemberCountBelowThreshold(60) >= 1)
            {
                if(utils.CanCast(LIGHTWELL))
                {
                    utils.LogActivity(LIGHTWELL);
                    utils.Cast(LIGHTWELL);
                    if (tank != null && tank.IsAlive && tank.Distance < 40 && tank.InLineOfSight)
                    {
                        return SpellManager.ClickRemoteLocation(tank.Location);
                    }
                    return SpellManager.ClickRemoteLocation(Me.Location);
                }
                else if (utils.CanCast(LIGHTSPRING))
                {
                    utils.LogActivity(LIGHTSPRING);
                    utils.Cast(LIGHTSPRING);
                    if (tank != null && tank.IsAlive && tank.Distance < 40 && tank.InLineOfSight)
                    {
                        return SpellManager.ClickRemoteLocation(tank.Location);
                    }
                    return SpellManager.ClickRemoteLocation(Me.Location);
                }

            }
            return false;
        }

        private bool Self()
        {

            if (Me.Combat && Me.HealthPercent <= HolyPriestSettings.Instance.DesperatePrayerPercent && utils.CanCast(DESPERATE_PRAYER))
            {
                utils.LogHealActivity(Me, DESPERATE_PRAYER);
                return utils.Cast(DESPERATE_PRAYER);
            }

            if (Me.Combat && HolyPriestSettings.Instance.SelfHealingPriorityEnabled && Me.HealthPercent <= HolyPriestSettings.Instance.SelfHealingPriorityHP)
            {
                //guardian Spirit
                if (Me.HealthPercent <= HolyPriestSettings.Instance.GuardianSpiritPercent && utils.CanCast(GUARDIAN_SPIRIT))
                {
                    utils.LogHealActivity(Me, GUARDIAN_SPIRIT, Me.Class.ToString());
                    return utils.Cast(GUARDIAN_SPIRIT,Me);
                }

                //Surge of Light
                if (utils.isAuraActive(SURGE_OF_LIGHT, Me) && utils.CanCast(FLASH_HEAL))
                {
                    utils.LogHealActivity(Me, FLASH_HEAL, Me.Class.ToString());
                    return utils.Cast(FLASH_HEAL, Me);
                }
                if (Me.HealthPercent <= HolyPriestSettings.Instance.FlashHealPercent && utils.CanCast(FLASH_HEAL))
                {
                    utils.LogHealActivity(Me, FLASH_HEAL, Me.Class.ToString());
                    CurrentHealtarget = Me;
                    return utils.Cast(FLASH_HEAL,Me);
                }
                if (!utils.isAuraActive(POWER_WORD_SHIELD, Me) && !utils.isAuraActive(WEAKENED_SOUL, Me) && utils.CanCast(POWER_WORD_SHIELD))
                {
                    utils.LogHealActivity(Me, POWER_WORD_SHIELD, Me.Class.ToString());
                    return utils.Cast(POWER_WORD_SHIELD, Me);
                }
                if (!WoWSpell.FromId(88684).Cooldown && utils.isAuraActive(CHAKRA_SERENITY) && Me.HealthPercent <= HolyPriestSettings.Instance.HWSerenityPercent)
                {
                    utils.LogHealActivity(Me, HOLY_WORD_SERENITY, Me.Class.ToString());
                    return utils.Cast(HOLY_WORD_SERENITY);
                }
                if (Me.HealthPercent <= HolyPriestSettings.Instance.GHPercent && utils.CanCast(GREATER_HEAL))
                {
                    utils.LogHealActivity(Me, GREATER_HEAL, Me.Class.ToString());
                    CurrentHealtarget = Me;
                    return utils.Cast(GREATER_HEAL,Me);
                }
                if (Me.HealthPercent <= HolyPriestSettings.Instance.HealPercent && utils.CanCast(HEAL))
                {
                    utils.LogHealActivity(Me, HEAL, Me.Class.ToString());
                    CurrentHealtarget = Me;
                    return utils.Cast(HEAL,Me);
                }
                if (!Me.HasAura(RENEW) && utils.CanCast(RENEW) && Me.HealthPercent <= HolyPriestSettings.Instance.RenewPercent)
                {
                    utils.LogHealActivity(Me, RENEW, Me.Class.ToString());
                    return utils.Cast(RENEW, Me);
                }
            }

            //power infusion for heal burst or mana low
            if (Me.Combat && !utils.isAuraActive(POWER_INFUSION) && utils.CanCast(POWER_INFUSION) &&
                ((utils.GetMemberCountBelowThreshold(HolyPriestSettings.Instance.PowerInfusionPercent) >= HolyPriestSettings.Instance.PowerInfusionNumber) ||
                Me.ManaPercent <= HolyPriestSettings.Instance.PowerInfusionManaPercent))
            {
                utils.LogActivity(POWER_INFUSION);
                return utils.Cast(POWER_INFUSION);
            }
            return false;
        }

        private bool AoE()
        {
            //HW: Sanctuary
            CastHW_Sanctuary(true);

            //Lightwell-lightspring
            Ligthwell();

            if (!ExtraUtilsSettings.Instance.DisableAOE_HealingRotation)
            {
                //circle of healing
                if (utils.CanCast(CIRCLE_OF_HEALING))
                {
                    WoWPlayer coh_target = utils.BestCircleOfHealing_Target(HolyPriestSettings.Instance.CircleOfHealingPercent, HolyPriestSettings.Instance.CircleOfHealingNumber);
                    if (coh_target != null)
                    {
                        utils.LogHealActivity(coh_target, CIRCLE_OF_HEALING, coh_target.Class.ToString());
                        return utils.Cast(CIRCLE_OF_HEALING, coh_target);
                    }
                }

                //Prayer of mending on Divine Insight proc used on ProcWorks

                //divine Hymn
                if ((utils.GetMemberCountBelowThreshold(HolyPriestSettings.Instance.DHPercent) >= HolyPriestSettings.Instance.DHNumber) && utils.CanCast(DIVINE_HYMN))
                {
                    utils.LogActivity(DIVINE_HYMN);
                    return utils.Cast(DIVINE_HYMN);
                }

                //divine star
                if (utils.CanCast(DIVINE_STAR))
                {
                    WoWUnit ds_target = utils.BestDSTarget(HolyPriestSettings.Instance.CascadeHaloPercent, HolyPriestSettings.Instance.CascadeHaloNumber, false);
                    if (ds_target != null && utils.CanCast(DIVINE_STAR, ds_target, true))
                    {
                        ds_target.Face();
                        utils.LogActivity("DIVINE_STAR", ds_target.Class.ToString());
                        return utils.Cast(DIVINE_STAR, ds_target);
                    }
                }

                //Halo/cascade
                if ((utils.GetMemberCountBelowThreshold(HolyPriestSettings.Instance.CascadeHaloPercent) >= HolyPriestSettings.Instance.CascadeHaloNumber))
                {
                    if (utils.CanCast(HALO))
                    {
                        utils.LogActivity(HALO);
                        return utils.Cast(HALO);
                    }
                    else if (utils.CanCast(CASCADE))
                    {
                        utils.LogActivity(CASCADE);
                        return utils.Cast(CASCADE, Me);
                    }
                }

                //Prayer Of Healing
                if (utils.CanCast(PRAYER_OF_HEALING) && !Me.IsMoving)
                {
                    WoWPlayer poh_target = utils.BestPoHTarget(HolyPriestSettings.Instance.PrayerOfHealingPercent, HolyPriestSettings.Instance.PrayerOfHealingNumber,true);
                    if (poh_target != null)
                    {
                        utils.LogActivity(PRAYER_OF_HEALING, poh_target.Class.ToString());
                        return utils.Cast(PRAYER_OF_HEALING, poh_target);
                    }
                }
            }
            return false;
        }

        private bool Healing()
        {
            if (HolyPriestSettings.Instance.UsePW_Solace)
            {
                WoWUnit target = utils.getTargetToAttack(30, tank);
                if (target != null && target.InLineOfSpellSight)
                {
                    if (!Me.IsMoving)
                    {
                        Me.SetFacing(target);
                    }
                    if (/*utils.CanCast(POWER_WORD_SOLACE, target, true)*/ talents.IsSelected(9) && utils.GetSpellCooldown(POWER_WORD_SOLACE).Milliseconds <= StyxWoW.WoWClient.Latency)
                    {
                        utils.LogActivity("POWER_WORD_SOLACE", target.Name);
                        return utils.Cast(POWER_WORD_SOLACE, target);
                    }
                }
            }
            WoWUnit healTarget = utils.GetHealTarget(40f);

            if (healTarget != null)
            {
                double hp = healTarget.HealthPercent;

                if (healTarget.Distance > 40 || !healTarget.InLineOfSight)
                {
                    Logging.Write("GetHealTarget give me a wrong target!!");
                    return false;
                }
                else
                {
                    if (healTarget.HealthPercent <= HolyPriestSettings.Instance.VoidShiftTarget && Me.HealthPercent >= HolyPriestSettings.Instance.VoidShiftMe
                    && HolyPriestSettings.Instance.UseVoidShift && !HolyPriestSettings.Instance.UseVoidShiftOnlyOnTank)
                    {
                        utils.LogHealActivity(healTarget, VOID_SHIFT, healTarget.Class.ToString());
                        return utils.Cast(VOID_SHIFT, healTarget);
                    }

                    //guardian Spirit
                    if (healTarget.Combat && hp <= HolyPriestSettings.Instance.GuardianSpiritPercent && utils.CanCast(GUARDIAN_SPIRIT))
                    {
                        utils.LogHealActivity(healTarget, GUARDIAN_SPIRIT, healTarget.Class.ToString());
                        return utils.Cast(GUARDIAN_SPIRIT, healTarget);
                    }

                    //Surge of Light
                    if (utils.isAuraActive(SURGE_OF_LIGHT, Me)
                            && utils.CanCast(FLASH_HEAL)
                            && (hp <= HolyPriestSettings.Instance.SoLPercent
                                || Me.GetAuraByName(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                                || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                    {
                        utils.LogHealActivity(healTarget, "surge of light:"+FLASH_HEAL, healTarget.Class.ToString());
                        return utils.Cast(FLASH_HEAL, healTarget);
                    }

                    //binding Heal
                    if (hp <= HolyPriestSettings.Instance.BindingHealPercent && Me.HealthPercent <= HolyPriestSettings.Instance.BindingHealPercent
                        && utils.CanCast(BINDING_HEAL, healTarget, true) && !Me.IsMoving)
                    {
                        utils.LogHealActivity(healTarget, BINDING_HEAL, healTarget.Class.ToString());
                        return utils.Cast(BINDING_HEAL, healTarget);
                    }

                    //Flash heal
                    if (hp <= HolyPriestSettings.Instance.FlashHealPercent && utils.CanCast(FLASH_HEAL) && !Me.IsMoving && utils.isAuraActive(WEAKENED_SOUL, healTarget))
                    {
                        utils.LogHealActivity(healTarget, FLASH_HEAL, healTarget.Class.ToString());
                        CurrentHealtarget = healTarget;
                        return utils.Cast(FLASH_HEAL, healTarget);
                    }

                    //Great Heal
                    if (hp <= HolyPriestSettings.Instance.GHPercent && utils.CanCast(GREATER_HEAL))
                    {
                        //Great Heal needed but surge of light available or surge of ligth 2 stacks
                        if (utils.isAuraActive(SURGE_OF_LIGHT, Me)
                            && utils.CanCast(FLASH_HEAL)
                            && (hp <= HolyPriestSettings.Instance.SoLPercent
                                || Me.GetAuraByName(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                                || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                        {
                            utils.LogHealActivity(healTarget, FLASH_HEAL + " instead of greater heal for SoL proc", healTarget.Class.ToString());
                            return utils.Cast(FLASH_HEAL, healTarget);
                        }
                        else
                        {
                            utils.LogHealActivity(healTarget, GREATER_HEAL, healTarget.Class.ToString());
                            CurrentHealtarget = healTarget;
                            return utils.Cast(GREATER_HEAL, healTarget);
                        }
                    }

                    //HW: Serenity
                    if (!WoWSpell.FromId(88684).Cooldown && utils.isAuraActive(CHAKRA_SERENITY) && hp <= HolyPriestSettings.Instance.HWSerenityPercent)
                    {
                        utils.LogHealActivity(healTarget, HOLY_WORD_SERENITY, healTarget.Class.ToString());
                        return utils.Cast(HOLY_WORD_SERENITY, healTarget);
                    }

                    //renew
                    if (!healTarget.HasAura(RENEW) && hp <= HolyPriestSettings.Instance.RenewPercent && utils.CanCast(RENEW))
                    {
                        utils.LogHealActivity(healTarget, RENEW, healTarget.Class.ToString());
                        return utils.Cast(RENEW, healTarget);
                    }

                    //Heal
                    if (hp <= HolyPriestSettings.Instance.HealPercent && utils.CanCast(HEAL) && !Me.IsMoving)
                    {
                        utils.LogHealActivity(healTarget, HEAL, healTarget.Class.ToString());
                        CurrentHealtarget = healTarget;
                        return utils.Cast(HEAL, healTarget);
                    }
                    

                }
            }
            return false;
        }

        private bool TankHealing()
        {
            if (tank != null && tank.IsAlive && tank.Distance < 40 && tank.InLineOfSight)
            {
                if (tank.HealthPercent <= HolyPriestSettings.Instance.VoidShiftTarget && Me.HealthPercent >= HolyPriestSettings.Instance.VoidShiftMe
                    && HolyPriestSettings.Instance.UseVoidShift)
                {
                    utils.LogHealActivity(tank, VOID_SHIFT, tank.Class.ToString());
                    return utils.Cast(VOID_SHIFT, tank);
                }

                if (!SoloBotType && tank.Combat && utils.GroupNeedsMyAura(PRAYER_OF_MENDING) && utils.CanCast(PRAYER_OF_MENDING))
                {
                    utils.LogHealActivity(tank, PRAYER_OF_MENDING, tank.Class.ToString());
                    return utils.Cast(PRAYER_OF_MENDING, tank);
                }

                //guardian Spirit
                if (tank.Combat && tank.HealthPercent <= HolyPriestSettings.Instance.GuardianSpiritPercent && utils.CanCast(GUARDIAN_SPIRIT))
                {
                    utils.LogHealActivity(tank, GUARDIAN_SPIRIT, tank.Class.ToString());
                    return utils.Cast(GUARDIAN_SPIRIT, tank);
                }

                //renew always active on combat tank?
                if (tank.Combat && HolyPriestSettings.Instance.RenewAlwaysActiveOnTank)
                {
                    if (!tank.HasAura(RENEW) && utils.CanCast(RENEW))
                    {
                        utils.LogHealActivity(tank, RENEW, tank.Class.ToString());
                        return utils.Cast(RENEW, tank);
                    }
                }

                //serenity active and keep renew on tank selected
                if (tank.Combat && utils.isAuraActive(CHAKRA_SERENITY) && HolyPriestSettings.Instance.RenewAlwaysActiveOnTank && !WoWSpell.FromId(88684).Cooldown && tank.GetAuraByName(RENEW).TimeLeft.TotalMilliseconds <= 3500)
                {
                    //HW: Serenity
                    if (utils.CanCast(HOLY_WORD_SERENITY))
                    {
                        utils.LogHealActivity(tank, HOLY_WORD_SERENITY, tank.Class.ToString());
                        return utils.Cast(HOLY_WORD_SERENITY, tank);
                    }

                    //Just cast an heal
                    if (utils.CanCast(HEAL))
                    {
                        utils.LogHealActivity(tank, HEAL, tank.Class.ToString());
                        CurrentHealtarget = tank;
                        return utils.Cast(HEAL, tank);
                    }

                }
 

            }
            return false;
        }

        private bool Cleansing()
        {
            if (HolyPriestSettings.Instance.UsePurify)
            {
                WoWPlayer player = utils.GetDispelTargetPriest(40f);

                if (player != null && !Blacklist.Contains(player.Guid, BlacklistFlags.All) && player.Distance < 40 && player.InLineOfSight)
                {
                    if (utils.NeedsDispelPriest(player) && player.Distance < 30 && utils.MassDispelCountForPlayer(player,15f) >= HolyPriestSettings.Instance.MassDispellCount && utils.CanCast(MASS_DISPEL))
                    {
                        utils.LogActivity(MASS_DISPEL, player.Class.ToString());
                        Blacklist.Add(player,BlacklistFlags.All, new TimeSpan(0, 0, 2));
                        utils.Cast(MASS_DISPEL);
                        return SpellManager.ClickRemoteLocation(player.Location);
                    }

                    if (utils.CanCast(PURIFY))
                    {
                        utils.LogActivity(PURIFY, player.Class.ToString());
                        Blacklist.Add(player,BlacklistFlags.All, new TimeSpan(0, 0, 2));
                        return utils.Cast(PURIFY, player);
                    }
                }
            }
            return false;
        }

        private bool Resurrect()
        {     
            //non critical so dont use framelock
            foreach (WoWPlayer player in utils.GetResurrectTargets(40f))
            {
                if (Blacklist.Contains(player.Guid, BlacklistFlags.All)) continue;
                else
                {
                    if (player.Distance > 40 || !player.InLineOfSight) return false;
                    else if (utils.CanCast(RESURRECTION, player) && HolyPriestSettings.Instance.UseResurrection && !Me.IsMoving)
                    {
                        utils.LogActivity(RESURRECTION, player.Class.ToString());
                        Blacklist.Add(player,BlacklistFlags.All, new TimeSpan(0, 0, 60));
                        return utils.Cast(RESURRECTION, player);
                    }

                    return false;
                }
            }
            return false;
        }

        private bool Buff()
        {
            if (utils.Mounted() || /*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld 
                || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD))
                return false;
            

            if (HolyPriestSettings.Instance.AutoPWFortitude)
            {
                if (!utils.isAuraActive(POWER_WORD_FORTITUDE) && utils.CanCast(POWER_WORD_FORTITUDE))
                {
                    utils.LogActivity(POWER_WORD_FORTITUDE);
                    return utils.Cast(POWER_WORD_FORTITUDE);
                }

                foreach (WoWPlayer player in Me.PartyMembers)
                {
                    if (player.Distance > 40 || player.IsDead || player.IsGhost || !player.InLineOfSight) continue;
                    else if (!utils.isAuraActive(POWER_WORD_FORTITUDE, player) && utils.CanCast(POWER_WORD_FORTITUDE))
                    {
                        utils.LogActivity(POWER_WORD_FORTITUDE, player.Class.ToString());
                        return utils.Cast(POWER_WORD_FORTITUDE, player);
                    }
                }
            }

            if (tank != null && tank.Combat && !utils.isAuraActive(FEAR_WARD, tank) && utils.CanCast(FEAR_WARD) && HolyPriestSettings.Instance.UseFearWard && tank.InLineOfSight)
            {
                utils.LogActivity(FEAR_WARD, tank.Class.ToString());
                return utils.Cast(FEAR_WARD, tank);
            }
            if (Me.Combat && (Me.GroupInfo.IsInParty || Me.GroupInfo.IsInRaid) && Me.CurrentMap.IsInstance && (Targeting.GetAggroOnMeWithin(Me.Location, 30) > 0) && utils.CanCast(FADE))
            {
                utils.LogActivity(FADE);
                return utils.Cast(FADE);
            }

            if (utils.CanCast(INNER_WILL) && !utils.isAuraActive(INNER_WILL) && HolyPriestSettings.Instance.InnerToUse == HolyPriestSettings.InnerType.WILL)
            {
                utils.LogActivity(INNER_WILL);
                return utils.Cast(INNER_WILL);
            }

            if (HolyPriestSettings.Instance.InnerWillOnMoving && Me.IsMoving && !Me.Mounted && !utils.isAuraActive(INNER_WILL))
            {
                utils.LogActivity("moving...." + INNER_WILL);
                return utils.Cast(INNER_WILL);

            }

            if (HolyPriestSettings.Instance.AngelicFeatherOnMoving && Me.IsMoving && !Me.Mounted && !utils.isAuraActive(ANGELIC_FEATHER))
            {
                utils.LogActivity("moving...." + ANGELIC_FEATHER);
                utils.Cast(ANGELIC_FEATHER);
                return SpellManager.ClickRemoteLocation(Me.Location);

            }

            if (!Me.IsMoving && utils.CanCast(INNER_FIRE) && !utils.isAuraActive(INNER_FIRE) && HolyPriestSettings.Instance.InnerToUse == HolyPriestSettings.InnerType.FIRE)
            {
                utils.LogActivity(INNER_FIRE);
                return utils.Cast(INNER_FIRE);
            }

            switch (HolyPriestSettings.Instance.ChakraToUse)
            {
                case HolyPriestSettings.ChakraType.SANCTUARY:
                    {
                        if (!utils.isAuraActive(CHAKRA_SANCTUARY) && utils.CanCast(CHAKRA_SANCTUARY))
                        {
                            utils.LogActivity(CHAKRA_SANCTUARY);
                            return utils.Cast(CHAKRA_SANCTUARY);
                        }
                    }
                    break;
                case HolyPriestSettings.ChakraType.SERENITY:
                    {
                        if (!utils.isAuraActive(CHAKRA_SERENITY) && utils.CanCast(CHAKRA_SERENITY))
                        {
                            utils.LogActivity(CHAKRA_SERENITY);
                            return utils.Cast(CHAKRA_SERENITY);
                        }
                    }
                    break;
                case HolyPriestSettings.ChakraType.CHASTISE:
                    {
                        if (!utils.isAuraActive(CHAKRA_CHASTISE) && utils.CanCast(CHAKRA_CHASTISE))
                        {
                            utils.LogActivity(CHAKRA_CHASTISE);
                            return utils.Cast(CHAKRA_CHASTISE);
                        }
                    }
                    break;
            }
            return false;
        }

        private bool Dps()
        {
            WoWUnit target = utils.getTargetToAttack(30,tank);
            if (target != null)
            {
                if (target.HealthPercent < 20 && utils.CanCast(SHADOW_WORD_DEATH))
                {
                    utils.LogActivity(SHADOW_WORD_DEATH, target.Name);
                    return utils.Cast(SHADOW_WORD_DEATH, target);
                }

                if (!utils.isAuraActive(SHADOW_WORD_PAIN, target) && utils.CanCast(SHADOW_WORD_PAIN))
                {
                    utils.LogActivity(SHADOW_WORD_PAIN, target.Name);
                    return utils.Cast(SHADOW_WORD_PAIN, target);
                }
                if (/*utils.CanCast(POWER_WORD_SOLACE, target, true)*/ talents.IsSelected(9) && utils.GetSpellCooldown(POWER_WORD_SOLACE).Milliseconds <= StyxWoW.WoWClient.Latency)
                {
                    utils.LogActivity("POWER_WORD_SOLACE", target.Name);
                    return utils.Cast(POWER_WORD_SOLACE, target);
                }
                if (utils.CanCast(HOLY_WORD_CHASTISE))
                {
                    if (!Me.IsFacing(target))
                    {
                        utils.LogActivity(FACING, target.Name);
                        Me.SetFacing(target);
                    }
                    utils.LogActivity(HOLY_WORD_CHASTISE, target.Name);
                    return utils.Cast(HOLY_WORD_CHASTISE, target);
                }

                if (utils.CanCast(SMITE) && !Me.IsMoving)
                {
                    if (!Me.IsFacing(target))
                    {
                        utils.LogActivity(FACING, target.Name);
                        Me.SetFacing(target);
                    }
                    utils.LogActivity(SMITE, target.Name);
                    return utils.Cast(SMITE, target);
                }
                if (SoloBotType && (!target.InLineOfSpellSight || !target.InLineOfSight || target.Distance > HolyPriestSettings.Instance.PullDistance))
                {
                    movement.KingHealMove(target.Location, HolyPriestSettings.Instance.PullDistance, true, true, target);
                }
            }
            return false;
        }

        private bool ProcWork()
        {
            //Surge of Light
            if (utils.isAuraActive(SURGE_OF_LIGHT, Me))
            {
                WoWUnit healTarget = utils.GetHealTarget(40f);
                if (healTarget != null)
                {
                    double hp = healTarget.HealthPercent;
                    if (healTarget.Distance > 40 || !healTarget.InLineOfSight)
                    {
                        Logging.Write("GetHealTarget give me a wrong target!!");
                        return false;
                    }
                    else
                    {
                        //Surge of Light
                        if (utils.CanCast(FLASH_HEAL) && (hp <= HolyPriestSettings.Instance.SoLPercent
                                                          || Me.GetAuraByName(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                                                          || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                        {
                            utils.LogActivity("Surge of light proc:"+FLASH_HEAL, healTarget.Class.ToString());
                            return utils.Cast(FLASH_HEAL, healTarget);
                        }
                    }
                }
            }
            //Prayer of mending on Divine Insight proc
            if(utils.isAuraActive(DIVINE_INSIGHT,Me) && utils.CanCast(PRAYER_OF_MENDING))
            {
                WoWPlayer pom_target = utils.BestPOM_PROC_DIVINE_INSIGHT_Target(HolyPriestSettings.Instance.PrayerOfMendingPercent, HolyPriestSettings.Instance.PrayerOfMendingNumber);
                if (pom_target != null)
                {
                    utils.LogActivity("Divine insight proc:" + PRAYER_OF_MENDING, pom_target.Class.ToString());
                    return utils.Cast(PRAYER_OF_MENDING,pom_target);
                }
            }
            return false;
        }

        private bool CastHW_Sanctuary(bool lockframe)
        {
            WoWPlayer hws_target = null;
            if (!Me.IsMoving && !WoWSpell.FromId(88685).Cooldown && utils.isAuraActive(CHAKRA_SANCTUARY,Me))
            {
                if (HolyPriestSettings.Instance.HWSanctuaryOnTank && tank != null && tank.InLineOfSight && tank.Distance <= 40 &&
                    (utils.AOEHealCount((WoWPlayer)tank, HolyPriestSettings.Instance.HWSanctuaryPercent, utils.PW_SanctuaryRadius()) >= HolyPriestSettings.Instance.HWSanctuaryNumber))
                {
                    hws_target = (WoWPlayer)tank;
                }
                else if (!HolyPriestSettings.Instance.HWSanctuaryOnTank)
                {
                    hws_target = utils.BestHWSanctuaryTargetLocation(lockframe,HolyPriestSettings.Instance.HWSanctuaryPercent, HolyPriestSettings.Instance.HWSanctuaryNumber);
                }
                if (hws_target != null)
                {
                    utils.LogActivity(HOLY_WORD_SANCTUARY, hws_target.Class.ToString());
                    utils.Cast(HOLY_WORD_SANCTUARY);
                    return SpellManager.ClickRemoteLocation(hws_target.Location);
                }
            }
            return false;
        }
    }
}
