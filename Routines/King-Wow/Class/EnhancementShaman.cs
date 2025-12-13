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
    class EnhancementShamanCombatClass : KingWoWAbstractBaseClass
    {

        private static string Name = "KingWoW EnhancementShaman'";

        #region CONSTANT AND VARIABLES

        //START OF CONSTANTS ==============================
        private const bool LOGGING = true;
        private const bool DEBUG = false;
        private const bool TRACE = false;
        private KingWoWUtility utils = null;
        private Movement movement = null;
        private ExtraUtils extra = null;
        private ShamanCommon shammyCommon = null;

        private WoWUnit tank = null;
        private WoWUnit lastTank = null;
        private bool SoloBotType = false;
        private string BaseBot = "unknown";
        private TalentManager talents = null;

        private const string DEBUG_LABEL = "DEBUG";
        private const string TRACE_LABEL = "TRACE";
        private const string TANK_CHANGE = "TANK CHANGED";
        private const string FACING = "FACING";

        private bool EverythingOnCoolDown
        { get { return (StromStrikeOnCoolDown && LavaLashOnCoolDown && UnleashedElementsOnCoolDown); } }


        private bool LavaLashOnCoolDown
        { get { return utils.GetSpellCooldown(LAVA_LASH).Milliseconds > StyxWoW.WoWClient.Latency; } }

        private bool StromStrikeOnCoolDown
        { get { return utils.GetSpellCooldown(STORMSTRIKE).Milliseconds > StyxWoW.WoWClient.Latency; } }

        private bool StromsBlastOnCoolDown
        { get { return utils.GetSpellCooldown(STORMBLAST).Milliseconds > StyxWoW.WoWClient.Latency; } }
          
        
        private bool UnleashedElementsOnCoolDown
        { get { return utils.GetSpellCooldown(UNLEASH_ELEMENTS).Milliseconds > StyxWoW.WoWClient.Latency; } }
     

        //START OF SPELLS AND AURAS ==============================
        private const string DRINK = "Drink";
        private const string FOOD = "Food";

        
     
        private const string SEARING_TOTEM = "Searing Totem";
        private const string SEARING_FLAMES = "Searing Flames";
        private const string GROUNDING_TOTEM = "Grounding Totem";
        private const string CAPACITOR_TOTEM = "Capacitor Totem";

        private const string WATER_SHIELD = "Water Shield";
        private const string LIGHTNING_SHIELD = "Lightning Shield";
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
                                                

        //HEAL
        private const string ANCESTRAL_SPIRIT = "Ancestral Spirit";       
        private const string CHAIN_HEAL = "Chain heal";
        private const string CLEANSE_SPIRIT = "Cleanse Spirit";
        private const string HEALING_SURGE = "Healing Surge";
        private const string HEALING_RAIN = "Healing Rain";
        private const string HEALING_STREAM_TOTEM = "Healing Stream Totem";

        private const string GHOST_WOLF = "Ghost Wolf";
        private const string HEX = "hex";
        private const string PURGE = "Purge";

        private const string SPIRIT_WALK = "Spirit Walk";
        private const string TOTEMIC_RECALL = "Totemic Recall";
        
   
        

        //CD
        private const string ASCENDANCE = "Ascendance";
        private const string FERAL_SPIRIT = "Feral Spirit";
        private const string FIRE_ELEMENTAL_TOTEM = "Fire Elemental Totem";
        private const string SHAMANISTIC_RAGE = "Shamanistic Rage";
        private const string SPIRITWALKERS_GRACE = "Spiritwalker's Grace";
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
        private const string HEALIG_TIDE_TOTEM = "Healing Tide Totem";
        private const string ELEMENTAL_BLAST = "Elemental Blast";
        //END TALENTS
        //END OF CONSTANTS ==============================

        #endregion

        public EnhancementShamanCombatClass()
        {
            utils = new KingWoWUtility();
            movement = new Movement();
            extra = new ExtraUtils();
            shammyCommon = new ShamanCommon();
            tank = null;
            lastTank = null;;
            SoloBotType = false;
            BaseBot = "unknown";
            talents = new TalentManager();
        }

        public override bool Combat
        {
            get 
            {
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld || /*utils.IsGlobalCooldown(true) ||*/ utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) || Me.IsChanneling || utils.MeIsCastingWithLag())
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
                return CombatRotation();
            }
        }

        public override bool Pulse
        {
            get 
            {
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld || /*utils.IsGlobalCooldown(true) ||*/ utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) || Me.IsChanneling || utils.MeIsCastingWithLag())
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
                //try full me
                if (EnhancementShamanSettings.Instance.FullMeOOC)
                {
                    if (!Me.IsMoving && !Me.Combat && !Me.Mounted && !utils.isAuraActive(DRINK) && !utils.isAuraActive(FOOD)
                        && ((Me.ManaPercent >= 70 && Me.HealthPercent < 90) || (utils.PlayerCountBuff(MAELSTROM_WEAPON) > 2 && Me.HealthPercent < 95)))
                    {
                        if (utils.CanCast(HEALING_SURGE))
                        {
                            utils.LogActivity(HEALING_SURGE, Me.Class.ToString());
                            return utils.Cast(HEALING_SURGE, Me);
                        }
                    }
                }
                if (EnhancementShamanSettings.Instance.Totemic_recall_OOC && !Me.Combat 
                    && StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.Searing || t.WoWTotem == WoWTotem.Magma) && utils.CanCast(TOTEMIC_RECALL))
                {
                    utils.LogActivity(TOTEMIC_RECALL);
                    return utils.Cast(TOTEMIC_RECALL);
                }
                return false; 
            }
        }

        public override bool Pull
        {
            get
            {
                WoWUnit target = Me.CurrentTarget;
                if (target != null && !target.IsFriendly && target.Attackable && !target.IsDead)
                {
                    if (!target.InLineOfSight || !target.InLineOfSpellSight || target.Distance2DSqr > EnhancementShamanSettings.Instance.PullDistance * EnhancementShamanSettings.Instance.PullDistance)
                    {
                        movement.KingHealMove(target.Location, EnhancementShamanSettings.Instance.PullDistance, true, true, target);
                    }
                    if (!Me.IsMoving && !Me.IsFacing(target))
                    {
                        Me.SetFacing(target);
                    }

                    if (utils.CanCast(UNLEASH_ELEMENTS,target,true) )
                    {                        
                        utils.LogActivity(UNLEASH_ELEMENTS, target.Name);
                        return utils.Cast(UNLEASH_ELEMENTS, target);
                    }
                    if (utils.CanCast(FLAME_SHOCK,target,true))
                    {
                        utils.LogActivity(FLAME_SHOCK, target.Name);
                        return utils.Cast(FLAME_SHOCK, target);
                    }
                    if (utils.CanCast(LIGHTNING_BOLT, target, true))
                    {
                        utils.LogActivity(LIGHTNING_BOLT, target.Name);
                        return utils.Cast(LIGHTNING_BOLT, target);
                    }
                }
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
            talents.Update();
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
                if (Me.ManaPercent <= EnhancementShamanSettings.Instance.ManaPercent &&
                !utils.isAuraActive(DRINK) && !Me.Combat && !Me.IsMoving && !utils.MeIsCastingWithLag())
                {
                    WoWItem mydrink = Consumable.GetBestDrink(false);
                    if (mydrink != null)
                    {
                        utils.LogActivity("Drinking/Eating");
                        Styx.CommonBot.Rest.DrinkImmediate();
                        return true;
                    }
                }
                if (Me.HealthPercent <= EnhancementShamanSettings.Instance.HealthPercent &&
                !utils.isAuraActive(FOOD) && !Me.Combat && !Me.IsMoving && !utils.MeIsCastingWithLag())
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

        private bool Buff()
        {
            if (utils.Mounted() /*ExtraUtilsSettings.Instance.PauseRotation || */)
                return false;
            //Weapon
            if (EnhancementShamanSettings.Instance.imbueType == EnhancementShamanSettings.ImbueType.DEFAULT)
            {
                shammyCommon.ImbueMainHand(Imbue.Windfury);
                shammyCommon.ImbueOffHand(Imbue.Flametongue);
            }
            else if (EnhancementShamanSettings.Instance.imbueType == EnhancementShamanSettings.ImbueType.CHOOSEN)
            {
                switch (EnhancementShamanSettings.Instance.MainHand_imbue)
                {
                    case EnhancementShamanSettings.ImbueWeaponType.WINDFURY:
                        {
                            shammyCommon.ImbueMainHand(Imbue.Windfury);
                        }
                        break;
                    case EnhancementShamanSettings.ImbueWeaponType.FLAMETONGUE:
                        {
                            shammyCommon.ImbueMainHand(Imbue.Flametongue);
                        }
                        break;
                    case EnhancementShamanSettings.ImbueWeaponType.FROSTBRAND:
                        {
                            shammyCommon.ImbueMainHand(Imbue.Frostbrand);
                        }
                        break;
                    case EnhancementShamanSettings.ImbueWeaponType.ROCKBITER:
                        {
                            shammyCommon.ImbueMainHand(Imbue.Rockbiter);
                        }
                        break;
                    case EnhancementShamanSettings.ImbueWeaponType.EARTHLIVING:
                        {
                            shammyCommon.ImbueMainHand(Imbue.Earthliving);
                        }
                        break;
                }
                switch (EnhancementShamanSettings.Instance.OffHand_imbue)
                {
                    case EnhancementShamanSettings.ImbueWeaponType.WINDFURY:
                        {
                            shammyCommon.ImbueOffHand(Imbue.Windfury);
                        }
                        break;
                    case EnhancementShamanSettings.ImbueWeaponType.FLAMETONGUE:
                        {
                            shammyCommon.ImbueOffHand(Imbue.Flametongue);
                        }
                        break;
                    case EnhancementShamanSettings.ImbueWeaponType.FROSTBRAND:
                        {
                            shammyCommon.ImbueOffHand(Imbue.Frostbrand);
                        }
                        break;
                    case EnhancementShamanSettings.ImbueWeaponType.ROCKBITER:
                        {
                            shammyCommon.ImbueOffHand(Imbue.Rockbiter);
                        }
                        break;
                    case EnhancementShamanSettings.ImbueWeaponType.EARTHLIVING:
                        {
                            shammyCommon.ImbueOffHand(Imbue.Earthliving);
                        }
                        break;
                }

            }
             

            //shield
            if (!utils.isAuraActive(LIGHTNING_SHIELD,Me) && utils.CanCast(LIGHTNING_SHIELD))
            {
                utils.LogActivity(LIGHTNING_SHIELD);
                return utils.Cast(LIGHTNING_SHIELD);
            }

            return shammyCommon.NeedToRecallTotems();
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

        private bool IsWieldingTwoHandedWeapon()
        {
            try
            {
                switch (Me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass)
                {
                    case WoWItemWeaponClass.ExoticTwoHand:
                    case WoWItemWeaponClass.MaceTwoHand:
                    case WoWItemWeaponClass.AxeTwoHand:
                    case WoWItemWeaponClass.SwordTwoHand:
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                utils.LogActivity("IsWieldingBigWeapon ", ex.ToString());
            }

            return false;
        }

        private bool CombatRotation()
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


            WoWUnit target = utils.getTargetToAttack(30, tank);
            if (target != null && !target.IsDead)
            {
                if ((EnhancementShamanSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                {
                    Me.SetFacing(target);
                }

                if (EnhancementShamanSettings.Instance.AutoInterrupt && (target.IsCasting || target.IsChanneling) && target.CanInterruptCurrentSpellCast 
                    && utils.GetSpellCooldown(WIND_SHEAR).Milliseconds <= StyxWoW.WoWClient.Latency)
                {
                    if (utils.CanCast(WIND_SHEAR, target, true))
                    {
                        utils.LogActivity(WIND_SHEAR, target.Name);
                        return utils.Cast(WIND_SHEAR, target);
                    }
                }

                WoWUnit focused_unit = Me.FocusedUnit;
                if (EnhancementShamanSettings.Instance.UseHex && focused_unit != null && focused_unit.Attackable && focused_unit.InLineOfSpellSight &&
                    focused_unit.Distance <= 30 && !utils.isAuraActive(HEX, target) && utils.CanCast(HEX, focused_unit, true))
                {
                    if (EnhancementShamanSettings.Instance.UseInstantHex && utils.PlayerCountBuff(MAELSTROM_WEAPON) == 5)
                    {
                        utils.LogActivity(HEX, focused_unit.Name);
                        return utils.Cast(HEX, focused_unit);
                    }
                    else if (!EnhancementShamanSettings.Instance.UseInstantHex)
                    {
                        utils.LogActivity(HEX, focused_unit.Name);
                        return utils.Cast(HEX, focused_unit);
                    }
                }

                if (SoloBotType && (!target.InLineOfSight || !target.InLineOfSpellSight || !target.IsWithinMeleeRange))
                {
                    movement.KingHealMove(target.Location, 5f, true, true, target);
                }

                if (Healing())
                    return true;
                if(UseFireTotem())
                    return true;
                if (UseAirTotem())
                    return true;
                if (UseCD())
                    return true;
                if (EnhancementShamanSettings.Instance.AutoTarget || SoloBotType)    
                    target.Target();
                if(!Me.IsAutoAttacking)
                    Lua.DoString("/startattack");

                if (talents.IsSelected(16) && utils.CanCast(UNLEASH_ELEMENTS, target, true))
                {
                    utils.LogActivity(UNLEASH_ELEMENTS, target.Name);
                    return utils.Cast(UNLEASH_ELEMENTS, target);
                }
                if (EnhancementShamanSettings.Instance.EB_on_five && utils.PlayerCountBuff(MAELSTROM_WEAPON) == 5 && utils.CanCast(ELEMENTAL_BLAST, target, true)
                    && target.Distance - target.CombatReach - 1 <= 40)
                {
                    utils.LogActivity(ELEMENTAL_BLAST, target.Name);
                    return utils.Cast(ELEMENTAL_BLAST, target);
                }
                if (utils.PlayerCountBuff(MAELSTROM_WEAPON) == 5 &&
                    utils.AllAttaccableEnemyMobsInRangeFromTarget(target, 15).Count() >= EnhancementShamanSettings.Instance.ChainLightining_number
                    && utils.CanCast(CHAIN_LIGHTNING, target, true) && target.Distance - target.CombatReach - 1 <= 30)
                {
                    utils.LogActivity(CHAIN_LIGHTNING, target.Name);
                    return utils.Cast(CHAIN_LIGHTNING, target);
                }
                if (utils.AllAttaccableEnemyMobsInRangeFromTarget(target, 15).Count() >= EnhancementShamanSettings.Instance.FireNova_number
                    && utils.CanCast(FIRE_NOVA) && utils.AllEnemyMobsHasMyAuraInRange(FLAME_SHOCK,15).Count()>0)
                {
                    utils.LogActivity(FIRE_NOVA);
                    return utils.Cast(FIRE_NOVA);
                }
                if (utils.PlayerCountBuff(MAELSTROM_WEAPON) == 5 && utils.CanCast(LIGHTNING_BOLT, target, true) && 
                    target.Distance - target.CombatReach -1 <= 30)
                {
                    utils.LogActivity(LIGHTNING_BOLT, target.Name);
                    return utils.Cast(LIGHTNING_BOLT, target);
                }

                if (utils.isAuraActive(ASCENDANCE) && target.Distance - target.CombatReach -1 <= 30 && !StromsBlastOnCoolDown)
                {
                    utils.LogActivity(STORMBLAST, target.Name);
                    utils.RunMacroText("/Cast " + STORMBLAST);
                }
                if (target.Distance - target.CombatReach -1 <= 5 && !StromStrikeOnCoolDown)
                {
                    utils.LogActivity(STORMSTRIKE, target.Name);
                    return utils.Cast(STORMSTRIKE, target);
                }
                
                if (utils.CanCast(FLAME_SHOCK, target, true) && target.Distance - target.CombatReach - 1 <= 30 &&
                    ((utils.MyAuraTimeLeft(FLAME_SHOCK, target) < 3000) || utils.MyAuraTimeLeft("Unleash Flame", target)>0) )
                {
                    utils.LogActivity(FLAME_SHOCK, target.Name);
                    return utils.Cast(FLAME_SHOCK, target);
                }
                if (/*utils.isAuraActive(FLAME_SHOCK, target) &&*/ utils.GetSpellCooldown(LAVA_LASH).Milliseconds <= StyxWoW.WoWClient.Latency
                    && target.Distance - target.CombatReach - 1 <= 5)
                {
                    utils.LogActivity(LAVA_LASH, target.Name);
                    return utils.Cast(LAVA_LASH, target);
                }
                if (utils.CanCast(UNLEASH_ELEMENTS, target, true) && target.Distance - target.CombatReach - 1 <= 40)
                {
                    utils.LogActivity(UNLEASH_ELEMENTS, target.Name);
                    return utils.Cast(UNLEASH_ELEMENTS, target);
                }
                if (!EnhancementShamanSettings.Instance.EB_on_five && utils.CanCast(ELEMENTAL_BLAST, target, true)
                    && target.Distance - target.CombatReach - 1 <= 40 && utils.PlayerCountBuff(MAELSTROM_WEAPON) >= 3)
                {
                    utils.LogActivity(ELEMENTAL_BLAST, target.Name);
                    return utils.Cast(ELEMENTAL_BLAST, target);
                }
                
                //relocate totem
                if (!Me.IsMoving && talents.IsSelected(9) && utils.CanCast(TOTEMIC_PROJECTION) && shammyCommon.TotemsInRangeOf(Me) == 0)
                {
                    utils.LogActivity(TOTEMIC_PROJECTION);
                    utils.Cast(TOTEMIC_PROJECTION);
                    return SpellManager.ClickRemoteLocation(target.Location);

                }                
                
                if (utils.CanCast(EARTH_SHOCK, target, true) && utils.isAuraActive(FLAME_SHOCK, target)
                    && utils.MyAuraTimeLeft(FLAME_SHOCK, target) >= 7000 && target.Distance - target.CombatReach - 1 <= 30)
                {
                    utils.LogActivity(EARTH_SHOCK, target.Name);
                    return utils.Cast(EARTH_SHOCK, target);
                } 

                if(EnhancementShamanSettings.Instance.Use_LB_or_CL_as_filler && 
                    utils.PlayerCountBuff(MAELSTROM_WEAPON) >= EnhancementShamanSettings.Instance.maelstrom_fillet_count )
                {
                    //Filler
                    if (utils.AllAttaccableEnemyMobsInRangeFromTarget(target, 15).Count() >= EnhancementShamanSettings.Instance.ChainLightining_number
                        && utils.CanCast(CHAIN_LIGHTNING, target, true) && target.Distance - target.CombatReach - 1 <= 30)
                    {
                        utils.LogActivity(CHAIN_LIGHTNING, target.Name);
                        return utils.Cast(CHAIN_LIGHTNING, target);
                    }
                    if (utils.CanCast(LIGHTNING_BOLT, target, true) && target.Distance - target.CombatReach - 1 <= 30)
                    {
                        utils.LogActivity(LIGHTNING_BOLT, target.Name);
                        return utils.Cast(LIGHTNING_BOLT, target);
                    }
                }
            }
            return false;
        }

        private bool UseCD()
        {
            if (Me.Combat && Me.GotTarget)
            {
                if (utils.CanCast(STORMLASH_TOTEM)
                    && EnhancementShamanSettings.Instance.StormlLashTotemCD == EnhancementShamanSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(STORMLASH_TOTEM);
                    return utils.Cast(STORMLASH_TOTEM);
                }
                if (utils.CanCast(ELEMENTAL_MASTERY)
                    && EnhancementShamanSettings.Instance.ElementalMasteryCD == EnhancementShamanSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(ELEMENTAL_MASTERY);
                    return utils.Cast(ELEMENTAL_MASTERY);
                }
                if (utils.CanCast(FERAL_SPIRIT) 
                    && EnhancementShamanSettings.Instance.FeralSpiritCD == EnhancementShamanSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(FERAL_SPIRIT);
                    return utils.Cast(FERAL_SPIRIT);
                }
                if (!utils.isAuraActive(ASCENDANCE) && utils.CanCast(ASCENDANCE) && utils.GetSpellCooldown(ASCENDANCE).Milliseconds <= StyxWoW.WoWClient.Latency
                    && EnhancementShamanSettings.Instance.AscendanceCD == EnhancementShamanSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(ASCENDANCE);
                    return utils.Cast(ASCENDANCE);
                }
                if (utils.CanCast(FIRE_ELEMENTAL_TOTEM)
                    && EnhancementShamanSettings.Instance.FireElementalCD == EnhancementShamanSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(FIRE_ELEMENTAL_TOTEM);
                    return utils.Cast(FIRE_ELEMENTAL_TOTEM);
                }

                if (extra.IsTargetBoss())
                {
                    if (utils.CanCast(STORMLASH_TOTEM)
                    && EnhancementShamanSettings.Instance.StormlLashTotemCD == EnhancementShamanSettings.CDUseType.BOSS)
                    {
                        utils.LogActivity(STORMLASH_TOTEM);
                        return utils.Cast(STORMLASH_TOTEM);
                    }
                    if (utils.CanCast(ELEMENTAL_MASTERY)
                    && EnhancementShamanSettings.Instance.ElementalMasteryCD == EnhancementShamanSettings.CDUseType.BOSS)
                    {
                        utils.LogActivity(ELEMENTAL_MASTERY);
                        return utils.Cast(ELEMENTAL_MASTERY);
                    }
                    if (utils.CanCast(FERAL_SPIRIT)
                    && EnhancementShamanSettings.Instance.FeralSpiritCD == EnhancementShamanSettings.CDUseType.BOSS)
                    {
                        utils.LogActivity(FERAL_SPIRIT);
                        return utils.Cast(FERAL_SPIRIT);
                    }
                    if (!utils.isAuraActive(ASCENDANCE) && utils.CanCast(ASCENDANCE)
                        && utils.GetSpellCooldown(ASCENDANCE).Milliseconds <= StyxWoW.WoWClient.Latency
                        && EnhancementShamanSettings.Instance.AscendanceCD == EnhancementShamanSettings.CDUseType.BOSS)
                    {
                        utils.LogActivity(ASCENDANCE);
                        return utils.Cast(ASCENDANCE);
                    }
                    if (utils.CanCast(FIRE_ELEMENTAL_TOTEM)
                        && EnhancementShamanSettings.Instance.FireElementalCD == EnhancementShamanSettings.CDUseType.BOSS)
                    {
                        utils.LogActivity(FIRE_ELEMENTAL_TOTEM);
                        return utils.Cast(FIRE_ELEMENTAL_TOTEM);
                    }
                }
            }
            return false;             
        }

        private bool UseFireTotem()
        {

            if (!Me.IsMoving && Me.GotTarget && utils.AllAttaccableEnemyMobsInRangeFromTarget(Me, 25).Count() >= EnhancementShamanSettings.Instance.MagmaTotem_number
                && utils.CanCast(MAGMA_TOTEM)
                && !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental || t.WoWTotem == WoWTotem.Magma))
            {
                utils.LogActivity(MAGMA_TOTEM);
                return utils.Cast(MAGMA_TOTEM);
            }
            if (!Me.IsMoving && Me.GotTarget && utils.AllAttaccableEnemyMobsInRangeFromTarget(Me, 25).Count() < EnhancementShamanSettings.Instance.MagmaTotem_number
                && Me.CurrentTarget.Distance2DSqr < (shammyCommon.GetTotemRange(WoWTotem.Searing) * shammyCommon.GetTotemRange(WoWTotem.Searing))
                && utils.CanCast(SEARING_TOTEM)
                && !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental || t.WoWTotem == WoWTotem.Searing))
            {
                utils.LogActivity(SEARING_TOTEM);
                return utils.Cast(SEARING_TOTEM);
            }
            return false;
        }

        private bool UseAirTotem()
        {
            switch (EnhancementShamanSettings.Instance.AirTotemActive)
            {
                case EnhancementShamanSettings.AirTotemType.GROUNDIG:
                    {
                        if (utils.CanCast(GROUNDING_TOTEM))
                        {
                            utils.LogActivity(GROUNDING_TOTEM);
                            return utils.Cast(GROUNDING_TOTEM);

                        }
                    }
                    break;
                case EnhancementShamanSettings.AirTotemType.CAPACITOR:
                    {
                        if (utils.CanCast(CAPACITOR_TOTEM))
                        {
                            utils.LogActivity(CAPACITOR_TOTEM);
                            return utils.Cast(CAPACITOR_TOTEM);

                        }
                    }
                    break;

            }

            return false;
        }

        private bool Healing()
        {
            if (Me.HealthPercent <= EnhancementShamanSettings.Instance.ShamanistcRageHP && utils.CanCast(SHAMANISTIC_RAGE))
            {
                utils.LogActivity(SHAMANISTIC_RAGE);
                return utils.Cast(SHAMANISTIC_RAGE, Me);
            }
            if (Me.HealthPercent <= EnhancementShamanSettings.Instance.AS_HP && utils.CanCast(ASTRAL_SHIFT))
            {
                utils.LogActivity(ASTRAL_SHIFT);
                return utils.Cast(ASTRAL_SHIFT, Me);
            }
            if (EnhancementShamanSettings.Instance.AssistHealing)
            {
                if (!EnhancementShamanSettings.Instance.HealingSurgeOnlyOnMe && utils.PlayerCountBuff(MAELSTROM_WEAPON) == 5 &&
                    utils.CanCast(HEALING_SURGE))
                {
                    WoWUnit healTarget = utils.GetHealTarget(40f);
                    if (healTarget != null && healTarget.HealthPercent <= EnhancementShamanSettings.Instance.HealingSurgeHP)
                    {
                        utils.LogActivity(HEALING_SURGE,healTarget.Class.ToString());
                        return utils.Cast(HEALING_SURGE,healTarget);
                    }          
                }
                if (EnhancementShamanSettings.Instance.HealingSurgeOnlyOnMe && utils.PlayerCountBuff(MAELSTROM_WEAPON) >= 3 &&
                    utils.CanCast(HEALING_SURGE))
                {
                    if (Me.HealthPercent <= EnhancementShamanSettings.Instance.HealingSurgeHP)
                    {
                        utils.LogActivity(HEALING_SURGE, Me.Class.ToString());
                        return utils.Cast(HEALING_SURGE, Me);
                    }
                }

                if (Me.HealthPercent <= EnhancementShamanSettings.Instance.AncestralGuidanceMyHP)
                    {
                        utils.LogActivity(ANCESTRAL_GUIDANCE);
                        return utils.Cast(ANCESTRAL_GUIDANCE);
                    }
                

                /*if (utils.PlayerCountBuff(MAELSTROM_WEAPON) == 5)
                {
                    Logging.Write("AOEhealcount = " + utils.AOEHealCount(true, Me, EnhancementShamanSettings.Instance.HealingRainHP, 15));
                    Logging.Write("HealingRainNumber = " + EnhancementShamanSettings.Instance.HealingRainNumber);
                    Logging.Write("utils.CanCast(HEALING_RAIN) = " + utils.CanCast(HEALING_RAIN).ToString());
                }*/
                if (utils.AOEHealCount(Me, EnhancementShamanSettings.Instance.HealingRain_AncestralGuidanceHP, 40) >= EnhancementShamanSettings.Instance.HealingRain_AncestralGuidanceNumber)
                {
                    if (utils.CanCast(ANCESTRAL_GUIDANCE))
                    {
                        utils.LogActivity(ANCESTRAL_GUIDANCE);
                        return utils.Cast(ANCESTRAL_GUIDANCE);
                    }
                    if (utils.PlayerCountBuff(MAELSTROM_WEAPON) == 5 && utils.CanCast(HEALING_RAIN))
                    {
                        utils.LogActivity(HEALING_RAIN);
                        utils.Cast(HEALING_RAIN);
                        return SpellManager.ClickRemoteLocation(Me.Location);
                    }
                }
                if (EnhancementShamanSettings.Instance.UseHealingTotem)
                {
                    if (utils.CanCast(HEALIG_TIDE_TOTEM))
                    {
                        utils.LogActivity(HEALIG_TIDE_TOTEM);
                        return utils.Cast(HEALIG_TIDE_TOTEM);
                    }
                    if (utils.CanCast(HEALING_STREAM_TOTEM))
                    {
                        utils.LogActivity(HEALING_STREAM_TOTEM);
                        return utils.Cast(HEALING_STREAM_TOTEM);
                    }
                }
            }
            return false;
        }
    }
}

