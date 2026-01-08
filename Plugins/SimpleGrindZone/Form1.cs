using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using System.Diagnostics;

using Styx;
using Styx.Helpers;
//using Styx.Logic;
//using Styx.Logic.AreaManagement;
//using Styx.Logic.BehaviorTree;
//using Styx.Logic.Combat;
//using Styx.Logic.Inventory.Frames.Gossip;
//using Styx.Logic.Inventory.Frames.LootFrame;
//using Styx.Logic.Pathing;
//using Styx.Logic.Profiles;
using Styx.Plugins;
//using Styx.Plugins.PluginClass;
using Styx.WoWInternals;
using Styx.WoWInternals.World;
using Styx.WoWInternals.WoWObjects;
using Styx.Common;
namespace SimpleGrindZone
{
    public partial class Form1 : Form
    {
        private string _FileName = null;
        private int _MyLevel = 0;
        private int _UnitNum = 0;
        private int _PointNum = 0;
        private bool _SpotFlag = true;
        private List<uint> ProtectedItems = new List<uint>()
        {
          6497,6949,6950,8926,8927,8928,21927,43230,43231,17031,118,858,929,1710,3928,13446,
          17556,28551,33447,30319,34581,34582,32760,32761,31737,31735,23773,33803,12654,10579,
          32883,30612,30611,32882,41164,13377,11630,41165,31949,3465,3464,23772,10512,19316,
          19317,10513,9399,24417,18042,15997,11284,28056,8068,8067,8069,4960,41584,2519,28060,
          28061,11285,2516,3030,2512,2515,5568,3033,41586,28053,17556,4541,3770,1645,8766,4496,
          5571,828,5572,805
        };
        
        public Form1()
        {
            InitializeComponent();

        }
       
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
            XmlDocument xmldoc;
           // XmlNode xmlnode;
            XmlElement xmlelem;
            XmlElement xmlelem2;
            XmlText xmltext;
            
            xmldoc = new XmlDocument();
            XmlDeclaration _XmlDcn;
            _XmlDcn = xmldoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmldoc.AppendChild(_XmlDcn);
            //Adding XML declaration paragraphs
           // xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            //xmldoc.AppendChild(xmlnode);
            //Add a root element
            xmlelem = xmldoc.CreateElement("HBProfile");
            //xmltext = xmldoc.CreateTextNode("_HBProfile Text");
           // xmlelem.AppendChild(xmltext);
            xmldoc.AppendChild(xmlelem);
            //Add another element
#region Basic information
            //Name
            xmlelem2 = xmldoc.CreateElement("Name");
            //xmlelem2 = xmldoc.CreateElement("", "SampleElement", "");
            xmltext = xmldoc.CreateTextNode(comboBox1.Text.ToString().Trim());
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //Minimum durability of equipment
            xmlelem2 = xmldoc.CreateElement("MinDurability");           
            xmltext = xmldoc.CreateTextNode("0.4");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //Minimum space for backpack
            xmlelem2 = xmldoc.CreateElement("MinFreeBagSlots");
            xmltext = xmldoc.CreateTextNode("2");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //Sale of grey items
            xmlelem2 = xmldoc.CreateElement("SellGrey");
            xmltext = xmldoc.CreateTextNode("True");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //White goods for sale
            xmlelem2 = xmldoc.CreateElement("SellWhite");
            xmltext = xmldoc.CreateTextNode("False");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //Green items for sale
            xmlelem2 = xmldoc.CreateElement("SellGreen");
            xmltext = xmldoc.CreateTextNode("False");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //Blue items for sale
            xmlelem2 = xmldoc.CreateElement("SellBlue");
            xmltext = xmldoc.CreateTextNode("False");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //Mailing of white goods
            xmlelem2 = xmldoc.CreateElement("MailWhite");
            xmltext = xmldoc.CreateTextNode("True");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //Mailing green items
            xmlelem2 = xmldoc.CreateElement("MailGreen");
            xmltext = xmldoc.CreateTextNode("True");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //Mailing of blue items
            xmlelem2 = xmldoc.CreateElement("MailBlue");
            xmltext = xmldoc.CreateTextNode("True");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
            //Mailing of purple items
            xmlelem2 = xmldoc.CreateElement("MailPurple");
            xmltext = xmldoc.CreateTextNode("True");
            xmlelem2.AppendChild(xmltext);
            xmldoc.ChildNodes.Item(1).AppendChild(xmlelem2);
#endregion

#region List of protected items
            //List of protected items

            XmlNode _HBProfile = xmldoc.SelectSingleNode("HBProfile");//Find<HBProfile>
            XmlElement _ProtectedItems = xmldoc.CreateElement("ProtectedItems");//Create一个<ProtectedItems>Nodes
            XmlElement _Item;
            foreach (uint ui in ProtectedItems)
            {
                if (ui.ToString() != "" && ui.ToString() != null)
                {
                    _Item = xmldoc.CreateElement("Item");
                    _Item.InnerText = ui.ToString();
                    _ProtectedItems.AppendChild(_Item);
                }
            }
            //_Item.InnerText = "6497";//Set text node
            //_ProtectedItems.AppendChild(_Item);
           //// _Item = xmldoc.CreateElement("Item");
           // _Item.InnerText = "17031";
           // _ProtectedItems.AppendChild(_Item);
            _HBProfile.AppendChild(_ProtectedItems);
#endregion

#region Combat area options
            XmlElement _SubProfile = xmldoc.CreateElement("SubProfile");
            XmlElement _MinLevel = xmldoc.CreateElement("MinLevel");
            _MyLevel = StyxWoW.Me.Level;
            _MinLevel.InnerText = textBox1.Text;            
            XmlElement _MaxLevel = xmldoc.CreateElement("MaxLevel");       
           
           _MaxLevel.InnerText = textBox5.Text;          
           
            XmlElement _Vendors = xmldoc.CreateElement("Vendors");
            _Vendors.InnerText = null;  
            XmlElement _Mailboxes = xmldoc.CreateElement("Mailboxes");
            _Mailboxes.InnerText = null; 
            //XmlElement _AvoidMobs = xmldoc.CreateElement("AvoidMobs");
            //_AvoidMobs.InnerText = null; 
           
            XmlElement _TargetMinLevel = xmldoc.CreateElement("TargetMinLevel");
           
                int _tx3 = Int32.Parse(textBox3.Text);
                int _minl = _MyLevel - _tx3;
                if (_minl < 1) _minl = 1;
                _TargetMinLevel.InnerText = _minl.ToString();
            
            XmlElement _TargetMaxLevel = xmldoc.CreateElement("TargetMaxLevel");
           
                int _tx4 = Int32.Parse(textBox4.Text);
                int _maxl = _MyLevel + _tx4;
                _TargetMaxLevel.InnerText = _maxl.ToString();
            

            XmlElement _Factions = xmldoc.CreateElement("Factions");
           
            XmlElement _Blackspots = xmldoc.CreateElement("Blackspots");
            _Blackspots.InnerText = null; 
            XmlElement _Hotspots = xmldoc.CreateElement("Hotspots");
            _Hotspots.InnerText = null;

 #region Testing data
            /*
            listView1.Items.Clear();
           // listView2.Items.Clear();
           // listView3.Items.Clear();
           // listView4.Items.Clear();

            listView1.View = View.Details;           
            listView1.BeginUpdate();
            for (int i = 0; i <= 10; i++)
            {
                listView1.Items.Add("Hotspot", i);
                listView1.Items[i].SubItems.Add("-3055.249");
                listView1.Items[i].SubItems.Add("-376.4899");
                listView1.Items[i].SubItems.Add("40.29762");
            }                          
            listView1.EndUpdate();
           
               listView2.View = View.Details;
               listView2.BeginUpdate();
               listView2.Items.Add("Vendor", 0);
               listView2.Items[0].SubItems.Add("Kawnie Softbreeze");
               listView2.Items[0].SubItems.Add("3072");
               listView2.Items[0].SubItems.Add("Repair");
               listView2.Items[0].SubItems.Add("-2893.718");
               listView2.Items[0].SubItems.Add("-279.3317");
               listView2.Items[0].SubItems.Add("53.91697");
               listView2.EndUpdate();
         
               listView3.View = View.Details;
          
               listView3.BeginUpdate();
               listView3.Items.Add("Vendor", 0);
               listView3.Items[0].SubItems.Add("Bronk Steelrage");
               listView3.Items[0].SubItems.Add("3594");
               listView3.Items[0].SubItems.Add("Food");
               listView3.Items[0].SubItems.Add("-2927.324");
               listView3.Items[0].SubItems.Add("-223.131");
               listView3.Items[0].SubItems.Add("54.17663");
               listView3.EndUpdate();
           
               listView4.View = View.Details;
               int _MaxNameLenth = 0;
               for (int i = 0; i <= 6; i=i+2)
               {
                   listView4.BeginUpdate();
                   listView4.Items.Add("Monster name"+(i+1).ToString(), i);
                   if (listView4.Items[i].Text.Length > _MaxNameLenth)
                   {
                       _MaxNameLenth = listView4.Items[i].Text.Length;
                       listView4.Columns[0].Width = _MaxNameLenth * 15;
                   }
                   listView4.Items[i].SubItems.Add((i+3).ToString());
                   listView4.Items[i].SubItems.Add("4");

                   listView4.Items.Add("Monster name" + i.ToString(), i+1);
               
                   listView4.Items[i + 1].SubItems.Add((i + 3).ToString());
                   listView4.Items[i + 1].SubItems.Add("5");
                
                   listView4.EndUpdate();

               }
                */
 #endregion


            XmlElement _Hotspot;
            foreach (ListViewItem _lv in listView1.Items)
            {
               
                this.Update();
                _Hotspot = xmldoc.CreateElement("Hotspot");
                _Hotspot.SetAttribute("X", _lv.SubItems[1].Text.ToString().Trim());
                _Hotspot.SetAttribute("Y", _lv.SubItems[2].Text.ToString().Trim());
                _Hotspot.SetAttribute("Z", _lv.SubItems[3].Text.ToString().Trim());
                _Hotspots.AppendChild(_Hotspot);
            }

            XmlElement _Vendor;
            foreach (ListViewItem _lv in listView2.Items)
            {

                this.Update();
                _Vendor = xmldoc.CreateElement("Vendor");
                _Vendor.SetAttribute("Name", _lv.SubItems[1].Text.ToString().Trim());
                _Vendor.SetAttribute("Entry", _lv.SubItems[2].Text.ToString().Trim());
                _Vendor.SetAttribute("Type", "Food");
                _Vendor.SetAttribute("X", _lv.SubItems[4].Text.ToString().Trim());
                _Vendor.SetAttribute("Y", _lv.SubItems[5].Text.ToString().Trim());
                _Vendor.SetAttribute("Z", _lv.SubItems[6].Text.ToString().Trim());
                _Vendors.AppendChild(_Vendor);
            }

            foreach (ListViewItem _lv in listView3.Items)
            {

                this.Update();
                _Vendor = xmldoc.CreateElement("Vendor");
                _Vendor.SetAttribute("Name", _lv.SubItems[1].Text.ToString().Trim());
                _Vendor.SetAttribute("Entry", _lv.SubItems[2].Text.ToString().Trim());
                _Vendor.SetAttribute("Type", "Repair");
                _Vendor.SetAttribute("X", _lv.SubItems[4].Text.ToString().Trim());
                _Vendor.SetAttribute("Y", _lv.SubItems[5].Text.ToString().Trim());
                _Vendor.SetAttribute("Z", _lv.SubItems[6].Text.ToString().Trim());
                _Vendors.AppendChild(_Vendor);
            }

           
            string _fn = null;
            List<string> _ls = new List<string>();
            foreach (ListViewItem _lv in listView4.Items)
            {
                //textBoxtest.Text += _lv.SubItems[1].Text;
               // textBoxtest.Text += _ls.Contains(_lv.SubItems[1].Text).ToString();
                if (!_ls.Contains(_lv.SubItems[1].Text))
                {
                   _ls.Add(_lv.SubItems[1].Text);
                }  
            }
            foreach (string _ts in _ls)
            {
                _fn += _ts;
                _fn += " ";
            }
            if (_fn != null) { _fn = _fn.Trim(); }
            _Factions.InnerText = _fn;

            _SubProfile.AppendChild(_Blackspots);
           
            _SubProfile.AppendChild(_Vendors);
            XmlElement _Mailbox;
            if (listView5.Items != null)
            {

                foreach (ListViewItem _lv in listView5.Items)
                {

                    this.Update();
                    _Mailbox = xmldoc.CreateElement("Mailbox");
                    _Mailbox.SetAttribute("X", _lv.SubItems[1].Text.ToString().Trim());
                    _Mailbox.SetAttribute("Y", _lv.SubItems[2].Text.ToString().Trim());
                    _Mailbox.SetAttribute("Z", _lv.SubItems[3].Text.ToString().Trim());
                    _Mailboxes.AppendChild(_Mailbox);
                }

               
            }
            _SubProfile.AppendChild(_Mailboxes);
           
           // _SubProfile.AppendChild(_AvoidMobs);
            //If you choose to generate a fight script, the script format will change
            if (radioButton2.Checked)
            {
                XmlElement _GrindArea = xmldoc.CreateElement("GrindArea");
                _GrindArea.AppendChild(_TargetMinLevel);
                _GrindArea.AppendChild(_TargetMaxLevel);
                _GrindArea.AppendChild(_Factions);
                _GrindArea.AppendChild(_Hotspots);

                _SubProfile.AppendChild(_MinLevel);
                _SubProfile.AppendChild(_MaxLevel);
                _SubProfile.AppendChild(_GrindArea);
            }
            else
            {
                _SubProfile.AppendChild(_Hotspots);
            }
            _HBProfile.AppendChild(_SubProfile);
#endregion



            






            //Save the created XML document
            try
            {
                _FileName = Application.StartupPath + @"\" + comboBox1.Text.ToString().Trim() + ".xml";
                xmldoc.Save(_FileName);
                MessageBox.Show("生成xml文件成功！");
            }
            catch (Exception ee)
            {
                //Display of error messages
               Logging.Write(ee.ToString());
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ObjectManager.Update();
            if(StyxWoW.Me.GotTarget)
            {
                /*
                textBoxtest.Text += "name:" + StyxWoW.Me.CurrentTarget.Name;
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "Entry:" + StyxWoW.Me.CurrentTarget.Entry;
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "Type:" + StyxWoW.Me.CurrentTarget.Type;
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "x:" +StyxWoW.Me.CurrentTarget.X;
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "y:"+StyxWoW.Me.CurrentTarget.Y;
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "z:" + ObjectManager..CurrentTargetMe.Z;
                textBoxtest.Text += "\r\n";
                 */
                listView2.Items.Clear();
                listView2.View = View.Details;
                listView2.BeginUpdate();
                listView2.Items.Add("Vendor", 0);
                listView2.Columns[1].Width = StyxWoW.Me.CurrentTarget.Name.Length * 15;
                listView2.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.Name);
                listView2.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.Entry.ToString());
                listView2.Items[0].SubItems.Add("Food");
                listView2.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.X.ToString());
                listView2.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.Y.ToString());
                listView2.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.Z.ToString());
                listView2.EndUpdate();

            }
            else { MessageBox.Show("No target"); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ObjectManager.Update();
            if (StyxWoW.Me.GotTarget)
            {
                listView3.Items.Clear();
                listView3.View = View.Details;
                listView3.BeginUpdate();
                listView3.Items.Add("Vendor", 0);
                listView3.Columns[1].Width = StyxWoW.Me.CurrentTarget.Name.Length * 15;
                listView3.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.Name);
                listView3.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.Entry.ToString());
                listView3.Items[0].SubItems.Add("Repair");
                listView3.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.X.ToString());
                listView3.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.Y.ToString());
                listView3.Items[0].SubItems.Add(StyxWoW.Me.CurrentTarget.Z.ToString());
                listView3.EndUpdate();

            }
            else { MessageBox.Show("No target"); }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ObjectManager.Update();
            if (StyxWoW.Me.GotTarget && StyxWoW.Me.CurrentTarget.Attackable)
            {
                /*
                textBoxtest.Text += "name:" + StyxWoW.Me.CurrentTarget.Name;
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "Faction:" + StyxWoW.Me.CurrentTarget.Faction.ToString();
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "Entry:" + StyxWoW.Me.CurrentTarget.Entry;
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "Type:" + StyxWoW.Me.CurrentTarget.Type;
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "level:" + StyxWoW.Me.CurrentTarget.Level.ToString();
                textBoxtest.Text += "\r\n";
                textBoxtest.Text += "Attc:" + StyxWoW.Me.CurrentTarget.Attackable.ToString();
                textBoxtest.Text += "\r\n";
                 * 
                textBoxtest.Text += "Faction:" + StyxWoW.Me.CurrentTarget.FactionId.ToString();
                textBoxtest.Text += "\r\n";
                */


                listView4.View = View.Details;
                listView4.BeginUpdate();
                listView4.Items.Add(StyxWoW.Me.CurrentTarget.Name,_UnitNum);
                listView4.Items[_UnitNum].SubItems.Add(StyxWoW.Me.CurrentTarget.FactionId.ToString());
                listView4.Items[_UnitNum].SubItems.Add(StyxWoW.Me.CurrentTarget.Level.ToString());
                listView4.EndUpdate();
                _UnitNum++;
            }
            else
            {
                MessageBox.Show("No target or target not attackable");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                _SpotFlag = true;
                listView1.Items.Clear();
                Thread t = new Thread(new ThreadStart(StartThreadProc));
                t.Start();
            }
            catch (Exception ee)
            {
                //Display of error messages
                Logging.Write(ee.ToString());
            }
        }

        private void StartThreadProc()
       {
           ListViewItem _Oldlvi = new ListViewItem();
           _Oldlvi.SubItems[0].Text = "Hotspot";
           _Oldlvi.SubItems.Add(StyxWoW.Me.X.ToString());
           _Oldlvi.SubItems.Add(StyxWoW.Me.Y.ToString());
           _Oldlvi.SubItems.Add(StyxWoW.Me.Z.ToString());
           GrindZone(_Oldlvi);
           while (_SpotFlag)
         {
             ListViewItem _lvi = new ListViewItem();
             //Empty children
             _lvi.SubItems.Clear();
             //Add each column value
             ObjectManager.Update();
             _lvi.SubItems[0].Text = "Hotspot";
             _lvi.SubItems.Add(StyxWoW.Me.X.ToString());
             _lvi.SubItems.Add(StyxWoW.Me.Y.ToString());
             _lvi.SubItems.Add(StyxWoW.Me.Z.ToString());
             
             Thread.Sleep(3000);
             
                 if (!(_lvi.SubItems[1].Text == _Oldlvi.SubItems[1].Text && _lvi.SubItems[2].Text == _Oldlvi.SubItems[2].Text && _lvi.SubItems[3].Text == _Oldlvi.SubItems[3].Text))
                 {
                     GrindZone(_lvi);
                 }
             
             Thread.Sleep(500);
             _Oldlvi = _lvi;
         }//while
       }

        private void GrindZone(ListViewItem lvi)
        {
            //Determine if a wakeup request is needed, if the control is in a thread with the main thread, you can write it asif(!InvokeRequired)
            if (!listView1.InvokeRequired)
            {



                listView1.BeginUpdate();
                if (!listView1.Items.Contains(lvi)) { listView1.Items.Add(lvi); }
                listView1.EndUpdate();
                
                
            }
            else
            {
                InvokeGrindZone _InvokeGZ = new InvokeGrindZone(GrindZone);
                Invoke(_InvokeGZ, new object[] { lvi });//Perform wake-up operations
            }
            
        }
        delegate void InvokeGrindZone(ListViewItem lstItem);//Create a proxy


        private void button4_Click(object sender, EventArgs e)
        {
           
            _SpotFlag = false;
        }

        public static bool f_mailbox = false;
        private void button9_Click(object sender, EventArgs e)
        {
            ObjectManager.Update();
            

                listView5.Items.Clear();
                listView5.View = View.Details;
                listView5.BeginUpdate();
                listView5.Items.Add("Mailbox", 0);
                listView5.Items[0].SubItems.Add(StyxWoW.Me.Location.X.ToString());
                listView5.Items[0].SubItems.Add(StyxWoW.Me.Location.Y.ToString());
                listView5.Items[0].SubItems.Add(StyxWoW.Me.Location.Z.ToString());
                listView5.EndUpdate();

                if (!f_mailbox)
                {
                    MessageBox.Show("Make sure your character is standing in front of the mailbox! Otherwise the coordinates obtained will not work！");
                    f_mailbox = true;
                }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.Select();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            button8.Enabled = false;
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            button8.Enabled = true;
        }

  
       

       


    }
}
