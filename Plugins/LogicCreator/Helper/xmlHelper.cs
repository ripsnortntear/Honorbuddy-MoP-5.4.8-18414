using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using Styx;
using Styx.CommonBot.Database;
using System.IO;

namespace PokehbuddyLogicCreator
{
    class XmlHelper
    {
        #region StableMaster

        //// <?xml version="1.0" encoding="utf-8"?>
        //// <PF_Stablemaster>
        ////   <EasternKindoms Name="Jenova Steinschild" Entry="11069" ZoneID="1519" Fraction="Alliance" />
        //// </PF_Stablemaster>

        //public static string StableMaster_Patch = Application.StartupPath + "\\Settings\\PetFighter\\Stablemaster.xml";

        //public static void StableMaster_Remove(string Entry, string MapName)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(StableMaster_Patch);
        //    XmlNode myNode = doc.SelectSingleNode("/PF_Stablemaster/" + MapName + "[@Entry='" + Entry + "']");
        //    myNode.ParentNode.RemoveChild(myNode);
        //    doc.Save(StableMaster_Patch);
        //}

        //public static void StableMaster_Add(string Name, string Entry, string MapId, string ZoneId)
        //{            
        //    StableMaster_Add(Name, Entry, StableMaster_getMapName(MapId), ZoneId, StableMaster_getFraction);
        //}

        //public static void StableMaster_Add(string Name, string Entry, string MapName, string ZoneId, string Fraction)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    XmlNode myRoot, myNode;

        //    doc.Load(StableMaster_Patch);

        //    myRoot = doc.SelectSingleNode("/PF_Stablemaster");
        //    myNode = doc.CreateElement(MapName);
        //    myNode.Attributes.Append(doc.CreateAttribute("Name")).InnerText = Name;
        //    myNode.Attributes.Append(doc.CreateAttribute("Entry")).InnerText = Entry;
        //    myNode.Attributes.Append(doc.CreateAttribute("ZoneID")).InnerText = ZoneId;
        //    myNode.Attributes.Append(doc.CreateAttribute("Fraction")).InnerText = Fraction;
        //    myRoot.AppendChild(myNode);

        //    doc.Save(StableMaster_Patch);
        //}

        //public static List<string> StableMaster_getList(string MapName)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(StableMaster_Patch);
        //    XmlNodeList root = doc.GetElementsByTagName(MapName);

            
        //    List<string> lstring = new List<string>();
        //    foreach (XmlNode _node in root)
        //    {
        //        //if (string.IsNullOrEmpty(@Blackspot.Attributes["Blackspot"].InnerText))
        //        //    ls.Add("NULL");

        //        string txt = string.Format("Name=\"{0}\" Entry=\"{1}\" ZoneID=\"{2}\" Fraction=\"{3}\"", _node.Attributes[0].InnerText,
        //            _node.Attributes[1].InnerText, _node.Attributes[2].InnerText, _node.Attributes[3].InnerText);
        //        lstring.Add(txt);
        //    }


        //    return lstring;
        //}

        //private static string StableMaster_getFraction
        //{ get { if (StyxWoW.Me.IsAlliance) return "Alliance"; else /*if (StyxWoW.Me.IsHorde)*/ return "Horde"; /*else return "Neutral";*/ } }

        //public static string StableMaster_getMapName(string MapId)
        //{
        //    switch (MapId)
        //    {
        //        case ("0"):
        //            return "EasternKindoms";
        //        case ("1"):
        //            return "Kalimbor";
        //        case ("530"):
        //            return "Outland";
        //        case ("571"):
        //            return "Northrend";
        //        case ("870"):
        //            return "Pandaren";
        //        default:
        //            break;
        //    }
        //    return null;
        //}

        //public static NpcResult StableMaster_getNextStablemaster
        //{
        //    get
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(StableMaster_Patch);
        //        XmlNode myNode = doc.SelectSingleNode("/PF_Stablemaster/" + StableMaster_getMapName(StyxWoW.Me.MapId.ToString()) + "[@ZoneID=" + StyxWoW.Me.ZoneId.ToString() + "]");

        //        if (myNode != null && myNode.Attributes[3].InnerText == StableMaster_getFraction)
        //        {
        //            return NpcQueries.GetNpcById(Convert.ToUInt32(myNode.Attributes[1].InnerText));
        //        }
        //        XmlNodeList root = doc.GetElementsByTagName(StableMaster_getMapName(StyxWoW.Me.MapId.ToString()));

        //        NpcResult _point = null;
        //        foreach (XmlNode _node in root)
        //        {
        //            if (/*StyxWoW.Me.Location.Distance(NpcQueries.GetNpcById(Convert.ToUInt32(_node.Attributes[1].InnerText)).Location) > 2500 ||*/ _node.Attributes[3].InnerText != StableMaster_getFraction)
        //                continue;

        //            if (_point == null)
        //            { _point = NpcQueries.GetNpcById(Convert.ToUInt32(_node.Attributes[1].InnerText)); continue; }

        //            if (StyxWoW.Me.Location.Distance(_point.Location) > StyxWoW.Me.Location.Distance(NpcQueries.GetNpcById(Convert.ToUInt32(_node.Attributes[1].InnerText)).Location))
        //            {
        //                _point = NpcQueries.GetNpcById(Convert.ToUInt32(_node.Attributes[1].InnerText));
        //            }
        //        }
        //        return _point;
        //    }
        //}

        //public static void StableMaster_Import(string patch)
        //{
        //    XmlDocument doc_import = new XmlDocument();
        //    XmlDocument doc = new XmlDocument();
        //    doc_import.Load(patch);
        //    doc.Load(StableMaster_Patch);

        //    XmlNodeList import = doc_import.GetElementsByTagName("EasternKindoms");
        //    foreach (XmlNode node_import in import)
        //    {
        //        XmlNode myNode = doc.SelectSingleNode("/PF_Stablemaster/EasternKindoms[@Entry=" + node_import.Attributes[1].InnerText + "]");

        //        if (myNode == null)
        //            StableMaster_Add(node_import.Attributes[0].InnerText, node_import.Attributes[1].InnerText, "EasternKindoms", node_import.Attributes[2].InnerText,
        //                node_import.Attributes[3].InnerText);
        //    }

        //    import = doc_import.GetElementsByTagName("Kalimbor");
        //    foreach (XmlNode node_import in import)
        //    {
        //        XmlNode myNode = doc.SelectSingleNode("/PF_Stablemaster/Kalimbor[@Entry=" + node_import.Attributes[1].InnerText + "]");

        //        if (myNode == null)
        //            StableMaster_Add(node_import.Attributes[0].InnerText, node_import.Attributes[1].InnerText, "Kalimbor", node_import.Attributes[2].InnerText,
        //                node_import.Attributes[3].InnerText);
        //    }

        //    import = doc_import.GetElementsByTagName("Northrend");
        //    foreach (XmlNode node_import in import)
        //    {
        //        XmlNode myNode = doc.SelectSingleNode("/PF_Stablemaster/Northrend[@Entry=" + node_import.Attributes[1].InnerText + "]");

        //        if (myNode == null)
        //            StableMaster_Add(node_import.Attributes[0].InnerText, node_import.Attributes[1].InnerText, "Northrend", node_import.Attributes[2].InnerText,
        //                node_import.Attributes[3].InnerText);
        //    }

        //    import = doc_import.GetElementsByTagName("Outland");
        //    foreach (XmlNode node_import in import)
        //    {
        //        XmlNode myNode = doc.SelectSingleNode("/PF_Stablemaster/Outland[@Entry=" + node_import.Attributes[1].InnerText + "]");

        //        if (myNode == null)
        //            StableMaster_Add(node_import.Attributes[0].InnerText, node_import.Attributes[1].InnerText, "Outland", node_import.Attributes[2].InnerText,
        //                node_import.Attributes[3].InnerText);
        //    }

        //    import = doc_import.GetElementsByTagName("Pandaren");
        //    foreach (XmlNode node_import in import)
        //    {
        //        XmlNode myNode = doc.SelectSingleNode("/PF_Stablemaster/Pandaren[@Entry=" + node_import.Attributes[1].InnerText + "]");

        //        if (myNode == null)
        //            StableMaster_Add(node_import.Attributes[0].InnerText, node_import.Attributes[1].InnerText, "Pandaren", node_import.Attributes[2].InnerText,
        //                node_import.Attributes[3].InnerText);
        //    }

        //}

        #endregion

        #region Makros

        // <?xml version="1.0" encoding="utf-8"?>
        // <PF_Makros>
        //   <Makro Name="Name" Logic="Logic" />
        // </PF_Makros>

        public static string Makro_Patch = CreatorHelper.PluginFolder + "Makros\\Makros.xml";

        public static void Makro_Add(string Name, string Logic)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode myRoot, myNode;

            doc.Load(Makro_Patch);

            myRoot = doc.SelectSingleNode("/PF_Makros");
            myNode = doc.CreateElement("Makro");
            myNode.Attributes.Append(doc.CreateAttribute("Name")).InnerText = Name;
            myNode.Attributes.Append(doc.CreateAttribute("Logic")).InnerText = Logic;
            myRoot.AppendChild(myNode);

            doc.Save(Makro_Patch);
        }

        public static Dictionary<string, string> Makro_getList
        {
            get
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Makro_Patch);

                Dictionary<string, string> makros = new Dictionary<string, string>();

                XmlNodeList root = doc.GetElementsByTagName("Makro");

                foreach (XmlNode _node in root)                
                    makros.Add(_node.Attributes[0].InnerText, _node.Attributes[1].InnerText);
                return makros;
            }
        }

        public static void Makro_Remove(string Name)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Makro_Patch);
            XmlNode myNode = doc.SelectSingleNode("/PF_Makros/Makro[@Name='" + Name + "']");
            myNode.ParentNode.RemoveChild(myNode);
            doc.Save(Makro_Patch);
        }

        public static void Makro_Import(string patch)
        {
            XmlDocument doc_import = new XmlDocument();
            XmlDocument doc = new XmlDocument();
            doc_import.Load(patch);
            doc.Load(Makro_Patch);

            XmlNodeList import = doc_import.GetElementsByTagName("Makro");
            foreach (XmlNode node_import in import)
            {
                XmlNode myNode1 = doc.SelectSingleNode("/PF_Stablemaster/Makro[@Name=" + node_import.Attributes[0].InnerText + "]");
                XmlNode myNode2 = doc.SelectSingleNode("/PF_Stablemaster/Makro[@Logic=" + node_import.Attributes[1].InnerText + "]");

                if (myNode1 == null && myNode2 == null)
                    Makro_Add(node_import.Attributes[0].InnerText, node_import.Attributes[1].InnerText);
            }

        }

        #endregion




    }
}
