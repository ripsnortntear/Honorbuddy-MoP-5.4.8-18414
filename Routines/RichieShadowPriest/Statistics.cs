using Styx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieShadowPriestPvP
{
    public static class Statistics
    {
        #region Privates

        private static Dictionary<int, uint> SpellCasts = new Dictionary<int, uint>(Enum.GetValues(typeof(SpellIDs)).Length);
        private static Dictionary<int, uint> SpellFails = new Dictionary<int, uint>(Enum.GetValues(typeof(SpellIDs)).Length);

        private static uint InterruptSuccess;
        private static uint InterruptFail;

        #endregion

        #region Casts

        public static void SpellCast(SpellIDs spell)
        {
            if (!SPSettings.Instance.CollectStatistics)
                return;

            uint currentCount;
            if (!SpellCasts.TryGetValue((int)spell, out currentCount))
                SpellCasts.Add((int)spell, 1);
            else
                SpellCasts[(int)spell] = ++currentCount;
        }

        public static void SpellFail(SpellIDs spell)
        {
            if (!SPSettings.Instance.CollectStatistics)
                return;

            uint currentCount;
            if (!SpellFails.TryGetValue((int)spell, out currentCount))
                SpellFails.Add((int)spell, 1);
            else
                SpellFails[(int)spell] = ++currentCount;
        }

        #endregion

        #region Interrupts

        public static void IncInterruptSuccesses()
        {
            if (!SPSettings.Instance.CollectStatistics)
                return;

            InterruptSuccess++;
        }

        public static void IncInterruptFailed()
        {
            if (!SPSettings.Instance.CollectStatistics)
                return;

            InterruptSuccess++;
        }

        #endregion

        #region Misc

        public static void Clear()
        {
            SpellCasts.Clear();
            SpellFails.Clear();

            InterruptSuccess = 0;
            InterruptFail = 0;
        }

        public static void Print()
        {
            if (!SPSettings.Instance.CollectStatistics)
                return;

            if (SpellCasts.Count == 0 && SpellFails.Count == 0 && InterruptSuccess == 0 && InterruptFail == 0)
                return;

            Logging.Write("------------Statistics------------");

            if (SpellCasts.Count != 0 && SpellFails.Count != 0)
            {
                if (SpellCasts.Count > 0)
                {
                    Logging.Write("-----------Spell Success----------");
                    foreach (var kvp in SpellCasts.OrderByDescending(stat => stat.Value))
                        Logging.Write(string.Format("   {0, -30}: {1, 6}", Spells.Get((SpellIDs)kvp.Key), kvp.Value));
                }

                if (SpellFails.Count > 0)
                {
                    Logging.Write("-----------Spell Fails-----------");
                    foreach (var kvp in SpellFails.OrderByDescending(stat => stat.Value))
                        Logging.Write(string.Format("   {0, -30}: {1, 6}", Spells.Get((SpellIDs)kvp.Key), kvp.Value));
                }
            }

            if (InterruptSuccess > 0 || InterruptFail > 0)
            {
                Logging.Write("-----------Interrupts-----------");
                Logging.Write(string.Format("     Successes: {0, 3} Fails: {1, 3}", InterruptSuccess, InterruptFail));
            }

            Logging.Write("--------End Of Statistics---------");
        }

        #endregion
    }
}