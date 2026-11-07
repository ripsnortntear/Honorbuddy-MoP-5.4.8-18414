// Credits to Phelon and Mirabis for making this work without using Assembly redirection.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
        private bool _loggedUnavailable;

        private readonly String[] Keep = new[] { "Loader.cs", "TuanHA_Combat_Routine.dll", "Settings", "User Settings", "Files", "Please Read - Installation Guide.txt", ".svn", "ChangeLog.txt" };

        public Loader()
        {
            string settingsDirectory = Path.Combine(Utilities.AssemblyDirectory, "Routines\\TuanHAPaladinSpecialEdition");
            string path = Path.Combine(settingsDirectory, "TuanHA_Combat_Routine.dll");

            if (!File.Exists(path))
            {
                Logging.Write(Colors.DarkRed, "TuanHAPaladinSpecialEdition: TuanHA_Combat_Routine.dll not found in " + settingsDirectory + ". Loader will remain inactive.");
                return;
            }

            string pathTemp = Path.Combine(settingsDirectory, "Files", "TuanHA_Combat_Routine.dll.update");
            string pathTemp2 = Path.Combine(settingsDirectory, "Files", "TuanHA_Combat_Routine.dll");

            if (File.Exists(pathTemp))
            {
                try
                {
                    if (File.Exists(path))
                        File.Delete(path);
                    if (File.Exists(pathTemp2))
                        File.Delete(pathTemp2);
                    File.Move(pathTemp, pathTemp2);
                    File.Copy(pathTemp2, path, true);
                }
                catch (IOException ex)
                {
                    Logging.Write("TuanHAPaladinSpecialEdition: update swap failed: " + ex.Message);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Logging.Write("TuanHAPaladinSpecialEdition: update swap failed: " + ex.Message);
                }
            }

            try
            {
                var DInfo = new DirectoryInfo(settingsDirectory);
                if (DInfo.Exists)
                {
                    foreach (FileInfo file in DInfo.GetFiles())
                    {
                        if (!Keep.Contains(file.Name))
                        {
                            try
                            {
                                Logging.Write("Removing " + file.Name + " from TuanHAPaladinSpecialEdition directory");
                                file.Delete();
                            }
                            catch (Exception ex)
                            {
                                Logging.Write("TuanHAPaladinSpecialEdition: could not remove " + file.Name + ": " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Write("TuanHAPaladinSpecialEdition: cleanup failed: " + ex.Message);
            }

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
                {
                    try
                    {
                        AssemblyName requestedName = new AssemblyName(e.Name);
                        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            if (string.Equals(a.GetName().Name, requestedName.Name, StringComparison.OrdinalIgnoreCase))
                                return a;
                        }
                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                };

                byte[] Bytes = File.ReadAllBytes(path);
                Assembly asm = Assembly.Load(Bytes);

                foreach (Type t in asm.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(CombatRoutine)) && t.IsClass && !t.IsAbstract)
                    {
                        object obj = Activator.CreateInstance(t);
                        CC = (CombatRoutine)obj;
                        break;
                    }
                }

                if (CC == null)
                    Logging.Write(Colors.DarkRed, "TuanHAPaladinSpecialEdition: loaded DLL but no CombatRoutine subclass was found.");
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
                    if (exSub == null) continue;
                    sb.AppendLine(exSub.Message);
                    var exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null && !string.IsNullOrEmpty(exFileNotFound.FusionLog))
                    {
                        sb.AppendLine("Fusion Log:");
                        sb.AppendLine(exFileNotFound.FusionLog);
                    }
                    sb.AppendLine();
                }
                Logging.Write(Colors.DarkRed, "TuanHAPaladinSpecialEdition: ReflectionTypeLoadException while loading routine.");
                Logging.Write(sb.ToString());
            }
            catch (Exception e)
            {
                Logging.Write(Colors.DarkRed, "An error occured while loading TuanHAPaladinSpecialEdition!");
                Logging.Write(e.ToString());
            }
        }

        private bool EnsureLoaded(string member)
        {
            if (CC != null)
                return true;
            if (!_loggedUnavailable)
            {
                _loggedUnavailable = true;
                Logging.Write(Colors.DarkRed, "TuanHAPaladinSpecialEdition: compiled routine unavailable; skipped " + member + ".");
            }
            return false;
        }

        #region Overrides of CombatRoutine

        public override string Name
        {
            get { return CC != null ? CC.Name : "TuanHAPaladinSpecialEdition (Unavailable)"; }
        }

        public override void Initialize()
        {
            if (EnsureLoaded("Initialize()"))
                CC.Initialize();
        }

        public override void ShutDown()
        {
            if (CC != null)
                CC.ShutDown();
        }

        public override Composite CombatBehavior
        {
            get { return CC != null ? CC.CombatBehavior : null; }
        }

        public override Composite PreCombatBuffBehavior
        {
            get { return CC != null ? CC.PreCombatBuffBehavior : null; }
        }

        public override Composite RestBehavior
        {
            get { return CC != null ? CC.RestBehavior : null; }
        }

        public override Composite DeathBehavior
        {
            get { return CC != null ? CC.DeathBehavior : null; }
        }

        public override WoWClass Class
        {
            get { return WoWClass.Paladin; }
        }

        public override void OnButtonPress()
        {
            if (EnsureLoaded("OnButtonPress()"))
                CC.OnButtonPress();
        }

        public override void Pulse()
        {
            if (CC != null)
                CC.Pulse();
        }

        public override bool WantButton
        {
            get { return CC != null; }
        }

        #endregion
    }
}