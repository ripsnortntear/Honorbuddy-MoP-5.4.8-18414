using System.Collections.Generic;
using System.Linq;
using Styx;

using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PureRotation.Classes.Shaman
{
    // yes this is from singular...no I do not want to write totem support all over again..that is all..all credit to the singular devs.
    internal static class Totems
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public static int ToSpellId(this WoWTotem totem)
        {
            return (int)(((long)totem) & ((1 << 32) - 1));
        }

        public static WoWTotemType ToType(this WoWTotem totem)
        {
            return (WoWTotemType)((long)totem >> 32);
        }

        private static readonly Dictionary<WoWTotemType, WoWTotem> LastSetTotems = new Dictionary<WoWTotemType, WoWTotem>();

        #region Helper shit

        public static bool NeedToRecallTotems
        {
            get
            {
                return TotemsInRange == 0 && StyxWoW.Me.Totems.Count(t => t.Unit != null) != 0;
            }
        }

        public static bool TotemIsKnown(WoWTotem totem)
        {
            return SpellManager.HasSpell(totem.ToSpellId());
        }

        #region Totem Existance

        public static bool Exist(WoWTotem ti)
        {
            return ti != WoWTotem.None
                && ti != WoWTotem.DummyAir
                && ti != WoWTotem.DummyEarth
                && ti != WoWTotem.DummyFire
                && ti != WoWTotem.DummyWater;
        }

        public static bool Exist(WoWTotemInfo ti)
        {
            return Exist(ti.WoWTotem);
        }

        public static bool Exist(WoWTotemType type)
        {
            WoWTotem wt = GetTotem(type).WoWTotem;
            return Exist(wt);
        }

        public static bool Exist(params WoWTotem[] wt)
        {
            return wt.Any(t => Exist(t));
        }

        public static bool ExistInRange(WoWPoint pt, WoWTotem tt)
        {
            if (!Exist(tt))
                return false;

            WoWTotemInfo ti = GetTotem(tt);
            return ti.Unit != null && ti.Unit.Location.Distance(pt) < GetTotemRange(tt);
        }

        public static bool ExistInRange(WoWPoint pt, params WoWTotem[] awt)
        {
            return awt.Any(t => ExistInRange(pt, t));
        }

        public static bool ExistInRange(WoWPoint pt, WoWTotemType type)
        {
            WoWTotemInfo ti = GetTotem(type);
            return Exist(ti) && ti.Unit != null && ti.Unit.Location.Distance(pt) < GetTotemRange(ti.WoWTotem);
        }

        #endregion Totem Existance

        /// <summary>
        /// gets reference to array element in Me.Totems[] corresponding to WoWTotemType of wt.  Return is always non-null and does not indicate totem existance
        /// </summary>
        /// <param name="wt">WoWTotem of slot to reference</param>
        /// <returns>WoWTotemInfo reference</returns>
        public static WoWTotemInfo GetTotem(WoWTotem wt)
        {
            return GetTotem(wt.ToType());
        }

        /// <summary>
        /// gets reference to array element in Me.Totems[] corresponding to type.  Return is always non-null and does not indicate totem existance
        /// </summary>
        /// <param name="type">WoWTotemType of slot to reference</param>
        /// <returns>WoWTotemInfo reference</returns>
        public static WoWTotemInfo GetTotem(WoWTotemType type)
        {
            return Me.Totems[(int)type - 1];
        }

        public static int TotemsInRange { get { return TotemsInRangeOf(StyxWoW.Me); } }

        public static int TotemsInRangeOf(WoWUnit unit)
        {
            return StyxWoW.Me.Totems.Where(t => t.Unit != null).Count(t => unit.Location.Distance(t.Unit.Location) < GetTotemRange(t.WoWTotem));
        }

        /// <summary>
        ///   Finds the max range of a specific totem, where you'll still receive the buff.
        /// </summary>
        /// <remarks>
        ///   Created 3/26/2011.
        /// </remarks>
        /// <param name = "totem">The totem.</param>
        /// <returns>The calculated totem range.</returns>
        public static float GetTotemRange(WoWTotem totem)
        {
            switch (totem)
            {
                case WoWTotem.HealingStream:

                case WoWTotem.Tremor:
                    return 30f;

                case WoWTotem.Searing:
                    return 20f;

                case WoWTotem.Earthbind:
                    return 10f;

                case WoWTotem.Grounding:

                case WoWTotem.Magma:
                    return 10f;

                case WoWTotem.EarthElemental:

                case WoWTotem.FireElemental:

                    // Not really sure about these 3.
                    return 20f;

                case WoWTotem.ManaTide:

                    // Again... not sure :S
                    return 40f;

                case WoWTotem.Earthgrab:
                    return 10f;

                case WoWTotem.StoneBulwark:

                    // No idea, unlike former glyphed stoneclaw it has a 5 sec pluse shield component so range is more important
                    return 40f;

                case WoWTotem.HealingTide:
                    return 40f;

                case WoWTotem.Capacitor:
                    return 8f;

                case WoWTotem.Stormlash:
                    return 30f;

                case WoWTotem.Windwalk:
                    return 40f;

                case WoWTotem.SpiritLink:
                    return 10f;
            }

            return 0f;
        }

        #endregion Helper shit
    }
}