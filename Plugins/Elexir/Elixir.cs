using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Drawing;

using Styx;
using Styx.Common;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.Misc;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;


// Alter 
//        public string elixir = ("Potion of Treasure Finding"); //Elixir Name
//        public string buff = ("Potion of Treasure Finding");   //Elixir Buff
//        public bool incombat = true; //Use Elixir in Combat? (True = Yes, False = No.)

namespace Elixir
{
    public class Elixir : HBPlugin
    {
		public override bool WantButton { get { return true; } }
        public string elixir = ("Potion of Treasure Finding"); //Elixir Name
        public string buff = ("Potion of Treasure Finding");   //Elixir Buff
        public bool incombat = false; //Use Elixir in Combat? (True = Yes, False = No.)
        private static Stopwatch timer = new Stopwatch();
        
		public void printLog(string s){
		}
		
		public override void Pulse()
        {
            if (StyxWoW.IsInGame)
            {
                if (incombat == false)
                {
                    if (!intMe.Combat && !intMe.Mounted && intMe.IsAlive || !intMe.IsDead)
                    {
                        _checkBuff();
                    }
                }
                if (incombat == true)
                {
                    if (!intMe.Mounted && intMe.IsAlive || !intMe.IsDead)
                    {
                        _checkBuff();
                    }
                }
            }
        }
        public void _checkBuff()
        {
            ObjectManager.Update();
            if (!intMe.ActiveAuras.ContainsKey(buff))
            {
                Logging.Write(">>No longer have Buff: " + buff + "...");
                _useElixir();
            }
        }
        public void _useElixir()
        {
            ObjectManager.Update();
            foreach (WoWItem item in ObjectManager.GetObjectsOfType<WoWItem>())
            {
				Logging.Write("Elixir check: '"+elixir+"'='"+item.Name+"'");
                if (item.Name == elixir)
                {
                    Lua.DoString("UseItemByName(\"" + elixir + "\")");
                    Logging.Write(">>Using: " + elixir + "...");
                }
            }
        }	
        private static LocalPlayer intMe { get { return StyxWoW.Me; } }
        public override string Name { get { return "Elixir"; } }
        public override string Author { get { return "PAB And Baxterboy"; } }
        public override Version Version { get { return new Version(1, 1); } }
    
		public override void OnButtonPress()
        {
			Settings s=new Settings();

			s.setPotion1Name(elixir);
			s.setPotion1Buff(buff);
			s.setPotion1UseCombat( incombat);

			
			s.ShowDialog();
			
			elixir = s.getPotion1Name();
			buff = s.getPotion1Buff();
			string str=s.getPotion1UseCombat();
			bool useCombat=false;
			if (str.Equals("true")) { 
				incombat = true; 
			}else{
				incombat = false;
			}
			 
		}
	
	}
	

	public class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }


        //static int Main()
        //{
        //    Form f=new Settings();
        //    f.ShowDialog();
            
        //    return 0;
        //}

        private void Potion1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string txt=Potion1.Text;
            switch(txt)
            {
                case "Destruction Potion": goto default;
                case "Fire Protection Potion": goto default;
                case "Free Action Potion": goto default;
                case "Frost Protection Potion": goto default;
                case "Greater Arcane Protection Potion": goto default;
                case "Greater Fire Protection Potion": goto default;
                case "Greater Frost Protection Potion": goto default;
                case "Greater Nature Protection Potion": goto default;
                case "Greater Shadow Protection Potion": goto default;
                case "Greater Stoneshield Potion": goto default;
                case "Haste Potion": goto default;
                case "Heroic Potion": goto default;
                case "Holy Protection Potion": goto default;
                case "Indestructible Potion": goto default;
                case "Insane Strength Potion": goto default;
                case "Invisibility Potion": goto default;
                case "Ironshield Potion": goto default;
                case "Jungle Remedy": goto default;
                case "Lesser Invisibility Potion": goto default;
                case "Lesser Stoneshield Potion": goto default;
                case "Limited Invulnerability Potion": goto default;
                case "Living Action Potion": goto default;
                case "Magic Resistance Potion": goto default;
                case "Major Arcane Protection Potion": goto default;
                case "Major Fire Protection Potion": goto default;
                case "Major Frost Protection Potion": goto default;
                case "Major Holy Protection Potion": goto default;
                case "Major Nature Protection Potion": goto default;
                case "Major Shadow Protection Potion": goto default;
                case "Minor Magic Resistance Potion": goto default;
                case "Nature Protection Potion": goto default;
                case "Potion of Curing": goto default;
                case "Potion of Speed": goto default;
                case "Potion of Wild Magic": goto default;
                case "Purification Potion": goto default;
                case "Restorative Potion": goto default;
                case "Shadow Protection Potion": goto default;
                case "Shrouding Potion": goto default;
                case "Sneaking Potion": goto default;
                case "Swiftness Potion": goto default;
                case "Swim Speed Potion": goto default;
                default:
                    Potion1Name.Text = txt;
                    Potion1Buff.Text = txt;
                    Potion1UseCombat.Text = "false";
                    break;
            }
        }

		private void saveSettings_Click(object sender, EventArgs e)
        {

        }
		
		
		public void setPotion1Name(string str){
            Potion1Name.Text=str;
        }

        public void setPotion1Buff(string str)
        {
            Potion1Buff.Text=str;
        }

        public void setPotion1UseCombat( bool str)
        {
			if (str){
				Potion1UseCombat.Text="true";
			}else{
				Potion1UseCombat.Text="false";
			}
            return;
        }
		
		
		
        public string getPotion1Name(){
            return Potion1Name.Text;
        }

        public string getPotion1Buff()
        {
            return Potion1Buff.Text;
        }

        public string getPotion1UseCombat()
        {
            return Potion1UseCombat.Text;
        }

		
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
			
            this.BattleElexir = new System.Windows.Forms.ComboBox();
            this.GuardianElexir = new System.Windows.Forms.ComboBox();
            this.Potion1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BattleElexirName = new System.Windows.Forms.TextBox();
            this.BattleElexirBuff = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BattleElexirUseCombat = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.GuardianElexirUseCombat = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.GuardianElexirBuff = new System.Windows.Forms.TextBox();
            this.GuardianElexirName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Potion1UseCombat = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.Potion1Buff = new System.Windows.Forms.TextBox();
            this.Potion1Name = new System.Windows.Forms.TextBox();
            this.saveSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();

			// 
            // BattleElexir
            // 
            this.BattleElexir.FormattingEnabled = true;
            this.BattleElexir.Location = new System.Drawing.Point(26, 29);
            this.BattleElexir.Name = "BattleElexir";
            this.BattleElexir.Size = new System.Drawing.Size(235, 21);
            this.BattleElexir.Sorted = true;
            this.BattleElexir.TabIndex = 0;
            // 
            // GuardianElexir
            // 
            this.GuardianElexir.FormattingEnabled = true;
            this.GuardianElexir.Location = new System.Drawing.Point(267, 29);
            this.GuardianElexir.Name = "GuardianElexir";
            this.GuardianElexir.Size = new System.Drawing.Size(240, 21);
            this.GuardianElexir.Sorted = true;
            this.GuardianElexir.TabIndex = 1;
		
            // 
            // Potion1
            // 
            this.Potion1.FormattingEnabled = true;
            this.Potion1.Items.AddRange(new object[] {
            "Destruction Potion",
            "Fire Protection Potion",
            "Free Action Potion",
            "Frost Protection Potion",
            "Greater Arcane Protection Potion",
            "Greater Fire Protection Potion",
            "Greater Frost Protection Potion",
            "Greater Nature Protection Potion",
            "Greater Shadow Protection Potion",
            "Greater Stoneshield Potion",
            "Haste Potion",
            "Heroic Potion",
            "Holy Protection Potion",
            "Indestructible Potion",
            "Insane Strength Potion",
            "Invisibility Potion",
            "Ironshield Potion",
            "Jungle Remedy",
            "Lesser Invisibility Potion",
            "Lesser Stoneshield Potion",
            "Limited Invulnerability Potion",
            "Living Action Potion",
            "Magic Resistance Potion",
            "Major Arcane Protection Potion",
            "Major Fire Protection Potion",
            "Major Frost Protection Potion",
            "Major Holy Protection Potion",
            "Major Nature Protection Potion",
            "Major Shadow Protection Potion",
            "Minor Magic Resistance Potion",
            "Nature Protection Potion",
            "Potion of Curing",
            "Potion of Speed",
			"Potion of Treasure Finding",
            "Potion of Wild Magic",
            "Purification Potion",
            "Restorative Potion",
            "Shadow Protection Potion",
            "Shrouding Potion",
            "Sneaking Potion",
            "Swiftness Potion",
            "Swim Speed Potion"});
            this.Potion1.Location = new System.Drawing.Point(515, 29);
            this.Potion1.Name = "Potion1";
            this.Potion1.Size = new System.Drawing.Size(239, 21);
            this.Potion1.Sorted = true;
            this.Potion1.TabIndex = 2;
            this.Potion1.SelectedIndexChanged += new System.EventHandler(this.Potion1_SelectedIndexChanged);

            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Battle Elexir";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Guardian Elexir";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(512, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Potion 1";
            // 
            // BattleElexirName
            // 
            this.BattleElexirName.Location = new System.Drawing.Point(95, 67);
            this.BattleElexirName.Name = "BattleElexirName";
            this.BattleElexirName.Size = new System.Drawing.Size(166, 20);
            this.BattleElexirName.TabIndex = 7;
            // 
            // BattleElexirBuff
            // 
            this.BattleElexirBuff.Location = new System.Drawing.Point(95, 93);
            this.BattleElexirBuff.Name = "BattleElexirBuff";
            this.BattleElexirBuff.Size = new System.Drawing.Size(166, 20);
            this.BattleElexirBuff.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "B. Elixirt Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "B. Elixirt Buff";
            // 
            // BattleElexirUseCombat
            // 
            this.BattleElexirUseCombat.FormattingEnabled = true;
            this.BattleElexirUseCombat.Items.AddRange(new object[] {
            "false",
            "true"});
            this.BattleElexirUseCombat.Location = new System.Drawing.Point(158, 120);
            this.BattleElexirUseCombat.Name = "BattleElexirUseCombat";
            this.BattleElexirUseCombat.Size = new System.Drawing.Size(103, 21);
            this.BattleElexirUseCombat.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "B. Elixirt in Combat";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(268, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "G. Elixirt in Combat";

			// 
            // GuardianElexirUseCombat
            // 
            this.GuardianElexirUseCombat.FormattingEnabled = true;
            this.GuardianElexirUseCombat.Items.AddRange(new object[] {
            "false",
            "true"});
            this.GuardianElexirUseCombat.Location = new System.Drawing.Point(404, 120);
            this.GuardianElexirUseCombat.Name = "GuardianElexirUseCombat";
            this.GuardianElexirUseCombat.Size = new System.Drawing.Size(103, 21);
            this.GuardianElexirUseCombat.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(269, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "G. Elixirt Buff";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(269, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "G. Elixirt Name";
            // 
            // GuardianElexirBuff
            // 
            this.GuardianElexirBuff.Location = new System.Drawing.Point(341, 93);
            this.GuardianElexirBuff.Name = "GuardianElexirBuff";
            this.GuardianElexirBuff.Size = new System.Drawing.Size(166, 20);
            this.GuardianElexirBuff.TabIndex = 15;
            // 
            // GuardianElexirName
            // 
            this.GuardianElexirName.Location = new System.Drawing.Point(341, 67);
            this.GuardianElexirName.Name = "GuardianElexirName";
            this.GuardianElexirName.Size = new System.Drawing.Size(166, 20);
            this.GuardianElexirName.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(515, 123);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Potion1 in Combat";
            // 
            // Potion1UseCombat
            // 
            this.Potion1UseCombat.FormattingEnabled = true;
            this.Potion1UseCombat.Items.AddRange(new object[] {
            "false",
            "true"});
            this.Potion1UseCombat.Location = new System.Drawing.Point(651, 120);
            this.Potion1UseCombat.Name = "Potion1UseCombat";
            this.Potion1UseCombat.Size = new System.Drawing.Size(103, 21);
            this.Potion1UseCombat.TabIndex = 24;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(516, 96);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Potion1 Buff";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(516, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Potion1 Name";
            // 
            // Potion1Buff
            // 
            this.Potion1Buff.Location = new System.Drawing.Point(588, 93);
            this.Potion1Buff.Name = "Potion1Buff";
            this.Potion1Buff.Size = new System.Drawing.Size(166, 20);
            this.Potion1Buff.TabIndex = 21;
            // 
            // Potion1Name
            // 
            this.Potion1Name.Location = new System.Drawing.Point(588, 67);
            this.Potion1Name.Name = "Potion1Name";
            this.Potion1Name.Size = new System.Drawing.Size(166, 20);
            this.Potion1Name.TabIndex = 20;
            // 
            // saveSettings
            // 
            this.saveSettings.Location = new System.Drawing.Point(680, 148);
            this.saveSettings.Name = "saveSettings";
            this.saveSettings.Size = new System.Drawing.Size(75, 23);
            this.saveSettings.TabIndex = 26;
            this.saveSettings.Text = "OK";
            this.saveSettings.UseVisualStyleBackColor = true;
            this.saveSettings.Click += new System.EventHandler(this.saveSettings_Click);
            

			
			// 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 183);
            this.Controls.Add(this.saveSettings);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Potion1UseCombat);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.Potion1Buff);
            this.Controls.Add(this.Potion1Name);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.GuardianElexirUseCombat);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.GuardianElexirBuff);
            this.Controls.Add(this.GuardianElexirName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BattleElexirUseCombat);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BattleElexirBuff);
            this.Controls.Add(this.BattleElexirName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Potion1);
            this.Controls.Add(this.GuardianElexir);
            this.Controls.Add(this.BattleElexir);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox BattleElexir;
        private System.Windows.Forms.ComboBox GuardianElexir;
        private System.Windows.Forms.ComboBox Potion1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox BattleElexirName;
        private System.Windows.Forms.TextBox BattleElexirBuff;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox BattleElexirUseCombat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox GuardianElexirUseCombat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox GuardianElexirBuff;
        private System.Windows.Forms.TextBox GuardianElexirName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox Potion1UseCombat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox Potion1Buff;
        private System.Windows.Forms.TextBox Potion1Name;
        private System.Windows.Forms.Button saveSettings;

		}

}