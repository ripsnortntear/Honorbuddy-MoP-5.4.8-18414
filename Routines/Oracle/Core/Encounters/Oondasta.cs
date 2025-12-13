using JetBrains.Annotations;
using Oracle.Core.CombatLog;
using Oracle.Shared.Logging;
using Styx.WoWInternals;
using System;

namespace Oracle.Core.Encounters
{
    [UsedImplicitly]
    internal class Oondasta : BossEncounter
    {
        private const int PiercingRoarID = 137457;

        private Oondasta()
        {
            IsPiercingRoar = false;
        }

        public override int BossId
        {
            get { return 69161; }
        }

        public override string Name
        {
            get { return "Oondasta"; }
        }

        public static bool IsPiercingRoar { get; set; }

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
                    if (args.SpellId == PiercingRoarID)
                        PiercingRoar_Start(args);
                    break;

                case "SPELL_CAST_FAILED":
                case "SPELL_CAST_SUCCESS":
                    if (args.SpellId == PiercingRoarID)
                        PiercingRoar_Finish(args);
                    break;
            }
        }

        private void PiercingRoar_Finish(CombatLogEventArgs args)
        {
            Logger.Output(" [BossEncounter] : Piercing Roar Finished {0}", DateTime.Now);
            IsPiercingRoar = false;
        }

        private void PiercingRoar_Start(CombatLogEventArgs args)
        {
            Logger.Output(" [BossEncounter] : Piercing Roar Start {0}", DateTime.Now);
            IsPiercingRoar = true;
        }
    }
}