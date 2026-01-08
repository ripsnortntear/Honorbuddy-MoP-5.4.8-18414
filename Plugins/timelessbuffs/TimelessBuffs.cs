using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.AreaManagement;
using Styx.Pathing;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Bots.DungeonBuddy.Helpers;
using System.Windows.Media;


namespace TimelessBuffs
{

    public class FightHere : HBPlugin
    {

        public override string Name { get { return "Timeless Buffs"; } }
        public override string Author { get { return "Pasterke"; } }
        public override Version Version { get { return _version; } }
        private readonly Version _version = new Version(1, 0, 0, 0);
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        public string LastSpell { get; set; }
        public int LastSpellID { get; set; }
        public Stopwatch checkBuffTimer = new Stopwatch();

        #region show form
        public override bool WantButton { get { return true; } }

        public override void OnButtonPress()
        {
            Form1 ConfigForm = new Form1();
            ConfigForm.ShowDialog();
        }
        #endregion

      public virtual void OnEnable()
        {
            LogMsg("My ZoneID = " + Me.ZoneId);
            if (Me.ZoneId != 6757)
            {
                LogMsg("We are not on Timeless Isle and we can't use Timeless Buffs");
                LogMsg("Crystal of Insanity will be used if you have one");
                LogMsg("Winterfall Firewater buff supported");
            }
            Updater.CheckForUpdate(UpdateSettingsTimelessBuffs.myPrefs.Revision);
        }

        public override void Pulse()
        {
            try
            {
                if (Me.CurrentMap.IsArena || buffExists("Censer of Eternal Agony", Me))
                {
                    return;
                }

                if (Me.ZoneId == 6757 && CanBuff)
                {
                    checkBuffs();
                }
                if (!HaveFlaskBuff && !buffExists(CRYSTAL_OF_INSANITY_BUFF, Me) //Crystal of Insanity
                    && TimelessSettings.myPrefs.AutoBuffCrystal
                    && CanBuff
                    && nextBuffAllowed <= DateTime.Now)
                {
                    applyBuffs(CRYSTAL_OF_INSANITY_ITEM);
                }
                if (Me.ZoneId == 618 && !buffExists(WINTERFALL_FIREWATER_BUFF, Me)
                    && CanBuff
                    && nextBuffAllowed <= DateTime.Now)
                {
                    applyBuffs(WINTERFAL_FIREWATER_ITEM);
                }

                return;
            }
            catch { }
        }
        public bool CanBuff
        {
            get
            {
                return !Me.Mounted
                        && !Me.IsFlying
                        && !Me.IsDead
                        && !Me.IsGhost
                        && !Me.OnTaxi
                        && !Me.IsOnTransport;
            }
        }
        public void checkBuffs()
        {

            if (!buffExists(DEW_OF_ETERNAL_MORNING_BUFF, Me) && nextBuffAllowed <= DateTime.Now) //Dew of Eternal Morning
            {
                applyBuffs(103643);
            }
            if (!buffExists(BOOK_OF_THE_AGES_BUFF, Me) && nextBuffAllowed <= DateTime.Now) //Book of the Ages
            {
                applyBuffs(BOOK_OF_THE_AGES_ITEM);
            }
            if (!buffExists(SINGING_CRYSTAL_BUFF, Me) && nextBuffAllowed <= DateTime.Now) //Singing Crystal
            {
                applyBuffs(SINGING_CRYSTAL_ITEM);
            }
            if (!buffExists(WINDFEATHER_PLUME_BUFF, Me) && nextBuffAllowed <= DateTime.Now)
            {
                applyBuffs(WINDFEATHER_PLUME_ITEM);
            }
            if (Me.Combat
                && Me.CurrentTarget != null
                && Me.CurrentTarget.IsHostile
                && !buffExists(GLOWING_MUSHROOM_BUFF, Me)
                //&& Me.HealthPercent <= TimelessSettings.myPrefs.AutoBuffGlowingMushroom
                && nextBuffAllowed <= DateTime.Now)
            {
                applyBuffs(GLOWING_MUSHROOM_ITEM);
            }
            if (Me.Combat
                && Me.CurrentTarget != null
                && Me.CurrentTarget.IsHostile
                && !buffExists(GLOWING_HERB_BUFF, Me)
                && nextBuffAllowed <= DateTime.Now)
            {
                applyBuffs(GLOWING_HERB_ITEM);
            }
            if (Me.Combat
                && Me.CurrentTarget != null
                && Me.CurrentTarget.IsHostile
                && nextBuffAllowed <= DateTime.Now)
            {
                dropBuffs(104335);
            }
            if (Me.Combat
                && Me.CurrentTarget != null
                && Me.CurrentTarget.IsHostile
                && nextBuffAllowed <= DateTime.Now)
            {
                dropBuffs(104334);
            }
            if (Me.Combat
                && Me.CurrentTarget != null
                && Me.CurrentTarget.IsHostile
                && nextBuffAllowed <= DateTime.Now)
            {
                dropBuffs(104336);
            }
            return;
        }
        public void applyBuffs(int buffName)
        {
            if (!buffExists(buffName, Me))
            {
                WoWItem potion = Me.BagItems.FirstOrDefault(h => h.Entry == buffName);

                if (potion == null)
                {
                    return;
                }
                if (potion != null && potion.CooldownTimeLeft.TotalMilliseconds <= 0)
                {
                    potion.Use();
                    LogMsg("Using " + potion.Name);
                    SetNextBuffAllowed();
                }
            }
        }
        public void dropBuffs(int buffName)
        {
            if (!buffExists(buffName, Me))
            {
                WoWItem potion = Me.BagItems.FirstOrDefault(h => h.Entry == buffName);

                if (potion == null)
                {
                    return;
                }
                if (potion != null && potion.CooldownTimeLeft.TotalMilliseconds <= 0)
                {
                    potion.Use();
                    SpellManager.ClickRemoteLocation(Me.Location);
                    LogMsg("Using " + potion.Name);
                    SetNextBuffAllowed();
                }
            }
        }
        #region logs
        public void LogMsg(string msg)
        {
            Logging.Write(msg);
        }
        #endregion logs
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
        #endregion buffchecks

        public int SINGING_CRYSTAL_BUFF = 147055;
        public int SINGING_CRYSTAL_ITEM = 103641;
        public int DEW_OF_ETERNAL_MORNING_BUFF = 147476;
        public int DEW_OF_ETERNAL_MORNING_ITEM = 103643;
        public int BOOK_OF_THE_AGES_BUFF = 147226;
        public int BOOK_OF_THE_AGES_ITEM = 103642;
        public int WINDFEATHER_PLUME_BUFF = 148521;
        public int WINDFEATHER_PLUME_ITEM = 104287;
        public int CRYSTAL_OF_INSANITY_ITEM = 86569;
        public int CRYSTAL_OF_INSANITY_BUFF = 127230;
        public int FLASK_OF_THE_WARM_SUN = 105691;
        public int FLASK_OF_WINTERS_BITE = 105696;
        public int FLASK_OF_THE_EARTH = 105694;
        public int FLASK_OF_SPRING_BLOSSOMS = 105689;
        public int FLASK_OF_FALING_LEAVES = 105693;
        public int GLOWING_HERB_ITEM = 104289;
        public int GLOWING_HERB_BUFF = 148525;
        public int WINTERFAL_FIREWATER_ITEM = 12820;
        public int WINTERFALL_FIREWATER_BUFF = 17038;
        public int GLOWING_MUSHROOM_ITEM = 104312;
        public int GLOWING_MUSHROOM_BUFF = 148554;

        #region valid units
        private static Color invalidColor = Colors.LightCoral;

        public static bool ValidUnit(WoWUnit p, bool showReason = false)
        {
            if (p == null || !p.IsValid)
                return false;

            // Ignore shit we can't select
            if (!p.CanSelect)
            {
                if (showReason) Logger.Write(invalidColor, "invalid attack unit {0} cannot be Selected", p.Name);
                return false;
            }

            // Ignore shit we can't attack
            if (!p.Attackable)
            {
                if (showReason) Logger.Write(invalidColor, "invalid attack unit {0} cannot be Attacked", p.Name);
                return false;
            }

            // Duh
            if (p.IsDead)
            {
                if (showReason) Logger.Write(invalidColor, "invalid attack unit {0} is already Dead", p.Name);
                return false;
            }

            // check for enemy players here as friendly only seems to work on npc's
            if (p.IsPlayer)
                return p.ToPlayer().IsHorde != StyxWoW.Me.IsHorde;

            // Ignore friendlies!
            if (p.IsFriendly)
            {
                if (showReason) Logger.Write(invalidColor, "invalid attack unit {0} is Friendly", p.Name);
                return false;
            }

            // If it is a pet/minion/totem, lets find the root of ownership chain
            WoWUnit pOwner = GetPlayerParent(p);

            // ignore if owner is player, alive, and not blacklisted then ignore (since killing owner kills it)
            if (pOwner != null && pOwner.IsAlive && !Blacklist.Contains(pOwner, BlacklistFlags.Combat))
            {
                if (showReason) Logger.Write(invalidColor, "invalid attack unit {0} has a Player as Parent", p.Name);
                return false;
            }

            // And ignore critters (except for those ferocious ones) /non-combat pets
            if (p.IsNonCombatPet)
            {
                if (showReason) Logger.Write(invalidColor, "{0} is a Noncombat Pet", p.Name);
                return false;
            }

            // And ignore critters (except for those ferocious ones) /non-combat pets
            if (p.IsCritter && p.ThreatInfo.ThreatValue == 0 && !p.IsTargetingMyRaidMember)
            {
                if (showReason) Logger.Write(invalidColor, "{0} is a Critter", p.Name);
                return false;
            }
            /*
                        if (p.CreatedByUnitGuid != 0 || p.SummonedByUnitGuid != 0)
                            return false;
            */
            return true;
        }
        public static WoWUnit GetPlayerParent(WoWUnit unit)
        {
            WoWUnit petOwner = unit;
            while (true)
            {
                if (petOwner.OwnedByUnit != null)
                    petOwner = petOwner.OwnedByRoot;
                else if (petOwner.CreatedByUnit != null)
                    petOwner = petOwner.CreatedByUnit;
                else if (petOwner.SummonedByUnit != null)
                    petOwner = petOwner.SummonedByUnit;
                else
                    break;
            }

            if (unit != petOwner && petOwner.IsPlayer)
                return petOwner;

            return null;
        }
        #endregion

        #region flask buffs
        public int Flask_OF_THE_EARTH_BUFF = 105694;
        public int Flask_OF_THE_WARM_SUN_BUFF = 105691;
        public int FLASK_OF_WINTERS_BITE_BUFF = 105696;
        public int FLASK_OF_FALLING_LEAVES_BUFF = 105693;
        public int FLASK_OF_SPRING_BLOSSOMS_BUFF = 105689;

        public bool HaveFlaskBuff
        {
            get
            {
                return buffExists(Flask_OF_THE_EARTH_BUFF, Me)
                    || buffExists(Flask_OF_THE_WARM_SUN_BUFF, Me)
                    || buffExists(FLASK_OF_WINTERS_BITE_BUFF, Me)
                    || buffExists(FLASK_OF_FALLING_LEAVES_BUFF, Me)
                    || buffExists(FLASK_OF_SPRING_BLOSSOMS_BUFF, Me);
            }
        }
        #endregion

        #region next buff allowed
        private DateTime nextBuffAllowed;

        public void SetNextBuffAllowed()
        {
            nextBuffAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 1500);
        }
        #endregion
        //the end
    }
}
