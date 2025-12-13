using JetBrains.Annotations;
using Oracle.Core.CombatLog;
using Oracle.UI.Settings;
using Styx.WoWInternals.WoWObjects;
using System;

namespace Oracle.Core.Encounters
{
    [UsedImplicitly]
    internal class Malkorok : BossEncounter
    {
        /*
         An ancient miasma fills the room, absorbing all healing received and creating a shield that absorbs damage equal to the amount of healing absorbed, up to a cap of 100% of max health. In addition, the miasma inflicts 30000 Shadow damage every 2 seconds.
         */

        private Malkorok()
        {
        }

        private static OracleSettings Setting { get { return OracleSettings.Instance; } }

        private static bool UsePredictedHealth { get { return Setting.UsePredictedHealth; } }

        private static Random Rand = new Random();

        private const int StrongAncientBarrier = 142865, //green = 100%
                           AncientBarrier = 142864, //yellow >= 50%
                           WeakAncientBarrier = 142863; // red < 50%

        public override int BossId
        {
            get { return 71454; }
        }

        public override string Name
        {
            get { return "Malkorok"; }
        }

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
            OracleSettings.Instance.MalkorokEncounter = false;
        }

        public override void HandleCombatLogEvent(CombatLogEventArgs args)
        {
            if (CombatLogHandler.CurrentBossEncounter == null) return;
            if (CombatLogHandler.CurrentBossEncounter.BossId != BossId) return;
        }

        public static double GetAncientBarrierHealth(WoWUnit unit)
        {
            var result = (UsePredictedHealth && unit.GetHPCheckType() != HPCheck.Tank ? unit.GetPredictedHealthPercent() : unit.HealthPercent);

            switch (GetAncientBarrier(unit))
            {
                case StrongAncientBarrier:
                    result = 100;
                    break;

                case AncientBarrier:
                    result = Rand.Next(65, 85);
                    break;

                case WeakAncientBarrier:
                    result = Rand.Next(20, 50);
                    break;

            }

            return result;
        }

        private static int GetAncientBarrier(WoWUnit unit)
        {
            if (unit.HasAura(StrongAncientBarrier))
            {
                return StrongAncientBarrier;
            }

            if (unit.HasAura(AncientBarrier))
            {
                return AncientBarrier;
            }

            if (unit.HasAura(WeakAncientBarrier))
            {
                return WeakAncientBarrier;
            }

            return 0;
        }
    }
}