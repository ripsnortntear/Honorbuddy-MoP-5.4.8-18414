using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Oracle.Healing;
using Oracle.Healing.Chronicle;
using Oracle.Shared.Logging;
using Styx;
using System;
using System.Windows.Forms;

namespace Oracle.Shared.Utilities.Clusters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Init(object sender, EventArgs e)
        {

            //var players = Chronicle.Players;
            //var healingspells = players.Select(sp => sp.Healingspells);

            //comboBox1.DataSource = players.Select(p => p.Name).ToList();
            //var spells = healingspells.SelectMany(d => d.Values).ToList();
            //comboBox2.DataSource = spells.Select(p => p.Name).ToList();

            //var player = HealTracker.Players.Find(a => a.Name == (string) comboBox1.SelectedValue);
            //var spell = spells.First(a => a.Name == (string)comboBox2.SelectedValue);

            //var g = this.propertyGrid1;
            //var s = player;
            //g.SelectedObject = s;

            //comboBox2.SelectedIndexChanged += onSelectedIndexChanged;
            //onSelectedIndexChanged(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClusterManager.Pulse();
            if (StyxWoW.Me.CurrentTarget != null)
                Logger.Output("Distance: {0}", StyxWoW.Me.CurrentTarget.Distance);
        }

        private void onSelectedIndexChanged(object sender, EventArgs e)
        {
            //var players = HealTracker.Players;
            //var healingspells = players.Select(sp => sp.Healingspells);
            //var spells = healingspells.SelectMany(d => d.Values).ToList();
            //var spell = spells.First(a => a.Name == (string)comboBox2.SelectedValue);

            //label18.Text = string.Format("{0:N0}", spell.Min);
            //label17.Text = string.Format("{0:N0}", spell.Max);
            //label16.Text = string.Format("{0:N0}", (spell.Healing / spell.Hits));
            //label15.Text = string.Format("{0:P0}", (spell.Critical / spell.Hits) * 100);
            //label14.Text = string.Format("{0:P0}", spell.Overhealing / (spell.Overhealing + spell.Healing) * 100);
            //label13.Text = string.Format("{0:P0}", spell.Absorbed / (spell.Overhealing + spell.Healing) * 100);
            //label12.Text = string.Format("{0:N0}", spell.Healing);
            //label11.Text = string.Format("{0:N0}", spell.Hits);
            //label10.Text = string.Format("{0:N0}", spell.Shielding);
            
        }

        private void updateSpellHealing(object sender, EventArgs e)
        {
            //var players = HealTracker.Players;
            //var healingspells = players.Select(sp => sp.Healingspells);
            //var spells = healingspells.SelectMany(d => d.Values).ToList();
            //var player = HealTracker.Players.Find(a => a.Name == (string) comboBox1.SelectedValue);

            //groupBox2.Controls.Clear();

            //Point p = new Point(10, 17); //initial location - adjust it suitably
            //foreach (var spell in spells.OrderByDescending(s => s.Healing))
            //{
            //    var label = new Label();

            //    var healing = string.Format("{0:#,##.#}", spell.Healing);
            //    var playerHealing = string.Format("{0:P0}", ((spell.Healing / player.Healing) * 100));
            //    var name = string.Format("{0,-7}", spell.Name);

            //    label.Text = name + " - " + healing + " (" + playerHealing + ") ";
            //    label.Width = 229;
            //    label.Location = new Point(p.X, p.Y);
            //    label.Tag = spell.Name; //optional
            //    groupBox2.Controls.Add(label);

            //    p.Y += label.Height - 2; //to align vertically
            //    //p.X += label.Width + 18; //to align horizontally. 
            //}
        }
    }
}