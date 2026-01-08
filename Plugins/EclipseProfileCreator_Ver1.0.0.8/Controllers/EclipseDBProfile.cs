using Eclipse.EclipsePlugins.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Eclipse.EclipsePlugins.Controllers
{
    public class EclipseDBProfile
    {
        public EclipseDBProfile(string filePath = "")
        {
            PosX = 0.ToString();
            PosY = 0.ToString();
            PosZ = 0.ToString();
            if (filePath != string.Empty && File.Exists(filePath))
            {
                Bosses = new List<Boss>();
                BlackSpots = new List<Blackspot>();
                FileInfo file = new FileInfo(filePath);
                doc = XDocument.Load(filePath);
                FileName = file.Name;
                GetSettings();
                GetBlackspots();
                GetBosses();
            }
        }

        #region StaticProfileData
        public static List<Boss> Bosses = new List<Boss>();
        public static List<Blackspot> BlackSpots = new List<Blackspot>();
        public static string ProfileLoadErrors = string.Empty;
        public static string Filename = "";
        #endregion

        #region InstanceProfileData
        public string Name { get; set; }
        public string DungeonId { get; set; }
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string PosZ { get; set; }
        public string FileName = "";
        public string DungeonName { get; set; }
        private XDocument doc;
        #endregion

        #region ParseFile
        public void GetSettings()
        {
            if (doc.Element("DungeonBuddyProfile").Element("Name") != null) Name = doc.Element("DungeonBuddyProfile").Element("Name").Value;
            if (doc.Element("DungeonBuddyProfile").Element("DungeonId") != null) DungeonId = doc.Element("DungeonBuddyProfile").Element("DungeonId").Value;
        }
        public void GetBlackspots()
        {
            if (doc.Element("DungeonBuddyProfile").Element("Blackspots") != null)
            {
                var nodeTree = doc.Element("DungeonBuddyProfile").Descendants("Blackspots").FirstOrDefault().DescendantNodes();
                if (nodeTree != null)
                {
                    var icount = 0;
                    foreach (XElement ele in nodeTree.Where(i => i.NodeType != System.Xml.XmlNodeType.Comment))
                    {
                        Blackspot bs = new Blackspot { X = ele.Attribute("X").Value, Y = ele.Attribute("Y").Value, Z = ele.Attribute("Z").Value, Radius = ele.Attribute("Radius").Value };
                        if (ele.Attribute("Name") != null) bs.Name = ele.Attribute("Name").Value;
                        else bs.Name = "BlackSpot" + icount;
                        BlackSpots.Add(bs);
                    }
                }
            }
        }
        public void GetBosses()
        {
            if (doc.Element("DungeonBuddyProfile").Element("BossEncounters") != null)
            {
                var nodeTree = doc.Element("DungeonBuddyProfile").Descendants("BossEncounters").FirstOrDefault().Elements();
                if (nodeTree != null)
                {
                    var icount = 0;
                    foreach (XElement ele in nodeTree)
                    {
                        Boss bs = new Boss();
                        if (ele.Attribute("X") != null) bs.X = ele.Attribute("X").Value;
                        if (ele.Attribute("Y") != null) bs.Y = ele.Attribute("Y").Value;
                        if (ele.Attribute("Z") != null) bs.Z = ele.Attribute("Z").Value;
                        if (ele.Attribute("Name") != null) bs.Name = ele.Attribute("Name").Value;
                        if (ele.Attribute("name") != null) bs.Name = ele.Attribute("name").Value;
                        if (ele.Attribute("isFinal") != null) bs.isFinal = ele.Attribute("isFinal").Value;
                        if (ele.Attribute("entry") != null) bs.Entry = ele.Attribute("entry").Value;
                        if (ele.Attribute("Entry") != null) bs.Entry = ele.Attribute("Entry").Value;
                        if (ele.Attribute("killOrder") != null) bs.KillOrder = ele.Attribute("killOrder").Value;
                        if (ele.Attribute("KillOrder") != null) bs.KillOrder = ele.Attribute("KillOrder").Value;
                        if (ele.Attribute("optional") != null) bs.KillOrder = ele.Attribute("optional").Value;
                        if (ele.HasElements)
                        {
                            bs = getBossStuff(bs, ele);
                        }
                        Bosses.Add(bs);
                    }
                }
            }
        }

        private Boss getBossStuff(Boss bs, XElement ele)
        {
            foreach (var d in ele.Elements())
            {
                if (d.Name == "Path")
                {
                    DBPath path = new DBPath();
                    if (d.HasElements)
                    {
                        var iCount = 0;
                        foreach(var spot in d.Elements()){
                            iCount++;
                            HotSpot hs = new HotSpot();
                            if (spot.Attribute("X") != null) hs.X = spot.Attribute("X").Value;
                            if (spot.Attribute("Y") != null) hs.Y = spot.Attribute("Y").Value;
                            if (spot.Attribute("Z") != null) hs.Z = spot.Attribute("Z").Value;
                            if (spot.Attribute("x") != null) hs.X = spot.Attribute("x").Value;
                            if (spot.Attribute("y") != null) hs.Y = spot.Attribute("y").Value;
                            if (spot.Attribute("z") != null) hs.Z = spot.Attribute("z").Value;
                            if (spot.Attribute("Name") == null) hs.Name = "Spot" + iCount;
                            else hs.Name = spot.Attribute("Name").Value;
                            path.HotSpots.Add(hs);
                        }
                    }
                    bs.Path = path;
                }

            }
            return bs;
        }

        #endregion

        #region SaveProfile
        public bool Save(string filename)
        {
            return true;
        }
        #endregion
    }
}
