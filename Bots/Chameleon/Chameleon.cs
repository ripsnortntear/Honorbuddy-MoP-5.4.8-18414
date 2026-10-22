using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Styx.Common;
using Styx.CommonBot;
using Styx.TreeSharp;

namespace Chameleon
{
    public class ChameleonBot : BotBase
    {
        private static readonly BotBase BotInstance;
        private static readonly bool Success;

        private static string BotFolder
        {
            get
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                try
                {
                    Assembly entry = Assembly.GetEntryAssembly();
                    if (entry != null && !string.IsNullOrEmpty(entry.Location))
                    {
                        baseDir = Path.GetDirectoryName(entry.Location);
                    }
                }
                catch
                {
                }
                return Path.Combine(baseDir, "Bots", "Chameleon");
            }
        }

        static ChameleonBot()
        {
            // Ensure any reference to Honorbuddy or Chameleon resolves dynamically
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs args)
            {
                if (args.Name.StartsWith("Honorbuddy"))
                {
                    return typeof(BotBase).Assembly;
                }
                if (args.Name.StartsWith("Chameleon"))
                {
                    string path = Path.Combine(BotFolder, "Chameleon.dll");
                    if (File.Exists(path))
                    {
                        return Assembly.Load(File.ReadAllBytes(path));
                    }
                }
                return null;
            };

            string filename = "Chameleon.dll";
            try
            {
                string dllPath = Path.Combine(BotFolder, filename);
                if (!File.Exists(dllPath))
                {
                    Logging.WriteQuiet(System.Windows.Media.Colors.Red, "[Chameleon] File not found: {0}", dllPath);
                    Success = false;
                    return;
                }

                byte[] buffer = File.ReadAllBytes(dllPath);
                Assembly botAssembly = Assembly.Load(buffer);
                Type botType = botAssembly.GetTypes().FirstOrDefault(delegate(Type t)
                {
                    return typeof(BotBase).IsAssignableFrom(t) && !t.IsAbstract;
                });

                if (botType != null)
                {
                    BotInstance = (BotBase)Activator.CreateInstance(botType);
                    Success = BotInstance != null;
                    if (Success)
                    {
                        Logging.WriteQuiet(System.Windows.Media.Colors.Green, "[Chameleon] Successfully loaded {0} from {1}", BotInstance.Name, dllPath);
                    }
                }
                else
                {
                    Logging.WriteQuiet(System.Windows.Media.Colors.Red, "[Chameleon] Could not find any class deriving from BotBase in {0}", filename);
                    Success = false;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteQuiet(System.Windows.Media.Colors.Red, "[Chameleon] Error loading bot: {0}", ex.Message);
                Logging.WriteDiagnostic(ex.ToString());
                Success = false;
            }
        }

        public override string Name
        {
            get { return Success ? BotInstance.Name : "[botHaven] Chameleon (Failed to load)"; }
        }

        public override bool RequiresProfile
        {
            get { return Success ? BotInstance.RequiresProfile : false; }
        }

        public override bool RequiresProfileScope
        {
            get { return Success ? BotInstance.RequiresProfileScope : false; }
        }

        public override Composite Root
        {
            get { return Success ? BotInstance.Root : null; }
        }

        public override PulseFlags PulseFlags
        {
            get { return Success ? BotInstance.PulseFlags : PulseFlags.All; }
        }

        public override System.Windows.Forms.Form ConfigurationForm
        {
            get { return Success ? BotInstance.ConfigurationForm : null; }
        }

        public override bool Equals(object obj)
        {
            return Success ? BotInstance.Equals(obj) : base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Success ? BotInstance.GetHashCode() : base.GetHashCode();
        }

        public override void Initialize()
        {
            if (Success)
            {
                BotInstance.Initialize();
            }
        }

        public override bool IsPrimaryType
        {
            get { return Success ? BotInstance.IsPrimaryType : base.IsPrimaryType; }
        }

        public override void Pulse()
        {
            if (Success)
            {
                BotInstance.Pulse();
            }
        }

        public override bool RequirementsMet
        {
            get { return Success ? BotInstance.RequirementsMet : true; }
        }

        public override void Start()
        {
            if (Success)
            {
                BotInstance.Start();
            }
        }

        public override void Stop()
        {
            if (Success)
            {
                BotInstance.Stop();
            }
        }

        public override void OnSelected()
        {
            if (Success)
            {
                BotInstance.OnSelected();
            }
            else
            {
                base.OnSelected();
            }
        }

        public override void OnDeselected()
        {
            if (Success)
            {
                BotInstance.OnDeselected();
            }
            else
            {
                base.OnDeselected();
            }
        }

        public override void OnPaused()
        {
            if (Success)
            {
                BotInstance.OnPaused();
            }
            else
            {
                base.OnPaused();
            }
        }

        public override void OnResumed()
        {
            if (Success)
            {
                BotInstance.OnResumed();
            }
            else
            {
                base.OnResumed();
            }
        }

        public override string ToString()
        {
            return Success ? BotInstance.ToString() : base.ToString();
        }
    }
}
