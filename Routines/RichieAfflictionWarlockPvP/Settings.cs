using System;
using System.IO;
using Styx;
using Styx.Helpers;

namespace RichieAfflictionWarlock
{
    public class AfflictionSettings : Settings
    {
        public static readonly AfflictionSettings Instance = new AfflictionSettings();

        public AfflictionSettings()
            : base(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                             string.Format(
                                 @"Routines/RichieAfflictionWarlockPvP/RichieAfflictionWarlockSettings-{0}.xml",
                                 StyxWoW.Me.Name)))
        {
        }

        [Setting, Styx.Helpers.DefaultValue(40)]
        public int SacrificialPactHP { get; set; }

        [Setting, Styx.Helpers.DefaultValue(1)]
        public int PreferredPet { get; set; }


        [Setting, Styx.Helpers.DefaultValue(50)]
        public int LifeTapOnMana { get; set; }

        [Setting, Styx.Helpers.DefaultValue(40)]
        public int DarkBargainHP { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoFace { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool UseBanish { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool BurstOnCD { get; set; }

        [Setting, Styx.Helpers.DefaultValue(10000)]
        public int AgonyRefresh { get; set; }

        [Setting, Styx.Helpers.DefaultValue(35)]
        public int HealthstonePercent { get; set; }

        [Setting, Styx.Helpers.DefaultValue(35)]
        public int CCFocusOrHealerBelow { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool DrainSoulForShards { get; set; }

        [Setting, Styx.Helpers.DefaultValue(40)]
        public int UnendingResolveHp { get; set; }

        [Setting, Styx.Helpers.DefaultValue(7000)]
        public int CorruptionRefresh { get; set; }

        [Setting, Styx.Helpers.DefaultValue(40)]
        public int DontCastSpellsForHpBelowHp { get; set; }

        [Setting, Styx.Helpers.DefaultValue(5000)]
        public int UnstableAfflictionRefresh { get; set; }

        [Setting, Styx.Helpers.DefaultValue(400)]
        public int SearchInterval { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool Multidot { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool UsePetSpells { get; set; }
        
        [Setting, Styx.Helpers.DefaultValue(80)]
        public int PeelSelf { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool WotF { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool rightClickMovementOff { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool useTrinketWithDP { get; set; }

        [Setting, Styx.Helpers.DefaultValue(13)]
        public int trinketSlotNumber { get; set; }

        [Setting, Styx.Helpers.DefaultValue(40)]
        public int DrainLifeBelowHp { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool CCWhenBursting { get; set; }       
         
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool Soulstone { get; set; }       

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool BGOwnageMode { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool FakeCast { get; set; }


    }
}