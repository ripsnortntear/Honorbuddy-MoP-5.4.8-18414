/*
 * FightHere by Kamilche
 * 
 * This is a plug-in that will any monster that comes within range.
 * You are 'leashed' to a center point, and won't stray more than X yards away from it.
 * 
 *   
 * 09/29/2012  v1.0.0.0 - First version.
 * 
 */

using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.AreaManagement;
using Styx.Pathing;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace FightHere
{

    public class FightHere : HBPlugin
    {

        public override string Name { get { return System.Text.Encoding.Default.GetString(System.Convert.FromBase64String("tqjLornWLbDrvrajqLKkwtzN+KOp")); } }
        public override string Author { get { return "Kamilche"; } }
        public override bool WantButton { get { return true; } }
        public override Version Version { get { return _version; } }
        private readonly Version _version = new Version(1, 0, 0, 0);
        public override string ButtonText { get { return "Show Mobs"; } }
        public static WoWPoint center = StyxWoW.Me.Location;
        public static int Range = 200; // How far away to look for mobs
        private static string _datapath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "Plugins\\FightHere");
        private static string filename = _datapath + @"\FightHere.xml";
        private static DateTime nexttime = DateTime.Now;
        public static bool Chests = false;
        private static System.Windows.Media.Color _color = System.Windows.Media.Colors.PowderBlue;
        private static int pullstate = 0;
        private static DateTime lastpull = DateTime.Now;
        private static WoWUnit pulltarget = null;
        private static int pullinterval = 0; // 1
        private static string fmt = @"<HBProfile>
<Name>Fight Here</Name>
<MinDurability>0.4</MinDurability>
<MinFreeBagSlots>1</MinFreeBagSlots>

<MinLevel>0</MinLevel>
<MaxLevel>91</MaxLevel>

<MailWhite>true</MailWhite>
<MailGreen>true</MailGreen>
<MailBlue>true</MailBlue>
<MailPurple>true</MailPurple>

<SellGrey>true</SellGrey>
<SellWhite>false</SellWhite>
<SellGreen>false</SellGreen>
<SellBlue>false</SellBlue>
<SellPurple>false</SellPurple>

<Vendors>
</Vendors>

<Mailboxes>
</Mailboxes>

<Blackspots>
</Blackspots>

<GrindArea>
    <Name>Fight Here</Name>
    <TargetMinLevel>2</TargetMinLevel>
    <TargetMaxLevel>91</TargetMaxLevel>
    <Factions>{5}</Factions>
    <LootRadius>{0}</LootRadius>
    <MaxDistance>{1}</MaxDistance>
    <Hotspots>
        <Hotspot X=""{2}"" Y=""{3}"" Z=""{4}"" />
        <Hotspot X=""{2}"" Y=""{3}"" Z=""{7}"" />
    </Hotspots>
    <!-- Factions:
{6}
    -->
</GrindArea>

</HBProfile>";

        public override void OnButtonPress()
        {
            FHConfig frm = new FHConfig();
            frm.ShowDialog();
            SaveProfile();
        }

        private void log(String format, params object[] args)
        {
            String s = string.Format(format, args);
            Logging.Write(_color, "[FightHere] " + s);
        }

        private void SaveProfile()
        {
            log("Center = {0}, range = {1}", center.ToString(), Range);
            log("Fighting the following mobs:");
            string s = "";
            string full = "";
            foreach (Mob mob in Mob.list())
            {
                if (mob.want)
                {
                    log("    {0}", mob.name);
                    s += mob.faction + " ";
                    full += string.Format("{0}", mob.name, Environment.NewLine);
                }
            }
            s = s.Trim();
            File.WriteAllText(filename, string.Format(fmt, Range, Range, center.X, center.Y, center.Z, s, full, center.Z + .5));
            log("Profile saved to {0}", filename);
            ProfileManager.LoadNew(filename);
        }

        public override void Pulse()
        {
            // Loot chests
            if (BotPoi.Current.Type == PoiType.None && Chests)
            {
                List<WoWGameObject> chests = (from c in ObjectManager.GetObjectsOfType<WoWGameObject>(false, false)
                                              where c.Distance < Styx.CommonBot.LootTargeting.LootRadius
                                              where c.SubType == WoWGameObjectType.Chest && !Blacklist.Contains(c.Guid) && c.Distance <= Range && c.CanLoot == true
                                              orderby c.Distance
                                              select c).ToList();
                if (chests.Count > 0)
                {
                    log("Moving towards lootable {0:0} yards away", chests[0].Distance);
                    BotPoi.Current = new BotPoi(chests[0], PoiType.Loot);
                }
            }

            // Look for mob to pull
            if (BotPoi.Current.Type == PoiType.None)
            {
                if (pullstate == 0)
                {
                    List<WoWUnit> units = (from u in ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                                           where Mob.Want(u)
                                           where u.IsDead == false
                                           where u.TaggedByOther == false
                                           where !Blacklist.Contains(u)
                                           where u.Location.Distance(center) <= Range
                                           orderby u.Distance
                                           select u).ToList();
                    if (units.Count > 0 && (
                        StyxWoW.Me.CurrentTarget == null ||
                        StyxWoW.Me.CurrentTarget.IsDead))
                    {
                        pulltarget = units[0];
                        pullstate = 1;
                        lastpull = DateTime.Now.AddSeconds(pullinterval);
                    }
                }

                else if (pullstate == 1 && DateTime.Now > lastpull)
                {
                    log("Moving towards new target {0} (faction {1} entry {2}) at {3:0} ({4:0} from center)",
                        pulltarget.Name,
                        pulltarget.FactionId,
                        pulltarget.Entry,
                        pulltarget.Distance,
                        pulltarget.Location.Distance(center));
                    //pulltarget.Target();
                    //Targeting.Instance.TargetList.Add(pulltarget);
                    //BotPoi.Current = new BotPoi(pulltarget, PoiType.Kill);
                    pullstate = 0;
                    pulltarget = null;
                    WoWPoint loc = pulltarget.Location;
                    if (!Navigator.CanNavigateFully(StyxWoW.Me.Location, pulltarget.Location))
                        loc.Z = Navigator.FindHeights(loc.X, loc.Y).Max();
                    Navigator.MoveTo(loc);

                }
            }
        }

        public class Mob
        {
            private string _name;
            private uint _faction;
            public string name
            {
                get { return _name; }
                set { _name = value; }
            }
            public uint entry;
            public uint faction
            {
                get { return _faction; }
                set { _faction = value; }
            }
            public uint count;
            public bool want;
            private static Dictionary<uint, Mob> d = new Dictionary<uint, Mob>();

            public static List<Mob> list()
            {
                return d.Values.ToList();
            }

            public static bool Want(WoWUnit unit)
            {
                if (!d.ContainsKey(unit.Entry) || d[unit.Entry].want == false)
                    return false;
                else
                    return true;
            }

            public static List<Mob> GetMobs()
            {
                d.Clear();
                List<WoWUnit> units = (from u in ObjectManager.GetObjectsOfType<WoWUnit>(false, false)
                                       where !u.IsPlayer
                                       where !u.IsPet
                                       where !u.IsFriendly
                                       where (u.Attackable || u.CanSelect)
                                       where u.Location.Distance(center) <= Range
                                       orderby u.Distance
                                       select u).ToList();
                foreach (WoWUnit unit in units)
                {
                    if (!d.ContainsKey(unit.Entry))
                    {
                        Mob mob = new Mob();
                        mob.name = unit.Name;
                        mob.entry = unit.Entry;
                        mob.faction = unit.FactionId;
                        mob.count = 1;
                        mob.want = true;
                        if (unit.IsCritter || unit.IsNonCombatPet || unit.IsFriendly || unit.IsPet)
                            mob.want = false;
                        d.Add(mob.entry, mob);
                    }
                    else
                    {
                        d[unit.Entry].count += 1;
                    }
                }
                foreach (Mob m in d.Values)
                    m.name = string.Format("{0}: Count {1}, faction {2}, entry {3}", m.name, m.count, m.faction, m.entry);
                List<Mob> list = d.Values.ToList();
                list.Sort(
                    delegate(Mob p1, Mob p2)
                    {
                        int compareCount = p2.count.CompareTo(p1.count);
                        if (compareCount == 0)
                            return p1.name.CompareTo(p2.name);
                        return compareCount;
                    }
                    );
                return list;
            }

            public static void SetMobs(List<Mob> list)
            {
                d.Clear();
                foreach (Mob mob in list)
                    d.Add(mob.entry, mob);
            }
        }
    }
}



