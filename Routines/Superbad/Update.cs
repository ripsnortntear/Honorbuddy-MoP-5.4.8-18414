#region

using System;
using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.Common;
using Styx.WoWInternals;

#endregion

namespace Superbad
{
    internal static class TalentManager
    {
        //Talents
        public static bool CenarionWard;
        public static bool DisorientingRoar;
        public static bool DisplacerBeast;
        public static bool DreamOfCenarius;
        public static bool FaerieSwarm;
        public static bool FelineSwiftness;
        public static bool ForceOfNature;
        public static bool HeartOfTheWild;
        public static bool Incarnation;
        public static bool MassEntanglement;
        public static bool MightyBash;
        public static bool NaturesSwiftness;
        public static bool NaturesVigil;
        public static bool Renewal;
        public static bool SoulOfTheForest;
        public static bool Typhoon;
        public static bool UrsolsVortex;
        public static bool WildCharge;

        //Glyphs
        public static bool Savagery;
        public static bool FrenziedRegeneration;
        public static bool Shred;

        static TalentManager()
        {
            Talents = new List<Talent>();
            Glyphs = new HashSet<string>();
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", UpdateTalentManager);
            Lua.Events.AttachEvent("GLYPH_UPDATED", UpdateTalentManager);
            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", UpdateTalentManager);
        }

        public static WoWSpec CurrentSpec { get; private set; }

        public static List<Talent> Talents { get; private set; }

        public static HashSet<string> Glyphs { get; private set; }

        public static bool IsSelected(int index)
        {
            return Talents.FirstOrDefault(t => t.Index == index).Selected;
        }

        /// <summary>
        ///     Checks if we have a glyph or not
        /// </summary>
        /// <param name="glyphName"> Name of the glyph without "Glyph of". i.e. HasGlyph("Aquatic Form") </param>
        /// <returns> </returns>
        public static bool HasGlyph(string glyphName)
        {
            return Glyphs.Count > 0 && Glyphs.Contains(glyphName);
        }

        private static void UpdateTalentManager(object sender, LuaEventArgs args)
        {
            WoWSpec oldSpec = CurrentSpec;

            Update();

            if (CurrentSpec != oldSpec)
            {
                Logging.Write(@"Your spec has been changed. Rebuilding behaviors");
            }
        }

        public static void Update()
        {
            // Keep the frame stuck so we can do a bunch of injecting at once.
            using (StyxWoW.Memory.AcquireFrame())
            {
                CurrentSpec = StyxWoW.Me.Specialization;
                Logging.Write(@"TalentManager - looks like a {0}", CurrentSpec);

                Talents.Clear();

                // Always 18 talents. 6 rows of 3 talents.
                for (int index = 1; index <= 6*3; index++)
                {
                    var selected =
                        Lua.GetReturnVal<bool>(
                            string.Format(
                                "local t= select(5,GetTalentInfo({0})) if t == true then return 1 end return nil", index),
                            0);
                    var t = new Talent {Index = index, Selected = selected};
                    Talents.Add(t);
                }
                PrintUsedTalents();
                SetUsedTalents();
                Glyphs.Clear();
                GlyphReset();

                // 6 glyphs all the time. Plain and simple!
                for (int i = 1; i <= 6; i++)
                {
                    List<string> glyphInfo = Lua.GetReturnValues(String.Format("return GetGlyphSocketInfo({0})", i));

                    // add check for 4 members before access because empty sockets weren't returning 'nil' as documented
                    if (glyphInfo != null && glyphInfo.Count >= 4 && glyphInfo[3] != "nil" &&
                        !string.IsNullOrEmpty(glyphInfo[3]))
                    {
                        string glyph = WoWSpell.FromId(int.Parse(glyphInfo[3])).Name.Replace("Glyph of ", "");
                        Glyphs.Add(glyph);
                        Logging.Write(@"Glyph of " + glyph);

                        if (glyph == "Savagery")
                            Savagery = true;

                        if (glyph == "Shred")
                            Shred = true;

                        if (glyph == "Frenzied Regeneration")
                            FrenziedRegeneration = true;
                    }
                }
                Superbad.SetLearnedSpells();
            }
        }

        private static void GlyphReset()
        {
            Savagery = false;
            FrenziedRegeneration = false;
            Shred = false;
        }

        private static void SetUsedTalents()
        {
            CenarionWard = IsSelected((int) talent.DruidTalents.CenarionWard);
            DisorientingRoar = IsSelected((int) talent.DruidTalents.DisorientingRoar);
            DisplacerBeast = IsSelected((int) talent.DruidTalents.DisplacerBeast);
            DreamOfCenarius = IsSelected((int) talent.DruidTalents.DreamOfCenarius);
            FaerieSwarm = IsSelected((int) talent.DruidTalents.FaerieSwarm);
            FelineSwiftness = IsSelected((int) talent.DruidTalents.FelineSwiftness);
            ForceOfNature = IsSelected((int) talent.DruidTalents.ForceOfNature);
            HeartOfTheWild = IsSelected((int) talent.DruidTalents.HeartOfTheWild);
            Incarnation = IsSelected((int) talent.DruidTalents.Incarnation);
            MassEntanglement = IsSelected((int) talent.DruidTalents.MassEntanglement);
            MightyBash = IsSelected((int) talent.DruidTalents.MightyBash);
            NaturesSwiftness = IsSelected((int) talent.DruidTalents.NaturesSwiftness);
            NaturesVigil = IsSelected((int) talent.DruidTalents.NaturesVigil);
            Renewal = IsSelected((int) talent.DruidTalents.Renewal);
            SoulOfTheForest = IsSelected((int) talent.DruidTalents.SoulOfTheForest);
            Typhoon = IsSelected((int) talent.DruidTalents.Typhoon);
            UrsolsVortex = IsSelected((int) talent.DruidTalents.UrsolsVortex);
            WildCharge = IsSelected((int) talent.DruidTalents.WildCharge);
        }

        private static void PrintUsedTalents()
        {
            if (IsSelected((int) talent.DruidTalents.CenarionWard))
                Logging.Write("Talent: " + "Cenarion Ward");
            if (IsSelected((int) talent.DruidTalents.DisorientingRoar))
                Logging.Write("Talent: " + "Disorienting Roar");
            if (IsSelected((int) talent.DruidTalents.DisplacerBeast))
                Logging.Write("Talent: " + "Displacer Beast");
            if (IsSelected((int) talent.DruidTalents.DreamOfCenarius))
                Logging.Write("Talent: " + "Dream of Cenarius");
            if (IsSelected((int) talent.DruidTalents.FaerieSwarm))
                Logging.Write("Talent: " + "Faerie Swarm");
            if (IsSelected((int) talent.DruidTalents.FelineSwiftness))
                Logging.Write("Talent: " + "Feline Swiftness");
            if (IsSelected((int) talent.DruidTalents.ForceOfNature))
                Logging.Write("Talent: " + "Force of Nature");
            if (IsSelected((int) talent.DruidTalents.HeartOfTheWild))
                Logging.Write("Talent: " + "Heart of the Wild");
            if (IsSelected((int) talent.DruidTalents.Incarnation))
                Logging.Write("Talent: " + "Incarnation");
            if (IsSelected((int) talent.DruidTalents.MassEntanglement))
                Logging.Write("Talent: " + "Mass Entanglement");
            if (IsSelected((int) talent.DruidTalents.MightyBash))
                Logging.Write("Talent: " + "Mighty Bash");
            if (IsSelected((int) talent.DruidTalents.NaturesSwiftness))
                Logging.Write("Talent: " + "Natures Swiftness");
            if (IsSelected((int) talent.DruidTalents.NaturesVigil))
                Logging.Write("Talent: " + "Natures Vigil");
            if (IsSelected((int) talent.DruidTalents.Renewal))
                Logging.Write("Talent: " + "Renewal");
            if (IsSelected((int) talent.DruidTalents.SoulOfTheForest))
                Logging.Write("Talent: " + "Soul of the Forest");
            if (IsSelected((int) talent.DruidTalents.Typhoon))
                Logging.Write("Talent: " + "Typhoon");
            if (IsSelected((int) talent.DruidTalents.UrsolsVortex))
                Logging.Write("Talent: " + "Ursols Vortex");
            if (IsSelected((int) talent.DruidTalents.WildCharge))
                Logging.Write("Talent: " + "Wild Charge");
        }

        #region Nested type: Talent

        public struct Talent
        {
            public int Index;
            public bool Selected;
        }

        #endregion
    }
}