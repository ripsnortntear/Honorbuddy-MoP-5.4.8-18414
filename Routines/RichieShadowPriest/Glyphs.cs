using Styx.Common;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieShadowPriestPvP
{
    internal enum Glyphs : int {
        GlyphOfFearWard = 55678,
        GlyphOfShadowWordDeath = 120583,
        GlyphOfMassDispel = 55691,
        GlyphOfFade = 55684
    }

    internal static class GlyphManager
    {
        private static Dictionary<int, WoWSpell> GlyphsDict = new Dictionary<int, WoWSpell>();
        private static DateTime LastUpdate = DateTime.MinValue;

        public static System.Action OnReloaded;

        public static bool Reload()
        {
            if (LastUpdate.AddSeconds(2) > DateTime.Now)
                return false;
            LastUpdate = DateTime.Now;

            GlyphsDict.Clear();

            int count = Lua.GetReturnVal<int>("return GetNumGlyphSockets()", 0);

            if (count == 0)
                return false;

            for (int i = 1; i <= count; i++)
            {
                try
                {
                    int id = Lua.GetReturnVal<int>(String.Concat("local enabled, glyphType, glyphTooltipIndex, glyphSpellID, icon = GetGlyphSocketInfo(", i.ToString(), ");if (enabled) then return glyphSpellID else return 0 end"), 0);

                    if (id > 0)
                        GlyphsDict.Add(id, WoWSpell.FromId(id));
                    else
                        Logging.Write("Glyphs.Reload - No Glyph in slot " + i);
                }
                catch (Exception ex)
                {
                    Logging.Write("Glyphs.Reload - We couldn't detect your Glyphs! Report this message to us: " + ex);
                }
            }

            if (OnReloaded != null)
                OnReloaded();

            return true;
        }

        public static bool Has(Glyphs glyph)
        {
            return GlyphsDict.ContainsKey((int)glyph);
        }

        public static void Print()
        {
            if (GlyphsDict.Count == 0)
                return;

            Logging.Write("\n---------Glyphs----------");

            foreach (var glyph in GlyphsDict)
                Logging.Write(string.Format("{0,8}:{1, 35}", glyph.Key, glyph.Value.Name));

            Logging.Write("------End Of Glyphs------\n");
        }
    }
}