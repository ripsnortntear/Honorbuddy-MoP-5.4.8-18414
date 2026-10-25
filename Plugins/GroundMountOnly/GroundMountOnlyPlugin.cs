using System;
using Styx.Common;
using Styx.Plugins;

namespace Plugins.GroundMountOnly
{
    public class GroundMountOnlyPlugin : HBPlugin
    {
        static GroundMountOnlyPlugin()
        {
            LevelbotFormHook.Initialize();
        }

        public GroundMountOnlyPlugin()
        {
            LevelbotFormHook.Initialize();
        }

        public override string Name
        {
            get { return "Ground Mount Only"; }
        }

        public override string Author
        {
            get { return "Antigravity"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0, 0); }
        }

        public override bool WantButton
        {
            get { return true; }
        }

        public override string ButtonText
        {
            get { return "Settings"; }
        }

        public override void OnButtonPress()
        {
            try
            {
                var form = new Levelbot.FormLevelbotSettings();
                form.Show();
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
        }

        public override void Pulse()
        {
            LevelbotFormHook.CheckOpenForms();

            if (GroundMountSettings.Instance.UseGroundMountsOnly)
            {
                LevelbotFormHook.ApplyFlightorRestriction(true);
            }
        }

        public override void OnEnable()
        {
            LevelbotFormHook.Initialize();
            GroundMountSettings.Instance.UseGroundMountsOnly = true;
            GroundMountSettings.Instance.Save();
            LevelbotFormHook.ApplyFlightorRestriction(true);
            Logging.Write("[GroundMountOnly] Ground mounts only mode ENABLED.");
        }

        public override void OnDisable()
        {
            GroundMountSettings.Instance.UseGroundMountsOnly = false;
            GroundMountSettings.Instance.Save();
            LevelbotFormHook.ApplyFlightorRestriction(false);
            Logging.Write("[GroundMountOnly] Ground mounts only mode DISABLED.");
        }
    }
}
