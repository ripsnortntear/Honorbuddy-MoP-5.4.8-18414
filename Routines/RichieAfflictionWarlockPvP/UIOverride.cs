using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieAfflictionWarlock {

    public partial class UI {

        private void OverrideControls() {

            this.rightClickMovementOff.Text = "Turn off Right-click movement";

            tableLayoutPanel3.RowStyles[11].SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.ccFocusOrHealerBelow.Value = 0;
            this.ccFocusOrHealerBelow.Visible = false;
            this.label12.Visible = false;

            tableLayoutPanel3.RowStyles[4].SizeType = System.Windows.Forms.SizeType.AutoSize;
            this.peelBelowHp.Visible = false;
            this.label5.Visible = false;


            this.UseBanish.Checked = false;
            this.UseBanish.Visible = false;

            this.UsePetSpells.Checked = false;
            this.UsePetSpells.Visible = false;

            this.DrainSoulForShards.Checked = false;
            this.DrainSoulForShards.Visible = false;

            this.CCWhenBursting.Checked = false;
            this.CCWhenBursting.Visible = false;

            this.BGOwnageMode.Checked = false;
            this.BGOwnageMode.Visible = false;

            upperTrinketSlot.Top = UseTrinketWithDP.Top;
            lowerTrinketSlot.Top = UseTrinketWithDP.Top;

            this.ClientSize = new System.Drawing.Size(778, 450);
         
            pictureBox1.Visible = false; 
            linkLabel1.Visible = false;

        }
    }
}
