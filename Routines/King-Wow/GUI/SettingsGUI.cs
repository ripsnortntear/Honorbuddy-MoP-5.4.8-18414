using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KingWoW
{
    public partial class KingHealSettingsGUIFORM : Form
    {
        private Styx.Helpers.Settings GuiSettingsBase;
        private string setting_path;
        private string setting_extra_path;

        public KingHealSettingsGUIFORM(Styx.Helpers.Settings s, string path, string extra_path)
        {
            setting_path = path;
            setting_extra_path = extra_path;
            GuiSettingsBase = s;
            InitializeComponent();
            ClassSpecificPropertyGrid.SelectedObject = GuiSettingsBase;
            UtilityGrid.SelectedObject = ExtraUtilsSettings.Instance;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (setting_path != null)
            {
                GuiSettingsBase.SaveToFile(setting_path);
            }
            if(setting_extra_path!=null)
            {
                ExtraUtilsSettings.Instance.SaveToFile(setting_extra_path);
            }
            this.Close();               
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Setting File|*.xml",
                Title = "Load Setting from a File",
                InitialDirectory = Styx.Common.Utilities.AssemblyDirectory + @"\Routines\king-wow\king-wow-Settings\"
            };
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Contains(".xml"))
            {
                try
                {
                    GuiSettingsBase.LoadFromXML(XElement.Load(openFileDialog.FileName));
                    ClassSpecificPropertyGrid.Refresh();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Setting File|*.xml",
                Title = "Save Setting to a File",
                InitialDirectory = Styx.Common.Utilities.AssemblyDirectory + @"\Routines\king-wow\king-wow-Settings\",
                DefaultExt = "xml"
            };

            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Contains(".xml"))
            {
                GuiSettingsBase.SaveToFile(saveFileDialog.FileName);
            }
            else
            {
                return;
            }
        }

        private void KingHealSettingsGUIFORM_FormClosed(object sender, FormClosedEventArgs e)
        {
            /*if (setting_path != null)
            {
                GuiSettingsBase.SaveToFile(setting_path);
            }
            if (setting_extra_path != null)
            {
                ExtraUtilsSettings.Instance.SaveToFile(setting_extra_path);
            }*/
        }
    }
}
