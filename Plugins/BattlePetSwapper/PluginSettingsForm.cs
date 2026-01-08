using System;
using System.Windows.Forms;

namespace BattlePetSwapper
{
    public partial class PluginSettingsForm : Form
    {
        IPluginSettings _settings;
        IPluginLogger _logger;

        public PluginSettingsForm(IPluginSettings settings, IPluginLogger logger)
        {
            _logger = logger;
            _settings = settings;

            InitializeComponent();
            propertyGrid.SelectedObject = settings;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            _settings.ConvertsPropertiesToSettings();
            _logger.Write("Saving settings.");
            _settings.Save();
            this.Close();
        }

        private void CancelButton_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void PluginSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _settings.ConvertSettingsToProperties();
            _logger.Write(_settings.ToString());
        }
    }
}
