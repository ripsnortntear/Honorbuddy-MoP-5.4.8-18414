using ArachnidCreations;
using ArachnidCreations.DevTools;
using Styx.Common;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Color = System.Windows.Media.Color;
namespace Eclipse.WoWDatabase
{
    public class Core
    {
        #region Variables
        public static string DataPath ="EclipseWoWDB.edb";
        public static LocalPlayer Me { get; set; }
        public static WoWUnit Target { get; set; }
        public static bool AddTargetOnly = true;
        public static bool init { get; private set; }
        public static string ilog = string.Empty;
        private static WoWUnit lastTarget = null;
        #endregion

      

       
        internal static void FindDB(){
            var path = Application.StartupPath;
            if (!File.Exists(DataPath))
            {
                var results = Directory.GetFiles(path, "*.edb", SearchOption.AllDirectories).ToList();
                if (results.Count > 0){
                    DataPath = results[0];
                    log(string.Format("--------------------------------Found {0}------------------------------------", results[0]));
                }
            }


        }

        #region Helper Methods
        public static void log(string text)
        {
            
            Logging.Write(Color.FromRgb(144,0,255),"Eclipse=>" + text);
        }
        public static void iLog(string text)
        {
            ilog += (string.Format("Eclipse | {0:MM-dd-yy hh:mm:ss} => {1} \r\n", DateTime.Now, text));
            log(text);
        }
        #endregion
    }
}
