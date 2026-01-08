using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class QuestORItem : QuestObjective
    {
        public string ItemId { get; set; }
        public string CollectCount { get; set; }
    }
}
