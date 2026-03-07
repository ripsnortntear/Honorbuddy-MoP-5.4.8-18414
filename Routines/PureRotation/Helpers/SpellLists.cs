using System.Collections.Generic;

namespace PureRotation.Helpers
{
    internal static class SpellLists
    {
        public static readonly HashSet<int> BurstHaste = new HashSet<int> {
            2825,                //Shaman (2825) Bloodlust
            32182,               //Shaman (32182) Heroism
            80353,               //Mage (80353) Time Warp
            90355,               //Hunter (Core Hound) (90355) Ancient Hysteria
            49016,               //Unholy DeathKnight (49016) Unholy Frenzy
            3045,                //Hunter (3045) Rapid Fire
        };

        // ====================  Buffs ====================
        internal static readonly HashSet<int> Stats = new HashSet<int> {
            117666,     //Legacy of the Emperor
            1126,       //Mark of The Wild
            20217,      //Blessing Of Kings
            90363,      //Embrace of the Shale Spider
        };

        internal static readonly HashSet<int> Stamina = new HashSet<int> {
            469,        //Commanding Shout
            6307,       //Imp. Blood Pact
            21562,      //Power Word: Fortitude
            90364,      //Qiraji Fortitude
        };

        internal static readonly HashSet<int> AttackPower = new HashSet<int> {
            19506,      //Trueshot Aura
            6673,       //Battle Shout
            57330,      //Horn of Winter
        };

        internal static readonly HashSet<int> SpellPower = new HashSet<int> {
            77747,      //Burning Wrath
            109773,     //Dark Intent
            61316,      //Dalaran Brilliance
            1459,       //Arcane Brilliance
            126309,     //Still Water
        };

        internal static readonly HashSet<int> AttackSpeed = new HashSet<int> {
            30809,      //Unleashed Rage
            113742,     //Swiftblade's Cunning
            55610,      //Improved Icy Talons
            128432,     //Cackling Howl
            50498,      //Tear Armor
        };

        internal static readonly HashSet<int> SpellHaste = new HashSet<int> {
            24907,      //Moonkin Aura
            51470,      //Elemental Oath
            49868,      //Mind Quickening
        };

        internal static readonly HashSet<int> CriticalStrike = new HashSet<int> {
            1459,      //Arcane Brilliance
            61316,     //Dalaran Brilliance
            24932,     //Leader of The Pact
            116781,    //Legacy of the White Tiger
            97229,     //Bellowing Roar
            24604,    //Furious Howl
            90309,    //Terrifying Roar
            126373,   //Fearless Roar
            126309,   //Still Water
        };

        internal static readonly HashSet<int> Mastery = new HashSet<int> {
            116956,    //Grace of Air
            19740,     //Blessing of Might
            93435,    //Roar of Courage
            128997,  //Spirit Beast Blessing
        };

        // ==================== Debuffs ====================
        private static readonly HashSet<int> MagicVulnerability = new HashSet<int> {

            //58410,      //Master Poisoner
            1490,       //Curse of the Elements TODO: Probably the better of the lot as it affects all targets within 15yrds..Soulburn: Curse ??

            //34889,      //Fire Breath (pet) the only 8% debuff..TODO: Do we want to overwrite the pets 8% with the 5% ?? --wulf
            //24844,      //Lightning Breath
        };

        public static HashSet<int> BuffRaidValid
        {
            get
            {
                return BRvalid;
            }
        }

        private static readonly HashSet<int> BRvalid = new HashSet<int> {

            // Mogu'shan Vaults
            117708,         // Maddening Shout -- Meng the Demented

            // Heart of Fear
            122784, 62402,  // Reshape Life -- Amber-Shaper Un'sok
            122740,         // Convert -- Imperial Vizier Zor'lok

            // Terrace of Endless Spring
            // -- None
        };

        //Soul Reaper valid non Boss Units -- Raid(de)buffs

        public static HashSet<uint> SoulReaperValid
        {
            get
            {
                return Srvalid;
            }
        }

        private static readonly HashSet<uint> Srvalid = new HashSet<uint> {

            // Mogu'shan Vaults
            60184, // Shadowy Minion -- Gara'Jal
            66992, // Severer of Souls -- Gara'Jal
            60913, // Energy Charge -- Elegon
            62618, // Cosmic Spark -- Elegon
            60793, // Celestial Protector -- Elegon
            60776, // Empyreal Focus -- Elegon
            60398, // Emperor's Courage -- Will of the Emporer
            60397, // Emperor's Strength -- Will of the Emporer
            60396, // Emperor's Rage -- Will of the Emporer

            // Heart of Fear
            62405, 65499, // Sra'thik Amber-Trapper -- Wind Lord Mel'jarak
            65498, 62408, // Zar'thik Battle-Mender -- Wind Lord Mel'jarak
            65500, 62402, // Kor'thik Elite Blademaster -- Wind Lord Mel'jarak
            62711, // Amber Monstrosity -- Amber-Shaper Un'sok
            63591, // Kor'thik Reaver -- Grand Empress Shek'zeer
            64453, // Set'thik Windblade -- Grand Empress Shek'zeer

            // Terrace of Endless Spring
            62919, // Unstable Sha -- Tsulong
            62969, // Embodied Terror -- Tsulong
            62995, // Animated Protector -- Lei Shi
            66100, // Apparition of Terror -- Unknown
            64393, // Night Terror -- Unknown
            61034, // Terror Spawn -- Sha of Fear
            61038, // Yang Guoshi -- Sha of Fear
            61042, // Cheng Kang -- Sha of Fear
            61046, // Jinlun Kun -- Sha of Fear
        };

        private static readonly HashSet<int> WeakenedBlows = new HashSet<int> {
            115798, // Weakened Blows
        };

        private static readonly HashSet<int> WeakenedArmor = new HashSet<int> {
            113746, // Weakened Armor
        };

        private static readonly HashSet<int> PhysicalVulnerability = new HashSet<int> {
            81326, // Physical Vulnerability
        };

        private static readonly HashSet<int> SlowCasting = new HashSet<int> {
            5760,  // Mind-numbing Poison
            73975, //Necrotic Strike TODO: Needs to be checked
            31589, //Slow TODO: Needs to be checked
            109466, //Curse of Enfeeblement TODO: Needs to be checked
        };

        private static readonly HashSet<int> MortalWounds = new HashSet<int> {
            115804,  // Mortal Wounds
        };

        // ===================== Trinket Proc List INT & Spellpower (made by alex) ========================
        private static readonly HashSet<int> AuraInt = new HashSet<int> {
            84488, // Dreadful Gladiator's Badge of Dominance (Dreadful Season 12)
            105574, // Zen Alchemist Stone
            126266, // Empty Fruit Barrel
            126478, // Flashfrozen Resin Globule
            127923, // Mithril Wristwatch
            126577, // Light of the Cosmos
            128985, // Relic of Yu'lon
            126588, // Qin-xi's Polarizing Seal
            126705, // Malevolent Gladiator's Insignia of Dominance (Season 12)
            126683, // Malevolent Gladiator's Badge of Dominance (Season 12)
            136082, // Shock-Charger Medallion
            104993, // Jade-Spirit Weapon Enchant
        };

        // ===================== Trinket Proc List INT & Spellpower (made by alex) ========================
        private static readonly HashSet<int> AuraMastery = new HashSet<int> {
            104510, //Windsong Mastery
            136092, // Durable Badge of the Shieldwall
        };

        // ===================== Interuptable Spells =====================================

        private static readonly HashSet<int> InteruptableSpells = new HashSet<int> {
            30451, // Arcane Blast, //
            32546, // Binding Heal, //
            1064, // Chain Heal, //
            421, // Chain Lightning, //
            50796, // Chaos Bolt, //
            693, // Create Healthstone, //
            6201, // Create Healthstone, //
            33786, // Cyclone, //
            82326, // Divine Light, //
            61882, // Earthquake, //
            339, // Entangling Roots, //
            12051, // Evocation, //
            879, // Exorcism, //
            5782, // Fear, //
            133, // Fireball, //
            2120, // Flamestrike, //
            2061, // Flash Heal, //
            19750, // Flash of Light, //
            116, // Frostbolt, //
            44614, // Frostfire Bolt, //
            2060, // Greater Heal, //
            77472, // Greater Healing Wave, //
            71521, // Hand of Gul'dan, //
            48181, // Haunt, //
            2050, // Heal, //
            73920, // Healing Rain, //
            8004, // Healing Surge, //
            5185, // Healing Touch, //
            331, // Healing Wave, //
            51514, // Hex, //
            2637, // Hibernate, //
            14914, // Holy Fire, //
            635, // Holy Light, //
            82327, // Holy Radiance, //
            5484, // Howl of Terror, //
            49203, // Hungering Cold, //
            348, // Immolate, //
            29722, // Incinerate, //
            51505, // Lava Burst, //
            403, // Lightning Bolt, //
            8129, // Mana Burn, //
            32375, // Mass Dispel, //
            8092, // Mind Blast, //
            605, // Mind Control, //
            73510, // Mind Spike, //
            50464, // Nourish, //
            118, // Polymorph, //
            28272, // Polymorph, //
            61721, // Polymorph, //
            61305, // Polymorph, //
            61780, // Polymorph, //
            28271, // Polymorph, //
            596, // Prayer of Healing, //
            11366, // Pyroblast, //
            20484, // Rebirth, //
            8936, // Regrowth, //
            1513, // Scare Beast, //
            2948, // Scorch, //
            5676, // Searing Pain, //
            27243, // Seed of Corruption, //
            9484, // Shackle Undead, //
            686, // Shadow Bolt, //
            585, // Smite, //
            6353, // Soul Fire, //
            2912, // Starfire, //
            78674, // Starsurge, //
            30146, // Summon Felguard, //
            691, // Summon Felhunter, //
            688, // Summon Imp, //
            1122, // Summon Infernal, //
            712, // Summon Succubus, //
            697, // Summon Voidwalker, //
            30108, // Unstable Affliction, //
            34914, // Vampiric Touch, //
            5176, // Wrath, //

            //MoP Instances
            113134, // Mass Resurrection High Inquisitor Whitemane, //
        };

        private static readonly HashSet<int> ChanneledInteruptableSpells = new HashSet<int> {
           5143, // Arcane Missiles, //
           42650, // Army of the Dead, //
           10, // Blizzard, //
           64843, // Divine Hymn, //
           689, // Drain Life, //
           89420, // Drain Life, //
           1120, // Drain Soul, //
           755, // Health Funnel, //
           1949, // Hellfire, //
           85403, // Hellfire, //
           16914, // Hurricane, //
           64901, // Hymn of Hope, //
           50589, // Immolation Aura, //
           15407, // Mind Flay, //
           47540, // Penance, //
           5740, // Rain of Fire, //
           740, // Tranquility, //
           103103, // Malefic Grasp //
        };

        // ============================ Dispel shit.============================
        private static readonly HashSet<string> ignoreDispel = new HashSet<string> {
            "Blackout",
            "Toxic Torment",
            "Frostburn Formula",
            "Burning Blood",
            "Unstable Affliction",
             "Flame Shock",              // Magic
        };

        private static readonly HashSet<string> urgentDispel = new HashSet<string> {
            "Disrupting Shadows",       // magic
            "Boulder Smash",            // ??
            "Chains of Ice",            // Magic
            "Freezing Trap",            // Magic
            "Tentacle Smash",           // Magic
            "Shackles of Ice",          // Magic
            "Righteous Shear",          // Magic
            "Twilight Shear",           // Magic
            "Molten Blast",             // Magic
            "Temporal Vortex",          // Magic
            "Earth and Moon",           // Magic
            "Arcane Bomb",              // Magic
            "Shriek of the Highborne",  // Magic
            "Frost Corruption",         // Magic
            "Static Cling",             // Magic
            "Static Discharge",         // Magic
            "Consuming Darkness",       // ???
            "Lash of Anguish",          // Magic
            "Static Disruption",        // Magic
            "Accelerated Corruption"    // Magic
        };
    }
}