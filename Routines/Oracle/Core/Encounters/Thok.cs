using JetBrains.Annotations;
using Oracle.Core.CombatLog;
using Oracle.Shared.Logging;
using System;

namespace Oracle.Core.Encounters
{
    [UsedImplicitly]
    internal class Thock : BossEncounter
    {
        private const int DeafeningScreechID = 143343;

        private Thock()
        {
            IsDeafeningScreech = false;
        }

        public override int BossId
        {
            get { return 71529; }
        }

        public override string Name
        {
            get { return "Thock the Bloodthirsty"; }
        }

        public static bool IsDeafeningScreech { get; set; }

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
                    if (args.SpellId == DeafeningScreechID)
                        DeafeningScreech_Start(args);
                    break;
                
                case "SPELL_CAST_FAILED":
                case "SPELL_CAST_SUCCESS":
                    if (args.SpellId == DeafeningScreechID)
                        DeafeningScreech_Finish(args);
                    break;
            }
        }

        private void DeafeningScreech_Finish(CombatLogEventArgs args)
        {
            Logger.Output(" [BossEncounter] : Deafening Screech Finished {0}", DateTime.Now);
            IsDeafeningScreech = false;
        }

        private void DeafeningScreech_Start(CombatLogEventArgs args)
        {
            Logger.Output(" [BossEncounter] : Deafening Screech Start {0}", DateTime.Now);
            IsDeafeningScreech = true;
        }
    }
}