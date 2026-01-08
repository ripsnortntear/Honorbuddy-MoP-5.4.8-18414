using Styx.Helpers;
namespace RoutesBuddy
{
    public class RoutesBuddySettings : Settings
    {
        public RoutesBuddySettings(string settingsPath)
            : base(settingsPath)
        {
            Load();
        }
        [Setting, DefaultValue("")]
        public string LastFolder { get; set; }
    }
}
