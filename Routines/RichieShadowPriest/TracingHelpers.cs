#define MEASURE_PERFORMANCE

using JetBrains.Annotations;
using Styx;
using Styx.TreeSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichieShadowPriestPvP
{
    public class TracePrioritySelector : PrioritySelector
    {
        private string Name;

        public TracePrioritySelector(string name, params Composite[] children)
            : base(children)
        {
            Name = name;
        }

        public override RunStatus Tick(object context)
        {
#if MEASURE_PERFORMANCE
            using (StyxWoW.Memory.AcquireFrame())
                using (var perf = PerfLogger.GetHelper(Name))
                    return base.Tick(context);
#else
            using (StyxWoW.Memory.AcquireFrame())
                return base.Tick(context);
#endif
        }

    }

    public class TraceAction : Styx.TreeSharp.Action
    {
        private string Name;

        public TraceAction(string name, ActionDelegate children)
            : base(children)
        {
            Name = name;
        }

        public override RunStatus Tick(object context)
        {
#if MEASURE_PERFORMANCE
            using (StyxWoW.Memory.AcquireFrame())
                using (var perf = PerfLogger.GetHelper(Name))
                    return base.Tick(context);
#else
            using (StyxWoW.Memory.AcquireFrame())
                return base.Tick(context);
#endif
        }

    }

    public class TraceSequence : Sequence
    {
        private string Name;

        public TraceSequence(string name, params Composite[] children)
            : base(children)
        {
            Name = name;
        }

        public override RunStatus Tick(object context)
        {
#if MEASURE_PERFORMANCE
            using (StyxWoW.Memory.AcquireFrame())
                using (var perf = PerfLogger.GetHelper(Name))
                    return base.Tick(context);
#else
            using (StyxWoW.Memory.AcquireFrame())
                return base.Tick(context);
#endif
        }

    }

    public class TraceDecorator : Decorator
    {
        private string Name;

        public TraceDecorator(string name, CanRunDecoratorDelegate func, Composite children)
            : base(func, children)
        {
            Name = name;
        }

        protected override bool CanRun(object context)
        {
#if MEASURE_PERFORMANCE
            using (StyxWoW.Memory.AcquireFrame())
                using (var perf = PerfLogger.GetHelper(Name + "_CanRun"))
                    return base.CanRun(context);
#else
            using (StyxWoW.Memory.AcquireFrame())
                return base.CanRun(context);
#endif
        }

        public override RunStatus Tick(object context)
        {
#if MEASURE_PERFORMANCE
            using (StyxWoW.Memory.AcquireFrame())
                using (var perf = PerfLogger.GetHelper(Name))
                    return base.Tick(context);
#else
            using (StyxWoW.Memory.AcquireFrame())
                return base.Tick(context);
#endif
        }

    }

    [UsedImplicitly]
    public class LockSelector : PrioritySelector
    {
        public LockSelector(params Composite[] children)
            : base(children)
        {
        }

        public override RunStatus Tick(object context)
        {
            using (StyxWoW.Memory.AcquireFrame())
                return base.Tick(context);
        }
    }
}