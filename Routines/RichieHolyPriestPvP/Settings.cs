using System;
using System.IO;
using Styx;
using Styx.Helpers;
using System.Collections.Generic;
using Styx.Common;

namespace RichieHolyPriestPvP
{
    public class HolySettings : Settings
    {
        public static readonly HolySettings Instance = new HolySettings();

        public HolySettings()
            : base(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Routines/RichieHolyPriestPvP/RichieHolyPriestPvPSettings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        public static bool IsDebugMode { get; set; }

        public static int InjuredPercent = 85;

        #region Direct Heal

        [Setting, DefaultValue(35)]
        public int UseGuardianSpiritBelowHealth { get; set; }

        [Setting, DefaultValue(90)]
        public int PowerWordShieldHp { get; set; }

        [Setting, DefaultValue(90)]
        public int RenewHp { get; set; }

        [Setting, DefaultValue(85)]
        public int PrayerOfMendingHp { get; set; }

        [Setting, DefaultValue(80)]
        public int InstantFlashHealBelowHP { get; set; }

        [Setting, DefaultValue(60)]
        public int FlashHealHp { get; set; }

        [Setting, DefaultValue(30)]
        public int CircleOfHealingBelowHealth { get; set; }

        [Setting, DefaultValue(70)]
        public int DivineStarHP { get; set; }

        [Setting, DefaultValue(3)]
        public int MinInjuredCountToCastCascadeHalo { get; set; }

        [Setting, DefaultValue(2)]
        public int MinInjuredCountToCastAOEHeal { get; set; }

        [Setting, DefaultValue(60)]
        public int UseBindingHealIfMyHP { get; set; }

        #endregion

        [Setting, DefaultValue(20)]
        public int UseLeapOfFaitHP { get; set; }

        [Setting, DefaultValue(35)]
        public int HealthstonePercent { get; set; }

        [Setting, DefaultValue(30)]
        public int VoidShiftBelowHp { get; set; }

        [Setting, DefaultValue(50)]
        public int DefCDsBelow { get; set; }

        [Setting, DefaultValue(20)]
        public int UseDamageSpellsIfHealTargetHP { get; set; }

        [Setting, DefaultValue(60)]
        public int UseShadowFiendIfMana { get; set; }

        [Setting, DefaultValue(2)]
        public int PurifyWhenLowPrioDebuffCount { get; set; }

        [Setting, DefaultValue(150)]
        public int PolyReactLatencyModifier { get; set; }

        [Setting, DefaultValue(1500)]
        public int CastLevitateAfterMs { get; set; }

        [Setting, DefaultValue(400)]
        public int SearchInterval { get; set; }

        [Setting, DefaultValue(500)]
        public int DispelDelay { get; set; }

        /*Auto = 0 Inner Fire = 1 Inner Will = 2 None = 3*/
        [Setting, DefaultValue(0)]
        public int PreferredBuff { get; set; }

        [Setting, DefaultValue(10)]
        public int AngelicFeatherDelay { get; set; }

        [Setting, DefaultValue(800)]
        public int InterruptSpellCastsAfter { get; set; }

        [Setting, DefaultValue(40)]
        public int UseDispelMagicHP { get; set; }

        [Setting, DefaultValue(80)]
        public int BlanketAboveHP { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseTrinket { get; set; }

        [Setting, DefaultValue(13)]
        public int TrinketSlotNumber { get; set; }

        [Setting, DefaultValue(true)]
        public bool UsePowerInfusion { get; set; }

        #region Defensive CDs

        [Setting, DefaultValue(false)]
        public bool DisableAllDefCDs { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseVoidTendrils { get; set; }

        [Setting, DefaultValue(true)]
        public bool UsePsychicScream { get; set; }

        [Setting, DefaultValue(true)]
        public bool UsePsyfiend { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseDespareatePrayer { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseSpectralGuise { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseHolyWordChackra { get; set; }

        #endregion

        [Setting, DefaultValue(true)]
        public bool UseVoidShiftToHeal { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseVoidShiftToHealMe { get; set; }

        [Setting, DefaultValue(true)]
        public bool LeapOfFaithOnScatteredTeammate { get; set; }

        [Setting, DefaultValue(true)]
        public bool CastLevitate { get; set; }

        [Setting, DefaultValue(true)]
        public bool InterruptFocus { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoFace { get; set; }

        [Setting, DefaultValue(true)]
        public bool FadeAuto { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoMassDispel { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseRacial { get; set; }

        [Setting, DefaultValue(true)]
        public bool RightClickMovementOff { get; set; }

        [Setting, DefaultValue(false)]
        public bool DontCancelDominateMind { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool DispelLowPrio { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool HealPets { get; set; }

        [Setting, DefaultValue(true)]
        public bool PowerWordFortitude { get; set; }

        [Setting, DefaultValue(true)]
        public bool FearWardSelf { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool StopCastOnInterrupt { get; set; }

        [Setting, DefaultValue(true)]
        public bool CounteractPoly { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseShadowFiend { get; set; }

        [Setting, DefaultValue(false)]
        public bool UseBlanketOnlyInArena { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseAngelicFeather { get; set; }

        [Setting, DefaultValue(true)]
        public bool CastFailedUserInitiatedSpell { get; set; }

        [Setting, DefaultValue(true)]
        public bool CollectStatistics { get; set; }

        //(\d+),(\s+)//(\s+)([^\n]+) -> <spell id="\1">\4</spell>
        public HashSet<int> BuffDispelASAPHS = new HashSet<int> {
            1044,   // Hand of Freedom
            6940,   // Hand of Sacrifice
            69369,  // Predatory Swiftness
            12472,  // Icy Veins
            1022,   // Hand of Protection
            11426,  // Ice Barrier
            20925,  // Sacred Shield
            114250, // Selfless Healer
            17,     // Power Word: Shield
            12043,  // Presence of Mind
            132158, // Nature's Swiftness
            16188,  // Ancestral Swiftness
            110909,	// Alter time
            6346,   // Fear Ward 
            974     // Earth Shield
        };

        public static HashSet<int> DebuffCCPurifyASAPHS = new HashSet<int>{
            105421, //Blinding Light
            123393, //Breath of Fire (Glyph of Breath of Fire)
            44572, //Deep Freeze
            605, //Dominate Mind
            31661, //Dragon's Breath
            5782, // target.Class != WoWClass.Warrior || //Fear
            118699, // target.Class != WoWClass.Warrior || //Fear
            130616, // target.Class != WoWClass.Warrior || //Fear (Glyph of Fear)
            3355, //Freezing Trap
            853, //Hammer of Justice
            110698, //Hammer of Justice (Paladin)
            2637, //Hibernate
            88625, //Holy Word: Chastise
            119072, //Holy Wrath
            5484, // target.Class != WoWClass.Warrior || //Howl of Terror
            115268, //Mesmerize (Shivarra)
            6789, //Mortal Coil
            115078, //Paralysis
            113953, //Paralysis (Paralytic Poison)
            126355, //Paralyzing Quill (Porcupine)
            118, //Polymorph
            61305, //Polymorph: Black Cat
            28272, //Polymorph: Pig
            61721, //Polymorph: Rabbit
            61780, //Polymorph: Turkey
            28271, //Polymorph: Turtle
            64044, // target.Class != WoWClass.Warrior || //Psychic Horror
            8122, // target.Class != WoWClass.Warrior || //Psychic Scream
            113792, // target.Class != WoWClass.Warrior || //Psychic Terror (Psyfiend)
            107079, //Quaking Palm
            115001, //Remorseless Winter
            20066, // target.Class != WoWClass.Warrior || //Repentance
            82691, //Ring of Frost
            1513, //Scare Beast
            132412, //Seduction (Grimoire of Sacrifice)
            6358, //Seduction (Succubus)
            9484, //Shackle Undead
            30283, //Shadowfury
            87204, //Sin and Punishment
            104045, //Sleep (Metamorphosis)
            118905, //Static Charge (Capacitor Totem)
            33395, //Freeze
            63685, //Freeze (Frozen Power)
            122, //Frost Nova
            110693, //Frost Nova (Mage)
            2944, //Devouring Plague
            10326 //Turn Evil
        };

        public static void Print()
        {
            Logging.WriteDiagnostic("Printing settings...");
            foreach (KeyValuePair<string, Object> kvp in Instance.GetSettings())
                Logging.WriteDiagnostic("{0} = {1}", kvp.Key, kvp.Value);
            Logging.WriteDiagnostic("Done");
        }
    }
}