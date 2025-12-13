using System;
using System.Collections.Generic;
using System.Linq;

using Styx;
using Styx.WoWInternals;
using System.Drawing;
using Bots.DungeonBuddy.Helpers;
using Styx.Common;

namespace KingWoW
{
    class TalentManager
    {
        //public const int TALENT_FLAG_ISEXTRASPEC = 0x10000;

        public WoWSpec CurrentSpec { get; private set; }

        public List<Talent> Talents { get; private set; }

        public HashSet<string> Glyphs { get; private set; }

        public TalentManager()
        {
            Talents = new List<Talent>();
            Glyphs = new HashSet<string>();
        }

        public bool IsSelected(int index)
        {
            return Talents.FirstOrDefault(t => t.Index == index).Selected;
        }

        /// <summary>
        ///   Checks if we have a glyph or not
        /// </summary>
        /// <param name = "glyphName">Name of the glyph without "Glyph of". i.e. HasGlyph("Aquatic Form")</param>
        /// <returns></returns>
        public bool HasGlyph(string glyphName)
        {
            return Glyphs.Any() && Glyphs.Contains(glyphName);
        }

        public void Update()
        {
            // Keep the frame stuck so we can do a bunch of injecting at once.
            using (StyxWoW.Memory.AcquireFrame())
            {
                CurrentSpec = StyxWoW.Me.Specialization;
                Talents.Clear();

                // Always 18 talents. 6 rows of 3 talents.
                for (int index = 1; index <= 6 * 3; index++)
                {
                    var selected =
                        Lua.GetReturnVal<bool>(
                            string.Format(
                                "local t= select(5,GetTalentInfo({0})) if t == true then return 1 end return nil", index),
                            0);
                    var t = new Talent { Index = index, Selected = selected };
                    Talents.Add(t);
                }

                Glyphs.Clear();

                // 6 glyphs all the time. Plain and simple!
                for (int i = 1; i <= 6; i++)
                {
                    List<string> glyphInfo = Lua.GetReturnValues(String.Format("return GetGlyphSocketInfo({0})", i));

                    // add check for 4 members before access because empty sockets weren't returning 'nil' as documented
                    if (glyphInfo != null && glyphInfo.Count >= 4 && glyphInfo[3] != "nil" &&
                        !string.IsNullOrEmpty(glyphInfo[3]))
                    {
                        Glyphs.Add(WoWSpell.FromId(int.Parse(glyphInfo[3])).Name.Replace("Glyph of ", ""));
                    }
                }

            }

        }


        public struct Talent
        {
            public bool Selected;
            public int Index;
        }

    }
}