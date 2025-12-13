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

        [Setting, DefaultValue(40)]
        public int SacrificialPactHP { get; set; }

        [Setting, DefaultValue(1)]
        public int PreferredPet { get; set; }


        [Setting, DefaultValue(50)]
        public int LifeTapOnMana { get; set; }

        [Setting, DefaultValue(40)]
        public int DarkBargainHP { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoFace { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseBanish { get; set; }

        [Setting, DefaultValue(true)]
        public bool BurstOnCD { get; set; }

        [Setting, DefaultValue(10000)]
        public int AgonyRefresh { get; set; }

        [Setting, DefaultValue(35)]
        public int HealthstonePercent { get; set; }

        [Setting, DefaultValue(35)]
        public int CCFocusOrHealerBelow { get; set; }

        [Setting, DefaultValue(true)]
        public bool DrainSoulForShards { get; set; }

        [Setting, DefaultValue(40)]
        public int UnendingResolveHp { get; set; }

        [Setting, DefaultValue(7000)]
        public int CorruptionRefresh { get; set; }

        [Setting, DefaultValue(40)]
        public int DontCastSpellsForHpBelowHp { get; set; }

        [Setting, DefaultValue(5000)]
        public int UnstableAfflictionRefresh { get; set; }

        [Setting, DefaultValue(400)]
        public int SearchInterval { get; set; }

        [Setting, DefaultValue(true)]
        public bool Multidot { get; set; }

        [Setting, DefaultValue(true)]
        public bool UsePetSpells { get; set; }
        
        [Setting, DefaultValue(80)]
        public int PeelSelf { get; set; }

        [Setting, DefaultValue(true)]
        public bool WotF { get; set; }

        [Setting, DefaultValue(true)]
        public bool rightClickMovementOff { get; set; }

        [Setting, DefaultValue(true)]
        public bool useTrinketWithDP { get; set; }

        [Setting, DefaultValue(13)]
        public int trinketSlotNumber { get; set; }

        [Setting, DefaultValue(40)]
        public int DrainLifeBelowHp { get; set; }

        [Setting, DefaultValue(false)]
        public bool CCWhenBursting { get; set; }       
         
        [Setting, DefaultValue(false)]
        public bool Soulstone { get; set; }       

        [Setting, DefaultValue(false)]
        public bool BGOwnageMode { get; set; }

        [Setting, DefaultValue(true)]
        public bool FakeCast { get; set; }


    }
}