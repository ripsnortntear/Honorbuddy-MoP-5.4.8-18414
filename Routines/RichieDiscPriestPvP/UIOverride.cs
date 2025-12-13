using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieDiscPriestPvP {

    public partial class UI {

        private void OverrideControls() {

            this.rightClickMovementOff.Text = "Turn off Right-click movement";

            tableLayoutPanel3.RowStyles[0].SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.desperatePrayerHp.Visible = false;
            this.label1.Visible = false;

            tableLayoutPanel3.RowStyles[4].SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.peelBelowHp.Visible = false;
            this.label5.Visible = false;

            tableLayoutPanel3.RowStyles[11].SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.VoidShiftBelowHp.Visible = false;
            this.label12.Visible = false;

            tableLayoutPanel3.RowStyles[13].SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.PWSBlanketAboveHp.Visible = false;
            this.label14.Visible = false;

            tableLayoutPanel3.RowStyles[14].SizeType = System.Windows.Forms.SizeType.AutoSize;           
            this.AtonementHealingAbove.Visible = false;
            this.label15.Visible = false;

            tableLayoutPanel3.RowStyles[15].SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.TotemAndPlayerKillingAboveHp.Visible = false;
            this.label16.Visible = false;

            this.shadowfiend.Checked = false;
            this.shadowfiend.Visible = false;

            this.lvl90safeCheck.Checked = false;
            this.lvl90safeCheck.Visible = false;

            this.VoidShiftGrief.Checked = false;
            this.VoidShiftGrief.Visible = false;

            this.OnlyUseSmite.Checked = false;
            this.OnlyUseSmite.Visible = false;

            this.massDispel.Checked = false;
            this.massDispel.Visible = false;

            this.PIonCD.Checked = false;
            this.PIonCD.Visible = false;

            this.LoF.Checked = false;
            this.LoF.Visible = false;

            upperTrinketSlot.Top = UseTrinketWithDP.Top;
            lowerTrinketSlot.Top = UseTrinketWithDP.Top;

            this.ClientSize = new System.Drawing.Size(786, 400);
         
            pictureBox1.Visible = false; 
            linkLabel1.Visible = false;

        }
    }
}
