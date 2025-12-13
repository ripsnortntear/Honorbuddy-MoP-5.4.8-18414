using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieShadowPriestPvP
{
    internal enum Talents : int
    {
        VoidTendrils = 108920,
        Psyfiend = 108921,
        DominateMind = 605,
        
        BodyAndSould = 64129,
        AngelicFeather = 121536,
        Phantasm = 108942,

        FromDarknessComesLight = 109186,
        Mindbender = 123040,
        
        SolaceAndInsanity = 139139,
        
        DesperatePrayer = 19236,
        SpectralGuise = 112833,
        AngelicBulwark = 108945,

        TwistOfFate = 109142,
        PowerInfusion = 10060,
        DivineInsight = 109175,

        Cascade = 121135,
        DivineStar = 110744,
        Halo = 120517
    }

    internal static class TalentManager
    {
        private static Dictionary<int, string> TalentDict = new Dictionary<int, string>();
        private static DateTime LastUpdate = DateTime.MinValue;

        public static System.Action OnReloaded;

        public static bool Reload()
        {
            if (LastUpdate.AddSeconds(2) > DateTime.Now)
                return false;
            LastUpdate = DateTime.Now;

            TalentDict.Clear();

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
                    else
                        Logging.Write("TalentManager.Reload - Talent '" + talentName + "' not found!");
                }

                if (spellId != 0)
                    TalentDict.Add(spellId, talentName);
            }

            foreach (int spellId in Enum.GetValues(typeof(Talents)))
            {
                SpellFindResults results;
                if (SpellManager.FindSpell(spellId, out results))
                    if (!TalentDict.ContainsKey(spellId))
                        TalentDict.Add(spellId, ((Talents)spellId).ToString());
            }

            if (OnReloaded != null)
                OnReloaded();

            return true;
        }

        public static bool Has(Talents talent)
        {
            return TalentDict.ContainsKey((int)talent);
        }

        public static void Print()
        {
            if (TalentDict.Count == 0)
                return;

            Logging.Write("\n---------Talents----------");

            foreach (var talent in TalentDict)
                Logging.Write(string.Format("{0,8}:{1, 35}", talent.Key, talent.Value));

            Logging.Write("------End Of Talents------\n");
        }
    }
}