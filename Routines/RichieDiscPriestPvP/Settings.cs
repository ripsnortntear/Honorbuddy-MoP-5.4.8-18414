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

        [Setting, DefaultValue(true)]
        public bool Cascade { get; set; }

        [Setting, DefaultValue(60)]
        public int DesperatePrayer { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoFace { get; set; }

        [Setting, DefaultValue(true)]
        public bool FadeAuto { get; set; }

        [Setting, DefaultValue(35)]
        public int HealthstonePercent { get; set; }

        [Setting, DefaultValue(true)]
        public bool PowerWordFortitude { get; set; }

        [Setting, DefaultValue(true)]
        public bool ShadowfiendAuto { get; set; }

        [Setting, DefaultValue(400)]
        public int SearchInterval { get; set; }

        [Setting, DefaultValue(true)]
        public bool GriefPpl { get; set; }

        [Setting, DefaultValue(true)]
        public bool FearWardSelf { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool AutoMassDispel { get; set; }

        [Setting, DefaultValue(80)]
        public int PeelSelf { get; set; }

        [Setting, DefaultValue(true)]
        public bool PowerInfusionOnCD { get; set; }

        [Setting, DefaultValue(true)]
        public bool LoF { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseRacial { get; set; }

        [Setting, DefaultValue(true)]
        public bool Shackle { get; set; }

        [Setting, DefaultValue(true)]
        public bool rightClickMovementOff { get; set; }

        [Setting, DefaultValue(true)]
        public bool useTrinket { get; set; }

        [Setting, DefaultValue(13)]
        public int trinketSlotNumber { get; set; }

        [Setting, DefaultValue(90)]
        public int PoMHp { get; set; }
        
        [Setting, DefaultValue(35)]
        public int painSuppressionHp { get; set; }
        
        [Setting, DefaultValue(85)]
        public int penanceHp { get; set; }
        
        [Setting, DefaultValue(80)]
        public int InstantFlashHealBelowHP { get; set; }
        
        [Setting, DefaultValue(40)]
        public int innerFocusHp { get; set; }
        
        [Setting, DefaultValue(50)]
        public int FlashHealHp { get; set; }
        
        [Setting, DefaultValue(40)]
        public int barrierHp { get; set; }
        
        [Setting, DefaultValue(30)]
        public int VoidShiftBelowHp { get; set; }
        
        [Setting, DefaultValue(90)]
        public int renewHp { get; set; }

        [Setting, DefaultValue(true)]
        public bool Lvl90SafeCheck { get; set; }

        [Setting, DefaultValue(90)]
        public int PWSHp { get; set; }

        [Setting, DefaultValue(80)]
        public int AtonementHealingAbove { get; set; }

        [Setting, DefaultValue(false)]
        public bool OnlyUseSmite { get; set; }

        [Setting, DefaultValue(80)]
        public int PWSBlanketAboveHp { get; set; }

        [Setting, DefaultValue(10)]
        public int AngelicFeatherDelay { get; set; }

        [Setting, DefaultValue(false)]
        public bool DontCancelDominateMind { get; set; }        

        [Setting, DefaultValue(40)]
        public int TotemAndPlayerKillingAboveHp { get; set; }

    }
}