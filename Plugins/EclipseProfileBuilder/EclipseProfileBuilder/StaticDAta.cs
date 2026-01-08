using Eclipse.EclipsePlugins.Models;
using Styx.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eclipse
{
    public static class EC
    {
        public static void log(string text)
        {
            Logging.Write("Eclipse=>" + text);
        }
        public static List<QuestOrder> ReflectionFind(string value)
        {
            List<QuestOrder> Results = new List<QuestOrder>();
            foreach (var qo in EclipseProfile.QuestOrders){
                foreach (PropertyInfo prop in typeof(QuestOrder).GetProperties())
                {
                    object propValue = prop.GetValue(qo, null);
                    if (propValue != null)
                    {
                        // Do something with propValue
                        if (propValue.GetType() == typeof(string))
                        {
                            if (propValue.ToString().Contains(value)) Results.Add(qo);
                            continue;
                        }
                    }
                }
            }
            return Results;
        }
    }
}
