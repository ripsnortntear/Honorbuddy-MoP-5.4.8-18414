using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Media;
using System.Windows.Forms;
using Styx;
using Styx.Helpers;
using Styx.Common;
using Styx.CommonBot.POI;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Runtime.InteropServices;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;


namespace Boomkin
{
    public partial class Classname : CombatRoutine
    {
        #region spell msgs
        public string HavePotionBuff = "";
        public string HaveFlaskBuff = "";
        public string HaveHealPotions = "";
        public string HaveLifeSpirit = "";
        public string HaveHealthStone = "";
        public string LastSpell = "";
        #endregion spellmsgs

        #region IsInRange
        public bool IsInRange(float range, WoWUnit target)
        {
            if (Me.CurrentTarget.Distance <= range)
            {
                return true;
            }
            return false;
        }
        #endregion IsInRange

        #region spellcasting
        public void CastSpell(string spell)
        {
            if (SpellManager.HasSpell(spell)
                && SpellManager.CanCast(spell))
            {
                SpellManager.Cast(spell);
                LogMsg(spell, 3);
            }
        }
        public void CastBuff(string spell)
        {
            if (SpellManager.HasSpell(spell)
                && SpellManager.CanCast(spell))
            {
                SpellManager.Cast(spell, Me);
                LogMsg(spell, 3);
            }
        }
        #endregion spellcasting

        #region faerie fire
        public string FF
        {
            get
            {
                if (SpellManager.HasSpell("Faerie Swarm"))
                {
                    return "Faerie Swarm";
                }
                return "Faerie Fire";
            }
        }
        #endregion faerie fire

        #region hastebuffs
        private bool HaveHasteBuff
        {
            get
            {
                if (buffExists("Tiger's Fury", Me)
                    && buffExists("Heroism", Me)
                    && buffExists("Bloodlust", Me)
                    && buffExists("Ancient Hysteria", Me))
                {
                    return true;
                }
                return false;
            }
        }
        #endregion hastebuffs

        #region statsbuffs
        public bool HaveStatsBuffs
        {
            get
            {
                if (buffExists("Mark of the Wild", Me)
                    || buffExists("Blessing of Kings", Me)
                    || buffExists("Legacy of the Emperor", Me)
                    || buffExists("Embrace of the Shale Spider", Me))
                {
                    return true;
                }
                return false;
            }
        }
        #endregion statsbuffs

        #region GetAsyncKeyState

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        #endregion

        #region pause

        public bool DisableAoe = false;
        public bool Paused = false;

        public override void ShutDown()
        {
            Lua.Events.DetachEvent("MODIFIER_STATE_CHANGED", HandleModifierStateChanged);
        }

        private void HandleModifierStateChanged(object sender, LuaEventArgs args)
        {
            if (CRSettings.myPrefs.PauseKeys == CRSettings.Keypress.None
                && CRSettings.myPrefs.AoePauseKeys == CRSettings.Keypress.None)
            {
                return;
            }

            if (Convert.ToInt32(args.Args[1]) == 1)
            {
                if (args.Args[0].ToString() == CRSettings.myPrefs.AoePauseKeys.ToString())
                {
                    DisableAoe = !DisableAoe;
                    if (DisableAoe)
                    {
                        Logging.Write("Disabling Aoe, press {0} in WOW again to enable Aoe again",
                                     CRSettings.myPrefs.AoePauseKeys.ToString());
                        if (CRSettings.myPrefs.PrintMsg)
                            Lua.DoString(
                                "RaidNotice_AddMessage(RaidWarningFrame, \"Disabling Aoe\", ChatTypeInfo[\"RAID_WARNING\"]);");
                    }
                    else
                    {
                        Logging.Write("Aoe enabled");
                        if (CRSettings.myPrefs.PrintMsg)
                            Lua.DoString(
                                "RaidNotice_AddMessage(RaidWarningFrame, \"Aoe Enabled\", ChatTypeInfo[\"RAID_WARNING\"]);");
                    }
                }
            }
            if (Convert.ToInt32(args.Args[1]) == 1)
            {
                if (args.Args[0].ToString() == CRSettings.myPrefs.PauseKeys.ToString())
                {
                    Paused = !Paused;
                    if (Paused)
                    {
                        Logging.Write("CR paused, press {0} in WOW again to turn back on",
                                     CRSettings.myPrefs.PauseKeys.ToString());
                        if (CRSettings.myPrefs.PrintMsg)
                            Lua.DoString(
                                "RaidNotice_AddMessage(RaidWarningFrame, \"CombatRoutine Paused\", ChatTypeInfo[\"RAID_WARNING\"]);");
                    }
                    else
                    {
                        Logging.Write("CombatRoutine running again ....");
                        if (CRSettings.myPrefs.PrintMsg)
                            Lua.DoString(
                                "RaidNotice_AddMessage(RaidWarningFrame, \"CombatRoutine running again ....\", ChatTypeInfo[\"RAID_WARNING\"]);");
                    }
                }
            }
        }
        #endregion pause

        #region logs
        public void LogMsg(string msg, int kleurtje)
        {
            Color kleur = Colors.Yellow;
            switch (kleurtje)
            {
                case 0:
                    kleur = Colors.Orange;
                    break;
                case 1:
                    kleur = Colors.PaleGreen;
                    break;
                case 2:
                    kleur = Colors.BlanchedAlmond;
                    break;
                case 3:
                    kleur = Colors.Yellow;
                    break;
                case 4:
                    kleur = Colors.Red;
                    break;
                case 5:
                    kleur = Colors.LightBlue;
                    break;
                case 6:
                    kleur = Colors.Crimson;
                    break;
                case 7:
                    kleur = Colors.Cyan;
                    break;
                case 8:
                    kleur = Colors.DarkSeaGreen; // TotH
                    break;
                case 9:
                    kleur = Colors.Honeydew; //Lock and load
                    break;
                case 10:
                    kleur = Colors.DeepSkyBlue; //focusdump
                    break;
            }
            Logging.Write(kleur, msg);
        }
        #endregion logs

        #region AddCounting
        private int addCount
        {
            get
            {
                int count = 0;
                foreach (WoWUnit u in ObjectManager.GetObjectsOfType<WoWUnit>(true, true))
                {
                    if (gotTarget
                        && u.CanSelect
                        && !u.IsPlayer
                        && u.IsAlive
                        && u.Guid != Me.Guid
                        && !u.IsFriendly
                        && u.IsHostile
                        && u.Attackable
                        && !u.IsTotem
                        && !u.IsCritter
                        && !u.IsNonCombatPet
                        && (u.Location.Distance(Me.CurrentTarget.Location) <= 10 || u.Location.Distance2D(Me.CurrentTarget.Location) <= 10))
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        IEnumerable<WoWUnit> UnfriendlyUnits
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(u => !u.IsDead && u.CanSelect && u.Attackable && !u.IsFriendly && u.IsWithinMeleeRange); }
        }
        #endregion AddCounting

        #region Kind of target
        public bool IsWoWBoss(WoWUnit mytarget)
        {

            if (gotTarget
                && Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss)
            {
                return true;
            }
            else if (gotTarget
                && target.IsBoss(mytarget))
            {
                return true;
            }
            else if (gotTarget
                && Me.CurrentTarget.MaxHealth >= (65 * 1000000))
            {
                return true;
            }
            else if (gotTarget
                && Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.RareElite
                && Me.CurrentTarget.MaxHealth >= (Me.MaxHealth * CRSettings.myPrefs.ElitesHealth)
                && !Me.GroupInfo.IsInParty
                && !Me.IsInInstance
                && !Me.GroupInfo.IsInRaid)
            {
                return true;
            }
            else if (gotTarget
                     && Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Rare
                     && Me.CurrentTarget.MaxHealth >= (Me.MaxHealth * CRSettings.myPrefs.ElitesHealth)
                     && !Me.GroupInfo.IsInParty
                     && !Me.IsInInstance
                     && !Me.GroupInfo.IsInRaid)
            {
                return true;
            }
            else if (gotTarget
                && Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.Elite
                && Me.CurrentTarget.MaxHealth >= (Me.MaxHealth * CRSettings.myPrefs.ElitesHealth)
                && !Me.GroupInfo.IsInParty
                && !Me.IsInInstance
                && !Me.GroupInfo.IsInRaid)
            {
                return true;
            }
            else if (gotTarget
                && Me.CurrentTarget.Name.Contains("Training Dummy")
                && !Me.IsInInstance)
            {
                return true;
            }
            else if (gotTarget
                && Me.CurrentTarget.IsPlayer
                && !Me.CurrentTarget.IsFriendly)
            {
                return true;
            }
            return false;
        }

        #endregion Kind of Target

        #region trinkets + flasks
       
        public void useAlchemistFlask()
        {
            if (CRSettings.myPrefs.alchemyflask)
            {

                if (!Me.HasAura("Enhanced Intellect")
                    && !buffExists("Flask of the Warm Sun", Me)
                    && !Me.Mounted)
                {
                    WoWItem alchflask = Me.BagItems.FirstOrDefault(h => h.Entry == 75525);

                    if (alchflask != null && LastSpell != "AlchFlask")
                    {
                        alchflask.Use();
                        Logging.Write(Colors.LightBlue, "Using Alchemist's Flask");
                        LastSpell = "AlchFlask";
                    }

                }
            }
        }
        public void useJadePotion()
        {
            if (gotTarget
                && (CRSettings.myPrefs.jadepotion == 1
                && Me.GroupInfo.IsInRaid)
                || (CRSettings.myPrefs.jadepotion == 2
                && (Me.GroupInfo.IsInRaid && IsWoWBoss(Me.CurrentTarget)))
                && IsInRange(39, Me.CurrentTarget))
            {
                WoWItem potion = Me.BagItems.FirstOrDefault(h => h.Entry == 76089);

                if (potion == null
                    && HavePotionBuff != "no")
                {
                    Logging.Write(Colors.BlanchedAlmond, "I have no " + potion.Name);
                    HavePotionBuff = "no";
                }
                if (potion != null && potion.CooldownTimeLeft.TotalMilliseconds <= 0 && LastSpell != "virmen")
                {
                    potion.Use();
                    Logging.Write(Colors.BlanchedAlmond, "Using " + potion.Name);
                    HavePotionBuff = "yes";
                    LastSpell = "virmen";
                }
            }
        }
        public void useHealthStone()
        {
            if (gotTarget
                && Me.HealthPercent <= CRSettings.myPrefs.healthstonepercent)
            {
                WoWItem hstone = Me.BagItems.FirstOrDefault(h => h.Entry == 5512);
                if (hstone == null
                    && HaveHealthStone != "no")
                {
                    Logging.Write(Colors.BlanchedAlmond, "I have no Healthstone");
                    HaveHealthStone = "no";
                }
                if (hstone != null && hstone.CooldownTimeLeft.TotalMilliseconds <= 0)
                {
                    hstone.Use();
                    Logging.Write(Colors.BlanchedAlmond, "Using Healthstone");
                    HaveHealthStone = "yes";
                    LastSpell = "healthstone";
                }
            }
        }
        public void useFlask()
        {
            bool useFlask = false;

            if ((CRSettings.myPrefs.intflask == 1
                && Me.GroupInfo.IsInRaid)
                || (CRSettings.myPrefs.intflask == 2
                && (Me.GroupInfo.IsInRaid && !Me.GroupInfo.IsInLfgParty))
                && Me.Combat)
            {
                useFlask = true;
            }

            if (Me.Combat
                && useFlask
                && !buffExists("Flask of the Warm Sun", Me))
            {
                WoWItem flask = Me.BagItems.FirstOrDefault(h => h.Entry == 76084);
                if (flask == null
                    && HaveFlaskBuff != "no")
                {
                    Logging.Write(Colors.BlanchedAlmond, "I have no Agility Flask");
                    HaveFlaskBuff = "no";
                }
                if (flask != null && flask.CooldownTimeLeft.TotalMilliseconds <= 0)
                {
                    flask.Use();
                    Logging.Write(Colors.BlanchedAlmond, "Using Agility Flask");
                    HaveFlaskBuff = "yes";
                }
            }
            return;
        }
        public void UseTrinket1()
        {
            if (gotTarget
                && Me.CurrentTarget.IsWithinMeleeRange)
            {

                var firstTrinket = StyxWoW.Me.Inventory.Equipped.Trinket1;
                if (CRSettings.myPrefs.trinket1 == 2
                    && IsWoWBoss(Me.CurrentTarget))
                {

                    if (firstTrinket != null && CanUseEquippedItem(firstTrinket) && LastSpell != "trinket1")
                    {
                        firstTrinket.Use();
                        LogMsg("Using 1st trinket", 2);
                        LastSpell = "trinket1";
                    }
                }
                if (CRSettings.myPrefs.trinket1 == 1)
                {
                    if (firstTrinket != null && CanUseEquippedItem(firstTrinket) && LastSpell != "trinket1")
                    {
                        firstTrinket.Use();
                        LogMsg("Using 1st trinket", 2);
                        LastSpell = "trinket1";
                    }
                }
            }
        }
        public void UseTrinket2()
        {

            if (gotTarget
                && Me.CurrentTarget.IsWithinMeleeRange)
            {
                var secondTrinket = StyxWoW.Me.Inventory.Equipped.Trinket2;


                if (CRSettings.myPrefs.trinket2 == 2
                    && IsWoWBoss(Me.CurrentTarget))
                {

                    if (secondTrinket != null && CanUseEquippedItem(secondTrinket) && LastSpell != "trinket2")
                    {
                        secondTrinket.Use();
                        LogMsg("Using 2nd trinket", 2);
                        LastSpell = "trinket2";
                    }
                }
                else if (CRSettings.myPrefs.trinket2 == 1)
                {
                    if (secondTrinket != null && CanUseEquippedItem(secondTrinket) && LastSpell != "trinket2")
                    {
                        secondTrinket.Use();
                        LogMsg("Using 2nd trinket", 2);
                        LastSpell = "trinket2";
                    }
                }
            }
        }
        public void UseEngiGloves()
        {

            if (gotTarget
                && Me.CurrentTarget.IsWithinMeleeRange)
            {
                var engiGloves = StyxWoW.Me.Inventory.Equipped.Hands;

                if (CRSettings.myPrefs.engigloves == 2
                    && IsWoWBoss(Me.CurrentTarget))
                {
                    if (engiGloves != null && CanUseEquippedItem(engiGloves) && LastSpell != "gloves")
                    {
                        engiGloves.Use();
                        LogMsg("Using Engineer Gloves", 2);
                        LastSpell = "gloves";
                    }
                }
                else if (CRSettings.myPrefs.engigloves == 1)
                {
                    if (engiGloves != null && CanUseEquippedItem(engiGloves) && LastSpell != "gloves")
                    {
                        engiGloves.Use();
                        LogMsg("Using Engineer Gloves", 2);
                        LastSpell = "gloves";
                    }
                }
            }
        }
        private static bool CanUseEquippedItem(WoWItem item)
        {
            // Check for engineering tinkers!
            string itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);
            if (string.IsNullOrEmpty(itemSpell))
                return false;


            return item.Usable && item.Cooldown <= 0;
        }
        #endregion trinkets + flasks

        #region Buff Checks

        public bool buffExists(int Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraById(Buff);
                if (Results != null)
                    return true;
            }
            return false;
        }

        public double buffTimeLeft(int Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraById(Buff);
                if (Results != null)
                {
                    if (Results.TimeLeft.TotalMilliseconds > 0)
                        return Results.TimeLeft.TotalMilliseconds;
                }
            }
            return 0;
        }

        public bool buffExists(string Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraByName(Buff);
                if (Results != null)
                    return true;
            }
            return false;
        }

        public double buffTimeLeft(string Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraByName(Buff);
                if (Results != null)
                {
                    if (Results.TimeLeft.TotalMilliseconds > 0)
                        return Results.TimeLeft.TotalMilliseconds;
                }
            }
            return 0;
        }



        public uint buffStackCount(int Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraById(Buff);
                if (Results != null)
                    return Results.StackCount;
            }
            return 0;
        }
        public uint buffStackCount(string Buff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.GetAuraByName(Buff);
                if (Results != null)
                    return Results.StackCount;
            }
            return 0;
        }
        #endregion

        #region Cache Checks
        public IEnumerable<WoWAura> cachedAuras = new List<WoWAura>();
        public IEnumerable<WoWAura> cachedTargetAuras = new List<WoWAura>();
        public void getCachedAuras()
        {
            if (Me.CurrentTarget != null)
                cachedTargetAuras = Me.CurrentTarget.GetAllAuras();
            cachedAuras = Me.GetAllAuras();
        }
        #endregion

        #region Cooldown Checks
        private Dictionary<WoWSpell, long> Cooldowns = new Dictionary<WoWSpell, long>();
        public TimeSpan cooldownLeft(int Spell)
        {
            SpellFindResults Results;
            if (SpellManager.FindSpell(Spell, out Results))
            {
                WoWSpell Result = Results.Override ?? Results.Original;
                if (Cooldowns.TryGetValue(Result, out lastUsed))
                {
                    if (DateTime.Now.Ticks < lastUsed)
                        return Result.CooldownTimeLeft;
                    return TimeSpan.MaxValue;
                }
            }
            return TimeSpan.MaxValue;
        }
        public TimeSpan cooldownLeft(string Spell)
        {
            SpellFindResults Results;
            if (SpellManager.FindSpell(Spell, out Results))
            {
                WoWSpell Result = Results.Override ?? Results.Original;
                if (Cooldowns.TryGetValue(Result, out lastUsed))
                {
                    if (DateTime.Now.Ticks < lastUsed)
                        return Result.CooldownTimeLeft;
                    return TimeSpan.MaxValue;
                }
            }
            return TimeSpan.MaxValue;
        }
        long lastUsed;
        public int lastCast;
        public bool onCooldown(int Spell)
        {
            SpellFindResults Results;
            if (SpellManager.FindSpell(Spell, out Results))
            {
                WoWSpell Result = Results.Override ?? Results.Original;
                if (Cooldowns.TryGetValue(Result, out lastUsed))
                {
                    if (DateTime.Now.Ticks < lastUsed)
                        return Result.Cooldown;
                    return false;
                }
            }
            return false;
        }
        public bool onCooldown(string Spell)
        {
            SpellFindResults Results;
            if (SpellManager.FindSpell(Spell, out Results))
            {
                WoWSpell Result = Results.Override ?? Results.Original;
                if (Cooldowns.TryGetValue(Result, out lastUsed))
                {
                    if (DateTime.Now.Ticks < lastUsed)
                        return Result.Cooldown;
                    return false;
                }
            }
            return false;
        }
        public TimeSpan spellCooldownLeft(int Spell, bool useTracker = false)
        {
            if (useTracker)
                return cooldownLeft(Spell);
            SpellFindResults results;
            if (SpellManager.FindSpell(Spell, out results))
            {
                if (results.Override != null)
                    return results.Override.CooldownTimeLeft;
                return results.Override.CooldownTimeLeft;
            }
            return TimeSpan.MaxValue;
        }
        public TimeSpan spellCooldownLeft(string Spell, bool useTracker = false)
        {
            if (useTracker)
                return cooldownLeft(Spell);
            SpellFindResults results;
            if (SpellManager.FindSpell(Spell, out results))
            {
                if (results.Override != null)
                    return results.Override.CooldownTimeLeft;
                return results.Override.CooldownTimeLeft;
            }
            return TimeSpan.MaxValue;
        }
        public bool spellOnCooldown(int Spell, bool useTracker = false)
        {
            if (useTracker)
                return !onCooldown(Spell);
            SpellFindResults results;
            if (SpellManager.FindSpell(Spell, out results))
                return results.Override != null ? results.Override.Cooldown : results.Original.Cooldown;
            return false;
        }
        public bool spellOnCooldown(string Spell, bool useTracker = false)
        {
            if (useTracker)
                return !onCooldown(Spell);
            SpellFindResults results;
            if (SpellManager.FindSpell(Spell, out results))
                return results.Override != null ? results.Override.Cooldown : results.Original.Cooldown;
            return false;
        }
        #endregion

        #region Debuff Checks

        public bool debuffExists(int Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a => a.SpellId == Debuff && a.CreatorGuid == Me.Guid);
                if (aura != null)
                {
                    return true;
                }
            }
            return false;
        }

        public double debuffTimeLeft(int Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a =>
                    a.SpellId == Debuff
                    && a.CreatorGuid == Me.Guid);

                if (aura == null)
                {
                    return 0;
                }
                return aura.TimeLeft.TotalMilliseconds;
            }
            return 0;
        }

        public uint debuffStackCount(int Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a =>
                    a.SpellId == Debuff
                    && a.CreatorGuid == Me.Guid);

                if (aura == null)
                {
                    return 0;
                }
                return aura.StackCount;
            }
            return 0;
        }
        public bool debuffExists(string Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a => a.Name == Debuff && a.CreatorGuid == Me.Guid);
                if (aura != null)
                {
                    return true;
                }
            }
            return false;
        }

        public double debuffTimeLeft(string Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a =>
                    a.Name == Debuff
                    && a.CreatorGuid == Me.Guid);

                if (aura == null)
                {
                    return 0;
                }
                return aura.TimeLeft.TotalMilliseconds;
            }
            return 0;
        }

        public uint debuffStackCount(string Debuff, WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                WoWAura aura = onTarget.GetAllAuras().FirstOrDefault(a =>
                    a.Name == Debuff
                    && a.CreatorGuid == Me.Guid);

                if (aura == null)
                {
                    return 0;
                }
                return aura.StackCount;
            }
            return 0;
        }
        #endregion

        #region Focus checks
        public double Focus
        {
            get
            {
                try
                {
                    using (StyxWoW.Memory.AcquireFrame())
                    {
                        return Styx.WoWInternals.Lua.GetReturnVal<int>("return UnitPower(\"player\");", 0);
                    }
                }
                catch { return 0; }
            }
        }
        #endregion

        #region Target Checks
        internal IEnumerable<WoWUnit> attackableTargets
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(U => U.Attackable && U.CanSelect && !U.IsFriendly && !U.IsDead && !U.IsNonCombatPet && !U.IsCritter); }
        }
        public bool gotTarget { get { return Me.GotTarget && Me.CurrentTarget != null && Me.CurrentTarget.Attackable && (!Me.CurrentTarget.IsDead && !Me.CurrentTarget.IsFriendly); } }
        
        internal IEnumerable<WoWUnit> nearbyTargets(WoWPoint fromLocation, double Radius)
        {
            var Hostile = attackableTargets;
            var maxDistance = Radius * Radius;
            return Hostile.Where(x => x.Location.DistanceSqr(fromLocation) < maxDistance);
        }
        public double targetDistance(WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.DistanceSqr;
                if (Results != null)
                    return onTarget.DistanceSqr;
            }
            return 0;
        }
        public double targetHP(WoWUnit onTarget)
        {
            if (onTarget != null)
            {
                var Results = onTarget.HealthPercent;
                if (Results != null)
                    return onTarget.HealthPercent;
            }
            return 0;
        }
        #endregion

        #region Timer Checks
        private int conv_Date2Timestam(DateTime _time)
        {
            var date1 = new DateTime(1970, 1, 1);
            DateTime date2 = _time;
            var ts = new TimeSpan(date2.Ticks - date1.Ticks);
            return (Convert.ToInt32(ts.TotalSeconds));
        }
        private uint current_life;
        private int current_time;
        private ulong guid;
        private uint first_life;
        private uint first_life_max;
        private int first_time;
        public long targetExistence(WoWUnit onTarget)
        {
            if (onTarget == null) return 0;
            if (Me.CurrentTarget.Name.Contains("Dummy")) return 9999;
            if (onTarget.CurrentHealth == 0 || onTarget.IsDead || !onTarget.IsValid || !onTarget.IsAlive)
                return 0;
            if (guid != onTarget.Guid)
            {
                guid = onTarget.Guid;
                first_life = onTarget.CurrentHealth;
                first_life_max = onTarget.MaxHealth;
                first_time = conv_Date2Timestam(DateTime.Now);
            }
            current_life = onTarget.CurrentHealth;
            current_time = conv_Date2Timestam(DateTime.Now);
            var time_diff = current_time - first_time;
            var hp_diff = first_life - current_life;
            if (hp_diff > 0)
            {
                var full_time = time_diff * first_life_max / hp_diff;
                var past_first_time = (first_life_max - first_life) * time_diff / hp_diff;
                var calc_time = first_time - past_first_time + full_time - current_time;
                if (calc_time < 1) calc_time = 99;
                var time_to_die = calc_time;
                var fight_length = full_time;
                return time_to_die;
            }
            if (hp_diff < 0)
            {
                guid = onTarget.Guid;
                first_life = onTarget.CurrentHealth;
                first_life_max = onTarget.MaxHealth;
                first_time = conv_Date2Timestam(DateTime.Now);
                return -1;
            }
            if (current_life == first_life_max)
                return 9999;
            return -1;
        }
        #endregion

        #region movement, facing, targeting

        public void MoveInRange(float range)
        {
            if (gotTarget && !CRSettings.myPrefs.Movement && Me.CurrentTarget.Distance > range)
            {
                while (Me.CurrentTarget.Distance > range || !Me.CurrentTarget.InLineOfSight)
                {
                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                }
                Navigator.PlayerMover.MoveStop();
            }
        }
        public void MoveInAoeRange(float range)
        {
            if (gotTarget && Me.CurrentTarget.Distance > range)
            {
                while (Me.CurrentTarget.Distance > range || !Me.CurrentTarget.InLineOfSight)
                {
                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                }
                Navigator.PlayerMover.MoveStop();
            }
        }

        Composite Facing()
        {
            return new Decorator(ret => gotTarget && !Me.IsSafelyFacing(Me.CurrentTarget) && !CRSettings.myPrefs.Facing,
                new Action(ret => Me.CurrentTarget.Face())
            );
        }
        Composite checkRangeAoe(float range)
        {
            return new Decorator(ret => gotTarget && Me.CurrentTarget.InLineOfSight && Me.CurrentTarget.Distance > range,
                new Action(ret =>
                {
                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                }));
        }
        Composite checkRange(float range)
        {
            return new Decorator(ret => gotTarget && !CRSettings.myPrefs.Movement && Me.CurrentTarget.InLineOfSight && Me.CurrentTarget.Distance > range,
                new Action(ret =>
                {
                    Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                }));
        }
        Composite MeMoving()
        {
            return new Decorator(ret => gotTarget && !CRSettings.myPrefs.Movement && Me.IsMoving && Me.CurrentTarget.Distance <= FightRange,
                new Action(ret => Navigator.PlayerMover.MoveStop()));
        }
        Composite MoveInSpellSight()
        {
            return new Decorator(ret => gotTarget && !CRSettings.myPrefs.Movement && !Me.CurrentTarget.InLineOfSight,
                new Action(ret =>
                    {
                        Navigator.MoveTo(StyxWoW.Me.CurrentTarget.Location);
                    }));

        }
        public float FightRange
        {
            get
            {
                float range = 5;
                if (Me.Specialization == WoWSpec.DruidBalance)
                {
                    range = 39;
                }
                if (Me.CurrentTarget.IsPlayer)
                {
                    range = 3.5f;
                }
                return range;
            }
        }
        Composite newTarget()
        {
            return new Decorator(ret => !gotTarget && Me.Combat && !CRSettings.myPrefs.Targeting,
                new Action(ret =>
                {
                    var nieuwTarget = FindTarget();
                    nieuwTarget.Target();
                }));
        }
        #endregion movement, facing, targeting

        #region get target
        public void getTarget()
        {
            if (!gotTarget
                && Me.Combat
                && !CRSettings.myPrefs.Targeting)
            {
                var nieuwTarget = FindTarget();
                nieuwTarget.Target();
            }
        }
        WoWUnit FindTarget()
        {
            WoWUnit newTarget = (from o in ObjectManager.ObjectList
                                 where o is WoWUnit
                                 let unit = o.ToUnit()
                                 where
                                     unit.Distance <= 8
                                     && unit.CanSelect
                                     && !unit.IsCritter
                                     && !unit.IsPetBattleCritter
                                     && !unit.TaggedByOther
                                     && unit.IsAlive
                                     && unit.IsHostile
                                     && !unit.IsPet
                                     && !Blacklist.Contains(unit.Guid, BlacklistFlags.All)
                                 orderby unit.Distance ascending
                                 select unit
                                 ).FirstOrDefault();
            //newTarget.Target();
            LogMsg("found new target " + newTarget.Name, 0);
            return newTarget;

        }
        #endregion get target

        #region Eat/Drink
        Composite Drinking()
        {
            return new Decorator(ret => Me.ManaPercent <= 45 
                && !Me.Mounted 
                && !MeInGroup
                && !Me.IsDead
                && !Me.IsGhost
                && !Me.IsSwimming,
                new Action(ret =>
                {
                    Styx.CommonBot.Rest.Feed();
                }));
        }
        Composite Eating()
        {
            return new Decorator(ret => Me.HealthPercent <= 45 
                && !Me.Mounted 
                && !MeInGroup
                && !Me.IsDead
                && !Me.IsGhost
                && !Me.IsSwimming,
                new Action(ret =>
                {
                    Styx.CommonBot.Rest.Feed();
                }));
        }
        #endregion Eat/Drink

        #region groupinfo
        public bool MeInGroup
        {
            get
            {
                if(Me.GroupInfo.IsInParty
                    || Me.GroupInfo.IsInRaid
                    || Me.IsInInstance)
                {
                    return true;
                }
                return false;
            }
        }
        #endregion groupinfo

    }
}
