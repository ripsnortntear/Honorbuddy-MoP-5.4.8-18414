// Credits to Phelon and Mirabis for making this work without using Assembly redirection.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;

namespace Loader
{
    public class Loader : CombatRoutine
    {
        private CombatRoutine CC;

        private readonly String[] Keep = new[] { "Loader.cs", "TuanHA_Combat_Routine.dll", "Settings", "User Settings", "Files", "Please Read - Installation Guide.txt", ".svn", "ChangeLog.txt" };

        public Loader()
        {
            string settingsDirectory = Path.Combine(Utilities.AssemblyDirectory, "Routines\\TuanHAWarriorPatronEdition");

            string path = settingsDirectory + @"\TuanHA_Combat_Routine.dll";

            if (!File.Exists(path))
            {
                MessageBox.Show("TuanHAWarriorPatronEdition is not installed correctly! Ensure files look like;" + 
                            Environment.NewLine + 
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHAWarriorPatronEdition/Files/<.doc .jpg>" +
							Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHAWarriorPatronEdition/Preset/<XML Files>" +
							Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHAWarriorPatronEdition/Settings/<XML Files>" +
							Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHAWarriorPatronEdition/User Settings/<XML Files>" +
							Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHAWarriorPatronEdition/ChangeLog.txt" +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHAWarriorPatronEdition/Loader.cs" + 
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHAWarriorPatronEdition/Please Read - Installation Guide.txt" +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/TuanHAWarriorPatronEdition/TuanHA_Combat_Routine.dll");
                return;
            }

            string pathTemp = settingsDirectory + @"\Files\TuanHA_Combat_Routine.dll.update";
            string pathTemp2 = settingsDirectory + @"\Files\TuanHA_Combat_Routine.dll";

            if (File.Exists(pathTemp))
            {
                if (File.Exists(path))
                {
                    try
                    {
                        //Logging.Write("Deleting " + path);
                        File.Delete(path);
                    }
                    catch (IOException ex)
                    {
                        Logging.Write(ex.ToString()); // Write error
                    }
                }

                try
                {
                    //Logging.Write("Moving " + pathTemp + " to " + path);
                    File.Move(pathTemp, pathTemp2);
                    File.Copy(pathTemp2, path);
                }
                catch (IOException ex)
                {
                    Logging.Write(ex.ToString()); // Write error
                }
            }

            bool removed = false;
            var DInfo = new DirectoryInfo(settingsDirectory);
            foreach (FileInfo file in DInfo.GetFiles())
            {
                if (!Keep.Contains(file.Name))
                {
                    removed = true;
                    Logging.Write("Removing " + file.Name + " from TuanHAWarriorPatronEdition directory");
                    file.Delete();
                }
            }
            //if (removed)
            //{
            //    MessageBox.Show("Removed unnecessary files from TuanHAWarriorPatronEdition directory, if TuanHAWarriorPatronEdition fails to work please restart honorbuddy.");
            //}

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

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
                Logging.Write(Colors.DarkRed, "An error occured while loading TuanHAWarriorPatronEdition!");
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

        //public override Composite PullBehavior
        //{
        //    get { return CC.PullBehavior; }
        //}

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
                return WoWClass.Warrior;
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

        #endregion
    }
}
