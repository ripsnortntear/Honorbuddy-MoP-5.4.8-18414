using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Styx.Common;

namespace RichieShadowPriestPvP {

    public partial class UI : Form {
	
        public UI() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

            desperatePrayerHp.Value = SPSettings.Instance.DesperatePrayer;
            UseFadeAsDefensiveCDBelow.Value = SPSettings.Instance.UseFadeAsDefensiveCDBelow;
            dispersionMana.Value = SPSettings.Instance.DispersionMana;
            dispersionHp.Value = SPSettings.Instance.DispersionHP;
            Lvl90MinUnits.Value = SPSettings.Instance.Lvl90MinUnits;
            healthstoneHp.Value = SPSettings.Instance.HealthstonePercent;            
            InstantHealBelowHp.Value = SPSettings.Instance.InstantHealBelowHp;
            swpRefresh.Value = SPSettings.Instance.SWPRefresh;
            vampiricEmbraceHp.Value = SPSettings.Instance.VampiricEmbracePercent;
            vtRefresh.Value = SPSettings.Instance.VampiricTouchRefresh;
            aoeMana.Value = SPSettings.Instance.AoEMana;
            CastedHealsBelowHp.Value = SPSettings.Instance.CastedHealsBelowHp;
            DispelDelay.Value = SPSettings.Instance.DispelDelay;
            DispelAboveHp.Value = SPSettings.Instance.DispelAboveHp;
            VTHighPrioBelowManaPercent.Value = SPSettings.Instance.VTHighPrioBelowManaPercent;
            interval.Value = SPSettings.Instance.SearchInterval;
            AngelicFeatherUsage.SelectedIndex = SPSettings.Instance.AngelicFeatherUsage;
            AngelicFeatherPriority.SelectedIndex = SPSettings.Instance.AngelicFeatherPriority;
            
                       

            lvl90safeCheck.Checked = SPSettings.Instance.Lvl90SafeCheck;
            VoidShiftGrief.Checked = SPSettings.Instance.GriefPpl;
            multidot.Checked = SPSettings.Instance.Multidot;
            healOthers.Checked = SPSettings.Instance.HealOthers;
            fearWard.Checked = SPSettings.Instance.FearWardSelf;
            massDispel.Checked = SPSettings.Instance.AutoMassDispel;
            face.Checked = SPSettings.Instance.AutoFace;
            fade.Checked = SPSettings.Instance.FadeAuto;
            AutoBurst.Checked = SPSettings.Instance.AutoBurst;
            pwf.Checked = SPSettings.Instance.PowerWordFortitude;
            UseShadowFiend.Checked = SPSettings.Instance.UseShadowFiend;
            UsePowerInfusion.Checked = SPSettings.Instance.UsePowerInfusion;
            LeapOfFaithOnScatteredTeammate.Checked = SPSettings.Instance.LeapOfFaithOnScatteredTeammate;
            UseRacial.Checked = SPSettings.Instance.UseRacial;
            RightClickMovementOff.Checked = SPSettings.Instance.RightClickMovementOff;
            UseTrinketWithDP.Checked = SPSettings.Instance.UseTrinketWithDP;
            upperTrinketSlot.Checked = SPSettings.Instance.TrinketSlotNumber == 13;
            lowerTrinketSlot.Checked = SPSettings.Instance.TrinketSlotNumber == 14;
            OnlyHealVIP.Checked = SPSettings.Instance.OnlyHealVIP;            
            HealPets.Checked = SPSettings.Instance.HealPets;
            StopCastOnInterrupt.Checked = SPSettings.Instance.StopCastOnInterrupt;
            CounteractPoly.Checked = SPSettings.Instance.CounteractPoly;
            CastFailedUserInitiatedSpell.Checked = SPSettings.Instance.CastFailedUserInitiatedSpell;
            CollectStatistics.Checked = SPSettings.Instance.CollectStatistics;
            CastLevitate.Checked = SPSettings.Instance.CastLevitate;
            

            if (!healOthers.Checked) {
                OnlyHealVIP.Checked = false;
                SPSettings.Instance.OnlyHealVIP = false;
            }

            if (OnlyHealVIP.Checked) {
                healOthers.Checked = true;
                SPSettings.Instance.HealOthers = true;
            }

            DisablePeeling.Checked = SPSettings.Instance.DisablePeeling;
            PeelBelow.Value = SPSettings.Instance.PeelBelow;
            DefensiveCDDelay.Value = SPSettings.Instance.DefensiveCDDelay;
            UseVoidTendrils.Checked = SPSettings.Instance.UseVoidTendrils;
            UsePsychicScream.Checked = SPSettings.Instance.UsePsychicScream;
            UsePsyfiend.Checked = SPSettings.Instance.UsePsyfiend;
            UseSpectralGuise.Checked = SPSettings.Instance.UseSpectralGuise;
            UsePsychicHorrorPeel.Checked = SPSettings.Instance.UsePsychicHorrorPeel;



            DisableCC.Checked = SPSettings.Instance.DisableCC;
            CCFocusOrHealerBelow.Value = SPSettings.Instance.CCFocusOrHealerBelow;
            CCDelay.Value = SPSettings.Instance.CCDelay;
            CCWhenBursting.Checked = SPSettings.Instance.CCWhenBursting;
            UsePsychicScreamCC.Checked = SPSettings.Instance.UsePsychicScreamCC;
            UseSilenceCC.Checked = SPSettings.Instance.UseSilenceCC;
            UsePsyfiendCC.Checked = SPSettings.Instance.UsePsyfiendCC;
            UsePsychicHorrorCC.Checked = SPSettings.Instance.UsePsychicHorrorCC;

            ComboBoxItem[] mods = new ComboBoxItem[] {
                new ComboBoxItem((int)ModifierKeys, "None"),
                new ComboBoxItem((int)Styx.Common.ModifierKeys.Shift, "Shift"),
                new ComboBoxItem((int)Styx.Common.ModifierKeys.Control, "Ctrl"), 
                new ComboBoxItem((int)Styx.Common.ModifierKeys.Alt, "Alt")                
            };

            ComboBoxItem[] keys = new ComboBoxItem[] {
                new ComboBoxItem((int)Keys.None, "None"),
                new ComboBoxItem((int)Keys.D0, "0"),
                new ComboBoxItem((int)Keys.D1, "1"),
                new ComboBoxItem((int)Keys.D2, "2"),
                new ComboBoxItem((int)Keys.D3, "3"),
                new ComboBoxItem((int)Keys.D4, "4"),
                new ComboBoxItem((int)Keys.D5, "5"),
                new ComboBoxItem((int)Keys.Q, "Q"),
                new ComboBoxItem((int)Keys.E, "E"),
                new ComboBoxItem((int)Keys.R, "R"),
                new ComboBoxItem((int)Keys.T, "T"),
                new ComboBoxItem((int)Keys.Z, "Z"),
                new ComboBoxItem((int)Keys.F, "F"),
                new ComboBoxItem((int)Keys.G, "G"),
                new ComboBoxItem((int)Keys.Y, "Y"),
                new ComboBoxItem((int)Keys.X, "X"),
                new ComboBoxItem((int)Keys.C, "C"),
                new ComboBoxItem((int)Keys.V, "V")
            };

            ModBurst.Items.AddRange(mods);
            ModPsyfiendTarget.Items.AddRange(mods);
            ModPsyfiendFocus.Items.AddRange(mods);
            ModAngelicFeather.Items.AddRange(mods);
            ModAngelicFeatherVIP.Items.AddRange(mods);

            KeyBurst.Items.AddRange(keys);
            KeyPsyfiendTarget.Items.AddRange(keys);
            KeyPsyfiendFocus.Items.AddRange(keys);
            KeyAngelicFeather.Items.AddRange(keys);
            KeyAngelicFeatherVIP.Items.AddRange(keys);

            SetComboBoxEnum(ModBurst, (int)SPSettings.Instance.ModBurst);
            SetComboBoxEnum(KeyBurst, (int)SPSettings.Instance.KeyBurst);
            SetComboBoxEnum(ModPsyfiendTarget, (int)SPSettings.Instance.ModPsyfiendTarget);
            SetComboBoxEnum(KeyPsyfiendTarget, (int)SPSettings.Instance.KeyPsyfiendTarget);
            SetComboBoxEnum(ModPsyfiendFocus, (int)SPSettings.Instance.ModPsyfiendFocus);
            SetComboBoxEnum(KeyPsyfiendFocus, (int)SPSettings.Instance.KeyPsyfiendFocus);
            SetComboBoxEnum(ModAngelicFeather, (int)SPSettings.Instance.ModAngelicFeather);
            SetComboBoxEnum(KeyAngelicFeather, (int)SPSettings.Instance.KeyAngelicFeather);
            SetComboBoxEnum(ModAngelicFeatherVIP, (int)SPSettings.Instance.ModAngelicFeatherVIP);
            SetComboBoxEnum(KeyAngelicFeatherVIP, (int)SPSettings.Instance.KeyAngelicFeatherVIP);

            OverrideControls();
        }

        private void Cancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void defaults_Click(object sender, EventArgs e) {
            desperatePrayerHp.Value = 60;
            UseFadeAsDefensiveCDBelow.Value = 45;
            dispersionMana.Value = 15;
            dispersionHp.Value = 30;
            Lvl90MinUnits.Value = 2;
            healthstoneHp.Value = 35;            
            InstantHealBelowHp.Value = 80;
            swpRefresh.Value = 3000;
            vampiricEmbraceHp.Value = 70;
            vtRefresh.Value = 3000;
            aoeMana.Value = 20;
            CastedHealsBelowHp.Value = 35;
            DispelDelay.Value = 4;
            DispelAboveHp.Value = 40; 
            VTHighPrioBelowManaPercent.Value = 20;
            interval.Value = 400;
            AngelicFeatherUsage.SelectedIndex = 1;
            AngelicFeatherPriority.SelectedIndex = 0;


            lvl90safeCheck.Checked = false;
            VoidShiftGrief.Checked = true;
            multidot.Checked = true;
            healOthers.Checked = true;
            fearWard.Checked = true;
            massDispel.Checked = true;
            face.Checked = true;
            fade.Checked = true;
            AutoBurst.Checked = true;
            pwf.Checked = true;
            UseShadowFiend.Checked = true;
            UsePowerInfusion.Checked = true;
            LeapOfFaithOnScatteredTeammate.Checked = true;
            UseRacial.Checked = true;
            RightClickMovementOff.Checked = true;
            UseTrinketWithDP.Checked = true;
            upperTrinketSlot.Checked = true;
            lowerTrinketSlot.Checked = false;
            OnlyHealVIP.Checked = false;            
            HealPets.Checked = true;
            StopCastOnInterrupt.Checked = true;
            CounteractPoly.Checked = true;
            CastFailedUserInitiatedSpell.Checked = true;
            CollectStatistics.Checked = true;
            CastLevitate.Checked = true;

            DisablePeeling.Checked = false;
            PeelBelow.Value = 50;
            DefensiveCDDelay.Value = 5;
            UseVoidTendrils.Checked = true;
            UsePsychicScream.Checked = true;
            UsePsyfiend.Checked = true;
            UseSpectralGuise.Checked = true;
            UsePsychicHorrorPeel.Checked = true;


            DisableCC.Checked = false;
            CCFocusOrHealerBelow.Value = 35;
            CCDelay.Value = 4;
            CCWhenBursting.Checked = false;
            UsePsychicScreamCC.Checked = true;
            UseSilenceCC.Checked = true;
            UsePsyfiendCC.Checked = true;
            UsePsychicHorrorCC.Checked = true;

            ModBurst.SelectedIndex = 0;
            KeyBurst.SelectedIndex = 0;
            ModPsyfiendTarget.SelectedIndex = 0;
            KeyPsyfiendTarget.SelectedIndex = 0;
            ModPsyfiendFocus.SelectedIndex = 0;
            KeyPsyfiendFocus.SelectedIndex = 0;
            ModAngelicFeather.SelectedIndex = 0;
            KeyAngelicFeather.SelectedIndex = 0;
            ModAngelicFeatherVIP.SelectedIndex = 0;
            KeyAngelicFeatherVIP.SelectedIndex = 0;         

        }

        private void save_Click(object sender, EventArgs e) {

            SPSettings.Instance.DesperatePrayer = (int)desperatePrayerHp.Value;
            SPSettings.Instance.UseFadeAsDefensiveCDBelow = (int)UseFadeAsDefensiveCDBelow.Value;
            SPSettings.Instance.DispersionMana = (int)dispersionMana.Value;
            SPSettings.Instance.DispersionHP = (int)dispersionHp.Value;
            SPSettings.Instance.Lvl90MinUnits = (int)Lvl90MinUnits.Value;
            SPSettings.Instance.HealthstonePercent = (int)healthstoneHp.Value;            
            SPSettings.Instance.InstantHealBelowHp = (int)InstantHealBelowHp.Value;
            SPSettings.Instance.SWPRefresh = (int)swpRefresh.Value;
            SPSettings.Instance.VampiricEmbracePercent = (int)vampiricEmbraceHp.Value;
            SPSettings.Instance.VampiricTouchRefresh = (int)vtRefresh.Value;
            SPSettings.Instance.AoEMana = (int)aoeMana.Value;
            SPSettings.Instance.CastedHealsBelowHp = (int)CastedHealsBelowHp.Value;
            SPSettings.Instance.DispelDelay = (int)DispelDelay.Value;
            SPSettings.Instance.DispelAboveHp = (int)DispelAboveHp.Value;
            SPSettings.Instance.VTHighPrioBelowManaPercent = (int)VTHighPrioBelowManaPercent.Value;
            SPSettings.Instance.SearchInterval = (int)interval.Value;
            SPSettings.Instance.AngelicFeatherUsage = AngelicFeatherUsage.SelectedIndex;
            SPSettings.Instance.AngelicFeatherPriority = AngelicFeatherPriority.SelectedIndex;


            SPSettings.Instance.Lvl90SafeCheck = lvl90safeCheck.Checked;
            SPSettings.Instance.GriefPpl = VoidShiftGrief.Checked;
            SPSettings.Instance.Multidot = multidot.Checked;
            SPSettings.Instance.HealOthers = healOthers.Checked;
            SPSettings.Instance.FearWardSelf = fearWard.Checked;
            SPSettings.Instance.AutoMassDispel = massDispel.Checked;
            SPSettings.Instance.AutoFace = face.Checked;
            SPSettings.Instance.FadeAuto = fade.Checked;
            SPSettings.Instance.AutoBurst = AutoBurst.Checked;
            SPSettings.Instance.PowerWordFortitude = pwf.Checked;
            SPSettings.Instance.UseShadowFiend = UseShadowFiend.Checked;
            SPSettings.Instance.UsePowerInfusion = UsePowerInfusion.Checked;
            SPSettings.Instance.LeapOfFaithOnScatteredTeammate = LeapOfFaithOnScatteredTeammate.Checked;
            SPSettings.Instance.UseRacial = UseRacial.Checked;
            SPSettings.Instance.RightClickMovementOff = RightClickMovementOff.Checked;
            SPSettings.Instance.UseTrinketWithDP = UseTrinketWithDP.Checked;
            SPSettings.Instance.TrinketSlotNumber = upperTrinketSlot.Checked ? 13 : 14;
            SPSettings.Instance.OnlyHealVIP = OnlyHealVIP.Checked;
            SPSettings.Instance.AngelicFeatherDelay = AngelicFeatherUsage.SelectedIndex == 2 ? 6 : (AngelicFeatherUsage.SelectedIndex == 3 ? 10 : 0);
            SPSettings.Instance.HealPets = HealPets.Checked;
            SPSettings.Instance.StopCastOnInterrupt = StopCastOnInterrupt.Checked;
            SPSettings.Instance.CounteractPoly = CounteractPoly.Checked;
            SPSettings.Instance.CastFailedUserInitiatedSpell = CastFailedUserInitiatedSpell.Checked;
            SPSettings.Instance.CollectStatistics = CollectStatistics.Checked;
            SPSettings.Instance.CastLevitate = CastLevitate.Checked;
            

            if (SPSettings.Instance.RightClickMovementOff && Styx.CommonBot.TreeRoot.Current.Name != "BGBuddy") {
                Styx.WoWInternals.Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 0\")');");
            } else {
                Styx.WoWInternals.Lua.DoString("RunMacroText('/run ConsoleExec(\"Autointeract 1\")');");
            }

            SPSettings.Instance.DisablePeeling = DisablePeeling.Checked;
            SPSettings.Instance.PeelBelow = (int)PeelBelow.Value;
            SPSettings.Instance.DefensiveCDDelay = (int)DefensiveCDDelay.Value;            
            SPSettings.Instance.UseVoidTendrils = UseVoidTendrils.Checked;
            SPSettings.Instance.UsePsychicScream = UsePsychicScream.Checked;
            SPSettings.Instance.UsePsyfiend = UsePsyfiend.Checked;
            SPSettings.Instance.UseSpectralGuise = UseSpectralGuise.Checked;
            SPSettings.Instance.UsePsychicHorrorPeel = UsePsychicHorrorPeel.Checked;


            SPSettings.Instance.DefCDsBelow = new List<int>() {
                SPSettings.Instance.HealthstonePercent,
                SPSettings.Instance.DesperatePrayer,
                SPSettings.Instance.DispersionHP,
                SPSettings.Instance.VampiricEmbracePercent,
                SPSettings.Instance.UseFadeAsDefensiveCDBelow,
                SPSettings.Instance.PeelBelow
            }.Max();


            SPSettings.Instance.DisableCC = DisableCC.Checked;
            SPSettings.Instance.CCFocusOrHealerBelow = (int)CCFocusOrHealerBelow.Value;
            SPSettings.Instance.CCDelay = (int)CCDelay.Value;
            SPSettings.Instance.CCWhenBursting = CCWhenBursting.Checked;
            SPSettings.Instance.UsePsychicScreamCC = UsePsychicScreamCC.Checked;
            SPSettings.Instance.UseSilenceCC = UseSilenceCC.Checked;
            SPSettings.Instance.UsePsyfiendCC = UsePsyfiendCC.Checked;
            SPSettings.Instance.UsePsychicHorrorCC = UsePsychicHorrorCC.Checked;


            combos = new HashSet<string>();

            SPSettings.Instance.ModBurst = (ModifierKeys)GetComboBoxEnum(ModBurst);
            SPSettings.Instance.KeyBurst = (Keys)GetComboBoxEnum(KeyBurst);

            if (!isHotkeyUsable(SPSettings.Instance.ModBurst, SPSettings.Instance.KeyBurst, "Burst")) {
                SPSettings.Instance.ModBurst = 0;
                SPSettings.Instance.KeyBurst = Keys.None;
            }

            SPSettings.Instance.ModPsyfiendTarget = (ModifierKeys)GetComboBoxEnum(ModPsyfiendTarget);
            SPSettings.Instance.KeyPsyfiendTarget = (Keys)GetComboBoxEnum(KeyPsyfiendTarget);

            if (!isHotkeyUsable(SPSettings.Instance.ModPsyfiendTarget, SPSettings.Instance.KeyPsyfiendTarget, "Psyfiend on target")) {
                SPSettings.Instance.ModPsyfiendTarget = 0;
                SPSettings.Instance.KeyPsyfiendTarget = Keys.None;
            }

            SPSettings.Instance.ModPsyfiendFocus = (ModifierKeys)GetComboBoxEnum(ModPsyfiendFocus);
            SPSettings.Instance.KeyPsyfiendFocus = (Keys)GetComboBoxEnum(KeyPsyfiendFocus);

            if (!isHotkeyUsable(SPSettings.Instance.ModPsyfiendFocus, SPSettings.Instance.KeyPsyfiendFocus, "Psyfiend on focus")) {
                SPSettings.Instance.ModPsyfiendFocus = 0;
                SPSettings.Instance.KeyPsyfiendFocus = Keys.None;
            }

            SPSettings.Instance.ModAngelicFeather = (ModifierKeys)GetComboBoxEnum(ModAngelicFeather);
            SPSettings.Instance.KeyAngelicFeather = (Keys)GetComboBoxEnum(KeyAngelicFeather);

            if (!isHotkeyUsable(SPSettings.Instance.ModAngelicFeather, SPSettings.Instance.KeyAngelicFeather, "Angelic Feather on Me")) {
                SPSettings.Instance.ModAngelicFeather = 0;
                SPSettings.Instance.KeyAngelicFeather = Keys.None;
            }

            SPSettings.Instance.ModAngelicFeatherVIP = (ModifierKeys)GetComboBoxEnum(ModAngelicFeatherVIP);
            SPSettings.Instance.KeyAngelicFeatherVIP = (Keys)GetComboBoxEnum(KeyAngelicFeatherVIP);

            if (!isHotkeyUsable(SPSettings.Instance.ModAngelicFeatherVIP, SPSettings.Instance.KeyAngelicFeatherVIP, "Angelic Feather on VIP")) {
                SPSettings.Instance.ModAngelicFeatherVIP = 0;
                SPSettings.Instance.KeyAngelicFeatherVIP = Keys.None;
            }

            SPSettings.Instance.Save();
            Logging.Write("----------------------------------");
            Logging.Write("Your settings have been saved");
            SPSettings.Print();
            Main.ReRegisterHotkeys();
            this.Close();
        }

        private void healOthers_CheckedChanged(object sender, EventArgs e) {
            if (!((CheckBox)sender).Checked) {
                OnlyHealVIP.Checked = false;
            }
        }

        private void OnlyHealVIP_CheckedChanged(object sender, EventArgs e) {
            if (((CheckBox)sender).Checked) {
                healOthers.Checked = true;
            }
        }

        private void OverrideControls() {
            if (Main.IsProVersion) {
                DisabledFeatures.Visible = false;                
            } else {

                DefensiveCDs.Enabled = false;
                groupBox4.Enabled = false;
                ModPsyfiendTarget.Enabled = false;
                ModPsyfiendFocus.Enabled = false;
                ModAngelicFeather.Enabled = false;
                ModAngelicFeatherVIP.Enabled = false;
                KeyPsyfiendTarget.Enabled = false;
                KeyPsyfiendFocus.Enabled = false;
                KeyAngelicFeather.Enabled = false;
                KeyAngelicFeatherVIP.Enabled = false;
                
                healOthers.Enabled = false;
                healOthers.Checked = false;
                OnlyHealVIP.Enabled = false;
                OnlyHealVIP.Checked = false;
                fade.Enabled = false;
                UseShadowFiend.Enabled = false;
                VoidShiftGrief.Enabled = false;
                massDispel.Enabled = false;
                LeapOfFaithOnScatteredTeammate.Enabled = false;
                RightClickMovementOff.Enabled = false;
                HealPets.Enabled = false;
                CounteractPoly.Enabled = false;
                CastFailedUserInitiatedSpell.Enabled = false;
                
                CastedHealsBelowHp.Enabled = false;
                UseFadeAsDefensiveCDBelow.Enabled = false;
                dispersionMana.Enabled = false;
                DispelDelay.Enabled = false;
                DispelAboveHp.Enabled = false;
                VTHighPrioBelowManaPercent.Enabled = false;
                
                DisabledFeatures.Visible = true;
            }
        }

        private static HashSet<string> combos = new HashSet<string>();

        private bool isHotkeyUsable(ModifierKeys mod, Keys key, string comboName) {

            if (mod == 0 && key == Keys.None)
                return true;

            if (mod == 0 || key == Keys.None) {
                Logging.Write(Colors.Red, comboName + " - Both the modifier and the key must be set! Setting default value.");
                return false;
            }

            string combo = mod.ToString() + "+" + key.ToString();

            if (!combos.Contains(combo)) {
                combos.Add(combo);
                return true;
            }

            Logging.Write(Colors.Red, comboName + " - Hotkey is already used! Setting default value.");
            return false;
        }

        #region ComboBoxItem - Thanks for this part for the developers of Tyrael. It was a lifesaver.

        public class ComboBoxItem {
            public readonly int E;
            private readonly string _s;

            public ComboBoxItem(int pe, string ps) {
                E = pe;
                _s = ps;
            }

            public override string ToString() {
                return _s;
            }
        }

        private static void SetComboBoxEnum(ComboBox cb, int e) {
            ComboBoxItem item;
            for (var i = 0; i < cb.Items.Count; i++) {
                item = (ComboBoxItem)cb.Items[i];
                if (item.E != e)
                    continue;
                cb.SelectedIndex = i;
                return;
            }
            item = (ComboBoxItem)cb.Items[0];
            Logging.Write("Dialog Error: Combobox {0} does not have item({1}) in list, defaulting to itenm({2})", cb.Name, e, item.E);
            cb.SelectedIndex = 0;
        }
        private static int GetComboBoxEnum(ComboBox cb) {
            var item = (ComboBoxItem)cb.Items[cb.SelectedIndex];

            return item.E;
        }
        #endregion

    }
}
