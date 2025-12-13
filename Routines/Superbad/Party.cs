#region

using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

#endregion

namespace Superbad
{
    /*
     * Apply Aura: Mod Rating (33554432) Value: 5
     * Apply Aura: Mod Stat - % (Strength, Agility, Intellect) Value: 5
     * Apply Aura: 
     */

    /// <summary>
    ///     indicates buff category an aura belongs to.  values must be a unique bit to allow creating
    ///     masks to represent a single aura that provides buffs in multiple categories, such as
    ///     Arcane Brilliance being PartyBuff.Spellpower+PartyBuff.Crit
    /// </summary>
    [Flags]
    public enum PartyBuffType
    {
        // from http://www.wowhead.com/guide=1100
        None = 0,
        Stats = 1, // Mark of the Wild, Legacy of the Emperor, Blessing of Kings, Embrace of the Shale Spider
        Stamina = 1 << 1, // PW:Fortitude, Imp: Blood Pact, Commanding Shout, Qiraji Fortitude
        AttackPower = 1 << 2, // Horn of Winter, Trueshot Aura, Battle Shout
        SpellPower = 1 << 3, // Arcane Brilliance, Dalaran Brilliance, Burning Wrath, Dark Intent, Still Water
        Haste = 1 << 4, // Unholy Aura, Swiftblade's Cunning, Unleashed Rage, Crackling Howl, Serpent's Swiftness
        SpellHaste = 1 << 5, // Moonkin Aura, Shadowform, Elemental Oath, Mind Quickening
        Crit = 1 << 6,
        // Leader of the Pack, Arcane Brilliance, Dalaran Brilliance, Legacy of the White Tiger, Bellowing Roar, Furious Howl, Terrifying Roar, Fearless Roar, Still Water
        Mastery = 1 << 7 // Blessing of Might, Grace of Air, Roar of Courage, Spirit Beast Blessing
    }


    public static class PartyBuff
    {
        private static readonly Dictionary<string, PartyBuffType> DictBuffs = new Dictionary<string, PartyBuffType>
        {
            {"Mark of the Wild", PartyBuffType.Stats},
            {"Legacy of the Emperor", PartyBuffType.Stats},
            {"Blessing of Kings", PartyBuffType.Stats},
            {"Embrace of the Shale Spider", PartyBuffType.Stats},
            {"Leader of the Pack", PartyBuffType.Crit},
            {"Legacy of the White Tiger", PartyBuffType.Crit},
            {"Bellowing Roar", PartyBuffType.Crit},
            {"Furious Howl", PartyBuffType.Crit},
            {"Terrifying Roar", PartyBuffType.Crit},
            {"Fearless Roar", PartyBuffType.Crit},
        };

        /// <summary>
        ///     time next PartyBuff attempt allowed
        /// </summary>
        public static DateTime TimeAllowBuff = DateTime.Now;

        /// <summary>
        ///     minimum TimeSpan to wait between PartyBuff casts
        /// </summary>
        private static int _secsBeforeBattle;

        /// <summary>
        ///     # of seconds this character waits prior to start of battleground
        ///     to do PartyBuff casts.  if 0 when retrieving value, will initialize
        ///     it to a random value between 5 and 12.  to force it to recalc a
        ///     new random value, set to 0 perodically
        /// </summary>
        public static int SecsBeforeBattle
        {
            get
            {
                if (_secsBeforeBattle == 0)
                    _secsBeforeBattle = new Random().Next(5, 12);

                return _secsBeforeBattle;
            }

            set { _secsBeforeBattle = value; }
        }

        /// <summary>
        ///     maps a Spell name to its associated PartyBuff vlaue
        /// </summary>
        /// <param name="name"> spell name </param>
        /// <returns> PartyBuff enum mask if exists for spell, otherwise PartyBuff.None </returns>
        public static PartyBuffType GetPartyBuffForSpell(string name)
        {
            PartyBuffType bc;
            if (!DictBuffs.TryGetValue(name, out bc))
                bc = PartyBuffType.None;

            return bc;
        }


        /// <summary>
        ///     gets a PartyBuff mask representing all categories of buffs present
        /// </summary>
        /// <param name="unit"> WoWUnit to check for buffs </param>
        /// <returns> PartyBuff mask representing all buffs founds </returns>
        public static PartyBuffType GetPartyBuffs(this WoWUnit unit)
        {
            return unit.GetAllAuras()
                .Select(a => GetPartyBuffForSpell(a.Name))
                .Aggregate(PartyBuffType.None, (current, bc) => current | bc);
        }

        /// <summary>
        ///     gets a PartyBuff mask representing all categories of buffs missing
        /// </summary>
        /// <param name="unit"> WoWUnit to check for missing buffs </param>
        /// <returns> PartyBuff mask representing all buffs that are missing </returns>
        public static PartyBuffType GetMissingPartyBuffs(this WoWUnit unit)
        {
            const PartyBuffType buffMask = PartyBuffType.Stats | PartyBuffType.Stamina | PartyBuffType.AttackPower |
                                           PartyBuffType.SpellPower | PartyBuffType.Haste | PartyBuffType.SpellHaste |
                                           PartyBuffType.Crit | PartyBuffType.Mastery;
            PartyBuffType buffs = GetPartyBuffs(unit);
            buffs = (~buffs) & buffMask;
            return buffs;
        }

        /// <summary>
        ///     returns true if has been atleast 1 minute since last blessing attempt.
        ///     .. if in battlegrounds will also check that the battle only has
        ///     .. random # seconds (5 to 12) remaining or has already begun
        /// </summary>
        /// <returns> </returns>
        public static bool IsItTimeToBuff()
        {
            if (!(StyxWoW.Me.GroupInfo.IsInParty || StyxWoW.Me.GroupInfo.IsInRaid))
                return true;

            if (DateTime.Now < TimeAllowBuff)
                return false;

            if (!Battlegrounds.IsInsideBattleground)
                return true;

            return DateTime.Now > (Battlegrounds.BattlefieldStartTime - new TimeSpan(0, 0, SecsBeforeBattle));
        }

        public static bool NeedGrpBuff()
        {
            WoWUnit buffunit = null;
            if (IsItTimeToBuff()
                && Superbad.HasSpellMarkoftheWild
                &&
                (!StyxWoW.Me.Mounted ||
                 (Battlegrounds.IsInsideBattleground && DateTime.Now < Battlegrounds.BattlefieldStartTime)))
            {
                buffunit =
                    Unit.GroupMembers.FirstOrDefault(
                        m =>
                            m.IsAlive && m.DistanceSqr < 30*30 &&
                            (PartyBuffType.None != (m.GetMissingPartyBuffs() & GetPartyBuffForSpell("Mark of the Wild"))));
            }
            return buffunit != null;
        }
    }
}