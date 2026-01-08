using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PokehbuddyLogicCreator;
using System.IO;
using Styx.WoWInternals;
using Styx.Common;
using System.Threading;

namespace PokehbuddyLogicCreator
{
    public partial class Menue : Form
    {
        private Dictionary<string, string> Makros = new Dictionary<string, string>();
        public static Menue ActiveForm;

        public Menue()
        {
            InitializeComponent();
            ActiveForm = this;
        }

        private void Config_UI_2_Load(object sender, EventArgs e)
        {
            CreatorHelper.updateCheckStart();
            lb_version.Text = string.Format("{0} - v{1} beta", CreatorHelper.Now.Name, CreatorHelper.Now.Version);
            Icon = PetBattle.convertToIcon(Image.FromFile(CreatorHelper.PluginFolder + "Images\\Petjournalportrait.png"), Color.Empty);
            btn_SwapPet.Image = Image.FromFile(CreatorHelper.PluginFolder + "Images\\SwapPet.png");
            pb_Logo.Image = Image.FromFile(CreatorHelper.PluginFolder + "Images\\header.jpg");


            #region MakroManager
            if (XmlHelper.Makro_getList.Count == 0)
            {
                //XmlHelper.Makro_Add("Swapout by bad enemyType", "");
                XmlHelper.Makro_Add("Cast spell 1 if ready", "CASTSPELL(1) COOLDOWN(SKILL(1)) EQUALS false");
                XmlHelper.Makro_Add("Cast spell 2 if ready", "CASTSPELL(2) COOLDOWN(SKILL(2)) EQUALS false");
                XmlHelper.Makro_Add("Cast spell 3 if ready", "CASTSPELL(3) COOLDOWN(SKILL(3)) EQUALS false");
                XmlHelper.Makro_Add("Swapout by low HP ( < 30% )", "SWAPOUT Health(THISPET) ISLESSTHAN 30");
                XmlHelper.Makro_Add("Swapout (Me-HP < 30%) AND (Enemy-HP > 35%)", "SWAPOUT Health(THISPET) ISLESSTHAN 30 $ Health(ENEMYPET) ISGREATERTHAN 35");
                XmlHelper.Makro_Add("PASSTURN if all on cooldown", "PASSTURN COOLDOWN(SKILL(1)) EQUALS true $ COOLDOWN(SKILL(2)) EQUALS true $ COOLDOWN(SKILL(3)) EQUALS true");
            }

            Makros.Clear();
            Makros = XmlHelper.Makro_getList;

            lBox_Makros.Items.Clear();
            lBox_Makros.Items.AddRange(Makros.Keys.ToArray());
            #endregion

        }

        private void Config_UI_2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Pokehbuddy.MySettings.Save();
            //PetFighterSettings.Instance.Save();
            //PetFighter.Instance.dlogSettings();
            //new PetFighterConfigForm().Show();
        }

        private void btn_startCreator_Click(object sender, EventArgs e)
        {
            new LogicCreator().ShowDialog();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(tbox_logic.Text);
            tbox_logic.Text = "";
        }

        /// <summary>
        /// by TeamRandom
        /// </summary>
        public void MoveItem(int direction)
        {
            // Checking selected item
            if (listBox1.SelectedItem == null || listBox1.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox1.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox1.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox1.SelectedItem;

            // Removing removable element
            listBox1.Items.Remove(selected);
            // Insert it in new position
            listBox1.Items.Insert(newIndex, selected);
            // Restore selection
            listBox1.SetSelected(newIndex, true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MoveItem(-1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MoveItem(1);
        }


        /// <summary>
        /// by Team Random
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + label71.Text + ".xml";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            listBox1.Items.Clear();
            //Pokehbuddy pok = new Pokehbuddy();
            //pok.LoadPetSettings(label71.Text, label22.Text);
            PetBattle.LoadPetSettings(label71.Text, label22.Text);

            //string dumdum = "";
            //string dumdum = Pokehbuddy.PetSettings.Logic;
            string dumdum = PetBattle.PetSettings.Logic;
            string[] PetLogics = dumdum.Split('@');
            foreach (string alogic in PetLogics)
            {
                listBox1.Items.Add(alogic);
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1) listBox1.Items.Remove(listBox1.SelectedItem);
        }

        /// <summary>
        /// by Team Random
        /// </summary>
        private void button11_Click(object sender, EventArgs e)
        {
            if (label71.Text != "")
            {
                string dummy = "";
                //Pokehbuddy pok = new Pokehbuddy();
                int i = 0;
                foreach (object item in listBox1.Items)
                {
                    dummy = dummy + item.ToString();
                    if (i < listBox1.Items.Count - 1) dummy = dummy + "@";
                    i++;
                }                 
                //pok.LoadPetSettings(label71.Text, label22.Text);
                //Pokehbuddy.PetSettings.Logic = dummy;
                //Pokehbuddy.PetSettings.Save();
                PetBattle.LoadPetSettings(label71.Text, label22.Text);
                PetBattle.PetSettings.Logic = dummy;
                PetBattle.PetSettings.Save();

            }
        }

        /// <summary>
        /// by Team Random
        /// </summary>
        private void button45_Click(object sender, EventArgs e)
        {
            //Pokehbuddy pok = new Pokehbuddy();
            if (label71.Text != "")
            {
                string dummy = "";

                int i = 0;
                foreach (object item in listBox1.Items)
                {
                    dummy = dummy + item.ToString();
                    if (i < listBox1.Items.Count - 1) dummy = dummy + "·";
                    i++;
                }
                PetBattle.LoadPetSettingsBN(PetBattle.GetNameByID(label71.Text));

                PetBattle.PetSettings.Logic = dummy;
                PetBattle.PetSettings.Save();

                Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
                Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
                ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");
                List<string> cnt1 = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = C_PetJournal.GetPetInfoByPetID(string.format('%X'," + label71.Text + ")); local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,_,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=ass end end return teller");
                int getal = 0;
                try
                {
                    getal = Convert.ToInt32(cnt1[0]);
                }
                catch (Exception exc)
                {

                }

                Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
                Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");

                for (int intI = 1; intI < getal; intI++)
                {
                    List<string> cnt = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = '" + PetBattle.GetSpeciesByName(label22.Text) + "' local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,speciesID,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=speciesID  end end return (retdata[" + intI + "])");
                    //Logging.Write(cnt[0]);
                    cnt[0] = PetBattle.GetNameBySpeciesID(cnt[0]);
                    string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + cnt[0] + ".xml";
                    string filename2 = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + label22.Text + ".xml";
                    //Logging.Write("File 1 : "+filename+ " File 2 :"+filename2);
                    if (File.Exists(filename) && filename != filename2) File.Delete(filename);

                    //string filename2=Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\"+pok.GetNameByID(label71.Text)+".xml";
                    if (File.Exists(filename2) && filename != filename2)
                    {
                        File.Copy(filename2, filename);
                    }

                    Logging.Write(cnt[0]);
                }

                // BBLog(cnt[0]);
                //if (cnt[0]=="1") dummy=true;

            }

        }

        /// <summary>
        /// by Team Random
        /// </summary>
        private void button44_Click(object sender, EventArgs e)
        {
            //Pokehbuddy pok = new Pokehbuddy();
            if (label71.Text != "")
            {
                string dummy = "";

                int i = 0;
                foreach (object item in listBox1.Items)
                {
                    dummy = dummy + item.ToString();
                    if (i < listBox1.Items.Count - 1) dummy = dummy + "@";
                    i++;
                }
                PetBattle.LoadPetSettingsBN(label22.Text);


                PetBattle.PetSettings.Logic = dummy;
                PetBattle.PetSettings.Save();


                Lua.DoString("C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_FAVORITES, false) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_COLLECTED, true) C_PetJournal.SetFlagFilter(LE_PET_JOURNAL_FLAG_NOT_COLLECTED, true) ");
                Lua.DoString("C_PetJournal.ClearSearchFilter() C_PetJournal.AddAllPetSourcesFilter() C_PetJournal.AddAllPetTypesFilter() ");
                ////Lua.DoString("C_PetJournal.SetSearchFilter('" + petname + "')");
                List<string> cnt1 = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = '" + PetBattle.GetSpeciesByName(label22.Text) + "' local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,_,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=ass end end return teller");
                int getal = 0;
                try
                {
                    getal = Convert.ToInt32(cnt1[0]);
                }
                catch (Exception exc)
                {

                }
                for (int intI = 1; intI < getal; intI++)
                {
                    List<string> cnt = Lua.GetReturnValues("local teller=0 local retdata={} retdata[0]='nothing' retdata[1]='nothing'  local dummy1 = '" + PetBattle.GetSpeciesByName(label22.Text) + "' local numpets = C_PetJournal.GetNumPets(false) local skillist = C_PetJournal.GetPetAbilityList(dummy1); for j = 1, numpets do  local _, dummy2 = C_PetJournal.GetPetInfoByIndex(j,false); local skillist2 = C_PetJournal.GetPetAbilityList(dummy2); if skillist[1] == skillist2[1] and skillist[2] == skillist2[2] and skillist[3] == skillist2[3] then  local _,speciesID,_,_,_,_,_,ass = C_PetJournal.GetPetInfoByIndex(j,false) teller=teller+1 retdata[teller]=speciesID end end return (retdata[" + intI + "])");
                    cnt[0] = PetBattle.GetNameBySpeciesID(cnt[0]);
                    string filename = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + cnt[0] + ".xml";
                    if (!File.Exists(filename))
                    {

                        string filename2 = Application.StartupPath + "\\Plugins\\Pokehbuddy\\PetSettings\\" + label22.Text + ".xml";
                        if (File.Exists(filename2))
                        {
                            File.Copy(filename2, filename);
                        }
                    }

                    Logging.Write(cnt[0]);
                }

                // BBLog(cnt[0]);
                //if (cnt[0]=="1") dummy=true;
            }

        }

        /// <summary>
        /// by Team Random
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            #region by Team Reandom
            listBox1.Items.Clear();
            //Pokehbuddy pok = new Pokehbuddy();
            PetBattle.LoadPetSettings(PetBattle.ReadActiveSlot(), PetBattle.ReadActiveSlotSpecies());
            label22.Text = PetBattle.ReadActiveSlotSpecies();
            label71.Text = PetBattle.ReadActiveSlot();
            //string dumdum = "";

            string dumdum = PetBattle.PetSettings.Logic;
            string[] PetLogics = dumdum.Split('@');
            foreach (string alogic in PetLogics)
            {
                listBox1.Items.Add(alogic);
            }
            #endregion


            if (!PetBattle.IsInPetBattle)
            {
                lb_noBattle.Visible = true;
                //nud_SwapPetSlot.Enabled = false;
                //btn_SwapPet.Enabled = false;
                //btn_QuickSetup.Enabled = false;
                //btn_startCreator.Enabled = false;
                //gbox_PetInfo.Visible = false;
                //return;
            }
            else
                lb_noBattle.Visible = false;

            if (PetBattle.GetActivePetSlod_Me == 1)
                nud_SwapPetSlot.Value = 2;
            else
                nud_SwapPetSlot.Value = 1;
            nud_SwapPetSlot.Enabled = true;          

            btn_SwapPet.Enabled = true;
            btn_QuickSetup.Enabled = true;
            btn_startCreator.Enabled = true;

            var _var = PetBattle.GetPetInfoBySlotID(PetBattle.GetActivePetSlod_Me);
            if (_var[1] == "nil")
                lb_petInfo_Name.Text = _var[7];
            else
                lb_petInfo_Name.Text = _var[1];
            lb_petInfo_Level.Text = "Level: " + _var[2];
            lb_petInfo_PetType.Text = "PetType: " + LogicCreator.PetTypeName[Convert.ToInt32(_var[9])];
            lb_petInfo_PetID.Text = "PetID: " + PetBattle.ReadActiveSlot();
            string img = _var[8];
            pb_Pet_Img.ErrorImage = Image.FromFile(CreatorHelper.PluginFolder + "Images\\noImg_pet.jpg");
            pb_Pet_Img.InitialImage = Image.FromFile(CreatorHelper.PluginFolder + "Images\\load_pet.jpg");
            img = img.Replace(@"INTERFACE\ICONS\", "").Replace(".BLP", "").ToLower();
            pb_Pet_Img.ImageLocation = "http://wow.zamimg.com/images/wow/icons/large/" + img + ".jpg";

            gbox_PetInfo.Visible = true; 
        }

        private void btn_SwapPet_Click(object sender, EventArgs e)
        {
            if (!PetBattle.IsInPetBattle)
                return;
            if (PetBattle.GetActivePetSlod_Me == nud_SwapPetSlot.Value)
            {
                MessageBox.Show("can not swap to the same pet!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!PetBattle.CanActivePetSwapOut)
            { MessageBox.Show("can not swap pet", "", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            PetBattle.ChangePetBySlotID((int)nud_SwapPetSlot.Value);
            MessageBox.Show("swaped to Pet " + nud_SwapPetSlot.Value.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Thread.Sleep(2000);
            button3_Click(null, null);

        }

        private void btn_QuickSetup_Click(object sender, EventArgs e)
        {
            if (!PetBattle.IsInPetBattle)
                return;

            string txt = @"It is not supported:
  - Buff
  - DoT / HoT
  - Shield
";
            DialogResult _result = new DialogResult();

            _result = MessageBox.Show(txt, "QuickSetup", MessageBoxButtons.OKCancel);
            if (_result != DialogResult.OK)
                return;

            List<string> Routine = new List<string>();

            Dictionary<int, List<string>> Abilitys = new Dictionary<int, List<string>>();

            Abilitys.Add(1, PetBattle.GetAbilityInfo_Me(PetBattle.GetActivePetSlod_Me, 1));
            Abilitys.Add(2, PetBattle.GetAbilityInfo_Me(PetBattle.GetActivePetSlod_Me, 2));
            Abilitys.Add(3, PetBattle.GetAbilityInfo_Me(PetBattle.GetActivePetSlod_Me, 3));


            // SWAPOUT bad ENEMYTYPE
            //Routine.Add("SWAPOUT ENEMYTYPE EQUALS " + LogicCreator_Step1.AttackModifier[PetBattle.GetActivePetSlod_Me, 1].ToUpper());
            Routine.Add("PASSTURN COOLDOWN(SKILL(1)) EQUALS true $ COOLDOWN(SKILL(2)) EQUALS true $ COOLDOWN(SKILL(3)) EQUALS true");

            int i = 1;
            // heal?
            foreach (KeyValuePair<int, List<string>> ability in Abilitys)
            {
                if (ability.Value[7] == "1" && (ability.Value[3] != "0" || ability.Value[5] != "1"))
                {
                    _result = MessageBox.Show("Is \"" + ability.Value[1] + "\" a Heal?", ability.Value[1], MessageBoxButtons.YesNoCancel);

                    if (_result == DialogResult.Yes)
                        Routine.Add("CASTSPELL(" + i.ToString() + ") COOLDOWN(SKILL(" + i.ToString() + ")) EQUALS false $ Health(THISPET) ISLESSTHAN 40 $ Health(ENEMYPET) ISGREATERTHAN 40");

                    else if (_result == DialogResult.Cancel)
                        return;
                }
                i++;
            }

            // SWAPOUT low hp
            Routine.Add("SWAPOUT Health(THISPET) ISLESSTHAN 30");

            i = 1;
            // keep on cooldown?
            foreach (KeyValuePair<int, List<string>> ability in Abilitys)
            {
                if (ability.Value[7] != "1" && ability.Value[3] != "0")
                {
                    _result = MessageBox.Show("keep \"" + ability.Value[1] + "\" on colldown?", ability.Value[1], MessageBoxButtons.YesNoCancel);

                    if (_result == DialogResult.Yes)
                        Routine.Add("CASTSPELL(" + i.ToString() + ") COOLDOWN(SKILL(" + i.ToString() + ")) EQUALS false");

                    else if (_result == DialogResult.Cancel)
                        return;
                }

                i++;
            }

            i = 1;
            // dmgAbility + noCD + noTurns
            foreach (KeyValuePair<int, List<string>> ability in Abilitys)
            {
                if (ability.Value[7] != "1" && ability.Value[3] == "0" && ability.Value[5] == "1")
                    Routine.Add("CASTSPELL(" + i.ToString() + ") COOLDOWN(SKILL(" + i.ToString() + ")) EQUALS false");
                i++;
            }

            listBox1.Items.Clear();
            listBox1.Items.AddRange(Routine.ToArray<object>());
            MessageBox.Show("QuickSetup was successful!");
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs args)
        {
            if (listBox1.Items.Count < 1)
                return;

            List<string> logic = new List<string>();
            List<Color> color = new List<Color>();
            int i = 1;

            foreach (string item in listBox1.Items)
            {
                logic.Add(item);
                if (i == 1) { color.Add(Color.Black); i = 2; continue; }
                if (i == 2) { color.Add(Color.DarkGoldenrod); i = 1; }
            }

            args.DrawBackground();
            args.DrawFocusRectangle();
            args.Graphics.DrawString(logic[args.Index], new Font(listBox1.Font.FontFamily, listBox1.Font.Size, listBox1.Font.Style),
                                  new SolidBrush(color[args.Index]), args.Bounds);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            tb_MakroLogic.Text = listBox1.SelectedItem.ToString();
        }

        private void lBox_Makros_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lBox_Makros.SelectedItem == null)
                return;


            //if (lBox_Makros.SelectedItem.ToString() == "Swapout by bad enemyType")
            //    listBox1.Items.Add("SWAPOUT ENEMYTYPE EQUALS " + CombatHelpers.AttackModifier[PetBattle.GetActivePetSlod_Me, 1].ToUpper());
            else
            {
                listBox1.Items.Add(Makros.Values.ElementAt(Makros.Keys.ToList().IndexOf(lBox_Makros.SelectedItem.ToString())));
            }
        }

        private void btn_Makro_Add_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_MakroName.Text))
            { MessageBox.Show("You still have to enter a MacroName!"); return; }

            if (Makros.Keys.Contains(tb_MakroName.Text))
            { MessageBox.Show("MacroName is already in use!"); return; }

            if (string.IsNullOrEmpty(tb_MakroLogic.Text))
            { MessageBox.Show("First, choose a CombatLogic!"); return; }


            Makros.Add(tb_MakroName.Text, tb_MakroLogic.Text);
            XmlHelper.Makro_Add(tb_MakroName.Text, tb_MakroLogic.Text);
            lBox_Makros.Items.Clear();
            lBox_Makros.Items.AddRange(Makros.Keys.ToArray());

            tb_MakroName.Text = "MakroName";
            tb_MakroLogic.Text = "";
        }

        private void btn_Makro_Delete_Click(object sender, EventArgs e)
        {
            if (lBox_Makros.SelectedItem == null)
                return;

            Makros.Remove(lBox_Makros.SelectedItem.ToString());
            XmlHelper.Makro_Remove(lBox_Makros.SelectedItem.ToString());
            lBox_Makros.Items.Clear();
            lBox_Makros.Items.AddRange(Makros.Keys.ToArray());
        }

        private void btn_Makro_import_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = Application.StartupPath;

            if (fd.ShowDialog() != DialogResult.OK)
                return;

            XmlHelper.Makro_Import(fd.FileName);
        }

        private void btn_help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(CreatorHelper.HelpLink);
        }

        private void pb_Logo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(CreatorHelper.ForumLink);
        }

        
        

        //private void chb_UpdatesCheck_CheckedChanged(object sender, EventArgs e)
        //{
        //    PetFighterSettings.Instance.CheckForUpdates = chb_UpdatesCheck.Checked;
        //}

        

        //private Thread update;
        //private void btn_updateNow_Click(object sender, EventArgs e)
        //{
        //    if (update != null && update.IsAlive)
        //        return;

        //    update = PetFighter.Instance.UpdateThread;
        //    update.Start();
        //}

        
    }
}
