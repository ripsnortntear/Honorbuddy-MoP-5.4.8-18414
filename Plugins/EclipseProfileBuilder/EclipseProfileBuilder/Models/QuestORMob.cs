using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class QuestORMob : QuestObjective
    {
        public string MobId { get; set; }
        public string KillCount { get; set; }

    }
}
