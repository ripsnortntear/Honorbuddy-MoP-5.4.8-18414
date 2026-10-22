using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Styx.CommonBot;
using Styx.TreeSharp;

namespace BothavenManagerLoaders.Bots
{
    public class BotBase84 : BotBase
    {
        static BotBase84()
        {
            var filename = "Chameleon" + ".dll";
            //BothavenManager.BothavenManager.User.CheckForUpdates(84);
            try
            {
                var buffer = File.ReadAllBytes(Path.Combine(BotFolder, filename));
                Assembly botAssembly = Assembly.Load(buffer);
                var nc = botAssembly.GetTypes().FirstOrDefault(t => typeof(BotBase).IsAssignableFrom(t));
                BotInstance = (BotBase)botAssembly.CreateInstance(nc.FullName);
                Success = BotInstance != null;
            }
            catch (Exception)
            {
                Success = false;
            }
        }

        private static readonly BotBase BotInstance;
        private static string BotFolder
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Bots", "Chameleon"); }
        }
        private static readonly bool Success;
        public override string Name
        {
            get
            {
                return Success ? BotInstance.Name : "[botHaven] Chameleon failed to load.";
            }
        }
		public override bool RequiresProfile
        {
            get
            {
                return Success ? BotInstance.RequiresProfile : base.RequiresProfile;
            }
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
            get
            {
                return Success ? BotInstance.ConfigurationForm : null;
            }
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
                BotInstance.Initialize();
        }
        public override bool IsPrimaryType
        {
            get
            {
                return Success ? BotInstance.IsPrimaryType : base.IsPrimaryType;
            }
        }
        public override void Pulse()
        {
            if (Success)
                BotInstance.Pulse();
        }
        public override bool RequirementsMet
        {
            get
            {
                return Success ? BotInstance.RequirementsMet : base.RequirementsMet;
            }
        }
        public override void Start()
        {
            if (Success)
                BotInstance.Start();
        }
        public override void Stop()
        {
            if (Success)
                BotInstance.Stop();
        }
        public override string ToString()
        {
            return Success ? BotInstance.ToString() : base.ToString();
        }
    }
}