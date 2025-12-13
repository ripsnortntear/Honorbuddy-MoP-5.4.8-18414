using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Helpers;
using System.IO;
using Styx;
using Styx.Common;
using System.ComponentModel;
using DefaultValue = Styx.Helpers.DefaultValueAttribute;
using Styx.TreeSharp;

namespace Boomkin
{
    public class CRSettings : Settings
    {
        public static readonly CRSettings myPrefs = new CRSettings();

        public CRSettings()
            : base(Path.Combine(Utilities.AssemblyDirectory, string.Format(@"Config/Pasterke/Druid/{0}-BalanceSettings-{1}.xml", StyxWoW.Me.RealmName, StyxWoW.Me.Name)))
        {
        }
        [Setting, DefaultValue(5)]
        public int PauseKey { get; set; }

        [Setting, DefaultValue(4)]
        public int AoePauseKey { get; set; }

        public enum Keypress
        {
            None,
            LSHIFT,
            RSHIFT,
            LCTRL,
            RCTRL,
            LALT,
            RALT
        };

        [Setting, DefaultValue(Keypress.None)]
        public Keypress PauseKeys { get; set; }

        [Setting, DefaultValue(Keypress.None)]
        public Keypress AoePauseKeys { get; set; }

        [Setting, DefaultValue(true)]
        public bool PrintMsg { get; set; }

        [Setting, DefaultValue(true)]
        public bool HaveSavageryGlyph { get; set; }

        [Setting, DefaultValue(true)]
        public bool useLifeblood { get; set; }

        [Setting, DefaultValue(4)]
        public int ElitesHealth { get; set; }

        [Setting, DefaultValue(0)]
        public int jadepotion { get; set; }

        [Setting, DefaultValue(0)]
        public int intflask { get; set; }

        [Setting, DefaultValue(false)]
        public bool alchemyflask { get; set; }

        [Setting, DefaultValue(0)]
        public int healthstonepercent { get; set; }

        [Setting, DefaultValue(1)]
        public int trinket1 { get; set; }

        [Setting, DefaultValue(1)]
        public int trinket2 { get; set; }

        [Setting, DefaultValue(0)]
        public int engigloves { get; set; } 

        [Setting, DefaultValue(3)]
        public int startAoe { get; set; }

        [Setting, DefaultValue(0)]
        public int useBerserk { get; set; }

        [Setting, DefaultValue(0)]
        public int useBerserking { get; set; }

        [Setting, DefaultValue(0)]
        public int useFeralSpirit { get; set; }

        [Setting, DefaultValue(0)]
        public int CenarionWardPercent { get; set; }

        [Setting, DefaultValue(0)]
        public int BarskinPercent { get; set; }

        [Setting, DefaultValue(0)]
        public int SurvivalInstinctsPercent { get; set; }

        [Setting, DefaultValue(0)]
        public int FrenziedRegenerationPercent { get; set; }

        [Setting, DefaultValue(true)]
        public bool Facing { get; set; }

        [Setting, DefaultValue(true)]
        public bool Targeting { get; set; }

        [Setting, DefaultValue(true)]
        public bool Movement { get; set; }

        [Setting, DefaultValue(0)]
        public int HealingTouch { get; set; }

        [Setting, DefaultValue(0)]
        public int Rejuvenation { get; set; }

        [Setting, DefaultValue(0)]
        public int NaturesSwiftness { get; set; }

        [Setting, DefaultValue(0)]
        public int Renewal { get; set; }

        [Setting, DefaultValue(0)]
        public int Eat { get; set; }

        [Setting, DefaultValue(0)]
        public int Drink { get; set; }

        [Setting, DefaultValue(0)]
        public int innervate { get; set; }

        [Setting, DefaultValue(0)]
        public int celestial { get; set; }

        [Setting, DefaultValue(0)]
        public int typhoon { get; set; }

    }
}
