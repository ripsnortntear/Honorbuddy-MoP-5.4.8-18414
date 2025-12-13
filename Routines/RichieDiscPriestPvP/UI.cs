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

namespace RichieDiscPriestPvP {

    public partial class UI : Form {
	
        public UI() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

            if (File.Exists(Utilities.AssemblyDirectory + @"\Routines\RichieDiscPriestPvP\donate.gif")) {
                pictureBox1.ImageLocation = Utilities.AssemblyDirectory + @"\Routines\RichieDiscPriestPvP\donate.gif";
            }

            desperatePrayerHp.Value = DiscSettings.Instance.DesperatePrayer;
            PoMHp.Value = DiscSettings.Instance.PoMHp;
            painSuppressionHp.Value = DiscSettings.Instance.painSuppressionHp;
            penanceHp.Value = DiscSettings.Instance.penanceHp;
            healthstoneHp.Value = DiscSettings.Instance.HealthstonePercent;
            InstantFlashHealHP.Value = DiscSettings.Instance.InstantFlashHealBelowHP;
            innerFocusHp.Value = DiscSettings.Instance.innerFocusHp;
            FlashHealHp.Value = DiscSettings.Instance.FlashHealHp;
            peelBelowHp.Value = DiscSettings.Instance.PeelSelf;
            barrierHp.Value = DiscSettings.Instance.barrierHp;
            VoidShiftBelowHp.Value = DiscSettings.Instance.VoidShiftBelowHp;
            renewHp.Value = DiscSettings.Instance.renewHp;
            PWSHp.Value = DiscSettings.Instance.PWSHp;
            PWSBlanketAboveHp.Value = DiscSettings.Instance.PWSBlanketAboveHp;
            AtonementHealingAbove.Value = DiscSettings.Instance.AtonementHealingAbove;
            TotemAndPlayerKillingAboveHp.Value = DiscSettings.Instance.TotemAndPlayerKillingAboveHp;            
            interval.Value = DiscSettings.Instance.SearchInterval;

            lvl90safeCheck.Checked = DiscSettings.Instance.Lvl90SafeCheck;
            VoidShiftGrief.Checked = DiscSettings.Instance.GriefPpl;
            DontCancelDominateMind.Checked = DiscSettings.Instance.DontCancelDominateMind;
            fearWard.Checked = DiscSettings.Instance.FearWardSelf;
            massDispel.Checked = DiscSettings.Instance.AutoMassDispel;
            face.Checked = DiscSettings.Instance.AutoFace;
            fade.Checked = DiscSettings.Instance.FadeAuto;
            pwf.Checked = DiscSettings.Instance.PowerWordFortitude;
            shadowfiend.Checked = DiscSettings.Instance.ShadowfiendAuto;
            PIonCD.Checked = DiscSettings.Instance.PowerInfusionOnCD;
            LoF.Checked = DiscSettings.Instance.LoF;
            UseRacial.Checked = DiscSettings.Instance.UseRacial;
            Shackle.Checked = DiscSettings.Instance.Shackle;
            rightClickMovementOff.Checked = DiscSettings.Instance.rightClickMovementOff;
            UseTrinketWithDP.Checked = DiscSettings.Instance.useTrinket;
            upperTrinketSlot.Checked = DiscSettings.Instance.trinketSlotNumber == 13;
            lowerTrinketSlot.Checked = DiscSettings.Instance.trinketSlotNumber == 14;
            OnlyUseSmite.Checked = DiscSettings.Instance.OnlyUseSmite;
            //Placeholder2.Checked = DiscSettings.Instance.PWSBlanketAboveHp;
            UseAngelicFeatherOnCD.Checked = DiscSettings.Instance.AngelicFeatherDelay == 6;
            
            OverrideControls();
        }

        private void Cancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void defaults_Click(object sender, EventArgs e) {
            desperatePrayerHp.Value = 60;
            PoMHp.Value = 90;
            painSuppressionHp.Value = 35;
            penanceHp.Value = 85;
            healthstoneHp.Value = 35;
            InstantFlashHealHP.Value = 80;
            innerFocusHp.Value = 40;
            FlashHealHp.Value = 50;
            peelBelowHp.Value = 80;
            barrierHp.Value = 40;
            VoidShiftBelowHp.Value = 30;
            renewHp.Value = 90;
            PWSHp.Value = 90;
            PWSBlanketAboveHp.Value = 80;
            AtonementHealingAbove.Value = 80;
            TotemAndPlayerKillingAboveHp.Value = 40;
            interval.Value = 400;

            lvl90safeCheck.Checked = true;
            VoidShiftGrief.Checked = true;
            OnlyUseSmite.Checked = true;
            DontCancelDominateMind.Checked = false;
            fearWard.Checked = true;
            massDispel.Checked = true;
            face.Checked = true;
            fade.Checked = true;
            pwf.Checked = true;
            shadowfiend.Checked = true;
            PIonCD.Checked = true;
            LoF.Checked = true;
            UseRacial.Checked = true;
            Shackle.Checked = true;
            rightClickMovementOff.Checked = true;
            UseTrinketWithDP.Checked = true;
            upperTrinketSlot.Checked = true;
            lowerTrinketSlot.Checked = false;
            OnlyUseSmite.Checked = false;
            //Placeholder2.Checked = false;
            UseAngelicFeatherOnCD.Checked = false;

            OverrideControls();

        }

        private void save_Click(object sender, EventArgs e) {

            OverrideControls();

            DiscSettings.Instance.DesperatePrayer = (int)desperatePrayerHp.Value;
            DiscSettings.Instance.PoMHp = (int)PoMHp.Value;
            DiscSettings.Instance.painSuppressionHp = (int)painSuppressionHp.Value;
            DiscSettings.Instance.penanceHp = (int)penanceHp.Value;
            DiscSettings.Instance.HealthstonePercent = (int)healthstoneHp.Value;
            DiscSettings.Instance.InstantFlashHealBelowHP = (int)InstantFlashHealHP.Value;
            DiscSettings.Instance.innerFocusHp = (int)innerFocusHp.Value;
            DiscSettings.Instance.FlashHealHp = (int)FlashHealHp.Value;
            DiscSettings.Instance.PeelSelf = (int)peelBelowHp.Value;
            DiscSettings.Instance.barrierHp = (int)barrierHp.Value;
            DiscSettings.Instance.VoidShiftBelowHp = (int)VoidShiftBelowHp.Value;
            DiscSettings.Instance.renewHp = (int)renewHp.Value;
            DiscSettings.Instance.PWSHp = (int)PWSHp.Value;
            DiscSettings.Instance.PWSBlanketAboveHp = (int)PWSBlanketAboveHp.Value;
            DiscSettings.Instance.AtonementHealingAbove = (int)AtonementHealingAbove.Value;
            DiscSettings.Instance.TotemAndPlayerKillingAboveHp = (int)TotemAndPlayerKillingAboveHp.Value;
            DiscSettings.Instance.SearchInterval = (int)interval.Value;

            DiscSettings.Instance.Lvl90SafeCheck = lvl90safeCheck.Checked;
            DiscSettings.Instance.GriefPpl = VoidShiftGrief.Checked;
            DiscSettings.Instance.DontCancelDominateMind = DontCancelDominateMind.Checked;
            DiscSettings.Instance.FearWardSelf = fearWard.Checked;
            DiscSettings.Instance.AutoMassDispel = massDispel.Checked;
            DiscSettings.Instance.AutoFace = face.Checked;
            DiscSettings.Instance.FadeAuto = fade.Checked;
            DiscSettings.Instance.OnlyUseSmite = OnlyUseSmite.Checked;
            DiscSettings.Instance.PowerWordFortitude = pwf.Checked;
            DiscSettings.Instance.ShadowfiendAuto = shadowfiend.Checked;
            DiscSettings.Instance.PowerInfusionOnCD = PIonCD.Checked;
            DiscSettings.Instance.LoF = LoF.Checked;
            DiscSettings.Instance.UseRacial = UseRacial.Checked;
            DiscSettings.Instance.Shackle = Shackle.Checked;
            DiscSettings.Instance.rightClickMovementOff = rightClickMovementOff.Checked;
            DiscSettings.Instance.useTrinket = UseTrinketWithDP.Checked;
            DiscSettings.Instance.trinketSlotNumber = upperTrinketSlot.Checked ? 13 : 14;
            //DiscSettings.Instance.PWSBlanketAboveHp = Placeholder2.Checked;
            DiscSettings.Instance.AngelicFeatherDelay = UseAngelicFeatherOnCD.Checked ? 6 : 10;

            if (DiscSettings.Instance.rightClickMovementOff && Styx.CommonBot.TreeRoot.Current.Name != "BGBuddy") {
                Styx.WoWInternals.Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 0\")');");
            } else {
                Styx.WoWInternals.Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 1\")');");
            }

            DiscSettings.Instance.Save();
            Logging.Write("----------------------------------");
            Logging.Write("Your settings have been saved");
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
