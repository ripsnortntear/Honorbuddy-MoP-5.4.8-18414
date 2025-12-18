
using Styx;
using Styx.Pathing;
using Styx.WoWInternals;
using Templar.GUI.Tabs;
using Templar.Helpers;

namespace Templar.Helpers
{
    public class PriorityTreeState
    {
        public enum State
        {
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
        }

        public static State TreeState = State.ReadyForTask;

        public static void TreeStateHandler()
        {
            if (!StyxWoW.Me.IsValid)
                return;

            if (!Variables.AlteredSettings)
            {
                AlterSettings();
                LogSettings();
                Variables.InventoryList.Clear();
                foreach (var bagItem in StyxWoW.Me.BagItems)
                {
                    Variables.InventoryList.Add(bagItem);
                }
            }

            TaskManager.HandleStartLocation();
            TaskManager.HandleAFKFlag();
            TaskManager.HandleCombatBug();

            Variables.NeedToPull = true;
            if (GeneralSettings.Instance.LootMobs)
                Variables.LootMob = Mob.GetLootMob;

            if (GeneralSettings.Instance.SkinMobs)
                Variables.SkinMob = Mob.GetSkinMob;

            switch (TreeState)
            {
                case State.Dead:
                    Variables.NextMob = null;
                    Variables.LootMob = null;
                    Variables.SkinMob = null;
                    if (StyxWoW.Me.IsAlive)
                        TreeState = State.ReadyForTask;
                    break;

                case State.ReadyForTask:
                    if (!StyxWoW.Me.Combat)
                    {
                        if (StyxWoW.Me.IsGhost || StyxWoW.Me.IsDead)
                        {
                            TreeState = State.Dead;
                            return;
                        }

                        // ✅ Loot first if not skinning
                        if (Variables.LootMob != null && Variables.SkinMob == null)
                        {
                            TreeState = State.Looting;
                            return;
                        }

                        // ✅ Skinning next
                        if (Variables.SkinMob != null)
                        {
                            TreeState = State.Skinning;
                            return;
                        }

                        if (Variables.NeedToVendor)
                        {
                            TreeState = State.Mailing;
                            return;
                        }
                    }

                    Variables.NextMob = Mob.GetNextMob;
                    if (Variables.NextMob != null && Variables.NeedToPull)
                    {
                        TreeState = State.Pulling;
                    }
                    else if (!StyxWoW.Me.Combat && Variables.LootMob == null && Variables.SkinMob == null)
                    {
                        TreeState = State.MoveToStartLocation;
                    }
                    break;

                case State.Looting:
                    TaskManager.HandleLooting();
                    break;

                case State.Skinning:
                    TaskManager.HandleSkinning();
                    break;

                case State.Pulling:
                    TaskManager.HandlePulling();
                    break;

                case State.MoveToStartLocation:
                    Flightor.MoveTo(Variables.StartLocation, true);
                    break;
            }
        }

        private static void AlterSettings()
        {
            Lua.DoString("SetCVar('AutoLootDefault','1')");
            Lua.DoString("SetCVar('maxFPS', '60')");
            Lua.DoString("SetCVar('maxFPSBk', '30')");
            Variables.AlteredSettings = true;
        }

        private static void LogSettings()
        {
            // Logging unchanged for brevity
        }
    }
}