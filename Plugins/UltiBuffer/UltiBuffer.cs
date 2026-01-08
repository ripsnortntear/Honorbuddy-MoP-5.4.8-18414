namespace UltiBuffer
{
    using Styx;
    using Styx.Common;
    using Styx.CommonBot;
    using Styx.Plugins;
    using Styx.WoWInternals;
    using Styx.WoWInternals.WoWObjects;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Media;

    public class UltiBuffer : HBPlugin
    {
        //Normal Stuff.
        public override string Name { get { return "UltiBuffer"; } }

        public override string Author { get { return "Trixiap"; } }

        public override Version Version { get { return new Version(1, 0, 1, 20141109); } }

        public override bool WantButton { get { return true; } }

        private LocalPlayer Me { get { return StyxWoW.Me; } }

        private Dictionary<int, int> itemIDs = new Dictionary<int, int>();
        private List<int> containersID = new List<int>();

        public override void OnEnable()
        {
            base.OnEnable();
            log("by " + Author + " enabled");
            bool state = loadIDs();
            if (state)
                log("List loaded successfully");
            else
                log("Problem with list, check diagnostic log");

            BotEvents.Player.OnMapChanged += zoneChange;
        }

        public override void OnDisable()
        {
            log("disabled");
            itemIDs.Clear();
            base.OnDisable();
        }

        public override void Pulse()
        {
            if (Me != null)
            {
                if ((!Me.IsActuallyInCombat) && (Me.PlayerControlled) && (Me.IsAlive) && (StyxWoW.IsInWorld) && (!Me.IsOnTransport) && (!Me.OnTaxi)) //thx Ramptar for mounted and taxi bugfix
                {
                    foreach (int buffID in itemIDs.Keys)
                    {
                        if (!Me.HasAura(buffID))
                        {
                            WoWItem item = buffMe(itemIDs[buffID]);
                            if ((item != null) && (item.Cooldown == 0) && (!Styx.CommonBot.SpellManager.GlobalCooldown))
                            {
                                bool sc = item.Use();
                                if (!sc)
                                {
                                    itemIDs.Remove(buffID);
                                    log(item.Name + " is not on cooldown but we can´t use it, Removing from list");
                                }
                            }
                        }
                    }
                    foreach (int itemID in containersID)
                    {
                        WoWItem item = buffMe(itemID);
                        if ((item != null) && (item.Cooldown == 0) && (!Styx.CommonBot.SpellManager.GlobalCooldown))
                        {
                            if (item.IsOpenable)
                            {
                                item.UseContainerItem();
                            }
                            else
                            {
                                item.Use();
                            }
                        }
                    }
                }
            }
        }

        public WoWItem buffMe(int itemID)
        {
            return Me.BagItems.FirstOrDefault(i => i.ItemInfo.Id == itemID);
        }

        public UltiBuffer()
        {
        }

        private bool loadIDs()
        {
            bool state = false;
            var lines = File.ReadLines(Utilities.AssemblyDirectory.ToString() + "\\Plugins\\UltiBuffer\\items.txt");
            foreach (string line in lines)
            {
                try
                {
                    int iID = int.Parse(line.Split(',')[0]);
                    int bID = int.Parse(line.Split(',')[1]);
                    if (bID == 0)
                    {
                        containersID.Add(iID);
                    }
                    else
                    {
                        itemIDs.Add(bID, iID);
                    }

                    state = true;
                }
                catch (Exception e)
                {
                    logD(e.ToString());
                    state = false;
                }
            }
            if (itemIDs.Count == 0)
            {
                logD("Item list is empty");
            }
            return state;
        }

        private static void log(string msg)
        {
            Logging.Write(LogLevel.Normal, Color.FromRgb(37, 185, 0), "[UltiBuffer]: " + msg);
        }

        private static void logD(string msg)
        {
            Logging.Write(LogLevel.Diagnostic, Color.FromRgb(255, 0, 0), "[UltiBuffer]: " + msg);
        }

        private void zoneChange(BotEvents.Player.MapChangedEventArgs args) //thx Stormchasing for hint about events
        {
            StyxWoW.Sleep(10000);
            if (Me.ZoneId == 6757)
            {
                log("We are on Timeless Isle - reloading list");
                bool state = loadIDs();
                if (state)
                    logD("List reloaded successfully");
                else
                    logD("Problem with list, check diagnostic log");
            }
        }
    }
}