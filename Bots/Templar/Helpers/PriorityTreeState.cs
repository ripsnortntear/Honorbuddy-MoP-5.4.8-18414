using Styx;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Templar.GUI.Tabs;
using Templar.Helpers;
using System;

namespace Templar.Helpers {
    public class PriorityTreeState {

        // ===========================================================
        // Constants
        // ===========================================================

        public enum State {
            ReadyForTask,
            Dead,
            Looting,
            Skinning,
            Pulling,
            NoMobs,
            Vendoring,
            Mailing,
            Disenchanting,
            MoveToStartLocation,
            MoveToTarget,
        }

        // ===========================================================
        // Fields
        // ===========================================================

        private static DateTime _pullStartTime = DateTime.MinValue;
        private static WoWUnit _stuckTarget = null;
        private static State _previousLoggedState = (State)(-1);
        private static readonly System.Diagnostics.Stopwatch _moveLogStopwatch = new System.Diagnostics.Stopwatch();

        public static State TreeState = State.ReadyForTask;

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        public static void TreeStateHandler() {
            // Place checks that are state-independent here

            /////////////////////////////////////
            // START
            /////////////////////////////////////

            if (!StyxWoW.Me.IsValid) {
                return;
            }

            if (TreeState != _previousLoggedState) {
                CustomLog.Diagnostic("State: {0}", TreeState);
                _previousLoggedState = TreeState;
            }

            // *** DEATH CHECK ***
            // If the player is dead or a ghost, immediately enter Dead state and bypass all pulse logic.
            // In zones like Pandaria, corpse retrieval uses a ghost flying mount. We must never attempt
            // to dismount or initiate pulls/combat while dead or a ghost.
            if (StyxWoW.Me.IsDead || StyxWoW.Me.IsGhost) {
                if (TreeState != State.Dead) {
                    CustomLog.Normal("Player is dead or ghost. Entering Dead state.");
                    TreeState = State.Dead;
                }

                Variables.NextMob = null;
                Variables.LootMob = null;
                Variables.SkinMob = null;
                _stuckTarget = null;
                _pullStartTime = DateTime.MinValue;

                if (StyxWoW.Me.CurrentTarget != null) {
                    StyxWoW.Me.ClearTarget();
                }
                return;
            }

            // *** EMERGENCY DISMOUNT CHECK ***
            if (!StyxWoW.Me.IsDead && !StyxWoW.Me.IsGhost && StyxWoW.Me.Mounted && (StyxWoW.Me.Combat || Variables.MobsTargettingMe > 0)) {
                CustomLog.Normal("EMERGENCY: Dismounting during combat situation.");
                MountHelper.DismountIfMounted();
                return;
            }

            if (StyxWoW.Me.Combat) {
                _stuckTarget = null;
                _pullStartTime = DateTime.MinValue;
            }

            if (!Variables.AlteredSettings) {
                AlterSettings();
                LogSettings();

                Variables.InventoryList.Clear();
                foreach(var bagItem in StyxWoW.Me.BagItems) {
                    Variables.InventoryList.Add(bagItem);
                    CustomLog.Diagnostic("Inventory item added. Name: {0}, Entry: {1}, StackCount: {2}", bagItem.Name, bagItem.Entry, bagItem.StackCount);
                }
            }

            TaskManager.HandleStartLocation();
            TaskManager.HandleAFKFlag();
            TaskManager.HandleCombatBug();

            if (TreeState == State.Mailing) {
                if (Variables.MobsTargettingMe != 0) {
                    return;
                }

                if (MailSettings.Instance.Mail) {
                    Mail.CheckBags();

                    if (Variables.MailList.Count > 0) {
                        Mail.HandleMailing();
                        return;
                    }

                    TreeState = State.Vendoring;
                } else {
                    TreeState = State.Vendoring;
                }
            }

            if (TreeState == State.Vendoring) {
                if (Variables.MobsTargettingMe != 0) {
                    return;
                }

                if (VendorSettings.Instance.Vendor) {
                    Vendor.CheckBags();

                    if (Variables.VendorSellList.Count > 0 || Variables.NeedToRepair) {
                        Vendor.HandleVendoring();
                        return;
                    }

                    TreeState = State.MoveToStartLocation;
                } else {
                    TreeState = State.MoveToStartLocation;
                }
            }

            if (TreeState == State.Disenchanting) {
                if (Variables.MobsTargettingMe != 0) {
                    return;
                }

                if (DisenchantSettings.Instance.Disenchant) {
                    Disenchant.CheckBags();

                    if (Variables.DisenchantList.Count > 0) {
                        Disenchant.HandleDisenchanting();
                        return;
                    }

                    // Get rid of any active pending cursor spell (UNTESTED)
                    Lua.DoString("RunMacroText('/stopspelltarget')");

                    TreeState = State.ReadyForTask;
                } else {
                    TreeState = State.ReadyForTask;
                }
            }

            Variables.NeedToPull = true;

            if (GeneralSettings.Instance.LootMobs) {
                Variables.LootMob = Mob.GetLootMob;
            }

            if (GeneralSettings.Instance.SkinMobs) {
                Variables.SkinMob = Mob.GetSkinMob;
            }

            /////////////////////////////////////
            // END 
            ////////////////////////////////////////

            switch (TreeState) {
            case State.Dead:
                Variables.NextMob = null;
                Variables.LootMob = null;
                Variables.SkinMob = null;
                _stuckTarget = null;
                _pullStartTime = DateTime.MinValue;

                if (StyxWoW.Me.IsAlive && !StyxWoW.Me.IsDead && !StyxWoW.Me.IsGhost) {
                    CustomLog.Normal("Player has resurrected. Returning to ReadyForTask.");
                    TreeState = State.ReadyForTask;
                }
                break;

            case State.ReadyForTask:
                // Ready to switch states when necessary
                break;

            case State.Looting:
                if (Variables.MobsTargettingMe == 0) {
                    TaskManager.HandleLooting();
                } else {
                    TreeState = State.ReadyForTask;
                }
                break;

            case State.Skinning:
                if (Variables.MobsTargettingMe == 0) {
                    TaskManager.HandleSkinning();
                } else {
                    TreeState = State.ReadyForTask;
                }
                break;

            case State.Pulling:
                // If mounted/flying, fly closer until within pull range, then dismount to pull
                if (!StyxWoW.Me.IsDead && !StyxWoW.Me.IsGhost && StyxWoW.Me.Mounted) {
                    if (Variables.NextMob != null && Variables.NextMob.Distance > Variables.EffectivePullDistance) {
                        Flightor.MoveTo(Variables.NextMob.Location, false);
                        return;
                    }
                    CustomLog.Normal("In pull range ({0:F1}yd), dismounting to pull mob.", Variables.NextMob != null ? Variables.NextMob.Distance : 0);
                    MountHelper.DismountIfMounted();
                    return;
                }

                if (GeneralSettings.Instance.MultiPull) {
                    Variables.NeedToPull = (StyxWoW.Me.HealthPercent > GeneralSettings.Instance.HealthPercentThreshold &&
                        Variables.MobsTargettingMe < GeneralSettings.Instance.MobThreshold);
                }

                if (GeneralSettings.Instance.LootMobs && Variables.LootMob != null) {
                    Variables.NeedToPull = false;
                }

                if (GeneralSettings.Instance.SkinMobs && Variables.SkinMob != null) {
                    Variables.NeedToPull = false;
                }

                if (!GeneralSettings.Instance.MultiPull && StyxWoW.Me.Combat) {
                    Variables.NeedToPull = true;
                }

                if (!Variables.NeedToPull) {
                    TreeState = State.ReadyForTask;
                    _stuckTarget = null;
                    _pullStartTime = DateTime.MinValue;
                    break;
                }

                WoWUnit currentTarget = StyxWoW.Me.CurrentTarget;
                
                // Reset timer if target is dead or invalid
                if (currentTarget == null || !currentTarget.IsValid || currentTarget.IsDead) {
                    _pullStartTime = DateTime.MinValue;
                } else if (_pullStartTime == DateTime.MinValue) {
                    _pullStartTime = DateTime.Now;
                }

                // If targeting mob for 4+ seconds with no combat and not casting, move closer
                if (currentTarget != null && 
                    !StyxWoW.Me.Combat && 
                    !currentTarget.Combat && 
                    !StyxWoW.Me.IsCasting &&
                    !StyxWoW.Me.IsChanneling &&
                    currentTarget.Distance > 5f &&
                    _pullStartTime != DateTime.MinValue &&
                    (DateTime.Now - _pullStartTime).TotalSeconds >= 4.0) {
                    
                    CustomLog.Normal("Target {0} not engaged after 4s ({1:F1}yd away), moving closer...", 
                        currentTarget.SafeName, currentTarget.Distance);
                    _stuckTarget = currentTarget;
                    TreeState = State.MoveToTarget;
                    break;
                }

                TaskManager.HandlePulling();
                break;

            case State.MoveToTarget:
                if (_stuckTarget == null || 
                    !_stuckTarget.IsValid || 
                    _stuckTarget.IsDead || 
                    StyxWoW.Me.Combat || 
                    StyxWoW.Me.IsActuallyInCombat ||
                    StyxWoW.Me.IsCasting ||
                    StyxWoW.Me.IsChanneling ||
                    Variables.MobsTargettingMe > 0 ||
                    (DateTime.Now - _pullStartTime).TotalSeconds > 30) { // 30 second timeout
                    _stuckTarget = null;
                    _pullStartTime = DateTime.MinValue;
                    TreeState = State.ReadyForTask;
                    break;
                }

                float distanceToTarget = (float)_stuckTarget.Distance;
                if (distanceToTarget > 5f) {
                    if (!_moveLogStopwatch.IsRunning || _moveLogStopwatch.ElapsedMilliseconds >= 3000) {
                        CustomLog.Diagnostic("Moving closer to {0} ({1:F1}yd away)", _stuckTarget.SafeName, distanceToTarget);
                        _moveLogStopwatch.Restart();
                    }
                    Flightor.MoveTo(_stuckTarget.Location, false);
                } else {
                    CustomLog.Normal("Close enough to {0}, resuming pull", _stuckTarget.SafeName);
                    _stuckTarget = null;
                    _pullStartTime = DateTime.MinValue;
                    TreeState = State.Pulling;
                }
                break;

            case State.MoveToStartLocation:
                if (!Variables.IsAtStartLocation) {
                    Flightor.MoveTo(Variables.StartLocation, true);
                } else {
                    MountHelper.DismountIfMounted();

                    if (!StyxWoW.Me.Mounted && !StyxWoW.Me.IsFalling) {
                        CustomLog.Normal("Arrived at start location and dismounted.");
                        TreeState = State.ReadyForTask;
                    }
                }
                break;
            }

            if (TreeState == State.ReadyForTask) {
                if (!StyxWoW.Me.Combat) {
                    if (StyxWoW.Me.IsGhost || StyxWoW.Me.IsDead) {
                        TreeState = State.Dead;
                        return;
                    }

                    if (Variables.NeedToVendor) {
                        TreeState = State.Mailing;
                        return;
                    }

                    if (GeneralSettings.Instance.LootMobs) {
                        if (Variables.LootMob != null) {
                            TreeState = State.Looting;
                            return;
                        }
                    }

                    if (GeneralSettings.Instance.SkinMobs) {
                        if (Variables.SkinMob != null) {
                            TreeState = State.Skinning;
                            return;
                        }
                    }
                }

                Variables.NextMob = Mob.GetNextMob;

                if (Variables.NextMob != null) {
                    if (Variables.NeedToPull) {
                        TreeState = State.Pulling;
                    }
                } else {
                    if (!StyxWoW.Me.Combat && !StyxWoW.Me.IsActuallyInCombat && Variables.LootMob == null && Variables.SkinMob == null) {
                        if (!Variables.IsAtStartLocation) {
                            CustomLog.Normal("No more mobs available, moving back to start location.");
                            TreeState = State.MoveToStartLocation;
                        }
                    }
                }
            }
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static void AlterSettings() {
            // Enable auto loot
            Lua.DoString("SetCVar('AutoLootDefault','1')");
            CustomLog.Normal("Enabled autoloot in WoW.");

            // Set FPS in WoW
            Lua.DoString("SetCVar('maxFPS', '60')");
            CustomLog.Normal("Foreground FPS set to 60 in WoW.");
            Lua.DoString("SetCVar('maxFPSBk', '30')");
            CustomLog.Normal("Background FPS set to 30 in WoW.");

            Variables.AlteredSettings = true;
        }

        private static void LogSettings() {
            CustomLog.Diagnostic("**** GENERAL TAB ****");
            CustomLog.Diagnostic(string.Format("Ignore Elites: {0}", GeneralSettings.Instance.IgnoreElites));
            CustomLog.Diagnostic(string.Format("Loot Mobs: {0}", GeneralSettings.Instance.LootMobs));
            CustomLog.Diagnostic(string.Format("Time Between Loots: {0}", GeneralSettings.Instance.TimeBetweenLoots));
            CustomLog.Diagnostic(string.Format("Skin Mobs: {0}", GeneralSettings.Instance.SkinMobs));
            CustomLog.Diagnostic(string.Format("Ninja Skin: {0}", GeneralSettings.Instance.NinjaSkin));
            CustomLog.Diagnostic(string.Format("Time Between Skins: {0}", GeneralSettings.Instance.TimeBetweenSkins));
            CustomLog.Diagnostic(string.Format("Multi Pull: {0}", GeneralSettings.Instance.MultiPull));
            CustomLog.Diagnostic(string.Format("Multi Pull Mob Threshold: {0}%", GeneralSettings.Instance.MobThreshold));
            CustomLog.Diagnostic(string.Format("Multi Pull Health Threshold: {0}", GeneralSettings.Instance.HealthPercentThreshold));
            CustomLog.Diagnostic(string.Format("Start Location Max Distance: {0}", GeneralSettings.Instance.StartingLocationMaxDistance));
            CustomLog.Diagnostic(string.Format("Minimum Bag Slots: {0}", GeneralSettings.Instance.MinimumFreeBagSlots));
            CustomLog.Diagnostic("**** WHITELIST TAB ****");
            CustomLog.Diagnostic("Whitelisted units:");
            foreach(var whitelistedUnit in WhitelistSettings.Instance.WhitelistedUnits) {
                CustomLog.Diagnostic(string.Format("Name: {0} Entry: {1}", whitelistedUnit.Name, whitelistedUnit.Entry));
            }
            CustomLog.Diagnostic(string.Format("Attack Only Whitelist: {0}", WhitelistSettings.Instance.AttackWhitelistOnly));
            CustomLog.Diagnostic("**** BLACKLIST TAB ****");
            CustomLog.Diagnostic("Blacklisted units:");
            foreach(var blacklistedUnit in BlacklistSettings.Instance.BlacklistedUnits) {
                CustomLog.Diagnostic(string.Format("Name: {0} Entry: {1}", blacklistedUnit.Name, blacklistedUnit.Entry));
            }
            CustomLog.Diagnostic("**** PROTECTED ITEMS TAB ****");
            CustomLog.Diagnostic("Protected items:");
            foreach(var protectedItem in ProtectedItemSettings.Instance.ProtectedItems) {
                CustomLog.Diagnostic(string.Format("Name: {0} Entry: {1}", protectedItem.Name, protectedItem.Entry));
            }
            CustomLog.Diagnostic("**** MAIL TAB ****");
            CustomLog.Diagnostic(string.Format("Mailing: {0}", MailSettings.Instance.Mail));
            CustomLog.Diagnostic(string.Format("Mail Whites: {0}", MailSettings.Instance.MailWhites));
            CustomLog.Diagnostic(string.Format("Mail Greens: {0}", MailSettings.Instance.MailGreens));
            CustomLog.Diagnostic(string.Format("Mail Blues: {0}", MailSettings.Instance.MailBlues));
            CustomLog.Diagnostic(string.Format("Mail Purples: {0}", MailSettings.Instance.MailPurples));
            CustomLog.Diagnostic(string.Format("Recipient: {0}", MailSettings.Instance.Recipient == "" ? "empty" : "contains name"));
            CustomLog.Diagnostic("**** VENDOR TAB ****");
            CustomLog.Diagnostic(string.Format("Vendoring: {0}", VendorSettings.Instance.Vendor));
            CustomLog.Diagnostic(string.Format("Sell Grays: {0}", VendorSettings.Instance.SellGrays));
            CustomLog.Diagnostic(string.Format("Sell Whites: {0}", VendorSettings.Instance.SellWhites));
            CustomLog.Diagnostic(string.Format("Sell Greens: {0}", VendorSettings.Instance.SellGreens));
            CustomLog.Diagnostic(string.Format("Sell Blues: {0}", VendorSettings.Instance.SellBlues));
            CustomLog.Diagnostic(string.Format("Sell Purples: {0}", VendorSettings.Instance.SellPurples));
            CustomLog.Diagnostic(string.Format("Repair at: {0}%", VendorSettings.Instance.MinimumDurabilityForRepairs));
            CustomLog.Diagnostic("**** DISENCHANT TAB ****");
            CustomLog.Diagnostic(string.Format("Disenchanting: {0}", DisenchantSettings.Instance.Disenchant));
            CustomLog.Diagnostic(string.Format("Disenchant Only New Items: {0}", DisenchantSettings.Instance.DisenchantOnlyNewItems));
            CustomLog.Diagnostic(string.Format("Disenchant Greens: {0}", DisenchantSettings.Instance.DisenchantGreens));
            CustomLog.Diagnostic(string.Format("Disenchant Blues: {0}", DisenchantSettings.Instance.DisenchantBlues));
            CustomLog.Diagnostic(string.Format("Disenchant Purples: {0}", DisenchantSettings.Instance.DisenchantPurples));
            CustomLog.Diagnostic(string.Format("Time Between Disenchants: {0}", DisenchantSettings.Instance.TimeBetweenDisenchants));
        }
    }
}