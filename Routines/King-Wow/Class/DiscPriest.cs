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
    public class DiscPriestCombatClass : KingWoWAbstractBaseClass
    {
        
        private static string Name = "KingWoW DisciplinePriest";

        #region CONSTANT AND VARIABLES
        //START OF CONSTANTS AND VARIABLES ==============================
        
        private KingWoWUtility utils = null;
        private Movement movement = null;
        private ExtraUtils extra = null;

        private WoWUnit tank = null;
        private WoWUnit off_tank = null;
        private WoWUnit lastTank = null;
        private WoWUnit lastOffTank = null;
        private bool UsingAtonementHealing = true;
            
        private bool SoloBotType = false;
        private string BaseBot = "unknown";
        private TalentManager talents = null;
        private int currentGroupForSS = 0;

        WoWUnit CurrentHealtarget = null;
        private double RangeOfAttack = 30f;

        private int PenancePercent, BindingHealPercent, HealPercent, GHPercent, FlashHealPercent, SoLPercent, DesperatePrayerPercent, RenewPercent;
        private bool UseRenewOnMoving;

        private const string TANK_CHANGE = "TANK CHANGED";
        private const string OFF_TANK_CHANGE = "OFFTANK CHANGED";
        private const string FACING = "FACING";

        //START OF SPELLS AND AURAS ==============================
        private const int DISPELL_MAGIC = 528;
        private const int SHACKLE_UNDEAD = 9484;
        private const int MIND_SEAR = 48045;
        private const int LEAP_OF_FAITH = 73325;
        private const int EVANGELISM = 81662;
        private const int ARCHANGEL = 81700;
        private const int INNER_WILL = 73413;
        private const int INNER_FIRE = 588;
        private const int INNER_FOCUS = 89485;
        private const int HYMN_OF_HOPE = 64901;
        private const int PRAYER_OF_HEALING = 596;
        private const int PRAYER_OF_MENDING = 33076;
        private const int POWER_WORD_SHIELD = 17;
        private const int POWER_WORD_FORTITUDE = 21562;
        private const int POWER_WORD_SOLACE = 129250;
        private const int POWER_WORD_BARRIER = 62618;
        private const int SHADOW_WORD_PAIN = 589;
        private const int SHADOW_WORD_DEATH = 32379;
        private const int DIVINE_INSIGHT = 123266;//109175;
        private const int FEAR_WARD = 6346;
        private const int MASS_DISPEL = 32375;
        private const int PAIN_SUPPRESSION = 33206;
        private const int PENANCE = 47540;
        private const int POWER_INFUSION = 10060;
        private const int PURIFY = 527;
        private const int FLASH_HEAL = 2061;
        private const int GREATER_HEAL = 2060;
        private const int HEAL = 2050;
        private const int RENEW = 139;
        private const int SURGE_OF_LIGHT = 114255; // 1 stack --> 128654   talent spell 109186
        private const int HOLY_FIRE = 14914;
        private const int DESPERATE_PRAYER = 19236;
        private const int RESURRECTION = 2006;
        private const int SMITE = 585;
        private const int FADE = 586;
        private const int SHADOWFIEND = 34433;
        private const int MINDBENDER = 123040;
        private const int WEAKENED_SOUL = 6788;
        private const int LEVITATE = 1706;
        private const int CASCADE = 121135;
        private const int HALO = 120517;
        private const int DIVINE_STAR = 110744;
        private const int VOID_SHIFT = 108968;
        private const int ANGELIC_FEATHER = 121536;
        private const int ANGELIC_FEATHER_AURA = 121557;
        //private const int GRACE = 47517;
        private const int GRACE = 77613;
        private const int BINDING_HEAL = 32546;
        private const int PSYCHIC_SCREAM = 8122;
        private const int VOID_TENDRILS = 108920;
        private const int PSYFIEND = 108921;

        private const int SPIRIT_SHELL = 109964;

        private const int DARK_INTENT = 109773;

        private const double time_dispell = 3000;

        private const string DRINK = "Drink";
        private const string FOOD = "Food";
        //END OF SPELLS AND AURAS ==============================

        //END OF CONSTANTS ==============================
        #endregion

        private void SetAtonementVariables()
        {
            PenancePercent = DiscPriestSettings.Instance.AtonementPenancePercent;
            BindingHealPercent = DiscPriestSettings.Instance.AtonementBindingHealPercent;
            HealPercent = DiscPriestSettings.Instance.AtonementHealPercent;
            GHPercent = DiscPriestSettings.Instance.AtonementGHPercent;
            FlashHealPercent = DiscPriestSettings.Instance.AtonementFlashHealPercent;
            SoLPercent = DiscPriestSettings.Instance.AtonementSoLPercent;
            DesperatePrayerPercent = DiscPriestSettings.Instance.AtonementDesperatePrayerPercent;
            RenewPercent = DiscPriestSettings.Instance.AtonementRenewPercent;
            UseRenewOnMoving = DiscPriestSettings.Instance.AtonementUseRenewOnMoving;
        }

        private void SetNoAtonementVariables()
        {
            PenancePercent = DiscPriestSettings.Instance.NoAtonementPenancePercent;
            BindingHealPercent = DiscPriestSettings.Instance.NoAtonementBindingHealPercent;
            HealPercent = DiscPriestSettings.Instance.NoAtonementHealPercent;
            GHPercent = DiscPriestSettings.Instance.NoAtonementGHPercent;
            FlashHealPercent = DiscPriestSettings.Instance.NoAtonementFlashHealPercent;
            SoLPercent = DiscPriestSettings.Instance.NoAtonementSoLPercent;
            DesperatePrayerPercent = DiscPriestSettings.Instance.NoAtonementDesperatePrayerPercent;
            RenewPercent = DiscPriestSettings.Instance.NoAtonementRenewPercent;
            UseRenewOnMoving = DiscPriestSettings.Instance.NoAtonementUseRenewOnMoving;
        }

        public DiscPriestCombatClass()
        {
            utils = new KingWoWUtility();
            movement = new Movement();
            extra = new ExtraUtils();
            tank = null;
            lastTank = null;
            off_tank = null;
            lastOffTank = null;
            SoloBotType = false;
            BaseBot = "unknown";
            talents = new TalentManager();
        }

        public override bool Pulse
        {
            get
            {
                if (/*DiscPriestSettings.Instance.AutoTriggerSS &&*/ SpiritShell_detected())
                    return true; 
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */!StyxWoW.IsInGame || !StyxWoW.IsInWorld || Me.Silenced || utils.IsGlobalCooldown(true) || Me.CastingSpell != null
                || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) || Me.Auras.Any(x => x.Value.SpellId == SPIRIT_SHELL) )
                    return false;
                
                tank = utils.SimpleGetTank(40f);
                if (tank == null || !tank.IsValid || !tank.IsAlive) tank = Me;

                if (tank != null && (lastTank == null || lastTank.Guid != tank.Guid))
                {
                    lastTank = tank;
                    utils.LogActivity(TANK_CHANGE, tank.Class.ToString());
                }
                off_tank = utils.SimpleGetOffTank(40f, tank);
                if (off_tank != null && (lastOffTank == null || lastOffTank.Guid != off_tank.Guid))
                {
                    lastOffTank = off_tank;
                    utils.LogActivity(OFF_TANK_CHANGE, off_tank.Class.ToString());
                }

                //full party or me
                if (!Me.Combat && !Me.Mounted && DiscPriestSettings.Instance.OOCHealing && !utils.isAuraActive(DRINK) && !utils.isAuraActive(FOOD))
                {
                    Resurrect();

                    WoWUnit healTarget = utils.GetHealTarget(40f);

                    if (healTarget != null && healTarget.HealthPercent < 100)
                    {
                        double hp = healTarget.HealthPercent;
                        if (utils.isAuraActive(SURGE_OF_LIGHT, Me)
                            && utils.CanCast(FLASH_HEAL)
                            && (hp <= SoLPercent
                                || Me.GetAuraById(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                                || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                        {
                            utils.LogHealActivity(healTarget," FLASH_HEAL", healTarget.Class.ToString());
                            return utils.Cast(FLASH_HEAL, healTarget);
                        }
                        if (!Me.IsMoving && utils.CanCast(PENANCE))
                        {
                            utils.LogHealActivity(healTarget," PENANCE", healTarget.Class.ToString());
                            return utils.Cast(PENANCE, healTarget);
                        }
                        if (!Me.IsMoving && hp <= GHPercent && utils.CanCast(GREATER_HEAL))
                        {
                            utils.LogHealActivity(healTarget," GREATER_HEAL", healTarget.Class.ToString());
                            CurrentHealtarget = healTarget;
                            return utils.Cast(GREATER_HEAL, healTarget);
                        }
                        if (!Me.IsMoving && utils.CanCast(HEAL) && hp <= HealPercent)
                        {
                            utils.LogHealActivity(healTarget," HEAL", healTarget.Class.ToString());
                            CurrentHealtarget = healTarget;
                            return utils.Cast(HEAL, healTarget);
                        }

                        if (UseRenewOnMoving && !healTarget.HasAura(RENEW) && utils.CanCast(RENEW)
                            && hp <= RenewPercent)
                        {
                            utils.LogHealActivity(healTarget," RENEW", healTarget.Class.ToString());
                            return utils.Cast(RENEW, healTarget);
                        }
                    }
                }
                if (!Me.Combat && ((tank != null && tank.Combat) || (off_tank != null && off_tank.Combat)))
                    return CombatRotation();
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
            utils.UpdateTotalGroupNumber();
            Logging.Write("Total group number = "+utils.total_group_number);
        }

        void BotEvents_OnBotStart(EventArgs args)
        {
            talents.Update();
            if (talents.HasGlyph("Holy Fire"))
            {
                utils.LogActivity("Glyph of Holy Fire Detected: range of attack spells incremented to 40");
                RangeOfAttack = 40f;
            }
            BotUpdate();
        }

        public override bool NeedRest
        {
            get
            {
                if ((utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD)) && (Me.ManaPercent < 100 || Me.HealthPercent < 100))
                    return true;
                if (Me.ManaPercent <= DiscPriestSettings.Instance.ManaPercent &&
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
                if (Me.HealthPercent <= DiscPriestSettings.Instance.HealthPercent &&
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

        public override bool NeedPullBuffs { 
            get 
            {
                return DisciPullBuff(); 
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
                    if (!target.InLineOfSpellSight || target.Distance - target.CombatReach -1  > DiscPriestSettings.Instance.PullDistance)
                    {
                        //Logging.Write("pull: target distance=" + target.Distance + " moving in range");
                        movement.KingHealMove(target.Location, DiscPriestSettings.Instance.PullDistance, true, true, target);
                    }
                    if (!Me.IsFacing(target) && !Me.IsMoving)
                    {
                        Me.SetFacing(target);
                    }

                    if (talents.IsSelected(9) && utils.GetSpellCooldown(POWER_WORD_SOLACE).Milliseconds <= StyxWoW.WoWClient.Latency)
                    {
                        utils.LogActivity("POWER_WORD_SOLACE", target.Name);
                        return utils.Cast(POWER_WORD_SOLACE, target);
                    }

                    if ( !talents.IsSelected(9) && utils.CanCast(HOLY_FIRE, target, true) /*&& !Me.IsMoving*/)
                    {
                        utils.LogActivity("HOLY_FIRE", target.Name);
                        return utils.Cast(HOLY_FIRE, target);
                    }

                    if (DiscPriestSettings.Instance.UseSWP && !utils.isAuraActive(SHADOW_WORD_PAIN, target) && utils.CanCast(SHADOW_WORD_PAIN, target, true))
                    {
                        utils.LogActivity("SHADOW_WORD_PAIN", target.Name);
                        return utils.Cast(SHADOW_WORD_PAIN, target);
                    }

                    if (utils.CanCast(SMITE,target,true) && !Me.IsMoving)
                    {
                        utils.LogActivity("SMITE", target.Name);
                        return utils.Cast(SMITE, target);
                    }
                }
                return false;          
            }
        }

        private bool DisciPullBuff()
        {
            if (utils.Mounted())
                return false;
            if (!utils.isAuraActive(POWER_WORD_FORTITUDE) && utils.CanCast(POWER_WORD_FORTITUDE))
            {
                utils.LogActivity("POWER_WORD_FORTITUDE");
                utils.Cast(POWER_WORD_FORTITUDE);
            }

            foreach (WoWPlayer player in Me.PartyMembers)
            {
                if (player.Distance - player.CombatReach -1  > 40f || player.IsDead || player.IsGhost || !player.InLineOfSight) continue;
                else if (!utils.isAuraActive(POWER_WORD_FORTITUDE, player) && utils.CanCast(POWER_WORD_FORTITUDE))
                {
                    utils.LogActivity("POWER_WORD_FORTITUDE", player.Class.ToString());
                    utils.Cast(POWER_WORD_FORTITUDE, player);
                }
            }

            if (DiscPriestSettings.Instance.UseFearWard && utils.CanCast(FEAR_WARD) && !utils.isAuraActive(FEAR_WARD, Me))
            {
                utils.LogActivity("FEAR_WARD", Me.Class.ToString());
                return utils.Cast(FEAR_WARD, Me);
            }
            if (!utils.isAuraActive(POWER_WORD_SHIELD, Me) && (!utils.isAuraActive(WEAKENED_SOUL, Me) || utils.isAuraActive(DIVINE_INSIGHT,Me)) && utils.CanCast(POWER_WORD_SHIELD))
            {
                utils.LogActivity("POWER_WORD_SHIELD", Me.Class.ToString());
                utils.Cast(POWER_WORD_SHIELD, Me); //no return see below
            }
            return false; //return false or loop trying cast PULLBUFF (not implemented)
        }

        private bool BotUpdate()
        {
            if(BaseBot.Equals(BotManager.Current.Name))
                return false;
            if (utils.IsBotBaseInUse("LazyRaider") || utils.IsBotBaseInUse("Tyrael"))
            {
                Logging.Write("Detected LazyRaider/tyrael:");
                Logging.Write("Disable all movements");
                Movement.DisableAllMovement = true;
                SoloBotType = false;
                BaseBot = BotManager.Current.Name;
                return true;
            }

            if (utils.IsBotBaseInUse("Raid Bot"))
            {
                Logging.Write("Detected RaidBot:");
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
                SoloBotType = true;
                BaseBot = BotManager.Current.Name;
                return true;
            }

            Logging.Write("Base bot detected: " + BotManager.Current.Name );
            SoloBotType = true;
            BaseBot = BotManager.Current.Name;
            return true;


        } 
        
        private bool ManaRegen()
        {
            WoWUnit target = utils.getTargetToAttack(RangeOfAttack,tank,DiscPriestSettings.Instance.AvoidCC);
            
            if (Me.Combat && target != null)
            {
                if (talents.IsSelected(9) && utils.GetSpellCooldown(POWER_WORD_SOLACE).Milliseconds <= StyxWoW.WoWClient.Latency)
                {
                    utils.LogActivity("POWER_WORD_SOLACE", target.Name);
                    return utils.Cast(POWER_WORD_SOLACE, target);
                }

                if (Me.ManaPercent <= DiscPriestSettings.Instance.ShadowFiendPercent && utils.CanCast(SHADOWFIEND))
                {
                    utils.LogActivity("SHADOWFIEND", target.Name);
                    return utils.Cast(SHADOWFIEND, target);
                }

                else if (Me.ManaPercent <= DiscPriestSettings.Instance.ShadowFiendPercent && utils.CanCast(MINDBENDER))
                {
                    utils.LogActivity("MINDBENDER", target.Name);
                    return utils.Cast(MINDBENDER, target);
                }
            }

            if (Me.Combat && Me.ManaPercent <= DiscPriestSettings.Instance.HymnOfHopePercent && utils.CanCast(HYMN_OF_HOPE) && !Me.IsMoving)
            {
                utils.LogActivity("HYMN_OF_HOPE");
                return utils.Cast(HYMN_OF_HOPE);
            }

            return false;
        }

        private bool Self()
        {
            
            if (Me.Combat && Me.HealthPercent <= DesperatePrayerPercent && utils.CanCast(DESPERATE_PRAYER))
            {
                utils.LogHealActivity(Me,"DESPERATE_PRAYER");
                return utils.Cast(DESPERATE_PRAYER);
            }

            if (DiscPriestSettings.Instance.RotationType == DiscPriestSettings.Rotation.PVP
                && !utils.isAuraActive(POWER_WORD_SHIELD, Me) && (!utils.isAuraActive(WEAKENED_SOUL, Me) || utils.isAuraActive(DIVINE_INSIGHT,Me)) && utils.CanCast(POWER_WORD_SHIELD))
            {
                utils.LogHealActivity(Me, "POWER_WORD_SHIELD", Me.Class.ToString());
                return utils.Cast(POWER_WORD_SHIELD, Me);
            }

            if (Me.Combat && DiscPriestSettings.Instance.SelfHealingPriorityEnabled && Me.HealthPercent <= DiscPriestSettings.Instance.SelfHealingPriorityHP)
            {
                //Surge of Light
                if (utils.isAuraActive(SURGE_OF_LIGHT, Me) && utils.CanCast(FLASH_HEAL))
                {
                    utils.LogHealActivity(Me, "Heal Me priority: Surge of light:" + FLASH_HEAL, Me.Class.ToString());
                    return utils.Cast(FLASH_HEAL, Me);
                }
                if (utils.CanCast(PENANCE) && (!Me.IsMoving || (Me.IsMoving && talents.HasGlyph("Penance"))))
                {
                    utils.LogHealActivity(Me, "Heal Me priority: PENANCE", Me.Class.ToString());
                    return utils.Cast(PENANCE, Me);
                }
                if (Me.HealthPercent <= FlashHealPercent && utils.CanCast(FLASH_HEAL))
                {
                    utils.LogHealActivity(Me, "Heal Me priority: FLASH_HEAL", Me.Class.ToString());
                    CurrentHealtarget = Me;
                    return utils.Cast(FLASH_HEAL);
                }
                if (!utils.isAuraActive(POWER_WORD_SHIELD, Me) && (!utils.isAuraActive(WEAKENED_SOUL, Me) || utils.isAuraActive(DIVINE_INSIGHT,Me)) && utils.CanCast(POWER_WORD_SHIELD))
                {
                    utils.LogHealActivity(Me, "Heal Me priority: POWER_WORD_SHIELD", Me.Class.ToString());
                    return utils.Cast(POWER_WORD_SHIELD, Me);
                }
                if (Me.HealthPercent <= GHPercent && utils.CanCast(GREATER_HEAL))
                {
                    utils.LogHealActivity(Me, "Heal Me priority: GREATER_HEAL", Me.Class.ToString());
                    CurrentHealtarget = Me;
                    return utils.Cast(GREATER_HEAL,Me);
                }
                if (Me.HealthPercent <= HealPercent && utils.CanCast(HEAL))
                {
                    utils.LogHealActivity(Me, "Heal Me priority: HEAL", Me.Class.ToString());
                    CurrentHealtarget = Me;
                    return utils.Cast(HEAL,Me);
                }             
            }

            if (Me.Combat && utils.CanCast(ARCHANGEL) && (utils.PlayerCountBuff("Evangelism") == 5 ) && DiscPriestSettings.Instance.UseArchangel)
            {
                utils.LogActivity("ARCHANGEL");
                return utils.Cast(ARCHANGEL);
            }

             //power infusion for heal burst or mana low
            if (utils.CanCast(POWER_INFUSION) && Me.Combat && !utils.isAuraActive(POWER_INFUSION) && 
                ((utils.GetMemberCountBelowThreshold(DiscPriestSettings.Instance.PowerInfusionPercent) >= DiscPriestSettings.Instance.PowerInfusionNumber) ||
                Me.ManaPercent <= DiscPriestSettings.Instance.PowerInfusionManaPercent) )
            {
                utils.LogActivity("POWER_INFUSION");
                return utils.Cast(POWER_INFUSION);
            }
            return false;
        }
  
        private bool AoE()
        {
            if (Me.Combat && utils.CanCast(POWER_WORD_BARRIER) && DiscPriestSettings.Instance.AutoUsePWBarrier)
            {
                WoWPlayer pwb_target = utils.BestPWBTargetLocation(DiscPriestSettings.Instance.PWBarrierPercent, DiscPriestSettings.Instance.PWBarrierNumber,true);
                if (pwb_target != null)
                {
                    utils.LogActivity("POWER_WORD_BARRIER", pwb_target.Class.ToString());
                    utils.Cast(POWER_WORD_BARRIER);
                    return SpellManager.ClickRemoteLocation(pwb_target.Location);
                }
            }

            if (!ExtraUtilsSettings.Instance.DisableAOE_HealingRotation)
            {
                //divine star
                if(!Me.IsMoving && utils.CanCast(DIVINE_STAR))
                {
                    WoWUnit ds_target = utils.BestDSTarget(DiscPriestSettings.Instance.CascadeHaloDivinestarPercent, DiscPriestSettings.Instance.CascadeHaloDivineStarNumber, DiscPriestSettings.Instance.DivineStarOffensive);
                    if (ds_target != null && utils.CanCast(DIVINE_STAR,ds_target,true))
                    {
                        ds_target.Face();
                        utils.LogActivity("DIVINE_STAR", ds_target.Class.ToString());
                        return utils.Cast(DIVINE_STAR,ds_target);
                    }
                }

                if ((utils.GetMemberCountBelowThreshold(DiscPriestSettings.Instance.CascadeHaloDivinestarPercent) >= DiscPriestSettings.Instance.CascadeHaloDivineStarNumber))
                {
                    if (utils.CanCast(HALO))
                    {
                        utils.LogActivity("HALO");
                        return utils.Cast(HALO);
                    }
                    else if (utils.CanCast(CASCADE))
                    {
                        utils.LogActivity("CASCADE");
                        return utils.Cast(CASCADE);
                    }
                }

                if (utils.CanCast(PRAYER_OF_HEALING) && !Me.IsMoving)
                {
                    WoWPlayer poh_target = utils.BestPoHTarget(DiscPriestSettings.Instance.PrayerOfHealingPercent, DiscPriestSettings.Instance.PrayerOfHealingNumber,true);
                    if (poh_target != null)
                    {
                        if (DiscPriestSettings.Instance.UseSS_on_POM && utils.CanCast(SPIRIT_SHELL))
                        {
                            utils.LogActivity("SPIRIT SHELL Before Prayer of Healing");
                            utils.Cast(SPIRIT_SHELL);
                        }
                        utils.LogActivity("PRAYER_OF_HEALING", poh_target.Class.ToString());
                        return utils.Cast(PRAYER_OF_HEALING, poh_target);
                    }
                }
            }
            return false;
        }
       
        private bool SoloHealing()
        {
            double hp = Me.HealthPercent;

            //Surge of Light
            if (utils.isAuraActive(SURGE_OF_LIGHT, Me)
                            && utils.CanCast(FLASH_HEAL)
                            && (hp <= SoLPercent
                                || Me.GetAuraById(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                                || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
            {
                utils.LogHealActivity(Me,"FLASH_HEAL", Me.Class.ToString());
                return utils.Cast(FLASH_HEAL, Me);
            }
            if (hp <= PenancePercent && utils.CanCast(PENANCE) && (!Me.IsMoving || (Me.IsMoving && talents.HasGlyph("Penance"))))
            {
                utils.LogHealActivity(Me," PENANCE", Me.Class.ToString());
                return utils.Cast(PENANCE, Me);
            }
            if (!utils.isAuraActive(POWER_WORD_SHIELD, Me) && (!utils.isAuraActive(WEAKENED_SOUL, Me) || utils.isAuraActive(DIVINE_INSIGHT,Me)) && utils.CanCast(POWER_WORD_SHIELD))
            {
                utils.LogHealActivity(Me," POWER_WORD_SHIELD", Me.Class.ToString());
                return utils.Cast(POWER_WORD_SHIELD, Me);
            }

            if (hp <= FlashHealPercent && utils.CanCast(FLASH_HEAL) && !Me.IsMoving && utils.isAuraActive(WEAKENED_SOUL, Me))
            {
                utils.LogHealActivity(Me," FLASH_HEAL", Me.Class.ToString());
                CurrentHealtarget = Me;
                return utils.Cast(FLASH_HEAL, Me);
            }          
            if (hp <= DiscPriestSettings.Instance.PainSuppressionPercent && utils.CanCast(PAIN_SUPPRESSION) && DiscPriestSettings.Instance.UsePainSuppression)
            {
                utils.LogHealActivity(Me," PAIN_SUPPRESSION", Me.Class.ToString());
                return utils.Cast(PAIN_SUPPRESSION, Me);
            }
            return false;
        }

        private bool Atonement()
        {
            if (DiscPriestSettings.Instance.AtonementHp < 100 && utils.GetMemberCountBelowThreshold(DiscPriestSettings.Instance.AtonementHp) == 0)
                return false;
            
            //Atonement heal
            WoWUnit target = utils.getTargetToAttack(RangeOfAttack,tank,DiscPriestSettings.Instance.AvoidCC);
            if (target != null)
            {
                if (!Me.IsMoving)
                {
                    Me.SetFacing(target);
                }
                if (talents.IsSelected(9) && utils.GetSpellCooldown(POWER_WORD_SOLACE).Milliseconds <= StyxWoW.WoWClient.Latency)
                {
                    utils.LogActivity("POWER_WORD_SOLACE", target.Name);
                    return utils.Cast(POWER_WORD_SOLACE, target);
                }
                if (DiscPriestSettings.Instance.UsePenanceInDPS && utils.CanCast(PENANCE, target, true) && (!Me.IsMoving || (Me.IsMoving && talents.HasGlyph("Penance"))))
                {
                    utils.LogActivity("PENANCE", target.Name);
                    return utils.Cast(PENANCE, target);
                }
                if (!talents.IsSelected(9) && utils.CanCast(HOLY_FIRE,target,true) )
                {
                    utils.LogActivity("HOLY_FIRE", target.Name);
                    return utils.Cast(HOLY_FIRE, target);
                }
                if (utils.CanCast(SMITE,target,true))
                {
                    utils.LogActivity("SMITE", target.Name);
                    return utils.Cast(SMITE, target);
                }
            }
            return false;            
        }

        private bool Healing()
        {
            WoWUnit healTarget = utils.GetHealTarget(40f);

            //valuate to heal current npc target
            if (Me.CurrentTarget != null && Me.CurrentTarget.IsFriendly && Me.CurrentTarget.IsValid 
                && !Me.CurrentTarget.IsDead && !Me.CurrentTarget.IsPlayer
                && Me.CurrentTarget.Distance - Me.CurrentTarget.CombatReach - 1 <= 40f && Me.CurrentTarget.InLineOfSpellSight
                && Me.CurrentTarget.HealthPercent < 99
                && healTarget.HealthPercent > DiscPriestSettings.Instance.HealNPC_threshold)
                healTarget = Me.CurrentTarget;


            if (healTarget != null)
            {
                double hp = healTarget.HealthPercent;

                if (healTarget.Distance - healTarget.CombatReach -1  > 40 || !healTarget.InLineOfSight)
                {
                    return false;
                }
                else
                {
                    if (healTarget.HealthPercent <= DiscPriestSettings.Instance.VoidShiftTarget && Me.HealthPercent >= DiscPriestSettings.Instance.VoidShiftMe
                    && DiscPriestSettings.Instance.UseVoidShift && !DiscPriestSettings.Instance.UseVoidShiftOnlyOnTank && utils.CanCast(VOID_SHIFT, healTarget, true))
                    {
                        utils.LogHealActivity(healTarget, " VOID_SHIFT", healTarget.Class.ToString());
                        return utils.Cast(VOID_SHIFT, healTarget);
                    }
                    //Surge of Light
                    if (utils.isAuraActive(SURGE_OF_LIGHT, Me)
                            && utils.CanCast(FLASH_HEAL)
                            && (hp <= SoLPercent
                                || Me.GetAuraById(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                                || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                    {
                        utils.LogHealActivity(healTarget, " FLASH_HEAL", healTarget.Class.ToString());
                        return utils.Cast(FLASH_HEAL, healTarget);
                    }
                    //binding Heal
                    if (hp <= BindingHealPercent && Me.HealthPercent <= BindingHealPercent
                        && utils.CanCast(BINDING_HEAL, healTarget, true) && !Me.IsMoving)
                    {
                        utils.LogHealActivity(healTarget, " BINDING_HEAL", healTarget.Class.ToString());
                        return utils.Cast(BINDING_HEAL, healTarget);
                    }
                    if (hp <= PenancePercent && utils.CanCast(PENANCE) && (!Me.IsMoving || (Me.IsMoving && talents.HasGlyph("Penance"))))
                    {
                        utils.LogHealActivity(healTarget, " PENANCE", healTarget.Class.ToString());
                        return utils.Cast(PENANCE, healTarget);
                    }
                    if (hp <= DiscPriestSettings.Instance.PWShieldPercent && !utils.isAuraActive(POWER_WORD_SHIELD, healTarget)
                        && (!utils.isAuraActive(WEAKENED_SOUL, healTarget) || utils.isAuraActive(DIVINE_INSIGHT, Me)) && utils.CanCast(POWER_WORD_SHIELD))
                    {
                        utils.LogHealActivity(healTarget, " POWER_WORD_SHIELD", healTarget.Class.ToString());
                        return utils.Cast(POWER_WORD_SHIELD, healTarget);
                    }
                    if (hp <= FlashHealPercent && utils.CanCast(FLASH_HEAL) && !Me.IsMoving && utils.isAuraActive(WEAKENED_SOUL, healTarget))
                    {
                        utils.LogHealActivity(healTarget, " FLASH_HEAL", healTarget.Class.ToString());
                        CurrentHealtarget = healTarget;
                        return utils.Cast(FLASH_HEAL, healTarget);
                    }
                    if (hp <= GHPercent && utils.CanCast(GREATER_HEAL))
                    {
                        //Better use Surge of Light procs
                        if (utils.isAuraActive(SURGE_OF_LIGHT, Me)
                            && utils.CanCast(FLASH_HEAL)
                            && (hp <= SoLPercent
                                || Me.GetAuraById(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                                || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                        {
                            utils.LogHealActivity(healTarget, FLASH_HEAL + " instead of greater heal for SoL proc", healTarget.Class.ToString());
                            return utils.Cast(FLASH_HEAL, healTarget);
                        }
                        else if (!Me.IsMoving)
                        {
                            utils.LogHealActivity(healTarget, " GREATER_HEAL", healTarget.Class.ToString());
                            CurrentHealtarget = healTarget;
                            return utils.Cast(GREATER_HEAL, healTarget);
                        }
                    }

                    if (!DiscPriestSettings.Instance.UsePainSuppressionOnlyOnTank && hp <= DiscPriestSettings.Instance.PainSuppressionPercent && utils.CanCast(PAIN_SUPPRESSION) && DiscPriestSettings.Instance.UsePainSuppression)
                    {
                        utils.LogHealActivity(healTarget, " PAIN_SUPPRESSION", healTarget.Class.ToString());
                        return utils.Cast(PAIN_SUPPRESSION, healTarget);
                    }

                    if (hp <= HealPercent && utils.CanCast(HEAL) && !Me.IsMoving)
                    {
                        utils.LogHealActivity(healTarget, " HEAL", healTarget.Class.ToString());
                        CurrentHealtarget = healTarget;
                        return utils.Cast(HEAL, healTarget);
                    }

                    if (UseRenewOnMoving && !healTarget.HasAura(RENEW) && hp <= RenewPercent && utils.CanCast(RENEW) && Me.IsMoving)
                    {
                        utils.LogHealActivity(healTarget, " RENEW", healTarget.Class.ToString());
                        return utils.Cast(RENEW, healTarget);
                    }

                }
            }
            return false;
        }     

        private bool TankHealing()
        {
            if (tank != null && tank.IsAlive && tank.Distance - tank.CombatReach -1  <= 40 && tank.InLineOfSight)
            {
                if (tank.HealthPercent <= DiscPriestSettings.Instance.VoidShiftTarget && Me.HealthPercent >= DiscPriestSettings.Instance.VoidShiftMe
                    && DiscPriestSettings.Instance.UseVoidShift && utils.CanCast(VOID_SHIFT,tank,true))
                {
                    utils.LogHealActivity(tank," VOID_SHIFT", tank.Class.ToString());
                    return utils.Cast(VOID_SHIFT, tank);
                }

                if (tank.Combat && tank.HealthPercent <= DiscPriestSettings.Instance.TankShieldPercent && !utils.isAuraActive(POWER_WORD_SHIELD, tank)
                    && (!utils.isAuraActive(WEAKENED_SOUL, tank) || utils.isAuraActive(DIVINE_INSIGHT, Me)) && utils.CanCast(POWER_WORD_SHIELD))
                {
                    utils.LogHealActivity(tank, " POWER_WORD_SHIELD", tank.Class.ToString());
                    return utils.Cast(POWER_WORD_SHIELD, tank);
                }

                if (tank.Combat && tank.HealthPercent <= DiscPriestSettings.Instance.PainSuppressionPercent && utils.CanCast(PAIN_SUPPRESSION,tank,true) && DiscPriestSettings.Instance.UsePainSuppression)
                {
                    utils.LogHealActivity(tank, " PAIN_SUPPRESSION", tank.Class.ToString());
                    return utils.Cast(PAIN_SUPPRESSION, tank);
                }

                if (tank.HealthPercent <= FlashHealPercent && utils.CanCast(FLASH_HEAL) && !Me.IsMoving && utils.isAuraActive(WEAKENED_SOUL, tank))
                {
                    utils.LogHealActivity(tank, " FLASH_HEAL", tank.Class.ToString());
                    CurrentHealtarget = tank;
                    return utils.Cast(FLASH_HEAL, tank);
                }
                if (tank.HealthPercent <= GHPercent && utils.CanCast(GREATER_HEAL))
                {
                    //Better use Surge of Light procs
                    if (utils.isAuraActive(SURGE_OF_LIGHT, Me)
                        && utils.CanCast(FLASH_HEAL)
                        && (tank.HealthPercent <= SoLPercent
                            || Me.GetAuraById(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                            || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                    {
                        utils.LogHealActivity(tank, FLASH_HEAL + " instead of greater heal for SoL proc", tank.Class.ToString());
                        return utils.Cast(FLASH_HEAL, tank);
                    }
                    else if (!Me.IsMoving)
                    {
                        utils.LogHealActivity(tank, " GREATER_HEAL", tank.Class.ToString());
                        CurrentHealtarget = tank;
                        return utils.Cast(GREATER_HEAL, tank);
                    }
                }

                if (DiscPriestSettings.Instance.Use_PoM 
                    && ((tank.Guid != Me.Guid && DiscPriestSettings.Instance.RotationType != DiscPriestSettings.Rotation.PVP) || DiscPriestSettings.Instance.RotationType == DiscPriestSettings.Rotation.PVP)
                    && utils.GroupNeedsMyAura("Prayer of Mending") && utils.CanCast(PRAYER_OF_MENDING, tank, true))
                {
                    utils.LogHealActivity(tank, " PRAYER_OF_MENDING", tank.Class.ToString());
                    return utils.Cast(PRAYER_OF_MENDING, tank);
                }
                if (tank.Combat && DiscPriestSettings.Instance.RenewOnMainTank && !utils.isAuraActive(RENEW, tank))
                {
                    utils.LogHealActivity(tank, " RENEW", tank.Class.ToString());
                    return utils.Cast(RENEW, tank);
                }
                if (DiscPriestSettings.Instance.keepGraceOnTank )
                {
                    uint tank_grace_count = utils.GetAuraStack(tank, GRACE, true);
                    if ((tank_grace_count < 2)
                        && utils.CanCast(PENANCE, tank, true))
                    {
                        utils.LogActivity("PENANCE Buffing Grace on tank Grace stacks=" + tank_grace_count + "    " + tank.Class.ToString());
                        return utils.Cast(PENANCE, tank);
                    }
                    else if ((tank_grace_count >= 2 && utils.MyAuraTimeLeft(GRACE, tank) <= DiscPriestSettings.Instance.keepGraceOnTankTime)
                        && utils.CanCast(HEAL, tank, true))
                    {
                        utils.LogActivity("HEAL Buffing Grace on tank Grace stacks=" + tank_grace_count + "    " + tank.Class.ToString());
                        return utils.Cast(HEAL, tank);
                    }
                }
            }
            return false;
        }

        private bool OffTankHealing()
        {
            if (off_tank != null && off_tank.IsAlive && off_tank.Distance - off_tank.CombatReach - 1 <= 40 && off_tank.InLineOfSight)
            {
                if (off_tank.HealthPercent <= DiscPriestSettings.Instance.VoidShiftTarget && Me.HealthPercent >= DiscPriestSettings.Instance.VoidShiftMe
                    && DiscPriestSettings.Instance.UseVoidShift && DiscPriestSettings.Instance.UseVoidShiftOnOffTank && utils.CanCast(VOID_SHIFT, off_tank, true))
                {
                    utils.LogHealActivity(off_tank, " VOID_SHIFT", off_tank.Class.ToString());
                    return utils.Cast(VOID_SHIFT, off_tank);
                }

                if (off_tank.Combat && off_tank.HealthPercent <= DiscPriestSettings.Instance.OffTankShieldPercent && !utils.isAuraActive(POWER_WORD_SHIELD, off_tank)
                    && (!utils.isAuraActive(WEAKENED_SOUL, off_tank) || utils.isAuraActive(DIVINE_INSIGHT, Me)) && utils.CanCast(POWER_WORD_SHIELD))
                {
                    utils.LogHealActivity(off_tank, " POWER_WORD_SHIELD", off_tank.Class.ToString());
                    return utils.Cast(POWER_WORD_SHIELD, off_tank);
                }

                if (off_tank.Combat && off_tank.HealthPercent <= DiscPriestSettings.Instance.PainSuppressionPercent && utils.CanCast(PAIN_SUPPRESSION, off_tank, true)
                    && DiscPriestSettings.Instance.UsePainSuppression && DiscPriestSettings.Instance.UsePainSuppressionOnOffTank)
                {
                    utils.LogHealActivity(off_tank, " PAIN_SUPPRESSION", off_tank.Class.ToString());
                    return utils.Cast(PAIN_SUPPRESSION, off_tank);
                }

                if (off_tank.HealthPercent <= FlashHealPercent && utils.CanCast(FLASH_HEAL) && !Me.IsMoving && utils.isAuraActive(WEAKENED_SOUL, off_tank))
                {
                    utils.LogHealActivity(off_tank, " FLASH_HEAL", off_tank.Class.ToString());
                    CurrentHealtarget = off_tank;
                    return utils.Cast(FLASH_HEAL, off_tank);
                }
                if (off_tank.HealthPercent <= GHPercent && utils.CanCast(GREATER_HEAL))
                {
                    //Better use Surge of Light procs
                    if (utils.isAuraActive(SURGE_OF_LIGHT, Me)
                        && utils.CanCast(FLASH_HEAL)
                        && (off_tank.HealthPercent <= SoLPercent
                            || Me.GetAuraById(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                            || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                    {
                        utils.LogHealActivity(off_tank, FLASH_HEAL + " instead of greater heal for SoL proc", off_tank.Class.ToString());
                        return utils.Cast(FLASH_HEAL, off_tank);
                    }
                    else if (!Me.IsMoving)
                    {
                        utils.LogHealActivity(off_tank, " GREATER_HEAL", off_tank.Class.ToString());
                        CurrentHealtarget = off_tank;
                        return utils.Cast(GREATER_HEAL, off_tank);
                    }
                }
                if (off_tank.Combat && DiscPriestSettings.Instance.RenewOnOffTank && !utils.isAuraActive(RENEW, off_tank))
                {
                    utils.LogHealActivity(off_tank, " RENEW", off_tank.Class.ToString());
                    return utils.Cast(RENEW, off_tank);
                }
            }
            return false;
        }

       private bool CleansingASAP()
        {
            if (DiscPriestSettings.Instance.UsePurify || DiscPriestSettings.Instance.UseMassDispell)
            {
                WoWPlayer player = utils.GetDispelASAPTarget(40f, time_dispell);

                if (player != null && /*!Blacklist.Contains(player.Guid) &&*/ player.Distance - player.CombatReach -1  <= 40f && player.InLineOfSpellSight)
                {
                    if (DiscPriestSettings.Instance.UseMassDispell && player.Distance - player.CombatReach -1  <= 30f && utils.MassDispelASAPCountForPlayer(player, 15f,time_dispell) >= DiscPriestSettings.Instance.MassDispellCount && utils.CanCast(MASS_DISPEL))
                    {
                        utils.LogActivity("MASS_DISPEL", player.Class.ToString());
                        //Blacklist.Add(player, new TimeSpan(0, 0, 2));
                        utils.Cast(MASS_DISPEL);
                        return SpellManager.ClickRemoteLocation(player.Location);
                    }

                    if (DiscPriestSettings.Instance.UsePurify && utils.CanCast(PURIFY,player,true))
                    {
                        utils.LogActivity("PURIFY", player.Class.ToString());
                        //Blacklist.Add(player, new TimeSpan(0, 0, 2));
                        return utils.Cast(PURIFY, player);
                    }
                }
            }
            return false;
        }

        private bool Cleansing()
        {
            if (DiscPriestSettings.Instance.UsePurify || DiscPriestSettings.Instance.UseMassDispell)
            {
                WoWPlayer player = utils.GetDispelTargetPriest(40f);

                if (player != null && /*!Blacklist.Contains(player.Guid) &&*/ player.Distance - player.CombatReach -1  <= 40f && player.InLineOfSight)
                {
                    if (DiscPriestSettings.Instance.UseMassDispell && utils.NeedsDispelPriest(player) && player.Distance - player.CombatReach -1  <= 30f && utils.MassDispelCountForPlayer(player, 15f) >= DiscPriestSettings.Instance.MassDispellCount && utils.CanCast(MASS_DISPEL)) 
                    {
                        utils.LogActivity("MASS_DISPEL", player.Class.ToString());
                        //Blacklist.Add(player, new TimeSpan(0, 0, 2));
                        utils.Cast(MASS_DISPEL);
                        return SpellManager.ClickRemoteLocation(player.Location);
                    }

                    if (DiscPriestSettings.Instance.UsePurify && utils.CanCast(PURIFY))
                    {
                        utils.LogActivity("PURIFY", player.Class.ToString());
                        //Blacklist.Add(player, new TimeSpan(0, 0, 2));
                        return utils.Cast(PURIFY, player);
                    }
                }
            }
            return false;
        }

        private bool Resurrect()
        {
            foreach (WoWPlayer player in utils.GetResurrectTargets(40f))
            {
                if (Blacklist.Contains(player.Guid,BlacklistFlags.All)) continue;
                else
                {
                    if (player.Distance - player.CombatReach -1  > 40f || !player.InLineOfSight) return false;
                    else if (utils.CanCast(RESURRECTION, player) && DiscPriestSettings.Instance.UseResurrection && !Me.IsMoving)
                    {
                        utils.LogActivity("RESURRECTION", player.Class.ToString());
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

            if (DiscPriestSettings.Instance.RotationType == DiscPriestSettings.Rotation.PVP
                && !utils.isAuraActive(POWER_WORD_SHIELD, Me) && (!utils.isAuraActive(WEAKENED_SOUL, Me) || utils.isAuraActive(DIVINE_INSIGHT, Me)) && utils.CanCast(POWER_WORD_SHIELD))
            {
                utils.LogHealActivity(Me, "POWER_WORD_SHIELD", Me.Class.ToString());
                return utils.Cast(POWER_WORD_SHIELD, Me);
            }

            if (DiscPriestSettings.Instance.AutoPWFortitude)
            {
                if ((!utils.isAuraActive(POWER_WORD_FORTITUDE) && !utils.isAuraActive(DARK_INTENT)) && utils.CanCast(POWER_WORD_FORTITUDE))
                {
                    utils.LogActivity("POWER_WORD_FORTITUDE");
                    return utils.Cast(POWER_WORD_FORTITUDE);
                }

                foreach (WoWPlayer player in Me.PartyMembers)
                {
                    if (player.Distance - player.CombatReach -1  > 40f || player.IsDead || player.IsGhost || !player.InLineOfSight) continue;
                    else if ((!utils.isAuraActive(POWER_WORD_FORTITUDE, player) && !utils.isAuraActive(DARK_INTENT, player)) && utils.CanCast(POWER_WORD_FORTITUDE))
                    {
                        utils.LogActivity("POWER_WORD_FORTITUDE", player.Class.ToString());
                        return utils.Cast(POWER_WORD_FORTITUDE, player);
                    }
                }
                if (CombatBuff())
                    return true;
            }
            return false;
        }

        private bool CombatBuff()
        {
            if (DiscPriestSettings.Instance.UseFearWard && utils.CanCast(FEAR_WARD) && !utils.isAuraActive(FEAR_WARD, Me))
            {
                utils.LogActivity("FEAR_WARD", Me.Class.ToString());
                return utils.Cast(FEAR_WARD, Me);
            }
            if (DiscPriestSettings.Instance.UseInnerFocus && utils.CanCast(INNER_FOCUS) && !utils.isAuraActive(INNER_FOCUS) )
            {
                utils.LogActivity("INNER_FOCUS");
                return utils.Cast(INNER_FOCUS);
            }

            if (DiscPriestSettings.Instance.UseFade && Me.Combat && (Me.GroupInfo.IsInParty || Me.GroupInfo.IsInRaid) && (Targeting.GetAggroOnMeWithin(Me.Location, 30) > 0) && utils.CanCast(FADE))
            {
                utils.LogActivity("FADE");
                return utils.Cast(FADE);
            }

            if (DiscPriestSettings.Instance.InnerToUse == DiscPriestSettings.InnerType.WILL && utils.CanCast(INNER_WILL) && !utils.isAuraActive(INNER_WILL))
            {
                utils.LogActivity("INNER_WILL");
                return utils.Cast(INNER_WILL);
            }

            if (DiscPriestSettings.Instance.InnerWillOnMoving && Me.IsMoving && !Me.Mounted && !utils.isAuraActive(INNER_WILL))
            {
                utils.LogActivity("moving....INNER_WILL");
                return utils.Cast(INNER_WILL);

            }

            if (DiscPriestSettings.Instance.BurstSpeedMoving && Me.IsMoving && !Me.IsCasting && !Me.IsChanneling && !Me.Mounted)
            {
                if (talents.IsSelected(5) && !utils.isAuraActive(ANGELIC_FEATHER_AURA) 
                    && utils.GetSpellCooldown(ANGELIC_FEATHER).Milliseconds <= StyxWoW.WoWClient.Latency)
                {
                    utils.LogActivity("moving....ANGELIC_FEATHER");
                    utils.Cast(ANGELIC_FEATHER);
                    return SpellManager.ClickRemoteLocation(Me.Location);
                }
                else if (talents.IsSelected(4) && !utils.isAuraActive(WEAKENED_SOUL, Me) && utils.CanCast(POWER_WORD_SHIELD))
                {
                    utils.LogActivity("moving....POWER_WORD_SHIELD");
                    return utils.Cast(POWER_WORD_SHIELD,Me);
                }

            }

            if (DiscPriestSettings.Instance.InnerToUse == DiscPriestSettings.InnerType.FIRE && !Me.IsMoving && utils.CanCast(INNER_FIRE) && !utils.isAuraActive(INNER_FIRE))
            {
                utils.LogActivity("INNER_FIRE");
                return utils.Cast(INNER_FIRE);
            }

            return false;
        }
 
        private bool Dps()
        {
            WoWUnit target = null;
            if(DiscPriestSettings.Instance.RotationType == DiscPriestSettings.Rotation.PVP)
                target = utils.getTargetPVPToAttack(RangeOfAttack, tank, DiscPriestSettings.Instance.AvoidCC);
            else
                target = utils.getTargetToAttack(RangeOfAttack, tank, DiscPriestSettings.Instance.AvoidCC);
            if (target != null)
            {
                if (SoloBotType && (target.Distance - target.CombatReach -1  > DiscPriestSettings.Instance.PullDistance || !target.InLineOfSpellSight || !target.InLineOfSight))
                    movement.KingHealMove(target.Location, DiscPriestSettings.Instance.PullDistance, true, true, target);

                if (!Me.IsMoving)
                {
                    Me.SetFacing(target);
                }

                if (talents.IsSelected(9) && utils.GetSpellCooldown(POWER_WORD_SOLACE).Milliseconds <= StyxWoW.WoWClient.Latency)
                {
                    utils.LogActivity("POWER_WORD_SOLACE", target.Name);
                    return utils.Cast(POWER_WORD_SOLACE, target);
                }

                if (DiscPriestSettings.Instance.UseSWD && target.HealthPercent < 20 && utils.CanCast(SHADOW_WORD_DEATH,target,true))
                {
                    utils.LogActivity("SHADOW_WORD_DEATH", target.Name);
                    return utils.Cast(SHADOW_WORD_DEATH, target);
                }

                if(DiscPriestSettings.Instance.UseSWP && !utils.isAuraActive(SHADOW_WORD_PAIN,target) && utils.CanCast(SHADOW_WORD_PAIN,target,true))
                {
                    utils.LogActivity("SHADOW_WORD_PAIN", target.Name);
                    return utils.Cast(SHADOW_WORD_PAIN, target);
                }
                if (DiscPriestSettings.Instance.UsePenanceInDPS && utils.CanCast(PENANCE, target) && (!Me.IsMoving || (Me.IsMoving && talents.HasGlyph("Penance"))))
                {
                    utils.LogActivity("PENANCE", target.Name);
                    return utils.Cast(PENANCE, target);
                }                
                if (!talents.IsSelected(9) && utils.CanCast(HOLY_FIRE,target,true) /*&& !Me.IsMoving*/)
                {                    
                    utils.LogActivity("HOLY_FIRE", target.Name);
                    return utils.Cast(HOLY_FIRE, target);
                }
                if (utils.CanCast(SMITE,target))
                {
                    utils.LogActivity("SMITE", target.Name);
                    return utils.Cast(SMITE, target);
                }
            }
            else if (SoloBotType && Me.CurrentTarget != null && !Me.CurrentTarget.IsDead && (!Me.CurrentTarget.InLineOfSpellSight || !Me.CurrentTarget.InLineOfSight || Me.CurrentTarget.Distance - Me.CurrentTarget.CombatReach - 1 > DiscPriestSettings.Instance.PullDistance))
            {
                movement.KingHealMove(Me.CurrentTarget.Location, DiscPriestSettings.Instance.PullDistance, true, true, target);
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
                    if (healTarget.Distance -healTarget.CombatReach -1  > 40f || !healTarget.InLineOfSight)
                    {
                        Logging.Write("GetHealTarget give me a wrong target!!");
                        return false;
                    }
                    else
                    {
                        //Surge of Light
                        if (utils.CanCast(FLASH_HEAL) && (hp <= SoLPercent
                                                          || Me.GetAuraById(SURGE_OF_LIGHT).TimeLeft.TotalMilliseconds <= 3500
                                                          || utils.PlayerCountBuff(SURGE_OF_LIGHT) == 2))
                        {
                            utils.LogActivity("FLASH_HEAL", healTarget.Class.ToString());
                            return utils.Cast(FLASH_HEAL, healTarget);
                        }
                    }
                }
            }
            return false;
        }
       
        private bool SpiritShell_detected()
        {
            if (Me.ActiveAuras.Any(x => x.Value.SpellId == SPIRIT_SHELL))
            {
                foreach (var woWPartyMember in utils.GetGroupMembers())
                {
                    if (currentGroupForSS >= utils.total_group_number)
                        currentGroupForSS = 0;
                    if (woWPartyMember.ToPlayer() != null)
                    {
                        if (woWPartyMember.GroupNumber == currentGroupForSS && !Me.IsMoving && !utils.MeIsChanneling && !Me.IsCasting
                            && utils.CanCast(PRAYER_OF_HEALING, woWPartyMember.ToPlayer(),true)
                            && woWPartyMember.ToPlayer().Distance - woWPartyMember.ToPlayer().CombatReach -1  <= 30
                            && woWPartyMember.ToPlayer().InLineOfSpellSight
                            && woWPartyMember.ToPlayer().IsAlive)
                        {
                            utils.LogActivity("SPIRIT_SHELL BUFFED casting Prayer of Healing on player of group:" + (woWPartyMember.GroupNumber + 1).ToString() + "   " + woWPartyMember.ToPlayer().Class.ToString());
                            utils.Cast(PRAYER_OF_HEALING, woWPartyMember.ToPlayer());
                            currentGroupForSS = currentGroupForSS + 1;
                            return true;
                        }
                    }

                }
            }
            else
                currentGroupForSS = 0;
            return false;
        }

        private bool CC()
        {
            if (utils.CanCast(PSYCHIC_SCREAM) && DiscPriestSettings.Instance.UsePsychicScream && Me.Combat && (utils.AllAttaccableEnemyMobsInRange(8).Count() >= 1))
            {
                utils.LogActivity("PSYCHIC_SCREAM");
                return utils.Cast(PSYCHIC_SCREAM);
            }
            if ( utils.CanCast(VOID_TENDRILS) && DiscPriestSettings.Instance.UseVoidTendrils && Me.Combat && (utils.AllAttaccableEnemyMobsInRange(8).Count() >= 1))
            {
                utils.LogActivity("VOID_TENDRILS");
                return utils.Cast(VOID_TENDRILS);
            }
            if (utils.CanCast(PSYFIEND) && DiscPriestSettings.Instance.UsePsyfiend && Me.Combat && (utils.AllAttaccableEnemyMobsInRange(20).Count() >= 1))
            {
                utils.LogActivity("PSYFIEND");
                utils.Cast(PSYFIEND);
                return SpellManager.ClickRemoteLocation(Me.Location);
            }

            return false;
        }

        private bool DispellMagic()
        {
            if (DiscPriestSettings.Instance.UseDispellMagic && Me.CurrentTarget != null && Me.CurrentTarget.Attackable && Me.CurrentTarget.IsAlive
                && utils.CanCast(DISPELL_MAGIC,Me.CurrentTarget,true))
            {
                if (utils.EnemyNeedDispellASAP(Me.CurrentTarget))
                {
                    utils.LogActivity("DISPELL_MAGIC", Me.CurrentTarget.Name);
                    return utils.Cast(DISPELL_MAGIC, Me.CurrentTarget);
                }
            }
            return false;
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

            CC();

            if (DiscPriestSettings.Instance.RotationType == DiscPriestSettings.Rotation.PVE)
            {
                Self();
                ManaRegen();
                if (DiscPriestSettings.Instance.PriorizeTankHealing)
                    TankHealing();
                Cleansing();
                AoE();
                if (!DiscPriestSettings.Instance.PriorizeTankHealing)
                    TankHealing();
                if (DiscPriestSettings.Instance.OffTankHealing)
                    OffTankHealing();
                Healing();
                if (UsingAtonementHealing /*&& DiscPriestSettings.Instance.UseAtonement*/ && Me.ManaPercent >= DiscPriestSettings.Instance.AtonementManaThreshold)
                {
                    Atonement();
                }
                return false;
            }
            if (DiscPriestSettings.Instance.RotationType == DiscPriestSettings.Rotation.PVP)
            {
                Self();
                ManaRegen();
                if (DiscPriestSettings.Instance.PriorizeTankHealing)
                    TankHealing();
                Cleansing();
                DispellMagic();
                AoE();
                if (!DiscPriestSettings.Instance.PriorizeTankHealing)
                    TankHealing();
                if (DiscPriestSettings.Instance.OffTankHealing)
                    OffTankHealing();
                Healing();
                if (UsingAtonementHealing /*&& DiscPriestSettings.Instance.UseAtonement*/ && Me.ManaPercent >= DiscPriestSettings.Instance.AtonementManaThreshold)
                {
                    Dps();
                }
                return false;
            }

            else if (SoloBotType || DiscPriestSettings.Instance.RotationType == DiscPriestSettings.Rotation.QUESTING)
            {
                Self();
                ManaRegen();
                Cleansing();
                SoloHealing();
                Dps();
                return false;
            }
            else
                utils.LogActivity("NO VALID ROTATION ");
            return false;

        }

        public void StopCastingCheck()
        {
            //Stop Casting Healing Spells
            if (Me.CastingSpell != null &&
                (Me.CurrentCastId == HEAL || Me.CurrentCastId == FLASH_HEAL || Me.CurrentCastId == GREATER_HEAL))
            {
                if (CurrentHealtarget != null && !CurrentHealtarget.IsAlive)
                {
                    CurrentHealtarget = null;
                    SpellManager.StopCasting();
                    Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Unit is Dead");
                }

                else if (CurrentHealtarget != null && CurrentHealtarget.IsAlive && !CurrentHealtarget.InLineOfSpellSight)
                {
                    CurrentHealtarget = null;
                    SpellManager.StopCasting();
                    Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Unit not In Line of Spell Sight");
                }

                else if (CurrentHealtarget != null && CurrentHealtarget.IsAlive && CurrentHealtarget.Distance - CurrentHealtarget.CombatReach -1  > 40f)
                {
                    CurrentHealtarget = null;
                    SpellManager.StopCasting();
                    Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Unit out of range");
                }

                else if (CurrentHealtarget != null && CurrentHealtarget.IsAlive &&
                        CurrentHealtarget.HealthPercent > 99)
                {
                    CurrentHealtarget = null;
                    SpellManager.StopCasting();
                    Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Unit Full HP");
                }

                else if (CurrentHealtarget != null && CurrentHealtarget.IsAlive &&
                        Me.CastingSpell.Name.Equals(GREATER_HEAL) &&
                        CurrentHealtarget.HealthPercent > 85)
                {
                    CurrentHealtarget = null;
                    SpellManager.StopCasting();
                    Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Save Mana Greater heal");
                }

                else if (CurrentHealtarget != null && CurrentHealtarget.IsAlive &&
                    Me.CastingSpell.Name.Equals(FLASH_HEAL) &&
                    CurrentHealtarget.HealthPercent > 70)
                {
                    CurrentHealtarget = null;
                    SpellManager.StopCasting();
                    Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Save Mana Flash Heal");
                }

                else if (Me.IsCasting && CurrentHealtarget != null && CurrentHealtarget.IsAlive &&
                    Me.CastingSpell.Name.Equals(HEAL)
                    && CurrentHealtarget.HealthPercent <= GHPercent)
                {
                    SpellManager.StopCasting();
                    Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting Heal. Need Greater Heal!");
                    utils.Cast(GREATER_HEAL, CurrentHealtarget);
                }              
            }
        }

        public bool SoS()
        {
            WoWUnit healTarget = utils.GetHealTarget(40f);
            if(healTarget!=null && healTarget.HealthPercent <= DiscPriestSettings.Instance.SoSPercent )
            {
                /*if (CurrentHealtarget.ToPlayer() != null && (CurrentHealtarget.ToPlayer().IsInMyPartyOrRaid || CurrentHealtarget.ToPlayer().IsMe) && CurrentHealtarget.ToPlayer().Guid != healTarget.Guid && !CurrentHealtarget.ToPlayer().IsDead
                    && CurrentHealtarget.ToPlayer().HealthPercent > DiscPriestSettings.Instance.SoSPercent && Me.CastingSpell != null)
                {
                    SpellManager.StopCasting();
                    utils.LogActivity("SOS:   Stop cast ");
                }*/


                if (utils.isAuraActive(SURGE_OF_LIGHT, Me) && utils.CanCast(FLASH_HEAL, healTarget, true))
                {
                    utils.LogHealActivity(healTarget, "SOS:   SURGE_OF_LIGHT::FLASH_HEAL", healTarget.Class.ToString());
                    CurrentHealtarget = healTarget;
                    return utils.Cast(FLASH_HEAL, healTarget);
                }
                if (healTarget.HealthPercent <= DiscPriestSettings.Instance.VoidShiftTarget && Me.HealthPercent >= DiscPriestSettings.Instance.VoidShiftMe
                    && DiscPriestSettings.Instance.UseVoidShift && utils.CanCast(VOID_SHIFT,healTarget,true) &&
                    ( (tank!=null && healTarget.Guid!= tank.Guid && !DiscPriestSettings.Instance.UseVoidShiftOnlyOnTank)
                    || (tank!=null && healTarget.Guid== tank.Guid && DiscPriestSettings.Instance.UseVoidShiftOnlyOnTank)
                    ) )
                {
                    CurrentHealtarget = healTarget;
                    utils.LogHealActivity(healTarget, "SOS:   VOID_SHIFT", healTarget.Class.ToString());
                    return utils.Cast(VOID_SHIFT, healTarget);
                }
                if (utils.CanCast(PENANCE, healTarget, true) && (!Me.IsMoving || (Me.IsMoving && talents.HasGlyph("Penance"))))
                {
                    CurrentHealtarget = healTarget;
                    utils.LogHealActivity(healTarget, "SOS:   PENANCE", healTarget.Class.ToString());
                    return utils.Cast(PENANCE, healTarget);
                }
                if (utils.CanCast(FLASH_HEAL) && !Me.IsMoving)
                {
                    utils.LogHealActivity(healTarget, "SOS:   FLASH_HEAL", healTarget.Class.ToString());
                    CurrentHealtarget = healTarget;
                    return utils.Cast(FLASH_HEAL, healTarget);
                }
           
            }
            return false;
        }

        public override bool Combat
        {
            get
            {
                if (DiscPriestSettings.Instance.AutoSwitchAtonementNormal)
                {
                    WoWUnit target = utils.getTargetToAttack(RangeOfAttack,tank,DiscPriestSettings.Instance.AvoidCC);
                    if (target != null)
                    {
                        if (!UsingAtonementHealing)
                        {
                            if(DiscPriestSettings.Instance.ShowMessageSwitching)
                                Lua.DoString(@"print('AUTOSWITCH: Atonement Healing \124cFF15E61C ENABLED!')");
                            UsingAtonementHealing = true;
                            SetAtonementVariables();
                        }
                    }
                    else
                    {
                        if (UsingAtonementHealing)
                        {
                            if (DiscPriestSettings.Instance.ShowMessageSwitching)
                                Lua.DoString(@"print('AUTOSWITCH: Atonement healing \124cFFE61515 DISABLED!')");
                            UsingAtonementHealing = false;
                            SetNoAtonementVariables();
                        }
                    }
                 
                }
                else 
                {
                    if (ExtraUtilsSettings.Instance.UseDisciplineAtonementHealingRotation)
                    {
                        if (!UsingAtonementHealing)
                        {
                            UsingAtonementHealing = true;
                            SetAtonementVariables();
                        }
                    }
                    else
                    {
                        if (UsingAtonementHealing)
                        {
                            UsingAtonementHealing = false;
                            SetNoAtonementVariables();
                        }
                    }
                }
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !Me.Mounted && !StyxWoW.IsInGame || !StyxWoW.IsInWorld || Me.Silenced || utils.IsGlobalCooldown(true) || utils.MeIsChanneling || Me.IsCasting
                || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) )
                    return false;
                if (DiscPriestSettings.Instance.SoSEnabled && SoS())
                    return true;
                if (DiscPriestSettings.Instance.PriorizeTankHealing && TankHealing())
                    return true;
                if (/*DiscPriestSettings.Instance.AutoTriggerSS &&*/ SpiritShell_detected())
                    return true;
                if (DiscPriestSettings.Instance.TrySaveMana)
                    StopCastingCheck();
                //UPDATE TANK
                //tank = utils.GetTank();
                tank = utils.SimpleGetTank(40f);
                if (tank == null || !tank.IsValid || !tank.IsAlive) tank = Me;

                if (tank != null && (lastTank == null || lastTank.Guid != tank.Guid))
                {
                    lastTank = tank;
                    utils.LogActivity(TANK_CHANGE, tank.Class.ToString());
                }
                off_tank = utils.SimpleGetOffTank(40f, tank);
                if (off_tank != null && (lastOffTank == null || lastOffTank.Guid != off_tank.Guid))
                {
                    lastOffTank = off_tank;
                    utils.LogActivity(OFF_TANK_CHANGE, off_tank.Class.ToString());
                }
                CombatBuff();
                if (!Me.ActiveAuras.Any(x => x.Value.SpellId == SPIRIT_SHELL))
                    return CombatRotation();
                return false;
            }
        }
    }
}
