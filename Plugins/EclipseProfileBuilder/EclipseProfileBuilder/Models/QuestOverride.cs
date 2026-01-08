using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class QuestOverride
    {
        public QuestOverride()
        {
            Objectives = new List<QuestObjective>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<QuestObjective> Objectives { get; set; }
        public enum Type
        {

        }
    }

}
