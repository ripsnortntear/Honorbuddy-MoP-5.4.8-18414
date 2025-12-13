#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-15 12:34:43 +1000 (Sun, 15 Sep 2013) $
 * $ID$
 * $Revision: 212 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Core/Spells/SpellList.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.Managers;
using Oracle.Shared.Logging;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

// Credit to Singular for Initial Implementation, ive just added some more ArrayItems and added some duty methods.

namespace Oracle.Core.Spells
{
    [XmlRoot("SpellList")]
    [XmlInclude(typeof(SpellEntry))]
    public class SpellList
    {
        [XmlElement("Version")]
        public string Version { get; set; }

        [XmlElement("Desc")]
        public string Description { get; set; }

        [XmlArray("Spells")]
        [XmlArrayItem("SpellEntry")]
        public List<SpellEntry> Spells { get; set; }

        [Browsable(false)]
        internal HashSet<int> SpellIds { get; set; }

        public string Filename;

        public SpellList()
        {
            Spells = new List<SpellEntry>();
            SpellIds = new HashSet<int>();
        }

        public SpellList(string sFilename)
        {
            Filename = sFilename;
            Spells = new List<SpellEntry>();
            SpellIds = new HashSet<int>();

            if (!File.Exists(Filename))
            {
                LoadDefaults();
                Save(Filename);
                return;
            }

            Load(Filename);
        }

        public void Add(int id, string name = null)
        {
            var se = new SpellEntry(id, name);
            Spells.Add(se);
            SpellIds.Add(id);
        }

        public void Add(int id, string name, DispelType type, DispelDelayType ddtype, int stackcount = 0, int range = 0, int delay = 0)
        {
            var se = new SpellEntry(id, name, type, ddtype, stackcount, range, delay);
            Spells.Add(se);
            SpellIds.Add(id);
        }

        public bool Contains(int id)
        {
            return SpellIds.Contains(id);
        }

        // load list from xml file
        public void Load(string sFilename = null)
        {
            if (string.IsNullOrEmpty(sFilename))
                sFilename = Filename;

            try
            {
                var ser = new XmlSerializer(typeof(SpellList));
                var reader = new StreamReader(sFilename);
                var fcl = (SpellList)ser.Deserialize(reader);
                Description = fcl.Description;
                Spells = fcl.Spells;
                SpellIds = new HashSet<int>(fcl.Spells.Select(sp => sp.Id).ToArray());
            }
            catch (Exception ex)
            {
                Logger.Output(ex.StackTrace);
                Spells = new List<SpellEntry>();
                SpellIds = new HashSet<int>();
            }
        }

        // save list to xml file
        public void Save(string sFilename = null)
        {
            if (string.IsNullOrEmpty(sFilename))
                sFilename = Filename;

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(sFilename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(sFilename));

                var ser = new XmlSerializer(typeof(SpellList), new Type[] { typeof(SpellEntry) });
                using (var writer = new StreamWriter(sFilename))
                {
                    Version = OracleRoutine.GetOracleVersion().ToString();
                    ser.Serialize(writer, this);
                }
            }
            catch (Exception ex)
            {
                Logger.Output(ex.StackTrace);
            }
        }

        private void LoadDefaults()
        {
            //BlackList
            Add(96328, "Toxic Torment (Green Cauldron)", DispelType.BlackList, DispelDelayType.None);
            Add(96325, "Frostburn Formula (Blue Cauldron)", DispelType.BlackList, DispelDelayType.None);
            Add(96326, "Frostburn Formula (?)", DispelType.BlackList, DispelDelayType.None);
            Add(131040, "Burning Blood (Red Cauldron)", DispelType.BlackList, DispelDelayType.None);
            Add(92876, "Burning Blood (?)", DispelType.BlackList, DispelDelayType.None);
            Add(92878, "Blackout (10man)", DispelType.BlackList, DispelDelayType.None);
            Add(86788, "Blackout (25man)", DispelType.BlackList, DispelDelayType.None);
            Add(30108, "Blackout (?)", DispelType.BlackList, DispelDelayType.None);

            //Priority
            Add(143446, "Shadow Word: Bane", DispelType.Priority, DispelDelayType.None);
            Add(143434, "Shadow Word: Bane", DispelType.Priority, DispelDelayType.None);
            Add(144514, "Lingering Corruption", DispelType.Priority, DispelDelayType.None);
            Add(144351, "Mark of Arrogance", DispelType.Priority, DispelDelayType.None);
            Add(77351, "Aqua Bomb", DispelType.Priority, DispelDelayType.None);
            Add(145206, "Aqua Bomb", DispelType.Priority, DispelDelayType.None);
            Add(136708, "Horridon - Stone Gaze", DispelType.Priority, DispelDelayType.None);
            Add(136719, "Farraki - Blazing Sunlight", DispelType.Priority, DispelDelayType.None);
            Add(136587, "Gurubashi- Venom Bolt Volley", DispelType.Priority, DispelDelayType.None);
            Add(136710, "Drakaki - Deadly Plague", DispelType.Priority, DispelDelayType.None);
            Add(136512, "Amani - Hex of Confusion", DispelType.Priority, DispelDelayType.None);
            Add(136857, "Entrapped", DispelType.Priority, DispelDelayType.None);
            Add(136185, "Fragile Bones", DispelType.Priority, DispelDelayType.None);
            Add(136187, "Clouded Mind", DispelType.Priority, DispelDelayType.None);
            Add(136183, "Dulled Synapses", DispelType.Priority, DispelDelayType.None);
            Add(136181, "Impaired Eyesight", DispelType.Priority, DispelDelayType.None);
            Add(138040, "Horrific Visage", DispelType.Priority, DispelDelayType.None);
            Add(117949, "Closed Curcuit", DispelType.Priority, DispelDelayType.None);
            Add(140179, "Mageara - Supression (Heroic)", DispelType.Priority, DispelDelayType.None);
            Add(117436, "Lightning Prison", DispelType.Priority, DispelDelayType.None);
            Add(145563, "Magistrike", DispelType.Priority, DispelDelayType.None);
            


            //Range
            Add(139822, "Mageara - Cinders", DispelType.Range, DispelDelayType.None, 0, 12);
            Add(142913, "Displaced Energy", DispelType.Range, DispelDelayType.None, 0, 9);

            //Delay
            Add(138609, "Dark Animus - Matter Swap", DispelType.Delay, DispelDelayType.CountingDown, 0, 0, 5000);
            Add(133597, "Durumu - Dark Parasite ( Heroic )", DispelType.Delay, DispelDelayType.CountingDown, 0, 0, 7000);
            Add(142942, "Torment", DispelType.Delay, DispelDelayType.CountingUp, 0, 0, 2000);
            Add(136885, "Torment", DispelType.Delay, DispelDelayType.CountingUp, 0, 0, 2000);
            Add(144514, "Lingering Corruption", DispelType.Delay, DispelDelayType.CountingDown, 0, 0, 5000);

            //Stack
            Add(000011, " Test-Stack", DispelType.Stack, DispelDelayType.None, 3);
            Add(143791, "Corrosive Blood", DispelType.Stack, DispelDelayType.None, 2);
            Add(143446, "Shadow Word: Bane", DispelType.Stack, DispelDelayType.None, 2);
        }
    }

    [XmlType("SpellEntry")]
    public class SpellEntry
    {
        private int _id;

        [XmlAttribute("Id")]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                if (value == 0)
                    Name = string.Empty;
                else
                {
                    WoWSpell spell = WoWSpell.FromId(value);
                    Name = spell != null ? spell.Name : "(unknown)";
                }
            }
        }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("DisType")]
        public DispelType DisType { get; set; }

        [XmlAttribute("Delay")]
        public int Delay { get; set; }

        [XmlAttribute("DisDelayType")]
        public DispelDelayType DisDelayType { get; set; }

        [XmlAttribute("StackCount")]
        public int StackCount { get; set; }

        [XmlAttribute("Range")]
        public int Range { get; set; }

        public SpellEntry()
        {
            Id = 0;
            Name = String.Empty;
        }

        public SpellEntry(int id, string name = null)
        {
            Id = id;
            if (name != null)
                Name = name;
        }

        public SpellEntry(int id, string name, DispelType type, DispelDelayType ddtype, int stackcount = 0, int range = 0, int delay = 0)
        {
            Id = id;
            if (name != null)
                Name = name;
            DisType = type;
            Delay = delay;
            DisDelayType = ddtype;
            StackCount = stackcount;
            Range = range;
        }
    }
}