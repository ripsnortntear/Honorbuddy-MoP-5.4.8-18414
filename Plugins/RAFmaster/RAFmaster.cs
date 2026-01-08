namespace RAFmaster2
{
    using Styx;
    using Styx.Common;
    using Styx.CommonBot;
    using Styx.CommonBot.Frames;
    using Styx.CommonBot.POI;
    using Styx.Helpers;
    using Styx.Plugins;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Media;

    internal class RAFmaster : HBPlugin
    {
        public List<string> completedQuests = new List<string>();
        public string curQuestID;
        public byte[] data;
        public string decodedName;
        private bool initDone;
        private bool isDisposed;
        public string LastCheckedQuest;
        public string LastCompQuest;
        public string LastQuestInGText;
        public static string MyRealm = Me.RealmName.Replace(" ", string.Empty).Replace("-", string.Empty);
        public Stopwatch PartyTimer = new Stopwatch();
        private string prevLog = "";
        private string prevTarget = "";
        public Stopwatch QCheckTimer = new Stopwatch();
        public int TimesChecked;

        public void CheckQuest(string QuestID)
        {
            this.CloseFrames();
            if (this.LastCheckedQuest != null)
            {
                if ((this.LastCheckedQuest == QuestID) && (this.TimesChecked > 2))
                {
                    return;
                }
                if (this.LastCheckedQuest != QuestID)
                {
                    this.TimesChecked = 0;
                }
            }
            if (!File.Exists(this.MyPath))
            {
                this.Log("Sending quest check request.");
                File.WriteAllText(this.MyPath, QuestID);
            }
            else if ((!File.ReadAllText(this.MyPath).Contains(QuestID) && !File.ReadAllText(this.MyPath).Contains("true")) && !File.ReadAllText(this.MyPath).Contains("false"))
            {
                this.Log("Quest ID in file does not match with quest ID I am turning.");
                File.WriteAllText(this.MyPath, QuestID);
            }
            else
            {
                if (this.IsFileBusy(this.MyPath))
                {
                    Thread.Sleep(500);
                }
                string str = File.ReadAllText(this.MyPath);
                if (str.Contains("true"))
                {
                    this.Log("Quest is completed by teammate.");
                    this.completedQuests.Add(QuestID);
                    File.Delete(this.MyPath);
                    this.TimesChecked++;
                    this.LastCheckedQuest = QuestID;
                }
                else if (str.Contains("false"))
                {
                    this.Log("Quest is NOT completed by teammate.");
                    File.Delete(this.MyPath);
                    this.TimesChecked++;
                    this.LastCheckedQuest = QuestID;
                }
            }
        }

        public void CloseFrames()
        {
            if (this.QTurn && ((QuestFrame.Instance.IsVisible || (QuestFrame.Instance != null)) || (GossipFrame.Instance.IsVisible || (GossipFrame.Instance != null))))
            {
                Me.ClearTarget();
                QuestFrame.Instance.Close();
                GossipFrame.Instance.Close();
            }
        }

        private void dispose()
        {
            this.Log("Disposed.");
            this.isDisposed = true;
        }

        public override void Dispose()
        {
            if (!this.isDisposed)
            {
                this.dispose();
            }
        }

        private void GuildInvite(object sender, LuaEventArgs e)
        {
            this.Log("We're invited to a guild!");
            if (Convert.ToInt32(e.Args[2]) >= 20)
            {
                this.Log("And we're joining it!");
                Lua.DoString("AcceptGuild()", "WoW.lua");
                Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/click GuildInviteFrameJoinButton"), "WoW.lua");
            }
            else
            {
                this.Log("But the level is too low :C declining the invite.");
                Lua.DoString("DeclineGuild()", "WoW.lua");
                Lua.DoString(string.Format("RunMacroText(\"{0}\")", "/click GuildInviteFrameDeclineButton"), "WoW.lua");
            }
            Lua.DoString("StaticPopup_Hide(\"GUILD_INVITE_REQUEST\")", "WoW.lua");
        }

        public bool HasQuest(uint QuestId)
        {
            return (Me.QuestLog.GetQuestById(QuestId) != null);
        }

        private void init()
        {
            this.Log("Initialized.");
            Lua.Events.AttachEvent("GUILD_INVITE_REQUEST", new LuaEventHandlerDelegate(this.GuildInvite));
            this.data = Encoding.Default.GetBytes(Me.Name);
            this.decodedName = Encoding.UTF8.GetString(this.data);
            if (QuestFrame.Instance.IsVisible || (QuestFrame.Instance != null))
            {
                QuestFrame.Instance.Close();
                GossipFrame.Instance.Close();
            }
            if (!Directory.Exists(Application.StartupPath + @"\RAF\"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\RAF\");
            }
        }

        public override void Initialize()
        {
            if (!this.initDone)
            {
                this.init();
            }
        }

        public bool IsFileBusy(string filepath)
        {
            bool flag;
            try
            {
                using (new FileStream(filepath, FileMode.Open))
                {
                    flag = false;
                }
            }
            catch
            {
                this.Log("File is busy.");
                flag = true;
            }
            return flag;
        }

        private void Log(string argument)
        {
            if (argument != this.prevLog)
            {
                Logging.Write(Colors.Lime, "[{0}] {1}", new object[] { this.Name, argument });
                this.prevLog = argument;
            }
        }

        private void Log(string argument, string target)
        {
            if (target != null)
            {
                if ((this.prevLog != argument) && (this.prevTarget != target))
                {
                    Logging.Write(Colors.Lime, "[{0}] {1} {2}", new object[] { this.Name, argument, target });
                    this.prevLog = argument;
                    this.prevTarget = target;
                }
            }
            else if (argument != this.prevLog)
            {
                Logging.Write(Colors.Lime, "[{0}] {1}", new object[] { this.Name, argument });
                this.prevLog = argument;
            }
        }

        public override void Pulse()
        {
            try
            {
                while ((!Me.GroupInfo.IsInParty || (Me.GroupInfo.PartySize < 2)) && ((!Me.Combat && !Me.IsDead) && !Me.IsGhost))
                {
                    if (!this.PartyTimer.IsRunning)
                    {
                        this.PartyTimer.Restart();
                        return;
                    }
                    if (this.PartyTimer.ElapsedMilliseconds < 0x1388L)
                    {
                        WoWMovement.MoveStop();
                        BotPoi.Clear();
                        return;
                    }
                    this.Log("Sending and accepting group invites in 5 seconds.");
                    Lua.DoString("RunMacroText(\"/run local name = GetFriendInfo(1); if (name) then InviteUnit(name) end\");", "WoW.lua");
                    Lua.DoString("RunMacroText(\"/run local name = GetFriendInfo(2); if (name) then InviteUnit(name) end\");", "WoW.lua");
                    Lua.DoString("RunMacroText(\"/run local a,b,c,d,e,f = BNGetFriendInfo(1); local name = e; local toonID = f; local a,b,c,d,e = BNGetToonInfo(toonID); local realm = d; local fullname = name .. '-' .. realm; InviteUnit(fullname)\");", "WoW.lua");
                    Lua.DoString("RunMacroText(\"/run local a,b,c,d,e,f = BNGetFriendInfo(2); local name = e; local toonID = f; local a,b,c,d,e = BNGetToonInfo(toonID); local realm = d; local fullname = name .. '-' .. realm; InviteUnit(fullname)\");", "WoW.lua");
                    Lua.DoString("RunMacroText(\"/click StaticPopup1Button1\");", "WoW.lua");
                    this.PartyTimer.Restart();
                    this.CloseFrames();
                }
                if (Me.GroupInfo.IsInParty && this.PartyTimer.IsRunning)
                {
                    this.PartyTimer.Reset();
                }
                if (Me.IsAFKFlagged)
                {
                    KeyboardManager.PressKey(' ');
                    Thread.Sleep(50);
                    KeyboardManager.ReleaseKey(' ');
                }
                if (this.CurGoalText.Contains("Turning") && (this.CurGoalText != this.LastQuestInGText))
                {
                    this.LastQuestInGText = this.CurGoalText;
                    this.curQuestID = Regex.Match(this.CurGoalText, @"\d+").Value;
                    File.WriteAllText(this.MyQuestPath, this.curQuestID);
                    this.Log("Current quest id is : " + this.curQuestID);
                }
                if ((File.Exists(this.PPath) && !File.ReadAllText(this.PPath).Contains("true")) && !File.ReadAllText(this.PPath).Contains("false"))
                {
                    this.RespondCheck();
                }
                if (((((BotPoi.Current.Location.Distance(Me.Location) < this.QTRange) && this.QTurn) && ((Me.ZoneId != 0x5ef) && (Me.ZoneId != 0x665))) && ((this.LastCompQuest != this.LastQuestInGText) && !this.qIsDone(this.curQuestID))) && this.CanContinue)
                {
                    this.LastCompQuest = this.LastQuestInGText;
                    File.Delete(this.MyPath);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteException(exception);
                throw;
            }
        }

        public bool qIsDone(string qid)
        {
            return this.completedQuests.Contains(qid);
        }

        public void RespondCheck()
        {
            this.CloseFrames();
            if (this.IsFileBusy(this.PPath))
            {
                Thread.Sleep(500);
            }
            if (!File.ReadAllText(this.PPath).Contains("true") && !File.ReadAllText(this.PPath).Contains("false"))
            {
                uint num = Convert.ToUInt32(File.ReadAllText(this.PPath));
                if (Me.QuestLog.GetCompletedQuests().Contains(num))
                {
                    File.WriteAllText(this.PPath, "true");
                    this.Log("I have completed quest with id: " + num);
                }
                else
                {
                    File.WriteAllText(this.PPath, "false");
                    this.Log("I have NOT completed quest with id: " + num);
                }
                StyxWoW.SleepForLagDuration();
            }
        }

        public bool AtQuestGiver
        {
            get
            {
                ObjectManager.Update();
                StyxWoW.SleepForLagDuration();
                if (((this.Player.Count >= (Me.GroupInfo.PartySize - 1)) && !Me.Combat) && this.QTurn)
                {
                    if (this.QuestGiver.Count == 0)
                    {
                        for (int i = 0; i < this.Player.Count; i++)
                        {
                            if (this.Player[i].Location.Distance(BotPoi.Current.Location) < this.QTRange)
                            {
                                return true;
                            }
                        }
                    }
                    if (this.QuestGiver.Count > 0)
                    {
                        for (int j = 0; j < this.QuestGiver.Count; j++)
                        {
                            if (this.Player[0].Location.Distance(this.QuestGiver[j].Location) <= this.QTRange)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        public override string Author
        {
            get
            {
                return "Infern1k";
            }
        }

        public override string ButtonText
        {
            get
            {
                return "-_-";
            }
        }

        public bool CanContinue
        {
            get
            {
                if (!this.AtQuestGiver)
                {
                    this.Log("Party members are too far.");
                    this.CloseFrames();
                    BotPoi.Clear();
                    if (!this.QCheckTimer.IsRunning)
                    {
                        this.QCheckTimer.Restart();
                        this.CheckQuest(this.curQuestID);
                    }
                    if (this.QCheckTimer.IsRunning && (this.QCheckTimer.ElapsedMilliseconds > 0x2710L))
                    {
                        this.CheckQuest(this.curQuestID);
                        this.QCheckTimer.Restart();
                    }
                    return false;
                }
                if (!File.Exists(this.MyQuestPath))
                {
                    this.CloseFrames();
                    File.WriteAllText(this.MyQuestPath, this.curQuestID);
                    return false;
                }
                if (!File.Exists(this.PQuestPath))
                {
                    this.CloseFrames();
                    return false;
                }
                if (File.Exists(this.MyQuestPath) && File.Exists(this.PQuestPath))
                {
                    string str = File.ReadAllText(this.MyQuestPath);
                    string str2 = File.ReadAllText(this.PQuestPath);
                    if (str != this.curQuestID)
                    {
                        this.CloseFrames();
                        File.WriteAllText(this.MyQuestPath, this.curQuestID);
                        return false;
                    }
                    if (str == str2)
                    {
                        this.Log("Quests synchronized, we can continue!");
                        return true;
                    }
                }
                return false;
            }
        }

        public string CurGoalText
        {
            get
            {
                if (TreeRoot.GoalText == null)
                {
                    return "";
                }
                return TreeRoot.GoalText;
            }
        }

        private static LocalPlayer Me
        {
            get
            {
                return StyxWoW.Me;
            }
        }

        public string MyPath
        {
            get
            {
                return (Application.StartupPath + @"\RAF\" + Me.Name + "-" + MyRealm + ".txt");
            }
        }

        public string MyQuestPath
        {
            get
            {
                return (Application.StartupPath + @"\RAF\" + Me.Name + "-" + MyRealm + "(quest).txt");
            }
        }

        public override string Name
        {
            get
            {
                return "RAFmaster";
            }
        }

        public List<WoWPlayer> Player
        {
            get
            {
                return (from q in ObjectManager.GetObjectsOfType<WoWPlayer>()
                    where (((!q.IsMe && q.IsInMyPartyOrRaid) && (q.IsValid && !q.IsGhost)) && !q.IsDead) && !q.Combat
                    select q).ToList<WoWPlayer>();
            }
        }

        public static string PlayerName
        {
            get
            {
                for (int i = 1; i <= Me.GroupInfo.PartySize; i++)
                {
                    string returnVal = Lua.GetReturnVal<string>("return GetRaidRosterInfo(" + i + ")", 0);
                    if ((returnVal != Me.Name) && (returnVal != null))
                    {
                        if (returnVal.Contains("-"))
                        {
                            return returnVal;
                        }
                        return (returnVal + "-" + MyRealm);
                    }
                }
                return string.Empty;
            }
        }

        public string PPath
        {
            get
            {
                return (Application.StartupPath + @"\RAF\" + PlayerName + ".txt");
            }
        }

        public string PQuestPath
        {
            get
            {
                return (Application.StartupPath + @"\RAF\" + PlayerName + "(quest).txt");
            }
        }

        public string qID
        {
            get
            {
                return Regex.Match(this.CurGoalText, @"\d+\").Value;
            }
        }

        public float QTRange
        {
            get
            {
                if (this.QuestGiver.Count != 0)
                {
                    return (this.QuestGiver[0].InteractRange + 1f);
                }
                return 20f;
            }
        }

        public bool QTurn
        {
            get
            {
                return (BotPoi.Current.Type == PoiType.QuestTurnIn);
            }
        }

        public List<WoWUnit> QuestGiver
        {
            get
            {
                return (from s in ObjectManager.GetObjectsOfType<WoWUnit>().Where<WoWUnit>(delegate (WoWUnit p) {
                    if ((p.QuestGiverStatus != QuestGiverStatus.TurnIn) && (p.QuestGiverStatus != QuestGiverStatus.TurnInRepeatable))
                    {
                        return false;
                    }
                    return p.IsAlive;
                })
                    orderby s.Distance
                    select s).ToList<WoWUnit>();
            }
        }

        public override bool WantButton
        {
            get
            {
                return false;
            }
        }

        public override System.Version Version
        {
            get
            {
                return new System.Version(1, 0, 0);
            }
        }
    }
}

