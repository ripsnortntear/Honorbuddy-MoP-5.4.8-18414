using System.ComponentModel;
using Styx;
using Styx.Common;
using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx.WoWInternals;

namespace UltimateFarmer.Settings
{
    internal class UFSettings : Styx.Helpers.Settings
    {
        private static UFSettings _instance;
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private UFSettings()
            : base(SettingsPath + ".config")
        {
        }

        public static UFSettings Instance
        {
            get { return _instance ?? (_instance = new UFSettings()); }
        }

        private static string SettingsPath
        {
            get
            {
                return string.Format("{0}\\Settings\\UltimateFarmer\\{1}\\Settings_{2}_{3}", Utilities.AssemblyDirectory,
                                     Realm, Me.Name, Realm);
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
                    realm = "NOrealm";
                }
                return realm;
            }
        }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("- Controls")]
        [DisplayName("MAX Mobs")]
        [Description("Max Number of Mobs to fight at the same Time")]
        public int MobMax { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("- Controls")]
        [DisplayName("Range")]
        [Description("Range to Search Mobs to Pull")]
        public int PullRange { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("- Controls")]
        [DisplayName("MAX Health")]
        [Description("Stop Pulling if Max Health Below %")]
        public int MaxHealth { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(10)]
        [Category("- Controls")]
        [DisplayName("MAX Corpses")]
        [Description("Force Loot if Lootable Corpses are More than the value")]
        public int MaxCorpse { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("- Factions")]
        [DisplayName("Faction 1")]
        [Description("Faction ID to Pull")]
        public int FactionId1 { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("- Factions")]
        [DisplayName("Faction 2")]
        [Description("Faction ID to Pull")]
        public int FactionId2 { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("- Factions")]
        [DisplayName("Faction 3")]
        [Description("Faction ID to Pull")]
        public int FactionId3 { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("- Factions")]
        [DisplayName("Kill All Factions")]
        [Description("Kill every Faction regardless the Settings")]
        public bool AllFactions { get; set; }

		[Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("- Blacklist")]
        [DisplayName("Blacklist 1")]
        [Description("ENTRY ID Target to Blacklist")]
        public int Bl1 { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("- Blacklist")]
        [DisplayName("Blacklist 2")]
        [Description("ENTRY ID Target to Blacklist")]
        public int Bl2 { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("- Blacklist")]
        [DisplayName("Blacklist 3")]
        [Description("ENTRY ID Target to Blacklist")]
        public int Bl3 { get; set; }
		/*
		[Setting]
        [Styx.Helpers.DefaultValue(false)]
		[Category("- Experimental")]
        [DisplayName("Kill All")]
        [Description("Kill all Mobs in the Area")]
        public bool KillAll { get; set; }
		*/
		[Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("- non-Stop")]
        [DisplayName("Pull in fight")]
        [Description("Pull in fight if [pulled mobs] < [MAX Mobs]")]
        public bool PullInFight { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("- non-Stop")]
        [DisplayName("Loot in fight")]
        [Description("Loot in fight MAX Coprses not used here")]
        public bool LootInCombat { get; set; }
		
		[Setting]
        [Styx.Helpers.DefaultValue(0)]
        [Category("- Timer")]
        [DisplayName("Timer in Min.")]
        [Description("Activate addon for a specific amount of time (in Min.)")]
        public int RunTime { get; set; }
    }
}
