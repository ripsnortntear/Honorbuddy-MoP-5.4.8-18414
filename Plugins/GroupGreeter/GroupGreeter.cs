using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Text;
using System;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.TreeSharp;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Pathing;
using Styx.Plugins;


namespace GroupGreet
{
    public partial class GroupGreeter : HBPlugin
    {
        public override string Name { get { return "GroupGreeter"; } }
        public override string Author { get { return "wownerds"; } }
        public override Version Version { get { return new Version(0, 0, 3, 4); } }
        public override string ButtonText { get { return "Config"; } }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }
        public override void OnButtonPress()
        {
            GroupGreet.GroupGreeterCFG cfg = new GroupGreet.GroupGreeterCFG();
            cfg.ShowDialog();
        }

        public bool gg_enable;
        public bool greetedyet;
        public bool tankmark;

        public override void Initialize()
        {
            GroupGreeterSettings.Instance.Load();
            greetedyet = false;
            Logging.Write("[GroupGreet]: GroupGreeter loaded up.");
            if (GroupGreeterSettings.Instance.gg_enable == false)
            {
                Logging.Write("[GroupGreet]: GroupGreeter is currently DISABLED!");
            }
            else
            {
                Logging.Write("[GroupGreet]: GroupGreeter is currently ENABLED!");
                Logging.Write("[GroupGreet]: Your greeting text is: \"" + GroupGreeterSettings.Instance.greeting1.ToString() + "\"!");
            }
            Logging.WriteDiagnostic("[DIAG_GroupGreet]: greeted? " + greetedyet.ToString());
            Pulse();
        }


        public override void Pulse()
        {
            if (!Me.GroupInfo.IsInParty && !Me.GroupInfo.IsInLfgParty && !Me.GroupInfo.IsInRaid)
            {
                greetedyet = false;
                //Logging.WriteDiagnostic("[DIAG_GroupGreet]: greeted? " + greetedyet.ToString());
            }
            Greet();
            TankMark();
        }

        public int rnd
        {
            get
            {
                Random rnd = new Random();
                int number = rnd.Next(1, 6); 
                return number;
            }
        }

        private void Greet()
        {
            int random = rnd;
            if (random == 1 && GroupGreeterSettings.Instance.gg_enable == true && GroupGreeterSettings.Instance.g1_enable && GroupGreeterSettings.Instance.greeting1.Length > 0 && greetedyet == false && (Me.GroupInfo.IsInParty || Me.GroupInfo.IsInLfgParty || Me.GroupInfo.IsInRaid))
            {
                Styx.Common.Logging.Write("[GroupGreet]: You joined a new group!");
                Styx.WoWInternals.Lua.DoString("SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting1.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\");");
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting1.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\")");
                Styx.Common.Logging.Write("[GroupGreet]: Your level " + Me.Level.ToString() + " " + Me.Class.ToString() + " greeted your group saying: \"" + GroupGreeterSettings.Instance.greeting1.ToString() + "\"!");
                Logging.WriteDiagnostic("[GroupGreet]: rnd: " + random.ToString());
                greetedyet = true;
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: greeted? " + greetedyet.ToString());
            }
            else if (random == 2 && GroupGreeterSettings.Instance.gg_enable == true && GroupGreeterSettings.Instance.g2_enable && GroupGreeterSettings.Instance.greeting2.Length > 0 && greetedyet == false && (Me.GroupInfo.IsInParty || Me.GroupInfo.IsInLfgParty || Me.GroupInfo.IsInRaid))
            {
                Styx.Common.Logging.Write("[GroupGreet]: You joined a new group!");
                Styx.WoWInternals.Lua.DoString("SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting2.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\");");
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting2.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\")");
                Styx.Common.Logging.Write("[GroupGreet]: Your level " + Me.Level.ToString() + " " + Me.Class.ToString() + " greeted your group saying: \"" + GroupGreeterSettings.Instance.greeting2.ToString() + "\"!");
                greetedyet = true;
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: greeted? " + greetedyet.ToString());
            }
            else if (random == 3 && GroupGreeterSettings.Instance.gg_enable == true && GroupGreeterSettings.Instance.g3_enable && GroupGreeterSettings.Instance.greeting3.Length > 0 && greetedyet == false && (Me.GroupInfo.IsInParty || Me.GroupInfo.IsInLfgParty || Me.GroupInfo.IsInRaid))
            {
                Styx.Common.Logging.Write("[GroupGreet]: You joined a new group!");
                Styx.WoWInternals.Lua.DoString("SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting3.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\");");
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting3.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\")");
                Styx.Common.Logging.Write("[GroupGreet]: Your level " + Me.Level.ToString() + " " + Me.Class.ToString() + " greeted your group saying: \"" + GroupGreeterSettings.Instance.greeting3.ToString() + "\"!");
                greetedyet = true;
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: greeted? " + greetedyet.ToString());
            }
            else if (random == 4 && GroupGreeterSettings.Instance.gg_enable == true && GroupGreeterSettings.Instance.g4_enable && GroupGreeterSettings.Instance.greeting4.Length > 0 && greetedyet == false && (Me.GroupInfo.IsInParty || Me.GroupInfo.IsInLfgParty || Me.GroupInfo.IsInRaid))
            {
                Styx.Common.Logging.Write("[GroupGreet]: You joined a new group!");
                Styx.WoWInternals.Lua.DoString("SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting4.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\");");
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting4.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\")");
                Styx.Common.Logging.Write("[GroupGreet]: Your level " + Me.Level.ToString() + " " + Me.Class.ToString() + " greeted your group saying: \"" + GroupGreeterSettings.Instance.greeting4.ToString() + "\"!");
                greetedyet = true;
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: greeted? " + greetedyet.ToString());
            }
            else if (random == 5 && GroupGreeterSettings.Instance.gg_enable == true && GroupGreeterSettings.Instance.g5_enable && GroupGreeterSettings.Instance.greeting5.Length > 0 && greetedyet == false && (Me.GroupInfo.IsInParty || Me.GroupInfo.IsInLfgParty || Me.GroupInfo.IsInRaid))
            {
                Styx.Common.Logging.Write("[GroupGreet]: You joined a new group!");
                Styx.WoWInternals.Lua.DoString("SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting5.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\");");
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: SendChatMessage(\"" + GroupGreeterSettings.Instance.greeting5.ToString() + "\", \"INSTANCE_CHAT\", \"nil\", \"INSTANCE_CHAT\")");
                Styx.Common.Logging.Write("[GroupGreet]: Your level " + Me.Level.ToString() + " " + Me.Class.ToString() + " greeted your group saying: \"" + GroupGreeterSettings.Instance.greeting5.ToString() + "\"!");
                greetedyet = true;
                Logging.WriteDiagnostic("[DIAG_GroupGreet]: greeted? " + greetedyet.ToString());
            }
        }

        private void TankMark()
        {
            if ((Me.GroupInfo.IsInParty || Me.GroupInfo.IsInLfgParty || Me.GroupInfo.IsInRaid) && Me.CurrentTarget != null && Me.CurrentTarget.IsBoss && Me.CurrentTarget.GetMark() != RaidTargetMarker.Skull && GroupGreeterSettings.Instance.tank_marking == true && Me.IsInInstance && Me.CurrentTarget.IsHostile)
            {
                Me.CurrentTarget.Mark(RaidTargetMarker.Skull);
                Logging.Write("[GroupGreet]: Marked " + Me.CurrentTarget.Name.ToString() + " with Skull Raidmark.");
            }
        }
    }
}
