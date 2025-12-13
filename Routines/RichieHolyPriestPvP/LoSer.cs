using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieHolyPriestPvP
{
    public static class LoSer
    {
        // In Millisecs
        private const int FreshTime = 500;

        private sealed class Result
        {
            private bool resultValue;
            public bool Value { 
                get { return resultValue; } 
                set { resultValue = value; LastCheck = DateTime.Now; } 
            }

            public bool IsFresh { get { return LastCheck.AddMilliseconds(FreshTime) > DateTime.Now; } }

            public DateTime LastCheck { get; set; }

            public Result(bool val)
            {
                this.Value = val;
            }
        }

        private static Dictionary<ulong, Result> LineOfSight = new Dictionary<ulong, Result>(40);
        private static Dictionary<ulong, Result> LineOfSpellSight = new Dictionary<ulong, Result>(40);

        public static bool InLineOfSight(this WoWUnit unit)
        {
            if (!unit.IsValidUnit())
                return false;

            if (unit == Main.Me)
                return true;

            Result result;
            if (LineOfSight.TryGetValue(unit.Guid, out result))
            {
                if (result.IsFresh)
                    return result.Value;
                else
                    result.Value = unit.InLineOfSight;
            }
            else
                LineOfSight.Add(unit.Guid, result = new Result(unit.InLineOfSight));

            return result.Value;
        }

        public static bool InLineOfSpellSight(this WoWUnit unit)
        {
            if (!unit.IsValidUnit())
                return false;

            if (unit == Main.Me)
                return true;

            Result result;
            if (LineOfSpellSight.TryGetValue(unit.Guid, out result))
            {
                if (result.IsFresh)
                    return result.Value;
                else
                    result.Value = unit.InLineOfSpellSight;
            }
            else
                LineOfSpellSight.Add(unit.Guid, result = new Result(unit.InLineOfSpellSight));

            return result.Value;
        }

        /// <summary>
        /// Call every zone swap
        /// </summary>
        public static void Clear()
        {
            LineOfSight.Clear();
            LineOfSpellSight.Clear();
        }
    }
}