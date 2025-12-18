using System;
using System.Globalization;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Frames;
using Styx.CommonBot.POI;
using Styx.Pathing;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Templar.GUI.Tabs;

namespace Templar.Helpers
{
    internal class TaskManager
    {
        public static void HandleErrorMessage(object sender, LuaEventArgs args)
        {
            var errorMessage = args.Args[0].ToString();
            var errLootDidntKill = Lua.GetReturnVal<string>("return ERR_LOOT_DIDNT_KILL", 0);
            var errLootGone = Lua.GetReturnVal<string>("return ERR_LOOT_GONE", 0);
            var spellFailedCantBeDisenchanted = Lua.GetReturnVal<string>("return SPELL_FAILED_CANT_BE_DISENCHANTED", 0);
            var errItemLocked = Lua.GetReturnVal<string>("return ERR_ITEM_LOCKED", 0);
            var spellFailedLowCastLevel = Lua.GetReturnVal<string>("return SPELL_FAILED_LOW_CASTLEVEL", 0);

            if (errorMessage.Equals(errLootDidntKill))
            {
                if (Variables.LootMob != null)
                {
                    CustomLog.Normal("Don't have permission to loot mob, blacklisting.");
                    CustomBlacklist.Add(Variables.LootMob.Guid, TimeSpan.FromDays(365));
                }
                if (Variables.SkinMob != null)
                {
                    CustomLog.Normal("Don't have permission to skin mob, blacklisting.");
                    CustomBlacklist.Add(Variables.SkinMob.Guid, TimeSpan.FromDays(365));
                }
            }

            if (errorMessage.Equals(errLootGone))
            {
                if (Variables.LootMob != null)
                {
                    CustomLog.Normal("Loot mob already looted, blacklisting.");
                    CustomBlacklist.Add(Variables.LootMob.Guid, TimeSpan.FromDays(365));
                }
                if (Variables.SkinMob != null)
                {
                    CustomLog.Normal("Skin mob already looted, blacklisting.");
                    CustomBlacklist.Add(Variables.SkinMob.Guid, TimeSpan.FromDays(365));
                }
            }

            if (errorMessage.Equals(spellFailedCantBeDisenchanted) ||
                errorMessage.Equals(errItemLocked) ||
                errorMessage.Equals(spellFailedLowCastLevel))
            {
                CustomBlacklist.Add(Variables.DisenchantItem, TimeSpan.FromDays(365));
                CustomLog.Normal("{0} blacklisted from disenchanting.", Variables.DisenchantItem.Name);
            }
        }

        public static void HandleAFKFlag()
        {
            if (!StyxWoW.Me.IsAFKFlagged)
                return;

            if (!Variables.AFKTimer.IsRunning)
            {
                if (StyxWoW.Me.Mounted || StyxWoW.Me.IsFlying)
                    return;

                KeyboardManager.KeyUpDown((char)KeyboardManager.eVirtualKeyMessages.VK_SPACE);
                Lua.DoString("JumpOrAscendStart()");
                StyxWoW.ResetAfk();
                CustomLog.Normal("AFK flag detected, jumped at {0}.", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                Variables.AFKTimer.Start();
            }
            else if (Variables.AFKTimer.ElapsedMilliseconds > 1000)
            {
                Variables.AFKTimer.Reset();
            }
        }

        public static void HandleCombatBug()
        {
            if (!StyxWoW.Me.Combat)
            {
                Variables.CombatBugStopwatch.Stop();
                return;
            }

            if (StyxWoW.Me.IsAutoAttacking || StyxWoW.Me.IsCasting || StyxWoW.Me.IsChanneling || !Variables.CombatBugStopwatch.IsRunning)
            {
                Variables.CombatBugStopwatch.Restart();
            }

            if (Variables.CombatBugStopwatch.ElapsedMilliseconds >= 45000)
            {
                CustomLog.Diagnostic("Combat bug detected, forcing new pull.");
                PriorityTreeState.TreeState = PriorityTreeState.State.Pulling;
            }
        }

        public static void HandlePulling()
        {
            if (Variables.NextMob != null && Variables.NextMob.IsValid)
            {
                if (Variables.NextMob.Guid != Variables.NextMobGuid)
                {
                    Variables.NextMobHP = Variables.NextMob.CurrentHealth;
                    Variables.NextMobGuid = Variables.NextMob.Guid;
                    Variables.PullBlacklistStopwatch.Reset();
                }

                if (Variables.NextMob.IsAlive && !Variables.NextMob.TaggedByOther)
                {
                    if (StyxWoW.Me.CurrentTarget != Variables.NextMob)
                        Variables.NextMob.Target();

                    if (Variables.NextMob.Location.Distance(StyxWoW.Me.Location) > 50)
                    {
                        var path = Navigator.GeneratePath(StyxWoW.Me.Location, Variables.NextMob.Location);
                        if (path != null && path.Length > 0)
                        {
                            Navigator.MoveTo(Variables.NextMob.Location);
                        }
                        else
                        {
                            CustomLog.Normal("Failed to generate path to {0}. Blacklisting.", Variables.NextMob.SafeName);
                            CustomBlacklist.Add(Variables.NextMob.Guid, TimeSpan.FromDays(365));
                            Variables.NextMob = null;
                        }
                    }
                }
                else
                {
                    Variables.NextMob = null;
                }
            }

            PriorityTreeState.TreeState = PriorityTreeState.State.ReadyForTask;
        }

        public static void HandleLooting()
        {
            if (Variables.LootMob != null && Variables.LootMob.IsValid)
            {
                if (Variables.LootMob.Location.Distance(StyxWoW.Me.Location) > 3)
                {
                    Navigator.MoveTo(Variables.LootMob.Location);
                    return;
                }

                if (StyxWoW.Me.IsMoving)
                    WoWMovement.MoveStop();

                if (!LootFrame.Instance.IsVisible)
                {
                    Variables.LootMob.Interact();
                    StyxWoW.Sleep(500);
                }

                // ✅ Clear both to prevent bouncing
                Variables.LootMob = null;
                Variables.SkinMob = null;
            }

            PriorityTreeState.TreeState = PriorityTreeState.State.ReadyForTask;
        }

        public static void HandleSkinning()
        {
            if (Variables.SkinMob != null && Variables.SkinMob.IsValid)
            {
                if (Variables.SkinMob.Location.Distance(StyxWoW.Me.Location) > 3)
                {
                    Navigator.MoveTo(Variables.SkinMob.Location);
                    return;
                }

                if (StyxWoW.Me.IsMoving)
                    WoWMovement.MoveStop();

                if (!LootFrame.Instance.IsVisible)
                {
                    Variables.SkinMob.Interact();
                    StyxWoW.Sleep(500);
                }

                // ✅ Clear both to prevent bouncing
                Variables.SkinMob = null;
                Variables.LootMob = null;
            }

            PriorityTreeState.TreeState = PriorityTreeState.State.ReadyForTask;
        }

        public static void HandleStartLocation()
        {
            if (Variables.SetStartLocation)
                return;

            if (StyxWoW.Me.IsFlying)
            {
                CustomLog.Normal("Must be on the ground to begin.");
                TreeRoot.Stop("Must be on the ground to begin.");
                return;
            }

            if (StyxWoW.Me.IsAlive)
            {
                Variables.StartLocation = StyxWoW.Me.Location;
                Variables.StartLocationZone = StyxWoW.Me.ZoneText;
                Variables.StartLocationSubzone = StyxWoW.Me.SubZoneText;
                Variables.StartLocationContinent = StyxWoW.Me.CurrentMap.Name;
                CustomLog.Normal("Start location set. Continent: {0} Zone: {1}, Subzone: {2}, XYZ: {3}",
                    Variables.StartLocationContinent, Variables.StartLocationZone, Variables.StartLocationSubzone, Variables.StartLocation);
                Variables.SetStartLocation = true;
            }
        }
    }

}
