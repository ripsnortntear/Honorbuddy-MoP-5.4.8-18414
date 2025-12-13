using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RichieHolyPriestPvP
{
    public class UserCastedSpell
    {
        public SpellIDs Id { get; private set; }
        public ulong Target { get; private set; }

        public UserCastedSpell(SpellIDs id, ulong target)
        {
            this.Id = id;
            this.Target = target;
        }
    }

    public static class SpecializedCombatLog
    {
        public static UserCastedSpell UserSpell { get; set; }

        static SpecializedCombatLog()
        {
            CombatLog.FilterCriteria = "return (args[8] == UnitGUID('player') and args[4] ~= UnitGUID('player') and args[2] == 'SPELL_CAST_SUCCESS'"
                                                        + " and (args[12] == 106839 or" //Skull Bash
                                                             + " args[12] == 80964 or"  //Skull Bash
                                                             + " args[12] == 115781 or" //Optical Blast
                                                             + " args[12] == 116705 or" //Spear Hand Strike
                                                             + " args[12] == 1766 or"   //Kick
                                                             + " args[12] == 19647 or"  //Spell Lock
                                                             + " args[12] == 2139 or"   //Counterspell
                                                             + " args[12] == 47528 or"  //Mind Freeze
                                                             + " args[12] == 57994 or"  //Wind Shear
                                                             + " args[12] == 6552 or"   //Pummel
                                                             + " args[12] == 147362 or" //Counter Shot
                                                             + " args[12] == 96231))"   //Rebuke
                                                        + " or ((args[2] == 'SPELL_CAST_FAILED' or args[2] == 'SPELL_CAST_SUCCESS') and args[4] == UnitGUID('player')) "
                                                        + " or (args[4] ~= UnitGUID('player') and args[12] == 102060)";//Disrupting Shout

            CombatLog.OnSpellCastSuccess += OnSpellCastSuccess;
            CombatLog.OnSpellCastFailed += OnSpellCastFailed;
            CombatLog.OnUnhandledEvent += OnUnhandledEvent;
        }

        public static bool Attach()
        {
            return CombatLog.Attach();
        }

        public static bool Detach()
        {
            return CombatLog.Detach();
        }

        #region Private Eventhandlers

        private static void OnSpellCastSuccess(ulong casterGUID, SpellIDs id, LuaEventArgs args)
        {
            if (!HolySettings.Instance.StopCastOnInterrupt || casterGUID == UnitManager.Me.Guid || UnitManager.Me.HasSpiritOfRedemption())
                return;

            // Disrupting Shout has no target
            if (id == SpellIDs.DisruptingShout)
            {
                WoWUnit caster = UnitManager.FindByGUID(casterGUID);
                if (caster == null || caster.Distance2D > 10)
                {
                    Logging.Write(Colors.Gold, DateTime.Now.ToString("ss:fff ") + "- Ignoring Disrupting Shout: Out of Range");
                    return;
                }
            }

            SpellManager.StopCasting();
            Logging.Write(Colors.Gold, DateTime.Now.ToString("ss:fff ") + "- [CombatLog] Stop casting because of an incoming " + args.Args[12].ToString());

            SpellFindResults results;
            if (SpellManager.FindSpell((int)SpellIDs.InnerFire, out results))
            {
                double cooldownLeft = (results.Override ?? results.Original).CooldownTimeLeft.TotalMilliseconds;

                if (cooldownLeft > 1500)
                {
                    Logging.Write(Colors.Gold, DateTime.Now.ToString("ss:fff ") + "- [CombatLog] Couldn't stop casting in time.");
                    HolyCoolDown.UpdateGCD();

                    Statistics.IncInterruptFailed();
                }
                else
                {
                    Logging.Write(Colors.Gold, DateTime.Now.ToString("ss:fff ") + "- [CombatLog] Cast successfully stopped before the interrupt.");
                    Statistics.IncInterruptSuccesses();
                }
            }
        }

        private static void OnSpellCastFailed(SpellIDs id, LuaEventArgs args)
        {
            Logging.WriteDiagnostic("Spell Cast(" + args.Args[12] + ") failed! Cause: " + args.Args[14]);

            if (Main.IsProVersion && HolySettings.Instance.CastFailedUserInitiatedSpell)
            {
                string failedType = args.Args[14].ToString();
                if (failedType.Equals("Another action is in progress", StringComparison.OrdinalIgnoreCase) ||
                    failedType.Equals("Not yet recovered", StringComparison.OrdinalIgnoreCase))
                {
                    string hexGUID = args.Args[7].ToString();
                    ulong guid = UnitManager.Me.Guid;
                    if (!string.IsNullOrEmpty(hexGUID))
                    {
                        // .net can't parse hex strings that start with "0x".... :/
                        if (hexGUID.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            hexGUID = hexGUID.Substring(2);
                        guid = string.IsNullOrEmpty(hexGUID) ? 0 : ulong.Parse(hexGUID, NumberStyles.HexNumber);
                    }

                    UserSpell = new UserCastedSpell(id, guid);
                }
            }
        }

        private static void OnUnhandledEvent(LuaEventArgs args)
        {
            StringBuilder sb = new StringBuilder(args.Args.Length * 5);
            sb.Append(DateTime.Now.ToString("ss:fff"));
            for (int i = 0; i < args.Args.Length; i++)
                sb.Append(" [" + i + "] = " + args.Args[i]);

            Logging.WriteDiagnostic(sb.ToString());
        }

        #endregion
    }
}