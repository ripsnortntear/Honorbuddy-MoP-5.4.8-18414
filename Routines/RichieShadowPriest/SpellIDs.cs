using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieShadowPriestPvP
{
    public enum SpellIDs : int {
        #region Priest

        Psyfiend = 108921,
        DominateMind = 605,
        AngelicFeather = 121536,
        PowerWordSolace = 129250,
        DespareatePrayer = 19236,
        SpectralGuise = 112833,
        PowerInfusion = 10060,
        Cascade_Holy = 121135,
        Cascade_Shadow = 127632,
        DivineStar_Holy = 110744,
        DivineStar_Shadow = 122121,
        Halo_Holy = 120517,
        Halo_Shadow = 120644,
        Archangel = 81700,
        BindingHeal = 32546,
        DispelMagic = 528,
        Fade = 586,
        FearWard = 6346,
        FlashHeal = 2061,
        GreaterHeal = 2060,
        Heal = 2050,
        HolyFire = 14914,
        HymnOfHope = 64901,
        InnerFire = 588,
        InnerFocus = 89485,
        InnerWill = 73413,
        LeapOfFaith = 73325,
        Levitate = 1706,
        MassDispel = 32375,
        MindSear = 48045,
        MindVision = 2096,
        Shadowfiend = 34433,
        PainSuppression = 33206,
        Penance = 47540,
        PowerWordBarrier = 62618,
        PowerWordFortitude = 21562,
        PowerWordShield = 17,
        PrayerOfHealing = 596,
        PrayerOfMending = 33076,
        PsychicScream = 8122,
        Purify = 527,
        Renew = 139,
        Resurrection = 2006,
        ShackleUndead = 9484,
        ShadowWordDeath = 32379,
        ShadowWordDeath_ = 129176,
        ShadowWordDeathCDResetProc = 125927,
        ShadowWordPain = 589,
        Smite = 585,
        SpiritShell = 109964,
        VoidTendrils = 108920,
        CircleOfHealing = 34861,
        DivineHymn = 64843,
        GuardianSpirit = 47788,
        HolyWordChastise = 88625,
        HolyWordSanctuary = 88685,
        HolyWordSerenity = 88684,
        Lightwell = 724,
        VoidShift = 108968,
        VoidShift_Shadow = 142723,
        DevouringPlague = 2944,
        Dispersion = 47585,
        MindBlast = 8092,
        MindFlay = 15407,
        MindFlay_Insanity = 129197,
        MindSpike = 73510,
        PsychicHorror = 64044,
        Shadowform = 15473,
        Silence = 15487,
        VampiricEmbrace = 15286,
        VampiricTouch = 34914,
        Mindbender = 123040,

        // Additional Auras and effects:

        AngelicFeatherAura = 121557,
        LevitateAura = 111759,
        Serendipity = 63735, // Reduced cast time of Greater Heal or Prayer of Healing
        WeakenedSoul = 6788, // Power Word: Shield proc
        JadeSpirit = 104993, // + intellect
        Evangelism = 81661, // smite or Holy Fire spells: +dmg, reduced mana cost
        SurgeOfLight = 114255, // From Darkness, Comes Light proc: instant Flash Heal with no mana
        SurgeOfDarkness_1stack = 87160, // Surge of Darkness one stack
        SurgeOfDarkness_2stacks = 126083, // Surge of Darkness two stacks
        DivineInsight_Holy = 123267, // Greater Heal, Prayer of Healing proc: Instant Prayer of Mending
        DivineInsight_Shadow = 124430, //Instant Mind Blast
        EchoOfLight = 77489, // Heal over time
        SpiritOfRedemption = 20711, // 
        SpiritOfRedemption1 = 27827,
        SpiritOfRedemption2 = 27795,
        SpiritOfRedemption3 = 62371,

        ChakraChastise = 81209,
        ChakraSanctuary = 81206,
        ChakraSerenity = 81208,

        CallOfDominance = 126683,

        #endregion

        #region Shaman

        WindShear = 57994,

        #endregion

        #region Shaman Totems

        StoneBulwarkTotem = 108270,
        EarthgrabTotem = 51485,
        WindwalkTotem = 108273,
        TotemicProjection = 108284,
        HealingTideTotem = 108280,
        TotemicRecall = 36936,
        CapacitorTotem = 108269,
        EarthElementalTotem = 2062,
        EarthbindTotem = 2484,
        FireElementalTotem = 2894,
        GroundingTotem = 8177,
        HealingStreamTotem = 5394,
        MagmaTotem = 8190,
        ManaTideTotem = 16190,
        RockbiterWeapon = 8017,
        SearingTotem = 3599,
        SpiritLinkTotem = 98008,
        StormlashTotem = 120668,
        ShamanSymbiosis = 110504,
        TremorTotem = 8143,

        #endregion

        #region Death Knight

        MindFreeze = 47528,

        #endregion

        #region Mage

        Polymorph = 118,
        IceBlock = 45438,
        Counterspell = 2139,

        #endregion

        #region Hunter

        ScatterShot = 19503,
        CounterShot = 147362,

        #endregion

        #region Druid

        Cyclone = 33786,
        SkullBash = 106839,
        SkullBash2 = 80964,
        IceBlockSymbiosis = 110696,

        #endregion

        #region Monk

        SpearHandStrike = 116705,

        #endregion

        #region Paladin

        Rebuke = 96231,
        Repentance = 20066,
        BlindingLight = 115750,
        DivineShield = 642,
        HandOfProtection = 1022,

        #endregion

        #region Warlock

        DarkIntent = 109773,
        UnstableAffliction = 30108,

        #endregion

        #region Warrior

        Pummel = 6552,
        CommandingShout = 469,
        DisruptingShout = 102060,

        #endregion

        #region Rouge

        Kick = 1766,

        #endregion

        #region Racials

        ArcaneTorrent = 28730,
        Stoneform = 20594,
        EscapeArtist = 20589,
        EveryManForHimself = 59752,
        Shadowmeld = 58984,
        GiftofTheNaaru = 28880,
        Darkflight = 68992,
        BloodFury = 20572,
        WarStomp = 20549,
        Berserking = 26297,
        WillOfTheForsaken = 7744,
        RocketJump = 69070,
        RocketBarrage = 69041,

        #endregion

        #region Professions

        LifeBlood = 55503,

        #endregion

        #region Others

        SpellLock = 19647,
        OpticalBlast = 115781,
        Healthstone = 6262,
        PvPTrinket = 42292,
        GroundingTotemEffect = 8178
        #endregion
    }
}