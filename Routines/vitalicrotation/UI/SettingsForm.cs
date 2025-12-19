using Styx;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using VitalicRotation.Settings;
using VitalicRotation.Helpers;
using VitalicRotation.UI;

namespace VitalicRotation.UI
{
    public partial class SettingsForm : Form
    {
        // Helper forward declarations moved before constructor for compiler scope
        private PictureBox FindPictureBoxByName(string name) { return FindControlByName<PictureBox>(this, name); }
        private T FindControlByName<T>(Control parent, string name) where T:Control { if(parent==null) return null; foreach(Control c in parent.Controls){ if(c is T && c.Name==name) return (T)c; var f=FindControlByName<T>(c,name); if(f!=null) return f;} return null; }
        private void ApplyCursorToTextBoxes(Control parent, Cursor cursor){ if(parent==null||cursor==null) return; foreach(Control c in parent.Controls){ var tb=c as TextBox; if(tb!=null) tb.Cursor=cursor; else if(c.HasChildren) ApplyCursorToTextBoxes(c,cursor);} }
        private static void ApplyFontRecursively(Control root, Font font){ if(root==null) return; root.Font=font; foreach(Control ch in root.Controls) ApplyFontRecursively(ch,font); }
        private void WireAutoSaveHandlers(Control root){ if(root==null) return; foreach(Control c in root.Controls){ if(c is CheckBox) ((CheckBox)c).CheckedChanged+=(_,__)=>AutoSave(); else if(c is ComboBox) ((ComboBox)c).SelectedIndexChanged+=(_,__)=>AutoSave(); else if(c is TrackBar) ((TrackBar)c).ValueChanged+=(_,__)=>AutoSave(); else if(c is NumericUpDown) ((NumericUpDown)c).ValueChanged+=(_,__)=>AutoSave(); else if(c is TextBox) ((TextBox)c).TextChanged+=(_,__)=>AutoSave(); else if(c is Button){ var b=(Button)c; var prop=NormalizeKeyBindName(b.Name); if(!string.IsNullOrEmpty(prop)) b.Click+=KeyBindButton_Click; else b.Click+=(_,__)=>AutoSave(); } if(c.HasChildren) WireAutoSaveHandlers(c);} }
        private void AutoSave(){ if(_loading) return; try{ VitalicSettings.Instance.Save(); }catch{} try{ labelSaved.Text="Modifications enregistrées"; labelSaved.Visible=true; _saveNoticeTimer.Stop(); _saveNoticeTimer.Start(); }catch{} }
        private static string NormalizeKeyBindName(string buttonName){ if(string.IsNullOrEmpty(buttonName)) return null; if(buttonName.EndsWith("KeyBindButton",StringComparison.OrdinalIgnoreCase)) return buttonName.Substring(0, buttonName.Length-"Button".Length); if(buttonName.EndsWith("KeyBind",StringComparison.OrdinalIgnoreCase)) return buttonName; return null; }
    private static void RefreshKeybindUI(Control root){ if(root==null) return; foreach(Control c in root.Controls){ var b=c as Button; if(b!=null&&!string.IsNullOrEmpty(b.Name)){ var prop=NormalizeKeyBindName(b.Name); if(!string.IsNullOrEmpty(prop)){ try{ var p=typeof(VitalicSettings).GetProperty(prop, BindingFlags.Instance|BindingFlags.Public|BindingFlags.IgnoreCase); if(p!=null && p.PropertyType==typeof(Keys)){ var k=(Keys)p.GetValue(VitalicSettings.Instance,null); b.Text = k==Keys.None?"Click to set Keybind":k.ToString(); } }catch{} } } if(c.HasChildren) RefreshKeybindUI(c);} }

        // === mapping affichage -> valeur (1..5) pour les poisons ===
        private sealed class PoisonOption
        {
            public readonly string Text;
            public readonly int Value;
            public PoisonOption(string text, int value) { Text = text; Value = value; }
            public override string ToString() { return Text; }
        }

        // Indicateur de sauvegarde + timer d'auto-masquage
        private readonly System.Windows.Forms.Timer _saveNoticeTimer = new System.Windows.Forms.Timer { Interval = 1500 };
        private bool _loading;

        // --- Debug tab controls ---
        private TabPage tabDebug;
        private Label lblTarget, lblEnergyCp, lblToggles;
        private Label lblDRStun, lblDRIncap, lblDRDisorient;
        private System.Windows.Forms.Timer uiTimer;

        private ComboBox comboForcedMode;
        private CheckBox chkAdvanced;
        private Panel panelExpertOptions;
        
        // Theme combo box
        private ComboBox comboTheme;

        // Etat de capture de keybinds (v.zip-like)
        private bool _isCapturing;
        private Button _capturingButton;
        private Action<Keys> _capturingSetter;
        private KeyCaptureFilter _filter;

        // =============================
        //   Câblage et initialisation
        // =============================
        // Appel dans le constructeur après InitializeComponent
        public SettingsForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += SettingsForm_KeyDown; // fallback capture
            
            // Build static combo data before applying settings
            _loading = true;
            try { PopulateComboData(); } catch { }
            _loading = false;
            
            // Phase 1: Load resources (icons, cursors) BEFORE wiring events
            try { this.ShadowstepTrapsCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; try { VitalicSettings.Instance.ShadowstepTraps = ShadowstepTrapsCheckBox.Checked; VitalicSettings.Instance.Save(); } catch { } }; } catch { }
            
            // Load icon/title assets
            var iconBmp = ResourceAccessor.TryLoadBitmap("icon.png");
            if (iconBmp != null) { try { this.Icon = Icon.FromHandle(iconBmp.GetHicon()); } catch { } }
            var titleBmp = ResourceAccessor.TryLoadBitmap("title.png");
            if (titleBmp != null)
            {
                // Respect Designer-only layout: only set image if a designer-defined PictureBox exists
                PictureBox pictureBoxTitle = FindPictureBoxByName("pictureBoxTitle") ?? FindPictureBoxByName("titlePictureBox");
                if (pictureBoxTitle != null)
                {
                    try { pictureBoxTitle.Image = titleBmp; } catch { }
                }
            }
            // IMPORTANT: n'utilisons plus de curseurs bitmap personnalisés (hotspot imprécis -> clics décalés)
            // Conservons les curseurs système pour un alignement parfait des clics
            try { this.Cursor = Cursors.Default; } catch { }

            _saveNoticeTimer.Tick += (s, e) => { try { labelSaved.Visible = false; } catch { } _saveNoticeTimer.Stop(); };
            
            // Phase 2: APPLY SETTINGS TO CONTROLS BEFORE WIRING AUTO-SAVE
            _loading = true;
            try { ApplySettingsToControls(); } catch { }
            _loading = false;
            
            // Phase 3: Wire autosave handlers AFTER controls reflect user settings
            WireAutoSaveHandlers(this);
            
            // Specific handlers (value-changed) AFTER initial load
            try
            {
                this.GougeNoKickHPBar.ValueChanged += (s, e) => { if (_loading) return; try { VitalicSettings.Instance.GougeNoKickHP = GougeNoKickHPBar.Value; GougeNoKickHPLabel.Text = GougeNoKickHPBar.Value.ToString(); VitalicSettings.Instance.Save(); } catch { } };
                this.ShadowstepBufferBar.ValueChanged += (s, e) => { if (_loading) return; try { VitalicSettings.Instance.InterruptShadowstepBuffer = ShadowstepBufferBar.Value / 10.0; ShadowstepBufferLabel.Text = (ShadowstepBufferBar.Value / 10.0).ToString("0.0"); VitalicSettings.Instance.Save(); } catch { } };
            }
            catch { }
            // Added handlers for interrupt / gouge delay sliders (previously missing)
            try { this.InterruptDelayBar.Scroll += (s,e)=> { InterruptDelayLabel.Text=(InterruptDelayBar.Value/10.0).ToString("0.0"); }; this.InterruptDelayBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.InterruptDelay = InterruptDelayBar.Value/10.0; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.InterruptMinimumBar.Scroll += (s,e)=> { InterruptMinimumLabel.Text=(InterruptMinimumBar.Value/10.0).ToString("0.0"); }; this.InterruptMinimumBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.InterruptMinimum = InterruptMinimumBar.Value/10.0; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.GougeDelayBar.Scroll += (s,e)=> { GougeDelayLabel.Text=(GougeDelayBar.Value/100.0).ToString("0.00"); }; this.GougeDelayBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.GougeDelay = GougeDelayBar.Value/100.0; VitalicSettings.Instance.Save(); }; } catch {}

            var uiFont = new Font("Segoe UI", 9F, FontStyle.Regular); this.Font = uiFont; ApplyFontRecursively(this, uiFont);
            this.Load += SettingsForm_Load; this.FormClosing += (s, e) => { try { VitalicSettings.Instance.Save(); } catch { } }; this.Shown += SettingsForm_Shown; this.Activated += (s, e) => { if (!_loading) RefreshKeybindUI(this); };

            // Core checkbox / slider wiring simplified (named handlers removed to match mirror build)
            try { this.StatusFrameCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.StatusFrameEnabled = StatusFrameCheckBox.Checked; VitalicSettings.Instance.Save(); if(StatusFrameCheckBox.Checked) VitalicUi.ShowStatus(); else VitalicUi.HideStatus(); }; } catch {}
            try { this.SpellAlertsCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.SpellAlertsEnabled = SpellAlertsCheckBox.Checked; VitalicSettings.Instance.Save(); VitalicUi.OnSpellAlertsSettingChanged(); }; } catch {}
            try { this.SoundAlertsCheckBox.CheckedChanged += (s,e)=> { VitalicSettings.Instance.SoundAlertsEnabled = SoundAlertsCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.LogMessagesCheckBox.CheckedChanged += (s,e)=> { VitalicSettings.Instance.LogMessagesEnabled = LogMessagesCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AlertFontsCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AlertFontsEnabled = AlertFontsCheckBox.Checked; VitalicSettings.Instance.Save(); VitalicUi.OnAlertFontsSettingChanged(); }; } catch {}
            try { this.MacrosEnabledCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.MacrosEnabled = MacrosEnabledCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.StickyDelayBar.Scroll += (s,e)=> { StickyDelayLabel.Text=(StickyDelayBar.Value/10.0).ToString("0.##"); }; this.StickyDelayBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.StickyDelay = StickyDelayBar.Value/10.0; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.MacroDelayBar.Scroll += (s,e)=> { MacroDelayLabel.Text=(MacroDelayBar.Value/10.0).ToString("0.##"); }; this.MacroDelayBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.MacroDelay = MacroDelayBar.Value/10.0; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.LowHealthWarningBar.Scroll += (s,e)=> { LowHealthWarningLabel.Text=LowHealthWarningBar.Value.ToString(); }; this.LowHealthWarningBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.LowHealthWarning = LowHealthWarningBar.Value; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.BurstEnergyBar.Scroll += (s,e)=> { BurstEnergyLabel.Text=BurstEnergyBar.Value.ToString(); }; this.BurstEnergyBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.BurstEnergy = BurstEnergyBar.Value; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.BurstEnergyOpenerBar.Scroll += (s,e)=> { BurstEnergyOpenerLabel.Text=BurstEnergyOpenerBar.Value.ToString(); }; this.BurstEnergyOpenerBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.BurstEnergyOpener = BurstEnergyOpenerBar.Value; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.BurstPreparationBar.Scroll += (s,e)=> { BurstPreparationLabel.Text=BurstPreparationBar.Value.ToString(); }; this.BurstPreparationBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.BurstPreparation = BurstPreparationBar.Value; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.BurstHealthBar.Scroll += (s,e)=> { BurstHealthLabel.Text=BurstHealthBar.Value.ToString(); }; this.BurstHealthBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.BurstHealth = BurstHealthBar.Value; VitalicSettings.Instance.Save(); }; } catch {}
            // Removed AoE sliders (grpAoe) and AutoBlindHealerTrinketCheckBox for archive parity
            // try { this.AoeFoKBar.Scroll += (s,e)=> { AoeFoKLabel.Text = AoeFoKBar.Value.ToString(); }; this.AoeFoKBar.MouseLeave += (s,e)=> { try { VitalicSettings.Instance.AoeThresholdFoK = AoeFoKBar.Value; VitalicSettings.Instance.Save(); } catch { } }; } catch { }
            // try { this.AoeCTBar.Scroll += (s,e)=> { AoeCTLabel.Text = AoeCTBar.Value.ToString(); }; this.AoeCTBar.MouseLeave += (s,e)=> { try { VitalicSettings.Instance.AoeThresholdCT = AoeCTBar.Value; VitalicSettings.Instance.Save(); } catch { } }; } catch { }
            // try { this.AoeBFBar.Scroll += (s,e)=> { AoeBFLabel.Text = AoeBFBar.Value.ToString(); }; this.AoeBFBar.MouseLeave += (s,e)=> { try { VitalicSettings.Instance.AoeThresholdBladeFlurry = AoeBFBar.Value; VitalicSettings.Instance.Save(); } catch { } }; } catch { }
            try { this.HemoDelayBar.Scroll += (s,e)=> { HemoDelayLabel.Text=(HemoDelayBar.Value/10.0).ToString("0.##"); }; this.HemoDelayBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.HemoDelay = HemoDelayBar.Value/10.0; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.KidneyShotEnergyBar.Scroll += (s,e)=> { KidneyShotEnergyLabel.Text=KidneyShotEnergyBar.Value.ToString(); }; this.KidneyShotEnergyBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.KidneyShotEnergy = KidneyShotEnergyBar.Value; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.KidneyShotCPsBar.Scroll += (s,e)=> { KidneyShotCPsLabel.Text=KidneyShotCPsBar.Value.ToString(); }; this.KidneyShotCPsBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.KidneyShotCPs = KidneyShotCPsBar.Value; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.SubterfugeOpenersComboBox.SelectedIndexChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.SubterfugeOpeners = SubterfugeOpenersComboBox.SelectedIndex; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.CombatBurstComboBox.SelectedIndexChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.CombatBurst = CombatBurstComboBox.SelectedIndex; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.BurstStunDRComboBox.SelectedIndexChanged += (s,e)=> { if(_loading) return; var item = BurstStunDRComboBox.SelectedItem; if(item is KeyValuePair<double,string>) { var kv = (KeyValuePair<double,string>)item; VitalicSettings.Instance.BurstStunDR = kv.Key; VitalicSettings.Instance.Save(); } }; } catch {}
            try { this.LazyPoolingCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.LazyPooling = LazyPoolingCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AlwaysUseHemoCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AlwaysUseHemo = AlwaysUseHemoCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.RuptureOverGarroteCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.RuptureOverGarrote = RuptureOverGarroteCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.LazyEviscerateCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.LazyEviscerate = LazyEviscerateCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AutoRedirectCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoRedirect = AutoRedirectCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AutoKidneyCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoKidney = AutoKidneyCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AutoPreparationCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoPreparation = AutoPreparationCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AutoSmokeBombCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoSmokeBomb = AutoSmokeBombCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AutoShroudCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoShroud = AutoShroudCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AutoShivCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoShiv = AutoShivCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AutoBurstOfSpeedCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoBurstOfSpeed = AutoBurstOfSpeedCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AlwaysStealthCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AlwaysStealth = AlwaysStealthCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AutoFlagReturnCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoFlagReturn = AutoFlagReturnCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AutoTargetCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoTarget = AutoTargetCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            //try { this.AutoBlindHealerTrinketCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoBlindHealerTrinket = AutoBlindHealerTrinketCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.PvEModeCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.PveMode = PvEModeCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.DiagnosticModeCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.DiagnosticMode = DiagnosticModeCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AcceptQueuesCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AcceptQueues = AcceptQueuesCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AlertQueuesCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AlertQueues = AlertQueuesCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.ClickToMoveCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.DisableCTM = ClickToMoveCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.AntiAFKCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AntiAFK = AntiAFKCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.MainHandPoisonComboBox.SelectedIndexChanged += (s,e)=> { if(_loading) return; var po= MainHandPoisonComboBox.SelectedItem as PoisonOption; VitalicSettings.Instance.MainHandPoison = po!=null?po.Value:0; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.OffHandPoisonComboBox.SelectedIndexChanged += (s,e)=> { if(_loading) return; var po= OffHandPoisonComboBox.SelectedItem as PoisonOption; VitalicSettings.Instance.OffHandPoison = po!=null?po.Value:0; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.ShadowstepRangeBar.Scroll += (s,e)=> { ShadowstepRangeLabel.Text=ShadowstepRangeBar.Value.ToString(); VitalicSettings.Instance.AutoShadowstep = ShadowstepRangeBar.Value; VitalicSettings.Instance.Save(); }; this.ShadowstepRangeBar.MouseLeave += (s,e)=> { VitalicSettings.Instance.AutoShadowstep = ShadowstepRangeBar.Value; VitalicSettings.Instance.Save(); }; } catch {}
            try { this.TricksTarget.Leave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.TricksTarget = TricksTarget.Text ?? string.Empty; VitalicSettings.Instance.Save(); }; } catch {}

            WireExtraRuntimeHandlers();
            
            if (VitalicSettings.IsFirstRun)
            {
                try { if (AutoFocusComboBox != null && AutoFocusComboBox.Items.Count > 0) AutoFocusComboBox.SelectedIndex = 0; } catch { }
                try { if (AutoFocusTargetsComboBox != null && AutoFocusTargetsComboBox.Items.Count > 0) AutoFocusTargetsComboBox.SelectedIndex = 0; } catch { }
                try { if (SubterfugeOpenersComboBox != null && SubterfugeOpenersComboBox.Items.Count > 0) SubterfugeOpenersComboBox.SelectedIndex = 0; } catch { }
                try { if (CombatBurstComboBox != null && CombatBurstComboBox.Items.Count > 0) CombatBurstComboBox.SelectedIndex = 0; } catch { }
                try
                {
                    if (BurstStunDRComboBox != null)
                    {
                        // Default to Half (0.5) like original burst DR preference
                        for (int i = 0; i < BurstStunDRComboBox.Items.Count; i++)
                        {
                            var kv = BurstStunDRComboBox.Items[i] as KeyValuePair<double, string>?
;
                            if (kv.HasValue && Math.Abs(kv.Value.Key - 0.5d) < 0.0001) { BurstStunDRComboBox.SelectedIndex = i; break; }
                        }
                    }
                }
                catch { }
                try
                {
                    if (MainHandPoisonComboBox != null)
                    {
                        // Default first item (Default = 0)
                        MainHandPoisonComboBox.SelectedIndex = 0;
                    }
                    if (OffHandPoisonComboBox != null)
                    {
                        OffHandPoisonComboBox.SelectedIndex = 0;
                    }
                }
                catch { }
            }

            // Synchronize totem toggles with settings bitmask instead of forcing true
            try
            {
                var tmask = VitalicSettings.Instance.TotemStomp;
                SetCheckIfExists("WindwalkToggle", (tmask & Totems.Windwalk) != 0);
                SetCheckIfExists("CapacitorToggle", (tmask & Totems.Capacitor) != 0);
                SetCheckIfExists("GroundingToggle", (tmask & Totems.Grounding) != 0);
                SetCheckIfExists("SpiritLinkToggle", (tmask & Totems.SpiritLink) != 0);
                SetCheckIfExists("EarthgrabToggle", (tmask & Totems.Earthgrab) != 0);
                SetCheckIfExists("HealingStreamToggle", (tmask & Totems.HealingStream) != 0);
                SetCheckIfExists("HealingTideToggle", (tmask & Totems.HealingTide) != 0);
            }
            catch { }
        }

        private void PopulateComboData()
        {
            try
            {
                if (AutoFocusComboBox != null) { AutoFocusComboBox.Items.Clear(); AutoFocusComboBox.Items.AddRange(new object[]{"Always","Arenas only","Battlegrounds only","Never"}); }
                if (AutoFocusTargetsComboBox != null) { AutoFocusTargetsComboBox.Items.Clear(); AutoFocusTargetsComboBox.Items.AddRange(new object[]{"Focus Healers + DPS","Focus Healers Only"}); }
            }
            catch { }
            try
            {
                if (CombatBurstComboBox != null) { CombatBurstComboBox.Items.Clear(); CombatBurstComboBox.Items.AddRange(new object[]{"ShB + Killing Spree","ShB + Adrenaline Rush"}); }
            }
            catch { }
            try
            {
                if (SubterfugeOpenersComboBox != null) { SubterfugeOpenersComboBox.Items.Clear(); SubterfugeOpenersComboBox.Items.AddRange(new object[]{"Always","Burst Mode only","Never"}); }
            }
            catch { }
            try
            {
                if (BurstStunDRComboBox != null)
                {
                    BurstStunDRComboBox.Items.Clear();
                    BurstStunDRComboBox.DisplayMember = "Value";
                    BurstStunDRComboBox.ValueMember = "Key";
                    BurstStunDRComboBox.Items.Add(new KeyValuePair<double,string>(1.0, "Full"));
                    BurstStunDRComboBox.Items.Add(new KeyValuePair<double,string>(0.5, "Half"));
                    BurstStunDRComboBox.Items.Add(new KeyValuePair<double,string>(0.0, "Any"));
                }
            }
            catch { }
            try
            {
                if (MainHandPoisonComboBox != null)
                {
                    MainHandPoisonComboBox.Items.Clear();
                    MainHandPoisonComboBox.Items.AddRange(new object[]{
                        new PoisonOption("Default",0),
                        new PoisonOption("Wound Poison", SpellBook.WoundPoison),
                        new PoisonOption("Deadly Poison", SpellBook.DeadlyPoison)
                    });
                }
                if (OffHandPoisonComboBox != null)
                {
                    OffHandPoisonComboBox.Items.Clear();
                    OffHandPoisonComboBox.Items.AddRange(new object[]{
                        new PoisonOption("Default",0),
                        new PoisonOption("Mind-Numbing Poison", SpellBook.MindNumbingPoison),
                        new PoisonOption("Crippling Poison", SpellBook.CripplingPoison),
                        new PoisonOption("Paralytic Poison", SpellBook.ParalyticPoison),
                        new PoisonOption("Leeching Poison", SpellBook.LeechingPoison)
                    });
                }
            }
            catch { }
        }

        // New: apply settings values to controls safely without firing autosave (wrapped by _loading flag)
        private void ApplySettingsToControls()
        {
            try {
                var s = VitalicSettings.Instance;
                // Interrupt / timing sliders
                try { InterruptDelayBar.Value = Coerce(0, InterruptDelayBar.Maximum, (int)Math.Round(s.InterruptDelay * 10.0)); InterruptDelayLabel.Text = (InterruptDelayBar.Value / 10.0).ToString("0.0"); } catch { }
                try { InterruptMinimumBar.Value = Coerce(0, InterruptMinimumBar.Maximum, (int)Math.Round(s.InterruptMinimum * 10.0)); InterruptMinimumLabel.Text = (InterruptMinimumBar.Value / 10.0).ToString("0.0"); } catch { }
                try { GougeDelayBar.Value = Coerce(0, GougeDelayBar.Maximum, (int)Math.Round(s.GougeDelay * 100.0)); GougeDelayLabel.Text = (GougeDelayBar.Value / 100.0).ToString("0.00"); } catch { }
                // Defensive sliders
                try { FeintHPBar.Value = Coerce(FeintHPBar.Minimum, FeintHPBar.Maximum, s.FeintHealth); FeintHPLabel.Text = FeintHPBar.Value.ToString(); } catch { }
                try { FeintLastDamageBar.Value = Coerce(FeintLastDamageBar.Minimum, FeintLastDamageBar.Maximum, s.FeintLastDamage); FeintLastDamageLabel.Text = FeintLastDamageBar.Value.ToString(); } catch { }
                try { HealthstoneHPBar.Value = Coerce(HealthstoneHPBar.Minimum, HealthstoneHPBar.Maximum, s.HealthstoneHP); HealthstoneHPLabel.Text = HealthstoneHPBar.Value.ToString(); } catch { }
                try { RecuperateHPBar.Value = Coerce(RecuperateHPBar.Minimum, RecuperateHPBar.Maximum, s.RecuperateHP); RecuperateHPLabel.Text = RecuperateHPBar.Value.ToString(); } catch { }
                try { TeammateSupportBar.Value = Coerce(TeammateSupportBar.Minimum, TeammateSupportBar.Maximum, s.TeammateHP); TeammateSupportLabel.Text = TeammateSupportBar.Value.ToString(); } catch { }
                // ManualCastPause & Opener TPS
                try { ManualCastPauseBar.Value = Coerce(ManualCastPauseBar.Minimum, ManualCastPauseBar.Maximum, s.ManualCastPause); ManualCastPauseLabel.Text = ManualCastPauseBar.Value.ToString(); } catch { }
                try { OpenerTPSBar.Value = Coerce(OpenerTPSBar.Minimum, OpenerTPSBar.Maximum, s.OpenerTPS); OpenerTPSLabel.Text = OpenerTPSBar.Value.ToString(); } catch { }
                // Burst sliders
                try { BurstEnergyBar.Value = Coerce(BurstEnergyBar.Minimum, BurstEnergyBar.Maximum, s.BurstEnergy); BurstEnergyLabel.Text = BurstEnergyBar.Value.ToString(); } catch { }
                try { BurstEnergyOpenerBar.Value = Coerce(BurstEnergyOpenerBar.Minimum, BurstEnergyOpenerBar.Maximum, s.BurstEnergyOpener); BurstEnergyOpenerLabel.Text = BurstEnergyOpenerBar.Value.ToString(); } catch { }
                try { BurstPreparationBar.Value = Coerce(BurstPreparationBar.Minimum, BurstPreparationBar.Maximum, s.BurstPreparation); BurstPreparationLabel.Text = BurstPreparationBar.Value.ToString(); } catch { }
                try { BurstHealthBar.Value = Coerce(BurstHealthBar.Minimum, BurstHealthBar.Maximum, s.BurstHealth); BurstHealthLabel.Text = BurstHealthBar.Value.ToString(); } catch { }
                // Kidney sliders
                try { KidneyShotEnergyBar.Value = Coerce(KidneyShotEnergyBar.Minimum, KidneyShotEnergyBar.Maximum, s.KidneyShotEnergy); KidneyShotEnergyLabel.Text = KidneyShotEnergyBar.Value.ToString(); } catch { }
                try { KidneyShotCPsBar.Value = Coerce(KidneyShotCPsBar.Minimum, KidneyShotCPsBar.Maximum, s.KidneyShotCPs); KidneyShotCPsLabel.Text = KidneyShotCPsBar.Value.ToString(); } catch { }
                // Shadowstep Range
                try { ShadowstepRangeBar.Value = Coerce(ShadowstepRangeBar.Minimum, ShadowstepRangeBar.Maximum, s.AutoShadowstep); ShadowstepRangeLabel.Text = ShadowstepRangeBar.Value.ToString(); } catch { }
                // Misc durations
                try { LowHealthWarningBar.Value = Coerce(LowHealthWarningBar.Minimum, LowHealthWarningBar.Maximum, s.LowHealthWarning); LowHealthWarningLabel.Text = LowHealthWarningBar.Value.ToString(); } catch { }
                try { StickyDelayBar.Value = Coerce(StickyDelayBar.Minimum, StickyDelayBar.Maximum, (int)Math.Round(s.StickyDelay * 10)); StickyDelayLabel.Text = (StickyDelayBar.Value/10.0).ToString("0.##"); } catch { }
                try { MacroDelayBar.Value = Coerce(MacroDelayBar.Minimum, MacroDelayBar.Maximum, (int)Math.Round(s.MacroDelay * 10)); MacroDelayLabel.Text = (MacroDelayBar.Value/10.0).ToString("0.##"); } catch { }
                try { HemoDelayBar.Value = Coerce(HemoDelayBar.Minimum, HemoDelayBar.Maximum, (int)Math.Round(s.HemoDelay * 10)); HemoDelayLabel.Text = (HemoDelayBar.Value/10.0).ToString("0.##"); } catch { }
                try { ShadowstepBufferBar.Value = Coerce(ShadowstepBufferBar.Minimum, ShadowstepBufferBar.Maximum, (int)Math.Round(s.InterruptShadowstepBuffer * 10)); ShadowstepBufferLabel.Text = (ShadowstepBufferBar.Value/10.0).ToString("0.0"); } catch { }
                // Checkboxes
                try { ShadowstepTrapsCheckBox.Checked = s.ShadowstepTraps; } catch { }
                try { AutoMoveOnTrapsCheckBox.Checked = s.AutoMoveOnTraps; } catch { }
                try { FeintInMeleeRangeCheckBox.Checked = s.FeintInMeleeRange; } catch { }
                try { AlwaysStealthCheckBox.Checked = s.AlwaysStealth; } catch { }
                try { AutoTargetCheckBox.Checked = s.AutoTarget; } catch { }
                try { AutoShivCheckBox.Checked = s.AutoShiv; } catch { }
                try { AutoRedirectCheckBox.Checked = s.AutoRedirect; } catch { }
                try { AutoKidneyCheckBox.Checked = s.AutoKidney; } catch { }
                try { AutoPreparationCheckBox.Checked = s.AutoPreparation; } catch { }
                try { AutoSmokeBombCheckBox.Checked = s.AutoSmokeBomb; } catch { }
                try { AutoShroudCheckBox.Checked = s.AutoShroud; } catch { }
                try { AutoBurstOfSpeedCheckBox.Checked = s.AutoBurstOfSpeed; } catch { }
                try { AutoFlagReturnCheckBox.Checked = s.AutoFlagReturn; } catch { }
                try { LazyPoolingCheckBox.Checked = s.LazyPooling; } catch { }
                try { AlwaysUseHemoCheckBox.Checked = s.AlwaysUseHemo; } catch { }
                try { RuptureOverGarroteCheckBox.Checked = s.RuptureOverGarrote; } catch { }
                try { LazyEviscerateCheckBox.Checked = s.LazyEviscerate; } catch { }
                try { PvEModeCheckBox.Checked = s.PveMode; } catch { }
                try { DiagnosticModeCheckBox.Checked = s.DiagnosticMode; } catch { }
                try { AcceptQueuesCheckBox.Checked = s.AcceptQueues; } catch { }
                try { AlertQueuesCheckBox.Checked = s.AlertQueues; } catch { }
                try { ClickToMoveCheckBox.Checked = s.DisableCTM; } catch { }
                try { AntiAFKCheckBox.Checked = s.AntiAFK; } catch { }
                try { StatusFrameCheckBox.Checked = s.StatusFrameEnabled; } catch { }
                try { LogMessagesCheckBox.Checked = s.LogMessagesEnabled; } catch { }
                try { SpellAlertsCheckBox.Checked = s.SpellAlertsEnabled; } catch { }
                try { AlertFontsCheckBox.Checked = s.AlertFontsEnabled; } catch { }
                try { SoundAlertsCheckBox.Checked = s.SoundAlertsEnabled; } catch { }
                try { MacrosEnabledCheckBox.Checked = s.MacrosEnabled; } catch { }
                // ComboBoxes with value mapping
                try { if (BurstStunDRComboBox != null) { for (int i=0;i<BurstStunDRComboBox.Items.Count;i++){ var kv = BurstStunDRComboBox.Items[i] as KeyValuePair<double,string>?; if(kv.HasValue && Math.Abs(kv.Value.Key - s.BurstStunDR) < 0.0001){ BurstStunDRComboBox.SelectedIndex = i; break; } } } } catch { }
                try { if (MainHandPoisonComboBox != null){ for(int i=0;i<MainHandPoisonComboBox.Items.Count;i++){ var po = MainHandPoisonComboBox.Items[i] as PoisonOption; if(po!=null && po.Value == s.MainHandPoison){ MainHandPoisonComboBox.SelectedIndex = i; break; } } } } catch { }
                try { if (OffHandPoisonComboBox != null){ for(int i=0;i<OffHandPoisonComboBox.Items.Count;i++){ var po = OffHandPoisonComboBox.Items[i] as PoisonOption; if(po!=null && po.Value == s.OffHandPoison){ OffHandPoisonComboBox.SelectedIndex = i; break; } } } } catch { }
                // Auto Focus persistence
                try { if (AutoFocusComboBox != null && AutoFocusComboBox.Items.Count > 0) { var idx = s.AutoFocus; if (idx < 0 || idx >= AutoFocusComboBox.Items.Count) idx = 0; AutoFocusComboBox.SelectedIndex = idx; } } catch { }
                try { if (AutoFocusTargetsComboBox != null && AutoFocusTargetsComboBox.Items.Count > 0) { var idx = s.AutoFocusTargets; if (idx < 0 || idx >= AutoFocusTargetsComboBox.Items.Count) idx = 0; AutoFocusTargetsComboBox.SelectedIndex = idx; } } catch { }
                try { SubterfugeOpenersComboBox.SelectedIndex = s.SubterfugeOpeners; } catch { }
                try { CombatBurstComboBox.SelectedIndex = s.CombatBurst; } catch { }
                // Text boxes
                try { EventBlacklist.Text = s.EventBlacklist ?? string.Empty; } catch { }
                try { TricksTarget.Text = s.TricksTarget ?? string.Empty; } catch { }
                // Totems
                try { LoadTotemToggles(); } catch { }
            } catch { }
        }

        private void SettingsForm_Load(object sender, EventArgs e){ /* Loading already done in constructor; only position persistence kept intentionally minimal */ }
        private void SettingsForm_Shown(object sender, EventArgs e){ try{ VitalicSettings.Instance.Load(); }catch{} RefreshKeybindUI(this); try { LoadTotemToggles(); } catch { } }

        // Sauvegarde état fenêtre / onglet
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                var s = VitalicSettings.Instance;
                s.UILocationX = this.Location.X; s.UILocationY = this.Location.Y; s.UIWidth = this.Size.Width; s.UIHeight = this.Size.Height;
                if (tabControl1 != null) s.UILastTab = tabControl1.SelectedIndex;
                s.Save();
            }
            catch { }
            base.OnFormClosing(e);
        }

        // ===================== Totem bitmask (Load / Save) =====================
        private void LoadTotemToggles()
        {
            var t = VitalicSettings.Instance.TotemStomp;
            // Utilise FindControlByName pour supporter d'éventuels totems optionnels
            SetCheckIfExists("GroundingToggle",     t.HasFlag(Totems.Grounding));
            SetCheckIfExists("SpiritLinkToggle",    t.HasFlag(Totems.SpiritLink));
            SetCheckIfExists("HealingStreamToggle", t.HasFlag(Totems.HealingStream));
            SetCheckIfExists("HealingTideToggle",   t.HasFlag(Totems.HealingTide));
            SetCheckIfExists("EarthgrabToggle",     t.HasFlag(Totems.Earthgrab));
            SetCheckIfExists("WindwalkToggle",      t.HasFlag(Totems.Windwalk));
            SetCheckIfExists("CapacitorToggle",     t.HasFlag(Totems.Capacitor));
            SetCheckIfExists("TremorToggle",        t.HasFlag(Totems.Tremor));
            SetCheckIfExists("StoneBulwarkToggle",  t.HasFlag(Totems.StoneBulwark));
            // Wire handlers once
            if (grpTotems != null)
            {
                foreach (var cb in grpTotems.Controls.OfType<CheckBox>())
                {
                    try { cb.CheckedChanged -= TotemToggle_CheckedChanged; } catch { }
                    cb.CheckedChanged += TotemToggle_CheckedChanged;
                }
            }
        }
        private void TotemToggle_CheckedChanged(object sender, EventArgs e)
        {
            if (_loading) return; SaveTotemToggles();
        }
        private void SaveTotemToggles()
        {
            try
            {
                Totems m = Totems.None;
                AddFlagIf("GroundingToggle",     Totems.Grounding, ref m);
                AddFlagIf("SpiritLinkToggle",    Totems.SpiritLink, ref m);
                AddFlagIf("HealingStreamToggle", Totems.HealingStream, ref m);
                AddFlagIf("HealingTideToggle",   Totems.HealingTide, ref m);
                AddFlagIf("EarthgrabToggle",     Totems.Earthgrab, ref m);
                AddFlagIf("WindwalkToggle",      Totems.Windwalk, ref m);
                AddFlagIf("CapacitorToggle",     Totems.Capacitor, ref m);
                AddFlagIf("TremorToggle",        Totems.Tremor, ref m);
                AddFlagIf("StoneBulwarkToggle",  Totems.StoneBulwark, ref m);
                VitalicSettings.Instance.TotemStomp = m; VitalicSettings.Instance.Save();
            }
            catch { }
        }
        private void SetCheckIfExists(string name, bool value){ try { var c = FindControlByName<CheckBox>(this, name); if (c!=null) { c.Checked = value; } } catch { } }
        private void AddFlagIf(string name, Totems flag, ref Totems acc){ try { var c = FindControlByName<CheckBox>(this, name); if (c!=null && c.Checked) acc |= flag; } catch { } }

        // ============ Handlers pour sliders supplémentaires ============
        private void WireExtraRuntimeHandlers()
        {
            try { FeintHPBar.Scroll += (s,e)=> { FeintHPLabel.Text = FeintHPBar.Value.ToString(); }; FeintHPBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.FeintHealth = FeintHPBar.Value; VitalicSettings.Instance.Save(); }; } catch { }
            try { FeintLastDamageBar.Scroll += (s,e)=> { FeintLastDamageLabel.Text = FeintLastDamageBar.Value.ToString(); }; FeintLastDamageBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.FeintLastDamage = FeintLastDamageBar.Value; VitalicSettings.Instance.Save(); }; } catch { }
            try { HealthstoneHPBar.Scroll += (s,e)=> { HealthstoneHPLabel.Text = HealthstoneHPBar.Value.ToString(); }; HealthstoneHPBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.HealthstoneHP = HealthstoneHPBar.Value; VitalicSettings.Instance.Save(); }; } catch { }
            try { RecuperateHPBar.Scroll += (s,e)=> { RecuperateHPLabel.Text = RecuperateHPBar.Value.ToString(); }; RecuperateHPBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.RecuperateHP = RecuperateHPBar.Value; VitalicSettings.Instance.Save(); }; } catch { }
            try { TeammateSupportBar.Scroll += (s,e)=> { TeammateSupportLabel.Text = TeammateSupportBar.Value.ToString(); }; TeammateSupportBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.TeammateHP = TeammateSupportBar.Value; VitalicSettings.Instance.Save(); }; } catch { }
            try { ManualCastPauseBar.Scroll += (s,e)=> { ManualCastPauseLabel.Text = ManualCastPauseBar.Value.ToString(); }; ManualCastPauseBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.ManualCastPause = ManualCastPauseBar.Value; VitalicSettings.Instance.Save(); }; } catch { }
            try { OpenerTPSBar.Scroll += (s,e)=> { OpenerTPSLabel.Text = OpenerTPSBar.Value.ToString(); }; OpenerTPSBar.MouseLeave += (s,e)=> { if(_loading) return; VitalicSettings.Instance.OpenerTPS = OpenerTPSBar.Value; VitalicSettings.Instance.Save(); }; } catch { }
            // Removed AutoFeintBar wiring (control deleted)
            try { EventBlacklist.TextChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.EventBlacklist = EventBlacklist.Text ?? string.Empty; VitalicSettings.Instance.Save(); }; } catch { }
            try { AutoMoveOnTrapsCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoMoveOnTraps = AutoMoveOnTrapsCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch { }
            try { FeintInMeleeRangeCheckBox.CheckedChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.FeintInMeleeRange = FeintInMeleeRangeCheckBox.Checked; VitalicSettings.Instance.Save(); }; } catch { }
            try { AutoFocusComboBox.SelectedIndexChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoFocus = AutoFocusComboBox.SelectedIndex; VitalicSettings.Instance.Save(); }; } catch { }
            try { AutoFocusTargetsComboBox.SelectedIndexChanged += (s,e)=> { if(_loading) return; VitalicSettings.Instance.AutoFocusTargets = AutoFocusTargetsComboBox.SelectedIndex; VitalicSettings.Instance.Save(); }; } catch { }
        }

        private void KeyBindButton_Click(object sender, EventArgs e){ var btn=sender as Button; if(btn==null) return; var prop=NormalizeKeyBindName(btn.Name); if(string.IsNullOrEmpty(prop)) return; _capturingButton=btn; _capturingSetter = k=>{ try{ var p=typeof(VitalicSettings).GetProperty(prop, BindingFlags.Instance|BindingFlags.Public|BindingFlags.IgnoreCase); if(p!=null&&p.PropertyType==typeof(Keys)){ p.SetValue(VitalicSettings.Instance,k,null); VitalicSettings.Instance.Save(); }}catch{} }; BeginKeyCapture(); }
        private void BeginKeyCapture(){ _isCapturing=true; if(_capturingButton!=null) _capturingButton.Text="Press key..."; if(_filter!=null) try{ Application.RemoveMessageFilter(_filter);}catch{} _filter=new KeyCaptureFilter(()=>_isCapturing, k=>{ try{ if(_capturingSetter!=null) _capturingSetter(k);}catch{} EndKeyCapture(); RefreshKeybindUI(this); }); Application.AddMessageFilter(_filter); }
        private void EndKeyCapture(){ _isCapturing=false; if(_filter!=null){ try{ Application.RemoveMessageFilter(_filter);}catch{} _filter=null;} _capturingButton=null; _capturingSetter=null; }
        private void SettingsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_isCapturing) return;
            try
            {
                var key = e.KeyCode;
                if (key == Keys.ShiftKey || key == Keys.ControlKey || key == Keys.Menu)
                    return; // ignore bare modifiers
                if (_capturingSetter != null)
                {
                    _capturingSetter(key);
                }
                if (_capturingButton != null)
                {
                    _capturingButton.Text = key.ToString();
                }
            }
            catch { }
            finally
            {
                EndKeyCapture();
                try { RefreshKeybindUI(this); } catch { }
                e.Handled = true;
            }
        }

        private static int Coerce(int min,int max,int v){ if(v<min) return min; if(v>max) return max; return v; }
        private sealed class KeyCaptureFilter : IMessageFilter
        {
            private readonly Func<bool> _active; private readonly Action<Keys> _on;
            public KeyCaptureFilter(Func<bool> active, Action<Keys> on){ _active=active; _on=on; }
            public bool PreFilterMessage(ref Message m){ if(!_active()) return false; const int WM_KEYDOWN=0x0100; const int WM_SYSKEYDOWN=0x0104; if(m.Msg==WM_KEYDOWN||m.Msg==WM_SYSKEYDOWN){ Keys k=(Keys)m.WParam.ToInt32(); try{ if(_on!=null) _on(k);}catch{} return true;} return false; }
        }
    }
}