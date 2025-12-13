using Oracle.Core.CombatLog;
using Oracle.Core.Encounters;
using Oracle.Shared.Logging;
using Oracle.UI.Settings;
using Styx.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Oracle.UI
{
    public partial class SelectBossEncounter : Form
    {
        private static List<BossEncounter> BossEncounters; // list of Rotations

        public SelectBossEncounter()
        {
            InitializeComponent();

            LoadBossEncounters();

            var Y = 40;
            var X = 25;
            var columnX = 290;
            foreach (var encounter in BossEncounters)
            {
                var radio = new RadioButton();
                radio.Text = encounter.Name;

                radio.Location = new Point(X, Y);
                radio.Width = columnX - X;
                radio.Height = 20;

                radio.TextAlign = ContentAlignment.MiddleLeft;

                this.Controls.Add(radio);

                radio.Click += new EventHandler(radio_Click);

                Y += radio.Height;
            }

            this.ClientSize = new Size(300, Y + 50);
            this.MaximumSize = this.ClientSize;
            this.MinimumSize = this.ClientSize;
        }

        private void radio_Click(object sender, EventArgs e)
        {
            var radio = sender as RadioButton;

            if (CombatLogHandler.CurrentBossEncounter != null) CombatLogHandler.CurrentBossEncounter.Shutdown();

            CombatLogHandler.CurrentBossEncounter = BossEncounters.First(x => radio != null && x.Name == radio.Text);

            if (CombatLogHandler.CurrentBossEncounter != null) CombatLogHandler.CurrentBossEncounter.Initialize();

            if (CombatLogHandler.CurrentBossEncounter != null)
            {
                Logger.Output("[BossEncounters] Active Boss Encounter is {0}", CombatLogHandler.CurrentBossEncounter.Name);

                OracleSettings.Instance.MalkorokEncounter = (CombatLogHandler.CurrentBossEncounter.BossId == 71454);
            }
            
            this.Close();
        }

        private static void LoadBossEncounters()
        {
            try
            {
                BossEncounters = new List<BossEncounter>();
                BossEncounters.AddRange(new TypeLoader<BossEncounter>());

                if (BossEncounters.Count == 0)
                {
                    Logger.Output(" No Boss Encounters loaded to List");
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                foreach (var exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Oracle Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                var errorMessage = sb.ToString();
                Logger.Output(" Woops, we could not set the Boss Encounter.");
                Logger.Output(errorMessage);
            }
        }

        private void btnUnload_Click(object sender, EventArgs e)
        {
            if (CombatLogHandler.CurrentBossEncounter != null) CombatLogHandler.CurrentBossEncounter.Shutdown();
            this.Close();
        }
    }
}