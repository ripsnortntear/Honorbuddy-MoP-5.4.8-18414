// Credits to Phelon and Mirabis for making this work without using Assembly redirection.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using System.Net;
using Styx;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
 
namespace Loader
{
    public class DemonicLoader : CombatRoutine
    {
        private CombatRoutine CC;

        private readonly String[] Keep = new[] { "DemonicLoader.cs", "Demonic.dll", "Settings", "Changelog.txt", "de-DE", "ru-RU", "zh-Hans" };

        public DemonicLoader()
        {
            string settingsDirectory = Path.Combine(Utilities.AssemblyDirectory, "Routines\\Demonic");

            string path = settingsDirectory + @"\Demonic.dll";

            if (!File.Exists(path))
            {
                MessageBox.Show("Demonic is not installed correctly! Ensure files look like;" + 
                            Environment.NewLine + 
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/Demonic/Demonic.dll" + Environment.NewLine +
                            "<Honorbuddy>/Routines/Demonic/DemonicLoader.cs" + Environment.NewLine +
                            "<Honorbuddy>/Routines/Demonic/Settings/<XML Files>");
                return;
            }
            bool removed = false;
            var DInfo = new DirectoryInfo(settingsDirectory);
            foreach (FileInfo file in DInfo.GetFiles())
            {
                if (!Keep.Contains(file.Name))
                {
                    removed = true;
                    Logging.Write("Removing " + file.Name + " from Demonic directory");
                    file.Delete();
                }
            }
            if (removed)
            {
                MessageBox.Show("Removed unnecessary files from Demonic directory, if Demonic fails to work please restart honorbuddy.");
            }

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

				// Ensure modern TLS protocols are enabled before any network calls in Demonic.dll
				try
				{
					ServicePointManager.Expect100Continue = true;
					// Added TLS 1.2, 1.1, and 1.0 support even on older .NET versions where enums may be missing
					ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072; // Tls12
					ServicePointManager.SecurityProtocol |= (SecurityProtocolType)768;  // Tls11
					ServicePointManager.SecurityProtocol |= (SecurityProtocolType)192;  // Tls
				}
				catch { }

				AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
                {
                    try
                    {
                        AssemblyName requestedName = new AssemblyName(e.Name);
                        if (requestedName.Name == "Honorbuddy")
                        {
                            return Assembly.LoadFile(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                        }
						if (requestedName.Name == "GreyMagic")
                        {
                            return Assembly.LoadFile(Utilities.AssemblyDirectory + @"\GreyMagic.dll");
                        }
                        return null;
                    }
                    catch (System.Exception)
                    {
                        return null;
                    }
                };

                byte[] Bytes = File.ReadAllBytes(path);
                Assembly asm = Assembly.Load(Bytes);

                foreach (Type t in asm.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(CombatRoutine)) && t.IsClass)
                    {
                        object obj = Activator.CreateInstance(t);
                        CC = (CombatRoutine)obj;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
                Logging.Write(errorMessage);
            }
            catch (Exception e)
            {
                Logging.Write(Colors.DarkRed, "An error occured while loading Demonic!");
                Logging.Write(e.ToString());
            }
        }

        #region Overrides of CombatRoutine

        public override string Name
        {
            get
            {
                return CC.Name;
            }
        }

        public override void Initialize()
        {
            CC.Initialize();
        }

        public override Composite CombatBehavior
        {
            get { return CC.CombatBehavior; }
        }

        public override Composite PreCombatBuffBehavior
        {
            get { return CC.PreCombatBuffBehavior; }
        }

        public override Composite PullBehavior
        {
            get { return CC.PullBehavior; }
        }

        public override Composite RestBehavior
        {
            get { return CC.RestBehavior; }
        }
		
		public override Composite DeathBehavior
        {
            get { return CC.DeathBehavior; }
        }

        public override WoWClass Class
        {
            get
            {
                return WoWClass.Warlock;
            }
        }

        public override void OnButtonPress()
        {
            CC.OnButtonPress();
        }

        public override void Pulse()
        {
            CC.Pulse();
        }

        public override bool WantButton
        {
            get { return true; }
        }
		
		public override bool NeedPreCombatBuffs
        {
            get
            {
                return CC.NeedPreCombatBuffs;
            }
        }

        #endregion
    }
}
