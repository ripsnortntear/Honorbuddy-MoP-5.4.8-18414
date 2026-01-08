#region System Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
#endregion System Namespace

#region Foreign Namespace
#endregion Foreign Namespace

#region Styx Namespace
using Styx;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.Common;
using Styx.CommonBot;
using Styx.WoWInternals.WoWObjects;
#endregion Styx Namespace

namespace RepairAnywhere
{
    public class RepairAnywhere : HBPlugin
    {
        #region Plugin Stats
        public override string Name { get { return "RepairAnywhere"; } }
        public override string Author { get { return "Giwin"; } }
        public override Version Version { get { return new Version(1, 0); } }
        public override bool WantButton { get { return false; } }
        #endregion

        public override void Pulse()
        { 
            double Durability = StyxWoW.Me.DurabilityPercent * 100;

            if (Durability < 60)
            {
                foreach (WoWUnit unit in ObjectManager.GetObjectsOfType<WoWUnit>().Where(o => o.Entry == 32639))
                {
                    if (unit != null)
                    {
                        unit.Target();
                        if (unit.WithinInteractRange)
                            unit.Interact();
                        if (StyxWoW.Me.CurrentTarget != null && StyxWoW.Me.CurrentTarget.Entry == 32639)
                            Styx.CommonBot.Vendors.RepairAllItems();
                    }
                }
            }
            }
        }
    }

