using System.Diagnostics;
using Styx.TreeSharp;

namespace FuryUnleashed.Core.Utilities
{
    public class TreeSharp
    {
        private static readonly Stopwatch TreePerformanceTimer = new Stopwatch();
        private static readonly Stopwatch CompositePerformanceTimer = new Stopwatch();

        /* Usage: TreeSharp.Tree(true) within a composite. */
        internal static Composite Tree(bool enable)
        {
            return new Action(ret =>
            {
                if (!enable)
                {
                    return RunStatus.Failure;
                }

                if (TreePerformanceTimer.ElapsedMilliseconds > 0)
                {
                    Logger.CombatLogWh("[TreePerformance] Elapsed Time to traverse Tree: {0} ms", TreePerformanceTimer.ElapsedMilliseconds);
                    TreePerformanceTimer.Stop();
                    TreePerformanceTimer.Reset();
                }
                TreePerformanceTimer.Start();

                return RunStatus.Failure;
            });
        }

        /* Usage: TreeSharp.Composite(Composite, "SomeComposite") within a composite. */
        internal static Composite Composite(Composite child, string name = "SomeComposite")
        {
            return new Sequence(
                new Action(delegate
                {
                    CompositePerformanceTimer.Reset();
                    CompositePerformanceTimer.Start();
                    return RunStatus.Success;
                }),
                child,
                new Action(delegate
                {
                    CompositePerformanceTimer.Stop();
                    Logger.CombatLogWh("[CompositePerformance] {0} took {1} ms", name, CompositePerformanceTimer.ElapsedMilliseconds);
                    return RunStatus.Success;
                }));
        }
    }
}
