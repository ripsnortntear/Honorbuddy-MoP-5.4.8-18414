using System;
using Styx.Common.Helpers;
using Styx.CommonBot.Inventory;
using Styx.Helpers;
using Styx.Plugins;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
//using Styx.Plugins.PluginClass;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Helpers;
using Styx;
//using Styx.Logic.Pathing;

using Styx.Helpers;


using System.Windows.Forms;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;

//using System.Linq;


//using Styx.Logic.BehaviorTree;

//using Styx.Logic;


//using Styx.Logic.Combat;
//using Styx.Logic.Inventory;
//using Styx.WoWInternals.WoWObjects;
//using Styx.Logic.Inventory.Frames.LootFrame;
//using Styx.Logic.Inventory.Frames.Gossip;
using Styx.WoWInternals.World;
//using Styx.Logic.Profiles;
//using Styx.Logic.AreaManagement;
using Styx.Plugins;
using Styx.WoWInternals.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using Bots.BGBuddy.Helpers;
using Styx;

using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace PetAreaSwitcher
{
    public class PetArea : HBPlugin
    {
		public static ProfileSettings ProfileSettings;
		private static readonly configForm Gui = new configForm();
        private readonly WaitTimer _updateTimer = WaitTimer.TenSeconds;
		public PetArea()
        {
		 string filename=Application.StartupPath + "\\Plugins\\PetArea\\Settings.xml";
		 try
            {
                ProfileSettings = new ProfileSettings(filename);
		
               
            }
            catch (Exception ex) { Logging.Write(ex.ToString()); }
		}
		public override void OnButtonPress()

        {
            Gui.ShowDialog();
        }

        public override void Pulse()
        {
		
			
		  
		  if (GetPetLevel() < 4) LoadProf(ProfileSettings.onetofour);
		  if (GetPetLevel() >= 4 && GetPetLevel() < 6) LoadProf(ProfileSettings.fourtosix);
		  if (GetPetLevel() >= 6 && GetPetLevel() < 8) LoadProf(ProfileSettings.sixtoeight);
		  if (GetPetLevel() >= 8 && GetPetLevel() < 10) LoadProf(ProfileSettings.eighttoten);
		  if (GetPetLevel() >= 10 && GetPetLevel() < 12) LoadProf(ProfileSettings.tentotwelve);
		  if (GetPetLevel() >= 12 && GetPetLevel() < 14) LoadProf(ProfileSettings.twelvetofourteen);
		  if (GetPetLevel() >= 14 && GetPetLevel() < 16) LoadProf(ProfileSettings.fourteentosixteen);
		  if (GetPetLevel() >= 16 && GetPetLevel() < 18) LoadProf(ProfileSettings.sixteentoeighteen);
		  if (GetPetLevel() >= 18 && GetPetLevel() < 20) LoadProf(ProfileSettings.eighteentotwenty);
		  if (GetPetLevel() >= 20 && GetPetLevel() < 22) LoadProf(ProfileSettings.twentytotwentytwo);
		  if (GetPetLevel() >= 22 && GetPetLevel() < 99) LoadProf(ProfileSettings.twentytwototwentyfive);
		  
		   
		   
		   
		   
		   
		   
		   
		   
        }
		public void LoadProf(string a){
		
		if (!a.Contains(":")) a = Application.StartupPath + "\\Plugins\\PetArea\\" + a;
		if (Styx.CommonBot.Profiles.ProfileManager.XmlLocation != a){
		 Styx.CommonBot.Profiles.ProfileManager.LoadNew(a, true);
		}
		 
		
		}
		
		//UnitBattlePetLevel
		
		public  int GetPetLevel(){
		 int getal=0;
		 List<string> cnt = Lua.GetReturnValues("local dummy = 99 for j=1,3 do local petID= C_PetJournal.GetPetLoadOutInfo(j) local speciesID, customName, level = C_PetJournal.GetPetInfoByPetID(petID) if level < dummy then dummy=level end end return dummy");
		 try
				{
				//BBLog(""+cnt[0]);
				getal = Convert.ToInt32(cnt[0]);
				}
				catch (Exception exc)
				{

				}
			
			
		 return getal;
		}
		
		public  int GetWildLevel(){
		//Logging.Write(GUID);
		 int getal=0;
		 List<string> cnt = Lua.GetReturnValues("return UnitBattlePetLevel('target')");
		 try
				{
				//BBLog(""+cnt[0]);
				getal = Convert.ToInt32(cnt[0]);
				}
				catch (Exception exc)
				{

				}
			
			
		 return getal;
		}
		
		
		
		
		
		
		public static WoWUnit WildBattleTarget()
        {
            //int dummy = GetPetLevel() -5;
			
			WoWUnit ret = (from unit in ObjectManager.GetObjectsOfType<WoWUnit>(true, true)
                             orderby unit.Distance ascending
							 
                             where !Blacklist.Contains(unit.Guid)
                             where unit.IsPetBattleCritter 
                             where !unit.IsDead
							 //where unit.Level < dummy
							 select unit).FirstOrDefault();
			
			
            if (ret != null){
                //Logging.Write(""+ret.Name+" range " + ret.Guid);
				return ret;
				
			}
            return null;
        }
		
		
		
		
		

        public override string Name { get { return "PetArea"; } }

        public override string Author { get { return "maybe"; } }

        public override Version Version { get { return new Version(1,0,0,0);} }
		public override bool WantButton { get { return true; } }
    }
	
	
	
	
	
	
	
	
	
	 public class ProfileSettings : Settings
    {
	
	/*
	  if (GetPetLevel() < 4) LoadProf("1-4-Durator.xml");
		  if (GetPetLevel() >= 4 && GetPetLevel() < 6) LoadProf("4-6-North Barrens.xml");
		  if (GetPetLevel() >= 6 && GetPetLevel() < 8) LoadProf("6-8-Ashenvale.xml");
		  if (GetPetLevel() >= 8 && GetPetLevel() < 10) LoadProf("8 - 10 Stonetalon Mountains.xml");
		  if (GetPetLevel() >= 10 && GetPetLevel() < 12) LoadProf("10 - 12 Desolace.xml");
		  if (GetPetLevel() >= 12 && GetPetLevel() < 14) LoadProf("12 - 14 Southern Barrens.xml");
		  if (GetPetLevel() >= 14 && GetPetLevel() < 16) LoadProf("14 - 16 - Feralas.xml");
		  if (GetPetLevel() >= 16 && GetPetLevel() < 18) LoadProf("16 - 18 - Felwood.xml");
		  if (GetPetLevel() >= 18 && GetPetLevel() < 20) LoadProf("18 - 20 - Silithus.xml");
		  if (GetPetLevel() >= 20 && GetPetLevel() < 22) LoadProf("20 - 22 - Terokkar Forest.xml");
		  if (GetPetLevel() >= 22 && GetPetLevel() < 99) LoadProf("22 - 25 - Netherstorm.xml");
		  */
	
	
        public ProfileSettings(string settingsPath) : base(settingsPath)
        {
            Load();
        }

        [Setting, Styx.Helpers.DefaultValue("1-4-Durator.xml")]
        public string onetofour { get; set; }
		
		[Setting, Styx.Helpers.DefaultValue("4-6-North Barrens.xml")]
        public string fourtosix { get; set; }
		
		[Setting, Styx.Helpers.DefaultValue("6-8-Ashenvale.xml")]
        public string sixtoeight { get; set; }
		
		[Setting, Styx.Helpers.DefaultValue("8 - 10 Stonetalon Mountains.xml")]
        public string eighttoten { get; set; }
		[Setting, Styx.Helpers.DefaultValue("10 - 12 Desolace.xml")]
        public string tentotwelve { get; set; }
		[Setting, Styx.Helpers.DefaultValue("12 - 14 Southern Barrens.xml")]
        public string twelvetofourteen { get; set; }
		[Setting, Styx.Helpers.DefaultValue("14 - 16 - Feralas.xml")]
        public string fourteentosixteen { get; set; }
		[Setting, Styx.Helpers.DefaultValue("16 - 18 - Felwood.xml")]
        public string sixteentoeighteen { get; set; }
		[Setting, Styx.Helpers.DefaultValue("18 - 20 - Silithus.xml")]
        public string eighteentotwenty { get; set; }
		
		[Setting, Styx.Helpers.DefaultValue("20 - 22 - Terokkar Forest.xml")]
        public string twentytotwentytwo { get; set; }
		[Setting, Styx.Helpers.DefaultValue("22 - 25 - Netherstorm.xml")]
        public string twentytwototwentyfive { get; set; }
		


		
    }
}
namespace PetAreaSwitcher
{
    partial class configForm : Form
    {
	 public configForm()
        {
            InitializeComponent();
			}
	   private void configForm_Load(object sender, EventArgs e)
        {
			
			textBox1.Text=PetArea.ProfileSettings.onetofour;
			textBox2.Text=PetArea.ProfileSettings.fourtosix;
			textBox3.Text=PetArea.ProfileSettings.sixtoeight;
			textBox4.Text=PetArea.ProfileSettings.eighttoten;
			textBox5.Text=PetArea.ProfileSettings.tentotwelve;
			textBox6.Text=PetArea.ProfileSettings.twelvetofourteen;
			textBox7.Text=PetArea.ProfileSettings.fourteentosixteen;
			textBox8.Text=PetArea.ProfileSettings.sixteentoeighteen;
			textBox9.Text=PetArea.ProfileSettings.eighteentotwenty;
			textBox10.Text=PetArea.ProfileSettings.twentytotwentytwo;
			textBox11.Text=PetArea.ProfileSettings.twentytwototwentyfive;
			
			
			//initdone=1;
			//Logging.Write("LAlalala");
		
           /* TheDungeonComboBox.SelectedIndex = Pokébuddy.MySettings.TheDungeon;
            HeartstoneOutSetting.Text = Pokébuddy.MySettings.HeartstoneAfter.ToString();
            WalkoutTimeSetting.Text = Pokébuddy.MySettings.WalkOutAfter.ToString();
            MailEveryResetCheck.Checked = Pokébuddy.MySettings.MailEveryReset;
            HordeCheck.Checked = Pokébuddy.MySettings.AmHorde;
            IBSupport.Checked = Pokébuddy.MySettings.IBSupport;*/
        }
		private string LoadFile(){
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			//openFileDialog1.InitialDirectory = Application.StartupPath ;
			openFileDialog1.Filter = "HB profiles (*.xml)|*.xml" ;
			openFileDialog1.FilterIndex = 1 ;
			openFileDialog1.RestoreDirectory = true ;

			if(openFileDialog1.ShowDialog() == DialogResult.OK)
			{
			 return openFileDialog1.FileName;
			}
			return "";
	
		}
		
		
		
		 private void button1_Click(object sender, EventArgs e)
        {
		string dummy=LoadFile();
		if (dummy!="") textBox1.Text=dummy;

        }

        private void button2_Click(object sender, EventArgs e)
        {
string dummy=LoadFile();
		if (dummy!="") textBox2.Text=dummy;
        }

        private void button3_Click(object sender, EventArgs e)
        {
string dummy=LoadFile();
		if (dummy!="") textBox3.Text=dummy;
        }

        private void button4_Click(object sender, EventArgs e)
        {
string dummy=LoadFile();
		if (dummy!="") textBox4.Text=dummy;
        }

        private void button5_Click(object sender, EventArgs e)
        {
string dummy=LoadFile();
		if (dummy!="") textBox5.Text=dummy;
        }

        private void button6_Click(object sender, EventArgs e)
        {
string dummy=LoadFile();
		if (dummy!="") textBox6.Text=dummy;
        }

        private void button7_Click(object sender, EventArgs e)
        {
string dummy=LoadFile();
		if (dummy!="") textBox7.Text=dummy;
        }

        private void button8_Click(object sender, EventArgs e)
        {
string dummy=LoadFile();
		if (dummy!="") textBox8.Text=dummy;
        }

        private void button9_Click(object sender, EventArgs e)
        {
		string dummy=LoadFile();
		if (dummy!="") textBox9.Text=dummy;

        }

        private void button10_Click(object sender, EventArgs e)
        {
string dummy=LoadFile();
		if (dummy!="") textBox10.Text=dummy;
        }

        private void button11_Click(object sender, EventArgs e)
        {
string dummy=LoadFile();
		if (dummy!="") textBox11.Text=dummy;
        }
		private void Save_Click(object sender, EventArgs e)
        {
		 			PetArea.ProfileSettings.onetofour=textBox1.Text;
			PetArea.ProfileSettings.fourtosix=textBox2.Text;
			PetArea.ProfileSettings.sixtoeight=textBox3.Text;
			PetArea.ProfileSettings.eighttoten=textBox4.Text;
			PetArea.ProfileSettings.tentotwelve=textBox5.Text;
			PetArea.ProfileSettings.twelvetofourteen=textBox6.Text;
			PetArea.ProfileSettings.fourteentosixteen=textBox7.Text;
			PetArea.ProfileSettings.sixteentoeighteen=textBox8.Text;
			PetArea.ProfileSettings.eighteentotwenty=textBox9.Text;
			PetArea.ProfileSettings.twentytotwentytwo=textBox10.Text;
			PetArea.ProfileSettings.twentytwototwentyfive=textBox11.Text;
			PetArea.ProfileSettings.Save();

        }
		 /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      /*  protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }*/

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.Save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "1-4";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(61, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(286, 20);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(353, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 21);
            this.button1.TabIndex = 2;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(353, 39);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(73, 21);
            this.button2.TabIndex = 5;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(61, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(286, 20);
            this.textBox2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "4-6";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(353, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(73, 21);
            this.button3.TabIndex = 8;
            this.button3.Text = "Browse";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(61, 66);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(286, 20);
            this.textBox3.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "6-8";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(353, 91);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(73, 21);
            this.button4.TabIndex = 11;
            this.button4.Text = "Browse";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(61, 92);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(286, 20);
            this.textBox4.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "8-10";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(353, 117);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(73, 21);
            this.button5.TabIndex = 14;
            this.button5.Text = "Browse";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(61, 118);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(286, 20);
            this.textBox5.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "10-12";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(353, 143);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(73, 21);
            this.button6.TabIndex = 17;
            this.button6.Text = "Browse";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(61, 144);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(286, 20);
            this.textBox6.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "12-14";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(353, 169);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(73, 21);
            this.button7.TabIndex = 20;
            this.button7.Text = "Browse";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(61, 170);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(286, 20);
            this.textBox7.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "14-16";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(353, 195);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(73, 21);
            this.button8.TabIndex = 23;
            this.button8.Text = "Browse";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(61, 196);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(286, 20);
            this.textBox8.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "16-18";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(353, 221);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(73, 21);
            this.button9.TabIndex = 26;
            this.button9.Text = "Browse";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(61, 222);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(286, 20);
            this.textBox9.TabIndex = 25;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 226);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "18-20";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(353, 247);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(73, 21);
            this.button10.TabIndex = 29;
            this.button10.Text = "Browse";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(61, 248);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(286, 20);
            this.textBox10.TabIndex = 28;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 252);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "20-22";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(353, 273);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(73, 21);
            this.button11.TabIndex = 32;
            this.button11.Text = "Browse";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(61, 274);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(286, 20);
            this.textBox11.TabIndex = 31;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 278);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "20-25";
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(124, 307);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(170, 25);
            this.Save.TabIndex = 33;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
			this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Form1
            // 
			this.Load += new System.EventHandler(this.configForm_Load);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 337);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "configForm";
            this.Text = "configForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button Save;
    
	}
}