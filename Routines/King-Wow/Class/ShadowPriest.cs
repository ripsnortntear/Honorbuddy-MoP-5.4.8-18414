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
    public class ShadowPriestCombatClass : KingWoWAbstractBaseClass
    {
        
        private static string Name = "KingWoW ShadowPriest";

        #region CONSTANT AND VARIABLES
        //START OF CONSTANTS AND VARIABLES ==============================
        
        private KingWoWUtility utils = null;
        private Movement movement = null;
        private ExtraUtils extra = null;

        private WoWUnit tank = null;
        private WoWUnit off_tank = null;
        private WoWUnit lastTank = null;
        private WoWUnit lastOffTank = null;
        int enemyAroundMainTank = 0;
        int enemyAroundOffTank = 0;

        private float RangeOfAttack = 40f;

            
        private bool SoloBotType = false;
        private string BaseBot = "unknown";
        private TalentManager talents = null;

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
        //private const int PENANCE = 47540;
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

        private const int SILENCE = 15487;
        private const int DEVOURING_PLAGUE = 2944;
        private const int MIND_BLAST = 8092;
        private const int VAMPIRIC_TOUCH = 34914;
        private const int DISPERSION = 47585;
        private const int VAMPIRIC_EMBRANCE = 15286;
        private const int MIND_SPIKE = 73510;
        private const int SURGE_OF_DARKNESS = 87160;
        private const int MIND_FLY = 15407;
        private const int MIND_FLY_INSANITY = 129197;

        private const string SHADOWFORM = "Shadowform";

        private DateTime nextTimeVampiricTouchAllowed;
        private DateTime nextChanneledMFSpellAllowed;
        private DateTime nextChanneledMSSpellAllowed;
        //END OF SPELLS AND AURAS ==============================

        //END OF CONSTANTS ==============================
        #endregion

        
        public ShadowPriestCombatClass()
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
            nextTimeVampiricTouchAllowed = DateTime.Now;
            nextChanneledMFSpellAllowed = DateTime.Now;
            nextChanneledMSSpellAllowed = DateTime.Now;
        }

        public override bool Pulse
        {
            get
            {
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld || Me.Silenced /*|| utils.IsGlobalCooldown(true)*/ || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) || Me.IsChanneling || utils.MeIsCastingWithLag())
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
                off_tank = utils.SimpleGetOffTank(40f, tank);
                if (off_tank != null && (lastOffTank == null || lastOffTank.Guid != off_tank.Guid))
                {
                    lastOffTank = off_tank;
                    utils.LogActivity(OFF_TANK_CHANGE, off_tank.Class.ToString());
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
            BotUpdate();
        }

        public override bool NeedRest
        {
            get
            {
                if ((utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD)) && (Me.ManaPercent < 100 || Me.HealthPercent < 100))
                    return true;
                if (Me.ManaPercent <= ShadowPriestSettings.Instance.ManaPercent &&
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
                if (Me.HealthPercent <= ShadowPriestSettings.Instance.HealthPercent &&
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
                return ShadowPullBuff(); 
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
                    if (!target.InLineOfSpellSight || target.Distance - target.CombatReach -1  > ShadowPriestSettings.Instance.PullDistance)
                    {
                        movement.KingHealMove(target.Location, ShadowPriestSettings.Instance.PullDistance, true, true, target);
                    }                   
                    if (!Me .IsMoving && !Me.IsFacing(target))
                    {
                        utils.LogActivity(FACING, target.Name);
                        Me.SetFacing(target);
                    }
                    if (Me.GetCurrentPower(WoWPowerType.ShadowOrbs) < 3 && utils.CanCast(VAMPIRIC_TOUCH, target))
                    {
                        utils.LogActivity("VAMPIRIC_TOUCH", target.Name);
                        return utils.Cast(VAMPIRIC_TOUCH, target);
                    }
                    else if (Me.GetCurrentPower(WoWPowerType.ShadowOrbs) == 3 && utils.CanCast(DEVOURING_PLAGUE, target))
                    {
                        utils.LogActivity("DEVOURING_PLAGUE", target.Name);
                        return utils.Cast(DEVOURING_PLAGUE, target);
                    }
                }
                return false;          
            }
        }

        private bool ShadowPullBuff()
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

            if (ShadowPriestSettings.Instance.UseFearWard && utils.CanCast(FEAR_WARD) && !utils.isAuraActive(FEAR_WARD, Me))
            {
                utils.LogActivity("FEAR_WARD", Me.Class.ToString());
                return utils.Cast(FEAR_WARD, Me);
            }
            if (!utils.isAuraActive(POWER_WORD_SHIELD, Me) && (!utils.isAuraActive(WEAKENED_SOUL, Me) || utils.isAuraActive(DIVINE_INSIGHT,Me)) && utils.CanCast(POWER_WORD_SHIELD))
            {
                utils.LogActivity("PULL BUFF: POWER_WORD_SHIELD", Me.Class.ToString());
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
            WoWUnit target = Me.CurrentTarget;
            if (Me.Combat && (target == null || !target.Attackable ||target.IsFriendly || !target.InLineOfSight || target.Distance - target.CombatReach -1  > RangeOfAttack || target.IsDead))
            {
                if (tank != null)
                    target = tank.CurrentTarget;
            }
            if (Me.Combat && target != null && target.Attackable && target.InLineOfSight && target.Distance - target.CombatReach -1  <= RangeOfAttack && !target.IsDead)
            {
                if (Me.ManaPercent <= ShadowPriestSettings.Instance.ShadowFiendPercent && utils.CanCast(SHADOWFIEND))
                {
                    utils.LogActivity("SHADOWFIEND", target.Name);
                    return utils.Cast(SHADOWFIEND, target);
                }

                else if (Me.ManaPercent <= ShadowPriestSettings.Instance.ShadowFiendPercent && utils.CanCast(MINDBENDER))
                {
                    utils.LogActivity("MINDBENDER", target.Name);
                    return utils.Cast(MINDBENDER, target);
                }
            }

         
            return false;
        }

        private bool Self()
        {
            if ((Me.HealthPercent <= ShadowPriestSettings.Instance.DispersionHP || Me.ManaPercent <= ShadowPriestSettings.Instance.DispersionMana)
                && utils.CanCast(DISPERSION))
            {
                utils.LogActivity("DISPERSION");
                return utils.Cast(DISPERSION);
            }
            if (Me.Combat && Me.HealthPercent <= ShadowPriestSettings.Instance.DesperatePrayerPercent && utils.CanCast(DESPERATE_PRAYER))
            {
                utils.LogHealActivity(Me,"DESPERATE_PRAYER");
                return utils.Cast(DESPERATE_PRAYER);
            }

            if (Me.HealthPercent <= ShadowPriestSettings.Instance.VampiricEmbranceHP && utils.CanCast(VAMPIRIC_EMBRANCE))
            {
                utils.LogHealActivity(Me, "VAMPIRIC_EMBRANCE");
                return utils.Cast(VAMPIRIC_EMBRANCE);
            }

            if (Me.Combat && talents.HasGlyph("Dark Binding") && !Me.HasAura(RENEW) && Me.HealthPercent <= ShadowPriestSettings.Instance.RenewPercent)
            {
                utils.LogHealActivity(Me, "self RENEW");
                return utils.Cast(RENEW);
            }            

            if (Me.Combat && ShadowPriestSettings.Instance.SelfHealingPriorityEnabled && Me.HealthPercent <= ShadowPriestSettings.Instance.SelfHealingPriorityHP)
            {
                if (!utils.isAuraActive(POWER_WORD_SHIELD, Me) && (!utils.isAuraActive(WEAKENED_SOUL, Me) || utils.isAuraActive(DIVINE_INSIGHT,Me)) && utils.CanCast(POWER_WORD_SHIELD))
                {
                    utils.LogHealActivity(Me, "Heal Me priority: POWER_WORD_SHIELD", Me.Class.ToString());
                    return utils.Cast(POWER_WORD_SHIELD, Me);
                }
                if (Me.HealthPercent <= ShadowPriestSettings.Instance.FlashHealPercent && utils.CanCast(FLASH_HEAL))
                {
                    utils.LogHealActivity(Me, "Heal Me priority: FLASH_HEAL", Me.Class.ToString());
                    return utils.Cast(FLASH_HEAL);
                }
                if (Me.HealthPercent <= ShadowPriestSettings.Instance.GHPercent && utils.CanCast(GREATER_HEAL))
                {
                    utils.LogHealActivity(Me, "Heal Me priority: GREATER_HEAL", Me.Class.ToString());
                    return utils.Cast(GREATER_HEAL,Me);
                }             
            }
            return false;
        }

        private bool Interrupt()
        {
            if (ShadowPriestSettings.Instance.AutoInterrupt)
            {
                WoWUnit target = null;
                WoWUnit InterruptTargetCandidate = Me.FocusedUnit;
                if (InterruptTargetCandidate == null || InterruptTargetCandidate.IsFriendly || InterruptTargetCandidate.IsDead
                    || !InterruptTargetCandidate.Attackable)
                {
                    if (ShadowPriestSettings.Instance.TargetTypeSelected == ShadowPriestSettings.TargetType.MANUAL)
                        target = Me.CurrentTarget;
                    else if (ShadowPriestSettings.Instance.TargetTypeSelected == ShadowPriestSettings.TargetType.AUTO)
                    {
                        target = utils.getTargetToAttack(30, tank);
                    }
                    else if (ShadowPriestSettings.Instance.TargetTypeSelected == ShadowPriestSettings.TargetType.SEMIAUTO)
                    {
                        target = Me.CurrentTarget;
                        if (target == null || target.IsDead || !target.InLineOfSpellSight || target.Distance - target.CombatReach - 1 > 30)
                            target = utils.getTargetToAttack(30, tank);
                    }
                    InterruptTargetCandidate = target;
                }
                if (InterruptTargetCandidate != null && (InterruptTargetCandidate.IsCasting || InterruptTargetCandidate.IsChanneling)
                    && InterruptTargetCandidate.CanInterruptCurrentSpellCast /*&& utils.CanCast(SILENCE, InterruptTargetCandidate)*/
                    && utils.GetSpellCooldown(SILENCE).Milliseconds == 0)
                {
                    utils.LogActivity("SILENCE", InterruptTargetCandidate.Name);
                    return utils.Cast(SILENCE, InterruptTargetCandidate);
                }
            }
            return false;
        }

        private bool CleansingASAP()
        {
            if (ShadowPriestSettings.Instance.UsePurify || ShadowPriestSettings.Instance.UseMassDispell)
            {
                WoWPlayer player = utils.GetDispelASAPTarget(40f, time_dispell);

                if (player != null && /*!Blacklist.Contains(player.Guid) &&*/ player.Distance - player.CombatReach -1  <= 40f && player.InLineOfSpellSight)
                {
                    if (ShadowPriestSettings.Instance.UseMassDispell && player.Distance - player.CombatReach -1  <= 30f && utils.MassDispelASAPCountForPlayer(player, 15f,time_dispell) >= ShadowPriestSettings.Instance.MassDispellCount && utils.CanCast(MASS_DISPEL))
                    {
                        utils.LogActivity("MASS_DISPEL", player.Class.ToString());
                        //Blacklist.Add(player, new TimeSpan(0, 0, 2));
                        utils.Cast(MASS_DISPEL);
                        return SpellManager.ClickRemoteLocation(player.Location);
                    }

                    if (ShadowPriestSettings.Instance.UsePurify && utils.CanCast(PURIFY,player,true))
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
            if (ShadowPriestSettings.Instance.UsePurify || ShadowPriestSettings.Instance.UseMassDispell)
            {
                WoWPlayer player = utils.GetDispelTargetPriest(40f);

                if (player != null && /*!Blacklist.Contains(player.Guid) &&*/ player.Distance - player.CombatReach -1  <= 40f && player.InLineOfSight)
                {
                    if (ShadowPriestSettings.Instance.UseMassDispell && utils.NeedsDispelPriest(player) && player.Distance - player.CombatReach -1  <= 30f && utils.MassDispelCountForPlayer(player, 15f) >= ShadowPriestSettings.Instance.MassDispellCount && utils.CanCast(MASS_DISPEL)) 
                    {
                        utils.LogActivity("MASS_DISPEL", player.Class.ToString());
                        //Blacklist.Add(player, new TimeSpan(0, 0, 2));
                        utils.Cast(MASS_DISPEL);
                        return SpellManager.ClickRemoteLocation(player.Location);
                    }

                    if (ShadowPriestSettings.Instance.UsePurify && utils.CanCast(PURIFY))
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
                    else if (utils.CanCast(RESURRECTION, player) && ShadowPriestSettings.Instance.UseResurrection && !Me.IsMoving)
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

            else if (Me.Combat && Me.HealthPercent <= ShadowPriestSettings.Instance.PWShieldPercent && !utils.isAuraActive(POWER_WORD_SHIELD, Me) && !utils.isAuraActive(WEAKENED_SOUL, Me) && utils.CanCast(POWER_WORD_SHIELD))
            {
                utils.LogHealActivity(Me, " Buff Me: POWER_WORD_SHIELD", Me.Class.ToString());
                return utils.Cast(POWER_WORD_SHIELD, Me);
            }

            else if (ShadowPriestSettings.Instance.AutoPWFortitude)
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
            if (!Me.HasAura(SHADOWFORM) && utils.CanCast(SHADOWFORM))
            {
                utils.LogActivity("SHADOWFORM", Me.Class.ToString());
                return utils.Cast(SHADOWFORM, Me);
            }
            else if (ShadowPriestSettings.Instance.UseFearWard && utils.CanCast(FEAR_WARD) && !utils.isAuraActive(FEAR_WARD, Me))
            {
                utils.LogActivity("FEAR_WARD", Me.Class.ToString());
                return utils.Cast(FEAR_WARD, Me);
            }
            else if (ShadowPriestSettings.Instance.UseInnerFocus && utils.CanCast(INNER_FOCUS) && !utils.isAuraActive(INNER_FOCUS))
            {
                utils.LogActivity("INNER_FOCUS");
                return utils.Cast(INNER_FOCUS);
            }

            else if (ShadowPriestSettings.Instance.UseFade && Me.Combat && (Me.GroupInfo.IsInParty || Me.GroupInfo.IsInRaid) && (Targeting.GetAggroOnMeWithin(Me.Location, 30) > 0) && utils.CanCast(FADE))
            {
                utils.LogActivity("FADE");
                return utils.Cast(FADE);
            }

            else if (ShadowPriestSettings.Instance.InnerToUse == ShadowPriestSettings.InnerType.WILL && utils.CanCast(INNER_WILL) && !utils.isAuraActive(INNER_WILL))
            {
                utils.LogActivity("INNER_WILL");
                return utils.Cast(INNER_WILL);
            }

            else if (ShadowPriestSettings.Instance.InnerWillOnMoving && Me.IsMoving && !Me.Mounted && !utils.isAuraActive(INNER_WILL))
            {
                utils.LogActivity("moving....INNER_WILL");
                return utils.Cast(INNER_WILL);

            }

            else if (ShadowPriestSettings.Instance.BurstSpeedMoving && Me.IsMoving && !Me.IsCasting && !Me.IsChanneling && !Me.Mounted)
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

            else if (ShadowPriestSettings.Instance.InnerToUse == ShadowPriestSettings.InnerType.FIRE && !Me.IsMoving && utils.CanCast(INNER_FIRE) && !utils.isAuraActive(INNER_FIRE))
            {
                utils.LogActivity("INNER_FIRE");
                return utils.Cast(INNER_FIRE);
            }

            return false;
        }
 
        private bool ProcWork(WoWUnit target)
        {
            if (Me.GetCurrentPower(WoWPowerType.ShadowOrbs) == 3 || utils.PlayerCountBuff(SURGE_OF_DARKNESS) > 0 || utils.isAuraActive(DIVINE_INSIGHT, Me))
            {
                if (target != null && !target.IsDead && target.InLineOfSpellSight && target.Distance - target.CombatReach - 1 <= 40)
                {
                    if (ShadowPriestSettings.Instance.TargetTypeSelected == ShadowPriestSettings.TargetType.AUTO)
                        target.Target();
                    if ((ShadowPriestSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                    {
                        Me.SetFacing(target);
                    }

                    //devouring plague
                    if (Me.GetCurrentPower(WoWPowerType.ShadowOrbs) == 3 && !(Me.IsChanneling && Me.ChanneledCastingSpellId==MIND_FLY_INSANITY))
                    {
                        utils.LogActivity("DEVOURING_PLAGUE", target.Name);
                        return utils.Cast(DEVOURING_PLAGUE, target);
                    }
                    else if (utils.PlayerCountBuff(SURGE_OF_DARKNESS) > 0 && !(Me.IsChanneling && Me.ChanneledCastingSpellId == MIND_FLY_INSANITY))
                    {
                        utils.LogActivity("MIND_SPIKE", target.Name);
                        return utils.Cast(MIND_SPIKE, target);
                    }
                    else if (utils.isAuraActive(DIVINE_INSIGHT, Me) && Me.GetCurrentPower(WoWPowerType.ShadowOrbs) < 3 && !(Me.IsChanneling && Me.ChanneledCastingSpellId == MIND_FLY_INSANITY))
                    {
                        utils.LogActivity("MIND_BLAST", target.Name);
                        return utils.Cast(MIND_BLAST, target);
                    }
                }
            }
            return false;
        }

        private bool CC()
        {
            if (utils.CanCast(PSYCHIC_SCREAM) && ShadowPriestSettings.Instance.UsePsychicScream && Me.Combat && (utils.AllAttaccableEnemyMobsInRange(8).Count() >= 1))
            {
                utils.LogActivity("PSYCHIC_SCREAM");
                return utils.Cast(PSYCHIC_SCREAM);
            }
            if ( utils.CanCast(VOID_TENDRILS) && ShadowPriestSettings.Instance.UseVoidTendrils && Me.Combat && (utils.AllAttaccableEnemyMobsInRange(8).Count() >= 1))
            {
                utils.LogActivity("VOID_TENDRILS");
                return utils.Cast(VOID_TENDRILS);
            }
            if (utils.CanCast(PSYFIEND) && ShadowPriestSettings.Instance.UsePsyfiend && Me.Combat && (utils.AllAttaccableEnemyMobsInRange(20).Count() >= 1))
            {
                utils.LogActivity("PSYFIEND");
                utils.Cast(PSYFIEND);
                return SpellManager.ClickRemoteLocation(Me.Location);
            }

            return false;
        }

        private bool DispellMagic()
        {
            if (ShadowPriestSettings.Instance.UseDispellMagic && Me.CurrentTarget != null && Me.CurrentTarget.Attackable && Me.CurrentTarget.IsAlive
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

        private bool Multidot()
        {
            if (ShadowPriestSettings.Instance.MultidotEnabled )
            {
                int enemyNumber = utils.AllAttaccableEnemyMobsInRangeTragettingMyParty(40f, ShadowPriestSettings.Instance.MultidotAvoidCC).Count();
                if (enemyNumber >= ShadowPriestSettings.Instance.Multidot_SW_Pain_EnemyNumberMin
                    /*|| enemyNumber >= ShadowPriestSettings.Instance.Multidot_VampiricTouch_EnemyNumberMin*/)
                {
                    WoWUnit TargetForMultidot = null;
                    //apply  ShadowWord:Pain and always refresh it right before the last tick;
                    if (/*utils.CanCast(SHADOW_WORD_PAIN) &&*/ utils.AllEnemyMobsHasMyAura(SHADOW_WORD_PAIN).Count() < ShadowPriestSettings.Instance.Multidot_SW_Pain_EnemyNumberMax)
                    {
                        TargetForMultidot = utils.NextApplyAuraTarget(SHADOW_WORD_PAIN, 40, 1500, ShadowPriestSettings.Instance.MultidotAvoidCC);
                        if (TargetForMultidot != null)
                        {
                            utils.LogActivity("   MULTIDOT SHADOW_WORD_PAIN  " , TargetForMultidot.Name);
                            return utils.Cast(SHADOW_WORD_PAIN, TargetForMultidot);
                        }
                    }
                    if (nextTimeVampiricTouchAllowed <= DateTime.Now /*&& utils.CanCast(VAMPIRIC_TOUCH)*/ && utils.AllEnemyMobsHasMyAura(VAMPIRIC_TOUCH).Count() < ShadowPriestSettings.Instance.Multidot_VampiricTouch_EnemyNumberMax)
                    {
                        TargetForMultidot = utils.NextApplyAuraTarget(VAMPIRIC_TOUCH, 40, 1500, ShadowPriestSettings.Instance.MultidotAvoidCC);
                        if (TargetForMultidot != null)
                        {
                            utils.LogActivity("   MULTIDOT VAMPIRIC_TOUCH  ", TargetForMultidot.Name);
                            SetNextTimeVampiricTouch();
                            return utils.Cast(VAMPIRIC_TOUCH, TargetForMultidot);
                        }
                    }
                }
            }
            return false;
        }

        private bool TankHealing()
        {
            if (tank != null && tank.IsAlive && tank.Distance - tank.CombatReach - 1 <= 40 && tank.InLineOfSight && (tank.Guid!=Me.Guid || Me.FocusedUnitGuid == Me.Guid))
            {
                if (tank.HealthPercent <= ShadowPriestSettings.Instance.VoidShiftTarget && Me.HealthPercent >= ShadowPriestSettings.Instance.VoidShiftMe
                    && ShadowPriestSettings.Instance.UseVoidShift && ShadowPriestSettings.Instance.UseVoidShiftOnTank && utils.CanCast(VOID_SHIFT, tank, true))
                {
                    utils.LogHealActivity(tank, " VOID_SHIFT", tank.Class.ToString());
                    return utils.Cast(VOID_SHIFT, tank);
                }

                if (tank.Combat && tank.HealthPercent <= ShadowPriestSettings.Instance.TankShieldPercent && !utils.isAuraActive(POWER_WORD_SHIELD, tank)
                    && !utils.isAuraActive(WEAKENED_SOUL, tank) && utils.CanCast(POWER_WORD_SHIELD))
                {
                    utils.LogHealActivity(tank, " TANK POWER_WORD_SHIELD", tank.Class.ToString());
                    return utils.Cast(POWER_WORD_SHIELD, tank);
                }

                if (ShadowPriestSettings.Instance.Use_PoM && talents.HasGlyph("Dark Binding") 
                    && !SoloBotType && utils.GroupNeedsMyAura("Prayer of Mending") && utils.CanCast(PRAYER_OF_MENDING, tank, true))
                {
                    utils.LogHealActivity(tank, " PRAYER_OF_MENDING", tank.Class.ToString());
                    return utils.Cast(PRAYER_OF_MENDING, tank);
                }
                if (talents.HasGlyph("Dark Binding") && tank.Combat && ShadowPriestSettings.Instance.RenewOnTank && !utils.isAuraActive(RENEW, tank))
                {
                    utils.LogHealActivity(tank, "tank RENEW", tank.Class.ToString());
                    return utils.Cast(RENEW, tank);
                }
            }
            else if (off_tank != null && off_tank.IsAlive && off_tank.Distance - off_tank.CombatReach - 1 <= 40 && off_tank.InLineOfSight)
            {
                if (off_tank.HealthPercent <= ShadowPriestSettings.Instance.VoidShiftTarget && Me.HealthPercent >= ShadowPriestSettings.Instance.VoidShiftMe
                    && ShadowPriestSettings.Instance.UseVoidShift && ShadowPriestSettings.Instance.UseVoidShiftOnOffTank && utils.CanCast(VOID_SHIFT, off_tank, true))
                {
                    utils.LogHealActivity(off_tank, " VOID_SHIFT", off_tank.Class.ToString());
                    return utils.Cast(VOID_SHIFT, off_tank);
                }

                if (off_tank.Combat && off_tank.HealthPercent <= ShadowPriestSettings.Instance.OffTankShieldPercent && !utils.isAuraActive(POWER_WORD_SHIELD, off_tank)
                    && !utils.isAuraActive(WEAKENED_SOUL, off_tank) && utils.CanCast(POWER_WORD_SHIELD))
                {
                    utils.LogHealActivity(off_tank, " OFF_TANK POWER_WORD_SHIELD", off_tank.Class.ToString());
                    return utils.Cast(POWER_WORD_SHIELD, off_tank);
                }

                if (ShadowPriestSettings.Instance.Use_PoM && talents.HasGlyph("Dark Binding") 
                    && !SoloBotType && utils.GroupNeedsMyAura("Prayer of Mending") && utils.CanCast(PRAYER_OF_MENDING, off_tank, true))
                {
                    utils.LogHealActivity(off_tank, " PRAYER_OF_MENDING", off_tank.Class.ToString());
                    return utils.Cast(PRAYER_OF_MENDING, off_tank);
                }
                if (talents.HasGlyph("Dark Binding") && off_tank.Combat && ShadowPriestSettings.Instance.RenewOnTank && !utils.isAuraActive(RENEW, off_tank))
                {
                    utils.LogHealActivity(off_tank, "offtank RENEW", tank.Class.ToString());
                    return utils.Cast(RENEW, off_tank);
                }
            }
            return false;
        }

        private bool AOE_Healing()
        {
            if ((utils.GetMemberCountBelowThreshold(ShadowPriestSettings.Instance.Cascade_Halo_HP) >= ShadowPriestSettings.Instance.Cascade_Halo_Number))
            {
                if (utils.CanCast(HALO))
                {
                    utils.LogActivity("HALO");
                    return utils.Cast(HALO);
                }
                else if (utils.CanCast(CASCADE))
                {
                    utils.LogActivity("CASCADE");
                    return utils.Cast(CASCADE,Me);
                }
            }

            else if (utils.CanCast(VAMPIRIC_EMBRANCE) && (utils.GetMemberCountBelowThreshold(ShadowPriestSettings.Instance.VampiricEmbranceHP) >= ShadowPriestSettings.Instance.VampiricEmbranceNumber))
            {           
                utils.LogActivity("VAMPIRIC_EMBRANCE");
                return utils.Cast(VAMPIRIC_EMBRANCE);

            }
            return false;
        }

        public void SetNextTimeVampiricTouch()
        {
            //2 seconds wait to avoid popping 2 consecutive vampiric touch
            nextTimeVampiricTouchAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 2500);
        }

        public void SetNextChannelMFSpellAllowed()
        {
            //2 seconds wait to avoid popping 2 consecutive vampiric touch
            nextChanneledMFSpellAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, ShadowPriestSettings.Instance.mindFlayDuration);
        }

        public void SetNextChannelMSSpellAllowed()
        {
            //2 seconds wait to avoid popping 2 consecutive vampiric touch
            nextChanneledMSSpellAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, ShadowPriestSettings.Instance.mindSearDuration);
        }

        private bool PowerInfusion()
        {
            if(ShadowPriestSettings.Instance.PowerInfusionUse == ShadowPriestSettings.PowerInfusionUseType.COOLDOWN && utils.CanCast(POWER_INFUSION))
            {
                utils.LogActivity("POWER_INFUSION ");
                return utils.Cast(POWER_INFUSION);
            }
            else if (ShadowPriestSettings.Instance.PowerInfusionUse == ShadowPriestSettings.PowerInfusionUseType.BOSS && utils.CanCast(POWER_INFUSION)
                && extra.IsTargetBoss())
            {
                utils.LogActivity("POWER_INFUSION ");
                return utils.Cast(POWER_INFUSION);
            } 
            return false;
        }

        public bool CombatRotation()
        {
            Interrupt();
            extra.UseHealthstone();
            extra.UseRacials();
            extra.UseTrinket1();
            extra.UseTrinket2();
            extra.UseEngineeringGloves();
            extra.UseLifeblood();
            extra.UseAlchemyFlask();
            extra.WaterSpirit();
            extra.LifeSpirit();
            ManaRegen();
            PowerInfusion();

            CC();
            Self();
            if (ShadowPriestSettings.Instance.TankHealing)
                TankHealing();

            WoWUnit target = null;
            if (ShadowPriestSettings.Instance.TargetTypeSelected == ShadowPriestSettings.TargetType.MANUAL)
                target = Me.CurrentTarget;
            else if (ShadowPriestSettings.Instance.TargetTypeSelected == ShadowPriestSettings.TargetType.AUTO)
            {
                target = utils.getTargetToAttack(40, tank);
            }
            else if (ShadowPriestSettings.Instance.TargetTypeSelected == ShadowPriestSettings.TargetType.SEMIAUTO)
            {
                target = Me.CurrentTarget;
                if (target == null || target.IsDead || !target.InLineOfSpellSight || target.Distance - target.CombatReach -1  > 40)
                    target = utils.getTargetToAttack(40, tank);
            }
            if (target != null && !target.IsDead && target.InLineOfSpellSight && target.Distance - target.CombatReach - 1 <= 40)
            {
                if (ShadowPriestSettings.Instance.TargetTypeSelected == ShadowPriestSettings.TargetType.AUTO)
                    target.Target();
                if ((ShadowPriestSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                {
                    Me.SetFacing(target);
                }

                ProcWork(target);
                //MindSear
                if (!Me.IsMoving && Me.GetCurrentPower(WoWPowerType.ShadowOrbs) < 3 && ShadowPriestSettings.Instance.UseMindSear && nextChanneledMSSpellAllowed <= DateTime.Now && !(Me.IsChanneling && Me.ChanneledCastingSpellId == MIND_FLY_INSANITY))
                {
                    enemyAroundOffTank = 0;
                    enemyAroundMainTank = 0;
                    if (tank != null && tank.IsAlive && tank.Distance - tank.CombatReach - 1 <= 40)
                        enemyAroundMainTank = utils.AllAttaccableEnemyMobsInRangeFromTarget(tank, 10).Count();
                    if (off_tank != null && off_tank.IsAlive && off_tank.Distance - off_tank.CombatReach - 1 <= 40)
                        enemyAroundOffTank = utils.AllAttaccableEnemyMobsInRangeFromTarget(off_tank, 10).Count();
                    if (enemyAroundMainTank >= enemyAroundOffTank && enemyAroundMainTank >= ShadowPriestSettings.Instance.MindSearEnemyNumberMin)
                    {
                        utils.LogActivity("MIND SEAR ", tank.Class.ToString());
                        SetNextChannelMSSpellAllowed();
                        return utils.Cast(MIND_SEAR, tank);
                    }
                    else if (enemyAroundOffTank >= enemyAroundMainTank && enemyAroundOffTank >= ShadowPriestSettings.Instance.MindSearEnemyNumberMin)
                    {
                        utils.LogActivity("MIND SEAR ", off_tank.Class.ToString());
                        SetNextChannelMSSpellAllowed();
                        return utils.Cast(MIND_SEAR, off_tank);
                    }
                    else if ((SoloBotType || ShadowPriestSettings.Instance.MindSearAlsoOnEnemies) && Me.CurrentTarget != null && Me.CurrentTarget.IsAlive && utils.AllAttaccableEnemyMobsInRangeFromTarget(Me.CurrentTarget, 10).Count() >= ShadowPriestSettings.Instance.MindSearEnemyNumberMin)
                    {
                        utils.LogActivity("MIND SEAR ", Me.CurrentTarget.Name);
                        SetNextChannelMSSpellAllowed();
                        return utils.Cast(MIND_SEAR, Me.CurrentTarget);
                    }
                }

                //aoe
                if (utils.AllAttaccableEnemyMobsInRangeFromTarget(Me, 40).Count() > ShadowPriestSettings.Instance.CascadeHaloDivineStarNumber && !(Me.IsChanneling && Me.ChanneledCastingSpellId == MIND_FLY_INSANITY))
                {
                    if (utils.CanCast(HALO))
                    {
                        utils.LogActivity("HALO");
                        return utils.Cast(HALO);
                    }
                    else if (utils.CanCast(CASCADE, target))
                    {
                        utils.LogActivity("CASCADE",target.Name);
                        return utils.Cast(CASCADE,target);
                    }
                    //divine start to be implemented
                }
                
                //DW:death
                if (Me.GetCurrentPower(WoWPowerType.ShadowOrbs) < 3 && target.HealthPercent <= 20 && utils.CanCast(SHADOW_WORD_DEATH, target) && !(Me.IsChanneling && Me.ChanneledCastingSpellId == MIND_FLY_INSANITY))
                {
                    utils.LogActivity("SHADOW_WORD_DEATH", target.Name);
                    return utils.Cast(SHADOW_WORD_DEATH, target);
                }

                //mind blast
                if ((!Me.IsMoving || utils.isAuraActive(DIVINE_INSIGHT, Me)) && Me.GetCurrentPower(WoWPowerType.ShadowOrbs) < 3 && (utils.GetSpellCooldown(MIND_BLAST).Milliseconds==0 || utils.isAuraActive(DIVINE_INSIGHT, Me))
                    && !(talents.IsSelected(9) && utils.MyAuraTimeLeft(DEVOURING_PLAGUE, target) > 0) && !(Me.IsChanneling && Me.ChanneledCastingSpellId == MIND_FLY_INSANITY))
                {
                    utils.LogActivity("MIND_BLAST", target.Name);
                    return utils.Cast(MIND_BLAST, target);
                }

                //apply dot
                if (/*utils.CanCast(SHADOW_WORD_PAIN, target) &&*/ utils.MyAuraTimeLeft(SHADOW_WORD_PAIN, target) < 3500
                    && !(talents.IsSelected(9) && utils.MyAuraTimeLeft(DEVOURING_PLAGUE, target) > 0) && !(Me.IsChanneling && Me.ChanneledCastingSpellId == MIND_FLY_INSANITY))
                {
                    utils.LogActivity("SHADOW_WORD_PAIN", target.Name);
                    return utils.Cast(SHADOW_WORD_PAIN, target);
                }
                if (!Me.IsMoving && nextTimeVampiricTouchAllowed <= DateTime.Now /*&& utils.CanCast(VAMPIRIC_TOUCH, target)*/ && utils.MyAuraTimeLeft(VAMPIRIC_TOUCH, target) < 4500
                    && !(talents.IsSelected(9) && utils.MyAuraTimeLeft(DEVOURING_PLAGUE, target) > 0) && !(Me.IsChanneling && Me.ChanneledCastingSpellId == MIND_FLY_INSANITY))
                {
                    utils.LogActivity("VAMPIRIC_TOUCH", target.Name);
                    SetNextTimeVampiricTouch();
                    return utils.Cast(VAMPIRIC_TOUCH, target);
                }

                //solace and insanity talented
                if (!Me.IsMoving && Me.GetCurrentPower(WoWPowerType.ShadowOrbs) < 3 && talents.IsSelected(9) && utils.MyAuraTimeLeft(DEVOURING_PLAGUE, target) > 0 /*&& utils.CanCast(MIND_FLY, target)*/
                    && nextChanneledMFSpellAllowed <= DateTime.Now && Me.CurrentChannelTimeLeft.Milliseconds <= 250)
                {
                    utils.LogActivity("MIND FLY POWERED", target.Name);
                    SetNextChannelMFSpellAllowed();
                    return utils.Cast(MIND_FLY, target);
                }

                //multidot
                if (!(talents.IsSelected(9) && utils.MyAuraTimeLeft(DEVOURING_PLAGUE, target) > 200) && utils.MyAuraTimeLeft(SHADOW_WORD_PAIN, target) >= 3500 && utils.MyAuraTimeLeft(VAMPIRIC_TOUCH, target) >= 4500 && Multidot())
                    return true;
                

                if (!Me.IsMoving && Me.GetCurrentPower(WoWPowerType.ShadowOrbs) < 3 && utils.MyAuraTimeLeft(VAMPIRIC_TOUCH, target) >= 4500
                    && utils.MyAuraTimeLeft(SHADOW_WORD_PAIN, target) >= 3500 /*&& utils.CanCast(MIND_FLY, target)*/
                    && nextChanneledMFSpellAllowed <= DateTime.Now && Me.CurrentChannelTimeLeft.Milliseconds <= 250)
                {
                    utils.LogActivity("MIND FLY", target.Name);
                    SetNextChannelMFSpellAllowed();
                    return utils.Cast(MIND_FLY, target);
                }


            }
            else if (SoloBotType && Me.CurrentTarget != null && !Me.CurrentTarget.IsDead && (!Me.CurrentTarget.InLineOfSpellSight || !Me.CurrentTarget.InLineOfSight || Me.CurrentTarget.Distance - Me.CurrentTarget.CombatReach - 1 > FrostMageSettings.Instance.PullDistance))
            {
                movement.KingHealMove(Me.CurrentTarget.Location, ShadowPriestSettings.Instance.PullDistance, true, true,target);
            }

            
            return false;

        }

        public override bool Combat
        {
            get
            {
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld || utils.IsGlobalCooldown(true) || Me.Silenced || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) 
                    || (utils.MeIsCastingWithLag() && !Me.IsChanneling)
                    || (Me.IsChanneling && Me.ChanneledCastingSpellId != MIND_FLY && Me.ChanneledCastingSpellId != MIND_FLY_INSANITY && Me.ChanneledCastingSpellId != MIND_SEAR))
                    //|| (Me.IsChanneling && (nextChanneledSpellAllowed > DateTime.Now || (Me.ChanneledCastingSpellId == MIND_FLY || Me.ChanneledCastingSpellId == MIND_FLY_INSANITY || Me.ChanneledCastingSpellId == MIND_SEAR) && Me.CurrentChannelTimeLeft.Milliseconds > 100))
                    return false;

                tank = utils.SimpleGetTank(40f);
                if (tank == null || !tank.IsValid || !tank.IsAlive) tank = Me;

                if (tank != null && (lastTank == null || lastTank.Guid != tank.Guid))
                {
                    lastTank = tank;
                    utils.LogActivity(TANK_CHANGE, tank.Class.ToString());
                } off_tank = utils.SimpleGetOffTank(40f, tank);
                if (off_tank != null && (lastOffTank == null || lastOffTank.Guid != off_tank.Guid))
                {
                    lastOffTank = off_tank;
                    utils.LogActivity(OFF_TANK_CHANGE, off_tank.Class.ToString());
                }
                return CombatRotation();
            }
        }
    }
}
