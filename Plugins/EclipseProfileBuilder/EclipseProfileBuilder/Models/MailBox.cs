using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class MailBox : EclipseGeneric
    {
        public MailBox()
        {
            this.OType = ObjectType.MailBox;
        }
    }
}
