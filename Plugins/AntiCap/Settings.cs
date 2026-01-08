using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx.WoWInternals;

namespace AntiCap
{
    public class Settings
    {
        public BindingList<Item> BuyItems;
        public static Settings Instance { get; set; }
        private Settings()
        {
            BuyItems = new BindingList<Item>();
        }
        private static string ExeFolder { get { return Path.GetDirectoryName(Application.ExecutablePath); } }
        private static string CharName
        {
            get
            {
                string name;
                try
                {
                    name = Lua.GetReturnVal<string>("return GetUnitName(\"player\")", 0);
                }
                catch (Exception)
                {
                    name = "Johndoe";
                }
                return name;
            }
        }
        private static string Realm
        {
            get
            {
                string realm;
                try
                {
                    realm = Lua.GetReturnVal<string>("return GetRealmName()", 0);
                }
                catch (Exception)
                {
                    realm = "Whimsyshire";
                }
                return realm;
            }
        }
        private static string CharSettingsFileName
        {
            get
            {
                return String.Format("AntiCap[{0}-{1}].xml", CharName, Realm);
            }
        }
        private static string SettingsPath
        {
            get { return Path.Combine(ExeFolder, "Settings", CharSettingsFileName); }
        }

        public static void LoadSettings()
        {
            try
            {
                Instance = ObjectXMLSerializer<Settings>.Load(SettingsPath);
            }
            catch (Exception)
            {
                Instance = new Settings();
            }
        }

        public static void SaveSettings()
        {
            ObjectXMLSerializer<Settings>.Save(Instance, SettingsPath);
        }
    }
}
