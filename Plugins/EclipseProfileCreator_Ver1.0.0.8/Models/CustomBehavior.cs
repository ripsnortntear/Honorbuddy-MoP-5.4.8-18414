using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class CustomBehavior : QuestOrder
    {
        public static string[] File = new string[] { "TalkToAndListenToStory", "InteractWith", "UserSettings", "LoadProfile", "CastSpellOn", "NoCombatMoveTo", "WaitTimer", "Escort", "UserDialog", "FlyTo" };
        public static string[] Attributes = new string[] { "QuestId", "QuestName", "Mobid", "DestinationName", "CollectionDistance", "WaitTime", "SpellId", "NumOfTimes", "InteractByUsingItemId", "AuraIdOnMob", "Range", "IgnoreMobsInBlackspots", "X", "Y", "Z" };
        public static List<Object> Templates = new List<object>();
        public Dictionary<string, string> attributes = new Dictionary<string, string>();
        public string file { get; set; }
        public Dictionary<string, string> AttributeList {
            get { return attributes; }  
        }
                
    }
}
