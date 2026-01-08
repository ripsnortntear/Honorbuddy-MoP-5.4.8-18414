using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Styx;
using Styx.CommonBot.Database;
using Styx.Helpers;
using Styx.Loaders;
using Styx.CommonBot.AreaManagement;
using Styx.CommonBot.AreaManagement.Triangulation;
using Styx.TreeSharp;
using Styx.CommonBot.Inventory;
using Styx.CommonBot.Frames;
using Styx.Pathing;
using Styx.Pathing.OnDemandDownloading;
using Styx.Patchables;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.DBC;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWCache;
using Styx.WoWInternals.WoWObjects;
using System.Diagnostics;
using Styx.CommonBot;
using Styx.Common;
using System.Windows.Forms;
using System.IO;

namespace AzeniusHelper2
{
    public class AzeniusHelper : HBPlugin
    {
        #region Plugin overrides

        public override string Name { get { return "Pandaria Dailies v2"; } }
        public override string Author { get { return "Buddy Community"; } }
        public override Version Version { get { return new Version(2, 0, 3); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Credits"; } }

        public override void OnButtonPress()
        {
            MessageBox.Show("This Plugin [" + Name + "] is originally made by Megser. \n(Modified by Vego, TheBrodieMan and BarryDurex)", 
                Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void dlog(string message, params object[] args)
        { Logging.WriteDiagnostic(System.Windows.Media.Colors.Firebrick, "[Pandaria Dailies]: " + message, args); }
        public static void dlog(System.Windows.Media.Color color, string message, params object[] args)
        { Logging.WriteDiagnostic(color, "[Pandaria Dailies]: " + message, args); }

        public override void Initialize()
        {
            //BotEvents.OnBotStart += onBotStart;
        }

        #endregion

        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static Stopwatch GlobalTimer = new Stopwatch();
        private bool firstPulse = true;

        #region on Bot start

        private void onBotStart(EventArgs args)
        {
            dlog("start [Pandaria Dailies] v{0} - ({1})", Version, Lua.GetReturnVal<string>("return GetLocale()",0));
            Logging.Write(System.Windows.Media.Colors.LawnGreen, "This Plugin [{0}] is originally made by Megser.", Name);
            Logging.Write(System.Windows.Media.Colors.LawnGreen, "(Modified by Vego, TheBrodieMan and BarryDurex)");


            // 1 to 3 is the Guild Reputation  http://www.wowwiki.com/API_GetFactionInfo            
            dlog("---------------------");
            for (int i = 4; i <= Lua.GetReturnVal<int>("return GetNumFactions();", 0); i++) 
            {
                // name, description, standingId, bottomValue, topValue, earnedValue, atWarWith, canToggleAtWar, isHeader, isCollapsed, hasRep, isWatched, isChild
                List<string> factionsInfo = Lua.GetReturnValues("return GetFactionInfo(" + i + ");");
                if (factionsInfo[0].Contains("Cataclysm"))
                    break;
                
                dlog("[{0}] - {1} - ({2}/{3})", factionsInfo[0], FactionStanding(factionsInfo[2]), factionsInfo[5], factionsInfo[4]);
            }
            dlog("---------------------");
        }

        /// <summary>
        /// http://www.wowwiki.com/API_TYPE_StandingId
        /// </summary>
        private string FactionStanding(string id)
        {
            string ret = string.Empty;
            switch (id)
            {
                case ("1"):
                    ret = "Hated";
                    break;
                case ("2"):
                    ret = "Hostile";
                    break;
                case ("3"):
                    ret = "Unfriendly";
                    break;
                case ("4"):
                    ret = "Neutral";
                    break;
                case ("5"):
                    ret = "Friendly";
                    break;
                case ("6"):
                    ret = "Honored";
                    break;
                case ("7"):
                    ret = "Revered";
                    break;
                case ("8"):
                    ret = "Exalted";
                    break;
                default:
                    ret = "Unknown";
                    break;
            }
            return ret;
        }

        #endregion
        
        #region List of WoWUnits and WoWGameObjects needed

        private Dictionary<ulong, WoWPoint> _captivePandarenSpirit = new Dictionary<ulong, WoWPoint>();
        public WoWUnit CaptivePandarenSpirit
        {
            get
            {
                List<WoWUnit> _pandaren = ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => !Blacklist.Contains(u, BlacklistFlags.All) && u.IsValid && u.Entry == 59231).ToList();

                if (_pandaren == null)
                    return null;

                foreach (WoWUnit P in _pandaren)
                {
                    if (_captivePandarenSpirit.Keys.Contains(P.Guid) && P.Location.Z != _captivePandarenSpirit[P.Guid].Z)
                    {
                        Blacklist.Add(P, BlacklistFlags.All, TimeSpan.FromMinutes(1));
                        continue;
                    }

                    else if (!_captivePandarenSpirit.Keys.Contains(P.Guid))
                        _captivePandarenSpirit.Add(P.Guid, P.Location);
                }
                return _pandaren.Where(u => !Blacklist.Contains(u, BlacklistFlags.All)).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        public WoWUnit DreadKunchong
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 64717 && !u.IsDead).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }

        public List<WoWUnit> PandarenSpirit
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 59231 && u.Distance < 5).OrderBy(u => u.Distance).ToList();
            }
        }

        public List<WoWUnit> ShaoTienMindbinder
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 63221 && u.Distance < 5).OrderBy(u => u.Distance).ToList();
            }
        }

        public List<WoWUnit> BrazierFire
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 63787 && u.Distance < 5).OrderBy(u => u.Distance).ToList();
            }
        }

        public WoWUnit Tormentor
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 59238 && !u.IsDead).OrderBy(u => u.Distance).FirstOrDefault();
            }
        }


        public WoWUnit Behemoth
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 65824 && u.IsAlive).FirstOrDefault();
            }
        }

        public List<WoWUnit> Krichon
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 63978 && u.Distance < 30).OrderBy(u => u.Distance).ToList();
            }
        }


        public WoWUnit Cracklefang
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.IsValid && u.Entry == 58768 && u.IsAlive).FirstOrDefault();
            }
        }


        public List<WoWUnit> Sydow
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 63240 && u.Distance < 30).OrderBy(u => u.Distance).ToList();
            }
        }

        public List<WoWUnit> SpiritofViolence
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 64656 && u.Distance < 30).OrderBy(u => u.Distance).ToList();
            }
        }

        public List<WoWUnit> SpiritofAnger
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 64684 && u.Distance < 30).OrderBy(u => u.Distance).ToList();
            }
        }

        public List<WoWUnit> SpiritofHatredA
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 64744 && u.Distance < 30).OrderBy(u => u.Distance).ToList();
            }
        }

        public List<WoWUnit> SpiritofHatredH
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 64742 && u.Distance < 30).OrderBy(u => u.Distance).ToList();
            }
        }

        public WoWUnit Vicejaw
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 58769 && u.IsAlive && u.Distance < 30).FirstOrDefault();
            }
        }

        public List<WoWUnit> MantidNiuzao
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == 61502 || u.Entry == 61508 || u.Entry == 61509 && u.Distance < 50 && !u.IsDead).OrderBy(u =>

u.Distance).ToList();
            }
        }

        //public List<WoWUnit> DreadKunchong
        // {
        //   get
        //   {
        //      return ObjectManager.GetObjectsOfType<WoWUnit>()
        //                        .Where(u => u.Entry == 64717 && !u.IsDead)
        //                       .OrderBy(u => u.Distance).ToList();
        //  }
        //  }

        #endregion

        #region Some Quest Helper Functions

        public bool IsOnQuest(uint questId)
        {
            return Me.QuestLog.GetQuestById(questId) != null && !Me.QuestLog.GetQuestById(questId).IsCompleted;
        }

        public bool QuestComplete(uint questId)
        {
            return Me.QuestLog.GetQuestById(questId).IsCompleted;
        }


        public bool QuestFailed(uint questId)
        {
            return Me.QuestLog.GetQuestById(questId).IsFailed;
        }

        public bool ItemOnCooldown(uint ItemId)
        {
            return Lua.GetReturnVal<bool>("GetItemCooldown(" + ItemId + ")", 0);

        }

        public void UseQuestItem(uint ItemId)
        {
            Lua.DoString("UseItemByName(" + ItemId + ")");
        }

        public void UseIfNotOnCooldown(uint ItemId)
        {
            if (!ItemOnCooldown(ItemId))
            {
                UseQuestItem(ItemId);
            }
        }

        public bool QuestObjectiveComplete(uint questId, int objectiveNum)
        {
            return (Lua.GetReturnVal<int>("a,b,c=GetQuestLogLeaderBoard(" + objectiveNum + ",GetQuestLogIndexByID(" + questId + "));if c==1 then return 1 else return 0 end", 0) == 1);
        }

        public bool TargetingNpc(uint npcId)
        {
            return Me.CurrentTarget.Entry == npcId;
        }

        public bool HasQuest(uint questId)
        {
            return Me.QuestLog.GetQuestById(questId) != null;
        }

        #endregion

        public override void Pulse()
        {
            if (firstPulse)
            { onBotStart(null); firstPulse = false; }

            if (Me.IsDead || Me.IsGhost)
                return;
            ObjectManager.Update();

            #region [Golden Lotus]

            #region Lightning Pool
            if (Me.HasAura("Lightning Pool"))
            {
                BarryDurex.QuestHelper.AvoidEnemyAOE(Me.Location, BarryDurex.QuestHelper.getLightningPoolList, "Lightning Pool", 15); //TODO test it..
            }
            #endregion

            #region http://www.wowhead.com/quest=30251
            if (Me.HasAura("Caustic Pitch"))
            {
                BarryDurex.QuestHelper.AvoidEnemyAOE(Me.Location, BarryDurex.QuestHelper.getCausticPitchList, "Caustic Pitch", 15);
            }

            #endregion

            if (Me.HasAura("Venom Splash") && BarryDurex.QuestHelper.getVenomSplashList != null && BarryDurex.QuestHelper.getVenomSplashList[0].Distance < (BarryDurex.QuestHelper.getVenomSplashList[0].Radius * 1.6f))
            {
                BarryDurex.QuestHelper.AvoidEnemyAOE(Me.Location, BarryDurex.QuestHelper.getVenomSplashList, "Venom Splash", 15);
            }

            #region http://www.wowhead.com/quest=30320 - done
            if (Me.HasAura("Spirit Void"))
            {
                if (!IsOnQuest(30320))
                {
                    Me.Auras["Spirit Void"].TryCancelAura();
                    Thread.Sleep(200);
                }
                else
                {
                    if (!Me.Combat && !Me.Mounted)
                        Flightor.MountHelper.MountUp();

                    if (Me.Mounted && (Me.Combat || (Tormentor != null && Tormentor.Location.Distance(Me.Location) < 10)))
                    { WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend); Thread.Sleep(836); WoWMovement.MoveStop(); }

                    if (CaptivePandarenSpirit != null && CaptivePandarenSpirit.Distance >= CaptivePandarenSpirit.InteractRange && Me.Mounted)
                    {
                        while (CaptivePandarenSpirit.Distance >= CaptivePandarenSpirit.InteractRange)
                        {
                            if (Me.Combat)
                            { WoWMovement.Move(WoWMovement.MovementDirection.JumpAscend); Thread.Sleep(836); WoWMovement.MoveStop(); continue; }

                            Flightor.MoveTo(CaptivePandarenSpirit.Location);
                            Thread.Sleep(80);
                        }
                        WoWMovement.MoveStop();
                    }
                }
            }
            #endregion

            #region http://www.wowhead.com/quest=31762 - done
            // dont use IsOnQuest! 
            // we need this behavior if we are (again) in combat with Behemoth
            if (Behemoth != null)
            {
                if (StyxWoW.Me.Combat && Behemoth.Distance2D <= 15 && Behemoth.IsCasting && Behemoth.CastingSpellId == 131043)
                {
                    BarryDurex.QuestHelper.AvoidEnemyCast(Behemoth, 80, 15);
                }

            }
            #endregion

            #region Vicejaw
            if (IsOnQuest(30234)) //TODO QuestBehavior (BarryDurex)
            {

                if (Vicejaw != null && Vicejaw.IsCasting && !Me.IsBehind(Vicejaw))
                {
                    Thread.Sleep(2000);
                    //Lua.DoString("StrafeLeftStart()");
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(2));
                    Thread.Sleep(2000);
                    WoWMovement.MoveStop();
                }

            }
            #endregion

            #region http://www.wowhead.com/quest=30192 - todo
            if (IsOnQuest(30192))
            {
                if (BrazierFire != null)
                    BrazierFire[0].Interact();
            }
            #endregion

            #region http://www.wowhead.com/quest=30482 - toDo
            if (IsOnQuest(30482))
            {

                if (Sydow != null && Sydow[0].CastingSpellId == 126347)
                {
                    //Lua.DoString("StrafeLeftStart()");
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(1));
                    Thread.Sleep(1000);
                    WoWMovement.MoveStop();
                }

            }
            #endregion

            #region http://www.wowhead.com/quest=30233 - done
            if (/*IsOnQuest(30233)*/ true)
            {
                if (Cracklefang != null && Cracklefang.Distance2D <= 20 && Cracklefang.IsCasting /*&& Cracklefang.CastingSpellId == 126032*/)
                {
                    BarryDurex.QuestHelper.AvoidEnemyCast(Cracklefang, 0, 20);
                }
            }
            #endregion

            #region http://www.wowhead.com/quest=30293
            // In Enemy Hands
            if (IsOnQuest(30293))
            {
                if (ShaoTienMindbinder != null && Me.CurrentTargetGuid != ShaoTienMindbinder[0].Guid)
                    ShaoTienMindbinder[0].Target();
            }

            #endregion

            #region http://www.wowhead.com/quest=30249 - todo
            if (IsOnQuest(30249))
            {

                if (Krichon != null && Krichon[0].IsCasting && !Me.IsBehind(Krichon[0]))
                {
                    //Lua.DoString("StrafeLeftStart()");
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(1));
                    Thread.Sleep(1000);
                    WoWMovement.MoveStop();
                }

            }
            #endregion

            #region A Celestial Experience - todo
            #region http://www.wowhead.com/quest=31394 - Allianz
            if (IsOnQuest(31394))
            {

                if (SpiritofViolence != null && SpiritofViolence[0].IsCasting && !Me.IsBehind(SpiritofViolence[0]))
                {
                    //Lua.DoString("StrafeLeftStart()");
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(1));
                    Thread.Sleep(1000);
                    WoWMovement.MoveStop();
                }

                if (SpiritofAnger != null && SpiritofAnger[0].IsCasting && !Me.IsBehind(SpiritofAnger[0]))
                {
                    //Lua.DoString("StrafeLeftStart()");
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(1));
                    Thread.Sleep(1000);
                    WoWMovement.MoveStop();
                }

                if (SpiritofHatredA != null && SpiritofHatredA[0].IsCasting && !Me.IsBehind(SpiritofHatredA[0]))
                {
                    //Lua.DoString("StrafeLeftStart()");
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(1));
                    Thread.Sleep(1000);
                    WoWMovement.MoveStop();
                }

            }
            #endregion

            #region http://www.wowhead.com/quest=31395 - Horde
            if (IsOnQuest(31395))
            {

                if (SpiritofViolence != null && SpiritofViolence[0].IsCasting && !Me.IsBehind(SpiritofViolence[0]))
                {
                    //Lua.DoString("StrafeLeftStart()");
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(1));
                    Thread.Sleep(1000);
                    WoWMovement.MoveStop();
                }

                if (SpiritofAnger != null && SpiritofAnger[0].IsCasting && !Me.IsBehind(SpiritofAnger[0]))
                {
                    //Lua.DoString("StrafeLeftStart()");
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(1));
                    Thread.Sleep(1000);
                    WoWMovement.MoveStop();
                }

                if (SpiritofHatredH != null && SpiritofHatredH[0].IsCasting && !Me.IsBehind(SpiritofHatredH[0]))
                {
                    //Lua.DoString("StrafeLeftStart()");
                    WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(1));
                    Thread.Sleep(1000);
                    WoWMovement.MoveStop();
                }

            }
            #endregion

            if (Me.HasAura("Sha Corruption"))
            {
                //Lua.DoString("StrafeLeftStart()"); // Strafe Left if we're in a pool
                WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(1));
                Thread.Sleep(1000);
                WoWMovement.MoveStop();
            }
            #endregion
            #endregion

            #region [The August Celestials]
            // (Die Himmlischen Erhabenen)
            // http://www.wowhead.com/quest=30952 http://www.wowhead.com/quest=30953 http://www.wowhead.com/quest=30954 http://www.wowhead.com/quest=30955
            // http://www.wowhead.com/quest=30956 http://www.wowhead.com/quest=30957 http://www.wowhead.com/quest=30958 http://www.wowhead.com/quest=30959
            if (IsOnQuest(30952) || IsOnQuest(30953) || IsOnQuest(30954) || IsOnQuest(30955) || IsOnQuest(30956) || IsOnQuest(30957) || IsOnQuest(30958) || IsOnQuest(30959))
            {
                if (Me.Combat && Me.IsMoving)
                {
                    ObjectManager.Update();
                    Thread.Sleep(200);
                    if (MantidNiuzao != null)
                    {
                        MantidNiuzao[0].Face();
                        MantidNiuzao[0].Target();
                        Thread.Sleep(1000);
                    }
                }

            }
            #endregion

            #region [Klaxxi]
            // http://www.wowhead.com/quest=31487
            if (DreadKunchong != null)
            {
                if (Me.Combat && DreadKunchong.Distance2D <= 15 && DreadKunchong.IsCasting && DreadKunchong.CastingSpellId == 128022)
                    BarryDurex.QuestHelper.AvoidEnemyCast(DreadKunchong, 80, 15);
            }
            #endregion
        }
    }
}

namespace BarryDurex
{
    static class QuestHelper
    {
        public static void slog(string message, params object[] args)
        { Styx.Common.Logging.Write(LogLevel.Quiet, System.Windows.Media.Colors.Salmon, "[QuestHelper]: " + message, args); }
        public static void dlog(string message, params object[] args)
        { Styx.Common.Logging.WriteDiagnostic(System.Windows.Media.Colors.Salmon, "[QuestHelper]: " + message, args); }
        public static void navlog(string message, params object[] args)
        { Styx.Common.Logging.WriteDiagnostic(System.Windows.Media.Colors.LawnGreen, "[QuestHelper]: " + message, args); }

        //private static int MinDistToPools = 7;
        //private static int MaxDistToMove = 40;
        //private static int TraceStep = 50;


        #region Lightning Pool Behaviors - not yet tested!

        public static void AvoidEnemyAOE(WoWPoint location, List<WoWDynamicObject> Objects, string Aura, int TraceStep)
        {
            if (Objects == null)
            { dlog("no Lightning Pools found .."); return; }
            dlog("found {0} {1}! start RayCast ..", Objects.Count, Aura);

            int MinDistToPools = (int)(Objects[0].Radius * 1.6f);
            int MaxDistToMove = MinDistToPools * 2;

            // get save location
            WoWPoint newP = getSaveLocation(location, Objects, MinDistToPools, MaxDistToMove, TraceStep);

            if (newP == WoWPoint.Empty)
            {
                // no save location found, move 2sec Strafe Left
                WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromSeconds(2));
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            else
            {
                // move to save location
                while (StyxWoW.Me.HasAura(Aura) && StyxWoW.Me.Location.Distance(newP) > 0.2f)
                {
                    Navigator.MoveTo(newP);
                    Thread.Sleep(80);
                }
            }

            WoWMovement.MoveStop();
            if (StyxWoW.Me.CurrentTargetGuid != 0)
                StyxWoW.Me.CurrentTarget.Face();

            //Styx.CommonBot.Blacklist.Add(
            //Styx.CommonBot.Profiles.Blackspot vv = new Styx.CommonBot.Profiles.Blackspot(
        }

        private static bool wlog(WoWDynamicObject obj)
        { dlog("add pool - dis2D: {0}", obj.Distance2D); return true; }
        public static List<WoWDynamicObject> getLightningPoolList
        {
            get
            {
                ObjectManager.Update();
                return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
                        orderby lp.Distance2D ascending
                        where lp.Entry == 129657
                        where wlog(lp)
                        select lp).ToList();
            }
        }

        public static List<WoWDynamicObject> getCausticPitchList
        {
            get
            {
                ObjectManager.Update();
                return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
                        orderby lp.Distance2D ascending
                        where lp.Entry == 126336
                        where wlog(lp)
                        select lp).ToList();
            }
        }

        public static List<WoWDynamicObject> getVenomSplashList
        {
            get
            {
                ObjectManager.Update();
                return (from lp in ObjectManager.GetObjectsOfType<WoWDynamicObject>()
                        orderby lp.Distance2D ascending
                        where lp.Entry == 79607
                        where wlog(lp)
                        select lp).ToList();
            }
        }

        #region RayCast

        private static WoWPoint getSaveLocation(WoWPoint Location, List<WoWDynamicObject> badObjects, int minDist, int maxDist, int traceStep)
        {
            navlog("Navigation: Looking for save Location around {0}.", Location);

            try
            {
                //float _PIx2 = 3.14159f * 2f;
                float _PIx2 = ((float)new Random().Next(1, 80) * (1.248349f + (float)new Random().NextDouble()));

                for (int i = 0, x = minDist; i < traceStep && x < maxDist; i++)
                {
                    WoWPoint p = Location.RayCast((i * _PIx2) / traceStep, x);

                    p.Z = getGroundZ(p);

                    if (p.Z != float.MinValue && StyxWoW.Me.Location.Distance2D(p) > 1 &&
                        (badObjects.FirstOrDefault(_obj => _obj.Location.Distance2D(p) <= minDist) == null) &&
                        //(ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(u => u.Location.Distance2D(p) < 20 && u.IsAlive && !u.Combat) == null) &&
                        Navigator.GeneratePath(StyxWoW.Me.Location, p).Length != 0)
                    {
                        if (getHighestSurroundingSlope(p) < 1.2f)
                        {
                            navlog("Navigation: Moving to {0}. Distance: {1}", p, Location.Distance(p));
                            return p;
                        }
                    }

                    if (i == (traceStep - 1))
                    {
                        i = 0;
                        x++;
                    }
                }
            }
            catch (Exception ex)
            { Logging.WriteException(ex); }


            dlog(" - No valid points returned by RayCast ...");
            return WoWPoint.Empty;

        }

        /// <summary>
        /// Credits to exemplar.
        /// </summary>
        /// <returns>Z-Coordinates for PoolPoints so we don't jump into the water.</returns>
        private static float getGroundZ(WoWPoint p)
        {
            WoWPoint ground = WoWPoint.Empty;

            GameWorld.TraceLine(new WoWPoint(p.X, p.Y, (p.Z + 100)), new WoWPoint(p.X, p.Y, (p.Z - 5)), GameWorld.CGWorldFrameHitFlags.HitTestGroundAndStructures/* | GameWorld.CGWorldFrameHitFlags.HitTestBoundingModels | GameWorld.CGWorldFrameHitFlags.HitTestWMO*/, out ground);

            if (ground != WoWPoint.Empty)
            {
                navlog(" - Ground Z: {0}.", ground.Z);
                return ground.Z;
            }
            dlog(" - Ground Z returned float.MinValue.");
            return float.MinValue;
        }

        /// <summary>
        /// Credits to funkescott.
        /// </summary>
        /// <returns>Highest slope of surrounding terrain, returns 100 if the slope can't be determined</returns>
        private static float getHighestSurroundingSlope(WoWPoint p)
        {
            navlog("Navigation: Sloapcheck on Point: {0}", p);
            float _PIx2 = 3.14159f * 2f;
            float highestSlope = -100;
            float slope = 0;
            int traceStep = 15;
            float range = 0.5f;
            WoWPoint p2;
            for (int i = 0; i < traceStep; i++)
            {
                p2 = p.RayCast((i * _PIx2) / traceStep, range);
                p2.Z = getGroundZ(p2);
                slope = Math.Abs(getSlope(p, p2));
                if (slope > highestSlope)
                {
                    highestSlope = (float)slope;
                }
            }
            navlog(" - Highslope {0}", highestSlope);
            return Math.Abs(highestSlope);
        }

        /// <summary>
        /// Credits to funkescott.
        /// </summary>
        /// <param name="p1">from WoWPoint</param>
        /// <param name="p2">to WoWPoint</param>
        /// <returns>Return slope from WoWPoint to WoWPoint.</returns>
        private static float getSlope(WoWPoint p1, WoWPoint p2)
        {
            float rise = p2.Z - p1.Z;
            float run = (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

            return rise / run;
        }
        #endregion

        #endregion

        #region Behemoth / Cracklefang -Behavior
        /// <summary>
        /// this behavior will move the bot StrafeRight/StrafeLeft only if enemy is casting and we needed to move!
        /// Credits to BarryDurex.
        /// </summary>
        /// <param name="EnemyAttackRadius">EnemyAttackRadius or 0 for move Behind</param>
        public static void AvoidEnemyCast(WoWUnit Unit, float EnemyAttackRadius, float SaveDistance)
        {
            if (!StyxWoW.Me.IsFacing(Unit))
            { Unit.Face(); Thread.Sleep(300); }

            float BehemothRotation = getPositive(Unit.RotationDegrees);
            float invertEnemyRotation = getInvert(BehemothRotation);

            WoWMovement.MovementDirection move = WoWMovement.MovementDirection.None;

            if (getPositive(StyxWoW.Me.RotationDegrees) > invertEnemyRotation)
            { move = WoWMovement.MovementDirection.StrafeRight; }
            else
            { move = WoWMovement.MovementDirection.StrafeLeft; }

            while (Unit.Distance2D <= SaveDistance && Unit.IsCasting && ((EnemyAttackRadius == 0 && !StyxWoW.Me.IsSafelyBehind(Unit)) ||
                (EnemyAttackRadius != 0 && Unit.IsSafelyFacing(StyxWoW.Me, EnemyAttackRadius)) || Unit.Distance2D <= 2 ))
            {
                WoWMovement.Move(move);
                Unit.Face();           
            }
            WoWMovement.MoveStop();
        }


        private static float getInvert(float f)
        {
            if (f < 180)
                return (f + 180);
            //else if (f >= 180)
            return (f - 180);
        }

        private static float getPositive(float f)
        {
            if (f < 0)
                return (f + 360);
            return f;
        }
        #endregion




    }
}