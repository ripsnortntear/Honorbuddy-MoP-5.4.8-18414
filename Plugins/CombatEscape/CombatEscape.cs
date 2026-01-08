#region Revision info
/*
 * $Author$
 * $Date$
 * $ID: $
 * $Revision$
 * $URL$
 * $LastChangedBy$
 * $ChangesMade: $
 */
#endregion

using System;
using System.Linq;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Windows.Media;

namespace CombatEscape
{
    public class CombatEscape : HBPlugin
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static string SvnRevision { get { return "$Rev$"; } }
        private static bool _initialized;
        private DateTime _throttleTime;
        private static DateTime _enteredCombat;
        private const int ThrottleDuration = 10000;
        private bool _inCombatPreviousPass;
        private static WoWUnit _hostileUnit;

        #region Default Overrides
        public override string Author { get { return "Millz"; } }
        public override string ButtonText { get { return "Settings"; } }
        public override string Name { get { return "Combat Escape"; } }
        public override bool WantButton { get { return false; } }
        public override Version Version { get { return new Version(0, 0, 1); } }
        #endregion Default Overrides

        public override void OnButtonPress()
        {

        }

        public override void Initialize()
        {
            if (!_initialized) // prevent init twice.
            {
                Log("Loaded Combat Escape [ {0}] ~ Millz", SvnRevision.Replace("$", ""));
                _initialized = true;
            }
        }

        public override void Dispose()
        {
            Log("Disposing Combat Escape [ {0}] ~ Millz", SvnRevision.Replace("$", ""));
            _initialized = false;
        }

        public override void Pulse()
        {
            // We want to check this frequently, off the throttle duration to drop the combat flag.
            if (_inCombatPreviousPass && !Me.IsActuallyInCombat)
            {
                _inCombatPreviousPass = false; // We've left combat.
            }

            // Let's throttle our calls to be nice to the rest of the bot.
            if (_initialized && _throttleTime.AddMilliseconds(ThrottleDuration) >= _throttleTime)
            {
                if (!_inCombatPreviousPass && Me.IsActuallyInCombat)
                {
                    _enteredCombat = DateTime.UtcNow;
                    _inCombatPreviousPass = true; // We've entered combat.
                }

                if (!Me.Mounted && !Me.IsOnTransport && Me.IsActuallyInCombat && _inCombatPreviousPass) 
                {
                    // We've been in combat for throttleDuration * 2 (i.e. 20s) at this point.
                    //  Make a call to the ObjectManager to see if we've become stuck in combat with no aggressive unit targetting us.

                    if (NeedToEscape())
                    {
                        Log("Looks like we've got stuck. Lets get back to work...");
                        UseBestAbility();
                    }
                    else
                    {
                        if (_hostileUnit != null)
                        {
                            Log("We've been in combat for a while, but unit [{0}] is targeting us.", _hostileUnit.Name);
                            if (!Me.GotTarget)
                            {
                                Log("I don't have a target. Targeting the unit that's trying to attack us.");
                                _hostileUnit.Target();
                            }
                        }
                    }

                    _throttleTime = DateTime.UtcNow;
                }
                
            }
        }

        public static bool NeedToEscape()
        {
            // Get any unit that we can attack, which is targeting me or my pet
            _hostileUnit = ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                .FirstOrDefault(u => u.Attackable && u.CanSelect && !u.IsFriendly && u.Distance <= 80 && u.IsTargetingMeOrPet);

            // If we're in combat, and there isn't a unit targeting us, it's bugged, hearth
            if (_hostileUnit == null)
            {
                return true;
            }

            // Something is targetting us, so doesn't look bugged...?
            // If we've been stuck for a while, we probably want to get the puk out of here.
            double combatDuration = (DateTime.UtcNow - _enteredCombat).TotalSeconds;
            Log("We entered combat roughly {0} seconds ago", combatDuration);

            if (combatDuration >= 300)
            {
                Log("Okay, its been over 5 minutes now. Let's make like hockey players and get the puk outta here.");
                return true;
            }


            return false;
        }

        private static void UseBestAbility()
        {

            if (Me.Race == WoWRace.NightElf)
            {
                if (!SpellOnCooldown("Shadowmeld")) { Cast("Shadowmeld"); }
            }

            if (Me.Class == WoWClass.Hunter)
            {
                if (!SpellOnCooldown("Feign Death")) { Cast("Feign Death"); }
            }

            if (Me.Class == WoWClass.Rogue)
            {
                if (!SpellOnCooldown("Vanish")) { Cast("Vanish"); }
            }

            if (Me.Class == WoWClass.Mage)
            {
                if (!SpellOnCooldown("Invisibility")) { Cast("Invisibility"); }
            }

            // If unable to use a race/class ability, default to using hearthstone.
            WoWItem hearthstone = ObjectManager.GetObjectsOfType<WoWItem>().FirstOrDefault(item => item.Entry == 6948);

            if (hearthstone == null)
            {
                Log("WARNING - Unable to find Hearthstone in bags");
            }

            if (hearthstone != null && hearthstone.Usable)
            {
                if (hearthstone.Cooldown <= 0)
                {
                    Log("Using Hearthstone");
                    hearthstone.Use();
                }
                else
                {
                    Log("Unable to use Hearthstone. Cooldown Remaining: {0}s", hearthstone.CooldownTimeLeft.TotalSeconds);
                }
            }

        }

        private static bool SpellOnCooldown(string spell)
        {
            SpellFindResults results;
            if (SpellManager.FindSpell(spell, out results))
            {
                return results.Override != null ? results.Override.Cooldown : results.Original.Cooldown;
            }

            return false;
        }

        private static void Cast(string spellName)
        {
            if (StyxWoW.Me.CurrentTarget != null && !Me.IsCasting && !Me.IsChanneling)
            {
                if (SpellManager.CanCast(spellName))
                {
                    Log("[Casting: {0}]", spellName);
                    var result = SpellManager.Cast(spellName);

                    if (!result)
                    {
                        Log("[Failed to Cast: {0}]", spellName);
                    }
                }
                {
                    if (!Me.IsCasting || !Me.IsChanneling)
                        Log("[CanCast Failed for: {0}]", spellName);
                }
            }
        }

        private static void Log(string logText, params object[] args)
        {
            if (logText == null) return;
            Logging.Write(LogLevel.Normal, Colors.Crimson, "[Combat Escape]: {0}", string.Format(logText, args));
        }
    }
}
