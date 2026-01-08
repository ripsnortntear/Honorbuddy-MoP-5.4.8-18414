using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using ObjectManager = Styx.WoWInternals.ObjectManager;


namespace Predator
{
	class Settings
	{
		// Variable for Kick to Lock the Settings he made
		// Set to true and nobody could change them accidently
		public bool LockUI = false;
		
		public bool SkinMobs = true;
		public bool Pickpocket = true;
		public bool PickpocketOnly = true;
		public bool JustFarmCloth = true;
		public bool JustFarmLeather = true;
		
		public string TimeToBlacklist = "15";

		string File = "Plugins\\Predator\\";
		public Settings()
		{
			if (StyxWoW.Me != null)
				try
				{
					Load();
				}
				catch (Exception e)
				{
					Logging.Write(e.Message);
				}
		}


				public void Load()
		{
			//    XmlTextReader reader;
			XmlDocument xml = new XmlDocument();
			XmlNode xvar;

			string sPath = Process.GetCurrentProcess().MainModule.FileName;
			sPath = Path.GetDirectoryName(sPath);
			sPath = Path.Combine(sPath, File);

			if (!Directory.Exists(sPath))
			{
				Logging.WriteDiagnostic("Predator: Creating config directory");
				Directory.CreateDirectory(sPath);
			}

			sPath = Path.Combine(sPath, "Predator.config");

			Logging.WriteDiagnostic("Predator: Loading config file: {0}", sPath);
			System.IO.FileStream fs = new System.IO.FileStream(@sPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
			try
			{
				xml.Load(fs);
				fs.Close();
			}
			catch (Exception e)
			{
				Logging.Write(e.Message);
				Logging.Write("Predator: Continuing with Default Config Values");
				fs.Close();
				return;
			}

			try
			{
				if (xml == null)
					return;
					
				xvar = xml.SelectSingleNode("//Predator/SkinMobs");
				if (xvar != null)
				{
					SkinMobs = Convert.ToBoolean(xvar.InnerText);
				}
				xvar = xml.SelectSingleNode("//Predator/Pickpocket");
				if (xvar != null)
				{
					Pickpocket = Convert.ToBoolean(xvar.InnerText);
				}
				xvar = xml.SelectSingleNode("//Predator/PickpocketOnly");
				if (xvar != null)
				{
					PickpocketOnly = Convert.ToBoolean(xvar.InnerText);
				}
				xvar = xml.SelectSingleNode("//Predator/JustFarmCloth");
				if (xvar != null)
				{
					JustFarmCloth = Convert.ToBoolean(xvar.InnerText);
				}
				xvar = xml.SelectSingleNode("//Predator/JustFarmLeather");
				if (xvar != null)
				{
					JustFarmLeather = Convert.ToBoolean(xvar.InnerText);
				}
				xvar = xml.SelectSingleNode("//Predator/TimeToBlacklist");
				if (xvar != null)
				{
					TimeToBlacklist = Convert.ToString(xvar.InnerText);
				}

			}
			catch (Exception e)
			{
				Logging.WriteDiagnostic("Predator: PROJECTE EXCEPTION, STACK=" + e.StackTrace);
				Logging.WriteDiagnostic("Predator: PROJECTE EXCEPTION, SRC=" + e.Source);
				Logging.WriteDiagnostic("Predator: PROJECTE : " + e.Message);
			}
		}
	}
}
