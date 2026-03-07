using System;
using System.Collections.Generic;
using System.Linq;
using FuryUnleashed.Core.Helpers;
using FuryUnleashed.Core.Utilities;
using Styx;
using Styx.Common.Helpers;
using Styx.WoWInternals;
using Enum = FuryUnleashed.Core.Helpers.Enum;

namespace FuryUnleashed.Core.Managers
{
    internal static class TalentManager
    {
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

            CurrentSpec = StyxWoW.Me.Specialization;

            Lua.Events.AttachEvent("PLAYER_LEVEL_UP", UpdateTalentManager);
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", UpdateTalentManager);
            Lua.Events.AttachEvent("GLYPH_UPDATED", UpdateTalentManager);
            Lua.Events.AttachEvent("ACTIVE_Talent_GROUP_CHANGED", UpdateTalentManager);
            Lua.Events.AttachEvent("PLAYER_SPECIALIZATION_CHANGED", UpdateTalentManager);
            Lua.Events.AttachEvent("LEARNED_SPELL_IN_TAB", UpdateTalentManager);
        }

        public static WoWSpec CurrentSpec { get; private set; }

        public static List<Talent> Talents { get; private set; }

        public static HashSet<string> Glyphs { get; private set; }

        private static int[] GlyphId { get; set; }

        private static int[] TalentId { get; set; }

        private static readonly WaitTimer EventRebuildTimer = new WaitTimer(TimeSpan.FromSeconds(1));

        private static bool _rebuild;

        private static bool RebuildNeeded
        {
            get
            {
                return _rebuild;
            }
            set
            {
                _rebuild = value;
                EventRebuildTimer.Reset();
            }
        }

        public static bool IsSelected(int index)
        {
            int tier = (index - 1) / 3;
            if (tier.Between(0, 5))
                return TalentId[tier] == index;
            return false;
        }

        public static bool HasTalent(Enum.WarriorTalents tal)
        {
            return IsSelected((int)tal);
        }

        public static bool HasGlyph(string glyphName)
        {
            return Glyphs.Any() && Glyphs.Contains(glyphName);
        }

        private static void UpdateTalentManager(object sender, LuaEventArgs args)
        {
            var oldSpec = CurrentSpec;
            int[] oldTalent = TalentId;
            int[] oldGlyph = GlyphId;

            Logger.DiagLogPu("{0} Event Fired!", args.EventName);

            Update();

            if (args.EventName == "PLAYER_LEVEL_UP")
            {
                RebuildNeeded = true;
                Logger.DiagLogPu("FU TalentManager: Your character has leveled up! Now level {0}", args.Args[0]);
            }

            if (CurrentSpec != oldSpec)
            {
                RebuildNeeded = true;
                Logger.DiagLogPu("FU TalentManager: Your spec has been changed.");
            }

            int i;
            for (i = 0; i < 6; i++)
            {
                if (oldTalent[i] != TalentId[i])
                {
                    RebuildNeeded = true;
                    Logger.DiagLogPu("FU TalentManager: Your Talents have changed.");
                    break;
                }
            }

            for (i = 0; i < 6; i++)
            {
                if (oldGlyph[i] != GlyphId[i])
                {
                    RebuildNeeded = true;
                    Logger.DiagLogPu("FU TalentManager: Your glyphs have changed.");
                    break;
                }
            }
        }

        public static void Update()
        {
            using (StyxWoW.Memory.AcquireFrame())
            {
                CurrentSpec = StyxWoW.Me.Specialization;

                Talents.Clear();
                TalentId = new int[6];

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

                for (int i = 1; i <= 6; i++)
                {
                    List<string> glyphInfo = Lua.GetReturnValues(String.Format("return GetGlyphSocketInfo({0})", i));

                    if (glyphInfo != null && glyphInfo.Count >= 4 && glyphInfo[3] != "nil" &&
                        !string.IsNullOrEmpty(glyphInfo[3]))
                    {
                        GlyphId[i - 1] = int.Parse(glyphInfo[3]);
                        Glyphs.Add(WoWSpell.FromId(GlyphId[i - 1]).Name.Replace("Glyph of ", ""));
                    }
                }

                foreach (var glyph in Glyphs)
                {
                    Logger.DiagLogPu("Glyph of {0}", glyph);
                }

                foreach (var talent in Talents)
                {
                    Logger.DiagLogPu("{0} : {1}", talent.Index, talent.Selected);
                }

            }
        }

        public static bool Pulse()
        {
            if (EventRebuildTimer.IsFinished && RebuildNeeded)
            {
                RebuildNeeded = false;
                Logger.DiagLogPu("FU: Rebuilding behaviors due to changes detected - TalentManager.");

                /* Updating Talents and Glyphs */
                Update();

                /* Restarting the DamageTracker */
                DamageTracker.Stop();
                DamageTracker.Initialize();

                /* Refreshing Stats and Setting GCD Spell */
                Item.RefreshSecondaryStats();
                Spell.InitGcdSpell();

                /* Restarting Rotations */
                Root.Instance.PreCombatSelector();
                Root.Instance.CombatSelector();

                /* Done! */
                Logger.DiagLogPu("FU: Rebuilding behaviors completed - TalentManager.");
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