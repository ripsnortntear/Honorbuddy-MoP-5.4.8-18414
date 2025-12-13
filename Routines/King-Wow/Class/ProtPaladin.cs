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
    public class ProtPaladinCombatClass : KingWoWAbstractBaseClass
    {

        private static string Name = "KingWoW ProtPaladin";


        #region CONSTANT AND VARIABLES

        //START OF CONSTANTS ==============================
        private const string FACING = "FACING";

        //START OF SPELLS AND AURAS ==============================
        private const string DRINK = "Drink";
        private const string FOOD = "Food";

        // AURA
        private const string GRAND_CRUSADER = "Grand Crusader";
        private const string BASTION_OF_GLORY = "Bastion of Glory";
        private const string RIGHTEOUS_FURY = "Righteous Fury";  

        //SEAL
        private const string SEAL_OF_INSIGHT = "Seal of Insight";
        private const string SEAL_OF_TRUTH = "Seal of Truth";
        private const string SEAL_OF_RIGHTEOUSNESS = "Seal of Righteousness";

        //BLESSING
        private const string BLESSING_OF_KINGS = "Blessing of Kings";
        private const string BLESSING_OF_MIGHT = "Blessing of Might";
        
        //HAND
        private const string HAND_OF_FREEDOM = "Hand of Freedom";
        private const string HAND_OF_PROTECTION = "Hand of Protection";
        private const string HAND_OF_SACRIFICE = "Hand of Sacrifice";
        private const string HAND_OF_SALVATION = "Hand of Salvation";
                   
        
        private const string CRUSADER_STRIKE = "Crusader Strike";
        private const string JUDGEMENT = "Judgment";
        private const string AVENGERS_SHIELD  = "Avenger's Shield";
        private const string HAMMER_OF_RIGHTEOUS = "Hammer of the Righteous";
        private const string HAMMER_OF_WRATH = "Hammer of Wrath";
        private const string CONSECRATION = "Consecration";
        private const string HOLY_WRATH = "Holy Wrath";
        private const string SHIELD_OF_RIGHTEOUS = "Shield of the Righteous";
        private const string WORD_OF_GLORY = "Word of Glory";
        private const string RECKONING = " Reckoning ";
        private const string GUARDIAN_OF_ANCIENT_KING = "Guardian of Ancient Kings";
        private const string ARDENT_DEFENDER = "Ardent Defender";
        private const string DIVINE_PROTECTION = "Divine Protection";
        private const string AVENGING_WRATH = "Avenging Wrath";
        private const string REBUKE = "Rebuke";
        private const string FIST_OF_JUSTICE = "Fist of Justice";     
        private const string HAMMER_OF_JUSTICE = "Hammer of Justice";
        private const string CLEANSE = "Cleanse";      
        private const string FLASH_OF_LIGHT = "Flash of Light";
        private const string LAY_ON_HANDS = "Lay on Hands";
        private const string DIVINE_SHIELD = "Divine Shield";
        private const string RESURRECTION = "Redemption";
        private const string HAND_OF_PURITY = "Hand Of Purity";

        //TALENT
        private const string ETERNAL_FLAME = "Eternal Flame";  //same use of Word of Glory
        private const string SACRED_SHIELD = "Sacred Shield"; 
        private const string SELFLESS_HEALER = "Selfless Healer"; //Flash of Light on 3 stack

        private const string HOLY_AVENGER = "Holy Avenger";
        private const string SANCTIFIED_WRATH = "Sanctified Wrath";
        private const string DIVINE_PURPOSE = "Divine Purpose";

        private const string EXECUTION_SENTENCE = "Execution Sentence";
        private const string HOLY_PRISM= "Holy Prism";
        private const string LIGHTS_HAMMER = "Light's Hammer";

        private const string FORBEARANCE = "Forbearance"; 
        //END OF SPELLS AND AURAS ==============================
        private KingWoWUtility utils = null;
        private ExtraUtils extra = null;
        private Movement movement = null;
        private bool SoloBotType = false;
        private string BaseBot = "unknown";
        private double previus_AP = 0;
        private double current_AP = 0;
        //END OF CONSTANTS ==============================

        #endregion

        public double CurrentAttackPower()
        {
            string command = "local base, posBuff, negBuff = UnitAttackPower(\"player\"); return base + posBuff + negBuff";
            int currAP = Lua.GetReturnVal<int>(command, 0);
            return currAP;       
        }

        public ProtPaladinCombatClass()
        {
            utils = new KingWoWUtility();
            movement = new Movement();
            extra = new ExtraUtils();
            SoloBotType = false;
            BaseBot = "unknown";
            previus_AP = 0;
            current_AP = 0;
        }

        public override bool Combat
        {
            get
            {
                extra.AnyKeyBindsPressed();
                extra.UseHealthstone();
                extra.UseRacials();
                extra.UseTrinket1();
                extra.UseTrinket2();
                extra.UseEngineeringGloves();
                extra.UseLifeblood();
                extra.UseAlchemyFlask();
                extra.WaterSpirit();
                extra.LifeSpirit();

                SOS();
                ProcHandler();
                SoloRotation();
                return false;
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
            //talents.Update();
            BotUpdate();
        }

        public override bool NeedCombatBuffs { get { return Buff(); } }

        public override bool NeedPreCombatBuffs { get { return Buff(); } }

        public override bool NeedPullBuffs { get { return Buff(); } }

        public override bool NeedRest
        {
            get
            {
                if ((utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD)) && (Me.ManaPercent < 100 || Me.HealthPercent < 100))
                    return true;
                if (Me.ManaPercent <= ProtPaladinSettings.Instance.ManaPercent &&
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
                if (Me.HealthPercent <= ProtPaladinSettings.Instance.HealthPercent &&
                !utils.isAuraActive(FOOD) && !Me.Combat && !Me.IsMoving && !Me.IsCasting)
                {
                    WoWItem myfood = Consumable.GetBestFood(false);
                    if (myfood != null)
                    {
                        utils.LogActivity("Eating");
                        Styx.CommonBot.Rest.DrinkImmediate();
                        return false;
                    }
                }
                return false;
            }
        }

        public override bool Pulse
        {
            get
            {
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld /*|| utils.IsGlobalCooldown(true)*/ || utils.MeIsChanneling || Me.IsCasting
                || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD))
                    return false;
                //try full me untill mana >= 60%
                if (!Me.Combat && !Me.Mounted && !utils.isAuraActive(DRINK) && !utils.isAuraActive(FOOD) && Me.ManaPercent >= 60 && Me.HealthPercent < 80)
                {
                    Resurrect();
                    if (utils.CanCast(ETERNAL_FLAME))
                    {
                        utils.LogActivity(ETERNAL_FLAME, Me.Class.ToString());
                        return utils.Cast(ETERNAL_FLAME, Me);
                    }
                    if (utils.CanCast(WORD_OF_GLORY))
                    {
                        utils.LogActivity(WORD_OF_GLORY, Me.Class.ToString());
                        return utils.Cast(WORD_OF_GLORY, Me);
                    }
                    if (utils.CanCast(FLASH_OF_LIGHT))
                    {
                        utils.LogActivity(FLASH_OF_LIGHT, Me.Class.ToString());
                        return utils.Cast(FLASH_OF_LIGHT, Me);
                    }
                }
                return false;
            }
        }

        public override bool Pull
        {
            get
            {
                WoWUnit target = Me.CurrentTarget;
                if (target != null && !target.IsDead)
                {
                    if (SoloBotType)
                    {
                        if(!Me.IsMoving)
                            Me.SetFacing(target);
                        if (!target.InLineOfSight || target.InLineOfSpellSight || target.Distance2DSqr > (ProtPaladinSettings.Instance.PullDistance * ProtPaladinSettings.Instance.PullDistance))
                        {
                            movement.KingHealMove(target.Location, ProtPaladinSettings.Instance.PullDistance, true, true, target);
                        }
                    }
                    
                    if (utils.CanCast(AVENGERS_SHIELD,target,true))
                    {
                        utils.LogActivity(AVENGERS_SHIELD, target.Name);
                        return utils.Cast(AVENGERS_SHIELD, target);
                    }

                    if (utils.CanCast(JUDGEMENT,target,true))
                    {
                        utils.LogActivity(JUDGEMENT, target.Name);
                        return utils.Cast(JUDGEMENT, target);
                    }
                }
                return false;
            }
        }

        private bool Buff()
        {
            if (utils.Mounted())
                return false;
            if (ProtPaladinSettings.Instance.BlessingToUse == ProtPaladinSettings.BlessingType.MANUAL)
            {
                ; //lol
            } //MANUAL

            if (ProtPaladinSettings.Instance.BlessingToUse == ProtPaladinSettings.BlessingType.KING)
            {
                //KING
                if (!utils.isAuraActive(BLESSING_OF_KINGS, Me) &&
                    !utils.isAuraActive("Mark of the Wild", Me) &&
                    !utils.isAuraActive("Embrace of the Shale Spider", Me) &&
                    !utils.isAuraActive("Legacy of the Emperor", Me) &&
                    utils.CanCast(BLESSING_OF_KINGS))
                {
                    utils.LogActivity(BLESSING_OF_KINGS);
                    return utils.Cast(BLESSING_OF_KINGS);
                }
            }

            if (ProtPaladinSettings.Instance.BlessingToUse == ProtPaladinSettings.BlessingType.MIGTH)
            {
                //MIGTH
                if (!utils.isAuraActive(BLESSING_OF_MIGHT, Me) &&
                    !utils.isAuraActive("Roar of Courage", Me) &&
                    !utils.isAuraActive("Grace of Air", Me) &&
                    !utils.isAuraActive("Spirit Beast Blessing", Me) &&
                    utils.CanCast(BLESSING_OF_MIGHT))
                {
                    utils.LogActivity(BLESSING_OF_MIGHT);
                    return utils.Cast(BLESSING_OF_MIGHT);
                }
            }
            if (!utils.isAuraActive(RIGHTEOUS_FURY) && utils.CanCast(RIGHTEOUS_FURY))
            {
                utils.LogActivity(RIGHTEOUS_FURY);
                return utils.Cast(RIGHTEOUS_FURY);
            }
            return false;
        }

        private bool BotUpdate()
        {
            if (BaseBot.Equals(BotManager.Current.Name))
                return false;
            if (utils.IsBotBaseInUse("LazyRaider") || utils.IsBotBaseInUse("Raid Bot") || utils.IsBotBaseInUse("Tyrael"))
            {
                Logging.Write("Detected LazyRaider/Raid/tyrael Bot:");
                Logging.Write("Disable all movements");
                Movement.DisableAllMovement = true;
                SoloBotType = false;
                BaseBot = BotManager.Current.Name;
                return true;
            }
            if (utils.IsBotBaseInUse("BGBuddy"))
            {
                Logging.Write("Detected BGBuddy Bot:");
                Logging.Write("Enable PVP Rotation");
                Movement.DisableAllMovement = true;
                SoloBotType = false;
                BaseBot = BotManager.Current.Name;
                return true;
            }
            Logging.Write("Base bot detected: " + BotManager.Current.Name);
            SoloBotType = true;
            BaseBot = BotManager.Current.Name;
            return true;
        }

        private bool ProcHandler()
        {
            #region urgent healing first on me
            if (StyxWoW.Me.ActiveAuras.ContainsKey(DIVINE_PURPOSE) && Me.HealthPercent <= ProtPaladinSettings.Instance.ProcAutoHealHP)
            {
                if(utils.CanCast(ETERNAL_FLAME))
                {
                    utils.LogActivity("DIVINE_PURPOSE PROC::" + ETERNAL_FLAME, Me.Class.ToString());
                    return utils.Cast(ETERNAL_FLAME, Me);
                }
                if (utils.CanCast(WORD_OF_GLORY))
                {
                    utils.LogActivity("DIVINE_PURPOSE PROC::" + WORD_OF_GLORY, Me.Class.ToString());
                    return utils.Cast(WORD_OF_GLORY, Me);
                }

            }
            //Flash of Light free
            if (Me.HealthPercent <= ProtPaladinSettings.Instance.ProcAutoHealHP && StyxWoW.Me.ActiveAuras.ContainsKey(SELFLESS_HEALER) && utils.GetAuraStack(Me, SELFLESS_HEALER, true)==3)
            {
                utils.LogActivity("SELFLESS_HEALER PROC::" + FLASH_OF_LIGHT, Me.Class.ToString());
                return utils.Cast(FLASH_OF_LIGHT, Me);
            }
            #endregion

            #region DPS proc
            if (StyxWoW.Me.ActiveAuras.ContainsKey(DIVINE_PURPOSE) || StyxWoW.Me.ActiveAuras.ContainsKey(GRAND_CRUSADER))
            {
                WoWUnit target = Me.CurrentTarget;
                if (target != null && !target.IsDead)
                {
                    if ((ProtPaladinSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                    {
                        Me.SetFacing(target);
                    }
                    if (StyxWoW.Me.ActiveAuras.ContainsKey(DIVINE_PURPOSE) && utils.CanCast(SHIELD_OF_RIGHTEOUS,target,true))
                    {
                        utils.LogActivity("DIVINE_PURPOSE PROC::" + SHIELD_OF_RIGHTEOUS, target.Name);
                        return utils.Cast(SHIELD_OF_RIGHTEOUS, target);
                    }

                    if (StyxWoW.Me.ActiveAuras.ContainsKey(GRAND_CRUSADER) && utils.CanCast(AVENGERS_SHIELD,target,true))
                    {
                        utils.LogActivity("GRAND_CRUSADER PROC::" + AVENGERS_SHIELD, target.Name);
                        return utils.Cast(AVENGERS_SHIELD, target);
                    }
                }

            }
            #endregion

            #region heal other party player
            //Flash of Light free
            if (ProtPaladinSettings.Instance.UseHealingPartyMember && StyxWoW.Me.ActiveAuras.ContainsKey(SELFLESS_HEALER) && utils.GetAuraStack(Me, SELFLESS_HEALER, true) == 3)
            {
                WoWUnit healTarget = utils.GetHealTarget(40f);
                if (healTarget != null)
                {
                    utils.LogActivity("SELFLESS_HEALER PROC::" + FLASH_OF_LIGHT, healTarget.Class.ToString());
                    return utils.Cast(FLASH_OF_LIGHT, healTarget);
                }
            }
            #endregion
            return false;

        }

        private bool SOS()
        {

            #region SOS ME
            if (Me.HealthPercent <= ProtPaladinSettings.Instance.LoHhp && utils.CanCast(LAY_ON_HANDS) && !utils.isAuraActive(FORBEARANCE,Me))
            {
                utils.LogActivity(LAY_ON_HANDS, Me.Class.ToString());
                return utils.Cast(LAY_ON_HANDS, Me);
            }
            if (Me.HealthPercent <= ProtPaladinSettings.Instance.DShp && utils.CanCast(DIVINE_SHIELD) && !utils.isAuraActive(FORBEARANCE, Me))
            {
                utils.LogActivity(DIVINE_SHIELD, Me.Class.ToString());
                return utils.Cast(DIVINE_SHIELD, Me);
            }
            if (Me.HealthPercent <= ProtPaladinSettings.Instance.HoPHp && utils.CanCast(HAND_OF_PROTECTION) && !utils.isAuraActive(FORBEARANCE, Me))
            {
                utils.LogActivity(HAND_OF_PROTECTION, Me.Class.ToString());
                return utils.Cast(HAND_OF_PROTECTION, Me);
            }

            if (Me.HealthPercent <= ProtPaladinSettings.Instance.ArdentDefender && utils.CanCast(ARDENT_DEFENDER))
            {
                utils.LogActivity(ARDENT_DEFENDER, Me.Class.ToString());
                return utils.Cast(ARDENT_DEFENDER, Me);
           }
            if (Me.HealthPercent <= ProtPaladinSettings.Instance.GuardianOfAncientKing && utils.CanCast(GUARDIAN_OF_ANCIENT_KING))
            {
                utils.LogActivity(GUARDIAN_OF_ANCIENT_KING, Me.Class.ToString());
                return utils.Cast(GUARDIAN_OF_ANCIENT_KING, Me);
            }

            current_AP = CurrentAttackPower();

            if (utils.CanCast(SACRED_SHIELD) && !utils.isAuraActive(SACRED_SHIELD))
            {
                previus_AP = current_AP;
                utils.LogActivity(SACRED_SHIELD + " current_AP=" + previus_AP + " I will recast it if AP>=" + previus_AP*(1+(ProtPaladinSettings.Instance.AttackPowerIncrement/100)));
                return utils.Cast(SACRED_SHIELD, Me);
            }
            if (utils.CanCast(SACRED_SHIELD) && utils.isAuraActive(SACRED_SHIELD) && (current_AP >= (previus_AP*(1+(ProtPaladinSettings.Instance.AttackPowerIncrement/100)))) )
            {
                utils.LogActivity(SACRED_SHIELD + " RECAST: previus_AP=" + previus_AP + "    current_AP=" + current_AP);
                previus_AP = current_AP;
                return utils.Cast(SACRED_SHIELD, Me);
            }
            if (Me.HealthPercent <= ProtPaladinSettings.Instance.HandOfPurity && SpellManager.CanBuff(114039, Me))
            {
                utils.LogActivity(HAND_OF_PURITY, Me.Class.ToString());
                SpellManager.CastSpellById(114039);
                return true;
                //return utils.Cast(HAND_OF_PURITY, Me);
            }
            if (Me.HealthPercent <= ProtPaladinSettings.Instance.DivineProtection && utils.CanCast(DIVINE_PROTECTION))
            {
                utils.LogActivity(DIVINE_PROTECTION, Me.Class.ToString());
                return utils.Cast(DIVINE_PROTECTION, Me);
            }
            if (utils.CanCast(AVENGING_WRATH))
            {
                if (ProtPaladinSettings.Instance.AvengersWrathTypeUseMode == ProtPaladinSettings.AvengersWrathType.COOLDOWN)
                {
                    utils.LogActivity(AVENGING_WRATH, Me.Class.ToString());
                    return utils.Cast(AVENGING_WRATH, Me);
                }
                else if (ProtPaladinSettings.Instance.AvengersWrathTypeUseMode == ProtPaladinSettings.AvengersWrathType.CONDITION
                    && current_AP >= ProtPaladinSettings.Instance.AttackPowerForAvengersWrath)
                {
                    utils.LogActivity(AVENGING_WRATH + " current_AP=" + current_AP, Me.Class.ToString());
                    return utils.Cast(AVENGING_WRATH, Me);
                }
            }
            #endregion

            #region SOS for party members
            if (ProtPaladinSettings.Instance.UseLoHOnPartyMember ||
                ProtPaladinSettings.Instance.UseHoPOnPartyMember ||
                ProtPaladinSettings.Instance.UseDSOnPartyMember)
            {
                WoWUnit healTarget = utils.GetHealTarget(40f);
                if (healTarget != null)
                {
                    //LAY_ON_HAND
                    if (healTarget.HealthPercent < ProtPaladinSettings.Instance.LoHhp &&
                        ProtPaladinSettings.Instance.UseLoHOnPartyMember &&
                        !utils.isAuraActive(FORBEARANCE, healTarget) && utils.CanCast(LAY_ON_HANDS))
                    {
                        utils.LogActivity(LAY_ON_HANDS, healTarget.Class.ToString());
                        return utils.Cast(LAY_ON_HANDS, healTarget);
                    }
                    //DIVINE_SHIELD
                    if (healTarget.HealthPercent < ProtPaladinSettings.Instance.DShp &&
                        ProtPaladinSettings.Instance.UseDSOnPartyMember &&
                        !utils.isAuraActive(FORBEARANCE, healTarget) && utils.CanCast(DIVINE_SHIELD))
                    {
                        utils.LogActivity(DIVINE_SHIELD, healTarget.Class.ToString());
                        return utils.Cast(DIVINE_SHIELD, healTarget);
                    }
                    //HAND_OF_PROTECTION
                    if (healTarget.HealthPercent < ProtPaladinSettings.Instance.HoPHp &&
                        ProtPaladinSettings.Instance.UseHoPOnPartyMember &&
                        !utils.isAuraActive(FORBEARANCE, healTarget) && utils.CanCast(HAND_OF_PROTECTION))
                    {
                        utils.LogActivity(HAND_OF_PROTECTION, healTarget.Class.ToString());
                        return utils.Cast(HAND_OF_PROTECTION, healTarget);
                    }
                }
            }
            #endregion
            return false;
        }

        private bool SoloRotation()
        {
            bool AOE = (utils.AllAttaccableEnemyMobsInRange(8).Count() >= ProtPaladinSettings.Instance.AOECount);

            #region Seal cast
            if (ProtPaladinSettings.Instance.SealToUse == ProtPaladinSettings.SealType.AUTO)
            {
                if (AOE && !utils.isAuraActive(SEAL_OF_RIGHTEOUSNESS) && Me.ManaPercent > 30)
                {
                    //utils.LogActivity("AOE TRUE enemy:" + utils.EnemyMeleeUnits.Count());
                    utils.LogActivity(SEAL_OF_RIGHTEOUSNESS);
                    return utils.Cast(SEAL_OF_RIGHTEOUSNESS);
                }
                else if (!AOE && !utils.isAuraActive(SEAL_OF_INSIGHT))
                {
                    //utils.LogActivity("AOE FALSE enemy:" + utils.EnemyMeleeUnits.Count());
                    utils.LogActivity(SEAL_OF_INSIGHT);
                    return utils.Cast(SEAL_OF_INSIGHT);
                }
            }
            else if (ProtPaladinSettings.Instance.SealToUse == ProtPaladinSettings.SealType.INSIGHT && 
                !utils.isAuraActive(SEAL_OF_INSIGHT))
            {
                utils.LogActivity(SEAL_OF_INSIGHT);
                return utils.Cast(SEAL_OF_INSIGHT);
            }
            else if (ProtPaladinSettings.Instance.SealToUse == ProtPaladinSettings.SealType.TRUTH && 
                !utils.isAuraActive(SEAL_OF_TRUTH))
            {
                utils.LogActivity(SEAL_OF_TRUTH);
                return utils.Cast(SEAL_OF_TRUTH);
            }
            else if (ProtPaladinSettings.Instance.SealToUse == ProtPaladinSettings.SealType.RIGHTEOUSNESS &&
                !utils.isAuraActive(SEAL_OF_RIGHTEOUSNESS))
            {
                utils.LogActivity(SEAL_OF_RIGHTEOUSNESS);
                return utils.Cast(SEAL_OF_RIGHTEOUSNESS);
            }
            #endregion

            #region HEAL ME
            if (Me.Combat && Me.HealthPercent <= ProtPaladinSettings.Instance.AutoHealHP)
            {
                if (Me.CurrentHolyPower >= 3)
                {
                    if (utils.CanCast(ETERNAL_FLAME))
                    {
                        {
                            utils.LogActivity(ETERNAL_FLAME, Me.Class.ToString());
                            return utils.Cast(ETERNAL_FLAME, Me);
                        }
                    }
                    if (utils.CanCast(WORD_OF_GLORY))
                    {
                        {
                            utils.LogActivity(WORD_OF_GLORY, Me.Class.ToString());
                            return utils.Cast(WORD_OF_GLORY, Me);
                        }
                    }
                }
                if (utils.CanCast(EXECUTION_SENTENCE))
                {
                    utils.LogActivity(EXECUTION_SENTENCE, Me.Class.ToString());
                    return utils.Cast(EXECUTION_SENTENCE, Me);
                }
                if (utils.CanCast(HOLY_PRISM))
                {
                    utils.LogActivity(HOLY_PRISM, Me.Class.ToString());
                    return utils.Cast(HOLY_PRISM, Me);
                }
                if (utils.CanCast(LIGHTS_HAMMER))
                {
                    utils.LogActivity(LIGHTS_HAMMER, Me.Class.ToString());
                    utils.Cast(LIGHTS_HAMMER);
                    return SpellManager.ClickRemoteLocation(Me.Location);
                }
            }
            #endregion

            #region HEAL OTHER PARTY IF ENABLED
            if (ProtPaladinSettings.Instance.UseHealingPartyMember)
            {
                WoWUnit healTarget = Me.FocusedUnit;
                if(healTarget==null || !healTarget.IsFriendly || healTarget.Distance >= 40)
                    healTarget = utils.GetHealTarget(40f);
                if (healTarget != null)
                {
                    if (healTarget.HealthPercent < ProtPaladinSettings.Instance.AutoHealHP &&
                        ProtPaladinSettings.Instance.UseHealingPartyMember &&
                        Me.CurrentHolyPower >= 3)
                    {
                        if (utils.CanCast(ETERNAL_FLAME))
                        {
                            utils.LogActivity(ETERNAL_FLAME, healTarget.Class.ToString());
                            return utils.Cast(ETERNAL_FLAME, healTarget);
                        }
                        if (utils.CanCast(WORD_OF_GLORY))
                        {
                            utils.LogActivity(WORD_OF_GLORY, healTarget.Class.ToString());
                            return utils.Cast(WORD_OF_GLORY, healTarget);
                        }
                    }
                    /*if (healTarget.HealthPercent < 20 && Me.ManaPercent >= 30)
                    {
                        utils.LogActivity(FLASH_OF_LIGHT, healTarget.Class.ToString());
                        return utils.Cast(FLASH_OF_LIGHT, healTarget);
                    }*/
                }
            }
            #endregion

            #region DPS
            if (!utils.isAuraActive(HAND_OF_PROTECTION, Me))
            {
                WoWUnit target = Me.CurrentTarget;
                if (target != null && !target.IsDead)
                {
                    if ((ProtPaladinSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                    {
                        Me.SetFacing(target);
                    }
                    if (ProtPaladinSettings.Instance.AutoInterrupt && target != null && (target.IsCasting || target.IsChanneling) && target.CanInterruptCurrentSpellCast)
                    {
                        if (utils.CanCast(FIST_OF_JUSTICE, target, true))
                        {
                            utils.LogActivity(FIST_OF_JUSTICE, target.Name);
                            return utils.Cast(FIST_OF_JUSTICE, target);
                        }
                        if (utils.CanCast(REBUKE, target, true))
                        {
                            utils.LogActivity(REBUKE, target.Name);
                            return utils.Cast(REBUKE, target);
                        }
                    }

                    if (SoloBotType && (!target.InLineOfSight || !target.InLineOfSpellSight || !target.IsWithinMeleeRange))
                    {
                        movement.KingHealMove(target.Location, 5f, true, true, target);
                    }

                    if (utils.CanCast(SHIELD_OF_RIGHTEOUS, target, true))
                    {
                        utils.LogActivity(SHIELD_OF_RIGHTEOUS, target.Name);
                        return utils.Cast(SHIELD_OF_RIGHTEOUS, target);
                    }
                    if (utils.CanCast(HAMMER_OF_RIGHTEOUS, target, true) && (!utils.isAuraActive("Weakened Blows", target) || AOE))
                    {
                        utils.LogActivity(HAMMER_OF_RIGHTEOUS, target.Name);
                        return utils.Cast(HAMMER_OF_RIGHTEOUS, target);
                    }
                    if (AOE && utils.CanCast(CONSECRATION) && Me.ManaPercent >= 50)
                    {
                        utils.LogActivity(CONSECRATION, target.Name);
                        return utils.Cast(CONSECRATION, target);
                    }
                    if (AOE && utils.CanCast(HOLY_WRATH))
                    {
                        utils.LogActivity(HOLY_WRATH);
                        return utils.Cast(HOLY_WRATH);
                    }
                    if (!AOE && utils.CanCast(CRUSADER_STRIKE, target, true))
                    {
                        utils.LogActivity(CRUSADER_STRIKE, target.Name);
                        return utils.Cast(CRUSADER_STRIKE, target);
                    }
                    if (utils.CanCast(JUDGEMENT, target, true))
                    {
                        utils.LogActivity(JUDGEMENT, target.Name);
                        return utils.Cast(JUDGEMENT, target);
                    }
                    if (utils.CanCast(AVENGERS_SHIELD, target, true))
                    {
                        utils.LogActivity(AVENGERS_SHIELD, target.Name);
                        return utils.Cast(AVENGERS_SHIELD, target);
                    }
                    if (utils.CanCast(HAMMER_OF_WRATH, target, true))
                    {
                        utils.LogActivity(HAMMER_OF_WRATH, target.Name);
                        return utils.Cast(HAMMER_OF_WRATH, target);
                    }
                    if (utils.CanCast(EXECUTION_SENTENCE, target, true))
                    {
                        utils.LogActivity(EXECUTION_SENTENCE, target.Name);
                        return utils.Cast(EXECUTION_SENTENCE, target);
                    }
                    if (utils.CanCast(HOLY_PRISM, target, true))
                    {
                        utils.LogActivity(HOLY_PRISM, target.Name);
                        return utils.Cast(HOLY_PRISM, target);
                    }
                    if (utils.CanCast(LIGHTS_HAMMER))
                    {
                        utils.LogActivity(LIGHTS_HAMMER, target.Name);
                        utils.Cast(LIGHTS_HAMMER, target);
                        return SpellManager.ClickRemoteLocation(target.Location);
                    }
                    if (utils.CanCast(CONSECRATION) && Me.ManaPercent >= 50)
                    {
                        utils.LogActivity(CONSECRATION, target.Name);
                        return utils.Cast(CONSECRATION, target);
                    }
                    if (utils.CanCast(HOLY_WRATH))
                    {
                        utils.LogActivity(HOLY_WRATH);
                        return utils.Cast(HOLY_WRATH);
                    }
                }
                else if (SoloBotType)
                {
                    target = utils.getTargetToAttack(30, Me);
                    if (target != null)
                        target.Target();
                }
            }
            else
            {
                //other healer spells used above
                if (Me.HealthPercent < 80 && utils.CanCast(FLASH_OF_LIGHT))
                {
                    utils.LogActivity("Hand of Protection active:" + FLASH_OF_LIGHT, Me.Class.ToString());
                    return utils.Cast(FLASH_OF_LIGHT, Me);
                }
            }
            #endregion

            return false;
           
        }

        private bool Resurrect()
        {
            foreach (WoWPlayer player in utils.GetResurrectTargets(40f))
            {
                if (Blacklist.Contains(player.Guid, BlacklistFlags.All)) continue;
                else
                {
                    if (player.Distance > 40 || !player.InLineOfSight) return false;
                    else if (utils.CanCast(RESURRECTION, player) && ProtPaladinSettings.Instance.UseRedemption && !Me.IsMoving)
                    {
                        utils.LogActivity(RESURRECTION, player.Class.ToString());
                        Blacklist.Add(player, BlacklistFlags.All,new TimeSpan(0, 0, 60));
                        return utils.Cast(RESURRECTION, player);
                    }

                    return false;
                }
            }

            return false;
        }
    }
}
