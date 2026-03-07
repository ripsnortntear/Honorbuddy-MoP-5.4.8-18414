using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PureRotation.Managers;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Settings.GUI
{
    public partial class HealingDebug : Form
    {
        public HealingDebug()
        {
            InitializeComponent();
            if (CachedUnits.HealList != null) SetHealBinding(CachedUnits.HealList);
            if (CachedUnits.TankList != null) SetTankBinding(CachedUnits.TankList);
            if (CachedUnits.HealerList != null) SetHealerBinding(CachedUnits.HealerList);
            if (CachedUnits.AttackableUnits != null) SetAttackableUnitBinding(CachedUnits.AttackableUnits);
        }

        public void SetHealBinding(List<WoWObject> list)
        {
            listBox1.DataSource = null;
            listBox1.DataSource = list;
        }

        public void SetPrioBinding(Dictionary<WoWPlayer, double> list)
        {   //#Mirabis : Name - HP - Priority
            var lists = list.OrderByDescending(x => x.Value).Select(p => new { p.Key.Name, HP = p.Key.HealthPercent, Prio = (int)p.Value, }).ToList();
            listBox3.DataSource = null;
            listBox3.DataSource = lists;
        }

        public void SetTankBinding(List<WoWPlayer> list)
        {
            listBox2.DataSource = null;
            listBox2.DataSource = list;
        }

        public void SetHealerBinding(List<WoWPlayer> list)
        {
            listBox4.DataSource = null;
            listBox4.DataSource = list;
        }

        public void SetAttackableUnitBinding(List<WoWUnit> list)
        {
            listBox5.DataSource = null;
            listBox5.DataSource = list;
        }

        public void refresh()
        {
            if (CachedUnits.HealList != null) SetHealBinding(CachedUnits.HealList);
            if (CachedUnits.TankList != null) SetTankBinding(CachedUnits.TankList);
            if (CachedUnits.HealerList != null) SetHealerBinding(CachedUnits.HealerList);
            if (CachedUnits.AttackableUnits != null) SetAttackableUnitBinding(CachedUnits.AttackableUnits);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            refresh();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            CachedUnits.UpdateCache();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            refresh();
        }

        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }
    }
}