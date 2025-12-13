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
    class FireMageCombatClass : KingWoWAbstractBaseClass
    {

        private static string Name = "KingWoW FireMage'";

        #region CONSTANT AND VARIABLES

        //START OF CONSTANTS ==============================
        private string[] StealBuffs = { "Innervate", "Hand of Freedom", "Hand of Protection", "Regrowth", "Rejuvenation", "Lifebloom", "Renew", 
                                      "Hand of Salvation", "Power Infusion", "Power Word: Shield", "Arcane Power", "Hot Streak!", /*"Avenging Wrath",*/ 
                                      "Elemental Mastery", "Nature's Swiftness", "Divine Plea", "Divine Favor", "Icy Veins", "Ice Barrier", "Holy Shield", 
                                      "Divine Aegis", "Bloodlust", "Time Warp", "Brain Freeze"};
        private const bool LOGGING = true;
        private const bool DEBUG = false;
        private const bool TRACE = false;
        private KingWoWUtility utils = null;
        private Movement movement = null;
        private ExtraUtils extra = null;

        private WoWUnit tank = null;
        private WoWUnit lastTank = null;
        private bool SoloBotType = false;
        private string BaseBot = "unknown";
        private DateTime nextTimeRuneOfPowerAllowed;
        private TalentManager talents = null;

        private const string DEBUG_LABEL = "DEBUG";
        private const string TRACE_LABEL = "TRACE";
        private const string TANK_CHANGE = "TANK CHANGED";
        private const string FACING = "FACING";

        //START OF SPELLS AND AURAS ==============================
        private const string DRINK = "Drink";
        private const string FOOD = "Food";

        private const string FROSTFIRE_BOLT = "Frostfire Bolt";
        private const string FROST_NOVA = "Frost Nova";
        private const string FIRE_BLAST = "Fire Blast";
        private const string BLINK = "Blink";
        private const string COUNTERSPELL = "Counterspell";
        private const string SUMMON_WATER_ELEMENTAL = "Summon Water Elemental";
        private const string FROSTBOLT = "Frostbolt";
        private const string POLYMORPH = "Polymorph";
        private const string ARCANE_EXPLOSION = "Arcane Explosion";
        private const string ICE_LANCE = "Ice Lance";
        private const string FINGER_OF_FROST = "Fingers of Frost";
        private const string ICE_BLOCK = "Ice Block";
        private const string CONE_OF_COLD = "Cone of Cold";
        private const string REMOVE_CURSE = "Remove Curse";
        private const string SLOW_FALL = "Slow Fall";
        private const string MOLTEN_ARMOR = "Molten Armor";
        private const string ICY_VEINS = "Icy Veins";
        private const string CONJURE_REFRESHMENT = "Conjure Refreshment";
        private const string EVOCATION = "Evocation";
        private const string FLAMESTRIKE = "Flamestrike";
        private const string CONJURE_MANA_GEM = "Conjure Mana Gem";
        private const string MIRROR_IMAGE = "Mirror Image";
        private const string BLIZZARD = "Blizzard";
        private const string FROST_ARMOR = "Frost Armor";
        private const string INVISIBILITY = "Invisibility";
        private const string ARCANE_BRILLANCE = "Arcane Brilliance";
        private const string FROZEN_ORB = "Frozen Orb";
        private const string SPELLSTEAL = "Spellsteal";
        private const string DEEP_FREEZE = "Deep Freeze";
        private const string CONJURE_REFRESHMENT_TABLE = "Conjure Refreshment Table";
        private const string BRAIN_FREEZE = "Brain Freeze";
        private const string MAGE_ARMOR = "Mage Armor";
        private const string TIME_WARP = "Time Warp";
        private const string ALTER_TIME = "Alter Time";

        private const string BRILLIANT_MANA_GEM = "Brilliant Mana Gem";
        private const string MANA_GEM = "Brilliant Mana Gem";

        private const string FREEZE = "Freeze";
        //END OF SPELLS AND AURAS ==============================

        //TALENTS
        private const string PRESENCE_OF_MIND = "Presence of Mind";
        private const string SCORCH = "Scorch";
        private const string ICE_FLOES = "Ice Floes";
        private const string TEMPORAL_SHIELD = "Temporal Shield";
        private const string BLAZING_SPEED = "Blazing Speed";
        private const string ICE_BARRIER = "Ice Barrier";
        private const string RING_OF_FROST = "Ring of Frost";
        private const string ICE_WARD = "Ice Ward";
        private const string FROSTJAW = "Frostjaw";
        private const string GREATER_INVISIBILITY = "Greater Invisibility";

        private const string COLD_SNAP = "Cold Snap";
        private const string NETHER_TEMPEST = "Nether Tempest";
        private const string LIVING_BOMB = "Living Bomb";
        private const string FROST_BOMB = "Frost Bomb";
        private const string INVOCATION_BUFF = "Invoker's Energy";
        private const string RUNE_OF_POWER = "Rune of Power";
        private const string INCANTER_WARD = "Incanter's Ward";

        private const string COMBUSTION = "Combustion";
        private const string IGNITE = "Ignite";
        private const string PYROBLAST = "Pyroblast";
        private const string PYROBLAST_PROC = "Pyroblast!";
        private const string HEATING_UP = "Heating Up";
        private const string INFERNO_BLAST = "Inferno Blast";
        private const string FIREBALL = "Fireball";
        private const string DRAGON_BREATH = "Dragon's Breath";
        
         

        //END TALENTS
        //END OF CONSTANTS ==============================

        #endregion

        public int GetTargetIgniteStrength()
        {
            string command = "return select(15,UnitDebuff(\"target\", \"Ignite\", nil, \"PLAYER\"))";
            int baseDmg = Lua.GetReturnVal<int>(command, 0);
            return baseDmg;       
        }

        private bool Decursing()
        {
            if (FireMageSettings.Instance.UseDecurse && utils.CanCast(REMOVE_CURSE))
            {
                WoWPlayer player = utils.GetDecurseTarget(40f);

                if (player != null && player.Distance - player.CombatReach -1  <= 40f && player.InLineOfSight)
                {
                    utils.LogActivity(REMOVE_CURSE, player.Class.ToString());
                    return utils.Cast(REMOVE_CURSE, player);
                }
            }
            return false;
        }

        public FireMageCombatClass()
        {
            utils = new KingWoWUtility();
            movement = new Movement();
            extra = new ExtraUtils();
            tank = null;
            lastTank = null; ;
            SoloBotType = false;
            BaseBot = "unknown";
            nextTimeRuneOfPowerAllowed = DateTime.Now;
            talents = new TalentManager();

        }

        public override bool Combat
        {
            get
            {
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld || Me.Silenced/*|| utils.IsGlobalCooldown(true)*/ || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) || Me.IsChanneling || utils.MeIsCastingWithLag())
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

        public override bool Pulse
        {
            get
            {
                extra.AnyKeyBindsPressed();
                if (/*ExtraUtilsSettings.Instance.PauseRotation || */ !StyxWoW.IsInGame || !StyxWoW.IsInWorld || Me.Silenced/*|| utils.IsGlobalCooldown(true)*/ || utils.isAuraActive(DRINK) || utils.isAuraActive(FOOD) || Me.IsChanneling || utils.MeIsCastingWithLag())
                    return false;

                if (!Me.Combat && FireMageSettings.Instance.UseEvocation)
                    Evocation();
                //UPDATE TANK
                //tank = utils.GetTank();
                tank = utils.SimpleGetTank(40f);
                if (tank == null || !tank.IsValid || !tank.IsAlive) tank = Me;

                if (tank != null && (lastTank == null || lastTank.Guid != tank.Guid))
                {
                    lastTank = tank;
                    utils.LogActivity(TANK_CHANGE, tank.Class.ToString());
                }
                if (tank != null && tank.Combat && !Me.GotTarget)
                    return CombatRotation();
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
                    if (!target.InLineOfSpellSight || !target.InLineOfSight || target.Distance > FireMageSettings.Instance.PullDistance)
                    {
                        movement.KingHealMove(target.Location, FireMageSettings.Instance.PullDistance, true, true, target);
                    }
                    if (utils.CanCast(FIREBALL, target)/*&& !Me.IsMoving*/)
                    {
                        if (!Me.IsMoving && !Me.IsFacing(target))
                        {
                            utils.LogActivity(FACING, target.Name);
                            Me.SetFacing(target);
                        }

                        utils.LogActivity(FIREBALL, target.Name);
                        return utils.Cast(FIREBALL, target);
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
            Logging.Write("Update Groups composition");
            utils.FillParties();
        }

        void BotEvents_OnBotStart(EventArgs args)
        {
            talents.Update();
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
                if (Me.ManaPercent <= FireMageSettings.Instance.ManaPercent &&
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
                if (Me.HealthPercent <= FireMageSettings.Instance.HealthPercent &&
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

        public void SetNextTimeRuneOfPower()
        {
            //3 seconds wait to avoid popping 2 rune of frost cause has high priority
            nextTimeRuneOfPowerAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 3000);
        }

        private bool PriorityBuff()
        {
            if (FireMageSettings.Instance.UseIncenterWardOnCD && Me.Combat && utils.CanCast(INCANTER_WARD))
            {
                utils.LogActivity(INCANTER_WARD);
                return utils.Cast(INCANTER_WARD);
            }

            if (FireMageSettings.Instance.UseRuneOfPower && !Me.IsMoving && nextTimeRuneOfPowerAllowed < DateTime.Now && !utils.isAuraActive(RUNE_OF_POWER, Me) && Me.Combat && utils.CanCast(RUNE_OF_POWER))
            {
                utils.LogActivity(RUNE_OF_POWER);
                utils.Cast(RUNE_OF_POWER);
                SetNextTimeRuneOfPower();
                return SpellManager.ClickRemoteLocation(Me.Location);
            }

            if (FireMageSettings.Instance.EvocationBuffAuto && !Me.IsMoving && talents.IsSelected(16) && !utils.isAuraActive(INVOCATION_BUFF) && utils.CanCast(EVOCATION))
            {
                utils.LogActivity(EVOCATION);
                return utils.Cast(EVOCATION);
            }

            if (FireMageSettings.Instance.UseIcebarrier && Me.Combat && utils.CanCast(ICE_BARRIER) && !utils.isAuraActive(ICE_BARRIER))
            {
                utils.LogActivity(ICE_BARRIER);
                return utils.Cast(ICE_BARRIER);
            }
            if (Me.Combat && utils.CanCast(TEMPORAL_SHIELD))
            {
                utils.LogActivity(TEMPORAL_SHIELD);
                return utils.Cast(TEMPORAL_SHIELD);
            }

            if (FireMageSettings.Instance.IceWardOnTank && Me.Combat && (tank != null && tank.Combat) && utils.CanCast(ICE_WARD) &&
                !utils.isAuraActive(ICE_WARD, tank) && tank != null && tank.IsAlive && tank.Distance < 40 &&
                tank.InLineOfSight)
            {
                utils.LogActivity(ICE_WARD, tank.Class.ToString());
                return utils.Cast(ICE_WARD, tank);
            }
            return false;
        }

        private bool Buff()
        {
            if (utils.Mounted() || utils.MeIsCastingWithLag() /*ExtraUtilsSettings.Instance.PauseRotation || */)
                return false;
            //Mana Gem
            if (!Me.Combat && !utils.HaveManaGem() && Me.Level >= 30 && utils.CanCast(CONJURE_MANA_GEM) && !Me.IsMoving)
            {
                utils.LogActivity(CONJURE_MANA_GEM);
                return utils.Cast(CONJURE_MANA_GEM);
            }

            if (!Me.Combat && !utils.GotMagefood && utils.CanCast(CONJURE_REFRESHMENT))
            {
                utils.LogActivity(CONJURE_REFRESHMENT);
                return utils.Cast(CONJURE_REFRESHMENT);
            }

            //Armor
            switch (FireMageSettings.Instance.ArmorToUse)
            {
                case FireMageSettings.ArmorType.FROST:
                    if (!utils.isAuraActive(FROST_ARMOR) && utils.CanCast(FROST_ARMOR) && !Me.IsMoving)
                    {
                        utils.LogActivity(FROST_ARMOR);
                        return utils.Cast(FROST_ARMOR);
                    }
                    break;
                case FireMageSettings.ArmorType.MOLTEN:
                    if (!utils.isAuraActive(MOLTEN_ARMOR) && utils.CanCast(MOLTEN_ARMOR) && !Me.IsMoving)
                    {
                        utils.LogActivity(MOLTEN_ARMOR);
                        return utils.Cast(MOLTEN_ARMOR);
                    }
                    break;
                case FireMageSettings.ArmorType.MAGE:
                    if (!utils.isAuraActive(MAGE_ARMOR) && utils.CanCast(MAGE_ARMOR) && !Me.IsMoving)
                    {
                        utils.LogActivity(MAGE_ARMOR);
                        return utils.Cast(MAGE_ARMOR);
                    }
                    break;
            }

            //arcane brillance
            if (FireMageSettings.Instance.AutoBuffBrillance && !utils.isAuraActive(ARCANE_BRILLANCE) && utils.CanCast(ARCANE_BRILLANCE))
            {
                utils.LogActivity(ARCANE_BRILLANCE);
                return utils.Cast(ARCANE_BRILLANCE);
            }

            
            return false;
        }

        private bool Interrupt()
        {
            if (FireMageSettings.Instance.AutoInterrupt)
            {
                WoWUnit target = null;
                WoWUnit InterruptTargetCandidate = Me.FocusedUnit;
                if (InterruptTargetCandidate == null || InterruptTargetCandidate.IsFriendly || InterruptTargetCandidate.IsDead
                    || !InterruptTargetCandidate.Attackable)
                {
                    if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.MANUAL)
                        target = Me.CurrentTarget;
                    else if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.AUTO)
                    {
                        target = utils.getTargetToAttack(40, tank);
                    }
                    else if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.SEMIAUTO)
                    {
                        target = Me.CurrentTarget;
                        if (target == null || target.IsDead || !target.InLineOfSpellSight || target.Distance - target.CombatReach - 1 > 40)
                            target = utils.getTargetToAttack(40, tank);
                    }
                    InterruptTargetCandidate = target;
                }
                if (InterruptTargetCandidate != null && (InterruptTargetCandidate.IsCasting || InterruptTargetCandidate.IsChanneling)
                    && InterruptTargetCandidate.CanInterruptCurrentSpellCast && utils.CanCast(COUNTERSPELL, InterruptTargetCandidate))
                {
                    utils.LogActivity(COUNTERSPELL, InterruptTargetCandidate.Name);
                    return utils.Cast(COUNTERSPELL, InterruptTargetCandidate);
                }
            }
            return false;
        }

        private bool Evocation()
        {
            if (Me.HealthPercent <= FireMageSettings.Instance.EvocationHP && utils.CanCast(EVOCATION) && !talents.IsSelected(16) && !talents.IsSelected(17))
            {
                utils.LogActivity(EVOCATION);
                return utils.Cast(EVOCATION);
            }
            return false;
        }

        private bool ProcWork()
        {
            //cast  Frost Bomb on cooldown.
            if (utils.CanCast(FROST_BOMB))
            {
                WoWUnit target = null;
                if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.MANUAL)
                    target = Me.CurrentTarget;
                else if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.AUTO)
                {
                    target = utils.getTargetToAttack(40, tank);
                }
                else if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.SEMIAUTO)
                {
                    target = Me.CurrentTarget;
                    if (target == null || target.IsDead || !target.InLineOfSpellSight || target.Distance - target.CombatReach - 1 > 40)
                        target = utils.getTargetToAttack(40, tank);
                }
                if (target != null && !target.IsDead && target.InLineOfSpellSight && target.Distance - target.CombatReach - 1 <= 40)
                {
                    utils.LogActivity(FROST_BOMB, target.Name);
                    return utils.Cast(FROST_BOMB, target);
                }
            }

            if (utils.isAuraActive(HEATING_UP) || utils.isAuraActive(PYROBLAST_PROC) || utils.isAuraActive(PRESENCE_OF_MIND) || (FireMageSettings.Instance.UseDeepFreeze && utils.CanCast(DEEP_FREEZE)))
            {
                WoWUnit target = null;
                if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.MANUAL)
                    target = Me.CurrentTarget;
                else if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.AUTO)
                {
                    target = utils.getTargetToAttack(40, tank);
                }
                else if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.SEMIAUTO)
                {
                    target = Me.CurrentTarget;
                    if (target == null || target.IsDead || !target.InLineOfSpellSight || target.Distance - target.CombatReach -1  > 40)
                        target = utils.getTargetToAttack(40, tank);
                }
                if (target != null && !target.IsFriendly && target.Attackable && !target.IsDead && target.InLineOfSpellSight && target.Distance - target.CombatReach -1  <= 40)
                {
                    if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.AUTO)
                        target.Target();
                    if ((FireMageSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                    {
                        Me.SetFacing(target);
                    }
                    //deep freeze
                    if (FireMageSettings.Instance.UseDeepFreeze && utils.CanCast(DEEP_FREEZE, target) && !target.HasAura(DEEP_FREEZE))
                    {
                        utils.LogActivity(DEEP_FREEZE, target.Name);
                        return utils.Cast(DEEP_FREEZE, target);
                    }
                    if (((utils.isAuraActive(PYROBLAST_PROC) && utils.isAuraActive(HEATING_UP) && FireMageSettings.Instance.PyroOnlyWithHU)
                          || (utils.isAuraActive(PYROBLAST_PROC) && !FireMageSettings.Instance.PyroOnlyWithHU) 
                          || (Me.HasAura(PYROBLAST_PROC) && Me.GetAuraByName(PYROBLAST_PROC).TimeLeft.TotalMilliseconds <= 2000)) 
                          && utils.CanCast(PYROBLAST, target))
                    {
                        utils.LogActivity(PYROBLAST, target.Name);
                        return utils.Cast(PYROBLAST, target);
                    }
                    if ((utils.isAuraActive(HEATING_UP) && !utils.isAuraActive(PYROBLAST_PROC))  && utils.CanCast(INFERNO_BLAST, target))
                    {
                        utils.LogActivity(INFERNO_BLAST, target.Name);
                        return utils.Cast(INFERNO_BLAST, target);
                    }
                    if (utils.isAuraActive(PRESENCE_OF_MIND) && !utils.isAuraActive(PYROBLAST_PROC) && utils.CanCast(PYROBLAST, target))
                    {
                        utils.LogActivity("PoM detected! instant PYROBLAST", target.Name);
                        return utils.Cast(PYROBLAST, target);
                    }
                    
                }
            }
            return false;
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

        private bool Defensivececk()
        {
            if (Me.Combat && FireMageSettings.Instance.IceBlockUse && Me.HealthPercent < FireMageSettings.Instance.IceBlockHP && !StyxWoW.Me.ActiveAuras.ContainsKey("Hypothermia") && utils.CanCast(ICE_BLOCK))
            {
                utils.LogActivity(ICE_BLOCK, Me.Class.ToString());
                return utils.Cast(ICE_BLOCK);
            }

            if (FireMageSettings.Instance.UseFrostNova && Me.Combat && (utils.AllAttaccableEnemyMobsInRange(12).Count() >= 1) && utils.CanCast(FROST_NOVA))
            {
                utils.LogActivity(FROST_NOVA);
                utils.Cast(FROST_NOVA);
            }

            if (FireMageSettings.Instance.UseBlink && Me.Combat && Me.IsMoving && (utils.AllAttaccableEnemyMobsInRange(12).Count() >= 1) && utils.CanCast(BLINK))
            {
                utils.LogActivity(BLINK);
                return utils.Cast(BLINK);
            }
            return false;
        }
        
        private bool RecMana()
        {
            if (Me.ManaPercent < FireMageSettings.Instance.UseManaGemPercent)
            {
                utils.UseBagItem(MANA_GEM);
                utils.UseBagItem(BRILLIANT_MANA_GEM);
            }
            return false;
        }

        private bool SpellSteal()
        {
            if (FireMageSettings.Instance.UseSpellSteal)
            {
                WoWUnit CandidateUnit = null;
                if (Me.FocusedUnit != null && Me.FocusedUnit.Attackable && Me.FocusedUnit.Distance - Me.FocusedUnit.CombatReach -1  <= 30 && Me.FocusedUnit.InLineOfSpellSight)
                    CandidateUnit = Me.FocusedUnit;
                else if (Me.CurrentTarget != null && Me.CurrentTarget.Attackable && Me.CurrentTarget.Distance - Me.CurrentTarget.CombatReach -1  <= 30 && Me.CurrentTarget.InLineOfSpellSight)
                    CandidateUnit = Me.CurrentTarget;
                if (CandidateUnit != null && utils.CanCast(SPELLSTEAL, CandidateUnit, true))
                    return SpellStealOnUnit(CandidateUnit);
            }

            return false;
        }

        private bool SpellStealOnUnit(WoWUnit u)
        {
            using (StyxWoW.Memory.AcquireFrame())
            {
                foreach (string spell in StealBuffs)
                {
                    if (u.HasAura(spell) && !StyxWoW.Me.HasAura(spell) && StyxWoW.Me.ManaPercent > 40)
                    {
                        utils.LogActivity(SPELLSTEAL + spell, u.Name);
                        return utils.Cast(SPELLSTEAL, u);
                    }
                }
            }
            return false;
        }

        private bool IsViableForPolymorph(WoWUnit unit)
        {
            if (utils.IsCrowdControlledPlayer(unit))
                return false;

            if (unit.CreatureType != WoWCreatureType.Beast && unit.CreatureType != WoWCreatureType.Humanoid)
                return false;

            if (StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget == unit)
                return false;

            /*if (StyxWoW.Me.GroupInfo.IsInParty && StyxWoW.Me.PartyMembers.Any(p => p.CurrentTarget != null && p.CurrentTarget == unit))
                 return false;*/

            return true;
        }

        private bool Polymorf()
        {
            if (FireMageSettings.Instance.UsePolymorf)
            {
                if (Me.FocusedUnit != null && Me.FocusedUnit.Attackable && Me.FocusedUnit.Distance - Me.FocusedUnit.CombatReach -1  <= 30
                    && Me.FocusedUnit.InLineOfSpellSight && IsViableForPolymorph(Me.FocusedUnit) && utils.CanCast(POLYMORPH, Me.FocusedUnit, true))
                {
                    utils.LogActivity(POLYMORPH, Me.FocusedUnit.Name);
                    return utils.Cast(POLYMORPH, Me.FocusedUnit);
                }

            }
            return false;

        }

        private bool CombatRotation()
        {
            if (Me.Combat && FireMageSettings.Instance.UseEvocationInCombat && FireMageSettings.Instance.UseEvocation)
                Evocation();
            extra.UseHealthstone();
            extra.UseRacials();
            extra.UseTrinket1();
            extra.UseTrinket2();
            extra.UseEngineeringGloves();
            extra.UseLifeblood();
            extra.UseAlchemyFlask();
            extra.WaterSpirit();
            extra.LifeSpirit();
            Defensivececk();
            Interrupt();
            PriorityBuff();
            ProcWork();
            UseCD();
            SpellSteal();
            Polymorf();
            //Multidot();
            RecMana();
            Decursing();

            WoWUnit target = null;
            if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.MANUAL)
                target = Me.CurrentTarget;
            else if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.AUTO)
            {
                target = utils.getTargetToAttack(40, tank);
            }
            else if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.SEMIAUTO)
            {
                target = Me.CurrentTarget;
                if (target == null || target.IsDead || !target.InLineOfSpellSight || target.Distance - target.CombatReach -1  > 40)
                    target = utils.getTargetToAttack(40, tank);
            }
            if (target != null && !target.IsFriendly && target.Attackable && !target.IsDead && target.InLineOfSpellSight && target.Distance - target.CombatReach -1  <= 40)
            {
                if (FireMageSettings.Instance.TargetTypeSelected == FireMageSettings.TargetType.AUTO)
                    target.Target();
                if ((FireMageSettings.Instance.AutofaceTarget || SoloBotType) && !Me.IsMoving)
                {
                    Me.SetFacing(target);
                }
                //COMBUSTION
                /*if (utils.CanCast(COMBUSTION) && utils.MyAuraTimeLeft(IGNITE, target) > 0 
                    && utils.MyAuraTimeLeft(PYROBLAST, target) > 0 )
                {
                    utils.LogActivity(COMBUSTION, target.Name);
                    return utils.Cast(COMBUSTION, target);
                }*/

                //COMBUSTION
                if (FireMageSettings.Instance.CDUseCombustion == FireMageSettings.CDCombustionUseType.COOLDOWN && utils.CanCast(COMBUSTION)
                    && utils.MyAuraTimeLeft(IGNITE, target) > 0 && utils.MyAuraTimeLeft(PYROBLAST, target) > 0)
                {
                    utils.LogActivity("IGNITE POWER:" + GetTargetIgniteStrength());
                    utils.LogActivity(COMBUSTION, target.Name);
                    return utils.Cast(COMBUSTION, target);
                }

                if (FireMageSettings.Instance.CDUseCombustion == FireMageSettings.CDCombustionUseType.CONDITION && utils.CanCast(COMBUSTION)
                    && utils.MyAuraTimeLeft(IGNITE, target) > 0 && GetTargetIgniteStrength() >= FireMageSettings.Instance.MinIgniteForCombustion)
                {
                    utils.LogActivity("IGNITE POWER:" + GetTargetIgniteStrength());
                    utils.LogActivity(COMBUSTION, target.Name);
                    return utils.Cast(COMBUSTION, target);
                }

                if (utils.CanCast(INFERNO_BLAST, target) &&
                    utils.AllAttaccableEnemyMobsInRangeFromTarget(target, 10).Count() >= FireMageSettings.Instance.AOECount 
                    && utils.MyAuraTimeLeft(IGNITE, target) > 0 /*&& utils.MyAuraTimeLeft(PYROBLAST, target) > 0*/
                    /*&& utils.MyAuraTimeLeft(LIVING_BOMB, target) > 0*/)
                {
                    utils.LogActivity(INFERNO_BLAST, target.Name);
                    return utils.Cast(INFERNO_BLAST, target);
                }

                //apply  Nether Tempest and always refresh it right before the last tick;
                if (utils.CanCast(NETHER_TEMPEST, target) && (utils.MyAuraTimeLeft(NETHER_TEMPEST, target) < 1500))
                {
                    utils.LogActivity(NETHER_TEMPEST, target.Name);
                    return utils.Cast(NETHER_TEMPEST, target);
                }

                //apply  Living Bomb and refresh it right before or right after the last tick (the expiring Living Bomb will explode in both cases);
                if (utils.CanCast(LIVING_BOMB, target) && (utils.MyAuraTimeLeft(LIVING_BOMB, target) < 1500))
                {
                    utils.LogActivity(LIVING_BOMB, target.Name);
                    return utils.Cast(LIVING_BOMB, target);
                }

                //+++++++++++++++++++++++++AOE rotation start+++++++++++++++++++++++++++++++//
                if (FireMageSettings.Instance.UseFlameStrike && utils.CanCast(FLAMESTRIKE) && target.Distance <= 40 && utils.AllAttaccableEnemyMobsInRangeFromTarget(target, 10).Count() >= FireMageSettings.Instance.AOECount)
                {
                    utils.LogActivity(FLAMESTRIKE, target.Name);
                    utils.Cast(FLAMESTRIKE);
                    return SpellManager.ClickRemoteLocation(target.Location);
                }

                if (FireMageSettings.Instance.UseDragonBreath && utils.CanCast(DRAGON_BREATH) && utils.AllAttaccableEnemyMobsInRange(15).Count() >= FireMageSettings.Instance.AOECount)
                {
                    utils.LogActivity(DRAGON_BREATH);
                    return utils.Cast(DRAGON_BREATH);
                }

                if (FireMageSettings.Instance.UseArcaneExplosion && utils.CanCast(ARCANE_EXPLOSION) && utils.AllAttaccableEnemyMobsInRange(10).Count() >= FireMageSettings.Instance.AOECount)
                {
                    utils.LogActivity(ARCANE_EXPLOSION);
                    return utils.Cast(ARCANE_EXPLOSION);
                }

                if (FireMageSettings.Instance.UseRingOfFrost && utils.CanCast(RING_OF_FROST) && target.Distance2DSqr <= 30 * 30)
                {
                    utils.LogActivity(RING_OF_FROST, target.Name);
                    utils.Cast(RING_OF_FROST);
                    return SpellManager.ClickRemoteLocation(target.Location);
                }

                Multidot(); 

                //Cast  Fireball as a filler spell.
                if (!Me.IsMoving && utils.CanCast(FIREBALL, target) && !utils.isAuraActive(PRESENCE_OF_MIND) && !utils.CanCast(FROST_BOMB) 
                    && !(utils.isAuraActive(PYROBLAST_PROC) && utils.isAuraActive(HEATING_UP)) && !(utils.isAuraActive(HEATING_UP) && utils.CanCast(INFERNO_BLAST)))
                {
                    utils.LogActivity(FIREBALL, target.Name);
                    return utils.Cast(FIREBALL, target);
                }

                //+++++++++++++++++++++++DPS moving   START+++++++++++++++++++++++++++
                //ICE_FLOE
                if (Me.IsMoving && utils.CanCast(ICE_FLOES) && !utils.isAuraActive(ICE_FLOES))
                {
                    utils.LogActivity(ICE_FLOES);
                    utils.Cast(ICE_FLOES);
                }
                //Scorch
                if (Me.IsMoving && SpellManager.HasSpell(SCORCH) && !utils.isAuraActive(PRESENCE_OF_MIND)
                    && !utils.isAuraActive(PYROBLAST_PROC) && !(utils.isAuraActive(HEATING_UP) && utils.CanCast(INFERNO_BLAST)))
                {
                    utils.LogActivity(SCORCH, target.Name);
                    return utils.Cast(SCORCH, target);
                }
            }
            else if (SoloBotType && Me.CurrentTarget != null && !Me.CurrentTarget.IsDead && (!Me.CurrentTarget.InLineOfSpellSight || !Me.CurrentTarget.InLineOfSight || Me.CurrentTarget.Distance - Me.CurrentTarget.CombatReach - 1 > FireMageSettings.Instance.PullDistance))
            {
                movement.KingHealMove(Me.CurrentTarget.Location, FireMageSettings.Instance.PullDistance, true, true, target);
            }
            return false;
        }

        //Ice veyn
        //Mirror image
        //Alter Time
        private bool UseCD()
        {
            if (Me.Combat && Me.GotTarget)
            {
                if (utils.CanCast(MIRROR_IMAGE) && FireMageSettings.Instance.CDUseMirrorImage == FireMageSettings.CDUseType.COOLDOWN)
                {
                    utils.LogActivity(MIRROR_IMAGE);
                    return utils.Cast(MIRROR_IMAGE);
                }
                if (utils.CanCast(ALTER_TIME) && FireMageSettings.Instance.CDUseAlterTime == FireMageSettings.CDUseType.COOLDOWN
                    && utils.isAuraActive(PYROBLAST_PROC) && utils.isAuraActive(HEATING_UP))
                {
                    if (utils.CanCast(PRESENCE_OF_MIND))
                    {
                        utils.LogActivity(PRESENCE_OF_MIND);
                        utils.Cast(PRESENCE_OF_MIND);
                    }
                    utils.LogActivity(ALTER_TIME);
                    return utils.Cast(ALTER_TIME);
                }

                if (extra.IsTargetBoss())
                {
                    if (utils.CanCast(MIRROR_IMAGE) && FireMageSettings.Instance.CDUseMirrorImage == FireMageSettings.CDUseType.BOSS)
                    {
                        utils.LogActivity(MIRROR_IMAGE);
                        return utils.Cast(MIRROR_IMAGE);
                    }
                    if (utils.CanCast(ALTER_TIME) && FireMageSettings.Instance.CDUseAlterTime == FireMageSettings.CDUseType.BOSS
                        && utils.isAuraActive(PYROBLAST_PROC) && utils.isAuraActive(HEATING_UP))
                    {
                        if (utils.CanCast(PRESENCE_OF_MIND))
                        {
                            utils.LogActivity(PRESENCE_OF_MIND);
                            utils.Cast(PRESENCE_OF_MIND);
                        }
                        utils.LogActivity(ALTER_TIME);
                        return utils.Cast(ALTER_TIME);
                    }

                }
            }
            return false;
        }

        private bool Multidot()
        {
            if (FireMageSettings.Instance.MultidotEnabled)
            {
                int enemyNumber = utils.AllAttaccableEnemyMobsInRangeTragettingMyParty(40f, FireMageSettings.Instance.MultidotAvoidCC).Count();
                if (enemyNumber >= FireMageSettings.Instance.MultidotEnemyNumberMin)
                {
                    WoWUnit TargetForMultidot = null;
                    //apply  Nether Tempest and always refresh it right before the last tick;
                    if (utils.CanCast(NETHER_TEMPEST) && utils.AllEnemyMobsHasMyAura(NETHER_TEMPEST).Count() < FireMageSettings.Instance.MultidotEnemyNumberMax)
                    {
                        TargetForMultidot = utils.NextApplyAuraTarget(NETHER_TEMPEST, 40, 1000, FireMageSettings.Instance.MultidotAvoidCC);
                        if (TargetForMultidot != null)
                        {
                            utils.LogActivity("   MULTIDOT   " + NETHER_TEMPEST, TargetForMultidot.Name);
                            return utils.Cast(NETHER_TEMPEST, TargetForMultidot);
                        }
                    }

                    //apply  Living Bomb and refresh it right before or right after the last tick (the expiring Living Bomb will explode in both cases);
                    if (utils.CanCast(LIVING_BOMB) && utils.AllEnemyMobsHasMyAura(LIVING_BOMB).Count() < 3)
                    {
                        TargetForMultidot = utils.NextApplyAuraTarget(LIVING_BOMB, 40, 1000, FireMageSettings.Instance.MultidotAvoidCC);
                        if (TargetForMultidot != null)
                        {
                            utils.LogActivity("   MULTIDOT   " + LIVING_BOMB, TargetForMultidot.Name);
                            return utils.Cast(LIVING_BOMB, TargetForMultidot);
                        }
                    }

                }
            }
            return false;
        }
    }
}
