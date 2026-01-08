//	--------------------------------------------------------------------------------
//
//	BuddyControl Version 1.2
//	------------------------
//
//	This Honorbuddy plugin brings you the advantage to control the bot within
//	your WOW Client by pressing a defined hotkey
//
//	By default this hotkey is "Q"
//		pressing ALT-Q pauses and resumes Honorbuddy
//		pressing CTRL-Q starts Honorbuddy or brings it to stop
//
//	By default you will be informed with a short info in the game and the bot log
//
//	written by Planetmaster in late January 2015
//
//	--------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;

namespace BuddyControl
{
	public class BuddyControl : HBPlugin
	{
		public override string Name { get { return "Buddy Control"; } }
		public override string Author { get { return "Planetmaster"; } }
		public override Version Version { get { return new Version(1,2); } }
		public override bool WantButton { get { return true; } }

		public static void BuddyControl_Log(string Message)
		{
			Logging.Write(LogLevel.Normal, Colors.LightBlue, "[BuddyControl]: " + Message);
		}

		private static void BuddyControl_Message(string Message)
		{
			if (BuddyControlSettings.Instance.BuddyControlReportWithinWOW) Lua.DoString("print('"+Message+"')");
		}

		public override void OnDisable()
		{
			HotkeyRemove();
			BuddyControl_Log("I am no longer active");
			BuddyControlSettings.Instance.Save();
			base.OnDisable();
		}

		public override void OnEnable()
		{
			base.OnEnable();
			BuddyControlSettings.Instance.Load();
			HotkeyApply();
			BuddyControl_Log("I am running now");
		}

		public override void OnButtonPress()
		{
			new BuddyControlMainForm().ShowDialog();
		}

		public override void Pulse()
		{}

		public static void HotkeyApply()
		{
			HotkeysManager.Register("BuddyControl_Pause",BuddyControlSettings.Instance.BuddyControlHotKey,ModifierKeys.Alt,hk => {ControlBuddy(false);});
			HotkeysManager.Register("BuddyControl_Stop",BuddyControlSettings.Instance.BuddyControlHotKey,ModifierKeys.Control,hk => {ControlBuddy(true);});
			BuddyControl_Log("press 'ALT-" + Convert.ToString(BuddyControlSettings.Instance.BuddyControlHotKey) + "' to pause/resume the bot");
			BuddyControl_Log("press 'CTRL-" + Convert.ToString(BuddyControlSettings.Instance.BuddyControlHotKey) + "' to start/stop the bot");
		}
		
		public static void HotkeyRemove()
		{
			HotkeysManager.Unregister("BuddyControl_Stop");
			HotkeysManager.Unregister("BuddyControl_Pause");
		}
		
		public static void ControlBuddy(bool FullStop)
		{
			if (FullStop)
			{
				if (TreeRoot.IsRunning)
				{
					BuddyControl_Message("HB is now stopped");
					BuddyControl_Log("HB stopped");
					TreeRoot.Stop("BuddyControl - user hotkey stop request");
				}
				else
				{
					BuddyControl_Message("HB started and running");
					BuddyControl_Log("HB started and running");
					TreeRoot.Start();
				}
			}
			else
			{
				if (TreeRoot.IsRunning)
				{
					if (TreeRoot.IsPaused)
					{
						TreeRoot.Start();
						BuddyControl_Message("HB resumed from pause");
						BuddyControl_Log("HB resumed from pause and is now running");
					}
					else
					{
						TreeRoot.Stop();
						BuddyControl_Message("HB is now paused");
						BuddyControl_Log("HB is now paused");
					}
				}
				else BuddyControl_Log("HB is currently not running. Use 'CTRL-" + Convert.ToString(BuddyControlSettings.Instance.BuddyControlHotKey) + "' to start");
			}
		}
	}

	public partial class BuddyControlMainForm : Form
	{
		public BuddyControlMainForm()
		{
			InitializeComponent();
			propertyGrid.SelectedObject = BuddyControlSettings.Instance;
		}

		private void PropertyGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			if (e.ChangedItem.Label == "Hotkey") BuddyControlSettings.Instance.BuddyControlHotKey = BuddyControlSettings.Instance.BuddyControlHotKey & Keys.KeyCode;
			BuddyControlSettings.Instance.Save();
			BuddyControl.HotkeyRemove();
			BuddyControl.BuddyControl_Log("Settings changed");
			BuddyControl.HotkeyApply();
		}
	}
	
	partial class BuddyControlMainForm
	{
		private System.ComponentModel.IContainer components = null;
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.BCHost = new System.Windows.Forms.Integration.ElementHost();
			this.tLPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tLPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.tLPanel1.SuspendLayout();
			this.tLPanel2.SuspendLayout();
			this.SuspendLayout();
			this.BCHost.Location = new System.Drawing.Point(0, 0);
			this.BCHost.Name = "BCHost";
			this.BCHost.Size = new System.Drawing.Size(200, 100);
			this.BCHost.TabIndex = 2;
			this.BCHost.Child = null;
			this.tLPanel1.ColumnCount = 1;
			this.tLPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tLPanel1.Controls.Add(this.tLPanel2, 0, 1);
			this.tLPanel1.Controls.Add(this.propertyGrid, 0, 0);
			this.tLPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tLPanel1.Location = new System.Drawing.Point(0, 0);
			this.tLPanel1.Name = "tLPanel1";
			this.tLPanel1.RowCount = 2;
			this.tLPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tLPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tLPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tLPanel1.Size = new System.Drawing.Size(344, 376);
			this.tLPanel1.TabIndex = 1;
			this.tLPanel2.ColumnCount = 3;
			this.tLPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tLPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tLPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tLPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tLPanel2.Location = new System.Drawing.Point(3, 349);
			this.tLPanel2.Name = "tLPanel2";
			this.tLPanel2.RowCount = 1;
			this.tLPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tLPanel2.Size = new System.Drawing.Size(338, 24);
			this.tLPanel2.TabIndex = 1;
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.Location = new System.Drawing.Point(3, 3);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(338, 340);
			this.propertyGrid.TabIndex = 2;
			this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGridPropertyValueChanged);
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(344, 376);
			this.Controls.Add(this.tLPanel1);
			this.Controls.Add(this.BCHost);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "BuddyControlForm";
			this.Text = "BuddyControl Configuration";
			this.tLPanel1.ResumeLayout(false);
			this.tLPanel2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Integration.ElementHost BCHost;
		private System.Windows.Forms.TableLayoutPanel tLPanel1;
		private System.Windows.Forms.TableLayoutPanel tLPanel2;
		private System.Windows.Forms.PropertyGrid propertyGrid;
	}
	//add id

	
	public sealed class BuddyControlSettings : Settings
	{
		private static BuddyControlSettings _singleton;
		public static BuddyControlSettings Instance
			{get {return _singleton ?? (_singleton = new BuddyControlSettings());}}

		public BuddyControlSettings()
			: base(Path.Combine(Utilities.AssemblyDirectory, string.Format("Settings/BuddyControl.xml", StyxWoW.Me.Name))) {}

		[Setting, DefaultValue(true)]
		[Category("Settings")]
		[DisplayName("Ingame-Message")]
		[Description("Display a WOW ingame message to inform you that a hotkey action (start/stop/pause/resume) is executed")]
		public bool BuddyControlReportWithinWOW { get; set; }

		[Setting, DefaultValue(Keys.Q)]
		[Category("Settings")]
		[DisplayName("Hotkey")]
		[Description("Hotkey to be used with ALT (pause/resume) or CTRL (start/stop) the bot")]
		public Keys BuddyControlHotKey { get; set; }
	}
}