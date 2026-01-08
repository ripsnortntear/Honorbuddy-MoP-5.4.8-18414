using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class Blackspot
    {
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
        public string Radius { get; set; }
        public string Name { get; set; } //not actually used by honorbuddy - just makes it easier to keep track of
    }
}
