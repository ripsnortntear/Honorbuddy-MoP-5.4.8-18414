using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.Plugins;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Serialization;
using Action = Styx.TreeSharp.Action;
using Sequence = Styx.TreeSharp.Sequence;

namespace EpicJunkbox
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbEpic = new System.Windows.Forms.CheckBox();
            this.cbBlue = new System.Windows.Forms.CheckBox();
            this.cbGreen = new System.Windows.Forms.CheckBox();
            this.cbWhite = new System.Windows.Forms.CheckBox();
            this.cbGrey = new System.Windows.Forms.CheckBox();
            this.cbUnknown = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cbMoney = new System.Windows.Forms.CheckBox();
            this.clrBlkLstBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbEpic
            // 
            this.cbEpic.AutoSize = true;
            this.cbEpic.Location = new System.Drawing.Point(12, 12);
            this.cbEpic.Name = "cbEpic";
            this.cbEpic.Size = new System.Drawing.Size(76, 17);
            this.cbEpic.TabIndex = 0;
            this.cbEpic.Text = "Loot Epics";
            this.cbEpic.UseVisualStyleBackColor = true;
            // 
            // cbBlue
            // 
            this.cbBlue.AutoSize = true;
            this.cbBlue.Location = new System.Drawing.Point(12, 35);
            this.cbBlue.Name = "cbBlue";
            this.cbBlue.Size = new System.Drawing.Size(76, 17);
            this.cbBlue.TabIndex = 0;
            this.cbBlue.Text = "Loot Blues";
            this.cbBlue.UseVisualStyleBackColor = true;
            // 
            // cbGreen
            // 
            this.cbGreen.AutoSize = true;
            this.cbGreen.Location = new System.Drawing.Point(12, 58);
            this.cbGreen.Name = "cbGreen";
            this.cbGreen.Size = new System.Drawing.Size(84, 17);
            this.cbGreen.TabIndex = 0;
            this.cbGreen.Text = "Loot Greens";
            this.cbGreen.UseVisualStyleBackColor = true;
            // 
            // cbWhite
            // 
            this.cbWhite.AutoSize = true;
            this.cbWhite.Location = new System.Drawing.Point(117, 12);
            this.cbWhite.Name = "cbWhite";
            this.cbWhite.Size = new System.Drawing.Size(83, 17);
            this.cbWhite.TabIndex = 0;
            this.cbWhite.Text = "Loot Whites";
            this.cbWhite.UseVisualStyleBackColor = true;
            // 
            // cbGrey
            // 
            this.cbGrey.AutoSize = true;
            this.cbGrey.Location = new System.Drawing.Point(117, 35);
            this.cbGrey.Name = "cbGrey";
            this.cbGrey.Size = new System.Drawing.Size(77, 17);
            this.cbGrey.TabIndex = 0;
            this.cbGrey.Text = "Loot Greys";
            this.cbGrey.UseVisualStyleBackColor = true;
            // 
            // cbUnknown
            // 
            this.cbUnknown.AutoSize = true;
            this.cbUnknown.Location = new System.Drawing.Point(117, 58);
            this.cbUnknown.Name = "cbUnknown";
            this.cbUnknown.Size = new System.Drawing.Size(96, 17);
            this.cbUnknown.TabIndex = 0;
            this.cbUnknown.Text = "Loot Unknown";
            this.cbUnknown.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(152, 114);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(61, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cbMoney
            // 
            this.cbMoney.AutoSize = true;
            this.cbMoney.Location = new System.Drawing.Point(12, 81);
            this.cbMoney.Name = "cbMoney";
            this.cbMoney.Size = new System.Drawing.Size(82, 17);
            this.cbMoney.TabIndex = 2;
            this.cbMoney.Text = "Loot Money";
            this.cbMoney.UseVisualStyleBackColor = true;
            // 
            // clrBlkLstBtn
            // 
            this.clrBlkLstBtn.Location = new System.Drawing.Point(12, 114);
            this.clrBlkLstBtn.Name = "clrBlkLstBtn";
            this.clrBlkLstBtn.Size = new System.Drawing.Size(84, 23);
            this.clrBlkLstBtn.TabIndex = 3;
            this.clrBlkLstBtn.Text = "Clear Blacklist";
            this.clrBlkLstBtn.UseVisualStyleBackColor = true;
            this.clrBlkLstBtn.Click += new System.EventHandler(this.clrBlkLstBtn_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 149);
            this.Controls.Add(this.clrBlkLstBtn);
            this.Controls.Add(this.cbMoney);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cbUnknown);
            this.Controls.Add(this.cbGrey);
            this.Controls.Add(this.cbWhite);
            this.Controls.Add(this.cbGreen);
            this.Controls.Add(this.cbBlue);
            this.Controls.Add(this.cbEpic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "EpicJunkbox";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button saveButton;
        private CheckBox cbEpic;
        private CheckBox cbBlue;
        private CheckBox cbGreen;
        private CheckBox cbWhite;
        private CheckBox cbGrey;
        private CheckBox cbUnknown;
        private CheckBox cbMoney;
        private Button clrBlkLstBtn;

        public static MainForm Instance;

        public MainForm()
        {
            Instance = this;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cbEpic.Checked = Settings.Instance.lootEpic;
            cbBlue.Checked = Settings.Instance.lootBlue;
            cbGreen.Checked = Settings.Instance.lootGreen;
            cbWhite.Checked = Settings.Instance.lootWhite;
            cbGrey.Checked = Settings.Instance.lootGrey;
            cbUnknown.Checked = Settings.Instance.lootUnknown;
            cbMoney.Checked = Settings.Instance.lootMoney;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Settings.Instance.lootEpic = cbEpic.Checked;
            Settings.Instance.lootBlue = cbBlue.Checked;
            Settings.Instance.lootGreen = cbGreen.Checked;
            Settings.Instance.lootWhite = cbWhite.Checked;
            Settings.Instance.lootGrey = cbGrey.Checked;
            Settings.Instance.lootUnknown = cbUnknown.Checked;
            Settings.Instance.lootMoney = cbMoney.Checked;
            Settings.Instance.Save();
            //this.Close();
        }

        private void clrBlkLstBtn_Click(object sender, EventArgs e)
        {
            EpicJunkbox.LockBoxBlackList.Clear();
        }

    }

    [Serializable]
    public class Settings
    {
        private static Settings instance;

        public static Settings Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = Load();
                }

                return instance;
            }
        }

        #region Settings Serialization

        public static string ConfigFileFormat = "EpicJunkbox_{0}.config";

        public static string ConfigFile
        {
            get { return string.Format(ConfigFileFormat, StyxWoW.Me.Name); }
        }

        public static string SavePath
        {
            get
            {
                string path = Process.GetCurrentProcess().MainModule.FileName;
                path = Path.GetDirectoryName(path);
                path = Path.Combine(path, @"Plugins\EpicJunkbox\settings");
                return path;
            }
        }

        private static XmlSerializer serializer;

        private static XmlSerializer Serializer
        {
            get
            {
                if (null == serializer)
                {
                    serializer = new XmlSerializer(typeof(Settings));
                }

                return serializer;
            }
        }

        public static Settings Load()
        {
            string path = SavePath;
            string file = Path.Combine(path, ConfigFile);

            try
            {
                using (FileStream fStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    return (Settings)Serializer.Deserialize(fStream);
                }
            }
            catch
            {
                return new Settings();
            }
        }

        public void Save()
        {
            string path = SavePath;
            string file = Path.Combine(path, ConfigFile);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                using (FileStream fStream = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    Serializer.Serialize(fStream, this);
                }
            }
            catch (Exception e)
            {
                Logging.Write(Colors.Red, "Error saving EpicJunkbox settings");
                Logging.WriteException(Colors.Red, e);
            }
        }

        #endregion

        public bool lootEpic = true;
        public bool lootBlue = true;
        public bool lootGreen = true;
        public bool lootWhite = false;
        public bool lootGrey = false;
        public bool lootUnknown = false;
        public bool lootMoney = false;
    }

    public class EpicJunkbox : HBPlugin
    {
        public override string Name { get { return "EpicJunkbox"; } }
        public override string Author { get { return "eracer"; } }
        public override Version Version { get { return _version; } }
        private readonly Version _version = new Version(3, 2, 3, 0);
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Settings"; } }
        public MainForm Gui = new MainForm();

        private static Stopwatch sw = new Stopwatch();
        //private readonly Styx.Common.Helpers.WaitTimer _updateTimer = Styx.Common.Helpers.WaitTimer.ThirtySeconds;
        public static List<ulong> LockBoxBlackList = new List<ulong>();
        //public class LockBoxItem
        //{
        //    public uint Entry { get; set; }
        //    public uint Level { get; set; }
        //}
        //public static List<LockBoxItem> LockBoxList = new List<LockBoxItem>();
        private static bool LootFrameIsOpen { get; set; }
        private Composite _root;
        private WoWItem item;


        private static Dictionary<uint, uint> LockBoxesList = new Dictionary<uint, uint>()
        {
	    {16883, 70}, // Worn Junkbox (70)
	    {16884, 175}, // Sturdy Junkbox (175)
	    {16885, 250}, // Heavy Junkbox (250)
        {16882, 1}, // Battered Junkbox (1)
        {29569, 300}, // Strong Junkbox (300)
        {31952, 325}, // Khorium Lockbox (325)
        {43575, 350}, // Reinforced Junkbox (350)
        {43622, 375}, // Froststeel Lockbox (375)
        {43624, 400}, // Titanium Lockbox (400)
        {45986, 400}, // Tiny Titanium Lockbox (400)
        {4632, 1}, // Ornate Bronze Lockbox (1)
        {4633, 25}, // Heavy Bronze Lockbox (25)
        {4634, 70}, // Iron Lockbox (70)
        {4636, 125}, // Strong Iron Lockbox (125)
        {4637, 175}, // Steel Lockbox (175)
        {4638, 225}, // Reinforced Steel Lockbox (225)
        {5758, 225},  // Mithril Lockbox (225)
        {5759, 225},  // Thorium Lockbox (225)
        {5760, 225},  // Eternium Lockbox (225)
        {63349, 400}, // Flame-Scarred Junkbox (400)
        {68729, 425}, // Elementium Lockbox (425)
        {88165, 450}, // Vine-Cracked Junkbox (450)
        {88567, 450} // Ghost Iron Lockbox (450)
        };

        private static Dictionary<uint, uint> SkeletonKeysList = new Dictionary<uint, uint>()
        {
        {15869, 100}, // Silver Skeleton Key (100)
	    {15870, 150}, // Golden Skeleton Key (150)
	    {15871, 200}, // Truesilver Skeleton Key (200)
	    {15872, 275}, // Arcanite Skeleton Key (275)
        {43854, 375}, // Cobalt Skeleton Key (375)
        {43853, 400}, // Titanium Skeleton Key (400)
        {55053, 425}, // Obsidium Skeleton Key (425)
        {82960, 450}, // Ghostly Skeleton Key (450)
        {82354, 450} // Ghost Iron Key (450)
        };

        private static bool lootEpic
        {
            get { return Settings.Instance.lootEpic; }
        }
        private static bool lootBlue
        {
            get { return Settings.Instance.lootBlue; }
        }
        private static bool lootGreen
        {
            get { return Settings.Instance.lootGreen; }
        }
        private static bool lootWhite
        {
            get { return Settings.Instance.lootWhite; }
        }
        private static bool lootGrey
        {
            get { return Settings.Instance.lootGrey; }
        }
        private static bool lootUnknown
        {
            get { return Settings.Instance.lootUnknown; }
        }
        private static bool lootMoney
        {
            get { return Settings.Instance.lootMoney; }
        }

        public override void OnButtonPress()
        {
            Gui.ShowDialog();
        }

        private bool reEnableAutolootSetting = false;

        private string AutolootSetting
        {
            get { return Lua.GetReturnVal<string>(string.Format("return GetCVar(\"autoLootDefault\")"), 0); }
        }

        public override void Initialize()
        {
            Lua.Events.AttachEvent("LOOT_OPENED", LootFrameOpenedHandler);
            Lua.Events.AttachEvent("LOOT_CLOSED", LootFrameClosedHandler);
            //disableAutoloot();
            //Styx.CommonBot.BotEvents.OnBotStop += botStop;
            //Styx.CommonBot.BotEvents.OnBotStart += botStart;
            base.Initialize();
        }

        //private void botStart(System.EventArgs args)
        //{
        //    disableAutoloot();
        //}

        //private void botStop(System.EventArgs args)
        //{
        //    restoreAutoloot();
        //}

        public override void Dispose()
        {
            Lua.Events.DetachEvent("LOOT_OPENED", LootFrameOpenedHandler);
            Lua.Events.DetachEvent("LOOT_CLOSED", LootFrameClosedHandler);
            Settings.Instance.Save();
            //restoreAutoloot();
            base.Dispose();
        }

        private void LootFrameClosedHandler(object sender, LuaEventArgs args) { LootFrameIsOpen = false; }

        private void LootFrameOpenedHandler(object sender, LuaEventArgs args) { LootFrameIsOpen = true; }

        public override void Pulse()
        {
            if (sw.Elapsed.TotalSeconds > 30 || !sw.IsRunning)
            {
                if (StyxWoW.Me.Combat ||
                    StyxWoW.Me.IsCasting ||
                    StyxWoW.Me.Mounted ||
                    StyxWoW.Me.IsDead ||
                    StyxWoW.Me.HasAura("Food") ||
                    StyxWoW.Me.HasAura("Drink") ||
                    Battlegrounds.IsInsideBattleground ||
                    StyxWoW.Me.HasAura(1784) || // Stealth
                    StyxWoW.Me.IsMoving) return;

                if (LockBoxItems.Count() == 0) // if we still don't have an item then start the 30 second timer
                {
                    restoreAutoloot();
                    sw.Reset();
                    sw.Start();
                    return;
                }
                else
                {
                    item = LockBoxItems.FirstOrDefault();
                    if (!disableAutoloot())
                    {
                        if (_root == null)
                            _root = CheckInventoryItems();
                        Tick(_root);
                    }
                }
            }
        }

        private static void Tick(Composite tree)
        {
            if (tree.LastStatus != RunStatus.Running)
                tree.Start(null);

            if (tree.Tick(null) != RunStatus.Running)
                tree.Stop(null);
        }

        private void log(String fmt, params object[] args)
        {
            String s = String.Format(fmt, args);
            log(Colors.DodgerBlue, fmt, args);
        }

        private void log(System.Windows.Media.Color color, String fmt, params object[] args)
        {
            String s = String.Format(fmt, args);
            Logging.Write(color, String.Format("[{0}]: {1}", Name, s));
        }

        private bool disableAutoloot()
        {
            if (AutolootSetting.Equals("1")) // only turn it off if it was on
            {
                reEnableAutolootSetting = true;
                Lua.DoString("SetCVar(\"autoLootDefault\", \"0\");");
                log("Disabled \"Auto Loot\".");
                return true;
            }
            return false;
        }

        private void restoreAutoloot()
        {
            if (reEnableAutolootSetting == true) // only turn it back on if we disabled it
            {
                Lua.DoString("SetCVar(\"autoLootDefault\", \"1\");");
                log("Enabled \"Auto Loot\".");
                reEnableAutolootSetting = false;
            }
        }

        public static LocalPlayer Me
        {
            get
            {
                return StyxWoW.Me;
            }
        }

        public Composite CheckInventoryItems()
        {
            return new PrioritySelector(
                new Decorator(ret => !item.IsOpenable,
                    new Sequence(

                        // Unlock item
                        new Action(delegate { UnlockBox(item); log("Unlocking {0} guid {1}", item.Name, item.Guid); }),

                        //// Wait for casting to start...
                //new WaitContinue(5, ret => Me.IsCasting, new ActionAlwaysSucceed()),

                        //// Wait for casting to complete...
                //new WaitContinue(10, ret => !Me.IsCasting, new ActionAlwaysSucceed()),

                        // Wait for up to 12 seconds for box to become openable...
                        new WaitContinue(12, ret => item.IsOpenable, new ActionAlwaysSucceed()),

                        // since its still not openable blacklist it or we will keep trying to unlock it
                        new Action(delegate { if (!item.IsOpenable) LockBoxBlackList.Add(item.Guid); })
                    )
                ),
                new Decorator(ret => item.IsOpenable,
                    new Sequence(

                        // Open the box
                        new Action(delegate { UseItem(item); log("Looting {0} guid {1}", item.Name, item.Guid); }),

                        // Wait for the loot frame to appear...
                        new WaitContinue(10, ret => LootFrameIsOpen, new ActionAlwaysSucceed()),

                        // Get the loot from the box...
                        new Action(delegate { GetLoot(); }),

                        // Wait for the loot frame to disappear...
                        new WaitContinue(10, ret => !LootFrameIsOpen, new ActionAlwaysSucceed()),

                        // Blacklist the box...
                        new Action(delegate { LockBoxBlackList.Add(item.Guid); })

                    )
                )
            );
        }

        private void GetLoot()
        {
            //log("Looting...");
            for (int i = 0; LootFrameIsOpen && i < LootFrame.Instance.LootItems; i++)
            {
                var lootInfo = LootFrame.Instance.LootInfo(i);
                if ((lootEpic && lootInfo.LootRarity == LootRarity.Epic)
                    || (lootBlue && lootInfo.LootRarity == LootRarity.Blue)
                    || (lootGreen && lootInfo.LootRarity == LootRarity.Green)
                    || (lootWhite && lootInfo.LootRarity == LootRarity.White && lootInfo.LootQuantity > 0)
                    || (lootGrey && lootInfo.LootRarity == LootRarity.Grey && lootInfo.LootQuantity > 0)
                    || (lootUnknown && lootInfo.LootRarity == LootRarity.Unknown)
                    || (lootMoney && lootInfo.LootQuantity == 0)
                    )
                    LootFrame.Instance.Loot(i);
            }
            LootFrame.Instance.Close();
        }

        private void UnlockBox(WoWItem item)
        {
            if (SpellManager.HasSpell(1804) && LockBoxesList[item.Entry] <= (Me.Level*5)) // if box is not openable and we have the pick lock spell then unlock it
            {
                //log("Unlocking...");
                SpellManager.Cast(1804); // Pick Lock
                UseItem(item);
            }
            //else if (SkeletonKeys.Where(o => o.ItemInfo.Level >= item.ItemInfo.Level).Count() > 0)
            //{
            //    WoWItem key = SkeletonKeys.Where(o => o.ItemInfo.Level >= item.ItemInfo.Level).FirstOrDefault();
            else if ((SkeletonKeys.Where(o => SkeletonKeysList.ContainsKey(o.Entry) && SkeletonKeysList[o.Entry] >= LockBoxesList[item.Entry]).Count() > 0))
            {
                WoWItem key = SkeletonKeys.Where(o => SkeletonKeysList[o.Entry] >= LockBoxesList[item.Entry]).FirstOrDefault();
                if (key != null && key.Usable)
                {
                    UseItem(key);
                    UseItem(item);
                }
            }
            else
            {
                log("Can't Unlock {0} guid {1}", item.Name, item.Guid);
                LockBoxBlackList.Add(item.Guid);
            }

        }

        private bool CanUseItem(WoWItem item)
        {
            return item.Usable && item.Cooldown <= 0;
        }

        private void UseItem(WoWItem item)
        {
            //log("Using {0} guid {1}", item.Name, item.Guid);
            item.Use();
        }

        public static IEnumerable<WoWItem> SkeletonKeys
        {
            get
            {
                try
                {
                    var ret = StyxWoW.Me.BagItems.Where(o => SkeletonKeysList.ContainsKey(o.Entry));
                    return ret;
                }
                catch (NullReferenceException)
                {
                    return new List<WoWItem>();
                }
            }
        }

        public static IEnumerable<WoWItem> LockBoxItems
        {
            get
            {
                try
                {
                    var ret = StyxWoW.Me.BagItems.Where(o => !LockBoxBlackList.Contains(o.Guid) && LockBoxesList.ContainsKey(o.Entry));
                    return ret;
                }
                catch (NullReferenceException)
                {
                    return new List<WoWItem>();
                }
            }
        }
    }

    //public class Custom
    //{
    //    string item; //will hold the item
    //    System.Timers.Timer timer; //will hanlde the expiry
    //    List<Custom> refofMainList; //will be used to remove the item once it is expired

    //    public Custom(string yourItem, int milisec, List<Custom> refOfList)
    //    {
    //        refofMainList = refOfList;
    //        item = yourItem;
    //        timer = new System.Timers.Timer(milisec);
    //        timer.Elapsed += new ElapsedEventHandler(Elapsed_Event);
    //        timer.Start();
    //    }

    //    private void Elapsed_Event(object sender, ElapsedEventArgs e)
    //    {
    //        timer.Elapsed -= new ElapsedEventHandler(Elapsed_Event);
    //        refofMainList.Remove(this);

    //    }
    //}
}
