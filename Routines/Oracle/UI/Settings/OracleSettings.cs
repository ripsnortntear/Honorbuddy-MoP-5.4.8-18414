#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-17 13:42:23 +1000 (Tue, 17 Sep 2013) $
 * $ID$
 * $Revision: 228 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/Settings/OracleSettings.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.Managers;
using Oracle.Core.WoWObjects;
using Oracle.Shared.Utilities;
using Styx;
using Styx.Common;
using Styx.Helpers;
using System.ComponentModel;

namespace Oracle.UI.Settings
{
    internal class OracleSettings : Styx.Helpers.Settings
    {
        private static OracleSettings _instance;

        public static OracleSettings Instance
        { get { return _instance ?? (_instance = new OracleSettings()); } }

        public OracleSettings()
            : base(SettingsPath + ".config")
        {
        }

        public static string GlobalSettingsPath
        { get { return string.Format("{0}\\Settings\\Oracle\\", Utilities.AssemblyDirectory); } }

        public static string SettingsPath
        { get { return string.Format("{0}OracleSettings_{1}-Rev{2}", GlobalSettingsPath, StyxWoW.Me.Name, OracleRoutine.GetOracleVersion()); } }

        

        [Setting]
        [Styx.Helpers.DefaultValue(TankMode.Automatic)]
        [Category("Tank Selection")]
        [DisplayName("Tank Mode")]
        [Description("Select the mode to use for tanks")]
        public TankMode TankMode { get; set; }


        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Proving Grounds")]
        [DisplayName("Enable Proving Grounds")]
        [Description("When this is enabled Proving Grounds logic will be executed.")]
        public bool EnableProvingGrounds { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Encounters")]
        [DisplayName("Malkorok")]
        [Description("When this is enabled Malkorok logic will be executed.")]
        public bool MalkorokEncounter { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Selective Healing")]
        [DisplayName("Enable Selective")]
        [Description("When this is enabled Selective healing will be available. You will be able to choose which targets to heal.")]
        public bool EnableSelectiveHealing { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("PvP")]
        [DisplayName("PvP Support")]
        [Description("When this is enabled some PvP Logic will be enabled for some classes.")]
        public bool PvPSupport { get; set; }

        #region Category: General - Checked (08/10/2013)

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Logging")]
        [DisplayName("Trace")]
        [Description("Trace shit")]
        public bool Trace { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("")]
        [Browsable(false)]
        [DisplayName("StatCounter")]
        [Description("Last time we updated our statCounter")]
        public string LastStatCounted { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Item")]
        [DisplayName("HealthStone")]
        [Description("Health Percent to use a HealthStone")]
        public int HealthStonePct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(30)]
        [Category("Item")]
        [DisplayName("Spirit Potion")]
        [Description("Mana Percent to use a Spirit Potion")]
        public int ManaPotionPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(PotionForRole.None)]
        [Category("Item")]
        [DisplayName("Potion Selection")]
        [Description("Select your potion")]
        public PotionForRole PotionSelection { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(RacialUsage.Never)]
        [Category("Racial")]
        [DisplayName("Racial Selection")]
        [Description("Select your Racial usage")]
        public RacialUsage UseRacials { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(TrinketUsage.Never)]
        [Category("Trinkets")]
        [DisplayName("First Trinket")]
        [Description("Select how you would like to trigger the First Trinket")]
        public TrinketUsage FirstTrinketUsage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(TrinketUsage.Never)]
        [Category("Trinkets")]
        [DisplayName("Second Trinket")]
        [Description("Select how you would like to trigger the Second Trinket")]
        public TrinketUsage SecondTrinketUsage { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("Trinkets")]
        [DisplayName("Trinket Health Percent")]
        [Description("Health Percent to use a Trinket")]
        public int TrinketHealthPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(50)]
        [Category("Trinkets")]
        [DisplayName("Trinket Mana Percent")]
        [Description("Mana Percent to use a Trinket")]
        public int TrinketManaPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(LogCategory.None)]
        [Category("Logging")]
        [DisplayName("Performance Logging")]
        [Description("Performance = on, None = off.")]
        public LogCategory PerformanceLogging { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Logging")]
        [DisplayName("Dispel Logging")]
        [Description("Log all Debuff/Dispel information, useful when your trying to identify a debuff to dispel.")]
        public bool EnableDispelLogging { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Logging")]
        [DisplayName("Dynamic Priority Spell Logging")]
        [Description("Log all Single target Priority spell healing information, useful when your trying to identify why a spell is given higher priority than another.")]
        public bool EnablePriorityLogging { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Logging")]
        [DisplayName("EF Blanket Logging")]
        [Description("Log how many EF's have been cast due to Eternal Flame Blanketing.")]
        public bool EnableEFLogging { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ChronicleHealing")]
        [DisplayName("Stop Casting")]
        [Description("Enable Disable the use of Stop casting")]
        public bool EnableStopCasting { get; set; }

        //[Setting]
        //[Styx.Helpers.DefaultValue(91)]
        //[Category("ChronicleHealing")]
        //[DisplayName("Dynamic Single Target Spells")]
        //[Description("Targets Health percent needs to be *below* this setting for Oracle to generate its Dynamic single target Spell priority")]
        //public int DynamicSingleTargetSpellsPct { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(91)]
        [Category("ChronicleHealing")]
        [DisplayName("Stop Casting Percent")]
        [Description(" This setting will stop casting HEALING SPELLS at the value set.")]
        public int StopCastingPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ChronicleHealing")]
        [DisplayName("Enable Heals Per Mana")]
        [Description("true = on, will order spells by heals per mana for single target healing (recommended.) otherwise it will be ordered by the spell closest to the health deficit.")]
        public bool ENABLE_HPM { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(91)]
        [Category("ChronicleHealing")]
        [DisplayName("Exclude Players above HP")]
        [Description("Health Percent of the player must be less than this setting in order to receive Single target heals.")]
        public int ExcludePlayersAboveHP { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("SHAMAN DEBUG")]
        [DisplayName("UseExperimentalChainHeal")]
        [Description("Will attempt to select the target in the cluster with Riptide first.")]
        public bool UseExperimentalChainHeal { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("ChronicleHealing")]
        [DisplayName("Predicted Health")]
        [Description("When this setting is enabled Oracle will use the Predicted health percent of the unit rather than its current health percent.")]
        public bool UsePredictedHealth { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue((uint)45)]
        [Category("ChronicleHealing")]
        [DisplayName("Urgent Health percent")]
        [Description("Will start ordering single target spells by Heals per second when healtargets Health Percent is less than the specified value. " +
                     "This should bring the instant heals to the top of the priority.")]
        public uint URGENT_HEALTH_PERCENT { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(91)]
        [Category("Clusters")]
        [DisplayName("Maximum AoE Health Percent")]
        [Description("This setting is the Maximum Health Percent of the players considered for AoE healing. Players need to be less than this value to be included in clustering.")]
        public int MAX_AOE_HP { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("General")]
        [DisplayName("Allow Usage Tracking")]
        [Description("This enabled allows private tracking on the amount of users of this routine - ALL INFO IS PRIVATE!!!")]
        public bool CheckAllowUsageTracking { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Dispel Manager")]
        [DisplayName("Dispel debuffs")]
        [Description("Dispel harmful debuffs")]
        public bool DispelDebuffs { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5)]
        [Category("Dispel Manager")]
        [DisplayName("Dispel Mana Percent")]
        [Description("Mana Percent must be greater than or equal too this value to Dispel players")]
        public int DispelManaPct { get; set; }

        
        #endregion Category: General - Checked (08/10/2013)

        #region Class Late-Loading Wrappers

        // Do not change anything within this region.
        // It's written so we ONLY load the settings we're going to use.
        // There's no reason to load the settings for every class, if we're only executing code for a Druid.

        private DruidSettings _druidSettings;

        private MonkSettings _monkSettings;

        private PaladinSettings _pallySettings;

        private DiscPriestSettings _discpriestSettings;

        private HolyPriestSettings _holypriestSettings;

        private ShamanSettings _shamanSettings;

        [Browsable(false)]
        public DruidSettings Druid
        {
            get { return _druidSettings ?? (_druidSettings = new DruidSettings()); }
        }

        [Browsable(false)]
        public MonkSettings Monk
        {
            get { return _monkSettings ?? (_monkSettings = new MonkSettings()); }
        }

        [Browsable(false)]
        public PaladinSettings Paladin
        {
            get { return _pallySettings ?? (_pallySettings = new PaladinSettings()); }
        }

        [Browsable(false)]
        public DiscPriestSettings DiscPriest
        {
            get { return _discpriestSettings ?? (_discpriestSettings = new DiscPriestSettings()); }
        }

        [Browsable(false)]
        public HolyPriestSettings HolyPriest
        {
            get { return _holypriestSettings ?? (_holypriestSettings = new HolyPriestSettings()); }
        }

        [Browsable(false)]
        public ShamanSettings Shaman
        {
            get { return _shamanSettings ?? (_shamanSettings = new ShamanSettings()); }
        }

        #endregion Class Late-Loading Wrappers

        #region Log Settings

        /// Write all Oracle Settings in effect to the Log file, taken from Singular (thanks again)
        public void LogSettings()
        {
            Logging.Write("");

            // reference the internal references so we can display only for our class
            LogSettings("Oracle General", Instance);
            if (StyxWoW.Me.Class == WoWClass.Druid) LogSettings("Oracle Druid", Druid);
            if (StyxWoW.Me.Class == WoWClass.Monk) LogSettings("Oracle Monk", Monk);
            if (StyxWoW.Me.Class == WoWClass.Paladin) LogSettings("Oracle Paladin", Paladin);

            if (StyxWoW.Me.Class == WoWClass.Priest)
            {
                if (StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline)
                {
                    LogSettings("Oracle  Discipline Priest", DiscPriest);
                }
                else
                    LogSettings("Oracle  Holy Priest", HolyPriest);
            }

            if (StyxWoW.Me.Class == WoWClass.Shaman) LogSettings("Oracle Shaman", Shaman);
            Logging.Write("");
        }

        public void LogSettings(string desc, Styx.Helpers.Settings set)
        {
            if (set == null)
                return;

            Logging.Write("====== {0} Settings ======", desc);
            foreach (var kvp in set.GetSettings())
            {
                Logging.Write("  {0}: {1}", kvp.Key, kvp.Value);
            }

            Logging.Write("");
        }

        #endregion Log Settings
    }
}