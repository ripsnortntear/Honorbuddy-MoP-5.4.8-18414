using System;
using System.IO;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;

namespace Pookthetook.CacheKiller
{
    public class CacheKiller : HBPlugin
    {
        private string _wowCachePath;

        public override string Author { get { return "Pookthetook"; } }
        public override string Name { get { return "Cache Killer"; } }
        public override Version Version { get { return new Version(0, 1, 0, 1); } }

        public override void OnDisable()
        {
            if (!StyxWoW.IsInGame)
            {
                string hbCachePath = Path.Combine(Utilities.AssemblyDirectory, "Cache");
                DeleteCache(hbCachePath);
                DeleteCache(_wowCachePath);
            }
        }

        public override void OnEnable()
        {
            Log("v" + Version + " - Enabled");
            _wowCachePath = Path.Combine(Path.GetDirectoryName(StyxWoW.Memory.Process.MainModule.FileName), "Cache");
        }

        public override void Pulse()
        {
        }

        private void DeleteCache(string directory)
        {
            Log("Deleting cache from " + directory);

            try
            {
                Directory.Delete(directory, true);
            }
            catch
            {
                // ignored
            }
        }

        private void Log(string message)
        {
            Logging.Write(Colors.Silver, "[" + Name + "]: " + message);
        }
    }
}
