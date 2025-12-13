#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-20 23:55:55 +1000 (Fri, 20 Sep 2013) $
 * $ID$
 * $Revision: 232 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/UI/HealingSelector.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using Oracle.Core.Groups;
using Oracle.Core.WoWObjects;
using Oracle.UI.Settings;
using Styx;
using Styx.Common;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GroupRole = Styx.WoWInternals.WoWObjects.WoWPartyMember.GroupRole;

namespace Oracle.UI
{
    public partial class HealingSelector : Form
    {
        #region settings

        private static OracleSettings OSetting { get { return OracleSettings.Instance; } }

        private static bool EnableProvingGrounds { get { return OSetting.EnableProvingGrounds; } }

        #endregion settings

        public static Dictionary<ulong, SelectablePlayer> SelectiveHealingPlayers;

        private static int _playerCount;
        private static int _tankCount;
        private static int _healerCount;
        private static int _damageCount;

        public HealingSelector()
        {
            InitializeComponent();
            Initialize();
            CreatelstSelectedPlayers("All");
        }

        public static void Initialize()
        {
            SelectiveHealingPlayers = new Dictionary<ulong, SelectablePlayer>();
            if (EnableProvingGrounds)
            {
                PopulateProvingGrounds();
            }
            else
            {
                PopulateAvailablePlayers();
            }
        }

        private static bool PopulateProvingGrounds()
        {
            SelectiveHealingPlayers.Clear();
            try
            {
                foreach (var p in Unit.NPCPriorities.ToList())
                {
                    if (p.Entry == 72218) //Oto the Protector
                    {
                        SelectiveHealingPlayers.Add(p.Guid, new SelectablePlayer(p.Guid, p.Name, GroupRole.Tank, WoWSpec.WarriorProtection, WoWClass.Warrior, 1, true));
                        continue;
                    }

                    if (p.Entry == 72219) //Ki the Assassin
                    {
                        SelectiveHealingPlayers.Add(p.Guid, new SelectablePlayer(p.Guid, p.Name, GroupRole.Damage, WoWSpec.RogueAssassination, WoWClass.Rogue, 1, true));
                        continue;
                    }

                    if (p.Entry == 72221) //Kavan the Arcanist
                    {
                        SelectiveHealingPlayers.Add(p.Guid, new SelectablePlayer(p.Guid, p.Name, GroupRole.Damage, WoWSpec.MageArcane, WoWClass.Mage, 1, true));
                        continue;
                    }

                    if (p.Entry == 72220) //Sooli the Survivalist
                    {
                        SelectiveHealingPlayers.Add(p.Guid, new SelectablePlayer(p.Guid, p.Name, GroupRole.Damage, WoWSpec.HunterSurvival, WoWClass.Hunter, 1, true));
                        continue;
                    }
                }

                SelectiveHealingPlayers.Add(StyxWoW.Me.Guid, new SelectablePlayer(StyxWoW.Me.Guid, StyxWoW.Me.Name, GroupRole.Healer, StyxWoW.Me.ToPlayer().GetSpecialization(), StyxWoW.Me.Class, 1, true));
            }
            catch (InvalidObjectPointerException)
            {
                MessageBox.Show("Sorry, Could not get valid List please try again!", "Woops: Error getting list of players", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }

            return true;
        }

        private static bool PopulateAvailablePlayers()
        {
            SelectiveHealingPlayers.Clear();
            try
            {
                var results = Unit.FriendlyPriorities.Union(Unit.NPCPriorities);

                foreach (var p in results.ToList())
                {
                    if (!OracleRoutine.IsViable(p)) continue;

                    SelectiveHealingPlayers.Add(p.Guid, new SelectablePlayer(p.Guid, p.Name, p.ToPlayer().GetRole(), p.ToPlayer().GetSpecialization(), p.Class, p.ToPlayer().GetGroupNumber(), true));
                }
            }
            catch (InvalidObjectPointerException)
            {
                MessageBox.Show("Sorry, Could not get valid List please try again!", "Woops: Error getting list of players", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }

            return true;
        }

        private void CreatelstSelectedPlayers(string filter)
        {
            // Create a new ListView control.
            lstSelectedPlayers.Clear();

            _playerCount = 0;
            _tankCount = 0;
            _healerCount = 0;
            _damageCount = 0;

            // Set the view to show details.
            lstSelectedPlayers.View = View.Details;
            // Allow the user to edit item text.
            lstSelectedPlayers.LabelEdit = false;
            // Allow the user to rearrange columns.
            lstSelectedPlayers.AllowColumnReorder = true;
            // Display check boxes.
            lstSelectedPlayers.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            lstSelectedPlayers.FullRowSelect = true;
            // Display grid lines.
            lstSelectedPlayers.GridLines = true;
            // Sort the items in the list in ascending order.
            lstSelectedPlayers.Sorting = SortOrder.Descending;

            // Populate our listviewitems...
            foreach (var member in SelectiveHealingPlayers)
            {
                // Apply our filter by radio button selected..gtfo if it dosnt match selection.
                switch (filter)
                {
                    case "All":
                        break;

                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        if (member.Value.GroupNumber.ToString(CultureInfo.InvariantCulture) != filter) continue;
                        break;

                    case "Tank":
                    case "Healer":
                    case "Damage":
                        if (member.Value.Role.ToString() != filter) continue;
                        break;
                }

                var lvi = new ListViewItem(member.Key.ToString(CultureInfo.InvariantCulture), GetImageFromRole(member.Value.Role))
                {
                    Checked = member.Value.IsChecked,
                    BackColor = GetClassColor(member.Value.Class)
                };

                lvi.SubItems.Add(member.Value.Name);
                lvi.SubItems.Add(member.Value.Role.ToString());

                switch (member.Value.Role)
                {
                    case GroupRole.Leader | GroupRole.Tank:
                    case GroupRole.Tank:
                        _tankCount++;
                        break;

                    case GroupRole.Leader | GroupRole.Healer:
                    case GroupRole.Healer:
                        _healerCount++;
                        break;

                    case GroupRole.Leader | GroupRole.Damage:
                    case GroupRole.Damage:
                        _damageCount++;
                        break;
                }

                lvi.SubItems.Add(member.Value.Class.ToString());
                lvi.SubItems.Add(member.Value.Specialization.ToString());
                lvi.SubItems.Add(member.Value.GroupNumber.ToString(CultureInfo.InvariantCulture));

                //Add the items to the ListView.
                lstSelectedPlayers.Items.Add(lvi);

                _playerCount++;
            }

            // Create columns for the items and subitems.
            // Width of -2 indicates auto-size.
            lstSelectedPlayers.Columns.Add("GUID", 150, HorizontalAlignment.Left);
            lstSelectedPlayers.Columns.Add("Name", 150, HorizontalAlignment.Center);
            lstSelectedPlayers.Columns.Add("Role", 80, HorizontalAlignment.Center);
            lstSelectedPlayers.Columns.Add("Class", 80, HorizontalAlignment.Center);
            lstSelectedPlayers.Columns.Add("Specialization", 120, HorizontalAlignment.Center);
            lstSelectedPlayers.Columns.Add("GroupNumber", 80, HorizontalAlignment.Center);

            // Create two ImageList objects.
            var imageListSmall = new ImageList();
            var imageListLarge = new ImageList();

            // Initialize the ImageList objects with our images..
            var damageImage = string.Format("{0}\\Routines\\Oracle\\UI\\Damage.jpg", Utilities.AssemblyDirectory);
            var tankImage = string.Format("{0}\\Routines\\Oracle\\UI\\Tanking.png", Utilities.AssemblyDirectory);
            var healImage = string.Format("{0}\\Routines\\Oracle\\UI\\Healing.png", Utilities.AssemblyDirectory);

            if (File.Exists(damageImage))
            {
                imageListSmall.Images.Add(Image.FromFile(damageImage));
                imageListLarge.Images.Add(Image.FromFile(damageImage));
            }

            if (File.Exists(tankImage))
            {
                imageListSmall.Images.Add(Image.FromFile(healImage));
                imageListLarge.Images.Add(Image.FromFile(healImage));
            }

            if (File.Exists(healImage))
            {
                imageListSmall.Images.Add(Image.FromFile(tankImage));
                imageListLarge.Images.Add(Image.FromFile(tankImage));
            }

            //Assign the ImageList objects to the ListView.
            lstSelectedPlayers.LargeImageList = imageListLarge;
            lstSelectedPlayers.SmallImageList = imageListSmall;

            // Add the ListView to the control collection.
            this.Controls.Add(lstSelectedPlayers);

            lblPlayers.Text = "Player Count: [" + _playerCount + "]";
            lblTanks.Text = "Tank Count: [" + _tankCount + "]";
            lblHealers.Text = "Healer Count: [" + _healerCount + "]";
            lblDamage.Text = "Damage Count: [" + _damageCount + "]";
        }

        #region Misc Helpers.

        private static int GetImageFromRole(GroupRole role)
        {
            switch (role)
            {
                case GroupRole.Leader | GroupRole.Tank:
                case GroupRole.Tank:
                    return 2;
                case GroupRole.Leader | GroupRole.Healer:
                case GroupRole.Healer:
                    return 1;
                case GroupRole.Leader | GroupRole.Damage:
                case GroupRole.Damage:
                    return 0;
            }
            return 99;
        }

        public static Color GetClassColor(WoWClass Class)
        {
            switch (Class)
            {
                case WoWClass.DeathKnight:
                    return Color.FromArgb(196, 30, 59);
                case WoWClass.Druid:
                    return Color.FromArgb(255, 124, 10);
                case WoWClass.Hunter:
                    return Color.FromArgb(170, 211, 114);
                case WoWClass.Mage:
                    return Color.FromArgb(104, 204, 239);
                case WoWClass.Monk:
                    return Color.FromArgb(0, 132, 93);
                case WoWClass.Paladin:
                    return Color.FromArgb(244, 140, 186);
                case WoWClass.Priest:
                    return Color.FromArgb(255, 255, 255);
                case WoWClass.Rogue:
                    return Color.FromArgb(255, 244, 104);
                case WoWClass.Shaman:
                    return Color.FromArgb(35, 89, 221);
                case WoWClass.Warlock:
                    return Color.FromArgb(147, 130, 170);
                case WoWClass.Warrior:
                    return Color.FromArgb(199, 156, 87);
                default:
                    return Color.FromArgb(47, 47, 47);
            }
        }

        #endregion Misc Helpers.

        #region Selectable Player

        public class SelectablePlayer
        {
            private GroupRole _role;

            public SelectablePlayer(ulong guid, string name, GroupRole role, WoWSpec spec, WoWClass spClass, uint groupNumber, bool isChecked)
            {
                GUID = guid;
                Name = name;
                Role = role;
                Specialization = spec;
                Class = spClass;
                GroupNumber = groupNumber;
                IsChecked = isChecked;
            }

            public ulong GUID { get; private set; }

            public string Name { get; private set; }

            // Fix for HB Api not returning the role for tank correctly.
            public GroupRole Role
            {
                get
                {
                    const GroupRole tankLeader = GroupRole.Tank | GroupRole.Leader;
                    if (((int)_role == 50) || (_role == GroupRole.Tank) || (_role & tankLeader) == tankLeader)
                    {
                        _role = GroupRole.Tank;
                    }

                    return _role;
                }
                private set { _role = value; }
            }

            public WoWClass Class { get; private set; }

            public WoWSpec Specialization { get; private set; }

            public uint GroupNumber { get; private set; }

            public string Display
            {
                get
                {
                    var result = "[Role: " + Role + "] [" + Name + "] [" + "Specialization: " + Specialization + "] [GroupNumber: " + GroupNumber + "]";

                    return result;
                }
            }

            public bool IsChecked { get; set; }
        }

        #endregion Selectable Player

        #region Filtering (Radio Buttons)

        private void UpdateGroupNumbersFilter()
        {
            foreach (Control control in this.groupNumbers.Controls)
            {
                if (control is RadioButton)
                {
                    var radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        CreatelstSelectedPlayers(radio.Text);
                    }
                }
            }
        }

        private void UpdateRoleFilter()
        {
            foreach (Control control in this.grpFilterRole.Controls)
            {
                if (control is RadioButton)
                {
                    var radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        CreatelstSelectedPlayers(radio.Text);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void lstSelectedPlayers_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (SelectiveHealingPlayers.ContainsKey(Convert.ToUInt64(e.Item.Text)))
            {
                SelectiveHealingPlayers[Convert.ToUInt64(e.Item.Text)].IsChecked = e.Item.Checked;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ObjectManager.Update();

            if (EnableProvingGrounds)
            {
                if (PopulateProvingGrounds())
                    CreatelstSelectedPlayers("All");
            }
            else
            {
                if (PopulateAvailablePlayers())
                    CreatelstSelectedPlayers("All");
            }
        }

        private void rdioGroup1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGroupNumbersFilter();
        }

        private void rdioGroup2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGroupNumbersFilter();
        }

        private void rdioGroup3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGroupNumbersFilter();
        }

        private void rdioGroup4_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGroupNumbersFilter();
        }

        private void rdioGroup5_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGroupNumbersFilter();
        }

        private void rdioGroupAll_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGroupNumbersFilter();
        }

        private void rdioGroupTank_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRoleFilter();
        }

        private void rdioGroupHealer_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRoleFilter();
        }

        private void rdioGroupDamage_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRoleFilter();
        }

        private void rdioGroupRoleAll_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRoleFilter();
        }

        #endregion Filtering (Radio Buttons)
    }
}