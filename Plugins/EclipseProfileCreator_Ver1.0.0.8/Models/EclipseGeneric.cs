using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class EclipseGeneric
    {
        private string _id = string.Empty;
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
        public string Name { get; set; } //not actually used by honorbuddy - just makes it easier to keep track of
        public string Entry {
            get { return _id; }
            set { _id = value; }
        }
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public  ObjectType OType {get;set;}
        public enum ObjectType { 
            Mob,
            Object, 
            MailBox,
            Item
        }

    }
}
