#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-07-27 19:56:11 +0200 (Sa, 27 Jul 2013) $
 * $ID$
 * $Revision: 1615 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Settings/Settings/PRSettings.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

// This file was part of Singular - A community driven Honorbuddy CC
using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using PureRotation.Core;
using Styx;
using Styx.Common;
using Styx.Helpers;

namespace PureRotation.Settings.Settings
{
    internal class PRSettings : Styx.Helpers.Settings
    {
        private static PRSettings _instance;

        public PRSettings()
            : base(SettingsPath + ".config")
        {
            CleanseBlacklist = new Dictionary<int, string>()
                {
                    {96328, "Toxic Torment (Green Cauldron)"},
                    {96325, "Frostburn Formula (Blue Cauldron)"},
                    {96449, "Frostburn Formula (?)"},
                    {96326, "Burning Blood (Red Cauldron)"},
                    {131040, "Burning Blood (?)"},
                    {92876, "Blackout (10man)"},
                    {92878, "Blackout (25man)"},
                    {86788, "Blackout (?)"},
                    {30108, "(Warlock) Unstable Affliction"},
                    {35183, "(Warlock) Unstable Affliction"},
                    {131736, "(Warlock) Unstable Affliction"},
                    {8050, "(Shaman) Flame Shock"},
                    {3600, "(Shaman) Earthbind"},
                    {34914, "(Priest) Vampiric Touch"},
                    {104050, "Torrent of Frost"},
                    {103962, "Torrent of Frost"},
                    {103904, "Torrent of Frost"},
                    {117756, "Cowardice"},
                    {117829, "Cowardice"},
                    //Throne of Thunder # Mirabis \\
                    {138732, "Ionization - Jin Rokh"},
                    {136184, "Thick Bones - Primordius"},
                    {136186, "Clear Mind - Primordius"},
                    {136182, "Improved Synapses - Primordius"},
                    {136180, "Keen Eyesight - Primordius"},
                    {136185, "Fragile Bones =- Primordius"},
                    {136187, "CLouded Mind - Primordius"},
                    {136183, "Dulled Synapses - Primordius"},
                    {136181, "Impaired Eyesight - Primordius"},
                    {138609, "Matter Swap - Aura"}, //BUG: Can cause bugs with the delaydispel if it checks for blacklist
                    {139822, "Cinders"},//BUG: Can cause bugs with the delaydispel if it checks for blacklist
                    {139919, "Matter Swap  - Targetted"},//BUG: Can cause bugs with the delaydispel if it checks for blacklist
                    {133597, "Durumu - Dark Parasite"},//BUG: Can cause bugs with the delaydispel if it checks for blacklist
                    
                };

            // SpellID, string description
            DispelPriorityCheck = new Dictionary<int, string>()
                {
                    {136708, "Horridon - Stone Gaze"}, 
                    {136719, "Farraki - Blazing Sunlight"},
                    {136587, "Gurubashi- Venom Bolt Volley"}, 
                    {136710, "Drakaki - Deadly Plague"},
                    {136512, "Amani - Hex of Confusion"},
                    {136857, "Entrapped"},
                    {136185, "Fragile Bones"},
                    {136187, "Clouded Mind"},
                    {136183, "Dulled Synapses"},
                    {136181, "Impaired Eyesight"},
                    {138040, "Horrific Visage"},
                    {117949, "Closed Curcuit"},
                    {140179, "Mageara - Supression (Heroic)"}
                    //{52798, "Borrowed Time"},  // test
                };

            // SpellID, Float range
            DispelRangeCheck = new Dictionary<int, float>()
                {
                    {139822, 12f}  // Mageara - Cinders
                };

            // SpellID, Delay in Milliseconds
            DispelDelayCheck = new Dictionary<int, int>()
                {
                    {138609, 5000}, //Dark Animus - Matter Swap
                    {133597, 7000}, //Durumu - Dark Parasite ( Heroic )
                };

            // SpellID, Stackcount
            DispelStackCheck = new Dictionary<int, int>()
                {
                    {136719, 3}  // Horridon - Blazing Sunlight
                };
        }

        [Browsable(false)]
        internal static Dictionary<int, string> CleanseBlacklist = new Dictionary<int, string>();

        [Browsable(false)]
        internal static Dictionary<int, float> DispelRangeCheck = new Dictionary<int, float>();

        [Browsable(false)]
        internal static Dictionary<int, int> DispelDelayCheck = new Dictionary<int, int>();

        [Browsable(false)]
        internal static Dictionary<int, int> DispelStackCheck = new Dictionary<int, int>();

        [Browsable(false)]
        internal static Dictionary<int, string> DispelPriorityCheck = new Dictionary<int, string>();
        internal static double ThrottleTime = 0.5;
        public static string SettingsPath
        {
            get
            {
                return string.Format("{0}\\Settings\\PureRotation\\PRSettings_{1}-Rev{2}", Utilities.AssemblyDirectory,
                                     StyxWoW.Me.Name,PureRotationRoutine.Version);
            }
        }
        /// <summary>
        /// Write all PureRotation Settings in effect to the Log file, taken from Singular (thanks again)
        /// </summary>
        public void LogSettings()
        {
            Logging.Write("");

            // reference the internal references so we can display only for our class
            LogSettings("PureRotation", PRSettings.Instance);
            if (StyxWoW.Me.Class == WoWClass.DeathKnight) LogSettings("DeathKnightSettings", DeathKnight);
            if (StyxWoW.Me.Class == WoWClass.Druid) LogSettings("DruidSettings", Druid);
            if (StyxWoW.Me.Class == WoWClass.Hunter) LogSettings("HunterSettings", Hunter);
            if (StyxWoW.Me.Class == WoWClass.Mage) LogSettings("MageSettings", Mage);
            if (StyxWoW.Me.Class == WoWClass.Monk) LogSettings("MonkSettings", Monk);
            if (StyxWoW.Me.Class == WoWClass.Paladin) LogSettings("PaladinSettings", Paladin);
            if (StyxWoW.Me.Class == WoWClass.Priest) LogSettings("PriestSettings", Priest);
            if (StyxWoW.Me.Class == WoWClass.Rogue) LogSettings("RogueSettings", Rogue);
            if (StyxWoW.Me.Class == WoWClass.Shaman) LogSettings("ShamanSettings", Shaman);
            if (StyxWoW.Me.Class == WoWClass.Warlock) LogSettings("WarlockSettings", Warlock);
            if (StyxWoW.Me.Class == WoWClass.Warrior) LogSettings("WarriorSettings", Warrior);
            Logging.Write("");
        }

        public void LogSettings(string desc, Styx.Helpers.Settings set)
        {
            if (set == null)
                return;

            Logging.Write("====== {0} Settings ======", desc);
            Logging.Write("Rotation Mode [{0}]", HotkeySettings.Instance.ModeChoice);
            foreach (var kvp in set.GetSettings())
            {
                Logging.Write("  {0}: {1}", kvp.Key, kvp.Value.ToString());
            }

            Logging.Write("");
        }

        public static PRSettings Instance
        {
            get { return _instance ?? (_instance = new PRSettings()); }
        }

        #region Category: General - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("General")]
        [DisplayName("WoW Chat Output")]
        [Description("Enable WoW Chat Output when using Hotkeys")]
        public bool EnableWoWChatOutput { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("General")]
        [DisplayName("Use Cooldowns")]
        [Description("When this is set to true, PureRotation will always use Cooldowns")]
        public bool UseCooldowns { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("General")]
        [DisplayName("Use AoE Abilities")]
        [Description("When this is set to true, PureRotation will always use AoE abilities such as DnD, Multishot, MindSear, HellFire, etc")]
        public bool UseAoEAbilities { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("General")]
        [DisplayName("Enable Raid and Party Buffing")]
        [Description("When this is set to true, PureRotation will buff the Raid or Party with thier specific abilitie (Kings, MoTW, Fortitude, etc)")]
        public bool EnableRaidPartyBuffing { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("General")]
        [DisplayName("Enable Interupts")]
        [Description("When this is set to true, Pure will always interupt the current target with thier specific abilitie such as Kick, Rebuke, Mind Freeze, etc")]
        public bool EnableInterupts { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("General")]
        [DisplayName("Enable Experimental Placement")]
        [Description("When this is set to true, Pure will always place placeable spells x yards away from the given location")]
        public bool EnableExperimentalPlacement { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(8)]
        [Category("General")]
        [DisplayName("Experimental Placement Distance")]
        [Description("When Enable Experimental Placement is set to true, Pure will calculate a new random location within this distance")]
        public int ExperimentalPlacementDistance { get; set; }

        #endregion Category: General - Checked (29/12/2012)

        #region Category: Misc - Checked (29/12/2012)

        #endregion Category: Misc - Checked (29/12/2012)

        #region Category: Healing - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Heal Manager")]
        [DisplayName("Framelock")]
        [Description("Increase Performance by enabling framelock for *selective* areas of the routine.")]
        public bool EnableFlock { get; set; }


        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Heal Manager")]
        [DisplayName("Overide Dispel")]
        [Description("Enable Dispel for non Healing Classes. This will Initialise the Healmanager and DispelMnager,")]
        public bool EnableDispelNonHeals { get; set; }


        [Setting]
        [Styx.Helpers.DefaultValue(95)]
        [Category("Heal Manager")]
        [DisplayName("Overheal")]
        [Description("Adjust this value Lower to overheal")]
        public int OverhealPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Heal Manager")]
        [DisplayName("Include totalAbsorb @")]
        [Description("heal prediction will take into consideration totalAbsorbs on the target when mana is less than the set value.")]
        public int IncludetotalAbsorb { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Heal Manager")]
        [DisplayName("Include My Heals")]
        [Description("Adjust this value TRUE = heal prediction will include your players Heals on the target when returning predicted health percent")]
        public bool includeMyHeals { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(DispelStyle.LowPriority)]
        [Category("Heal Manager")]
        [DisplayName("Dispel debuffs")]
        [Description("Dispel harmful debuffs")]
        public DispelStyle DispelDebuffs { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3)]
        [Category("Heal Manager")]
        [DisplayName("AoE Heal - Player Count")]
        [Description("Will use AoE healing abilities when the amount of players reaches this value")]
        public int AoEHealCount { get; set; }


        [Setting]
        [Styx.Helpers.DefaultValue(87)]
        [Category("Heal Manager")]
        [DisplayName("AoE Heal - Player HP")]
        [Description("Will use AoE healing abilities when the players Healthpercent reaches this value")]
        public int AoEHealHP { get; set; }


        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Heal Manager")]
        [DisplayName("Targeting")]
        [Description("Ok to target tank's target if we have no target")]
        public bool TargetTankTarget
        {
            get;
            set;
        }


        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Healing")]
        [DisplayName("Self Healing")]
        [Description("When this is set to true, PureRotation will automaticly handle Healing such as Vampiric Blood, Lay on Hands, Healthstones, etc")]
        public bool EnableSelfHealing { get; set; }





        #endregion Category: Healing - Checked (29/12/2012)

        #region Category: Items - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Items")]
        [DisplayName("Use Potion")]
        [Description("Uses class appropriate Potion during lust.")]
        public bool UsePotion { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(TrinketUsage.OnBoss)]
        [Category("Items")]
        [Description("Defines the usage of Trinket in slot 12")]
        public TrinketUsage Trinket1Choice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(TrinketUsage.OnBoss)]
        [Category("Items")]
        [Description("Defines the usage of Trinket in slot 13")]
        public TrinketUsage Trinket2Choice { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Items")]
        [Description("Enables / Disables the usage of Gloves")]
        public bool UseGloves { get; set; }
        #endregion Category: Items - Checked (29/12/2012)

        #region GiftOfNaaru - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Racials")]
        [DisplayName("Use Gift Of The Naaru")]
        public bool EnableGiftoftheNaaru { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("Racials")]
        [DisplayName("Gift of the Naaru Percent")]
        [Description("Will use Gift of the Naaru on this Percent")]
        public int GiftNaaruHp
        {
            get;
            set;
        }

        #endregion GiftOfNaaru - Checked (29/12/2012)

        #region Category: Racials - Checked (29/12/2012)

        [Setting]
        [Styx.Helpers.DefaultValue(RacialUsage.OnBoss)]
        [Category("Racials")]
        [DisplayName("Use Racials")]
        public RacialUsage UseRacials { get; set; }

        #endregion Category: Racials - Checked (29/12/2012)

        #region Class Late-Loading Wrappers

        // Do not change anything within this region.
        // It's written so we ONLY load the settings we're going to use.
        // There's no reason to load the settings for every class, if we're only executing code for a Druid.

        private DeathKnightSettings _dkSettings;

        private DruidSettings _druidSettings;

        private HunterSettings _hunterSettings;

        private MageSettings _mageSettings;

        private MonkSettings _monkSettings;

        private PaladinSettings _pallySettings;

        private PriestSettings _priestSettings;

        private RogueSettings _rogueSettings;

        private ShamanSettings _shamanSettings;

        private WarlockSettings _warlockSettings;

        private WarriorSettings _warriorSettings;

        [Browsable(false)]
        public DeathKnightSettings DeathKnight
        {
            get { return _dkSettings ?? (_dkSettings = new DeathKnightSettings()); }
        }

        [Browsable(false)]
        public DruidSettings Druid
        {
            get { return _druidSettings ?? (_druidSettings = new DruidSettings()); }
        }

        [Browsable(false)]
        public HunterSettings Hunter
        {
            get { return _hunterSettings ?? (_hunterSettings = new HunterSettings()); }
        }

        [Browsable(false)]
        public MageSettings Mage
        {
            get { return _mageSettings ?? (_mageSettings = new MageSettings()); }
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
        public PriestSettings Priest
        {
            get { return _priestSettings ?? (_priestSettings = new PriestSettings()); }
        }

        [Browsable(false)]
        public RogueSettings Rogue
        {
            get { return _rogueSettings ?? (_rogueSettings = new RogueSettings()); }
        }

        [Browsable(false)]
        public ShamanSettings Shaman
        {
            get { return _shamanSettings ?? (_shamanSettings = new ShamanSettings()); }
        }

        [Browsable(false)]
        public WarlockSettings Warlock
        {
            get { return _warlockSettings ?? (_warlockSettings = new WarlockSettings()); }
        }

        [Browsable(false)]
        public WarriorSettings Warrior
        {
            get { return _warriorSettings ?? (_warriorSettings = new WarriorSettings()); }
        }

        #endregion Class Late-Loading Wrappers
    }
}