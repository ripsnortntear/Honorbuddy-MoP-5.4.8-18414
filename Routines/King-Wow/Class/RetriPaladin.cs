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
    public class RetriPaladinCombatClass : KingWoWAbstractBaseClass
    {

        private static string Name = "KingWoW ProtPaladin";


        #region CONSTANT AND VARIABLES

        //START OF CONSTANTS ==============================
        private const string FACING = "FACING";
        private const string TANK_CHANGE = "TANK CHANGED";

        //START OF SPELLS AND AURAS ==============================
        private const string DRINK = "Drink";
        private const string FOOD = "Food";

        // AURA
        private const string GRAND_CRUSADER = "Grand Crusader";
        private const string BASTION_OF_GLORY = "Bastion of Glory";
        private const string THE_ART_OF_WAR = "The Art of War";
        private const string INQUISITION = "Inquisition";

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
        //private const string AVENGERS_SHIELD = "Avenger's Shield";
        private const string HAMMER_OF_RIGHTEOUS = "Hammer of the Righteous";
        private const string HAMMER_OF_WRATH = "Hammer of Wrath";
        //private const string CONSECRATION = "Consecration";
        //private const string HOLY_WRATH = "Holy Wrath";
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
        private const string TEMPLARS_VERDICT = "Templar's Verdict";
        private const string DIVINE_STORM = "Divine Storm";
        private const string EXORCISM = "Exorcism";
        private const string EMANCIPATE = "Emancipate";

        //TALENT
        private const string ETERNAL_FLAME = "Eternal Flame";  //same use of Word of Glory
        private const string SACRED_SHIELD = "Sacred Shield";
        private const string SELFLESS_HEALER = "Selfless Healer"; //Flash of Light on 3 stack

        private const string HOLY_AVENGER = "Holy Avenger";
        private const string SANCTIFIED_WRATH = "Sanctified Wrath";
        private const string DIVINE_PURPOSE = "Divine Purpose";

        private const string EXECUTION_SENTENCE = "Execution Sentence";
        private const string HOLY_PRISM = "Holy Prism";
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
        private WoWUnit tank = null;
        private WoWUnit lastTank = null;
        //END OF CONSTANTS ==============================

        #endregion

        public double CurrentAttackPower()
        {
            string command = "local base, posBuff, negBuff = UnitAttackPower(\"player\"); return base + posBuff + negBuff";
            int currAP = Lua.GetReturnVal<int>(command, 0);
            return currAP;
        }

        public RetriPaladinCombatClass()
        {
            utils = new KingWoWUtility();
            movement = new Movement();
            extra = new ExtraUtils();
            tank = null;
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
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld || utils.IsGlobalCooldown(true) || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) || Me.IsChanneling || utils.MeIsCastingWithLag())
                    return false;

                //UPDATE TANK
                //tank = utils.GetTank();
                tank = utils.SimpleGetTank(40f);
                //if (tank == null || !tank.IsValid || !tank.IsAlive) tank = Me;

                if (tank != null && (lastTank == null || lastTank.Guid != tank.Guid))
                {
                    lastTank = tank;
                    utils.LogActivity(TANK_CHANGE, tank.Class.ToString());
                }
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
                UseCD();
                ProcHandler();
                Emancipate();
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
                if (Me.ManaPercent <= RetriPaladinSettings.Instance.ManaPercent &&
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
                if (Me.HealthPercent <= RetriPaladinSettings.Instance.HealthPercent &&
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
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld || utils.IsGlobalCooldown(true) || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) || Me.IsChanneling || utils.MeIsCastingWithLag())
                    return false;

                //UPDATE TANK
                //tank = utils.GetTank();
                tank = utils.SimpleGetTank(40f);
                //if (tank == null || !tank.IsValid || !tank.IsAlive) tank = Me;

                if (tank != null && (lastTank == null || lastTank.Guid != tank.Guid))
                {
                    lastTank = tank;
                    utils.LogActivity(TANK_CHANGE, tank.Class.ToString());
                }

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
                WoWUnit target = utils.getTargetToAttack(30, tank);
                if (target != null && !target.IsDead)
                {
                    if (RetriPaladinSettings.Instance.AutoTarget || SoloBotType)
                        target.Target();
                    if (SoloBotType)
                    {
                        if (!Me.IsMoving)
                            Me.SetFacing(target);
                        if (!target.InLineOfSight || target.InLineOfSpellSight || target.Distance2DSqr > (RetriPaladinSettings.Instance.PullDistance * RetriPaladinSettings.Instance.PullDistance))
                        {
                            movement.KingHealMove(target.Location, RetriPaladinSettings.Instance.PullDistance, true, true, target);
                        }
                    }

                    if (utils.CanCast(EXORCISM, target, true))
                    {
                        utils.LogActivity(EXORCISM, target.Name);
                        return utils.Cast(EXORCISM, target);
                    }

                    if (utils.CanCast(JUDGEMENT, target, true))
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
            if (RetriPaladinSettings.Instance.BlessingToUse == RetriPaladinSettings.BlessingType.KING)
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

            if (RetriPaladinSettings.Instance.BlessingToUse == RetriPaladinSettings.BlessingType.MIGTH)
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
            //Flash of Light free
            if (Me.HealthPercent <= RetriPaladinSettings.Instance.ProcAutoHealHPSelfless && StyxWoW.Me.ActiveAuras.ContainsKey(SELFLESS_HEALER) && utils.GetAuraStack(Me, SELFLESS_HEALER, true) == 3)
            {
                utils.LogActivity("SELFLESS_HEALER PROC::" + FLASH_OF_LIGHT, Me.Class.ToString());
                return utils.Cast(FLASH_OF_LIGHT, Me);
            }
            if (StyxWoW.Me.ActiveAuras.ContainsKey(DIVINE_PURPOSE) && Me.HealthPercent <= RetriPaladinSettings.Instance.ProcAutoHealHP_EF_WoG)
            {
                if (utils.CanCast(ETERNAL_FLAME))
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
            #endregion

            #region heal other party player
            //Flash of Light free
            if (RetriPaladinSettings.Instance.UseHealingPartyMember && StyxWoW.Me.ActiveAuras.ContainsKey(SELFLESS_HEALER) && utils.GetAuraStack(Me, SELFLESS_HEALER, true) == 3)
            {
                WoWUnit healTarget = utils.GetHealTarget(40f);
                if (healTarget != null && healTarget.HealthPercent <= RetriPaladinSettings.Instance.ProcAutoHealHPSelfless)
                {
                    utils.LogActivity("SELFLESS_HEALER PROC::" + FLASH_OF_LIGHT, healTarget.Class.ToString());
                    return utils.Cast(FLASH_OF_LIGHT, healTarget);
                }
            }
            #endregion

            #region DPS proc
            
            if (StyxWoW.Me.ActiveAuras.ContainsKey(DIVINE_PURPOSE))
            {
                int enemy_around = utils.AllAttaccableEnemyMobsInRange(10).Count();
                //inquisition
                if (!Me.HasAura(84963) || utils.MyAuraTimeLeft("Inquisition", Me) <= 3000)
                {
                    utils.LogActivity("DIVINE_PURPOSE PROC::" + INQUISITION);
                    return utils.Cast(INQUISITION);
                }
                //templar's Verdict
                else
                {
                    WoWUnit target = utils.getTargetToAttack(30, tank);
                    if (target != null && !target.IsDead)
                    {
                        if (RetriPaladinSettings.Instance.AutoTarget || SoloBotType)
                            target.Target();
                        if ((RetriPaladinSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                        {
                            Me.SetFacing(target);
                        }
                        //Divine storm?
                        if (enemy_around >= RetriPaladinSettings.Instance.AOE_Divine_Storm)
                        {
                            utils.LogActivity("DIVINE_PURPOSE PROC::" + DIVINE_STORM, target.Name);
                            return utils.Cast(DIVINE_STORM, target);
                        }
                        //templar's Verdict
                        else if (utils.CanCast(TEMPLARS_VERDICT, target, true))
                        {
                            utils.LogActivity("DIVINE_PURPOSE PROC::" + TEMPLARS_VERDICT, target.Name);
                            return utils.Cast(TEMPLARS_VERDICT, target);
                        }
                    }
                }
            }
            //exorcism
            if (StyxWoW.Me.ActiveAuras.ContainsKey(THE_ART_OF_WAR))
            {
                WoWUnit target = utils.getTargetToAttack(30, tank);
                if (target != null && !target.IsDead && utils.CanCast(EXORCISM,target,true))
                {
                    if (RetriPaladinSettings.Instance.AutoTarget || SoloBotType)
                        target.Target();
                
                    if ((RetriPaladinSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                    {
                        Me.SetFacing(target);
                    }
                    utils.LogActivity("THE ART OF WAR PROC::" + EXORCISM, target.Name);
                    return utils.Cast(EXORCISM, target);
            }

            }
            #endregion

           
            return false;

        }

        private bool Emancipate()
        {
            if (RetriPaladinSettings.Instance.UseEmancipate && Me.ManaPercent > 20 && utils.HasRootSnare(Me) && utils.CanCast(EMANCIPATE))
            {
                utils.LogActivity(EMANCIPATE);
                return utils.Cast(EMANCIPATE);
            }
            return false;
        }

        private bool SOS()
        {

            #region SOS ME
            if (Me.HealthPercent <= RetriPaladinSettings.Instance.LoHhp && utils.CanCast(LAY_ON_HANDS) && !utils.isAuraActive(FORBEARANCE, Me))
            {
                utils.LogActivity(LAY_ON_HANDS, Me.Class.ToString());
                return utils.Cast(LAY_ON_HANDS, Me);
            }
            if (Me.HealthPercent <= RetriPaladinSettings.Instance.DShp && utils.CanCast(DIVINE_SHIELD) && !utils.isAuraActive(FORBEARANCE, Me))
            {
                utils.LogActivity(DIVINE_SHIELD, Me.Class.ToString());
                return utils.Cast(DIVINE_SHIELD, Me);
            }
            if (Me.HealthPercent <= RetriPaladinSettings.Instance.HoPHp && utils.CanCast(HAND_OF_PROTECTION) && !utils.isAuraActive(FORBEARANCE, Me))
            {
                utils.LogActivity(HAND_OF_PROTECTION, Me.Class.ToString());
                return utils.Cast(HAND_OF_PROTECTION, Me);
            }

            current_AP = CurrentAttackPower();

            if (utils.CanCast(SACRED_SHIELD) && !utils.isAuraActive(SACRED_SHIELD))
            {
                previus_AP = current_AP;
                utils.LogActivity(SACRED_SHIELD + " current_AP=" + previus_AP + " I will recast it if AP>=" + previus_AP * (1 + (RetriPaladinSettings.Instance.AttackPowerIncrement / 100)));
                return utils.Cast(SACRED_SHIELD, Me);
            }
            if (utils.CanCast(SACRED_SHIELD) && utils.isAuraActive(SACRED_SHIELD) && (current_AP >= (previus_AP * (1 + (RetriPaladinSettings.Instance.AttackPowerIncrement / 100)))))
            {
                utils.LogActivity(SACRED_SHIELD + " RECAST: previus_AP=" + previus_AP + "    current_AP=" + current_AP);
                previus_AP = current_AP;
                return utils.Cast(SACRED_SHIELD, Me);
            }
            if (Me.HealthPercent <= RetriPaladinSettings.Instance.HandOfPurity && SpellManager.CanBuff(114039, Me))
            {
                utils.LogActivity(HAND_OF_PURITY, Me.Class.ToString());
                SpellManager.CastSpellById(114039);
                return true;
                //return utils.Cast(HAND_OF_PURITY, Me);
            }
            if (Me.HealthPercent <= RetriPaladinSettings.Instance.DivineProtection && utils.CanCast(DIVINE_PROTECTION))
            {
                utils.LogActivity(DIVINE_PROTECTION, Me.Class.ToString());
                return utils.Cast(DIVINE_PROTECTION, Me);
            }
            #endregion

            #region SOS for party members
            if (RetriPaladinSettings.Instance.UseLoHOnPartyMember ||
                RetriPaladinSettings.Instance.UseHoPOnPartyMember ||
                RetriPaladinSettings.Instance.UseDSOnPartyMember)
            {
                WoWUnit healTarget = utils.GetHealTarget(40f);
                if (healTarget != null)
                {
                    //LAY_ON_HAND
                    if (healTarget.HealthPercent < RetriPaladinSettings.Instance.LoHhp &&
                        RetriPaladinSettings.Instance.UseLoHOnPartyMember &&
                        !utils.isAuraActive(FORBEARANCE, healTarget) && utils.CanCast(LAY_ON_HANDS))
                    {
                        utils.LogActivity(LAY_ON_HANDS, healTarget.Class.ToString());
                        return utils.Cast(LAY_ON_HANDS, healTarget);
                    }
                    //DIVINE_SHIELD
                    if (healTarget.HealthPercent < RetriPaladinSettings.Instance.DShp &&
                        RetriPaladinSettings.Instance.UseDSOnPartyMember &&
                        !utils.isAuraActive(FORBEARANCE, healTarget) && utils.CanCast(DIVINE_SHIELD))
                    {
                        utils.LogActivity(DIVINE_SHIELD, healTarget.Class.ToString());
                        return utils.Cast(DIVINE_SHIELD, healTarget);
                    }
                    //HAND_OF_PROTECTION
                    if (healTarget.HealthPercent < RetriPaladinSettings.Instance.HoPHp &&
                        RetriPaladinSettings.Instance.UseHoPOnPartyMember &&
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

        private bool UseCD()
        {
            if (Me.Combat && Me.GotTarget && Me.HasAura(84963))
            {
                if (utils.CanCast(AVENGING_WRATH) && RetriPaladinSettings.Instance.CDUseAvengingWrath == RetriPaladinSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(AVENGING_WRATH);
                    return utils.Cast(AVENGING_WRATH);
                }
                if (utils.CanCast(GUARDIAN_OF_ANCIENT_KING) && RetriPaladinSettings.Instance.CDUseGuardian_of_Ancient_Kings == RetriPaladinSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(GUARDIAN_OF_ANCIENT_KING);
                    return utils.Cast(GUARDIAN_OF_ANCIENT_KING);
                }
                if (utils.CanCast(HOLY_AVENGER) && RetriPaladinSettings.Instance.CDUseHoly_Avenger == RetriPaladinSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(HOLY_AVENGER);
                    return utils.Cast(HOLY_AVENGER);
                }

                if (extra.IsTargetBoss())
                {
                    if (utils.CanCast(AVENGING_WRATH) && RetriPaladinSettings.Instance.CDUseAvengingWrath == RetriPaladinSettings.CDUseType.BOSS)
                    {
                        utils.LogActivity(AVENGING_WRATH);
                        return utils.Cast(AVENGING_WRATH);
                    }
                    if (utils.CanCast(GUARDIAN_OF_ANCIENT_KING) && RetriPaladinSettings.Instance.CDUseGuardian_of_Ancient_Kings == RetriPaladinSettings.CDUseType.BOSS)
                    {
                        utils.LogActivity(GUARDIAN_OF_ANCIENT_KING);
                        return utils.Cast(GUARDIAN_OF_ANCIENT_KING);
                    }
                    if (utils.CanCast(HOLY_AVENGER) && RetriPaladinSettings.Instance.CDUseHoly_Avenger == RetriPaladinSettings.CDUseType.BOSS)
                    {
                        utils.LogActivity(HOLY_AVENGER);
                        return utils.Cast(HOLY_AVENGER);
                    }

                }
            }
            return false;
        }

        private bool SoloRotation()
        {
            int enemy_around = utils.AllAttaccableEnemyMobsInRange(8).Count();
            

            #region Seal cast
            if (RetriPaladinSettings.Instance.SealToUse == RetriPaladinSettings.SealType.AUTO)
            {
                if (enemy_around >= RetriPaladinSettings.Instance.AOE_Seal_Of_Righteousness && !utils.isAuraActive(SEAL_OF_RIGHTEOUSNESS) && Me.ManaPercent > 30)
                {
                    utils.LogActivity(SEAL_OF_RIGHTEOUSNESS);
                    return utils.Cast(SEAL_OF_RIGHTEOUSNESS);
                }
                else if (enemy_around < RetriPaladinSettings.Instance.AOE_Seal_Of_Righteousness && !utils.isAuraActive(SEAL_OF_TRUTH))
                {
                    utils.LogActivity(SEAL_OF_TRUTH);
                    return utils.Cast(SEAL_OF_TRUTH);
                }
            }
            else if (RetriPaladinSettings.Instance.SealToUse == RetriPaladinSettings.SealType.INSIGHT &&
                !utils.isAuraActive(SEAL_OF_INSIGHT))
            {
                utils.LogActivity(SEAL_OF_INSIGHT);
                return utils.Cast(SEAL_OF_INSIGHT);
            }
            else if (RetriPaladinSettings.Instance.SealToUse == RetriPaladinSettings.SealType.TRUTH &&
                !utils.isAuraActive(SEAL_OF_TRUTH))
            {
                utils.LogActivity(SEAL_OF_TRUTH);
                return utils.Cast(SEAL_OF_TRUTH);
            }
            else if (RetriPaladinSettings.Instance.SealToUse == RetriPaladinSettings.SealType.RIGHTEOUSNESS &&
                !utils.isAuraActive(SEAL_OF_RIGHTEOUSNESS))
            {
                utils.LogActivity(SEAL_OF_RIGHTEOUSNESS);
                return utils.Cast(SEAL_OF_RIGHTEOUSNESS);
            }
            #endregion

            #region HEAL ME
            if (Me.Combat && Me.HealthPercent <= RetriPaladinSettings.Instance.AutoHealHP)
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
            if (RetriPaladinSettings.Instance.UseHealingPartyMember)
            {
                WoWUnit healTarget = utils.GetHealTarget(40f);
                if (healTarget != null)
                {
                    if (healTarget.HealthPercent < RetriPaladinSettings.Instance.AutoHealHP &&
                        RetriPaladinSettings.Instance.UseHealingPartyMember &&
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
                }
            }
            #endregion

            #region DPS
            if (!utils.isAuraActive(HAND_OF_PROTECTION, Me))
            {
                WoWUnit target = utils.getTargetToAttack(30, tank);
                if (target != null && !target.IsDead)
                {
                    if (RetriPaladinSettings.Instance.AutoTarget || SoloBotType)
                        target.Target();
                    if ((RetriPaladinSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                    {
                        Me.SetFacing(target);
                    }
                    double effective_range = target.Distance - target.CombatReach - 1;
                    if (RetriPaladinSettings.Instance.AutoInterrupt && target != null && (target.IsCasting || target.IsChanneling) && target.CanInterruptCurrentSpellCast)
                    {
                        if (utils.CanCast(FIST_OF_JUSTICE, target, true) && effective_range <= 20)
                        {
                            utils.LogActivity(FIST_OF_JUSTICE, target.Name);
                            return utils.Cast(FIST_OF_JUSTICE, target);
                        }
                        if (utils.CanCast(HAMMER_OF_JUSTICE, target, true) && effective_range <= 10)
                        {
                            utils.LogActivity(HAMMER_OF_JUSTICE, target.Name);
                            return utils.Cast(HAMMER_OF_JUSTICE, target);
                        }
                        if (utils.CanCast(REBUKE, target, true) && effective_range <= 5)
                        {
                            utils.LogActivity(REBUKE, target.Name);
                            return utils.Cast(REBUKE, target);
                        }
                    }

                    if (SoloBotType && (!target.InLineOfSight || !target.InLineOfSpellSight || !target.IsWithinMeleeRange))
                    {
                        movement.KingHealMove(target.Location, 5f, true, true, target);
                    }

                    //inquisition
                    if ((!Me.HasAura(84963) || utils.MyAuraTimeLeft("Inquisition", Me) <= 3000) && Me.CurrentHolyPower >=3)
                    {
                        utils.LogActivity(INQUISITION);
                        return utils.Cast(INQUISITION);
                    }


                    //T6 talent
                    if (utils.CanCast(EXECUTION_SENTENCE, target, true) && effective_range <= 40)
                    {
                        utils.LogActivity(EXECUTION_SENTENCE, target.Name);
                        return utils.Cast(EXECUTION_SENTENCE, target);
                    }
                    
                    if (utils.CanCast(LIGHTS_HAMMER) && effective_range <= 30)
                    {
                        utils.LogActivity(LIGHTS_HAMMER, target.Name);
                        utils.Cast(LIGHTS_HAMMER, target);
                        return SpellManager.ClickRemoteLocation(target.Location);
                    }

                    if(Me.CurrentHolyPower > 4 )
                    {
                        //Divine storm?
                        if (enemy_around >= RetriPaladinSettings.Instance.AOE_Divine_Storm)
                        {
                            utils.LogActivity(DIVINE_STORM,target.Name);
                            return utils.Cast(DIVINE_STORM, target);
                        }
                        //templar's Verdict
                        else if (utils.CanCast(TEMPLARS_VERDICT,target,true) && effective_range <= 5)
                        {
                            utils.LogActivity(TEMPLARS_VERDICT,target.Name);
                            return utils.Cast(TEMPLARS_VERDICT,target);
                        }
                    }
                    if (utils.CanCast(HAMMER_OF_WRATH, target, true) && effective_range <= 30)
                    {
                        utils.LogActivity(HAMMER_OF_WRATH, target.Name);
                        return utils.Cast(HAMMER_OF_WRATH, target);
                    }

                    if (utils.CanCast(EXORCISM, target, true) && effective_range <= 30)
                    {
                        utils.LogActivity(EXORCISM, target.Name);
                        return utils.Cast(EXORCISM, target);
                    }
                    
                    if (enemy_around >= RetriPaladinSettings.Instance.AOE_Hammer_Of_The_Righteous && utils.CanCast(HAMMER_OF_RIGHTEOUS, target, true))
                    {
                        utils.LogActivity(HAMMER_OF_RIGHTEOUS, target.Name);
                        return utils.Cast(HAMMER_OF_RIGHTEOUS, target);
                    }
                    else if (enemy_around < RetriPaladinSettings.Instance.AOE_Hammer_Of_The_Righteous && utils.CanCast(CRUSADER_STRIKE,target,true) && effective_range <= 5)
                    {
                        utils.LogActivity(CRUSADER_STRIKE, target.Name);
                        return utils.Cast(CRUSADER_STRIKE, target);
                    }

                    if (utils.CanCast(JUDGEMENT, target, true) && effective_range <= 30)
                    {
                        utils.LogActivity(JUDGEMENT, target.Name);
                        return utils.Cast(JUDGEMENT, target);
                    }
                    if (utils.CanCast(HOLY_PRISM, target, true))//&& effective_range <= 40)
                    {
                        if (utils.AllAttaccableEnemyMobsInRange(15).Count() > 1)
                        {
                            utils.LogActivity(HOLY_PRISM + "on Me for AOE");
                            return utils.Cast(HOLY_PRISM, Me);
                        }
                        else if (effective_range <= 30)
                        {
                            utils.LogActivity(HOLY_PRISM, target.Name);
                            return utils.Cast(HOLY_PRISM, target);
                        }
                    }
                    if(Me.CurrentHolyPower == 3 )
                    {
                        //Divine storm?
                        if (enemy_around >= RetriPaladinSettings.Instance.AOE_Divine_Storm)
                        {
                            utils.LogActivity(DIVINE_STORM,target.Name);
                            return utils.Cast(DIVINE_STORM, target);
                        }
                        //templar's Verdict
                        else if (utils.CanCast(TEMPLARS_VERDICT, target, true) && effective_range <= 5)
                        {
                            utils.LogActivity(TEMPLARS_VERDICT,target.Name);
                            return utils.Cast(TEMPLARS_VERDICT,target);
                        }
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
                    else if (utils.CanCast(RESURRECTION, player) && RetriPaladinSettings.Instance.UseRedemption && !Me.IsMoving)
                    {
                        utils.LogActivity(RESURRECTION, player.Class.ToString());
                        Blacklist.Add(player, BlacklistFlags.All, new TimeSpan(0, 0, 60));
                        return utils.Cast(RESURRECTION, player);
                    }

                    return false;
                }
            }

            return false;
        }
    }
}
