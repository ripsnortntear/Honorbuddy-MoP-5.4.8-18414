#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-08-25 12:44:43 +0200 (So, 25 Aug 2013) $
 * $ID$
 * $Revision: 1699 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Core/enums.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

using System;

namespace PureRotation.Core
{
    #region Mode enum

    public enum Mode
    {
        Auto,
        SemiAuto,
        Hotkey
    }

    #endregion Mode enum

    #region TrinketUsage enum

    public enum TrinketUsage
    {
        Never,
        OnBoss,
        OnCooldownInCombat,
        LowHealth
    }

    #endregion

    public enum ClusterType
    {
        // Circular cluster centered around 'target'
        Radius//,

        //Chained,
        //Cone,
        // returns a cluster of units that are between LocalPlayer location and 'target'
        //Path
    }

    enum DispelStyle
    {
        None = 0,
        LowPriority,
        HighPriority
    }

    public enum RacialUsage
    {
        Never,
        OnCooldown,
        OnBoss
    };

    public enum PvPHealMode
    {
        Self,
        Healers,
        Casters,
        Melee,
        Tanks,
        Focus,
        Others,
        Anyone
    };

    public enum PvPCCMode
    {
        Anyone,
        Healers,
        Casters,
        Melee,
        Tanks,
        Focus,
        CurrentTarget,
        NotCurrentTarget
    };

    public enum PvPInteruptMode
    {
        Anyone,
        Healers,
        Casters,
        Melee,
        Tanks,
        Focus,
        CurrentTarget,
        NotCurrentTarget
    };

    public enum EclipseType
    {
        None,
        Solar,
        Lunar
    };

    public enum ItemUsage
    {
        Never,
        OnCooldown,
        OnBoss
    }

    public enum SpellUsage
    {
        Never,
        OnCooldown,
        OnCondition,
        PoolWithCooldowns
    }

    public enum PetSlot
    {
        FirstSlot = 1,
        SecondSlot,
        ThirdSlot,
        FourthSlot,
        FifthSlot
    }

    public enum WindShear
    {
        OnBoss,
        OnCooldown
    }

    public enum Ascendance
    {
        OnBoss,
        OnCooldown
    }

    public enum ElementalMastery
    {
        OnBoss,
        OnCooldown
    }

    public enum FeralSpirit
    {
        OnBoss,
        OnCooldown,
    }

    public enum Burst
    {
        onBoss,
        onMob,
    }

    public enum StormlashTotem
    {
        OnCooldown,
        OnHaste,
        Never
    }

    public enum DeathKnightTierOneTalent
    {
        PlagueLeech,
        UnholyBlight,
        RoilingBlood,
        None
    }

    public enum HealingAquisitionMethod
    {
        Proximity,
        RaidParty
    }

    public enum PaladinBlessing
    {
        Kings,
        Might
    }

    public enum MonkLegacy
    {
        Tiger,
        Emperor
    }

    public enum WarriorShout
    {
        Battle,
        Commanding
    }

    public enum DruidForm
    {
        None,
        Bear,
        Cat,
        Moonkin
    }

    public enum ShadowPriestRotation
    {
        Leveling,
        Default,
    }

    public enum SubtletyRogueRotation
    {
        Algorithmic,
        Estimated
    }

    public enum ChakraStance
    {
        None,
        Serenity,
        Sanctuary
    }

    public enum MHPoisonType
    {
        Wound,
        Deadly
    }

    public enum OHPoisonType
    {
        MindNumbing,
        Crippling,
        Leeching,
        Paralytic
    }


    public enum WarlockPet
    {
        Disabled,
        Auto,
        Imp,
        Felhunter,
        Succubus,
        Voidwalker,
        Felguard
    }

    public enum PetType
    {
        // These are CreatureFamily IDs. See 'CurrentPet' for usage.
        None = 0,

        Imp = 23,
        Felguard = 29,
        Voidwalker = 16,
        Felhunter = 15,
        Succubus = 17,
        FelImp = 100,
        Wrathguard = 104,
        Voidlord = 101,
        Observer = 103,
        Shivarra = 102
    }

    public enum OracleWatchMode
    {
        Healer,
        DPS,
        Tank
    }

    public enum GroupType
    {
        Solo,
        Party,
        Raid
    }

    public enum WoWContext
    {
        PVE,
        Battleground,
        Solo,
        Instances,
        None
    }

    [Flags]
    public enum TargetFilter
    {
        None = 0,
        Tanks = 1,
        Healers = 2,
        Damage = 4,
        FlagCarrier = 5,
        EnemyHealers = 6,
        Threats = 7,
        LowHealth = 8,
        MostFocused = 9
    }

    public enum FeralSymbiosis
    {
        None = 0,
        DeathCoil = 1,                       // DK      -> Feral Druid
        PlayDead = 2,                        // Hunter  -> Feral Druid
        FrostNova = 3,                       // Mage    -> Feral Druid
        Clash = 4,                           // Monk    -> Feral Druid
        DivineShield = 5,                    // Paladin -> Feral Druid
        Dispersion = 6,                      // Priest  -> Feral Druid
        Redirect = 7,                        // Rogue   -> Feral Druid
        FeralSpirit = 8,                     // Shaman  -> Feral Druid
        SoulSwap = 9,                        // Warlock -> Feral Druid
        ShatteringBlow = 10,                 // Warrior -> Feral Druid
    }

    public enum GuardianSymbiosis
    {
        None = 0,
        BoneShield = 1,                          // DK      -> Guardian Druid
        IceTrap = 2,                             // Hunter  -> Guardian Duid
        MageWard = 3,                            // Mage    -> Guardian Druid
        ElusiveBrew = 4,                         // Monk    -> Guardian Druid
        Consecration = 5,                        // Paladin -> Guardian Druid
        FearWard = 6,                            // Priest  -> Guardian Druid
        Feint = 7,                               // Rogue   -> Guardian Druid
        LightningShield = 8,                     // Shaman  -> Guardian Druid
        LifeTap = 9,                             // Warlock -> Guardian Druid
        SpellReflection = 10,                    // Warrior -> Guardian Druid
    }

    internal enum MultiDoTMethod
    {
        AroundTarget,
        EverythingAroundMe,
        None
    }

    public enum KeyStates
    {
        None = 0,
        Down = 1,
        Toggled = 2
    }
    public enum TeBUsage
    { 
        None,
        Cooldown,
        Heroism,
        RoRo
    }
}