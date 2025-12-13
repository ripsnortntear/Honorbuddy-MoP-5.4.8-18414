#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/ChronicleHealing.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using JetBrains.Annotations;
using Oracle.Healing.Chronicle.Classes;
using Oracle.Core;
using Oracle.Shared.Logging;
using Oracle.Shared.Utilities;
using Oracle.UI.Settings;
using Styx;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Healing.Chronicle
{
    [UsedImplicitly]
    internal class ChronicleHealing : Chronicle
    {
        private static bool ENABLE_HPM { get { return OracleSettings.Instance.ENABLE_HPM; } }

        private static uint URGENT_HEALTH_PERCENT { get { return OracleSettings.Instance.URGENT_HEALTH_PERCENT; } }

        internal static Dictionary<string, ChronicleSpell> AvailableChronicleSpells = new Dictionary<string, ChronicleSpell>();

        //Spellname, TotalCalculatedHeal - healthDeficit, ManaCost, CastTime, prio, Totalheal
        internal static List<Tuple<string, float, float, float, float, float, ChronicleSpellType, Tuple<bool>>> SingleTargetOrderedSpells = new List<Tuple<string, float, float, float, float, float, ChronicleSpellType, Tuple<bool>>>();

        public static ChronicleSpell GetChronicleSpell(string spell)
        {
            ChronicleSpell result;
            return AvailableChronicleSpells.TryGetValue(spell, out result) ? result : null;
        }

        public static void PopulateSingleTargetOrderedSpells(WoWUnit unit)
        {
            
                if (!OracleRoutine.IsViable(unit)) return; // gtfo

                //var maxhealth = unit.MaxHealth;
                //var currentHealth = Math.Min(unit.GetPredictedHealth(StyxWoW.Me.Class != WoWClass.Monk), maxhealth);
                //var healthDeficit = Math.Min(maxhealth - currentHealth, maxhealth);
                //var healthPercent = (currentHealth * 100) / maxhealth;

                ////Logger.Output("---> {0} MaxHealth: {1} - currentHealth {2} = Health Deficit {3}", unit.SafeName, unit.MaxHealth, unit.GetPredictedHealth(StyxWoW.Me.Class != WoWClass.Monk), healthDeficit);

                //Logger.Output("---> {1} Health Deficit: {0}", healthDeficit, unit.SafeName);

                var maxhealth = unit.MaxHealth;
                var currentHealth = (float)unit.CurrentHealth(unit.GetHPCheckType());
                var healthDeficit = maxhealth - currentHealth;
                var healthPercent = (currentHealth * 100) / maxhealth;

                Logger.PrioLogging(" [DIAG] ---> {1} ({4}) Health Deficit: {0} [Max: {2} Current: {3}]", healthDeficit, unit.SafeName, maxhealth, currentHealth, unit.GetHPCheckType());

                if (healthDeficit < 24000) return; // gtfo

                CreateSpellList();

                var chronicleSpells = AvailableChronicleSpells.Where(s => (int)s.Value.FriendlyCount == 1); // this should exclude AoE spells.

                SingleTargetOrderedSpells.Clear();

                var encounter = Total;
                if (encounter != null)
                {
                    // Extract us from the recorded encounter so we can check our spells.
                    var player = encounter.Players.Select(p => p.Value).FirstOrDefault(u => u.Name == StyxWoW.Me.Name);

                    if (player != null)
                    {
                        foreach (var healingSpell in player.Healingspells)
                        {
                            var prio = 100f;
                            var cSpell = healingSpell.Value;
                            var totalHeal = cSpell.MaxHeal;

                            // Calculated Error (Lower values are better)
                            float calcError = Math.Abs(totalHeal - healthDeficit);
                            var calcErrorPct = (calcError * 100) / maxhealth;

                            // Higher the number the closer the spell is to the deficit
                            prio -= (float)Math.Floor(calcErrorPct);

                            var xSpell = chronicleSpells.FirstOrDefault(s => s.Value.SpellName == cSpell.Name);

                            if (xSpell.Key == null) continue;

                            var HPS = (xSpell.Value.HPS);
                            var MPS = (xSpell.Value.MPS);
                            var SpellType = (xSpell.Value.SpellType);
                            var isInstant = xSpell.Value.Instant;
                            var manaCost = 0;

                            if (ChronicleSpell.IsPeriodicHeal(SpellType)) { continue; } // Exclude HoTs as it only records the initial heal.

                            if (ChronicleSpell.IsDamageHeal(SpellType))
                            {
                                prio += 10f; // for Smite, Holy Fire, etc
                            }

                            if (healthPercent < URGENT_HEALTH_PERCENT || (unit.Entry == 71604))
                            {
                                if (isInstant) prio += 50f;  // instants kgo!

                                prio += HPS; // Player is in trouble lets pull out the instant casts, etc
                            }
                            else
                            {
                                if (ENABLE_HPM)
                                {
                                    prio += (totalHeal / MPS); // Heals per mana should be fine here
                                }
                            }

                            SingleTargetOrderedSpells.Add(new Tuple<string, float, float, float, float, float, ChronicleSpellType, Tuple<bool>>(xSpell.Value.SpellNameOverload != "Ignore" ? xSpell.Value.SpellNameOverload : cSpell.Name, (float)Math.Floor(calcErrorPct), MPS, HPS, prio, totalHeal, SpellType, Tuple.Create(true)));
                        }
                    }
                }

                foreach (var s in chronicleSpells)
                {
                    var prio = 100f;
                    var cSpell = s.Value;
                    var HPS = (cSpell.HPS);
                    var MPS = (cSpell.MPS);
                    var totalHeal = cSpell.TotalCalculatedHeal;
                    var isInstant = cSpell.Instant;
                    var SpellType = cSpell.SpellType;

                    // Calculated Error (Lower values are better)
                    var calcError = Math.Abs(totalHeal - healthDeficit);
                    float calcErrorPct = (calcError * 100) / maxhealth;

                    // Higher the number the closer the spell is to the deficit
                    prio -= (float)Math.Floor(calcErrorPct);

                    if (ChronicleSpell.IsDamageHeal(SpellType))
                    {
                        prio += 10f; // for Smite, Holy Fire, etc
                    }

                    if (healthPercent < URGENT_HEALTH_PERCENT || (unit.Entry == 71604))
                    {
                        if (isInstant) prio += 50f;  // instants kgo!

                        prio += HPS; // Player is in trouble lets pull out the instant casts, etc
                    }
                    else
                    {
                        if (ENABLE_HPM)
                        {
                            prio += (totalHeal / MPS);  // Heals per mana should be fine here / calcErrorPct
                        }
                    }

                    var result = new Tuple<string, float, float, float, float, float, ChronicleSpellType, Tuple<bool>>(cSpell.SpellNameOverload != "Ignore" ? cSpell.SpellNameOverload : cSpell.SpellName, (float)Math.Floor(calcErrorPct), MPS, HPS, prio, totalHeal, cSpell.SpellType, Tuple.Create(false));

                    if (SingleTargetOrderedSpells.Exists(a => a.Item1 == result.Item1)) continue;

                    SingleTargetOrderedSpells.Add(result);
                }
            
        }

        public static double GetHPS(Encounter encounter, Player player)
        {
            using (new PerformanceLogger("GetHPS"))
            {
                var totalTime = GetPlayerActiveTime(encounter, player);

                return player.Healing / Math.Max(1, totalTime);
            }
        }

        public static double GetHPS(Encounter encounter, Player player, int healing)
        {
            using (new PerformanceLogger("GetHPSEncounter"))
            {
                var totalTime = GetPlayerActiveTime(encounter, player);

                return healing / Math.Max(1, totalTime);
            }
        }

        public static void Report(Encounter encounter)
        {
            Logger.Output("\n");
            Logger.Output("------ [Player Healing] ------");
            Logger.Output("Report for: {0}", encounter.Name);
            foreach (var player in encounter.Players.Values.ToList())
            {
                if (player.Healing < 1) continue;

                Logger.Output(string.Format("{0} .............{1:N0} : {2:N0} hps ({3:P0})", player.Name, player.Healing, GetHPS(encounter, player), (player.Healing / encounter.Healing) * 100));
            }
            Logger.Output("\n");
            Logger.Output("------ [              ] ------");

            foreach (var encounter1 in Encounters.Values.ToList())
            {
                Logger.Output(string.Format("{0} .............{1} {2} to {3}", encounter1.Name, encounter1.Healing, encounter1.StartTime, encounter1.EndTime));
            }
        }

        internal static void LogHeal(Encounter encounter, Heal heal, bool isAbsorb = false)
        {
            using (new PerformanceLogger("LogHeal"))
            {
                // Get the player from encounter.
                var player = GetPlayer(encounter, heal.Playerid, heal.Playername);

                // Should Subtract overhealing here..but, we want the total heal!  //- heal.Overhealing
                var amount = Math.Max(0, heal.Amount);

                // Add absorbed
                amount = amount + heal.Absorbed;

                // Add to player total.
                player.Healing = player.Healing + amount;
                player.Overhealing = player.Overhealing + heal.Overhealing;
                player.Healingabsorbed = player.Healingabsorbed + heal.Absorbed;
                if (isAbsorb)
                    player.Shielding = player.Shielding + amount;

                // Also add to encounter total healing.
                encounter.Healing = encounter.Healing + amount;
                encounter.Overhealing = encounter.Overhealing + heal.Overhealing;
                encounter.Healingabsorbed = encounter.Healingabsorbed + heal.Absorbed;
                if (isAbsorb)
                    encounter.Shielding = encounter.Shielding + amount;

                var unitClass = "Uknown"; //TODO: //Lua.GetReturnVal<string>("return select(2, UnitClass(" + heal.DstName + "))", 0);

                // Add to recipient healing.
                Healed healed;
                if (player.PlayerHealed.TryGetValue(heal.DstName, out healed))
                {
                    healed.Amount = healed.Amount + amount;
                    if (isAbsorb)
                        healed.Shielding = healed.Shielding + amount;
                }
                else
                {
                    // Create Healed if it does not exist.
                    healed = new Healed { Class = unitClass, Amount = 0, Shielding = 0 };
                    healed.Amount = healed.Amount + amount;
                    if (isAbsorb)
                        healed.Shielding = healed.Shielding + amount;
                }

                player.PlayerHealed[heal.DstName] = healed;

                // Add to spell healing
                Spell spell;
                if (player.Healingspells.TryGetValue(heal.Spellname, out spell))
                {
                    spell.Healing = spell.Healing + amount;

                    if (heal.Critical)
                        spell.Critical = spell.Critical + 1;

                    spell.Overhealing = spell.Overhealing + heal.Overhealing;

                    spell.Absorbed = spell.Absorbed + heal.Absorbed;
                    if (isAbsorb)
                        spell.Shielding = spell.Shielding + amount;

                    spell.Hits = spell.Hits + 1;

                    if (spell.Min == 0 || amount < spell.Min)
                        spell.Min = amount;

                    if (spell.Max == 0 || amount > spell.Max)
                        spell.Max = amount;

                    // Total amount the heal hit for + overhealing
                    var averageHeal = (spell.Min + spell.Max) / 2;
                    if (spell.MaxHeal == 0 || averageHeal > spell.MaxHeal)
                        spell.MaxHeal = averageHeal;
                }
                else
                {
                    // Create spell if it does not exist.
                    spell = new Spell
                        {
                            Id = heal.Spellid,
                            Name = heal.Spellname,
                            Hits = 0,
                            Healing = 0,
                            Overhealing = 0,
                            Absorbed = 0,
                            Shielding = 0,
                            Critical = 0,
                            Min = 0,
                            Max = 0
                        };

                    spell.Healing = spell.Healing + amount;

                    if (heal.Critical)
                        spell.Critical = spell.Critical + 1;

                    spell.Overhealing = spell.Overhealing + heal.Overhealing;

                    spell.Absorbed = spell.Absorbed + heal.Absorbed;
                    if (isAbsorb)
                        spell.Shielding = spell.Shielding + amount;

                    spell.Hits = spell.Hits + 1;

                    if (spell.Min == 0 || amount < spell.Min)
                        spell.Min = amount;

                    if (spell.Max == 0 || amount > spell.Max)
                        spell.Max = amount;

                    // Total amount the heal hit for + overhealing
                    var averageHeal = (spell.Min + spell.Max) / 2;
                    if (spell.MaxHeal == 0 || averageHeal > spell.MaxHeal)
                        spell.MaxHeal = averageHeal;
                }

                player.Healingspells[heal.Spellname] = spell;
            }
        }

        public static void CreateSpellList()
        {
            switch (StyxWoW.Me.Specialization)
            {
                case WoWSpec.PaladinHoly:
                    ChroniclePaladin.GenerateSpellList();
                    break;

                case WoWSpec.PriestDiscipline:
                    ChronicleDiscPriest.GenerateSpellList();
                    break;

                case WoWSpec.PriestHoly:
                    ChronicleHolyPriest.GenerateSpellList();
                    break;

                case WoWSpec.ShamanRestoration:
                    ChronicleShaman.GenerateSpellList();
                    break;

                case WoWSpec.DruidRestoration:
                    ChronicleDruid.GenerateSpellList();
                    break;

                case WoWSpec.MonkMistweaver:
                    ChronicleMonk.GenerateSpellList();
                    break;
            }
        }
    }
}