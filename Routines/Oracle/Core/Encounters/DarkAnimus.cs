using JetBrains.Annotations;
using Oracle.Core.CombatLog;
using Oracle.Shared.Logging;
using System;

namespace Oracle.Core.Encounters
{
    [UsedImplicitly]
    internal class DarkAnimus : BossEncounter
    {
        private const int InterruptingJoltID = 138763;

        private DarkAnimus()
        {
            IsInterruptingJolt = false;
        }

        public override int BossId
        {
            get { return 69427; }
        }

        public override string Name
        {
            get { return "Dark Animus"; }
        }

        public static bool IsInterruptingJolt { get; set; }

        public override void Initialize()
        {
            CombatLogHandler.Register("SPELL_CAST_START", HandleCombatLogEvent);
            CombatLogHandler.Register("SPELL_CAST_FAILED", HandleCombatLogEvent);
            CombatLogHandler.Register("SPELL_CAST_SUCCESS", HandleCombatLogEvent);
        }

        public override void Shutdown()
        {
            CombatLogHandler.Remove("SPELL_CAST_START");
            CombatLogHandler.Remove("SPELL_CAST_FAILED");
            CombatLogHandler.Remove("SPELL_CAST_SUCCESS");
        }

        public override void HandleCombatLogEvent(CombatLogEventArgs args)
        {
            if (CombatLogHandler.CurrentBossEncounter == null) return;
            if (CombatLogHandler.CurrentBossEncounter.BossId != BossId) return;

            switch (args.Event)
            {
                case "SPELL_CAST_START":
                    if (args.SpellId == InterruptingJoltID)
                        InterruptingJolt_Start(args);
                    break;

                case "SPELL_CAST_FAILED":
                case "SPELL_CAST_SUCCESS":
                    if (args.SpellId == InterruptingJoltID)
                        InterruptingJolt_Finish(args);
                    break;
            }
        }

        private void InterruptingJolt_Finish(CombatLogEventArgs args)
        {
            Logger.Output(" [BossEncounter] : Interupting Jolt Finished {0}", DateTime.Now);
            IsInterruptingJolt = false;
        }

        private void InterruptingJolt_Start(CombatLogEventArgs args)
        {
            Logger.Output(" [BossEncounter] : Interupting Jolt Start {0}", DateTime.Now);
            IsInterruptingJolt = true;
        }
    }
}