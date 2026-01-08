using Styx.Common;
using Styx.Helpers;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using VitalicRotation.Helpers; // Totems enum

namespace VitalicRotation.Settings
{
    public class VitalicSettings : Styx.Helpers.Settings
    {
        private static readonly string SettingsFile =
            Path.Combine(Styx.Helpers.Settings.CharacterSettingsDirectory, "VitalicRotation.xml");

        internal static bool IsFirstRun { get; private set; }

        private static readonly VitalicSettings _instance = new VitalicSettings();
        public static VitalicSettings Instance { get { return _instance; } }

        // NOTE: Honorbuddy Settings requires a parameterless ctor calling base(file)
        private VitalicSettings() : base(SettingsFile)
        {
            try
            {
                IsFirstRun = !File.Exists(SettingsFile);

                // Ensure defaults or previously saved values are loaded before any UI reads
                if (IsFirstRun)
                {
                    // Apply DefaultValue attributes as the initial persisted values
                    ResetDefaults();
                    Save();
                }
                else
                {
                    // Load existing settings from disk
                    Load();
                }

                // Keybind defaults (mirror original) — keep overrides only if not set
                // Apply keybind defaults only on first run (if settings file does not yet exist)
                if (IsFirstRun)
                {
                    if (FocusMacroKeyBind == Keys.None) FocusMacroKeyBind = Keys.LShiftKey;
                    if (BurstKeyBind == Keys.None) BurstKeyBind = Keys.LMenu;               // Alt
                    if (BurstNoShadowBladesKeyBind == Keys.None) BurstNoShadowBladesKeyBind = (Keys)65700; // combo
                    if (LazyKeyBind == Keys.None) LazyKeyBind = Keys.RShiftKey;
                    if (PauseKeyBind == Keys.None) PauseKeyBind = Keys.LShiftKey;
                    if (PauseDamageKeyBind == Keys.None) PauseDamageKeyBind = (Keys)131236; // combo
                    if (EventsKeyBind == Keys.None) EventsKeyBind = Keys.RControlKey;
                    if (KidneyShotKeyBind == Keys.None) KidneyShotKeyBind = Keys.LControlKey;
                    if (SmokeBombKeyBind == Keys.None) SmokeBombKeyBind = Keys.XButton2;

                    Save(); // persist first-run defaults once
                }
            }
            catch { }
        }

        public void ResetDefaults()
        {
            var props = TypeDescriptor.GetProperties(this);
            foreach (PropertyDescriptor prop in props)
            {
                var attr = (Styx.Helpers.DefaultValueAttribute)prop.Attributes[typeof(Styx.Helpers.DefaultValueAttribute)];
                if (attr != null)
                    prop.SetValue(this, attr.Value);
            }
        }

        // ===================== KEYBINDS =====================
        [Setting, Styx.Helpers.DefaultValue(Keys.None)]
        public Keys GarroteKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.None)]
        public Keys CheapShotKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.None)]
        public Keys BlindKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.None)]
        public Keys GougeKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.None)]
        public Keys RedirectKidneyKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.LShiftKey)]
        public Keys FocusMacroKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.LMenu)]
        public Keys BurstKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue((Keys)65700)]
        public Keys BurstNoShadowBladesKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.RShiftKey)]
        public Keys LazyKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.LShiftKey)]
        public Keys PauseKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue((Keys)131236)]
        public Keys PauseDamageKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.RControlKey)]
        public Keys EventsKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.LControlKey)]
        public Keys KidneyShotKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.XButton2)]
        public Keys SmokeBombKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.None)]
        public Keys RestealthKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.None)]
        public Keys FastKickKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.None)]
        public Keys AutoKidneyKeyBind { get; set; }

        [Setting, Styx.Helpers.DefaultValue(Keys.None)]
        public Keys OpenerModifierKeyBind { get; set; }

        // ===================== POISONS =====================
        [Setting, Styx.Helpers.DefaultValue(0)]
        public int MainHandPoison { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0)]
        public int OffHandPoison { get; set; }

        // ===================== UI PERSISTENCE =====================
        [Setting, Styx.Helpers.DefaultValue(612)]
        public int UIWidth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(425)]
        public int UIHeight { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0)]
        public int UILocationX { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0)]
        public int UILocationY { get; set; }

        [Setting, Styx.Helpers.DefaultValue(3)]
        public int UILastTab { get; set; }

        [Setting, Styx.Helpers.DefaultValue(3)]
        public int UIColorStyle { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0d)]
        public double StatusFrameLeft { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0d)]
        public double StatusFrameBottom { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool StatusFrameEnabled { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AlertFontsEnabled { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool SpellAlertsEnabled { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool SoundAlertsEnabled { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool LogMessagesEnabled { get; set; }

        // ===================== MACROS / TIMING =====================
        [Setting, Styx.Helpers.DefaultValue(2d)]
        public double MacroDelay { get; set; }

        [Setting, Styx.Helpers.DefaultValue(1d)]
        public double StickyDelay { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool MacrosEnabled { get; set; }

        // ===================== ALERTES/HEALTH =====================
        [Setting, Styx.Helpers.DefaultValue(30)]
        public int LowHealthWarning { get; set; }

        // ===================== OPENERS / BURST =====================
        [Setting, Styx.Helpers.DefaultValue(0)]
        public int SubterfugeOpeners { get; set; }

        [Setting, Styx.Helpers.DefaultValue(90)]
        public int BurstEnergy { get; set; }

        [Setting, Styx.Helpers.DefaultValue(70)]
        public int BurstEnergyOpener { get; set; }

        [Setting, Styx.Helpers.DefaultValue(30)]
        public int BurstHealth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(15)]
        public int BurstPreparation { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0.5d)]
        public double BurstStunDR { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0)]
        public int CombatBurst { get; set; }

        // ===================== AUTOMATION / BASE =====================
        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoTarget { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AlwaysStealth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoRedirect { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoKidney { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoPreparation { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoSmokeBomb { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoShroud { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoBurstOfSpeed { get; set; }

        [Setting, Styx.Helpers.DefaultValue(15)]
        public int AutoShadowstep { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoFlagReturn { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool EventAutoFace { get; set; }

        [Setting, Styx.Helpers.DefaultValue("")]
        public string EventBlacklist { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoShiv { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool LazyEviscerate { get; set; }

        // ===================== INTERRUPTS (legacy simple window) =====================
        [Setting, Styx.Helpers.DefaultValue(0.5d)]
        public double InterruptDelay { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0.2d)]
        public double InterruptMinimum { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0.2d)]
        public double InterruptShadowstepBuffer { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0.7d)]
        public double GougeDelay { get; set; }

        [Setting, Styx.Helpers.DefaultValue(80)]
        public int GougeNoKickHP { get; set; }

        [Setting, Styx.Helpers.DefaultValue(25)]
        public int KidneyShotEnergy { get; set; }

        [Setting, Styx.Helpers.DefaultValue(4)]
        public int KidneyShotCPs { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool AlwaysUseHemo { get; set; }

        [Setting, Styx.Helpers.DefaultValue(2d)]
        public double HemoDelay { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool RuptureOverGarrote { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool LazyPooling { get; set; }

        [Setting, Styx.Helpers.DefaultValue(30)]
        public int OpenerTPS { get; set; }

        [Setting, Styx.Helpers.DefaultValue("")]
        public string TricksTarget { get; set; }

        // ===================== AOE THRESHOLDS (simple) =====================
        [Setting, Styx.Helpers.DefaultValue(3)]
        public int AoeThresholdFoK { get; set; }

        [Setting, Styx.Helpers.DefaultValue(3)]
        public int AoeThresholdCT { get; set; }

        [Setting, Styx.Helpers.DefaultValue(2)]
        public int AoeThresholdBladeFlurry { get; set; }

        // ===================== TOTEMS =====================
        [Setting, Styx.Helpers.DefaultValue(Totems.Windwalk | Totems.Capacitor | Totems.Grounding | Totems.SpiritLink | Totems.Earthgrab | Totems.HealingStream | Totems.HealingTide)]
        public Totems TotemStomp { get; set; }

        // ===================== DEFENSIVES =====================
        [Setting, Styx.Helpers.DefaultValue(30)]
        public int HealthstoneHP { get; set; }

        [Setting, Styx.Helpers.DefaultValue(40)]
        public int TeammateHP { get; set; }

        [Setting, Styx.Helpers.DefaultValue(50)]
        public int RecuperateHP { get; set; }

        [Setting, Styx.Helpers.DefaultValue(60)]
        public int AutoFeint { get; set; }

        [Setting, Styx.Helpers.DefaultValue(5)]
        public int FeintLastDamage { get; set; }

        // Nouveau seuil Feint distinct (P1.8)
        [Setting, Styx.Helpers.DefaultValue(60)]
        public int FeintHealth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool FeintInMeleeRange { get; set; }

        [Setting, Styx.Helpers.DefaultValue(50)]
        public int CloakHealth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(40)]
        public int EvasionHealth { get; set; }

        // ===================== COMBO POINTS & FINISHERS =====================
        [Setting, Styx.Helpers.DefaultValue(3)]
        public int EnvenomCPs { get; set; }

        [Setting, Styx.Helpers.DefaultValue(30)]
        public int EnvenomEnergy { get; set; }

        [Setting, Styx.Helpers.DefaultValue(40)]
        public int RecuperateHealth { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool ShadowstepTraps { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AutoMoveOnTraps { get; set; }

    // ===================== GLUE (stick-to-target) =====================
    [Setting, Styx.Helpers.DefaultValue(false)]
    public bool EnableGluePlugin { get; set; }

    [Setting, Styx.Helpers.DefaultValue(true)]
    public bool GlueAutoRun { get; set; }

        // ===================== MODES / SYSTEME =====================
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool PveMode { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AcceptQueues { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AlertQueues { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool AntiAFK { get; set; }

        [Setting, Styx.Helpers.DefaultValue(true)]
        public bool DisableCTM { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool DiagnosticMode { get; set; }

        [Setting, Styx.Helpers.DefaultValue(100)]
        public int ManualCastPause { get; set; }


        [Setting, Styx.Helpers.DefaultValue(0)]
        public int AutoFocus { get; set; }

        [Setting, Styx.Helpers.DefaultValue(0)]
        public int AutoFocusTargets { get; set; }

        // ===================== PVP / CONTROLS =====================
        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool AutoBlindHealerTrinket { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool HandlePsyfiend { get; set; }

        [Setting, Styx.Helpers.DefaultValue(false)]
        public bool PsyfiendArenasOnly { get; set; }

        [Setting, Styx.Helpers.DefaultValue(10)]
        public int PsyfiendFoKRange { get; set; }

        [Setting, Styx.Helpers.DefaultValue(1500)]
        public int PsyfiendThrottleMs { get; set; }

        public void ValidatePoisons()
        {
            // Accept both legacy selector values (1..5) and direct spell IDs.
            // 0 means Default.
            try { MainHandPoison = NormalizePoisonValue(MainHandPoison, true); } catch { }
            try { OffHandPoison = NormalizePoisonValue(OffHandPoison, false); } catch { }
        }

        private static int NormalizePoisonValue(int val, bool isMain)
        {
            // Default
            if (val == 0) return 0;

            // Legacy selector values (1..5)
            if (val >= 1 && val <= 5)
            {
                switch (val)
                {
                    case 1: return Helpers.SpellBook.DeadlyPoison;
                    case 2: return Helpers.SpellBook.WoundPoison;
                    case 3: return Helpers.SpellBook.CripplingPoison;
                    case 4: return Helpers.SpellBook.MindNumbingPoison;
                    case 5: return Helpers.SpellBook.LeechingPoison;
                }
            }

            // Already a spell ID? allow pass-through for supported poisons
            if (val == Helpers.SpellBook.DeadlyPoison ||
                val == Helpers.SpellBook.WoundPoison ||
                val == Helpers.SpellBook.CripplingPoison ||
                val == Helpers.SpellBook.MindNumbingPoison ||
                val == Helpers.SpellBook.LeechingPoison ||
                val == Helpers.SpellBook.ParalyticPoison)
            {
                return val;
            }

            // Unknown value → reset to Default
            return 0;
        }
    }
}
