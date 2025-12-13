#define USE_TRAINING_DUMMY

using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieHolyPriestPvP
{
    public static class Extensions
    {
        /// <summary>
        /// Nem veszi figyelembe, hogy az aura kitol erkezett.
        /// </summary>
        internal static double AuraTimeLeft(this WoWUnit unit, SpellIDs spellId)
        {
            if (unit == null || !unit.IsValid || !unit.IsAlive)
                return 0;

            WoWAura aura = unit.GetAllAuras().FirstOrDefault(a => a.SpellId == (int)spellId);

            if (aura == null)
                return 0;

            return aura.TimeLeft.TotalMilliseconds;
        }

        internal static bool HasAura(this WoWUnit unit, SpellIDs spellId)
        {
            return unit.HasAura((int)spellId);
        }

        internal static bool HasAura(this WoWUnit unit, SpellIDs spellId, int stacks, WoWUnit creator = null)
        {
            return unit.GetAllAuras().Any(a => a.SpellId == (int)spellId && a.StackCount >= stacks && (creator == null || a.CreatorGuid == creator.Guid));
        }

        internal static bool HasPendingSpell(this LocalPlayer player, SpellIDs spellId)
        {
            if (player == null)
                return false;
            
            return player.HasPendingSpell((int)spellId);
        }

        internal static bool IsValidUnit(this WoWUnit u)
        {
            return u != null && u.IsValid && u.Attackable && u.IsAlive;
        }

        internal static bool IsTrainingDummy(this WoWUnit unit)
        {
#if USE_TRAINING_DUMMY
            return unit.Name.Contains("Training Dummy");
#else
            return false;
#endif
        }

        public static bool Between(this double num, double lower, double upper, bool inclusive = false)
        {
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }

        public static bool HasSpiritOfRedemption(this WoWUnit unit)
        {
            if (unit == null)
                return false;

            return unit.GetAllAuras().Any(a => {
                int spellId = a.SpellId;
                return spellId == (int)SpellIDs.SpiritOfRedemption1 ||
                       spellId == (int)SpellIDs.SpiritOfRedemption2 ||
                       spellId == (int)SpellIDs.SpiritOfRedemption3;
            });
        }

        #region IsEnemyOrPartyMember

        public static bool IsMyPartyRaidMember(this WoWUnit u)
        {
            if (u == UnitManager.Me)
                return true;

            WoWPlayer player = (!u.IsPlayer ? u.CreatedByUnit : u) as WoWPlayer;
            if (player == null || IsEnemy(player))
                return false;

            return player.IsInMyPartyOrRaid;
        }

        public static bool IsEnemy(this WoWUnit target)
        {
            if (target == null || target == UnitManager.Me || !target.CanSelect || !target.Attackable || !target.IsAlive || target.IsNonCombatPet || target.IsCritter)
                return false;

            if (!target.IsPlayer)
                return target.IsHostile || target.Aggro || target.PetAggro;

            WoWPlayer p = target as WoWPlayer;
            //p.GetReactionTowards(UnitManager.Me) == WoWUnitReaction.Hostile
            
            return p.IsHorde != UnitManager.Me.IsHorde || ((Main.IsArena || Main.IsRBG) && !p.IsInMyPartyOrRaid);
        }

        #endregion

        #region TalentSort

        public static SpecTypes TalentSort(this WoWUnit target)
        {
            if (target == null || !target.IsValid || target.IsDisabled)
                return SpecTypes.Invalid;
            
            switch (target.Class)
            {
                case WoWClass.DeathKnight:
                    return SpecTypes.Melee;
                case WoWClass.Druid:
                    if (target.MaxMana < target.MaxHealth / 2)
                        return SpecTypes.Melee;
                    if (target.HasAura(106735)) //Restoration Overrides Passive
                        return SpecTypes.Healer;
                    return SpecTypes.Caster;
                case WoWClass.Hunter:
                    return SpecTypes.Ranged;
                case WoWClass.Mage:
                    return SpecTypes.Caster;
                case WoWClass.Monk:
                    if (target.HasAura(115070)) //Stance of the Wise Serpent
                        return SpecTypes.Healer;
                    return SpecTypes.Melee;
                case WoWClass.Paladin:
                    if (target.MaxMana > target.MaxHealth / 2)
                        return SpecTypes.Healer;
                    return SpecTypes.Melee;
                case WoWClass.Priest:
                    if (target.HasAura(114884)) //Vampiric Touch <DND>
                        return SpecTypes.Caster;
                    return SpecTypes.Healer;
                case WoWClass.Rogue:
                    return SpecTypes.Melee;
                case WoWClass.Shaman:
                    if (target.MaxMana < target.MaxHealth / 2)
                        return SpecTypes.Melee;
                    if (target.HasAura(16213)) //Purification
                        return SpecTypes.Healer;
                    return SpecTypes.Caster;
                case WoWClass.Warlock:
                    return SpecTypes.Caster;
                case WoWClass.Warrior:
                    return SpecTypes.Melee;
            }
            return SpecTypes.Invalid;
        }

        #endregion

        #region MassDispelableInvulnerability

        public static bool MassDispelableInvulnerability(this WoWUnit target)
        {
            if (target == null || !target.IsValid)
                return false;

            return target.HasAura(SpellIDs.IceBlock) || 
                   target.HasAura(SpellIDs.DivineShield) || 
                   target.HasAura(SpellIDs.IceBlockSymbiosis) || 
                   target.HasAura(SpellIDs.HandOfProtection);
        }

        #endregion

        #region DebuffCCPurifyASAPHS

        public static bool DebuffCCleanseASAP(this WoWUnit target)
        {
            if (target == null || !target.IsValid || !target.IsPlayer)
                return false;

            if (target.ActiveAuras.Any(a => HolySettings.DebuffCCPurifyASAPHS.Contains(a.Value.SpellId) && a.Value.TimeLeft.TotalMilliseconds > 3000))
                return true;
            return false;
        }

        #endregion

        #region DebuffDoNotCleanse

        public static bool DebuffDoNotDispel(this WoWUnit target)
        {
            if (target == null)
                return false;

            return //target.HasAura(SpellIDs.FlameShock) ||
                   target.HasAura(SpellIDs.UnstableAffliction) ||
                   target.HasAura(SpellIDs.VampiricTouch);
        }

        #endregion

        #region CountDebuff

        public static int CountDebuff(this WoWUnit u)
        {
            if (u == null || !u.IsValid || !u.IsAlive)
                return 0;

            int numberofDebuff =
                u.Debuffs.Values.Count(
                    debuff =>
                    (debuff.Spell.DispelType == WoWDispelType.Magic ||
                        debuff.Spell.DispelType == WoWDispelType.Disease));

            return numberofDebuff;
        }

        #endregion

        #region Spells to be interrupted

        private static readonly HashSet<int> SpellsToInterruptHS = new HashSet<int>() {
            111771, //Demonic Gateway 
            115750, //Blinding Light 
            635, //Holy Light 
            82326, //Divine Light 
            31687, //Summon Water Elemental        
            12051, //Evocation 
            113724, //Ring Of Frost 
            689, //Drain Life 
            1120, //Drain Soul 
            73920, //Healing Rain 
            740, //Tranquility 
            15407, //Mind Flay 
            64843, //Divine Hymn 
            2006, //Resurrection 
            47540, //Penance 
            64901, //Hymn Of Hope 
            32546, //Binding Heal 
            33786, //Cyclone 
            1064, //Chain Heal 
            123986, //Chi Burst 
            116858, //Chaos Bolt 
            48018, //Demonic Circle: Summon 
            82326, //Divine Light 
            605, //Dominate Mind 
            339, //Entangling Roots 
            124682, //Enveloping Mist 
            5782, //Fear 
            133, //Fireball 
            2061, //Flash Heal 
            19750, //Flash of Light 
            112948, //Frost Bomb 
            102051, //Frostjaw 
            116, //Frostbolt 
            2060, //Greater Heal 
            77472, //Greater Healing Wave 
            2050, //Heal 
            8004, //Healing Surge 
            5185, //Healing Touch 
            331, //Healing Wave 
            51514, //Hex 
            82327, //Holy Radiance 
            32375, //Mass Dispel 
            8092, //Mind Blast 
            118, //Polymorph 
            11366, //Pyroblast 
            20484, //Rebirth 
            8936, //Regrowth 
            20066, //Repentance 
            6353, //Soul Fire 
            2912, //Starfire 
            78674, //Starsurge 
            116694, //Surging Mist 
            101643, //Transcendence  - 0.5sec
            119996, //Transcendence: Transfer  - 0.5sec
            30108, //Unstable Affliction 
            5176, //Wrath 
            34914 //Vampiric Touch 
        };

        public static bool NeedsInterrupt(this WoWUnit target)
        {
            if (!target.IsPlayer ||
                target.Class == WoWClass.DeathKnight ||
                target.Class == WoWClass.Hunter ||
                target.Class == WoWClass.Rogue ||
                target.Class == WoWClass.Warrior ||
                !(target.IsCasting || target.IsChanneling))
            {
                return false;
            }

            if (HolySettings.IsDebugMode)
            {
                if (target.IsCasting)
                    Logging.Write(target.Name + " is casting " + target.CastingSpell.Name + "(id: " + target.CastingSpellId + ")");

                if (target.IsChanneling)
                    Logging.Write(target.Name + " is channeling " + target.ChanneledSpell.Name + "(id: " + target.ChanneledCastingSpellId + ")");
            }

            return SpellsToInterruptHS.Contains(target.CastingSpellId) && target.CurrentCastTimeLeft.TotalMilliseconds <= HolySettings.Instance.InterruptSpellCastsAfter || SpellsToInterruptHS.Contains(target.ChanneledCastingSpellId);
        }

        #endregion

        #region Moving - Internal Extentions
        //Thanks for the PureRotation developers

        /// <summary>
        /// Internal IsMoving check which ignores turning on the spot.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool IsMoving(this WoWUnit unit)
        {
            return unit.MovementInfo.MovingBackward || unit.MovementInfo.MovingForward || unit.MovementInfo.MovingStrafeLeft || unit.MovementInfo.MovingStrafeRight;
        }

        /// <summary>
        /// Internal IsMoving check which ignores turning on the spot, and allows specifying how long you've been moving for before accepting it as actually moving. 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="movingDuration">Duration in MS how long the unit has been moving before accepting it as a moving unit</param>
        /// <returns></returns>
        public static bool IsMoving(this WoWUnit unit, int movingDuration)
        {
            return unit.IsMoving() && unit.MovementInfo.TimeMoved >= movingDuration;
        }

        #endregion

        #region MyAuraTimeLeft

        public static bool IsMyAuraFadingOnUnit(this WoWUnit unit, SpellIDs spellId, long millisecs)
        {
            return unit.MyAuraTimeLeft(spellId) <= millisecs;
        }

        public static double MyAuraTimeLeft(this WoWUnit u, SpellIDs auraId)
        {
            if (u == null || !u.IsValid || !u.IsAlive)
                return 0;

            WoWAura aura = u.GetAllAuras().FirstOrDefault(a => a.SpellId == (int)auraId && a.CreatorGuid == UnitManager.Me.Guid);

            if (aura == null)
                return 0;

            return aura.TimeLeft.TotalMilliseconds;
        }

        #endregion

        #region Has My Aura (can use stacks)

        public static bool HasMyAura(this WoWUnit unit, SpellIDs spellId, int stacks)
        {
            return UnitManager.Me.HasAuraCreatedBy(spellId, stacks, UnitManager.Me);
        }

        public static bool HasAuraCreatedBy(this WoWUnit unit, SpellIDs spellId, int stacks, WoWUnit creator)
        {
            return unit.GetAllAuras().Any(a => a.SpellId == (int)spellId && a.StackCount >= stacks && (creator == null || a.CreatorGuid == creator.Guid));
        }

        #endregion

        #region DebuffRootOrSnare

        private static readonly HashSet<int> DebuffRootOrSnareHS = new HashSet<int> {
            96294, //Chains of Ice (Chilblains)
            116706, //Disable
            64695, //Earthgrab (Earthgrab Totem)
            339, //Entangling Roots
            113770, //Entangling Roots (Force of Nature - Balance Treants)
            19975, //Entangling Roots (Nature's Grasp)
            113275, //Entangling Roots (Symbiosis)
            113275, //Entangling Roots (Symbiosis)
            19185, //Entrapment
            33395, //Freeze
            63685, //Freeze (Frozen Power)
            39965, //Frost Grenade
            122, //Frost Nova
            110693, //Frost Nova (Mage)
            55536, //Frostweave Net
            87194, //Glyph of Mind Blast
            111340, //Ice Ward
            45334, //Immobilized (Wild Charge - Bear)
            90327, //Lock Jaw (Dog)
            102359, //Mass Entanglement
            128405, //Narrow Escape
            13099, //Net-o-Matic
            115197, //Partial Paralysis
            50245, //Pin (Crab)
            91807, //Shambling Rush (Dark Transformation)
            123407, //Spinning Fire Blossom
            107566, //Staggering Shout
            54706, //Venom Web Spray (Silithid)
            114404, //Void Tendril's Grasp
            4167, //Web (Spider)
            50433, //Ankle Crack (Crocolisk)
            110300, //Burden of Guilt
            35101, //Concussive Barrage
            5116, //Concussive Shot
            120, //Cone of Cold
            3409, //Crippling Poison
            18223, //Curse of Exhaustion
            45524, //Chains of Ice
            50435, //Chilblains
            121288, //Chilled (Frost Armor)
            1604, //Dazed
            63529, //Dazed - Avenger's Shield
            50259, //Dazed (Wild Charge - Cat)
            26679, //Deadly Throw
            119696, //Debilitation
            116095, //Disable
            123727, //Dizzying Haze
            3600, //Earthbind (Earthbind Totem)
            77478, //Earthquake (Glyph of Unstable Earth)
            123586, //Flying Serpent Kick
            113092, //Frost Bomb
            54644, //Frost Breath (Chimaera)
            8056, //Frost Shock
            116, //Frostbolt
            8034, //Frostbrand Attack
            44614, //Frostfire Bolt
            61394, //Frozen Wake (Glyph of Freezing Trap)
            1715, //Hamstring
            13810, //Ice Trap
            58180, //Infected Wounds
            118585, //Leer of the Ox
            15407, //Mind Flay
            12323, //Piercing Howl
            115000, //Remorseless Winter
            20170, //Seal of Justice
            47960, //Shadowflame
            31589, //Slow
            129923, //Sluggish (Glyph of Hindering Strikes)
            61391, //Typhoon
            51490, //Thunderstorm
            127797, //Ursol's Vortex
            137637, //Warbringer
        };

        public static bool ShouldDebuffRootOrSnare(this WoWUnit target)
        {
            return target.MovementInfo.RunSpeed < 4.5 || target.ActiveAuras.Any(a => a.Value.ApplyAuraType == WoWApplyAuraType.ModRoot ||
                                                                                        a.Value.ApplyAuraType == WoWApplyAuraType.ModDecreaseSpeed ||
                                                                                        DebuffRootOrSnareHS.Contains(a.Value.SpellId));
        }

        #endregion

        #region Buffs to Dispel ASAP

        public static bool ShouldDispelBuffASAP(this WoWUnit target)
        {
            if (target == null || !target.IsValid || target.IsPet || !target.IsPlayer)
                return false;

            return target.ActiveAuras.Any(a => HolySettings.Instance.BuffDispelASAPHS.Contains(a.Value.SpellId));
        }

        #endregion

        #region IsFeared

        private static readonly HashSet<int> FearedOrIncapacitatedHS = new HashSet<int> {
            5782, //Fear
            118699, //Fear
            130616, //Fear (Glyph of Fear)
            5484, //Howl of Terror
            113056, //Intimidating Roar [Cowering in fear] (Warrior)
            113004, //Intimidating Roar [Fleeing in fear] (Warrior)
            5246, //Intimidating Shout (aoe)
            20511, //Intimidating Shout (targeted)
            64044, //Psychic Horror
            8122, //Psychic Scream
            113792, //Psychic Terror (Psyfiend)
            6770, //Sap
            20066 //Repentance
        };


        public static bool IsFearedOrIncapacitated(this WoWUnit target)
        {
            return target.ActiveAuras.Any(a => FearedOrIncapacitatedHS.Contains(a.Value.SpellId));
        }

        private static readonly HashSet<int> FearedOrCharmedHS = new HashSet<int> {
            5782, //Fear
            118699, //Fear
            130616, //Fear (Glyph of Fear)
            5484, //Howl of Terror
            113056, //Intimidating Roar [Cowering in fear] (Warrior)
            113004, //Intimidating Roar [Fleeing in fear] (Warrior)
            5246, //Intimidating Shout (aoe)
            20511, //Intimidating Shout (targeted)
            115268, //Mesmerize (Shivarra)
            64044, //Psychic Horror
            8122, //Psychic Scream
            113792, //Psychic Terror (Psyfiend)
            132412, //Seduction (Grimoire of Sacrifice)
            6358, //Seduction (Succubus)
            87204, //Sin and Punishment
            104045 //Sleep (Metamorphosis)
        };


        public static bool IsFearedOrCharmed(this WoWUnit target)
        {
            return target.ActiveAuras.Any(a => FearedOrCharmedHS.Contains(a.Value.SpellId));
        }

        #endregion

        #region DebuffRoot

        private static readonly HashSet<int> DebuffRootHS = new HashSet<int> {
            96294, //Chains of Ice (Chilblains)
            116706, //Disable
            64695, //Earthgrab (Earthgrab Totem)
            339, //Entangling Roots
            113770, //Entangling Roots (Force of Nature - Balance Treants)
            19975, //Entangling Roots (Nature's Grasp)
            113275, //Entangling Roots (Symbiosis)
            113275, //Entangling Roots (Symbiosis)
            19185, //Entrapment
            33395, //Freeze
            63685, //Freeze (Frozen Power)
            39965, //Frost Grenade
            122, //Frost Nova
            110693, //Frost Nova (Mage)
            55536, //Frostweave Net
            87194, //Glyph of Mind Blast
            111340, //Ice Ward
            45334, //Immobilized (Wild Charge - Bear)
            90327, //Lock Jaw (Dog)
            102359, //Mass Entanglement
            128405, //Narrow Escape
            13099, //Net-o-Matic
            115197, //Partial Paralysis
            50245, //Pin (Crab)
            91807, //Shambling Rush (Dark Transformation)
            123407, //Spinning Fire Blossom
            107566, //Staggering Shout
            54706, //Venom Web Spray (Silithid)
            114404, //Void Tendril's Grasp
            4167, //Web (Spider)
        };

        public static bool ShouldRemoveRoot(this WoWUnit target)
        {
            return target.ActiveAuras.Any(a => DebuffRootHS.Contains(a.Value.SpellId));
        }

        #endregion

        #region Unfearable

        private static readonly HashSet<int> UnfearableHS = new HashSet<int> {
            6346, // Fear Ward
            18499 // Berserker Rage
        };

        public static bool IsUnfearable(this WoWUnit target)
        {
            return target.ActiveAuras.Any(a => UnfearableHS.Contains(a.Value.SpellId));
        }

        #endregion

        #region InvulnerableSpell

        public static bool HasInvulnerableSpell(this WoWUnit target)
        {
            if (target == null)
                return false;

            return target.HasAura(48707) || //"Anti-Magic Shell"
                   target.HasAura(31224) || //"Cloak of Shadows"
                   target.HasAura(45438) || //"Ice Block"
                   target.HasAura(8178)  || //"Grounding Totem Effect"
                   target.HasAura(114028)|| //"Mass Spell Reflection"
                   target.HasAura(23920) || //"Spell Reflection"
                   target.HasAura(33786) || //"Cyclone"
                   target.HasAura(19263) || //"Deterrence"
                   target.HasAura(642)   || //"Divine Shield"
                   target.HasAura(115176);  //"Zen Meditation"
        }

        #endregion

    }
}