using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bots.Grind;
using Styx.CommonBot.Frames;
using System;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.TreeSharp;
using Styx.Common.Helpers;
using System.Windows.Media;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bots.Grind;
using Styx.CommonBot.Frames;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Common.Helpers;
using System.Threading;
using Action = Styx.TreeSharp.Action;
using System.Xml;
using System.Xml.Linq;
namespace EggCollector
{
    public class EggCollector : HBPlugin
    {
        public override string Name
        {
            get { return "EggCollector"; }
        }

        public override string Author
        {
            get { return "Silent"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0); }
        }

        public override bool WantButton
        {
            get { return false; }
        }

        public override void Initialize()
        {
        }


        public override void Pulse()
        {

            if (!StyxWoW.IsInWorld || !StyxWoW.Me.IsValid || StyxWoW.Me.IsDead)
            {
                return;
            }

            ObjectManager.Update();
            var chest =
                from obj in ObjectManager.GetObjectsOfType<WoWGameObject>(false, false)
                where obj.Entry == 214945
                orderby obj.Distance ascending
                select obj;
            if (chest != null && chest.Count() != 0 && chest.First().IsValid)
            {
                var egg = chest.First();
               
                Styx.Common.Logging.Write("egg is valid");
                while (egg.Distance > StyxWoW.Me.InteractRange)
                {
                        Styx.Common.Logging.Write("fly to egg....");
                        Flightor.MoveTo(egg.Location);
                        StyxWoW.Sleep(500);
                }
                Flightor.MountHelper.Dismount();
                StyxWoW.Sleep(800);
                if (!StyxWoW.Me.IsCasting)
                {
                    egg.Interact();
                    Styx.Common.Logging.Write("waiting for action complete");
                    StyxWoW.Sleep(2000);
                }
                StyxWoW.Sleep(500);
                Flightor.MountHelper.MountUp();
                
                
            }
            

           
   
            
        }

    }
}
