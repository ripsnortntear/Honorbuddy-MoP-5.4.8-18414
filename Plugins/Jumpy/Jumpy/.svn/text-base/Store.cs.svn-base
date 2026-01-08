using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Windows.Forms;
namespace Jumpy
{
	public class Package
	{
		public double RandMountJumpSpan = 100;
		public double BaseMountJumpSpan = 900;
		public double RandWalkJumpSpan = 100;
		public double BaseWalkJumpSpan = 850;
		public bool UsePlayerRange = true;
		public List<string> CombatRoutines = new List<string>();
		public List<string> MountedRoutines = new List<string>();
		public List<string> WalkingRoutines = new List<string>();
		public double PlayerRange = 100;
		public bool StandJumpInCombat = false;
		public bool StandJumpOutCombat = false;
	}

	public static class Storage
	{
        public static bool FirstTimeLoaded { get { return !File.Exists(fileName); } }
		public static string fileName;
		public static Package Package = new Package();
		static Storage()
		{
			Directory.CreateDirectory(Path.Combine(Jumpy.ApplicationStartupPath, "Plugins/Jumpy"));
			fileName = Path.Combine(Jumpy.ApplicationStartupPath, "Plugins/Jumpy/Jumpy.xml");
			Load();

			Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
		}

		static void Application_ApplicationExit(object sender, EventArgs e)
		{
			Save();
		}

		public static void Save()
		{

			if (File.Exists(fileName))
				File.Delete(fileName);
			XDocument doc = new XDocument(new XElement("Jumpy", (typeof(Package).GetFields().Select(f => 
			{
				if (f.FieldType == typeof(List<string>))
					return new XElement(f.Name, ((List<string>)f.GetValue(Package)).Select(s => new XElement("Item", s)));
				else
					return new XElement(f.Name, f.GetValue(Package));
			}))));
			doc.Save(fileName);
		}
		public static void Load()
		{
			try
			{
				if (File.Exists(fileName))
				{
					using (StreamReader stream = new StreamReader(fileName))
					{

						XDocument doc = XDocument.Parse(stream.ReadToEnd());
						foreach (XElement x in doc.Root.Elements())
						{
							System.Reflection.FieldInfo field = typeof(Package).GetField(x.Name.ToString());
							if (field.FieldType.ToString().Contains("Collection"))
							{
								List<string> values = new List<string>();
								foreach (XElement xx in x.Elements("Item"))
								{
									values.Add(xx.Value);
								}
								field.SetValue(Package, values);
							}
							else
								field.SetValue(Package, Convert.ChangeType(x.Value, field.FieldType));
						}
					}
				}
			}
			catch (Exception ee) { MessageBox.Show(ee.Message + "\r\nLoading default settings instead.", "Jumpy Error"); }
		}
	}
}
