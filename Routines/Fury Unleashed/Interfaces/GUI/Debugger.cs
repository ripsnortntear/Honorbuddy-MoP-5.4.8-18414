using System;
using System.Linq;
using System.Windows.Forms;
using FuryUnleashed.Core;
using Styx;

namespace FuryUnleashed.Interfaces.GUI
{
    public partial class DebuggerGui : Form
    {
        public DebuggerGui()
        {
            InitializeComponent();
        }

        private void Debugger_Load(object sender, EventArgs e)
        {

        }

        #region Update Information Void's
        private void UpdateMyAuras()
        {
            if (Unit.IsViable(StyxWoW.Me))
            {
                myaurasdatagrid.DataSource = StyxWoW.Me.Auras.Values.OrderBy(a => a.Name).ToList();
            }
        }

        private void UpdateMyAurasLinq()
        {
            if (Unit.IsViable(StyxWoW.Me))
            {
                myaurasdatagrid.DataSource = StyxWoW.Me.GetAllAuras().OrderBy(a => a.Name).ToList();
            }
        }

        private void UpdateTargetAuras()
        {
            if (Unit.IsViable(StyxWoW.Me.CurrentTarget))
            {
                mytargetaurasdatagrid.DataSource = StyxWoW.Me.CurrentTarget.Auras.Values.OrderBy(a => a.Name).ToList();
            }
        }

        private void UpdateTargetAurasLinq()
        {
            if (Unit.IsViable(StyxWoW.Me.CurrentTarget))
            {
                mytargetaurasdatagrid.DataSource = StyxWoW.Me.CurrentTarget.GetAllAuras().OrderBy(a => a.Name).ToList();
            }
        }

        private void UpdateMyCachedAuras()
        {
            if (Unit.IsViable(StyxWoW.Me))
            {
                mycachedaurasdatagrid.DataSource = Spell.CachedAuras.OrderBy(a => a.Name).ToList();
            }
        }

        private void UpdateTargetCachedAuras()
        {
            if (Unit.IsViable(StyxWoW.Me.CurrentTarget))
            {
                mytargetcachedaurasdatagrid.DataSource = Spell.CachedTargetAuras.OrderBy(a => a.Name).ToList();
            }
        }

        private void UpdateShieldBarrierSize()
        {
            if (Unit.IsViable(StyxWoW.Me))
            {
                shieldbarriersizetextbox.Text = Core.Utilities.Logger.PrintBarrierSize;
            }
        }

        private void UpdateShieldBlockSize()
        {
            if (Unit.IsViable(StyxWoW.Me))
            {
                shieldblocksizetextbox.Text = Core.Utilities.Logger.PrintBlockSize;
            }
        }

        private void UpdateDamageTaken()
        {
            if (Unit.IsViable(StyxWoW.Me))
            {
                damagetakentextbox.Text = Core.Utilities.Logger.PrintDamageTaken;
            }
        }

        private void UpdateVigilanceTarget()
        {
            if (Unit.IsViable(StyxWoW.Me))
            {
                vigilancetargettextbox.Text = Core.Utilities.Logger.PrintVigilanceTarget;
            }
        }

        private void UpdateCachedAttackableUnitsList()
        {
            if (Unit.IsViable(StyxWoW.Me))
            {
                //cachedattackableunitsdatagrid.DataSource = Unit.CachedAttackableUnitsList;
            }
        }

        private void UpdateCachedRaidMembersList()
        {
            if (Unit.IsViable(StyxWoW.Me))
            {
                //cachedraidmembersdatagrid.DataSource = Unit.CachedRaidMembersList;
            }
        }
        #endregion

        #region Errors
        private void myaurasdatagrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void mytargetaurasdatagrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void mycachedaurasdatagrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void mytargetcachedaurasdatagrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        #endregion

        // Update Buttons
        private void myaurasbutton_Click(object sender, EventArgs e)
        {
            UpdateMyAuras();
        }

        private void myaurasbutton2_Click(object sender, EventArgs e)
        {
            UpdateMyAurasLinq();
        }

        private void targetaurasbutton_Click(object sender, EventArgs e)
        {
            UpdateTargetAuras();
        }

        private void targetaurasbutton2_Click(object sender, EventArgs e)
        {
            UpdateTargetAurasLinq();
        }

        private void mycachedaurasbutton_Click(object sender, EventArgs e)
        {
            UpdateMyCachedAuras();
        }

        private void targetcachedaurasbutton_Click(object sender, EventArgs e)
        {
            UpdateTargetCachedAuras();
        }

        private void updateshieldbarrierbutton_Click(object sender, EventArgs e)
        {
            UpdateShieldBarrierSize();
        }

        private void updateshieldblockbutton_Click(object sender, EventArgs e)
        {
            UpdateShieldBlockSize();
        }

        private void updatedamagetakenbutton_Click(object sender, EventArgs e)
        {
            UpdateDamageTaken();
        }

        private void updatevigilancetargetbutton_Click(object sender, EventArgs e)
        {
            UpdateVigilanceTarget();
        }

        private void cachedattackableunitsbutton_Click(object sender, EventArgs e)
        {
            UpdateCachedAttackableUnitsList();
        }

        private void cachedraidmembersbutton_Click(object sender, EventArgs e)
        {
            UpdateCachedRaidMembersList();
        }
    }
}
