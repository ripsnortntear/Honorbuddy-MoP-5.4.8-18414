using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Styx.Plugins;
using Styx.Common;

namespace UltimatePvPSuiteLoader
{
    public class UltimatePvPSuiteLoader : HBPlugin
    {
        private Type _currentAssembly;
        private MethodInfo _pulse;
        private MethodInfo _onButtonPress;
        public override Version Version { get { return new Version(1, 0); } }
        private Type _currentAssemblySVN;
        private MethodInfo update;

        public override string Author
        {
            get { return "Phelon"; }
        }

        public override string Name
        {
            get { return "* Ultimate PVP Suite *"; }
        }

        public override string ButtonText
        {
            get
            {
                return "* Ultimate PVP Suite *";
            }
        }
        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {
            try
            {
                _onButtonPress.Invoke(null, null);
            }
            catch (TargetInvocationException ex)
            {
                MessageBox.Show("Error" + ex.ToString());
            }
        }

        public override void Pulse()
        {
            try
            {
                _pulse.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Logging.Write("Error" + ex.ToString());
                throw;
            }
        }

        public override void Initialize()
        {
            try
            {
                var directory = DirectoryLocator();
                AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
                {
                    try
                    {
                        AssemblyName requestedName = new AssemblyName(e.Name);
                        Logging.Write(requestedName.FullName);
                        if (requestedName.Name == "Honorbuddy")
                        {
                            Logging.Write("Loading " + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                            return Assembly.LoadFile(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                        }
                        if (requestedName.Name == "GreyMagic")
                        {
                            Logging.Write("Loading " + Utilities.AssemblyDirectory + @"\GreyMagic.dll");
                            return Assembly.LoadFile(Utilities.AssemblyDirectory + @"\GreyMagic.dll");
                        }
                        return null;
                    }
                    catch (System.Exception)
                    {
                        return null;
                    }
                };
                if (_currentAssembly == null)
                {
                    Logging.Write(Utilities.AssemblyDirectory + @"\Plugins\" + directory +
                                              @"\UltimatePvPSuite.dll");
                    var asm =
                        File.ReadAllBytes(Utilities.AssemblyDirectory + @"\Plugins\" + directory +
                                          @"\UltimatePvPSuite.dll");
                    _currentAssembly = Assembly.Load(asm).GetType("UltimatePvPSuite.UltimatePvPSuite");
                    _currentAssembly.GetMethod("Initialize").Invoke(null, null);
                    _onButtonPress = _currentAssembly.GetMethod("OnButtonPress");
                    _pulse = _currentAssembly.GetMethod("Pulse");
                }
            }
            catch (Exception ex)
            {
                Logging.Write("Error" + ex.ToString());
                throw;
            }
        }

        private string DirectoryLocator()
        {
            try
            {
                foreach (
                    var dll in
                        Directory.EnumerateFiles(Utilities.AssemblyDirectory + @"\Plugins\", "UltimatePvPSuite.dll",
                            SearchOption.AllDirectories))
                {
                    return new FileInfo(dll).Directory.Name;
                }
                return "";
            }
            catch (Exception ex)
            {
                Logging.Write("Error" + ex.ToString());
                throw;
            }
        }
    }
}
