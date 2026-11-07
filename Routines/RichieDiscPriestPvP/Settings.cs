using System;
using System.IO;
using Styx;
using Styx.Helpers;

namespace RichieDiscPriestPvP
{
    public class DiscSettings : Settings
    {
        public static readonly DiscSettings Instance = new DiscSettings();
        
        public DiscSettings()
            : base(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                             string.Format(
                                 @"Routines/RichieDiscPriestPvP/RichieDiscPriestPvPSettings-{0}.xml",
                                 StyxWoW.Me.Name)))
        {
        }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool Cascade { get; set; }

        [Setting, Styx.Helpers.DefaultValue(60)]
        public int DesperatePrayer { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoFace { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool FadeAuto { get; set; }

        [Setting, Styx.Helpers.DefaultValue(35)]
        public int HealthstonePercent { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool PowerWordFortitude { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool ShadowfiendAuto { get; set; }

        [Setting, Styx.Helpers.DefaultValue(400)]
        public int SearchInterval { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool GriefPpl { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool FearWardSelf { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoMassDispel { get; set; }

        [Setting, Styx.Helpers.DefaultValue(80)]
        public int PeelSelf { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool PowerInfusionOnCD { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool LoF { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool UseRacial { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool Shackle { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool rightClickMovementOff { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useTrinket { get; set; }

        [Setting, Styx.Helpers.DefaultValue(13)]
        public int trinketSlotNumber { get; set; }

        [Setting, Styx.Helpers.DefaultValue(90)]
        public int PoMHp { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(35)]
        public int painSuppressionHp { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(85)]
        public int penanceHp { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(80)]
        public int InstantFlashHealBelowHP { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(40)]
        public int innerFocusHp { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(50)]
        public int FlashHealHp { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(40)]
        public int barrierHp { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(30)]
        public int VoidShiftBelowHp { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(90)]
        public int renewHp { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool Lvl90SafeCheck { get; set; }

        [Setting, Styx.Helpers.DefaultValue(90)]
        public int PWSHp { get; set; }

        [Setting, Styx.Helpers.DefaultValue(80)]
        public int AtonementHealingAbove { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool OnlyUseSmite { get; set; }

        [Setting, Styx.Helpers.DefaultValue(80)]
        public int PWSBlanketAboveHp { get; set; }

        [Setting, Styx.Helpers.DefaultValue(10)]
        public int AngelicFeatherDelay { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool DontCancelDominateMind { get; set; }        

        [Setting, Styx.Helpers.DefaultValue(40)]
        public int TotemAndPlayerKillingAboveHp { get; set; }

    }
}