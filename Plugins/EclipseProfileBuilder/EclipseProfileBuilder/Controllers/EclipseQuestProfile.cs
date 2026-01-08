using ArachnidCreations;
using ArachnidCreations.DevTools;
using Eclipse.EclipsePlugins.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Eclipse
{
    public class EclipseProfile
    {

        #region StaticProfileData
        public static List<EclipseMob> AvoidMobs = new List<EclipseMob>();
        public static List<Blackspot> BlackSpots = new List<Blackspot>();
        public static List<QuestOverride> QuestOverrides = new List<QuestOverride>();
        public static List<QuestOrder> QuestOrders = new List<QuestOrder>();
        public static List<EclipseVendor> Vendors = new List<EclipseVendor>();
        public static List<MailBox> MailBoxes = new List<MailBox>();
        public static List<CustomBehavior> CustomBehaviors = new List<CustomBehavior>();
        public static List<QuestOrderLogic> LogicBlocks = new List<QuestOrderLogic>();
        public static string ProfileLoadErrors = string.Empty;
        public static string Filename = "";
        #endregion
        
        #region InstanceProfileData
        public string Name { get; set; }
        public string MinLevel { get; set; }
        public string MaxLevel { get; set; }
        public bool SellGrey { get; set; }
        public bool SellWhite { get; set; }
        public bool SellGreen { get; set; }
        public bool SellBlue { get; set; }
        public bool SellPurple { get; set; }
        public bool MailGrey { get; set; }
        public bool MailWhite { get; set; }
        public bool MailGreen { get; set; }
        public bool MailBlue { get; set; }
        public bool MailPurple { get; set; }
        public string MinFreeBagSlots { get; set; }
        public string MinDurability { get; set; }
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string PosZ { get; set; }
        public string FileName = "";
        #endregion
        
        private XDocument doc = new XDocument();     
        public EclipseProfile(string filePath = "")
        {
            PosX = 0.ToString();
            PosY = 0.ToString();
            PosZ = 0.ToString();

            MinLevel = 0.ToString();
            MaxLevel = 0.ToString();

            Name = string.Empty;
            QuestOverrides = new List<QuestOverride>();
            BlackSpots = new List<Blackspot>();
            AvoidMobs = new List<EclipseMob>();
            QuestOrders = new List<QuestOrder>();
            Vendors = new List<EclipseVendor>();
            MailBoxes = new List<MailBox>();
            CustomBehaviors = new List<CustomBehavior>();
            LogicBlocks = new List<QuestOrderLogic>();
            QuestOverrides = new List<QuestOverride>();


            if (filePath != string.Empty && File.Exists(filePath))
            {
                FileInfo file = new FileInfo(filePath);
                doc = XDocument.Load(filePath);
                FileName = file.Name;
                GetSettings();
                GetBlackspots();
                GetAvoids();
                GetQuestOverrides();
                GetQuestOrder();
                GetVendors();
                GetMailBoxes();

            }
        }
        public bool UpdateNodebyName(string name, string value)
        {
            doc.Element("HBProfile").SetElementValue(name, value);
            return true;
        }
        public bool Save(string filename)
        {
            XDocument newDoc = new XDocument(new XElement("HBProfile")) { Declaration = new XDeclaration("1.0", "utf-16","yes") };
            newDoc.Element("HBProfile").Add(new XElement("Name") { Value = Name.Replace(".xml", "") });
            newDoc.Element("HBProfile").Add(new XElement("MinDurability") { Value = MinDurability });
            newDoc.Element("HBProfile").Add(new XElement("MinFreeBagSlots") { Value = MinFreeBagSlots });
            newDoc.Element("HBProfile").Add(new XElement("MinLevel") { Value = MinLevel });
            newDoc.Element("HBProfile").Add(new XElement("MaxLevel") { Value = MaxLevel });
            newDoc.Element("HBProfile").Add(new XElement("SellGrey") { Value = SellGrey.ToString() });
            newDoc.Element("HBProfile").Add(new XElement("SellWhite") { Value = SellWhite.ToString() });
            newDoc.Element("HBProfile").Add(new XElement("SellGreen") { Value = SellGreen.ToString() });
            newDoc.Element("HBProfile").Add(new XElement("SellBlue") { Value = SellBlue.ToString() });
            newDoc.Element("HBProfile").Add(new XElement("SellPurple") { Value = SellPurple.ToString() });
            newDoc.Element("HBProfile").Add(new XElement("MailGrey") { Value = MailGrey.ToString() });
            newDoc.Element("HBProfile").Add(new XElement("MailWhite") { Value = MailWhite.ToString() });
            newDoc.Element("HBProfile").Add(new XElement("MailGreen") { Value = MailGreen.ToString() });
            newDoc.Element("HBProfile").Add(new XElement("MailBlue") { Value = MailBlue.ToString() });
            newDoc.Element("HBProfile").Add(new XElement("MailPurple") { Value = MailPurple.ToString() });

            var avoidContainer = new XElement("AvoidMobs");
            var testtable = DAL.getTableStructure("AvoidMobs");
            if (testtable == null) DAL.ExecuteSL3Query(ORM.generateCreateSQL(new EclipseMob(), "AvoidMobs", ""), true);
            foreach (var avoid in AvoidMobs)
            {
                avoidContainer.Add(new XElement("Mob", new XAttribute("Name",avoid.Name), new XAttribute("Entry", avoid.Entry) ));
                DAL.ExecuteSL3Query(ORM.InsertOrUpdate(avoid, "AvoidMobs", "", false, DAL.getTableStructure("AvoidMobs")));
            }
            newDoc.Element("HBProfile").Add(avoidContainer);
            var bsContainer = new XElement("Blackspots");
            testtable = DAL.getTableStructure("Blackspots");
            if (testtable == null) DAL.ExecuteSL3Query(ORM.generateCreateSQL(new Blackspot(), "Blackspots", ""), true);
            foreach (var bs in BlackSpots)
            {
                //bsContainer.Add(new XComment(string.Format("BlackSpot: {0}", bs.Name)));
                bsContainer.Add(new XElement("Blackspot", 
                    new XAttribute("Name", bs.Name), 
                    new XAttribute("Radius", bs.Radius),
                    new XAttribute("X", bs.X.Replace(",", ".")),
                    new XAttribute("Y", bs.Y.Replace(",", ".")),
                    new XAttribute("Z", bs.Z.Replace(",", "."))
                ));
                DAL.ExecuteSL3Query(ORM.InsertOrUpdate(bs, "Blackspots", "", false, DAL.getTableStructure("Blackspots")));
                
            }
            newDoc.Element("HBProfile").Add(bsContainer);
            var vendorContainer = new XElement("Vendors");
            testtable = DAL.getTableStructure("Vendors");
            if (testtable == null) DAL.ExecuteSL3Query(ORM.generateCreateSQL(new EclipseVendor(), "Vendors", ""), true);
            foreach (var bs in Vendors)
            {
                vendorContainer.Add(new XElement("Vendor",
                    new XAttribute("Name", bs.Name),
                    new XAttribute("Type", bs.Type),
                    new XAttribute("Entry", bs.Id),
                    new XAttribute("X", bs.X.Replace(",", ".")),
                    new XAttribute("Y", bs.Y.Replace(",", ".")),
                    new XAttribute("Z", bs.Z.Replace(",", "."))
                ));
                DAL.ExecuteSL3Query(ORM.InsertOrUpdate(bs, "Vendors", "", false, DAL.getTableStructure("Vendors")));
            }
            newDoc.Element("HBProfile").Add(vendorContainer);
            testtable = DAL.getTableStructure("QuestOverrides");
            if (testtable == null) DAL.ExecuteSL3Query(ORM.generateCreateSQL(new QuestOverride(), "QuestOverrides", ""), true);
            foreach (var qo in QuestOverrides)
            {
                var questContainer = new XElement("Quest",
                    new XAttribute("Name", qo.Name),
                    new XAttribute("Id", qo.Id));
                foreach (var obj in qo.Objectives)
                {

                    var objectiveContainer = new XElement("Objective");
                    if (obj.Type == QuestObjective.QuestType.KillMob)
                    {
                        var typedobj = (QuestORMob)obj;
                        objectiveContainer.SetAttributeValue("MobId", typedobj.MobId);
                        objectiveContainer.SetAttributeValue("Type", typedobj.Type);
                        objectiveContainer.SetAttributeValue("KillCount", typedobj.KillCount);
                        
                    }
                    if (obj.Type == QuestObjective.QuestType.CollectItem)
                    {
                        var typedobj = (QuestORItem)obj;
                        objectiveContainer.SetAttributeValue("ItemId", typedobj.ItemId);
                        objectiveContainer.SetAttributeValue("Type", typedobj.Type);
                        objectiveContainer.SetAttributeValue("CollectCount", typedobj.CollectCount);

                    }
                    if (obj.HotSpots.Count > 0)
                    {
                        var hotspotContainer = new XElement("Hotspots");
                        foreach (var hs in obj.HotSpots)
                        {
                            hotspotContainer.Add(new XElement("Hotspot",
                                new XAttribute("X", hs.X.Replace(",", ".")),
                                new XAttribute("Y", hs.Y.Replace(",", ".")),
                                new XAttribute("Z", hs.Z.Replace(",", "."))
                            ));
                        }
                        objectiveContainer.Add(hotspotContainer);
                    }
                    
                    if (obj.CollectFrom.Count > 0)
                    {
                        var CollectFromContainer = new XElement("CollectFrom");
                        foreach (var cf in obj.CollectFrom)
                        {
                            var ele = new XElement("GameObject");
                            if (cf.Name != null) ele.SetAttributeValue("Name", cf.Name);
                            if (cf.Id != null) ele.SetAttributeValue("Id", cf.Id);
                            CollectFromContainer.Add(ele);
                        }
                        objectiveContainer.Add(CollectFromContainer);
                    }
                    questContainer.Add(objectiveContainer);
                }
                newDoc.Element("HBProfile").Add(questContainer);
            }
            var questOrderContainer = new XElement("QuestOrder");

            List<XElement> Level = new List<XElement>();
            Level.Add(questOrderContainer);
            testtable = DAL.getTableStructure("QuestOrders");
            if (testtable == null) DAL.ExecuteSL3Query(ORM.generateCreateSQL(new QuestOrder(), "QuestOrders", ""), true);
            var dt = DAL.getTableStructure("QuestOrders");
            var count = 0;
            foreach (var qo in QuestOrders)
            {
                count++;
                int level = Level.Count();
                if (qo.type == QuestOrder.QOType.LogicBlock)
                {
                    var lb = (QuestOrderLogic)qo;
                    if (lb.StartTag)
                    {
                        var ifele = new XElement(lb.LogicType,
                            new XAttribute("Condition", lb.Condition)
                        );
                        Level.Add(ifele);
                    }
                    else
                    {
                        var oldparent = Level.Last();
                        Level.Remove(oldparent);
                        Level.Last().Add(oldparent);
                    }
                    DAL.ExecuteSL3Query(ORM.InsertOrUpdate(qo, "QuestOrders", "", false, dt));

                }
                if (qo.type == QuestOrder.QOType.PickUp)
                {
                    Level.Last().Add(new XElement("PickUp",
                        new XAttribute("QuestName", qo.QuestName),
                        new XAttribute("QuestId", qo.QuestId),
                        new XAttribute("GiverId", qo.GiverId),
                        new XAttribute("GiverName", qo.GiverName)
                    ));
                    DAL.ExecuteSL3Query(ORM.InsertOrUpdate(qo, "QuestOrders", "", false, dt));
                }
                if (qo.type == QuestOrder.QOType.TurnIn)
                {
                    var ele = new XElement("TurnIn");
                    if (qo.QuestId != null) ele.SetAttributeValue("QuestId", qo.QuestId);
                    if (qo.TurnInId != null) ele.SetAttributeValue("TurnInId", qo.TurnInId);
                    if (qo.TurnInName != null) ele.SetAttributeValue("TurnInName", qo.TurnInName);
                    if (qo.QuestName != null) ele.SetAttributeValue("QuestName", qo.QuestName);
                    Level.Last().Add(ele);
                    DAL.ExecuteSL3Query(ORM.InsertOrUpdate(qo, "QuestOrders", "", false, dt));
                }
                if (qo.type == QuestOrder.QOType.UseItem)
                {
                    if (qo.ItemName == null) qo.ItemName = qo.ItemId;
                    if (qo.NumOfTimes == null) qo.NumOfTimes = "1";

                    Level.Last().Add(new XElement("UseItem",
                        new XAttribute("QuestName", qo.QuestName),
                        new XAttribute("QuestId", qo.QuestId),
                        new XAttribute("ItemId", qo.ItemId),
                        new XAttribute("ItemName", qo.ItemName),
                        new XAttribute("NumOfTimes", qo.NumOfTimes),
                        new XAttribute("X", qo.X),
                        new XAttribute("Y", qo.Y),
                        new XAttribute("Z", qo.Z)

                    ));
                    DAL.ExecuteSL3Query(ORM.InsertOrUpdate(qo, "QuestOrders", "", false, dt));
                }
                if (qo.type == QuestOrder.QOType.CustomBehavior)
                {
                    var custom = new XElement("CustomBehavior");
                    var cb = (CustomBehavior)qo;
                    custom.SetAttributeValue("File", cb.file);
                    foreach (var item in ((CustomBehavior)qo).attributes)
                    {
                        custom.SetAttributeValue(item.Key, item.Value);
                    }
                    Level.Last().Add(custom);
                    DAL.ExecuteSL3Query(ORM.InsertOrUpdate(qo, "QuestOrders", "", false, dt));
                }
                if (qo.type == QuestOrder.QOType.MoveTo)
                {
                    Level.Last().Add(new XElement("MoveTo",
                        new XAttribute("X", qo.X),
                        new XAttribute("Y", qo.Y),
                        new XAttribute("Z", qo.Z)
                    ));
                    DAL.ExecuteSL3Query(ORM.InsertOrUpdate(qo, "QuestOrders", "", false, dt));
                }
                if (qo.type == QuestOrder.QOType.RunTo)
                {
                    Level.Last().Add(new XElement("RunTo",
                        new XAttribute("X", qo.X),
                        new XAttribute("Y", qo.Y),
                        new XAttribute("Z", qo.Z)
                    ));
                    DAL.ExecuteSL3Query(ORM.InsertOrUpdate(qo, "QuestOrders", "", false, dt));
                }
                if (qo.type == QuestOrder.QOType.FlyTo)
                {
                    Level.Last().Add(new XElement("FlyTo",
                        new XAttribute("QuestName", qo.QuestName),
                        new XAttribute("QuestId", qo.QuestId),
                        new XAttribute("X", qo.X),
                        new XAttribute("Y", qo.Y),
                        new XAttribute("Z", qo.Z)
                    ));
                    DAL.ExecuteSL3Query(ORM.InsertOrUpdate(qo, "QuestOrders", "", false,dt));
                }

                if (qo.type == QuestOrder.QOType.Objective)
                {
                    if (qo.QuestName == null) qo.QuestName = qo.QuestId.ToString();
                    var objective = new XElement("Objective",
                        new XAttribute("QuestName", qo.QuestName),
                        new XAttribute("QuestId", qo.QuestId),
                        new XAttribute("Type", qo.objectiveType)
                    );
                    if (qo.objectiveType == QuestObjective.QuestType.KillMob)
                    {
                        objective.SetAttributeValue("KillCount", qo.KillCount);
                        objective.SetAttributeValue("MobId", qo.MobId);
                        objective.SetAttributeValue("MobName", qo.MobName);
                    }
                    if (qo.objectiveType == QuestObjective.QuestType.CollectItem)
                    {
                        objective.SetAttributeValue("ItemName", qo.ItemName);
                        objective.SetAttributeValue("ItemId", qo.ItemId);
                        objective.SetAttributeValue("CollectCount", qo.CollectCount);
                    }
                   Level.Last().Add(objective);
                   DAL.ExecuteSL3Query(ORM.InsertOrUpdate(qo, "QuestOrders", "", false, dt));
                }

            }
            //cleanup the ones with no questid
            DAL.ExecuteSL3Query("delete from questorders where id not in (select id from questorders where length(questid) > 0)");
            newDoc.Element("HBProfile").Add(questOrderContainer);
            if (filename.Substring(filename.Length-4,4) == ".xml")  newDoc.Save(filename);
            else newDoc.Save(filename + ".xml");
            return true;
        }
        
        #region ParseProfileData
        private void GetSettings()
        {
            if (doc.Element("HBProfile").Descendants("Name").FirstOrDefault() != null) Name = doc.Element("HBProfile").Descendants("Name").FirstOrDefault().Value;
            if (doc.Element("HBProfile").Descendants("MinLevel").FirstOrDefault() != null) MinLevel = doc.Element("HBProfile").Descendants("MinLevel").FirstOrDefault().Value;
            if (doc.Element("HBProfile").Descendants("MaxLevel").FirstOrDefault() != null) MaxLevel = doc.Element("HBProfile").Descendants("MaxLevel").FirstOrDefault().Value;
            if (doc.Element("HBProfile").Descendants("MinFreeBagSlots").FirstOrDefault() != null) MinFreeBagSlots = doc.Element("HBProfile").Descendants("MinFreeBagSlots").FirstOrDefault().Value;
            if (doc.Element("HBProfile").Descendants("MinDurability").FirstOrDefault() != null) MinDurability = doc.Element("HBProfile").Descendants("MinDurability").FirstOrDefault().Value;

            if (doc.Element("HBProfile").Descendants("SellGrey").FirstOrDefault() != null) SellGrey = bool.Parse(doc.Element("HBProfile").Descendants("SellGrey").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("SellWhite").FirstOrDefault() != null) SellWhite = bool.Parse(doc.Element("HBProfile").Descendants("SellWhite").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("SellGreen").FirstOrDefault() != null) SellGreen = bool.Parse(doc.Element("HBProfile").Descendants("SellGreen").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("SellBlue").FirstOrDefault() != null) SellBlue = bool.Parse(doc.Element("HBProfile").Descendants("SellBlue").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("SellPurple").FirstOrDefault() != null) SellPurple = bool.Parse(doc.Element("HBProfile").Descendants("SellPurple").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailGrey").FirstOrDefault() != null) MailGrey = bool.Parse(doc.Element("HBProfile").Descendants("MailGrey").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailWhite").FirstOrDefault() != null) MailWhite = bool.Parse(doc.Element("HBProfile").Descendants("MailWhite").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailGreen").FirstOrDefault() != null) MailGreen = bool.Parse(doc.Element("HBProfile").Descendants("MailGreen").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailBlue").FirstOrDefault() != null) MailBlue = bool.Parse(doc.Element("HBProfile").Descendants("MailBlue").FirstOrDefault().Value);
            if (doc.Element("HBProfile").Descendants("MailPurple").FirstOrDefault() != null) MailPurple = bool.Parse(doc.Element("HBProfile").Descendants("MailPurple").FirstOrDefault().Value);
        }
        private void GetMailBoxes()
        {
            if (doc.Element("HBProfile").Element("Mailboxes") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("Mailboxes").FirstOrDefault().DescendantNodes();
                if (nodeTree != null)
                {
                    var icount = 0;
                    foreach (XElement ele in nodeTree.Where(i => i.NodeType != System.Xml.XmlNodeType.Comment))
                    {
                        MailBox mb = new MailBox { X = ele.Attribute("X").Value, Y = ele.Attribute("Y").Value, Z = ele.Attribute("Z").Value};
                        if (ele.Attribute("Name") != null) mb.Name = ele.Attribute("Name").Value;
                        else mb.Name = "MailBox" + icount;
                        MailBoxes.Add(mb);
                    }
                }
            }
        }
        public void GetBlackspots()
        {
            if (doc.Element("HBProfile").Element("Blackspots") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("Blackspots").FirstOrDefault().DescendantNodes();
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
        public void GetVendors()
        {
            if (doc.Element("HBProfile").Element("Vendors") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("Vendors").FirstOrDefault().DescendantNodes();
                if (nodeTree != null)
                {
                    foreach (XElement ele in nodeTree.Where(i => i.NodeType != System.Xml.XmlNodeType.Comment))
                    {
                        EclipseVendor vendor = new EclipseVendor { X = ele.Attribute("X").Value.Replace(",", "."), Y = ele.Attribute("Y").Value.Replace(",", "."), Z = ele.Attribute("Z").Value.Replace(",", "."), Type = ele.Attribute("Type").Value, Name = ele.Attribute("Name").Value };
                        Vendors.Add(vendor);
                    }
                }
            }
        }
        public void GetQuestOverrides()
        {
            if (doc.Element("HBProfile").Element("Quest") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("Quest").ToList();
                if (nodeTree != null)
                {
                    foreach (XElement ele in nodeTree)
                    {
                        QuestOverride questOR = new QuestOverride { Id=int.Parse(ele.Attribute("Id").Value) };
                        if (ele.Attribute("Name") != null) questOR.Name = ele.Attribute("Name").Value;
                        if (ele.Attribute("NName") != null) questOR.Name = ele.Attribute("NName").Value;
                        


                        if (ele.Descendants("Objectives") != null)
                        {
                            var obj = ele.Descendants("Objective").FirstOrDefault();
                            if (obj == null) continue;
                            var type = "";
                            QuestObjective.QuestType qtype = QuestObjective.QuestType.CollectItem;
                            if (obj.Attribute("Type").Value != null) type = obj.Attribute("Type").Value;
                            if (type == "CollectItem") qtype = QuestObjective.QuestType.CollectItem;
                            if (type == "KillMob") qtype = QuestObjective.QuestType.KillMob;
                            


                            if (qtype == QuestObjective.QuestType.CollectItem)
                            {
                                QuestORItem qo = new QuestORItem { Type = qtype };

                                if (obj.Attribute("ObjectId") != null) qo.ItemId = obj.Attribute("ObjectId").Value;
                                if (obj.Attribute("ItemId") != null) qo.ItemId = obj.Attribute("ItemId").Value;
                                if (obj.Attribute("MobId") != null) qo.ItemId = obj.Attribute("MobId").Value;
                                if (obj.Attribute("CollectCount") != null) qo.CollectCount = obj.Attribute("CollectCount").Value;
                                if (obj.Attribute("CollectionCount") != null) qo.CollectCount = obj.Attribute("CollectionCount").Value;
                                qo.HotSpots.AddRange(getHotSpots(obj));
                                qo.CollectFrom.AddRange(getCollectFroms(obj));
                                questOR.Objectives.Add(qo);
                            }
                            if (qtype == QuestObjective.QuestType.KillMob)
                            {
                                
                                QuestORMob qo = new QuestORMob { Type = qtype };
                                
                                //This is because of inconsistency in the profiles.
                                if (obj.Attribute("MobId") == null && obj.Attribute("ItemId") != null) qo.MobId = obj.Attribute("ItemId").Value;
                                if (obj.Attribute("MobId") != null) qo.KillCount = obj.Attribute("MobId").Value;

                                if (obj.Attribute("KillCount") == null && obj.Attribute("CollectCount") != null) qo.KillCount = obj.Attribute("CollectCount").Value;
                                if (obj.Attribute("KillCount") != null) qo.KillCount = obj.Attribute("KillCount").Value;

                                qo.HotSpots.AddRange(getHotSpots(obj));
                                qo.CollectFrom.AddRange(getCollectFroms(obj));
                                questOR.Objectives.Add(qo);
                            }
                        }
                        QuestOverrides.Add(questOR);
                    }
                }
            }
        }
        private IEnumerable<HotSpot> getHotSpots(XElement obj)
        {
            var ele = obj.Descendants().ToList();
            List<HotSpot> spots = new List<HotSpot>();
            if (ele != null)
            {
                int iCount = 0;
                
                foreach (var spot in ele.Where(e=>e.Name == "Hotspot").ToList())
                {
                    iCount++;
                    HotSpot hs = new HotSpot ();
                    if (spot.Attribute("X") != null) hs.X = spot.Attribute("X").Value;
                    if (spot.Attribute("Y") != null) hs.Y = spot.Attribute("Y").Value;
                    if (spot.Attribute("Z") != null) hs.Z = spot.Attribute("Z").Value;
                    if (spot.Attribute("x") != null) hs.X = spot.Attribute("x").Value;
                    if (spot.Attribute("y") != null) hs.Y = spot.Attribute("y").Value;
                    if (spot.Attribute("z") != null) hs.Z = spot.Attribute("z").Value;
                    if (spot.Attribute("Name") == null) hs.Name = "Spot" + iCount;
                    else hs.Name = spot.Attribute("Name").Value;
                    spots.Add(hs);
                }
            }
            return spots;            
        }
        private IEnumerable<EclipseGeneric> getCollectFroms(XElement obj)
        {
            var ele = obj.Descendants().ToList();
            List<EclipseGeneric> collect = new List<EclipseGeneric>();
            if (ele != null)
            {
                foreach (var item in ele.Where(e => e.Name == "GameObject" || e.Name == "Mob").ToList())
                {
                    if (item.Name == "GameObject")
                    {
                        EclipseObject eo = new EclipseObject ();
                        if (item.Attribute("Name") != null) eo.Name = item.Attribute("Name").Value;
                        if (item.Attribute("ID") != null) eo.Id = item.Attribute("ID").Value;
                        if (item.Attribute("Id") != null) eo.Id = item.Attribute("Id").Value;
                        collect.Add(eo);
                    }
                    if (item.Name == "Mob")
                    {
                        EclipseMob eo = new EclipseMob { };
                        if (item.Attribute("Id") != null) eo.Id = item.Attribute("Id").Value;
                        if (item.Attribute("Entry") != null) eo.Id = item.Attribute("Entry").Value;
                        if (item.Attribute("Name") != null) eo.Name = item.Attribute("Name").Value;
                        collect.Add(eo);
                    }
                }
            }
            return collect;
        }
        public void GetAvoids(){
            if (doc.Element("HBProfile").Element("AvoidMobs") != null)
            {
                var nodeTree = doc.Element("HBProfile").Descendants("AvoidMobs").FirstOrDefault().DescendantNodes();
                if (nodeTree != null)
                {
                    foreach (XElement ele in nodeTree.Where(i => i.NodeType != System.Xml.XmlNodeType.Comment))
                    {
                        EclipseMob mob = new EclipseMob { Name = ele.FirstAttribute.Value, Entry = ele.LastAttribute.Value };
                        AvoidMobs.Add(mob);
                    }
                }
            }
        }
        public void GetQuestOrder()
        {

            var questorderItems = doc.Element("HBProfile").Element("QuestOrder");
            if (questorderItems != null)
            {
                if (questorderItems.HasElements) GetQuestOrderDescendants(questorderItems);
            }
        }
        public void GetQuestOrderDescendants(XElement root, uint qid = 0){
            var desc = root.Elements().ToList();
            foreach (var n in desc)
            {
                var i  = (XElement)n;
                if (i.Name == "PickUp")
                {
                    QuestOrder qo = new QuestOrder { QuestName = i.Attribute("QuestName").Value, QuestId =  uint.Parse(i.Attribute("QuestId").Value), GiverId = i.Attribute("GiverId").Value, type = QuestOrder.QOType.PickUp };
                    if (i.Attribute("GiverName") != null) qo.GiverName = i.Attribute("GiverName").Value;
                    if (qo.QuestId == 0) qo.QuestId = qid;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "UseItem")
                {
                    QuestOrder qo = new QuestOrder { QuestId =  uint.Parse(i.Attribute("QuestId").Value), ItemId = i.Attribute("ItemId").Value,  X = i.Attribute("X").Value, Y = i.Attribute("Y").Value, Z = i.Attribute("Z").Value, type = QuestOrder.QOType.UseItem };
                    if (i.Attribute("QuestName") != null) qo.QuestName = i.Attribute("QuestName").Value;
                    if (i.Attribute("QuestId") != null) qo.QuestName = i.Attribute("QuestId").Value;
                    if (i.Attribute("NumOfTimes") != null) qo.QuestName = i.Attribute("NumOfTimes").Value;
                    if (qo.QuestId == 0) qo.QuestId = qid;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "RunTo")
                {
                    QuestOrder qo = new QuestOrder { type = QuestOrder.QOType.RunTo };
                    if (i.Attribute("QuestId") != null) qo.QuestName = i.Attribute("QuestId").Value;
                    if (i.Attribute("QuestName") != null) qo.QuestName = i.Attribute("QuestName").Value;
                    if (i.Attribute("DestName") != null) qo.QuestName = i.Attribute("DestName").Value;
                    if (i.Attribute("X") != null) qo.X = i.Attribute("X").Value;
                    if (i.Attribute("Y") != null) qo.Y = i.Attribute("Y").Value;
                    if (i.Attribute("Z") != null) qo.Z = i.Attribute("Z").Value;
                    if (i.Attribute("x") != null) qo.X = i.Attribute("x").Value;
                    if (i.Attribute("y") != null) qo.Y = i.Attribute("y").Value;
                    if (i.Attribute("z") != null) qo.Z = i.Attribute("z").Value;
                    if (qo.QuestId == 0) qo.QuestId = qid;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "MoveTo")
                {
                    QuestOrder qo = new QuestOrder { type = QuestOrder.QOType.MoveTo };
                    if (i.Attribute("QuestId") != null) qo.QuestName = i.Attribute("QuestId").Value;
                    if (i.Attribute("QuestName") != null) qo.QuestName = i.Attribute("QuestName").Value;
                    if (i.Attribute("DestName") != null) qo.QuestName = i.Attribute("DestName").Value;
                    if (i.Attribute("X") != null) qo.X = i.Attribute("X").Value;
                    if (i.Attribute("Y") != null) qo.Y = i.Attribute("Y").Value;
                    if (i.Attribute("Z") != null) qo.Z = i.Attribute("Z").Value;
                    if (i.Attribute("x") != null) qo.X = i.Attribute("x").Value;
                    if (i.Attribute("y") != null) qo.Y = i.Attribute("y").Value;
                    if (i.Attribute("z") != null) qo.Z = i.Attribute("z").Value;
                    if (qo.QuestId == 0) qo.QuestId = qid;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "FlyTo")
                {
                    QuestOrder qo = new QuestOrder { QuestName = i.Attribute("QuestName").Value, QuestId =  uint.Parse(i.Attribute("QuestId").Value), X = i.Attribute("X").Value, Y = i.Attribute("Y").Value, Z = i.Attribute("Z").Value, type = QuestOrder.QOType.RunTo };
                    if (qo.QuestId == 0) qo.QuestId = qid;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "CustomBehavior")
                {

                    CustomBehavior cb = new CustomBehavior { type = QuestOrder.QOType.CustomBehavior };
                    foreach (var att in i.Attributes().ToList())
                    {
                        if (att.Name != "File") cb.attributes.Add(att.Name.ToString(), att.Value.ToString());
                        else cb.file = att.Value.ToString();
                    }
                    CustomBehaviors.Add(cb);
                    QuestOrders.Add(cb);
                }
                if (i.Name == "Objective")
                {
                    var val = i.Attribute("Type").Value;
                    if (val == "Collect") val = "CollectItem";
                    if (val == "CollectItem") val = "CollectItem";
                    if (val == "UseItem" || val == "UseObject") continue;
                    QuestObjective.QuestType objectiveType = QuestObjective.QuestType.CollectItem;
                    Enum.TryParse(val, true, out objectiveType);
                    QuestOrder qo = new QuestOrder { objectiveType = objectiveType, type = QuestOrder.QOType.Objective };
                    if (i.Attribute("Questid") != null) qo.QuestId = uint.Parse(i.Attribute("Questid").Value);
                    if (i.Attribute("QuestId") != null) qo.QuestId =  uint.Parse(i.Attribute("QuestId").Value);
                    if (i.Attribute("QuestName") != null) qo.QuestName = i.Attribute("QuestName").Value;
                    if (objectiveType == QuestObjective.QuestType.CollectItem)
                    {
                        qo.ItemId = i.Attribute("ItemId").Value;
                        qo.CollectCount = i.Attribute("CollectCount").Value;
                    }
                    if (objectiveType == QuestObjective.QuestType.KillMob)
                    {
                        qo.MobId = i.Attribute("MobId").Value;
                        qo.KillCount = i.Attribute("KillCount").Value;
                    }
                    if (qo.QuestId == 0) qo.QuestId = qid;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "TurnIn")
                {
                    QuestOrder qo = new QuestOrder { type = QuestOrder.QOType.TurnIn };
                    if (i.Attribute("QuestName") != null) qo.QuestName = i.Attribute("QuestName").Value;
                    if (i.Attribute("QuestId") != null) qo.QuestId =  uint.Parse(i.Attribute("QuestId").Value);
                    if (i.Attribute("TurnInId") != null) qo.TurnInId = i.Attribute("TurnInId").Value;
                    if (i.Attribute("TurnInId") == null) ProfileLoadErrors += string.Format("Missing TurnInId: {0} \r\n", i.Value);
                    if (i.Attribute("TurnInId") != null && i.Attribute("TurnInId").Value == "") ProfileLoadErrors += string.Format("Missing TurnInId: {0} \r\n", i.Value);
                    if (i.Attribute("TurnInName") != null) qo.TurnInName = i.Attribute("TurnInName").Value;
                    if (qo.QuestId == 0) qo.QuestId = qid;
                    QuestOrders.Add(qo);
                }
                if (i.Name == "If" || i.Name == "While" || i.Name == "ElseIf")
                {
                    QuestOrderLogic qo = new QuestOrderLogic { type = QuestOrder.QOType.LogicBlock, LogicType = i.Name.ToString(), StartTag = true };
                    if (i.Attribute("Condition") != null) qo.Condition = i.Attribute("Condition").Value;
                    if (qo.Condition.Contains("HasQuest"))
                    {
                        var preframe = qo.Condition.Substring(0, qo.Condition.IndexOf(")")).Replace("HasQuest", "").Replace("(", "").Replace(")", "").Replace("!", "");
                        uint quid = 0;
                        uint.TryParse(preframe, out quid);
                        qo.QuestId = quid;
                    }
                    QuestOrders.Add(qo);
                    if (i.HasElements) GetQuestOrderDescendants(i, qo.QuestId);
                    qo = new QuestOrderLogic { type = QuestOrder.QOType.LogicBlock, LogicType = i.Name.ToString(), StartTag = false };
                    if (i.Attribute("Condition") != null) qo.Condition = i.Attribute("Condition").Value;
                    if (qo.QuestId == 0) qo.QuestId = qid;
                    EclipseProfile.LogicBlocks.Add(qo);
                }

            }
        }
        #endregion

    }
}
