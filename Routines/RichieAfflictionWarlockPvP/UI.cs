using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Styx.Common;

namespace RichieAfflictionWarlock {

    public partial class UI : Form {
	
        public UI() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

            if (File.Exists(Utilities.AssemblyDirectory + @"\Routines\RichieShadowPriest\donate.gif")) {
                pictureBox1.ImageLocation = Utilities.AssemblyDirectory + @"\Routines\RichieShadowPriest\donate.gif";
            }

            SacrificialPactHp.Value = AfflictionSettings.Instance.SacrificialPactHP;
            DarkBargainHp.Value = AfflictionSettings.Instance.DarkBargainHP;            
            healthstoneHp.Value = AfflictionSettings.Instance.HealthstonePercent;
            UnendingResolveHp.Value = AfflictionSettings.Instance.UnendingResolveHp;
            DrainLifeBelowHp.Value = AfflictionSettings.Instance.DrainLifeBelowHp;
            DontCastSpellsForHpBelowHp.Value = AfflictionSettings.Instance.DontCastSpellsForHpBelowHp;
            LifeTapOnMana.Value = AfflictionSettings.Instance.LifeTapOnMana;
            AgonyRefresh.Value = AfflictionSettings.Instance.AgonyRefresh;
            CorruptionRefresh.Value = AfflictionSettings.Instance.CorruptionRefresh;
            UnstableAfflictionRefresh.Value = AfflictionSettings.Instance.UnstableAfflictionRefresh;
            ccFocusOrHealerBelow.Value = AfflictionSettings.Instance.CCFocusOrHealerBelow;
            peelBelowHp.Value = AfflictionSettings.Instance.PeelSelf;
            interval.Value = AfflictionSettings.Instance.SearchInterval;
            PreferredPet.SelectedIndex = AfflictionSettings.Instance.PreferredPet;


            face.Checked = AfflictionSettings.Instance.AutoFace;
            UseBanish.Checked = AfflictionSettings.Instance.UseBanish;
            UsePetSpells.Checked = AfflictionSettings.Instance.UsePetSpells;
            multidot.Checked = AfflictionSettings.Instance.Multidot;
            WotF.Checked = AfflictionSettings.Instance.WotF;
            rightClickMovementOff.Checked = AfflictionSettings.Instance.rightClickMovementOff;
            UseTrinketWithDP.Checked = AfflictionSettings.Instance.useTrinketWithDP;
            upperTrinketSlot.Checked = AfflictionSettings.Instance.trinketSlotNumber == 13;
            lowerTrinketSlot.Checked = AfflictionSettings.Instance.trinketSlotNumber == 14;
            DrainSoulForShards.Checked = AfflictionSettings.Instance.DrainSoulForShards;
            BurstOnCD.Checked = AfflictionSettings.Instance.BurstOnCD;
            CCWhenBursting.Checked = AfflictionSettings.Instance.CCWhenBursting;
            Soulstone.Checked = AfflictionSettings.Instance.Soulstone;
            BGOwnageMode.Checked = AfflictionSettings.Instance.BGOwnageMode;
            FakeCast.Checked = AfflictionSettings.Instance.FakeCast;
            
            OverrideControls();
        }

        private void Cancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void defaults_Click(object sender, EventArgs e) {
            SacrificialPactHp.Value = 60;
            DarkBargainHp.Value = 40;
            healthstoneHp.Value = 35;
            UnendingResolveHp.Value = 40;
            DontCastSpellsForHpBelowHp.Value = 40;
            DrainLifeBelowHp.Value = 40;
            LifeTapOnMana.Value = 50;
            AgonyRefresh.Value = 10000;
            CorruptionRefresh.Value = 7000;
            UnstableAfflictionRefresh.Value = 5000;
            ccFocusOrHealerBelow.Value = 35;
            peelBelowHp.Value = 80;
            interval.Value = 400;
            PreferredPet.SelectedIndex = 0;

            face.Checked = true;
            UseBanish.Checked = true;
            multidot.Checked = true;
            UsePetSpells.Checked = true;
            WotF.Checked = true;
            rightClickMovementOff.Checked = true;
            UseTrinketWithDP.Checked = true;
            upperTrinketSlot.Checked = true;
            lowerTrinketSlot.Checked = false;
            DrainSoulForShards.Checked = false;
            BurstOnCD.Checked = true;
            CCWhenBursting.Checked = false;
            Soulstone.Checked = true;
            BGOwnageMode.Checked = false;
            FakeCast.Checked = true;

            OverrideControls();
        }

        private void save_Click(object sender, EventArgs e) {

            OverrideControls();

            AfflictionSettings.Instance.SacrificialPactHP = (int)SacrificialPactHp.Value;
            AfflictionSettings.Instance.LifeTapOnMana = (int)LifeTapOnMana.Value;
            AfflictionSettings.Instance.DarkBargainHP = (int)DarkBargainHp.Value;
            AfflictionSettings.Instance.AgonyRefresh = (int)AgonyRefresh.Value;
            AfflictionSettings.Instance.HealthstonePercent = (int)healthstoneHp.Value;
            AfflictionSettings.Instance.CCFocusOrHealerBelow = (int)ccFocusOrHealerBelow.Value;
            AfflictionSettings.Instance.CorruptionRefresh = (int)CorruptionRefresh.Value;
            AfflictionSettings.Instance.PeelSelf = (int)peelBelowHp.Value;
            AfflictionSettings.Instance.DontCastSpellsForHpBelowHp = (int)DontCastSpellsForHpBelowHp.Value;
            AfflictionSettings.Instance.UnstableAfflictionRefresh = (int)UnstableAfflictionRefresh.Value;
            AfflictionSettings.Instance.UnendingResolveHp = (int)UnendingResolveHp.Value;
            AfflictionSettings.Instance.SearchInterval = (int)interval.Value;
            AfflictionSettings.Instance.DrainLifeBelowHp = (int)DrainLifeBelowHp.Value;
            AfflictionSettings.Instance.PreferredPet = PreferredPet.SelectedIndex;

            AfflictionSettings.Instance.AutoFace = face.Checked;
            AfflictionSettings.Instance.UseBanish = UseBanish.Checked;
            AfflictionSettings.Instance.Multidot = multidot.Checked;
            AfflictionSettings.Instance.UsePetSpells = UsePetSpells.Checked;
            AfflictionSettings.Instance.useTrinketWithDP = UseTrinketWithDP.Checked;
            AfflictionSettings.Instance.trinketSlotNumber = upperTrinketSlot.Checked ? 13 : 14;
            AfflictionSettings.Instance.rightClickMovementOff = rightClickMovementOff.Checked;
            AfflictionSettings.Instance.WotF = WotF.Checked;
            AfflictionSettings.Instance.DrainSoulForShards = DrainSoulForShards.Checked;
            AfflictionSettings.Instance.BurstOnCD = BurstOnCD.Checked;
            AfflictionSettings.Instance.CCWhenBursting = CCWhenBursting.Checked;
            AfflictionSettings.Instance.Soulstone = Soulstone.Checked;
            AfflictionSettings.Instance.BGOwnageMode = BGOwnageMode.Checked;
            AfflictionSettings.Instance.FakeCast = FakeCast.Checked;

            if (AfflictionSettings.Instance.rightClickMovementOff && Styx.CommonBot.TreeRoot.Current.Name != "BGBuddy") {
                Styx.WoWInternals.Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 0\")');");
            } else {
                Styx.WoWInternals.Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 1\")');");
            }


            AfflictionSettings.Instance.Save();
            Logging.Write("----------------------------------");
            Logging.Write("Your settings have been saved");
            Main.printSettings();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=E6RB38DW4EVGA");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://richiecr.blogspot.hu/");
        }
    }
}
