using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class QuestObjective
    {
        public QuestObjective()
        {
            CollectFrom = new List<EclipseGeneric>();
            HotSpots = new List<HotSpot>();
        }
        public string QuestId { get; set; }
        public List<EclipseGeneric> CollectFrom {get;set;}
        public List<HotSpot> HotSpots { get; set; }
        public QuestType Type;

        public enum QuestType
        {
            CollectItem,
            KillMob
        }
    }

}
