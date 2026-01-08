using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class Boss : EclipseMob
    {
        public string isFinal  {get;set;}
        public string Optional { get; set; }
        public string KillOrder { get; set; }
        public DBPath Path { get; set; }
    }
}
