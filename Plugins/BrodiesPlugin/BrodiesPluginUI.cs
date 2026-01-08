using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.CommonBot.Profiles;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace BrodiesPlugin
{
    public partial class BrodiesPluginUI : Form
    {
        public int lastUseProfile = 0;
        public int numberBotBase;
        public string pathToCharSettings = Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Settings\Char" + StyxWoW.Me.Name + ".xml");
        public string pathToSettings = Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Settings\Main-Settings.xml");
        public string pathToProfiles = Path.Combine(Utilities.AssemblyDirectory + @"\Default Profiles\TBMC\");
        public string profileToLoad = "";
        bool isRunningdepois;
        bool BeenInitialized4;

        public BrodiesPluginUI()
        {
            InitializeComponent();
        }

        private void BrodiesPluginUI_Load(object sender, EventArgs e)
        {
			UpdateStuff(); // Load Saved Settings
			string Folder = "Plugins\\BrodiesPlugin\\Settings\\";
			string dailyFolder = "Default Profiles\\TBMC\\Reputation\\TMOPDE\\";
			string sPath = Process.GetCurrentProcess().MainModule.FileName;
			sPath = Path.GetDirectoryName(sPath);
			
			BPSettings.Instance.dailiesDirName = Path.Combine(sPath, dailyFolder);
			
			BPSettings.Instance.Profile1 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Golden Lotus Dailies [Brodie].xml");
			BPSettings.Instance.Profile2 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Tillers Dailies [Brodie].xml");
			BPSettings.Instance.Profile3 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Cloud Serpent Dailies [Brodie].xml");
			BPSettings.Instance.Profile4 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] The Anglers Dailies [Brodie].xml");
			BPSettings.Instance.Profile5 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Shieldwall [Brodie].xml");
			BPSettings.Instance.Profile6 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] The Klaxxi Dailies [Brodie].xml");
			BPSettings.Instance.Profile7 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Shado Pan Dailies [Brodie].xml");
			if (StyxWoW.Me.IsAlliance)
			{
				BPSettings.Instance.Profile8 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Isle of Thunder A [Brodie].xml");
				BPSettings.Instance.Profile9 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Isle of Thunder PvP A [Brodie].xml");
			}
			if (StyxWoW.Me.IsHorde)
			{
				BPSettings.Instance.Profile8 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Isle of Thunder H [Brodie].xml");
				BPSettings.Instance.Profile9 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Isle of Thunder PvP H [Brodie].xml");
			}
			BPSettings.Instance.Profile10 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] August Celestials Dailies [Brodie].xml");
			BPSettings.Instance.Profile11 = Path.Combine(BPSettings.Instance.dailiesDirName, "[Rep] Nat Pagle Dailies [TBMP].xml");
			// BPSettings.Instance.Profile12 = Path.Combine(BPSettings.Instance.dailiesDirName, "");
			
            // Init the Questing Tab
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton20.Checked = false;
            // Init the Grinding Tab
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            radioButton7.Checked = false;
            radioButton11.Checked = false;
            radioButton10.Checked = false;
            radioButton9.Checked = false;
            radioButton8.Checked = false;
            radioButton19.Checked = false;
            radioButton18.Checked = false;
            radioButton17.Checked = false;
            radioButton16.Checked = false;
            radioButton15.Checked = false;
            radioButton14.Checked = false;
            radioButton13.Checked = false;
            radioButton12.Checked = false;
            radioButton21.Checked = false;
			// Init the Dailies checkBoxes
			checkBox1.Checked = BPSettings.Instance.Active1;
            RecolorBoxes(checkBox1);
			checkBox2.Checked = BPSettings.Instance.Active2;
            RecolorBoxes(checkBox2);
			checkBox3.Checked = BPSettings.Instance.Active3;
            RecolorBoxes(checkBox3);
			checkBox4.Checked = BPSettings.Instance.Active4;
            RecolorBoxes(checkBox4);
			checkBox5.Checked = BPSettings.Instance.Active5;
            RecolorBoxes(checkBox5);
			checkBox6.Checked = BPSettings.Instance.Active6;
            RecolorBoxes(checkBox6);
			checkBox7.Checked = BPSettings.Instance.Active7;
            RecolorBoxes(checkBox7);
			checkBox8.Checked = BPSettings.Instance.Active8;
            RecolorBoxes(checkBox8);
			checkBox9.Checked = BPSettings.Instance.Active9;
            RecolorBoxes(checkBox9);
			checkBox10.Checked = BPSettings.Instance.Active10;
            RecolorBoxes(checkBox10);
			checkBox11.Checked = BPSettings.Instance.Active11;
            RecolorBoxes(checkBox11);
			checkBox12.Checked = BPSettings.Instance.Active12;
            RecolorBoxes(checkBox12);
            checkBox13.Checked = BPSettings.Instance.Active13;
            RecolorBoxes(checkBox13);
			checkBox1.Enabled = false;
			checkBox2.Enabled = false;
			checkBox3.Enabled = false;
			checkBox4.Enabled = false;
			checkBox5.Enabled = false;
			checkBox6.Enabled = false;
			checkBox7.Enabled = false;
			checkBox8.Enabled = false;
			checkBox9.Enabled = false;
			checkBox10.Enabled = false;
			checkBox11.Enabled = false;
            checkBox12.Enabled = false;

            if (checkBox13.Checked == true)
            {
                checkBox1.Enabled = true;
				checkBox2.Enabled = true;
				checkBox3.Enabled = true;
				checkBox4.Enabled = true;
				checkBox5.Enabled = true;
				checkBox6.Enabled = true;
				checkBox7.Enabled = true;
				checkBox8.Enabled = true;
				checkBox9.Enabled = true;
				checkBox10.Enabled = true;
				checkBox11.Enabled = true;
                //checkBox12.Enabled = true;
                checkBox13.BackColor = System.Drawing.Color.Blue;
            }
            else
                checkBox13.BackColor = System.Drawing.Color.Red;
			
			// Check for Alliance Shieldwall - Disable Dominance
			if (StyxWoW.Me.IsAlliance)
			{
				textBox5.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1376)); // Operation: Shieldwall
                textBox8.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1387)); // Kirin Tor Offensive
				textBox9.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1387)); // Kirin Tor Offensive
			}
			// Check for Horde Dominance - Disable Shieldwall
			if (StyxWoW.Me.IsHorde)
			{
				textBox5.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1375)); // Dominance Offensive
				textBox8.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1388)); // Sunreaver Onslaught
                textBox9.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1388)); // Sunreaver Onslaught
			}

			// Let's check Faction reps... why not make things easier? We are checking above for Ally / Horde only.
			textBox1.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1269)); // Golden Lotus
			textBox2.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1272)); // The Tillers
			textBox3.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1271)); // Cloud Serpent
			textBox4.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1302)); // The Anglers
			textBox6.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1337)); // The Klaxxi
			textBox7.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1270)); // Shado Pan
			textBox10.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1341)); // August Celestials
			textBox11.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1358)); // Nat Pagle
			textBox12.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1492)); // Emperor Shaohao

            this.tabPage1.BackgroundImage = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\WoW-Boss-Compilation-Wallpaper.jpg");
            this.checkBox1.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_faction_goldenlotus.jpg");
            this.checkBox2.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_faction_tillers.jpg");
            this.checkBox3.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_faction_serpentriders.jpg");
            this.checkBox4.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_faction_anglers.jpg");
            if (StyxWoW.Me.IsAlliance)
            {
                this.checkBox5.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\pvpcurrency-honor-alliance.jpg");
                this.checkBox8.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_reputation_kirintor_offensive.jpg");
                this.checkBox9.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_reputation_kirintor_offensive.jpg");
            }
            if (StyxWoW.Me.IsHorde)
            {
                this.checkBox5.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\pvpcurrency-honor-horde.jpg");
                this.checkBox8.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_faction_sunreaveronslaught.jpg");
                this.checkBox9.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_faction_sunreaveronslaught.jpg");
            }
            this.checkBox6.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_faction_klaxxi.jpg");
            this.checkBox7.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_faction_shadopan.jpg");
            this.checkBox10.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\achievement_faction_celestials.jpg");
            this.checkBox11.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\inv_helmet_50.jpg");
            this.checkBox12.Image = new Bitmap(Application.StartupPath + "\\Plugins\\BrodiesPlugin\\Images\\timelesscoin.jpg");

            if (StyxWoW.Me.IsAlliance)
            {
                this.cookComboBox1.Items.AddRange(new object[] {"Stormwind","Ironforge","Darnassus"});
                this.fishComboBox1.Items.AddRange(new object[] {"Stormwind","Ironforge","Darnassus"});
            }
            if (StyxWoW.Me.IsHorde)
            {
                this.cookComboBox1.Items.AddRange(new object[] { "Orgrimmar", "Thunder Bluff", "Undercity" });
                this.fishComboBox1.Items.AddRange(new object[] { "Orgrimmar", "Thunder Bluff", "Undercity" });
            }

            pictureBox1.ImageLocation = Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\donate.png");
            pictureBox2.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox3.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox4.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox5.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox6.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox7.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox8.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox9.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox10.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox11.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox12.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox13.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\Capture_CapableLevels.jpg"));
            pictureBox14.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\WorldmapAzeroth.jpg"));
            pictureBox15.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\outland_map.jpg"));
            pictureBox16.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\coollogo.png"));
            pictureBox17.BackgroundImage = new Bitmap(Path.Combine(Utilities.AssemblyDirectory + @"\Plugins\BrodiesPlugin\Images\Northrend.jpg"));

            if (StyxWoW.Me.IsAlliance)
            {
                this.radioButton36.Text = "Honor Hold";
                this.radioButton38.Text = "Kurenai";
            }
            if (StyxWoW.Me.IsHorde)
            {
                this.radioButton36.Text = "Thrallmar";
                this.radioButton38.Text = "Mag'har";
            }

            // Questing Tab Description, now static
            richTextBox1.Text = "This profile set will level-up any character from as low as level 1 up til level 90 (see above supported level range, faction may not support 90 at this time). \n Each race and faction combo has its own path. \n Profiles added and updated regularly.";
            // Noblegarden Rich Text Boxes
            richTextBox6.Text = "Chocoholic will grind for the Achievement Chocoholic by eating ALL of your chocolates in your inventory. It will stop when you either run out, or get the achievement. This profile will NOT farm for more chocolate, just eat it.";
            richTextBox7.Text = "This profile will head to one of the starting areas to farm for Easter Eggs. It is set to work in Stormwind/Elwynn Forest, and Orgrimmar/Razor Hill only. It will head to your area regardless of which continent you are on.";
            // Children's Week Setup
            richTextBox9.Text = "Please click one of the buttons shown to start the quest line for that areas Childrens Week quests. Please note that once an area becomes unavailable to you, or you complete it, you will no longer be able to click on it.";
            if (StyxWoW.Me.IsAlliance)
            {
                if (IsQuestComplete(171)) // Final quest of Stormwind Kid's Week Line
                {
                    this.button20.Enabled = false;
                    this.button20.Text = "Stormwind Complete"; // Children's Week Home Button
                }
                else
                {
                    this.button20.Enabled = true;
                    this.button20.Text = "Stormwind"; // Children's Week Home Button
                }
                if (IsQuestComplete(10966)) // Final quest of Outlands Kid's Week Line
                {
                    this.button21.Enabled = false;
                    this.button21.Text = "Shattrath Complete"; // Children's Week Home Button
                }
                else
                    this.button21.Enabled = true;
            }
            if (StyxWoW.Me.IsHorde)
            {
                if (IsQuestComplete(5502)) // Final quest of Orgrimmar Kid's Week Line
                {
                    this.button20.Enabled = false;
                    this.button20.Text = "Orgrimmar Complete"; // Children's Week Home Button
                }
                else
                {
                    this.button20.Enabled = true;
                    this.button20.Text = "Orgrimmar"; // Children's Week Home Button
                }
                if (IsQuestComplete(10967)) // Final quest of Outlands Kid's Week Line
                {
                    this.button21.Enabled = false;
                    this.button21.Text = "Shattrath Complete"; // Children's Week Home Button
                }
                else
                    this.button21.Enabled = true;
            }
            if (IsQuestComplete(13926) || IsQuestComplete(13960)) // First of Oracles, Final quest of Northrend Wolvar Line
                this.button23.Enabled = false;
            else
                this.button23.Enabled = true;
            if (IsQuestComplete(13927) || IsQuestComplete(13959)) // First of Wolvar, Final quest of Northrend Oracle Line
                this.button24.Enabled = false;
            else
                this.button24.Enabled = true;

            BeenInitialized4 = BrodiesPlugin.hasBeenInitialized4;
        }

        public void UpdateStuff()
        {
            BPSettings.Instance.Load();
            BPGlobalSettings.Instance.Load();
            lastUseProfile = BPSettings.Instance.lastUsedPath;
            if (BeenInitialized4 == false)
            {
                BPGlobalSettings.Instance.Save();
            }
		}

        public bool IsQuestComplete(uint id)
        {
            PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById(id);
            if (quest != null)
            {
                return quest.IsCompleted;
            }

            return StyxWoW.Me.QuestLog.GetCompletedQuests().Contains(id);
        }

        #region Settings and Color Boxes

        public class BPSettings : Settings
        {
            public static readonly BPSettings Instance = new BPSettings();
            public BPSettings()
                : base(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Plugins\BrodiesPlugin\Settings\Char-Settings-{0}-{1}.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name)))
            {
            }
            [Setting, DefaultValue(0)]
            public int lastUsedPath { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active1 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile1 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active2 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile2 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active3 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile3 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active4 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile4 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active5 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile5 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active6 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile6 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active7 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile7 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active8 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile8 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active9 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile9 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active10 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile10 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active11 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile11 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active12 { get; set; }
			[Setting, DefaultValue("")]
            public string Profile12 { get; set; }
			[Setting, DefaultValue(false)]
            public bool Active13 { get; set; }
			[Setting, DefaultValue("")]
            public string dailiesDirName { get; set; }
        }

        public class BPGlobalSettings : Settings
        {
            public static readonly BPGlobalSettings Instance = new BPGlobalSettings();
            public BPGlobalSettings()
                : base(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Plugins\BrodiesPlugin\Settings\Main-Settings.xml")))
            {
            }
            [Setting, DefaultValue(false)]
            public bool AllowUpdate { get; set; }
            [Setting, DefaultValue(false)]
            public bool Allowlaunch { get; set; }
            [Setting, DefaultValue(0)]
            public int BaseProfileToLaunch { get; set; }
        }

        public void RecolorBoxes(CheckBox CB)
        {
            if (CB.Checked)
                CB.BackColor = System.Drawing.Color.Blue;
            else
                CB.BackColor = System.Drawing.Color.Red;
        }

        #endregion

        #region Enable Dailies and Start Dailies Buttons

        private void checkBox13_CheckedChanged_1(object sender, EventArgs e) // Enable Dailies
        {
            if (checkBox13.Checked == false) // If Dailies are off
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
                checkBox6.Enabled = false;
                checkBox7.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Enabled = false;
                checkBox10.Enabled = false;
                checkBox11.Enabled = false;
                checkBox12.Enabled = false;
                lastUseProfile = 0;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = true;
                checkBox7.Enabled = true;
                checkBox8.Enabled = true;
                checkBox9.Enabled = true;
                checkBox10.Enabled = true;
                checkBox11.Enabled = true;
                //checkBox12.Enabled = true; DISABLED
                lastUseProfile = 5; // Dailies Selector Code
            }
            RecolorBoxes(checkBox13);
        }

        #region Color Dailies Boxes

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox2);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox3);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox4);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox5);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox6);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox7);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox8);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox9);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox10);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox11);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            RecolorBoxes(checkBox12);
        }

        #endregion

        private void button4_Click_1(object sender, EventArgs e) // Start Dailies
        {
            lastUseProfile = 5; // Dailies Run
            BPGlobalSettings.Instance.Save();
            SaveProfileChoice();
            BPSettings.Instance.Save();
            ProfileSelector();
        }
        
        #endregion

        #region Profile Swapper Choices and Execution

        public void SaveProfileChoice()
		{
			BPSettings.Instance.Active1 = checkBox1.Checked;
			BPSettings.Instance.Active2 = checkBox2.Checked;
			BPSettings.Instance.Active3 = checkBox3.Checked;
			BPSettings.Instance.Active4 = checkBox4.Checked;
			BPSettings.Instance.Active5 = checkBox5.Checked;
			BPSettings.Instance.Active6 = checkBox6.Checked;
			BPSettings.Instance.Active7 = checkBox7.Checked;
			BPSettings.Instance.Active8 = checkBox8.Checked;
			BPSettings.Instance.Active9 = checkBox9.Checked;
			BPSettings.Instance.Active10 = checkBox10.Checked;
			BPSettings.Instance.Active11 = checkBox11.Checked;
			BPSettings.Instance.Active12 = checkBox12.Checked;
			BPSettings.Instance.Active13 = checkBox13.Checked;
		}

        private void ProfileSelector()
        {
            if (lastUseProfile == null || lastUseProfile == 0)
                return;

            // Questing Profiles
            if (lastUseProfile == 1) { lancaprofile(pathToProfiles + "Questing\\[Quest] The Level Grind [Brodie].xml"); }
            if (lastUseProfile == 2) { lancaprofile(pathToProfiles + "Achievements\\Lost and Found\\[Ach] Lost and Found [Brodie].xml"); }
            if (lastUseProfile == 4) { lancaprofile(pathToProfiles + "Achievements\\Lorewalkers\\[Ach] Lorewalkers [Brodie].xml"); }
            // Reserved for future use
            //if (lastUseProfile == 3) { lancaprofile(pathToProfiles + "Grinding\\Barrens\\Battlefield Barrens [Brodie].xml"); }
            //if (lastUseProfile == 6) { lancaprofile(pathToProfiles + "Grinding\\Barrens\\Barrens Wood [Brodie].xml"); }
            //if (lastUseProfile == 7) { lancaprofile(pathToProfiles + "Grinding\\Barrens\\Barrens Oil [Brodie].xml"); }
            //if (lastUseProfile == 8) { lancaprofile(pathToProfiles + "Grinding\\Barrens\\Barrens Stone [Brodie].xml"); }
            //if (lastUseProfile == 9) { lancaprofile(pathToProfiles + "Grinding\\Barrens\\Barrens Meat [Brodie].xml"); }
            // Dailies Profile(s)
            if (lastUseProfile == 5) { lancaprofile(pathToProfiles + "Reputation\\TMOPDE\\[Rep] Daily Grind [Brodie].xml"); }
            // Rep Grind Profiles
            if (lastUseProfile == 101)
            {
                object cookChoice = cookComboBox1.SelectedItem;
                switch (cookChoice.ToString())
                {
                    case ("Stormwind"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Stormwind Cooking [Brodie].xml");
                        break;
                    case ("Ironforge"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Ironforge Cooking [Brodie].xml");
                        break;
                    case ("Darnassus"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Darnassus Cooking [Brodie].xml");
                        break;
                    case ("Orgrimmar"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Orgrimmar Cooking [Brodie].xml");
                        break;
                    case ("Thunder Bluff"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Thunder Bluff Cooking [Brodie].xml");
                        break;
                    case ("Undercity"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Undercity Cooking [Brodie].xml");
                        break;
                    default:
                        break;
                }
            }
            if (lastUseProfile == 102) { lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Darkmoon Faire [Brodie].xml"); }
            if (lastUseProfile == 103) { lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Cenarion Circle [Brodie].xml"); }
            if (lastUseProfile == 104) { lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Steamwheedle Cartel [Brodie].xml"); }
            if (lastUseProfile == 105) { lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Hydraxian Waterlords [Brodie].xml"); }
            if (lastUseProfile == 106) { lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Timbermaw Hold [Brodie].xml"); }
            if (lastUseProfile == 107) { lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Brood of Nozdormu [Brodie].xml"); }
            if (lastUseProfile == 108)
            {
                object fishChoice = fishComboBox1.SelectedItem;
                switch (fishChoice.ToString())
                {
                    case ("Stormwind"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Stormwind Fishing [Brodie].xml");
                        break;
                    case ("Ironforge"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Ironforge Fishing [Brodie].xml");
                        break;
                    case ("Darnassus"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Darnassus Fishing [Brodie].xml");
                        break;
                    case ("Orgrimmar"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Orgrimmar Fishing [Brodie].xml");
                        break;
                    case ("Thunder Bluff"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Thunder Bluff Fishing [Brodie].xml");
                        break;
                    case ("Undercity"):
                        lancaprofile(pathToProfiles + "Reputation\\Classic\\[Rep] Undercity Fishing [Brodie].xml");
                        break;
                    default:
                        break;
                }
            }
            if (lastUseProfile == 110) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Ogri'la [Brodie].xml"); }
            if (lastUseProfile == 111) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Netherwind [Brodie].xml"); }
            if (lastUseProfile == 112) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] TBC Cooking [Brodie].xml"); } // Cooking Daily
            if (lastUseProfile == 113) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] TBC Fishing [Brodie].xml"); } // Fishing Daily
            if (lastUseProfile == 114) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Cenarion Expedition [Brodie].xml"); }
            if (lastUseProfile == 115) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Hellfire [Brodie].xml"); }
            if (lastUseProfile == 116) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Keepers of Time [Brodie].xml"); }
            if (lastUseProfile == 117) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Nagrand [Brodie].xml"); }
            if (lastUseProfile == 118) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Sporeggar [Brodie].xml"); }
            if (lastUseProfile == 119) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Consortium [Brodie].xml"); }
            if (lastUseProfile == 120) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Lower City [Brodie].xml"); }
            if (lastUseProfile == 121) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Shatari Skyguard [Brodie].xml"); }
            if (lastUseProfile == 122) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Shattered Sun [Brodie].xml"); }
            if (lastUseProfile == 123) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Aldor [Brodie].xml"); }
            if (lastUseProfile == 124) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Scryers [Brodie].xml"); }
            if (lastUseProfile == 125) { lancaprofile(pathToProfiles + "Reputation\\Outlands\\[Rep] Shatar [Brodie].xml"); }

            if (lastUseProfile == 26) { lancaprofile(pathToProfiles + "Grinding\\The Black Prince\\[Rep] The Black Prince - ZuTual [Brodie].xml"); }
            if (lastUseProfile == 27) { lancaprofile(pathToProfiles + "Grinding\\Isle of Giants\\[Rep] Isle of Giants Direhorns[Brodie].xml"); }
            if (lastUseProfile == 28) { lancaprofile(pathToProfiles + "Grinding\\Isle of Giants\\[Rep] Isle of Giants Dinomancers [Brodie].xml"); }
            if (lastUseProfile == 29) { lancaprofile(pathToProfiles + "Grinding\\Timeless Isle\\[Rep] Timeless Isle Coin [Brodie].xml"); }
            if (lastUseProfile == 30) { lancaprofile(pathToProfiles + "Grinding\\Timeless Isle\\[Rep] Timeless Isle Spend [Brodie].xml"); }
            if (lastUseProfile == 31) { lancaprofile(pathToProfiles + "Grinding\\Timeless Isle\\[Rep] Shaohao Rep Farm [Brodie].xml"); }
            if (lastUseProfile == 32) { lancaprofile(pathToProfiles + "Grinding\\Timeless Isle\\[Grind] Crystal of Insanity [Brodie].xml"); }

            // Farm Grinds
            if (lastUseProfile == 10) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Enigma Seeds.xml"); }
            if (lastUseProfile == 11) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Green Cabbage Seeds.xml"); }
            if (lastUseProfile == 12) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Jade Squash Seeds.xml"); }
            if (lastUseProfile == 13) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Juicyfruit Carrot Seeds.xml"); }
            if (lastUseProfile == 14) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Magebulb Seeds.xml"); }
            if (lastUseProfile == 15) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Mogu Pumpkin Seeds.xml"); }
            if (lastUseProfile == 16) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Pink Turnip Seeds.xml"); }
            if (lastUseProfile == 17) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Raptorleaf Seeds.xml"); }
            if (lastUseProfile == 18) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Red Blossom Leeks Seeds.xml"); }
            if (lastUseProfile == 19) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Scallion Seeds.xml"); }
            if (lastUseProfile == 20) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Snakeroot Seeds.xml"); }
            if (lastUseProfile == 21) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Songbell Seeds.xml"); }
            if (lastUseProfile == 22) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Striped Melon Seeds.xml"); }
            if (lastUseProfile == 23) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\White Turnip Seeds.xml"); }
            if (lastUseProfile == 24) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Windshear Cactus Seeds.xml"); }
            if (lastUseProfile == 25) { lancaprofile(pathToProfiles + "Grinding\\Tillers Farm Planting Profiles\\Witchberry Seeds.xml"); }

            // Holiday Profiles
            if (lastUseProfile == 200) { lancaprofile(pathToProfiles + "Holiday\\Noblegarden\\[Hol] Chocoholic [Brodie].xml"); }
            if (lastUseProfile == 201) { lancaprofile(pathToProfiles + "Holiday\\Noblegarden\\[Hol] Noblegarden [Brodie].xml"); }
            if (lastUseProfile == 202)
            {
                if (StyxWoW.Me.IsAlliance)
                    lancaprofile(pathToProfiles + "Holiday\\Children Week\\[Hol] Kids - Alliance Azeroth [Brodie].xml");
                else
                    lancaprofile(pathToProfiles + "Holiday\\Children Week\\[Hol] Kids - Horde Azeroth [Brodie].xml");
            }
            if (lastUseProfile == 203)
            {
                if (StyxWoW.Me.IsAlliance)
                    lancaprofile(pathToProfiles + "Holiday\\Children Week\\[Hol] Kids - Alliance Outlands [Brodie].xml");
                else
                    lancaprofile(pathToProfiles + "Holiday\\Children Week\\[Hol] Kids - Horde Outlands [Brodie].xml");
            }
            if (lastUseProfile == 204) { lancaprofile(pathToProfiles + "Holiday\\Children Week\\[Hol] Kids - Wolvar [Brodie].xml"); }
            if (lastUseProfile == 205) { lancaprofile(pathToProfiles + "Holiday\\Children Week\\[Hol] Kids - Oracle [Brodie].xml"); }

            if (lastUseProfile == 206)
            {
                if (StyxWoW.Me.IsAlliance)
                    lancaprofile(pathToProfiles + "Holiday\\Fire Festival\\Kalimdor\\[Hol] Kalimdor Warden Extinguish A [Brodie].xml");
                else
                    lancaprofile(pathToProfiles + "Holiday\\Fire Festival\\Kalimdor\\[Hol] Kalimdor Keeper Extinguish H [Brodie].xml");
            }
            if (lastUseProfile == 207)
            {
                if (StyxWoW.Me.IsAlliance)
                    lancaprofile(pathToProfiles + "Holiday\\Fire Festival\\Eastern Kingdoms\\[Hol] EK Warden Extinguish A [Brodie].xml");
                else
                    lancaprofile(pathToProfiles + "Holiday\\Fire Festival\\Eastern Kingdoms\\[Hol] EK Keeper Extinguish H [Brodie].xml");
            }
            if (lastUseProfile == 208)
            {
                if (StyxWoW.Me.IsAlliance)
                    lancaprofile(pathToProfiles + "Holiday\\Fire Festival\\Outland\\[Hol] Outlands Warden Extinguish A [Brodie].xml");
                else
                    lancaprofile(pathToProfiles + "Holiday\\Fire Festival\\Outland\\[Hol] Outlands Keeper Extinguish H [Brodie].xml");
            }
            if (lastUseProfile == 209)
            {
                if (StyxWoW.Me.IsAlliance)
                    lancaprofile(pathToProfiles + "Holiday\\Fire Festival\\Northrend\\[Hol] Northrend Warden Extinguish A [Brodie].xml");
                else
                    lancaprofile(pathToProfiles + "Holiday\\Fire Festival\\Northrend\\[Hol] Northrend Keeper Extinguish H [Brodie].xml");
            }
            if (lastUseProfile == 210) { lancaprofile(pathToProfiles + "Holiday\\Fire Festival\\Pandaria\\[Hol] Pandaria Flames [Brodie].xml"); }
        }

        private void lancaprofile(string ProfileToLoad)
        {
            bool isRunning = TreeRoot.IsRunning;
            if (isRunning)
            {
                BPGlobalSettings.Instance.Allowlaunch = true;
                BPGlobalSettings.Instance.BaseProfileToLaunch = lastUseProfile;
                BPGlobalSettings.Instance.Save();
                Close();
            }
            else
            {
                var questBot = BotManager.Instance.Bots.FirstOrDefault(kvp => kvp.Key == "Questing");
                if (questBot.Key == "Questing")
					BotManager.Instance.SetCurrent(questBot.Value);
				else
					Logging.Write("Unable to locate Questing bot");
			}
			Styx.CommonBot.Profiles.ProfileManager.LoadNew(ProfileToLoad);
			Close();
			TreeRoot.Start();
        }

        private void SaveAndClose()
        {
            SaveProfileChoice();
            BPSettings.Instance.Save();
            BPGlobalSettings.Instance.Save();
            Close();
            isRunningdepois = TreeRoot.IsRunning;
            if (isRunningdepois) { TreeRoot.Stop(); }
        }

        private void NotDailyStartProfile()
        {
            checkBox13.Checked = false;
            BPGlobalSettings.Instance.Save();
            SaveProfileChoice();
            BPSettings.Instance.Save();
            ProfileSelector();
        }

        #endregion

        #region Close and Close/Save Buttons

        private void button1_Click(object sender, EventArgs e) // Main Page
        {
            SaveAndClose();
        }

        private void button13_Click(object sender, EventArgs e) // Reputation - Burning Crusade
        {
            SaveAndClose();
        }

        private void button5_Click_1(object sender, EventArgs e) // Reputation - Pandaria
        {
            SaveAndClose();
        }

        private void button6_Click(object sender, EventArgs e) // Questing Page
        {
            SaveAndClose();
        }

        private void button7_Click(object sender, EventArgs e) // Grind It Page
        {
            SaveAndClose();
        }

        private void button8_Click(object sender, EventArgs e) // Links Page
        {
            SaveAndClose();
        }

        private void button9_Click(object sender, EventArgs e) // Reputation - Classic
        {
            SaveAndClose();
        }

        private void button11_Click(object sender, EventArgs e) // Achievements Page
        {
            SaveAndClose();
        }

        private void button15_Click(object sender, EventArgs e) // Holiday Page
        {
            SaveAndClose();
        }

        #endregion

        #region Start Profile Buttons

        private void button2_Click(object sender, EventArgs e) // Questing Page
        {
            lastUseProfile = 1; // Leveling 1 - 90
            NotDailyStartProfile();
        }

        private void button3_Click(object sender, EventArgs e) // Grind It Page
        {
            NotDailyStartProfile();
        }

        private void button10_Click(object sender, EventArgs e) // Reputation - Classic
        {
            NotDailyStartProfile();
        }

        private void button12_Click(object sender, EventArgs e) // Achievements Page
        {
            NotDailyStartProfile();
        }

        private void button14_Click(object sender, EventArgs e) // Reputation - Burning Crusade
        {
            NotDailyStartProfile();
        }

        #endregion

        #region Links List (including Paypal buttons)

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.thebuddyforum.com/honorbuddy-forum/submitted-profiles/neutral/97995-rep-ach-thebrodiemans-profile-compendium.html");
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.bothaven.com/Products/Details/1123/thebrodiemans-profile-compendium-premium");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.thebuddyforum.com/honorbuddy-forum/honorbuddy-profiles/152037-animus-thebrodieman-productions-loremaster-plugin.html");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.thebuddyforum.com/");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.thebuddyforum.com/honorbuddy-forum/honorbuddy-guides/108771-help-desk.html");
        }

        private void pictureBox1_Click(object sender, EventArgs e) // Paypal
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=davidl_rt06%40yahoo%2ecom&lc=US&item_name=TheBrodieMan%27s%20Dinner%20Fund&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_LG%2egif%3aNonHosted");
            Process.Start(sInfo);
        }

        #endregion

        #region Specific Achievement Profiles

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            lastUseProfile = 2; // Lost and Found Item Grind
            richTextBox1.Text = "This profile will fly around Pandaria, attempting to collect items for Lost and Found. \n This profile requires Pandaria Flying. \n Recommended you have a plugin or addon that automatically clicks the YES button for Bind on Pickup items.";
        }

        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 4; // Lorewalkers
            richTextBox1.Text = "This profile will fly to all the locations of scrolls for the Lorewalkers reputation quests. \n Requires Pandaria Flying.";
        }

        #endregion

        #region Grinding Profiles

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 29; // Timeless Coins
            richTextBox3.Text = "This profile will grind for Timeless Coins on the Timeless Isle. It will open all the chests it can that you have yet to so far before hunting mobs for coins.\n Must have done prereqs to zone, recommend starting within zone.";
        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 30; // Timeless Chests
            richTextBox3.Text = "This profile will grind for Timeless Chests in Kukuru's Cache. \n Must have done prereqs to zone, recommend starting within zone.";
        }

        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 31; // Shaohao Rep Grind
            richTextBox3.Text = "This profile will grind for Emperor Shaohao Reputation on the Timeless Isle. \n Must have done prereqs to zone, recommend starting within zone.";
        }

        private void radioButton55_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 32; // Crystal of Insanity
            richTextBox3.Text = "This profile will grind for the Crystal of Insanity on the Timeless Isle. It will head to the cave which it resides, and open crystals until the correct one is found.\n Prereqs to zone not required, however you are required to start profile on Pandaria.";
        }

        private void radioButton25_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 26; // Black Prince Rep
            richTextBox3.Text = "This profile will grind for Black Prince rep in the Isle of Thunder (Za'Tual Area). \n Must have done prereqs to zone, recommend starting within zone.";
        }

        private void radioButton26_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 27; // Dino Bones Grind Direhorns
            richTextBox3.Text = "This profile will grind for Dino Bones on the Isle of Giants by killing Direhorns. \n Your safety is not guaranteed. Avoidance is not perfect. Pathing is not perfect.";
        }

        private void radioButton27_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 28; // Dino Bones Grind Dinomancers
            richTextBox3.Text = "This profile will grind for Dino Bones on the Isle of Giants by killing Dinomancers. \n Your safety is not guaranteed. Avoidance is not perfect. Pathing is not perfect.";
        }

        #endregion

        #region Plant Farm Grind Profiles

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 10;
            richTextBox3.Text = "This profile will plant Enigma seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 11;
            richTextBox3.Text = "This profile will plant Green Cabbage seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 12;
            richTextBox3.Text = "This profile will plant Jade Squash seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 13;
            richTextBox3.Text = "This profile will plant Juicyfruit Carrot seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 14;
            richTextBox3.Text = "This profile will plant Magebulb seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 15;
            richTextBox3.Text = "This profile will plant Mogu Pumpkin seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 16;
            richTextBox3.Text = "This profile will plant Pink Turnip seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 17;
            richTextBox3.Text = "This profile will plant Raptorleaf seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 18;
            richTextBox3.Text = "This profile will plant Red Blossom Leek seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 19;
            richTextBox3.Text = "This profile will plant Scallion seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 20;
            richTextBox3.Text = "This profile will plant Snakeroot seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 21;
            richTextBox3.Text = "This profile will plant Songbell seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 22;
            richTextBox3.Text = "This profile will plant Striped Melon seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 23;
            richTextBox3.Text = "This profile will plant White Turnip seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 24;
            richTextBox3.Text = "This profile will plant Windshear Cactus seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 25;
            richTextBox3.Text = "This profile will plant Witchberry seeds at the farm, and farm up any plants available. \n It will plant only as many spots as available. \n This profile requires you to have the farm unlocked (tillers prereqs done).";
        }

        #endregion

        #region Reputation Grind Profiles

        #region Classic Zone Reps

        private void radioButton23_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 101;
            richTextBox4.Text = "This profile will run the dailies for Cooking in the chosen major city. \n It will start on which ever continent you are on, and move to your chosen capital. \n This profile will train cooking if you do not have it.";
        }

        private void radioButton24_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 108;
            richTextBox4.Text = "This profile will run the dailies for Fishing in the chosen major city. \n It will start on which ever continent you are on, and move to your chosen capital. \n This profile will train fishing if you do not have it.";
        }

        private void radioButton28_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 102;
            richTextBox4.Text = "This profile will run the dailies for Darkmoon Faire. \n You can start it on which ever continent, it will move to either your factions capital, or (if on a continent with access) to the nearest access point. \n This profile requires that the Faire be in town, obviously.";
        }

        private void radioButton29_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 103;
            richTextBox4.Text = "This profile will grind mobs and items for reputation towards Cenarion Circle. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Kalimdor -- Silithus. \n This profile requires you be at least 60 with flying, for best results.";
        }

        private void radioButton30_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 104;
            richTextBox4.Text = "This profile will grind mobs for reputation towards the Steamwheedle Cartel. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Kalimdor -- Tanaris. \n This profile recommends you be at least 60 with flying, for best results.";
        }

        private void radioButton31_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 105;
            richTextBox4.Text = "This profile will grind mobs and bosses in Molten Core for reputation towards the Hydraxian Waterlords. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to EK -- Searing Gorge. \n This profile recommends you be 90+ with flying, for best and fastest results.";
        }

        private void radioButton32_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 106;
            richTextBox4.Text = "This profile will grind mobs, quests and items for reputation towards Timbermaw Hold. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Kalimdor -- Felwood/Winterspring. \n This profile recommends you be 60+ with flying, for best and fastest results.";
        }

        private void radioButton33_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 107;
            richTextBox4.Text = "This profile will grind mobs, items, and bosses in Ahn'Qiraj for reputation towards the Brood of Nozdormu. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Kalimdor -- Tanaris. \n This profile recommends you be 60+, geared, and with flying, for best and fastest results.";
        }

        #endregion

        #region The Burning Crusade Reps

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 110;
            richTextBox5.Text = "This profile will run dailies for reputation towards Ogri'la. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to the Outlands -- Blade's Edge Mountains. \n This profile requires you be 70+ with flying.";
        }

        private void radioButton34_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 111;
            richTextBox5.Text = "This profile will run dailies for reputation towards Netherwing. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to the Outlands -- Netherstorm. \n This profile requires you be 70+ with flying.";
        }

        private void radioButton36_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 115;
            if (StyxWoW.Me.IsHorde)
                richTextBox5.Text = "This profile will grind mobs, quests, and bosses in Shattered Halls for reputation towards Thrallmar. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Outlands -- Hellfire Peninsula. \n This profile recommends you be 70+, geared, and with flying, for best and fastest results.";
            if (StyxWoW.Me.IsAlliance)
                richTextBox5.Text = "This profile will grind mobs, quests, and bosses in Shattered Halls for reputation towards Honor Hold. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Outlands -- Hellfire Peninsula. \n This profile recommends you be 70+, geared, and with flying, for best and fastest results.";
        }

        private void radioButton38_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 117;
            if (StyxWoW.Me.IsHorde)
                richTextBox5.Text = "This profile will grind mobs and items in Nagrand for reputation towards The Mag'har. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Outlands -- Nagrand. \n This profile recommends you be 70+, geared, and with flying, for best and fastest results.";
            if (StyxWoW.Me.IsAlliance)
                richTextBox5.Text = "This profile will grind mobs and items in Nagrand for reputation towards Kurenai. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Outlands -- Nagrand. \n This profile recommends you be 70+, geared, and with flying, for best and fastest results.";
        }

        private void radioButton40_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 119;
            richTextBox5.Text = "This profile will grind mobs for items and do minimal quests south of Area 52 in Netherstorm for reputation towards The Consortium. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Outlands -- Netherstorm. \n This profile recommends you be 70+, geared, and with flying, for best and fastest results.";
        }

        private void radioButton43_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 120;
            richTextBox5.Text = "This profile will grind mobs, quests, and bosses in The Shadow Labyrinth for reputation towards Lower City. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Outlands -- Terokkar Forest. \n This profile recommends you be 70+, geared, and with flying, for best and fastest results.";
        }

        private void radioButton44_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 121;
            richTextBox5.Text = "This profile will grind mobs, items and quests in Skettis for reputation towards the Sha'tari Skyguard.\nYou can start this profile from any location, and will move (in whatever manner is most efficient) to Outlands -- Terokkar Forest.\nThis profile recommends you be 70+, geared, and with flying, for best and fastest results, and requires you to have chosen one or the other faction.";
        }

        private void radioButton46_CheckedChanged(object sender, EventArgs e)
        {
            lastUseProfile = 123;
            richTextBox5.Text = "This profile will grind mobs and bosses in The Shadow Labyrinth for items towards The Aldor. \n You can start this profile from any location, and will move (in whatever manner is most efficient) to Outlands -- Terokkar Forest. \n This profile recommends you be 70+, geared, and with flying, for best and fastest results, and requires you to have chosen one or the other faction.";
        }

        #endregion

        #endregion

        #region Holiday Starters

        private void button17_Click(object sender, EventArgs e)
        {
            lastUseProfile = 200; // Chocoholic
            NotDailyStartProfile();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            lastUseProfile = 201; // Noblegarden Quests and Easter Egg Grind
            NotDailyStartProfile();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            lastUseProfile = 202; // Stormwind/Orgrimmar Children Week
            NotDailyStartProfile();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            lastUseProfile = 203; // Outlands Children Week
            NotDailyStartProfile();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            lastUseProfile = 204; // Wolvar Orphan Children Week
            NotDailyStartProfile();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            lastUseProfile = 205; // Oracle Orphan Children Week
            NotDailyStartProfile();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            lastUseProfile = 206; // Kalimdor Fire Festival
            NotDailyStartProfile();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            lastUseProfile = 207; // Eastern Kingdoms Fire Festival
            NotDailyStartProfile();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            lastUseProfile = 208; // Outlands Fire Festival
            NotDailyStartProfile();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            lastUseProfile = 209; // Northrend Fire Festival
            NotDailyStartProfile();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            lastUseProfile = 210; // Pandaria Fire Festival
            NotDailyStartProfile();
        }

        #endregion

    }
}
