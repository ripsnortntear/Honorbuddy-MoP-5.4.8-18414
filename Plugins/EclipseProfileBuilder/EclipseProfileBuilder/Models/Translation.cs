using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.EclipsePlugins.Models
{
    public class Translation
    {
        public int id { get; set; }
        public int language { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public int groupid { get; set; } //for many to one relationships (where languages are many)
    }
    public class TranslationControls
    {
        public int Id { get; set; } //GroupId
        public string Name { get; set; } //Control Name
        
    }
}
