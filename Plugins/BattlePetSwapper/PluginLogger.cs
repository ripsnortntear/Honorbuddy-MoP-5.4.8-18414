using Styx.Common;

namespace BattlePetSwapper
{
    public class PluginLogger : IPluginLogger
    {
        public void Write(string message)
        {
            Logging.Write(System.Windows.Media.Colors.Gold, "[BPS] " + message);
        }
    }
}