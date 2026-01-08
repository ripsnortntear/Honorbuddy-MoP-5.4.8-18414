using System;
using System.Windows.Forms;

namespace TurnOffCTM.Gui
{
    public partial class Settings : Form
    {
        private readonly PluginSettings _settings = PluginSettings.Instance;
        
        public Settings()
        {
            InitializeComponent();
        }

        private void SettingsLoad(object sender, EventArgs e)
        {
            _settings.Load();
            switch (_settings.TurnOffType)
            {
                case TurnOffTypes.OnBotStopped:
                    ComboType.SelectedIndex = 0;
                    break;
                case TurnOffTypes.OnBotExited:
                    ComboType.SelectedIndex = 1;
                    break;
            }
            CheckBotException.Checked = _settings.BotException;
            TextBotName.Text = _settings.BotName;
        }

        private void ComboTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.Load();
            switch (ComboType.SelectedIndex)
            {
                case 0:
                    _settings.TurnOffType = TurnOffTypes.OnBotStopped;
                    break;
                case 1:
                    _settings.TurnOffType = TurnOffTypes.OnBotExited;
                    break;
            }
            _settings.Save();
        }

        private void CheckBotExceptionCheckedChanged(object sender, EventArgs e)
        {
            _settings.Load();
            TextBotName.Enabled = CheckBotException.Checked;
            _settings.BotException = CheckBotException.Checked;
            _settings.Save();
        }

        private void TextBotNameTextChanged(object sender, EventArgs e)
        {
            _settings.Load();
            _settings.BotName = TextBotName.Text;
            _settings.Save();
        }
    }
}
