#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/UI/ChronicleViewPort.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Styx;

namespace Oracle.Healing.Chronicle.UI
{
    public partial class ChronicleViewPort : Form
    {
        // this shit is messy as fuck, but does the job

        public ChronicleViewPort()
        {
            InitializeComponent();
            Init();
        }

        private List<Chronicle.Encounter> _encounters;

        private void Init()
        {
            _encounters = new List<Chronicle.Encounter>();

            mboLoaded();

            if (cmboEncounters.SelectedValue != null) popit();

            lblInfo.Text = @"
Below is what is logged in the combat log.
Oracle uses this information to get the
average Direct heal.
";
        }

        private void mboLoaded()
        {
            if (Chronicle.Encounters.Count == 0) return;
            cmboEncounters.DataSource = new BindingSource(Chronicle.Encounters, null);
            cmboEncounters.DisplayMember = "Key";
            cmboEncounters.ValueMember = "Value";
        }

        private void popit()
        {
            try
            {
                var selectedEncounter = cmboEncounters.SelectedValue;
                _encounters.Add((Chronicle.Encounter)selectedEncounter);

                var players = new List<Chronicle.Player>();
                var healed = new List<Chronicle.Healed>();
                var spells = new List<Chronicle.Spell>();
                foreach (var player in _encounters.SelectMany(encounter => encounter.Players.Where(u => u.Value.Name == StyxWoW.Me.Name)))
                {
                    players.Add(player.Value);

                    healed.AddRange(player.Value.PlayerHealed.Select(heal => heal.Value));

                    spells.AddRange(player.Value.Healingspells.Select(spell => spell.Value));
                }

                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView3.DataSource = null;
                dataGridView4.DataSource = null;

                dataGridView1.DataSource = _encounters;
                dataGridView2.DataSource = players;
                dataGridView3.DataSource = healed;
                dataGridView4.DataSource = spells;
            }
            catch { } // eat em like a good bitch..
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mboLoaded();
            _encounters.Clear();
            popit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Chronicle.Instance.ResetEncounter();
        }

        private void cmboEncounters_SelectedIndexChanged(object sender, EventArgs e)
        {
            _encounters.Clear();
            popit();
        }
    }
}