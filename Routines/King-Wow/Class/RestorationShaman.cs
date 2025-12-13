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
    public class RestorationShamanCombatClass : KingWoWAbstractBaseClass
    {

        private static string Name = "KingWoW RestorationShaman";

         #region CONSTANT AND VARIABLES

        //START OF CONSTANTS ==============================
        private const bool LOGGING = true;
        private const bool DEBUG = false;
        private const bool TRACE = false;
        private KingWoWUtility utils = null;
        private Movement movement = null;
        private ExtraUtils extra = null;
        private ShamanCommon shammyCommon = null;
        WoWUnit CurrentHealtarget = null;
        private TalentManager talents = null;

        private WoWUnit tank = null;
        private WoWUnit lastTank = null;
        private bool SoloBotType = false;
        private string BaseBot = "unknown";

        private const string DEBUG_LABEL = "DEBUG";
        private const string TRACE_LABEL = "TRACE";
        private const string TANK_CHANGE = "TANK CHANGED";
        private const string FACING = "FACING";

        //START OF SPELLS AND AURAS ==============================
        private const string DRINK = "Drink";
        private const string FOOD = "Food";

        
     
        private const string SEARING_TOTEM = "Searing Totem";
        private const string SEARING_FLAMES = "Searing Flames";
        private const string GROUNDING_TOTEM = "Grounding Totem";
        private const string CAPACITOR_TOTEM = "Capacitor Totem";
        private const string MANA_TIDE_TOTEM = "Mana Tide Totem";
        private const string SPIRIT_LINK_TOTEM = "Spirit Link Totem";

        private const string WATER_SHIELD = "Water Shield";
        private const string LIGHTNING_SHIELD = "Lightning Shield";
        private const string EARTH_SHIELD = "Earth Shield";
        private const string LIGHTNING_BOLT = "Lightning Bolt";
        private const string MAELSTROM_WEAPON = "Maelstrom Weapon";
        private const string WIND_SHEAR = "Wind Shear";      
        private const string FLAME_SHOCK = "Flame Shock";
        private const string EARTH_SHOCK = "Earth Shock";
        private const string LAVA_LASH = "Lava Lash";
        private const string STORMSTRIKE = "Stormstrike";
        private const string STORMBLAST = "Stormblast";
        private const string UNLEASH_ELEMENTS = "Unleash Elements";
        private const string UNLEASHED_FURY = "Unleashed Fury";
        private const string FIRE_NOVA = "Fire Nova";
        private const string MAGMA_TOTEM = "Magma Totem";
        private const string CHAIN_LIGHTNING = "Chain Lightning";
        private const string PURIFY_SPIRIT = "Purify Spirit";
        private const string ANCESTRAL_SWIFTNESS = "Ancestral Swiftness";
                                                

        //HEAL
        private const string ANCESTRAL_SPIRIT = "Ancestral Spirit";       
        private const string CHAIN_HEAL = "Chain Heal";
        private const string CLEANSE_SPIRIT = "Cleanse Spirit";
        private const string HEALING_SURGE = "Healing Surge";
        private const string HEALING_RAIN = "Healing Rain";
        private const string HEALING_WAVE = "Healing Wave";
        private const string GREATER_HEALING_WAVE = "Greater Healing Wave";

        private const string TIDAL_WAVES = "Tidal Waves";
              
        
        private const string HEALING_STREAM_TOTEM = "Healing Stream Totem";
        private const string RIPTIDE = "Riptide";

        private const string GHOST_WOLF = "Ghost Wolf";
        private const string HEX = "hex";
        private const string PURGE = "Purge";

        private const string TOTEMIC_RECALL = "Totemic Recall";
        
   
        

        //CD
        private const string ASCENDANCE = "Ascendance";
        private const string FERAL_SPIRIT = "Feral Spirit";
        private const string FIRE_ELEMENTAL_TOTEM = "Fire Elemental Totem";
        private const string SHAMANISTIC_RAGE = "Shamanistic Rage";
        private const string SPIRIT_WALK = "Spiritwalker's Grace";
        private const string STORMLASH_TOTEM = "Stormlash Totem";
          
        //END OF SPELLS AND AURAS ==============================

        //TALENTS
        private const string STONE_BULWALRK_TOTEM = "Stone Bulwark Totem";
        private const string ASTRAL_SHIFT = "Astral Shift";
        private const string EARTHGRAB_TOTEM = "Earthgrab Totem";
        private const string WINDWALK_TOTEM = "Windwalk Totem";
        private const string TOTEMIC_PROJECTION = "Totemic Projection";
        private const string CALL_OF_THE_ELEMENTS = "Call of the Elements";
        private const string ASTRAL_SWIFTNESS = "Ancestral Swiftness";
        private const string ELEMENTAL_MASTERY = "Elemental Mastery";
        private const string ANCESTRAL_GUIDANCE = "Ancestral Guidance";
        private const string HEALING_TIDE_TOTEM = "Healing Tide Totem";
        private const string ELEMENTAL_BLAST = "Elemental Blast";
        //END TALENTS
        //END OF CONSTANTS ==============================

        #endregion

        public RestorationShamanCombatClass()
        {
            utils = new KingWoWUtility();
            movement = new Movement();
            extra = new ExtraUtils();
            shammyCommon = new ShamanCommon();
            tank = null;
            lastTank = null; ;
            SoloBotType = false;
            BaseBot = "unknown";
            talents = new TalentManager();

        }

        private bool HealingRain(bool lockframe)
        {
            WoWPlayer hr_target = null;
            if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && utils.CanCast(HEALING_RAIN))
            {
                if (RestorationShamanSettings.Instance.HealingRainOnlyOnTankLocation && tank != null &&
                    (utils.AOEHealCount((WoWPlayer)tank, RestorationShamanSettings.Instance.HealingRainPercent, utils.HEALING_RAIN_Radius()) >= RestorationShamanSettings.Instance.HealingRainNumber))
                {
                    hr_target = (WoWPlayer)tank;
                }
                else if (!RestorationShamanSettings.Instance.HealingRainOnlyOnTankLocation)
                {
                    hr_target = utils.BestHealingRainTargetLocation(lockframe, RestorationShamanSettings.Instance.HealingRainPercent, RestorationShamanSettings.Instance.HealingRainNumber);
                }
                if (hr_target != null)
                {
                    if(utils.CanCast(UNLEASH_ELEMENTS,hr_target,true))
                    {
                        utils.LogActivity(UNLEASH_ELEMENTS, hr_target.Class.ToString());
                        utils.Cast(UNLEASH_ELEMENTS,hr_target);
                    }
                    utils.LogActivity(HEALING_RAIN, hr_target.Class.ToString());
                    utils.Cast(HEALING_RAIN);
                    return SpellManager.ClickRemoteLocation(hr_target.Location);
                }
            }
            return false;
        }

        private bool ChainHeal(bool lockframe)
        {
            WoWPlayer ch_target = null;
            if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && utils.CanCast(CHAIN_HEAL))
            {
                if (RestorationShamanSettings.Instance.ChainHealOnlyFromTank && tank != null &&
                    (utils.AOEHealCount((WoWPlayer)tank, RestorationShamanSettings.Instance.ChainHealPercent, utils.CHAIN_HEAL_Radius()) >= RestorationShamanSettings.Instance.ChainHealNumber))
                {
                    ch_target = (WoWPlayer)tank;
                }
                else if (!RestorationShamanSettings.Instance.ChainHealOnlyFromTank)
                {
                    ch_target = utils.BestChaiHeal_Target(RestorationShamanSettings.Instance.ChainHealPercent, RestorationShamanSettings.Instance.ChainHealNumber);
                }
                if (ch_target != null)
                {
                    if (utils.CanCast(UNLEASH_ELEMENTS))
                    {
                        utils.LogActivity(UNLEASH_ELEMENTS, ch_target.Class.ToString());
                        utils.Cast(UNLEASH_ELEMENTS, ch_target);
                    }
                    utils.LogActivity(CHAIN_HEAL, ch_target.Class.ToString());
                    return utils.Cast(CHAIN_HEAL, ch_target);
                }
            }
            return false;

        }

        public override bool Pull
        {
            get
            {
                WoWUnit target = Me.CurrentTarget;
                if (target != null && !target.IsFriendly && target.Attackable && !target.IsDead)
                {
                    if (!target.InLineOfSight || !target.InLineOfSpellSight || target.Distance > RestorationShamanSettings.Instance.PullDistance)
                    {
                        movement.KingHealMove(target.Location, RestorationShamanSettings.Instance.PullDistance, true, true, target);
                    }
                    if (!Me.IsMoving && !Me.IsFacing(target))
                    {
                        utils.LogActivity(FACING, target.Name);
                        Me.SetFacing(target);
                    }
                    if (utils.CanCast(FLAME_SHOCK))
                    {
                        utils.LogActivity(FLAME_SHOCK, target.Name);
                        return utils.Cast(FLAME_SHOCK, target);
                    }
                }
                return false;
            }
        }

        public override bool Pulse
        {
            get
            {
                if (RestorationShamanSettings.Instance.TrySaveMana)
                    StopCastingCheck();
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !Me.Mounted && !StyxWoW.IsInGame || !StyxWoW.IsInWorld || utils.IsGlobalCooldown(true) || utils.MeIsChanneling || Me.IsCasting
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
                if (!Me.Combat && !Me.Mounted && RestorationShamanSettings.Instance.OOCHealing && !utils.isAuraActive(DRINK) && !utils.isAuraActive(FOOD))
                {
                    Resurrect();

                    HealingRain(false);
                    ChainHeal(false);

                    WoWUnit healTarget = utils.GetHealTarget(40f);

                    if (healTarget != null && healTarget.HealthPercent < 100)
                    {
                        double hp = healTarget.HealthPercent;
                     
                        if (!utils.isAuraActive(TIDAL_WAVES, Me) && utils.MyAuraTimeLeft(RIPTIDE,healTarget) == 0)
                        {
                            utils.LogHealActivity(healTarget, RIPTIDE, healTarget.Class.ToString());
                            return utils.Cast(RIPTIDE, healTarget);
                        }
                        if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && hp < RestorationShamanSettings.Instance.GreaterHealingWavePercent && utils.CanCast(GREATER_HEALING_WAVE))
                        {
                            if (utils.CanCast(UNLEASH_ELEMENTS))
                            {
                                utils.LogActivity(UNLEASH_ELEMENTS, healTarget.Class.ToString());
                                utils.Cast(UNLEASH_ELEMENTS, healTarget);
                            }
                            utils.LogHealActivity(healTarget, GREATER_HEALING_WAVE, healTarget.Class.ToString());
                            CurrentHealtarget = healTarget;
                            return utils.Cast(GREATER_HEALING_WAVE, healTarget);
                        }
                        if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && hp <= RestorationShamanSettings.Instance.HealingWavePercent && utils.CanCast(HEALING_WAVE))
                        {
                            utils.LogHealActivity(healTarget, HEALING_WAVE, healTarget.Class.ToString());
                            CurrentHealtarget = healTarget;
                            return utils.Cast(HEALING_WAVE, healTarget);
                        }

                    }
                }
                if (!Me.Combat && tank != null && tank.Combat)
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
                if (Me.ManaPercent <= RestorationShamanSettings.Instance.ManaPercent &&
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
                if (Me.HealthPercent <= RestorationShamanSettings.Instance.HealthPercent &&
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
                return Buff();
            }
        }

        public override bool NeedCombatBuffs { get { return Buff(); } }

        public override bool NeedPreCombatBuffs { get { return Buff(); } }

        private bool BotUpdate()
        {
            if (BaseBot.Equals(BotManager.Current.Name))
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
                SoloBotType = false;
                BaseBot = BotManager.Current.Name;
                return true;
            }

            Logging.Write("Base bot detected: " + BotManager.Current.Name);
            SoloBotType = true;
            BaseBot = BotManager.Current.Name;
            return true;


        }

        private bool ManaRegen()
        {
            if (Me.ManaPercent <= RestorationShamanSettings.Instance.ManaTideTotemPercent && utils.CanCast(MANA_TIDE_TOTEM)
                && !Me.IsMoving && !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.HealingTide || t.WoWTotem == WoWTotem.ManaTide || t.WoWTotem == WoWTotem.HealingStream))
            {
                utils.LogActivity(MANA_TIDE_TOTEM);
                return utils.Cast(MANA_TIDE_TOTEM);
            }

            return false;
        }

        private bool Self()
        {
            if (RestorationShamanSettings.Instance.SelfHealingPriorityEnabled && Me.HealthPercent <= RestorationShamanSettings.Instance.SelfHealingPriorityHP)
            {    
                if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && Me.HealthPercent < RestorationShamanSettings.Instance.HealingSurgePercent && utils.CanCast(HEALING_SURGE))
                {
                    utils.LogHealActivity(Me, HEALING_SURGE, Me.Class.ToString());
                    CurrentHealtarget = Me;
                    return utils.Cast(HEALING_SURGE, Me);
                }
                if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && Me.HealthPercent < RestorationShamanSettings.Instance.GreaterHealingWavePercent && utils.CanCast(GREATER_HEALING_WAVE))
                {
                    utils.LogHealActivity(Me, GREATER_HEALING_WAVE, Me.Class.ToString());
                    CurrentHealtarget = Me;
                    return utils.Cast(GREATER_HEALING_WAVE, Me);
                }
                if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && Me.HealthPercent <= RestorationShamanSettings.Instance.HealingWavePercent && utils.CanCast(HEALING_WAVE))
                {
                    utils.LogHealActivity(Me, HEALING_WAVE, Me.Class.ToString());
                    CurrentHealtarget = Me;
                    return utils.Cast(HEALING_WAVE, Me);
                }             
            }
            return false;
        }

        private bool AoE()
        {
            //Healing Rain
            HealingRain(true);

            if (!ExtraUtilsSettings.Instance.DisableAOE_HealingRotation)
            {
                //Spirit Link Totem
                if (utils.AOEHealCount(Me, RestorationShamanSettings.Instance.SpiritLinkPercent, 10) >= RestorationShamanSettings.Instance.SpiritLinkNumber
                    && utils.CanCast(SPIRIT_LINK_TOTEM))
                {
                    utils.LogActivity(SPIRIT_LINK_TOTEM);
                    return utils.Cast(SPIRIT_LINK_TOTEM);
                }

                //Healing Stream Totem
                if (utils.CanCast(HEALING_STREAM_TOTEM) &&
                   !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.HealingTide || t.WoWTotem == WoWTotem.ManaTide || t.WoWTotem == WoWTotem.HealingStream))
                {
                    switch (RestorationShamanSettings.Instance.WhenCastStreamTotem)
                    {
                        case RestorationShamanSettings.healingTotemCastType.ALWAYS:
                            {
                                utils.LogActivity(HEALING_STREAM_TOTEM);
                                return utils.Cast(HEALING_STREAM_TOTEM);
                            }
                        case RestorationShamanSettings.healingTotemCastType.AT_CONDITION:
                            {
                                if (utils.AOEHealCount(Me, RestorationShamanSettings.Instance.StreamTotemPercent, 30) >= RestorationShamanSettings.Instance.StreamTotemNumber)
                                {
                                    utils.LogActivity(HEALING_STREAM_TOTEM);
                                    return utils.Cast(HEALING_STREAM_TOTEM);
                                }
                                break;
                            }
                    }
                }

                //Healing Tide Totem 
                if (utils.CanCast(HEALING_TIDE_TOTEM) && !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.HealingTide || t.WoWTotem == WoWTotem.ManaTide || t.WoWTotem == WoWTotem.HealingStream))
                {
                    switch (RestorationShamanSettings.Instance.WhenCastHealingTideTotem)
                    {
                        case RestorationShamanSettings.healingTotemCastType.ALWAYS:
                            {
                                utils.LogActivity(HEALING_TIDE_TOTEM);
                                return utils.Cast(HEALING_TIDE_TOTEM);
                            }
                        case RestorationShamanSettings.healingTotemCastType.AT_CONDITION:
                            {
                                if (utils.AOEHealCount(Me, RestorationShamanSettings.Instance.HealingTideTotemPercent, 40) >= RestorationShamanSettings.Instance.HealingTideTotemNumber)
                                {
                                    utils.LogActivity(HEALING_TIDE_TOTEM);
                                    return utils.Cast(HEALING_TIDE_TOTEM);
                                }
                                break;
                            }
                    }
                }

                //Chain Heal
                ChainHeal(true);

            }
            return false;
        }

        private bool UseAscendance()
        {
            //ascendance
            if (!utils.isAuraActive(ASCENDANCE) && utils.CanCast(ASCENDANCE) && utils.GetSpellCooldown(ASCENDANCE).Milliseconds <= StyxWoW.WoWClient.Latency)
            {
                if(RestorationShamanSettings.Instance.WhenUseAscendance == RestorationShamanSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(ASCENDANCE);
                    return utils.Cast(ASCENDANCE);
                }
                else if (RestorationShamanSettings.Instance.WhenUseAscendance == RestorationShamanSettings.CDUseType.AT_CONDITION
                    && (utils.GetMemberCountBelowThreshold(RestorationShamanSettings.Instance.AscendanceHP) >= RestorationShamanSettings.Instance.AscendanceNumber))
                {
                    utils.LogActivity(ASCENDANCE);
                    return utils.Cast(ASCENDANCE);
                }
            }
            return false;
        }

        private bool Healing()
        {
            WoWUnit healTarget = utils.GetHealTarget(40f);

            if (healTarget != null)
            {
                double hp = healTarget.HealthPercent;
                if (RestorationShamanSettings.Instance.MantainTidalWaves && !utils.isAuraActive(TIDAL_WAVES) && utils.CanCast(RIPTIDE, healTarget, true))
                {
                    utils.LogActivity("RIPTIDE to Mantain TIDAL WAVES on " + healTarget.Class.ToString());
                    utils.Cast(RIPTIDE, healTarget);
                }
                if (Me.Combat && hp <= RestorationShamanSettings.Instance.HealingWavePercent && Me.IsMoving && !utils.isAuraActive(SPIRIT_WALK) && RestorationShamanSettings.Instance.Autocast_Spirit_Walk)
                {
                    utils.LogActivity(SPIRIT_WALK);
                    return utils.Cast(SPIRIT_WALK);
                }

                if (healTarget.Distance - healTarget.CombatReach -1  > 40 || !healTarget.InLineOfSight)
                {
                    return false;
                }
                else
                {
                    if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && hp <= RestorationShamanSettings.Instance.HealingSurgePercent && utils.CanCast(HEALING_SURGE))
                    {
                        utils.LogHealActivity(healTarget, HEALING_SURGE, healTarget.Class.ToString());
                        CurrentHealtarget = healTarget;
                        return utils.Cast(HEALING_SURGE, healTarget);
                    }
                    if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && hp < RestorationShamanSettings.Instance.GreaterHealingWavePercent && utils.CanCast(GREATER_HEALING_WAVE))
                    {
                        if (utils.CanCast(UNLEASH_ELEMENTS))
                        {
                            utils.LogActivity(UNLEASH_ELEMENTS, healTarget.Class.ToString());
                            utils.Cast(UNLEASH_ELEMENTS, healTarget);
                        }
                        if (utils.CanCast(ANCESTRAL_SWIFTNESS))
                        {
                            utils.LogActivity(ANCESTRAL_SWIFTNESS);
                            utils.Cast(ANCESTRAL_SWIFTNESS);
                        }
                        utils.LogHealActivity(healTarget, GREATER_HEALING_WAVE, healTarget.Class.ToString());
                        CurrentHealtarget = healTarget;
                        return utils.Cast(GREATER_HEALING_WAVE, healTarget);
                    }
                    if (hp <= RestorationShamanSettings.Instance.RiptidePercent && utils.MyAuraTimeLeft(RIPTIDE, healTarget) < 1500)
                    {
                        utils.LogHealActivity(healTarget, RIPTIDE, healTarget.Class.ToString());
                        return utils.Cast(RIPTIDE, healTarget);
                    }
                    if ((!Me.IsMoving || utils.isAuraActive(SPIRIT_WALK)) && hp <= RestorationShamanSettings.Instance.HealingWavePercent && utils.CanCast(HEALING_WAVE))
                    {
                        utils.LogHealActivity(healTarget, HEALING_WAVE, healTarget.Class.ToString());
                        CurrentHealtarget = healTarget;
                        return utils.Cast(HEALING_WAVE, healTarget);
                    }
                    
                }

            }

            if (RestorationShamanSettings.Instance.LB_for_rec_mana && talents.HasGlyph("Telluric Currents"))
            {
                WoWUnit target = utils.getTargetToAttack(40, tank);
                if (target != null && !target.IsDead && utils.CanCast(LIGHTNING_BOLT, target, true))
                {
                    if ((RestorationShamanSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                    {
                        Me.SetFacing(target);
                    }
                    utils.LogActivity(LIGHTNING_BOLT, target.Name);
                    return utils.Cast(LIGHTNING_BOLT, target);
                }
            }
            return false;
        }

        private bool interrut_CC()
        {
            WoWUnit target = utils.getTargetToAttack(30f, tank);
            if (target != null && !target.IsDead)
            {
                if ((RestorationShamanSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                {
                    Me.SetFacing(target);
                }

                if (RestorationShamanSettings.Instance.AutoInterrupt && (target.IsCasting || target.IsChanneling) && target.CanInterruptCurrentSpellCast
                    && utils.GetSpellCooldown(WIND_SHEAR).Milliseconds <= StyxWoW.WoWClient.Latency)
                {
                    if (utils.CanCast(WIND_SHEAR, target, true))
                    {
                        utils.LogActivity(WIND_SHEAR, target.Name);
                        return utils.Cast(WIND_SHEAR, target);
                    }
                }
                WoWUnit focused_unit = Me.FocusedUnit;
                if (RestorationShamanSettings.Instance.UseHex && focused_unit != null && focused_unit.Attackable && focused_unit.InLineOfSpellSight && 
                    focused_unit.Distance <= 30 && !utils.isAuraActive(HEX, target) && utils.CanCast(HEX, focused_unit, true))
                {
                    utils.LogActivity(HEX, focused_unit.Name);
                    return utils.Cast(HEX, focused_unit);
                }
            }
            return false;
        }

        private bool TankHealing()
        {
            if (tank != null && tank.IsAlive && tank.Distance < 40 && tank.InLineOfSight)
            {
                //earth shield
                if (tank.Guid != Me.Guid && !utils.isAuraActive(EARTH_SHIELD, tank) && utils.CanCast(EARTH_SHIELD))
                {
                    utils.LogActivity(EARTH_SHIELD, tank.Class.ToString());
                    return utils.Cast(EARTH_SHIELD, tank);
                }
                //riptide if configured always up
                if (RestorationShamanSettings.Instance.RiptideAlwaysUPOnTank && utils.MyAuraTimeLeft(RIPTIDE,tank)< 1500 && utils.CanCast(RIPTIDE))
                {
                    utils.LogActivity(RIPTIDE, tank.Class.ToString());
                    return utils.Cast(RIPTIDE, tank);
                }
                if (!Me.IsMoving && tank.HealthPercent < RestorationShamanSettings.Instance.GreaterHealingWavePercent && utils.CanCast(GREATER_HEALING_WAVE))
                {
                    if (utils.CanCast(UNLEASH_ELEMENTS))
                    {
                        utils.LogActivity(UNLEASH_ELEMENTS, tank.Class.ToString());
                        utils.Cast(UNLEASH_ELEMENTS, tank);
                    }
                    if (utils.CanCast(ANCESTRAL_SWIFTNESS))
                    {
                        utils.LogActivity(ANCESTRAL_SWIFTNESS);
                        utils.Cast(ANCESTRAL_SWIFTNESS);
                    }
                    utils.LogHealActivity(tank, GREATER_HEALING_WAVE, tank.Class.ToString());
                    CurrentHealtarget = tank;
                    return utils.Cast(GREATER_HEALING_WAVE, tank);
                }
            }
            
            return false;
        }

        private bool Cleansing()
        {
            if (RestorationShamanSettings.Instance.UseDispell)
            {
                WoWPlayer player = utils.GetDispellTargetShammy(40f);

                if (player != null && player.Distance < 40 && player.InLineOfSight)
                {
                    if (utils.CanCast(PURIFY_SPIRIT))
                    {
                        utils.LogActivity(PURIFY_SPIRIT, player.Class.ToString());
                        return utils.Cast(PURIFY_SPIRIT, player);
                    }
                }
            }
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
                    else if (utils.CanCast(ANCESTRAL_SPIRIT, player) && RestorationShamanSettings.Instance.UseResurrection && !Me.IsMoving)
                    {
                        utils.LogActivity(ANCESTRAL_SPIRIT, player.Class.ToString());
                        Blacklist.Add(player,BlacklistFlags.All, new TimeSpan(0, 0, 60));
                        return utils.Cast(ANCESTRAL_SPIRIT, player);
                    }

                    return false;
                }
            }

            return false;
        }

        private bool Buff()
        {
            if (utils.Mounted() /*ExtraUtilsSettings.Instance.PauseRotation || */)
                return false;
            //Weapon
            if (RestorationShamanSettings.Instance.imbueType == RestorationShamanSettings.ImbueType.DEFAULT)
                shammyCommon.ImbueMainHand(Imbue.Earthliving);


            //shield
            if (!utils.isAuraActive(WATER_SHIELD, Me) && utils.CanCast(WATER_SHIELD))
            {
                utils.LogActivity(WATER_SHIELD);
                return utils.Cast(WATER_SHIELD);
            }

            return shammyCommon.NeedToRecallTotems();
        }

        private bool SoS()
        {
            if (RestorationShamanSettings.Instance.UseSoS)
            {
                WoWUnit SoShealTarget;
                if (RestorationShamanSettings.Instance.SoS_healing_only_on_tank)
                    SoShealTarget = tank;
                else
                    SoShealTarget = utils.GetHealTarget(40f);

                if (SoShealTarget != null && SoShealTarget.HealthPercent <= RestorationShamanSettings.Instance.SoSHP
                    && SoShealTarget.InLineOfSpellSight && (SoShealTarget.Distance - SoShealTarget.CombatReach -1  <= 40))
                {
                    if (utils.CanCast(UNLEASH_ELEMENTS))
                    {
                        utils.LogActivity("SoS" + UNLEASH_ELEMENTS, SoShealTarget.Class.ToString());
                        utils.Cast(UNLEASH_ELEMENTS, SoShealTarget);
                    }
                    if (utils.CanCast(ANCESTRAL_SWIFTNESS) && utils.CanCast(GREATER_HEALING_WAVE, SoShealTarget, true))
                    {
                        utils.LogActivity("SoS" + ANCESTRAL_SWIFTNESS);
                        utils.Cast(ANCESTRAL_SWIFTNESS);
                        utils.LogHealActivity(SoShealTarget, "SoS" + GREATER_HEALING_WAVE, SoShealTarget.Class.ToString());
                        CurrentHealtarget = SoShealTarget;
                        return utils.Cast(GREATER_HEALING_WAVE, SoShealTarget);
                    }
                    if (utils.CanCast(HEALING_SURGE, SoShealTarget, true))
                    {
                        utils.LogHealActivity(SoShealTarget, "SoS" + HEALING_SURGE, SoShealTarget.Class.ToString());
                        CurrentHealtarget = SoShealTarget;
                        return utils.Cast(HEALING_SURGE, SoShealTarget);
                    }
                }
            }
            return false;
        }

        public bool CombatRotation()
        {
            extra.UseHealthstone();
            extra.UseRacials();
            extra.UseTrinket1();
            extra.UseTrinket2();
            extra.UseEngineeringGloves();
            extra.UseLifeblood();
            extra.UseAlchemyFlask();
            extra.WaterSpirit();
            extra.LifeSpirit();
            Buff();

            //relocate totem if talented
            if (!Me.IsMoving && Me.Combat && StyxWoW.Me.Totems.Count() > 0 && utils.CanCast(TOTEMIC_PROJECTION) && shammyCommon.TotemsInRangeOf(Me) == 0)
            {
                utils.LogActivity(TOTEMIC_PROJECTION);
                utils.Cast(TOTEMIC_PROJECTION);
                return SpellManager.ClickRemoteLocation(Me.Location);

            }

            SoS();
            Self();
            interrut_CC();
            ManaRegen();
            UseFireTotem();
            UseAscendance();
            TankHealing();
            AoE();
            Cleansing();
            Healing();
            return false;

        }

        private bool UseFireTotem()
        {
            if (!Me.IsMoving && Me.GotTarget && Me.CurrentTarget.Distance < shammyCommon.GetTotemRange(WoWTotem.Searing) - 2f
                && utils.CanCast(SEARING_TOTEM)
                && !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental || t.WoWTotem == WoWTotem.Searing || t.WoWTotem == WoWTotem.Magma))
            {
                utils.LogActivity(SEARING_TOTEM);
                return utils.Cast(SEARING_TOTEM);
            }
            return false;
        }

        public void StopCastingCheck()
        {
            //Stop Casting Healing Spells
            if (Me.CastingSpell != null && Me.CurrentCastTimeLeft != null &&
                (Me.CastingSpell.Name == HEALING_WAVE || Me.CastingSpell.Name == HEALING_SURGE ||
                 Me.CastingSpell.Name == GREATER_HEALING_WAVE) && Me.CurrentCastTimeLeft.TotalMilliseconds > 500)
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
                        Me.CastingSpell.Name == GREATER_HEALING_WAVE &&
                        (CurrentHealtarget.HealthPercent > 80 ||
                        CurrentHealtarget.HealthPercent >= RestorationShamanSettings.Instance.GreaterHealingWavePercent + 20))
                    {
                        SpellManager.StopCasting();
                        Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Save Mana Greater heal");
                    }

                    if (Me.IsCasting && CurrentHealtarget != null && CurrentHealtarget.IsValid &&
                        Me.CastingSpell.Name == HEALING_SURGE &&
                        (CurrentHealtarget.HealthPercent > 80 ||
                        CurrentHealtarget.HealthPercent >= RestorationShamanSettings.Instance.HealingSurgePercent + 30))
                    {
                        SpellManager.StopCasting();
                        Logging.Write(DateTime.Now.ToString("ss:fff ") + "Stop Casting: Save Mana Flash Heal");
                    }

                    if (Me.IsCasting && CurrentHealtarget != null && CurrentHealtarget.IsValid &&
                        Me.CastingSpell.Name == HEALING_WAVE &&
                        Me.CurrentCastTimeLeft.TotalMilliseconds > 500 &&
                        CurrentHealtarget.HealthPercent < RestorationShamanSettings.Instance.GreaterHealingWavePercent &&
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

        public override bool Combat
        {
            get
            {                
                if (RestorationShamanSettings.Instance.TrySaveMana)
                    StopCastingCheck();
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !Me.Mounted && !StyxWoW.IsInGame || !StyxWoW.IsInWorld || utils.IsGlobalCooldown(true) || utils.MeIsChanneling || Me.IsCasting
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
    }
}
