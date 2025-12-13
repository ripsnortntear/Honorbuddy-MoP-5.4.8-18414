using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Styx;
using Styx.Common;
using Styx.Helpers;

namespace RichieShadowPriestPvP
{
    public class SPSettings : Settings
    {
        public static readonly SPSettings Instance = new SPSettings();

        public SPSettings()
            : base(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                             string.Format(
                                 @"Routines/RichieShadowPriest/RichieShadowPriestSettings-{0}.xml",
                                 StyxWoW.Me.Name)))
        {
        }

        public static bool IsDebugMode { get; set; }

        [Setting, DefaultValue(true)]
        public bool Cascade { get; set; }

        [Setting, DefaultValue(60)]
        public int DesperatePrayer { get; set; }

        [Setting, DefaultValue(45)]
        public int UseFadeAsDefensiveCDBelow { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoBurst { get; set; }

        [Setting, DefaultValue(15)]
        public int DispersionMana { get; set; }

        [Setting, DefaultValue(30)]
        public int DispersionHP { get; set; }

        [Setting, DefaultValue(true)]
        public bool AutoFace { get; set; }

        [Setting, DefaultValue(true)]
        public bool FadeAuto { get; set; }

        [Setting, DefaultValue(true)]
        public bool Lvl90SafeCheck { get; set; }

        [Setting, DefaultValue(2)]
        public int Lvl90MinUnits { get; set; }

        [Setting, DefaultValue(35)]
        public int HealthstonePercent { get; set; }

        [Setting, DefaultValue(true)]
        public bool PowerWordFortitude { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool UseShadowFiend { get; set; }

        [Setting, DefaultValue(20)]
        public int AoEMana { get; set; }

        [Setting, DefaultValue(true)]
        public bool CastLevitate { get; set; }

        [Setting, DefaultValue(3000)]
        public int SWPRefresh { get; set; }

        [Setting, DefaultValue(70)]
        public int VampiricEmbracePercent { get; set; }

        [Setting, DefaultValue(3000)]
        public int VampiricTouchRefresh { get; set; }

        [Setting, DefaultValue(400)]
        public int SearchInterval { get; set; }

        [Setting, DefaultValue(80)]
        public int InstantHealBelowHp { get; set; }

        [Setting, DefaultValue(35)]
        public int CastedHealsBelowHp { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool GriefPpl { get; set; }

        [Setting, DefaultValue(true)]
        public bool Multidot { get; set; }

        [Setting, DefaultValue(true)]
        public bool HealOthers { get; set; }

        [Setting, DefaultValue(true)]
        public bool FearWardSelf { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool AutoMassDispel { get; set; }

        [Setting, DefaultValue(true)]
        public bool UsePowerInfusion { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool LeapOfFaithOnScatteredTeammate { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseRacial { get; set; }

        [Setting, DefaultValue(true)]
        public bool RightClickMovementOff { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseTrinketWithDP { get; set; }

        [Setting, DefaultValue(13)]
        public int TrinketSlotNumber { get; set; }

        [Setting, DefaultValue(false)]
        public bool OnlyHealVIP { get; set; }

        [Setting, DefaultValue(50)]
        public int VTHighPrioBelowManaPercent { get; set; }

        [Setting, DefaultValue(10)]
        public int AngelicFeatherDelay { get; set; }

        [Setting, DefaultValue(4)]
        public int DispelDelay { get; set; }

        [Setting, DefaultValue(5)]
        public int DefensiveCDDelay { get; set; }

        [Setting, DefaultValue(40)]
        public int DispelAboveHp { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool HealPets { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool StopCastOnInterrupt { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool CounteractPoly { get; set; }

        [Setting, DefaultValue(true)]
        public bool CastFailedUserInitiatedSpell { get; set; }    

        [Setting, DefaultValue(true)]
        public bool CollectStatistics { get; set; }
        /*
        Never = 0
        Only hotkeys = 1
        Automatic(Every 6 sec) + Hotkeys = 2
        Automatic(Every 10 sec) + Hotkeys = 3*/
        [Setting, DefaultValue(1)]
        public int AngelicFeatherUsage { get; set; }
        /*
        High = 0
        Low = 1*/
        [Setting, DefaultValue(0)]
        public int AngelicFeatherPriority { get; set; }


        [Setting, DefaultValue(50)]
        public int PeelBelow { get; set; }

        [Setting, DefaultValue(false)]
        public bool DisablePeeling { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseVoidTendrils { get; set; }

        [Setting, DefaultValue(true)]
        public bool UsePsychicScream { get; set; }

        [Setting, DefaultValue(true)]
        public bool UsePsyfiend { get; set; }

        [Setting, DefaultValue(true)]
        public bool UseSpectralGuise { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool UsePsychicHorrorPeel { get; set; }

        [Setting, DefaultValue(100)]
        public int DefCDsBelow { get; set; }

        

        [Setting, DefaultValue(false)]
        public bool DisableCC { get; set; }

        [Setting, DefaultValue(35)]
        public int CCFocusOrHealerBelow { get; set; }

        [Setting, DefaultValue(4)]
        public int CCDelay { get; set; }

        [Setting, DefaultValue(false)]
        public bool CCWhenBursting { get; set; }

        [Setting, DefaultValue(true)]
        public bool UsePsychicScreamCC { get; set; }
        
        [Setting, DefaultValue(true)]
        public bool UseSilenceCC { get; set; }

        [Setting, DefaultValue(true)]
        public bool UsePsyfiendCC { get; set; }
       
        [Setting, DefaultValue(true)]
        public bool UsePsychicHorrorCC { get; set; }


/*Hotkeys*/
        [Setting, DefaultValue(ModifierKeys.NoRepeat)]
        public ModifierKeys ModBurst { get; set; }

        [Setting, DefaultValue(Keys.None)]
        public Keys KeyBurst { get; set; }

        [Setting, DefaultValue(ModifierKeys.NoRepeat)]
        public ModifierKeys ModPsyfiendTarget { get; set; }
        
        [Setting, DefaultValue(Keys.None)]
        public Keys KeyPsyfiendTarget { get; set; }

        [Setting, DefaultValue(ModifierKeys.NoRepeat)]
        public ModifierKeys ModPsyfiendFocus { get; set; }

        [Setting, DefaultValue(Keys.None)]
        public Keys KeyPsyfiendFocus { get; set; }

        [Setting, DefaultValue(ModifierKeys.NoRepeat)]
        public ModifierKeys ModAngelicFeather { get; set; }

        [Setting, DefaultValue(Keys.None)]
        public Keys KeyAngelicFeather { get; set; }

        [Setting, DefaultValue(ModifierKeys.NoRepeat)]
        public ModifierKeys ModAngelicFeatherVIP { get; set; }

        [Setting, DefaultValue(Keys.None)]
        public Keys KeyAngelicFeatherVIP { get; set; }




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

        public static void Print() {
            Logging.WriteDiagnostic("Printing settings...");
            foreach (KeyValuePair<string, Object> kvp in Instance.GetSettings())
                Logging.WriteDiagnostic("{0} = {1}", kvp.Key, kvp.Value);
            Logging.WriteDiagnostic("Done");
        }

    }
}