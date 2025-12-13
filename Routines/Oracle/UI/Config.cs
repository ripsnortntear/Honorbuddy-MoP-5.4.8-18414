#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-17 09:11:30 +1000 (Tue, 17 Sep 2013) $
 * $ID$
 * $Revision: 226 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/Config.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using System.Collections.Generic;
using Oracle.Core.Spells.Debuffs;
using Oracle.Healing.Chronicle;
using Oracle.Healing.Chronicle.UI;
using Oracle.Shared.Logging;
using Oracle.UI.Settings;
using Styx;
using Styx.Common;
using Styx.WoWInternals;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Oracle.UI
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private bool DispelSettingsChanged { get; set; }

        private void ConfigurationFormLoad(object sender, EventArgs e)
        {
            var LogoToLoad = string.Format("{0}\\Routines\\Oracle\\UI\\logo.png", Utilities.AssemblyDirectory);
            var ReadMeUrl = string.Format("{0}\\Routines\\Oracle\\UI\\ReadMe.html", Utilities.AssemblyDirectory);

            if (File.Exists(LogoToLoad))
                logo.Image = Image.FromFile(LogoToLoad);

            if (File.Exists(ReadMeUrl))
                webReadme.Url = new Uri(ReadMeUrl);

            // setup the bindings
            pgMain.SelectedObject = OracleSettings.Instance;
            OracleSettings main = OracleSettings.Instance;
            Styx.Helpers.Settings toSelect = null;
            switch (StyxWoW.Me.Class)
            {
                case WoWClass.Paladin:
                    toSelect = main.Paladin;
                    break;

                case WoWClass.Priest:
                    if (StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline)
                    {
                        Logger.Output("Priest Disc settings loaded {0}", StyxWoW.Me.Specialization);
                        toSelect = main.DiscPriest;
                    }
                    else if (StyxWoW.Me.Specialization == WoWSpec.PriestHoly)
                    {
                        Logger.Output("Priest holy settings loaded {0}", StyxWoW.Me.Specialization);
                        toSelect = main.HolyPriest;
                    }

                    break;

                case WoWClass.Shaman:
                    toSelect = main.Shaman;
                    break;

                case WoWClass.Druid:
                    toSelect = main.Druid;
                    break;

                case WoWClass.Monk:
                    toSelect = main.Monk;
                    break;
            }

            if (toSelect != null)
            {
                pgClass.SelectedObject = toSelect;
            }

            InitializePlainStyle();

            PopulateHealCalculationList();

            dataGridDispel.DataSource = DispelableSpell.Instance.SpellList.Spells;

            DispelSettingsChanged = false;

            lblDispelInformation.Text = @" Click on the Spell ID's to Edit the Cell. You can Also enter new
  records by clicking on any spell ID.";
        }

        #region Main Form Settings

        private static Color GetClassForeColor()
        {
            switch (StyxWoW.Me.Class)
            {
                case WoWClass.Priest:
                case WoWClass.Paladin:
                case WoWClass.Druid:
                    return Color.FromArgb(0, 0, 0);
                default:
                    return Color.FromArgb(255, 255, 255);
            }
        }

        public static Color GetClassColor()
        {
            switch (StyxWoW.Me.Class)
            {
                case WoWClass.Druid:
                    return Color.FromArgb(255, 124, 10);
                case WoWClass.Monk:
                    return Color.FromArgb(0, 132, 93);
                case WoWClass.Paladin:
                    return Color.FromArgb(244, 140, 186);
                case WoWClass.Priest:
                    return Color.FromArgb(255, 255, 255);
                case WoWClass.Shaman:
                    return Color.FromArgb(35, 89, 221);
                default:
                    return Color.FromArgb(47, 47, 47);
            }
        }

        public static Color GetClassColor(WoWClass Class)
        {
            switch (Class)
            {
                case WoWClass.DeathKnight:
                    return Color.FromArgb(196, 30, 59);
                case WoWClass.Druid:
                    return Color.FromArgb(255, 124, 10);
                case WoWClass.Hunter:
                    return Color.FromArgb(170, 211, 114);
                case WoWClass.Mage:
                    return Color.FromArgb(104, 204, 239);
                case WoWClass.Monk:
                    return Color.FromArgb(0, 132, 93);
                case WoWClass.Paladin:
                    return Color.FromArgb(244, 140, 186);
                case WoWClass.Priest:
                    return Color.FromArgb(255, 255, 255);
                case WoWClass.Rogue:
                    return Color.FromArgb(255, 244, 104);
                case WoWClass.Shaman:
                    return Color.FromArgb(35, 89, 221);
                case WoWClass.Warlock:
                    return Color.FromArgb(147, 130, 170);
                case WoWClass.Warrior:
                    return Color.FromArgb(199, 156, 87);
                default:
                    return Color.FromArgb(47, 47, 47);
            }
        }

        private Color GetClassForeColor(WoWClass Class)
        {
            switch (StyxWoW.Me.Class)
            {
                case WoWClass.Rogue:
                case WoWClass.Priest:
                case WoWClass.Paladin:
                case WoWClass.Mage:
                case WoWClass.Hunter:
                case WoWClass.Druid:
                case WoWClass.DeathKnight:
                    return Color.FromArgb(0, 0, 0);
                default:
                    return Color.FromArgb(255, 255, 255);
            }
        }

        private void InitializePlainStyle()
        {
            pgMain.LineColor = Color.FromArgb(255, 255, 255); // Category
            pgMain.CategoryForeColor = Color.FromArgb(0, 0, 0);
            pgMain.HelpBackColor = Color.FromArgb(255, 255, 255);

            pgClass.LineColor = GetClassColor(); // Category
            pgClass.CategoryForeColor = GetClassForeColor();
            pgClass.HelpBackColor = Color.FromArgb(255, 255, 255);

            // hide the toolbar
            pgMain.ToolbarVisible = false;
            pgClass.ToolbarVisible = false;

            pgClass.PropertySort = PropertySort.Categorized;
            pgMain.PropertySort = PropertySort.Categorized;

            lblVersion.Text = string.Format("Version: {0}", OracleRoutine.GetOracleVersion());
        }

        #endregion Main Form Settings

        #region Heal Calculations

        private void txtHealCalculation_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtHealCalculation.Clear();
            if (ChronicleHealing.AvailableChronicleSpells.Count == 0) return;
            txtHealCalculation.Text += Environment.NewLine +
                                            cmboHealCalc.SelectedItem.ToString()
                                                .Replace("\r\n", "\n")
                                                .Replace("\n", Environment.NewLine);
        }

        private static Dictionary<string, ChronicleSpell> acs {
            get
            {
                var result = new Dictionary<string, ChronicleSpell>();
                if (ChronicleHealing.AvailableChronicleSpells.Count == 0) return result;

                return ChronicleHealing.AvailableChronicleSpells;
            }
        }

        private void PopulateHealCalculationList()
        {
            if (ChronicleHealing.AvailableChronicleSpells.Count == 0) return;
            cmboHealCalc.DataSource = new BindingSource(acs, null);
            cmboHealCalc.DisplayMember = "Key";
            cmboHealCalc.ValueMember = "Value";
            txtHealCalculation.Text = Environment.NewLine +
                                      cmboHealCalc.SelectedItem.ToString()
                                          .Replace("\r\n", "\n")
                                          .Replace("\n", Environment.NewLine);
        }

        private void btnHealCalc_Click(object sender, EventArgs e)
        {
            ChronicleHealing.CreateSpellList();
            PopulateHealCalculationList();
        }

        #endregion Heal Calculations

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {
            Form dlgSettingsChoice = new SettingsDialog();

            var loadFileDialog = new OpenFileDialog
                {
                    Filter = @"Setting File|*.xml",
                    Title = @"Load Settings from a File",
                    InitialDirectory =
                        string.Format("{0}\\Routines\\Oracle\\UI\\Settings\\CustomSettings\\",
                                      Utilities.AssemblyDirectory)
                };

            DialogResult settingsChoice = dlgSettingsChoice.ShowDialog();

            if (settingsChoice == DialogResult.Cancel) return;

            DialogResult fileChoice = loadFileDialog.ShowDialog();

            if (loadFileDialog.FileName.Contains(".xml") && fileChoice == DialogResult.OK)
            {
                try
                {
                    if (settingsChoice == DialogResult.Yes)
                    {
                        OracleSettings.Instance.LoadFromXML(XElement.Load(loadFileDialog.FileName));
                    }
                    else
                    {
                        switch (StyxWoW.Me.Class)
                        {
                            case WoWClass.Paladin:
                                OracleSettings.Instance.Paladin.LoadFromXML(XElement.Load(loadFileDialog.FileName));
                                break;

                            case WoWClass.Priest:
                                if (StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline)
                                {
                                    OracleSettings.Instance.DiscPriest.LoadFromXML(XElement.Load(loadFileDialog.FileName));
                                }
                                else
                                    OracleSettings.Instance.HolyPriest.LoadFromXML(XElement.Load(loadFileDialog.FileName));
                                break;

                            case WoWClass.Shaman:
                                OracleSettings.Instance.Shaman.LoadFromXML(XElement.Load(loadFileDialog.FileName));
                                break;

                            case WoWClass.Druid:
                                OracleSettings.Instance.Druid.LoadFromXML(XElement.Load(loadFileDialog.FileName));
                                break;

                            case WoWClass.Monk:
                                OracleSettings.Instance.Monk.LoadFromXML(XElement.Load(loadFileDialog.FileName));
                                break;
                        }
                    }

                    ConfigurationFormLoad(null, null);

                    Logger.Output(" Loaded file:  {0}", loadFileDialog.FileName);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show(
                        string.Format(
                            "You are {0} !! You tried to load: \n\n {1} \n\nWhich is not your class. Please select the right class file.",
                            StyxWoW.Me.Class, loadFileDialog.FileName),
                        @"An Error Has Occured",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            Form dlgSettingsChoice = new SettingsDialog();

            var saveFileDialog = new SaveFileDialog
                {
                    Filter = @"Setting File|*.xml",
                    Title = @"Save Settings from a File",
                    InitialDirectory =
                        string.Format("{0}\\Routines\\Oracle\\UI\\Settings\\CustomSettings\\",
                                      Utilities.AssemblyDirectory),
                    DefaultExt = "xml",
                    FileName =
                        string.Format("OracleSettings_{0}-Rev{1}_{2}.xml", StyxWoW.Me.Name, OracleRoutine.GetOracleVersion(),
                                      StyxWoW.Me.Class)
                };

            DialogResult settingsChoice = dlgSettingsChoice.ShowDialog();

            if (settingsChoice == DialogResult.Cancel) return;

            DialogResult fileChoice = saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Contains(".xml") && fileChoice == DialogResult.OK)
            {
                if (settingsChoice == DialogResult.Yes)
                {
                    OracleSettings.Instance.SaveToFile(saveFileDialog.FileName);
                }
                else
                {
                    switch (StyxWoW.Me.Class)
                    {
                        case WoWClass.Paladin:
                            OracleSettings.Instance.Paladin.SaveToFile(saveFileDialog.FileName);
                            break;

                        case WoWClass.Priest:
                            if (StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline)
                            {
                                OracleSettings.Instance.DiscPriest.SaveToFile(saveFileDialog.FileName);
                            }
                            else
                                OracleSettings.Instance.HolyPriest.SaveToFile(saveFileDialog.FileName);
                            break;

                        case WoWClass.Shaman:
                            OracleSettings.Instance.Shaman.SaveToFile(saveFileDialog.FileName);
                            break;

                        case WoWClass.Druid:
                            OracleSettings.Instance.Druid.SaveToFile(saveFileDialog.FileName);
                            break;

                        case WoWClass.Monk:
                            OracleSettings.Instance.Monk.SaveToFile(saveFileDialog.FileName);
                            break;

                        default:
                            OracleSettings.Instance.SaveToFile(saveFileDialog.FileName);
                            break;
                    }
                }

                ConfigurationFormLoad(null, null);

                OracleSettings.Instance.LogSettings();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ((Styx.Helpers.Settings)pgMain.SelectedObject).Save();

                if (pgClass.SelectedObject != null)
                {
                    ((Styx.Helpers.Settings)pgClass.SelectedObject).Save();
                }

                OracleSettings.Instance.LogSettings();

                if (DispelSettingsChanged) btnSaveXml_Click(null, null);

                OracleRoutine.Instance.LoadSpells();

                Close();
            }
            catch (Exception ex)
            {
                Logger.Output(@"ERROR saving settings: {0}", ex.ToString());
            }
        }

        private void lblPlusRep_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.thebuddyforum.com/reputation.php?do=addreputation&p=1253045");
        }

        private Form _newtempui;

        private void btnCombatLog_Click(object sender, EventArgs e)
        {
            if (_newtempui == null || _newtempui.IsDisposed || _newtempui.Disposing)
                _newtempui = new ChronicleViewPort();
            if (_newtempui != null || _newtempui.IsDisposed) _newtempui.ShowDialog();
        }

        private void btnLoadXml_Click(object sender, EventArgs e)
        {
            var loadFileDialog = new OpenFileDialog
            {
                Filter = @"DispelableSpells File|*.xml",
                Title = @"Load DispelableSpells from a File",
                InitialDirectory = string.Format("{0}\\Routines\\Oracle\\UI\\Settings\\CustomSettings\\", Utilities.AssemblyDirectory)
            };

            DialogResult fileChoice = loadFileDialog.ShowDialog();

            if (fileChoice == DialogResult.Cancel) return;

            if (loadFileDialog.FileName.Contains(".xml") && fileChoice == DialogResult.OK)
            {
                DispelableSpell.Instance.SpellList.Load(loadFileDialog.FileName);
                dataGridDispel.DataSource = null;
                dataGridDispel.DataSource = DispelableSpell.Instance.SpellList.Spells;

                DispelSettingsChanged = true;
            }
        }

        private void btnSaveXml_Click(object sender, EventArgs e)
        {
            DialogResult settingsChoice = MessageBox.Show(@"Are you sure you want to save these settings to file and to memory ?",
                                                       @"Question ?",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Question);

            if (settingsChoice == DialogResult.No) return;

            DispelableSpell.Instance.SpellList.Save();

            DispelSettingsChanged = false;
        }

        private void btnExportXML_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = @"DispelableSpells File|*.xml",
                Title = @"Save DispelableSpells from a File",
                InitialDirectory = string.Format("{0}\\Routines\\Oracle\\UI\\Settings\\CustomSettings\\", Utilities.AssemblyDirectory),
                DefaultExt = "xml",
                FileName = string.Format("DispelableSpells.xml")
            };

            DialogResult fileChoice = saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Contains(".xml") && fileChoice == DialogResult.OK)
            {
                DispelableSpell.Instance.SpellList.Save(saveFileDialog.FileName);
            }
        }

        private void dataGridDispel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridDispel.RowCount > 0)
            {
                if (dataGridDispel.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name != "Id") return;

                var dlgDispel = new DispelDialog();

                dlgDispel.Init(Convert.ToInt32(dataGridDispel.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));

                if (dlgDispel.ValidRecordFound)
                {
                    DialogResult settingsChoice = dlgDispel.ShowDialog();
                    if (settingsChoice == DialogResult.OK)
                    {
                        dataGridDispel.DataSource = null;
                        dataGridDispel.DataSource = DispelableSpell.Instance.SpellList.Spells;
                    }
                }
            }
        }

        private Form _newHealingSelectorui;

        private void btnSelectPlayers_Click(object sender, EventArgs e)
        {
            if (_newHealingSelectorui == null || _newHealingSelectorui.IsDisposed || _newHealingSelectorui.Disposing)
                _newHealingSelectorui = new HealingSelector();
            if (_newHealingSelectorui != null || _newHealingSelectorui.IsDisposed) _newHealingSelectorui.ShowDialog();
        }

        private void btnDumpAuras_Click(object sender, EventArgs e)
        {
            WoWAuraCollection TargetAuras = null;

            if (StyxWoW.Me.GotTarget)
            {
                TargetAuras = StyxWoW.Me.CurrentTarget.GetAllAuras();
            }

            if (TargetAuras == null)
            {
                return;
            }

            Logger.Dispel("[=============== Auras]");
            foreach (var aura in TargetAuras)
            {
                Logger.Dispel("{0} : ID [{1}] Harmful: {2} Duration: {3}", aura.Name, aura.SpellId,aura.IsHarmful,aura.Duration);
            }
            Logger.Dispel("[=============== End]");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrioWeights.Display();
        }

        private Form _newLoadBossEncounterui;
        private void btnLoadBossEncounter_Click(object sender, EventArgs e)
        {
            if (_newLoadBossEncounterui == null || _newLoadBossEncounterui.IsDisposed || _newLoadBossEncounterui.Disposing)
                _newLoadBossEncounterui = new SelectBossEncounter();
            if (_newLoadBossEncounterui != null || _newLoadBossEncounterui.IsDisposed) _newLoadBossEncounterui.ShowDialog();
        }
    }
}