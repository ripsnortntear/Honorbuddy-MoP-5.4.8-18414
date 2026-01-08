using System;
using System.ComponentModel;
using System.Text;
using Styx;
using Styx.Helpers;

namespace BattlePetSwapper
{
    public enum eMode
    {
        Relative,
        Ringer,
        Ringerx2,
        Capture
    }

    [DefaultPropertyAttribute("Mode")]
    public class PluginSettings : Settings, IPluginProperties, IPluginSettings
    {
        IPluginLogger _logger;

        const string NAMESPACE = "BattlePetSwapper";

        public PluginSettings(IPluginLogger logger)
            : base(Styx.Helpers.Settings.SettingsDirectory + "\\" + NAMESPACE + "_" + (StyxWoW.Me != null ? StyxWoW.Me.Name : "") + ".xml")
        {
            _logger = logger;
            _logger.Write("Loading Settings file:" + Styx.Helpers.Settings.SettingsDirectory + "\\"+NAMESPACE+"_*****.xml");
            
            Load();

            ConvertSettingsToProperties();
        }

        #region Convert settings to properties and back again

        public void ConvertSettingsToProperties()
        {
            foreach (eMode mode in System.Enum.GetValues(typeof(eMode)))
            {
                if (mode.ToString() == ModeSetting) { Mode = mode; }
            }

            MinLevel = ToIntValue(MinLevelSetting, 1);
            MinPetHealth = ToIntValue(MinPetHealthSetting, 90);
            MaxLevel = ToIntValue(MaxLevelSetting, 25);
            UseWildPets = ToBoolValue(UseWildPetsSetting, false);
            OnlyBluePets = ToBoolValue(OnlyBluePetsSetting, true);
            UseFavouritePetsOnly = ToBoolValue(UseFavouritePetsOnlySetting, false);
            UseFavouriteRingers = ToBoolValue(UseFavouriteRingersSetting, false);
            if (UseFavouritePetsOnly) { UseFavouriteRingers = true; }
            MinRingerPetHealth = ToIntValue(MinRingerPetHealthSetting, 90);
        }

        public void ConvertsPropertiesToSettings()
        {
            ModeSetting = Mode.ToString();
            MinLevelSetting = MinLevel.ToString();
            MinPetHealthSetting = MinPetHealth.ToString();
            MaxLevelSetting = MaxLevel.ToString();
            UseWildPetsSetting = UseWildPets.ToString();
            OnlyBluePetsSetting = OnlyBluePets.ToString();
            UseFavouritePetsOnlySetting = UseFavouritePetsOnly.ToString();
            UseFavouriteRingersSetting = UseFavouriteRingers.ToString();
            if (UseFavouritePetsOnly) { UseFavouriteRingersSetting = UseFavouritePetsOnly.ToString(); }
            MinRingerPetHealthSetting = MinRingerPetHealth.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Settings:");
            sb.Append(" Mode=" + Mode.ToString());
            sb.Append(string.Format(", Pet Levels={0}-{1} ", MinLevel, MaxLevel));
            if (!UseWildPets) { sb.Append(", UseWildPets=" + UseWildPets.ToString()); }
            if (OnlyBluePets) { sb.Append(", OnlyBluePets=" + OnlyBluePets.ToString()); }
            sb.Append(", Min Pet Health=" + MinPetHealth.ToString());
            if (UseFavouritePetsOnly) { sb.Append(", Use Favourite Pets Only=" + UseFavouritePetsOnly.ToString()); }
            else if (UseFavouriteRingers) { sb.Append(", Use Favourite Ringer Pets Only=" + UseFavouriteRingers.ToString()); }
            return sb.ToString();
        }

        private int ToIntValue(string s, int defaultValue)
        {
            if (string.IsNullOrEmpty(s)) { return defaultValue; }
            int value = 0;
            if (!int.TryParse(s, out value)) { return defaultValue; }
            return value;
        }

        private bool ToBoolValue(string s, bool defaultValue)
        {
            if (string.IsNullOrEmpty(s)) { return defaultValue; }
            bool value = false;
            if (!bool.TryParse(s, out value)) { return defaultValue; }
            return value;
        }

        #endregion

        #region Validation

        private static void ValidateIntRange(int value, int min, int max)
        {
            if (value < min || value > max) throw new ArgumentException(string.Format("Value must be between {0} and {1}", min, max));
        }

        #endregion

        #region Property - Mode

        [Setting, Styx.Helpers.DefaultValue("Relative")]
        [Browsable(false)]
        public string ModeSetting { get; set; }

        [CategoryAttribute("Mode"),
        System.ComponentModel.DefaultValueAttribute(eMode.Relative)]
        [DescriptionAttribute("Relative - will choose 3 pets 2 with slighty lower than zone level + 1 slightly higher.\r\nRinger - will choose 2 lowest level pets with 1 of the highest level. e.g. 1,1,25.\r\nRingerX2 - will choose 1 lowest level pet with 2 of the highest level. e.g. 1,25,25.\r\nCapture - will choose 3 pets from 1 level above the zone pet level.")]
        public eMode Mode { get;set; }

        #endregion

        #region Property -MinLevel

        [Setting, Styx.Helpers.DefaultValue("1")]
        [Browsable(false)]
        public string MinLevelSetting { get; set; }

        int _minLevelOfPetToChoose=1;
        [CategoryAttribute("Pets to Choose")]
        [DescriptionAttribute("The minimum pet level which will be chosen")]
        [DisplayName("Minimum Level")]
        public int MinLevel
        {
            get { return _minLevelOfPetToChoose; }
            set { ValidateIntRange(value, 1, 25); _minLevelOfPetToChoose = value; }
        }

        #endregion

        #region Property -MinPetHealth

        [Setting, Styx.Helpers.DefaultValue("90")]
        [Browsable(false)]
        public string MinPetHealthSetting { get; set; }

        int _minPetHealth = 90;
        [CategoryAttribute("Pets to Choose")]
        [DescriptionAttribute("The minimum pet health a pet can have to be chosen")]
        [DisplayName("Minimum Health %")]
        public int MinPetHealth
        {
            get { return _minPetHealth; }
            set { ValidateIntRange(value,1,100); _minPetHealth = value; }
        }

        #endregion

        #region Property -MaxLevel

        [Setting, Styx.Helpers.DefaultValue("25")]
        [Browsable(false)]
        public string MaxLevelSetting { get; set; }

        int _maxLevelOfPetToChoose=25;
        [CategoryAttribute("Pets to Choose")]
        [DescriptionAttribute("The maximum pet level which will be chosen")]
        [DisplayName("Maximum Level")]
        public int MaxLevel
        {
            get { return _maxLevelOfPetToChoose; }
            set 
            {
                ValidateIntRange(value,1,25);
                _maxLevelOfPetToChoose = value; 
            }
        }

        #endregion

        #region Property -UseWildPets

        [Setting, Styx.Helpers.DefaultValue("-1")]
        [Browsable(false)]
        public string UseWildPetsSetting { get; set; }

        [CategoryAttribute("Pets to Choose")]
        [DescriptionAttribute("Choose wild caught pets?")]
        [DisplayName("Use Wild Pets")]
        public bool UseWildPets { get;set; }

        #endregion

        #region Property -OnlyBluePets

        [Setting, Styx.Helpers.DefaultValue("1")]
        [Browsable(false)]
        public string OnlyBluePetsSetting { get; set; }

        [CategoryAttribute("Pets to Choose")]
        [DescriptionAttribute("Choose only blue pets?")]
        [DisplayName("Only Blue Pets")]
        public bool OnlyBluePets { get; set; }

        #endregion

        #region Property -UseFavourites

        [Setting, Styx.Helpers.DefaultValue("-1")]
        [Browsable(false)]
        public string UseFavouritePetsOnlySetting { get; set; }

        [CategoryAttribute("Pets to Choose")]
        [DescriptionAttribute("Choose only favourite pets, if set to true this will set 'Only favourite ringer pets' to true.")]
        [DisplayName("Only favourite pets")]
        public bool UseFavouritePetsOnly { get; set; }

        #endregion#

        #region Property -UseFavouriteRingers

        [Setting, Styx.Helpers.DefaultValue("-1")]
        [Browsable(false)]
        public string UseFavouriteRingersSetting { get; set; }

        [CategoryAttribute("Ringer")]
        [DescriptionAttribute("Choose only favourite pets as ringers.")]
        [DisplayName("Only favourite ringer pets")]
        public bool UseFavouriteRingers { get; set; }

        #endregion

        #region Property -MinRingerPetHealth

        [Setting, Styx.Helpers.DefaultValue("90")]
        [Browsable(false)]
        public string MinRingerPetHealthSetting { get; set; }

        int _minRingerPetHealth = 90;
        [CategoryAttribute("Ringer")]
        [DescriptionAttribute("The minimum pet health a ringer pet can have to be chosen")]
        [DisplayName("Ringer Minimum Health %")]
        public int MinRingerPetHealth
        {
            get { return _minRingerPetHealth; }
            set { ValidateIntRange(value, 1, 100); _minRingerPetHealth = value; }
        }

        #endregion
    }
}
