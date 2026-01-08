using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;

using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using Styx.Plugins;


namespace Predator
{
	public partial class Config : Form
	{
		public Config()
		{
			InitializeComponent();
			BSave.Enabled = !(Predator.Settings.LockUI);

			SkinMobs.Checked = Predator.Settings.SkinMobs;
			Pickpocket.Checked = Predator.Settings.Pickpocket;
			PickpocketOnly.Checked = Predator.Settings.PickpocketOnly;
			JustFarmCloth.Checked = Predator.Settings.JustFarmCloth;
			JustFarmLeather.Checked = Predator.Settings.JustFarmLeather;
			TimeToBlacklist.Text = Predator.Settings.TimeToBlacklist;
		}

		private void BSave_Click(object sender, EventArgs e)
		{
			if (TimeToBlacklist.Text == "")
			{
				Logging.Write("Predator: Line 1: Setting time to engage blacklist to 15 seconds by default.");
				TimeToBlacklist.Text = "15";
			}	
			
			if (JustFarmCloth.Checked && JustFarmLeather.Checked)
			{
				Logging.Write("Predator: Line 1: Cannot enable both JustFarmCloth and JustFarmLeather, disabling both for now; please choose one or the other or neither.");
				JustFarmCloth.Checked = false;
				JustFarmLeather.Checked = false;
			}

			Predator.Settings.SkinMobs = SkinMobs.Checked;
			Predator.Settings.Pickpocket = Pickpocket.Checked;
			Predator.Settings.PickpocketOnly = PickpocketOnly.Checked;
			Predator.Settings.JustFarmCloth = JustFarmCloth.Checked;
			Predator.Settings.JustFarmLeather = JustFarmLeather.Checked;
			Predator.Settings.TimeToBlacklist = TimeToBlacklist.Text;

			string File = "Plugins\\Predator\\";
			Logging.Write("Predator: SettingsSaved!");

			XmlDocument xml;
			XmlElement root;
			XmlElement element;
			XmlText text;
			XmlComment xmlComment;

			string sPath = Process.GetCurrentProcess().MainModule.FileName;
			sPath = Path.GetDirectoryName(sPath);
			sPath = Path.Combine(sPath, File);

			if (!Directory.Exists(sPath))
			{
				Logging.WriteDiagnostic("Predator: Creating config directory");
				Directory.CreateDirectory(sPath);
			}

			sPath = Path.Combine(sPath, "Predator.config");

			Logging.WriteDiagnostic("Predator: Saving config file: {0}", sPath);
			xml = new XmlDocument();
			XmlDeclaration dc = xml.CreateXmlDeclaration("1.0", "utf-8", null);
			xml.AppendChild(dc);

			xmlComment = xml.CreateComment(
				"=======================================================================\n" +
				".CONFIG  -  This is the Config File For Predator\n\n" +
				"XML file containing settings to customize in the Predator Plugin\n" +
				"It is STRONGLY recommended you use the Configuration UI to change this\n" +
				"file instead of direct changein it here.\n" +
				"========================================================================");

			//let's add the root element
			root = xml.CreateElement("Predator");
			root.AppendChild(xmlComment);


			//let's add another element (child of the root)
			element = xml.CreateElement("SkinMobs");
			text = xml.CreateTextNode(SkinMobs.Checked.ToString());
			element.AppendChild(text);
			root.AppendChild(element);
			
			//let's add another element (child of the root)
			element = xml.CreateElement("Pickpocket");
			text = xml.CreateTextNode(Pickpocket.Checked.ToString());
			element.AppendChild(text);
			root.AppendChild(element);
			
			//let's add another element (child of the root)
			element = xml.CreateElement("PickpocketOnly");
			text = xml.CreateTextNode(PickpocketOnly.Checked.ToString());
			element.AppendChild(text);
			root.AppendChild(element);
			
			//let's add another element (child of the root)
			element = xml.CreateElement("JustFarmCloth");
			text = xml.CreateTextNode(JustFarmCloth.Checked.ToString());
			element.AppendChild(text);
			root.AppendChild(element);
			
			//let's add another element (child of the root)
			element = xml.CreateElement("JustFarmLeather");
			text = xml.CreateTextNode(JustFarmLeather.Checked.ToString());
			element.AppendChild(text);
			root.AppendChild(element);

			//let's add another element (child of the root)
			element = xml.CreateElement("TimeToBlacklist");
			text = xml.CreateTextNode(TimeToBlacklist.Text.ToString());
			element.AppendChild(text);
			root.AppendChild(element);

			System.IO.FileStream fs = new System.IO.FileStream(@sPath, System.IO.FileMode.Create,
															   System.IO.FileAccess.Write);
			try
			{
				xml.Save(fs);
				fs.Close();
			}
			catch (Exception np)
			{
				Logging.Write(np.Message);
			}

		}
	}
}
