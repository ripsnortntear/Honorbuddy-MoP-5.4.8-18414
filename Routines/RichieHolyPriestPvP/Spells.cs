using System;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace RichieHolyPriestPvP
{
    internal static class Spells
    {
        private static Dictionary<int, string> SpellDict = new Dictionary<int, string>();
        private static DateTime LastUpdate = DateTime.MinValue;

        public static System.Action OnReloaded;

        public static List<int> GetKnownSpellIds()
        {
            return SpellDict.Keys.ToList<int>();
        }

        public static bool Reload()
        {
            if (LastUpdate.AddSeconds(2) > DateTime.Now)
                return false;
            LastUpdate = DateTime.Now;

            SpellDict.Clear();

            SpellManager.Spells.ForEach(spell => SpellDict.Add(spell.Value.Id, spell.Key));

            // Add talents to the spell dictionary
            for (int i = 1; i <= 18; i++)
            {
                string istr = i.ToString();
                string talentName = Lua.GetReturnVal<String>(string.Concat("local t= select(5,GetTalentInfo(", istr, ")) if t == true then return select(1,GetTalentInfo(", istr, ")) end return nil"), 0);

                if (string.IsNullOrEmpty(talentName))
                    continue;

                int spellId = Lua.GetReturnVal<int>(string.Concat("return select(2,GetSpellBookItemInfo('", istr, "'))", talentName), 0);

                if (spellId == 0)
                {
                    SpellFindResults results;
                    if (SpellManager.FindSpell(talentName, out results))
                        spellId = results.Override != null ? results.Override.Id : results.Original.Id;
                }

                if (spellId != 0 && !SpellDict.ContainsKey(spellId))
                    SpellDict.Add(spellId, talentName);
            }

            foreach (int spellId in Enum.GetValues(typeof(Talents)))
            {
                SpellFindResults results;
                if (SpellManager.FindSpell(spellId, out results))
                    if (!SpellDict.ContainsKey(spellId))
                        SpellDict.Add(spellId, ((Talents)spellId).ToString());
            }

            if (OnReloaded != null)
                OnReloaded();

            return true;
        }

        public static bool Has(SpellIDs spellId)
        {
            return SpellDict.ContainsKey((int)spellId);
        }

        public static string Get(SpellIDs spellId)
        {
            string result = string.Empty;
            if (!SpellDict.TryGetValue((int)spellId, out result))
                return spellId.ToString();

            return result;
        }

        public static void AddAdditional(SpellIDs spellId, string name)
        {
            if (!SpellDict.ContainsKey((int)spellId))
                SpellDict.Add((int)spellId, name);
        }

        public static void Print()
        {
            if (SpellDict.Count == 0)
                return;

            Logging.Write("\n---------Spells----------");

            foreach(var spell in SpellDict)
                Logging.Write("{0, 8}{1, 35}", spell.Key, spell.Value);

            Logging.Write("------End Of Spells------\n");
        }
    }
}