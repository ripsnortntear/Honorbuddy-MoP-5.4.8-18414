#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-15 14:09:17 +1000 (Sun, 15 Sep 2013) $
 * $ID$
 * $Revision: 213 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Managers/TalentManager.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.CombatLog;
using Oracle.Shared.Logging;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oracle.Core.Managers
{
    // Credit to the Singular team for this class.

    internal static class TalentManager
    {
        //public const int TALENT_FLAG_ISEXTRASPEC = 0x10000;

        public static bool Between(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        static TalentManager()
        {
            Talents = new List<Talent>();
            TalentId = new int[6];
            Glyphs = new HashSet<string>();
            GlyphId = new int[6];

            CombatLogHandler.Register("PLAYER_LEVEL_UP", UpdateTalentManager);
            CombatLogHandler.Register("CHARACTER_POINTS_CHANGED", UpdateTalentManager);
            CombatLogHandler.Register("GLYPH_UPDATED", UpdateTalentManager);
            CombatLogHandler.Register("ACTIVE_TALENT_GROUP_CHANGED", UpdateTalentManager);
            CombatLogHandler.Register("PLAYER_SPECIALIZATION_CHANGED", UpdateTalentManager);
            CombatLogHandler.Register("LEARNED_SPELL_IN_TAB", UpdateTalentManager);
        }

        public static WoWSpec CurrentSpec { get; private set; }

        public static List<Talent> Talents { get; private set; }

        private static int[] TalentId { get; set; }

        public static HashSet<string> Glyphs { get; private set; }

        private static int[] GlyphId { get; set; }

        private static WaitTimer EventRebuildTimer = new WaitTimer(TimeSpan.FromSeconds(1));

        private static bool _Rebuild = false;

        private static bool RebuildNeeded
        {
            get
            {
                return _Rebuild;
            }
            set
            {
                _Rebuild = value;
                EventRebuildTimer.Reset();
            }
        }

        public static bool IsSelected(int index)
        {
            // return Talents.FirstOrDefault(t => t.Index == index).Selected;
            int tier = (index - 1) / 3;
            if (tier.Between(0, 5))
                return TalentId[tier] == index;
            return false;
        }

        /// <summary>
        ///   Checks if we have a glyph or not
        /// </summary>
        /// <param name = "glyphName">Name of the glyph without "Glyph of". i.e. HasGlyph("Aquatic Form")</param>
        /// <returns></returns>
        public static bool HasGlyph(string glyphName)
        {
            return Glyphs.Any() && Glyphs.Contains(glyphName);
        }

        private static void UpdateTalentManager(CombatLogEventArgs args)
        {
            var oldSpec = CurrentSpec;
            int[] oldTalent = TalentId;
            int[] oldGlyph = GlyphId;

            Logger.Output("{0} Event Fired!", args.Event);

            Update();

            if (args.Event == "PLAYER_LEVEL_UP")
            {
                RebuildNeeded = true;
                Logger.Output(" TalentManager: Your character has leveled up! Now level {0}", args.Event);
            }

            if (CurrentSpec != oldSpec)
            {
                RebuildNeeded = true;
                Logger.Output(" TalentManager: Your spec has been changed.");
            }

            int i;
            for (i = 0; i < 6; i++)
            {
                if (oldTalent[i] != TalentId[i])
                {
                    RebuildNeeded = true;
                    Logger.Output(" TalentManager: Your talents have changed.");
                    break;
                }
            }

            for (i = 0; i < 6; i++)
            {
                if (oldGlyph[i] != GlyphId[i])
                {
                    RebuildNeeded = true;
                    Logger.Output(" TalentManager: Your glyphs have changed.");
                    break;
                }
            }
        }

        public static void Update()
        {
            // Keep the frame stuck so we can do a bunch of injecting at once.
            using (StyxWoW.Memory.AcquireFrame())
            {
                CurrentSpec = StyxWoW.Me.Specialization;

                Talents.Clear();
                TalentId = new int[6];

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

                    if (selected)
                        TalentId[(index - 1) / 3] = index;
                }

                Glyphs.Clear();
                GlyphId = new int[6];

                // 6 glyphs all the time. Plain and simple!
                for (int i = 1; i <= 6; i++)
                {
                    List<string> glyphInfo = Lua.GetReturnValues(String.Format("return GetGlyphSocketInfo({0})", i));

                    // add check for 4 members before access because empty sockets weren't returning 'nil' as documented
                    if (glyphInfo != null && glyphInfo.Count >= 4 && glyphInfo[3] != "nil" &&
                        !string.IsNullOrEmpty(glyphInfo[3]))
                    {
                        GlyphId[i - 1] = int.Parse(glyphInfo[3]);
                        Glyphs.Add(WoWSpell.FromId(GlyphId[i - 1]).Name.Replace("Glyph of ", ""));
                    }
                }

                foreach (var glyph in Glyphs)
                {
                    Logging.WriteDiagnostic("Glyph of {0}", glyph);
                }

                foreach (var talent in Talents)
                {
                    Logging.WriteDiagnostic("{0} : {1}", talent.Index, talent.Selected);
                }
            }
        }

        public static bool Pulse()
        {
            if (EventRebuildTimer.IsFinished && RebuildNeeded)
            {
                RebuildNeeded = false;
                Logger.Output(" TalentManager: Rebuilding behaviors due to changes detected.");
                Update();   // reload talents just in case
                OracleRoutine.Instance.BuildBehaviors();
                return true;
            }

            return false;
        }

        #region Nested type: Talent

        public struct Talent
        {
            public bool Selected;
            public int Index;
        }

        #endregion Nested type: Talent
    }
}