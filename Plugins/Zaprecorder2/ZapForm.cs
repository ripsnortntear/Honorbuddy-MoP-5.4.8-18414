using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Globalization;
using System.Threading;

using Styx;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Pathing;
using Styx.Helpers;


// TODO - Blackspot notes

namespace ZapRecorder2
{

    delegate void HotkeyHandler();

    public partial class ZapMainForm : Form
    {
        public Thread backgroundThread, recordingThread;
        public WoWPoint oldLocation = new WoWPoint(0, 0, 0);
        public bool isRecording = false;
        public bool chatMessage = true;

        private LocalPlayer intMe = StyxWoW.Me;

        private ZapHotspot HotspotMgr;
        private ZapBlackspot BlackspotMgr;
        private ZapMailbox MailboxMgr;
        private ZapRepair RepairMgr;

        public ZapMainForm()
        {
            InitializeComponent();

            HotspotMgr = new ZapHotspot();
            BlackspotMgr = new ZapBlackspot();
            MailboxMgr = new ZapMailbox();
            RepairMgr = new ZapRepair();

            backgroundThread = new Thread(new ThreadStart(BackgroundPulse));
            backgroundThread.IsBackground = true;
            backgroundThread.Start();


            LoadSettings();

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            txtLog.TextChanged += new EventHandler(txtLog_TextChanged);
            onTop.CheckedChanged += new EventHandler(onTop_CheckedChanged);

            ((Control)this.tabTest).Enabled = false;

            

            //Lua.Events.AttachEvent("MODIFIER_STATE_CHANGED", HandleModifierStateChanged);
            //ZapLog("Registered RControl as hotkey with LUA");
            //ShowWoWChatMessage("Registered HOTKEY for stuff");
            //bool myBool = Hotkeys.RegisterHotkey("Log Waypoint", WriteSomething, Keys.LControlKey);

            //ShowWoWChatMessage("Registered hotkeys for Zaprecorder2");

            /*
            Lua.Events.AttachEvent("MODIFIER_STATE_CHANGED", HandleModifierStateChanged);
            Log("Registered RControl as hotkey with LUA");

            //Keys[] registeredHotkey = new Keys[1] { Keys.LControlKey };

            Action hotkeyDelegate = HandleZapHotkey;
            
             */
            //ZapLog("Hotkey registration: " + myBool.ToString());
            //ZapLog("Stop me here");

            //Keys[] comboKey = new Keys[1];
            //comboKey[0] = ;

            //Action ZapCallback = () => HandleZapHotkey();
            //ZapCallback.Invoke(); // Successfully prints to log

            /*try
            {
                Action ZapCallback = this.HandleZapHotkey;

                Hotkeys.SetWindowHandle(StyxWoW.Memory.Process.MainWindowHandle);

                bool TestResult = Hotkeys.RegisterHotkey("Test Hotkey", ZapCallback, Keys.LControlKey);

                ZapLog("Registered LControl as a hotkey with HB");
            }
            catch (Exception ex)
            {
                ZapException(ex, "Setting hotkeys");
            }
             * */

        }

        /*
         * Sound temporarily disabled
        public void PlaySound(string fileName)
        {
            new SoundPlayer(Utilities.AssemblyDirectory + "\\Plugins\\ZapGB2Recorder\\Sound\\" + fileName).Play();
        }
         * 
         * */
        //Writes to the log

        


        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
            txtLog.Refresh();
        }

        //Saves the settings
        public void SaveSettings()
        {
            //Setting sell settings

            #region SellBlue

            if (sellBlue.Checked == true)
            {
                ZapRecorder2.Properties.Settings.Default.sellBlue = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.sellBlue = false;
            }

            #endregion

            #region SellGreen
            if (sellGreen.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.sellGreen = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.sellGreen = false;
            }
            #endregion

            #region SellGrey
            if (sellGrey.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.sellGrey = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.sellGrey = false;
            }
            #endregion

            #region SellPurple
            if (sellPurple.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.sellPurple = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.sellPurple = false;
            }
            #endregion

            #region SellWhite
            if (sellWhite.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.sellWhite = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.sellWhite = false;
            }
            #endregion

            //Setting mail settings

            #region MailBlue

            if (mailBlue.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.mailBlue = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.mailBlue = false;
            }

            #endregion

            #region MailGreen

            if (mailGreen.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.mailGreen = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.mailGreen = false;
            }

            #endregion

            #region MailGrey

            if (mailGrey.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.mailGrey = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.mailGrey = false;
            }

            #endregion

            #region MailPurple

            if (mailPurple.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.mailPurple = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.mailPurple = false;
            }

            #endregion

            #region MailWhite

            if (mailWhite.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.mailWhite = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.mailWhite = false;
            }

            #endregion

            //Setting misc settings

            #region PlaySound

            if (playSound.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.playSound = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.playSound = false;
            }

            #endregion

            #region OnTop

            if (onTop.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.onTop = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.onTop = false;
            }

            #endregion

            #region Chat Messages

            if (chkChatMessage.Checked)
            {
                ZapRecorder2.Properties.Settings.Default.chatMessages = true;
            }
            else
            {
                ZapRecorder2.Properties.Settings.Default.chatMessages = false;
            }

            #endregion

            #region MinimumDurability

            ZapRecorder2.Properties.Settings.Default.minimumDurability = txtMinDurability.Text;

            #endregion

            #region MinimumFreeBagSlots

            ZapRecorder2.Properties.Settings.Default.minimumFreeBagSlots = txtMinFreeBagSlots.Text;

            #endregion

            #region RecordingSleepTime

            ZapRecorder2.Properties.Settings.Default.recorderSleepTime = recordingTimeTextbox.Text;

            #endregion

            #region AddIfMoved

            ZapRecorder2.Properties.Settings.Default.addIfMoved = addIfMovedTextbox.Text;

            #endregion

            #region BlackspotRadius

            ZapRecorder2.Properties.Settings.Default.blackspotRadius = Convert.ToInt32(numericUpDown1.Value);

            #endregion


            ZapRecorder2.Properties.Settings.Default.Save();
            ZapLog("Settings saved");
        }

        //Loads the settings
        public void LoadSettings()
        {
            //Load sell settings

            #region SellBlue
            if (ZapRecorder2.Properties.Settings.Default.sellBlue == true)
            {
                sellBlue.Checked = true;
            }
            else
            {
                sellBlue.Checked = false;
            }
            #endregion

            #region SellGreen
            if (ZapRecorder2.Properties.Settings.Default.sellGreen == true)
            {
                sellGreen.Checked = true;
            }
            else
            {
                sellGreen.Checked = false;
            }
            #endregion

            #region SellGrey
            if (ZapRecorder2.Properties.Settings.Default.sellGrey == true)
            {
                sellGrey.Checked = true;
            }
            else
            {
                sellGrey.Checked = false;
            }
            #endregion

            #region SellPurple
            if (ZapRecorder2.Properties.Settings.Default.sellPurple == true)
            {
                sellPurple.Checked = true;
            }
            else
            {
                sellPurple.Checked = false;
            }
            #endregion

            #region SellWhite
            if (ZapRecorder2.Properties.Settings.Default.sellWhite == true)
            {
                sellWhite.Checked = true;
            }
            else
            {
                sellWhite.Checked = false;
            }
            #endregion

            //Load mail settings

            #region MailBlue
            if (ZapRecorder2.Properties.Settings.Default.mailBlue == true)
            {
                mailBlue.Checked = true;
            }
            else
            {
                mailBlue.Checked = false;
            }
            #endregion

            #region MailGreen
            if (ZapRecorder2.Properties.Settings.Default.mailGreen == true)
            {
                mailGreen.Checked = true;
            }
            else
            {
                mailGreen.Checked = false;
            }
            #endregion

            #region MailGrey
            if (ZapRecorder2.Properties.Settings.Default.mailGrey == true)
            {
                mailGrey.Checked = true;
            }
            else
            {
                mailGrey.Checked = false;
            }
            #endregion

            #region MailPurple
            if (ZapRecorder2.Properties.Settings.Default.mailPurple == true)
            {
                mailPurple.Checked = true;
            }
            else
            {
                mailPurple.Checked = false;
            }
            #endregion

            #region MailWhite
            if (ZapRecorder2.Properties.Settings.Default.mailWhite == true)
            {
                mailWhite.Checked = true;
            }
            else
            {
                mailWhite.Checked = false;
            }
            #endregion

            //Load misc settings

            #region PlaySound
            if (ZapRecorder2.Properties.Settings.Default.playSound == true)
            {
                playSound.Checked = true;
            }
            else
            {
                playSound.Checked = false;
            }
            #endregion

            #region Chat Messages
            if (ZapRecorder2.Properties.Settings.Default.chatMessages == true)
            {
                chkChatMessage.Checked = true;
                chatMessage = true;
            }
            else
            {
                chkChatMessage.Checked = false;
                chatMessage = false;
            }
            #endregion

            #region OnTop
            if (ZapRecorder2.Properties.Settings.Default.onTop == true)
            {
                onTop.Checked = true;
                this.TopMost = true;
            }
            else
            {
                onTop.Checked = false;
            }
            #endregion

            #region MinimumDurability

            txtMinDurability.Text = ZapRecorder2.Properties.Settings.Default.minimumDurability;

            #endregion

            #region MinimumFreeBagSlots

            txtMinFreeBagSlots.Text = ZapRecorder2.Properties.Settings.Default.minimumFreeBagSlots;

            #endregion

            #region RecordingSleepTime

            recordingTimeTextbox.Text = ZapRecorder2.Properties.Settings.Default.recorderSleepTime;

            #endregion

            #region AddIfMoved

            addIfMovedTextbox.Text = ZapRecorder2.Properties.Settings.Default.addIfMoved;

            #endregion

            #region BlackspotRadius

            numericUpDown1.Value = ZapRecorder2.Properties.Settings.Default.blackspotRadius;

            #endregion

            ZapLog("Settings loaded");
        }

        //Saves the settings when a setting is changed under the setting tab
        public void SettingsChanged(object sender, EventArgs arg)
        {
            SaveSettings();
        }

        //On form closing
        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Hide the form when it's closing instead of closing it!
            ZapPlugin.isHidden = true;
            SaveSettings();
            e.Cancel = true;
            this.Hide();
        }

        // So far just updates labels.. Necessary?
        public void BackgroundPulse()
        {
            ZapLog("Background thread started");

            while (true)
            {
                Thread.Sleep(50);

                try
                {
                    lblBlackspots.Text = "Blackspots: " + BlackspotMgr.Count;
                    lblVendors.Text = "Vendors: " + RepairMgr.Count;
                    lblHotspots.Text = "Hotspots: " + HotspotMgr.Count;
                    lblMailboxes.Text = "Mailboxes: " + MailboxMgr.Count;
                    // Do not attempt to update location. Frequent calls WILL crash HB
                }
                catch (Exception ex)
                {
                    // REMOVEME
                    ZapException(ex, "BackgroundPulse");
                }
            }
        }

        public void RecordingPulse()
        {
            try {
                while (isRecording)
                {
                    if (HasMovedNeedsHotspot())
                    {
                        /*if (playSound.Checked == true)
                        {
                            PlaySound("bloop.wav");
                        }
                        */
                    
                    
                        oldLocation = new WoWPoint(intMe.Location.X, intMe.Location.Y, intMe.Location.Z);

                        HotspotMgr.Add();
                        UpdateHotspotList();

                        // Delay according to recording time
                        Thread.Sleep(Convert.ToInt32(recordingTimeTextbox.Text));
                    }
                    else
                    {
                        // Hasn't moved enough for hotspot. Check again in 100ms
                        Thread.Sleep(100);
                    }
                }
            } catch(Exception ex) {
                ZapException(ex,"RecordingPulse");
                RestartRecordingPulse();

            }
        }

        private void RestartRecordingPulse()
        {
            isRecording = true;
            recordingThread = new Thread(new ThreadStart(RecordingPulse));
            recordingThread.IsBackground = true;
            recordingThread.Start();

            ShowWoWChatMessage("Recording pulse crashed! It has been restarted.");
            ZapLog("RecordingPulse restarted after a crash!");
        }

        //TODO - Move this to hotspot class
        public bool HasMovedNeedsHotspot()
        {
            try
            {
                return (intMe.Location.Distance(oldLocation) > Convert.ToInt32(addIfMovedTextbox.Text));
            }
            catch (Exception ex)
            {
                ZapException(ex, "HasMovedNeedsHotspot");
                return false;
            }
        }

        

        private void btnRecording_Click(object sender, EventArgs e)
        {
            ToggleRecording();
            
        }

        public void ToggleRecording()
        {
            if (isRecording)
            {
                // Stop the recording
                isRecording = false;
                btnRecording.Text = "Start Recording";
                btnRecording.ForeColor = Color.Black;
                ShowWoWChatMessage("Recording stopped!");
                ZapLog("Recording stopped!");
            }
            else
            {
                try
                {
                    //We're not recording, so lets start!
                    isRecording = true;
                    recordingThread = new Thread(new ThreadStart(RecordingPulse));
                    recordingThread.IsBackground = true;
                    recordingThread.Start();

                    btnRecording.Text = "Stop Recording";
                    btnRecording.ForeColor = Color.Red;
                    //recordLabel.Text = "Recording: Yes";

                    ShowWoWChatMessage("Recording started!");
                    ZapLog("Recording started!");
                }
                catch
                {

                }
            }
        }

        //TODO - REDO SAVE PROFILE ENTIRELY
        public void SaveProfile()
        {
            if(HotspotMgr.Count >= 1)
            {

                try
                {
                    dlgSaveFile.InitialDirectory = Utilities.AssemblyDirectory;

                    if (dlgSaveFile.ShowDialog() == DialogResult.OK)
                    {
                        string path = dlgSaveFile.FileName;

                        StreamWriter writer = new StreamWriter(path);
                        ZapLog("Writing profile: " + path);

                        //BEGINNING OF THE PROFILE WRITING
                        writer.WriteLine("<HBProfile>");

                        writer.WriteLine("///////////////////////////////////////////////////");
                        writer.WriteLine("//                                               //");
                        writer.WriteLine("// Profile generated with ZapRecorder2 v1.2.0    //");
                        writer.WriteLine("// Created by BadWolf                            //");
                        writer.WriteLine("//                                               //");
                        writer.WriteLine("///////////////////////////////////////////////////");

                        if (txtProfileName.Text.Trim() == "")
                        {
                            writer.WriteLine("<Name>" + intMe.ZoneText + "</Name>");
                        }
                        else
                        {
                            writer.WriteLine("<Name>" + txtProfileName.Text + "</Name>");
                        }
                        writer.WriteLine("<MinDurability>" + txtMinDurability.Text + "</MinDurability>");
                        writer.WriteLine("<MinFreeBagSlots>" + txtMinFreeBagSlots.Text + "</MinFreeBagSlots>");
                        writer.WriteLine("");

                        writer.WriteLine("<MinLevel>" + txtMinLevel.Text + "</MinLevel>");
                        writer.WriteLine("<MaxLevel>" + txtMaxLevel.Text + "</MaxLevel>"); 
                        writer.WriteLine("<Factions>" + txtFactions.Text + "</Factions>");
                        writer.WriteLine("");


                        #region Mail
                        if (mailGrey.Checked == true)
                        {
                            writer.WriteLine("<MailGrey>true</MailGrey>");
                        }
                        else
                        {
                            writer.WriteLine("<MailGrey>false</MailGrey>");
                        }

                        if (mailWhite.Checked == true)
                        {
                            writer.WriteLine("<MailWhite>true</MailWhite>");
                        }
                        else
                        {
                            writer.WriteLine("<MailWhite>false</MailWhite>");
                        }

                        if (mailGreen.Checked == true)
                        {
                            writer.WriteLine("<MailGreen>true</MailGreen>");
                        }
                        else
                        {
                            writer.WriteLine("<MailGreen>false</MailGreen>");
                        }

                        if (mailBlue.Checked == true)
                        {
                            writer.WriteLine("<MailBlue>true</MailBlue>");
                        }
                        else
                        {
                            writer.WriteLine("<MailBlue>false</MailBlue>");
                        }

                        if (mailPurple.Checked == true)
                        {
                            writer.WriteLine("<MailPurple>true</MailPurple>");
                        }
                        else
                        {
                            writer.WriteLine("<MailPurple>false</MailPurple>");
                        }
                        #endregion

                        writer.WriteLine("");

                        #region SellOptions

                        if (sellGrey.Checked == true)
                        {
                            writer.WriteLine("<SellGrey>true</SellGrey>");
                        }
                        else
                        {
                            writer.WriteLine("<SellGrey>false</SellGrey>");
                        }

                        if (sellWhite.Checked == true)
                        {
                            writer.WriteLine("<SellWhite>true</SellWhite>");
                        }
                        else
                        {
                            writer.WriteLine("<SellWhite>false</SellWhite>");
                        }

                        if (sellGreen.Checked == true)
                        {
                            writer.WriteLine("<SellGreen>true</SellGreen>");
                        }
                        else
                        {
                            writer.WriteLine("<SellGreen>false</SellGreen>");
                        }

                        if (sellBlue.Checked == true)
                        {
                            writer.WriteLine("<SellBlue>true</SellBlue>");
                        }
                        else
                        {
                            writer.WriteLine("<SellBlue>false</SellBlue>");
                        }

                        if (sellPurple.Checked == true)
                        {
                            writer.WriteLine("<SellPurple>true</SellPurple>");
                        }
                        else
                        {
                            writer.WriteLine("<SellPurple>false</SellPurple>");
                        }

                        #endregion

                        writer.WriteLine("");

                        #region Misc

                        writer.WriteLine("<Vendors>");


                        foreach (RepairMerchant merchant in RepairMgr.Merchants)
                        {
                            writer.WriteLine(merchant.ToHotspot());
                        }

                        writer.WriteLine("</Vendors>");
                        writer.WriteLine("");

                        
                        writer.WriteLine("<Mailboxes>");

                        foreach (WoWPoint mailPoint in MailboxMgr.Mailboxes)
                        {
                            writer.WriteLine(MailboxMgr.WowPointToHotspot(mailPoint));
                        }

                        writer.WriteLine("</Mailboxes>");
                        writer.WriteLine("");


                        writer.WriteLine("<Blackspots>");

                        foreach (WoWPoint blackspotPoint in BlackspotMgr.Blackspots)
                        {
                            writer.WriteLine(BlackspotMgr.WowPointToBlackspot(blackspotPoint));
                        }

                        writer.WriteLine("</Blackspots>");
                        writer.WriteLine("");


                        writer.WriteLine("<Hotspots>");

                        foreach (WoWPoint hotspotPoint in HotspotMgr.Hotspots)
                        {
                            writer.WriteLine(HotspotMgr.WowPointToHotspot(hotspotPoint));
                        }

                        writer.WriteLine("</Hotspots>");

                        #endregion


                        writer.WriteLine("</HBProfile>");
                        writer.Close();


                        ZapLog("Profile writing completed!");
                    }
                    else
                    {
                        // User cancelled Save Dialog
                    }

                }
                catch (Exception ex)
                {
                    ZapException(ex, "SaveProfile");
                }
            }
            else
            {
                MessageBox.Show("You dont have any hotspots to save!");
            }
        }

        private void btnAddBlackspot_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you standing in the middle of the blackspot and have choosen the prefered radius?\nCurrent radius = " + numericUpDown1.Value.ToString(), this.Text) == true)
            {
                BlackspotMgr.Add();
                UpdateBlackspotList();

                ZapLog("Added blackspot");
                ShowWoWChatMessage("Blackspot added at your location");
            }
        }

        //TODO - Change this to use Mailbox class
        private void btnAddMailbox_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you standing next to a mailbox?", this.Text) == true)
            {
                MailboxMgr.Add();
                UpdateMailboxList();

                ShowWoWChatMessage("Mailbox added at your location");
            }
        }

        private void btnAddRepair_Click(object sender, EventArgs e)
        {
            AddRepairMerchant();
        }

        
        private void btnClearEverything_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you sure you wish to clear everything from this profile?", this.Text) == true)
            {
                txtProfileAuthor.Text = "";
                txtProfileName.Text = "";
                HotspotMgr.Clear();
                UpdateHotspotList();

                BlackspotMgr.Clear();
                UpdateBlackspotList();

                MailboxMgr.Clear();
                UpdateMailboxList();

                RepairMgr.Clear();
                UpdateRepairList();

                ZapLog("Cleared everything from the current profile");
            }
        }

        public void UpdateHotspotList()
        {
            try
            {
                listHotspots.Items.Clear();
                listTestHotspots.Items.Clear();

                foreach (WoWPoint hotspotPoint in HotspotMgr.Hotspots)
                {
                    listHotspots.Items.Add(HotspotMgr.WowPointToHotspot(hotspotPoint));
                    listTestHotspots.Items.Add(HotspotMgr.WowPointToHotspot(hotspotPoint));
                }
            }
            catch (Exception ex)
            {
                ZapException(ex, "UpdateHotspotList");
            }
        }

        public void UpdateBlackspotList()
        {
            try {
                listBlackspots.Items.Clear();

                foreach (WoWPoint blackspotPoint in BlackspotMgr.Blackspots)
                {
                    listBlackspots.Items.Add(BlackspotMgr.WowPointToBlackspot(blackspotPoint));
                }
            }
            catch (Exception ex)
            {
                ZapException(ex, "UpdateBlackspotList");
            }
        }

        public void UpdateRepairList()
        {
            try
            {

                listRepair.Items.Clear();

                foreach (RepairMerchant merchant in RepairMgr.Merchants)
                {
                    listRepair.Items.Add(merchant.ToHotspot());
                }
            }
            catch (Exception ex)
            {
                ZapException(ex, "UpdateRepairList");
            }
        }

        public void UpdateMailboxList()
        {
            try
            {
                listMailboxes.Items.Clear();

                foreach (WoWPoint mailboxPoint in MailboxMgr.Mailboxes)
                {
                    listMailboxes.Items.Add(MailboxMgr.WowPointToHotspot(mailboxPoint));
                }
            }
            catch (Exception ex)
            {
                ZapException(ex, "UpdateMailboxList");
            }
        }

        private void btnLocRefresh_Click(object sender, EventArgs e)
        {
            lblLocation.Text = "X = " + intMe.Location.X + ", Y = " + intMe.Location.Y + ", Z = " + intMe.Location.Z;
        }


        private void btnAddHotspot_Click(object sender, EventArgs e)
        {
            HotspotMgr.Add();
            UpdateHotspotList();

            //ZapLog("Added " + getHotspot());

        }

        

        private string getProfileDataString(string profileLine, string nodeName)
        {
            return profileLine.Replace("<" + nodeName + ">", "").Replace("</" + nodeName + ">", "").Trim();
        }

        private bool getProfileDataBool(string profileLine, string nodeName)
        {
            return profileLine.Replace("<" + nodeName + ">", "").Replace("</" + nodeName + ">", "").Trim().ToBoolean();
        }


        #region Profile Info Tab

        //TODO - CREATE PROFILE CLASS
        //TODO - MAKE PROFILE CLASS USE HOTSPOT CLASS
        
        private void btnOpenProfile_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Loading a profile will replace all current hotspots, settings, etc..\n\n", "Confirm Open Profile", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == true)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                DialogResult results = new DialogResult();

                results = dialog.ShowDialog();

                if (results == DialogResult.OK)
                {
                    HotspotMgr.Clear();
                    UpdateHotspotList();

                    BlackspotMgr.Clear();
                    UpdateBlackspotList();

                    MailboxMgr.Clear();
                    UpdateMailboxList();

                    RepairMgr.Clear();
                    UpdateRepairList();

                    ZapLog("Loading profile " + dialog.FileName);



                    string tempLine;

                    StreamReader file = new System.IO.StreamReader(@dialog.FileName);
                    while ((tempLine = file.ReadLine()) != null)
                    {
                        if (tempLine.Contains("<Hotspot "))
                        {
                            if (!HotspotMgr.Exists(tempLine))
                            {
                                HotspotMgr.Add(tempLine.Trim());
                            }
                        }
                        else if (tempLine.Contains("<Blackspot "))
                        {
                            if (!BlackspotMgr.Exists(tempLine))
                            {
                                BlackspotMgr.Add(tempLine.Trim());
                            }
                        }
                        else if (tempLine.Contains("<Mailbox "))
                        {

                            if (!HotspotMgr.Exists(tempLine))
                            {
                                MailboxMgr.Add(tempLine.Trim());
                            }
                        }
                        else if (tempLine.Contains("<Vendor ") && tempLine.Contains("Repair"))
                        {

                            if (!RepairMgr.Exists(tempLine))
                            {
                                RepairMgr.Add(tempLine.Trim());
                            }
                        }
                        else if (tempLine.Contains("<Name"))
                        {
                            txtProfileName.Text = getProfileDataString(tempLine, "Name");
                        }
                        else if (tempLine.Contains("<MailGrey"))
                        {
                            mailGrey.Checked = getProfileDataBool(tempLine, "MailGrey");
                        }
                        else if (tempLine.Contains("<MailWhite"))
                        {
                            mailWhite.Checked = getProfileDataBool(tempLine, "MailWhite");
                        }
                        else if (tempLine.Contains("<MailGreen"))
                        {
                            mailGreen.Checked = getProfileDataBool(tempLine, "MailGreen");
                        }
                        else if (tempLine.Contains("<MailBlue"))
                        {
                            mailBlue.Checked = getProfileDataBool(tempLine, "MailBlue");
                        }
                        else if (tempLine.Contains("<MailPurple"))
                        {
                            mailPurple.Checked = getProfileDataBool(tempLine, "MailPurple");
                        }
                        else if (tempLine.Contains("<SellGrey"))
                        {
                            sellGrey.Checked = getProfileDataBool(tempLine, "SellGrey");
                        }
                        else if (tempLine.Contains("<SellWhite"))
                        {
                            sellWhite.Checked = getProfileDataBool(tempLine, "SellWhite");
                        }
                        else if (tempLine.Contains("<SellGreen"))
                        {
                            sellGreen.Checked = getProfileDataBool(tempLine, "SellGreen");
                        }
                        else if (tempLine.Contains("<SellBlue"))
                        {
                            sellBlue.Checked = getProfileDataBool(tempLine, "SellBlue");
                        }
                        else if (tempLine.Contains("<SellPurple"))
                        {
                            sellPurple.Checked = getProfileDataBool(tempLine, "SellPurple");
                        }
                        else if (tempLine.Contains("<MinDurability"))
                        {
                            txtMinDurability.Text = getProfileDataString(tempLine, "MinDurability");
                        }
                        else if (tempLine.Contains("<MinFreeBagSlots"))
                        {
                            txtMinFreeBagSlots.Text = getProfileDataString(tempLine, "MinFreeBagSlots");
                        }
                        else if (tempLine.Contains("<MinLevel"))
                        {
                            txtMinLevel.Text = getProfileDataString(tempLine, "MinLevel");
                        }
                        else if (tempLine.Contains("<MaxLevel"))
                        {
                            txtMaxLevel.Text = getProfileDataString(tempLine, "MaxLevel");
                        }
                        else if (tempLine.Contains("<Factions"))
                        {
                            txtFactions.Text = getProfileDataString(tempLine, "Factions");
                        }


                    }

                    file.Close();


                    UpdateHotspotList();
                    UpdateBlackspotList();
                    UpdateMailboxList();
                    UpdateRepairList();

                    ZapLog("Profile load complete!");
                }
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            SaveProfile();
        }

        #endregion

        #region Hotspots Tab

        //TODO - Clear multiple hotspots from hotspots tab
        private void btnClearSelectedHotspots_Click(object sender, EventArgs e)
        {
            if (HotspotMgr.Count > 0)
            {
                if (MessageBoxHelper("Remove selected hotspot?\n\n" + listHotspots.SelectedItem.ToString(), this.Text) == true)
                {
                    HotspotMgr.Delete(listHotspots.SelectedIndex);
                    UpdateHotspotList();
                }
            }
        }

        private void btnClearHotspots_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you sure you wish to clear all hotspots?", this.Text) == true)
            {
                HotspotMgr.Clear();
                UpdateHotspotList();
            }
        }

        private void btnReverseHotspots_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you sure you wish to reverse all hotspots?", this.Text) == true)
            {
                HotspotMgr.Reverse();
                UpdateHotspotList();
                ZapLog("Reversed hotspots");
            }
        }

        private void btnLoadHotspots_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you sure you wish to load hotspots from an existing profile? \n\n Please note that this will DELETE all existing hotspots!", this.Text) == true)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                DialogResult results = new DialogResult();

                results = dialog.ShowDialog();

                if (results == DialogResult.OK)
                {
                    HotspotMgr.Clear();
                    UpdateHotspotList();

                    string tempLine;

                    StreamReader file = new System.IO.StreamReader(@dialog.FileName);
                    while ((tempLine = file.ReadLine()) != null)
                    {
                        if (tempLine.Contains("<Hotspot "))
                        {
                            if (!HotspotMgr.Exists(tempLine))
                            {
                                HotspotMgr.Add(tempLine);
                            }
                        }
                    }

                    UpdateHotspotList();
                    ZapLog("Loaded hotspots from profile");
                }
            }
        }

        #endregion

        #region Blackspots Tab

        private void btnAddBlackspot2_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you standing in the middle of the blackspot and have choosen the prefered radius?\nCurrent radius = " + numericUpDown1.Value.ToString(), this.Text) == true)
            {
                BlackspotMgr.Add();
                UpdateBlackspotList();
                ShowWoWChatMessage("Blackspot added at your location");
            }
        }

        // TODO - TRY CATCHES ON ALL BUTTONS WHICH CAN THROW EXCEPTIONS
        private void btnClearSelectedBlackspot_Click(object sender, EventArgs e)
        {
            if (listBlackspots.SelectedItems.Count > 0)
            {
                if (MessageBoxHelper("Are you sure you wish to remove the selected blackspot?\n\n" + listBlackspots.SelectedItem.ToString(), this.Text) == true)
                {
                    BlackspotMgr.Delete(listBlackspots.SelectedIndex);
                    UpdateBlackspotList();
                }
            }
        }

        private void btnClearBlackspots_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you sure you wish to clear all blackspots?", this.Text) == true)
            {
                BlackspotMgr.Clear();
            }

            UpdateBlackspotList();
        }

        private void btnLoadBlackspots_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you sure you wish to load blackspots from an existing profile?", this.Text) == true)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                DialogResult results = new DialogResult();

                results = dialog.ShowDialog();

                if (results == DialogResult.OK)
                {
                    string tempLine;

                    StreamReader file = new System.IO.StreamReader(@dialog.FileName);
                    while ((tempLine = file.ReadLine()) != null)
                    {
                        if (tempLine.Contains("<Blackspot "))
                        {
                            if (!BlackspotMgr.Exists(tempLine))
                            {
                                BlackspotMgr.Add(tempLine);
                            }
                        }
                    }

                    UpdateBlackspotList();
                    ZapLog("Loaded blackspots from profile");
                }
            }
        }


        #endregion

        #region Mailbox Tab

        private void btnAddMailbox2_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you standing next to a mailbox?", this.Text) == true)
            {
                MailboxMgr.Add();
                UpdateMailboxList();
                ShowWoWChatMessage("Mailbox added at your location");
            }
        }

        private void btnClearSelectedMailboxes_Click(object sender, EventArgs e)
        {
            if (listMailboxes.SelectedItems.Count > 0)
            {
                if (MessageBoxHelper("Remove selected mailbox?\n\n" + listMailboxes.SelectedItem.ToString(), this.Text) == true)
                {
                    MailboxMgr.Delete(listMailboxes.SelectedIndex);
                    UpdateMailboxList();
                }
            }
        }

        private void btnClearAllMailboxes_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Clear all mailboxes?", this.Text) == true)
            {
                MailboxMgr.Clear();
                UpdateMailboxList();
            }
        }

        // TODO - Loading of mailboxes and other items
        private void btnLoadMailboxes_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you sure you wish to load mailboxes from an existing profile?", this.Text) == true)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                DialogResult results = new DialogResult();

                results = dialog.ShowDialog();

                if (results == DialogResult.OK)
                {
                    string tempLine;

                    StreamReader file = new System.IO.StreamReader(@dialog.FileName);
                    while ((tempLine = file.ReadLine()) != null)
                    {
                        if (tempLine.Contains("<Mailbox "))
                        {
                            if (!MailboxMgr.Exists(tempLine))
                            {
                                MailboxMgr.Add(tempLine);
                            }
                        }
                    }

                    UpdateMailboxList();
                    ZapLog("Loaded mailboxes from profile");
                }
            }
        }

        #endregion

        #region Repair Tab


        private void AddRepairMerchant()
        {

            if (intMe.GotTarget == true)
            {
                string merchantName = intMe.CurrentTarget.Name;

                if (intMe.CurrentTarget.IsRepairMerchant == true)
                {

                    if (MessageBoxHelper("Do you want to add " + merchantName + " as a repair merchant?\n\n", "Confirm Repair NPC") == true)
                    {
                        ZapLog("Adding " + merchantName + " as a repair merchant");

                        RepairMgr.Add(intMe.CurrentTarget);
                        UpdateRepairList();

                        // Remove the WoW message because it is unlikely they will have WoW open while clicking add button
                        //ShowWoWChatMessage("Added " + vendorName + " as a repair NPC");
                    }
                }
                else
                {
                    if (merchantName == "Unknown NPC")
                    {
                        Logging.WriteDiagnostic("Uknown NPC info: ");
                        Logging.WriteDiagnostic("NPC GUID: " + intMe.CurrentTargetGuid.ToString());
                        Logging.WriteDiagnostic("NPC Name: " + intMe.CurrentTarget.Name.ToString());
                        Logging.WriteDiagnostic("NPC Loc: X=" + intMe.CurrentTarget.Location.X.ToString() + ", Y=" + intMe.CurrentTarget.Location.X.ToString() + ", Z=" + intMe.CurrentTarget.Location.Z.ToString());
                        Logging.WriteDiagnostic("NPC Type: " + intMe.CurrentTarget.Type.ToString());

                        ZapLog("NPC shows as 'Unknown NPC'. This is an existing issue with HB. Please send a log to BadWolf on the forums! Thank you.");
                    }
                    else
                    {
                        ZapLog("Cannot add " + merchantName + " as they are not a repair merchant");
                    }
                }
            }
            else if (intMe.GotTarget != true)
            {
                ZapLog("Cannot add repair merchant as you do not have a target!");
            }
        }

        // Merge this with other add repair
        private void btnAddRepair2_Click(object sender, EventArgs e)
        {

            AddRepairMerchant();
        }

        private void btnClearSelectedRepair_Click(object sender, EventArgs e)
        {
            if (RepairMgr.Count > 0)
            {
                if (MessageBoxHelper("Remove selected repair merchant?\n\n" + listRepair.SelectedItem.ToString(), "Confirm Deletion") == true)
                {
                    RepairMgr.Delete(listRepair.SelectedIndex);
                    UpdateRepairList();
                }
            }
        }

        private void btnClearAllRepair_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Clear all repair mechants?", "Confirm Deletion") == true)
            {
                RepairMgr.Clear();
            }

            UpdateRepairList();
        }

        private void btnLoadRepair_Click(object sender, EventArgs e)
        {
            if (MessageBoxHelper("Are you sure you wish to load repair vendors from an existing profile?", this.Text) == true)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                DialogResult results = new DialogResult();

                results = dialog.ShowDialog();

                if (results == DialogResult.OK)
                {
                    string tempLine;

                    StreamReader file = new System.IO.StreamReader(@dialog.FileName);
                    while ((tempLine = file.ReadLine()) != null)
                    {
                        if (tempLine.Contains("<Vendor "))
                        {
                            
                            if (!RepairMgr.Exists(tempLine))
                            {
                                RepairMgr.Add(tempLine);
                            }
                        }
                    }

                    UpdateRepairList();
                    ZapLog("Loaded vendors from profile");
                }
            }
        }

        #endregion

        #region Test Profile Tab

        private void btnGoToHotspot_Click(object sender, EventArgs e)
        {
            if(listTestHotspots.Items.Count < 1)
            {
                ZapLog("You do not have any hotspots to go to.");
                return;
            }

            if (listTestHotspots.SelectedItem == null)
            {
                ZapLog("You must select a hotspot to go to it!");
                return;
            }

            try
            {
                HotspotMgr.GoTo(HotspotMgr.AtIndex(listTestHotspots.SelectedIndex));
            }
            catch (Exception ex)
            {
                ZapException(ex, "HotspotMgr.GoTo");
            }
            
        }

        private void btnPrevHotspot_Click(object sender, EventArgs e)
        {
            if (listTestHotspots.Items.Count < 1)
            {
                ZapLog("You do not have any hotspots to go to.");
                return;
            }

            if (listTestHotspots.SelectedIndex == 0)
            {
                listTestHotspots.SelectedIndex = (listTestHotspots.Items.Count - 1);
            }
            else
            {
                listTestHotspots.SelectedIndex--;
            }

            try
            {
                HotspotMgr.GoTo(HotspotMgr.AtIndex(listTestHotspots.SelectedIndex));
            }
            catch (Exception ex)
            {
                ZapException(ex, "HotspotMgr.GoTo");
            }
        }

        private void btnNextHotspot_Click(object sender, EventArgs e)
        {
            if (listTestHotspots.Items.Count < 1)
            {
                ZapLog("You do not have any hotspots to go to.");
                return;
            }

            if (listTestHotspots.SelectedIndex == (listTestHotspots.Items.Count - 1))
            {
                listTestHotspots.SelectedIndex = 0;
            }
            else
            {
                listTestHotspots.SelectedIndex++;
            }

            try
            {
                HotspotMgr.GoTo(HotspotMgr.AtIndex(listTestHotspots.SelectedIndex));
            }
            catch (Exception ex)
            {
                ZapException(ex, "HotspotMgr.GoTo");
            }
            
        }

        private void btnReplaceHotspot_Click(object sender, EventArgs e)
        {
            int storedIndex = listTestHotspots.SelectedIndex;

            HotspotMgr.Replace(storedIndex);
            UpdateHotspotList();

            listTestHotspots.SelectedIndex = storedIndex;
            
        }

        private void btnAddHotspotAtIndex_Click(object sender, EventArgs e)
        {
            HotspotMgr.Add(listTestHotspots.SelectedIndex);
            UpdateHotspotList(); 

        }
        
        private void btnDeleteHotspot_Click(object sender, EventArgs e)
        {
            if (HotspotMgr.Count < 1)
            {
                ZapLog("You do not have any hotspots to go to.");
                return;
            }

            if (listTestHotspots.SelectedItem == null)
            {
                ZapLog("You must select a hotspot to delete");
                return;
            }

            HotspotMgr.Delete(listTestHotspots.SelectedIndex);
            UpdateHotspotList();

        }
        
        #endregion

        #region Settings Tab


        private void onTop_CheckedChanged(object sender, EventArgs e)
        {
            if (onTop.Checked == true)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }

        private void chkChatMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChatMessage.Checked == true)
            {
                //WoWHelper
                Logging.Write("Turning on chat messages.");
            }
            else
            {
                chatMessage = false;
                ZapLog("Turning off chat messages.");
            }
        }

        #endregion

        #region Helpers

        public bool MessageBoxHelper(string text, string title)
        {
            DialogResult dialog = MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MessageBoxHelper(string text, string title, MessageBoxButtons boxButtons)
        {
            DialogResult dialog = MessageBox.Show(text, title, boxButtons, MessageBoxIcon.Question);

            if ((dialog == DialogResult.Yes) || (dialog == DialogResult.OK))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MessageBoxHelper(string text, string title, MessageBoxButtons boxButtons, MessageBoxIcon boxIcon)
        {
            DialogResult dialog = MessageBox.Show(text, title, boxButtons, boxIcon);

            if ((dialog == DialogResult.Yes) || (dialog == DialogResult.OK))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ZapLog(string msg)
        {
            txtLog.Text += "[" + DateTime.Now.ToLongTimeString() + "] - " + msg + Environment.NewLine;
        }

        public void ZapException(Exception ex)
        {
            txtLog.Text += "[" + DateTime.Now.ToLongTimeString() + "] [ERROR] - " + ex.Message + Environment.NewLine;
        }

        public void ZapException(Exception ex, string exLocation)
        {
            txtLog.Text += "[" + DateTime.Now.ToLongTimeString() + "] [ERROR in " + exLocation + "] - " + ex.Message + Environment.NewLine;
        }

        public void ShowWoWChatMessage(string message)
        {
            //if (chatMessage == true)
            //{
            Lua.DoString("getglobal(\"ChatFrame1\"):AddMessage(\"|cff2dbbc4ZapRecorder2: |cffffffff " + message + "\",0, 0, 0, 0);");
            //}

        }

        #endregion




    }
}
