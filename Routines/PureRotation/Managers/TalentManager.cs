#region Revision info

/*
 * $Author: tumatauenga1980 $
 * $Date: 2013-09-26 21:34:50 +0200 (Do, 26 Sep 2013) $
 * $ID$
 * $Revision: 1740 $
 * $URL: https://subversion.assembla.com/svn/purerotation/trunk/PureRotation/Managers/TalentManager.cs $
 * $LastChangedBy: tumatauenga1980 $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Collections.Generic;
using System.Linq;
using PureRotation.Helpers;
using PureRotation.Core.CombatLog;
using Styx;
using Styx.WoWInternals;

namespace PureRotation.Managers
{
    internal static class TalentManager
    {
        static TalentManager()
        {
            Talents = new List<Talent>();
            Glyphs = new HashSet<string>();
            CombatLogHandler.Register("CHARACTER_POINTS_CHANGED", UpdateTalentManager);
            CombatLogHandler.Register("GLYPH_UPDATED", UpdateTalentManager);
            CombatLogHandler.Register("ACTIVE_TALENT_GROUP_CHANGED", UpdateTalentManager);
            CombatLogHandler.Register("LEARNED_SPELL_IN_TAB", UpdateTalentManager);
        }

        public static WoWSpec CurrentSpec { get; private set; }

        private static List<Talent> Talents { get; set; }

        private static HashSet<string> Glyphs { get; set; }

        public static bool HasGlyph(string glyphName)
        {
            return Glyphs.Count > 0 && Glyphs.Contains(glyphName);
        }

        public static bool HasTalent(int index)
        {
            return Talents.FirstOrDefault(t => t.Index == index).Count != 0;
        }

        private static void UpdateTalentManager(CombatLogEventArgs args)
        {
            WoWSpec oldSpec = CurrentSpec;

            Update();

            if (CurrentSpec != oldSpec)
            {
                Logger.InfoLog("Your spec has been changed. Rebuilding rotation");
                PureRotationRoutine.Instance.RebuildBehaviors();
            }
        }

        public static void Update()
        {
            // Don't bother if we're < 10
            if (StyxWoW.Me.Level < 10)
            {
                CurrentSpec = WoWSpec.None;
                return;
            }

            // Keep the frame stuck so we can do a bunch of injecting at once.
            using (StyxWoW.Memory.AcquireFrame())
            {
                CurrentSpec = StyxWoW.Me.Specialization;

                Talents.Clear();

                // Always 18 talents. 6 rows of 3 talents.
                for (int index = 0; index <= 6 * 3; index++)
                {
                    var selected = Styx.WoWInternals.Lua.GetReturnVal<int>(string.Format("return GetTalentInfo({0})", index), 4);
                    switch (selected)
                    {
                        case 1:
                            {
                                var t = new Talent { Index = index, Count = 1 }; //Name = talentName
                                Talents.Add(t);
                            }
                            break;
                    }
                }
                Logger.DebugLog("Talentdetection - Talents choosen");
                foreach(var t in Talents)
                {
                    Logger.DebugLog("Talentdetection - Talent {0} - {1}", t.Index, t.Count > 0);
                }
                Glyphs.Clear();

                var glyphCount = Styx.WoWInternals.Lua.GetReturnVal<int>("return GetNumGlyphSockets()", 0);

                Logger.DebugLog("Glyphdetection - GetNumGlyphSockets {0}", glyphCount);

                if (glyphCount != 0)
                {
                    for (int i = 1; i <= glyphCount; i++)
                    {
                        var lua = String.Format("local enabled, glyphType, glyphTooltipIndex, glyphSpellID, icon = GetGlyphSocketInfo({0});if (enabled) then return glyphSpellID else return 0 end", i);
                        var glyphSpellId = Styx.WoWInternals.Lua.GetReturnVal<int>(lua, 0);
                        try
                        {
                            if (glyphSpellId > 0)
                            {
                                Logger.DebugLog("Glyphdetection - SpellId: {0},Name:{1} ,WoWSpell: {2}", glyphSpellId, WoWSpell.FromId(glyphSpellId).Name, WoWSpell.FromId(glyphSpellId));
                                Glyphs.Add(WoWSpell.FromId(glyphSpellId).Name.Replace("Glyph of ", ""));
                            }
                            else
                            {
                                Logger.DebugLog("Glyphdetection - Couldn't find all values to detect the Glyph in slot {0}", i);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.InfoLog("We couldn't detect your Glyphs");
                            Logger.InfoLog("Report this message to us: " + ex);
                        }
                    }
                }
            }
        }

        public static void Dumpglyphs()
        {
            foreach (var glyph in Glyphs)
            {
                Logger.InfoLog("{0}", glyph);
            }
        }

        #region Nested type: Talent

        private struct Talent
        {
            public int Count;
            public int Index;
        }

        #endregion Nested type: Talent
    }
}