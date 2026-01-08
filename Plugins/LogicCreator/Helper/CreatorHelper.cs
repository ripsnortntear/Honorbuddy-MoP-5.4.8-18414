using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Styx.Plugins;
using Styx.Common;
using System.Windows.Media;
using System.Windows.Forms;
using Styx.Helpers;
using Styx.CommonBot;
using System.Threading;

namespace PokehbuddyLogicCreator
{
    class CreatorHelper : HBPlugin
    {
        public override string Author { get { return "BarryDurex"; } }
        public override string Name { get { return "[LogicCreator]"; } }
        public override Version Version { get { return new Version(1, 0, 0); } }
        public override string ButtonText { get { return this.Name; } }
        public override bool WantButton { get { return true; } }
        public static CreatorHelper Now;
        public static Thread UpdateCheckThread;
        public static string ForumLink = "http://www.thebuddyforum.com/honorbuddy-forum/plugins/uncataloged/96589-petbattle-plugin-logiccreator-pok-buddy.html";
        public static string HelpLink = "http://www.thebuddyforum.com/honorbuddy-forum/plugins/uncataloged/96589-petbattle-plugin-logiccreator-pok-buddy.html#post952327";

        public CreatorHelper()
        {
            Now = this;
        }

        public override void OnButtonPress()
        {
            if (TreeRoot.IsRunning)
            {
                DialogResult res = MessageBox.Show(@"Honorbuddy is running!
Dont let HonorBuddy run while editing and start a battle manually!

  - Do you want to stop Honorbuddy now?", "stop Honorbuddy?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (res == DialogResult.Yes)
                    TreeRoot.Stop();
                else if (res != DialogResult.No)
                    return;
            }
            new Menue().Show();
        }

        public static string PluginFolder { get { return Application.StartupPath + "\\Plugins\\LogicCreator\\"; } }

        public static void slog(Color color, string format, params object[] args)
        { Logging.Write(color, Now.Name + ": " + format, args); }

        public static void slog(string format, params object[] args)
        { slog(Colors.Goldenrod, format, args); }

        public static void dlog(string format, params object[] args)
        { Logging.WriteDiagnostic(Colors.Salmon, Now.Name + ": " + format, args); }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Pulse()
        {
            throw new NotImplementedException();
        }

        public static void updateCheckStart()
        {
            if ((UpdateCheckThread != null && UpdateCheckThread.IsAlive) || (DateTime.Now - lastCheck < TimeSpan.FromHours(1)))
                return;

            UpdateCheckThread = new Thread(updateCheck) { IsBackground = true, Name = "LogicCreator - update check" };
            UpdateCheckThread.Start();
            lastCheck = DateTime.Now;
            Menue.ActiveForm.lb_updateCheck.Text = "checking for updates ...";
            Menue.ActiveForm.lb_updateCheck.Visible = true;
        }

        private static DateTime lastCheck = DateTime.FromBinary(0);
        private static void updateCheck()
        {
            try
            {
                System.Net.WebClient wClient = new System.Net.WebClient();
                string strSource = wClient.DownloadString("http://petfighter.googlecode.com/svn/trunk/LogicCreator.txt");
                Version _v = new Version(strSource);

                if (Now.Version < _v)
                {
                    Menue.ActiveForm.lb_updateCheck.Text = "new version is available!";
                    string _msg = string.Format("a new version is available! \n Version: {0} \nDo you want to visit the forum now?", strSource);

                    if (MessageBox.Show(_msg, "new Version", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        System.Diagnostics.Process.Start(ForumLink);
                    return;
                }

                if (Menue.ActiveForm == null)
                    return;

                Menue.ActiveForm.lb_updateCheck.Text = "no update found!";
                Thread.Sleep(TimeSpan.FromSeconds(4));
                Menue.ActiveForm.lb_updateCheck.Visible = false;
            }
            catch
            { }
        }
    }
}


