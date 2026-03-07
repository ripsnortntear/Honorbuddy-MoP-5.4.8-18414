using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PureRotation.Helpers;
using Styx;
using Styx.Common;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Settings.GUI
{
    public partial class Debug : Form
    {
        public Debug()
        {
            InitializeComponent();
        }

        private void DebugLoad(object sender, EventArgs e)
        {
            try
            {
                InitializePlainStyle();
                propertyGrid1.SelectedObject = Styx.StyxWoW.Me;
                Text = Styx.StyxWoW.Me.Name + " => no one";
                if (Styx.StyxWoW.Me.CurrentTarget != null || Styx.StyxWoW.Me.CurrentTarget == Styx.StyxWoW.Me)
                {
                    Text = Styx.StyxWoW.Me.Name + " => " + Styx.StyxWoW.Me.Name;
                }
                if (Styx.StyxWoW.Me.CurrentTarget == null || Styx.StyxWoW.Me.CurrentTarget.IsMe) return;
                propertyGrid2.SelectedObject = Styx.StyxWoW.Me.CurrentTarget;
                Text = Styx.StyxWoW.Me.Name + " => " + Styx.StyxWoW.Me.CurrentTarget.Name;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitializePlainStyle()
        {
            propertyGrid1.LineColor = Color.FromArgb(127, 0, 0); // Category
            propertyGrid1.CategoryForeColor = Color.FromArgb(255, 255, 255);

            propertyGrid2.LineColor = Color.FromArgb(127, 0, 0); // Category
            propertyGrid2.CategoryForeColor = Color.FromArgb(255, 255, 255);

            // hide the toolbar
            propertyGrid1.ToolbarVisible = false;
            propertyGrid2.ToolbarVisible = false;

            // Collapse All Grid Items
            propertyGrid1.CollapseAllGridItems();
            propertyGrid2.CollapseAllGridItems();

            //Button
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            if (File.Exists(Utilities.AssemblyDirectory + @"\Routines\PureRotation\Settings\GUI\refresh.png"))
                button1.BackgroundImage = new Bitmap(Utilities.AssemblyDirectory + @"\Routines\PureRotation\Settings\GUI\refresh.png");
        }

        private void InitializeStyle()
        {
            propertyGrid1.HelpBackColor = Color.FromArgb(27, 27, 27); // Description
            propertyGrid1.HelpForeColor = Color.FromArgb(24, 131, 209);// Description
            propertyGrid2.BackColor = Color.FromArgb(00, 00, 00); // Category
            propertyGrid1.ForeColor = Color.FromArgb(24, 131, 209); // Category
            propertyGrid1.LineColor = Color.FromArgb(27, 27, 27); // Category
            propertyGrid1.CategoryForeColor = Color.FromArgb(24, 131, 209);
            propertyGrid1.ViewBackColor = Color.FromArgb(57, 57, 57); // properties
            propertyGrid1.ViewForeColor = Color.FromArgb(203, 205, 217); // properties

            propertyGrid2.HelpBackColor = Color.FromArgb(27, 27, 27);// Description
            propertyGrid2.HelpForeColor = Color.FromArgb(24, 131, 209); // Description
            propertyGrid1.BackColor = Color.FromArgb(00, 00, 00); // Category
            propertyGrid2.ForeColor = Color.FromArgb(255, 255, 255); // Category
            propertyGrid2.LineColor = Color.FromArgb(27, 27, 27); // Category
            propertyGrid2.CategoryForeColor = Color.FromArgb(24, 131, 209);
            propertyGrid2.ViewBackColor = Color.FromArgb(57, 57, 57); // properties
            propertyGrid2.ViewForeColor = Color.FromArgb(203, 205, 217); // properties

            // hide the toolbar
            propertyGrid1.ToolbarVisible = false;
            propertyGrid2.ToolbarVisible = false;

            // Collapse All Grid Items
            propertyGrid1.CollapseAllGridItems();
            propertyGrid2.CollapseAllGridItems();

            //Panel1
            this.BackColor = Color.FromArgb(16, 16, 16);

            //Button
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            if (File.Exists(Utilities.AssemblyDirectory + @"\Routines\PureRotation\Settings\GUI\refresh.png"))
                button1.BackgroundImage = new Bitmap(Utilities.AssemblyDirectory + @"\Routines\PureRotation\Settings\GUI\refresh.png");
        }

        private void Button1Click(object sender, EventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObject = Styx.StyxWoW.Me;
                Text = Styx.StyxWoW.Me.Name + " => no one";
                if (Styx.StyxWoW.Me.CurrentTarget != null || Styx.StyxWoW.Me.CurrentTarget == Styx.StyxWoW.Me)
                {
                    Text = Styx.StyxWoW.Me.Name + " => " + Styx.StyxWoW.Me.Name;
                }
                if (Styx.StyxWoW.Me.CurrentTarget == null)
                    propertyGrid2.SelectedObject = null;
                else
                {
                    propertyGrid2.SelectedObject = Styx.StyxWoW.Me.CurrentTarget;
                    Text = Styx.StyxWoW.Me.Name + " => " + Styx.StyxWoW.Me.CurrentTarget.Name;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (StyxWoW.Me.CurrentTarget != null) DumpAuraTables(StyxWoW.Me.CurrentTarget);
        }

        private static void DumpAuraTables(WoWUnit target)
        {
            using (StyxWoW.Memory.AcquireFrame())
            {
                Logger.CombatLog("----------- Buffs ------------");
                using (var timer1 = new PerformanceTimer("Buffs"))
                {
                    timer1.Start();
                    foreach (KeyValuePair<string, WoWAura> aura in target.Buffs)
                    {
                        Logger.CombatLog("{0}", aura.Key);
                    }

                    Logger.CombatLog("Buffs: {0}ms", timer1.ElapsedMilliseconds);
                }

                Logger.CombatLog("----------- Debuffs ------------");
                using (var timer2 = new PerformanceTimer("Debuffs"))
                {
                    timer2.Start();
                    foreach (KeyValuePair<string, WoWAura> aura in target.Debuffs)
                    {
                        Logger.CombatLog("{0}", aura.Key);
                    }

                    Logger.CombatLog("Debuffs: {0}ms", timer2.ElapsedMilliseconds);
                }

                Logger.CombatLog("----------- ActiveAuras ------------");
                using (var timer3 = new PerformanceTimer("ActiveAuras"))
                {
                    timer3.Start();
                    foreach (KeyValuePair<string, WoWAura> aura in target.ActiveAuras)
                    {
                        Logger.CombatLog("{0}", aura.Key);
                    }

                    Logger.CombatLog("ActiveAuras: {0}ms", timer3.ElapsedMilliseconds);
                }

                Logger.CombatLog("----------- PassiveAuras ------------");
                using (var timer4 = new PerformanceTimer("PassiveAuras"))
                {
                    timer4.Start();
                    foreach (KeyValuePair<string, WoWAura> aura in target.PassiveAuras)
                    {
                        Logger.CombatLog("{0}", aura.Key);
                    }

                    Logger.CombatLog("PassiveAuras: {0}ms", timer4.ElapsedMilliseconds);
                }

                Logger.CombatLog("----------- Auras ------------");
                using (var timer5 = new PerformanceTimer("PassiveAuras"))
                {
                    timer5.Start();
                    foreach (KeyValuePair<string, WoWAura> aura in target.Auras)
                    {
                        Logger.CombatLog("{0}", aura.Key);
                    }

                    Logger.CombatLog("Auras: {0}ms", timer5.ElapsedMilliseconds);
                }
                Logger.CombatLog("----------- GetAllAuras ------------");
                using (var timer6 = new PerformanceTimer("GetAllAuras"))
                {
                    timer6.Start();
                    foreach (WoWAura aura in target.GetAllAuras())
                    {
                        Logger.CombatLog("{0}", aura.Name);
                    }

                    Logger.CombatLog("GetAllAuras: {0}ms", timer6.ElapsedMilliseconds);
                }
            }
        }
    }
}