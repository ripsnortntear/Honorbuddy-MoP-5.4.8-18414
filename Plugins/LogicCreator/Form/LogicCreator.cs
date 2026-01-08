using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styx.WoWInternals;
using Styx.Common;

namespace PokehbuddyLogicCreator
{
    public partial class LogicCreator : Form
    {
        string Action = string.Empty;
        int ActionBtn = 0;
        int ActionBtnTemp = 0;
        Size minSize = new Size(950, 458); // new Size(950, 356);
        Size nSize = new Size(950, 610); // new Size(950, 458);
        Size maxSize = new Size(950, 610);

        private Image LCimage(string img)
        { return Image.FromFile( CreatorHelper.PluginFolder + "Images\\LogicCreator\\" + img); }


        public LogicCreator()
        {
            InitializeComponent();
        }

        private void LogicCreator_Step1_Load(object sender, EventArgs e)
        {
            comboBox1.Location = new Point(446, 120);

            if (!PetBattle.IsInPetBattle)
                lb_Info.Text = ("Start a battle manually first").ToUpper();

            //this.BackgroundImage = LCimage("background.jpg");
            //pbox_back.BackgroundImage = LCimage("background.jpg");
            //pbox_Step2_back.BackgroundImage = LCimage("background.jpg");
            pbox_back.Image = LCimage("step1.png");
            pbox_onCD.Image = LCimage("step1_bCD.png");
            pbox_b1.ErrorImage = LCimage("step1_b1.jpg");
            pbox_b1.InitialImage = LCimage("step1_b1.jpg");
            pbox_b1.Image = LCimage("step1_b1.jpg");
            pbox_b2.ErrorImage = LCimage("step1_b2.jpg");
            pbox_b2.InitialImage = LCimage("step1_b2.jpg");
            pbox_b2.Image = LCimage("step1_b2.jpg");
            pbox_b3.ErrorImage = LCimage("step1_b3.jpg");
            pbox_b3.InitialImage = LCimage("step1_b3.jpg");
            pbox_b3.Image = LCimage("step1_b3.jpg");
            panel_b4.BackgroundImage = LCimage("step1_b4.jpg");
            panel_bPass.BackgroundImage = LCimage("step1_bpass.jpg");

            pbox_BuffsMe.BackgroundImage = LCimage("btn_buffs.png");
            pbox_BuffsEnemy.BackgroundImage = LCimage("btn_buffs.png");
            pbox_HealthEnemy.BackgroundImage = LCimage("btn_health.png");
            pbox_HealthMe.BackgroundImage = LCimage("btn_health.png");
            pbox_SpeedEnemy.BackgroundImage = LCimage("btn_petSpeed.png");
            pbox_SpeedMe.BackgroundImage = LCimage("btn_petSpeed.png");
            pbox_PetType.BackgroundImage = LCimage("btn_petType.png");
            pbox_Weather.BackgroundImage = LCimage("btn_weather.png");
            pbox_Step2_back.Image = LCimage("step2.png");

            UpdatePetInfo();

            this.Size = minSize;
            this.MaximumSize = nSize;
        }

        private void btn_AbilitysInfo_Click(object sender, EventArgs e)
        {
            if (!gBox_AbilityInfo.Visible)
            {
                UpdatePetInfo();
                this.MaximumSize = maxSize;
                this.Size = this.MaximumSize;
                this.MinimumSize = this.MaximumSize;
                gBox_AbilityInfo.Visible = true;
                gbox_ListBuffs.Visible = true;
            }
            else
            {
                this.MinimumSize = minSize;

                //if (panel_step1_2.Visible)
                //    this.Size = nSize;
                //else
                    this.Size = this.MinimumSize;

                this.MaximumSize = nSize;
                gBox_AbilityInfo.Visible = false;
                gbox_ListBuffs.Visible = false;
            }
        }

        private void UpdatePetInfo()
        {
            if (!PetBattle.IsInPetBattle)
                return;
            try
            {
                var _var = PetBattle.GetPetInfoBySlotID(PetBattle.GetActivePetSlod_Me);
                gBox_AbilityInfo.Text = string.Format("AbilitysInfo from Pet: {0} - Lvl {1}", _var[7], _var[2]);

                _var = PetBattle.GetAbilityInfo_Me(PetBattle.GetActivePetSlod_Me, 1);
                lb_Info_Name_1.Text = _var[1];
                lb_Info_PetType_1.Text = PetTypeName[Convert.ToInt32(_var[6])];
                lb_Info_CD_1.Text = "CD: " + _var[3];
                lb_Info_turn_1.Text = "Duration: " + _var[5];
                lb_Info_gMod_1.Text = AttackModifier[Convert.ToInt32(_var[6]), 0];
                lb_Info_bMod_1.Text = AttackModifier[Convert.ToInt32(_var[6]), 1];
                pbox_Info_badMod_1.Image = getAttackModi(lb_Info_bMod_1.Text);
                pbox_Info_goodMod_1.Image = getAttackModi(lb_Info_gMod_1.Text);
                pbox_Info_goodMod_1.Visible = true;
                pbox_Info_badMod_1.Visible = true;
                string img = _var[2];
                img = img.Replace(@"INTERFACE\ICONS\", "").Replace(".BLP", "").ToLower();
                pbox_b1.ImageLocation = "http://wow.zamimg.com/images/wow/icons/large/" + img + ".jpg";

                _var = PetBattle.GetAbilityInfo_Me(PetBattle.GetActivePetSlod_Me, 2);
                lb_Info_Name_2.Text = _var[1];
                lb_Info_PetType_2.Text = PetTypeName[Convert.ToInt32(_var[6])];
                lb_Info_CD_2.Text = "CD: " + _var[3];
                lb_Info_turn_2.Text = "Duration: " + _var[5];
                lb_Info_gMod_2.Text = AttackModifier[Convert.ToInt32(_var[6]), 0];
                lb_Info_bMod_2.Text = AttackModifier[Convert.ToInt32(_var[6]), 1];
                pbox_Info_badMod_2.Image = getAttackModi(lb_Info_bMod_2.Text);
                pbox_Info_goodMod_2.Image = getAttackModi(lb_Info_gMod_2.Text);
                pbox_Info_goodMod_2.Visible = true;
                pbox_Info_badMod_2.Visible = true;
                img = _var[2];
                img = img.Replace(@"INTERFACE\ICONS\", "").Replace(".BLP", "").ToLower();
                pbox_b2.ImageLocation = "http://wow.zamimg.com/images/wow/icons/large/" + img + ".jpg";

                _var = PetBattle.GetAbilityInfo_Me(PetBattle.GetActivePetSlod_Me, 3);
                lb_Info_Name_3.Text = _var[1];
                lb_Info_PetType_3.Text = PetTypeName[Convert.ToInt32(_var[6])];
                lb_Info_CD_3.Text = "CD: " + _var[3];
                lb_Info_turn_3.Text = "Duration: " + _var[5];
                lb_Info_gMod_3.Text = AttackModifier[Convert.ToInt32(_var[6]), 0];
                lb_Info_bMod_3.Text = AttackModifier[Convert.ToInt32(_var[6]), 1];
                pbox_Info_badMod_3.Image = getAttackModi(lb_Info_bMod_3.Text);
                pbox_Info_goodMod_3.Image = getAttackModi(lb_Info_gMod_3.Text);
                pbox_Info_goodMod_3.Visible = true;
                pbox_Info_badMod_3.Visible = true;
                img = _var[2];
                img = img.Replace(@"INTERFACE\ICONS\", "").Replace(".BLP", "").ToLower();
                pbox_b3.ImageLocation = "http://wow.zamimg.com/images/wow/icons/large/" + img + ".jpg";
            }
            catch (Exception ex)
            { Logging.WriteException(ex); }
        }

        private Image getAttackModi(string imgName)
        { return Image.FromFile(CreatorHelper.PluginFolder + "Images\\" + imgName + ".png"); }

        private void btn_X_Click(object sender, EventArgs e)
        {
            Action = string.Empty;
            numericUpDown1.Value = 0;
            tbox_logic.Text = string.Empty;
            ActionBtn = 0;
            ActionBtnTemp = 0;

            comboBox1.Visible = false;
            gbox_ListBuffs.Visible = false;
            comboBox3.Visible = false;
            numericUpDown1.Visible = false;
            btn_OK.Visible = false;
            btn_OK.Enabled = false;
            pbox_BuffsMe.Visible = false;
            pbox_BuffsEnemy.Visible = false;
            pbox_HealthEnemy.Visible = false;
            pbox_HealthMe.Visible = false;
            pbox_SpeedEnemy.Visible = false;
            pbox_SpeedMe.Visible = false;
            pbox_PetType.Visible = false;
            pbox_Weather.Visible = false;
            pbox_Step2_back.Image = LCimage("step2.png");

            //this.Size = this.MinimumSize;
            panel_step1_2.Visible = false;
            lb_Info.Text = "\nStep 1:\n - select a Action!";
            logicReady(false);
        }

        private void panel_b1_Click(object sender, EventArgs e)
        { addAction(1); }

        private void panel_b2_Click(object sender, EventArgs e)
        { addAction(2); }

        private void panel_b3_Click(object sender, EventArgs e)
        { addAction(3); }

        private void panel_b4_Click(object sender, EventArgs e)
        { addAction(4); }

        private void panel_bPass_Click(object sender, EventArgs e)
        { addAction(5); }

        private void logicReady(bool ready)
        {
            btn_AND.Enabled = ready; btn_finish.Enabled = ready;
        }

        private void addAction(int btn)
        {
            ActionBtnTemp = btn;
            if (string.IsNullOrEmpty(Action) || (!string.IsNullOrEmpty(Action) && btn > 3))
            {
                logicReady(false);
                Action = getAction(btn);
                tbox_logic.Text = Action;
                ActionBtn = btn; 
                lb_useAbility.Text = tbox_logic.Text + "if ..";
                lb_Info.Text = "\nStep 2:\n - " + lb_useAbility.Text;

                pbox_BuffsMe.Visible = true;
                pbox_BuffsEnemy.Visible = true;
                pbox_HealthEnemy.Visible = true;
                pbox_HealthMe.Visible = true;
                pbox_SpeedEnemy.Visible = true;
                pbox_SpeedMe.Visible = true;
                pbox_PetType.Visible = true;
                pbox_Weather.Visible = true;
                pbox_Step2_back.Image = LCimage("step2_active.png");

                if (btn > 3)
                {
                    pbox_noCD.Visible = false;
                    pbox_onCD.Visible = false;
                }
                else
                {
                    pbox_noCD.Visible = true;
                    pbox_onCD.Visible = false;

                    switch (btn)
                    {
                        case(1):
                            pbox_noCD.BackgroundImage = pbox_b1.Image;
                            break;

                        case(2):
                            pbox_noCD.BackgroundImage = pbox_b2.Image;
                            break;

                        case(3):
                            pbox_noCD.BackgroundImage = pbox_b3.Image;
                            break;

                        default:
                            pbox_noCD.BackgroundImage = LCimage("step1_b" + btn.ToString() + ".jpg");
                            break;
                    }
                    

                }
                //this.Size = this.MaximumSize;
                panel_step1_2.Visible = true;
                pbox_Step2_back.Image = LCimage("step2_active.png");


            }
            else if (btn <= 3)
            {
                pbox_noCD.Visible = true;
                switch (btn)
                {
                    case (1):
                        pbox_noCD.BackgroundImage = pbox_b1.Image;
                        break;

                    case (2):
                        pbox_noCD.BackgroundImage = pbox_b2.Image;
                        break;

                    case (3):
                        pbox_noCD.BackgroundImage = pbox_b3.Image;
                        break;

                    default:
                        pbox_noCD.BackgroundImage = LCimage("step1_b" + btn.ToString() + ".jpg");
                        break;
                }

                if (btn == ActionBtn)
                    pbox_onCD.Visible = false;
                else
                { pbox_onCD.Visible = true; pbox_onCD.BackgroundImage = pbox_noCD.BackgroundImage; }
            }


        }

        private string getAction(int btn)
        {
            switch (btn)
            {
                case (1):
                    return "CASTSPELL(1) ";
                case (2):
                    return "CASTSPELL(2) ";
                case (3):
                    return "CASTSPELL(3) ";
                case (4):
                    return "SWAPOUT ";
                default:
                    return "PASSTURN ";
            }
        }

        // DefaultValue("SWAPOUT Health(THISPET) ISLESSTHAN 30·CASTSPELL(1) COOLDOWN(SKILL(1)) EQUALS false")
        //private void addNewAction(int btn)
        //{
        //    switch (btn)
        //    {
        //        case (1):
        //            tbox_logic.Text = "CASTSPELL(1) ";
        //            break;
        //        case (2):
        //            tbox_logic.Text = "CASTSPELL(2) ";
        //            break;
        //        case (3):
        //            tbox_logic.Text = "CASTSPELL(3) ";
        //            break;
        //        case (4):
        //            tbox_logic.Text = "SWAPOUT ";
        //            break;
        //        case (5):
        //            tbox_logic.Text = "PASSTURN ";
        //            break;
        //        default:
        //            return;
        //    }
        //    logic1 = tbox_logic.Text;
        //    lb_useAbility.Text = tbox_logic.Text + "if ..";
        //    lb_Info.Text = "\nStep 2:\n - " + lb_useAbility.Text;
        //}

        private void addLogic(int btn)
        {
            string _Action = string.Empty;
            switch (btn)
            {
                case (1): // on CD
                    _Action += "COOLDOWN(SKILL(" + ActionBtnTemp.ToString() + ")) EQUALS true";
                    break;
                case (2): // no CD
                    _Action += "COOLDOWN(SKILL(" + ActionBtnTemp.ToString() + ")) EQUALS false";
                    break;
                default:
                    return;
            }

            Action = Action + _Action;
            tbox_logic.Text = Action;
            logicReady(true);

            lb_Info.Text = "\nStep 3: \n - press \"Finish\" or\n   \"AND\" to expand the logic";
        }

        private void pbox_onCD_Click(object sender, EventArgs e)
        { addLogic(1); }

        private void pbox_noCD_Click(object sender, EventArgs e)
        { addLogic(2); }

        private void btn_AND_Click(object sender, EventArgs e)
        {
            Action += " $ ";
            tbox_logic.Text = Action;
            lb_Info.Text = "\nStep 2.2:\n - expand your logic\n   ( Action -> When AND When .. )";
            logicReady(false);
        }

        private void tbox_num_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar == (Char)Keys.Enter && numericUpDown1.Value != 0)
            { btn_OK.PerformClick();  }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            lb_Info.Text = "\nStep 3: \n - press \"Finish\" or\n   \"AND\" to expand the logic";

            if (comboBox3.Items.Contains("ISNOT"))
                tbox_logic.Text += (comboBox3.SelectedItem.ToString() + " " + comboBox1.SelectedItem.ToString());
            else if (comboBox3.Items.Contains("ISLESSTHAN"))
                tbox_logic.Text += (comboBox3.SelectedItem.ToString() + " " + numericUpDown1.Value.ToString());
            else if (comboBox3.Items.Contains("true"))
                tbox_logic.Text += (numericUpDown1.Value.ToString() + ") EQUALS " + comboBox3.SelectedItem.ToString());

            numericUpDown1.Value = 0;

            comboBox1.Visible = false;
            gbox_ListBuffs.Visible = false;
            comboBox3.Visible = false;
            numericUpDown1.Visible = false;
            btn_OK.Visible = false;
            btn_OK.Enabled = false;

            Action = tbox_logic.Text;
            logicReady(true);
        }

        private void tbox_num_TextChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0)
                btn_OK.Enabled = false;
            else if (!btn_OK.Enabled)
                btn_OK.Enabled = true;
        }

        private void pbox_HealthMe_Click(object sender, EventArgs e)
        {
            logicReady(false);
            comboBox3.Items.Clear();
            comboBox3.Items.Add("EQUALS");
            comboBox3.Items.Add("ISLESSTHAN");
            comboBox3.Items.Add("ISGREATERTHAN");
            comboBox3.SelectedIndex = 0;

            comboBox1.Visible = false;
            gbox_ListBuffs.Visible = false;
            comboBox3.Visible = true;
            numericUpDown1.Visible = true;
            btn_OK.Visible = true;
            tbox_logic.Text = Action + "Health(THISPET) ";
        }

        private void pbox_HealthEnemy_Click(object sender, EventArgs e)
        {
            logicReady(false);
            comboBox3.Items.Clear();
            comboBox3.Items.Add("EQUALS");
            comboBox3.Items.Add("ISLESSTHAN");
            comboBox3.Items.Add("ISGREATERTHAN");
            comboBox3.SelectedIndex = 0;

            comboBox1.Visible = false;
            gbox_ListBuffs.Visible = false;
            comboBox3.Visible = true;
            numericUpDown1.Visible = true;
            btn_OK.Visible = true;
            tbox_logic.Text = Action + "Health(ENEMYPET) ";
        }
        private void addBuff(string buff)
        {
            logicReady(false);
            comboBox3.Items.Clear();
            comboBox3.Items.Add("true");
            comboBox3.Items.Add("false");
            comboBox3.SelectedIndex = 0;

            comboBox1.Visible = false;
            comboBox3.Visible = true;
            numericUpDown1.Visible = true;
            btn_OK.Visible = true;
            tbox_logic.Text = Action + buff;

            //gbox_ListBuffs.Visible = true;
            switch (buff)
            {
                case ("WEATHERBUFF("):
                    button99_Click(null, null);
                    break;
                case ("HASBUFF("):
                    button9_Click(null, null);
                    break;
                case ("HASTEAMBUFF("):
                    button919_Click(null, null);
                    break;
                case ("HASENEMYBUFF("):
                    button8_Click(null, null);
                    break;
                case ("ENEMYTEAMBUFF("):
                    button929_Click(null, null);
                    break;
                default:
                    Logging.WriteDiagnostic("Error in addBuff(string) \nshow groupBox");
                    gbox_ListBuffs.Visible = true;
                    break;
            }
        }

        private void pbox_BuffsMe_Click(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Exclamation.Play();
            //addBuff("HASBUFF(");
        }

        private void pbox_BuffsEnemy_Click(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
            //addBuff("HASENEMYBUFF(");
        }

        private void pbox_Weather_Click(object sender, EventArgs e)
        {
            addBuff("WEATHERBUFF(");
        }

        private void pbox_SpeedMe_Click(object sender, EventArgs e)
        {
            logicReady(false);
            comboBox3.Items.Clear();
            comboBox3.Items.Add("EQUALS");
            comboBox3.Items.Add("ISLESSTHAN");
            comboBox3.Items.Add("ISGREATERTHAN");
            comboBox3.SelectedIndex = 0;

            gbox_ListBuffs.Visible = false;
            comboBox1.Visible = false;
            comboBox3.Visible = true;
            numericUpDown1.Visible = true;
            btn_OK.Visible = true;
            tbox_logic.Text = Action + "MYPETSPEED ";
        }

        private void pbox_SpeedEnemy_Click(object sender, EventArgs e)
        {
            logicReady(false);
            comboBox3.Items.Clear();
            comboBox3.Items.Add("EQUALS");
            comboBox3.Items.Add("ISLESSTHAN");
            comboBox3.Items.Add("ISGREATERTHAN");
            comboBox3.SelectedIndex = 0;

            gbox_ListBuffs.Visible = false;
            comboBox1.Visible = false;
            comboBox3.Visible = true;
            numericUpDown1.Visible = true;
            btn_OK.Visible = true;
            tbox_logic.Text = Action + "ENEMYSPEED ";
        }

        private void Buff_Me_ActivePet_Click(object sender, EventArgs e)
        {
            logicReady(false);
            addBuff("HASBUFF(");
        }

        private void Buff_Me_Team_Click(object sender, EventArgs e)
        {
            logicReady(false);
            addBuff("HASTEAMBUFF(");
        }

        private void Buff_Enemy_ActivePet_Click(object sender, EventArgs e)
        {
            logicReady(false);
            addBuff("HASENEMYBUFF(");
        }

        private void Buff_Enemy_Team_Click(object sender, EventArgs e)
        {
            logicReady(false);
            addBuff("ENEMYTEAMBUFF(");
        }

        private void pbox_PetType_Click(object sender, EventArgs e)
        {
            logicReady(false);
            comboBox3.Items.Clear();
            comboBox3.Items.Add("EQUALS");
            comboBox3.Items.Add("ISNOT");
            comboBox3.SelectedIndex = 0;
            comboBox3.Visible = true;

            gbox_ListBuffs.Visible = false;
            numericUpDown1.Visible = false;
            comboBox1.SelectedIndex = 0;
            comboBox1.Visible = true;
            btn_OK.Visible = true;
            btn_OK.Enabled = true;
            tbox_logic.Text = Action + "ENEMYTYPE ";
        }

        #region lb_Tip.Text
        private void LogicCreator_Step1_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = string.Empty;
        }

        private void panel_b1_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action: CASTSPELL(1)";
        }

        private void panel_b2_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action: CASTSPELL(2)";
        }

        private void panel_b3_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action: CASTSPELL(3)";
        }

        private void panel_b4_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action: SWAPOUT";
        }

        private void panel_bPass_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action: PASSTURN";
        }

        private void pbox_BuffsMe_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: hasBuff(X) / hasTeamBuff(X)  ( !!! RIGHT-MOUSE-CLICK !!! )";
        }

        private void pbox_BuffsEnemy_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: hasEnemyBuff(X) / hasEnemyTeamBuff(X)  ( !!! RIGHT-MOUSE-CLICK !!! )";
        }

        private void pbox_SpeedMe_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: MyPetSpeed";
        }

        private void pbox_SpeedEnemy_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: EnemyPetSpeed";
        }

        private void pbox_Weather_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: WeatherBuff(X)";
        }

        private void pbox_HealthMe_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: HEALTH(THISPET)";
        }

        private void pbox_HealthEnemy_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: HEALTH(ENEMYPET)";
        }

        private void pbox_PetType_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: EnemyPetType";
        }

        private void comboBox3_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When -> Compare ..";
        }

        private void comboBox1_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When -> Compare -> Value";
        }

        private void btn_OK_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When -> Compare -> Value -> OK";
        }

        private void pbox_onCD_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: OnCooldown";
        }

        private void pbox_noCD_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "Action -> When: IsNotOnCooldown";
        }

        private void btn_finish_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "finished this logic and go back";
        }

        private void btn_AND_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "extended this logic with an \"AND\"";
        }

        private void btn_X_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "clear all entries";
        }

        private void btn_AbilitysInfo_MouseEnter(object sender, EventArgs e)
        {
            lb_Tip.Text = "show from your ActivePet the AbilityInfos";
        }
        #endregion

        private void btn_finish_Click(object sender, EventArgs e)
        {
            Menue.ActiveForm.tbox_logic.Text = tbox_logic.Text;
            Menue.ActiveForm.btn_add.Enabled = true;
        }

        #region List Buffs
        private void button9_Click(object sender, EventArgs e)
        {
            Lua.DoString("for j=1,C_PetBattles.GetNumAuras(1,C_PetBattles.GetActivePet(1)) do  local buffid = C_PetBattles.GetAuraInfo(1,C_PetBattles.GetActivePet(1),j)  print (buffid) end");
            showBuffBox("C_PetBattles.GetNumAuras(1,C_PetBattles.GetActivePet(1))", "1,C_PetBattles.GetActivePet(1)", "List ME Buffs");
        }

        private void button99_Click(object sender, EventArgs e)
        {
            Lua.DoString("for i=1, C_PetBattles.GetNumAuras(0,0) do local auraID = C_PetBattles.GetAuraInfo(LE_BATTLE_PET_WEATHER, PET_BATTLE_PAD_INDEX, i) print(auraID) end");
            showBuffBox("C_PetBattles.GetNumAuras(0,0)", "LE_BATTLE_PET_WEATHER, PET_BATTLE_PAD_INDEX", "List Weather Buffs");
        }

        private void button919_Click(object sender, EventArgs e)
        {
            Lua.DoString("for i=1, C_PetBattles.GetNumAuras(1,0) do local auraID = C_PetBattles.GetAuraInfo(1, PET_BATTLE_PAD_INDEX, i) print(auraID) end");
            showBuffBox("C_PetBattles.GetNumAuras(1,0)", "1, PET_BATTLE_PAD_INDEX", "List Team Buffs");
        }
        private void button929_Click(object sender, EventArgs e)
        {
            Lua.DoString("for i=1, C_PetBattles.GetNumAuras(2,0) do local auraID = C_PetBattles.GetAuraInfo(2, PET_BATTLE_PAD_INDEX, i) print(auraID) end");
            showBuffBox("C_PetBattles.GetNumAuras(2,0)", "2, PET_BATTLE_PAD_INDEX", "List Enemy Team Buffs");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Lua.DoString("for j=1,C_PetBattles.GetNumAuras(2,C_PetBattles.GetActivePet(2)) do  local buffid = C_PetBattles.GetAuraInfo(2,C_PetBattles.GetActivePet(2),j)  print (buffid) end");
            showBuffBox("C_PetBattles.GetNumAuras(2,C_PetBattles.GetActivePet(2))", "2,C_PetBattles.GetActivePet(2)", "List Enemy Buffs");
        }

        private void showBuffBox(string maxBuffs, string info, string BuffType)
        {
            int numBuffs = Lua.GetReturnVal<int>("return " + maxBuffs, 0);
            List<string> _buffs = new List<string>();

            for (int i = 1; i <= numBuffs; i++)
            { _buffs.Add(Lua.GetReturnVal<string>("return C_PetBattles.GetAuraInfo(" + info + "," + i.ToString() + ")", 0)); }

            new PokehbuddyLogicCreator.BuffBox(_buffs, BuffType).Show();
        }
        #endregion

        private void btn_help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(CreatorHelper.HelpLink);
        }

        public static int[,] AttackModifierInt = new int[,] {
            // {good, bad},
            {0, 0},     // ----
            {2, 8},     // PetType 1 - Humanoid
            {6, 4},     // PetType 2 - Dragonkin
            {9, 2},     // PetType 3 - Flying
            {1, 9},     // PetType 4 - Undead
            {4, 1},     // PetType 5 - Critter
            {3, 10},    // PetType 6 - Magic
            {10, 5},    // PetType 7 - Elemental
            {5, 3},     // PetType 8 - Beast
            {7, 6},     // PetType 9 - Aquatic
            {8, 7},     // PetType 10 - Mechanical
        };

        public static string[,] AttackModifier = new string[,] {
            // {good, bad},
            {"0", "0"},                 // ----
            {"Dragonkin", "Beast"},     // PetType 1 - Humanoid
            {"Magic", "Undead"},        // PetType 2 - Dragonkin
            {"Aquatic", "Dragonkin"},   // PetType 3 - Flying
            {"Humanoid", "Aquatic"},    // PetType 4 - Undead
            {"Undead", "Humanoid"},     // PetType 5 - Critter
            {"Flying", "Mechanical"},   // PetType 6 - Magic
            {"Mechanical", "Critter"},  // PetType 7 - Elemental
            {"Critter", "Flying"},      // PetType 8 - Beast
            {"Elemental", "Magic"},     // PetType 9 - Aquatic
            {"Beast", "Elemental"},     // PetType 10 - Mechanical
        };

        public static string[] PetTypeName = new string[] {
            "",
            "Humanoid",
            "Dragonkin",
            "Flying",
            "Undead",
            "Critter",
            "Magic",
            "Elemental",
            "Beast",
            "Aquatic",
            "Mechanical",
        };
    }
}
