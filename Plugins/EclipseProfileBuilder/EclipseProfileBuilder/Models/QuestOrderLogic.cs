using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class QuestOrderLogic:QuestOrder
    {
        
        public string Condition { get; set; }
        public string LogicType { get; set; }
        public bool StartTag { get; set; }
    }
}
