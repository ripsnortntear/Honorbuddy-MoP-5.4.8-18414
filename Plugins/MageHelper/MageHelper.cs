using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals.World;
using System.Windows.Forms;
using Styx.Common;
using Styx.Plugins;
using Styx.Pathing;




namespace MageHelper
{
    partial class MageHelper : HBPlugin
    {

        public override string Name { get { return "Mage Helper"; } }
        public override string Author { get { return "No1KnowsY"; } }
        public override Version Version { get { return new Version(1, 2, 3); } }
        public LocalPlayer Me { get { return Styx.StyxWoW.Me; } }
        public bool checkedMobs = false;
        private WoWUnit lastLoot = null;
        private WoWObject LastPOI;

        public override bool WantButton { get { return true; } }

        private Form _configForm;
        public override void OnButtonPress()
        {
            if (_configForm == null || _configForm.IsDisposed || _configForm.Disposing)
                _configForm = new UI();

            _configForm.ShowDialog();
        }

        public void Log(string argument)
        {
            Logging.Write("[{0}] {1}", Name, argument);
        }

        public void Log(string argument ,string target)
        {
            if (target != null)
                Logging.Write("[{0}] {1} {2}", Name, argument, target);
            else
                Logging.Write("[{0}] {1}", Name, argument);
        }

        public override void Initialize()
        {

            MageHelperSettings.Instance.Load();
            if (Me.Class == WoWClass.Mage) { Log("Starting plugin.", null); }
            else{ Log("Current character is not a Mage. Plugin is worthless.", null); }
        }

        public bool UseBlinkLoot { get { return MageHelperSettings.Instance.UseBlink; } }
        public bool UseSlowFall { get { return MageHelperSettings.Instance.UseSlowFall; } }
        public bool UseInvis { get { return MageHelperSettings.Instance.UseInvis; } }
        public bool UseBlinkKill { get { return MageHelperSettings.Instance.UseBlinkKillPOI; } }
        public bool AlwaysIceB { get { return MageHelperSettings.Instance.AlwaysHaveIceBarrier; } }
        public int InvisAdds { get { return MageHelperSettings.Instance.InvisAdds; } }
        public int InvisHP { get { return MageHelperSettings.Instance.InvisHP; } }


        private void checkUnits(string unitType, string checkThis)
        {
            //Potential method to check for certain properties depending on unit type
            ObjectManager.Update();
            List<WoWUnit> objList = ObjectManager.GetObjectsOfType<WoWUnit>()
            .Where(o => !o.IsDead)
            .OrderBy(o => o.Distance).ToList();
        }


        

        public override void Dispose()
        {
        }

        Random rnd = new Random();
        public int ranNum(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public override void Pulse()
        {
            if (Me.Class == WoWClass.Mage)
            {
                BlinkToPOI();
                SlowFall();
                Invisibility();
                IceBarrier();
                //PortalsRule();
            }

        }
    }
}


